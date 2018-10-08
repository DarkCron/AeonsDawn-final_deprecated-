using Game1.Utilities.GamePlay.Battle;
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
using TBAGW.Utilities.Characters;

namespace Game1.Forms.PlayTestForms
{
    public partial class GenerateRandomZoneEncounterForm : Form
    {
        public GenerateRandomZoneEncounterForm()
        {
            InitializeComponent();
        }

        MapZone zone;
        MapRegion region;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void Start(MapZone zone, MapRegion region) {
            Show();
            this.zone = zone;
            label2.Text = zone.ToString();
            String enemies = "";
            foreach (var item in zone.zoneEncounterInfo.enemies)
            {
                BaseCharacter temp = item.enemyCharBase;
                enemies += temp.CharacterName +" Spawn %: "+zone.zoneEncounterInfo.enemySpawnChance[zone.zoneEncounterInfo.enemies.IndexOf(item)]+ "%\n";
            }
            label3.Text = enemies;
            label5.Text = zone.zoneEncounterInfo.encounterChance + "%\n";
            label7.Text = zone.zoneEncounterInfo.packSizeMin + " ~ " + zone.zoneEncounterInfo.packSizeMax + " enemies per battle";
            this.region = region;
        }

        private void GenerateRandomZoneEncounterForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //EncounterGenerator.Start(region, zone);
        }
    }
}
