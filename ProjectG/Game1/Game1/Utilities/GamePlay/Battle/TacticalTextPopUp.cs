using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    internal class TacticalTextPopUp
    {
        String text;
        Vector2 location;
        BaseCharacter parent;
        Rectangle textBox;
        internal Color oriTextColor = Color.Black;
        internal Color oriBgTextColor = Color.Silver;
        internal Color textColor = Color.Black;
        internal Color bgTextColor = Color.Silver;
        internal TimingUtility timer;
        internal bool bRemove = false;

        internal delegate void UpdateFunc(GameTime gt, TacticalTextPopUp pop);
        UpdateFunc updateFunction;
        internal delegate void DrawFunc(SpriteBatch sb, TacticalTextPopUp pop);
        DrawFunc drawFunction;

        static SpriteFont font;
        static bool bInitialize = true;

        internal static void Inititialize()
        {
            bInitialize = false;
            font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32"); //48,32,25,20
        }

        internal TacticalTextPopUp(String s, BaseCharacter p, Vector2 offSet, Point textBoxSize, int steps, UpdateFunc uf = null, DrawFunc df = null)
        {
            if (bInitialize) { Inititialize(); }
            text = s;
            parent = p;
            location = p.trueMapSize().Location.ToVector2() + offSet;
            textBox = new Rectangle(location.ToPoint(), textBoxSize);
            timer = new TimingUtility(16, true, standardTimer);
            timer.SetStepTimer(steps, 0);
            updateFunction = uf;
            drawFunction = df;
        }

        internal void InitializeTextColor(Color tc, Color ltc)
        {
            oriTextColor = tc;
            oriBgTextColor = ltc;
            SetTextColor(tc, ltc);
        }

        internal void SetTextColor(Color tc, Color ltc)
        {
            textColor = tc;
            bgTextColor = ltc;
        }

        internal void Update(GameTime gt)
        {


            if (updateFunction != null)
            {
                updateFunction(gt, this);
            }
        }

        internal void Draw(SpriteBatch sb)
        {
            if (drawFunction != null)
            {
                drawFunction(sb, this);
            }
            else
            {
                try
                {
                    TextUtility.Draw(sb, text, font, textBox, TextUtility.OutLining.Left, textColor, 1f, true, GameProcessor.CameraScaleMatrix, bgTextColor, false);
                    sb.End();
                }
                catch (Exception e)
                {

                }
            }
        }

        internal bool standardTimer()
        {
            if (timer.percentageDone() >= 1.0f)
            {
                return true;
            }
            return false;
        }
    }

    internal class TacticalTextManager
    {
        internal List<TacticalTextPopUp> texts = new List<TacticalTextPopUp>();
        static RenderTarget2D render = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

        internal void AddText(String s, BaseCharacter p, Vector2 os, Point tbs, int steps)
        {
            texts.Add(new TacticalTextPopUp(s, p, os, tbs, steps, UpdateAreaCombatText));
        }

        internal void Update(GameTime gt)
        {
            if (BattleGUI.bHandleAreaAttack && !BattleGUI.castAbilityGBC.PAanim.bAnimationFinished && !BattleGUI.bIsRunning)
            {
                BattleGUI.castAbilityGBC.PAanim.UpdateAnimationForItems(gt);
            }

            for (int i = 0; i < texts.Count; i++)
            {
                texts[i].Update(gt);
            }
            texts.RemoveAll(t => t.bRemove);
        }

        internal void GenerateRender(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            for (int i = 0; i < texts.Count; i++)
            {
                texts[i].Draw(sb);
            }
            sb.End();

            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
            if (BattleGUI.bHandleAreaAttack && !BattleGUI.castAbilityGBC.PAanim.bAnimationFinished && !BattleGUI.bIsRunning)
            {
                for (int i = 0; i < BattleGUI.charsForHandleArea.Count; i++)
                {
                    Point offset = new Point(40);
                    Rectangle size = new Rectangle(BattleGUI.charsForHandleArea[i].trueMapSize().Location, BattleGUI.charsForHandleArea[i].trueMapSize().Size);
                    size.Location -= offset + new Point(0, 20);
                    size.Size += offset + offset;
                    BattleGUI.castAbilityGBC.PAanim.Draw(sb, size);
                }
            }else if (BattleGUI.bHandleAreaAttack && BattleGUI.castAbilityGBC.PAanim.bAnimationFinished && !BattleGUI.bIsRunning)
            {
                BattleGUI.charsForHandleArea.Clear();
            }

            sb.End();
        }

        internal RenderTarget2D getRender()
        {
            return render;
        }

        internal void UpdateAreaCombatText(GameTime gt, TacticalTextPopUp pop)
        {
            pop.bRemove = pop.timer.IsDone();
            pop.SetTextColor(pop.oriTextColor * (1.0f - pop.timer.percentageDone()), pop.oriBgTextColor * (1.0f - pop.timer.percentageDone()));
            pop.timer.Tick(gt);
        }
    }
}
