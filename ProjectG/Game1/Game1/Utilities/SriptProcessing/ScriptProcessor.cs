using Game1.Utilities.GamePlay.Battle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TBAGW.AI;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.Sprite;

namespace TBAGW.Utilities.SriptProcessing
{
    static public class ScriptProcessor
    {
        public enum ActiveScriptDisplayMode { Text = 0, Choice, Conversation, None }
        static String ScriptsFolder = Environment.CurrentDirectory + @"\Scripts\";
        public static BasicMap gameMap;
        public static BaseScript script = new BaseScript();
        public static bool bScriptRunning = false;
        public static bool bCompletedProcessingLine = false;
        public static bool bIsRunning = false;
        public static String processedPieceTextString = "";
        private static int letterTimer = 10; //ms
        private static int letterTimePassed = 0;
        private static int ControllerIndex = -1;

        public static int mapPartyIndex = -1;
        public static BaseSprite mapHero;
        static public int currentDisplayMode = (int)ActiveScriptDisplayMode.None;

        static Texture2D textWindowBG = Game1.contentManager.Load<Texture2D>(@"Editor\Scripts\TextWindowBG");
        static SpriteFont textWindowFont = Game1.contentManager.Load<SpriteFont>(@"Editor\Scripts\TextWindowFont");
        static Rectangle textWindow = new Rectangle(183, 420, 1000, 300);
        static Rectangle textWindowInputRoom = new Rectangle(233, 434, 900, 250);
        static public List<BaseScript> backgroundScripts = new List<BaseScript>();

        public enum ChoiceBox { Choice_1 = 0, Choice_2, Choice_3, Choice_4 }
        static int choicesAvailable = 0;
        static List<String> choiceText = new List<string>();

        static public List<Object> scriptTempObjects = new List<object>();

        static public Vector2 startChoicePos = new Vector2(182, 192);
        static public int choiceTextOffSetX = 96;
        static public int choiceTextOffSetY = 16;
        static public int widthChoiceText = 1002;
        static public int heightBetweenChoiceText = 32;
        static public List<Rectangle> choices = new List<Rectangle>();

        static public List<Vector2> choicesTextPos = new List<Vector2>();

        static Texture2D choiceIndicator = Game1.contentManager.Load<Texture2D>(@"Design\ChoiceIndicator");
        static int selectedChoice = -1;
        static internal ScriptBool changingBool = new ScriptBool();
        static String completeScriptLineConversation = "";
        static String ScriptLineOnlyConversation = "";

        static object activator;
        static object activatedFrom;

        static bool bInitialize = true;
        static TexPanel selectedTexPanel = null;

        static TexPanel normalTextPanel;
        static Rectangle normalTextPanelPos;
        static Rectangle normalTextPanelTextPos;
        static TexPanel choiceTexPanelSource;
        static Texture2D texSource;
        static List<TexPanel> choiceTexPanels = new List<TexPanel>();

        static void Initialize()
        {
            bInitialize = false;
            texSource = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
            choiceTexPanelSource = new TextTexPanel(texSource, new Rectangle(50, 50, 50, 50), new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
            normalTextPanelPos = new Rectangle((1366 - 900) / 2, 768 - 50 - 360, 900, 360);
            normalTextPanelTextPos = new Rectangle(normalTextPanelPos.Location + new Point(25), normalTextPanelPos.Size - new Point(50));
            normalTextPanel = choiceTexPanelSource.positionCopy(normalTextPanelPos);
        }

        static void ChangeScriptContent(int scriptIdentifier)
        {
            foreach (var script in gameMap.listOfAllUniqueScripts)
            {
                if (script.identifier == scriptIdentifier)
                {

                }
            }
        }

        public static void ChangeActiveTestScript(BaseScript newscript, bool bActivate = true)
        {
            script = newscript;
            bScriptRunning = bActivate;

            if (bInitialize)
            {
                Initialize();
            }
        }

        public static void ChangeActiveScript(BaseScript newscript, object from, object by, bool bActivate = true)
        {
            KeyboardMouseUtility.bPressed = true;
            if (bInitialize)
            {
                Initialize();
            }

            charLeftIDAssigned = -1; charRightIDAssigned = -1; charSpeakerIDAssigned = -1;
            if (newscript != null && (script.identifier != newscript.identifier || bActivate) && !bIsRunning && newscript.scriptLineIndex < newscript.scriptContent.Count)
            {

                TextUtility.ClearCache();
                activator = by;
                activatedFrom = from;
                newscript.scriptLineIndex = newscript.repeatLine;
                if (newscript.scriptContent.Count != 0 && newscript.scriptContent[0].Length >= 4)
                {
                    String commandString = newscript.scriptContent[0].Substring(0, 4);
                    if (!commandString.StartsWith("@TBS"))
                    {
                        script = newscript;
                        //  script.scriptLineIndex = 0;
                        GameProcessor.bGameUpdateIsPaused = true;
                        bIsRunning = true;
                        bScriptRunning = bActivate;
                        ProcessCurrentScriptLine();
                    }
                    else
                    {
                        if (backgroundScripts.FindAll(s => s.identifier == newscript.identifier).Count == 0)
                        {
                            newscript.scriptLineIndex = 0;
                            newscript.scriptLineIndex++;
                            backgroundScripts.Add(newscript);
                        }
                    }
                }
                else
                {
                    script = newscript;

                    GameProcessor.bGameUpdateIsPaused = true;
                    bIsRunning = true;
                    bScriptRunning = bActivate;
                    ProcessCurrentScriptLine();
                }

            }


        }

        public static void AssignCombatScript(int charLeftIDAssignedTemp, int charRightIDAssignedTemp, int charSpeakerIDAssignedTemp)
        {
            charLeftIDAssigned = charLeftIDAssignedTemp;
            charRightIDAssigned = charRightIDAssignedTemp;
            charSpeakerIDAssigned = charSpeakerIDAssignedTemp;
        }

        internal static void UpdateBGScript(GameTime gt)
        {
            foreach (var item in backgroundScripts)
            {

            }
        }

        static public bool bStopUpdatingScriptLine = false;
        public static void Update(GameTime gameTime)
        {
            if (completeScriptLineConversation.Equals("") && currentDisplayMode == (int)ActiveScriptDisplayMode.Text)
            {
                currentDisplayMode = (int)ActiveScriptDisplayMode.None;
            }

            foreach (var item in ScriptProcessor.scriptTempObjects)
            {
                if (item is NPC)
                {
                    (item as NPC).Update(gameTime);
                    (item as NPC).baseCharacter.Update(gameTime);
                }
            }
            ScriptProcessorLogic();

            if (bScriptRunning && !bStopUpdatingScriptLine && !PathMoveHandler.bIsBusy)
            {
                //HandleNextLine();
                letterTimePassed += gameTime.ElapsedGameTime.Milliseconds;
                if (currentDisplayMode != (int)ActiveScriptDisplayMode.Text)
                {
                    ProcessCurrentScriptLine();
                }
                else
                {
                    if (!completeScriptLineConversation.Equals(script.scriptContent[script.scriptLineIndex], StringComparison.OrdinalIgnoreCase))
                    {

                    }
                    if (PlayerController.selectedSprite != null)
                    {
                        (PlayerController.selectedSprite as BaseCharacter).animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                    }
                    ProcessNormalText();
                }

            }

            if (PathMoveHandler.bIsBusy)
            {
                PathMoveHandler.Update(gameTime);
            }
            //StopProcessor();
            if (bCompletedProcessingLine && LUA.LuaExecutionList.commandList.Count == 0)
            {

                if (currentDisplayMode == (int)ActiveScriptDisplayMode.Conversation)
                {
                    letterTimePassed += gameTime.ElapsedGameTime.Milliseconds;
                    ProcessNormalText();
                }

            }
            else if (currentDisplayMode == (int)ActiveScriptDisplayMode.Conversation)
            {
                letterTimePassed += gameTime.ElapsedGameTime.Milliseconds;
                ProcessNormalText();
            }

            switch (currentDisplayMode)
            {
                case (int)ActiveScriptDisplayMode.Choice:


                    break;
            }

            if (PlayerController.currentController == PlayerController.Controllers.ScriptProcessor)
            {
                PlayerController.Update(gameTime);
            }

        }

        internal static void ChoiceHandleMouseClick()
        {
            try
            {
                Rectangle choiceChosen = choices.Find(box => box.Contains(CursorUtility.cursorPos));
                if (choiceChosen != null)
                {
                    selectedChoice = choiceTexPanels.IndexOf(selectedTexPanel);

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        changingBool.scriptChoice = selectedChoice;
                        Console.WriteLine("Clicked");
                        ScriptProcessorCtrl.Stop();
                        PlayerSaveData.AdjustBool(changingBool);
                        currentDisplayMode = (int)ActiveScriptDisplayMode.None;
                        script.scriptLineIndex++;
                    }
                }
                else
                {
                    selectedChoice = -1;
                }
            }
            catch
            {
            }

        }

        internal static void HandleChoiceMouseMove(Point uiMousePos)
        {
            selectedTexPanel = choiceTexPanels.Find(tp => tp.ContainsMouse(uiMousePos));
        }

        internal static void HandleChoiceMoveUp()
        {
            int index = choiceTexPanels.IndexOf(selectedTexPanel);
            index--;
            if (index < 0)
            {
                index = choiceTexPanels.Count - 1;
            }
            selectedTexPanel = choiceTexPanels[index];
        }

        internal static void HandleChoiceMoveDown()
        {
            int index = choiceTexPanels.IndexOf(selectedTexPanel);
            index++;
            if (index >= choiceTexPanels.Count)
            {
                index = 0;
            }
            selectedTexPanel = choiceTexPanels[index];
        }

        internal static void HandleChoiceConfirmButton()
        {
            if (selectedTexPanel != null)
            {
                if (choiceTexPanels.Contains(selectedTexPanel))
                {
                    selectedChoice = choiceTexPanels.IndexOf(selectedTexPanel);

                    changingBool.scriptChoice = selectedChoice;
                    Console.WriteLine("Clicked");
                    ScriptProcessorCtrl.Stop();
                    PlayerSaveData.AdjustBool(changingBool);
                    currentDisplayMode = (int)ActiveScriptDisplayMode.None;
                    KeyboardMouseUtility.bPressed = true;
                    script.scriptLineIndex++;

                }
            }
        }

        internal static void ConversationTextConfirmHandle()
        {
           // if (buttonPressUtility.isPressed(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                ScriptProcessorCtrl.Stop();
                script.scriptLineIndex++;
                if (currentDisplayMode != (int)ActiveScriptDisplayMode.Conversation)
                {
                    currentDisplayMode = (int)ActiveScriptDisplayMode.None;
                }
                KeyboardMouseUtility.bPressed = true;

                if (script.scriptLineIndex != script.scriptContent.Count)
                {
                    //script.scriptLineIndex++;
                    bCompletedProcessingLine = false;
                    bStopUpdatingScriptLine = false;
                    processedPieceTextString = "";
                    letterTimePassed = 0;
                    completeScriptLineConversation = "";
                }
                //else
                //{
                //    script.scriptLineIndex = script.repeatLine;
                //    bCompletedProcessingLine = true;
                //    bIsRunning = false;
                //    GameProcessor.bGameUpdateIsPaused = false;
                //    bStopUpdatingScriptLine = false;
                //    processedPieceTextString = "";
                //    letterTimePassed = 0;
                //    GameScreenEffect.currentEffect = GameScreenEffect.Effects.None;
                //}

                if (script.scriptLineIndex == script.scriptContent.Count)
                {
                    //script.scriptLineIndex = script.repeatLine;
                    bCompletedProcessingLine = true;
                    bIsRunning = false;
                    GameProcessor.bGameUpdateIsPaused = false;
                    bStopUpdatingScriptLine = false;
                    processedPieceTextString = "";
                    letterTimePassed = 0;
                    GameScreenEffect.Reset();
                    StopProcessor();
                }
            }

        }

        static private void HandleNextLine()
        {
            if (currentDisplayMode == (int)ActiveScriptDisplayMode.None)
            {
                script.scriptLineIndex++;

                if (script.scriptLineIndex != script.scriptContent.Count)
                {
                    //script.scriptLineIndex++;
                    bCompletedProcessingLine = false;
                    bStopUpdatingScriptLine = false;
                    processedPieceTextString = "";
                    letterTimePassed = 0;
                }
            }



            //else
            //{
            //    script.scriptLineIndex = script.repeatLine;
            //    bCompletedProcessingLine = true;
            //    bIsRunning = false;
            //    GameProcessor.bGameUpdateIsPaused = false;
            //    bStopUpdatingScriptLine = false;
            //    processedPieceTextString = "";
            //    letterTimePassed = 0;
            //    GameScreenEffect.currentEffect = GameScreenEffect.Effects.None;
            //}

            if (script.scriptLineIndex == script.scriptContent.Count)
            {
                script.scriptLineIndex = script.repeatLine;
                bCompletedProcessingLine = true;
                bIsRunning = false;
                GameProcessor.bGameUpdateIsPaused = false;
                bStopUpdatingScriptLine = false;
                processedPieceTextString = "";
                letterTimePassed = 0;
                GameScreenEffect.currentEffect = GameScreenEffect.Effects.None;
                StopProcessor();
            }
        }

        private static void ProcessCurrentScriptLine()
        {
            //Console.WriteLine(script.scriptContent.Count + "    " + script.scriptLineIndex);
            String scriptLine = "";
            try
            {
                if (script.scriptLineIndex == script.scriptContent.Count)
                {
                    StopProcessor();
                    goto skip;
                }
                scriptLine = script.scriptContent[script.scriptLineIndex];
            }
            catch
            {
                StopProcessor();
            }

            if (!scriptLine.StartsWith("@"))
            {
                currentDisplayMode = (int)ActiveScriptDisplayMode.Text;
                ScriptProcessorCtrl.Start();
                if (script.scriptLineIndex < script.scriptContent.Count && !scriptLine.Equals(""))
                {
                    //completeScriptLineConversation = script.scriptContent[script.scriptLineIndex];
                    ProcessText(script.scriptContent[script.scriptLineIndex]);
                }

                ProcessNormalText();
            }
            else if (currentDisplayMode != (int)ActiveScriptDisplayMode.Choice)
            {
                ProcessScriptCommand(activatedFrom as BaseSprite);
                if (currentDisplayMode == (int)ActiveScriptDisplayMode.Conversation)
                {
                    ProcessNormalText();
                }
            }

        skip: { }
        }

        private static void ProcessText(String completeLine)
        {
            completeScriptLineConversation = completeLine;
            letterTimer = 10;
            letterTimePassed = 0;
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

        private static void DisplayConversationNPC(string commandStringAddition)
        {

            var args = commandStringAddition.Split('_').ToList();
            //@MSG_SpeakerID_charLeftID_charRightID_charLeft_Exp_CharRightExp_conv
            try
            {
                int charSpeakerID = int.Parse(args[0]);
                int charLeftID = int.Parse(args[1]);
                int charRightID = int.Parse(args[2]);
                int charLeftExpression = int.Parse(args[3]);
                int charRightExpression = int.Parse(args[4]);
                String conversation = args[5];


                if (charLeftIDAssigned != -1)
                {
                    charLeftID = charLeftIDAssigned;
                }

                if (charRightIDAssigned != -1)
                {
                    charRightID = charRightIDAssigned;
                }

                if (charSpeakerIDAssigned != -1)
                {
                    charSpeakerID = charSpeakerIDAssigned;
                }

                speaker = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == charSpeakerID);
                charLeft = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == charLeftID);
                charRight = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == charRightID);

                BaseCharacter.PortraitExpressions speakerExpression = BaseCharacter.PortraitExpressions.Neutral;
                if (speaker == charLeft)
                {
                    speakerExpression = (BaseCharacter.PortraitExpressions)charLeftExpression;
                }
                else if (speaker == charRight)
                {
                    speakerExpression = (BaseCharacter.PortraitExpressions)charRightExpression;
                }

                currentDisplayMode = (int)ActiveScriptDisplayMode.Conversation;
                bStopUpdatingScriptLine = true;

                if (GameScreenEffect.currentEffect != GameScreenEffect.Effects.Conversation)
                {
                    GameScreenEffect.InitializeConversationEffect();
                }
                try
                {
                    bCompletedProcessingLine = false;
                    GameProcessor.bGameUpdateIsPaused = true;
                    // completeScriptLineConversation = conversation;
                    ProcessText(conversation);
                    //bIsRunning = false;
                }
                catch
                {
                    Console.WriteLine("Error found trying to play the sound effect");
                }

                try
                {
                    if (lastInstance != null)
                    {
                        lastInstance.Stop();
                    }
                    lastInstance = speaker.PlayRandomConvExpression(speakerExpression);
                }
                catch (Exception)
                {

                }
            }
            catch (Exception)
            {
            }
        }

        private static void ProcessScriptCommand(BaseSprite s = default(BaseSprite))
        {
            String commandString = script.scriptContent[script.scriptLineIndex].Substring(0, 4);
            String commandStringAddition = "";
            try
            {
                commandStringAddition = script.scriptContent[script.scriptLineIndex].Substring(4);
            }
            catch
            {
                StopProcessor();
            }

            try
            {
                if (commandString.Equals("@TRE"))
                {
                    var arg = script.scriptContent[script.scriptLineIndex].Substring(5).Split('_')[0];
                    HandleRepeatTag(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.Equals("@TSP"))
                {
                    String commandInfo = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
                    HandleStartPointCommand(int.Parse(commandInfo), s);
                }
                else if (commandString.StartsWith("@AST"))
                {
                    script.scriptLineIndex++;
                }
                else if (commandString.StartsWith("@MCT"))
                { //@T4C-boolID@text1@text2@text3@text4
                    HandleChoiceTag(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@TIF"))
                {
                    HandleIfCommand(commandStringAddition);
                }
                else if (commandString.StartsWith("@TIS"))
                {
                    ChangeVisibility(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@TSE"))
                {
                    PlaySoundEffectLogic(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@END"))
                {
                    script.scriptLineIndex++;
                    //     StopProcessor();
                }
                else if (commandString.StartsWith("@TPA"))
                {
                    PlayAnimationLogic(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@PAE"))
                {
                    PlayAnimationEndWithLogic(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@TSM"))
                {
                    ChangeSubMapLogic(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@MSG"))
                {
                    DisplayConversation(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@PMW"))
                {
                    CreateTempNpc(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                //else if (commandString.StartsWith("@TBB")) //COMBINES WITH @TBE
                //{
                //    HandleTagBeforeBattle(script.scriptContent[script.scriptLineIndex].Substring(5));
                //}
                //else if (commandString.StartsWith("@TAB")) //COMBINES WITH @TAE
                //{
                //    HandleTagAfterBattle(script.scriptContent[script.scriptLineIndex].Substring(5));
                //}
                else if (commandString.StartsWith("@TRC")) //COMBINES WITH @TRF (Tag random Finish))
                {
                    HandleTagRandomEvent(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@TGO")) //Combins with @THE
                {
                    HandleTagGoto(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@TMC")) //TagMoveCamera
                {
                    HandleTagMoveCamera(script.scriptContent[script.scriptLineIndex]);
                }
                else if (commandString.StartsWith("@TCR")) //TagMoveCamera
                {
                    HandleTagCameraReset(script.scriptContent[script.scriptLineIndex]);
                }
                else if (commandString.StartsWith("@TCC")) //TagMoveCamera
                {
                    HandleTagCameraCentre(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@COS")) //TagMoveCamera
                {
                    HandleChangeOpacitySprite(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@CWB")) //TagMoveCamera
                {
                    HandleChangeWorldBrightness(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@TPT")) //TagMoveCamera
                {
                    HandleTeleportTo(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@PSE")) //TagMoveCamera
                {
                    HandlePlaySoundEffect(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@GMP")) //TagMoveCamera
                {
                    HandleGenerateMovePathPlayerMove(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@PSS")) //TagMoveCamera
                {
                    HandleSummonSpriteLight(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@CMT")) //TagMoveCamera
                {
                    HandleContinueMoveTempSprite(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@DLH")) //TagMoveCamera
                {
                    HandleDayLightHandlerCmd(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@TGT")) //TagMoveCamera
                {
                    HandleTagGameTimeCmd(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@TLH")) //TagMoveCamera
                {
                    HandleTagLuaHandle(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@DST")) //TagMoveCamera
                {
                    HandleTagDaySetTime(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@DTT")) //TagMoveCamera
                {
                    HandleTagDayTickTime(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@CTM")) //TagMoveCamera
                {
                    HandleTagCameraToPlayer();
                }
                else if (commandString.StartsWith("@CSB")) //TagChangeScriptBool
                {
                    HandleTagChangeScriptBool(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@CPP")) //TagChangePlayerPosition -instant-
                {
                    HandleTagChangePlayerPosition(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@TLB")) //TagChangePlayerPosition -instant-
                {
                    HandleTagLaunchBattle(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else if (commandString.StartsWith("@TPH")) //TagChangePlayerPosition -instant-
                {
                    HandleTagPlayerHeal(script.scriptContent[script.scriptLineIndex].Substring(5));
                }
                else
                {
                    Console.WriteLine("Command not found: " + commandString + " ...Skipping to next commandline");
                    script.scriptLineIndex++;
                }
            }
            catch
            {

            }

        }

        private static void HandleTagPlayerHeal(string v)
        {
            foreach (var item in PlayerSaveData.heroParty)
            {
                item.Heal();
            }
            script.scriptLineIndex++;
            ProcessCurrentScriptLine();
        }

        private static void HandleTagLaunchBattle(string commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();
            int min = int.Parse(args[0]);
            int max = int.Parse(args[1]);
            if (min != -1 || max != -1)
            {
                EncounterGenerator.cSpawnAmount = new EncounterGenerator.customSpawnAmount(Math.Abs(min), Math.Abs(max));
            }

            StopProcessor();
            GameProcessor.AttemptStartRandomZoneFight();
            script.scriptLineIndex++;
            ProcessCurrentScriptLine();
        }

        private static void HandleTagChangePlayerPosition(string commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();
            String pointString = args[0];
            var pointsArgs = commandStringAddition.Split(',').ToList();
            int x = int.Parse(pointsArgs[0]);
            int y = int.Parse(pointsArgs[1]);
            Point p = new Point(x * 64, y * 64);
            PlayerController.selectedSprite.position = p.ToVector2();
            PlayerController.selectedSprite.UpdatePosition();
            GameProcessor.loadedMap.ForceCheckChunksToConsider();
            script.scriptLineIndex++;
            ProcessCurrentScriptLine();
        }

        private static void HandleTagChangeScriptBool(string commandStringAddition)
        {
            try
            {
                var args = commandStringAddition.Split('_').ToList();
                int boolID = int.Parse(args[0]);
                int scriptChoice = int.Parse(args[1]);
                bool isOn = bool.Parse(args[2]);


                ScriptBool sb = PlayerSaveData.getBool(boolID);
                sb.scriptChoice = scriptChoice;
                sb.isOn = isOn;
                PlayerSaveData.AdjustBool(sb);


                script.scriptLineIndex++;
                ProcessCurrentScriptLine();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void HandleTagCameraToPlayer()
        {
            try
            {
                if (PlayerController.selectedSprite != null)
                {
                    GameProcessor.GenerateCamera(PlayerController.selectedSprite, 0, GameProcessor.zoom);
                }

                script.scriptLineIndex++;
                ProcessCurrentScriptLine();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void HandleTagDayTickTime(string commandStringAddition)
        {
            try
            {
                var args = commandStringAddition.Split('_').ToList();
                int tickTime = int.Parse(args[0]);

                try
                {
                    DayLightHandler.setTickTime(tickTime);
                }
                catch (Exception e)
                {
                    throw;
                }

                script.scriptLineIndex++;
                ProcessCurrentScriptLine();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void HandleTagDaySetTime(string commandStringAddition)
        {
            try
            {
                var args = commandStringAddition.Split('_').ToList();
                int hours = int.Parse(args[0]);
                int minutes = int.Parse(args[1]);
                bool bPause = bool.Parse(args[2]);

                try
                {

                    DayLightHandler.TimeToLight(hours * 60 + minutes);
                    DayLightHandler.bPaused = bPause;
                }
                catch (Exception e)
                {
                    throw;
                }

                script.scriptLineIndex++;
                ProcessCurrentScriptLine();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void HandleTagLuaHandle(string commandStringAddition)
        {
            try
            {
                var args = commandStringAddition.Split('_').ToList();
                var luaLoc = args[0];
                var luaCommand = args[1];

                try
                {

                    NLua.Lua state2 = new NLua.Lua();
                    state2.LoadCLRPackage();
                    if (luaLoc.StartsWith(@"\"))
                    {
                        luaLoc = luaLoc.Remove(0, 1);
                    }
                    String comp = System.IO.Path.Combine(Game1.rootContent, luaLoc);
                    state2.DoFile(comp);
                    if (state2.GetFunction(luaCommand) != null)
                    {
                        (state2[luaCommand] as NLua.LuaFunction).Call();
                    }

                }
                catch (Exception e)
                {
                    throw;
                }

                script.scriptLineIndex++;
                ProcessCurrentScriptLine();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void HandleTagGameTimeCmd(string commandStringAddition)
        {
        }

        private static void HandleDayLightHandlerCmd(string commandStringAddition)
        {

            try
            {
                var args = commandStringAddition.Split('_').ToList();

                switch (args.Count)
                {
                    case 1:
                        bool pause = bool.Parse(args[0]);
                        DayLightHandler.bPaused = pause;

                        break;
                    case 2:
                        pause = bool.Parse(args[0]);
                        bool bIsInside = bool.Parse(args[1]);
                        DayLightHandler.bPaused = pause;
                        GameProcessor.bIsOverWorldOutsideGame = bIsInside;
                        break;
                    default:
                        break;
                }
                script.scriptLineIndex++;
                ProcessCurrentScriptLine();
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: error @DLH");
            }
        }

        private static void HandleContinueMoveTempSprite(string commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();

            try
            {
                int tempCharID = int.Parse(args[0]);
                var path = args[1].Split(',').Select(Int32.Parse).ToList();

                NPC moverNPC = scriptTempObjects.Where(obj => obj.GetType() == typeof(NPC)).ToList().Find(npc => (npc as NPC).identifier == tempCharID) as NPC;

                if (moverNPC != null)
                {
                    for (int i = 0; i < path.Count; i += 2)
                    {
                        moverNPC.NPCArea.Add(GameProcessor.loadedMap.tilesOnPosition(new Vector2(path[i], path[i + 1]), 0)[0]);
                        moverNPC.characterIsFinishedMoving = false;
                    }
                }

                script.scriptLineIndex++;
            }
            catch (Exception)
            {
                Console.WriteLine("Error in HandleContinueMoveTempSprite");
            }


        }

        private static void HandleSummonSpriteLight(string commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();
            //@PSE_ID_Volume_Fade
            try
            {
                int lightID = int.Parse(args[0]);
                float lightScale = float.Parse(args[1].Replace('f', ' '));
                var lightPos = args[2].Split(',').Select(Int32.Parse).ToList();
                var end = new Vector2(lightPos[0] * 64, lightPos[1] * 64);
                Color lightColor = Color.White;
                try
                {
                    var movePosition = args[3].Split(',').Select(Int32.Parse).ToList();
                    lightColor = new Color(movePosition[0], movePosition[1], movePosition[2]);
                }
                catch (Exception)
                {
                    lightColor = Color.White;
                }


                SpriteLight temp = GameProcessor.gcDB.gameLights.Find(light => light.lightID == lightID).Clone();
                temp.lightScale = lightScale;
                temp.lightColor = lightColor;
                temp.position = end;
                temp.UpdatePosition();

                GameProcessor.loadedMap.activeLights.Add(temp);
                GameProcessor.loadedMap.mapLights.Add(temp);
                scriptTempObjects.Add(temp);
                script.scriptLineIndex++;
            }
            catch (Exception)
            {
            }
        }

        private static void HandleGenerateMovePathPlayerMove(string commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();
            //@CWB_FADE_TIME
            try
            {
                var movePosition = args[0].Split(',').Select(Int32.Parse).ToList();
                var end = new Vector2(movePosition[0] * 64, movePosition[1] * 64);
                int endRotation = -1;
                bool bHasEndRotation = int.TryParse(args[1], out endRotation);
                (PlayerController.selectedSprite).position = new Vector2(((int)(PlayerController.selectedSprite.position.X / 64)) * 64, ((int)(PlayerController.selectedSprite.position.Y / 64)) * 64);

                var regionsThatContainPlayer = GameProcessor.loadedMap.mapRegions.FindAll(r => r.Contains(PlayerController.selectedSprite));

                foreach (var region in regionsThatContainPlayer)
                {
                    //foreach (var zone in region.regionZones)
                    //{
                    //    if (zone.Contains(PlayerController.selectedSprite))
                    //    {
                    //        var tiles = GameProcessor.loadedMap.possibleTilesWithController(zone.returnBoundingZone());
                    //        try
                    //        {
                    //            var allPossibleNodes = PathFinder.NewPathSearch(PlayerController.selectedSprite.position, end, tiles);
                    //            allPossibleNodes.Reverse();
                    //            var lastVector = new Vector2(allPossibleNodes[allPossibleNodes.Count - 1].coord.X * 64, allPossibleNodes[allPossibleNodes.Count - 1].coord.Y * 64);
                    //            if (lastVector == end)
                    //            {
                    //                // (PlayerController.selectedSprite as BaseCharacter).changePosition(new Vector2(allPossibleNodes[1].coord.X * 64, allPossibleNodes[1].coord.Y * 64));
                    //                allPossibleNodes.Reverse();
                    //                var NodesWithinRange = MapListUtility.findEqualNodesToTileList(allPossibleNodes, tiles);
                    //                //PathMoveHandler.Start(gts.character, NodesWithinRange);
                    //                PathMoveHandler.Start(PlayerController.selectedSprite, NodesWithinRange, endRotation);
                    //                bCompletedProcessingLine = false;
                    //                if(bHasEndRotation)
                    //                {
                    //                    NonCombatCtrl.ri = endRotation;
                    //                }
                    //            }
                    //        }
                    //        catch (Exception)
                    //        {
                    //            script.scriptLineIndex++;
                    //        }
                    //    }
                    //}
                    var tiles = GameProcessor.loadedMap.possibleTilesWithController(region.returnBoundingZone(), (PlayerController.selectedSprite));
                    try
                    {
                        var allPossibleNodes = PathFinder.NewPathSearch(PlayerController.selectedSprite.position, end, tiles);
                        allPossibleNodes.Reverse();
                        var lastVector = new Vector2(allPossibleNodes[allPossibleNodes.Count - 1].coord.X * 64, allPossibleNodes[allPossibleNodes.Count - 1].coord.Y * 64);
                        if (lastVector == end)
                        {
                            // (PlayerController.selectedSprite as BaseCharacter).changePosition(new Vector2(allPossibleNodes[1].coord.X * 64, allPossibleNodes[1].coord.Y * 64));
                            allPossibleNodes.Reverse();
                            var NodesWithinRange = MapListUtility.findEqualNodesToTileList(allPossibleNodes, tiles);
                            //PathMoveHandler.Start(gts.character, NodesWithinRange);
                            PathMoveHandler.Start(PlayerController.selectedSprite, NodesWithinRange, endRotation);
                            bCompletedProcessingLine = false;
                            if (bHasEndRotation)
                            {
                                NonCombatCtrl.ri = endRotation;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        script.scriptLineIndex++;
                    }
                }


                script.scriptLineIndex++;
            }
            catch (Exception)
            {
            }
        }

        private static void HandlePlaySoundEffect(string commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();
            //@PSE_ID_Volume_Fade
            try
            {
                int sfxID = int.Parse(args[0]);
                int volume = 0;
                bool bHasVolume = false;
                if (args.Count > 1)
                {
                    bHasVolume = int.TryParse(args[1], out volume);
                }



                SFXInfo temp = GameProcessor.gcDB.gameSFXs.Find(sfx => sfx.sfxID == sfxID);
                Microsoft.Xna.Framework.Audio.SoundEffectInstance tempInstance = null;
                if (temp != null && temp.sfx != null)
                {
                    tempInstance = temp.sfx.CreateInstance();
                }

                if (bHasVolume)
                {
                    tempInstance.Volume = ((float)volume) / 100f;
                }

                if (tempInstance != null)
                {
                    tempInstance.Play();
                }

                script.scriptLineIndex++;
            }
            catch (Exception)
            {
            }
        }

        private static void HandleTeleportTo(string commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();
            //@CWB_FADE_TIME
            try
            {
                var startPosition = args[0].Split(',').Select(Int32.Parse).ToList();
                int rotationAtEnd = -1;
                bool bHasRotation = int.TryParse(args[1], out rotationAtEnd);

                (PlayerController.selectedSprite as BaseCharacter).changePosition(new Vector2(startPosition[0] * 64, startPosition[1] * 64));
                (PlayerController.selectedSprite as BaseCharacter).UpdatePosition();
                if (bHasRotation)
                {
                    (PlayerController.selectedSprite as BaseCharacter).rotationIndex = rotationAtEnd;
                    NonCombatCtrl.ri = rotationAtEnd;
                }

                script.scriptLineIndex++;
            }
            catch (Exception)
            {
            }
        }

        private static void HandleChangeWorldBrightness(string commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();
            //@CWB_FADE_TIME
            try
            {
                int fadeTo = int.Parse(args[0]);
                float fadeTime = float.Parse(args[1]);

                GameScreenEffect.GenerateOpacitySteps(fadeTo, fadeTime);
                script.scriptLineIndex++;
                // ProcessCurrentScriptLine();
            }
            catch (Exception)
            {
            }
        }

        private static void HandleChangeOpacitySprite(string commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();
            //@COS_TRUE/FALSE_ID_FADE_TIME
            try
            {
                bool args1 = bool.Parse(args[0]);
                int tempID = int.Parse(args[1]);
                int fadeTo = int.Parse(args[2]);
                float fadeTime = float.Parse(args[3]);
                bool bUpdateDirectlyAfter = false;
                bool handleNextLine = false;
                if (args.Count > 3)
                {

                    handleNextLine = bool.TryParse(args[4], out bUpdateDirectlyAfter);
                }


                if (args1)
                {
                    GameProcessor.loadedMap.mapSprites.Find(s => s.objectIDAddedOnMap == tempID).GenerateOpacitySteps(fadeTo, fadeTime);
                }
                else
                {
                    foreach (var item in scriptTempObjects)
                    {
                        if ((item.GetType() == typeof(BaseSprite) || item.GetType() == typeof(BaseCharacter)) && (item as BaseSprite).shapeID == tempID)
                        {
                            (item as BaseSprite).GenerateOpacitySteps(fadeTo, fadeTime);
                        }

                        if ((item.GetType() == typeof(NPC)) && (item as NPC).identifier == tempID)
                        {
                            (item as NPC).baseCharacter.GenerateOpacitySteps(fadeTo, fadeTime);
                        }
                    }

                }

                if (bUpdateDirectlyAfter)
                {
                    if (handleNextLine)
                    {
                        script.scriptLineIndex++;
                        ProcessCurrentScriptLine();
                    }
                }
                else
                {
                    script.scriptLineIndex++;
                }
            }
            catch (Exception)
            {
            }

        }

        private static void HandleTagCameraCentre(string commandStringAddition)
        {
            bool bUseExistingChar = bool.TryParse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("_")), out bUseExistingChar);
            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);

            if (!bUseExistingChar)
            {
                String temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1, commandStringAddition.IndexOf("_"));
                int tempCharID = (int.Parse(temp));

                float cs = 3;
                float zoom = 0f;

                try
                {
                    commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
                    temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1, commandStringAddition.IndexOf("_"));
                    cs = (float.Parse(temp));
                }
                catch (Exception)
                {

                }

                try
                {
                    commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
                    temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
                    zoom = (float.Parse(temp));
                }
                catch (Exception)
                {

                }

                BaseSprite bc = PlayerController.selectedSprite;
                if (typeof(NPC) == scriptTempObjects[tempCharID].GetType())
                {
                    bc = ((NPC)scriptTempObjects[tempCharID]).baseCharacter;
                }
                if (bc == null)
                {
                    bc = PlayerController.selectedSprite;
                }


                GameProcessor.GenerateCamera(bc, cs, zoom);
            }
            else
            {
                String temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1, commandStringAddition.IndexOf("_"));
                int existingCharID = (int.Parse(temp));

                float cs = 3;
                float zoom = 0f;

                try
                {
                    commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
                    temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1, commandStringAddition.IndexOf("_"));
                    cs = (float.Parse(temp));
                }
                catch (Exception)
                {

                }

                try
                {
                    commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
                    temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
                    zoom = (float.Parse(temp));
                }
                catch (Exception)
                {

                }

                BaseSprite bc = GameProcessor.loadedMap.mapSprites.Find(s => s.objectIDAddedOnMap == existingCharID);
                if (bc == null)
                {
                    bc = PlayerController.selectedSprite;
                }



                GameProcessor.GenerateCamera(bc, cs, zoom);
            }


            //mover.shapeID = tempCharID;


        }

        private static void HandleTagCameraReset(string commandStringAddition)
        {
            GameProcessor.ResetCamera();
        }

        private static void HandleTagMoveCamera(string commandStringAddition) //@TMC-IDMover
        {

        }

        private static void HandleTagGoto(string commandStringAddition)
        {


            try
            {
                var args = commandStringAddition.Split('_').ToList();
                int THEid = int.Parse(args[0]);

                var temp = script.scriptContent.Find(l => script.scriptContent.IndexOf(l) > script.scriptLineIndex && l.StartsWith("@THE_" + THEid, StringComparison.InvariantCultureIgnoreCase));
                if (temp != null)
                {
                    script.scriptLineIndex = script.scriptContent.IndexOf(temp);
                }
                else if (script.scriptContent.Find(l => l.StartsWith("@THE", StringComparison.InvariantCultureIgnoreCase)) != null)
                {
                    Console.WriteLine("SOFT ERROR: @THE tag found, but no matching identifier, skipping to next line");
                    script.scriptLineIndex = script.scriptContent.IndexOf(script.scriptContent.Find(l => l.StartsWith("@THE", StringComparison.InvariantCultureIgnoreCase)));
                }
                else
                {
                    Console.WriteLine("ERROR: no @THE tag found, skipping to next line");
                    script.scriptLineIndex++;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: error processing @TGO tag");
                script.scriptLineIndex++;
            }

        }

        private static void HandleTagRandomEvent(string commandStringAddition)
        {
            try
            {
                commandStringAddition = commandStringAddition.Replace("-", "_");
                var args = commandStringAddition.Split('_').ToList();


                int percentage = (int.Parse(args[0])); //percentage
                int failSafe = (int.Parse(args[1])); //if failed try jump where to?

                bool bSucceeds = false;
                int nextline = script.scriptLineIndex + 1;
                if (GamePlayUtility.Randomize(0, 100) <= percentage)
                {
                    bSucceeds = true;
                    Console.WriteLine("Succesful random event chance.");
                    script.scriptLineIndex++;
                }
                else
                {
                    int tempIndex = script.scriptLineIndex + 1;
                    String line = script.scriptContent.Find(l => script.scriptContent.IndexOf(l) >= tempIndex && l.StartsWith("@THE_" + failSafe, StringComparison.OrdinalIgnoreCase));
                    if (line == default(String))
                    {
                        Console.WriteLine("NO @THE tag found as @THE_" + failSafe + " at: " + script.scriptLineIndex);
                        line = script.scriptContent.Find(l => script.scriptContent.IndexOf(l) >= tempIndex && l.StartsWith("@THE", StringComparison.OrdinalIgnoreCase));
                        if (line != default(String))
                        {
                            script.scriptLineIndex = script.scriptContent.FindAll(l => script.scriptContent.IndexOf(l) >= tempIndex).IndexOf(line);
                        }
                        else
                        {
                            Console.WriteLine("NO @THE tag found for @TRF at: " + script.scriptLineIndex);
                        }
                    }
                    else
                    {
                        script.scriptLineIndex = script.scriptContent.FindAll(l => script.scriptContent.IndexOf(l) >= tempIndex).IndexOf(line);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: error processing @TRF tag");
                script.scriptLineIndex++;
            }

        }

        private static void HandleTagBeforeBattle(string commandStringAddition)
        {
            int charMoverID = (int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("_"))));
            BaseCharacter mover = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == charMoverID).Clone();

            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            String temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1, commandStringAddition.IndexOf("_"));
            int charRotationAtEndT = (int.Parse(temp));
            mover.rotationIndex = charRotationAtEndT;
            charRotationAtEnd = charRotationAtEndT;

            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1, commandStringAddition.IndexOf("_"));
            int tempCharID = (int.Parse(temp));
            //mover.shapeID = tempCharID;

            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            temp = commandStringAddition.Substring(0, commandStringAddition.IndexOf("_"));
            var startPosition = temp.Split(',').Select(Int32.Parse).ToList();
            mover.changePosition(new Vector2(startPosition[0] * 64, startPosition[1] * 64));

            NPC moverNPC = new NPC();
            moverNPC.NPCArea.Add(GameProcessor.loadedMap.tilesOnPosition(new Vector2(startPosition[0], startPosition[1]), 0)[0]);
            moverNPC.baseCharacter = mover;

            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            var path = temp.Split(',').Select(Int32.Parse).ToList();

            for (int i = 0; i < path.Count; i += 2)
            {
                moverNPC.NPCArea.Add(GameProcessor.loadedMap.tilesOnPosition(new Vector2(path[i], path[i + 1]), 0)[0]);
            }

            moverNPC.identifier = tempCharID;
            moverNPC.NPCMovesAndEnds = true;
            moverNPC.rotationAtEnd = charRotationAtEnd;
            scriptTempObjects.Add(moverNPC);
            currentDisplayMode = (int)ActiveScriptDisplayMode.None;
            bStopUpdatingScriptLine = true;
        }

        private static void HandleTagAfterBattle(string commandStringAddition)
        {
            int charMoverID = (int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("_"))));
            BaseCharacter mover = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == charMoverID).Clone();

            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            String temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1, commandStringAddition.IndexOf("_"));
            int charRotationAtEndT = (int.Parse(temp));
            mover.rotationIndex = charRotationAtEndT;
            charRotationAtEnd = charRotationAtEndT;

            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1, commandStringAddition.IndexOf("_"));
            int tempCharID = (int.Parse(temp));
            //mover.shapeID = tempCharID;

            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            temp = commandStringAddition.Substring(0, commandStringAddition.IndexOf("_"));
            var startPosition = temp.Split(',').Select(Int32.Parse).ToList();
            mover.changePosition(new Vector2(startPosition[0] * 64, startPosition[1] * 64));

            NPC moverNPC = new NPC();
            moverNPC.NPCArea.Add(GameProcessor.loadedMap.tilesOnPosition(new Vector2(startPosition[0], startPosition[1]), 0)[0]);
            moverNPC.baseCharacter = mover;

            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            var path = temp.Split(',').Select(Int32.Parse).ToList();

            for (int i = 0; i < path.Count; i += 2)
            {
                moverNPC.NPCArea.Add(GameProcessor.loadedMap.tilesOnPosition(new Vector2(path[i], path[i + 1]), 0)[0]);
            }

            moverNPC.identifier = tempCharID;
            moverNPC.NPCMovesAndEnds = true;
            moverNPC.rotationAtEnd = charRotationAtEnd;
            scriptTempObjects.Add(moverNPC);
            currentDisplayMode = (int)ActiveScriptDisplayMode.None;
            bStopUpdatingScriptLine = true;
        }

        static int charRotationAtEnd = (int)BaseCharacter.Rotation.Up;
        private static void CreateTempNpc(string commandStringAddition)
        {

            var args = commandStringAddition.Split('_').ToList();
            //@PMW_true_3_0_0_8,15_8,16,8,17,8,18,8,19_0_true
            try
            {
                bool bCharExists = bool.Parse(args[0]);
                int charMoverID = int.Parse(args[1]);
                int charRotationAtEndT = int.Parse(args[2]);
                int tempCharID = int.Parse(args[3]);
                var startPosition = args[4].Split(',').Select(Int32.Parse).ToList();
                var path = args[5].Split(',').Select(Int32.Parse).ToList();
                int charFade = 100;
                bool bHasFade = false;
                if (args.Count > 6)
                {
                    bHasFade = int.TryParse(args[6], out charFade);
                }
                bool bUpdateDirectlyAfter = false;
                bool handleNextLine = false;
                if (args.Count > 7)
                {

                    handleNextLine = bool.TryParse(args[7], out bUpdateDirectlyAfter);
                }


                //String conversation = args[5];

                BaseCharacter mover = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == charMoverID).Clone();
                mover.rotationIndex = charRotationAtEndT;
                charRotationAtEnd = charRotationAtEndT;
                mover.shapeID = tempCharID;
                mover.changePosition(new Vector2(startPosition[0] * 64, startPosition[1] * 64));

                NPC moverNPC = new NPC();
                moverNPC.NPCArea.Add(GameProcessor.loadedMap.tilesOnPosition(new Vector2(startPosition[0], startPosition[1]), 0)[0]);
                moverNPC.baseCharacter = mover;
                moverNPC.identifier = tempCharID;
                moverNPC.ignoresMc = true;
                if (path.Count != 2)
                {
                    for (int i = 0; i < path.Count; i += 2)
                    {
                        moverNPC.NPCArea.Add(GameProcessor.loadedMap.tilesOnPosition(new Vector2(path[i], path[i + 1]), 0)[0]);
                    }
                }
                else
                {
                    var tempV = new Vector2(path[0], path[0 + 1]);
                    if (tempV != new Vector2(startPosition[0], startPosition[1]))
                    {
                        moverNPC.NPCArea.Add(GameProcessor.loadedMap.tilesOnPosition(new Vector2(path[0], path[0 + 1]), 0)[0]);
                    }
                }


                moverNPC.NPCMovesAndEnds = true;
                moverNPC.rotationAtEnd = charRotationAtEnd;
                scriptTempObjects.Add(moverNPC);
                currentDisplayMode = (int)ActiveScriptDisplayMode.None;
                bStopUpdatingScriptLine = true;

                if (bHasFade)
                {
                    mover.spriteOpacity = charFade;
                }

                if (bUpdateDirectlyAfter)
                {
                    if (handleNextLine)
                    {
                        script.scriptLineIndex++;
                        ProcessCurrentScriptLine();
                    }
                }
                else
                {
                    script.scriptLineIndex++;
                }


            }
            catch (Exception)
            {
            }
            #region
            //int charMoverID = (int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("-"))));


            //commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1);
            //String temp = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1, commandStringAddition.IndexOf("-"));
            //int charRotationAtEndT = (int.Parse(temp));


            // commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1);
            //temp = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1, commandStringAddition.IndexOf("-"));
            //int tempCharID = (int.Parse(temp));
            //mover.shapeID = tempCharID;

            // commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1);
            //temp = commandStringAddition.Substring(0, commandStringAddition.IndexOf("_"));
            //var startPosition = temp.Split(',').Select(Int32.Parse).ToList();




            //commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            //temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            //var path = temp.Split(',').Select(Int32.Parse).ToList();

            #endregion
        }

        #region
        static BaseCharacter speaker = null;
        static BaseCharacter charLeft = null;
        static BaseCharacter charRight = null;
        static int charLeftExpression = 0;
        static int charRightExpression = 0;
        static String conversation = "";
        static Texture2D speechBubbleSource = Game1.contentManager.Load<Texture2D>(@"Design\Speech");
        static int charLeftIDAssigned = -1; static int charRightIDAssigned = -1; static int charSpeakerIDAssigned = -1;
        static Microsoft.Xna.Framework.Audio.SoundEffectInstance lastInstance;
        #endregion
        private static void DisplayConversation(string commandStringAddition)
        {

            var args = commandStringAddition.Split('_').ToList();
            //@MSG_SpeakerID_charLeftID_charRightID_charLeft_Exp_CharRightExp_conv
            try
            {
                int charSpeakerID = int.Parse(args[0]);
                int charLeftID = int.Parse(args[1]);
                int charRightID = int.Parse(args[2]);
                int charLeftExpression = int.Parse(args[3]);
                int charRightExpression = int.Parse(args[4]);
                int conversationID = int.Parse(args[5]);

                GameText gt = GameProcessor.gcDB.gameTextCollection.Find(t => t.textID == conversationID);
                String conversation = gt.getText();

                if (charLeftID == charRightID)
                {
                    charRightID = -1;
                }

                if (charLeftIDAssigned != -1)
                {
                    charLeftID = charLeftIDAssigned;
                }

                if (charRightIDAssigned != -1)
                {
                    charRightID = charRightIDAssigned;
                }

                if (charSpeakerIDAssigned != -1)
                {
                    charSpeakerID = charSpeakerIDAssigned;
                }

                speaker = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == charSpeakerID);
                charLeft = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == charLeftID);
                charRight = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == charRightID);

                BaseCharacter.PortraitExpressions speakerExpression = BaseCharacter.PortraitExpressions.Neutral;
                if (speaker == charLeft)
                {
                    speakerExpression = (BaseCharacter.PortraitExpressions)charLeftExpression;
                }
                else if (speaker == charRight)
                {
                    speakerExpression = (BaseCharacter.PortraitExpressions)charRightExpression;
                }

                ScriptProcessorCtrl.Start();
                currentDisplayMode = (int)ActiveScriptDisplayMode.Conversation;
                bStopUpdatingScriptLine = true;

                if (GameScreenEffect.currentEffect != GameScreenEffect.Effects.Conversation)
                {
                    GameScreenEffect.InitializeConversationEffect();
                }
                try
                {
                    bCompletedProcessingLine = false;
                    GameProcessor.bGameUpdateIsPaused = true;
                    // completeScriptLineConversation = conversation;
                    ProcessText(conversation);
                    //bIsRunning = false;
                }
                catch
                {
                    Console.WriteLine("Error found trying to play the sound effect");
                }

                try
                {
                    if (lastInstance != null)
                    {
                        lastInstance.Stop();
                    }
                    lastInstance = speaker.PlayRandomConvExpression(speakerExpression);
                }
                catch (Exception)
                {

                }
            }
            catch (Exception)
            {
            }
            #region
            // int charSpeakerID = (int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("-")))); //submapID
            // String temp = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1, commandStringAddition.IndexOf("-"));
            //int charLeftID = (int.Parse(temp)); //shapeID

            //commandStringAddition = commandStringAddition.Replace(temp, "");
            //  commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1);
            // temp = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1, commandStringAddition.IndexOf("-"));
            // int charRightID = (int.Parse(temp));

            //commandStringAddition = commandStringAddition.Replace(temp, "");
            //commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1);
            // temp = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1, commandStringAddition.IndexOf("-"));
            //charLeftExpression = (int.Parse(temp));

            //commandStringAddition = commandStringAddition.Replace(temp, "");
            // commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1);
            //temp = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1, commandStringAddition.IndexOf("-"));
            // charRightExpression = (int.Parse(temp));

            //commandStringAddition = commandStringAddition.Replace(temp, "");
            // commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1);
            //temp = commandStringAddition.Substring(commandStringAddition.IndexOf("-") + 1);
            //conversation = temp;
            #endregion
        }

        private static void ChangeSubMapLogic(string commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();

            int subMapID = int.Parse(args[0]);
            int shapeMapID = int.Parse(args[1]);


            try
            {
                List<BasicMap> maps = new List<BasicMap>(GameProcessor.parentMap.subMaps);
                maps.Add(GameProcessor.parentMap);
                BasicMap SM = maps.Find(sm => sm.identifier == subMapID);
                var obj = SM.mapSprites.Find(s => s.objectIDAddedOnMap == shapeMapID);
                obj.bActivateAgainOncePlayerGone = true;
                obj.bActivateOnTouch = false;
                GameProcessor.loadedMap = SM;
                PlayerController.selectedSprite.position = obj.position;
                PlayerController.selectedSprite.UpdatePosition();

                GameProcessor.sceneCamera = new Vector2(-(PlayerController.selectedSprite.position.X + PlayerController.selectedSprite.spriteGameSize.Width / 2 - 1366 / 2 / GameProcessor.zoom), -(PlayerController.selectedSprite.position.Y + PlayerController.selectedSprite.spriteGameSize.Height / 2 - 768 / 2 / GameProcessor.zoom));
                GameProcessor.GenerateCamera(PlayerController.selectedSprite, .06f, GameProcessor.zoom);
                GameProcessor.ChangeSubMap(SM, obj);
                Console.WriteLine("Moved");
                bCompletedProcessingLine = true;
                GameProcessor.bGameUpdateIsPaused = false;
                bIsRunning = false;
                script.scriptLineIndex++;
                ProcessCurrentScriptLine();
            }
            catch
            {
                Console.WriteLine("Error found trying to play the sound effect");
            }
        }

        private static void PlayAnimationEndWithLogic(string commandStringAddition)
        {
            int shapeID = (int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("_"))));
            String temp = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1, commandStringAddition.IndexOf("_") - 1);
            int animID = (int.Parse(temp));
            commandStringAddition = commandStringAddition.Replace(temp, "");
            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 2);
            int endAnimID = (int.Parse(commandStringAddition));

            try
            {
                BaseSprite bs = GameProcessor.loadedMap.mapSprites.Find(x => x.shapeID == shapeID);
                bs.animationIndex = animID;
                bs.baseAnimations[animID].bMustEndAnimation = true;
                bs.baseAnimations[animID].animationEnder = endAnimID;
                bs.baseAnimations[endAnimID].bMustEndAnimation = false;
            }
            catch
            {
                Console.WriteLine("Error found trying to play the sound effect");
            }
            //script.scriptLineIndex++;

        }

        private static void PlayAnimationLogic(string commandStringAddition)
        {
            int shapeID = (int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("_"))));
            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            int animID = (int.Parse(commandStringAddition));

            try
            {
                BaseSprite bs = GameProcessor.loadedMap.mapSprites.Find(x => x.shapeID == shapeID);
                bs.animationIndex = animID;
                bs.baseAnimations[animID].bMustEndAnimation = true;
            }
            catch
            {
                Console.WriteLine("Error found trying to play the sound effect");
            }
            // script.scriptLineIndex++;
        }

        private static void PlaySoundEffectLogic(string commandStringAddition)
        {
            int soundPoolID = (int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("_"))));
            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            int SFXID = (int.Parse(commandStringAddition));

            try
            {
                GameProcessor.loadedMap.soundPools.Find(x => x.songCollectionID == soundPoolID).SEPlay(SFXID);
            }
            catch
            {
                Console.WriteLine("Error found trying to play the sound effect");
            }
            // script.scriptLineIndex++;
        }

        static void HandleChoiceTag(String commandStringAddition)
        {
            choiceText.Clear();
            choices.Clear();
            choicesTextPos.Clear();
            choiceTexPanels.Clear();

            // commandStringAddition = script.scriptContent[script.scriptLineIndex].Substring(5);
            String boolIDString = commandStringAddition;

            int boolID = int.Parse(boolIDString);
            currentDisplayMode = (int)ActiveScriptDisplayMode.Choice;
            ScriptBool infoBool = PlayerSaveData.getBool(boolID);
            changingBool = infoBool;
            int choicesAvailable = infoBool.choiceText.Count;

            Rectangle r = new Rectangle((1366 - 1200) / 2, 84, 1366 - 166, 768 - 166);
            int choiceOffSet = 50;
            int firstBoxOffset = 80;
            Point p = new Point(600, 120);

            int index = 0;
            TextUtility.ClearCache();
            foreach (var item in infoBool.choiceDescription)
            {
                //var pairText = TextUtility.GenerateBestStringAndRectangle(item, textWindowFont, widthChoiceText - choiceTextOffSetX);
                //String textChoice = pairText.Key;
                //if (infoBool.choiceDescription.IndexOf(item) == 0)
                //{
                //    choices.Add(new Rectangle((int)startChoicePos.X - choiceTextOffSetX, (int)startChoicePos.Y, widthChoiceText + choiceTextOffSetX * 2, pairText.Value + choiceTextOffSetY * 2));
                //    choicesTextPos.Add(new Vector2(choices[choices.Count - 1].X + choiceTextOffSetX, choices[choices.Count - 1].Y + choiceTextOffSetY));

                //}
                //else
                //{
                //    choices.Add(new Rectangle(choices[choices.Count - 1].X, choices[choices.Count - 1].Y + choices[choices.Count - 1].Height + heightBetweenChoiceText, choices[choices.Count - 1].Width, pairText.Value + choiceTextOffSetY * 2));
                //    choicesTextPos.Add(new Vector2(choicesTextPos[choicesTextPos.Count - 1].X, choices[choices.Count - 2].Y + choices[choices.Count - 2].Height + heightBetweenChoiceText + choiceTextOffSetY));
                //}
                //choiceText.Add(textChoice);
                choices.Add(new Rectangle((1366 - p.X) / 2, firstBoxOffset + choiceOffSet * (index + 1) + p.Y * index, p.X, p.Y));
                choicesTextPos.Add(choices.Last().Location.ToVector2());
                choiceText.Add(item);
                choiceTexPanels.Add(choiceTexPanelSource.positionCopy(choices.Last()));
                index++;
            }

            ScriptProcessorCtrl.Start();
            bCompletedProcessingLine = true;
        }

        public static void UpdateExecuteScript(GameTime gt, BaseSprite s)
        {

            if (backgroundScripts.Contains(s.script) && !s.script.bReachedEnd)
            {

                if (s.script.scriptContent.Count < s.script.scriptLineIndex)
                {
                    Console.WriteLine("Script stuck in a loop, no proper end!" + s);
                    s.script.scriptLineIndex = 1;
                    if (s.script.scriptContent.Count == 1)
                    {
                        s.script.scriptLineIndex = 0;
                    }
                }


                BaseScript bgs = s.script;
                bgs.Update(gt);

                String commandString = "";
                String commandStringAddition = "";



                try
                {
                    while (!bgs.scriptContent[bgs.scriptLineIndex].StartsWith("@"))
                    {
                        if (bgs.scriptLineIndex + 1 != bgs.scriptContent.Count)
                        {
                            bgs.scriptLineIndex++;
                        }

                    }

                    commandString = bgs.scriptContent[bgs.scriptLineIndex].Substring(0, 4);
                    if (bgs.scriptContent[bgs.scriptLineIndex].Length > 4)
                    {
                        commandStringAddition = bgs.scriptContent[bgs.scriptLineIndex].Substring(5);
                    }

                }
                catch
                {

                }


                if (commandString.Equals("@TRE"))
                {
                    HandleRepeatTag(s.script.scriptContent[s.script.scriptLineIndex].Substring(5));
                }
                else if (commandString.Equals("@TIV"))
                {
                    int value = (int.Parse(commandStringAddition));
                    s.spriteOpacity = value;
                    if (value == 0)
                    {
                        s.bIsVisible = false;
                    }
                    else
                    {
                        s.bIsVisible = true;
                    }
                }
                else if (commandString.Equals("@TIS"))
                {
                    int searchID = (int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("_"))));
                    commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
                    int value = (int.Parse(commandStringAddition));
                    BaseSprite toChangeSprite = GameProcessor.loadedMap.mapSprites.Find(x => x.shapeID == searchID);

                    toChangeSprite.spriteOpacity = value;
                    if (value == 0)
                    {
                        toChangeSprite.bIsVisible = false;
                    }
                    else
                    {
                        toChangeSprite.bIsVisible = true;
                    }
                    //    GameProcessor.loadedMap.mapSprites[(int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("_") - 1)))].spriteOpacity = (int.Parse(commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1)));
                }
                else if (commandString.Equals("@ESE"))
                {
                    s.script.bReachedEnd = true;
                    //    GameProcessor.loadedMap.mapSprites[(int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("_") - 1)))].spriteOpacity = (int.Parse(commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1)));
                }
                else if (commandString.StartsWith("@TSM"))
                {
                    ChangeSubMapLogic(s.script.scriptContent[s.script.scriptLineIndex].Substring(5));
                }

                s.script.scriptLineIndex++;
            }


        }

        private static void HandleStartPointCommand(int v, BaseSprite s)
        {
            if (PlayerSaveData.heroParty.Count == 0)
            {
                mapPartyIndex = v;
                foreach (var team in gameMap.mapTeams)
                {
                    if (team.teamIdentifier == mapPartyIndex)
                    {
                        List<BaseCharacter> temp = new List<BaseCharacter>();
                        foreach (var item in team.teamMembers)
                        {
                            temp.Add(gameMap.gcdb.gameCharacters.Find(c => c.shapeID == item).Clone());
                        }
                        PlayerSaveData.heroParty = temp;
                        mapHero = temp[0];
                    }
                }

                mapHero.position = s.position;

            }
            script.scriptLineIndex++;
            ProcessCurrentScriptLine();
        }

        private static void HandleRepeatTag(string commandStringAddition)
        {
            try
            {
                var args = commandStringAddition.Split('_').ToList();
                int THEid = int.Parse(args[0]);

                // var temp = script.scriptContent.Find(l => script.scriptContent.IndexOf(l) > script.scriptLineIndex && l.StartsWith("@THE_" + THEid, StringComparison.InvariantCultureIgnoreCase));
                var temp = script.scriptContent.Find(l => l.StartsWith("@THE_" + THEid, StringComparison.InvariantCultureIgnoreCase));
                if (temp != null)
                {
                    script.repeatLine = script.scriptContent.IndexOf(temp);
                }
                else if (script.scriptContent.Find(l => l.StartsWith("@THE", StringComparison.InvariantCultureIgnoreCase)) != null)
                {
                    Console.WriteLine("SOFT ERROR: @THE tag found, but no matching identifier, skipping to next line");
                    script.scriptLineIndex = script.scriptContent.IndexOf(script.scriptContent.Find(l => l.StartsWith("@THE", StringComparison.InvariantCultureIgnoreCase)));
                }
                else
                {
                    Console.WriteLine("ERROR: no @THE tag found, skipping to next line");
                    script.scriptLineIndex++;
                }

                script.scriptLineIndex++;
                ProcessCurrentScriptLine();
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: error processing @TGO tag");
                script.scriptLineIndex++;
            }
        }

        internal enum TIFTypes { Bool, PlayerFacing, Time }

        private static void HandleIfCommand(String commandStringAddition)
        {
            var args = commandStringAddition.Split('_').ToList();
            args.RemoveAll(l => l.Equals("", StringComparison.OrdinalIgnoreCase));

            // DayLightHandler.TimeBlocksNames timeTest = DayLightHandler.TimeBlocksNames.am12am13;
            // bool isTimeSlot = Enum.TryParse<DayLightHandler.TimeBlocksNames>(args[0], out timeTest);
            TIFTypes tifType = (TIFTypes)Enum.GetNames(typeof(TIFTypes)).ToList().IndexOf(args.Last());


            switch (tifType)
            {
                case TIFTypes.Bool:
                    #region

                    ScriptBoolCheck check = args[0];
                    int THEid = (int.Parse(args[1]));


                    // ScriptBool scriptBool = PlayerSaveData.getBool(scriptID);

                    if (check != null && check.CanBeActivated())
                    {
                        Console.WriteLine("IF correctly handled");
                        script.scriptLineIndex++;
                        ProcessCurrentScriptLine();
                    }
                    else
                    {
                        var temp = script.scriptContent.Find(l => script.scriptContent.IndexOf(l) > script.scriptLineIndex && l.StartsWith("@THE_" + THEid, StringComparison.InvariantCultureIgnoreCase));
                        if (temp != null)
                        {
                            script.scriptLineIndex = script.scriptContent.IndexOf(temp);
                        }
                        else if (script.scriptContent.Find(l => l.StartsWith("@THE", StringComparison.InvariantCultureIgnoreCase)) != null)
                        {
                            Console.WriteLine("SOFT ERROR: @THE tag found, but no matching identifier, skipping to next line");
                            script.scriptLineIndex = script.scriptContent.IndexOf(script.scriptContent.Find(l => l.StartsWith("@THE", StringComparison.InvariantCultureIgnoreCase)));
                        }
                        else
                        {
                            Console.WriteLine("ERROR: no @THE tag found, skipping to next line");
                            script.scriptLineIndex++;
                        }
                    }
                    #endregion
                    break;
                case TIFTypes.PlayerFacing:
                    #region
                    int rotation = (int.Parse(args[0]));
                    THEid = (int.Parse(args[1]));

                    if ((PlayerController.selectedSprite as BaseCharacter).rotationIndex == rotation)
                    {
                        Console.WriteLine("IF correctly handled, correct rotation");
                        script.scriptLineIndex++;
                        //ProcessCurrentScriptLine();
                    }
                    else
                    {
                        var temp = script.scriptContent.Find(l => script.scriptContent.IndexOf(l) > script.scriptLineIndex && l.StartsWith("@THE_" + THEid, StringComparison.InvariantCultureIgnoreCase));
                        if (temp != null)
                        {
                            script.scriptLineIndex = script.scriptContent.IndexOf(temp);
                        }
                        else if (script.scriptContent.Find(l => l.StartsWith("@THE", StringComparison.InvariantCultureIgnoreCase)) != null)
                        {
                            Console.WriteLine("SOFT ERROR: @THE tag found, but no matching identifier, skipping to next line");
                            script.scriptLineIndex = script.scriptContent.IndexOf(script.scriptContent.Find(l => l.StartsWith("@THE", StringComparison.InvariantCultureIgnoreCase)));
                        }
                        else
                        {
                            Console.WriteLine("ERROR: no @THE tag found, skipping to next line");
                            script.scriptLineIndex++;
                        }
                    }
                    #endregion
                    break;
                case TIFTypes.Time:
                    int slot1 = (int)Enum.Parse(typeof(DayLightHandler.TimeBlocksNames), args[0]);
                    int slot2 = (int)Enum.Parse(typeof(DayLightHandler.TimeBlocksNames), args[1]);
                    THEid = (int.Parse(args[2]));

                    int daySlot = (int)DayLightHandler.currentTimeBlock.timeBlockName;
                    if (slot1 > slot2)
                    {
                        slot2 += Enum.GetNames(typeof(DayLightHandler.TimeBlocksNames)).Length;
                        //  daySlot += Enum.GetNames(typeof(DayLightHandler.TimeBlocksNames)).Length;
                    }
                    if (slot1 > daySlot)
                    {
                        //slot2 += Enum.GetNames(typeof(DayLightHandler.TimeBlocksNames)).Length;
                        daySlot += Enum.GetNames(typeof(DayLightHandler.TimeBlocksNames)).Length;
                    }
                    if (daySlot >= slot1 && daySlot < slot2)
                    {
                        Console.WriteLine("IF correctly handled, correct time");
                        script.scriptLineIndex++;
                        //  ProcessCurrentScriptLine();
                    }
                    else
                    {
                        var temp = script.scriptContent.Find(l => script.scriptContent.IndexOf(l) > script.scriptLineIndex && l.StartsWith("@THE_" + THEid, StringComparison.InvariantCultureIgnoreCase));
                        if (temp != null)
                        {
                            script.scriptLineIndex = script.scriptContent.IndexOf(temp);
                        }
                        else if (script.scriptContent.Find(l => l.StartsWith("@THE", StringComparison.InvariantCultureIgnoreCase)) != null)
                        {
                            Console.WriteLine("SOFT ERROR: @THE tag found, but no matching identifier, skipping to next line");
                            script.scriptLineIndex = script.scriptContent.IndexOf(script.scriptContent.Find(l => l.StartsWith("@THE", StringComparison.InvariantCultureIgnoreCase)));
                        }
                        else
                        {
                            Console.WriteLine("ERROR: no @THE tag found, skipping to next line");
                            script.scriptLineIndex++;
                        }
                    }

                    break;
                default:
                    break;


            }

        }

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
            if (letterTimePassed > letterTimer && script.scriptLineIndex < script.scriptContent.Count)
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

        public static void StopProcessor(int textLine = -1)
        {
            GameProcessor.loadedMap.RemoveTempObjects(scriptTempObjects);
            GameProcessor.ResetCamera();
            scriptTempObjects.Clear();
            Console.WriteLine("Ending processor");
            currentDisplayMode = (int)ActiveScriptDisplayMode.None;
            GameScreenEffect.Reset();
            letterTimePassed = 0;
            processedPieceTextString = "";
            bScriptRunning = false;
            bCompletedProcessingLine = false;
            if (textLine != -1)
            {
                script.scriptLineIndex = textLine;
            }
            int previousID = script.identifier;
            script = new BaseScript();
            script.identifier = previousID;
            bIsRunning = false;
            bStopUpdatingScriptLine = false;
            GameProcessor.bGameUpdateIsPaused = false;

        }

        public static bool HasPCController(BasicMap bm)
        {
            gameMap = bm;
            bool bFoundPCC = false;
            foreach (var sprite in bm.mapSprites)
            {
                if (sprite.script != null && sprite.script.scriptContent.Count != 0)
                {
                    if (sprite.script.scriptContent[0].StartsWith("@TPC"))
                    {
                        bFoundPCC = true;
                        ControllerIndex = bm.mapSprites.IndexOf(sprite);
                        break;
                    }

                    if (sprite.script.scriptContent[0].StartsWith("@TSP"))
                    {

                        bFoundPCC = true;
                        //script = sprite.script;
                        script.scriptLineIndex = 0;
                        ControllerIndex = bm.mapSprites.IndexOf(sprite);
                        //ProcessScriptCommand(sprite);
                        ChangeActiveScript(sprite.script, sprite, null);
                        break;
                    }
                }
            }

            if (!bFoundPCC)
            {
                ControllerIndex = -1;
                System.Windows.Forms.MessageBox.Show("Found no object marked with Player Control Tag '@TPC' as first line,\nno player found playtest will exit.");
            }

            return bFoundPCC;
        }

        public static bool HasPCController()
        {
            bool bFoundPCC = false;
            foreach (var sprite in gameMap.mapSprites)
            {
                if (sprite.script != null && sprite.script.scriptContent.Count != 0)
                {
                    if (sprite.script.scriptContent[0].StartsWith("@TPC"))
                    {
                        bFoundPCC = true;
                        ControllerIndex = gameMap.mapSprites.IndexOf(sprite);
                        break;
                    }

                    if (sprite.script.scriptContent[0].StartsWith("@TSP"))
                    {

                        bFoundPCC = true;
                        //script = sprite.script;
                        script.scriptLineIndex = 0;
                        ControllerIndex = gameMap.mapSprites.IndexOf(sprite);
                        ChangeActiveScript(sprite.script, sprite, null);
                        //ProcessScriptCommand(sprite);
                        break;
                    }
                }
            }

            if (!bFoundPCC)
            {
                ControllerIndex = -1;
                System.Windows.Forms.MessageBox.Show("Found no object marked with Player Control Tag '@TPC' as first line,\nno player found playtest will exit.");
            }

            return bFoundPCC;
        }

        public static BaseSprite getController()
        {
            if (ControllerIndex != -1)
            {
                return gameMap.mapSprites[ControllerIndex];
            }

            if (mapPartyIndex != -1)
            {

                return mapHero;
            }
            return null;
        }

        public static void Draw(SpriteBatch sb)
        {
            switch (currentDisplayMode)
            {
                case (int)ActiveScriptDisplayMode.Text:
                    if (!processedPieceTextString.Equals(""))
                    {
                        TextUtility.ClearCache();
                        normalTextPanel.Draw(sb, Color.White);
                        //sb.Draw(textWindowBG, textWindow, textWindowBG.Bounds, Color.White);
                        //sb.DrawString(textWindowFont, ScriptLineOnlyConversation, textWindowInputRoom.Location.ToVector2(), Color.Black);
                        TextUtility.DrawComplex(sb, ScriptLineOnlyConversation, textWindowFont, normalTextPanelTextPos, TextUtility.OutLining.Left, Color.White, 1f, false, SceneUtility.transform, Color.Gray);
                        //if (bCompletedProcessingLine)
                        //{
                        //    sb.DrawString(textWindowFont, "Press ENTER to continue", textWindowInputRoom.Location.ToVector2() + new Vector2(600, 700), Color.Black);
                        //}
                    }
                    break;

                case (int)ActiveScriptDisplayMode.Choice:

                    //Matrix m = Matrix.CreateTranslation(0, 0, 1) * Matrix.CreateScale(ResolutionUtility.stdScale.X, ResolutionUtility.stdScale.Y, 1);
                    //sb.End();
                    //sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, m);

                    if (selectedTexPanel == null)
                    {
                        int index = 0;
                        // TextUtility.ClearCache();
                        //ScriptProcessorCtrl.Start();
                        foreach (var choice in choices)
                        {
                            //   sb.Draw(textWindowBG, doubleChoiceBoxes[i], textWindowBG.Bounds, Color.White);
                            // sb.Draw(textWindowBG, choice, textWindowBG.Bounds, Color.White);
                            choiceTexPanels[index].Draw(sb, Color.White*.6f);
                            TextUtility.DrawComplex(sb, choiceText[index], textWindowFont, choice, TextUtility.OutLining.Center, Color.White * .6f, 1f, false, SceneUtility.transform, Color.DarkGray * .6f);
                            // sb.DrawString(textWindowFont, choiceText[choices.IndexOf(choice)], choicesTextPos[choices.IndexOf(choice)], Color.Black);
                            index++;
                        }
                    }
                    else if (selectedTexPanel != null)
                    {
                        int index = 0;
                        // TextUtility.ClearCache();
                        //ScriptProcessorCtrl.Start();
                        foreach (var choice in choices)
                        {
                            //   sb.Draw(textWindowBG, doubleChoiceBoxes[i], textWindowBG.Bounds, Color.White);
                            // sb.Draw(textWindowBG, choice, textWindowBG.Bounds, Color.White);
                            if (choiceTexPanels[index] != selectedTexPanel)
                            {
                                choiceTexPanels[index].Draw(sb, Color.White * .6f);
                                TextUtility.DrawComplex(sb, choiceText[index], textWindowFont, choice, TextUtility.OutLining.Center, Color.White * .6f, 1f, false, SceneUtility.transform, Color.DarkGray * .6f);
                                // sb.DrawString(textWindowFont, choiceText[choices.IndexOf(choice)], choicesTextPos[choices.IndexOf(choice)], Color.Black);
                                index++;
                            }
                            else
                            {
                                choiceTexPanels[index].Draw(sb, Color.White);
                                TextUtility.DrawComplex(sb, choiceText[index], textWindowFont, choice, TextUtility.OutLining.Center, Color.White, 1f, false, SceneUtility.transform, Color.DarkGray);
                                // sb.DrawString(textWindowFont, choiceText[choices.IndexOf(choice)], choicesTextPos[choices.IndexOf(choice)], Color.Black);
                                index++;
                            }
                        }
                    }
                    break;
                case (int)ActiveScriptDisplayMode.Conversation:
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
                        if (speaker == charLeft)
                        {
                            charLeft.portraitAnimations[charLeftExpression].Draw(sb, Lface);
                        }
                        else if (charLeft != null)
                        {
                            charLeft.portraitAnimations[charLeftExpression].Draw(sb, Lface, 0.7f);
                        }
                    }
                    catch
                    {
                        try
                        {
                            if (speaker == charLeft)
                            {
                                charLeft.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(sb, Lface);
                            }
                            else if (charLeft != null)
                            {
                                charLeft.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(sb, Lface, 0.7f);
                            }
                        }
                        finally
                        {


                        }
                    }


                    Rectangle Rface = new Rectangle(1266 - 300, 500 - 300, 300, 300);
                    try
                    {
                        if (speaker == charRight)
                        {
                            charRight.portraitAnimations[charRightExpression].Draw(sb, Rface, 1.0f, SpriteEffects.FlipHorizontally);
                        }
                        else if (charRight != null)
                        {
                            charRight.portraitAnimations[charRightExpression].Draw(sb, Rface, 0.7f, SpriteEffects.FlipHorizontally);
                        }

                    }
                    catch
                    {
                        try
                        {
                            if (speaker == charRight)
                            {
                                charRight.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(sb, Rface, 1.0f, SpriteEffects.FlipHorizontally);
                            }
                            else if (charRight != null)
                            {
                                charRight.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(sb, Rface, 0.7f, SpriteEffects.FlipHorizontally);
                            }
                        }
                        finally
                        {


                        }
                    }
                    String speakerName = "";
                    if (activatedFrom != null && activatedFrom.GetType() == typeof(NPC))
                    {
                        speakerName = (activatedFrom as NPC).npcDisplayName;
                    }
                    else
                    {
                        speakerName = speaker.displayName;
                    }
                    //conversation = "Hello, is it my you're looking for? I can see it in your eyes, I can see it in your smiiiiillleee. Is this enough text?";
                    sb.DrawString(textWindowFont, speakerName + ":", new Vector2(120, 510), Color.White);
                    sb.DrawString(textWindowFont, TextUtility.bestMatchStringForBox(ScriptLineOnlyConversation, textWindowFont, new Rectangle(120, 530, 1123 - 30, 190)), new Vector2(150, 580), Color.White);
                    //sb.DrawString(textWindowFont, TextUtility.bestMatchStringForBox(conversation, textWindowFont, new Rectangle(120, 530, 1123 - 30, 190)), new Vector2(150, 580), Color.White);
                    bCompletedProcessingLine = true;
                    break;
                case (int)ActiveScriptDisplayMode.None:

                    break;
            }

        }

        public static void FinalizeScript(BaseSprite bs)
        {
            String commandString = "";
            String commandStringAddition = "";
            int startLine = bs.script.repeatLine;
            int endLine = 0;

            foreach (var line in bs.script.scriptContent)
            {
                try
                {
                    commandString = line.Substring(0, 4);
                }
                catch
                {
                }

                if (commandString.Equals("@TES"))
                {
                    endLine = bs.script.scriptContent.IndexOf(line);
                }
            }

            for (int i = startLine; i < endLine; i++)
            {
                String line = bs.script.scriptContent[i];
                if (line.StartsWith("@"))
                {
                    try
                    {
                        commandString = line.Substring(0, 4);
                        commandStringAddition = line.Substring(4);
                    }
                    catch
                    {
                    }


                    if (commandString.Equals("@RAS"))
                    {
                        bs.script.scriptLineIndex = startLine;
                        bs.script.bReachedEnd = false;
                    }
                    else if (commandString.Equals("@TIV"))
                    {
                        commandStringAddition = line.Substring(5);
                        int value = (int.Parse(commandStringAddition));
                        bs.spriteOpacity = value;
                        if (value == 0)
                        {
                            bs.bIsVisible = false;
                        }
                        else
                        {
                            bs.bIsVisible = true;
                        }
                    }
                    else if (commandString.Equals("@TIS"))
                    {
                        ChangeVisibility(line.Substring(5));
                    }
                }
            }


            backgroundScripts.Remove(bs.script);
            if (bs.bIsVisible)
            {
                bs.spriteOpacity = 100;
            }
            else
            {
                bs.spriteOpacity = 0;
            }

        }

        static void ChangeVisibility(String commandStringAddition)
        {
            int searchID = (int.Parse(commandStringAddition.Substring(0, commandStringAddition.IndexOf("_"))));
            commandStringAddition = commandStringAddition.Substring(commandStringAddition.IndexOf("_") + 1);
            int value = (int.Parse(commandStringAddition));
            BaseSprite toChangeSprite = GameProcessor.loadedMap.mapSprites.Find(x => x.shapeID == searchID);

            toChangeSprite.spriteOpacity = value;
            if (value == 0)
            {
                toChangeSprite.bIsVisible = false;
                toChangeSprite.spriteOpacity = value;
            }
            else
            {
                toChangeSprite.bIsVisible = true;
                toChangeSprite.spriteOpacity = value;
            }
            script.scriptLineIndex++;
        }

        static private void ScriptProcessorLogic()
        {
            if (ScriptProcessor.scriptTempObjects.Count != 0 && ScriptProcessor.currentDisplayMode == (int)ScriptProcessor.ActiveScriptDisplayMode.None)
            {
                bool bStartSciptAgain = true;

                var temp = ScriptProcessor.scriptTempObjects.FindAll(obj => obj.GetType() == typeof(NPC)).Find(npc => !(npc as NPC).characterIsFinishedMoving);
                if (temp != null)
                {
                    bStartSciptAgain = false;
                }

                if (bStartSciptAgain)
                {
                    // HandleNextLine();
                    // script.scriptLineIndex++;
                    bCompletedProcessingLine = false;
                    bStopUpdatingScriptLine = false;
                    processedPieceTextString = "";
                    letterTimePassed = 0;
                }

            }
        }

        static public List<BaseSprite> returnTempObjsAsBaseSprites()
        {
            List<BaseSprite> temp = new List<BaseSprite>();

            foreach (var item in ScriptProcessor.scriptTempObjects)
            {
                if (item is NPC)
                {
                    temp.Add((item as NPC).baseCharacter);
                }

                if (item is BaseSprite)
                {
                    temp.Add((item as BaseSprite));
                }

                if (item is SpriteLight)
                {
                    temp.Add((item as SpriteLight) as BaseSprite);
                }
            }

            return temp;
        }

        static public BaseScript backgroundScriptFrom(BaseScript sc)
        {
            BaseScript s = sc.Clone();
            String begin = "@TBS";
            String end = "@TBE";
            if (s.scriptContent.Contains(begin) && s.scriptContent.Contains(end))
            {
                int lineStart = s.scriptContent.IndexOf(begin);
                int lineEnd = s.scriptContent.IndexOf(end);

                if (lineStart < lineEnd)
                {
                    s.scriptContent = new List<string>(s.scriptContent.GetRange(lineStart, lineEnd - lineStart));
                }
            }


            return s;

        }
    }
}
