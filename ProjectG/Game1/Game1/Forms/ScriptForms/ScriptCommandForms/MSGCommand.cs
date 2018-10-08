using Game1.Forms.ScriptForms;
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

namespace TBAGW.Forms.ScriptForms.ScriptCommandForms
{
    public partial class MSGCommand : Form
    {
        public MSGCommand()
        {
            InitializeComponent();
        }

        ScriptBaseForm scriptBaseForm;
        GameText gt = new GameText();

        public void Start(ScriptBaseForm sbf)
        {
            gt.Initialize();
            scriptBaseForm = sbf;
            Show();
            listBox1.DataSource = null;
            listBox1.DataSource = new List<BaseCharacter>(MapBuilder.gcDB.gameCharacters);
            listBox2.DataSource = null;
            listBox2.DataSource = new List<BaseCharacter>(MapBuilder.gcDB.gameCharacters);
            listBox3.DataSource = null;
            listBox3.DataSource = new List<BaseCharacter>(MapBuilder.gcDB.gameCharacters);
            listBox6.SelectedIndex = 0;

            if (ScriptBaseForm.parentObject != null && ScriptBaseForm.parentObject.GetType() == typeof(NPC))
            {
                var NPC = ScriptBaseForm.parentObject as NPC;
                if (NPC.baseCharacter == null)
                {
                    Console.WriteLine("Error npc bc is null");
                }
                listBox3.SelectedIndex = MapBuilder.gcDB.gameCharacters.IndexOf(MapBuilder.gcDB.gameCharacters.Find(c => NPC.baseCharacter.shapeID == c.shapeID));
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.DataSource = null;
                listBox1.DataSource = new List<BaseCharacter>(MapBuilder.gcDB.gameCharacters);
            }
            else if (!textBox1.Text.Equals(""))
            {
                listBox1.DataSource = null;
                //  listBox1.Items.AddRange(MapBuilder.loadedMap.mapRegions.FindAll(r => r.regionName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                listBox1.DataSource = new List<BaseCharacter>(MapBuilder.gcDB.gameCharacters.FindAll(i => i.CharacterName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(""))
            {
                listBox2.DataSource = null;
                listBox2.DataSource = new List<BaseCharacter>(MapBuilder.gcDB.gameCharacters);
            }
            else if (!textBox2.Text.Equals(""))
            {
                listBox2.DataSource = null;
                //  listBox1.Items.AddRange(MapBuilder.loadedMap.mapRegions.FindAll(r => r.regionName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                listBox2.DataSource = new List<BaseCharacter>(MapBuilder.gcDB.gameCharacters.FindAll(i => i.CharacterName.IndexOf(textBox2.Text, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals(""))
            {
                listBox3.DataSource = null;
                listBox3.DataSource = new List<BaseCharacter>(MapBuilder.gcDB.gameCharacters);
            }
            else if (!textBox3.Text.Equals(""))
            {
                listBox3.DataSource = null;
                //  listBox1.Items.AddRange(MapBuilder.loadedMap.mapRegions.FindAll(r => r.regionName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                listBox3.DataSource = new List<BaseCharacter>(MapBuilder.gcDB.gameCharacters.FindAll(i => i.CharacterName.IndexOf(textBox3.Text, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private void MSGCommand_Load(object sender, EventArgs e)
        {
            listBox4.DataSource = Enum.GetNames(typeof(BaseCharacter.PortraitExpressions));
            listBox5.DataSource = Enum.GetNames(typeof(BaseCharacter.PortraitExpressions));
            listBox6.DataSource = Enum.GetNames(typeof(GameText.Language));
            numericUpDown1.Value = MapBuilder.gcDB.highestTextID;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //((BaseCharacter)listBox1.SelectedItem).shapeID.ToString();
            SID.Text = ((BaseCharacter)listBox1.SelectedItem).shapeID.ToString();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CLID.Text = ((BaseCharacter)listBox2.SelectedItem).shapeID.ToString();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRID.Text = ((BaseCharacter)listBox3.SelectedItem).shapeID.ToString();
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            EL.Text = listBox4.SelectedIndex.ToString();
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ER.Text = listBox5.SelectedIndex.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MapBuilder.gcDB.AddTextCollection(gt);
            String temp = "@MSG_" + SID.Text + "_" + CLID.Text + "_" + CRID.Text + "_" + EL.Text + "_" + ER.Text + "_" + gt.textID;
            scriptBaseForm.AddLine(temp);
            Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
            gt.textCollection[listBox6.SelectedIndex] = richTextBox1.Text;
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = gt.textCollection[listBox6.SelectedIndex];
        }
    }
}
