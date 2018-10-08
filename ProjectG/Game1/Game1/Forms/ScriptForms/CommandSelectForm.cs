using Game1.Forms.ScriptForms.ScriptCommandForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TBAGW;
using TBAGW.Forms.ScriptForms.ScriptCommandForms;

namespace Game1.Forms.ScriptForms
{
    public partial class CommandSelectForm : Form
    {
        ScriptBaseForm scriptBaseForm;
        TIFCommand tifCmd = new TIFCommand();
        TRECommand treCmd = new TRECommand();
        TSPCommand tspCmd = new TSPCommand();
        TEXTCommand textCmd = new TEXTCommand();
        TSMCommand tsmCmd = new TSMCommand();
        TGOCommand tgoCommand = new TGOCommand();
        TRCCommand trcCommand = new TRCCommand();
        TLHCommand tlhCommand = new TLHCommand();
        CWBCommand cwbCommand = new CWBCommand();
        DSTCommand dstCommand = new DSTCommand();
        DTTCommand dttCommand = new DTTCommand();
        CTMCommand ctmCommand = new CTMCommand();
        THECommand theCommand = new THECommand();
        CSBCommand csbCommand = new CSBCommand();
        CPPCommand cppCommand = new CPPCommand();
        MCTCommand mctCommand = new MCTCommand();
        TLBCommand tlbCommand = new TLBCommand();
        TPHCommand tphCommand = new TPHCommand();

        public CommandSelectForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@AST");
            Hide();
            scriptBaseForm.ReloadScriptContentList();
        }

        internal void LoadCMD(ScriptBaseForm scriptBaseForm)
        {
            this.scriptBaseForm = scriptBaseForm;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@TBS");
            Hide();
            scriptBaseForm.ReloadScriptContentList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@ESE");
            Hide();
            scriptBaseForm.ReloadScriptContentList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@END");
            Hide();
            scriptBaseForm.ReloadScriptContentList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@RAS");
            Hide();
            scriptBaseForm.ReloadScriptContentList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            scriptBaseForm.AddLine("@TES");
            Hide();
            scriptBaseForm.ReloadScriptContentList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tifCmd.Start(scriptBaseForm);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            treCmd.Start(scriptBaseForm);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tspCmd.Start(scriptBaseForm);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textCmd.Start(scriptBaseForm);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tsmCmd.Start(scriptBaseForm);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MSGCommand msgc = new MSGCommand();
            msgc.Start(scriptBaseForm);
        }

        DLHCommand dlhc = new DLHCommand();
        private void button13_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(dlhc);
            dlhc = new DLHCommand();
            dlhc.Start(scriptBaseForm);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(tgoCommand);
            tgoCommand = new TGOCommand();
            tgoCommand.Start(scriptBaseForm);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(trcCommand);
            trcCommand = new TRCCommand();
            trcCommand.Start(scriptBaseForm);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(tlhCommand);
            tlhCommand = new TLHCommand();
            tlhCommand.Start(scriptBaseForm);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(cwbCommand);
            cwbCommand = new CWBCommand();
            cwbCommand.Start(scriptBaseForm);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(dstCommand);
            dstCommand = new DSTCommand();
            dstCommand.Start(scriptBaseForm);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(dttCommand);
            dttCommand = new DTTCommand();
            dttCommand.Start(scriptBaseForm);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(ctmCommand);
            ctmCommand = new CTMCommand();
            ctmCommand.Start(scriptBaseForm);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(theCommand);
            theCommand = new THECommand();
            theCommand.Start(scriptBaseForm);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(csbCommand);
            csbCommand = new CSBCommand();
            csbCommand.Start(scriptBaseForm);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(cppCommand);
            cppCommand = new CPPCommand();
            cppCommand.Start(scriptBaseForm);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(mctCommand);
            mctCommand = new MCTCommand();
            mctCommand.Start(scriptBaseForm);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(tlbCommand);
            tlbCommand = new TLBCommand();
            tlbCommand.Start(scriptBaseForm);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(tphCommand);
            tphCommand = new TPHCommand();
            tphCommand.Start(scriptBaseForm);
        }
    }
}
