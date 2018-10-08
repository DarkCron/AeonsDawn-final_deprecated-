using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW
{
    [XmlRoot("Map Zone")]
    public class MapZone
    {
        [XmlElement("Parent Region of Zone")]
        public MapRegion parentRegion = new MapRegion();
        [XmlArrayItem("sizes")]
        public List<Rectangle> zoneSizes = new List<Rectangle>();
        [XmlElement("zone ID")]
        public int zoneID = 0;
        [XmlElement("Region name")]
        public String zoneName = "Default zone name";
        [XmlElement("Zone Encounter Info")]
        public ZoneEncounterInfo zoneEncounterInfo = new ZoneEncounterInfo();

        [XmlElement("Zone entrance script ID")]
        public int scriptIdentifier = -1;

        [XmlIgnore]
        public BaseScript entranceScript = new BaseScript();

        internal List<BasicTile> zoneTiles = new List<BasicTile>();

        public MapZone()
        {

        }

        public bool Contains(BaseSprite bs)
        {
            foreach (var item in zoneSizes)
            {
                if (item.Contains(bs.spriteGameSize) || item.Intersects(bs.spriteGameSize))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Contains(Vector2 v)
        {
            foreach (var item in zoneSizes)
            {
                if (item.Contains(v))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reload(GameContentDataBase gcdb, List<EnemyAIInfo> enemyPool)
        {
            zoneEncounterInfo.Reload(gcdb);

            foreach (var item in zoneEncounterInfo.enemyInfoIDs)
            {
                zoneEncounterInfo.enemies.Add(enemyPool.Find(i=>i.infoID==item));
            }

            if (scriptIdentifier!=-1)
            {
                entranceScript = gcdb.gameScripts.Find(s=>s.identifier == scriptIdentifier);
            }
        }

        public Rectangle returnBoundingZone()
        {
            int minX = zoneSizes[0].X;
            int maxX = zoneSizes[0].X + zoneSizes[0].Width;
            foreach (var item in zoneSizes)
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

            int minY = zoneSizes[0].Y;
            int maxY = zoneSizes[0].Y + zoneSizes[0].Height;
            foreach (var item in zoneSizes)
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

        public KeyValuePair<Rectangle, float> returnOptimalCameraAndAngle()
        {
            int stdCameraWidth = 1366;
            int stdCameraHeight = 768;
            Rectangle zoneSize = returnBoundingZone();
            float scaleX = (float)zoneSize.Width / (float)stdCameraWidth;
            float scaleY = (float)zoneSize.Height / (float)stdCameraHeight;
            if(zoneSize.Width<stdCameraWidth) {
                scaleX = (float)stdCameraWidth / (float)zoneSize.Width;
            }

            if (zoneSize.Height < stdCameraHeight)
            {
                scaleY = (float)stdCameraHeight / (float)zoneSize.Height;
            }

            if (scaleX<scaleY) {
                scaleY = scaleX;
            }
            else {
                scaleX = scaleY;
            }

            float cameraZoom = scaleY;
            int cameraWidth;
            int cameraHeight;
            int cameraXPos;
            int cameraYPos;
            if (cameraZoom<1) {
                cameraWidth = (int)(stdCameraWidth * cameraZoom);
                cameraHeight = (int)(stdCameraHeight * cameraZoom);
                cameraXPos = (zoneSize.Width - cameraWidth) / 2 + zoneSize.X;
                cameraYPos = (zoneSize.Height - cameraHeight) / 2 + zoneSize.Y;
            }
            else {
                cameraWidth = (int)(stdCameraWidth / cameraZoom);
                cameraHeight = (int)(stdCameraHeight / cameraZoom);
                cameraXPos = (zoneSize.Width - cameraWidth) / 2 + zoneSize.X;
                cameraYPos = (zoneSize.Height - cameraHeight) / 2 + zoneSize.Y;
            }
          

            Rectangle camera = new Rectangle(-cameraXPos,-cameraYPos,cameraWidth,cameraHeight);


            return new KeyValuePair<Rectangle, float>(camera,cameraZoom);
        }

        public override string ToString()
        {
            return zoneName + " ,ID: " + zoneID;
        }

        public bool HasGoodEntranceScript()
        {
            if (entranceScript!=default(BaseScript))
            {
                return true;
            }

            return false;
        }
    }
}
