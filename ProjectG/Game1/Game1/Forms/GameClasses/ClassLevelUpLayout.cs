using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBAGW.Forms.GameClasses
{
    public partial class ClassLevelUpLayout : Form
    {
        public ClassLevelUpLayout()
        {
            InitializeComponent();
        }

        BaseClass selectedClass;

        internal void Start(BaseClass c)
        {
            selectedClass = c;
            selectedClass.CheckScript();
            Text = selectedClass.ToString();
            button2.Enabled = false;
            Show();
        }

        private void ClassLevelUpLayout_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value > numericUpDown1.Value)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var temp = LUA.LuaGameClass.editorSummon(selectedClass.Clone());
            temp.parent.classLevel = (int)numericUpDown1.Value;
            temp.level = (int)numericUpDown1.Value;
            temp.parent.HandleLevelUpEditorSimulation(temp);
            temp.HandleLevelUp(temp.parent, new Utilities.Characters.BaseCharacter());


            GetListBoxReady(temp.additionStat);

        }

        private void GetListBoxReady(STATChart sc)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            for (int i = 0; i < sc.currentPassiveStats.Count; i++)
            {
                if (sc.currentPassiveStats[i] != 0)
                {
                    int index = i;
                    String s = ((STATChart.PASSIVESTATSNames)index).ToString() + ": ";
                    listBox1.Items.Add(s);
                    listBox2.Items.Add("+" + sc.currentPassiveStats[i]);
                }
            }

            for (int i = 0; i < sc.currentSpecialStats.Count; i++)
            {
                if (sc.currentSpecialStats[i] != 0)
                {
                    int index = i;
                    String s = ((STATChart.SPECIALSTATSNames)index).ToString() + ": ";
                    listBox1.Items.Add(s);
                    listBox2.Items.Add("+" + sc.currentSpecialStats[i]);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Enabled)
            {
                List<LUA.LuaGameClass> tempList = new List<LUA.LuaGameClass>();
                for (int i = 0; i < numericUpDown2.Value - numericUpDown1.Value; i++)
                {
                    var temp = LUA.LuaGameClass.editorSummon(selectedClass.Clone());
                    temp.parent.classLevel = ((int)numericUpDown1.Value) + i;
                    temp.level = ((int)numericUpDown1.Value) + i;
                    temp.parent.HandleLevelUpEditorSimulation(temp);
                    temp.HandleLevelUp(temp.parent, new Utilities.Characters.BaseCharacter());
                    tempList.Add(temp);
                }

                STATChart tempStats = new STATChart(true);
                for (int i = 0; i < tempList.Count; i++)
                {
                    tempStats.AddStatChartWithoutActive(tempList[i].additionStat);
                }
                GetListBoxReady(tempStats);
            }
        }
    }
}
