using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.Sprite;

namespace Game1.Forms.ScriptForms.ScriptCommandForms
{
    public partial class TSMCommand : Form
    {
        List<BasicMap> mapsList = new List<BasicMap>();
        List<BaseSprite> shapes = new List<BaseSprite>();

        public TSMCommand()
        {
            InitializeComponent();
        }

        ScriptBaseForm scriptBaseForm;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            this.scriptBaseForm = scriptBaseForm;
            Show();

            mapsList.Clear();
            MapSearch.Text = "";
            ObjectSearch.Text = "";
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            listBox2.SelectedIndex = -1;
            listBox2.Items.Clear();

            foreach (var item in MapBuilder.parentMap.subMaps)
            {
                mapsList.Add(item);
                listBox1.Items.Add(item);
            }
            mapsList.Add(MapBuilder.parentMap);
            listBox1.Items.Add(MapBuilder.parentMap);
        }


        private void TSMCommand_Load(object sender, EventArgs e)
        {

        }

        private void TSMCommand_Enter(object sender, EventArgs e)
        {
            mapsList.Clear();
            MapSearch.Text = "";
            ObjectSearch.Text = "";
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            listBox2.SelectedIndex = -1;
            listBox2.Items.Clear();

            foreach (var item in MapBuilder.parentMap.subMaps)
            {
                mapsList.Add(item);
                listBox1.Items.Add(item);
            }
            mapsList.Add(MapBuilder.parentMap);
            listBox1.Items.Add(MapBuilder.parentMap);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@TSM"+"_"+SMID.Text+"_"+SID.Text);
            Hide();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1) {
                BasicMap temp = (BasicMap)listBox1.SelectedItem;
                listBox2.SelectedIndex = -1;
                listBox2.Items.Clear();
                foreach (var item in temp.mapSprites)
                {
                    shapes.Add(item);
                    listBox2.Items.Add(item);
                }

                SMID.Text = temp.identifier.ToString();
            }
        }

        private void MapSearch_TextChanged(object sender, EventArgs e)
        {
            if (MapSearch.Equals(""))
            {
                listBox1.Items.Clear();
                listBox1.SelectedIndex = -1;
                foreach (var item in mapsList)
                {
                    listBox1.Items.Add(item);
                }
                listBox1.Items.Add(MapBuilder.parentMap);
            }
            else if (!MapSearch.Equals(""))
            {
                listBox1.Items.Clear();
                listBox1.SelectedIndex = -1;
                foreach (var item in mapsList)
                {
                    if(item.mapName.Contains(MapSearch.Text)||item.identifier.ToString().Equals(MapSearch.Text)){
                        listBox1.Items.Add(item);
                    }
                   
                }
                listBox1.Items.Add(MapBuilder.parentMap);
            }
        }

        private void ObjectSearch_TextChanged(object sender, EventArgs e)
        {
            if (ObjectSearch.Equals(""))
            {
                listBox2.Items.Clear();
                listBox2.SelectedIndex = -1;
                foreach (var item in shapes)
                {
                    listBox2.Items.Add(item);
                }
                listBox2.Items.Add(MapBuilder.parentMap);
            }
            else if (!ObjectSearch.Equals(""))
            {
                listBox2.Items.Clear();
                listBox2.SelectedIndex = -1;
                foreach (var item in shapes)
                {
                    if (item.shapeName.Contains(ObjectSearch.Text) || item.shapeID.ToString().Equals(ObjectSearch.Text))
                    {
                        listBox2.Items.Add(item);
                    }

                }
                listBox2.Items.Add(MapBuilder.parentMap);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex!=-1) {
                BaseSprite temp = (BaseSprite)listBox2.SelectedItem;
                SID.Text = temp.objectIDAddedOnMap.ToString();
            }
           
        }
    }
}
