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

namespace TBAGW.Forms.Bools
{
    public partial class ScrptBoolCheckEditor : Form
    {
        public ScrptBoolCheckEditor()
        {
            InitializeComponent();
        }

        ScriptBoolCheck sbc;
        ScriptBool sBool;

        public void Start(ScriptBoolCheck sbc)
        {
            this.sbc = sbc;
            sBool = MapBuilder.gcDB.gameScriptBools.Find(b => b.boolID == sbc.boolID);
            checkBox2.Enabled = true;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(MapBuilder.gcDB.gameScriptBools.ToArray());
            listBox1.SelectedIndex = sbc.boolID;
            Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameScriptBools.ToArray());
            }
            else
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameScriptBools.FindAll(o => o.ToString().IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                sBool = listBox1.SelectedItem as ScriptBool;
                sbc.boolID = ((ScriptBool)listBox1.SelectedItem).boolID;
                switch (sbc.checkType)
                {
                    case ScriptBoolCheck.CheckType.Bool:
                        checkBox1.Checked = sbc.bSameAsSBisOn;
                        checkBox2.Checked = sbc.checkType == ScriptBoolCheck.CheckType.Choice;
                        listBox2.Enabled = sbc.checkType == ScriptBoolCheck.CheckType.Choice;
                        checkBox2.Checked = sbc.checkType == ScriptBoolCheck.CheckType.Choice;

                        break;
                    case ScriptBoolCheck.CheckType.Choice:
                        checkBox2.Checked = sbc.checkType == ScriptBoolCheck.CheckType.Choice;
                        checkBox1.Checked = sbc.bSameAsSBisOn;
                        listBox2.Enabled = sbc.checkType == ScriptBoolCheck.CheckType.Choice;
                        if (sBool != null)
                        {
                            int amountOfChoices = sBool.choiceDescription.Count;
                            if (amountOfChoices != 0)
                            {
                                listBox2.Items.Clear();
                                listBox2.Items.AddRange(sBool.choices().ToArray());

                                var corrTest = sbc.choices.FindAll(c => c >= amountOfChoices);
                                if (corrTest.Count != 0)
                                {
                                    Console.WriteLine("Removed " + corrTest.Count + " unavailable choices");
                                    sbc.choices.RemoveAll(c => corrTest.Contains(c));
                                }

                                for (int i = 0; i < sbc.choices.Count; i++)
                                {
                                    listBox2.SetSelected(sbc.choices[i], true);
                                }
                                if (false)
                                {
                                    sbc.choices.ForEach(c => listBox2.SetSelected(c, true));
                                }

                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                sBool = null;
            }
        }

        private void ScrptBoolCheckEditor_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                sbc.bSameAsSBisOn = checkBox1.Checked;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                sbc.choices.Clear();
                richTextBox2.Text = "";
                foreach (var item in listBox2.SelectedItems)
                {
                    int index = listBox2.Items.IndexOf(item);
                    richTextBox2.Text += sBool.choices()[index] + "\n\n";
                    richTextBox2.Text += sBool.choiceDescription[index] + "\n\n";
                    sbc.choices.Add(listBox2.Items.IndexOf(item));
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (checkBox2.Checked)
                {
                    sbc.checkType = ScriptBoolCheck.CheckType.Choice;
                }
                else
                {
                    sbc.checkType = ScriptBoolCheck.CheckType.Bool;
                }

                if (sbc.checkType == ScriptBoolCheck.CheckType.Choice)
                {
                    checkBox1.Checked = sbc.bSameAsSBisOn;
                    checkBox2.Checked = sbc.checkType == ScriptBoolCheck.CheckType.Choice;
                    listBox2.Enabled = sbc.checkType == ScriptBoolCheck.CheckType.Choice;
                    checkBox2.Checked = sbc.checkType == ScriptBoolCheck.CheckType.Choice;

                    if (sBool != null)
                    {
                        int amountOfChoices = sBool.choiceDescription.Count;
                        if (amountOfChoices != 0)
                        {
                            listBox2.Items.Clear();
                            listBox2.Items.AddRange(sBool.choices().ToArray());

                            var corrTest = sbc.choices.FindAll(c => c >= amountOfChoices);
                            if (corrTest.Count != 0)
                            {
                                Console.WriteLine("Removed " + corrTest.Count + " unavailable choices");
                                sbc.choices.RemoveAll(c => corrTest.Contains(c));
                            }

                            for (int i = 0; i < sbc.choices.Count; i++)
                            {
                                listBox2.SetSelected(sbc.choices[i], true);
                            }
                            //sbc.choices.ForEach(c => listBox2.SetSelected(c, true));
                        }
                    }
                }
                else
                {
                    checkBox2.Checked = false;
                    checkBox1.Checked = sbc.bSameAsSBisOn;
                    listBox2.Enabled = false;
                }
            }
        }
    }
}
