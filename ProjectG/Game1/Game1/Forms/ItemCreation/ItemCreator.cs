using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Forms.General;
using TBAGW.Scenes.Editor;

namespace TBAGW.Forms.ItemCreation
{
    public partial class ItemCreator : Form
    {
        public ItemCreator()
        {
            InitializeComponent();
        }

        private void ItemCreator_Load(object sender, EventArgs e)
        {

        }

        public void Start()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(MapBuilder.gcDB.gameItems.ToArray());
            ItemSearch.Text = "";
            richTextBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(BaseItem.ITEM_RARITY)));

            listBox2.Items.Clear();
            listBox2.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc=>gc.itemType==BaseItem.ITEM_TYPES.Consumables).ToArray());

            listBox3.Items.Clear();
            listBox3.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Equipment).ToArray());

            listBox4.Items.Clear();
            listBox4.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Materials).ToArray());

            listBox5.Items.Clear();
            listBox5.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Quest_Item).ToArray());
            Show();
        }

        private void ItemSearch_TextChanged(object sender, EventArgs e)
        {
            if (!ItemSearch.Text.Equals(""))
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(i => i.itemName.Contains(ItemSearch.Text)).ToArray());
            }
            else
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.gcDB.gameItems.ToArray());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this item?", "Delete item?", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    var item = (BaseItem)listBox1.SelectedItem;
                    deleteItemEverywhere(item);
                }
                else if (dialogResult == System.Windows.Forms.DialogResult.No)
                {
                    //do something else
                }
            }

        }

        private void reloadLB1()
        {
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(MapBuilder.gcDB.gameItems.ToArray());

            listBox2.SelectedIndex = -1;
            listBox2.Items.Clear();
            listBox2.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Consumables).ToArray());

            listBox3.SelectedIndex = -1;
            listBox3.Items.Clear();
            listBox3.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Equipment).ToArray());

            listBox4.SelectedIndex = -1;
            listBox4.Items.Clear();
            listBox4.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Materials).ToArray());

            listBox5.SelectedIndex = -1;
            listBox5.Items.Clear();
            listBox5.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Quest_Item).ToArray());
        }

        private void deleteItemEverywhere(BaseItem bi)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                BaseConsumable bc = new BaseConsumable(true);
                bc.itemName = textBox1.Text;
                MapBuilder.gcDB.gameItems.Add(bc);
                reloadLB1();
                textBox1.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                BaseEquipment be = new BaseEquipment(true);
                be.itemName = textBox1.Text;
                MapBuilder.gcDB.gameItems.Add(be);
                reloadLB1();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                BaseKeyItem bki = new BaseKeyItem(true);
                bki.itemName = textBox1.Text;
                MapBuilder.gcDB.gameItems.Add(bki);
                reloadLB1();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                BaseMaterials bm = new BaseMaterials(true);
                bm.itemName = textBox1.Text;
                MapBuilder.gcDB.gameItems.Add(bm);
                reloadLB1();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MapBuilder.SaveMap();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                BaseItem bi = (BaseItem)listBox1.SelectedItem;
                STATChartForm scf = new STATChartForm();
                scf.Start(bi.statModifier);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                BaseItem bi = (BaseItem)listBox1.SelectedItem;
                if (bi.itemTexLoc.Equals(""))
                {
                    MessageBox.Show("Please select base texture file.");
                    OpenFileDialog openTex = new OpenFileDialog();
                    openTex.Title = "Open texture file";
                    if (Game1.bIsDebug)
                    {
                        openTex.Filter = "Texture File|*.jpg;*.png;*.jpeg;*.xnb";
                        openTex.InitialDirectory = Game1.rootContent;
                    }
                    else
                    {
                        openTex.Filter = "Texture File|*.jpg;*.png;*.jpeg";
                        openTex.InitialDirectory = Game1.rootContentExtra;
                    }

                    bool bDone = false;

                    while (!bDone)
                    {
                        DialogResult dia = openTex.ShowDialog();
                        if (dia == DialogResult.OK && openTex.FileName.Contains(openTex.InitialDirectory))
                        {

                            bi.itemTexLoc = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
                            Console.WriteLine("Successful item texture selection");
                            bDone = true;
                        }
                        else if (!openTex.FileName.Contains(openTex.InitialDirectory))
                        {
                            MessageBox.Show(@"Please select a file within the application folder under Content\Mods and it's subfolders");
                        }
                        else if (dia == DialogResult.Cancel)
                        {
                            bDone = true;
                        }
                    }


                }

                try
                {
                    bi.ReloadTexture();
                    FrameSelector.StartComplex(bi.itemTexAndAnimation, (int)frameWidth.Value, (int)frameHeight.Value, (int)xOffSet.Value, (int)yOffSet.Value);
                }
                catch
                {

                    throw;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FrameSelector.Stop();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                BaseItem bi = (BaseItem)listBox1.SelectedItem;

                MessageBox.Show("Please select base texture file.");
                OpenFileDialog openTex = new OpenFileDialog();
                openTex.Title = "Open texture file";
                if (Game1.bIsDebug)
                {
                    openTex.Filter = "Texture File|*.jpg;*.png;*.jpeg;*.xnb";
                    openTex.InitialDirectory = Game1.rootContent;
                }
                else
                {
                    openTex.Filter = "Texture File|*.jpg;*.png;*.jpeg";
                    openTex.InitialDirectory = Game1.rootContentExtra;
                }

                bool bDone = false;

                while (!bDone)
                {
                    DialogResult dia = openTex.ShowDialog();
                    if (dia == DialogResult.OK && openTex.FileName.Contains(openTex.InitialDirectory))
                    {

                        bi.itemTexLoc = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").LastIndexOf("."));
                        Console.WriteLine("Successful item texture selection");
                        bDone = true;
                        bi.itemTexAndAnimation.animationFrames.Clear();
                    }
                    else if (!openTex.FileName.Contains(openTex.InitialDirectory))
                    {
                        MessageBox.Show(@"Please select a file within the application folder under Content\Mods and it's subfolders");
                    }
                    else if (dia == DialogResult.Cancel)
                    {
                        bDone = true;
                    }
                }

                try
                {
                    bi.ReloadTexture();
                    FrameSelector.StartComplex(bi.itemTexAndAnimation, (int)frameWidth.Value, (int)frameHeight.Value, (int)xOffSet.Value, (int)yOffSet.Value);
                }
                catch
                {

                    throw;
                }

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                frameTimer.Value = ((BaseItem)listBox1.SelectedItem).itemTexAndAnimation.frameInterval;
                richTextBox1.Text = ((BaseItem)listBox1.SelectedItem).itemDescription;
                textBox2.Text = ((BaseItem)listBox1.SelectedItem).itemShortDescription;
                comboBox1.SelectedIndex = (int)((BaseItem)listBox1.SelectedItem).ItemRarity;
                textBox3.Text = ((BaseItem)listBox1.SelectedItem).itemName;
                numericUpDown1.Value = ((BaseItem)listBox1.SelectedItem).itemBuyPrice;
                numericUpDown2.Value = ((BaseItem)listBox1.SelectedItem).itemSellPrice;
            }
        }

        private void frameTimer_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ((BaseItem)listBox1.SelectedItem).itemTexAndAnimation.frameInterval = (int)frameTimer.Value;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ((BaseItem)listBox1.SelectedItem).itemDescription = richTextBox1.Text;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ((BaseItem)listBox1.SelectedItem).itemShortDescription = textBox2.Text;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ((BaseItem)listBox1.SelectedItem).ItemRarity = (BaseItem.ITEM_RARITY)comboBox1.SelectedIndex;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ((BaseItem)listBox1.SelectedItem).itemName = textBox3.Text;

            }
        }

        private void SoftReloadLB1()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(MapBuilder.gcDB.gameItems.ToArray());
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!textBox4.Text.Equals(""))
            {
                listBox2.Items.Clear();
                listBox2.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Consumables).FindAll(i => i.itemName.Contains(textBox4.Text)).ToArray());
            }
            else
            {
                listBox2.Items.Clear();
                listBox2.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Consumables).ToArray());
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (!textBox5.Text.Equals(""))
            {
                listBox3.Items.Clear();
                listBox3.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Equipment).FindAll(i => i.itemName.Contains(textBox5.Text)).ToArray());
            }
            else
            {
                listBox3.Items.Clear();
                listBox3.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Equipment).ToArray());
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (!textBox6.Text.Equals(""))
            {
                listBox4.Items.Clear();
                listBox4.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Materials).FindAll(i => i.itemName.Contains(textBox6.Text)).ToArray());
            }
            else
            {
                listBox4.Items.Clear();
                listBox4.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Materials).ToArray());
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (!textBox7.Text.Equals(""))
            {
                listBox5.Items.Clear();
                listBox5.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Quest_Item).FindAll(i => i.itemName.Contains(textBox7.Text)).ToArray());
            }
            else
            {
                listBox5.Items.Clear();
                listBox5.Items.AddRange(MapBuilder.gcDB.gameItems.FindAll(gc => gc.itemType == BaseItem.ITEM_TYPES.Quest_Item).ToArray());
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                listBox3.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                listBox5.SelectedIndex = -1;
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                listBox2.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                listBox5.SelectedIndex = -1;
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                listBox3.SelectedIndex = -1;
                listBox2.SelectedIndex = -1;
                listBox5.SelectedIndex = -1;
            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedIndex != -1)
            {
                listBox3.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                listBox2.SelectedIndex = -1;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                ConsumableEditor ce = new ConsumableEditor();
                ce.Start((BaseConsumable)listBox2.SelectedItem);
            }

            if (listBox3.SelectedIndex != -1)
            {
                EquipItemEditor eie = new EquipItemEditor();
                eie.Start((BaseEquipment)listBox3.SelectedItem);
            }

            if (listBox4.SelectedIndex != -1)
            {
                MaterialEditor me = new MaterialEditor();
                me.Start((BaseMaterials)listBox4.SelectedItem);
            }

            if (listBox5.SelectedIndex != -1)
            {
                KeyItemEditor kie = new KeyItemEditor();
                kie.Start((BaseKeyItem)listBox5.SelectedItem);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ((BaseItem)listBox1.SelectedItem).itemBuyPrice = (int)numericUpDown1.Value;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ((BaseItem)listBox1.SelectedItem).itemSellPrice = (int)numericUpDown2.Value;
            }
        }

        private void xOffSet_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
