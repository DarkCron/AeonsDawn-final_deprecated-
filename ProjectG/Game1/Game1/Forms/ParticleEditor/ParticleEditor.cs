using Microsoft.Xna.Framework;
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

namespace TBAGW.Forms.ParticleEditor
{
    public partial class ParticleEditor : Form
    {
        ParticleSystemSource testSystem;
        public ParticleEditor()
        {
            InitializeComponent();

            List<Microsoft.Xna.Framework.Rectangle> frames = new List<Microsoft.Xna.Framework.Rectangle>();
            testSystem = new ParticleSystemSource();
            testSystem.bRandomFrameStart = true;
            testSystem.decayX = 0.01f;
            testSystem.gravity = -0.14f;
            testSystem.lifeTimeMax = 5000;
            testSystem.lifeTimeMin = 3000;
            frames.Clear();
            frames.Add(new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16));
            frames.Add(new Microsoft.Xna.Framework.Rectangle(16, 0, 16, 16));
            frames.Add(new Microsoft.Xna.Framework.Rectangle(32, 0, 16, 16));
            frames.Add(new Microsoft.Xna.Framework.Rectangle(48, 0, 16, 16));
            testSystem.particleFrames = new List<Microsoft.Xna.Framework.Rectangle>(frames); ;
            testSystem.particleTexSource = @"Graphics\Particles\TestPaticle_flame_16x16";
            testSystem.particleBaseTexSource = @"Graphics\Particles\TestPaticle_16x16-sheet";
            frames.Add(new Microsoft.Xna.Framework.Rectangle(64, 0, 16, 16));
            testSystem.particleBaseFrames = new List<Microsoft.Xna.Framework.Rectangle>(frames);
            testSystem.baseScale = 5f;
            testSystem.baseFrameTimer = 90;
            testSystem.particleFrameTimer = 90;
            testSystem.particleMaxVelocity = 3f;
            testSystem.particleMinVelocity = 1f;
            testSystem.particleRotationSpeedMax = (float)Math.PI / 180f;
            testSystem.particleRotationSpeedMin = 0f;
            testSystem.particleStyle = ParticleSystemSource.ParticleStyle.Twister;
            testSystem.spawnArea = new Microsoft.Xna.Framework.Point(30, 30);
            testSystem.scaleMax = 2.5f;
            testSystem.scaleMin = 1.5f;
            testSystem.spawnAngleMax = (float)Math.PI;
            testSystem.spawnAngleMin = 3 / 2 * (float)Math.PI;
            testSystem.spawnPosition = new Microsoft.Xna.Framework.Point(100, 50);
            testSystem.spawnTimeMin = 10;
            testSystem.spawnTimeMax = 20;
            testSystem.wind = -5f;
            testSystem.bFadeOutOverTime = true;
            testSystem.particleScaleModifier = -0.01f;
            testSystem.bHasGravity = false;
            testSystem.bHasWind = false;
            testSystem.radiusMin = 2f;
            testSystem.radiusMax = 8f;
            testSystem.pivot = new Vector2(8);
            testSystem.ReloadTextures();
        }

        private void ParticleEditor_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
        }

        public void Start()
        {
            Show();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(ParticleSystemSource.ParticleStyle)));
            ReloadList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.particleStyle = (ParticleSystemSource.ParticleStyle)comboBox1.SelectedIndex;
            }
        }

        private void ReloadList()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(MapBuilder.gcDB.gameParticleSystems.ToArray());
            listBox1.SelectedIndex = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                ParticleSystemSource temp = testSystem.Clone();
                temp.Name = textBox1.Text;
                MapBuilder.gcDB.AddParticleSystem(temp);
                listBox1.Items.Add(temp);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        private void AssignValues()
        {
            ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
            GravityNumeric.Value = (decimal)temp.gravity;
            comboBox1.SelectedIndex = (int)temp.particleStyle;
            DecayNumeric.Value = (decimal)temp.decayX;
            WindNumeric.Value = (decimal)temp.wind;
            GravEnabler.Checked = temp.bHasGravity;
            WindEnabler.Checked = temp.bHasWind;
            MinScaleNumeric.Value = (decimal)temp.scaleMin;
            MaxScaleNumeric.Value = (decimal)temp.scaleMax;
            MinSANumeric.Value = (decimal)temp.spawnAngleMin;
            MaxSANumeric.Value = (decimal)temp.spawnAngleMax;
            MinRSNumeric.Value = (decimal)temp.particleRotationSpeedMin;
            MaxRSNumeric.Value = (decimal)temp.particleRotationSpeedMax;
            MinVSNumeric.Value = (decimal)temp.particleMinVelocity;
            MaxVSNumeric.Value = (decimal)temp.particleMaxVelocity;
            radiusMinNumeric.Value = (decimal)temp.radiusMin;
            radiusMaxNumeric.Value = (decimal)temp.radiusMax;
            ScaleModifierNumeric.Value = (decimal)temp.particleScaleModifier;
            pivotXNumeric.Value = (decimal)temp.pivot.X;
            pivotYNumeric.Value = (decimal)temp.pivot.Y;
            RandomFrameStartEnabler.Checked = temp.bRandomFrameStart;
            FadeOutEnabler.Checked = temp.bFadeOutOverTime;
            MinLifeNumeric.Value = (decimal)temp.lifeTimeMin;
            MaxLifeNumeric.Value = (decimal)temp.lifeTimeMax;
            MinSpawnNumeric.Value = temp.spawnTimeMin;
            MaxSpawnNumeric.Value = temp.spawnTimeMax;
            MinAreaNumeric.Value = temp.spawnArea.X;
            MaxAreaNumeric.Value = temp.spawnArea.Y;
            ((ParticleSystemSource)listBox1.SelectedItem).ReloadTextures();
            SystemNameTextBox.Text = temp.Name;
            numericUpDown5.Value = (decimal)temp.baseScale;
            numericUpDown6.Value = temp.particleAmountSpawn;
        }

        private void GravityNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.gravity = (float)GravityNumeric.Value;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                AssignValues();

            }
        }

        private void DecayNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.decayX = (float)DecayNumeric.Value;
            }
        }

        private void WindNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.wind = (float)WindNumeric.Value;
            }
        }

        private void GravEnabler_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.bHasGravity = GravEnabler.Checked;
            }
        }

        private void WindEnabler_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.bHasWind = WindEnabler.Checked;
            }
        }

        private void MinScaleNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.scaleMin = (float)MinScaleNumeric.Value;
            }
        }

        private void MaxScaleNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.scaleMax = (float)MaxScaleNumeric.Value;
            }
        }

        private void MinSANumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.spawnAngleMin = (float)MinSANumeric.Value;
            }
        }

        private void MaxSANumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.spawnAngleMax = (float)MaxSANumeric.Value;
            }
        }

        private void MinRSNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.particleRotationSpeedMin = (float)MinRSNumeric.Value;
            }
        }

        private void MaxRSNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.particleRotationSpeedMax = (float)MaxRSNumeric.Value;
            }
        }

        private void MinVSNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.particleMinVelocity = (float)MinVSNumeric.Value;
            }
        }

        private void MaxVSNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.particleMaxVelocity = (float)MaxVSNumeric.Value;
            }
        }

        private void radiusMinNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.radiusMin = (float)radiusMinNumeric.Value;
            }
        }

        private void radiusMaxNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.radiusMax = (float)radiusMaxNumeric.Value;
            }
        }

        private void ScaleModifierNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.particleScaleModifier = (float)ScaleModifierNumeric.Value;
            }
        }

        private void pivotXNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.pivot.X = (int)pivotXNumeric.Value;
            }
        }

        private void pivotYNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.pivot.Y = (float)pivotYNumeric.Value;
            }
        }

        private void RandomFrameStartEnabler_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RandomFrameStartEnabler_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.bRandomFrameStart = RandomFrameStartEnabler.Checked;
            }
        }

        private void FadeOutEnabler_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.bFadeOutOverTime = FadeOutEnabler.Checked;
            }
        }

        private void MinLifeNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.lifeTimeMin = (int)MinLifeNumeric.Value;
            }
        }

        private void MaxLifeNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.lifeTimeMax = (int)MaxLifeNumeric.Value;
            }
        }

        private void MinSpawnNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.spawnTimeMin = (int)MinSpawnNumeric.Value;
            }
        }

        private void MaxSpawnNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.spawnTimeMax = (int)MaxSpawnNumeric.Value;
            }
        }

        private void MinAreaNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.spawnArea.X = (int)MinAreaNumeric.Value;
            }
        }

        private void MaxAreaNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.spawnArea.Y = (int)MaxAreaNumeric.Value;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource ba = (ParticleSystemSource)listBox1.SelectedItem;
                if (ba.particleBaseTexSource.Equals(""))
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

                            ba.particleBaseTexSource = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
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
                    ba.ReloadTextures();
                    FrameSelector.StartComplex(ba.particleBaseTex, ba.particleBaseFrames, (int)frameWidth.Value, (int)frameHeight.Value, (int)xOffSet.Value, (int)yOffSet.Value);
                }
                catch
                {

                    throw;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource ba = (ParticleSystemSource)listBox1.SelectedItem;

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

                        ba.particleBaseTexSource = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
                        Console.WriteLine("Successful item texture selection");
                        bDone = true;
                        ba.particleBaseFrames.Clear();
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
                    ba.ReloadTextures();
                    FrameSelector.StartComplex(ba.particleBaseTex, ba.particleBaseFrames, (int)frameWidth.Value, (int)frameHeight.Value, (int)xOffSet.Value, (int)yOffSet.Value);
                }
                catch
                {

                    throw;
                }

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FrameSelector.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrameSelector.Stop();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource ba = (ParticleSystemSource)listBox1.SelectedItem;
                if (ba.particleTexSource.Equals(""))
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

                            ba.particleTexSource = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
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
                    ba.ReloadTextures();
                    FrameSelector.StartComplex(ba.particleTex, ba.particleFrames, (int)numericUpDown2.Value, (int)numericUpDown1.Value, (int)numericUpDown4.Value, (int)numericUpDown3.Value);
                }
                catch
                {

                    throw;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ParticleSystemSource ba = (ParticleSystemSource)listBox1.SelectedItem;

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

                        ba.particleTexSource = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
                        Console.WriteLine("Successful item texture selection");
                        bDone = true;
                        ba.particleFrames.Clear();
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
                    ba.ReloadTextures();
                    FrameSelector.StartComplex(ba.particleTex, ba.particleFrames, (int)numericUpDown2.Value, (int)numericUpDown1.Value, (int)numericUpDown4.Value, (int)numericUpDown3.Value);
                }
                catch
                {

                    throw;
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                MapBuilder.ShowParticleSystem = !MapBuilder.ShowParticleSystem;
                MapBuilder.selectedParticleSource = (ParticleSystemSource)listBox1.SelectedItem;
            }
        }

        private void SystemNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (!SystemNameTextBox.Text.Equals(""))
                {
                    ((ParticleSystemSource)listBox1.SelectedItem).Name = SystemNameTextBox.Text;
                }
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {

                ((ParticleSystemSource)listBox1.SelectedItem).baseScale = (int)numericUpDown5.Value;

            }
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1) {
                ParticleSystemSource temp = (ParticleSystemSource)listBox1.SelectedItem;
                temp.particleAmountSpawn = (int)numericUpDown6.Value;
            }
        }
    }
}
