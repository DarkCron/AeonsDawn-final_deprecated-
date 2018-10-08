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

namespace TBAGW.Forms.GameObjects
{
    public partial class ObjectSelector : Form
    {
        public ObjectSelector()
        {
            InitializeComponent();
        }

        private void ObjectSelector_Load(object sender, EventArgs e)
        {

        }

        enum SelectTypeObject { None, Object, ObjectGroup, Light, Character };

        public void Start()
        {
            Show();
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectObjects.ToArray());
            listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectGroups.ToArray());
            listBox1.Items.AddRange(MapBuilder.gcDB.gameLights.ToArray());
            listBox1.Items.AddRange(MapBuilder.gcDB.gameCharacters.ToArray());
            textBox1.Text = "";

            listBox2.DataSource = null;
            listBox2.DataSource = Enum.GetNames(typeof(SelectTypeObject));
            listBox2.SelectedIndex = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SelectTypeObject type = (SelectTypeObject)listBox2.SelectedIndex;

            switch (type)
            {
                case SelectTypeObject.None:
                    if (textBox1.Text.Equals(""))
                    {
                        listBox1.SelectedIndex = -1;
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectObjects.ToArray());
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectGroups.ToArray());
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameLights.ToArray());
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameCharacters.ToArray());
                    }
                    else
                    {
                        listBox1.SelectedIndex = -1;
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectObjects.FindAll(o => o.shapeName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectGroups.FindAll(o => o.groupName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameLights.FindAll(o => o.shapeName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameCharacters.FindAll(o => o.shapeName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0|| o.CharacterName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                    }
                    break;
                case SelectTypeObject.Object:
                    if (textBox1.Text.Equals(""))
                    {
                        listBox1.SelectedIndex = -1;
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectObjects.ToArray());
                    }
                    else
                    {
                        listBox1.SelectedIndex = -1;
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectObjects.FindAll(o => o.shapeName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                    }
                    break;
                case SelectTypeObject.ObjectGroup:
                    if (textBox1.Text.Equals(""))
                    {
                        listBox1.SelectedIndex = -1;
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectGroups.ToArray());
                    }
                    else
                    {
                        listBox1.SelectedIndex = -1;
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameObjectGroups.FindAll(o => o.groupName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                    }
                    break;
                case SelectTypeObject.Light:
                    if (textBox1.Text.Equals(""))
                    {
                        listBox1.SelectedIndex = -1;
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameLights.ToArray());
                    }
                    else
                    {
                        listBox1.SelectedIndex = -1;
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameLights.FindAll(o => o.shapeName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
                    }
                    break;
                case SelectTypeObject.Character:
                    if (textBox1.Text.Equals(""))
                    {
                        listBox1.SelectedIndex = -1;
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameCharacters.ToArray());
                    }
                    else
                    {
                        listBox1.SelectedIndex = -1;
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(MapBuilder.gcDB.gameCharacters.FindAll(o => o.shapeName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0 || o.CharacterName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());

                    }
                    break;
                default:
                    break;
            }

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            MapBuilder.bObjectLayer = false;
            base.OnFormClosing(e);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MapBuilder.controls.currentType = Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerAdding;
            MapBuilder.objAddition.Assign(listBox1.SelectedItem);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(sender,e);
        }
    }
}
