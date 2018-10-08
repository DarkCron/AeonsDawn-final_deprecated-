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

namespace TBAGW.Forms.GameClasses
{
    public partial class CreateClassForm : Form
    {
        GameClassCreator gcf;

        public CreateClassForm()
        {
            InitializeComponent();
        }

        private void CreateClassForm_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }

        public void Start(GameClassCreator gcf)
        {
            this.gcf = gcf;
            this.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                if(MapBuilder.gcDB.gameClasses.Find(gc=>gc.ClassName.Equals(textBox1.Text,StringComparison.OrdinalIgnoreCase))==default(BaseClass))
                {
                    button1.Enabled = true;
                }else {
                    button1.Enabled = false;
                }
            }else
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Enabled)
            {
                BaseClass temp = new BaseClass();
                temp.ClassName = textBox1.Text;
                MapBuilder.gcDB.AddClass(temp);
                gcf.ReloadClassList();
                this.Close();
            }
        }
    }
}
