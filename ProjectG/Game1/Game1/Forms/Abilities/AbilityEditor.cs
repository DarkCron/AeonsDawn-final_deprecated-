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
using TBAGW.Forms.General;
using TBAGW.Forms.Particle_Animation;
using TBAGW.Scenes.Editor;

namespace TBAGW.Forms.Abilities
{
    public partial class AbilityEditor : Form
    {
        static public BasicAbility selectedAbility = null;

        public AbilityEditor()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void Start()
        {
            Show();
            textBox1.Text = "";
            ResetAbilityList();
            selectedAbility = null;
        }

        private void ResetAbilityList()
        {
            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            listBox1.Items.AddRange(MapBuilder.gcDB.gameAbilities.ToArray());
        }

        private void AbilityEditor_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(Enum.GetNames(typeof(BasicAbility.ABILITY_TYPE)));
            comboBox2.Items.AddRange(Enum.GetNames(typeof(BasicAbility.ABILITY_CAST_TYPE)));
            comboBox3.Items.AddRange(Enum.GetNames(typeof(BaseClass.CLASSType)));
            comboBox4.Items.AddRange(Enum.GetNames(typeof(BasicAbility.ABILITY_AFFINITY)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool bThisAbilityIsOkToAdd = true;
            if (!textBox3.Text.Equals(""))
            {
                if (MapBuilder.gcDB.gameAbilities.Find(a => a.abilityName.Equals(textBox3.Text)) != null)
                {
                    bThisAbilityIsOkToAdd = false;
                    MessageBox.Show("Please create only abilities with UNIQUE names, ability creation cancelled.");
                }
            }
            if (!textBox3.Text.Equals("") && bThisAbilityIsOkToAdd)
            {
                BasicAbility newAbility = new BasicAbility();
                newAbility.AbilityHitChance = 80;
                newAbility.CreateAbility();
                newAbility.abilityName = textBox3.Text;
                selectedAbility = newAbility;
                MapBuilder.gcDB.AddAbility(selectedAbility);
                // MapBuilder.gcDB.gameAbilities.Add(selectedAbility);
                //AssignValues();
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameAbilities.ToArray());
                textBox3.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                STATChartForm statEditor = new STATChartForm();
                statEditor.Start(selectedAbility.abilityModifier);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility = (BasicAbility)listBox1.SelectedItem;
                AssignValues();
            }
        }

        private void AssignValues()
        {
            textBox4.Text = selectedAbility.modFunctionName;
            numericUpDown5.Value = selectedAbility.AbilityManaCost;
            numericUpDown4.Value = selectedAbility.AbilityAPCost;
            numericUpDown6.Value = selectedAbility.AbilityHitChance;
            numericUpDown7.Value = selectedAbility.AbilityCritChance;
            richTextBox1.Text = selectedAbility.abilityDescription;

            checkBox2.Checked = selectedAbility.bIsAOE;
            numericUpDown11.Value = selectedAbility.baseDamage;
            numericUpDown10.Value = ((BasicAbility)listBox1.SelectedItem).abilityIcon.frameInterval;
            textBox2.Text = selectedAbility.scriptFunction;
            listBox3.Items.Clear();
            numericUpDown1.Value = selectedAbility.abilityMinRange;
            numericUpDown2.Value = selectedAbility.abilityMaxRange;
            textBox1.Text = selectedAbility.abilityName;
            numericUpDown3.Value = selectedAbility.abilityCooldown;
            comboBox1.SelectedIndex = (int)selectedAbility.abilityType;
            comboBox2.SelectedIndex = (int)selectedAbility.abilityFightStyle;
            foreach (var item in selectedAbility.targetableTypes)
            {
                listBox3.Items.Add(item);
            }
            checkBox1.Checked = selectedAbility.bCanBeAIAbility;
            numericUpDown8.Value = selectedAbility.castChance;
            numericUpDown9.Value = selectedAbility.minZoneLevel;
            if (!checkBox1.Checked)
            {
                comboBox3.Visible = false;
                numericUpDown8.Visible = false;
                numericUpDown9.Visible = false;
                listBox3.Visible = false;
                button8.Visible = false;
                button9.Visible = false;
            }
            else
            {
                comboBox3.Visible = true;
                numericUpDown8.Visible = true;
                numericUpDown9.Visible = true;
                listBox3.Visible = true;
                button8.Visible = true;
                button9.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedAbility != null)
            {
                selectedAbility.EditorAddLevel();
                AssignValues();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (selectedAbility != null && !textBox1.Text.Equals(""))
            {
                selectedAbility.abilityName = textBox1.Text;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedAbility != null && selectedAbility.maxAbilityLevel > 1)
            {
                selectedAbility.EditorRemoveLevel();
                AssignValues();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ModifierEditor form = new ModifierEditor();
                form.Start(selectedAbility.statModifier);

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ModifierEditor form = new ModifierEditor();
                form.Start(selectedAbility.EnemyStatModifier);
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (selectedAbility != null)
            {
                selectedAbility.abilityCooldown = (int)numericUpDown3.Value;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedAbility != null)
            {
                selectedAbility.abilityType = comboBox1.SelectedIndex;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedAbility != null)
            {
                selectedAbility.abilityFightStyle = comboBox2.SelectedIndex;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != 1)
            {
                MapBuilder.gcDB.gameAbilities.Remove((BasicAbility)listBox1.SelectedItem);
                listBox1.Items.Clear();
                listBox1.SelectedIndex = -1;
                listBox1.Items.AddRange(MapBuilder.gcDB.gameAbilities.ToArray());
                richTextBox1.Text = selectedAbility.abilityDescription;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (selectedAbility != null)
            {
                selectedAbility.abilityMinRange = (int)numericUpDown1.Value;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (selectedAbility != null)
            {
                selectedAbility.abilityMaxRange = (int)numericUpDown2.Value;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                numericUpDown5.Value = selectedAbility.AbilityManaCost;
                numericUpDown4.Value = selectedAbility.AbilityAPCost;
                numericUpDown6.Value = selectedAbility.AbilityHitChance;
                numericUpDown7.Value = selectedAbility.AbilityCritChance;
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.AbilityManaCost = (int)numericUpDown5.Value;
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.AbilityAPCost = (int)numericUpDown4.Value;
            }
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.AbilityHitChance = (int)numericUpDown6.Value;
            }
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.AbilityCritChance = (int)numericUpDown7.Value;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            selectedAbility.bCanBeAIAbility = checkBox1.Checked;

            if (!checkBox1.Checked)
            {
                comboBox3.Visible = false;
                numericUpDown8.Visible = false;
                numericUpDown9.Visible = false;
                listBox3.Visible = false;
                button8.Visible = false;
                button9.Visible = false;
            }
            else
            {
                comboBox3.Visible = true;
                numericUpDown8.Visible = true;
                numericUpDown9.Visible = true;
                listBox3.Visible = true;
                button8.Visible = true;
                button9.Visible = true;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && !selectedAbility.targetableTypes.Contains((BaseClass.CLASSType)comboBox3.SelectedIndex))
            {
                selectedAbility.targetableTypes.Add((BaseClass.CLASSType)comboBox3.SelectedIndex);
                listBox3.Items.Add((BaseClass.CLASSType)comboBox3.SelectedIndex);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && selectedAbility.targetableTypes.Contains((BaseClass.CLASSType)comboBox3.SelectedIndex))
            {
                selectedAbility.targetableTypes.Remove((BaseClass.CLASSType)comboBox3.SelectedIndex);
                listBox3.Items.Remove((BaseClass.CLASSType)comboBox3.SelectedIndex);
            }
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.castChance = (int)numericUpDown8.Value;
            }
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.minZoneLevel = (int)numericUpDown9.Value;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!richTextBox1.Text.Equals(" "))
            {
                selectedAbility.abilityDescription = richTextBox1.Text;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                BasicAbility ba = (BasicAbility)listBox1.SelectedItem;
                if (ba.abilityIcon.texFileLoc.Equals(""))
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

                            ba.abilityIcon.texFileLoc = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
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
                    ba.ReloadTexture();
                    FrameSelector.StartComplex(ba.abilityIcon, (int)frameWidth.Value, (int)frameHeight.Value, (int)xOffSet.Value, (int)yOffSet.Value);
                }
                catch
                {

                    throw;
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                BasicAbility ba = (BasicAbility)listBox1.SelectedItem;

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

                        ba.abilityIcon.texFileLoc = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
                        Console.WriteLine("Successful item texture selection");
                        bDone = true;
                        ba.abilityIcon.animationFrames.Clear();
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
                    ba.ReloadTexture();
                    FrameSelector.StartComplex(ba.abilityIcon, (int)frameWidth.Value, (int)frameHeight.Value, (int)xOffSet.Value, (int)yOffSet.Value);
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

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            selectedAbility.abilityIcon.frameInterval = (int)numericUpDown10.Value;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //if (listBox2.SelectedIndex != -1 && selectedAbility.bIsAOE == true)
            //{
            //    AbilityAOEEditor aaoee = new AbilityAOEEditor();
            //    aaoee.Start(selectedAbility);
            //}
        }

        private void xOffSet_ValueChanged(object sender, EventArgs e)
        {

        }

        PASelection pas = new PASelection();
        private void button14_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                Form1.MakeSureFormClosed(pas);
                pas = new PASelection();
                pas.Start(AssignPAanim);
            }
        }

        private void AssignPAanim(ParticleAnimation pa)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem as BasicAbility;
                temp.abilityPAID = pa.particleAnimationID;
                temp.PAanim = pa.Clone();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedAbility != null)
            {
                selectedAbility.affinity = (BasicAbility.ABILITY_AFFINITY)comboBox4.SelectedIndex;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Lua file|*.lua";
                ofd.InitialDirectory = TBAGW.Game1.rootContent;
                ofd.Title = "open Lua";
                DialogResult dia = ofd.ShowDialog();
                if (DialogResult.OK == dia && ofd.FileName.Contains(ofd.InitialDirectory))
                {
                    String fi = ofd.FileName;
                    selectedAbility.scriptLoc = fi.Replace(ofd.InitialDirectory, "");
                    selectedAbility.CheckScript();

                    FillLuaBox();
                }
                else if (System.Windows.Forms.DialogResult.Cancel == dia)
                {

                    System.Windows.Forms.MessageBox.Show("Cancelled, returning to Editor.");
                }

            }
        }

        private void FillLuaBox()
        {
            var s = selectedAbility.script;
            richTextBox2.Text = "";
            if (s != null)
            {
                try
                {   // Open the text file using a stream reader.
                    using (StreamReader sr = new StreamReader(TBAGW.Game1.rootContent + selectedAbility.scriptLoc))
                    {
                        // Read the stream to a string, and write the string to the console.
                        String line = sr.ReadToEnd();
                        richTextBox2.Text = line;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.scriptLoc = "";
                selectedAbility.scriptFunction = "";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.scriptFunction = textBox2.Text;
            }
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.baseDamage = (int)numericUpDown11.Value;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.bIsAOE = checkBox2.Checked;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals(""))
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameAbilities.ToArray());
            }
            else
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameAbilities.FindAll(abi => abi.abilityName.IndexOf(textBox3.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.abilityPAID = -1;
                selectedAbility.PAanim = null;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedAbility.modFunctionName = textBox4.Text;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                BasicAbility ba = (BasicAbility)listBox1.SelectedItem;

                MessageBox.Show("Please select sound file.");
                OpenFileDialog openTex = new OpenFileDialog();
                openTex.Title = "Open sound file";

                openTex.Filter = "Sound File|*.wav;*.wma;*.ogg;*.xnb";
                openTex.InitialDirectory = Game1.rootContent;


                bool bDone = false;

                while (!bDone)
                {
                    DialogResult dia = openTex.ShowDialog();
                    if (dia == DialogResult.OK && openTex.FileName.Contains(openTex.InitialDirectory))
                    {

                        ba.soundEffectLoc = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
                        Console.WriteLine("Successful item texture selection");
                        bDone = true;
                    }
                    else if (dia == DialogResult.Cancel)
                    {
                        bDone = true;
                    }
                    else if (!openTex.FileName.Contains(openTex.InitialDirectory))
                    {
                        MessageBox.Show(@"Please select a file within the application folder under Content\Mods and it's subfolders");
                    }

                }


            }
        }
    }
}
