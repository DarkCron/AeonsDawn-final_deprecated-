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
    public partial class AbilitySelector : Form
    {
        public AbilitySelector()
        {
            InitializeComponent();
        }

        BaseClass selectedClass;
        BasicAbility selectedAbility;

        public void Start(BaseClass selectedClass)
        {
            textBox1.Text = "";
            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            button1.Enabled = false;
            button2.Enabled = false;

            listBox1.Items.AddRange(MapBuilder.gcDB.gameAbilities.ToArray());
            this.selectedClass = selectedClass;

            Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameAbilities.ToArray());
            }
            else
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameAbilities.FindAll(gc => gc.abilityName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
            }
        }

        private void AbilitySelector_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectedClass.AddAbility(selectedAbility);
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex !=-1)
            {
                selectedAbility = (BasicAbility)listBox1.SelectedItem;
                if(selectedClass.classAbilities.Find(ba=>ba.abilityIdentifier==selectedAbility.abilityIdentifier)==default(BasicAbility))
                {
                    button1.Enabled = true;
                }else {
                    button1.Enabled = false;
                }

                button2.Enabled = true;
            }else {
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BasicAbility tempClone = selectedAbility.Clone();
            MapBuilder.gcDB.AddAbility(tempClone);
            selectedClass.AddAbility(tempClone);
            Close();
        }
    }
}
