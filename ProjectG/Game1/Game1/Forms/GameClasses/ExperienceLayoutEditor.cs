using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBAGW.Forms.GameClasses
{
    public partial class ExperienceLayoutEditor : Form
    {
        public ExperienceLayoutEditor()
        {
            InitializeComponent();
        }

        private void ExperienceLayoutEditor_Load(object sender, EventArgs e)
        {

        }

        BaseClass selectedClass;
        public void Start(BaseClass cl)
        {
            
            selectedClass = cl;
            GenerateExpList();
            Text = selectedClass.ToString();
            Show();
        }

        private void GenerateExpList()
        {
            var s = selectedClass.classEXP.getLevelScript();
            listBox1.Items.Clear();
            for (int i = 0; i < 50; i++)
            {
                int j = selectedClass.classEXP.ExpRequirementLevel(i);
                String level = "Lvl " + i + ":\t" + selectedClass.classEXP.ExpRequirementLevel(i);
                listBox1.Items.Add(level);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Lua file|*.lua";
            ofd.InitialDirectory = TBAGW.Game1.rootContent;
            ofd.Title = "open Lua";
            DialogResult dia = ofd.ShowDialog();
            if (DialogResult.OK == dia && ofd.FileName.Contains(ofd.InitialDirectory))
            {
                String fi = ofd.FileName;
                selectedClass.classEXP.loc = fi.Replace(ofd.InitialDirectory, "");
                var s = selectedClass.classEXP.getLevelScript();
                if (s != null)
                {
                    try
                    {   // Open the text file using a stream reader.
                        using (StreamReader sr = new StreamReader(TBAGW.Game1.rootContent + (listBox1.SelectedItem as EnemyAIInfo).luaLoc))
                        {
                            // Read the stream to a string, and write the string to the console.
                            String line = sr.ReadToEnd();
                            richTextBox1.Text = line;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else if (System.Windows.Forms.DialogResult.Cancel == dia)
            {

                System.Windows.Forms.MessageBox.Show("Cancelled, returning to Editor.");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            selectedClass.classEXP.loc = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            selectedClass.classEXP.funcName = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var s = selectedClass.classEXP.getLevelScript();
            if (s != null)
            {
                try
                {   // Open the text file using a stream reader.
                    using (StreamReader sr = new StreamReader(TBAGW.Game1.rootContent + selectedClass.classEXP.loc))
                    {
                        // Read the stream to a string, and write the string to the console.
                        String line = sr.ReadToEnd();
                        richTextBox1.Text = line;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(ex.Message);
                }
            }

            GenerateExpList();
        }
    }
}
