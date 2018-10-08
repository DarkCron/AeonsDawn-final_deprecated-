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
    public partial class TEXTCommand : Form
    {
        public TEXTCommand()
        {
            InitializeComponent();
        }

        ScriptBaseForm scriptBaseForm;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            this.scriptBaseForm = scriptBaseForm;
            Show();
        }


        private void TEXTCommand_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine(richTextBox1.Text);
            Hide();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
