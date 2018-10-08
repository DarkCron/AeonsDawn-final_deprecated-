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
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW.Forms.ScriptForms.ScriptCommandForms
{
    public partial class CSBCommand : Form
    {
        public CSBCommand()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.SelectedIndex = -1;
        }

        ScriptBaseForm scriptBaseForm;
        internal void Start(ScriptBaseForm scriptBaseForm)
        {

            this.scriptBaseForm = scriptBaseForm;
            listBox1.DataSource = MapBuilder.gcDB.gameScriptBools;
            Show();
        }

        private void CSBCommand_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                
                ScriptBool sb = (ScriptBool)listBox1.SelectedItem;
                scriptBaseForm.AddLine("@CSB" + "_" + sb.boolID + "_" + listBox2.SelectedIndex + "_" + checkBox1.Checked.ToString() + "_" + ScriptProcessor.TIFTypes.Bool.ToString());
                Close();
            }
        }
    }
}
