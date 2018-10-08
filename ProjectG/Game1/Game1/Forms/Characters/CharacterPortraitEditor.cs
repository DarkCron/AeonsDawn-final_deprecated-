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

namespace TBAGW.Forms.Characters
{
    public partial class CharacterPortraitEditor : Form
    {
        public CharacterPortraitEditor()
        {
            InitializeComponent();
        }

        private void CharacterPortraitEditor_Load(object sender, EventArgs e)
        {

        }

        BaseCharacter bc;

        public void Start(BaseCharacter selectedChar)
        {
            bc = selectedChar;
            listBox1.SelectedIndex = -1;
            listBox1.DataSource = Enum.GetNames(typeof(BaseCharacter.PortraitExpressions));
            listBox1.SelectedIndex = 0;

            listBox3.DataSource = null;
            listBox3.SelectedIndex = -1;
            listBox3.DataSource = MapBuilder.gcDB.gameSFXs;

            Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox2.SelectedIndex = -1;
                listBox2.DataSource = null;
                listBox2.DataSource = bc.dialogueExpressions.Find(de => de.isCurrentExpression((BaseCharacter.PortraitExpressions)listBox1.SelectedIndex)).sfxList;
                listBox3.SelectedIndex = -1;

            }

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                button2.Enabled = true;
                button3.Enabled = true;

            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                SFXInfo temp = (SFXInfo)listBox2.SelectedItem;
                temp.sfx.CreateInstance().Play();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                bc.dialogueExpressions.Find(de => de.isCurrentExpression((BaseCharacter.PortraitExpressions)listBox1.SelectedIndex)).dialogueList.RemoveAt(listBox2.SelectedIndex);
                bc.dialogueExpressions.Find(de => de.isCurrentExpression((BaseCharacter.PortraitExpressions)listBox1.SelectedIndex)).sfxList.RemoveAt(listBox2.SelectedIndex);
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                var temp = bc.dialogueExpressions.Find(de => de.isCurrentExpression((BaseCharacter.PortraitExpressions)listBox1.SelectedIndex)).sfxList;
                button4.Enabled = true;
                if (!temp.Contains((SFXInfo)listBox3.SelectedItem))
                {
                    button1.Enabled = true;
                }
                else { button3.Enabled = false; }
            }
            else
            {
                button1.Enabled = false;
                button4.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Enabled && listBox3.SelectedIndex != -1)
            {
                bc.dialogueExpressions.Find(de => de.isCurrentExpression((BaseCharacter.PortraitExpressions)listBox1.SelectedIndex)).sfxList.Add((SFXInfo)listBox3.SelectedItem);
                bc.dialogueExpressions.Find(de => de.isCurrentExpression((BaseCharacter.PortraitExpressions)listBox1.SelectedIndex)).dialogueList.Add(((SFXInfo)listBox3.SelectedItem).sfxID);
                button1.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1 && button4.Enabled)
            {
                var temp = (SFXInfo)listBox3.SelectedItem;
                temp.sfx.CreateInstance().Play();
            }
        }
    }
}
