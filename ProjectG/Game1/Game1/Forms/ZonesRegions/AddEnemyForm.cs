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
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.ReadWrite;

namespace Game1.Forms.ZonesRegions
{
    public partial class AddEnemyForm : Form
    {
        RegionEditor regEdit;
        public AddEnemyForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void Start(RegionEditor regEdit)
        {
            Show();
            this.regEdit = regEdit;
            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            listBox1.Items.AddRange(MapBuilder.gcDB.gameCharacters.ToArray());
        }

        private void AddEnemyForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
                OpenFileDialog openChar = new OpenFileDialog();
                if (TBAGW.Game1.bIsDebug)
                {
                    openChar.Filter = "Character file|*.cgbcc;*.cgbcc";
                    openChar.InitialDirectory = TBAGW.Game1.rootTBAGW;

                }
                else
                {
                    openChar.Filter = "Character file|*.cgbc;*.cgbcc";
                    openChar.InitialDirectory = TBAGW.Game1.rootContentExtra;
                }

                openChar.Title = "Load Character";
                DialogResult dia = openChar.ShowDialog();
                if (DialogResult.OK == dia && openChar.FileName.Contains(openChar.InitialDirectory))
                {
                    String fi = openChar.FileName;
                    BaseCharacter temp = EditorFileWriter.BaseCharacterReader(fi);
                    if(!regEdit.listBox1.Items.Contains(temp)) {
                        regEdit.region.enemyPool.Add(temp);
                        regEdit.listBox1.Items.Add(temp);
                    }
                }
                else if (System.Windows.Forms.DialogResult.Cancel == dia)
                {

                    System.Windows.Forms.MessageBox.Show("Cancelled, returning to Editor.");
                }
                else if (!openChar.FileName.Contains(openChar.InitialDirectory))
                {
                    System.Windows.Forms.MessageBox.Show("Save within mods folder please.");
                }*/

            if (listBox1.SelectedIndex != -1)
            {
                BaseCharacter temp = (BaseCharacter)listBox1.SelectedItem;
                bool bHasThisChar = false;
                foreach (var item in regEdit.listBox1.Items)
                {
                    if (((TBAGW.EnemyAIInfo)item).enemyCharBase.shapeID == temp.shapeID)
                    {
                        bHasThisChar = true;
                    }
                }


                if (!bHasThisChar)
                {
                    regEdit.region.enemyPool.Add(new TBAGW.EnemyAIInfo(temp,regEdit.region));
                    regEdit.listBox1.Items.Add(regEdit.region.enemyPool.Last());
                }
            }
        }
    }
}

