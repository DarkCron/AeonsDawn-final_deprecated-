using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Forms.Animation;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Sprite;

namespace TBAGW.Forms.Particle_Animation
{
    public partial class PAEditor : Form
    {
        public PAEditor()
        {
            InitializeComponent();
        }

        Animation.AnimationEditor ae = new Animation.AnimationEditor();

        public void Start()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = MapBuilder.gcDB.gameParticleAnimations;
            listBox2.DataSource = null;
            listBox2.DataSource = MapBuilder.gcDB.gameMagicCircleAnimations;
            listBox3.DataSource = null;
            listBox3.DataSource = MapBuilder.gcDB.gameMagicCircleAnimations;
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                Form1.MakeSureFormClosed(ae);
                if (FrameSelector.bIsRunning)
                {
                    FrameSelector.Stop();
                }
                ae = new Animation.AnimationEditor();
                ae.Start(listBox1.SelectedItem as ShapeAnimation);
            }
        }

        private void MakeSureFormIsClosed(Form f)
        {
            if (!f.IsDisposed)
            {
                f.Close();
                f.Dispose();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                textBox2.Text = (listBox1.SelectedItem as ParticleAnimation).particleAnimationName;
                numericUpDown1.Value = (listBox1.SelectedItem as ParticleAnimation).lengthTime;
                numericUpDown2.Value = (listBox1.SelectedItem as ParticleAnimation).buildUp;
                numericUpDown3.Value = (listBox1.SelectedItem as ParticleAnimation).buildOff;


                if ((listBox1.SelectedItem as ParticleAnimation).mcb != null)
                {
                    numericUpDown4.Value = (listBox1.SelectedItem as ParticleAnimation).magicCircleVOS;

                    numericUpDown8.Value = (decimal)(listBox1.SelectedItem as ParticleAnimation).mcb.frameInterval;
                }

                numericUpDown5.Value = (decimal)(listBox1.SelectedItem as ParticleAnimation).scalePA;
                numericUpDown6.Value = (decimal)(listBox1.SelectedItem as ParticleAnimation).scalePAOS.X;
                numericUpDown7.Value = (decimal)(listBox1.SelectedItem as ParticleAnimation).scalePAOS.Y;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MapBuilder.gcDB.AddParticleAnimation(new ParticleAnimation());
            textBox1_TextChanged(sender, e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                // (listBox1.SelectedItem as ParticleAnimation).particleAnimationName = textBox2.Text;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (!textBox2.Text.Equals(""))
                {
                    (listBox1.SelectedItem as ParticleAnimation).particleAnimationName = textBox2.Text;
                    textBox1_TextChanged(sender, e);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.SelectedIndex = -1;
                listBox1.DataSource = null;
                listBox1.DataSource = (MapBuilder.gcDB.gameParticleAnimations);
            }
            else
            {
                listBox1.SelectedIndex = -1;
                listBox1.DataSource = null;
                listBox1.DataSource = (MapBuilder.gcDB.gameParticleAnimations.FindAll(o => o.particleAnimationName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private void PAEditor_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox2.SelectedIndex = -1;
                listBox2.DataSource = null;
                listBox2.DataSource = (MapBuilder.gcDB.gameMagicCircleAnimations);
            }
            else
            {
                listBox2.SelectedIndex = -1;
                listBox2.DataSource = null;
                listBox2.DataSource = (MapBuilder.gcDB.gameMagicCircleAnimations.FindAll(o => o.magicCircleBaseName.IndexOf(textBox3.Text, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                if (!textBox4.Text.Equals(""))
                {
                    (listBox2.SelectedItem as MagicCircleBase).magicCircleBaseName = textBox4.Text;
                    textBox3_TextChanged(sender, e);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                Form1.MakeSureFormClosed(ae);
                if (FrameSelector.bIsRunning)
                {
                    FrameSelector.Stop();
                }
                ae = new Animation.AnimationEditor();
                ae.Start(listBox2.SelectedItem as ShapeAnimation);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MapBuilder.gcDB.AddMagicCircle(new MagicCircleBase());
            textBox3_TextChanged(sender, e);
            listBox3.DataSource = null;
            listBox3.DataSource = MapBuilder.gcDB.gameMagicCircleAnimations;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            var temp = (listBox1.SelectedItem as ParticleAnimation).Clone();
            temp.GenerateLogic(MapBuilder.gcDB.gameCharacters[0], Utilities.Characters.BaseCharacter.CharacterBattleAnimations.Hurt, Utilities.Characters.BaseCharacter.CharacterBattleAnimations.Idle);
            MapBuilder.eprList.Add(new EditorPreviewRender(typeof(ParticleAnimation), temp, 3));

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (listBox3.SelectedIndex != -1)
                {
                    var temp1 = listBox1.SelectedItem as ParticleAnimation;
                    var temp2 = listBox3.SelectedItem as MagicCircleBase;

                    temp1.magicCircleID = temp2.magicCircleBaseID;
                    temp1.mcb = temp2.Clone();
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp1 = listBox1.SelectedItem as ParticleAnimation;
                temp1.lengthTime = (int)numericUpDown1.Value;
                temp1.GenerateFrameIntervals();

                if ((listBox1.SelectedItem as ParticleAnimation).mcb != null)
                {
                    numericUpDown8.Value = (decimal)(listBox1.SelectedItem as ParticleAnimation).mcb.frameInterval;
                }
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp1 = listBox1.SelectedItem as ParticleAnimation;
                int amount = (int)numericUpDown2.Value;
                if (amount > temp1.lengthTime - temp1.buildOff)
                {
                    amount = temp1.lengthTime - temp1.buildOff;
                }
                temp1.buildUp = amount;
                temp1.GenerateFrameIntervals();

                if ((listBox1.SelectedItem as ParticleAnimation).mcb != null)
                {
                    numericUpDown8.Value = (decimal)(listBox1.SelectedItem as ParticleAnimation).mcb.frameInterval;
                }
            }
        }


        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp1 = listBox1.SelectedItem as ParticleAnimation;

                ColorDialog colorDialog1 = new ColorDialog();
                DialogResult result = colorDialog1.ShowDialog();
                // See if user pressed ok.
                Color temp = Color.White;
                if (result == DialogResult.OK)
                {
                    // Set form background to the selected color.
                    temp = colorDialog1.Color;
                }

                temp1.magicCircleColor = new Microsoft.Xna.Framework.Color(temp.R, temp.G, temp.B, temp.A);
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp1 = listBox1.SelectedItem as ParticleAnimation;
                int amount = (int)numericUpDown3.Value;
                if (amount > temp1.lengthTime - temp1.buildUp)
                {
                    amount = temp1.lengthTime - temp1.buildUp;
                }
                temp1.buildOff = amount;
                temp1.GenerateFrameIntervals();

                if ((listBox1.SelectedItem as ParticleAnimation).mcb != null)
                {
                    numericUpDown8.Value = (decimal)(listBox1.SelectedItem as ParticleAnimation).mcb.frameInterval;
                }
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp1 = listBox1.SelectedItem as ParticleAnimation;
                temp1.magicCircleVOS = (int)numericUpDown4.Value;
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp1 = listBox1.SelectedItem as ParticleAnimation;
                temp1.scalePA = (float)numericUpDown5.Value;
            }
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp1 = listBox1.SelectedItem as ParticleAnimation;
                temp1.scalePAOS.X = (int)numericUpDown6.Value;
            }
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp1 = listBox1.SelectedItem as ParticleAnimation;
                temp1.scalePAOS.Y = (int)numericUpDown7.Value;
            }
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {



        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp1 = listBox1.SelectedItem as ParticleAnimation;

                if (temp1.mcb != null)
                {
                    temp1.mcb.frameInterval = (int)numericUpDown8.Value;
                }
            }
        }
    }
}
