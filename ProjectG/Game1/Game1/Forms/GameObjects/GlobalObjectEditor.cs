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
using TBAGW.Utilities.Sprite;

namespace TBAGW.Forms.GameObjects
{
    public partial class GlobalObjectEditor : Form
    {
        public GlobalObjectEditor()
        {
            InitializeComponent();
        }

        private void GlobalObjectEditor_Load(object sender, EventArgs e)
        {

        }

        public void Start()
        {
            listBox1.Items.Clear();
            listBox1.SelectedItem = -1;

            var temp = MapBuilder.gcDB.gameObjectObjects.GroupBy(shape => shape.shapeID).Select(grp => grp.First()).ToList();
            //List<BaseSprite> lsprites = MapBuilder.gcDB.gameObjectObjects;
            temp.AddRange(MapBuilder.gcDB.gameCharacters);
            listBox1.DataSource = temp.ToList();
            Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                int totalAmountSameIDs = MapBuilder.gcDB.gameObjectObjects.FindAll(shape => shape.shapeID == ((BaseSprite)listBox1.SelectedItem).shapeID).Count;

                int totalAmountSameIDWithCollision = MapBuilder.gcDB.gameObjectObjects.FindAll(shape => shape.shapeID == ((BaseSprite)listBox1.SelectedItem).shapeID && shape.bHasCollision).Count;
                collision.Text = "Out of " + totalAmountSameIDs + " objects, " + totalAmountSameIDWithCollision + " have collision " + (totalAmountSameIDs - totalAmountSameIDWithCollision) + " do not.";

                int totalAmountSameIDWithWind = MapBuilder.gcDB.gameObjectObjects.FindAll(shape => shape.shapeID == ((BaseSprite)listBox1.SelectedItem).shapeID && shape.bIsAffectedByWind).Count;
                wind.Text = "Out of " + totalAmountSameIDs + " objects, " + totalAmountSameIDWithWind + " are affected by wind " + (totalAmountSameIDs - totalAmountSameIDWithWind) + " are not.";

                int totalAmountSameIDWithShadow = MapBuilder.gcDB.gameObjectObjects.FindAll(shape => shape.shapeID == ((BaseSprite)listBox1.SelectedItem).shapeID && shape.bHasShadow).Count;
                shadow.Text = "Out of " + totalAmountSameIDs + " objects, " + totalAmountSameIDWithShadow + " have a shadow " + (totalAmountSameIDs - totalAmountSameIDWithShadow) + " do not.";

                //checkBox4.Checked = false;
                //checkBox5.Checked = false;
                //checkBox6.Checked = false;
                //listBox2.Items.Clear();
                var temp = ((BaseSprite)listBox1.SelectedItem);
                listBox2.SelectedIndex = -1;
                listBox2.DataSource = null;
                listBox2.DataSource = ((BaseSprite)listBox1.SelectedItem).baseAnimations;

                numericUpDown1.Value = temp.spriteGameSize.Width;
                numericUpDown2.Value = temp.spriteGameSize.Height;
            }
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (checkBox4.Checked)
                {
                    MapBuilder.gcDB.gameObjectObjects.FindAll(shape => shape.shapeID == ((BaseSprite)listBox1.SelectedItem).shapeID).ForEach(s => s.bHasCollision = true);
                    checkBox4.Checked = true;
                }
                else
                {
                    MapBuilder.gcDB.gameObjectObjects.FindAll(shape => shape.shapeID == ((BaseSprite)listBox1.SelectedItem).shapeID).ForEach(s => s.bHasCollision = false);
                    checkBox4.Checked = false;
                }

                listBox1_SelectedIndexChanged(sender, e);
            }
        }

        private void checkBox5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (checkBox5.Checked)
                {
                    MapBuilder.gcDB.gameObjectObjects.FindAll(shape => shape.shapeID == ((BaseSprite)listBox1.SelectedItem).shapeID).ForEach(s => s.bIsAffectedByWind = true);
                    checkBox5.Checked = true;
                }
                else
                {
                    MapBuilder.gcDB.gameObjectObjects.FindAll(shape => shape.shapeID == ((BaseSprite)listBox1.SelectedItem).shapeID).ForEach(s => s.bIsAffectedByWind = false);
                    checkBox5.Checked = false;
                }

                listBox1_SelectedIndexChanged(sender, e);
            }
        }

        private void checkBox6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (checkBox6.Checked)
                {
                    MapBuilder.gcDB.gameObjectObjects.FindAll(shape => shape.shapeID == ((BaseSprite)listBox1.SelectedItem).shapeID).ForEach(s => s.bHasShadow = true);
                    checkBox6.Checked = true;
                }
                else
                {
                    MapBuilder.gcDB.gameObjectObjects.FindAll(shape => shape.shapeID == ((BaseSprite)listBox1.SelectedItem).shapeID).ForEach(s => s.bHasShadow = false);
                    checkBox6.Checked = false;
                }

                listBox1_SelectedIndexChanged(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                TBAGW.Forms.Animation.AnimationEditor ae = new TBAGW.Forms.Animation.AnimationEditor();
                ae.Start((ShapeAnimation)listBox2.SelectedItem);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem as BaseSprite;
                temp.baseAnimations.Add(new ShapeAnimation());
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = ((BaseSprite)listBox1.SelectedItem);
                temp.spriteGameSize.Width = (int)numericUpDown1.Value;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = ((BaseSprite)listBox1.SelectedItem);
                temp.spriteGameSize.Height = (int)numericUpDown2.Value;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MapBuilder.gcDB.ReloadObjectGroups();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                HitboxEditor.Start((listBox1.SelectedItem as BaseSprite), (listBox1.SelectedItem as BaseSprite).spriteGameSize.Width, (listBox1.SelectedItem as BaseSprite).spriteGameSize.Height);
            }
        }
    }
}
