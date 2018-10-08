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
    public partial class MCTCommand : Form
    {
        public MCTCommand()
        {
            InitializeComponent();
        }

        ScriptBaseForm scriptBaseForm;
        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            listBox1.DataSource = null;
            listBox1.DataSource = MapBuilder.gcDB.gameScriptBools;
            listBox2.DataSource = null;

            this.scriptBaseForm = scriptBaseForm;
            Show();
        }

        private void MCTCommand_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ScriptBool temp = listBox1.SelectedItem as ScriptBool;
                if (temp.choices().Count != 0)
                {
                    scriptBaseForm.AddLine("@MCT" + "_" + temp.boolID);
                    Close();
                }else
                {
                    MessageBox.Show("Soft error! Selected scriptbool contains no choices.");
                }

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ScriptBool temp = listBox1.SelectedItem as ScriptBool;
                listBox2.DataSource = null;
                listBox2.DataSource = temp.choices();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = null;
            listBox1.DataSource = MapBuilder.gcDB.gameScriptBools;
            listBox2.DataSource = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
