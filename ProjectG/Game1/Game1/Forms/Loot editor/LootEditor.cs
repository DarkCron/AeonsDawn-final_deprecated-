using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Forms.ItemCreation;
using TBAGW.Utilities.Characters;

namespace TBAGW.Forms.Loot_editor
{
    public partial class LootEditor : Form
    {
        public LootEditor()
        {
            InitializeComponent();
        }

        private void LootEditor_Load(object sender, EventArgs e)
        {

        }

        LootList lootTable = new LootList();
        List<ItemLootInfo> concernedList = new List<ItemLootInfo>();
        ItemFind itemF = new ItemFind();

        public void Start(BaseCharacter bc)
        {
            lootTable = bc.lootList;

            listBox1.DataSource = null;
            listBox1.DataSource = lootTable.universalDrop;

            listBox2.Items.Clear();
            int level = 0;
            foreach (var item in lootTable.moneyDropPerLevel)
            {
                listBox2.Items.Add(level);
                level++;
            }


            Show();
        }

        public void ReloadListBoxes()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = lootTable.universalDrop;

            listBox2.Items.Clear();
            int level = 0;
            foreach (var item in lootTable.moneyDropPerLevel)
            {
                listBox2.Items.Add(level);
                level++;
            }

            if (listBox2.SelectedIndex != -1)
            {
                int index = listBox2.SelectedIndex;
                numericUpDown1.Value = lootTable.moneyDropPerLevel[index][0];
                numericUpDown2.Value = lootTable.moneyDropPerLevel[index][1];

                listBox3.DataSource = null;
                listBox3.DataSource = lootTable.dropsPerRegionLevel[index];
            }
        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            lootTable.AddLevel();
            ReloadListBoxes();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                int index = listBox2.SelectedIndex;
                numericUpDown1.Value = lootTable.moneyDropPerLevel[index][0];
                numericUpDown2.Value = lootTable.moneyDropPerLevel[index][1];
                numericUpDown3.Value = lootTable.expDropPerLevel[index];

                listBox3.DataSource = null;
                listBox3.DataSource = lootTable.dropsPerRegionLevel[index];
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                lootTable.moneyDropPerLevel[listBox2.SelectedIndex][0] = (int)numericUpDown1.Value;

                if (numericUpDown1.Value > numericUpDown2.Value)
                {
                    numericUpDown2.Value = numericUpDown1.Value;
                }
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                lootTable.moneyDropPerLevel[listBox2.SelectedIndex][1] = (int)numericUpDown2.Value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MakeSureItemFindIsClosed();
            concernedList = lootTable.universalDrop;
            itemF.Start(CreateItemLootInfo);
        }

        private void MakeSureItemFindIsClosed()
        {
            if (!itemF.IsDisposed)
            {
                itemF.Close();
                itemF.Dispose();
            }

            itemF = new ItemFind();
        }

        private void CreateItemLootInfo(BaseItem bi)
        {
            if (bi != null)
            {
                concernedList.Add(new ItemLootInfo(bi));
                ReloadListBoxes();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                lootTable.universalDrop.RemoveAt(listBox1.SelectedIndex);
                ReloadListBoxes();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                lootTable.dropsPerRegionLevel[listBox2.SelectedIndex].RemoveAt(listBox3.SelectedIndex);
                ReloadListBoxes();
            }
        }

        LootItemInfoEditor lie = new LootItemInfoEditor();
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                MakeSureFormIsClosed(lie);
                lie = new LootItemInfoEditor();
                lie.Start(listBox1.SelectedItem as ItemLootInfo);
            }
        }

        public void MakeSureFormIsClosed(Form f)
        {
            if(!f.IsDisposed)
            {
                f.Close();
                f.Dispose();
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                lootTable.expDropPerLevel[listBox2.SelectedIndex] = (int)numericUpDown3.Value;
            }
        }
    }
}
