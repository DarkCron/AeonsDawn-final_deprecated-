using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBAGW.Forms.General
{
    public partial class ModifierEditor : Form
    {
        public ModifierEditor()
        {
            InitializeComponent();
        }

        public BaseModifier selectedModifier = null;
        
        public void Start(BaseModifier modifier)
        {
            selectedModifier = modifier;
            AssignValues();
            Show();
        }

        private void AssignValues()
        {
            numericUpDown1.Value = selectedModifier.abilityModifierLength;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            STATChartForm form = new STATChartForm();
            form.Start(selectedModifier.statModifier);
        }

        private void ModifierEditor_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            selectedModifier.abilityModifierLength = (int)numericUpDown1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
