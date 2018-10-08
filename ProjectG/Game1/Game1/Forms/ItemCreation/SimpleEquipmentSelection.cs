using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Forms.Characters;
using TBAGW.Scenes.Editor;

namespace TBAGW.Forms.ItemCreation
{
    public partial class SimpleEquipmentSelection : Form
    {
        public SimpleEquipmentSelection()
        {
            InitializeComponent();
        }

        private void SimpleEquipmentSelection_Load(object sender, EventArgs e)
        {

        }

        BaseEquipment.EQUIP_TYPES selectedItemType;
        public void Start(BaseEquipment.EQUIP_TYPES biit) {
            Show();
            selectedItemType = biit;
            listBox1.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(i => i.itemType == BaseItem.ITEM_TYPES.Equipment).Cast<BaseEquipment>().ToList().FindAll(i => i.EquipType == biit).ToArray());

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
            if(selectedItemType ==BaseEquipment.EQUIP_TYPES.Weapon) {
                if (listBox1.SelectedIndex != -1)
                {
                    CharacterUniqueEditor.AssignWeapon(((BaseItem)listBox1.SelectedItem).itemID);
                }
                else
                {
                    CharacterUniqueEditor.AssignWeapon(-1);
                }
            }

            if (selectedItemType == BaseEquipment.EQUIP_TYPES.Armor)
            {
                if (listBox1.SelectedIndex != -1)
                {
                    CharacterUniqueEditor.AssignArmor(((BaseItem)listBox1.SelectedItem).itemID);
                }
                else
                {
                    CharacterUniqueEditor.AssignArmor(-1);
                }
            }
        }
    }
}
