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
using TBAGW.Utilities.Characters;

namespace TBAGW.Forms.General
{
    public partial class STATChartForm : Form
    {
        public static STATChart editingStatChart = new STATChart(true);

        public STATChartForm()
        {
            InitializeComponent();
        }

        public void Start(STATChart start)
        {
            Show();
            editingStatChart = start;
            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            listBox1.Items.AddRange(editingStatChart.PassiveChartNames().ToArray());

            listBox2.Items.Clear();
            listBox2.SelectedIndex = -1;
            listBox2.Items.AddRange(editingStatChart.SpecialChartNames().ToArray());

            listBox3.Items.Clear();
            listBox3.SelectedIndex = -1;
            listBox3.Items.AddRange(editingStatChart.ActiveChartNames().ToArray());

        }

        private void STATChartForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                editingStatChart.currentPassiveStats[listBox1.SelectedIndex] = (int)numericUpDown1.Value;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                numericUpDown1.Value = editingStatChart.currentPassiveStats[listBox1.SelectedIndex];
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                numericUpDown2.Value = editingStatChart.currentSpecialStats[listBox2.SelectedIndex];
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                numericUpDown3.Value = editingStatChart.currentActiveStats[listBox3.SelectedIndex];
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                editingStatChart.currentSpecialStats[listBox2.SelectedIndex] = (int)numericUpDown2.Value;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                editingStatChart.currentActiveStats[listBox3.SelectedIndex] = (int)numericUpDown3.Value;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
