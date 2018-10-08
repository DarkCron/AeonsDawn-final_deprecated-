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
    public partial class TPHCommand : Form
    {
        public TPHCommand()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@TPH" + "_" + "/");
            Close();
        }


        ScriptBaseForm scriptBaseForm;
        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            this.scriptBaseForm = scriptBaseForm;
            Show();
        }
    }
}
