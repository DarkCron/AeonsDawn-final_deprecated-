using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Utilities.ReadWrite;

namespace TBAGW.Forms.Sound
{
    public partial class SongEncrypt : Form
    {
        public SongEncrypt()
        {
            InitializeComponent();
        }

        String songFile = "";
        String cSongFile = "";

        private void button1_Click(object sender, EventArgs e)
        {
            LoadSongFile();
            EditorFileWriter.SongEncrypter(songFile,cSongFile);
        }

        private void LoadSongFile()
        {
            OpenFileDialog openTex = new OpenFileDialog();
            openTex.Title = "Open texture file";

            openTex.Filter = "Sound File|*.mp3;*.wav;*.xnb; *.wma";
            openTex.InitialDirectory = TBAGW.Game1.rootContent;


            bool bDone = false;

            while (!bDone)
            {
                DialogResult dia = openTex.ShowDialog();
                if (dia == DialogResult.OK && openTex.FileName.Contains(openTex.InitialDirectory))
                {
                    songFile = openTex.FileName;
                    String path = System.IO.Path.GetDirectoryName(songFile);
                    cSongFile = System.IO.Path.Combine(path, System.IO.Path.GetFileNameWithoutExtension(songFile) + ".cwma");
                    bDone = true;
                }
                else if (dia == DialogResult.Cancel)
                {
                    bDone = true;
                }
            }

        }
    }
}
