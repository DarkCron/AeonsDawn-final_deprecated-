using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW;
using TBAGW.Forms.Abilities;
using TBAGW.Forms.General;
using TBAGW.Forms.ItemCreation;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;

namespace Game1.Forms.ZonesRegions
{
    public partial class RegionEditor : Form
    {
        public MapRegion region = new MapRegion();
        AddEnemyForm addEnemyForm = new AddEnemyForm();
        RegionCombatSong regionBG = null;

        public RegionEditor()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void Start(MapRegion region)
        {
            Show();
            this.region = region;
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            foreach (var item in region.enemyPool)
            {
                listBox1.Items.Add(item);
            }
            regionBG = region.regionBGinfo;
            GetRegionBGInfo();
        }

        private void GetRegionBGInfo()
        {
            label2.Text = regionBG.SongLocL1;
            label3.Text = regionBG.SongLocL2;
            label4.Text = regionBG.SongNonCombat;
        }

        public void ReloadLB1()
        {
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            foreach (var item in region.enemyPool)
            {
                listBox1.Items.Add(item);
            }
        }

        private void RegionEditor_Load(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
            MapBuilder.SaveMap();
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            addEnemyForm.Start(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                region.enemyPool.RemoveAt(listBox1.SelectedIndex);
            }
        }

        STATChartForm form = new STATChartForm();
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                Form1.MakeSureFormClosed(form);
                form = new STATChartForm();

                if (((EnemyAIInfo)listBox1.SelectedItem).enemyStats.currentActiveStats.Count == 0)
                {
                    ((EnemyAIInfo)listBox1.SelectedItem).enemyStats.DefaultStatChart();
                }
                form.Start(((EnemyAIInfo)listBox1.SelectedItem).enemyStats);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                CharAbilitiesForm form = new CharAbilitiesForm();
                form.Start((((EnemyAIInfo)listBox1.SelectedItem).CCC),(EnemyAIInfo)listBox1.SelectedItem);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (var zone in region.regionZones)
            {
                //zone.zoneEncounterInfo.enemies = region.enemyPool;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if ((listBox1.SelectedItem as EnemyAIInfo).HasProperLua())
                {
                    try
                    {   // Open the text file using a stream reader.
                        using (StreamReader sr = new StreamReader(TBAGW.Game1.rootContent + (listBox1.SelectedItem as EnemyAIInfo).luaLoc))
                        {
                            // Read the stream to a string, and write the string to the console.
                            String line = sr.ReadToEnd();
                            richTextBox1.Text = line;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(ex.Message);
                    }
                }else
                {
                    richTextBox1.Text = "";
                }
            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                EquipmentSelection ise = new EquipmentSelection();
                ise.Start(BaseEquipment.EQUIP_TYPES.Weapon, ((EnemyAIInfo)listBox1.SelectedItem).enemyWeaponArray);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                EquipmentSelection ise = new EquipmentSelection();
                ise.Start(BaseEquipment.EQUIP_TYPES.Armor, ((EnemyAIInfo)listBox1.SelectedItem).enemyArmourArray);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Lua file|*.lua";
                ofd.InitialDirectory = TBAGW.Game1.rootContent;
                ofd.Title = "open Lua";
                DialogResult dia = ofd.ShowDialog();
                if (DialogResult.OK == dia && ofd.FileName.Contains(ofd.InitialDirectory))
                {
                    String fi = ofd.FileName;
                    (listBox1.SelectedItem as EnemyAIInfo).luaLoc = fi.Replace(ofd.InitialDirectory, "");
                }
                else if (System.Windows.Forms.DialogResult.Cancel == dia)
                {

                    System.Windows.Forms.MessageBox.Show("Cancelled, returning to Editor.");
                }

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                (listBox1.SelectedItem as EnemyAIInfo).luaLoc = "";
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            LoadSongFile(AssignL1);
        }

        private void AssignL1(String s)
        {
            regionBG.SongLocL1 = s;
        }

        private void AssignL2(String s)
        {
            regionBG.SongLocL2 = s;
        }

        private void AssignNonCombat(String s)
        {
            regionBG.SongNonCombat = s;
        }

        private delegate void AssignString(String s);
        private void LoadSongFile(AssignString ds)
        {
            OpenFileDialog openTex = new OpenFileDialog();
            openTex.Title = "Open texture file";

                openTex.Filter = "Sound File|*.mp3;*.wav;*.xnb; *.wma";
                openTex.InitialDirectory = TBAGW.Game1.rootContent;


            bool bDone = false;

            while (!bDone)
            {
                DialogResult dia = openTex.ShowDialog();
                if (dia == DialogResult.OK && openTex.FileName.Contains(openTex.InitialDirectory))
                {

                    ds( openTex.FileName.Replace(TBAGW.Game1.rootContent, "").Substring(0, openTex.FileName.Replace(TBAGW.Game1.rootContent, "").LastIndexOf(".")));
                    bDone = true;
                }
                else if (dia == DialogResult.Cancel)
                {
                    ds("");
                    bDone = true;
                }
                else if (!openTex.FileName.Contains(openTex.InitialDirectory))
                {
                    MessageBox.Show(@"Please select a file within the application folder under Content\Mods and it's subfolders");
                }

            }

            GetRegionBGInfo();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            LoadSongFile(AssignL2);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            LoadSongFile(AssignNonCombat);
        }
    }
}
