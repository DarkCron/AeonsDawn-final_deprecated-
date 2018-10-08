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
    public partial class TGOCommand : Form
    {
        public TGOCommand()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool bHasThatTheTag = scriptBaseForm.script.scriptContent.Find(l => l.Equals("@THE_" + numericUpDown1.Value, StringComparison.OrdinalIgnoreCase)) == default(String) ? false : true;
            if (!bHasThatTheTag)
            {
                scriptBaseForm.AddLine("@THE" + "_" + numericUpDown1.Value);
            }
            scriptBaseForm.AddLine("@TGO" + "_" + numericUpDown1.Value);

            Close();
        }

        ScriptBaseForm scriptBaseForm;
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

        private void TGOCommand_Load(object sender, EventArgs e)
        {

        }
    }
}
