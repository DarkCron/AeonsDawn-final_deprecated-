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

namespace TBAGW.Forms.GameClasses
{
    public partial class GameClassCreator : Form
    {
        public BaseClass selectedClass = null;
        public BasicAbility selectedAbility = null;

        public GameClassCreator()
        {
            InitializeComponent();
        }

        public void Start()
        {
            selectedClass = null;
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(MapBuilder.gcDB.gameClasses.ToArray());

            Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameClasses.ToArray());
            }
            else
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameClasses.FindAll(gc => gc.ClassName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
            }
        }

        public void ReloadClassList()
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameClasses.ToArray());
            }
            else
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameClasses.FindAll(gc => gc.ClassName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateClassForm ccf = new CreateClassForm();
            ccf.Start(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedClass != null && button2.Enabled)
            {
                selectedClass.ClassName = textBox2.Text;
                ReloadClassList();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(""))
            {
                button2.Enabled = false;
            }
            else
            {
                if (MapBuilder.gcDB.gameClasses.Find(gc => gc.ClassName.Equals(textBox2.Text)) == default(BaseClass))
                {
                    button2.Enabled = true;
                }
                else
                {
                    button2.Enabled = false;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
            if (listBox1.SelectedIndex != -1)
            {
                selectedClass = (BaseClass)listBox1.SelectedItem;
                comboBox1.SelectedIndex = (int)selectedClass.classType;
                numericUpDown1.Value = selectedClass.baseThreatClass;
                comboBox2.SelectedIndex = (int)selectedClass.classTypeArmour;
                comboBox3.SelectedIndex = (int)selectedClass.classAffinity;

                listBox2.Items.Clear();
                listBox2.Items.AddRange(selectedClass.classAbilities.ToArray());
                listBox3.Items.Clear();
                textBox3.Text = selectedClass.functionName;
            }
        }

        private void GameClassCreator_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.DataSource = Enum.GetNames(typeof(BaseClass.CLASSType)).ToArray();
            comboBox2.Items.Clear();
            comboBox2.DataSource = Enum.GetNames(typeof(BaseEquipment.ARMOUR_TYPES)).ToArray();
            comboBox3.Items.Clear();
            comboBox3.DataSource = Enum.GetNames(typeof(BasicAbility.ABILITY_AFFINITY)).ToArray();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedClass.classType = (BaseClass.CLASSType)comboBox1.SelectedIndex;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedClass.baseThreatClass = (int)numericUpDown1.Value;
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedClass.classTypeArmour = (BaseEquipment.ARMOUR_TYPES)comboBox2.SelectedIndex;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 && selectedClass != null && listBox1.SelectedIndex != -1)
            {
                selectedAbility = (BasicAbility)listBox2.SelectedItem;
                listBox3.Items.Clear();
                MapBuilder.gcDB.gameClasses.FindAll(sa => sa.classAbilitiesIDs.Contains(selectedAbility.abilityIdentifier));

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                AbilitySelector abis = new AbilitySelector();
                abis.Start(selectedClass);
            }


        }

        ExperienceLayoutEditor ele = new ExperienceLayoutEditor();
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                MakeSureClosed(ele);
                ele = new ExperienceLayoutEditor();
                ele.Start(selectedClass);
            }
        }

        private void MakeSureClosed(Form f)
        {
            if (!f.IsDisposed)
            {
                f.Close();
                f.Dispose();
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedClass.functionName = textBox3.Text;
            }
        }

        private void button6_Click(object sender, EventArgs e)
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
                    selectedClass.scriptLoc = fi.Replace(ofd.InitialDirectory, "");
                    selectedClass.CheckScript();
                }
                else if (System.Windows.Forms.DialogResult.Cancel == dia)
                {

                    System.Windows.Forms.MessageBox.Show("Cancelled, returning to Editor.");
                }

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedClass.scriptLoc = "";
                selectedClass.CheckScript();
            }
        }

        ClassLevelUpLayout clul = new ClassLevelUpLayout();
        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                Form1.MakeSureFormClosed(clul);
                clul = new ClassLevelUpLayout();
                clul.Start(selectedClass);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && listBox3.SelectedIndex != -1)
            {
                selectedClass.classAffinity = (BasicAbility.ABILITY_AFFINITY)listBox3.SelectedIndex;
            }
        }

        private void splitContainer3_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
