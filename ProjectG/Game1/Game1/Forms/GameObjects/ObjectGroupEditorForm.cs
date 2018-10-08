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
using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sprite;

namespace Game1.Forms.GameObjects
{
    public partial class ObjectGroupEditorForm : Form
    {
        public ObjectGroupEditorForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            MapBuilder.SaveMap();
        }

        internal void Start()
        {
            tabControl1.SelectedIndex = 0;
            listBox2.SelectedIndex = -1;
            listBox2.SelectedItems.Clear();
            listBox2.Items.Clear();
            listBox3.SelectedIndex = -1;
            listBox3.SelectedItems.Clear();
            listBox3.Items.Clear();


            groupNameInput.Text = "";

            //foreach (var item in MapBuilder.gcDB.gameObjectObjects)
            //{
            //    listBox1.Items.Add(item);
            //}

            foreach (var item in MapBuilder.gcDB.gameObjectGroups)
            {
                listBox2.Items.Add(item);
            }
        }

        public void reloadLBs()
        {
            listBox2.SelectedIndex = -1;
            listBox2.SelectedItems.Clear();
            listBox2.Items.Clear();
            listBox3.SelectedIndex = -1;
            listBox3.SelectedItems.Clear();
            listBox3.Items.Clear();
            foreach (var item in MapBuilder.gcDB.gameObjectGroups)
            {
                listBox2.Items.Add(item);
            }
        }

        private void AddGroupButton_Click(object sender, EventArgs e)
        {
            if (!groupNameInput.Text.Equals("") && MapBuilder.loadedMap.mapObjectGroups.Find(x => x.groupName.Equals(groupNameInput.Text)) == null)
            {
                ObjectGroup temp = new ObjectGroup();
                temp.groupName = groupNameInput.Text;
                MapBuilder.gcDB.AddObjectGroup(temp);
                reloadLBs();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                listBox3.SelectedIndex = -1;
                listBox3.SelectedItems.Clear();
                listBox3.Items.Clear();
                var temp = listBox2.SelectedItem as ObjectGroup;
                foreach (var item in temp.groupItems)
                {
                    listBox3.Items.Add(item);
                }


                textBox1.Text = temp.groupName;
                checkBox2.Checked = temp.bHasCollision;
                checkBox4.Checked = temp.bIsVisible;
            }
        }

        private void ObjectGroupEditorForm_Load(object sender, EventArgs e)
        {

        }


        private void shapeSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupNameInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                if (listBox3.SelectedIndex != -1)
                {
                    ObjectGroup og = listBox2.SelectedItem as ObjectGroup;
                    BaseSprite temp = og.groupItems[listBox3.SelectedIndex];

                  


                    checkBox9.Checked = temp.bIsVisible;
                    checkBox6.Checked = temp.bIsActive;
                    checkBox7.Checked = temp.bHasCollision;
                    checkBox8.Checked = temp.bActivateOnTouch;

                }
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                if (listBox3.SelectedIndex != -1)
                {
                    ObjectGroup og = listBox2.SelectedItem as ObjectGroup;
                    BaseSprite temp = og.groupItems[listBox3.SelectedIndex];
                    temp.bActivateOnTouch = checkBox8.Checked;
                }
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                if (listBox3.SelectedIndex != -1)
                {
                    ObjectGroup og = listBox2.SelectedItem as ObjectGroup;
                    BaseSprite temp = og.groupItems[listBox3.SelectedIndex];
                    temp.bIsVisible = checkBox9.Checked;
                }
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                if (listBox3.SelectedIndex != -1)
                {
                    ObjectGroup og = listBox2.SelectedItem as ObjectGroup;
                    BaseSprite temp = og.groupItems[listBox3.SelectedIndex];
                    temp.bHasCollision = checkBox7.Checked;
                }
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                if (listBox3.SelectedIndex != -1)
                {
                    ObjectGroup og = listBox2.SelectedItem as ObjectGroup;
                    BaseSprite temp = og.groupItems[listBox3.SelectedIndex];
                    temp.bIsActive = checkBox6.Checked;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                ObjectGroup og = listBox2.SelectedItem as ObjectGroup;
                og.groupName = textBox1.Text;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                ObjectGroup og = listBox2.SelectedItem as ObjectGroup;
                og.bHasCollision = checkBox2.Checked;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                ObjectGroup og = listBox2.SelectedItem as ObjectGroup;
                og.bIsVisible = checkBox4.Checked;
            }
        }
    }
}
