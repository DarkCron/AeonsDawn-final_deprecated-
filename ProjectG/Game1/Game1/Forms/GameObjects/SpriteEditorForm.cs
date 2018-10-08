using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sprite;

namespace Game1.Forms.GameObjects
{
    public partial class SpriteEditorForm : Form
    {
        public SpriteEditorForm()
        {
            InitializeComponent();
        }

        BaseSprite newSprite = new BaseSprite();
        Texture2D selectedTex = null;
        int texBoxX = 64;
        int texBoxY = 64;
        int offX = 0;
        int offY = 0;
        bool bSkip = false;

        public void Start()
        {
            textBox1.Text = "";
            newSprite = new BaseSprite();
            numericUpDown1.Value = 0;
            numericUpDown4.Value = 64;
            numericUpDown5.Value = 64;
            textureloclabel.Text = "";
            selectedTex = null;
            texBoxX = 64;
            texBoxY = 64;
            offX = 0;
            offY = 0;

            listBox1.DataSource = null;
            listBox1.DataSource = Enum.GetNames(typeof(SaveTypesObject));

            LoadLightEditorStuff();

            Show();

        }

        private void LoadLightEditorStuff()
        {

            listBox3.DataSource = null;
            listBox3.DataSource = Enum.GetNames(typeof(DayLightHandler.TimeBlocksNames));
            listBox4.DataSource = null;
            listBox4.DataSource = Enum.GetNames(typeof(DayLightHandler.TimeBlocksNames));
            listBox2.DataSource = null;
            listBox2.DataSource = MapBuilder.gcDB.gameLights;
            bSkip = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void SpriteCreatorTab_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            newSprite.shapeName = textBox1.Text;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            newSprite.shapeID = (int)numericUpDown1.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            newSprite.spriteGameSize.Width = (int)numericUpDown5.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            newSprite.spriteGameSize.Height = (int)numericUpDown4.Value;
        }

        TBAGW.Forms.Animation.AnimationEditor ae = new TBAGW.Forms.Animation.AnimationEditor();
        private void button2_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(ae);
            ae = new TBAGW.Forms.Animation.AnimationEditor();
            if (newSprite.baseAnimations.Count == 0)
            {
                newSprite.baseAnimations.Add(new ShapeAnimation());
            }
            ae.Start(newSprite.baseAnimations[0]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedTex != null)
            {
                SpriteSelector.StartComplex(newSprite, texBoxX, texBoxY, offX, offY);
            }
        }

        enum SaveTypesObject { Object, ObjectGroup, Light };

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                SaveTypesObject temp = (SaveTypesObject)listBox1.SelectedIndex;

                switch (temp)
                {
                    case SaveTypesObject.Object:
                        MapBuilder.gcDB.AddObject(newSprite);
                        break;
                    case SaveTypesObject.ObjectGroup:
                        ObjectGroup og = new ObjectGroup();
                        og.AddObject(newSprite);
                        og.groupName = newSprite.shapeName + " group";
                        MapBuilder.gcDB.AddObjectGroup(og);
                        break;
                    case SaveTypesObject.Light:

                        MapBuilder.gcDB.AddLight(SpriteLight.convertFromBase(newSprite));
                        LoadLightEditorStuff();
                        break;
                    default:
                        break;
                }

                listBox1.SelectedIndex = -1;
            }
        }

        private bool HasCompleteSprite()
        {
            if (newSprite.shapeTexture != null)
            {
                if (newSprite.hitBoxTexture != null)
                {
                    return true;
                }
            }
            return false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SpriteSelector.Stop();
        }

        private void SpriteEditorForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                var temp = listBox2.SelectedItem as SpriteLight;

                Form1.MakeSureFormClosed(ae);
                ae = new TBAGW.Forms.Animation.AnimationEditor();
                if (temp.baseAnimations.Count == 0)
                {
                    temp.baseAnimations.Add(new ShapeAnimation());
                }
                ae.Start(temp.baseAnimations[0]);
            }

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                var temp = listBox2.SelectedItem as SpriteLight;

                Form1.MakeSureFormClosed(ae);
                ae = new TBAGW.Forms.Animation.AnimationEditor();

                ae.Start(temp.lightOffAnim);
            }

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 && listBox2.SelectedItem != null && !bSkip)
            {
                if (listBox3.SelectedIndex != -1)
                {
                    var temp = listBox2.SelectedItem as SpriteLight;
                    temp.timeOn = (DayLightHandler.TimeBlocksNames)listBox3.SelectedIndex;
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                var temp = listBox2.SelectedItem as SpriteLight;
                listBox3.SelectedIndex = (int)temp.timeOn;
                listBox4.SelectedIndex = (int)temp.timeOff;
                numericUpDown2.Value = (decimal)temp.lightScale;
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 && listBox2.SelectedItem != null && !bSkip)
            {
                if (listBox4.SelectedIndex != -1)
                {
                    var temp = listBox2.SelectedItem as SpriteLight;
                    temp.timeOff = (DayLightHandler.TimeBlocksNames)listBox4.SelectedIndex;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                var tempLight = listBox2.SelectedItem as SpriteLight;
                ColorDialog colorDialog1 = new ColorDialog();
                DialogResult result = colorDialog1.ShowDialog();
                // See if user pressed ok.
                Color temp = Color.White;
                if (result == DialogResult.OK)
                {
                    // Set form background to the selected color.
                    temp = colorDialog1.Color;
                }

                tempLight.lightColor = new Microsoft.Xna.Framework.Color(temp.R, temp.G, temp.B, temp.A);
            }

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 && listBox2.SelectedItem != null)
            {
                var temp = listBox2.SelectedItem as SpriteLight;
                temp.lightScale = (float)numericUpDown2.Value;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                var temp = listBox2.SelectedItem as SpriteLight;

                Form1.MakeSureFormClosed(ae);
                ae = new TBAGW.Forms.Animation.AnimationEditor();
                ae.Start(temp.lightMask);


            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                var temp = listBox2.SelectedItem as SpriteLight;
                SpriteLight lightCopy = temp.Clone();
                MapBuilder.gcDB.AddLight(lightCopy);
                bSkip = true;
                LoadLightEditorStuff();

            }
        }
    }
}
