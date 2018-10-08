using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBAGW.Forms.General
{
    public partial class ActiveSTATForm : Form
    {
        public ActiveSTATForm()
        {
            InitializeComponent();
        }

        ActiveStatModifier asm;
        public void Start(ActiveStatModifier asm) {
            this.asm = asm;
            listBox3.SelectedIndex = -1;
            listBox3.Items.Clear();
            listBox3.Items.AddRange(asm.ActiveChartNames().ToArray());
            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                asm.activeStatModifier[listBox3.SelectedIndex] = (int)numericUpDown3.Value;
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                numericUpDown3.Value = asm.activeStatModifier[listBox3.SelectedIndex];
            }
        }
    }
}
