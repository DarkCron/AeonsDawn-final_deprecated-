using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TBAGW;
using TBAGW.Forms.Bools;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.SriptProcessing;

namespace Game1.Forms.ScriptForms.ScriptCommandForms
{
    public partial class TIFCommand : Form
    {
        ScriptBaseForm scriptBaseForm;

        public TIFCommand()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void TIFCommand_Load(object sender, EventArgs e)
        {


        }

        ScrptBoolCheckEditor sbcEdit = new ScrptBoolCheckEditor();
        ScriptBoolCheck sbc = new ScriptBoolCheck();
        public void Start()
        {
            sbc = new ScriptBoolCheck();
            listBox3.DataSource = null;
            listBox3.DataSource = Enum.GetNames(typeof(BaseCharacter.Rotation));
            Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    if (sbc != default(ScriptBoolCheck))
                    {
                        scriptBaseForm.AddLine("@TIF" + "_" + sbc.toCastString() + "_" + numericUpDown1.Value + "_" + ScriptProcessor.TIFTypes.Bool.ToString());
                    }


                    break;
                case 1:
                    if (listBox3.SelectedIndex != -1)
                    {
                        scriptBaseForm.AddLine("@TIF" + "_" + CID.Text + "_" + numericUpDown1.Value + "_" + ScriptProcessor.TIFTypes.PlayerFacing.ToString());
                    }
                    break;
                case 2:
                    if (listBox4.SelectedIndex != -1 && listBox5.SelectedIndex != -1)
                    {
                        scriptBaseForm.AddLine("@TIF" + "_" + listBox4.SelectedItem + "_" + listBox5.SelectedItem + "_" + numericUpDown1.Value + "_" + ScriptProcessor.TIFTypes.Time.ToString());
                    }
                    break;
            }
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            Show();
            this.scriptBaseForm = scriptBaseForm;
            listBox3.DataSource = null;
            listBox3.DataSource = Enum.GetNames(typeof(BaseCharacter.Rotation));
            listBox4.DataSource = null;
            listBox4.DataSource = Enum.GetNames(typeof(DayLightHandler.TimeBlocksNames));
            listBox5.DataSource = null;
            listBox5.DataSource = Enum.GetNames(typeof(DayLightHandler.TimeBlocksNames));
            listBox6.DataSource = scriptBaseForm.script.scriptContent;
        }

        private void TIFCommand_Enter(object sender, EventArgs e)
        {
        }

        private void CID_Click(object sender, EventArgs e)
        {

        }

        private void SBID_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void TIF_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                CID.Text = listBox3.SelectedIndex.ToString();
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(sbcEdit);
            sbcEdit = new ScrptBoolCheckEditor();
            sbcEdit.Start(sbc);
        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
