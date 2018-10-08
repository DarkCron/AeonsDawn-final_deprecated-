using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Forms.General;

namespace TBAGW.Forms.ItemCreation
{
    public partial class ConsumableEditor : Form
    {
        public ConsumableEditor()
        {
            InitializeComponent();
        }

        private void ConsumableEditor_Load(object sender, EventArgs e)
        {

        }

        private BaseConsumable bc;
        public void Start(BaseConsumable bc)
        {
            this.bc = bc;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(BaseConsumable.TargetRange)));
            comboBox1.SelectedIndex = (int)bc.ConsumableTargetRange;

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(Enum.GetNames(typeof(BaseConsumable.ConsumableType)));
            comboBox2.SelectedIndex = (int)bc.ConsumableTargetRange;

            checkBox1.Checked = bc.bCanTargetEveryone;

            numericUpDown1.Value = bc.itemStackSize;

            numericUpDown2.Value = bc.itemMaxAmount;

            Show();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value == 0)
            {
                numericUpDown2.Value = -1;
            }

            bc.itemMaxAmount = (int)numericUpDown2.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            bc.itemStackSize = (int)numericUpDown1.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bc.ConsumableTargetRange = (BaseConsumable.TargetRange)comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            bc.ConsumableTypeEffect = (BaseConsumable.ConsumableType)comboBox2.SelectedIndex;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
            }

            bc.bCanTargetEveryone = checkBox1.Checked;
        }

        ActiveSTATForm asf = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (asf == null || asf.IsDisposed)
            {
                asf = new ActiveSTATForm();
                asf.Start(bc.ConsumableActiveStatModifier);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            if(asf != null && !asf.IsDisposed) {
                asf.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ModifierEditor me = new ModifierEditor();
            me.Start(bc.ConsumableModifier);
        }
    }
}
