using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TBAGW.Utilities.SriptProcessing;

namespace Game1.Forms.ScriptForms
{
    public partial class ScriptOverview : Form
    {
        BaseScript selectedScript = new BaseScript();
        List<String> scriptLines = new List<string>();

        public ScriptOverview()
        {
            InitializeComponent();
        }

        private void ScriptOverview_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void ProcessScript(BaseScript s)
        {
            if(s!=null) {
                Show();
                selectedScript = s;
                scriptLines = s.scriptContent;
                listBox1.Items.Clear();
                Text = s.ToString();
                foreach (var item in scriptLines)
                {
                    listBox1.Items.Add(item);
                }
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
