using Game1.Forms.ScriptForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBAGW.Forms.ScriptForms.ScriptCommandForms
{
    public partial class DLHCommand : Form
    {
        public DLHCommand()
        {
            InitializeComponent();
        }

        ScriptBaseForm sbf;
        public void Start(ScriptBaseForm scriptBaseForm)
        {
            sbf = scriptBaseForm;
            listBox1.Items.Clear();
            listBox1.Items.Add(bool.TrueString);
            listBox1.Items.Add(bool.FalseString);
            listBox2.Items.Clear();
            listBox2.Items.AddRange(listBox1.Items);
            listBox3.Items.Clear();
            listBox3.Items.AddRange(listBox1.Items);
            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    if (listBox1.SelectedIndex != -1)
                    {
                        sbf.AddLine("@DLH" + "_" + listBox1.SelectedItem.ToString());
                    }

                    break;
                case 1:
                    if (listBox2.SelectedIndex != -1 && listBox3.SelectedIndex != -1)
                    {
                         sbf.AddLine("@DLH" + "_" + listBox2.SelectedItem.ToString() + "_" + listBox3.SelectedItem.ToString());
                    }

                    break;
            }
            Hide();
        }

        private void DLHCommand_Load(object sender, EventArgs e)
        {

        }
    }
}
