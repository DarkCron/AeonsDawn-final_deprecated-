using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TBAGW.Scenes.Editor;

namespace Game1.Forms.ScriptForms.ScriptCommandForms
{
    public partial class TSPCommand : Form
    {
        ScriptBaseForm scriptBaseForm;

        public TSPCommand()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            Show();
            this.scriptBaseForm = scriptBaseForm;

            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            foreach (var item in MapBuilder.parentMap.mapTeams)
            {
                listBox1.Items.Add(item);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1) {
                TI.Text = MapBuilder.parentMap.mapTeams[listBox1.SelectedIndex].teamIdentifier.ToString();
                listBox2.SelectedIndex = -1;
                listBox2.Items.Clear();
                foreach (var item in MapBuilder.parentMap.mapTeams[listBox1.SelectedIndex].teamMembers)
                {
                    listBox2.Items.Add(item);
                }
            }
        }

        private void TSPCommand_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            foreach (var item in MapBuilder.parentMap.mapTeams)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                TI.Text = "0";
            }

            scriptBaseForm.AddLine("@TSP" + "_" + TI.Text);
            Hide();
        }

        private void TSPCommand_Enter(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            foreach (var item in MapBuilder.parentMap.mapTeams)
            {
                listBox1.Items.Add(item);
            }
        }
    }
}
