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
using TBAGW.Utilities.Map;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW.Forms.NPC_Editor
{
    public partial class NPC_Editor : Form
    {
        public NPC_Editor()
        {
            InitializeComponent();
        }

        NPC npc;

        public void Start(NPC npc)
        {
            this.npc = npc;
            listBox1.DataSource = null;
            listBox1.DataSource = npc.npcCommands;
            listBox2.DataSource = null;
            listBox2.DataSource = Enum.GetNames(typeof(commandTypes));
            listBox2.SelectedIndex = 0;

            textBox2.Text = npc.npcName;
            richTextBox2.Text = npc.npcDescription;
            textBox3.Text = npc.npcDisplayName;

            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(MapBuilder.oiw);
            MapBuilder.oiw = new GameObjects.ObjectInfoWindow();
            MapBuilder.oiw.Start(npc.baseCharacter);
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        internal enum commandTypes { Move_To_Command, Change_Map_Command }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                switch ((commandTypes)listBox2.SelectedIndex)
                {
                    case commandTypes.Move_To_Command:
                        npc.npcCommands.Add(new NPCMoveToCommand());
                        break;
                    case commandTypes.Change_Map_Command:
                        npc.npcCommands.Add(new NPCChangeMap());
                        break;
                    default:
                        break;
                }

                npc.npcCommands.Last().parentObject = npc;
                listBox1.DataSource = null;
                listBox1.DataSource = npc.npcCommands;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > 0)
            {
                var temp = listBox1.SelectedItem as NPCMoveToCommand;
                npc.npcCommands.Remove(temp);
                npc.npcCommands.Insert(listBox1.SelectedIndex - 1, temp);

                listBox1.DataSource = null;
                listBox1.DataSource = npc.npcCommands;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && listBox1.SelectedIndex != listBox1.Items.Count - 1)
            {
                var temp = listBox1.SelectedItem as NPCMoveToCommand;
                npc.npcCommands.Remove(temp);
                npc.npcCommands.Insert(listBox1.SelectedIndex + 1, temp);

                listBox1.DataSource = null;
                listBox1.DataSource = npc.npcCommands;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem as NPCMoveToCommand;
                npc.npcCommands.Remove(temp);

                listBox1.DataSource = null;
                listBox1.DataSource = npc.npcCommands;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem;
                if (temp.GetType() == typeof(NPCMoveToCommand))
                {
                    MapBuilder.controls.startTileSelect(AssignDestinationFromSelectedTile);
                }
                else if (temp.GetType() == typeof(NPCChangeMap))
                {
                    var temp2 = listBox1.SelectedItem as NPCChangeMap;
                    if (temp2.MapToMoveToID == temp2.mapID)
                    {
                        MessageBox.Show("Please, first select a valid map to move to.");
                        goto Skip;
                    }
                    MapBuilder.ReloadActiveMap(BasicMap.allMapsGame().Find(map => map.identifier == temp2.MapToMoveToID));
                    MapBuilder.controls.startTileSelect(AssignDestinationFromSelectedTile);
                Skip: { }
                }

            }


        }

        internal void AssignDestinationFromSelectedTile(BasicTile tile)
        {
            if (listBox1.SelectedIndex != -1)
            {
                int possibleMapID = 0;
                if (listBox1.SelectedIndex == 0)
                {
                    possibleMapID = npc.mapPlacedID;
                }
                else
                {
                    var selectedCommand = npc.npcCommands[listBox1.SelectedIndex];
                    if (selectedCommand.GetType() != typeof(NPCChangeMap))
                    {
                        var previousCommand = npc.npcCommands[listBox1.SelectedIndex - 1];
                        if (previousCommand.GetType() == typeof(NPCMoveToCommand))
                        {
                            possibleMapID = previousCommand.mapID;
                        }
                        else if (previousCommand.GetType() == typeof(NPCChangeMap))
                        {
                            possibleMapID = (previousCommand as NPCChangeMap).MapToMoveToID;
                        }
                    }
                    else if (selectedCommand.GetType() == typeof(NPCChangeMap))
                    {
                        possibleMapID = (selectedCommand as NPCChangeMap).MapToMoveToID;
                    }


                }
                var map = MapBuilder.loadedMap;

                if (map.identifier == possibleMapID)
                {
                    (listBox1.SelectedItem as NPCMoveToCommand).destination = tile.positionGrid * 64;
                    (listBox1.SelectedItem as NPCMoveToCommand).mapID = possibleMapID;
                    MapBuilder.controls.currentType = MapBuilder.controls.previousType;
                }
                else
                {
                    DialogResult warning = MessageBox.Show("Destination on specified map not reachable, continue?", "WARNING!", MessageBoxButtons.YesNo);
                    if (warning == DialogResult.Yes)
                    {
                        (listBox1.SelectedItem as NPCMoveToCommand).destination = tile.positionGrid * 64;
                        (listBox1.SelectedItem as NPCMoveToCommand).mapID = possibleMapID;
                    }
                }
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem as NPCMoveToCommand;
                numericUpDown1.Value = (decimal)(listBox1.SelectedItem as NPCMoveToCommand).speed;
                textBox1.Text = temp.commandName;
                richTextBox1.Text = temp.commandDescription;
                numericUpDown2.Value = temp.tickStart;
                numericUpDown3.Value = temp.tickStop;

                if (temp.GetType() == typeof(NPCMoveToCommand))
                {
                    tabControl1.SelectedIndex = 0;
                }
                else if (temp.GetType() == typeof(NPCChangeMap))
                {
                    var temp2 = listBox1.SelectedItem as NPCChangeMap;
                    tabControl1.SelectedIndex = 1;
                    listBox3.DataSource = null;
                    listBox3.DataSource = BasicMap.allMapsGame();
                    if (BasicMap.allMapsGame().Find(m => m.identifier == temp2.MapToMoveToID) != null)
                    {
                        listBox3.SelectedIndex = BasicMap.allMapsGame().IndexOf(BasicMap.allMapsGame().Find(m => m.identifier == temp2.MapToMoveToID));
                    }
                    else
                    {
                        temp2.MapToMoveToID = MapBuilder.loadedMap.identifier;
                        listBox3.SelectedIndex = BasicMap.allMapsGame().IndexOf(BasicMap.allMapsGame().Find(m => m.identifier == temp2.MapToMoveToID));
                    }

                }
            }

            if (MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.TileSelect)
            {
                MapBuilder.controls.currentType = MapBuilder.controls.previousType;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                (listBox1.SelectedItem as NPCMoveToCommand).speed = (float)numericUpDown1.Value;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem as NPCMoveToCommand;
                temp.commandName = textBox1.Text;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem as NPCMoveToCommand;
                temp.commandDescription = richTextBox1.Text;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem as NPCMoveToCommand;
                Form1.MakeSureFormClosed(Form1.sbce);
                Form1.sbce = new Bools.ScrptBoolCheckEditor();
                Form1.sbce.Start(temp.sbc);
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem as NPCMoveToCommand;
                temp.tickStart = (int)numericUpDown2.Value;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem as NPCMoveToCommand;
                temp.tickStop = (int)numericUpDown3.Value;
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                var temp = listBox1.SelectedItem as NPCMoveToCommand;
                if (temp.GetType() == typeof(NPCChangeMap))
                {
                    var temp2 = listBox1.SelectedItem as NPCChangeMap;
                    temp2.MapToMoveToID = (listBox3.SelectedItem as BasicMap).identifier;
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            npc.npcDisplayName = textBox3.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            npc.npcName = textBox2.Text;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            npc.npcDescription = richTextBox2.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var temp = (listBox1.SelectedItem as NPCMoveToCommand).script;

                DialogResult dialogResult = MessageBox.Show("Would you like to create a -completely- separate script for the object ? If not, every single object with the same script ID on this map will have the newly edited script.", "Script warning", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    if (temp != null)
                    {
                        temp = temp.Clone();
                    }
                    else
                    {
                        temp = new BaseScript();
                    }

                    MapBuilder.gcDB.AddScript(temp, (listBox1.SelectedItem as NPCMoveToCommand));
                    Form1.MakeSureFormClosed(Form1.scriptForm);
                    Form1.scriptForm = new ScriptBaseForm();
                    Form1.scriptForm.Show();
                    Form1.scriptForm.LoadScript(temp, npc);
                    (listBox1.SelectedItem as NPCMoveToCommand).script = temp;
                }
                else if (dialogResult == DialogResult.No)
                {
                    if ((listBox1.SelectedItem as NPCMoveToCommand).scriptIdentifier == -1)
                    {
                        temp = new BaseScript();
                        MapBuilder.gcDB.AddScript(temp, (listBox1.SelectedItem as NPCMoveToCommand));
                        // MapBuilder.parentMap.massChangeScript(temp, temp.script);
                    }
                    Form1.MakeSureFormClosed(Form1.scriptForm);
                    Form1.scriptForm = new ScriptBaseForm();
                    Form1.scriptForm.Show();
                    Form1.scriptForm.LoadScript(temp, npc);
                    (listBox1.SelectedItem as NPCMoveToCommand).script = temp;
                }
            }
        }
    }
}