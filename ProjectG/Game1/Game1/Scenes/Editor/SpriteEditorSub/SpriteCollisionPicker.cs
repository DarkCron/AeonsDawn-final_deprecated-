using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TBAGW.Scenes.Editor.SpriteEditorSub
{
    class SpriteCollisionPicker : Scene
    {
        //OPENFILEDIALOG HERE

        Rectangle Step3Box;
        ScreenButton Step3Button = new ScreenButton(null, Game1.defaultFont, "Skip this step (no collision)", Vector2.Zero);
        Texture2D DisplayTexture;
        System.Windows.Forms.OpenFileDialog openCollisionTexture = new System.Windows.Forms.OpenFileDialog();

        public String selectedFile="";

        public void Initialize(Game1 game, Rectangle Step3Box, Texture2D DisplayTexture)
        {
            this.Step3Box = Step3Box;
            this.DisplayTexture = DisplayTexture;
            Step3Button.position = new Vector2(500, 150);
        }

        public override void Reload()
        {

        }

        public override void Update(GameTime gameTime, Game1 game)
        {


            Vector2 EditorCursorPos = Mouse.GetState().Position.ToVector2();

            Step3Button.Update(gameTime);
            Step3Button.bButtonSelected = Step3Button.ButtonBox().Contains(EditorCursorPos);
            if (Step3Button.bButtonSelected && Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.bMousePressed)
            {
                selectedFile = "SKIP";
            }

            if (Game1.bIsDebug)
            {
                System.Windows.Forms.MessageBox.Show("Choose a texture file from within the application's Content folder please.");
                openCollisionTexture.Filter = "Texture File|*.xnb";
                openCollisionTexture.InitialDirectory = Game1.rootContent;

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Choose a texture file from within the application's Content Mod folder please.");
                openCollisionTexture.Filter = "Texture File|*.jpg;*.png;*.jpeg";
                openCollisionTexture.InitialDirectory = Game1.rootContentExtra;
            }
            openCollisionTexture.Title = "Load Base Texture";
            if (Game1.bIsDebug)
            {
                System.Windows.Forms.DialogResult dia = openCollisionTexture.ShowDialog();

                if (System.Windows.Forms.DialogResult.OK == dia && openCollisionTexture.FileName.Contains(Game1.rootContent))
                {
                    String fi = Path.GetFileNameWithoutExtension(openCollisionTexture.FileName);
                    String fo = Path.GetDirectoryName(openCollisionTexture.FileName.Replace(Game1.rootContent, ""));
                    Console.WriteLine(fo + fi);
                    selectedFile = Path.Combine(fo, fi);
                }
                else if (System.Windows.Forms.DialogResult.Cancel == dia)
                {
                    SpriteEditor.currentScene = (int)SpriteEditor.SpriteEditorScenes.SpritePicker;
                    System.Windows.Forms.MessageBox.Show("Cancelled, returning to base texture picker.");
                }
            }
            else
            {
                System.Windows.Forms.DialogResult dia = openCollisionTexture.ShowDialog();

                if (System.Windows.Forms.DialogResult.OK == dia && openCollisionTexture.FileName.Contains(Game1.rootContentExtra))
                {
                    String fi = Path.GetFileNameWithoutExtension(openCollisionTexture.FileName);
                    String fo = Path.GetDirectoryName(openCollisionTexture.FileName.Replace(Game1.rootContent, ""));
                    Console.WriteLine(fo + fi);
                    selectedFile = Path.Combine(fo, fi);
                }
                else if (System.Windows.Forms.DialogResult.Cancel == dia)
                {
                    SpriteEditor.currentScene = (int)SpriteEditor.SpriteEditorScenes.SpritePicker;
                    System.Windows.Forms.MessageBox.Show("Cancelled, returning to base texture picker.");
                }
            }
        }

        public String EnableNextStep()
        {
            if (!selectedFile.Equals(""))
            {
                return selectedFile;
            }

            return "";
        }

        public override void UnloadContent(Game1 game)
        {

        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);


            spriteBatch.DrawString(Game1.defaultFont, "Step 3: Select SpriteSheet from which you would like to create a static collision\nChosen Sprite-texture:", new Vector2(100, 50), Color.Black);

            spriteBatch.Draw(Game1.hitboxHelp, new Vector2(100, 120), Step3Box, Color.Blue*.2f);
            spriteBatch.Draw(DisplayTexture, new Vector2(100, 120), Step3Box, Color.White);

            if (!Step3Button.bButtonSelected)
            {
                spriteBatch.DrawString(Game1.defaultFont, Step3Button.buttonText, new Vector2(Step3Button.position.X, Step3Button.position.Y), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(Game1.defaultFont, Step3Button.buttonText, new Vector2(Step3Button.position.X, Step3Button.position.Y), Color.BlueViolet);
            }

            spriteBatch.End();
            spriteBatch.Begin();


            return spriteBatch;
        }
    }
}
