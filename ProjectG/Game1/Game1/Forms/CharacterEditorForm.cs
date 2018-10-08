using Game1.Forms.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TBAGW.Forms.Characters;
using TBAGW.Forms.General;
using TBAGW.Forms.Loot_editor;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sprite;

namespace TBAGW.Forms
{
    public partial class CharacterEditorForm : Form
    {
        BaseClass selectedClass;
        public String charFolder = "";
        public static List<BaseCharacter> characters = new List<BaseCharacter>();

        public CharacterEditorForm()
        {
            InitializeComponent();
        }

        private void CharacterEditorForm_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        //private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    listBox2.SelectedIndex = -1;
        //    if (listBox1.SelectedIndex != -1)
        //    {
        //        LoadAttributes(MapBuilder.loadedMGClasses[listBox1.SelectedIndex]);
        //        button2.Show();
        //        button3.Show();
        //    }
        //    else
        //    {
        //        button2.Hide();
        //    }
        //}

        //private void LoadAttributes(BaseClass bc)
        //{
        //    selectedClass = bc;
        //    textBox1.Text = bc.ClassName;
        //}

        //private void saveAttributes()
        //{
        //    selectedClass.ClassName = textBox1.Text;
        //}

        //private void tabPage1_Enter(object sender, EventArgs e)
        //{
        //    button2.Hide();
        //    button3.Hide();
        //    listBox1.Items.Clear();
        //    String[] files = Directory.GetFiles(Game1.rootGClasses, "*.cgclc", SearchOption.AllDirectories);
        //    Console.WriteLine(files.Length);
        //    MapBuilder.loadedMGClasses.Clear();
        //    foreach (var item in files)
        //    {
        //        try
        //        {
        //            MapBuilder.loadedMGClasses.Add(EditorFileWriter.ClassReader(item));
        //            listBox1.Items.Add(MapBuilder.loadedMGClasses[MapBuilder.loadedMGClasses.Count - 1]);
        //        }
        //        catch (Exception)
        //        {
        //            Console.WriteLine("Error reading file: " + item);
        //        }
        //    }

        //    listBox2.Items.Clear();
        //    files = Directory.GetFiles(Game1.rootContentExtra, ".cgcl", SearchOption.AllDirectories);
        //    MapBuilder.loadedCUSTOMClasses.Clear();
        //    foreach (var item in files)
        //    {
        //        try
        //        {
        //            MapBuilder.loadedCUSTOMClasses.Add(EditorFileWriter.ClassReader(item));
        //            listBox1.Items.Add(MapBuilder.loadedCUSTOMClasses[MapBuilder.loadedCUSTOMClasses.Count - 1]);
        //        }
        //        catch (Exception)
        //        {
        //            Console.WriteLine("Error reading file: " + item);
        //        }
        //    }
        //}

        //private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    listBox1.SelectedIndex = -1;
        //    if (listBox2.SelectedIndex != -1)
        //    {
        //        LoadAttributes(MapBuilder.loadedCUSTOMClasses[listBox1.SelectedIndex]);
        //        button2.Show();
        //        button3.Show();
        //    }
        //    else
        //    {
        //        button2.Hide();
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            bool bCreatedClass = false;
            String classLoc = "";
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("This will create a new class, please select a save location for the class info. A class name can be given later and doesn't need to match the file name.", "Creating a new class?", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                SaveFileDialog classSave = new SaveFileDialog();
                if (Game1.bIsDebug)
                {
                    classSave.InitialDirectory = Game1.rootGClasses;
                    classSave.Filter = "Game Class File|*.cgcl";
                }
                else
                {
                    classSave.InitialDirectory = Game1.rootContentExtra;
                    classSave.Filter = "Game Class File|*.cgcl";
                }

                bool correctSave = false;
                while (!correctSave)
                {
                    if (classSave.ShowDialog() == DialogResult.OK)
                    {
                        if (Game1.bIsDebug)
                        {
                            if (classSave.FileName.StartsWith(Game1.rootGClasses))
                            {
                                correctSave = true;
                                bCreatedClass = true;
                                classLoc = classSave.FileName;
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show(@"Please select a file/folder within \TBAGW\Characters\Classes");
                            }
                        }
                        else
                        {
                            if (classSave.FileName.StartsWith(Game1.rootContentExtra))
                            {
                                correctSave = true;
                                bCreatedClass = true;
                                classLoc = classSave.FileName;
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show(@"Please select a file/folder within Content\Mods\");
                            }
                        }
                    }
                    else
                    {
                        correctSave = true;
                        bCreatedClass = false;
                    }
                }

            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                bCreatedClass = false;
            }


            if (bCreatedClass)
            {
                if (Game1.bIsDebug)
                {
                    BaseClass newClass = new BaseClass(classLoc.Replace(Game1.rootTBAGW, ""));
                    Console.WriteLine(newClass.ClassLoc);
                    EditorFileWriter.ClassWriter(newClass);
                }
                else
                {
                    BaseClass newClass = new BaseClass(classLoc.Replace(Game1.rootContentExtra, ""));
                    EditorFileWriter.ClassWriter(newClass);
                }
            }

        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("This will save your new class over the old one, the old one will be gone forever. Proceed?", "Overwrite warning", System.Windows.Forms.MessageBoxButtons.YesNo);
        //    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
        //    {
        //        saveAttributes();
        //        EditorFileWriter.ClassWriter(selectedClass);
        //    }
        //    else if (dialogResult == System.Windows.Forms.DialogResult.No)
        //    {
        //        //do something else
        //    }
        //}

        private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveTempChar = new SaveFileDialog();
            saveTempChar.Title = "Select save location for temporary/new character";
            BaseCharacter bc = new BaseCharacter();
            bc.CreateTempChar();

            if (Game1.bIsDebug)
            {
                saveTempChar.InitialDirectory = Game1.rootTBAGW;
                saveTempChar.Filter = "CGBCC File|*.cgbcc";
            }
            else
            {
                saveTempChar.InitialDirectory = Game1.rootContentExtra;
                saveTempChar.Filter = "CGBC File|*.cgbc";
            }


            bool bCreated = false;
            bool bDone = false;
            while (!bDone)
            {
                DialogResult dia = saveTempChar.ShowDialog();
                if (dia == DialogResult.OK && saveTempChar.FileName.Contains(saveTempChar.InitialDirectory))
                {
                    bc.CharacterName = Path.GetFileNameWithoutExtension(saveTempChar.FileName);
                    EditorFileWriter.BasicSpriteWriter(saveTempChar.FileName, bc);
                    Console.WriteLine("Successful char creation");
                    characters.Add(bc);
                    listBox3.Items.Add(bc);
                    bCreated = true;
                    bDone = true;
                }
                else if (dia == DialogResult.Cancel)
                {
                    bDone = true;
                }
            }


            if (bCreated)
            {
                MessageBox.Show("Temporary character created, please select character base texture file.");
                OpenFileDialog openTex = new OpenFileDialog();
                openTex.Title = "Open texture file";
                if (Game1.bIsDebug)
                {
                    openTex.Filter = "Texture File|*.jpg;*.png;*.jpeg;*.xnb";
                    openTex.InitialDirectory = Game1.rootContent;
                }
                else
                {
                    openTex.Filter = "Texture File|*.jpg;*.png;*.jpeg";
                    openTex.InitialDirectory = Game1.rootContentExtra;
                }

                bDone = false;

                while (!bDone)
                {
                    DialogResult dia = openTex.ShowDialog();
                    if (dia == DialogResult.OK && openTex.FileName.Contains(openTex.InitialDirectory))
                    {
                        foreach (var anim in bc.charAnimations)
                        {
                            anim.texFileLoc = openTex.FileName.Replace(Game1.rootContent, "").Substring(0, openTex.FileName.Replace(Game1.rootContent, "").IndexOf("."));
                            anim.ReloadTexture();
                        }

                        EditorFileWriter.BasicSpriteWriter(saveTempChar.FileName, bc);
                        Console.WriteLine("Successful char update");
                        bDone = true;
                    }
                    else if (!openTex.FileName.Contains(openTex.InitialDirectory))
                    {
                        MessageBox.Show(@"Please select a file within the application folder under Content\Mods and it's subfolders");
                    }
                    else if (dia == DialogResult.Cancel)
                    {
                        bDone = true;
                    }
                }
            }

        }

        private void tabControl1_Enter(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            FrameSelector.Stop();
            button8.Visible = false;
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            listBox3.SelectedIndex = -1;
            label18.Visible = false;
            label19.Visible = false;
            label20.Visible = false;
            label21.Visible = false;
            label22.Visible = false;
            textBox2.Visible = false;
            textBox2.Text = "";
            pictureBox1.Image = pictureBox1.InitialImage;
            pictureBox2.Image = pictureBox2.InitialImage;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            listBox4.SelectedIndex = -1;
            listBox4.Visible = false;
            listBox5.SelectedIndex = -1;
            listBox5.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            checkBox1.Visible = false;
            checkBox1.Checked = false;
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            button8.Visible = false;
            numericUpDown1.Visible = false;

            if (charFolder.Equals(""))
            {
                characters.Clear();
                FolderBrowserDialog folderChoice = new FolderBrowserDialog();
                folderChoice.Description = "Please select folder where to load characters from!";

                bool bDone = false;
                while (!bDone)
                {
                    if (Game1.bIsDebug)
                    {
                        folderChoice.SelectedPath = Game1.rootTBAGW;
                    }
                    else
                    {
                        folderChoice.SelectedPath = Game1.rootContentExtra;
                    }

                    if (folderChoice.ShowDialog() == DialogResult.OK)
                    {
                        if (Game1.bIsDebug && folderChoice.SelectedPath.StartsWith(Game1.rootTBAGW))
                        {
                            charFolder = folderChoice.SelectedPath;
                            bDone = true;
                        }
                        else if (folderChoice.SelectedPath.StartsWith(Game1.rootContentExtra))
                        {
                            charFolder = folderChoice.SelectedPath;
                            bDone = true;
                        }

                    }
                }

                if (Game1.bIsDebug)
                {
                    String[] files = Directory.GetFiles(charFolder, "*.cgbcc", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        // characters.Add(EditorFileWriter.BaseCharacterReader(item.Replace(Game1.rootContentExtra, "")));
                        try
                        {
                            characters.Add(EditorFileWriter.BaseCharacterReader(item));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error reading file: " + item);
                        }
                    }
                }
                else
                {
                    String[] files = Directory.GetFiles(charFolder, "*.cgbc", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {

                        try
                        {
                            characters.Add(EditorFileWriter.BaseCharacterReader(item.Replace(Game1.rootContentExtra, "")));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error reading file: " + item);
                        }
                    }
                }
            }


            foreach (var character in characters)
            {
                listBox3.Items.Add(character);
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                button24.Visible = TBAGW.Game1.bIsDebug;
                textBox2.Text = characters[listBox3.SelectedIndex].CharacterName;
                textBox2.Visible = true;
                label18.Visible = true;
                label19.Visible = true;
                label20.Visible = true;
                listBox4.Visible = true;
                button7.Visible = true;
                checkBox1.Checked = false;
                button29.Visible = true;

                listBox4.Items.Clear();
                int i = 0;
                foreach (var anim in characters[listBox3.SelectedIndex].charAnimations)
                {
                    listBox4.Items.Add(Enum.GetNames(typeof(BaseCharacter.CharacterAnimations))[i]);
                    i++;
                }

                listBox4.SelectedIndex = -1;
                listBox5.SelectedIndex = -1;
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                listBox5.Visible = false;
                label22.Visible = false;
                label21.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
                checkBox1.Visible = false;
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                listBox5.SelectedIndex = -1;
                pictureBox1.Visible = true;
                pictureBox2.Visible = true;
                label22.Visible = true;
                label21.Visible = true;
                button5.Visible = !characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex].bDirectional;
                button6.Visible = !characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex].bDirectional;
                checkBox1.Visible = true;
                checkBox1.Checked = characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex].bDirectional;
                listBox5.Visible = characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex].bDirectional;
                numericUpDown1.Visible = true;
                numericUpDown1.Value = characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex].frameInterval;

                Texture2D charSprite = characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex].animationTexture;
                byte[] data = new byte[charSprite.Width * charSprite.Height];
                MemoryStream m = new MemoryStream(data);
                charSprite.SaveAsPng(m, charSprite.Width, charSprite.Height);
                Bitmap bmp = new System.Drawing.Bitmap(m);
                pictureBox2.Image = bmp;
                pictureBox2.Refresh();

            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                characters[listBox3.SelectedIndex].CharacterName = textBox2.Text;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            BaseCharacter bc = characters[listBox3.SelectedIndex];
            SaveFileDialog saveBC = new SaveFileDialog();
            if (Game1.bIsDebug)
            {
                saveBC.InitialDirectory = Game1.rootTBAGW;
            }
            else
            {
                saveBC.InitialDirectory = Game1.rootContentExtra;
            }

            if (saveBC.ShowDialog() == DialogResult.OK)
            {
                bc.SaveCharacter();
                EditorFileWriter.BasicSpriteWriter(saveBC.FileName, bc);
                bc = EditorFileWriter.BaseCharacterReader(saveBC.FileName);

                listBox3.Items.Clear();
                characters.Clear();
                if (Game1.bIsDebug)
                {
                    String[] files = Directory.GetFiles(charFolder, "*.cgbcc", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        try
                        {
                            characters.Add(EditorFileWriter.BaseCharacterReader(item));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error reading file: " + item);
                        }
                    }
                }
                else
                {
                    String[] files = Directory.GetFiles(charFolder, "*.cgbc", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        try
                        {
                            characters.Add(EditorFileWriter.BaseCharacterReader(item.Replace(Game1.rootContentExtra, "")));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error reading file: " + item);
                        }
                    }
                }

                foreach (var character in characters)
                {
                    listBox3.Items.Add(character);
                    if (character.CharacterName.Equals(bc.CharacterName))
                    {
                        listBox3.SelectedIndex = characters.IndexOf(character);
                    }
                }


            }
            else
            {
                MessageBox.Show("Cancelled saving character named: " + bc.CharacterName);
            }



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedIndex != -1)
            {
                button5.Visible = true;
                button6.Visible = true;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex].ToggleDirectionalAnim();
                listBox5.SelectedIndex = -1;
                listBox5.Visible = true;
                button5.Visible = false;
                button6.Visible = false;
            }
            else //checkbox was false
            {
                characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex].ToggleDirectionalAnim();
                listBox5.SelectedIndex = -1;
                listBox5.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button8.Visible = true;
            if (listBox5.SelectedIndex == -1)
            {
                FrameSelector.StartSimple(characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex]);
            }
            else if (listBox5.SelectedIndex != -1)
            {
                FrameSelector.StartSimple(characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex], listBox5.SelectedIndex);
            }

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                characters[listBox3.SelectedIndex].charAnimations[listBox4.SelectedIndex].frameInterval = (int)numericUpDown1.Value;
            }

        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                var temp = listBox6.SelectedItem as BaseCharacter;
                textBox4.Text = temp.displayName;
            }
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            listBox6.Items.Clear();
            listBox6.Items.AddRange(MapBuilder.gcDB.gameCharacters.ToArray());
            #region
            /*
               listBox6.Items.Clear();

               if (charFolder.Equals(""))
               {
                   characters.Clear();
                   FolderBrowserDialog folderChoice = new FolderBrowserDialog();
                   folderChoice.Description = "Please select folder where to load characters from!";

                   bool bDone = false;
                   while (!bDone)
                   {
                       if (Game1.bIsDebug)
                       {
                           folderChoice.SelectedPath = Game1.rootTBAGW;
                       }
                       else
                       {
                           folderChoice.SelectedPath = Game1.rootContentExtra;
                       }

                       if (folderChoice.ShowDialog() == DialogResult.OK)
                       {
                           if (Game1.bIsDebug && folderChoice.SelectedPath.StartsWith(Game1.rootTBAGW))
                           {
                               charFolder = folderChoice.SelectedPath;
                               bDone = true;
                           }
                           else if (folderChoice.SelectedPath.StartsWith(Game1.rootContentExtra))
                           {
                               charFolder = folderChoice.SelectedPath;
                               bDone = true;
                           }

                       }
                   }

                   if (Game1.bIsDebug)
                   {
                       String[] files = Directory.GetFiles(charFolder, "*.cgbcc", SearchOption.AllDirectories);
                       foreach (var item in files)
                       {
                           try
                           {
                               characters.Add(EditorFileWriter.BaseCharacterReader(item));
                           }
                           catch (Exception)
                           {
                               Console.WriteLine("Error reading file: " + item);
                           }
                       }
                   }
                   else
                   {
                       String[] files = Directory.GetFiles(charFolder, "*.cgbc", SearchOption.AllDirectories);
                       foreach (var item in files)
                       {
                           try
                           {
                               characters.Add(EditorFileWriter.BaseCharacterReader(item.Replace(Game1.rootContentExtra, "")));
                           }
                           catch (Exception)
                           {
                               Console.WriteLine("Error reading file: " + item);
                           }
                       }
                   }
               }


               foreach (var character in characters)
               {
                   listBox6.Items.Add(character);
               }
               */
            #endregion
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            //TO DO
            BaseCharacter bc = characters[listBox6.SelectedIndex];
            SaveFileDialog saveBC = new SaveFileDialog();
            if (Game1.bIsDebug)
            {
                saveBC.InitialDirectory = Game1.rootTBAGW;
            }
            else
            {
                saveBC.InitialDirectory = Game1.rootContentExtra;
            }

            if (saveBC.ShowDialog() == DialogResult.OK)
            {
                EditorFileWriter.BasicSpriteWriter(saveBC.FileName, bc);
                bc = EditorFileWriter.BaseCharacterReader(saveBC.FileName);

                listBox6.Items.Clear();
                characters.Clear();
                if (Game1.bIsDebug)
                {
                    String[] files = Directory.GetFiles(charFolder, "*.cgbcc", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        try
                        {
                            characters.Add(EditorFileWriter.BaseCharacterReader(item));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error reading file: " + item);
                        }
                    }
                }
                else
                {
                    String[] files = Directory.GetFiles(charFolder, "*.cgbc", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        try
                        {
                            characters.Add(EditorFileWriter.BaseCharacterReader(item.Replace(Game1.rootContentExtra, "")));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error reading file: " + item);
                        }
                    }
                }

                foreach (var character in characters)
                {
                    listBox6.Items.Add(character);
                    if (character.CharacterName.Equals(bc.CharacterName))
                    {
                        listBox6.SelectedIndex = characters.IndexOf(character);
                    }
                }


            }
            else
            {
                MessageBox.Show("Cancelled saving character named: " + bc.CharacterName);
            }
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Enter(object sender, EventArgs e)
        {

            listBox9.SelectedIndex = -1;
            listBox11.SelectedIndex = -1;
            label29.Visible = false;
            label30.Visible = false;
            label31.Visible = false;
            label32.Visible = false;


            numericUpDown4.Visible = false;


            pictureBox5.Visible = false;
            pictureBox4.Visible = false;

            listBox11.Visible = false;
            listBox11.Items.Clear();

            button11.Visible = false;
            button12.Visible = false;
            button15.Visible = false;

            #region
            /*
            listBox9.Items.Clear();

            if (charFolder.Equals(""))
            {
                characters.Clear();
                FolderBrowserDialog folderChoice = new FolderBrowserDialog();
                folderChoice.Description = "Please select folder where to load characters from!";

                bool bDone = false;
                while (!bDone)
                {
                    if (Game1.bIsDebug)
                    {
                        folderChoice.SelectedPath = Game1.rootTBAGW;
                    }
                    else
                    {
                        folderChoice.SelectedPath = Game1.rootContentExtra;
                    }

                    if (folderChoice.ShowDialog() == DialogResult.OK)
                    {
                        if (Game1.bIsDebug && folderChoice.SelectedPath.StartsWith(Game1.rootTBAGW))
                        {
                            charFolder = folderChoice.SelectedPath;
                            bDone = true;
                        }
                        else if (folderChoice.SelectedPath.StartsWith(Game1.rootContentExtra))
                        {
                            charFolder = folderChoice.SelectedPath;
                            bDone = true;
                        }

                    }
                }

                if (Game1.bIsDebug)
                {
                    String[] files = Directory.GetFiles(charFolder, "*.cgbcc", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        try
                        {
                            characters.Add(EditorFileWriter.BaseCharacterReader(item));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error reading file: " + item);
                        }
                    }
                }
                else
                {
                    String[] files = Directory.GetFiles(charFolder, "*.cgbc", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        try
                        {
                            characters.Add(EditorFileWriter.BaseCharacterReader(item.Replace(Game1.rootContentExtra, "")));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error reading file: " + item);
                        }
                    }
                }
            }

            foreach (var character in characters)
            {
                listBox9.Items.Add(character);
            }
            */
            #endregion
            listBox9.Items.Clear();
            listBox9.Items.AddRange(MapBuilder.gcDB.gameCharacters.ToArray());

            foreach (var item in Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations)))
            {
                listBox11.Items.Add(item);
            }

            listBox7.Items.Clear();
            listBox7.SelectedIndex = -1;
            listBox7.Items.AddRange(Enum.GetNames(typeof(BaseCharacter.PortraitExpressions)));

        }

        private void listBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {
                #region visibilities
                label29.Visible = true;
                label30.Visible = true;
                label31.Visible = true;
                label32.Visible = true;
               

                numericUpDown4.Visible = true;
              

                pictureBox5.Visible = true;
                pictureBox4.Visible = true;

                listBox11.Visible = true;

                button11.Visible = true;
                button12.Visible = true;
                
                button15.Visible = true;
                #endregion
                listBox11.SelectedIndex = -1;
                if (((BaseCharacter)listBox9.SelectedItem).charBattleAnimations.Count != Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations)).Length)
                {
                    ((BaseCharacter)listBox9.SelectedItem).charBattleAnimations.Clear();
                    foreach (var item in Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations)))
                    {
                        ((BaseCharacter)listBox9.SelectedItem).charBattleAnimations.Add(new ShapeAnimation());
                        ((BaseCharacter)listBox9.SelectedItem).charBattleAnimations[((BaseCharacter)listBox9.SelectedItem).charBattleAnimations.Count - 1].texFileLoc = ((BaseCharacter)listBox9.SelectedItem).charAnimations[(int)BaseCharacter.CharacterAnimations.Idle].texFileLoc;
                        ((BaseCharacter)listBox9.SelectedItem).charBattleAnimations[((BaseCharacter)listBox9.SelectedItem).charBattleAnimations.Count - 1].animationTexture = ((BaseCharacter)listBox9.SelectedItem).charAnimations[(int)BaseCharacter.CharacterAnimations.Idle].animationTexture;
                    }
                }

                try
                {
                    numericUpDown19.Value = (listBox9.SelectedItem as BaseCharacter).climaxAttackFrame;
                }
                catch (ArgumentOutOfRangeException)
                {
                    (listBox9.SelectedItem as BaseCharacter).climaxAttackFrame = (int)numericUpDown19.Minimum;
                    numericUpDown19.Value = (listBox9.SelectedItem as BaseCharacter).climaxAttackFrame;
                }

                try
                {
                    numericUpDown18.Value = (listBox9.SelectedItem as BaseCharacter).climaxAttackFrame;
                }
                catch (ArgumentOutOfRangeException)
                {
                    (listBox9.SelectedItem as BaseCharacter).climaxAttackFrame = (int)numericUpDown18.Minimum;
                    numericUpDown18.Value = (listBox9.SelectedItem as BaseCharacter).climaxAttackFrame;
                }
            }
        }

        private void listBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox11.SelectedIndex != -1)
            {
                ShapeAnimation sa = ((BaseCharacter)listBox9.SelectedItem).charBattleAnimations[listBox11.SelectedIndex];
                numericUpDown4.Value = ((BaseCharacter)listBox9.SelectedItem).charBattleAnimations[listBox11.SelectedIndex].frameInterval;

                if (sa.animationTexture != null)
                {
                    Texture2D charSprite = sa.animationTexture;
                    byte[] data = new byte[charSprite.Width * charSprite.Height];
                    MemoryStream m = new MemoryStream(data);
                    charSprite.SaveAsPng(m, charSprite.Width, charSprite.Height);
                    Bitmap bmp = new System.Drawing.Bitmap(m);
                    pictureBox4.Image = bmp;
                    pictureBox4.Refresh();
                }
            }
        }

        static Forms.Animation.AnimationEditor ae = new Animation.AnimationEditor();

        private void button15_Click(object sender, EventArgs e)
        {
            //button12.Visible = true;
            //ShapeAnimation sa = ((BaseCharacter)listBox9.SelectedItem).charBattleAnimations[listBox11.SelectedIndex];
            //if (listBox11.SelectedIndex != -1)
            //{
            //    FrameSelector.StartComplex(sa, (int)numericUpDown5.Value, (int)numericUpDown6.Value, (int)numericUpDown8.Value, (int)numericUpDown7.Value);
            //}

            Form1.MakeSureFormClosed(ae);
            ae = new Animation.AnimationEditor();
            ae.Start(((BaseCharacter)listBox9.SelectedItem).charBattleAnimations[listBox11.SelectedIndex]);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (listBox11.SelectedIndex != -1)
            {
                ((BaseCharacter)listBox9.SelectedItem).charBattleAnimations[listBox11.SelectedIndex].frameInterval = (int)numericUpDown4.Value;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            BaseCharacter bc = ((BaseCharacter)listBox9.SelectedItem);
            SaveFileDialog saveBC = new SaveFileDialog();
            if (Game1.bIsDebug)
            {
                saveBC.InitialDirectory = Game1.rootTBAGW;
            }
            else
            {
                saveBC.InitialDirectory = Game1.rootContentExtra;
            }

            if (saveBC.ShowDialog() == DialogResult.OK)
            {
                bc.SaveCharacter();
                EditorFileWriter.BasicSpriteWriter(saveBC.FileName, bc);
                bc = EditorFileWriter.BaseCharacterReader(saveBC.FileName);

                listBox9.Items.Clear();
                characters.Clear();
                if (Game1.bIsDebug)
                {
                    String[] files = Directory.GetFiles(charFolder, "*.cgbcc", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        try
                        {
                            characters.Add(EditorFileWriter.BaseCharacterReader(item));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error reading file: " + item);
                        }
                    }
                }
                else
                {
                    String[] files = Directory.GetFiles(charFolder, "*.cgbc", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        try
                        {
                            characters.Add(EditorFileWriter.BaseCharacterReader(item.Replace(Game1.rootContentExtra, "")));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error reading file: " + item);
                        }
                    }
                }

                foreach (var character in characters)
                {
                    listBox9.Items.Add(character);
                    if (character.CharacterName.Equals(bc.CharacterName))
                    {
                        listBox9.SelectedIndex = characters.IndexOf(character);
                    }
                }


            }
            else
            {
                MessageBox.Show("Cancelled saving character named: " + bc.CharacterName);
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            FrameSelector.Stop();
            button12.Visible = false;
        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {
            #region
            /*
                if (!charFolder.Equals(""))
                {
                    characters.Clear();
                    if (Game1.bIsDebug)
                    {
                        String[] files = Directory.GetFiles(charFolder, "*.cgbcc", SearchOption.AllDirectories);
                        foreach (var item in files)
                        {
                            try
                            {
                                characters.Add(EditorFileWriter.BaseCharacterReader(item));
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Error reading file: " + item);
                            }
                        }
                    }
                    else
                    {
                        String[] files = Directory.GetFiles(charFolder, "*.cgbc", SearchOption.AllDirectories);
                        foreach (var item in files)
                        {
                            try
                            {
                                characters.Add(EditorFileWriter.BaseCharacterReader(item.Replace(Game1.rootContentExtra, "")));
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Error reading file: " + item);
                            }
                        }
                    }
                }
                comboBox12.Items.Clear();
                comboBox13.Items.Clear();
                button13.Visible = false;
                foreach (var item in characters)
                {
                    comboBox12.Items.Add(item);
                    comboBox13.Items.Add(item);
                }
                */
            #endregion

            comboBox12.Items.Clear();
            comboBox13.Items.Clear();
            button13.Visible = false;
            foreach (var item in MapBuilder.gcDB.gameCharacters)
            {
                comboBox12.Items.Add(item);
                comboBox13.Items.Add(item);
            }

            comboBox12.SelectedIndex = -1;
            comboBox13.SelectedIndex = -1;
            comboBox14.SelectedIndex = -1;
            comboBox15.SelectedIndex = -1;

            #region visibilities

            button16.Visible = false;
            checkBox2.Visible = true;
            label39.Visible = false;
            label40.Visible = false;
            label41.Visible = false;
            label42.Visible = false;
            label43.Visible = false;
            label44.Visible = false;
            label45.Visible = false;
            label47.Visible = false;
            comboBox14.Visible = false;
            comboBox15.Visible = false;
            numericUpDown9.Visible = false;
            numericUpDown10.Visible = false;
            numericUpDown11.Visible = false;
            numericUpDown12.Visible = false;
            numericUpDown13.Visible = false;
            numericUpDown14.Visible = false;
            #endregion
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MapBuilder.bDrawGUI = !MapBuilder.bDrawGUI;
            BaseCharacter g = (BaseCharacter)((BaseCharacter)comboBox12.SelectedItem);
            BaseCharacter r = (BaseCharacter)((BaseCharacter)comboBox13.SelectedItem);
            String FistIcon = @"Graphics\Items\Icons\FistIcon";
            String ShieldIcon = @"Graphics\Items\Icons\ShieldIcon";
            #region statchart check
            if (g.statChart.currentActiveStats.Count == 0)
            {
                g.statChart = new STATChart(true);
                Console.WriteLine("Error in statchart: " + g + " ---Creating a new one");
            }
            if (r.statChart.currentActiveStats.Count == 0)
            {
                r.statChart = new STATChart(true);
                Console.WriteLine("Error in statchart: " + r + " ---Creating a new one");
            }
            #endregion


            if (g.battleAnimLocs.Count != g.charBattleAnimations.Count)
            {
                g.battleAnimLocs.Clear();
                foreach (var item in g.charBattleAnimations)
                {
                    g.battleAnimLocs.Add(new Vector2(100, 100));
                }
            }

            if (r.battleAnimLocs.Count != r.charBattleAnimations.Count)
            {
                r.battleAnimLocs.Clear();
                foreach (var item in r.charBattleAnimations)
                {
                    r.battleAnimLocs.Add(new Vector2(100, 100));
                }
            }

            BasicAbility gAbi = new BasicAbility();
            BasicAbility rAbi = new BasicAbility();
            //rAbi.abilityName = "Defend";
            //rAbi.abilityIconTexLoc = ShieldIcon;
            //rAbi.ReloadTexture();
            //rAbi.iconTexBox = rAbi.abilityIcon.Bounds;
            //gAbi.abilityName = "Melee attack";
            //gAbi.abilityIconTexLoc = FistIcon;
            //gAbi.ReloadTexture();
            //gAbi.iconTexBox = gAbi.abilityIcon.Bounds;
            /*
            BaseWeapon gWp = new BaseWeapon("Longsword");
            BaseWeapon rWp = new BaseWeapon("Dual daggers");
            g.charEquipment[(int)BaseItem.EQUIP_TYPES.Weapon] = gWp;
            r.charEquipment[(int)BaseItem.EQUIP_TYPES.Weapon] = rWp;*/
            BattleGUI.Start(g.statChart, r.statChart, g, r, gAbi, rAbi);
            Console.WriteLine(g + "     " + r);
            #region visibilities
            label39.Visible = true;
            label40.Visible = true;
            comboBox15.Visible = true;
            comboBox14.Visible = true;
            button16.Visible = true;
            checkBox2.Visible = true;
            #endregion
            comboBox15.Items.Clear();
            comboBox14.Items.Clear();
            foreach (var item in Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations)))
            {
                comboBox15.Items.Add(item);
                comboBox14.Items.Add(item);
            }

        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox12.SelectedIndex != -1)
            {
                GameProcessor.g = ((BaseCharacter)comboBox12.SelectedItem);

                numericUpDown2.Value = (decimal)((BaseCharacter)comboBox12.SelectedItem).battleAnimShadowAdjustLocs.X;
                numericUpDown3.Value = (decimal)((BaseCharacter)comboBox12.SelectedItem).battleAnimShadowAdjustLocs.Y;
            }
            if (comboBox12.SelectedIndex != -1 && comboBox13.SelectedIndex != -1)
            {
                button13.Visible = true;
                GameProcessor.g = ((BaseCharacter)comboBox12.SelectedItem);

                numericUpDown2.Value = (decimal)((BaseCharacter)comboBox12.SelectedItem).battleAnimShadowAdjustLocs.X;
                numericUpDown3.Value = (decimal)((BaseCharacter)comboBox12.SelectedItem).battleAnimShadowAdjustLocs.Y;
            }
            else
            {
                button13.Visible = false;
            }
        }

        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox13.SelectedIndex != -1)
            {
                GameProcessor.r = ((BaseCharacter)comboBox13.SelectedItem);

                numericUpDown17.Value = (decimal)((BaseCharacter)comboBox13.SelectedItem).battleAnimShadowAdjustLocs.X;
                numericUpDown16.Value = (decimal)((BaseCharacter)comboBox13.SelectedItem).battleAnimShadowAdjustLocs.Y;
            }
            if (comboBox12.SelectedIndex != -1 && comboBox13.SelectedIndex != -1)
            {
                button13.Visible = true;
                GameProcessor.r = ((BaseCharacter)comboBox13.SelectedItem);

                numericUpDown17.Value = (decimal)((BaseCharacter)comboBox13.SelectedItem).battleAnimShadowAdjustLocs.X;
                numericUpDown16.Value = (decimal)((BaseCharacter)comboBox13.SelectedItem).battleAnimShadowAdjustLocs.Y;
            }
            else
            {
                button13.Visible = false;
            }
        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox15.SelectedIndex != -1)
            {
                label44.Visible = true;
                label41.Visible = true;
                label42.Visible = true;
                label43.Visible = true;
                numericUpDown9.Visible = true;
                numericUpDown10.Visible = true;
                numericUpDown11.Visible = true;

                numericUpDown2.Value = (decimal)((BaseCharacter)comboBox12.SelectedItem).battleAnimShadowAdjustLocs.X;
                numericUpDown3.Value = (decimal)((BaseCharacter)comboBox12.SelectedItem).battleAnimShadowAdjustLocs.Y;

                try
                {
                    numericUpDown9.Value = (int)((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs[comboBox15.SelectedIndex].X;
                    numericUpDown10.Value = (int)((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs[comboBox15.SelectedIndex].Y;
                }
                catch
                {
                    numericUpDown9.Value = 0;
                    numericUpDown10.Value = 0;
                }

                numericUpDown11.Value = (decimal)((BaseCharacter)comboBox12.SelectedItem).battleAnimScale;

                if (((BaseCharacter)comboBox12.SelectedItem).charBattleAnimations[comboBox15.SelectedIndex].animationFrames.Count != 0)
                {
                    ((BaseCharacter)comboBox12.SelectedItem).animationBattleIndex = comboBox15.SelectedIndex;
                }
                else
                {
                    MessageBox.Show("Character: " + ((BaseCharacter)comboBox12.SelectedItem) + " doesn't have animation" + Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations))[comboBox15.SelectedIndex]);
                }

            }
        }

        private void comboBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox14.SelectedIndex != -1)
            {
                label44.Visible = true;
                label47.Visible = true;
                label46.Visible = true;
                label45.Visible = true;
                numericUpDown14.Visible = true;
                numericUpDown13.Visible = true;
                numericUpDown12.Visible = true;


                numericUpDown12.Value = (decimal)((BaseCharacter)comboBox13.SelectedItem).battleAnimScale;

                numericUpDown17.Value = (decimal)((BaseCharacter)comboBox13.SelectedItem).battleAnimShadowAdjustLocs.X;
                numericUpDown16.Value = (decimal)((BaseCharacter)comboBox13.SelectedItem).battleAnimShadowAdjustLocs.Y;

                try
                {

                    numericUpDown14.Value = (int)((BaseCharacter)comboBox13.SelectedItem).battleAnimLocs[comboBox14.SelectedIndex].X;
                    numericUpDown13.Value = (int)((BaseCharacter)comboBox13.SelectedItem).battleAnimLocs[comboBox14.SelectedIndex].Y;
                }
                catch
                {
                    numericUpDown14.Value = 0;
                    numericUpDown13.Value = 0;
                }

                if (((BaseCharacter)comboBox13.SelectedItem).charBattleAnimations[comboBox14.SelectedIndex].animationFrames.Count != 0)
                {
                    ((BaseCharacter)comboBox13.SelectedItem).animationBattleIndex = comboBox14.SelectedIndex;
                }
                else
                {
                    MessageBox.Show("Character: " + ((BaseCharacter)comboBox13.SelectedItem) + " doesn't have animation" + Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations))[comboBox14.SelectedIndex]);
                }
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to save the location of the current battle animation for every battle animation of the selected character?", "Save locations for every battle animation?", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Vector2 loc = new Vector2((int)numericUpDown9.Value, (int)numericUpDown10.Value);
                for (int i = 0; i < ((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs.Count; i++)
                {
                    try
                    {
                        ((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs[i] = loc;
                    }
                    catch
                    {
                        ((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs.Add(loc);
                    }
                }
               ((BaseCharacter)comboBox12.SelectedItem).battleAnimScale = (int)numericUpDown11.Value;

                loc = new Vector2((int)numericUpDown14.Value, (int)numericUpDown13.Value);
                for (int i = 0; i < ((BaseCharacter)comboBox13.SelectedItem).battleAnimLocs.Count; i++)
                {
                    try
                    {
                        ((BaseCharacter)comboBox13.SelectedItem).battleAnimLocs[i] = loc;
                    }
                    catch
                    {
                        ((BaseCharacter)comboBox13.SelectedItem).battleAnimLocs.Add(loc);
                    }
                }
                ((BaseCharacter)comboBox13.SelectedItem).battleAnimScale = (int)numericUpDown12.Value;

            }

            BaseCharacter bc = ((BaseCharacter)comboBox12.SelectedItem);
            System.Windows.Forms.MessageBox.Show("Saving \"Left\": " + ((BaseCharacter)comboBox12.SelectedItem) + " character first.");
            SaveFileDialog saveBC = new SaveFileDialog();
            if (Game1.bIsDebug)
            {
                saveBC.InitialDirectory = Game1.rootTBAGW;
            }
            else
            {
                saveBC.InitialDirectory = Game1.rootContentExtra;
            }

            if (saveBC.ShowDialog() == DialogResult.OK)
            {
                bc.SaveCharacter();
                EditorFileWriter.BasicSpriteWriter(saveBC.FileName, bc);



            }
            else
            {
                MessageBox.Show("Cancelled saving character named: " + bc.CharacterName);
            }

            bc = ((BaseCharacter)comboBox13.SelectedItem);
            System.Windows.Forms.MessageBox.Show("Saving \"Right\": " + ((BaseCharacter)comboBox13.SelectedItem) + " character.");
            saveBC = new SaveFileDialog();
            if (Game1.bIsDebug)
            {
                saveBC.InitialDirectory = Game1.rootTBAGW;
            }
            else
            {
                saveBC.InitialDirectory = Game1.rootContentExtra;
            }

            if (saveBC.ShowDialog() == DialogResult.OK)
            {
                bc.SaveCharacter();
                EditorFileWriter.BasicSpriteWriter(saveBC.FileName, bc);
            }
            else
            {
                MessageBox.Show("Cancelled saving character named: " + bc.CharacterName);
            }
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox14.SelectedIndex != -1 && comboBox15.SelectedIndex != -1 && (int)numericUpDown10.Value != 0)
            {
                // ((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs.ForEach(v => v = new Vector2((int)numericUpDown9.Value, (int)numericUpDown10.Value));
                ((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs.Clear();
                for (int i = 0; i < Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations)).Length; i++)
                {
                    ((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs.Add(new Vector2((int)numericUpDown9.Value, (int)numericUpDown10.Value));
                }
            }
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox14.SelectedIndex != -1 && comboBox15.SelectedIndex != -1 && (int)numericUpDown9.Value != 0)
            {
                // ((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs.ForEach(v => v = new Vector2((int)numericUpDown9.Value, (int)numericUpDown10.Value));
                ((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs.Clear();
                for (int i = 0; i < Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations)).Length; i++)
                {
                    ((BaseCharacter)comboBox12.SelectedItem).battleAnimLocs.Add(new Vector2((int)numericUpDown9.Value, (int)numericUpDown10.Value));
                }
            }
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox14.SelectedIndex != -1 && comboBox15.SelectedIndex != -1)
            {
                ((BaseCharacter)comboBox12.SelectedItem).battleAnimScale = (float)numericUpDown11.Value;
            }
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox14.SelectedIndex != -1 && comboBox15.SelectedIndex != -1 && (int)numericUpDown13.Value != 0)
            {
                ((BaseCharacter)comboBox13.SelectedItem).battleAnimLocs.Clear();
                for (int i = 0; i < Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations)).Length; i++)
                {
                    ((BaseCharacter)comboBox13.SelectedItem).battleAnimLocs.Add(new Vector2((int)numericUpDown14.Value, (int)numericUpDown13.Value));
                }
                //((BaseCharacter)comboBox13.SelectedItem).battleAnimLocs.ForEach(v=>v = new Vector2((int)numericUpDown14.Value, (int)numericUpDown13.Value));
            }
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox13.SelectedIndex != -1 && (int)numericUpDown14.Value != 0)
            {
                ((BaseCharacter)comboBox13.SelectedItem).battleAnimLocs.Clear();
                for (int i = 0; i < Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations)).Length; i++)
                {
                    ((BaseCharacter)comboBox13.SelectedItem).battleAnimLocs.Add(new Vector2((int)numericUpDown14.Value, (int)numericUpDown13.Value));
                }
            }
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox14.SelectedIndex != -1 && comboBox15.SelectedIndex != -1)
            {
                ((BaseCharacter)comboBox13.SelectedItem).battleAnimScale = (float)numericUpDown12.Value;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            BattleGUI.bDrawFrames = checkBox2.Checked;
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            if (listBox10.SelectedIndex != -1)
            {
                MapBuilder.parentMap.mapTeams[listBox10.SelectedIndex].teamIdentifier = (int)numericUpDown15.Value;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            MapBuilder.SaveMap();
            int prevIndex = listBox10.SelectedIndex;
            listBox10.Items.Clear();
            foreach (var team in MapBuilder.parentMap.mapTeams)
            {
                listBox10.Items.Add(team);
            }
            listBox10.SelectedIndex = prevIndex;
        }

        private void listBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox12.Items.Clear();

            if (listBox10.SelectedIndex != -1)
            {
                foreach (var characters in MapBuilder.parentMap.mapTeams[listBox10.SelectedIndex].teamMembers)
                {
                    BaseCharacter temp = MapBuilder.gcDB.gameCharacters.Find(c => c.shapeID == characters);
                    listBox12.Items.Add(temp);
                }
            }

            numericUpDown15.Value = MapBuilder.parentMap.mapTeams[listBox10.SelectedIndex].teamIdentifier;
            textBox3.Text = MapBuilder.parentMap.mapTeams[listBox10.SelectedIndex].teamName;
        }

        private void loadCharacters()
        {
            characters.Clear();
            if (Game1.bIsDebug)
            {
                String[] files = Directory.GetFiles(charFolder, "*.cgbcc", SearchOption.AllDirectories);
                foreach (var item in files)
                {
                    try
                    {
                        characters.Add(EditorFileWriter.BaseCharacterReader(item));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error reading file: " + item);
                    }
                }
            }
            else
            {
                String[] files = Directory.GetFiles(charFolder, "*.cgbc", SearchOption.AllDirectories);
                foreach (var item in files)
                {
                    try
                    {
                        characters.Add(EditorFileWriter.BaseCharacterReader(item.Replace(Game1.rootContentExtra, "")));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error reading file: " + item);
                    }
                }
            }
        }

        private void getCharFolder()
        {
            FolderBrowserDialog folderChoice = new FolderBrowserDialog();
            folderChoice.Description = "Please select folder where to load characters from!";

            bool bDone = false;
            while (!bDone)
            {
                if (Game1.bIsDebug)
                {
                    folderChoice.SelectedPath = Game1.rootTBAGW;
                }
                else
                {
                    folderChoice.SelectedPath = Game1.rootContentExtra;
                }

                if (folderChoice.ShowDialog() == DialogResult.OK)
                {
                    if (Game1.bIsDebug && folderChoice.SelectedPath.StartsWith(Game1.rootTBAGW))
                    {
                        charFolder = folderChoice.SelectedPath;
                        bDone = true;
                    }
                    else if (folderChoice.SelectedPath.StartsWith(Game1.rootContentExtra))
                    {
                        charFolder = folderChoice.SelectedPath;
                        bDone = true;
                    }

                }
            }


        }

        private void tabPage6_Enter(object sender, EventArgs e)
        {
            listBox10.Items.Clear();
            listBox13.Items.Clear();
            listBox12.Items.Clear();
            foreach (var team in MapBuilder.parentMap.mapTeams)
            {
                listBox10.Items.Add(team);
            }

            if (!charFolder.Equals(""))
            {
                loadCharacters();
            }

            foreach (var character in MapBuilder.gcDB.gameCharacters)
            {
                listBox13.Items.Add(character);
            }
        }

        private void listBox13_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            listBox13.SelectedIndex = -1;



            if (!charFolder.Equals(""))
            {
                loadCharacters();
            }
            else
            {
                getCharFolder();
                loadCharacters();
            }

            foreach (var character in characters)
            {
                listBox13.Items.Add(character);
            }
        }

        private void listBox12_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (listBox10.SelectedIndex != -1)
            {
                MapBuilder.parentMap.mapTeams[listBox10.SelectedIndex].teamName = textBox3.Text;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            MapBuilder.parentMap.mapTeams.Add(new BaseTeam());

            tabPage6_Enter(sender, e);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (listBox10.SelectedIndex != -1)
            {
                MapBuilder.parentMap.mapTeams.RemoveAt(listBox10.SelectedIndex);
                tabPage6_Enter(sender, e);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (listBox13.SelectedIndex != -1 && listBox10.SelectedIndex != -1)
            {
                bool bHasThatChar = false;
                foreach (var item in MapBuilder.parentMap.mapTeams[listBox10.SelectedIndex].teamMembers)
                {
                    BaseCharacter temp = MapBuilder.gcDB.gameCharacters.Find(c => c.shapeID == item);
                    if (temp == ((BaseCharacter)listBox13.SelectedItem))
                    {
                        bHasThatChar = true;
                        break;
                    }
                }

                if (!bHasThatChar)
                {
                    MapBuilder.parentMap.mapTeams[listBox10.SelectedIndex].teamMembers.Add(((BaseCharacter)listBox13.SelectedItem).shapeID);
                    listBox12.Items.Clear();
                    foreach (var characters in MapBuilder.parentMap.mapTeams[listBox10.SelectedIndex].teamMembers)
                    {
                        BaseCharacter temp = MapBuilder.gcDB.gameCharacters.Find(c => c.shapeID == characters);
                        listBox12.Items.Add(temp);
                    }

                }
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (listBox12.SelectedIndex != -1)
            {
                MapBuilder.parentMap.mapTeams[listBox10.SelectedIndex].teamMembers.RemoveAt(listBox12.SelectedIndex);
                listBox12.Items.Clear();
                foreach (var characters in MapBuilder.parentMap.mapTeams[listBox10.SelectedIndex].teamMembers)
                {
                    BaseCharacter temp = MapBuilder.gcDB.gameCharacters.Find(c => c.shapeID == characters);
                    listBox12.Items.Add(temp);
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer4_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        CopyCharForm copyChar = new CopyCharForm();
        private void button23_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                copyChar.Start((BaseCharacter)listBox3.SelectedItem);
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                OpenFileDialog savePrefab = new OpenFileDialog();
                if (TBAGW.Game1.bIsDebug)
                {
                    savePrefab.Filter = "Texture file|*.xnb";
                    savePrefab.InitialDirectory = TBAGW.Game1.rootContent;

                }
                else
                {
                    savePrefab.Filter = "Texture file|*.png;*.jpg";
                    savePrefab.InitialDirectory = TBAGW.Game1.rootContentExtra;
                }

                savePrefab.Title = "open texture";
                DialogResult dia = savePrefab.ShowDialog();
                if (DialogResult.OK == dia && savePrefab.FileName.Contains(savePrefab.InitialDirectory))
                {
                    String fi = Path.GetFileNameWithoutExtension(savePrefab.FileName);
                    String fo = Path.GetDirectoryName(savePrefab.FileName);
                    fo = fo.Replace(Game1.rootContent, "") + @"\";
                    fi = fo + fi;
                    BaseCharacter temp = (BaseCharacter)listBox3.SelectedItem;
                    foreach (var item in temp.charAnimations)
                    {
                        item.texFileLoc = fi;
                    }
                    foreach (var item in temp.charBattleAnimations)
                    {
                        item.texFileLoc = fi;
                    }
                }
                else if (System.Windows.Forms.DialogResult.Cancel == dia)
                {

                    System.Windows.Forms.MessageBox.Show("Cancelled, returning to Editor.");
                }
                else if (!savePrefab.FileName.Contains(savePrefab.InitialDirectory))
                {
                    System.Windows.Forms.MessageBox.Show("Save within mods folder please.");
                }
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            /*
                if (characters.Count != 0)
                {
                    if (listBox10.SelectedIndex != -1)
                    {
                        List<BaseCharacter> tempList = new List<BaseCharacter>(MapBuilder.loadedMap.mapTeams[listBox10.SelectedIndex].teamMembers);
                        foreach (var item in MapBuilder.loadedMap.mapTeams[listBox10.SelectedIndex].teamMembers)
                        {
                            var t = characters.Find(c => c.shapeID == item.shapeID && c.CharacterName.Equals(item.CharacterName));
                            if (t != null)
                            {
                                tempList[MapBuilder.loadedMap.mapTeams[listBox10.SelectedIndex].teamMembers.IndexOf(item)] = t;
                                Console.WriteLine("Changed character: " + t.ToString());
                            }
                        }
                        MapBuilder.loadedMap.mapTeams[listBox10.SelectedIndex].teamMembers = tempList;

                    }
                }*/
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                STATChartForm form = new STATChartForm();
                if (((BaseCharacter)listBox6.SelectedItem).statChart.currentActiveStats.Count == 0)
                {
                    ((BaseCharacter)listBox6.SelectedItem).statChart.DefaultStatChart();
                }
                form.Start(((BaseCharacter)listBox6.SelectedItem).statChart);
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {
                UpdateBattleAnimationInfoOfCharactersThisMap((BaseCharacter)listBox9.SelectedItem);
            }
        }

        private void UpdateBattleAnimationInfoOfCharactersThisMap(BaseCharacter bc)
        {

        }

        private void UpdateAnimationInfoOfCharactersThisMap(BaseCharacter bc)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                UpdateAnimationInfoOfCharactersThisMap((BaseCharacter)listBox3.SelectedItem);
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                if (!MapBuilder.gcDB.gameCharacters.Contains((BaseCharacter)listBox3.SelectedItem))
                {
                    MapBuilder.gcDB.AddCharacter((BaseCharacter)listBox3.SelectedItem);
                }
            }
        }

        private void label52_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer6_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (comboBox12.SelectedIndex != -1 && comboBox15.SelectedIndex != -1)
            {
                BaseCharacter temp = ((BaseCharacter)comboBox12.SelectedItem);
                Vector2 tempLoc = new Vector2((int)numericUpDown9.Value, (int)numericUpDown10.Value);
                int t = temp.battleAnimLocs.Count;
                temp.battleAnimLocs.Clear();
                for (int i = 0; i < t; i++)
                {
                    temp.battleAnimLocs.Add(tempLoc);
                }
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (comboBox13.SelectedIndex != -1 && comboBox14.SelectedIndex != -1)
            {
                BaseCharacter temp = ((BaseCharacter)comboBox13.SelectedItem);
                Vector2 tempLoc = new Vector2((int)numericUpDown14.Value, (int)numericUpDown13.Value);
                int t = temp.battleAnimLocs.Count;
                temp.battleAnimLocs.Clear();
                for (int i = 0; i < t; i++)
                {
                    temp.battleAnimLocs.Add(tempLoc);
                }
            }
        }

        private void splitContainer5_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button33_Click(object sender, EventArgs e)
        {
            Form1.MakeSureFormClosed(ae);
            ae = new Animation.AnimationEditor();
            ae.Start(((BaseCharacter)listBox9.SelectedItem).portraitAnimations[listBox7.SelectedIndex]);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                CharacterUniqueEditor cue = new CharacterUniqueEditor();
                cue.Start((BaseCharacter)listBox6.SelectedItem);
            }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                HitboxEditor.Start((BaseCharacter)listBox6.SelectedItem);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox6.SelectedIndex = -1;
                listBox6.Items.Clear();
                listBox6.Items.AddRange(MapBuilder.gcDB.gameCharacters.ToArray());
            }
            else
            {
                listBox6.SelectedIndex = -1;
                listBox6.Items.Clear();
                listBox6.Items.AddRange(MapBuilder.gcDB.gameCharacters.FindAll(gc => gc.CharacterName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToArray());
            }
        }
        ClassCollectionEditor cce;
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                Form1.MakeSureFormClosed(cce);
                cce = new ClassCollectionEditor();
                cce.Start((BaseCharacter)listBox6.SelectedItem);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CharacterPortraitEditor cpe = new CharacterPortraitEditor();
            cpe.Start((BaseCharacter)listBox6.SelectedItem);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1 && !textBox4.Text.Equals("", StringComparison.OrdinalIgnoreCase))
            {
                var temp = listBox6.SelectedItem as BaseCharacter;
                temp.displayName = textBox4.Text;
            }
        }

        LootEditor le = new LootEditor();
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                MakeSureLeIsGone();
                le = new LootEditor();
                le.Start(listBox6.SelectedItem as BaseCharacter);
            }

        }

        private void MakeSureLeIsGone()
        {
            if (!le.IsDisposed)
            {
                le.Close();
                le.Dispose();

            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

            if (comboBox12.SelectedIndex != -1)
            {
                GameProcessor.g.battleAnimShadowAdjustLocs.X = (float)numericUpDown2.Value;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox12.SelectedIndex != -1)
            {
                GameProcessor.g.battleAnimShadowAdjustLocs.Y = (float)numericUpDown3.Value;
            }
        }

        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox13.SelectedIndex != -1)
            {
                GameProcessor.r.battleAnimShadowAdjustLocs.X = (float)numericUpDown17.Value;
            }
        }

        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox13.SelectedIndex != -1)
            {
                GameProcessor.r.battleAnimShadowAdjustLocs.Y = (float)numericUpDown16.Value;
            }
        }

        private void numericUpDown19_ValueChanged(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {
                int max = (listBox9.SelectedItem as BaseCharacter).charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Attack].animationFrames.Count;
                if (numericUpDown19.Value >= max && max != 0)
                {
                    numericUpDown19.Value = max - 1;
                }
                else if (max == 0)
                {
                    numericUpDown19.Value = 0;
                }

                (listBox9.SelectedItem as BaseCharacter).climaxAttackFrame = (int)numericUpDown19.Value;
            }
        }

        private void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            if (listBox9.SelectedIndex != -1)
            {
                int max = (listBox9.SelectedItem as BaseCharacter).charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Hurt].animationFrames.Count;
                if (numericUpDown18.Value >= max && max != 0)
                {
                    numericUpDown18.Value = max - 1;
                }
                else if (max == 0)
                {
                    numericUpDown18.Value = 0;
                }

                  (listBox9.SelectedItem as BaseCharacter).climaxAttackFrame = (int)numericUpDown18.Value;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button32_Click(object sender, EventArgs e)
        {

        }
    }
}
