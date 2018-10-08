using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Forms.General;

namespace TBAGW.Forms.ItemCreation
{
    public partial class KeyItemEditor : Form
    {
        public KeyItemEditor()
        {
            InitializeComponent();
        }

        private void KeyItemEditor_Load(object sender, EventArgs e)
        {

        }

        private BaseKeyItem bki;
        public void Start(BaseKeyItem bki) {
            this.bki = bki;
            numericUpDown1.Value = bki.itemStackSize;
            numericUpDown2.Value = bki.itemMaxAmount;
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            if (asf != null && !asf.IsDisposed)
            {
                asf.Close();
            }
        }

        ActiveSTATForm asf = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (asf == null || asf.IsDisposed)
            {
                asf = new ActiveSTATForm();
                asf.Start(bki.KeyItemActiveStatModifier);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            bki.itemStackSize = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value == 0)
            {
                numericUpDown2.Value = -1;
            }

            bki.itemMaxAmount = (int)numericUpDown2.Value;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
