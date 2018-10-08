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
    public partial class TLBCommand : Form
    {
        public TLBCommand()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@TLB" + "_" + numericUpDown1.Value.ToString() + "_" + numericUpDown2.Value.ToString());
            Close();
        }


        ScriptBaseForm scriptBaseForm;
        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            this.scriptBaseForm = scriptBaseForm;
            Show();
        }


        private void TLBCommand_Load(object sender, EventArgs e)
        {

        }
    }
}
