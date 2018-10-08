using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW.Utilities.Sprite
{
    [XmlRoot("Sprite Info")]
    public class BaseSpriteInfoHolder
    {
        public enum SpriteType { Object, objectGroup, SpriteLight, Character }

        [XmlElement("BaseSprite ID")]
        public int objID = 0;
        [XmlElement("BaseSprite Map name")]
        public String objMapName = "";
        [XmlElement("script ID")]
        public int scriptID = 0;
        [XmlElement("Map ID")]
        public int mapID = 0;
        [XmlElement("Position")]
        public Vector2 position = Vector2.Zero;
        [XmlElement("scale")]
        public Vector2 scaleVector = new Vector2(1);
        [XmlElement("Sprite Type")]
        public SpriteType spriteType = SpriteType.Object;
        [XmlElement("ground tile type")]
        public TileSource.TileType tileType = TileSource.TileType.Ground;
        [XmlElement("visible")]
        public bool bIsVisible = true;
        [XmlElement("activate on touch")]
        public bool bActivateOnTouch = true;
        [XmlElement("collision")]
        public bool bHasCollision = false;
        [XmlArrayItem("relative positions")]
        public List<Vector2> relativeOffSet = new List<Vector2>();
        [XmlArrayItem("Items in group IDs")]
        public List<int> groupItemsIDs = new List<int>();
        [XmlArrayItem("Ground tile")]
        public List<TileSource.TileType> gTileType = new List<TileSource.TileType>();
        [XmlElement("items visible")]
        public List<bool> bIsVisibleGroup = new List<bool>();
        [XmlElement("items activate on touch")]
        public List<bool> bActivateOnTouchGroup = new List<bool>();
        [XmlElement("items collision")]
        public List<bool> bHasCollisionGroup = new List<bool>();
        [XmlElement("Water reflection")]
        public bool bNeedsWaterReflection = false;
        [XmlElement("Rotation")]
        public int rotation = 0;

        [XmlIgnore]
        public object spriteObject;

        public BaseSpriteInfoHolder() { }

        public BaseSpriteInfoHolder(object obj)
        {
            if (obj is BaseSprite)
            {
                bNeedsWaterReflection = (obj as BaseSprite).bNeedsWaterReflection;
            }else if (obj is ObjectGroup)
            {
                bNeedsWaterReflection = (obj as ObjectGroup).bNeedsWaterReflection;
            }


            if (obj.GetType() == typeof(BaseSprite))
            {
                var bs = obj as BaseSprite;
                spriteObject = bs;
                objID = bs.shapeID;
                position = bs.position;
                spriteType = SpriteType.Object;
                tileType = bs.groundTileType;
                mapID = bs.objectIDAddedOnMap;
                objMapName = bs.shapeMapName;
                scriptID = bs.script == null ? -1 : bs.script.identifier;
                bs.scriptID = scriptID;
                bIsVisible = bs.bIsVisible;
                bActivateOnTouch = bs.bActivateOnTouch;
                scaleVector = bs.scaleVector;
                bHasCollision = bs.bHasCollision;
                gTileType = new List<TileSource.TileType> { bs.groundTileType };
                rotation = bs.rotationIndex;
               
            }
            else if (obj.GetType() == typeof(ObjectGroup))
            {
                var og = obj as ObjectGroup;
                spriteObject = og;
                objID = og.groupID;
                position = og.mainController().position;
                tileType = og.mainController().groundTileType;
                groupItemsIDs = og.groupItemsIDs;
                relativeOffSet = og.relativeOffSet;
                spriteType = SpriteType.objectGroup;
                scaleVector = og.scaleVector;
                bIsVisible = og.bIsVisible;
                bHasCollision = og.bHasCollision;
                objMapName = og.groupMapName;
                gTileType = og.groupItems.Select(gi => gi.groundTileType).ToList();
                bIsVisibleGroup = og.groupItems.Select(gi => gi.bIsVisible).ToList();
                bActivateOnTouchGroup = og.groupItems.Select(gi => gi.bActivateOnTouch).ToList();
                bHasCollisionGroup = og.groupItems.Select(gi => gi.bHasCollision).ToList();
            }
            else if (obj.GetType() == typeof(SpriteLight))
            {
                var sl = obj as SpriteLight;
                objID = sl.lightID;
                position = sl.position;
                mapID = sl.objectIDAddedOnMap;
                spriteType = SpriteType.SpriteLight;
                tileType = sl.groundTileType;
                scaleVector = sl.scaleVector;
                bIsVisible = sl.bIsVisible;
                bActivateOnTouch = sl.bActivateOnTouch;
                scriptID = sl.scriptID;
                objMapName = sl.shapeMapName;
                gTileType = new List<TileSource.TileType> { sl.groundTileType };
                rotation = sl.rotationIndex;
            }
            else if (obj.GetType() == typeof(BaseCharacter))
            {
                var bc = obj as BaseCharacter;
                objID = bc.shapeID;
                position = bc.position;
                spriteType = SpriteType.Character;
                tileType = bc.groundTileType;
                scaleVector = bc.scaleVector;
                bIsVisible = bc.bIsVisible;
                bHasCollision = bc.bHasCollision;
                bActivateOnTouch = bc.bActivateOnTouch;
                scriptID = bc.scriptID;
                objMapName = bc.shapeMapName;
                mapID = bc.objectIDAddedOnMap;
                gTileType = new List<TileSource.TileType> { bc.groundTileType };
                rotation = bc.rotationIndex;
            }

        }

        public object Reload(GameContentDataBase gcdb)
        {
            switch (spriteType)
            {
                case SpriteType.Object:
                    spriteObject = gcdb.gameObjectObjects.Find(so => so.shapeID == objID).ShallowCopy();

                    var baseSprite = spriteObject as BaseSprite;
                    baseSprite.position = position;
                    baseSprite.UpdatePositioNonMovement();
                    baseSprite.groundTileType = tileType;
                    baseSprite.objectIDAddedOnMap = mapID;
                    baseSprite.shapeMapName = objMapName;
                    baseSprite.scriptID = scriptID;
                    baseSprite.bIsVisible = bIsVisible;
                    baseSprite.bActivateOnTouch = bActivateOnTouch;
                    baseSprite.scaleVector = scaleVector;
                    baseSprite.AssignScaleVector(scaleVector);
                    baseSprite.groundTileType = gTileType[0];
                    baseSprite.bHasCollision = bHasCollision;
                    baseSprite.bNeedsWaterReflection = bNeedsWaterReflection;
                    baseSprite.rotationIndex = rotation;

                    baseSprite.script = gcdb.gameScripts.Find(so => so.identifier == scriptID) == null ? new SriptProcessing.BaseScript() : gcdb.gameScripts.Find(so => so.identifier == scriptID);
                    return baseSprite;
                case SpriteType.objectGroup:
                    spriteObject = gcdb.gameObjectGroups.Find(so => so.groupID == objID).ShallowCopy();
                    var og = spriteObject as ObjectGroup;
                    og.relativeOffSet = relativeOffSet;
                    og.groupItemsIDs = groupItemsIDs;
                    og.Reload(gcdb);
                    og.PlaceGroupFromReload(position);
                    og.groupItems.ForEach(o => o.groundTileType = tileType);
                    og.mainController().groundTileType = tileType;
                    og.bIsVisible = bIsVisible;
                    og.scaleVector = scaleVector;
                    og.AssignScaleVector(scaleVector);
                    og.groupMapName = objMapName;
                    int OGindex = 0;
                    foreach (var item in og.groupItems)
                    {
                        item.groundTileType = gTileType[OGindex];
                        item.bHasCollision = bHasCollisionGroup[OGindex];
                        item.bIsVisible = bIsVisibleGroup[OGindex];
                        item.bActivateOnTouch = bActivateOnTouchGroup[OGindex];

                        OGindex++;
                    }
                    og.bHasCollision = bHasCollision;
                    og.bNeedsWaterReflection = bNeedsWaterReflection;

                    return og;
                case SpriteType.SpriteLight:
                    spriteObject = gcdb.gameLights.Find(so => so.lightID == objID).Clone();
                    var sl = spriteObject as SpriteLight;
                    sl.position = position;
                    sl.UpdatePositioNonMovement();
                    sl.groundTileType = tileType;
                    sl.shapeMapName = objMapName;
                    sl.scriptID = scriptID;
                    sl.bIsVisible = bIsVisible;
                    sl.bActivateOnTouch = bActivateOnTouch;
                    sl.scaleVector = scaleVector;
                    sl.AssignScaleVector(scaleVector);
                    sl.groundTileType = gTileType[0];
                    sl.bHasCollision = bHasCollision;
                    sl.script = gcdb.gameScripts.Find(so => so.identifier == scriptID) == null ? new SriptProcessing.BaseScript() : gcdb.gameScripts.Find(so => so.identifier == scriptID);
                    sl.bNeedsWaterReflection = bNeedsWaterReflection;
                    sl.rotationIndex = rotation;
                    return sl;
                case SpriteType.Character:
                    spriteObject = gcdb.gameCharacters.Find(so => so.shapeID == objID).Clone();

                    var baseCharacter = spriteObject as BaseCharacter;
                    baseCharacter.position = position;
                    baseCharacter.UpdatePositioNonMovement();
                    baseCharacter.groundTileType = tileType;
                    baseCharacter.objectIDAddedOnMap = mapID;
                    baseCharacter.shapeMapName = objMapName;
                    baseCharacter.scriptID = scriptID;
                    baseCharacter.bIsVisible = bIsVisible;
                    baseCharacter.bActivateOnTouch = bActivateOnTouch;
                    baseCharacter.scaleVector = scaleVector;
                    baseCharacter.AssignScaleVector(scaleVector);
                    baseCharacter.groundTileType = gTileType[0];
                    baseCharacter.bHasCollision = bHasCollision;
                    baseCharacter.bNeedsWaterReflection = bNeedsWaterReflection;
                    baseCharacter.rotationIndex = rotation;

                    return baseCharacter;
            }

            return null;
        }
    }
}
