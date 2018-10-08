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
    public partial class CPPCommand : Form
    {
        public CPPCommand()
        {
            InitializeComponent();
        }

        private void CPPCommand_Load(object sender, EventArgs e)
        {

        }

        ScriptBaseForm scriptBaseForm;
        internal void Start(ScriptBaseForm scriptBaseForm)
        {

            this.scriptBaseForm = scriptBaseForm;
            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@CPP" + "_" + numericUpDown1.Value + "," + numericUpDown2.Value);
            Close();
        }
    }
}
