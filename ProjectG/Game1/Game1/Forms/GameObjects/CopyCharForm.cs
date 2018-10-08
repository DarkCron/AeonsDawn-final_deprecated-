using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.ReadWrite;

namespace Game1.Forms.GameObjects
{
    public partial class CopyCharForm : Form
    {
        public CopyCharForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
        BaseCharacter character;
        internal void Start(BaseCharacter selectedItem)
        {
            Show();
            character = selectedItem.Clone();
            textBox1.Text = character.CharacterName;
            numericUpDown1.Value = character.shapeID;
        }

        private void CopyCharForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            character.CharacterName = textBox1.Text;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            character.shapeID = (int)numericUpDown1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveCharacter = new SaveFileDialog();
            if (TBAGW.Game1.bIsDebug)
            {
                saveCharacter.Filter = "Character file|*.cgbcc";
                saveCharacter.InitialDirectory = TBAGW.Game1.rootTBAGW;

            }
            else
            {
                saveCharacter.Filter = "Character file|*.cgbc";
                saveCharacter.InitialDirectory = TBAGW.Game1.rootContentExtra;
            }

            saveCharacter.Title = "save character";
            DialogResult dia = saveCharacter.ShowDialog();
            if (DialogResult.OK == dia && saveCharacter.FileName.Contains(saveCharacter.InitialDirectory))
            {
                String fi = saveCharacter.FileName;
                EditorFileWriter.BasicSpriteWriter(fi,character);
                Hide();
            }
            else if (System.Windows.Forms.DialogResult.Cancel == dia)
            {

                System.Windows.Forms.MessageBox.Show("Cancelled, returning to Editor.");
            }
            else if (!saveCharacter.FileName.Contains(saveCharacter.InitialDirectory))
            {
                System.Windows.Forms.MessageBox.Show("Save within mods folder please.");
            }
        }
    }
}
