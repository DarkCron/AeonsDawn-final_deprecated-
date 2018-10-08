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

namespace TBAGW.Forms.Abilities
{
    public partial class CharAbilitiesForm : Form
    {
        public CharAbilitiesForm()
        {
            InitializeComponent();
        }

        EnemyAIInfo AII;
        CharacterClassCollection CCC;

        public void Start(CharacterClassCollection ccc, EnemyAIInfo AII)
        {
            CCC = ccc;
            this.AII = AII;
            ReloadAbilities();
            Show();
        }

        private void ReloadAbilities()
        {
            listBox1.DataSource = MapBuilder.gcDB.gameAbilities;

            #region list only unique separate abilities
            var tempL = new List<BasicAbility>(CCC.charSeparateAbilities);
            tempL = tempL.OrderBy(abi => abi.abilityIdentifier).ToList();
            tempL = tempL.GroupBy(abi => abi.abilityIdentifier).Select(abi => abi.First()).ToList();
            CCC.charSeparateAbilities = new List<BasicAbility>(tempL);
            #endregion


            listBox2.DataSource = CCC.charSeparateAbilities;
        }

        private void CharAbilitiesForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                CCC.charSeparateAbilities.RemoveAt(listBox2.SelectedIndex);
                listBox2.DataSource = null;
                listBox2.DataSource = CCC.charSeparateAbilities;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && !CCC.charSeparateAbilities.Contains((BasicAbility)listBox1.SelectedItem) && CCC.charSeparateAbilities.Find(abi => abi.abilityIdentifier == ((BasicAbility)listBox1.SelectedItem).abilityIdentifier) == default(BasicAbility))
            {
                CCC.charSeparateAbilities.Add(((BasicAbility)listBox1.SelectedItem).Clone());
                listBox2.DataSource = null;
                listBox2.DataSource = CCC.charSeparateAbilities;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CCC.AIDefaultAttack.abilityIdentifier == ((BasicAbility)listBox2.SelectedItem).abilityIdentifier)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 && !checkBox1.Checked)
            {
                CCC.AIDefaultAttack = new BasicAbility();
                CCC.defaultAbilityID = -1;
                AII.defaultAIAbilityID = -1;
            }
            else if (listBox2.SelectedIndex != -1 && checkBox1.Checked)
            {
                CCC.AIDefaultAttack = ((BasicAbility)listBox2.SelectedItem).Clone();
                CCC.defaultAbilityID = CCC.AIDefaultAttack.abilityIdentifier;
                AII.defaultAIAbilityID = CCC.defaultAbilityID;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
