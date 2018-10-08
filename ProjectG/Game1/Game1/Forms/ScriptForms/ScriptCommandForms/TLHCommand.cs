using Game1.Forms.ScriptForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBAGW.Forms.ScriptForms.ScriptCommandForms
{
    public partial class TLHCommand : Form
    {
        public TLHCommand()
        {
            InitializeComponent();
        }

      
        String fn = "";
        String functionName = "";

        ScriptBaseForm scriptBaseForm;
        internal void Start(ScriptBaseForm scriptBaseForm)
        {
            Show();
            this.scriptBaseForm = scriptBaseForm;


            textBox1.Text = functionName;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            scriptBaseForm.AddLine("@TLH" + "_" + fn + "_"+ functionName);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "LUA files (*.lua)|*.lua";
            ofd.InitialDirectory = Game1.rootContent;
            bool bDone = false;
            String fn = null;

            while (!bDone)
            {

                System.Windows.Forms.DialogResult diagres = ofd.ShowDialog();

                if (diagres == System.Windows.Forms.DialogResult.OK && ofd.FileName.Contains(Game1.rootContent))
                {
                    fn = ofd.FileName.Replace(Game1.rootContent, "");
                    bDone = true;
                }
                else if (diagres == DialogResult.Cancel)
                {
                    bDone = true;
                }
            }

            if (fn != null) { this.fn = fn; }

        }

        private void TLHCommand_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            functionName = textBox1.Text;
        }
    }
}
