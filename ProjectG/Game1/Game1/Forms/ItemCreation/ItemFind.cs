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
    public partial class ItemFind : Form
    {
        public ItemFind()
        {
            InitializeComponent();
        }

        private void ItemFind_Load(object sender, EventArgs e)
        {
            listBox2.DataSource = Enum.GetNames(typeof(BaseItem.ITEM_TYPES));
            listBox2.SelectedIndex = Enum.GetNames(typeof(BaseItem.ITEM_TYPES)).Length - 1;
            listBox1.DataSource = MapBuilder.gcDB.gameItems;
        }


        ItemSelectionFunction func;
        BaseItem bi = null;

        public void Start(ItemSelectionFunction func)
        {
            listBox2.DataSource = Enum.GetNames(typeof(BaseItem.ITEM_TYPES));
            listBox2.SelectedIndex = Enum.GetNames(typeof(BaseItem.ITEM_TYPES)).Length - 1;
            listBox1.DataSource = MapBuilder.gcDB.gameItems;
            this.func = func;
            bi = null;
            Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("", StringComparison.OrdinalIgnoreCase) && listBox2.SelectedIndex != -1)
            {
                if ((listBox2.SelectedItem as String).Equals("None", StringComparison.OrdinalIgnoreCase))
                {
                    listBox1.DataSource = null;
                    listBox1.DataSource = (MapBuilder.gcDB.gameItems).ToArray();
                }
                else
                {
                    BaseItem.ITEM_TYPES type = (BaseItem.ITEM_TYPES)listBox2.SelectedIndex;
                    listBox1.DataSource = null;
                    listBox1.DataSource = (MapBuilder.gcDB.gameItems.FindAll(i => i.itemType == type).ToList());
                }

            }
            else if (!textBox1.Text.Equals("", StringComparison.OrdinalIgnoreCase))
            {
                //listBox1.DataSource = null;
                //  listBox1.Items.AddRange(MapBuilder.loadedMap.mapRegions.FindAll(r => r.regionName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                //listBox1.DataSource = (MapBuilder.gcDB.gameItems.FindAll(i => i.itemType == BaseItem.ITEM_TYPES.Equipment).Cast<BaseEquipment>().ToList().FindAll(i => i.EquipType == selectedItemType).FindAll(i => i.itemName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0));

                if ((listBox2.SelectedItem as String).Equals("None", StringComparison.OrdinalIgnoreCase))
                {
                    listBox1.DataSource = null;
                    listBox1.DataSource = (MapBuilder.gcDB.gameItems).FindAll(i => i.itemName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                }
                else
                {
                    BaseItem.ITEM_TYPES type = (BaseItem.ITEM_TYPES)listBox2.SelectedIndex;
                    listBox1.DataSource = null;
                    listBox1.DataSource = MapBuilder.gcDB.gameItems.FindAll(i => i.itemType == type).FindAll(i => i.itemName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                bi = listBox1.SelectedItem as BaseItem;
            }
        }

        public delegate void ItemSelectionFunction(BaseItem bi);

        private void button1_Click(object sender, EventArgs e)
        {
            if (bi != null)
            {
                func(bi);
                Close();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(sender, e);
        }
    }
}
