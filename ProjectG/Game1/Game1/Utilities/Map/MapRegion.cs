using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sound.BG;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    [XmlRoot("Map Region")]
    public class MapRegion
    {
        [XmlArrayItem("Contained zones")]
        public List<MapZone> regionZones = new List<MapZone>();
        [XmlArrayItem("sizes")]
        public List<Rectangle> regionSizes = new List<Rectangle>();
        [XmlElement("Region ID")]
        public int regionID = 0;
        [XmlElement("Region name")]
        public String regionName = "Default region name";
        [XmlArrayItem("Enemy character pool")]
        public List<EnemyAIInfo> enemyPool = new List<EnemyAIInfo>();
        [XmlElement("Region Level zones")]
        public List<int> regionLevels = new List<int>();
        [XmlElement("Region enemy info highest ID")]
        public int highestEnemyID = 0;
        [XmlElement("Region BG info")]
        public RegionCombatSong regionBGinfo = new RegionCombatSong();

        [XmlIgnore]
        public int currentRegionLevel = 0;

        public MapRegion()
        {

        }

        public void Reload(GameContentDataBase gcdb)
        {
            //enemyPool.Clear();
            foreach (var item in enemyPool)
            {
                item.Reload(gcdb);
            }


            foreach (var item in regionZones)
            {
                item.Reload(gcdb, enemyPool);
            }


        }

        public bool Contains(BaseSprite bs)
        {
            foreach (var item in regionSizes)
            {
                if (item.Contains(bs.trueMapSize()) || item.Intersects(bs.trueMapSize()))
                {
                    return true;
                }
            }
            return false;
        }

        public MapZone getZone(BaseSprite bs)
        {
            return regionZones.Find(z => z.Contains(bs));
        }

        public void initialzeRegionLogic(int averageTeamLevel)
        {
            currentRegionLevel = regionLevels.IndexOf(regionLevels.Last(rl => averageTeamLevel >= rl));
        }

        public int getCurrentRegionLevel()
        {
            return currentRegionLevel;
        }

        public override string ToString()
        {
            return regionName + " ,ID: " + regionID;
        }

        public Rectangle returnBoundingZone()
        {
            int minX = regionSizes[0].X;
            int maxX = regionSizes[0].X + regionSizes[0].Width;
            foreach (var item in regionSizes)
            {
                int min = item.X;
                int max = item.X + item.Width;

                if (min < minX)
                {
                    minX = min;
                }

                if (max > maxX)
                {
                    maxX = max;
                }
            }

            int minY = regionSizes[0].Y;
            int maxY = regionSizes[0].Y + regionSizes[0].Height;
            foreach (var item in regionSizes)
            {
                int min = item.Y;
                int max = item.Y + item.Height;

                if (min < minY)
                {
                    minY = min;
                }

                if (max > maxY)
                {
                    maxY = max;
                }
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public void GetRegionReady()
        {
            regionBGinfo.getSoundReady();
            if (!CombatProcessor.bStartPhase1)
            {
                regionBGinfo.StartNonCombat();
            }
        }

        internal void EnterCombat()
        {
            regionBGinfo.StartCombat();
        }

        internal void LeaveCombat()
        {
            regionBGinfo.StartNonCombat();
        }
    }

    public class RegionCombatSong
    {
        [XmlElement("Song loc l1")]
        public String SongLocL1 = "";
        [XmlElement("Song loc l2")]
        public String SongLocL2 = "";
        [XmlElement("Song non Combat")]
        public String SongNonCombat = "";

        internal SoundEffect sL1 = null;
        internal SoundEffect sL2 = null;
        internal SoundEffect nonCombatSong = null;
        internal SoundEffectSong nonCombatSES = null;
        internal SoundEffectSong currentLayer = null;
        internal LayeredSong regionLayerCombatSong = null;

        internal static Song lastNonCombatSong = null;

        public RegionCombatSong() { }

        public void getSoundReady()
        {
            if (sL1 == null && !SongLocL1.Equals(""))
            {
                try
                {
                    sL1 = Game1.contentManager.Load<SoundEffect>(SongLocL1);
                    //EditorFileWriter.SongToFileTest(SongLocL1);
                }
                catch (Exception e)
                {

                }

                if (!SongLocL2.Equals(""))
                {

                    try
                    {
                        sL2 = Game1.contentManager.Load<SoundEffect>(SongLocL2);
                    }
                    catch (Exception e)
                    {

                    }


                }
            }

            if (!SongNonCombat.Equals("") && nonCombatSong == null)
            {
                try
                {
                    nonCombatSong = Game1.contentManager.Load<SoundEffect>(SongNonCombat);
                    nonCombatSES = new SoundEffectSong(nonCombatSong,true,false);
                }
                catch (Exception e)
                {

                }
            }

            if (sL1 != null && sL2 != null)
            {
                regionLayerCombatSong = new LayeredSong(new SoundEffectSong(sL1, true), new SoundEffectSong(sL2, true));
            }
            else if (sL1 != null)
            {
                regionLayerCombatSong = new LayeredSong(new SoundEffectSong(sL1, true));
            }
        }

        internal void SwitchLayer()
        {
            if (currentLayer == null)
            {
                currentLayer = SoundEffectSong.soundEffectSongs[0];
            }

            if (sL2 != null && currentLayer.parentSE == sL1 && (BattleGUI.bIsRunning||GameProcessor.bStartCombatZoom))
            {

                //regionLayerCombatSong.SwitchLayer(1,100,500);

                currentLayer = regionLayerCombatSong.SwitchLayer(1, 100, 2500);
            }
            else if (currentLayer.parentSE == sL2 && sL1 != null && sL1 != currentLayer.parentSE)
            {
                regionLayerCombatSong.SwitchLayer(1, 0, 2500);
                currentLayer = SoundEffectSong.soundEffectSongs[0];
            }


        }

        public void StartCombat()
        {
            if (sL1 != null && !SoundEffectSong.IsPlaying(currentLayer))
            {
                SoundEffectSong.ClearSongs();
                currentLayer = regionLayerCombatSong.StartPlay();

            }
        }

        public void StartNonCombat()
        {
            if (nonCombatSES!=null)
            {
                SoundEffectSong.ClearSongs();
                SoundEffectSong.Start(nonCombatSES);
            }
            //if (nonCombatSong != null && lastNonCombatSong != nonCombatSong)
            //{
            //    MediaPlayer.Play(nonCombatSong);
            //    lastNonCombatSong = nonCombatSong;
            //    MediaPlayer.IsRepeating = true;
            //}
        }
    }

}
