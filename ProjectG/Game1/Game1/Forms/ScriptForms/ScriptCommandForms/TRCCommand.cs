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
    public partial class TRCCommand : Form
    {
        public TRCCommand()
        {
            InitializeComponent();
        }

        private void TRCCommand_Load(object sender, EventArgs e)
        {

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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            LI.Text = numericUpDown1.Value.ToString();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            label6.Text = numericUpDown2.Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool bHasThatTheTag = scriptBaseForm.script.scriptContent.Find(l => l.Equals("@THE_" + numericUpDown2.Value, StringComparison.OrdinalIgnoreCase)) == default(String) ? false : true;
            if (!bHasThatTheTag)
            {
                scriptBaseForm.AddLine("@THE_" + numericUpDown2.Value);
            }
            scriptBaseForm.AddLine("@TRC" + "_" + numericUpDown1.Value + "_" + numericUpDown2.Value);

            Close();
        }
    }
}
