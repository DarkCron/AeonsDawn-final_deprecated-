using TBAGW.Scenes.Editor.SpriteEditorSub;
using TBAGW.Utilities;
using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace TBAGW.Scenes.Editor
{
    class SpriteEditor : Scene
    {
        public List<Scene> scenes = new List<Scene>();
        public List<Scene> localScenes = new List<Scene>();
        public enum SpriteEditorScenes { SpriteEditor = 0, SpritePicker, SpriteCollisionPicker, SpriteCollisionSelect, SpriteTypeSelection, SimpleTypeSpriteEditor };
        static public int currentScene = (int)SpriteEditorScenes.SpriteEditor;

        SpritePicker spritePicker = new SpritePicker();
        SpriteCollisionPicker spriteCollisionPicker = new SpriteCollisionPicker();
        SpriteCollisionSelect spriteCollisionSelect = new SpriteCollisionSelect();
        SpriteTypeSelection spriteTypeSelection = new SpriteTypeSelection();
        SimpleTypeSpriteEditor simpleTypeSpriteEditor = new SimpleTypeSpriteEditor();

        Rectangle Step3Box;
        Rectangle Step5Box;
        String CollisionStepFileName = "nope";
        Texture2D Step2Texture;
        Texture2D Step4Texture;

        int spriteType = -1;
        System.Windows.Forms.OpenFileDialog openTextureBase = new System.Windows.Forms.OpenFileDialog();


        public override void Initialize(Game1 game)
        {

        }


        bool bShowed = false;
        public override void Update(GameTime gameTime, Game1 game)
        {
            Vector2 EditorCursorPos = Mouse.GetState().Position.ToVector2();
            if (localScenes.Count == 0)
            {
                localScenes.Add(spritePicker);
                localScenes.Add(spriteCollisionPicker);
                localScenes.Add(spriteCollisionSelect);
                localScenes.Add(spriteTypeSelection);
                localScenes.Add(simpleTypeSpriteEditor);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L) && !bShowed)
            {
                bShowed = true;
                var types = Assembly.GetAssembly(typeof(Shape)).GetTypes().Where(t => t.IsSubclassOf(typeof(Shape)));
                foreach (var type in types)
                {
                    Console.Out.WriteLine(type);
                }
                //System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }


            if ((int)SpriteEditor.SpriteEditorScenes.SpriteTypeSelection >= currentScene && Mouse.GetState().RightButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed() && currentScene > 0)
            {
                currentScene--;
            }

            switch (currentScene)
            {
                case (int)SpriteEditor.SpriteEditorScenes.SpriteEditor:
                    if (Game1.bIsDebug)
                    {
                        System.Windows.Forms.MessageBox.Show("Choose a texture file from within the application's Content folder please.");
                        openTextureBase.Filter = "Texture File|*.xnb";
                        openTextureBase.InitialDirectory = Game1.rootContent;

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Choose a texture file from within the application's Content Mod folder please.");
                        openTextureBase.Filter = "Texture File|*.jpg;*.png;*.jpeg";
                        openTextureBase.InitialDirectory = Game1.rootContentExtra;
                    }
                    openTextureBase.Title = "Load Base Texture";
                    if (Game1.bIsDebug)
                    {
                        System.Windows.Forms.DialogResult dia = openTextureBase.ShowDialog();

                        if (System.Windows.Forms.DialogResult.OK == dia && openTextureBase.FileName.Contains(Game1.rootContent))
                        {
                            String fi = Path.GetFileNameWithoutExtension(openTextureBase.FileName);
                            String fo = Path.GetDirectoryName(openTextureBase.FileName.Replace(Game1.rootContent, ""));
                            Console.WriteLine(fo + fi);
                            spritePicker.Initialize(game, Path.Combine(fo, fi));
                        }
                        else if (System.Windows.Forms.DialogResult.Cancel == dia)
                        {
                            Editor.currentEditor = (int)Editor.EditorsCollection.MapEditor;
                            System.Windows.Forms.MessageBox.Show("Cancelled, returning to MapEditor.");
                        }
                    }
                    else
                    {
                        System.Windows.Forms.DialogResult dia = openTextureBase.ShowDialog();

                        if (System.Windows.Forms.DialogResult.OK == dia && openTextureBase.FileName.Contains(Game1.rootContentExtra))
                        {
                            String fi = Path.GetFileNameWithoutExtension(openTextureBase.FileName);
                            String fo = Path.GetDirectoryName(openTextureBase.FileName.Replace(Game1.rootContent, ""));
                            Console.WriteLine(fo + fi);
                            spritePicker.Initialize(game, Path.Combine(fo, fi));
                        }
                        else if (System.Windows.Forms.DialogResult.Cancel == dia)
                        {
                            Editor.currentEditor = (int)Editor.EditorsCollection.MapEditor;
                            System.Windows.Forms.MessageBox.Show("Cancelled, returning to MapEditor.");
                        }
                    }


                    break;
                case (int)SpriteEditor.SpriteEditorScenes.SpritePicker:
                    spritePicker.Update(gameTime, game);
                    if (spritePicker.EnableNextStep() != default(Rectangle))
                    {
                        spritePicker.ResetCamera();
                        Step3Box = spritePicker.EnableNextStep();
                        spritePicker.selectedTextureBox = default(Rectangle);
                        Step2Texture = spritePicker.ProcessedTexture();
                        currentScene = (int)SpriteEditorScenes.SpriteCollisionPicker;
                        spriteCollisionPicker.Initialize(game, Step3Box, spritePicker.ProcessedTexture());
                    }
                    break;
                case (int)SpriteEditor.SpriteEditorScenes.SpriteCollisionPicker:
                    spriteCollisionPicker.Update(gameTime, game);
                    if (!spriteCollisionPicker.EnableNextStep().Equals("") && !spriteCollisionPicker.EnableNextStep().Equals("SKIP"))
                    {
                        CollisionStepFileName = spriteCollisionPicker.EnableNextStep();
                        spriteCollisionPicker.selectedFile = "";
                        currentScene = (int)SpriteEditorScenes.SpriteCollisionSelect;
                        spriteCollisionSelect.Initialize(game, CollisionStepFileName, Step3Box, Step2Texture);

                    }
                    else if (spriteCollisionPicker.EnableNextStep().Equals("SKIP"))
                    {
                        Console.Out.WriteLine("Attempting to skip...");
                        currentScene = (int)SpriteEditor.SpriteEditorScenes.SpriteTypeSelection;
                        spriteCollisionSelect.Initialize(game, spritePicker.ProcessedTexture().Name, Step3Box, Step2Texture);
                        spriteTypeSelection.Initialize(game, Step3Box, Step2Texture, Step2Texture);

                    }
                    break;
                case (int)SpriteEditor.SpriteEditorScenes.SpriteCollisionSelect:
                    if (!CollisionStepFileName.Equals("SKIP") || !CollisionStepFileName.Equals("nope"))
                    {
                        spriteCollisionPicker.selectedFile = "";
                        spriteCollisionSelect.Update(gameTime, game);
                        if (spriteCollisionSelect.EnableNextStep() != default(Rectangle))
                        {
                            spriteCollisionSelect.ResetCamera();
                            Step5Box = spriteCollisionSelect.EnableNextStep();
                            Step4Texture = spriteCollisionSelect.ProcessedTexture();
                            currentScene = (int)SpriteEditorScenes.SpriteTypeSelection;
                            spriteCollisionSelect.selectedTextureBox = default(Rectangle);
                            spriteTypeSelection.Initialize(game, Step3Box, Step2Texture, Step4Texture, Step5Box);
                        }
                    }
                    else
                    {
                        spriteCollisionSelect.ResetCamera();
                        currentScene = (int)SpriteEditorScenes.SpritePicker;
                        spriteTypeSelection.Initialize(game, Step3Box, Step2Texture, Step4Texture);
                    }

                    break;
                case (int)SpriteEditor.SpriteEditorScenes.SpriteTypeSelection:
                    spriteTypeSelection.Update(gameTime, game);
                    spriteType = spriteTypeSelection.EnableNextStep();
                    if (spriteType >= 0)
                    {
                        switch (spriteType)
                        {
                            case (int)SpriteTypeSelection.SpriteTypes.SimpleType:
                                spriteTypeSelection.ResetCamera();
                                spriteTypeSelection.selectedType = -1;
                                currentScene = (int)SpriteEditor.SpriteEditorScenes.SimpleTypeSpriteEditor;
                                simpleTypeSpriteEditor.Start();
                                break;
                        }
                    }

                    break;
                case (int)SpriteEditor.SpriteEditorScenes.SimpleTypeSpriteEditor:
                    simpleTypeSpriteEditor.Update(gameTime, game);
                    if (Mouse.GetState().RightButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed() && currentScene > 0)
                    {
                        currentScene = (int)SpriteEditor.SpriteEditorScenes.SpriteTypeSelection;
                    }
                    break;

            }

        }

        Vector2 startPosButtons = new Vector2(100, 100);

        public override void UnloadContent(Game1 game)
        {
        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);

            Game1.graphics.GraphicsDevice.Clear(Color.White);
            switch (currentScene)
            {
                case (int)SpriteEditor.SpriteEditorScenes.SpriteEditor:
                    spriteBatch.DrawString(Game1.defaultFont, "Step 1: Select SpriteSheet (In any situation press RMB to go to previous step)", new Vector2(100, 50), Color.Black);

                    break;
                case (int)SpriteEditor.SpriteEditorScenes.SpritePicker:
                    spritePicker.Draw(gametime, spriteBatch);
                    break;
                case (int)SpriteEditor.SpriteEditorScenes.SpriteCollisionPicker:
                    spriteCollisionPicker.Draw(gametime, spriteBatch);
                    break;
                case (int)SpriteEditor.SpriteEditorScenes.SpriteCollisionSelect:
                    spriteCollisionSelect.Draw(gametime, spriteBatch);
                    break;
                case (int)SpriteEditor.SpriteEditorScenes.SpriteTypeSelection:
                    spriteTypeSelection.Draw(gametime, spriteBatch);
                    break;
                case (int)SpriteEditor.SpriteEditorScenes.SimpleTypeSpriteEditor:
                    simpleTypeSpriteEditor.Draw(gametime, spriteBatch);
                    break;
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);

            return spriteBatch;
        }

    }

    public class ActiveInput
    {
        String inputText = "";
        String propertyText = "";

        SpriteFont defaultFont;
        public Vector2 position = Vector2.Zero;

        //Changed automatically via update function
        Rectangle LPartInputHitBox = new Rectangle(0, 0, 8, 64);
        Rectangle MPartInputHitBox;
        Rectangle RPartInputHitBox = new Rectangle(0, 0, 8, 64);

        //These values are default values and are not to be changed.
        Rectangle LPartInputTexture = new Rectangle(0, 0, 8, 64);
        Rectangle MPartInputTexture = new Rectangle(8, 0, 48, 64);
        Rectangle RPartInputTexture = new Rectangle(56, 0, 8, 64);
        Rectangle CPartInputTexture = new Rectangle(64, 0, 64, 64);

        Texture2D InputBoxTexture;

        public bool bEnableInput = false;

        float cursorTimerMin = 500f;
        float cursorTimerMax = 1000f;
        float cursorTimerElapsed = 0f;

        public bool bNumericInputOnly = false;
        public bool bHasMinValue = false;
        public int minValue = 0;

        public void ResetBool()
        {
            inputText = "0";
        }

        public void AssignInput(int i)
        {
            inputText = i.ToString();
        }

        public void AssignInput(String s)
        {
            if (!bNumericInputOnly)
            {
                inputText = s;
            }
        }

        public ActiveInput(String propertyText, String inputText, Vector2 position)
        {
            defaultFont = Game1.defaultFont;
            this.inputText = inputText;
            this.position = position;
            this.propertyText = propertyText;
            InputBoxTexture = Game1.contentManager.Load<Texture2D>(@"Editor\General\ActiveTextInput");
        }

        public ActiveInput(String propertyText, String inputText, Vector2 position, Game1 game)
        {
            defaultFont = Game1.defaultFont;
            this.inputText = inputText;
            this.position = position;
            this.propertyText = propertyText;
            InputBoxTexture = game.Content.Load<Texture2D>(@"Editor\General\ActiveTextInput");
        }

        public ActiveInput(String propertyText, String inputText, Vector2 position, ContentManager manager)
        {
            defaultFont = Game1.defaultFont;
            this.inputText = inputText;
            this.position = position;
            this.propertyText = propertyText;
            InputBoxTexture = manager.Load<Texture2D>(@"Editor\General\ActiveTextInput");
        }

        public void Update(GameTime gameTime)
        {
            LPartInputHitBox = new Rectangle((int)defaultFont.MeasureString(propertyText).X + (int)position.X, (int)position.Y, 8, 64);
            MPartInputHitBox = new Rectangle((int)defaultFont.MeasureString(propertyText).X + (int)position.X + 8, (int)position.Y, (int)defaultFont.MeasureString(inputText).X + 1 + 32, 64);
            RPartInputHitBox = new Rectangle((int)defaultFont.MeasureString(propertyText).X + (int)position.X + MPartInputHitBox.Width + 8, (int)position.Y, 8, 64);

            cursorTimerElapsed += gameTime.ElapsedGameTime.Milliseconds;



            if (bEnableInput)
            {
                if (!bNumericInputOnly)
                {
                    if (Keyboard.GetState().GetPressedKeys().Length >= 1)
                    {
                        // Console.Out.WriteLine((int)Keyboard.GetState().GetPressedKeys()[0]);

                        if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            //Numdpad keys are not enabled...yet?... They need manual reprogramming they're values 96-105 starting at 0-9
                            if (((int)Keyboard.GetState().GetPressedKeys()[0] > 47 && (int)Keyboard.GetState().GetPressedKeys()[0] < 91) && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                if (inputText.Length > 0)
                                {
                                    inputText = inputText.Insert(inputText.Length, ((char)Keyboard.GetState().GetPressedKeys()[0]).ToString().ToLower());
                                }
                                else
                                {
                                    inputText = ((char)Keyboard.GetState().GetPressedKeys()[0]).ToString().ToLower();
                                }

                            }
                            else if (inputText.Length > 0 && Keyboard.GetState().IsKeyDown(Keys.Back) && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                inputText = inputText.Remove(inputText.Length - 1);
                            }
                            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                bEnableInput = false;
                            }
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().GetPressedKeys().Length == 2)
                        {
                            //Numdpad keys are not enabled...yet?... They need manual reprogramming they're values 96-105 starting at 0-9
                            if (((int)Keyboard.GetState().GetPressedKeys()[0] > 47 && (int)Keyboard.GetState().GetPressedKeys()[0] < 91) && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                if (inputText.Length > 0)
                                {
                                    inputText = inputText.Insert(inputText.Length, ((char)Keyboard.GetState().GetPressedKeys()[0]).ToString().ToUpper());
                                }
                                else
                                {
                                    inputText = ((char)Keyboard.GetState().GetPressedKeys()[0]).ToString().ToUpper();
                                }
                                // Console.Out.WriteLine((char)Keyboard.GetState().GetPressedKeys()[0]);
                            }
                            else if (inputText.Length > 0 && Keyboard.GetState().IsKeyDown(Keys.Back) && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                inputText = inputText.Remove(inputText.Length - 1);
                            }
                            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                bEnableInput = false;
                            }

                        }
                    }
                }
                else if (bNumericInputOnly)
                {
                    if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    {
                        if (((int)Keyboard.GetState().GetPressedKeys()[0] > 47 && (int)Keyboard.GetState().GetPressedKeys()[0] < 58) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            if (inputText.Length > 0)
                            {
                                inputText = inputText.Insert(inputText.Length, ((char)Keyboard.GetState().GetPressedKeys()[0]).ToString().ToLower());
                            }
                            else
                            {
                                inputText = ((char)Keyboard.GetState().GetPressedKeys()[0]).ToString().ToLower();
                            }

                        }
                        else if (inputText.Length > 0 && Keyboard.GetState().IsKeyDown(Keys.Back) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            inputText = inputText.Remove(inputText.Length - 1);
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            bEnableInput = false;
                        }
                    }
                }

            }

            if (bNumericInputOnly)
            {
                if (bHasMinValue && !inputText.Equals(""))
                {
                    if (minValue > ReturnInputNumbers())
                    {
                        inputText = minValue.ToString();
                    }
                }
            }

        }

        public void Contains(Vector2 mousePos)
        {
            if (!bEnableInput)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (LPartInputHitBox.Contains(mousePos))
                    {
                        bEnableInput = true;
                    }
                    else if (MPartInputHitBox.Contains(mousePos))
                    {
                        bEnableInput = true;
                    }
                    else if (RPartInputHitBox.Contains(mousePos))
                    {
                        bEnableInput = true;
                    }

                }
            }
            else if (bEnableInput)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    bool bMouseInsideTarget = false;

                    if (LPartInputHitBox.Contains(mousePos))
                    {
                        bMouseInsideTarget = true;
                    }
                    else if (MPartInputHitBox.Contains(mousePos))
                    {
                        bMouseInsideTarget = true;
                    }
                    else if (RPartInputHitBox.Contains(mousePos))
                    {
                        bMouseInsideTarget = true;
                    }

                    bEnableInput = bMouseInsideTarget;
                }
            }
        }

        /// <summary>
        /// Draw inside begin() and end()
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="textColor"></param>
        public void Draw(SpriteBatch spriteBatch, Color textColor = default(Color))
        {
            spriteBatch.Draw(InputBoxTexture, LPartInputHitBox, LPartInputTexture, Color.White);
            spriteBatch.Draw(InputBoxTexture, MPartInputHitBox, MPartInputTexture, Color.White);
            spriteBatch.Draw(InputBoxTexture, RPartInputHitBox, RPartInputTexture, Color.White);

            if (bEnableInput)
            {
                if (cursorTimerElapsed > cursorTimerMin && cursorTimerElapsed < cursorTimerMax)
                {
                    spriteBatch.Draw(InputBoxTexture, new Vector2(MPartInputHitBox.X + MPartInputHitBox.Width - 32, RPartInputHitBox.Y), CPartInputTexture, Color.White);

                }
                else if (cursorTimerElapsed > cursorTimerMax)
                {
                    cursorTimerElapsed = 0;
                }
            }


            if (textColor == default(Color))
            {
                spriteBatch.DrawString(defaultFont, propertyText, new Vector2(position.X, position.Y + 16), Color.Black);
                spriteBatch.DrawString(defaultFont, inputText, new Vector2(MPartInputHitBox.Location.ToVector2().X, MPartInputHitBox.Location.ToVector2().Y + 16), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(defaultFont, propertyText, new Vector2(position.X, position.Y + 16), textColor);
                spriteBatch.DrawString(defaultFont, inputText, new Vector2(MPartInputHitBox.Location.ToVector2().X, MPartInputHitBox.Location.ToVector2().Y + 16), textColor);
            }
        }

        public String ReturnInputString()
        {
            return inputText;
        }

        public int ReturnInputNumbers()
        {
            try
            {
                return int.Parse(inputText);
            }
            catch (Exception)
            {
                Console.Out.WriteLine("Not correct input numbers");
            }

            return 0;
        }
    }
}
