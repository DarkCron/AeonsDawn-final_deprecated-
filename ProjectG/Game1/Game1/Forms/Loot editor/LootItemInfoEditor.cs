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

namespace TBAGW.Forms.Loot_editor
{
    public partial class LootItemInfoEditor : Form
    {
        public LootItemInfoEditor()
        {
            InitializeComponent();
        }

        private void LootItemInfoEditor_Load(object sender, EventArgs e)
        {

        }

        ItemLootInfo selectedILI;
        bool bCanDropMoreThanOne = false;
        public void Start(ItemLootInfo ili)
        {
            Text = ili.ToString();
            selectedILI = ili;
            bCanDropMoreThanOne = ili.bItemIsStackable;

            numericUpDown1.Value = (decimal)ili.chanceToDrop;

            if (bCanDropMoreThanOne)
            {
                numericUpDown2.Value = ili.minDrop;
                numericUpDown3.Value = ili.maxDrop;
            }

            checkBox1.Checked = selectedILI.bIsRare;

            listBox1.DataSource = MapBuilder.gcDB.gameScriptBools;

            numericUpDown4.Value = ili.boolID;

            checkBox2.Checked = selectedILI.dropStackChanceStack;

            Show();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (bCanDropMoreThanOne)
            {
                selectedILI.minDrop = (int)numericUpDown2.Value;
                if (numericUpDown2.Value > numericUpDown3.Value && numericUpDown3.Value != 0)
                {
                    numericUpDown3.Value = numericUpDown2.Value;
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            selectedILI.chanceToDrop = (float)numericUpDown1.Value;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            selectedILI.bIsRare = !selectedILI.bIsRare;
            checkBox1.Checked = selectedILI.bIsRare;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.DataSource = null;
                listBox1.DataSource = (MapBuilder.gcDB.gameScriptBools);
            }
            else if (!textBox1.Text.Equals("", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.DataSource = null;
                listBox1.DataSource = (MapBuilder.gcDB.gameScriptBools.FindAll(i => i.boolName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                richTextBox1.Text = (listBox1.SelectedItem as ScriptBool).boolDescription;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                numericUpDown4.Value = (listBox1.SelectedItem as ScriptBool).boolID;
            }
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            selectedILI.dropStackChanceStack = !selectedILI.dropStackChanceStack;
            checkBox2.Checked = selectedILI.dropStackChanceStack;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (bCanDropMoreThanOne)
            {
                selectedILI.maxDrop = (int)numericUpDown3.Value;

            }
        }
    }
}
