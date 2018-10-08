using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBAGW.Forms.ItemCreation
{
    public partial class MaterialEditor : Form
    {
        public MaterialEditor()
        {
            InitializeComponent();
        }

        private void MaterialEditor_Load(object sender, EventArgs e)
        {

        }

        private BaseMaterials bm;
        public void Start(BaseMaterials bm)
        {
            this.bm = bm;
            numericUpDown2.Value = bm.itemMaxAmount;
            numericUpDown1.Value = bm.itemStackSize;
            Show();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if(numericUpDown2.Value==0) {
                numericUpDown2.Value = -1;
            }

            bm.itemMaxAmount = (int)numericUpDown2.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            bm.itemStackSize = (int)numericUpDown1.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
