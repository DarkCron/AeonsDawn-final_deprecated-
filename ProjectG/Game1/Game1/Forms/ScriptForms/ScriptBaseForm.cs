using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TBAGW;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.SriptProcessing;

namespace Game1.Forms.ScriptForms
{
    public partial class ScriptBaseForm : Form
    {
        public ScriptBaseForm()
        {
            InitializeComponent();
        }

        static public object parentObject = null;
        static public bool bShowError = true;
        public BaseScript script = new BaseScript();
        static int lineIndex = 0;
        CommandSelectForm cmdSelect = new CommandSelectForm();

        public void LoadScript(BaseScript script, Object po = null)
        {
            this.script = script;
           // script.scriptContent.Add(@"@TLH_\LUA\Intro\IntroBattle.lua_start");
            parentObject = po;
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            foreach (var item in script.scriptContent)
            {
                listBox1.Items.Add(item);
            }
        }

        private void ScriptBaseForm_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bShowError)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmdSelect.Show();
            cmdSelect.LoadCMD(this);
            lineIndex = script.scriptContent.Count;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                lineIndex = listBox1.SelectedIndex;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                script.scriptContent.RemoveAt(listBox1.SelectedIndex);
                ReloadScriptContentList();
            }
        }

        public void ReloadScriptContentList()
        {
            listBox1.Items.Clear();
            foreach (var item in script.scriptContent)
            {
                listBox1.Items.Add(item);
            }
            if (!(listBox1.SelectedIndex < script.scriptContent.Count))
            {
                listBox1.SelectedIndex = script.scriptContent.Count - 1;
            }
        }

        public void AddLine(String s)
        {
            if (lineIndex == script.scriptContent.Count)
            {
                script.scriptContent.Add(s);
            }
            else
            {
                script.scriptContent.Insert(lineIndex, s);
            }

            ReloadScriptContentList();
            cmdSelect.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                lineIndex = listBox1.SelectedIndex;
                cmdSelect.Show();
                cmdSelect.LoadCMD(this);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && listBox1.SelectedIndex != script.scriptContent.Count - 1)
            {
                lineIndex = listBox1.SelectedIndex + 1;
                cmdSelect.Show();
                cmdSelect.LoadCMD(this);
            }
            else if (listBox1.SelectedIndex == script.scriptContent.Count - 1)
            {
                button1_Click(sender, e);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MapBuilder.SaveMap();
            ReloadScriptContentList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                AddLine(listBox1.SelectedItem as String);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && listBox1.SelectedIndex != 0)
            {
                var line = listBox1.SelectedItem as String;
                var index = listBox1.SelectedIndex - 1;
                script.scriptContent.RemoveAt(listBox1.SelectedIndex);
                script.scriptContent.Insert(index, line);
                listBox1.Items.Clear();
                foreach (var item in script.scriptContent)
                {
                    listBox1.Items.Add(item);
                }
                listBox1.SelectedIndex = index;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && listBox1.SelectedIndex != script.scriptContent.Count - 1)
            {
                var line = listBox1.SelectedItem as String;
                var index = listBox1.SelectedIndex + 1;
                script.scriptContent.RemoveAt(listBox1.SelectedIndex);
                script.scriptContent.Insert(index, line);
                listBox1.Items.Clear();
                foreach (var item in script.scriptContent)
                {
                    listBox1.Items.Add(item);
                }
                listBox1.SelectedIndex = index;
            }
        }
    }
}
