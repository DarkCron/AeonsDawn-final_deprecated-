using TBAGW.Scenes.Editor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;
using TBAGW.Utilities.SriptProcessing.ScriptTriggers;
using Game1.Forms.ScriptForms;
using TBAGW.Utilities;
using Microsoft.Xna.Framework;
using Game1.Forms;
using TBAGW.Utilities.Map;
using TBAGW.Forms;
using Microsoft.Xna.Framework.Graphics;
using Game1.Forms.GameObjects;
using Game1.Forms.ZonesRegions;
using TBAGW.Forms.Abilities;
using System.Threading;
using TBAGW;
using TBAGW.Utilities.Particles;
using System.Diagnostics;
using TBAGW.Forms.ItemCreation;
using TBAGW.Forms.GameObjects;
using TBAGW.Forms.ParticleEditor;
using TBAGW.Forms.GameClasses;
using TBAGW.Forms.Sound;
using System.Net;
using TBAGW.Forms.FTP_Utility;
using TBAGW.Forms.Particle_Animation;
using TBAGW.Forms.TileSheetEditor;
using TBAGW.Forms.NPC_Editor;
using TBAGW.Forms.Bools;

namespace TBAGW
{
    public partial class Form1 : Form
    {
        public String mapName;
        public String mapX;
        public String mapY;
        public bool bUpdateImage = false;
        public bool bPressedSaveButton = false;
        public bool bPressedMMB = false;
        public bool bPresedsTSMB = false;
        public bool bPressedTEB = false;
        public bool bTSOpen = true;
        static public int layerDepth = 0;
        static int progress = 0;
        List<BaseSprite> lbs = new List<BaseSprite>();
        List<BaseScript> ls = new List<BaseScript>();
        List<TileSheet> lts = new List<TileSheet>();
        ScriptOverview so = new ScriptOverview();
        static public Vector2 editorRes = new Vector2();
        public Vector2 playTestRes = new Vector2();
        static public CharacterEditorForm charEditorForm = new CharacterEditorForm();
        static public SoundEditorForm soundEditorForm = new SoundEditorForm();
        static public ScriptBaseForm scriptForm = new ScriptBaseForm();
        public ObjectGroupEditorForm objGForm = new ObjectGroupEditorForm();
        SpriteEditorForm spriteEditForm = new SpriteEditorForm();
        ZoneEditor zoneEditor = new ZoneEditor();
        RegionEditor regionEditor = new RegionEditor();
        static public AbilityEditor abilityEditor = new AbilityEditor();
        Thread mainThread;
        static internal NPC_Editor npcEditor = new NPC_Editor();
        static internal ScrptBoolCheckEditor sbce = new ScrptBoolCheckEditor();
        static internal SongEncrypt songEncrypt = new SongEncrypt();

        public Form1()
        {
            mainThread = Thread.CurrentThread;
            InitializeComponent();
            this.ControlBox = false;
            mapName = MapNameLabel.Text;
            mapX = MapSizeXLabel.Text;
            mapY = MapSizeYLabel.Text;


            progress = 0;

            listBox3.DataSource = null;
            listBox3.DataSource = MapBuilder.gcDB.gameSourceTiles;

            listBox13.DataSource = null;
            listBox13.DataSource = Enum.GetNames(typeof(TileSource.TileType));

            checkBox7.Checked = MapBuilder.bShowInvisibleObjects;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();

        }

        public void UpdateFormContent()
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages[0])
            {
                MapNameLabel.Text = mapName;
                MapSizeXLabel.Text = mapX;
                MapSizeYLabel.Text = mapY;
                bUpdateImage = true;
            }
            else
            {
                bUpdateImage = false;
            }

            label6.Text = "Brush Size: " + MapBuilder.brushSize.ToString();
            numericUpDown1.Value = MapBuilder.brushSize;
            numInput.Value = layerDepth;

            ResetButtonEvents();

        }

        public void AssignSpriteList(List<BaseSprite> lbs)
        {
            this.lbs = lbs;
            listBox1.Items.Clear();
            foreach (var bs in lbs)
            {
                listBox1.Items.Add(bs);
            }
            listBox1.Items.AddRange(MapBuilder.loadedMap.mapObjectGroups.ToArray());
        }

        public void AssignScriptList(List<BaseScript> ls)
        {
            this.ls = ls;
            listBox2.Items.Clear();
            foreach (var s in ls)
            {
                listBox2.Items.Add(s);
            }

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            e.Cancel = true;
            Hide();
        }

        private void ResetButtonEvents()
        {
            bPressedSaveButton = false;
            bPressedMMB = false;
            bPresedsTSMB = false;
            bPressedTEB = false;
            progress = 0;
        }

        public void doSomething(Bitmap bmp)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages[0])
            {
                PB1.Image = bmp;
                PB1.Refresh();
            }

        }

        private void SaveMapButton_Click(object sender, EventArgs e)
        {

            MapBuilder.SaveMap();
            progress = 0;
        }

        private void MapMakerButton_Click(object sender, EventArgs e)
        {
            MapBuilder.StandardMapBuilder();
        }

        private void TSMakerButton_Click(object sender, EventArgs e)
        {
            bPresedsTSMB = true;
        }

        private void TileEditorButton_Click(object sender, EventArgs e)
        {
            bPressedTEB = true;
        }

        private void LoadNewTSButton_Click(object sender, EventArgs e)
        {
            TileSheet ts;
            bool bTSSelected = false;

            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Title = "Open new .cgts Tilesheet";
            if (Game1.bIsDebug)
            {
                openFileDialog1.InitialDirectory = Game1.rootTBAGW;
                openFileDialog1.Filter = "CG Files (.cgts)|*.cgts;*.cgtsc";

            }
            else
            {
                openFileDialog1.InitialDirectory = Game1.rootContentExtra;
                openFileDialog1.Filter = "CG Files (.cgts)|*.cgts";
            }

            System.Windows.Forms.DialogResult dia = openFileDialog1.ShowDialog();

            if (Game1.bIsDebug)
            {

                if (dia == System.Windows.Forms.DialogResult.OK && openFileDialog1.FileName.Contains(openFileDialog1.InitialDirectory))
                {
                    bTSSelected = true;
                }
            }
            else
            {


                if (dia == System.Windows.Forms.DialogResult.OK && openFileDialog1.FileName.Contains(openFileDialog1.InitialDirectory))
                {
                    bTSSelected = true;
                }
            }

            if (bTSSelected)
            {
                ts = EditorFileWriter.TileSheetReader(openFileDialog1.FileName);
                MapBuilder.OpenNewTileSheet(ts);
            }
            openFileDialog1.Reset();
        }

        private void OpenTSButton_Click(object sender, EventArgs e)
        {
            bTSOpen = !bTSOpen;
        }

        private void AMButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(Directory.Exists(Environment.CurrentDirectory + @"\Content\"));

            AnimationEditor.Start();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            MapBuilder.StandardMapBuilder();
        }

        public static void MakeSureFormClosed(Form f)
        {
            if (f != null && !f.IsDisposed)
            {
                f.Close();
                f.Dispose();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Game1.bIsDebug)
            {
                button39.Enabled = true;
            }
            else
            {
                button39.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Editor.currentEditor = (int)Editor.EditorsCollection.SpriteEditor;

            MakeSureFormClosed(spriteEditForm);
            spriteEditForm = new SpriteEditorForm();
            spriteEditForm.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MapBuilder.bObjectLayer = true;
            tabControl1.SelectedIndex = 3;
            tabControl2.SelectedIndex = 3;
        }

        public void Progress(int i)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar2.Value = i;
            Console.WriteLine(progressBar2.Value);

            if (progressBar2.Value == 25)
            {
                label4.Text = "Progress: Starting save success";
                Refresh();
            }
            else if (progressBar2.Value == 50)
            {
                label4.Text = "Progress: Save succesful -starting encryption";
                Refresh();
            }
            else if (progressBar2.Value == 75)
            {
                label4.Text = "Progress: Encryption completed successfully";
                Refresh();
            }
            else if (progressBar2.Value == 100)
            {
                label4.Text = "Progress: DONE";
                Refresh();
            }
        }

        private void numInput_ValueChanged(object sender, EventArgs e)
        {
            if (numInput.Value == 0)
            {
                label5.Text = "Ground layer";
                Refresh();
            }
            else if (numInput.Value == 1)
            {
                label5.Text = "Ground opacity layer";
                Refresh();
            }
            else if (numInput.Value == 2)
            {
                label5.Text = "Ground opacity layer 2";
                Refresh();
            }
            else if (numInput.Value == 3)
            {
                label5.Text = "Height layer";
                Refresh();
            }

            layerDepth = (int)numInput.Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("You sure want to exit Map editing? NOTE: be sure to save first!", "Leaving", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                MapEditor.bIsRunning = true;
                MakeSureFormClosed(MapBuilder.oiw);
                MapBuilder.oiw = new ObjectInfoWindow();
                this.Close();
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                //do something else
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            MapBuilder.brushSize = (int)numericUpDown1.Value;
            MapBuilder.CalculateBrushSize();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MapBuilder.ActivateScriptLayer();
        }
        public static ObjectSelector ojs = null;
        private void button7_Click_1(object sender, EventArgs e)
        {
            MakeSureFormClosed(ojs);
            MapBuilder.controls.currentType = Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerAdding;
            MapBuilder.bObjectLayer = true;
            ojs = new ObjectSelector();
            ojs.Start();
            listBox1.SelectedIndex = -1;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            button4.PerformClick();
            button4_Click(sender, e);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                MakeSureFormClosed(ojs);
                MapBuilder.controls.currentType = Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing;

                var selectedItem = listBox1.SelectedItem;

                if (selectedItem is BaseSprite)
                {
                    var sprite = selectedItem as BaseSprite;
                    MapBuilder.objSelection.Assign(sprite);
                    //MapBuilder.selectedSprite = lbs[listBox1.SelectedIndex];
                    // so.ProcessScript(((BaseSprite)(listBox1.Items[listBox1.SelectedIndex])).script);

                    textBox2.Text = sprite.shapeName;
                    numericUpDown5.Value = sprite.shapeID;
                    numericUpDown6.Value = (decimal)sprite.position.X;
                    numericUpDown7.Value = (decimal)sprite.position.Y;
                }
                else if (selectedItem is ObjectGroup)
                {
                    ObjectGroup temp = (selectedItem as ObjectGroup);
                    MapBuilder.objSelection.Assign(temp);

                    //MapBuilder.selectedSprite = temp.mainController();
                    // so.ProcessScript((MapBuilder.selectedSprite).script);

                    textBox2.Text = temp.groupName;
                    numericUpDown5.Value = temp.groupID;
                    numericUpDown6.Value = (decimal)temp.mainController().position.X;
                    numericUpDown7.Value = (decimal)temp.mainController().position.Y;
                }

                button13.Visible = true;
            }
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            AssignScriptList(MapBuilder.parentMap.listOfAllUniqueScripts);
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            AssignSpriteList(MapBuilder.loadedMap.mapSprites);
            listBox1.SelectedIndex = -1;
            button22.Visible = true;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            so.ProcessScript(((BaseScript)(listBox2.Items[listBox2.SelectedIndex])));
        }

        private void listBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }
        #region PREVENTS KEYBOARD CONTROL LISTBOX
        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void listBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void listBox2_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void listBox2_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void listBox3_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void listBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void listBox3_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        #endregion

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            listBox3.DataSource = null;
            listBox3.DataSource = MapBuilder.gcDB.gameSourceTiles;

        }


        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                button11.Visible = true;
            }
            else
            {
                button11.Visible = false;
            }
            // Console.WriteLine(listBox4.Text.IndexOf("x"));

        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {
            listBox4.SelectedIndex = -1;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                ResolutionUtility.AdjustResolution(int.Parse(listBox4.Text.Substring(0, listBox4.Text.IndexOf("x"))),
                    int.Parse(listBox4.Text.Substring(listBox4.Text.IndexOf("x") + 1)),
                    Game1.graphics);
            }
        }

        private void tabControl1_Enter(object sender, EventArgs e)
        {

        }

        private void LayerControl_Enter(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 0;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            GameProcessor.gcDB = MapBuilder.gcDB;
            ResolutionUtility.AdjustResolution(playTestRes.X, playTestRes.Y, Game1.graphics);

            //ResolutionUtility.gameScale.X = playTestRes.X / 1366;
            //ResolutionUtility.gameScale.Y = playTestRes.Y / 768;
            //Console.WriteLine(ResolutionUtility.gameScale);

            ScriptProcessor.gameMap = MapBuilder.loadedMap;
            GameProcessor.LaunchForEditor(MapBuilder.loadedMap);
            MapBuilder.bPlayTest = true;
            Hide();

        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedIndex != -1)
            {
                editorRes = new Vector2(Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight);
                Console.WriteLine(editorRes);
                playTestRes = new Vector2(int.Parse(listBox5.Text.Substring(0, listBox5.Text.IndexOf("x"))),
    int.Parse(listBox5.Text.Substring(listBox5.Text.IndexOf("x") + 1)));
                button2.Visible = true;
            }
            else
            {
                button2.Visible = false;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                MapBuilder.parentMap.mapSprites.RemoveAt(listBox1.SelectedIndex);
                AssignSpriteList(MapBuilder.parentMap.mapSprites);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {

            charEditorForm.tabControl1.SelectedIndex = 1;
            charEditorForm.tabControl1.SelectedTab = charEditorForm.tabControl1.TabPages[1];
            charEditorForm.Show();
        }

        private void tabPage7_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "SubMap";

            label18.Text = MapBuilder.parentMap.mapName;
            listBox6.DataSource = null;
            listBox6.DataSource = MapBuilder.parentMap.subMaps;

            listBox6.SelectedIndex = -1;

            listBox12.DataSource = MapBuilder.gcDB.gameMapGroups;
            listBox12.SelectedIndex = 0;

        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1 && listBox6.SelectedIndex != MapBuilder.parentMap.subMaps.Count)
            {
                button16.Show();
                button18.Show();
                textBox1.Text = MapBuilder.parentMap.subMaps[listBox6.SelectedIndex].mapName;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                var map = listBox6.SelectedItem as BasicMap;
                MapBuilder.ReloadActiveMap(map);
                listBox19.DataSource = null;
                listBox19.DataSource = map.WPPoints;
            }
        }

        public void getEditorWayPointsReady(BasicMap bm)
        {
            listBox19.DataSource = null;
            listBox19.DataSource = bm.WPPoints;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MapBuilder.gcDB.AddSubMap(MapBuilder.parentMap, new BasicMap());
            textBox7.Text = "";
            ReloadSubMapList(sender, e);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                DialogResult dialogResult = MessageBox.Show("Are you certain about deleting the selected map?", "Delete?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var map = listBox6.SelectedItem as BasicMap;
                    MapBuilder.parentMap.subMaps.Remove(map);
                    listBox6.SelectedIndex = -1;
                    ReloadSubMapList(sender, e);
                }
                else if (dialogResult == DialogResult.No)
                {

                }
            }

        }

        private void ReloadSubMapList(object sender, EventArgs e, int index = -1)
        {
            textBox7_TextChanged(sender, e);
            listBox6.SelectedIndex = index;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            MapBuilder.loadedMap = MapBuilder.parentMap;

            listBox6.Items.Clear();
            foreach (var item in MapBuilder.parentMap.subMaps)
            {
                listBox6.Items.Add(item);
            }
            listBox6.Items.Add(MapBuilder.parentMap);
        }

        private void PB1_Click(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing)
            {
                if (listBox1.SelectedItem is BaseSprite)
                {
                    lbs[listBox1.SelectedIndex].position = new Vector2((float)numericUpDown6.Value, lbs[listBox1.SelectedIndex].position.Y);
                }
                else if (listBox1.SelectedItem is ObjectGroup)
                {
                    //lbs[listBox1.SelectedIndex].position = new Vector2((float)numericUpDown6.Value, lbs[listBox1.SelectedIndex].position.Y);
                    (listBox1.SelectedItem as ObjectGroup).Move(new Vector2((float)numericUpDown6.Value, (listBox1.SelectedItem as ObjectGroup).mainController().position.Y), (MapBuilder.objSelection.selectedObject as ObjectGroup).mainController());
                }

            }
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing)
            {
                if (listBox1.SelectedItem is BaseSprite)
                {
                    lbs[listBox1.SelectedIndex].position = new Vector2(lbs[listBox1.SelectedIndex].position.X, (float)numericUpDown7.Value);
                }
                else if (listBox1.SelectedItem is ObjectGroup)
                {
                    //lbs[listBox1.SelectedIndex].position = new Vector2((float)numericUpDown6.Value, lbs[listBox1.SelectedIndex].position.Y);
                    var mainController = (MapBuilder.objSelection.selectedObject as ObjectGroup).mainController();
                    (listBox1.SelectedItem as ObjectGroup).Move(new Vector2((listBox1.SelectedItem as ObjectGroup).mainController().position.X, (float)numericUpDown7.Value), mainController);
                }
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            MapBuilder.SaveMap();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing)
            {
                if (listBox1.SelectedItem is BaseSprite)
                {
                    lbs[listBox1.SelectedIndex].shapeName = textBox2.Text;
                }
                else if (listBox1.SelectedItem is ObjectGroup)
                {
                    (listBox1.SelectedItem as ObjectGroup).groupName = textBox2.Text;
                }
            }

        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                //lbs[listBox1.SelectedIndex].shapeID = (int)numericUpDown5.Value;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            charEditorForm.Show();
            charEditorForm.tabControl1.SelectedIndex = 5;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                DialogResult dialogResult = MessageBox.Show("Would you like to create a -completely- separate copy of the object with a different ID? If not, every single object with the same ID on this map will have the newly created script.", "Script warning", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    BaseSprite temp = (BaseSprite)((BaseSprite)listBox1.SelectedItem).ShallowCopy();
                    temp.script = new BaseScript();
                    MapBuilder.gcDB.AddScript(temp.script, temp);
                    MapBuilder.gcDB.AddObject(temp);
                    MapBuilder.loadedMap.mapSprites.Remove((BaseSprite)listBox1.SelectedItem);
                    MapBuilder.loadedMap.mapSprites.Add(temp);
                    scriptForm.Show();
                    scriptForm.LoadScript(temp.script);
                    listBox1.SelectedIndex = -1;
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(MapBuilder.loadedMap.mapSprites.ToArray());
                    listBox1.Items.AddRange(MapBuilder.loadedMap.mapObjectGroups.ToArray());
                }
                else if (dialogResult == DialogResult.No)
                {
                    if (listBox1.SelectedIndex != -1)
                    {
                        //   MapBuilder.bCreatingScript = true;
                        scriptForm.Show();
                        scriptForm.LoadScript(MapBuilder.loadedMap.mapSprites[listBox1.SelectedIndex].script);
                        //    ScriptBuilder.Start(ref MapBuilder.parentMap.mapSprites[listBox1.SelectedIndex].script);
                    }
                }
            }


        }

        private void tabPage8_Enter(object sender, EventArgs e)
        {
            listBox7.SelectedIndex = -1;
            listBox8.SelectedIndex = -1;
            lbs = MapBuilder.loadedMap.mapSprites;
            listBox7.Items.Clear();
            listBox8.Items.Clear();
            foreach (var bs in lbs)
            {
                listBox7.Items.Add(bs);
            }
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox7.SelectedIndex != -1)
            {
                listBox8.Items.Clear();
                listBox8.SelectedIndex = -1;
                int i = 0;
                foreach (var item in lbs[listBox7.SelectedIndex].baseAnimations)
                {
                    listBox8.Items.Add(item);
                    lbs[listBox7.SelectedIndex].baseAnimations[i].animationID = i;
                    i++;
                }

                textBox3.Text = lbs[listBox7.SelectedIndex].shapeName;
                numericUpDown8.Value = lbs[listBox7.SelectedIndex].shapeID;


                checkBox1.Checked = lbs[listBox7.SelectedIndex].bIsActive;
                checkBox2.Checked = lbs[listBox7.SelectedIndex].bIsVisible;
                checkBox3.Checked = lbs[listBox7.SelectedIndex].bActivateOnTouch;
                checkBox4.Checked = lbs[listBox7.SelectedIndex].bHasCollision;

                MapBuilder.objSelection.Assign(listBox7.SelectedItem);
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (listBox7.SelectedIndex != -1)
            {
                ShapeAnimation temp = new ShapeAnimation();
                temp.animationTexture = lbs[listBox7.SelectedIndex].shapeTexture;
                temp.texFileLoc = lbs[listBox7.SelectedIndex].textureLoc;
                lbs[listBox7.SelectedIndex].baseAnimations.Add(temp);

                listBox8.Items.Clear();
                listBox8.SelectedIndex = -1;
                int i = 0;
                foreach (var item in lbs[listBox7.SelectedIndex].baseAnimations)
                {
                    listBox8.Items.Add(item);
                    lbs[listBox7.SelectedIndex].baseAnimations[i].animationID = i;
                    i++;
                }


            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                lbs[listBox7.SelectedIndex].baseAnimations.RemoveAt(listBox8.SelectedIndex);

                listBox8.Items.Clear();
                listBox8.SelectedIndex = -1;
                int i = 0;
                foreach (var item in lbs[listBox7.SelectedIndex].baseAnimations)
                {
                    listBox8.Items.Add(item);
                    lbs[listBox7.SelectedIndex].baseAnimations[i].animationID = i;
                    i++;
                }


            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            FrameSelector.Stop();
            MapBuilder.SaveMap();
        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                Texture2D charSprite = lbs[listBox7.SelectedIndex].baseAnimations[listBox8.SelectedIndex].animationTexture;
                byte[] data = new byte[charSprite.Width * charSprite.Height];
                MemoryStream m = new MemoryStream(data);
                charSprite.SaveAsPng(m, charSprite.Width, charSprite.Height);
                Bitmap bmp = new System.Drawing.Bitmap(m);
                pictureBox2.Image = bmp;
                pictureBox2.Refresh();


                textBox4.Text = lbs[listBox7.SelectedIndex].baseAnimations[listBox8.SelectedIndex].animationName;
                numericUpDown9.Value = lbs[listBox7.SelectedIndex].baseAnimations[listBox8.SelectedIndex].animationID;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                lbs[listBox7.SelectedIndex].baseAnimations[listBox8.SelectedIndex].animationName = textBox4.Text;

            }
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                lbs[listBox7.SelectedIndex].baseAnimations[listBox8.SelectedIndex].animationID = (int)numericUpDown9.Value;
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1)
            {
                FrameSelector.StartSimple(lbs[listBox7.SelectedIndex].baseAnimations[listBox8.SelectedIndex]);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (listBox7.SelectedIndex != -1)
            {
            }
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (listBox7.SelectedIndex != -1)
            {
                lbs[listBox7.SelectedIndex].shapeID = (int)numericUpDown8.Value;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (listBox7.SelectedIndex != -1)
            {
                lbs[listBox7.SelectedIndex].bIsActive = !lbs[listBox7.SelectedIndex].bIsActive;
                checkBox1.Checked = lbs[listBox7.SelectedIndex].bIsActive;
            }
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (listBox7.SelectedIndex != -1)
            {
                lbs[listBox7.SelectedIndex].bIsVisible = !lbs[listBox7.SelectedIndex].bIsVisible;
                checkBox2.Checked = lbs[listBox7.SelectedIndex].bIsVisible;
                if (lbs[listBox7.SelectedIndex].bIsVisible)
                {
                    lbs[listBox7.SelectedIndex].spriteOpacity = 100;
                }
                else
                {
                    lbs[listBox7.SelectedIndex].spriteOpacity = 0;
                }
            }
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            if (listBox7.SelectedIndex != -1)
            {
                lbs[listBox7.SelectedIndex].bActivateOnTouch = !lbs[listBox7.SelectedIndex].bActivateOnTouch;
                checkBox3.Checked = lbs[listBox7.SelectedIndex].bActivateOnTouch;
            }
        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button30_Click(object sender, EventArgs e)
        {
            MapBuilder.SaveMap();
        }

        private void tabPage10_Enter(object sender, EventArgs e)
        {
            listBox9.SelectedIndex = -1;
            listBox9.DataSource = null;
            listBox10.SelectedIndex = -1;
            listBox10.Items.Clear();
            listBox9.DataSource = MapBuilder.gcDB.gameScriptBools;
        }

        private void listBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {
                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                numericUpDown10.Value = selectedScriptBool.boolID;
                checkBox8.Checked = selectedScriptBool.isOn;
                textBox5.Text = selectedScriptBool.boolName;

                richTextBox2.Text = selectedScriptBool.boolDescription;
                numericUpDown2.Maximum = selectedScriptBool.choices().Count - 1;

                listBox10.SelectedIndex = -1;
                listBox10.Items.Clear();
                foreach (var choice in selectedScriptBool.choiceText)
                {
                    listBox10.Items.Add(choice);
                }
            }

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
        }

        private void button28_Click(object sender, EventArgs e)
        {
            MapBuilder.gcDB.AddScriptBool(new ScriptBool());
            listBox9.SelectedIndex = -1;
            listBox9.DataSource = null;
            listBox9.DataSource = MapBuilder.gcDB.gameScriptBools;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {
                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                MapBuilder.gcDB.gameScriptBools.Remove(selectedScriptBool);
                listBox9.DataSource = null;
                listBox9.DataSource = MapBuilder.gcDB.gameScriptBools;
            }
        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void button32_Click(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {

                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                numericUpDown2.Maximum = selectedScriptBool.choices().Count - 1;
                listBox10.SelectedIndex = -1;
                listBox10.Items.Clear();
                selectedScriptBool.choiceText.Add("A choice");
                selectedScriptBool.choiceDescription.Add("A choice description");
                foreach (var choice in selectedScriptBool.choiceText)
                {
                    listBox10.Items.Add(choice);
                }
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1 && listBox10.SelectedIndex != -1)
            {

                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                if (selectedScriptBool.scriptChoice == selectedScriptBool.choices().Count - 1)
                {
                    selectedScriptBool.scriptChoice = -1;
                    numericUpDown2.Value = selectedScriptBool.scriptChoice;
                }
                numericUpDown2.Maximum = selectedScriptBool.choices().Count - 1;
                selectedScriptBool.choiceText.RemoveAt(listBox10.SelectedIndex);
                selectedScriptBool.choiceDescription.RemoveAt(listBox10.SelectedIndex);
                listBox10.SelectedIndex = -1;
                listBox10.Items.Clear();
                foreach (var choice in selectedScriptBool.choiceText)
                {
                    listBox10.Items.Add(choice);
                }
            }
        }

        private void listBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox10.SelectedIndex != -1)
            {
                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                richTextBox1.Text = selectedScriptBool.choiceDescription[listBox10.SelectedIndex];
                textBox6.Text = selectedScriptBool.choiceText[listBox10.SelectedIndex];
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (listBox10.SelectedIndex != -1)
            {
                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                selectedScriptBool.choiceDescription[listBox10.SelectedIndex] = richTextBox1.Text;
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {
                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                selectedScriptBool.boolDescription = richTextBox2.Text;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (listBox10.SelectedIndex != -1 && listBox9.SelectedIndex != -1)
            {
                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                selectedScriptBool.choiceText[listBox10.SelectedIndex] = textBox6.Text;
            }
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tabPage9_Enter(object sender, EventArgs e)
        {
            tabControl4.SelectedIndex = 0;
            tabPage10_Enter(sender, e);

        }

        private void checkBox4_Click_1(object sender, EventArgs e)
        {
            if (listBox7.SelectedIndex != -1)
            {

                lbs[listBox7.SelectedIndex].bHasCollision = !lbs[listBox7.SelectedIndex].bHasCollision;
                checkBox4.Checked = lbs[listBox7.SelectedIndex].bHasCollision;

            }
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void button33_Click(object sender, EventArgs e)
        {
            soundEditorForm.Show();
        }

        private void tabPage12_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer10_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            MapBuilder.SaveMap();
        }



        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {
                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                selectedScriptBool.boolName = textBox5.Text;
            }
        }

        private void button42_Click(object sender, EventArgs e)
        {
            var tempSprites = new KeyValuePair<List<BaseSprite>, int>();
            var tempGroups = new KeyValuePair<List<ObjectGroup>, int>();

            if (listBox1.SelectedIndex != -1)
            {
                if (listBox1.SelectedItem is BaseSprite)
                {
                    tempSprites = GenerateSpriteList();



                    if (listBox1.SelectedIndex != -1 && tempSprites.Key.Count != 1)
                    {

                        BaseSprite temp = MapBuilder.loadedMap.mapSprites[listBox1.SelectedIndex];
                        MapBuilder.loadedMap.mapSprites.Remove(temp);

                        if (MapBuilder.loadedMap.mapSprites.IndexOf(tempSprites.Key[tempSprites.Key.Count - 1]) == MapBuilder.loadedMap.mapSprites.Count - 1)
                        {
                            MapBuilder.loadedMap.mapSprites.Add(temp);
                        }
                        else
                        {
                            MapBuilder.loadedMap.mapSprites.Insert(MapBuilder.loadedMap.mapSprites.IndexOf(tempSprites.Key[tempSprites.Key.Count - 1]), temp);
                        }

                        AssignSpriteList(MapBuilder.loadedMap.mapSprites);
                        listBox1.SelectedIndex = listBox1.Items.IndexOf(temp);
                    }
                }
                else if (listBox1.SelectedItem is ObjectGroup)
                {
                    tempGroups = GenerateGroupList();

                    if (listBox1.SelectedIndex != -1 && tempGroups.Key.Count != 1)
                    {

                        ObjectGroup temp = listBox1.SelectedItem as ObjectGroup;

                        MapBuilder.loadedMap.mapObjectGroups.Remove(temp);

                        if (MapBuilder.loadedMap.mapObjectGroups.IndexOf(tempGroups.Key[tempGroups.Key.Count - 1]) == MapBuilder.loadedMap.mapObjectGroups.Count - 1)
                        {
                            MapBuilder.loadedMap.mapObjectGroups.Add(temp);
                        }
                        else
                        {
                            MapBuilder.loadedMap.mapObjectGroups.Insert(0, temp);
                        }

                        AssignSpriteList(MapBuilder.loadedMap.mapSprites);
                        listBox1.SelectedIndex = listBox1.Items.IndexOf(temp);
                    }
                }
            }
        }

        private KeyValuePair<List<BaseSprite>, int> GenerateSpriteList()
        {
            List<BaseSprite> tempSprites = MapBuilder.loadedMap.mapSprites.FindAll(msp => msp.spriteGameSize.Contains(((BaseSprite)(listBox1.SelectedItem)).spriteGameSize)
            || msp.spriteGameSize.Intersects(((BaseSprite)(listBox1.SelectedItem)).spriteGameSize)
            || msp == (BaseSprite)(listBox1.SelectedItem));
            int spriteIndex = 0;

            if (tempSprites.Count == 1)
            {
                spriteIndex = 0;
            }
            else if (tempSprites.Count > 1)
            {

                spriteIndex = tempSprites.IndexOf((BaseSprite)listBox1.SelectedItem);
            }

            return new KeyValuePair<List<BaseSprite>, int>(tempSprites, spriteIndex);
        }

        private void button43_Click(object sender, EventArgs e)
        {
            var tempSprites = new KeyValuePair<List<BaseSprite>, int>();
            var tempGroups = new KeyValuePair<List<ObjectGroup>, int>();


            if (listBox1.SelectedIndex != -1)
            {
                if (listBox1.SelectedItem is BaseSprite)
                {
                    tempSprites = GenerateSpriteList();


                    if (listBox1.SelectedIndex != -1 && tempSprites.Key.Count != 1)
                    {

                        BaseSprite temp = MapBuilder.loadedMap.mapSprites[listBox1.SelectedIndex];
                        MapBuilder.loadedMap.mapSprites.Remove(temp);
                        try
                        {
                            MapBuilder.loadedMap.mapSprites.Insert(MapBuilder.loadedMap.mapSprites.IndexOf(tempSprites.Key[tempSprites.Value - 1]), temp);
                        }
                        catch
                        {
                            MapBuilder.loadedMap.mapSprites.Insert(listBox1.SelectedIndex, temp);
                        }
                        AssignSpriteList(MapBuilder.loadedMap.mapSprites);
                        listBox1.SelectedIndex = MapBuilder.loadedMap.mapSprites.IndexOf(temp);
                    }
                }
                else if (listBox1.SelectedItem is ObjectGroup)
                {
                    tempGroups = GenerateGroupList();

                    if (listBox1.SelectedIndex != -1 && tempGroups.Key.Count != 1)
                    {

                        ObjectGroup temp = listBox1.SelectedItem as ObjectGroup;
                        MapBuilder.loadedMap.mapObjectGroups.Remove(temp);
                        try
                        {
                            MapBuilder.loadedMap.mapObjectGroups.Insert(MapBuilder.loadedMap.mapObjectGroups.IndexOf(tempGroups.Key[tempGroups.Value - 1]), temp);
                        }
                        catch
                        {
                            MapBuilder.loadedMap.mapObjectGroups.Insert(listBox1.SelectedIndex - MapBuilder.loadedMap.mapSprites.Count, temp);
                        }
                        AssignSpriteList(MapBuilder.loadedMap.mapSprites);
                        listBox1.SelectedIndex = listBox1.Items.IndexOf(temp);
                    }
                }
            }

        }

        private void button45_Click(object sender, EventArgs e)
        {

            var tempSprites = new KeyValuePair<List<BaseSprite>, int>();
            var tempGroups = new KeyValuePair<List<ObjectGroup>, int>();

            if (listBox1.SelectedIndex != -1)
            {
                if (listBox1.SelectedItem is BaseSprite)
                {
                    tempSprites = GenerateSpriteList();

                    if (listBox1.SelectedIndex != -1 && tempSprites.Key.Count != 1)
                    {

                        BaseSprite temp = MapBuilder.loadedMap.mapSprites[listBox1.SelectedIndex];
                        MapBuilder.loadedMap.mapSprites.Remove(temp);
                        try
                        {
                            MapBuilder.loadedMap.mapSprites.Insert(MapBuilder.loadedMap.mapSprites.IndexOf(tempSprites.Key[0]), temp);
                        }
                        catch
                        {
                            MapBuilder.loadedMap.mapSprites.Add(temp);
                        }

                        AssignSpriteList(MapBuilder.loadedMap.mapSprites);
                        listBox1.SelectedIndex = MapBuilder.loadedMap.mapSprites.IndexOf(temp);
                    }
                }
                else if (listBox1.SelectedItem is ObjectGroup)
                {
                    tempGroups = GenerateGroupList();

                    if (listBox1.SelectedIndex != -1 && tempGroups.Key.Count != 1)
                    {

                        ObjectGroup temp = listBox1.SelectedItem as ObjectGroup;
                        MapBuilder.loadedMap.mapObjectGroups.Remove(temp);
                        try
                        {
                            MapBuilder.loadedMap.mapObjectGroups.Insert(MapBuilder.loadedMap.mapObjectGroups.IndexOf(tempGroups.Key[0]), temp);
                        }
                        catch
                        {
                            MapBuilder.loadedMap.mapObjectGroups.Add(temp);
                        }

                        AssignSpriteList(MapBuilder.loadedMap.mapSprites);
                        listBox1.SelectedIndex = listBox1.Items.IndexOf(temp);
                    }
                }

            }


        }

        private KeyValuePair<List<ObjectGroup>, int> GenerateGroupList()
        {
            ObjectGroup tempGroup = listBox1.SelectedItem as ObjectGroup;
            List<ObjectGroup> tempGroups = MapBuilder.loadedMap.mapObjectGroups.FindAll(msp => msp.Contains((tempGroup))
             || msp == tempGroup);
            int spriteIndex = 0;

            if (tempGroups.Count == 1)
            {
                spriteIndex = 0;
            }
            else if (tempGroups.Count > 1)
            {

                spriteIndex = tempGroups.IndexOf(tempGroup);
            }

            return new KeyValuePair<List<ObjectGroup>, int>(tempGroups, spriteIndex);
        }

        private void button44_Click(object sender, EventArgs e)
        {
            var tempSprites = new KeyValuePair<List<BaseSprite>, int>();
            var tempGroups = new KeyValuePair<List<ObjectGroup>, int>();

            if (listBox1.SelectedIndex != -1)
            {
                if (listBox1.SelectedItem is BaseSprite)
                {
                    tempSprites = GenerateSpriteList();

                    if (listBox1.SelectedIndex != -1 && tempSprites.Key.Count != 1)
                    {

                        BaseSprite temp = MapBuilder.loadedMap.mapSprites[listBox1.SelectedIndex];
                        MapBuilder.loadedMap.mapSprites.Remove(temp);
                        try
                        {
                            MapBuilder.loadedMap.mapSprites.Insert(MapBuilder.loadedMap.mapSprites.IndexOf(tempSprites.Key[tempSprites.Value + 2]), temp);
                        }
                        catch
                        {
                            MapBuilder.loadedMap.mapSprites.Add(temp);
                        }
                        AssignSpriteList(MapBuilder.loadedMap.mapSprites);
                        listBox1.SelectedIndex = MapBuilder.loadedMap.mapSprites.IndexOf(temp);
                    }
                }
                else if (listBox1.SelectedItem is ObjectGroup)
                {
                    tempGroups = GenerateGroupList();

                    if (listBox1.SelectedIndex != -1 && tempGroups.Key.Count != 1)
                    {

                        ObjectGroup temp = listBox1.SelectedItem as ObjectGroup; ;
                        MapBuilder.loadedMap.mapObjectGroups.Remove(temp);
                        try
                        {
                            MapBuilder.loadedMap.mapObjectGroups.Insert(MapBuilder.loadedMap.mapObjectGroups.IndexOf(tempGroups.Key[tempGroups.Value + 2]), temp);
                        }
                        catch
                        {
                            MapBuilder.loadedMap.mapObjectGroups.Add(temp);
                        }
                        AssignSpriteList(MapBuilder.loadedMap.mapSprites);
                        listBox1.SelectedIndex = listBox1.Items.IndexOf(temp);
                    }
                }
            }
        }


        private void button46_Click(object sender, EventArgs e)
        {
            objGForm.Start();
            objGForm.Show();
        }

        private void button47_Click(object sender, EventArgs e)
        {
            if (listBox15.SelectedIndex != -1)
            {
                MapBuilder.StartRegionEditing(((MapRegion)listBox15.SelectedItem));
            }

        }

        private void tabPage11_Enter(object sender, EventArgs e)
        {
            textBox9.Text = "";
            listBox15.Items.Clear();
            listBox15.SelectedIndex = -1;
            listBox14.Items.Clear();
            listBox14.SelectedIndex = -1;
            foreach (var item in MapBuilder.loadedMap.mapRegions)
            {
                listBox15.Items.Add(item);
            }
        }

        private void button48_Click(object sender, EventArgs e)
        {
            if (!textBox9.Text.Equals("") && MapBuilder.loadedMap.mapRegions.Find(reg => reg.regionName.Equals(textBox9.Text)) == null)
            {
                MapRegion temp = new MapRegion();
                temp.regionName = textBox9.Text;
                MapBuilder.loadedMap.mapRegions.Add(temp);

                listBox15.Items.Clear();
                listBox15.SelectedIndex = -1;
                foreach (var item in MapBuilder.loadedMap.mapRegions)
                {
                    listBox15.Items.Add(item);
                }
            }
        }

        private void button49_Click(object sender, EventArgs e)
        {
            if (listBox15.SelectedIndex != -1)
            {
                MapBuilder.loadedMap.mapRegions.RemoveAt(listBox15.SelectedIndex);

                listBox15.Items.Clear();
                listBox15.SelectedIndex = -1;
                foreach (var item in MapBuilder.loadedMap.mapRegions)
                {
                    listBox15.Items.Add(item);
                }
            }
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            if (listBox15.SelectedIndex != -1)
            {
                ((MapRegion)listBox15.SelectedItem).regionID = (int)numericUpDown14.Value;
                listBox15.Items[listBox15.SelectedIndex] = MapBuilder.loadedMap.mapRegions[listBox15.SelectedIndex];
            }
        }

        private void listBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox15.SelectedIndex != -1)
            {
                numericUpDown14.Value = ((MapRegion)listBox15.SelectedItem).regionID;
                textBox10.Text = ((MapRegion)listBox15.SelectedItem).regionName;


                listBox14.Items.Clear();

                listBox14.SelectedIndex = -1;
                foreach (var zone in ((MapRegion)listBox15.SelectedItem).regionZones)
                {
                    listBox14.Items.Add(zone);
                }
            }
        }

        private void button52_Click(object sender, EventArgs e)
        {
            MapBuilder.SaveMap();
        }

        private void button51_Click(object sender, EventArgs e)
        {
            if (!textBox11.Text.Equals("") && listBox15.SelectedIndex != -1 && MapBuilder.loadedMap.mapRegions[listBox15.SelectedIndex].regionZones.Find(reg => reg.zoneName.Equals(textBox11.Text)) == null)
            {
                MapZone temp = new MapZone();
                temp.zoneName = textBox11.Text;
                MapBuilder.loadedMap.mapRegions[listBox15.SelectedIndex].regionZones.Add(temp);

                listBox14.Items.Clear();
                listBox14.SelectedIndex = -1;
                foreach (var item in MapBuilder.loadedMap.mapRegions[listBox15.SelectedIndex].regionZones)
                {
                    listBox14.Items.Add(item);
                }
            }
        }

        private void button50_Click(object sender, EventArgs e)
        {
            if (listBox14.SelectedIndex != -1)
            {
                MapBuilder.loadedMap.mapRegions[listBox15.SelectedIndex].regionZones.RemoveAt(listBox14.SelectedIndex);

                listBox14.Items.Clear();
                listBox14.SelectedIndex = -1;
                foreach (var item in MapBuilder.loadedMap.mapRegions[listBox15.SelectedIndex].regionZones)
                {
                    listBox14.Items.Add(item);
                }
            }
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            if (listBox14.SelectedIndex != -1)
            {
                ((MapRegion)listBox15.SelectedItem).regionZones[listBox14.SelectedIndex].zoneID = (int)numericUpDown15.Value;
                listBox14.Items[listBox14.SelectedIndex] = MapBuilder.loadedMap.mapRegions[listBox15.SelectedIndex].regionZones[listBox14.SelectedIndex];
            }
        }

        private void listBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox14.SelectedIndex != -1)
            {
                numericUpDown15.Value = ((MapRegion)listBox15.SelectedItem).regionZones[listBox14.SelectedIndex].zoneID;
                textBox12.Text = ((MapRegion)listBox15.SelectedItem).regionZones[listBox14.SelectedIndex].zoneName;
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (listBox15.SelectedIndex != -1)
            {
                ((MapRegion)listBox15.SelectedItem).regionName = textBox10.Text;
                listBox15.Items[listBox15.SelectedIndex] = MapBuilder.loadedMap.mapRegions[listBox15.SelectedIndex];
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (listBox14.SelectedIndex != -1)
            {
                ((MapRegion)listBox15.SelectedItem).regionZones[listBox14.SelectedIndex].zoneName = textBox12.Text;
                listBox14.Items[listBox14.SelectedIndex] = MapBuilder.loadedMap.mapRegions[listBox15.SelectedIndex].regionZones[listBox14.SelectedIndex];
            }
        }

        private void button53_Click(object sender, EventArgs e)
        {
            if (listBox15.SelectedIndex != -1)
            {
                MapBuilder.StartZoneEditing(((MapZone)listBox14.SelectedItem));
            }
        }

        private void button55_Click(object sender, EventArgs e)
        {
            if (listBox14.SelectedIndex != -1)
            {
                zoneEditor.Start((MapZone)listBox14.SelectedItem, (MapRegion)listBox15.SelectedItem);
            }
        }

        private void button54_Click(object sender, EventArgs e)
        {
            if (listBox15.SelectedIndex != -1)
            {
                regionEditor.Start((MapRegion)listBox15.SelectedItem);
            }
        }

        private void button56_Click(object sender, EventArgs e)
        {
            abilityEditor.Start();
        }

        private void button57_Click(object sender, EventArgs e)
        {
            //Process p = Process.Start(Environment.CurrentDirectory + @"\ParticleEditor.exe");
            new ParticleEditor().Start();
        }

        private void button58_Click(object sender, EventArgs e)
        {

        }

        static public ItemCreator ic = null;
        private void button59_Click(object sender, EventArgs e)
        {
            if (ic == null || ic.IsDisposed)
            {
                ic = new ItemCreator();
                ic.Start();
            }

        }
        ObjectManager om = null;
        private void button60_Click(object sender, EventArgs e)
        {
            if (om == null || om.IsDisposed)
            {
                ObjectManager om = new ObjectManager();
                om.Start();
            }

        }

        private void button61_Click(object sender, EventArgs e)
        {
            OpenFileDialog savePrefab = new OpenFileDialog();
            if (TBAGW.Game1.bIsDebug)
            {
                savePrefab.Filter = "cgdb file|*.cgdbc";
                savePrefab.InitialDirectory = TBAGW.Game1.rootTBAGW;

            }
            else
            {
                savePrefab.Filter = "cgdb file|*.cgdb";
                savePrefab.InitialDirectory = TBAGW.Game1.rootContentExtra;
            }

            savePrefab.Title = "import game content database";
            DialogResult dia = savePrefab.ShowDialog();
            if (DialogResult.OK == dia && savePrefab.FileName.Contains(savePrefab.InitialDirectory))
            {
                String fi = savePrefab.FileName;
                MapBuilder.gcDB = EditorFileWriter.gcdbReader(fi);
            }
            else if (System.Windows.Forms.DialogResult.Cancel == dia)
            {

                System.Windows.Forms.MessageBox.Show("Cancelled, returning to Editor.");
            }
            else if (!savePrefab.FileName.Contains(savePrefab.InitialDirectory))
            {
                System.Windows.Forms.MessageBox.Show("Save within mods folder please.");
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (textBox13.Text.Equals(""))
            {
                listBox3.DataSource = null;
                listBox3.DataSource = (MapBuilder.gcDB.gameSourceTiles);
            }
            else if (!textBox13.Text.Equals(""))
            {
                listBox3.DataSource = null;
                //listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectObjects.FindAll(o => o.shapeName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                listBox3.DataSource = (MapBuilder.gcDB.gameSourceTiles.FindAll(i => i.tileName.IndexOf(textBox13.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
            }
        }

        Forms.Animation.AnimationEditor ae = new Forms.Animation.AnimationEditor();
        private void button62_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                Form1.MakeSureFormClosed(ae);
                ae = new Forms.Animation.AnimationEditor();
                ae.Start(((TileSource)listBox3.SelectedItem).tileAnimation);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FrameSelector.Stop();
        }

        private void button63_Click(object sender, EventArgs e)
        {
            if (MapBuilder.gcDB.gameSourceTiles.Find(gst => gst.tileName.Equals(textBox14.Text)) == null)
            {
                var t = new TileSource();
                t.tileName = textBox14.Text;
                t.ReloadTextures();

                MapBuilder.gcDB.AddTile(t);

                listBox3.DataSource = null;
                listBox3.DataSource = MapBuilder.gcDB.gameSourceTiles;
            }
        }

        private void button64_Click(object sender, EventArgs e)
        {
            MapBuilder.bAllowEditing = false;
            MapBuilder.paintTile = null;
            MapBuilder.bShowPaint = false;
            listBox3.SelectedIndex = -1;
        }

        private void listBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1 && listBox13.SelectedIndex != -1)
            {
                BasicTile temp = new BasicTile();
                temp.tileSource = ((TileSource)listBox3.SelectedItem);
                temp.tsID = ((TileSource)listBox3.SelectedItem).tileID;
                MapBuilder.paintTile = temp;
                MapBuilder.bAllowEditing = true;
                MapBuilder.specialBrush.Clear();
                defaultLayerNumericTile.Value = ((TileSource)listBox3.SelectedItem).defaultLayer;
                listBox13.SelectedIndex = (int)temp.tileSource.tileType;
                MapBuilder.controls.currentType = Scenes.Editor.MapEditorSub.MapEditorControls.controlType.TileLayer;
                MapBuilder.smartBrush.Start(temp);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button65_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                // ((TileSource)listBox3.SelectedItem).tileHitBoxes.Add(new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64));
                HitboxEditor.Start((listBox3.SelectedItem as TileSource), 64, 64);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            GameClassCreator gcc = new GameClassCreator();
            gcc.Start();

        }

        private void button37_Click(object sender, EventArgs e)
        {
            SoundFXCreator sfxc = new SoundFXCreator();
            sfxc.Start();
        }

        private void button39_Click(object sender, EventArgs e)
        {
            FTPWindow FTPwin = new FTPWindow();
            FTPwin.Show();
        }

        private void Functions_Click(object sender, EventArgs e)
        {

        }

        private void defaultLayerNumericTile_ValueChanged(object sender, EventArgs e)
        {
            if (numInput.Value == 0)
            {
                label46.Text = "Ground layer";
                Refresh();
            }
            else if (numInput.Value == 1)
            {
                label46.Text = "Ground opacity layer";
                Refresh();
            }
            else if (numInput.Value == 2)
            {
                label46.Text = "Height layer";
                Refresh();
            }

            ((TileSource)listBox3.SelectedItem).defaultLayer = (int)defaultLayerNumericTile.Value;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button38_Click(object sender, EventArgs e)
        {
            GlobalObjectEditor goe = new GlobalObjectEditor();
            goe.Start();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void xOffSet_ValueChanged(object sender, EventArgs e)
        {

        }

        PAEditor pe = new PAEditor();
        private void button40_Click(object sender, EventArgs e)
        {
            MakeSureFormClosed(pe);
            pe = new PAEditor();
            pe.Start();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            String gName = listBox12.SelectedItem as String;

            if (textBox7.Text.Equals(""))
            {
                listBox6.DataSource = null;
                var tempList = MapBuilder.parentMap.subMapsByGroup(gName);
                listBox6.DataSource = tempList;
            }
            else if (!textBox7.Text.Equals(""))
            {
                listBox6.DataSource = null;
                var tempList = MapBuilder.parentMap.subMapsByGroup(gName);
                listBox6.DataSource = tempList.FindAll(i => i.mapName.IndexOf(textBox7.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            MapBuilder.ReloadActiveMap();
            getEditorWayPointsReady(MapBuilder.parentMap);
        }

        private void listBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadSubMapList(sender, e);
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                var map = listBox6.SelectedItem as BasicMap;
                map.mapName = textBox1.Text;
                ReloadSubMapList(sender, e, listBox6.SelectedIndex);
            }
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                var map = listBox6.SelectedItem as BasicMap;
                MapBuilder.eprList.Add(new EditorPreviewRender(map.GetType(), map));
            }
        }

        private void Overview_Click(object sender, EventArgs e)
        {

        }

        private void button21_Click_1(object sender, EventArgs e)
        {
            MapBuilder.controls.currentType = Scenes.Editor.MapEditorSub.MapEditorControls.controlType.None;
            MapBuilder.controls.tileType = Scenes.Editor.MapEditorSub.MapEditorControls.tileLayerType.normal;
            MapBuilder.controls.currentObjectEditType = Scenes.Editor.MapEditorSub.MapEditorControls.objectLayerType.normal;
        }

        private void button41_Click(object sender, EventArgs e)
        {
            MapBuilder.controls.currentType = Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing;
            MapBuilder.objSelection = new EditorObjectSelection();
        }

        private void button66_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(MapBuilder.loadedMap.mapSprites.ToArray());
            listBox1.Items.AddRange(MapBuilder.loadedMap.mapObjectGroups.ToArray());
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void listBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1 && listBox13.SelectedIndex != -1)
            {
                var temp = listBox3.SelectedItem as TileSource;
                temp.tileType = (TileSource.TileType)listBox13.SelectedIndex;
            }
        }

        ProcessSheetForm psf = new ProcessSheetForm();
        private void button67_Click(object sender, EventArgs e)
        {
            MakeSureFormClosed(psf);
            psf = new ProcessSheetForm();
            psf.Show();
        }

        private void tabControl6_Click(object sender, EventArgs e)
        {
            listBox16.Items.Clear();
            listBox16.Items.Add("NONE");
            listBox16.Items.AddRange(BasicMap.allMapsGame().ToArray());
            var list = MapObjectHelpClass.objectsToUpdateOutsideOfMap.FindAll(o => o.GetType() == typeof(NPC)).Cast<NPC>().ToList();
            listBox17.Items.AddRange(list.ToArray());
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            BasicMap mapToSearchFrom = null;
            if (listBox16.SelectedIndex != -1)
            {
                if (listBox16.SelectedIndex != 0)
                {
                    mapToSearchFrom = listBox16.SelectedItem as BasicMap;
                }
            }
            if (textBox8.Text.Equals(""))
            {
                listBox17.SelectedIndex = -1;
                listBox17.Items.Clear();
                var list = MapObjectHelpClass.objectsToUpdateOutsideOfMap.FindAll(o => o.GetType() == typeof(NPC)).Cast<NPC>().ToList();
                if (mapToSearchFrom == null)
                {
                    listBox17.Items.AddRange(list.ToArray());
                }
                else
                {
                    listBox17.Items.AddRange(list.FindAll(npc => npc.IsOnCurrentMap(mapToSearchFrom)).ToArray());
                }
            }
            else
            {
                listBox17.SelectedIndex = -1;
                listBox17.Items.Clear();

                var list = MapObjectHelpClass.objectsToUpdateOutsideOfMap.FindAll(o => o.GetType() == typeof(NPC)).Cast<NPC>().ToList();
                if (mapToSearchFrom == null)
                {
                    listBox17.Items.AddRange(list.FindAll(o => o.ToString().IndexOf(textBox8.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                }
                else
                {
                    listBox17.Items.AddRange(list.FindAll(npc => npc.IsOnCurrentMap(mapToSearchFrom)).FindAll(o => o.ToString().IndexOf(textBox8.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                }

            }
        }

        private void splitContainer25_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox17.SelectedIndex != -1)
            {
                var temp = listBox17.SelectedItem as NPC;
                textBox15.Text = temp.npcName;
                textBox16.Text = temp.npcDisplayName;
                richTextBox3.Text = temp.npcDescription;
            }
        }

        private void button68_Click(object sender, EventArgs e)
        {
            if (listBox17.SelectedIndex != -1)
            {
                var temp = listBox17.SelectedItem as NPC;
                MapBuilder.controls.currentType = Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing;
                MapBuilder.objSelection.Assign(temp);
                MapBuilder.objSelection.OpenEditWindow();
                MapBuilder.ReloadActiveMap(BasicMap.allMapsGame().Find(map => map.identifier == temp.mapActiveID));
                MapBuilder.Center(temp.baseCharacter.trueMapSize());
            }
        }

        private void tabControl2_Click(object sender, EventArgs e)
        {
            listBox16.Items.Clear();
            listBox16.Items.Add("NONE");
            listBox16.Items.AddRange(BasicMap.allMapsGame().ToArray());
            var list = MapObjectHelpClass.objectsToUpdateOutsideOfMap.FindAll(o => o.GetType() == typeof(NPC)).Cast<NPC>().ToList();
            listBox17.Items.AddRange(list.ToArray());
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer3_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox19_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox19.SelectedIndex != -1)
            {
                var temp = listBox19.SelectedItem as MapEditorWaypoint;
                richTextBox4.Text = temp.WPDescription;
                textBox17.Text = temp.WPName;
            }
        }

        private void button69_Click(object sender, EventArgs e)
        {
            MapEditorWaypoint temp = new MapEditorWaypoint();
            temp.WPPosition = new Vector2(MapBuilder.cameraPosX, MapBuilder.cameraPosY);
            MapBuilder.loadedMap.WPPoints.Add(temp);
            getEditorWayPointsReady(MapBuilder.loadedMap);
        }

        private void button70_Click(object sender, EventArgs e)
        {
            if (listBox19.SelectedIndex != -1)
            {
                MapBuilder.loadedMap.WPPoints.RemoveAt(listBox19.SelectedIndex);
                getEditorWayPointsReady(MapBuilder.loadedMap);
            }
        }

        private void button71_Click(object sender, EventArgs e)
        {
            if (listBox19.SelectedIndex != -1)
            {
                MapBuilder.loadedMap.WPPoints[listBox19.SelectedIndex].WPPosition = new Vector2(MapBuilder.cameraPosX, MapBuilder.cameraPosY);
            }
        }

        private void button72_Click(object sender, EventArgs e)
        {
            if (listBox19.SelectedIndex != -1)
            {
                var temp = listBox19.SelectedItem as MapEditorWaypoint;
                MapBuilder.UpdateCameraPosition(temp.WPPosition);
            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            if (listBox19.SelectedIndex != -1)
            {
                var temp = listBox19.SelectedItem as MapEditorWaypoint;
                temp.WPName = textBox17.Text;
            }
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (listBox19.SelectedIndex != -1)
            {
                var temp = listBox19.SelectedItem as MapEditorWaypoint;
                temp.WPDescription = richTextBox4.Text;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            MapBuilder.bShowInvisibleObjects = checkBox7.Checked;
        }

        private void button73_Click(object sender, EventArgs e)
        {
            if (listBox14.SelectedIndex != -1 && listBox15.SelectedIndex != -1)
            {
                MapZone zone = listBox14.SelectedItem as MapZone;


                DialogResult dialogResult = MessageBox.Show("Would you like to create a -completely- separate script for the object ? If not, every single object with the same script ID on this map will have the newly edited script.", "Script warning", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    zone.entranceScript = zone.entranceScript.Clone();
                    MapBuilder.gcDB.AddScript(zone.entranceScript, zone);
                    Form1.MakeSureFormClosed(Form1.scriptForm);
                    Form1.scriptForm = new ScriptBaseForm();
                    Form1.scriptForm.Show();
                    Form1.scriptForm.LoadScript(zone.entranceScript);
                }
                else if (dialogResult == DialogResult.No)
                {
                    Form1.MakeSureFormClosed(Form1.scriptForm);
                    Form1.scriptForm = new ScriptBaseForm();
                    Form1.scriptForm.Show();
                    Form1.scriptForm.LoadScript(zone.entranceScript);
                }
            }
        }

        private void numericUpDown2_ValueChanged_1(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {
                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                selectedScriptBool.scriptChoice = (int)numericUpDown2.Value;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {
                ScriptBool selectedScriptBool = (ScriptBool)listBox9.SelectedItem;
                selectedScriptBool.isOn = checkBox8.Checked;
            }
        }

        private void button74_Click(object sender, EventArgs e)
        {
            MapBuilder.HandleBuildings();
        }

        private void button75_Click(object sender, EventArgs e)
        {
            ReflectionProcessor.ProcessReflection(MapBuilder.loadedMap);
        }
    }
}
