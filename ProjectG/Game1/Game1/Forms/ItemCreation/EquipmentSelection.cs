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

namespace TBAGW.Forms.ItemCreation
{
    public partial class EquipmentSelection : Form
    {
        public EquipmentSelection()
        {
            InitializeComponent();
        }

        private void EquipmentSelection_Load(object sender, EventArgs e)
        {

        }

        List<int> listToAddTo;
        BaseEquipment.EQUIP_TYPES selectedItemType;
        public void Start(BaseEquipment.EQUIP_TYPES biit, List<int> list)
        {
            Show();
            selectedItemType = biit;
            listToAddTo = list;
            listBox1.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(i => i.itemType == BaseItem.ITEM_TYPES.Equipment).Cast<BaseEquipment>().ToList().FindAll(i => i.EquipType == biit).ToArray());
            listBox2.DataSource = null;
            List<BaseItem> lbi = MapBuilder.gcDB.gameItems.FindAll(i=>listToAddTo.Contains(i));
            listBox2.DataSource = lbi;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                listToAddTo.Remove((int)listBox2.SelectedItem);
                listBox2.DataSource = null;
                List<BaseItem> lbi = MapBuilder.gcDB.gameItems.FindAll(i => listToAddTo.Contains(i));
                listBox2.DataSource = lbi;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.DataSource = null;
                listBox1.DataSource = (MapBuilder.gcDB.gameItems.FindAll(i => i.itemType == BaseItem.ITEM_TYPES.Equipment).Cast<BaseEquipment>().ToList().FindAll(i => i.EquipType == selectedItemType).ToArray());
            }
            else if (!textBox1.Text.Equals(""))
            {
                listBox1.DataSource = null;
                //  listBox1.Items.AddRange(MapBuilder.loadedMap.mapRegions.FindAll(r => r.regionName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                listBox1.DataSource = (MapBuilder.gcDB.gameItems.FindAll(i => i.itemType == BaseItem.ITEM_TYPES.Equipment).Cast<BaseEquipment>().ToList().FindAll(i => i.EquipType == selectedItemType).FindAll(i => i.itemName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (!listToAddTo.Contains(((BaseItem)listBox1.SelectedItem).itemID))
                {
                    listToAddTo.Add(((BaseItem)listBox1.SelectedItem).itemID);
                    listBox2.DataSource = null;
                    List<BaseItem> lbi = MapBuilder.gcDB.gameItems.FindAll(i => listToAddTo.Contains(i));
                    listBox2.DataSource = lbi;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

