using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    static public class DayLightHandler
    {
        public enum TimeBlocksNames { pm12am4 = 0, am4am7, am7am12, am12am13, am13am14, am14am19, am19am22, am22pm12 }

        static public List<TimeBlock> timeBlocks = new List<TimeBlock>();
        static int tickTimer = 60;
        static int timePassed = 0;
        static int tickTimerToUpdateShadows = 200;
        static int timePassedToUpdateShadows = 0;


        static public int timeIndex = 0;

        static public float ShadowOffset = 0.6f;
        static public float ShadowDivider = 2.1f;
        static public float ShadowOpacity = 0.5f;
        static public Color lightColor = Color.White;
        static public TimeBlock currentTimeBlock;
        static public bool bPaused = false;

        static int currentHour = 0;

        public static void GenerateDefaultLight()
        {
            timeBlocks.Clear();
            timeBlocks.Add(new TimeBlock(-0.5f, 2.1f, 0f, 1, new Color(165, 172, 176), TimeBlocksNames.pm12am4, 0, 240));
            timeBlocks.Add(new TimeBlock(0.8f, 2.1f, 0.5f, 1, new Color(179, 196, 196), TimeBlocksNames.am4am7, 240, 420));
            timeBlocks.Add(new TimeBlock(1.5f, 5f, 0.6f, 1, new Color(229, 229, 200), TimeBlocksNames.am7am12, 420, 720));
            timeBlocks.Add(new TimeBlock(0.8f, 10f, 0.7f, 1, new Color(242, 242, 235), TimeBlocksNames.am12am13, 720, 780));
            timeBlocks.Add(new TimeBlock(-0.1f, 10f, 0.7f, 1, new Color(242, 242, 235), TimeBlocksNames.am13am14, 780, 840));
            timeBlocks.Add(new TimeBlock(-0.5f, 7f, 0.6f, 1, new Color(229, 229, 200), TimeBlocksNames.am14am19, 840, 1140));
            timeBlocks.Add(new TimeBlock(-1.5f, 2.1f, 0.3f, 1, new Color(179, 196, 196), TimeBlocksNames.am19am22, 1140, 1320));
            timeBlocks.Add(new TimeBlock(-1.8f, 2.1f, 0f, 1, new Color(165, 172, 176), TimeBlocksNames.am22pm12, 1320, 1440));

            currentTimeBlock = timeBlocks[0];
            currentTimeBlock.GenerateModifiers(timeBlocks[1]);
            ShadowOffset = 0f;
            ShadowDivider = 2.1f;
            ShadowOpacity = 0f;
            GameScreenEffect.worldBrightnessLM = 1.0f;
        }

        public static void TimeToLight(int tick)
        {
            timeIndex = tick;
            if (timeBlocks.Count == 0)
            {
                GenerateDefaultLight();
            }
            var timeBlock = timeBlocks.Find(block => block.IsWithinTimeBlock(tick));
            int index = timeBlocks.IndexOf(timeBlock);
            int nextIndex = index + 1;
            if (nextIndex >= timeBlocks.Count) { nextIndex = 0; }
            var nextBlock = timeBlocks[nextIndex];
            currentTimeBlock = timeBlock;
            currentTimeBlock.SetTimeBlockMods();
            int catchUp = currentTimeBlock.CatchUpFactor(tick);
            currentTimeBlock.GenerateModifiers(nextBlock);
            currentTimeBlock.ModifyLight(tick);
        }

        public static void Update(GameTime gt)
        {
            if (!bPaused)
            {
                timePassed += gt.ElapsedGameTime.Milliseconds;
                timePassedToUpdateShadows += gt.ElapsedGameTime.Milliseconds;
                if (timePassedToUpdateShadows >= tickTimer + tickTimerToUpdateShadows)
                {
                    timePassedToUpdateShadows = 0;
                    GameProcessor.bUpdateShadows = true;
                }
                if (timePassed > tickTimer)
                {

                    timePassed = 0;
                    timeIndex++;
                    if (timeIndex >= 24 * 60)
                    {
                        timeIndex = 0;
                    }
                    int hour = timeIndex / 60;


                    var nextBlock = timeBlocks.Find(tb => tb.IsWithinTimeBlock(timeIndex));
                    if (currentTimeBlock != nextBlock && nextBlock != null)
                    {


                        if (timeBlocks.IndexOf(nextBlock) == timeBlocks.Count - 1)
                        {
                            nextBlock.GenerateModifiers(timeBlocks[0]);
                        }
                        else
                        {
                            nextBlock.GenerateModifiers(timeBlocks[timeBlocks.IndexOf(nextBlock) + 1]);
                        }

                        currentTimeBlock = nextBlock;
                        currentTimeBlock.SetValues();



                        TimeBlockChange();


                    }

                    if (hour != currentHour)
                    {
                        currentHour = hour;
                        HourChange();
                    }

                    currentTimeBlock.ModifyLight(timeIndex);

                    // var toBeUpdatedNPCs = GameProcessor.loadedMap.mapNPCs.FindAll(npcs => npcs.MustChangeCommand(timeIndex));
                    var toBeUpdatedNPCs = Utilities.Map.MapObjectHelpClass.objectsToUpdateOutsideOfMap.FindAll(obj => obj.GetType() == typeof(NPC)).Cast<NPC>().ToList().FindAll(npcs => npcs.MustChangeCommand(timeIndex));

                    foreach (var item in toBeUpdatedNPCs)
                    {
                        item.ChangeCommand(timeIndex);
                    }
                    GameProcessor.loadedMap.activeLights.ForEach(l => l.LightUpdate(currentTimeBlock.timeBlockName));
                }
            }
            else if (bPaused)
            {
                timePassedToUpdateShadows += gt.ElapsedGameTime.Milliseconds;
                if (timePassedToUpdateShadows >= tickTimer + tickTimerToUpdateShadows)
                {
                    timePassedToUpdateShadows = 0;
                    GameProcessor.bUpdateShadows = true;
                }

                var toBeUpdatedNPCs = Utilities.Map.MapObjectHelpClass.objectsToUpdateOutsideOfMap.FindAll(obj => obj.GetType() == typeof(NPC)).Cast<NPC>().ToList().FindAll(npcs => npcs.MustChangeCommand(timeIndex));
                foreach (var item in toBeUpdatedNPCs)
                {
                    item.ChangeCommand(timeIndex);
                }
            }

        }

        private static void HourChange()
        {
            //if (currentTimeBlock.timeBlockName == TimeBlocksNames.am7am12 ||
            //currentTimeBlock.timeBlockName == TimeBlocksNames.am7am12 ||
            //currentTimeBlock.timeBlockName == TimeBlocksNames.am12am13 ||
            //currentTimeBlock.timeBlockName == TimeBlocksNames.am13am14 ||
            // currentTimeBlock.timeBlockName == TimeBlocksNames.am13am14)
            //{
            //    //test();
            //}
        }

        public static void TimeBlockChange()
        {
            if (currentTimeBlock.timeBlockName == TimeBlocksNames.am14am19)
            {
                // GameProcessor.loadedMap.mapNPCs.Clear();
            }



            //if (currentTimeBlock.timeBlockName == TimeBlocksNames.am7am12)
            //{
            //    var testShape = GameProcessor.loadedMap.mapSprites.Find(sprite => sprite.shapeID == -25);
            //    while (testShape != null)
            //    {
            //        GameProcessor.loadedMap.mapSprites.Remove(testShape);
            //        testShape = GameProcessor.loadedMap.mapSprites.Find(sprite => sprite.shapeID == -25);
            //    }

            //    test();
            //}
        }

        private static void test()
        {

            //var region = GameProcessor.loadedMap.mapRegions[0];
            //var tiles = GameProcessor.loadedMap.possibleTilesWithController(region.returnBoundingZone());
            //var tempChar = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == 3).Clone();

            //tempChar.shapeID = -25;
            //tempChar.position = new Vector2(19 * 64, 14 * 64);
            //Vector2 end = new Vector2(6 * 64, 14 * 64);
            ////end = tiles[Utilities.GamePlayUtility.Randomize(0, tiles.Count-1)].mapPosition.Location.ToVector2();
            //tempChar.UpdatePosition();
            //try
            //{
            //    var allPossibleNodes = AI.PathFinder.NewPathSearch(tempChar.position, end, tiles);
            //    allPossibleNodes.Reverse();
            //    var lastVector = new Vector2(allPossibleNodes[allPossibleNodes.Count - 1].coord.X * 64, allPossibleNodes[allPossibleNodes.Count - 1].coord.Y * 64);
            //    if (lastVector == end)
            //    {
            //        // (PlayerController.selectedSprite as BaseCharacter).changePosition(new Vector2(allPossibleNodes[1].coord.X * 64, allPossibleNodes[1].coord.Y * 64));
            //        allPossibleNodes.Reverse();
            //        var NodesWithinRange = MapListUtility.findEqualNodesToTileList(allPossibleNodes, tiles);
            //        //PathMoveHandler.Start(gts.character, NodesWithinRange);
            //        tempChar.StartSpriteMoveHandler(NodesWithinRange);
            //    }
            //    GameProcessor.loadedMap.mapSprites.Add(tempChar);
            //}
            //catch (Exception)
            //{

            //}
        }

        static internal int timeToIndex(int h, int m)
        {
            int hours = h % 24;
            int minutes = m % 60;

            return hours * 60 + minutes;

        }

        static internal String indexToTimeString(int i)
        {
            int index = i % 1440; //(24*60 = 1440)
            int hours = i / 60;
            int minutes = (i - 60 * hours);

            return hours.ToString() + "h:" + minutes.ToString() + "m";

        }

        static public void setTickTime(int t)
        {
            tickTimer = t;
        }
    }

    public class TimeBlock
    {
        float offSet = 0.0f;
        float divider = 0.0f;
        float opacity = 0.0f;
        float brightness = 0.0f;

        float offSetModifier = 0.0f;
        float dividerModifier = 0.0f;
        float opacityModifier = 0.0f;

        float brightnessMod = 0.0f;
        int startsAt = 0;
        int stopsAt = 0;

        float[] lightColorChange = new float[3];
        Color timeColor = Color.White;

        public DayLightHandler.TimeBlocksNames timeBlockName = DayLightHandler.TimeBlocksNames.pm12am4;

        public int CatchUpFactor(int index)
        {
            return index - startsAt;
        }

        public void SetTimeBlockMods()
        {
            DayLightHandler.ShadowOpacity = opacity;
            DayLightHandler.ShadowOffset = offSet;
            DayLightHandler.ShadowDivider = divider;
        }

        public TimeBlock(float offSet, float divider, float opacity, float brightness, Color timeColor, DayLightHandler.TimeBlocksNames timeBlockName, int startsAt, int stopsAt)
        {
            this.offSet = offSet;
            this.divider = divider;
            this.timeColor = timeColor;
            this.timeBlockName = timeBlockName;
            this.startsAt = startsAt;
            this.stopsAt = stopsAt;
            this.opacity = opacity;
            this.brightness = brightness;
        }

        public void GenerateModifiers(TimeBlock nextBlock)
        {
            int steps = stopsAt - startsAt;
            offSetModifier = (nextBlock.offSet - offSet) / (float)steps;
            opacityModifier = (nextBlock.opacity - opacity) / (float)steps;
            dividerModifier = (nextBlock.divider - divider) / (float)steps;
            brightnessMod = (nextBlock.brightness - brightness) / (float)steps;

            lightColorChange[0] = (float)(nextBlock.timeColor.R - timeColor.R) / (float)steps;
            lightColorChange[1] = (float)(nextBlock.timeColor.G - timeColor.G) / (float)steps;
            lightColorChange[2] = (float)(nextBlock.timeColor.B - timeColor.B) / (float)steps;
        }

        public void ModifyLight(int index)
        {
            int currentStep = index - startsAt;

            DayLightHandler.lightColor = new Color((int)(timeColor.R + currentStep * lightColorChange[0]), (int)(timeColor.G + currentStep * lightColorChange[1]), (int)(timeColor.B + currentStep * lightColorChange[2]));
            DayLightHandler.ShadowOffset += offSetModifier;
            DayLightHandler.ShadowDivider += dividerModifier;
            DayLightHandler.ShadowOpacity += opacityModifier;
            GameScreenEffect.worldColor = DayLightHandler.lightColor;
            GameScreenEffect.worldBrightnessLM += brightnessMod;
        }

        public void SetValues()
        {
            DayLightHandler.lightColor = timeColor;
            GameScreenEffect.worldBrightnessLM = brightness;
            GameScreenEffect.worldColor = timeColor;
            DayLightHandler.ShadowOffset = offSet;
            DayLightHandler.ShadowDivider = divider;
            DayLightHandler.ShadowOpacity = opacity;

        }

        public bool IsWithinTimeBlock(int timeIndex)
        {
            if (timeIndex >= startsAt && timeIndex < stopsAt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
