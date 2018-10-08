using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW;

namespace LUA
{
    public static class LuaExecutionList
    {
        public static List<LuaCommand> commandList = new List<LuaCommand>();
        static RenderTarget2D r2d = new RenderTarget2D(TBAGW.Game1.graphics.GraphicsDevice, 1366, 768);

        public static void Add(LuaCommand lc)
        {
            commandList.Add(lc);
        }

        public static void Start()
        {
            GameProcessor.bGameUpdateIsPaused = true;
        }

        public static void Update(GameTime gt)
        {
            if (commandList.Count > 0 && !commandList[0].bInitialized)
            {
                bCompletedProcessingLine = false;
                if (commandList.Count > 0) //Initialize block
                {
                    switch (commandList[0].cType)
                    {
                        case LuaCommand.CommandType.Dialogue:
                            InitializeDialogue(gt);
                            break;
                        case LuaCommand.CommandType.nameChange:

                            break;
                        default:
                            break;
                    }
                }
                commandList[0].bInitialized = true;
            }

            if (commandList.Count > 0 && commandList[0].bIsDone)
            {
                bCompletedProcessingLine = false;
                commandList.RemoveAt(0);
                if (commandList.Count > 0) //Initialize block
                {
                    switch (commandList[0].cType)
                    {
                        case LuaCommand.CommandType.Dialogue:
                            InitializeDialogue(gt);
                            break;
                        case LuaCommand.CommandType.nameChange:

                            break;
                        default:
                            break;
                    }

                    commandList[0].bInitialized = true;
                }
              
            }

            if (commandList.Count > 0)
            {
                switch (commandList[0].cType)
                {
                    case LuaCommand.CommandType.Dialogue:
                        UpdateDialogue(gt);
                        break;
                    case LuaCommand.CommandType.nameChange:
                        ChangeName();
                        break;
                    default:
                        break;
                }
            }

            if (commandList.Count == 0)
            {
                GameProcessor.bGameUpdateIsPaused = false;
                GameScreenEffect.Reset();
            }
        }

        private static void ChangeName()
        {

        }

        private static void InitializeDialogue(GameTime gt)
        {
            letterTimePassed = 0;
            processedPieceTextString = "";
            bCompletedProcessingLine = false;
            var temp = (commandList[0] as LuaDialogue).getDialogue(TBAGW.GameText.Language.English);
            completeScriptLineConversation = temp.text;

        }

        public static RenderTarget2D Draw(SpriteBatch sb, RenderTarget2D mr)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(r2d);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            if (commandList.Count > 0)
            {
                switch (commandList[0].cType)
                {
                    case LuaCommand.CommandType.Dialogue:
                        DrawDialogue(sb);
                        break;
                    case LuaCommand.CommandType.nameChange:
                        break;
                    default:
                        break;
                }
            }
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(mr);
            return r2d;
        }

        private static void DrawDialogue(SpriteBatch sb)
        {
            #region stuff
            var temp = commandList[0] as LuaDialogue;


            Rectangle UL = new Rectangle(100, 500, 9, 9);
            sb.Draw(speechBubbleSource, UL, new Rectangle(0, 0, 3, 3), Color.White);
            Rectangle UM = new Rectangle(109, 500, 1366 - 200 - 9, 9);
            sb.Draw(speechBubbleSource, UM, new Rectangle(4, 0, 32, 3), Color.White);
            Rectangle M = new Rectangle(109, 509, 1366 - 200 - 9, 250);
            sb.Draw(speechBubbleSource, M, new Rectangle(4, 4, 32, 32), Color.White);
            Rectangle LM = new Rectangle(100, 509, 9, 250);
            sb.Draw(speechBubbleSource, LM, new Rectangle(0, 4, 3, 32), Color.White);
            Rectangle RM = new Rectangle(1366 - 100, 509, 9, 250);
            sb.Draw(speechBubbleSource, RM, new Rectangle(0, 4, 3, 32), Color.White);
            Rectangle LL = new Rectangle(100, 759, 9, 9);
            sb.Draw(speechBubbleSource, LL, new Rectangle(0, 37, 3, 3), Color.White);
            Rectangle ML = new Rectangle(109, 759, 1160, 9);
            sb.Draw(speechBubbleSource, ML, new Rectangle(4, 0, 32, 3), Color.White);
            Rectangle LR = new Rectangle(1263, 759, 9, 9);
            sb.Draw(speechBubbleSource, LR, new Rectangle(37, 37, 3, 3), Color.White);
            Rectangle UR = new Rectangle(1263, 500, 9, 9);
            sb.Draw(speechBubbleSource, UR, new Rectangle(37, 0, 3, 3), Color.White);

            Rectangle Lface = new Rectangle(100, 500 - 300, 300, 300);
            try
            {
                if (temp.lbc != null && temp.speakerbc == temp.lbc && temp.lbc.portraitAnimations.Count != 0)
                {
                    temp.lbc.portraitAnimations[(int)TBAGW.Utilities.Characters.BaseCharacter.PortraitExpressions.Neutral].Draw(sb, Lface);
                }
                else if (temp.lbc != null && temp.lbc.portraitAnimations.Count != 0)
                {
                    temp.lbc.portraitAnimations[(int)TBAGW.Utilities.Characters.BaseCharacter.PortraitExpressions.Neutral].Draw(sb, Lface, 0.7f);
                }
            }
            catch
            {
            }


            Rectangle Rface = new Rectangle(1266 - 300, 500 - 300, 300, 300);
            try
            {
                if (temp.rbc != null && temp.speakerbc == temp.rbc && temp.rbc.portraitAnimations.Count != 0)
                {
                    temp.rbc.portraitAnimations[(int)TBAGW.Utilities.Characters.BaseCharacter.PortraitExpressions.Neutral].Draw(sb, Lface, 1.0f, SpriteEffects.FlipHorizontally);
                }
                else if (temp.rbc != null && temp.rbc.portraitAnimations.Count != 0)
                {
                    temp.rbc.portraitAnimations[(int)TBAGW.Utilities.Characters.BaseCharacter.PortraitExpressions.Neutral].Draw(sb, Lface, 0.7f, SpriteEffects.FlipHorizontally);
                }

            }
            catch
            {
            }

            String speakerName = "";
            if (temp.lbc == temp.speakerbc)
            {
                speakerName = temp.lCharInfo.dialogueName;
            }
            else if (temp.rbc == temp.speakerbc)
            {
                speakerName = temp.rCharInfo.dialogueName;
            }
            else
            {
                speakerName = "???";
            }

            //conversation = "Hello, is it my you're looking for? I can see it in your eyes, I can see it in your smiiiiillleee. Is this enough text?";
            sb.DrawString(textWindowFont, speakerName + ":", new Vector2(120, 510), Color.White);
            sb.DrawString(textWindowFont, TBAGW.TextUtility.bestMatchStringForBox(ScriptLineOnlyConversation, textWindowFont, new Rectangle(120, 530, 1123 - 30, 190)), new Vector2(150, 580), Color.White);
            //sb.DrawString(textWindowFont, TextUtility.bestMatchStringForBox(conversation, textWindowFont, new Rectangle(120, 530, 1123 - 30, 190)), new Vector2(150, 580), Color.White);
            // bCompletedProcessingLine = true;
            #endregion
        }

        private static void UpdateDialogue(GameTime gt)
        {
            letterTimePassed += gt.ElapsedGameTime.Milliseconds;
            ProcessNormalText();
        }

        private static int letterTimer = 10;
        private static int letterTimePassed = 0;
        private static String completeScriptLineConversation = "";
        private static String processedPieceTextString = "";
        private static bool bCompletedProcessingLine = false;
        static Texture2D speechBubbleSource = TBAGW.Game1.contentManager.Load<Texture2D>(@"Design\Speech");
        static SpriteFont textWindowFont = TBAGW.Game1.contentManager.Load<SpriteFont>(@"Editor\Scripts\TextWindowFont");
        static String ScriptLineOnlyConversation = "";
        private static void ProcessNormalText()
        {
            if (letterTimePassed > letterTimer)
            {
                String scriptLineTemp = completeScriptLineConversation;
                String scriptLine = scriptLineTemp.Substring(processedPieceTextString.Length, scriptLineTemp.Length - processedPieceTextString.Length);
                if (scriptLine.Length > 4 && scriptLine.StartsWith("<"))
                {
                    if (scriptLine[1].Equals('/') && scriptLine.Length > 4 && scriptLine[4].Equals('>'))
                    {
                        String textTag = scriptLine.Substring(1, 3);
                        processedPieceTextString += "<" + textTag + ">";
                    }
                    else if (!scriptLine[1].Equals('/') && scriptLine[3].Equals('>'))
                    {
                        String textTag = scriptLine.Substring(1, 2);
                        processedPieceTextString += "<" + textTag + ">";
                    }
                }
            }
            //currentDisplayMode = (int)ActiveScriptDisplayMode.Text;
            if (letterTimePassed > letterTimer)
            {

                //String scriptLineTemp = script.scriptContent[script.scriptLineIndex];
                String scriptLineTemp = completeScriptLineConversation;
                String scriptLine = scriptLineTemp.Substring(processedPieceTextString.Length, scriptLineTemp.Length - processedPieceTextString.Length);

                if (!scriptLine.StartsWith("\n") && !processedPieceTextString.Equals(completeScriptLineConversation))
                {
                    processedPieceTextString = processedPieceTextString + scriptLine[0];
                }
                else if (scriptLine.StartsWith("\n"))
                {
                    processedPieceTextString = processedPieceTextString + "\n";
                }
                else if (processedPieceTextString.Equals(completeScriptLineConversation))
                {
                    bCompletedProcessingLine = true;
                }

                letterTimePassed = 0;
                ScriptLineOnlyConversation = actualTextToDisplay();
            }
        }

        private static String actualTextToDisplay()
        {
            String temp = (String)processedPieceTextString.Clone();
            int previousIndex = -1;
            int index = 0;
            while (temp.Contains("<") || previousIndex == index)
            {
                if (index + 1 == temp.Length)
                {
                    break;
                }
                index = temp.Substring(index + 1).IndexOf('<') + index + 1;

                if (index == -1)
                {
                    break;
                }

                if (index < temp.Length - 3)
                {
                    // var test = @temp[index + 1];
                    if (temp[index + 1].Equals('/'))
                    {
                        if (temp[index + 4].Equals('>'))
                        {
                            temp = temp.Remove(index, 5);
                        }
                    }
                    else if (!temp[index + 1].Equals('/'))
                    {
                        //var test = temp[index + 3];
                        if (temp[index + 3].Equals('>'))
                        {
                            temp = temp.Remove(index, 4);
                        }
                    }
                }
                else
                {
                    break;
                }

            }

            return temp;
        }

        internal static bool DemandOverride()
        {
            if (commandList.Count != 0)
            {
                return true;
            }
            return false;
        }

        internal static void ControlOverride(List<TBAGW.Utilities.Actions.ActionKey> pressedKeys)
        {
            switch (commandList[0].cType)
            {
                case LuaCommand.CommandType.Dialogue:
                    DialogueControls(pressedKeys);
                    break;
                case LuaCommand.CommandType.nameChange:
                    break;
                default:
                    break;
            }

        }

        private static void DialogueControls(List<TBAGW.Utilities.Actions.ActionKey> pressedKeys)
        {
            TBAGW.Utilities.Actions.ActionKey key = pressedKeys.Find(k => k != null);
            if (key.actionIndentifierString.Equals(TBAGW.Game1.confirmString) && !TBAGW.Utilities.KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (completeScriptLineConversation.Equals(processedPieceTextString, StringComparison.OrdinalIgnoreCase))
                {
                    bCompletedProcessingLine = true;
                    commandList[0].bIsDone = true;
                }
                else
                {
                    while (!completeScriptLineConversation.Equals(processedPieceTextString, StringComparison.OrdinalIgnoreCase))
                    {
                        letterTimePassed = 9999999;
                        ProcessNormalText();
                    }
                }
                TBAGW.Utilities.KeyboardMouseUtility.bPressed = true;
            }
        }

        internal static RenderTarget2D GetRender()
        {
            return r2d;
        }
    }

    public abstract class LuaCommand
    {
        public enum CommandType { Dialogue, nameChange }
        internal CommandType cType = CommandType.Dialogue;
        internal bool bIsDone = false;
        internal bool bInitialized = false;

        public LuaCommand() { }
    }
}
