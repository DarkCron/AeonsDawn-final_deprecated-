using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TBAGW.Scenes.Editor.SpriteEditorSub
{
    static public class BasicSpriteFinalize
    {
        #region FIELDS
        static public Rectangle shapeTextureBounds = new Rectangle();
        static public bool bCollision = false;
        static public String shapeName = "";
        static public Rectangle rectangleToDraw = new Rectangle(); //Original sprite bounds
        static public Texture2D shapeTexture;
        static public Texture2D hitboxTexture;
        static public Rectangle spriteGameSize = new Rectangle();
        static public Rectangle hitBoxTexBox;
        static String rootBS = Environment.CurrentDirectory + @"\SpriteContent\";
        static public System.Windows.Forms.SaveFileDialog spriteSave = new System.Windows.Forms.SaveFileDialog();
        #endregion
        //Texture2D shapeTexture, Texture2D hitboxTexture, Rectangle spriteGameSize, Rectangle hitBoxTexBox, Rectangle rectangleToDraw
        static public void Start()
        {
            spriteGameSize = rectangleToDraw;
            BaseSprite testSprite = new BaseSprite(shapeTexture, hitboxTexture, spriteGameSize, hitBoxTexBox, rectangleToDraw, 1, Vector2.Zero);
            if (Game1.bIsDebug)
            {
                spriteSave.Filter = "CG BaseSprite|*.cgbsc";
                spriteSave.Title = "Save a Sprite File";
                spriteSave.InitialDirectory = Game1.rootTBAGW;

                System.Windows.Forms.DialogResult dia = spriteSave.ShowDialog();

                if (System.Windows.Forms.DialogResult.OK == dia && spriteSave.FileName.Contains(Game1.rootTBAGW))
                {
                    EditorFileWriter.BasicSpriteWriter(spriteSave.FileName,testSprite);
                }
                else if (System.Windows.Forms.DialogResult.Cancel == dia)
                {
                    System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Leave the Sprite unsaved?", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo);
                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        Editor.currentEditor = (int)Editor.EditorsCollection.MapEditor;
                        System.Windows.Forms.MessageBox.Show("Cancelled, returning to MapEditor.");
                    }
                    else if (dialogResult == System.Windows.Forms.DialogResult.No)
                    {
                        Start();
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Select a folder within TBAGW");
                    Start();
                }
            }
            else
            {
                spriteSave.Filter = "CG BaseSprite|*.cgbs";
                spriteSave.Title = "Save a Sprite File";
                spriteSave.InitialDirectory = Game1.rootContentExtra;

                System.Windows.Forms.DialogResult dia = spriteSave.ShowDialog();

                if (System.Windows.Forms.DialogResult.OK == dia && spriteSave.FileName.Contains(Game1.rootContentExtra))
                {
                    EditorFileWriter.BasicSpriteWriter(spriteSave.FileName, testSprite);
                }
                else if (System.Windows.Forms.DialogResult.Cancel == dia)
                {
                    System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Leave the Sprite unsaved?", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo);
                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        Editor.currentEditor = (int)Editor.EditorsCollection.MapEditor;
                        System.Windows.Forms.MessageBox.Show("Cancelled, returning to MapEditor.");
                    }
                    else if (dialogResult == System.Windows.Forms.DialogResult.No)
                    {
                        Start();
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Select a folder within Content Mods");
                    Start();
                }
            }

            System.Windows.Forms.MessageBox.Show("Sprite created, returning to map editor");
            SpriteEditor.currentScene = (int)SpriteEditor.SpriteEditorScenes.SpriteEditor;
            Editor.currentEditor = (int)Editor.EditorsCollection.MapEditor;

        }
    }
}
