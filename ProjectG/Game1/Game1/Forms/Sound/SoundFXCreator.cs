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
using TBAGW.Scenes.Editor;

namespace TBAGW.Forms.Sound
{
    public partial class SoundFXCreator : Form
    {
        public SoundFXCreator()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        String path = Environment.CurrentDirectory;
        private void SoundFXCreator_Load(object sender, EventArgs e)
        {

        }

        List<String> sfxLocs = new List<string>();

        public void Start()
        {
            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            listBox1.DataSource = MapBuilder.gcDB.gameSFXs;
            if (Game1.bIsDebug)
            {
                path = Path.Combine(Game1.rootContent, @"SFX");
                listBox2.Items.Clear();
                listBox2.SelectedIndex = -1;


                sfxLocs = new List<String>(Directory.GetFiles(path, "*.xnb", SearchOption.AllDirectories));
                List<String> files = new List<string>();
                foreach (var item in sfxLocs)
                {
                    files.Add(Path.GetFileNameWithoutExtension(item));
                }
                for (int i = 0; i < sfxLocs.Count; i++)
                {
                    sfxLocs[i] = sfxLocs[i].Replace(Game1.rootContent, "");
                    sfxLocs[i] = sfxLocs[i].Replace(".xnb", "");
                }


                listBox2.DataSource = files;
            }
            else
            {
                path = Path.Combine(Game1.rootContentExtra, @"SFX");
                listBox2.Items.Clear();
                listBox2.SelectedIndex = -1;


                sfxLocs = new List<String>(Directory.GetFiles(path, "*.wav", SearchOption.AllDirectories));
                List<String> files = new List<string>();
                foreach (var item in sfxLocs)
                {
                    files.Add(Path.GetFileNameWithoutExtension(item));
                }
                for (int i = 0; i < sfxLocs.Count; i++)
                {
                    sfxLocs[i] = sfxLocs[i].Replace(Game1.rootContent, "");
                    sfxLocs[i] = sfxLocs[i].Replace(".wav", "");
                }

                listBox2.DataSource = files;
            }

            Show();
        }

        public void ReloadLB1()
        {
            listBox1.SelectedIndex = -1;
            listBox1.DataSource = null;
            listBox1.DataSource = MapBuilder.gcDB.gameSFXs;
            button1.Enabled = false;
            listBox2.SelectedIndex = -1;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                String sfxName = listBox2.SelectedItem.ToString();
                if (MapBuilder.gcDB.gameSFXs.Find(sfx => sfx.sfxName.Equals(sfxName, StringComparison.OrdinalIgnoreCase)) == default(SFXInfo))
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Enabled && listBox2.SelectedIndex != -1)
            {
                SFXInfo temp = new SFXInfo();
                temp.sfxLoc = sfxLocs[listBox2.SelectedIndex];
                temp.sfxName = listBox2.SelectedItem.ToString();
                temp.ReloadContent();
                MapBuilder.gcDB.AddSFX(temp);
                ReloadLB1();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex!=-1)
            {
                SFXInfo temp = new SFXInfo();
                temp.sfxLoc = sfxLocs[listBox2.SelectedIndex];
                temp.sfxName = listBox2.SelectedItem.ToString();
                temp.ReloadContent();
                temp.sfx.CreateInstance().Play();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ((SFXInfo)listBox1.SelectedItem).sfx.CreateInstance().Play();
            }
        }
    }
}
