using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;

namespace Game1.Forms.ZonesRegions
{
    public partial class ZoneEditor : Form
    {
        MapZone zone;
        MapRegion region;

        public ZoneEditor()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void Start(MapZone mz, MapRegion region) {
            Show();
            zone = mz;
            this.region = region;
            textBox1.Text = zone.zoneName;
            label2.Text = mz.zoneName;
            numericUpDown2.Value = zone.zoneEncounterInfo.encounterChance;

            listBox2.Items.Clear();
            listBox2.SelectedIndex = -1;
            
            foreach (var item in region.enemyPool)
            {
                listBox2.Items.Add(item);
            }

            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            foreach (var item in zone.zoneEncounterInfo.enemies)
            {
                listBox1.Items.Add(item);
            }

            numericUpDown4.Value = zone.zoneEncounterInfo.packSizeMin;
            numericUpDown5.Value = zone.zoneEncounterInfo.packSizeMax;
        }

        private void ZoneEditor_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            zone.zoneName = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zone.zoneID = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            zone.zoneEncounterInfo.encounterChance = (int)numericUpDown2.Value;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MapBuilder.SaveMap();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex!=-1) {
                zone.zoneEncounterInfo.AddEnemy(((EnemyAIInfo)listBox2.SelectedItem));
                listBox1.Items.Clear();
                listBox1.SelectedIndex = -1;
                foreach (var item in zone.zoneEncounterInfo.enemies)
                {
                    listBox1.Items.Add(item);
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1) {
                zone.zoneEncounterInfo.RemoveEnemy(listBox1.SelectedIndex);
                listBox1.Items.Clear();
                listBox1.SelectedIndex = -1;
                foreach (var item in zone.zoneEncounterInfo.enemies)
                {
                    listBox1.Items.Add(item);
                }
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1) {
                zone.zoneEncounterInfo.packSizeMin = (int)numericUpDown4.Value;
                int min = zone.zoneEncounterInfo.packSizeMin;
                int max = zone.zoneEncounterInfo.packSizeMax;

                if(min>max) {
                    zone.zoneEncounterInfo.packSizeMax = zone.zoneEncounterInfo.packSizeMin;
                    numericUpDown5.Value = zone.zoneEncounterInfo.packSizeMin;
                }
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                int min = (int)numericUpDown4.Value;
                int max = (int)numericUpDown5.Value;
                if(max<min) {
                    max = min;
                    numericUpDown5.Value = max;
                }
                zone.zoneEncounterInfo.packSizeMax = max;

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex!=-1) {
                try
                {
                    numericUpDown3.Value = zone.zoneEncounterInfo.enemySpawnChance[listBox1.SelectedIndex];
                }
                catch (Exception)
                {
                    while(zone.zoneEncounterInfo.enemies.Count!=zone.zoneEncounterInfo.enemySpawnChance.Count) {
                        zone.zoneEncounterInfo.enemySpawnChance.Add(33);
                    }
                }
               
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            zone.zoneEncounterInfo.enemySpawnChance[listBox1.SelectedIndex] = (int)numericUpDown3.Value;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
