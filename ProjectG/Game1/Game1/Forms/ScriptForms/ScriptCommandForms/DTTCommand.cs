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
    public partial class DTTCommand : Form
    {
        public DTTCommand()
        {
            InitializeComponent();
        }

        private void DTTCommand_Load(object sender, EventArgs e)
        {

        }

        ScriptBaseForm scriptBaseForm;
        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            Show();
            this.scriptBaseForm = scriptBaseForm;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@DTT" + "_" + numericUpDown1.Value);
            Close();
        }
    }
}
