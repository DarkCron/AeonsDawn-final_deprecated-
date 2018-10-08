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
using TBAGW.Utilities.Sprite;

namespace TBAGW.Forms.Animation
{
    public partial class AnimationEditor : Form
    {
        public AnimationEditor()
        {
            InitializeComponent();
        }

        private void AnimationEditor_Load(object sender, EventArgs e)
        {

        }

        ShapeAnimation selectedAnimation;
        public void Start(ShapeAnimation sa)
        {
            selectedAnimation = sa;

            if (sa.animationFrames.Count != 0)
            {
                frameWidth.Value = sa.animationFrames[0].Width;
                frameHeight.Value = sa.animationFrames[0].Height;
            }

            numericUpDown1.Value = sa.frameInterval;

            Show();
        }

        private void button62_Click(object sender, EventArgs e)
        {
            if (selectedAnimation.texFileLoc.Equals(""))
            {
                MessageBox.Show("Please select base texture file.");
                OpenFileDialog openTex = new OpenFileDialog();
                openTex.Title = "Open texture file";
                if (Game1.bIsDebug)
                {
                    openTex.Filter = "Texture File|*.jpg;*.png;*.jpeg;*.xnb";
                    openTex.InitialDirectory = Game1.rootContent;
                }
                else
                {
                    openTex.Filter = "Texture File|*.jpg;*.png;*.jpeg";
                    openTex.InitialDirectory = Game1.rootContentExtra;
                }

                bool bDone = false;

                while (!bDone)
                {
                    DialogResult dia = openTex.ShowDialog();
                    if (dia == DialogResult.OK && openTex.FileName.Contains(openTex.InitialDirectory))
                    {

                        selectedAnimation.texFileLoc = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
                        Console.WriteLine("Successful item texture selection");
                        bDone = true;
                    }
                    else if (!openTex.FileName.Contains(openTex.InitialDirectory))
                    {
                        MessageBox.Show(@"Please select a file within the application folder under Content\Mods and it's subfolders");
                    }
                    else if (dia == DialogResult.Cancel)
                    {
                        bDone = true;
                    }
                }

            }


            try
            {
                selectedAnimation.ReloadTexture();
                FrameSelector.StartComplex(selectedAnimation, (int)frameWidth.Value, (int)frameHeight.Value, (int)-xOffSet.Value, (int)yOffSet.Value);
            }
            catch
            {


            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please select base texture file.");
            OpenFileDialog openTex = new OpenFileDialog();
            openTex.Title = "Open texture file";
            if (Game1.bIsDebug)
            {
                openTex.Filter = "Texture File|*.jpg;*.png;*.jpeg;*.xnb";
                openTex.InitialDirectory = Game1.rootContent;
            }
            else
            {
                openTex.Filter = "Texture File|*.jpg;*.png;*.jpeg";
                openTex.InitialDirectory = Game1.rootContentExtra;
            }

            bool bDone = false;

            while (!bDone)
            {
                DialogResult dia = openTex.ShowDialog();
                if (dia == DialogResult.OK && openTex.FileName.Contains(openTex.InitialDirectory))
                {

                    selectedAnimation.texFileLoc = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
                    Console.WriteLine("Successful item texture selection");
                    bDone = true;
                    selectedAnimation.animationFrames.Clear();
                }
                else if (!openTex.FileName.Contains(openTex.InitialDirectory))
                {
                    MessageBox.Show(@"Please select a file within the application folder under Content\Mods and it's subfolders");
                }
                else if (dia == DialogResult.Cancel)
                {
                    bDone = true;
                }
            }

            try
            {
                selectedAnimation.ReloadTexture();
                FrameSelector.StartComplex(selectedAnimation, (int)frameWidth.Value, (int)frameHeight.Value, (int)-xOffSet.Value, (int)yOffSet.Value);
            }
            catch
            {

                throw;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FrameSelector.Stop();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            selectedAnimation.frameInterval = (int)numericUpDown1.Value;
        }
    }
}
