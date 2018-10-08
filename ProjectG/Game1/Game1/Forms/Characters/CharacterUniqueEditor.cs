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
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;

namespace TBAGW.Forms.Characters
{
    public partial class CharacterUniqueEditor : Form
    {
        public CharacterUniqueEditor()
        {
            InitializeComponent();
        }

        private void CharacterUniqueEditor_Load(object sender, EventArgs e)
        {

        }

        static BaseCharacter bc;
        public void Start(BaseCharacter bca)
        {
            Show();
            bc = bca;
            this.Text = bc.CharacterName;
            listBox2.DataSource = bc.CCC.charSeparateAbilities;
            listBox1.DataSource = MapBuilder.gcDB.gameAbilities;
        }

        static public void AssignWeapon(int ID)
        {
            bc.weaponID = ID;
            if (bc.weaponID != -1)
            {
            }
        }

        static public void AssignArmor(int ID)
        {
            bc.armourID = ID;
            if (bc.armourID != -1)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bc.weaponID == -1)
            {
                Weapon.Text = "NONE";
            }
            else
            {
                bc.ReloadFromDatabase(MapBuilder.gcDB);
                try
                {
                    Weapon.Text = bc.weapon.itemName;
                }
                catch (Exception)
                {
                    var be = new BaseEquipment();
                    be.EquipType = BaseEquipment.EQUIP_TYPES.Weapon;
                    bc.weapon = be;
                }
               
            }

            if (bc.armourID == -1)
            {
                Armour.Text = "NONE";
            }
            else
            {
                bc.ReloadFromDatabase(MapBuilder.gcDB);
               // Armour.Text = bc.armour.itemName;
                try
                {
                    Armour.Text = bc.armour.itemName;
                }
                catch (Exception)
                {
                    var be = new BaseEquipment();
                    be.EquipType = BaseEquipment.EQUIP_TYPES.Armor;
                    bc.armour = be;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.DataSource = null;
                listBox1.DataSource = MapBuilder.gcDB.gameAbilities;
            }
            else if (!textBox1.Text.Equals(""))
            {
                listBox1.DataSource = null;
                //  listBox1.Items.AddRange(MapBuilder.loadedMap.mapRegions.FindAll(r => r.regionName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                listBox1.DataSource = (MapBuilder.gcDB.gameAbilities.FindAll(i => i.abilityName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && !bc.CCC.charSeparateAbilities.Contains((BasicAbility)listBox1.SelectedItem))
            {
                var temp = ((BasicAbility)listBox1.SelectedItem).Clone();
                bc.CCC.charSeparateAbilityIDs.Add(temp.abilityIdentifier);
                bc.CCC.charSeparateAbilities.Add(temp);
                listBox2.DataSource = null;
                listBox2.DataSource = bc.CCC.charSeparateAbilities;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                bc.CCC.charSeparateAbilities.RemoveAt(listBox2.SelectedIndex);
                bc.CCC.charSeparateAbilityIDs.RemoveAt(listBox2.SelectedIndex);
                listBox2.DataSource = null;
                listBox2.DataSource = bc.CCC.charSeparateAbilities;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SimpleEquipmentSelection ses = new SimpleEquipmentSelection();
            ses.Start(BaseEquipment.EQUIP_TYPES.Weapon);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SimpleEquipmentSelection ses = new SimpleEquipmentSelection();
            ses.Start(BaseEquipment.EQUIP_TYPES.Armor);
        }
    }
}
