using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities;
using TBAGW.Utilities.Actions;

namespace TBAGW
{
    [XmlRoot("Settings File")]
    public class SettingsFile
    {
        [XmlElement("Generate shadows")]
        public bool bShadowGeneration = true;
        [XmlElement("Generate reflection")]
        public bool bReflectionGeneration = true;
        [XmlElement("Shadow Quality")]
        public int shadowPercentage = 50;
        [XmlElement("Resolution")]
        public Vector2 resolution = new Vector2(1366, 768);
        [XmlElement("FullScreen")]
        public bool bFullScreen = false;
        [XmlElement("Battle Speed")]
        public int battleSpeed = 1;
        [XmlElement("Battle Camera Speed")]
        public int battleCameraSpeed = 1;
        [XmlElement("Master Volume level")]
        public int MasterVolume = 80;
        [XmlElement("Sound Effect Volume level")]
        public int SoundEffectVolume = 80;
        [XmlElement("Music Volume level")]
        public int MusicVolume = 80;
        [XmlElement("Controls")]
        public List<ActionKey> controls = new List<ActionKey>();

        internal static int speedMod = 4;
        internal static int speedModCamera = 4;

        public SettingsFile() { }

        static internal SettingsFile generateFile()
        {
            SettingsFile f = new SettingsFile();
            f.bShadowGeneration = GameProcessor.bShadowsEnabled;
            f.bReflectionGeneration = GameProcessor.bWaterReflectionEnabled;
            f.shadowPercentage = (int)(GameProcessor.shadowQualityPercentage * 100);
            f.resolution = ResolutionUtility.WindowSizeBeforeFullScreen;
            f.battleSpeed = speedMod;
            f.battleCameraSpeed = speedModCamera;
            f.MasterVolume = SceneUtility.masterVolume;
            f.SoundEffectVolume = SceneUtility.soundEffectsVolume;
            f.MusicVolume = SceneUtility.musicVolume;
            f.controls = Game1.actionKeyList;
            return f;
        }

        static public String GenerateSettingsSavePath()
        {
            String saveFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Aeon's Dawn");
            return Path.Combine(saveFolder, "UserSettings.xml");
        }

        internal void Load()
        {
            GameProcessor.bShadowsEnabled = bShadowGeneration;
            GameProcessor.bWaterReflectionEnabled = bReflectionGeneration;
            GameProcessor.shadowQualityPercentage = (float)((float)shadowPercentage / 100f);

            if (resolution.X < 100 && resolution.Y < 100)
            {
                resolution = new Vector2(1366, 768);
            }
            ResolutionUtility.AdjustResolution(resolution.X, resolution.Y, Game1.graphics);
            if (bFullScreen)
            {
                ResolutionUtility.toggleFullscreen();
            }

            speedMod = battleSpeed;
            speedModCamera = battleCameraSpeed;

            if (Math.Abs(MasterVolume) > 100)
            {
                MasterVolume = Math.Abs(MasterVolume) % 100;
            }
            SceneUtility.masterVolume = MasterVolume;

            if (Math.Abs(SoundEffectVolume) > 100)
            {
                SoundEffectVolume = Math.Abs(SoundEffectVolume) % 100;
            }
            SceneUtility.soundEffectsVolume = SoundEffectVolume;

            if (Math.Abs(MusicVolume) > 100)
            {
                MusicVolume = Math.Abs(MusicVolume) % 100;
            }
            SceneUtility.musicVolume = MusicVolume;

            List<ActionKey> lak = new List<ActionKey>();
            if (controls.Count != 0)
            {
                foreach (var item in Game1.actionKeyList)
                {
                    if (controls.Find(ak => ak.actionIndentifierString.Equals(item.actionIndentifierString) && ak.column == item.column) != null)
                    {
                        lak.Add(controls.Find(ak => ak.actionIndentifierString.Equals(item.actionIndentifierString) && ak.column == item.column));
                    }
                    else
                    {
                        lak.Add(item);
                    }
                }
                Game1.actionKeyList = lak;
            }
            else
            {
                controls = Game1.actionKeyList;
            }

            foreach (var act in Game1.actionList)
            {
                for (int i = 0; i < act.whatKeysIsActionAssignedTo.Length; i++)
                {
                    var k = Game1.actionKeyList.Find(ak=>ak.actionIndentifierString.Equals(act.actionIndentifierString)&&ak.column==i);
                    act.whatKeysIsActionAssignedTo[i] = k;
                }
            }
        }
    }
}
