using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Scenes.Editor
{
    public static class AnimationEditor
    {
        #region FIELDS
        static System.Windows.Forms.OpenFileDialog BasicSpriteSelector;
        static String root = Environment.CurrentDirectory;
        public static bool bRunFileSelector = false;
        #endregion

        public static void Start()
        {
            BasicSpriteSelector = new System.Windows.Forms.OpenFileDialog();
            bRunFileSelector = true;
            BasicSpriteSelector.InitialDirectory = Environment.CurrentDirectory;

            BasicSpriteSelector.Filter = "CGBS Files (.cgbs)|*.cgbs";
            BasicSpriteSelector.FilterIndex = 1;
            BasicSpriteSelector.Multiselect = false;

            System.Windows.Forms.DialogResult testDia = BasicSpriteSelector.ShowDialog();

            if (testDia == System.Windows.Forms.DialogResult.OK)
            {
                Console.WriteLine("You selected: " + BasicSpriteSelector.FileName);
                BaseSprite test = EditorFileWriter.BasicSpriteReader(BasicSpriteSelector.FileName);
                MapBuilder.testSprite = test;
                bRunFileSelector = false;
            }
        }
    }
}
