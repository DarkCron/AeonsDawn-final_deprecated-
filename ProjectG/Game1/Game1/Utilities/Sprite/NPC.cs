using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.AI;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW
{
    [XmlRoot("NPC")]
    public class NPC
    {
        [XmlElement("BaseCharacter source")]
        public int baseCharacterSourceID = -1;
        [XmlElement("NPC wanders area or patrols path")]
        public bool NPCWanders = false;
        [XmlElement("NPC move path than end")]
        public bool NPCMovesAndEnds = false;
        [XmlElement("NPC Area")]
        public List<BasicTile> NPCArea = new List<BasicTile>();
        [XmlElement("Minimum Step timer")]
        public int minStepTimer = 1000;
        [XmlElement("Maximum Step timer")]
        public int maxStepTimer = 4000;
        [XmlElement("Object map ID")]
        public int objectIDAddedOnMap = 0;
        [XmlElement("Identifier")]
        public int identifier = 0;
        [XmlElement("NPC ignores collision with mc")]
        public bool ignoresMc = false;
        [XmlElement("NPC time commands")]
        public List<NPCMoveToCommand> npcCommands = new List<NPCMoveToCommand>();
        [XmlElement("Map Placed")]
        public int mapPlacedID = 0;
        [XmlElement("Map Active")]
        public int mapActiveID = 0;
        [XmlElement("Sprite info holder")]
        public BaseSpriteInfoHolder infoHolder = new BaseSpriteInfoHolder();
        [XmlElement("NPC name")]
        public String npcName = "";
        [XmlElement("NPC description")]
        public String npcDescription = "";
        [XmlElement("NPC display name")]
        public String npcDisplayName = "";

        [XmlIgnore]
        public NPCMoveToCommand currentTimeCommand = new NPCMoveToCommand();
        [XmlIgnore]
        public int rotationAtEnd = 0;
        [XmlIgnore]
        int randomStep = 2000;
        [XmlIgnore]
        int stepTimePassed = 0;
        [XmlIgnore]
        public BaseCharacter baseCharacter;
        [XmlIgnore]
        public bool characterIsFinishedMoving = false;
        internal bool bReachedDestinationAtLeastOnce = false;
        internal objectInfo objInfo;
        bool bResetCycle = false;

        public static NPC ConvertToNPC(BaseCharacter bc, BasicMap bm)
        {
            bm.mapSprites.Remove(bc);
            NPC temp = new NPC();
            temp.baseCharacter = bc;
            temp.mapPlacedID = bm.identifier;
            temp.mapActiveID = bm.identifier;
            bc.parentObject = temp;
            temp.objectIDAddedOnMap = bc.objectIDAddedOnMap;
            bc.fromMap = bm;
            bc.currentMapToDisplayOn = bm;
            bc.scriptID = -1;
            bc.script = null;
            bm.mapNPCs.Add(temp);
            bm.ForceCheckChunksToConsider(); //Should get rid of any old bc references, to prevent doubles
            MapObjectHelpClass.objectFromOutsideOnThisMap.Add(temp);
            MapObjectHelpClass.objectsToUpdateOutsideOfMap.Add(temp);
            return temp;
        }

        public void GetNPCLogicReady(BasicMap bm)
        {

            npcCommands.ForEach(c => c.parentObject = this);
            mapPlacedID = bm.identifier;
            //baseCharacter.fromMap = bm;
            //baseCharacter.currentMapToDisplayOn = bm;
        }

        public void DataBaseReload(GameContentDataBase gcdb)
        {
            try
            {
                //baseCharacter = gcdb.gameCharacters.Find(gc => gc.shapeID == baseCharacterSourceID).Clone();
                baseCharacter = infoHolder.Reload(gcdb) as BaseCharacter;
                baseCharacter.fromMap = BasicMap.allMapsGame().Find(map => map.identifier == mapPlacedID);
                baseCharacter.currentMapToDisplayOn = BasicMap.allMapsGame().Find(map => map.identifier == mapActiveID);
                if (baseCharacter.spriteGameSize.Size == new Point(0))
                {
                    baseCharacter.spriteGameSize.Size = new Point(64);
                }

                foreach (var item in npcCommands)
                {
                    item.script = gcdb.gameScripts.Find(s => s.identifier == item.scriptIdentifier);
                }
                //  baseCharacter.changePosition(NPCArea[0].positionGrid * 64);
            }
            finally
            {
            }


        }

        public void Update(GameTime gt)
        {
            if (baseCharacter.currentMapToDisplayOn == null)
            {
                if (mapPlacedID == 0)
                {
                    baseCharacter.fromMap = GameProcessor.parentMap;
                }
                else
                {
                    baseCharacter.fromMap = GameProcessor.parentMap.subMaps.Find(m => m.identifier == mapPlacedID);
                }
                baseCharacter.currentMapToDisplayOn = baseCharacter.fromMap;
            }

            if (baseCharacter.parentObject == null)
            {
                baseCharacter.parentObject = this;
            }

            if (currentTimeCommand == default(NPCMoveToCommand) && npcCommands.Count == 0)
            {
                GenerateDefaultTimeCommand();
            }

            baseCharacter.Update(gt);
            if (!baseCharacter.smh.bIsBusy)
            {
                currentTimeCommand.bCompletedCommand = true;
            }

        }

        public void MinimalOutsideUpdate(GameTime gt)
        {
            if (baseCharacter.currentMapToDisplayOn == null)
            {
                if (mapPlacedID == 0)
                {
                    baseCharacter.fromMap = GameProcessor.parentMap;
                }
                else
                {
                    baseCharacter.fromMap = GameProcessor.parentMap.subMaps.Find(m => m.identifier == mapPlacedID);
                }
                baseCharacter.currentMapToDisplayOn = baseCharacter.fromMap;
            }

            if (baseCharacter.parentObject == null)
            {
                baseCharacter.parentObject = this;
            }

            if (currentTimeCommand == default(NPCMoveToCommand) && npcCommands.Count == 0)
            {
                GenerateDefaultTimeCommand();
            }

            baseCharacter.UpdateMinimalUpdateOutsideMap(gt);
            if (!baseCharacter.smh.bIsBusy)
            {
                currentTimeCommand.bCompletedCommand = true;
            }

        }

        public void Draw(SpriteBatch sb)
        {
            baseCharacter.Draw(sb);
        }

        private void PatrolLogic()
        {
            // moveHandler.Start(currentTimeCommand.NPCArea[currentTimeCommand.areaIndex], this);
            currentTimeCommand.areaIndex++;
            if (currentTimeCommand.areaIndex == currentTimeCommand.NPCArea.Count)
            {
                currentTimeCommand.areaIndex = 0;
            }

            if (currentTimeCommand.areaIndex == currentTimeCommand.NPCArea.Count - 1)
            {
                if (currentTimeCommand.NPCArea[0].distanceCoordsFrom(currentTimeCommand.NPCArea[currentTimeCommand.areaIndex].positionGrid * 64) != 1)
                {
                    currentTimeCommand.areaIndex = 0;
                    currentTimeCommand.NPCArea.Reverse();
                }
            }
        }

        private void WanderLogic()
        {
            List<BasicTile> possibleMovement = new List<BasicTile>();
            possibleMovement.AddRange(currentTimeCommand.NPCArea.FindAll(t => t.distanceCoordsFrom(currentTimeCommand.NPCArea[currentTimeCommand.areaIndex].positionGrid * 64) == 1));
            currentTimeCommand.areaIndex = currentTimeCommand.NPCArea.IndexOf(possibleMovement[GamePlayUtility.Randomize(0, possibleMovement.Count)]);
            //moveHandler.Start(currentTimeCommand.NPCArea[currentTimeCommand.areaIndex], this);
        }

        public void Move(Vector2 v, int direction)
        {
            baseCharacter.changePositionRelative(v);
            baseCharacter.rotationIndex = direction;
        }

        void GenerateDefaultTimeCommand()
        {
            currentTimeCommand.maxStepTimer = maxStepTimer;
            currentTimeCommand.minStepTimer = minStepTimer;
            currentTimeCommand.NPCArea = NPCArea;
            currentTimeCommand.NPCWanders = NPCWanders;
            currentTimeCommand.NPCMovesAndEnds = NPCMovesAndEnds;
        }

        public bool MustChangeCommand(int tick)
        {

            if (!currentTimeCommand.bCompletedCommand)
            {
                return false;
            }
            if (npcCommands.Count < 2)
            {
                return false;
            }
            var futureAvailableCommands = npcCommands.FindAll(command => command.CanBeActivated(currentTimeCommand, this));
            NPCMoveToCommand temp = null;
            if (futureAvailableCommands.Count != 0)
            {
                if (futureAvailableCommands[0].IsWithinTimeBlock(tick))
                {
                    temp = futureAvailableCommands[0];
                }
                else
                {
                    int timeFinal = futureAvailableCommands[0].tickStop;
                    var bestCommandInstead = futureAvailableCommands.FindLast(c => c.tickStart < tick && c.tickStop > tick);

                    temp = bestCommandInstead;
                }
            }
            // var temp = npcCommands.Find(command => command.CanBeActivated(currentTimeCommand, this) && command.IsWithinTimeBlock(tick));
            if (currentTimeCommand != temp && temp != null)
            {

                return true;
            }
            else
            {
                if (temp == null && bResetCycle)
                {
                    bResetCycle = false;
                    return true;
                }
                return false;
            }
        }

        public void ChangeCommand(int tick)
        {
            currentTimeCommand.EndOfCommand();
            var tempList = npcCommands.FindAll(command => command.CanBeActivated(currentTimeCommand, this) && command.IsWithinTimeBlock(tick));
            if (tempList.Count == 0)
            {
                tempList = npcCommands.FindAll(command => command.CanBeActivatedResetTest(currentTimeCommand, this) && command.IsWithinTimeBlock(tick));
            }
            currentTimeCommand = tempList[0];
            //currentTimeCommand = npcCommands[4];
            currentTimeCommand.areaIndex = 0;
            baseCharacter.speed = currentTimeCommand.speed;
            baseCharacter.position = ((baseCharacter.position / 64).ToPoint().ToVector2() * 64);
            if (currentTimeCommand.GetType() == typeof(NPCMoveToCommand))
            {
                baseCharacter.StartSpriteMoveHandler(currentTimeCommand.destination, currentTimeCommand.mapID, currentTimeCommand);
                bResetCycle = true;
            }

            bReachedDestinationAtLeastOnce = false;
            currentTimeCommand.bCompletedCommand = false;
            currentTimeCommand.parentObject = this;
            currentTimeCommand.Execute();
        skip: { }
        }

        public void ChangeCommandEdit(int index)
        {
            currentTimeCommand.EndOfCommand();
            currentTimeCommand = npcCommands[index];
            currentTimeCommand.areaIndex = 0;
            baseCharacter.position = ((baseCharacter.position / 64).ToPoint().ToVector2() * 64);
            baseCharacter.speed = npcCommands[index].speed;
            baseCharacter.StartSpriteMoveHandler(currentTimeCommand.destination, currentTimeCommand.mapID, currentTimeCommand);

            bReachedDestinationAtLeastOnce = false;
            currentTimeCommand.bCompletedCommand = false;
            currentTimeCommand.parentObject = this;
            currentTimeCommand.Execute();
        }

        public bool Contains(Rectangle r)
        {
            if (baseCharacter.Contains(r))
            {
                return true;
            }
            return false;
        }

        public bool Contains(Vector2 r)
        {
            if (baseCharacter.Contains(r))
            {
                return true;
            }
            return false;
        }

        internal bool IsOnCurrentMap(BasicMap bm)
        {
            if (baseCharacter.currentMapToDisplayOn == null)
            {
                if (mapPlacedID == 0)
                {
                    baseCharacter.fromMap = GameProcessor.parentMap;
                }
                else
                {
                    baseCharacter.fromMap = GameProcessor.parentMap.subMaps.Find(m => m.identifier == mapPlacedID);
                }
                baseCharacter.currentMapToDisplayOn = baseCharacter.fromMap;
            }

            if (baseCharacter.currentMapToDisplayOn == bm)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool ContainsForEditorSelection(Vector2 mouse)
        {
            if (baseCharacter.ContainsForEditorSelection(mouse))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            String text = "NPC: ";
            text += npcName;
            text += " aka " + npcDisplayName + ", with " + baseCharacter.ToString();
            return text;
        }

        internal void MinimalUpdate(GameTime gt)
        {
            if (baseCharacter.currentMapToDisplayOn == null)
            {
                if (mapPlacedID == 0)
                {
                    baseCharacter.fromMap = GameProcessor.parentMap;
                }
                else
                {
                    baseCharacter.fromMap = GameProcessor.parentMap.subMaps.Find(m => m.identifier == mapPlacedID);
                }
                baseCharacter.currentMapToDisplayOn = baseCharacter.fromMap;
            }

            if (baseCharacter.parentObject == null)
            {
                baseCharacter.parentObject = this;
            }

            if (currentTimeCommand == default(NPCMoveToCommand) && npcCommands.Count == 0)
            {
                GenerateDefaultTimeCommand();
            }

            baseCharacter.MinimalUpdate(gt);
        }

        internal BaseScript GetScript()
        {
            if (npcCommands.Count == 1)
            {
                if (npcCommands[0].tickStart == -1)
                {
                    return npcCommands[0].script;
                }
            }
            if (currentTimeCommand != null)
            {
                return currentTimeCommand.script;
            }
            return null;
        }
    }

    [XmlRoot("NPC Command")]
    [XmlInclude(typeof(NPCChangeMap))]
    public class NPCMoveToCommand
    {
        [XmlElement("Command Name")]
        public String commandName = "Command";
        [XmlElement("Command Description")]
        public String commandDescription = "Command Description";
        [XmlElement("Requirement to be activated")]
        public ScriptBoolCheck sbc = new ScriptBoolCheck();
        [XmlElement("Command starts at")]
        public int tickStart = -1;
        [XmlElement("Command stops at")]
        public int tickStop = -1;
        [XmlElement("NPC wanders area or patrols path")]
        public bool NPCWanders = false;
        [XmlElement("NPC move path than end")]
        public bool NPCMovesAndEnds = false;
        [XmlElement("NPC Area")]
        public List<BasicTile> NPCArea = new List<BasicTile>();
        [XmlElement("Minimum Step timer")]
        public int minStepTimer = 1000;
        [XmlElement("Maximum Step timer")]
        public int maxStepTimer = 4000;
        [XmlElement("NPC destination")]
        public Vector2 destination = new Vector2(64);
        [XmlElement("NPC WayPoints")]
        public List<BasicTile> wayPointsToDestination = new List<BasicTile>();
        [XmlElement("Path to destination Exists")]
        public bool bPathExists = false;
        [XmlElement("NPC Map to move in index")]
        public int mapID = 0;
        [XmlElement("NPC speed")]
        public float speed = 1.5f;
        [XmlElement("Script ID")]
        public int scriptIdentifier = -1;

        [XmlIgnore]
        public BaseScript script = null;
        [XmlIgnore]
        public bool bCompletedCommand = false;
        [XmlIgnore]
        public int areaIndex = 0;
        internal NPC parentObject;
        internal List<AI.Node> nodePath = null;
        internal bool bCheckJustOneMoreTime = false;

        public NPCMoveToCommand() { }

        public bool CanBeActivatedResetTest(NPCMoveToCommand currentCommand, NPC npc)
        {
            if (!sbc.CanBeActivated())
            {
                return false;
            }

            if (currentCommand == this)
            {
                return false;
            }

            if (this.GetType() != typeof(NPCChangeMap))
            {
                if (parentObject == null)
                {
                    parentObject = npc;
                }
                if (parentObject.baseCharacter.currentMapToDisplayOn.identifier != mapID)
                {
                    return false;
                }
            }
            else if (this.GetType() == typeof(NPCChangeMap))
            {
                if (parentObject == null)
                {
                    parentObject = npc;
                }

                if (parentObject.baseCharacter.position != currentCommand.destination)
                {
                    return false;
                }
            }

            return true;

            if (wayPointsToDestination.Count != 0 && wayPointsToDestination[0].mapPosition.Contains(destination))
            {
                return true;
            }
            else
            {
                //   parentObject.baseCharacter.smh.Start(parentObject.baseCharacter,);
            }

            return false;
        }

        public bool CanBeActivated(NPCMoveToCommand currentCommand, NPC npc)
        {
            if (npc.npcCommands.IndexOf(this) <= npc.npcCommands.IndexOf(currentCommand))
            {
                return false;
            }

            if (!sbc.CanBeActivated())
            {
                return false;
            }

            if (currentCommand == this)
            {
                return false;
            }

            if (parentObject == null)
            {
                parentObject = npc;
            }

            if (this.GetType() == typeof(NPCMoveToCommand))
            {
                if (parentObject.baseCharacter.currentMapToDisplayOn.identifier != mapID)
                {
                    return false;
                }
            }

            return true;

            if (wayPointsToDestination.Count != 0 && wayPointsToDestination[0].mapPosition.Contains(destination))
            {
                return true;
            }
            else
            {
                //   parentObject.baseCharacter.smh.Start(parentObject.baseCharacter,);
            }

            return false;
        }

        public bool IsWithinTimeBlock(int timeIndex)
        {
            if (tickStart == -1)
            {
                return true;
            }

            if (timeIndex >= tickStart && timeIndex < tickStop)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static public bool TryCalculatePath(NPC npc, NPCMoveToCommand NPCtc, BasicMap parentMap, BasicTile start, BasicTile end)
        {
            BasicMap mapToCheckIn;
            if (NPCtc.mapID == 0)
            {
                mapToCheckIn = parentMap;
            }
            else
            {
                mapToCheckIn = parentMap.subMaps.Find(m => m.identifier == NPCtc.mapID);
            }

            var chunks = MapChunk.returnChunkRadius(mapToCheckIn, start.mapPosition.Location.ToVector2(), 2);

            if (chunks.Count != 0)
            {
                List<BasicTile> tiles = new List<BasicTile>();
                foreach (var chunk in chunks)
                {
                    tiles.AddRange(mapToCheckIn.possibleTilesWithController(chunk.region, mapToCheckIn));
                }
                tiles = tiles.Distinct().ToList();
                var test = PathFinder.NewPathSearch(start, end, tiles);
                if (test != null && test.Count != 0)
                {
                    return true;
                }
            }


            return false;
        }

        public virtual void Execute()
        {
        }

        public virtual void EndOfCommand()
        {

        }

        public override string ToString()
        {
            if (parentObject == null)
            {
                return base.ToString();
            }

            String text = "";
            text += "NPC command '" + parentObject.npcCommands.IndexOf(this) + "'' from " + DayLightHandler.indexToTimeString(tickStart) + " to " + DayLightHandler.indexToTimeString(tickStop);
            return text;
        }
    }

    [XmlRoot("NPC change map command")]
    public class NPCChangeMap : NPCMoveToCommand
    {
        [XmlElement("Map To Move To")]
        public int MapToMoveToID = 0;

        public override void EndOfCommand()
        {
            base.EndOfCommand();
            parentObject.baseCharacter.position = destination;
            parentObject.baseCharacter.UpdatePosition();

            if (parentObject.IsOnCurrentMap(GameProcessor.loadedMap))
            {
                MapChunk.consideredSprites.Add(new objectInfo(parentObject, parentObject.baseCharacter.getHeightIndicator(), objectInfo.type.Character, parentObject.baseCharacter.trueMapSize(), true));
            }
            else if (!parentObject.IsOnCurrentMap(GameProcessor.loadedMap))
            {
                var test = MapChunk.consideredSprites.Find(cs => cs.obj == parentObject);
                if (test != null)
                {
                    MapChunk.consideredSprites.Remove(test);
                }
                //  MapChunk.consideredSprites.Add(new objectInfo(parentObject, parentObject.baseCharacter.getHeightIndicator(), objectInfo.type.Character, parentObject.baseCharacter.trueMapSize()));
            }
        }

        public override void Execute()
        {
            base.Execute();
            if (parentObject.baseCharacter.fromMap == null)
            {
                parentObject.baseCharacter.fromMap = BasicMap.allMapsGame().Find(m => m.identifier == parentObject.mapPlacedID);
                if (parentObject.mapPlacedID == 0)
                {
                    parentObject.baseCharacter.fromMap = GameProcessor.parentMap;
                }
            }
            else
            {
                parentObject.baseCharacter.fromMap = GameProcessor.parentMap.subMaps.Find(m => m.identifier == mapID);
            }

            if (MapToMoveToID == 0)
            {
                parentObject.baseCharacter.currentMapToDisplayOn = parentObject.baseCharacter.fromMap;
            }
            else
            {
                parentObject.baseCharacter.currentMapToDisplayOn = GameProcessor.parentMap.subMaps.Find(m => m.identifier == MapToMoveToID);
            }

            bCompletedCommand = true;

            if (parentObject.baseCharacter.currentMapToDisplayOn == GameProcessor.loadedMap && parentObject.baseCharacter.currentMapToDisplayOn != parentObject.baseCharacter.fromMap)
            {
                MapObjectHelpClass.objectFromOutsideOnThisMap.Add(parentObject);
            }
            else if (parentObject.baseCharacter.currentMapToDisplayOn == parentObject.baseCharacter.fromMap)
            {
                MapObjectHelpClass.objectFromOutsideOnThisMap.Remove(parentObject);
            }
        }

    }
}
