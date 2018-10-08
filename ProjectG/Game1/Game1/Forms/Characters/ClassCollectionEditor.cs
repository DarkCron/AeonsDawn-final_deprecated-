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
using TBAGW.Utilities.Characters;

namespace TBAGW.Forms.Characters
{
    public partial class ClassCollectionEditor : Form
    {
        public ClassCollectionEditor()
        {
            InitializeComponent();
        }

        public BaseCharacter selectedBC;

        public void Start(BaseCharacter selectedBC)
        {
            if (selectedBC.CCCidentifier == 0)
            {
                var CCC = new CharacterClassCollection();
                CCC.ReloadFromDatabase(MapBuilder.gcDB);
                CCC.parent = selectedBC;
                MapBuilder.gcDB.AddCCC(CCC);
                selectedBC.CCC = CCC;
                selectedBC.CCCidentifier = CCC.identifier;
            }

            this.selectedBC = selectedBC;
            Text = "Editing character: " + selectedBC.shapeName + ", ID: " + selectedBC.shapeID;
            listBox1.SelectedIndex = -1;
            listBox2.SelectedIndex = -1;
            textBox1.Text = "";
            button1.Enabled = false;
            button2.Enabled = false;
            checkBox1.Checked = false;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(MapBuilder.gcDB.gameClasses.ToArray());
            listBox2.Items.Clear();
            listBox2.Items.AddRange(selectedBC.CCC.charClassList.ToArray());
            Show();
        }

        private void ReloadCharArray()
        {
            listBox2.SelectedIndex = -1;
            listBox2.Items.Clear();
            listBox2.Items.AddRange(selectedBC.CCC.charClassList.ToArray());
        }

        private void ClassCollectionEditor_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameClasses.ToArray());
            }
            else
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameClasses.FindAll(gc => gc.ClassName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedBC.CCC.charClassList.Find(cc => cc.classIdentifier == ((BaseClass)listBox1.SelectedItem).classIdentifier) == default(BaseClass))
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                if (MessageBox.Show("Delete the class? All abilities in the class will be deleted too.", "WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    selectedBC.CCC.charClassList.RemoveAt(listBox2.SelectedIndex);
                    selectedBC.CCC.charClassListIDs.RemoveAt(listBox2.SelectedIndex);
                    button2.Enabled = false;
                }

            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                if (selectedBC.CCC.equippedClass.classIdentifier == ((BaseClass)listBox2.SelectedItem).classIdentifier)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }

                listBox3.DataSource = null;
                listBox3.DataSource = ((BaseClass)listBox2.SelectedItem).classAbilityInfos;
                listBox3.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                listBox5.SelectedIndex = -1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedBC.CCC.charClassList.Add(((BaseClass)listBox1.SelectedItem).Clone());
                selectedBC.CCC.charClassListIDs.Add(((BaseClass)listBox1.SelectedItem).Clone().classIdentifier);
                ReloadCharArray();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                if (checkBox1.Checked)
                {
                    var temp = (BaseClass)listBox2.SelectedItem;
                    selectedBC.CCC.equippedClass = temp.Clone();
                    selectedBC.CCC.equippedClassIdentifier = temp.classIdentifier;
                }
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                var temp = listBox3.SelectedItem as ClassAbilityInfo;
                var tempList = selectedBC.CCC.charClassList.FindAll(c => c.classIdentifier != (listBox2.SelectedItem as BaseClass).classIdentifier);
                listBox4.DataSource = null;
                listBox4.DataSource = tempList;
                GenerateLB5();
                numericUpDown1.Value = temp.reqClassLevel;
            }
            else
            {
                listBox4.DataSource = null;
                listBox5.Items.Clear();
            }
        }

        private void GenerateLB5()
        {
            var temp = listBox3.SelectedItem as ClassAbilityInfo;
            listBox5.Items.Clear();
            listBox5.Items.AddRange(temp.classPoints.ToArray());

            foreach (var item in temp.classPoints)
            {
                foreach (var c in listBox4.Items)
                {
                    if ((c as BaseClass).classIdentifier == item.classID)
                    {
                        listBox5.SelectedItems.Add(c);
                    }
                }
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                button3.Enabled = true;
                button4.Enabled = true;

            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedIndex != -1)
            {
                numericUpDown2.Value = (listBox5.SelectedItem as ClassPoints).points;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedIndex != -1)
            {
                (listBox5.SelectedItem as ClassPoints).points = (int)numericUpDown2.Value;
            }
        }

        private void listBox4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                var temp = listBox3.SelectedItem as ClassAbilityInfo;
                GenerateLB5();

                foreach (var item in temp.classPoints)
                {
                    object t = null;
                    foreach (var cp in listBox5.Items)
                    {
                        if ((cp as ClassPoints).classID == (item as ClassPoints).classID)
                        {
                            t = cp;
                            break;
                        }
                    }
                    if (t == null)
                    {
                        temp.classPoints.Add(new ClassPoints(listBox4.SelectedItem as BaseClass));
                    }

                }

                if (temp.classPoints.Count == 0)
                {
                    temp.classPoints.Add(new ClassPoints(listBox4.SelectedItem as BaseClass));
                }

                GenerateLB5();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                var temp = listBox3.SelectedItem as ClassAbilityInfo;
                GenerateLB5();
                object t = null;
                foreach (var item in temp.classPoints)
                {

                    foreach (var cp in listBox5.Items)
                    {
                        if ((cp as ClassPoints).classID == (item as ClassPoints).classID)
                        {
                            t = cp;
                            break;
                        }
                    }
                }


                if (t != null)
                {
                    listBox5.Items.Remove(t);
                    temp.classPoints.RemoveAll(cp => cp.classID == (t as ClassPoints).classID);
                }

                GenerateLB5();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                var temp = listBox3.SelectedItem as ClassAbilityInfo;
                temp.reqClassLevel = (int)numericUpDown1.Value;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                listBox6.DataSource = null;
                listBox6.DataSource = selectedBC.CCC.allCharacterAbilities();
                listBox7.DataSource = null;
                listBox7.DataSource = selectedBC.CCC.possibleAbilities();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                selectedBC.CCC.abiEquipList.Add(listBox6.SelectedItem as BasicAbility);
                listBox7.DataSource = null;
                listBox7.DataSource = selectedBC.CCC.possibleAbilities();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox7.SelectedIndex != -1)
            {
                selectedBC.CCC.abiEquipList.Remove(listBox7.SelectedItem as BasicAbility);
                listBox7.DataSource = null;
                listBox7.DataSource = selectedBC.CCC.possibleAbilities();
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            TalentGridEditor.Start(selectedBC);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox8.DataSource = null;
            listBox8.DataSource = selectedBC.CCC.baseTalentSlot;
        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                var temp = listBox8.SelectedItem as BaseTalentSlot;
                listBox9.DataSource = null;
                listBox9.DataSource = temp.requiredTalents;
                listBox10.DataSource = null;
                listBox10.DataSource = selectedBC.CCC.baseTalentSlot.FindAll(t => t != listBox8.SelectedItem && !temp.requiredTalents.Contains(t));
                listBox11.DataSource = null;
                listBox11.DataSource = Enum.GetNames(typeof(BaseTalentSlot.TalentType)).ToList().FindAll(tt => !tt.Equals(BaseTalentSlot.TalentType.None.ToString()));
                listBox11.SelectedIndex = (int)temp.talentType - 1;
                textBox2.Text = temp.name.getText();
                richTextBox1.Text = temp.description.getText();
                listBox12.DataSource = null;
                listBox12.DataSource = selectedBC.CCC.charClassList;
                listBox13.DataSource = null;
                listBox13.DataSource = temp.cpReq;
                checkBox2.Checked = temp.bUnlocked;

                if (temp.talentType == BaseTalentSlot.TalentType.ClassUnlock)
                {
                    listBox14.DataSource = null;
                    listBox14.DataSource = selectedBC.CCC.charClassList;
                    listBox14.SelectedItem = selectedBC.CCC.charClassListIDs.IndexOf((temp as ClassUnlockTalent).classID);
                }
                else
                {
                    listBox14.DataSource = null;
                }


            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                if (listBox10.SelectedIndex != -1)
                {
                    var temp = listBox8.SelectedItem as BaseTalentSlot;
                    var tempOther = listBox10.SelectedItem as BaseTalentSlot;
                    temp.requiredTalents.Add(tempOther);
                    temp.requiredTalentIDs.Add(tempOther.ID);

                    listBox9.DataSource = null;
                    listBox9.DataSource = temp.requiredTalents;
                    listBox10.DataSource = null;
                    listBox10.DataSource = selectedBC.CCC.baseTalentSlot.FindAll(t => t != listBox8.SelectedItem && !temp.requiredTalents.Contains(t));

                    TalentGridEditor.talentGrid = new TalentGrid(selectedBC.CCC.getEditorTalentNodesForGrid());
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                if (listBox9.SelectedIndex != -1)
                {
                    var temp = listBox8.SelectedItem as BaseTalentSlot;
                    var tempOther = listBox9.SelectedItem as BaseTalentSlot;
                    temp.requiredTalents.Remove(tempOther);
                    temp.requiredTalentIDs.Remove(tempOther.ID);

                    listBox9.DataSource = null;
                    listBox9.DataSource = temp.requiredTalents;
                    listBox10.DataSource = null;
                    listBox10.DataSource = selectedBC.CCC.baseTalentSlot.FindAll(t => t != listBox8.SelectedItem && !temp.requiredTalents.Contains(t));

                    TalentGridEditor.talentGrid = new TalentGrid(selectedBC.CCC.getEditorTalentNodesForGrid());
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                var temp = listBox8.SelectedItem as BaseTalentSlot;
                temp.name.SetText(textBox2.Text);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                var temp = listBox8.SelectedItem as BaseTalentSlot;
                temp.description.SetText(richTextBox1.Text);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                if (listBox12.SelectedIndex != -1)
                {
                    var temp = listBox8.SelectedItem as BaseTalentSlot;
                    var c = listBox12.SelectedItem as BaseClass;
                    if (temp.cpReq.Find(cp => cp.classID == c.classIdentifier) == null)
                    {
                        temp.AddCPRequiremenent(c);
                    }

                    listBox12.DataSource = null;
                    listBox12.DataSource = selectedBC.CCC.charClassList;
                    listBox13.DataSource = null;
                    listBox13.DataSource = temp.cpReq;
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                if (listBox13.SelectedIndex != -1)
                {
                    var temp = listBox8.SelectedItem as BaseTalentSlot;
                    temp.cpReq.RemoveAt(listBox13.SelectedIndex);

                    listBox12.DataSource = null;
                    listBox12.DataSource = selectedBC.CCC.charClassList;
                    listBox13.DataSource = null;
                    listBox13.DataSource = temp.cpReq;
                }
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                if (listBox13.SelectedIndex != -1)
                {
                    var temp = listBox8.SelectedItem as BaseTalentSlot;
                    var cp = listBox13.SelectedItem as ClassPoints;
                    cp.points = (int)numericUpDown3.Value;
                }
            }
        }

        private void listBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                if (listBox13.SelectedIndex != -1)
                {
                    numericUpDown3.Value = (listBox13.SelectedItem as ClassPoints).points;
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                var temp = listBox8.SelectedItem as BaseTalentSlot;
                temp.bUnlocked = checkBox2.Checked;
            }
        }

        private void listBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                if (listBox14.SelectedIndex != -1)
                {
                    var temp = listBox8.SelectedItem as BaseTalentSlot;
                    var c = listBox14.SelectedItem as BaseClass;
                    if (temp.talentType == BaseTalentSlot.TalentType.ClassUnlock)
                    {
                        (temp as ClassUnlockTalent).classID = c.classIdentifier;
                    }
                }

            }
        }
    }
}
