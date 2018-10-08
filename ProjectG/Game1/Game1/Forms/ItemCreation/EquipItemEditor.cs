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
    public partial class EquipItemEditor : Form
    {
        public EquipItemEditor()
        {
            InitializeComponent();
        }

        private void EquipItemEditor_Load(object sender, EventArgs e)
        {

        }

        private BaseEquipment be;
        public void Start(BaseEquipment be)
        {
            this.be = be;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(BaseEquipment.EQUIP_TYPES)));
            comboBox1.SelectedIndex = (int)be.EquipType;

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(Enum.GetNames(typeof(BaseEquipment.ARMOUR_TYPES)));
            comboBox2.SelectedIndex = (int)be.ArmourUseType;

            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(Enum.GetNames(typeof(BaseClass.CLASSType)));
            comboBox3.SelectedIndex = (int)be.WeaponUseType;

            Show();
        }

        ActiveSTATForm asf = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (asf == null || asf.IsDisposed)
            {
                asf = new ActiveSTATForm();
                asf.Start(be.EquipmentActiveStatModifier);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            be.EquipType = (BaseEquipment.EQUIP_TYPES)comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            be.ArmourUseType = (BaseEquipment.ARMOUR_TYPES)comboBox2.SelectedIndex;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            be.WeaponUseType = (BaseClass.CLASSType)comboBox3.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            if (asf != null && !asf.IsDisposed)
            {
                asf.Close();
            }
        }
    }
}
