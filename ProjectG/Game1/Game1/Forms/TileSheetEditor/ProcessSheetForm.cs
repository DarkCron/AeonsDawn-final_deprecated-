using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sprite;

namespace TBAGW.Forms.TileSheetEditor
{
    public partial class ProcessSheetForm : Form
    {
        public ProcessSheetForm()
        {
            InitializeComponent();
        }

        SpriteExportObjectCollection seoColl;
        Microsoft.Xna.Framework.Graphics.Texture2D sheetTex;
        String texLoc;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML files (*.xml) | *.xml;";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                seoColl = EditorFileWriter.TileSheetProcessor(ofd.FileName);
            }
            Console.WriteLine("Tiles: " + seoColl.lseo.FindAll(s => s.spriteObjectType == SpriteExportObject.spriteType.Tile).Count + ", Objects: " + seoColl.lseo.FindAll(s => s.spriteObjectType == SpriteExportObject.spriteType.Object).Count);
            if (seoColl != null)
            {
                label2.Text = "OK.";
                label3.Text = ofd.FileName;
            }
            else
            {
                label2.Text = "NOT OK.";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XNB files (*.xnb) | *.xnb;";
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.Contains(Game1.rootContent))
            {
                String temp1 = ofd.FileName.Replace(Game1.rootContent, "");
                temp1 = temp1.Substring(0, temp1.IndexOf("."));
                sheetTex = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(temp1);
                texLoc = temp1;
            }
            else if (ofd.ShowDialog() == DialogResult.OK && !ofd.FileName.Contains(Game1.rootContent))
            {
                MessageBox.Show("Please select a file inside the Game's Content folder.");
            }

            if (sheetTex != null)
            {
                label5.Text = "OK.";
                label4.Text = ofd.FileName;
            }
            else
            {
                label5.Text = "NOT OK.";
            }
        }

        List<TileSource> generatedSources = new List<TileSource>();
        List<BaseSprite> generatedObjects = new List<BaseSprite>();
        private void button3_Click(object sender, EventArgs e)
        {
            if (seoColl != null && sheetTex != null)
            {
                generatedSources = new List<TileSource>();
                generatedObjects = new List<BaseSprite>();

                foreach (var item in seoColl.lseo)
                {
                    if (item.spriteObjectType == SpriteExportObject.spriteType.Tile)
                    {
                        TileSource ts = new TileSource();
                        ts.tileName = item.name;
                        ts.tileAnimation = new ShapeAnimation();
                        ts.tileAnimation.texFileLoc = texLoc;
                        ts.tileAnimation.animationFrames = new List<Microsoft.Xna.Framework.Rectangle>(item.animationFrames);
                        ts.tileAnimation.ReloadTexture();
                        generatedSources.Add(ts);
                    }

                    if (item.spriteObjectType == SpriteExportObject.spriteType.Object)
                    {
                        BaseSprite ts = new BaseSprite();
                        ts.shapeName = item.name;
                        ShapeAnimation temp = new ShapeAnimation();
                        temp.texFileLoc = texLoc;
                        temp.animationFrames = new List<Microsoft.Xna.Framework.Rectangle>(item.animationFrames);
                        temp.ReloadTexture();
                        ts.baseAnimations.Add(temp);
                        ts.spriteGameSize = new Microsoft.Xna.Framework.Rectangle(0,0,item.width,item.height);
                        generatedObjects.Add(ts);
                        MapBuilder.gcDB.AddObject(ts);
                    }
                }

                foreach (var item in generatedSources)
                {
                    MapBuilder.gcDB.AddTile(item);
                }

                this.Close();
            }
        }
    }
}
