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
    public partial class THECommand : Form
    {
        public THECommand()
        {
            InitializeComponent();
        }

        ScriptBaseForm scriptBaseForm;
        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            Show();
            this.scriptBaseForm = scriptBaseForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@THE_"+numericUpDown2.Value);
            Close();
        }
    }
}
