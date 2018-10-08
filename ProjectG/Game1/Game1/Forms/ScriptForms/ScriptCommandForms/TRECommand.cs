using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Game1.Forms.ScriptForms.ScriptCommandForms
{
    public partial class TRECommand : Form
    {
        ScriptBaseForm scriptBaseForm;

        public TRECommand()
        {
            InitializeComponent();
        }

        private void TRECommand_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            foreach (var item in scriptBaseForm.script.scriptContent)
            {
                listBox1.Items.Add(item);
            }
            Show();
            this.scriptBaseForm = scriptBaseForm;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1) {
              //  LI.Text = listBox1.SelectedIndex.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex==-1) {
                LI.Text = "0";
            }

            scriptBaseForm.AddLine("@TRE" + "_" + numericUpDown1.Value);
            Hide();
        }

        private void TRECommand_Enter(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            foreach (var item in scriptBaseForm.script.scriptContent)
            {
                listBox1.Items.Add(item);
            }
        }
    }
}
