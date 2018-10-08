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
    public partial class ItemSelection : Form
    {
        public ItemSelection()
        {
            InitializeComponent();
        }

        private void ItemSelection_Load(object sender, EventArgs e)
        {

        }

        List<int> listToAddTo;
        BaseItem.ITEM_TYPES selectedItemType;
        public void Start(BaseItem.ITEM_TYPES biit, List<int> list) {
            Show();
            selectedItemType = biit;
            listToAddTo = list;
            listBox1.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(i=>i.itemType==selectedItemType).ToArray());
            listBox2.DataSource = listToAddTo;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex!=-1) {
                listToAddTo.Remove((int)listBox2.SelectedItem);
                listBox2.DataSource = listToAddTo;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Equals("")) {
                listBox1.DataSource = (MapBuilder.gcDB.gameItems.FindAll(i => i.itemType == selectedItemType));
            }
            else if (!textBox1.Text.Equals(""))
            {
                //  listBox1.Items.AddRange(MapBuilder.loadedMap.mapRegions.FindAll(r => r.regionName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                listBox1.DataSource = (MapBuilder.gcDB.gameItems.FindAll(i => i.itemType == selectedItemType).FindAll(i=>i.itemName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1) {
                if(!listToAddTo.Contains((int)listBox1.SelectedItem)) {
                    listToAddTo.Add((int)listBox1.SelectedItem);
                    listBox2.DataSource = listToAddTo;
                }
            }
        }
    }
}
