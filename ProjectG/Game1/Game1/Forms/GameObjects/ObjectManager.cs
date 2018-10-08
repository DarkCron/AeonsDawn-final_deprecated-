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
using TBAGW.Utilities.Sprite;

namespace TBAGW.Forms.GameObjects
{
    public partial class ObjectManager : Form
    {
        public ObjectManager()
        {
            InitializeComponent();
        }

        private void ObjectManager_Load(object sender, EventArgs e)
        {

        }

        public void Start()
        {
            Show();
            splitContainer1.Panel1Collapsed = false;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(MapBuilder.loadedMap.mapRegions.ToArray());
            textBox1.Text = "";
            listBox2.Items.Clear();
            listBox2.Items.AddRange(MapBuilder.loadedMap.mapSprites.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel1Collapsed)
            {
                splitContainer1.Panel1Collapsed = false;
            }
            else
            {
                splitContainer1.Panel1Collapsed = true;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.loadedMap.mapRegions.ToArray());
            }
            else
            {
                listBox1.SelectedIndex = -1;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(MapBuilder.loadedMap.mapRegions.FindAll(r => r.regionName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox2.SelectedIndex = -1;
                listBox2.Items.Clear();
                listBox2.Items.AddRange(MapBuilder.loadedMap.mapSprites.FindAll(s => ((MapRegion)listBox1.SelectedItem).Contains(s)).ToArray());
            }
            else
            {
                listBox2.SelectedIndex = -1;
                listBox2.Items.Clear();
                listBox2.Items.AddRange(MapBuilder.loadedMap.mapSprites.ToArray());
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        ScriptOverview so = new ScriptOverview();
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1&&listBox2.SelectedIndex!=-1)
            {
                so.ProcessScript(((BaseSprite)(listBox2.SelectedItem)).script);

                MapBuilder.objSelection.Assign((BaseSprite)listBox2.SelectedItem);
            }
        }

        static public ScriptBaseForm scriptForm = new ScriptBaseForm();
        private void button22_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Would you like to create a -completely- separate copy of the object with a differen ID? If not, every single object with the same ID on this map will have the newly created script.", "Script warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                BaseSprite temp = (BaseSprite)((BaseSprite)listBox2.SelectedItem).ShallowCopy();
                MapBuilder.gcDB.AddObject(temp);
                MapBuilder.loadedMap.mapSprites.Remove((BaseSprite)listBox2.SelectedItem);
                MapBuilder.loadedMap.mapSprites.Add(temp);
                scriptForm.Show();
                scriptForm.LoadScript(temp.script);
                if (listBox1.SelectedIndex != -1)
                {
                    listBox2.SelectedIndex = -1;
                    listBox2.Items.Clear();
                    listBox2.Items.AddRange(MapBuilder.loadedMap.mapSprites.FindAll(s => ((MapRegion)listBox1.SelectedItem).Contains(s)).ToArray());
                }
                else
                {
                    listBox2.SelectedIndex = -1;
                    listBox2.Items.Clear();
                    listBox2.Items.AddRange(MapBuilder.loadedMap.mapSprites.ToArray());
                }

            }
            else if (dialogResult == DialogResult.No)
            {
                if (listBox2.SelectedIndex != -1)
                {
                    //   MapBuilder.bCreatingScript = true;
                    scriptForm.Show();
                    scriptForm.LoadScript(((BaseSprite)listBox2.SelectedItem).script);
                    //    ScriptBuilder.Start(ref MapBuilder.parentMap.mapSprites[listBox1.SelectedIndex].script);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
