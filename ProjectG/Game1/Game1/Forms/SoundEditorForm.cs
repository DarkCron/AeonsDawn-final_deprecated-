using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TBAGW;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.ReadWrite;

namespace Game1.Forms
{
    public partial class SoundEditorForm : Form
    {
        public SoundEditorForm()
        {
            InitializeComponent();
        }

        String currentContentFolder = "";
        BGInfo selectedBGI = new BGInfo();
        List<SFXInfo> sfxs = new List<SFXInfo>();


        private void SoundEditorForm_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentContentFolder.Equals(""))
            {
                MessageBox.Show("Please, first select the soundpool engine file (the '.cgbgi' file).");
                OpenFileDialog openBG = new OpenFileDialog();
                openBG.InitialDirectory = currentContentFolder;
                if (TBAGW.Game1.bIsDebug)
                {
                    openBG.Filter = "Sound engine file|*.cgbgic";
                }
                else {
                    openBG.Filter = "Sound engine file|*.cgbgi";
                }
                openBG.Title = "Load Sound File";

                if (TBAGW.Game1.bIsDebug)
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContent))
                    {
                        selectedBGI = EditorFileWriter.BGReader(openBG.FileName);
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                    else
                    {
                        Console.WriteLine("Cheater");
                    }
                }
                else
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContentExtra) && !openBG.FileName.Contains(TBAGW.Game1.rootContent + @"Sounds"))
                    {
                        selectedBGI = EditorFileWriter.BGReader(openBG.FileName);
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                }
            }
            else {
                MessageBox.Show("Please, first select the soundpool engine file (the '.cgbgi' file).");
                OpenFileDialog openBG = new OpenFileDialog();
                openBG.InitialDirectory = currentContentFolder;
                if (TBAGW.Game1.bIsDebug)
                {
                    openBG.Filter = "Sound engine file|*.cgbgic";
                }
                else {
                    openBG.Filter = "Sound engine file|*.cgbgi";
                }
                openBG.Title = "Load Sound File";

                if (TBAGW.Game1.bIsDebug)
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContent))
                    {
                        selectedBGI = EditorFileWriter.BGReader(openBG.FileName);
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                    else
                    {
                        Console.WriteLine("Cheater");
                    }
                }
                else
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContentExtra) && !openBG.FileName.Contains(TBAGW.Game1.rootContent + @"Sounds"))
                    {
                        selectedBGI = EditorFileWriter.BGReader(openBG.FileName);
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                }
            }

            textBox1.Text = selectedBGI.songCollectionName;
            numericUpDown1.Value = selectedBGI.songCollectionID;
            listBox1.Items.Clear();
            listBox1.SelectedIndex = -1;
            foreach (var item in selectedBGI.songNames)
            {
                listBox1.Items.Add(item);

            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderChoice = new FolderBrowserDialog();
            folderChoice.Description = "Please select folder where to load SFX's from!";

            bool bDone = false;
            while (!bDone)
            {
                folderChoice.SelectedPath = TBAGW.Game1.rootContent;

                if (folderChoice.ShowDialog() == DialogResult.OK)
                {
                    if (folderChoice.SelectedPath.StartsWith(TBAGW.Game1.rootContent))
                    {
                        currentContentFolder = folderChoice.SelectedPath.Replace(TBAGW.Game1.rootContent, "");
                        bDone = true;
                    }


                }
            }

            String completePath = Path.Combine(TBAGW.Game1.rootContent, currentContentFolder);
            if (TBAGW.Game1.bIsDebug)
            {
                String[] files = Directory.GetFiles(completePath, "*.cgsfxc", SearchOption.AllDirectories);
                foreach (var item in files)
                {
                    try
                    {
                        sfxs.Add(EditorFileWriter.SFXReader(item));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error reading file: " + item);
                    }
                }
            }
            else
            {
                String[] files = Directory.GetFiles(completePath, "*.cgsfx", SearchOption.AllDirectories);
                foreach (var item in files)
                {
                    try
                    {
                        sfxs.Add(EditorFileWriter.SFXReader(item));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error reading file: " + item);
                    }
                }
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedBGI != null)
            {
                try
                {
                    selectedBGI.ReloadContent();
                    Cue testCue = selectedBGI.soundBank.GetCue(textBox3.Text);
                    selectedBGI.songNames.Add(textBox3.Text);
                    selectedBGI.songIDs.Add(0);
                    listBox1.Items.Add(textBox3.Text);
                    selectedBGI.Dispose();
                }
                catch
                {
                    MessageBox.Show("Cue: '"+textBox3.Text + "' doesn't exist, please check again.");
                }

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedBGI != null)
            {
                if (TBAGW.Game1.bIsDebug)
                {
                    string f = Path.Combine(TBAGW.Game1.rootContent, currentContentFolder, selectedBGI.songCollectionName) + ".cgbgic";
                    if (File.Exists(f))
                    {
                        try
                        {
                            Console.WriteLine("File found, deleting! " + f);
                            File.Delete(f);
                            Console.WriteLine("Succesfully deleted!");
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                else {
                    string f = Path.Combine(TBAGW.Game1.rootContent, currentContentFolder, selectedBGI.songCollectionName) + ".cgbgi";
                    if (File.Exists(f))
                    {
                        try
                        {
                            Console.WriteLine("File found, deleting! " + f);
                            File.Delete(f);
                            Console.WriteLine("Succesfully deleted!");
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedBGI != null)
            {

                EditorFileWriter.BGWriter(selectedBGI, Path.Combine(TBAGW.Game1.rootContent, currentContentFolder, selectedBGI.songCollectionName));
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (selectedBGI != null&& listBox1.SelectedIndex!=-1)
            {
                selectedBGI.songCollectionID = (int)numericUpDown1.Value;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (selectedBGI != null&& listBox1.SelectedIndex!=-1)
            {
                selectedBGI.songNames[listBox1.SelectedIndex] = textBox1.Text;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                textBox3.Text = selectedBGI.songNames[listBox1.SelectedIndex];
                numericUpDown3.Value = selectedBGI.songIDs[listBox1.SelectedIndex];
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            String engineLoc = "";
            String sbLoc = "";
            String wbLoc = "";

{
                MessageBox.Show("Please, first select the soundpool engine file (the '.xgs' file).");
                OpenFileDialog openBG = new OpenFileDialog();
                openBG.InitialDirectory = currentContentFolder;
                if (TBAGW.Game1.bIsDebug)
                {
                    openBG.Filter = "Sound engine file|*.xgs";
                }
                else {
                    openBG.Filter = "Sound engine file|*.xgs";
                }
                openBG.Title = "Load Sound File";

                if (TBAGW.Game1.bIsDebug)
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContent) && !openBG.FileName.Contains(TBAGW.Game1.rootContent + @"Sounds"))
                    {
                        engineLoc = openBG.FileName.Replace(TBAGW.Game1.rootContent, "");
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                    else
                    {
                        Console.WriteLine("Cheater");
                    }
                }
                else
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContentExtra))
                    {
                        engineLoc = openBG.FileName.Replace(TBAGW.Game1.rootContent, "");
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                }
            }

 {
                MessageBox.Show("Please, first select the soundbank engine file (the '.xsb' file).");
                OpenFileDialog openBG = new OpenFileDialog();
                openBG.InitialDirectory = currentContentFolder;
                if (TBAGW.Game1.bIsDebug)
                {
                    openBG.Filter = "Sound engine file|*.xsb";
                }
                else {
                    openBG.Filter = "Sound engine file|*.xsb";
                }
                openBG.Title = "Load Sound File";

                if (TBAGW.Game1.bIsDebug)
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContent) && !openBG.FileName.Contains(TBAGW.Game1.rootContent + @"Sounds"))
                    {
                        sbLoc = openBG.FileName.Replace(TBAGW.Game1.rootContent, "");
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                    else
                    {
                        Console.WriteLine("Cheater");
                    }
                }
                else
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContentExtra))
                    {
                        sbLoc = openBG.FileName.Replace(TBAGW.Game1.rootContent, "");
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                }
            }

{
                MessageBox.Show("Please, first select the WaveBank engine file (the '.xwb' file).");
                OpenFileDialog openBG = new OpenFileDialog();
                openBG.InitialDirectory = currentContentFolder;
                if (TBAGW.Game1.bIsDebug)
                {
                    openBG.Filter = "Sound engine file|*.xwb";
                }
                else {
                    openBG.Filter = "Sound engine file|*.xwb";
                }
                openBG.Title = "Load Sound File";

                if (TBAGW.Game1.bIsDebug)
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContent) && !openBG.FileName.Contains(TBAGW.Game1.rootContent + @"Sounds"))
                    {
                        wbLoc = openBG.FileName.Replace(TBAGW.Game1.rootContent, "");
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                    else
                    {
                        Console.WriteLine("Cheater");
                    }
                }
                else
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContentExtra))
                    {
                        wbLoc = openBG.FileName.Replace(TBAGW.Game1.rootContent, "");
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                }
            }

            if (!engineLoc.Equals("") && !sbLoc.Equals("") && !wbLoc.Equals(""))
            {
                BGInfo tempBGI = new BGInfo();
                tempBGI.songELoc = engineLoc;
                tempBGI.songSBLoc = sbLoc;
                tempBGI.songWBLoc = wbLoc;
                tempBGI.songCollectionName = engineLoc;
                selectedBGI = tempBGI;
                EditorFileWriter.BGWriter(tempBGI, Path.Combine(TBAGW.Game1.rootContent, currentContentFolder, tempBGI.songCollectionName));
                listBox1.Items.Clear();
                listBox1.SelectedIndex = -1;
                foreach (var item in tempBGI.songNames)
                {
                    listBox1.Items.Add(item);
                }
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedBGI.songIDs[listBox1.SelectedIndex] = (int)numericUpDown3.Value;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selectedBGI.songNames.RemoveAt(listBox1.SelectedIndex);
                selectedBGI.songIDs.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (currentContentFolder.Equals(""))
            {
                MessageBox.Show("Please, first select the soundpool engine file (the '.cgbgi' file).");
                OpenFileDialog openBG = new OpenFileDialog();
                openBG.InitialDirectory = currentContentFolder;
                if (TBAGW.Game1.bIsDebug)
                {
                    openBG.Filter = "Sound engine file|*.cgbgic";
                }
                else {
                    openBG.Filter = "Sound engine file|*.cgbgi";
                }
                openBG.Title = "Load Sound File";

                if (TBAGW.Game1.bIsDebug)
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContent))
                    {
                        selectedBGI = EditorFileWriter.BGReader(openBG.FileName);
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                    else
                    {
                        Console.WriteLine("Cheater");
                    }
                }
                else
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContentExtra) && !openBG.FileName.Contains(TBAGW.Game1.rootContent + @"Sounds"))
                    {
                        selectedBGI = EditorFileWriter.BGReader(openBG.FileName);
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                }
            }
            else {
                MessageBox.Show("Please, first select the soundpool engine file (the '.cgbgi' file).");
                OpenFileDialog openBG = new OpenFileDialog();
                openBG.InitialDirectory = currentContentFolder;
                if (TBAGW.Game1.bIsDebug)
                {
                    openBG.Filter = "Sound engine file|*.cgbgic";
                }
                else {
                    openBG.Filter = "Sound engine file|*.cgbgi";
                }
                openBG.Title = "Load Sound File";

                if (TBAGW.Game1.bIsDebug)
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContent))
                    {
                        selectedBGI = EditorFileWriter.BGReader(openBG.FileName);
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                    else
                    {
                        Console.WriteLine("Cheater");
                    }
                }
                else
                {
                    DialogResult dia = openBG.ShowDialog();
                    if (DialogResult.OK == dia && openBG.FileName.Contains(TBAGW.Game1.rootContentExtra) && !openBG.FileName.Contains(TBAGW.Game1.rootContent + @"Sounds"))
                    {
                        selectedBGI = EditorFileWriter.BGReader(openBG.FileName);
                    }
                    else if (DialogResult.Cancel == dia)
                    {
                    }
                }
            }

            
            if (!MapBuilder.parentMap.soundPools.Contains(selectedBGI)) {
                MapBuilder.parentMap.soundPools.Add(selectedBGI);
                listBox3.Items.Add(selectedBGI);
            }
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if(listBox3.SelectedIndex!=-1) {
                MapBuilder.parentMap.soundPools.RemoveAt(listBox3.SelectedIndex);
            }
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            listBox3.SelectedIndex = -1;
            foreach (var item in MapBuilder.parentMap.soundPools)
            {
                listBox3.Items.Add(item);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MapBuilder.SaveMap();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox3.SelectedIndex!=-1) {
                listBox2.Items.Clear();
                listBox2.SelectedIndex = -1;
                foreach (var item in MapBuilder.parentMap.soundPools[listBox3.SelectedIndex].songNames)
                {
                    listBox2.Items.Add(item);
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex!=-1) {
                labelName.Text = MapBuilder.parentMap.soundPools[listBox3.SelectedIndex].songNames[listBox2.SelectedIndex];
                labelID.Text = MapBuilder.parentMap.soundPools[listBox3.SelectedIndex].songIDs[listBox2.SelectedIndex].ToString();
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex!=-1) {
                MapBuilder.parentMap.soundPools[listBox3.SelectedIndex].SEPlay(listBox2.SelectedIndex);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                MapBuilder.parentMap.soundPools[listBox3.SelectedIndex].StopAllActiveCues();
            }
        }
    }
}
