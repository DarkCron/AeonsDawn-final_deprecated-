using TBAGW.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using TBAGW.Utilities.ReadWrite;

namespace TBAGW.Utilities.SriptProcessing
{
    static public class ScripTestingScene
    {
        #region FIELDS
        static Rectangle textWindow = new Rectangle(183, 234, 1000, 300);
        static Rectangle textWindowInputRoom = new Rectangle(233, 248, 900, 250);
        static Rectangle testScriptButton = new Rectangle(1200, 600, 150, 75);
        static Vector2 scriptInfoLoc = new Vector2(1200, 234);
        static Texture2D textWindowBG;
        static SpriteFont textWindowFont;

        static public String inputText = "";

        static bool bEnableInput = false;
        static bool bTestScript = false;

        static ScriptTestObject scriptObject;
        static int currentLineInScript = 0;

        #endregion
        static public void Start(Game game)
        {
            game.IsMouseVisible = true;

            textWindowBG = game.Content.Load<Texture2D>(@"Editor\Scripts\TextWindowBG");
            textWindowFont = game.Content.Load<SpriteFont>(@"Editor\Scripts\TextWindowFont");

            try
            {
                scriptObject = EditorFileWriter.ScriptObjectReader(Environment.CurrentDirectory + @"\Scripts\" + 1001 + ".cgScript");
                inputText = scriptObject.script.scriptContent[scriptObject.script.scriptContent.Count - 1];
                currentLineInScript = scriptObject.script.scriptContent.Count - 1;
            }
            catch (Exception)
            {
                Console.WriteLine("Error loading script");
                scriptObject = new ScriptTestObject(1001);
                scriptObject.script.scriptContent.Add(inputText);
                currentLineInScript = 0;
            }

        }

        static public void Update(GameTime gameTime, Game1 game)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                bEnableInput = true;
            }

            if (!game.IsMouseVisible)
            {
                game.IsMouseVisible = true;
            }

            #region testScriptButton
            if (!bEnableInput)
            {
                if (testScriptButton.Contains(Mouse.GetState().Position)&&!KeyboardMouseUtility.AnyButtonsPressed())
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        bTestScript = true;
                    }
                }
            }

            if (bTestScript)
            {
                Console.WriteLine("Attempting to execute script...");
                //ScriptProcessor.ChangeActiveScript(scriptObject.script);
                bTestScript = false;
            }

            #endregion

            #region inputHandler
            Keys lastPressedKey = Keys.None;

            if (Keyboard.GetState().GetPressedKeys().Length > 0 && Keyboard.GetState().GetPressedKeys().Length < 2)
            {
                lastPressedKey = Keyboard.GetState().GetPressedKeys()[0];
            }
            else if (Keyboard.GetState().GetPressedKeys().Length > 1 && Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                if (Keyboard.GetState().GetPressedKeys()[0] == Keys.LeftShift)
                {
                    lastPressedKey = Keyboard.GetState().GetPressedKeys()[1];
                }
                else if (Keyboard.GetState().GetPressedKeys()[1] == Keys.LeftShift)
                {
                    lastPressedKey = Keyboard.GetState().GetPressedKeys()[0];
                }
            }
            else if (Keyboard.GetState().GetPressedKeys().Length > 0)
            {
                lastPressedKey = Keyboard.GetState().GetPressedKeys()[0];
            }

            if (bEnableInput)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    if (lastPressedKey == Keys.OemPeriod && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, ".");
                    }

                    if (lastPressedKey == Keys.OemComma && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "?");
                    }

                    if (lastPressedKey == ((Keys)(226)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, ">");
                    }

                    if (lastPressedKey == ((Keys)(192)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "%");
                    }

                    if (lastPressedKey == ((Keys)(187)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "+");
                    }




                }
                else if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    if (lastPressedKey == (Keys.OemComma) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, ",");
                    }

                    if (lastPressedKey == ((Keys)49) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "&");
                    }

                    if (lastPressedKey == ((Keys)51) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "\"");
                    }

                    if (lastPressedKey == ((Keys)(52)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "'");
                    }

                    if (lastPressedKey == ((Keys)(53)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "(");
                    }

                    if (lastPressedKey == ((Keys)(219)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, ")");
                    }

                    if (lastPressedKey == ((Keys)(56)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "!");
                    }

                    if (lastPressedKey == ((Keys)(191)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, ":");
                    }

                    if (lastPressedKey == ((Keys)(226)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "<");
                    }

                    if (lastPressedKey == ((Keys)(189)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "-");
                    }

                    if (lastPressedKey == ((Keys)(187)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "=");
                    }

                    if (lastPressedKey == ((Keys)(192)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        inputText = inputText.Insert(inputText.Length, "@");
                    }

                }


                if (Keyboard.GetState().GetPressedKeys().Length >= 1)
                {
                    // Console.Out.WriteLine((int)Keyboard.GetState().GetPressedKeys()[0]);
                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                    {
                        //Numdpad keys are not enabled...yet?... They need manual reprogramming they're values 96-105 starting at 0-9
                        //From 47-57 0-9 normal keyboard
                        if (((int)Keyboard.GetState().GetPressedKeys()[0] > 57 && (int)Keyboard.GetState().GetPressedKeys()[0] < 91 || (int)Keyboard.GetState().GetPressedKeys()[0] == 32) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {

                            if (inputText.Length > 0)
                            {

                                inputText = inputText.Insert(inputText.Length, ((char)lastPressedKey).ToString().ToLower());
                            }
                            else
                            {
                                inputText = ((char)lastPressedKey).ToString().ToLower();
                            }

                        }
                        else if (inputText.Length > 0 && Keyboard.GetState().IsKeyDown(Keys.Back) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            if (inputText.Length >= 2)
                            {
                                if (inputText.Substring(inputText.Length - 2, 2).Equals("\n"))
                                {
                                    inputText = inputText.Remove(inputText.Length - 2);
                                }
                                else
                                {
                                    inputText = inputText.Remove(inputText.Length - 1);
                                }
                            }
                            else
                            {
                                inputText = inputText.Remove(inputText.Length - 1);
                            }

                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            inputText = inputText.Insert(inputText.Length, "\n");

                        }
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().GetPressedKeys().Length == 2)
                    {
                        //Numdpad keys are not enabled...yet?... They need manual reprogramming they're values 96-105 starting at 0-9
                        if (((int)Keyboard.GetState().GetPressedKeys()[0] > 47 && (int)Keyboard.GetState().GetPressedKeys()[0] < 91 || (int)Keyboard.GetState().GetPressedKeys()[0] == 32) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            if (inputText.Length > 0)
                            {
                                inputText = inputText.Insert(inputText.Length, ((char)lastPressedKey).ToString().ToUpper());
                            }
                            else
                            {
                                inputText = ((char)lastPressedKey).ToString().ToUpper();
                            }
                            // Console.Out.WriteLine((char)Keyboard.GetState().GetPressedKeys()[0]);
                        }
                        else if (inputText.Length > 0 && Keyboard.GetState().IsKeyDown(Keys.Back) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            if (inputText.Length >= 2)
                            {
                                if (inputText.Substring(inputText.Length - 2, 2).Equals("\n"))
                                {
                                    inputText = inputText.Remove(inputText.Length - 2);
                                }
                                else
                                {
                                    inputText = inputText.Remove(inputText.Length - 1);
                                }
                            }
                            else
                            {
                                inputText = inputText.Remove(inputText.Length - 1);
                            }
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            bEnableInput = false;
                            scriptObject.script.scriptContent[currentLineInScript] = inputText;
                        }
                    }
                }
            }
            #endregion
            #region Input outside typing
            else
            {

                //Re-enable editing
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    bEnableInput = true;
                }

                //Add new line downward
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    if (scriptObject.script.scriptContent.Count == 1)
                    {
                        scriptObject.script.scriptContent.Add(inputText);
                        scriptObject.script.scriptContent[currentLineInScript] = "";
                        inputText = "";
                    }
                    else
                    {


                        if (currentLineInScript == 0)
                        {
                            scriptObject.script.scriptContent.Insert(0, "");
                            inputText = "";
                        }
                        else if (currentLineInScript != 0)
                        {
                            scriptObject.script.scriptContent.Insert(currentLineInScript, "");
                            inputText = "";
                        }
                    }


                }

                //Add new line upward
                if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    if (scriptObject.script.scriptContent.Count == 1)
                    {
                        scriptObject.script.scriptContent.Add("");
                        scriptObject.script.scriptContent[currentLineInScript] = inputText;
                        inputText = "";
                        currentLineInScript++;
                    }
                    else
                    {
                        if (currentLineInScript != scriptObject.script.scriptContent.Count)
                        {
                            scriptObject.script.scriptContent.Insert(currentLineInScript + 1, "");
                        }
                        else if (currentLineInScript == scriptObject.script.scriptContent.Count)
                        {
                            scriptObject.script.scriptContent.Add("");
                            inputText = "";
                        }

                    }
                }

                //Save current script
                if (Keyboard.GetState().IsKeyDown(Keys.I) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    scriptObject.script.scriptContent[currentLineInScript] = inputText;
                    EditorFileWriter.ScriptObjectWriter(Environment.CurrentDirectory + @"\Scripts\", scriptObject);
                }

                //Load previous script
                if (Keyboard.GetState().IsKeyDown(Keys.K) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
                {

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    if (currentLineInScript < scriptObject.script.scriptContent.Count - 1)
                    {
                        currentLineInScript++;
                        inputText = scriptObject.script.scriptContent[currentLineInScript];
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    if (currentLineInScript > 0)
                    {
                        currentLineInScript--;
                        inputText = scriptObject.script.scriptContent[currentLineInScript];
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.X) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) &&!KeyboardMouseUtility.AnyButtonsPressed())
                {
                    if (currentLineInScript > 0)
                    {
                        scriptObject.script.scriptContent.RemoveAt(currentLineInScript);

                        if (scriptObject.script.scriptContent.Count-1<currentLineInScript)
                        {
                            currentLineInScript--;
                        }

                        inputText = scriptObject.script.scriptContent[currentLineInScript];
                    }
                }
            }
            #endregion

            if (ScriptProcessor.bScriptRunning)
            {
                ScriptProcessor.Update(gameTime);
            }
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.White);
            if (!ScriptProcessor.bScriptRunning)
            {
                #region draw other lines of script
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                if (scriptObject.script.scriptContent.Count > 0)
                {
                    for (int i = currentLineInScript - 1; i > -1; i--)
                    {
                        String shortenString = scriptObject.script.scriptContent[i];

                        shortenString = shortenString.Replace("\n", " ");

                        spriteBatch.DrawString(Game1.defaultFont, "line " + i + ": " + shortenString, new Vector2(50, textWindow.Location.ToVector2().Y) - new Vector2(0, 30 + 30 * (currentLineInScript - i)), Color.Black);

                    }

                    for (int i = currentLineInScript + 1; i < scriptObject.script.scriptContent.Count; i++)
                    {
                        String shortenString = scriptObject.script.scriptContent[i];

                        shortenString = shortenString.Replace("\n", " ");

                        spriteBatch.DrawString(Game1.defaultFont, "line " + i + ": " + shortenString, new Vector2(50, textWindow.Location.ToVector2().Y + textWindow.Height) + new Vector2(0, 30 + 30 * (-currentLineInScript + i)), Color.Black);

                    }
                }
                spriteBatch.End();
                #endregion
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                spriteBatch.Draw(textWindowBG, textWindow, textWindowBG.Bounds, Color.White);
                spriteBatch.Draw(Game1.hitboxHelp, textWindowInputRoom, Color.Black);
                spriteBatch.DrawString(textWindowFont, inputText, textWindowInputRoom.Location.ToVector2(), Color.Black);
                spriteBatch.DrawString(Game1.defaultFont, "L#: " + currentLineInScript, scriptInfoLoc, Color.Black);
                spriteBatch.DrawString(Game1.defaultFont, "Lines: " + scriptObject.script.scriptContent.Count, scriptInfoLoc + new Vector2(0, 30), Color.Black);
                if (bEnableInput)
                {
                    spriteBatch.DrawString(Game1.defaultFont, "Input enabled ", scriptInfoLoc + new Vector2(-10, 60), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(Game1.defaultFont, "Input disabled ", scriptInfoLoc + new Vector2(-10, 60), Color.Black);

                }
                spriteBatch.DrawString(Game1.defaultFont, "Test Script", testScriptButton.Location.ToVector2(), Color.Black);
                spriteBatch.End();

            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                spriteBatch.Draw(textWindowBG, textWindow, textWindowBG.Bounds, Color.White);
                spriteBatch.DrawString(textWindowFont,ScriptProcessor.processedPieceTextString,textWindowInputRoom.Location.ToVector2(),Color.Black);
                if (ScriptProcessor.bCompletedProcessingLine)
                {
                    spriteBatch.DrawString(textWindowFont, "Press ENTER to continue", textWindowInputRoom.Location.ToVector2()+new Vector2(600,300), Color.Black);
                }
                spriteBatch.End();
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(Game1.hitboxHelp,testScriptButton,Color.Gray);
            spriteBatch.End();
        }
    }

    [XmlRoot("TestScriptObject")]
    public class ScriptTestObject
    {
        [XmlElement("ObjectScript")]
        public BaseScript script = new BaseScript();

        public ScriptTestObject()
        {

        }

        public ScriptTestObject(int identifier)
        {
            script = new BaseScript(identifier);
        }
    }
}
