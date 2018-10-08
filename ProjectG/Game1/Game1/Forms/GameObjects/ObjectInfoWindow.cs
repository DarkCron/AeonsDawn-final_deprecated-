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
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW.Forms.GameObjects
{
    public partial class ObjectInfoWindow : Form
    {
        public ObjectInfoWindow()
        {
            InitializeComponent();
        }

        object selectedObj;
        private void ObjectInfoWindow_Load(object sender, EventArgs e)
        {
            listBox2.DataSource = null;
            listBox2.DataSource = Enum.GetNames(typeof(BaseCharacter.Rotation));
        }

        public void Start(object obj)
        {
            selectedObj = obj;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;

            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = false;
            listBox1.Enabled = false;

            if (obj.GetType() == (typeof(BaseSprite)))
            {
                var temp = (BaseSprite)selectedObj;
                numericUpDown1.Value = temp.objectIDAddedOnMap;
                numericUpDown2.Value = temp.shapeID;
                textBox1.Text = temp.shapeMapName;
                label1.Text = temp.ToString();
                checkBox1.Checked = temp.bIsVisible;
                checkBox2.Checked = temp.bActivateOnTouch;
                checkBox3.Checked = temp.bIsActive;
                checkBox5.Checked = temp.bHasCollision;
                Show();
                if (temp.scaleVector.X == temp.scaleVector.Y)
                {
                    checkBox4.Checked = true;
                }
                numericUpDown3.Value = (decimal)temp.scaleVector.X;
                numericUpDown4.Value = (decimal)temp.scaleVector.Y;

                if (temp.scaleVector.X == temp.scaleVector.Y)
                {
                    checkBox4.Checked = true;
                }

                numericUpDown4.ReadOnly = checkBox4.Checked;
                listBox1.DataSource = null;
                tabControl1.SelectedIndex = 0;
            }
            else if (selectedObj.GetType() == typeof(ObjectGroup))
            {
                var temp = (ObjectGroup)selectedObj;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = temp.groupID;
                textBox1.Text = temp.groupMapName;
                label1.Text = temp.ToString();
                checkBox1.Checked = temp.bIsVisible;
                checkBox2.Enabled = false;
                checkBox2.Enabled = false;
                checkBox5.Checked = temp.bHasCollision;
                Show();
                if (temp.scaleVector.X == temp.scaleVector.Y)
                {
                    checkBox4.Checked = true;
                }
                numericUpDown3.Value = (decimal)temp.scaleVector.X;
                numericUpDown4.Value = (decimal)temp.scaleVector.Y;

                if (temp.scaleVector.X == temp.scaleVector.Y)
                {
                    checkBox4.Checked = true;
                }

                numericUpDown4.ReadOnly = checkBox4.Checked;
                button1.Enabled = false;

                listBox1.DataSource = null;
                listBox1.DataSource = temp.groupItems;
                button2.Enabled = true;
                listBox1.Enabled = true;
                tabControl1.SelectedIndex = 1;
            }

            if (selectedObj.GetType() == typeof(BaseCharacter))
            {
                button1.Enabled = false;
                Show();
                listBox2.SelectedIndex = (selectedObj as BaseCharacter).rotationIndex;
                tabControl1.SelectedIndex = 2;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (selectedObj.GetType().IsAssignableFrom(typeof(BaseSprite)))
            {
                var temp = (BaseSprite)selectedObj;
                temp.shapeMapName = textBox1.Text;
                label1.Text = temp.ToString();
            }
            else if (selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                var temp = (ObjectGroup)selectedObj;
                temp.groupMapName = textBox1.Text;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (selectedObj.GetType().IsAssignableFrom(typeof(BaseSprite)))
            {
                var temp = (BaseSprite)selectedObj;
                temp.bIsVisible = checkBox1.Checked;
            }
            else if (selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                var temp = (ObjectGroup)selectedObj;
                temp.bIsVisible = checkBox1.Checked;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedObj.GetType().IsAssignableFrom(typeof(BaseSprite)))
            {
                var temp = (BaseSprite)selectedObj;
                temp.bActivateOnTouch = checkBox2.Checked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedObj.GetType().IsAssignableFrom(typeof(BaseSprite)))
            {
                var temp = (BaseSprite)selectedObj;

                DialogResult dialogResult = MessageBox.Show("Would you like to create a -completely- separate script for the object ? If not, every single object with the same script ID on this map will have the newly edited script.", "Script warning", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    if (temp.script == null)
                    {
                        temp.script = new BaseScript();
                    }
                    temp.script = temp.script.Clone();
                    MapBuilder.gcDB.AddScript(temp.script, temp);
                    Form1.MakeSureFormClosed(Form1.scriptForm);
                    Form1.scriptForm = new ScriptBaseForm();
                    Form1.scriptForm.Show();
                    Form1.scriptForm.LoadScript(temp.script);
                }
                else if (dialogResult == DialogResult.No)
                {
                    if (temp.scriptID == -1)
                    {
                        temp.script = new BaseScript();
                        MapBuilder.gcDB.AddScript(temp.script, temp);
                        MapBuilder.parentMap.massChangeScript(temp, temp.script);
                    }
                    Form1.MakeSureFormClosed(Form1.scriptForm);
                    Form1.scriptForm = new ScriptBaseForm();
                    Form1.scriptForm.Show();
                    Form1.scriptForm.LoadScript(temp.script);
                }
            }

        }

        private void checkBox3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedObj.GetType().IsAssignableFrom(typeof(BaseSprite)))
            {
                var temp = (BaseSprite)selectedObj;
                temp.bActivateOnTouch = checkBox3.Checked;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //if (selectedObj.GetType().IsAssignableFrom(typeof(BaseSprite)))
            //{
            //    var temp = (BaseSprite)selectedObj;
            //    checkBox3.Checked = !checkBox3.Checked;
            //}
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            if (selectedObj.GetType().IsAssignableFrom(typeof(BaseSprite)))
            {
                var temp = (BaseSprite)selectedObj;
                //checkBox4.Checked = !checkBox4.Checked;
                numericUpDown4.ReadOnly = checkBox4.Checked;
            }
            else if (selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                var temp = (ObjectGroup)selectedObj;
                numericUpDown4.ReadOnly = checkBox4.Checked;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (selectedObj.GetType().IsAssignableFrom(typeof(BaseSprite)))
            {
                var temp = (BaseSprite)selectedObj;
                if (checkBox4.Checked)
                {
                    temp.scaleVector = new Microsoft.Xna.Framework.Vector2((float)numericUpDown3.Value);
                    numericUpDown4.Value = numericUpDown3.Value;
                    temp.AssignScaleVector(temp.scaleVector);
                }
                else
                {
                    temp.scaleVector.X = (float)numericUpDown3.Value;
                    temp.AssignScaleVector(temp.scaleVector);
                }
            }
            else if (selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                var temp = (ObjectGroup)selectedObj;
                if (checkBox4.Checked)
                {
                    temp.scaleVector = new Microsoft.Xna.Framework.Vector2((float)numericUpDown3.Value);
                    numericUpDown4.Value = numericUpDown3.Value;
                    temp.AssignScaleVector(temp.scaleVector);
                }
                else
                {
                    temp.scaleVector.X = (float)numericUpDown3.Value;
                    temp.AssignScaleVector(temp.scaleVector);
                }
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (selectedObj.GetType().IsAssignableFrom(typeof(BaseSprite)))
            {
                var temp = (BaseSprite)selectedObj;
                if (!checkBox4.Checked)
                {
                    temp.scaleVector.Y = (float)numericUpDown4.Value;
                    temp.AssignScaleVector(temp.scaleVector);
                }

            }
            else if (selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                var temp = (ObjectGroup)selectedObj;
                if (!checkBox4.Checked)
                {
                    temp.scaleVector.Y = (float)numericUpDown4.Value;
                    temp.AssignScaleVector(temp.scaleVector);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedObj.GetType().IsAssignableFrom(typeof(BaseSprite)))
            {
                var temp = (BaseSprite)selectedObj;
                temp.bHasCollision = checkBox5.Checked;
            }
            else if (selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                var temp = (ObjectGroup)selectedObj;
                temp.bHasCollision = checkBox5.Checked;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                if (listBox1.SelectedIndex != 0)
                {
                    var temp = (ObjectGroup)selectedObj;
                    temp.groupItems.RemoveAt(listBox1.SelectedIndex);
                    temp.groupItemsIDs.RemoveAt(listBox1.SelectedIndex);
                    temp.relativeOffSet.RemoveAt(listBox1.SelectedIndex);
                    listBox1.SelectedIndex = -1;
                    listBox1.DataSource = null;
                    listBox1.DataSource = temp.groupItems;
                    temp.bGenerateRender = true;
                }
                else
                {
                    MessageBox.Show("Can't remove the first/base element of the group.");
                }
            }
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (selectedObj.GetType() == typeof(ObjectGroup))
            {
                if (listBox1.SelectedIndex != -1)
                {
                    var temp = listBox1.SelectedItem as BaseSprite;
                    checkBox9.Checked = temp.bIsVisible;
                    checkBox6.Checked = temp.bIsActive;
                    checkBox7.Checked = temp.bHasCollision;
                    checkBox8.Checked = temp.bActivateOnTouch;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedObj.GetType() == typeof(ObjectGroup))
            {
                if (listBox1.SelectedIndex != -1)
                {
                    var temp = (BaseSprite)listBox1.SelectedItem;

                    DialogResult dialogResult = MessageBox.Show("Would you like to create a -completely- separate script for the object ? If not, every single object with the same script ID on this map will have the newly edited script.", "Script warning", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        temp.script = temp.script.Clone();
                        MapBuilder.gcDB.AddScript(temp.script, temp);
                        Form1.MakeSureFormClosed(Form1.scriptForm);
                        Form1.scriptForm = new ScriptBaseForm();
                        Form1.scriptForm.Show();
                        Form1.scriptForm.LoadScript(temp.script);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        if (temp.scriptID == -1)
                        {
                            temp.script = new BaseScript();
                            MapBuilder.gcDB.AddScript(temp.script, temp);
                            MapBuilder.parentMap.massChangeScript(temp, temp.script);
                        }
                        Form1.MakeSureFormClosed(Form1.scriptForm);
                        Form1.scriptForm = new ScriptBaseForm();
                        Form1.scriptForm.Show();
                        Form1.scriptForm.LoadScript(temp.script);
                    }
                }
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                BaseSprite temp = listBox1.SelectedItem as BaseSprite;
                ObjectGroup tempOG = selectedObj as ObjectGroup;
                temp.bIsVisible = checkBox9.Checked;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedObj.GetType() == typeof(BaseCharacter))
            {
                if (MapBuilder.loadedMap.mapSprites.Contains(selectedObj))
                {
                    DialogResult warning = MessageBox.Show("Are you sure? This action is irreversible, for the selected sprite.", "WARNING", MessageBoxButtons.YesNo);
                    if (warning == DialogResult.Yes)
                    {
                        var temp = NPC.ConvertToNPC(selectedObj as BaseCharacter, MapBuilder.loadedMap);
                        Form1.MakeSureFormClosed(Form1.npcEditor);
                        Form1.npcEditor = new NPC_Editor.NPC_Editor();
                        Form1.npcEditor.Start(temp);
                        this.Close();
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                int index = listBox1.SelectedIndex;
                int newIndex = listBox1.SelectedIndex - 1;
                int removeIndex = listBox1.SelectedIndex + 1;
                ObjectGroup temp = selectedObj as ObjectGroup;
                if (newIndex != -1)
                {
                    Microsoft.Xna.Framework.Vector2 os = temp.relativeOffSet[index];
                    int ID = temp.groupItemsIDs[index];
                    BaseSprite copy = temp.groupItems[index];

                    temp.relativeOffSet.Insert(newIndex, os);
                    temp.relativeOffSet.RemoveAt(removeIndex);

                    temp.groupItemsIDs.Insert(newIndex, ID);
                    temp.groupItemsIDs.RemoveAt(removeIndex);

                    temp.groupItems.Insert(newIndex, copy);
                    temp.groupItems.RemoveAt(removeIndex);

                    temp.bGenerateRender = true;
                    listBox1.DataSource = null;
                    listBox1.DataSource = temp.groupItems;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && selectedObj.GetType() == (typeof(ObjectGroup)) && listBox1.SelectedIndex != (selectedObj as ObjectGroup).groupItems.Count - 1)
            {
                int index = listBox1.SelectedIndex;
                int newIndex = listBox1.SelectedIndex + 2;
                int removeIndex = listBox1.SelectedIndex;
                ObjectGroup temp = selectedObj as ObjectGroup;
                if (newIndex != -1)
                {
                    Microsoft.Xna.Framework.Vector2 os = temp.relativeOffSet[index];
                    int ID = temp.groupItemsIDs[index];
                    BaseSprite copy = temp.groupItems[index];

                    temp.relativeOffSet.Insert(newIndex, os);
                    temp.relativeOffSet.RemoveAt(removeIndex);

                    temp.groupItemsIDs.Insert(newIndex, ID);
                    temp.groupItemsIDs.RemoveAt(removeIndex);

                    temp.groupItems.Insert(newIndex, copy);
                    temp.groupItems.RemoveAt(removeIndex);

                    temp.bGenerateRender = true;
                    listBox1.DataSource = null;
                    listBox1.DataSource = temp.groupItems;
                }
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                BaseSprite temp = listBox1.SelectedItem as BaseSprite;
                ObjectGroup tempOG = selectedObj as ObjectGroup;
                temp.bHasCollision = checkBox7.Checked;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                BaseSprite temp = listBox1.SelectedItem as BaseSprite;
                ObjectGroup tempOG = selectedObj as ObjectGroup;
                temp.bIsActive = checkBox6.Checked;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && selectedObj.GetType() == (typeof(ObjectGroup)))
            {
                BaseSprite temp = listBox1.SelectedItem as BaseSprite;
                ObjectGroup tempOG = selectedObj as ObjectGroup;
                temp.bActivateOnTouch = checkBox8.Checked;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 && selectedObj.GetType() == (typeof(BaseCharacter)))
            {
                (selectedObj as BaseCharacter).rotationIndex = listBox2.SelectedIndex;
            }
        }
    }
}
