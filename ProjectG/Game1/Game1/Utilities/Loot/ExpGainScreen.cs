using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;

namespace TBAGW.Utilities
{
    public static class ExpGainScreen
    {
        public static BaseCharacter currentBC;
        public static bool bIsDone = false;
        public static List<KeyValuePair<BaseCharacter, int>> charsAndExpGains = new List<KeyValuePair<BaseCharacter, int>>();
        static int expAmount = 0;
        static int amountHandled = 0;
        static int amountPerFrame = 0;
        static int length = (int)(1.0f * 60);

        static public bool bDoneGivingExp = false;
        static public bool bLevelledUp = false;
        static internal LUA.LuaGameClass levelUpInfo = null;
        static internal LevelUpStatIncreaseDisplay lustid = null;
        static internal GameText abilityGainText = new GameText("Gained a new ability!");
        static internal bool bGainedANewAbility = false;

        static int abilityPreCount = 0;
        static int abilityPostCount = 0;

        static public void Start(List<KeyValuePair<BaseCharacter, int>> cae)
        {
            bDoneGivingExp = false;
            bLevelledUp = false;
            bIsDone = false;

            charsAndExpGains = cae;
            if (cae.Count == 0)
            {
                Stop();
            }
            else
            {

                GenerateLogic();
            }
        }

        static public void ConfirmPress()
        {
            if (bLevelledUp)
            {
                bLevelledUp = false;
                currentBC.CCC.equippedClass.classEXP.bLevelledUp = false;
                levelUpInfo = null;
                lustid = null;
            }
            else
            {
                if (charsAndExpGains.Count != 0)
                {
                    GenerateLogic();
                }
                else
                {
                    Stop();
                }
            }


        }

        static public void GenerateLogic()
        {
            amountHandled = 0;
            bDoneGivingExp = false;
            bLevelledUp = false;
            bIsDone = false;


            currentBC = charsAndExpGains[0].Key;
            expAmount = charsAndExpGains[0].Value;
            amountPerFrame = expAmount / length;
            charsAndExpGains.RemoveAt(0);

            PreExpGain();

        }

        private static void PreExpGain()
        {
            abilityPreCount = currentBC.CCC.equippedClass.possibleClassAbilities().Count;
            bGainedANewAbility = false;
        }

        static public void Update(GameTime gt)
        {
            if (!bLevelledUp && !bDoneGivingExp)
            {

                if (amountHandled < expAmount && !bDoneGivingExp)
                {
                    amountHandled += amountPerFrame;
                }

                if (amountHandled + amountPerFrame >= expAmount)
                {
                    bDoneGivingExp = true;
                    amountPerFrame = 0;

                }

                if (amountPerFrame<=0)
                {
                    amountPerFrame = 5;
                }



                if (amountHandled >= expAmount)
                {
                    int leftOver = expAmount - amountHandled;
                    amountPerFrame += leftOver;
                    amountHandled = expAmount;
                }


                if (amountHandled != 0)
                {
                    currentBC.CCC.equippedClass.classEXP.AddExp(amountPerFrame);
                    bLevelledUp = currentBC.CCC.equippedClass.classEXP.bLevelledUp;
                }

                if (bLevelledUp)
                {
                    HandleLevelUp();
                }
            }
            else
            {
                if (levelUpInfo != null)
                {
                    if (lustid != null)
                    {
                        lustid.Update(gt);
                    }
                }
            }

        }

        private static void HandleLevelUp() //Is properly called once :D, I don't know how but the me in the past was a genius, no idea how I did that
        {
            currentBC.CCC.equippedClass.CheckScript();
            if (currentBC.CCC.equippedClass.HasProperScript())
            {
                try
                {
                    currentBC.CCC.equippedClass.HandleLevelup(currentBC.CCC.equippedClass, currentBC);
                }
                catch (Exception e)
                {

                }
            }

            if (levelUpInfo != null)
            {
                lustid = new LevelUpStatIncreaseDisplay(levelUpInfo, currentBC);
            }

            abilityPostCount = currentBC.CCC.equippedClass.possibleClassAbilities().Count;

            if (abilityPreCount != abilityPostCount)
            {
                bGainedANewAbility = true;
            }
        }

        static RenderTarget2D expScreenRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D expScreenLevelUPRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static public RenderTarget2D Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(expScreenRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

            currentBC.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(sb, new Rectangle(100, 200, 200, 200));
            TextUtility.Draw(sb, currentBC.CCC.equippedClass.classEXP.totalExp.ToString(), BattleGUI.testSF32, new Rectangle(100, 505, 200, 50), TextUtility.OutLining.Center, Color.Gold, 0);
            sb.Draw(Game1.hitboxHelp, new Rectangle(100, 505, 200, 50), Color.White);

            sb.Draw(Game1.hitboxHelp, new Rectangle(1366 / 2 - 800 / 2, 400, 800, 64), Color.White);

            if (currentBC.CCC.equippedClass.bCanLevelUp() && !bLevelledUp)
            {

                float percentage = ((float)currentBC.CCC.equippedClass.classEXP.ExpRequirementCurrentLevel() - (float)currentBC.CCC.equippedClass.classEXP.expTillNextLevel) / (float)currentBC.CCC.equippedClass.classEXP.ExpRequirementCurrentLevel();
                //Console.WriteLine(percentage);
                sb.Draw(Game1.hitboxHelp, new Rectangle(1366 / 2 - 800 / 2, 400, (int)(800 * percentage), 64), Color.Green);
                TextUtility.Draw(sb, currentBC.CCC.equippedClass.classEXP.expTillNextLevel + " until level up!", BattleGUI.testSF32, new Rectangle(1366 / 2 - 800 / 2, 400, 800, 64), TextUtility.OutLining.Center, Color.Black, 0f, false);
            }
            //else
            //{
            //    TextUtility.Draw(sb, "Max Level!", BattleGUI.testSF32, new Rectangle(1366 / 2 - 800 / 2, 400, 800, 64), TextUtility.OutLining.Center, Color.Black, 0f, false);
            //}


            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            if (bLevelledUp)
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(expScreenLevelUPRender);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                sb.Draw(expScreenRender, expScreenRender.Bounds, new Color(75, 75, 75, 125));

                //sb.Draw(Game1.hitboxHelp, new Rectangle(1366 / 2 - 800 / 2, 100, 800, 250), Color.White);
                // TextUtility.ClearCache();
                TextUtility.Draw(sb, "LEVEL UP!", BattleGUI.testSF48, new Rectangle(1366 / 2 - 800 / 2, 100, 800, 250), TextUtility.OutLining.Center, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);
                TextUtility.Draw(sb,levelUpInfo.getQuality().getText() + " Level up" , BattleGUI.testSF48, new Rectangle(1366 / 2 - 800 / 2, 300, 800, 60), TextUtility.OutLining.Center, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);

                DrawStatGains(sb);

                sb.End();

                sb.GraphicsDevice.SetRenderTarget(null);
                return expScreenLevelUPRender;
            }
            return expScreenRender;
        }

        private static void DrawStatGains(SpriteBatch sb)
        {
            // sb.Draw(Game1.hitboxHelp, new Rectangle(1366 / 2 - 1100 / 2, 375, 1100, 300), Color.Yellow);
            //int x = 1366 / 2 - 1100 / 2;
            //sb.Draw(Game1.WhiteTex, new Rectangle((1366 / 2 - 1100 / 2), 375, 350, 300), Color.Green);
            //sb.Draw(Game1.WhiteTex, new Rectangle((1366 / 2 - 1100 / 2) + 350 + 25, 375, 350, 300), Color.Green);
            //sb.Draw(Game1.WhiteTex, new Rectangle((1366 / 2 - 1100 / 2) + 350 + 350 + 50, 375, 350, 300), Color.Green);

            //for (int i = 0; i < 5; i++)
            //{
            //    sb.Draw(Game1.WhiteTex, new Rectangle((1366 / 2 - 1100 / 2), 375 + 62 * i, 350, 52), Color.White);
            //}

            //for (int i = 0; i < 5; i++)
            //{
            //    sb.Draw(Game1.WhiteTex, new Rectangle((1366 / 2 - 1100 / 2) + 350 + 25, 375 + 62 * i, 350, 52), Color.White);
            //}

            //for (int i = 0; i < 5; i++)
            //{
            //    sb.Draw(Game1.WhiteTex, new Rectangle((1366 / 2 - 1100 / 2) + 350 + 350 + 50, 375 + 62 * i, 350, 52), Color.White);
            //}

            //sb.Draw(Game1.WhiteTex, new Rectangle(1366 / 2 - 1100 / 2, 700, 1100, 50), Color.Yellow);
            if (levelUpInfo != null)
            {
                if (lustid != null)
                {
                    lustid.Draw(sb);
                }
            }
        }

        static public void Stop()
        {
            PlayerController.currentController = PlayerController.Controllers.NonCombat;
            CombatProcessor.LeaveCombat();
        }
    }

    internal class LevelUpStatIncreaseDisplay
    {
        LUA.LuaGameClass info;
        List<StatUpDisplay> statUps;
        StatUpDisplay start = default(StatUpDisplay);
        bool bHandle = false;
        static bool bInitialize = false;
        static SpriteFont PointLineFont;
        static GameText pointsGainedText;
        static Rectangle pointsGainedLinePos;

        internal LevelUpStatIncreaseDisplay(LUA.LuaGameClass info, BaseCharacter bc)
        {
            if (!bInitialize)
            {
                Initialize();
            }
            statUps = new List<StatUpDisplay>();
            this.info = info;
            int count = 0;
            for (int i = 0; i < info.additionStat.currentPassiveStats.Count; i++)
            {
                if (info.additionStat.currentPassiveStats[i] != 0)
                {
                    if (count < 5)
                    {
                        String s = ((STATChart.PASSIVESTATSNames)i).ToString() + ": " + bc.trueSTATChart().currentPassiveStats[i];
                        statUps.Add(new StatUpDisplay(new Rectangle((1366 / 2 - 1100 / 2), 375 + 62 * count, 350, 52), s, info.additionStat.currentPassiveStats[i]));
                    }
                    if (count >= 5 && count < 10)
                    {
                        String s = ((STATChart.PASSIVESTATSNames)i).ToString() + ": " + bc.trueSTATChart().currentPassiveStats[i];
                        statUps.Add(new StatUpDisplay(new Rectangle((1366 / 2 - 1100 / 2) + 350 + 25, 375 + 62 * count % 5, 350, 52), s, info.additionStat.currentPassiveStats[i]));
                    }
                    if (count >= 10 && count < 15)
                    {
                        String s = ((STATChart.PASSIVESTATSNames)i).ToString() + ": " + bc.trueSTATChart().currentPassiveStats[i];
                        statUps.Add(new StatUpDisplay(new Rectangle((1366 / 2 - 1100 / 2) + 350 + 350 + 50, 375 + 62 * count % 10, 350, 52), s, info.additionStat.currentPassiveStats[i]));
                    }


                    count++;
                }
            }

            for (int i = 0; i < info.additionStat.currentSpecialStats.Count; i++)
            {
                if (info.additionStat.currentSpecialStats[i] != 0)
                {
                    if (count < 5)
                    {
                        String s = ((STATChart.SPECIALSTATSNames)i).ToString() + ": " + bc.trueSTATChart().currentSpecialStats[i];
                        statUps.Add(new StatUpDisplay(new Rectangle((1366 / 2 - 1100 / 2), 375 + 62 * count, 350, 52), s, info.additionStat.currentSpecialStats[i]));
                    }
                    if (count >= 5 && count < 10)
                    {
                        String s = ((STATChart.SPECIALSTATSNames)i).ToString() + ": " + bc.trueSTATChart().currentSpecialStats[i];
                        statUps.Add(new StatUpDisplay(new Rectangle((1366 / 2 - 1100 / 2) + 350 + 25, 375 + 62 * count % 5, 350, 52), s, info.additionStat.currentSpecialStats[i]));
                    }
                    if (count >= 10 && count < 15)
                    {
                        String s = ((STATChart.SPECIALSTATSNames)i).ToString() + ": " + bc.trueSTATChart().currentSpecialStats[i];
                        statUps.Add(new StatUpDisplay(new Rectangle((1366 / 2 - 1100 / 2) + 350 + 350 + 50, 375 + 62 * count % 10, 350, 52), s, info.additionStat.currentSpecialStats[i]));
                    }


                    count++;
                }
            }

            if (statUps.Count != 0)
            {
                bHandle = true;
                start = statUps[0];
                start.Start();
            }
            else
            {
                bHandle = false;
            }
        }

        static void Initialize()
        {
            bInitialize = true;
            PointLineFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");
            pointsGainedText = new GameText("Points gained: ");
            pointsGainedLinePos = new Rectangle(1366 / 2 - 1100 / 2, 700, 1100, 50);
        }

        internal void Update(GameTime gt)
        {
            if (bHandle)
            {
                if (statUps.Last().Equals(start))
                {
                    bHandle = false;
                }
                else if (start.HandleNext())
                {
                    start = statUps[statUps.IndexOf(start) + 1];
                    start.Start();
                }
            }
            else
            {

            }

            if (statUps.Count != 0)
            {
                for (int i = 0; i < statUps.Count; i++)
                {
                    statUps[i].Update(gt);
                }
            }
        }

        internal void Draw(SpriteBatch sb)
        {
            
            for (int i = 0; i < statUps.Count; i++)
            {
                statUps[i].Draw(sb);
            }

            String s = info.parent.ClassName + " " + info.additionPoints.points;
            TextUtility.Draw(sb, pointsGainedText.getText() + s, PointLineFont, pointsGainedLinePos, TextUtility.OutLining.Center, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);
            if (ExpGainScreen.bGainedANewAbility)
            {
                TextUtility.Draw(sb, ExpGainScreen.abilityGainText.getText() , PointLineFont, new Rectangle(1366 / 2 - 1100 / 2, 50, 1100, 50), TextUtility.OutLining.Center, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);
            }
        }

        internal class StatUpDisplay
        {
            Rectangle position;
            Rectangle statNamePos;
            Rectangle statAmountPos;
            Rectangle statArrowPos;
            static int middlePartLength = 50;
            String name;
            String amount;
            int amountNum;
            static Rectangle statArrowSource = new Rectangle(0, 0, 50, 52);
            static Texture2D statArrowTex;
            static SpriteFont font;
            static bool bInitialize = false;
            bool bIsActive;
            float opacity;


            internal StatUpDisplay(Rectangle pos, String name, int amount)
            {
                amountNum = amount;
                this.name = name;
                this.amount = amount.ToString();
                statNamePos = new Rectangle(pos.X, pos.Y, (pos.Width - middlePartLength) / 2 + middlePartLength * 2, pos.Height);
                statAmountPos = new Rectangle(pos.X + ((pos.Width - middlePartLength) / 2) + middlePartLength + middlePartLength * 2, pos.Y, (pos.Width - middlePartLength) / 2 - middlePartLength * 2, pos.Height);
                statArrowPos = new Rectangle(pos.X + (pos.Width - middlePartLength) / 2 + middlePartLength * 2, pos.Y, middlePartLength, pos.Height);
                position = pos;
                bIsActive = false;
                opacity = 0.0f;

                if (!bInitialize)
                {
                    Initialize();
                }
            }

            private void Initialize()
            {
                bInitialize = true;
                statArrowTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\StatArrow");
                statArrowSource = new Rectangle(0, 0, 50, 52);
                font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");//48,32,25
            }

            internal void Update(GameTime gt)
            {
                if (bIsActive && opacity != 1.0f)
                {
                    opacity += 1.0f / 60f;
                }
                if (opacity >= 1.0f)
                {
                    opacity = 1.0f;
                }
            }

            internal void Draw(SpriteBatch sb)
            {
                if (bIsActive && opacity != 0.0f)
                {
                    TextUtility.Draw(sb, name, font, statNamePos, TextUtility.OutLining.Center, Color.Gold * opacity, 1f, true, default(Matrix), Color.Silver * opacity, false);
                    sb.Draw(statArrowTex, statArrowPos, statArrowSource, Color.White * opacity);

                    Color amountColor = amountNum > 0 ? Color.Green : Color.Red;
                    TextUtility.Draw(sb, amount, font, statAmountPos, TextUtility.OutLining.Center, amountColor * opacity, 1f, true, default(Matrix), Color.White * opacity, false);
                }
            }

            internal bool Equals(StatUpDisplay stud)
            {
                if (name.Equals(stud.name))
                {
                    return true;
                }
                return false;
            }

            internal bool HandleNext()
            {
                if (opacity >= 0.5f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            internal void Start()
            {
                bIsActive = true;
            }
        }
    }
}
