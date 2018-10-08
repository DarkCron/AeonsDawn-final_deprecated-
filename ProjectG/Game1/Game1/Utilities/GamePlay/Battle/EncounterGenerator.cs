using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;

using TBAGW.Utilities.Sprite;

namespace Game1.Utilities.GamePlay.Battle
{
    public static class EncounterGenerator
    {
        static MapRegion region;
        static MapZone zone;
        static List<BaseCharacter> enemies = new List<BaseCharacter>();
        static internal customSpawnAmount cSpawnAmount = default(customSpawnAmount);

        internal struct customSpawnAmount
        {
            internal int min;
            internal int max;

            internal customSpawnAmount(int min, int max)
            {
                this.min = min;
                this.max = max;
            }
        }

        static public void Start(MapRegion r, MapZone z, List<BasicTile> startZone)
        {
            //GameProcessor.encounterEnemies.Clear();
            enemies.Clear();
            region = r;
            zone = z;
            int min = z.zoneEncounterInfo.packSizeMin;
            int max = z.zoneEncounterInfo.packSizeMax;
            if (!cSpawnAmount.Equals( default(customSpawnAmount)))
            {
                min = cSpawnAmount.min;
                max = cSpawnAmount.max;
            }
            int amountOfEnemies = GamePlayUtility.Randomize(min, max);
            var spawnList = new List<TBAGW.KeyValuePair<int, EnemyAIInfo>>();
            int index = 0;
            foreach (var item in zone.zoneEncounterInfo.enemySpawnChance)
            {
                spawnList.Add(new TBAGW.KeyValuePair<int, EnemyAIInfo>(item, z.zoneEncounterInfo.enemies[index]));
                index++;
            }
            spawnList = spawnList.OrderBy(ele => ele.Key).ToList();

            while (enemies.Count < amountOfEnemies)
            {
                int maxTries = spawnList.Count;
                int currentTry = 0;
                while (currentTry < maxTries)
                {
                    int percentage = GamePlayUtility.Randomize(0, 100);
                    if (spawnList[currentTry].Key > percentage)
                    {
                        // enemies.Add(zone.zoneEncounterInfo.enemies[currentTry].ShallowCopy());
                        enemies.Add(spawnList[currentTry].Value.enemyCharBase);

                    }
                    currentTry++;
                }


            }

            //Console.WriteLine("I think I have a random list of enemies ready... Not sure though...");

            //  GenerateLocations();
            NewGenerateLocationsCode(startZone);
            // return enemies;
        }

        private static void NewGenerateLocationsCode(List<BasicTile> startZonePlayer)
        {

            foreach (var item in enemies)
            {
                var temp = GameProcessor.loadedMap.possibleTilesGameZoneForEnemyINITIALIZATION(CombatProcessor.zoneTiles).Except(startZonePlayer).ToList();
                int randomNum = GamePlayUtility.Randomize(0, temp.Count);
                var randomTile = temp[randomNum];
                item.spriteGameSize = new Rectangle(((Rectangle)randomTile.mapPosition).X, ((Rectangle)randomTile.mapPosition).Y, ((Rectangle)randomTile.mapPosition).Width, ((Rectangle)randomTile.mapPosition).Height);
                item.spriteGameSize.Width = 64;
                item.spriteGameSize.Height = 64;
                item.rotationIndex = GamePlayUtility.Randomize(0, 4);
                item.position = item.spriteGameSize.Location.ToVector2();
                //GameProcessor.encounterEnemies = enemies;
                CombatProcessor.encounterEnemies.Add(item);
            }

            LootGenerator.Generate(CombatProcessor.encounterEnemies);
            // GameProcessor.encounterEnemies = enemies;
            // CombatProcessor.encounterEnemies = enemies;
        }

    }
}
