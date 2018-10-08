using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    [XmlRoot("Map Save Info")]
    public class MapSaveInfo
    {
        [XmlAnyElement("Pick Up Entities")]
        public List<PickUpEntity> mapPUEntities = new List<PickUpEntity>();
        [XmlElement("Is Parent map")]
        public bool bIsParentMap = false;
        [XmlElement("Map ID")]
        public int mapID = -1;

        [XmlElement]
        internal bool bInitialized = false;

        PickUpEntity PUEntityHandle = null;
        List<PickUpEntity> mapPUEntitiesInRange = new List<PickUpEntity>();

        public MapSaveInfo() { }

        internal MapSaveInfo(BasicMap bm)
        {
            bInitialized = true;

            if (GameProcessor.parentMap == bm)
            {
                bIsParentMap = true;
            }

            mapID = bm.identifier;
        }

        internal bool CanPickUp(BaseSprite bs)
        {
            mapPUEntitiesInRange.Clear();
            List<Rectangle> regions = GameProcessor.loadedMap.chunksToConsider.Select(c => c.region).ToList();

            var tempList = mapPUEntities.FindAll(e => e.isInRegion(regions));

            for (int i = 0; i < tempList.Count; i++)
            {
                var temp = tempList[i].CanPickUp(bs);

                if (temp) { mapPUEntitiesInRange.Add(tempList[i]); }
            }

            mapPUEntitiesInRange.RemoveAll(mue=>mue.itemList.Count == 0);
            mapPUEntities.RemoveAll(mue => mue.itemList.Count == 0);

            return mapPUEntitiesInRange.Count != 0 ? true : false;
        }

        internal void HandlePickUp()
        {
            if (mapPUEntitiesInRange.Count == 1)
            {
                PUEntityHandle = mapPUEntitiesInRange[0];
            }
            if (mapPUEntitiesInRange.Count > 1)
            {
                int index = mapPUEntitiesInRange.IndexOf(PUEntityHandle);
                if (++index >= mapPUEntitiesInRange.Count)
                {
                    index = 0;
                }
                PUEntityHandle = mapPUEntitiesInRange[index];
            }


            var temp = new PickUpScreen(new Rectangle(250, 220, (int)(178 * 1.7f), (int)(226 * 1.7f)), "Inventory " + PlayerSaveData.playerInventory.localInventory.Count.ToString() + @"/" + PlayerSaveData.playerInventory.localInventoryMaxSize.ToString(), PUEntityHandle);
            GameProcessor.popUpRenders.Add(temp);
            Utilities.Control.Player.NonCombatCtrl.changeToPickUpScreen(temp);
        }

        internal void EndPickUpEntity(PickUpEntity pue)
        {
            mapPUEntities.Remove(pue);
            mapPUEntitiesInRange.Clear();
        }
    }
}
