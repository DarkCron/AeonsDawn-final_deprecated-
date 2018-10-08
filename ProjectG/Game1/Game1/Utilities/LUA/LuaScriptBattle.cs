using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW;
using TBAGW.Utilities;

namespace LUA
{
    public class LuaScriptBattle
    {
        public List<Enemy> enemies = new List<Enemy>();
        public LuaPoint partySpawn = new LuaPoint(0);
        public int regionID = 0;
        public int zoneID = 0;


        public LuaScriptBattle() { }


        public static bool Initialize(LuaScriptBattle lsb)
        {
            MapRegion mr = GameProcessor.loadedMap.mapRegions.Find(r => r.regionID == lsb.regionID);
            if (mr == null) { return false; }
            MapZone mz = mr.regionZones.Find(z => z.zoneID == lsb.zoneID);
            if (mz == null) { return false; }

            List<BasicTile> zoneTiles = new List<BasicTile>();

            TBAGW.Utilities.Sprite.BaseSprite controller = TBAGW.Utilities.Control.Player.PlayerController.selectedSprite;
            controller.position = new Vector2(((int)lsb.partySpawn.x) * 64, ((int)lsb.partySpawn.y) * 64);
            controller.spriteGameSize.Location = controller.position.ToPoint();
            controller.UpdatePosition();

            if (mz.zoneTiles.Count == 0)
            {
                zoneTiles = GameProcessor.loadedMap.possibleTilesWithController(mz.returnBoundingZone(), TBAGW.Utilities.Control.Player.PlayerController.selectedSprite);
                mz.zoneTiles = zoneTiles;
            }
            else
            {
                zoneTiles = mz.zoneTiles;
            }

            var temp_loc = MapListUtility.returnMapRadius(2, zoneTiles, controller);
            //LUA.LuaScriptBattle.Initialize(lsb);
            foreach (var item in lsb.enemies)
            {
                item.ToEnemy(mz, temp_loc);
            }

            GameProcessor.EnableCombatStage();
            GameProcessor.mainControllerBeforeCombat = TBAGW.Utilities.Control.Player.PlayerController.selectedSprite;
            CombatProcessor.InitializeFromScript(new Vector2(lsb.partySpawn.x, lsb.partySpawn.y), mr, mz, lsb);

            return true;
        }
    }

    public class Enemy
    {
        public String name = "";
        public LuaPoint location = new LuaPoint(0);
        public int rot = 0;

        public Enemy() { }

        public Enemy(String n, LuaPoint l)
        {
            name = n;
            location = l;
        }

        public TBAGW.EnemyAIInfo ToEnemy(TBAGW.MapZone mz, List<BasicTile> playerZone)
        {
            TBAGW.EnemyAIInfo temp = mz.zoneEncounterInfo.enemies.Find(e => e.enemyName.Equals(name, StringComparison.OrdinalIgnoreCase)
            || name.Equals(e.enemyCharBase.CharacterName, StringComparison.OrdinalIgnoreCase)
            || name.Equals(e.enemyCharBase.displayName, StringComparison.OrdinalIgnoreCase));

            if (temp == null)
            {
                return null;
            }

            bool bGenerateLocation = true;
            Vector2 pos = new Vector2(location.x * 64, location.y * 64);
            BasicTile tile = new BasicTile();
            if (mz.Contains(pos))
            {
                tile = GameProcessor.loadedMap.possibleTilesGameZoneForEnemyINITIALIZATION(mz.zoneTiles).Except(playerZone).ToList().Find(t => t.positionGrid == new Vector2(location.x, location.y));
                if (tile != default(TBAGW.BasicTile))
                {
                    Console.WriteLine("From LuaScriptBattle, script enemy has correct spawn location!");
                    bGenerateLocation = false;

                    //int randomNum = GamePlayUtility.Randomize(0, temp.Count);
                    //var randomTile = temp[randomNum];


                }
            }

            if (bGenerateLocation)
            {
                Console.WriteLine("From LuaScriptBattle, script enemy has incorrect spawn location!");
                var tiles = GameProcessor.loadedMap.possibleTilesGameZoneForEnemyINITIALIZATION(mz.zoneTiles).Except(playerZone).ToList();
                if (tiles.Count < PlayerSaveData.heroParty.Count)
                {
                    return null;
                }
                int randomNum = GamePlayUtility.Randomize(0, tiles.Count);
                tile = tiles[randomNum];
            }

            CombatProcessor.encounterEnemies.Add(temp.enemyCharBase);
            CombatProcessor.encounterEnemies.Last().spriteGameSize = new Rectangle(((Rectangle)tile.mapPosition).X, ((Rectangle)tile.mapPosition).Y, ((Rectangle)tile.mapPosition).Width, ((Rectangle)tile.mapPosition).Height);

            CombatProcessor.encounterEnemies.Last().position = CombatProcessor.encounterEnemies.Last().spriteGameSize.Location.ToVector2();
            CombatProcessor.encounterEnemies.Last().rotationIndex = rot % 4;
            CombatProcessor.encounterEnemies.Last().UpdatePosition();

            //temp.enemyCharBase.spriteGameSize = new Rectangle(((Rectangle)tile.mapPosition).X, ((Rectangle)tile.mapPosition).Y, ((Rectangle)tile.mapPosition).Width, ((Rectangle)tile.mapPosition).Height);
            //item.spriteGameSize.Width = 64;
            //item.spriteGameSize.Height = 64;
            //temp.enemyCharBase.spriteGameSize = new Rectangle(((Rectangle)tile.mapPosition).X, ((Rectangle)tile.mapPosition).Y, ((Rectangle)tile.mapPosition).Width, ((Rectangle)tile.mapPosition).Height);
            //temp.enemyCharBase.rotationIndex = rot % 4;
            //temp.enemyCharBase.position = temp.enemyCharBase.spriteGameSize.Location.ToVector2();
            //GameProcessor.encounterEnemies = enemies;
            //CombatProcessor.encounterEnemies.Add(temp.enemyCharBase);
            //CombatProcessor.encounterEnemies.Last().spriteGameSize = new Rectangle(((Rectangle)tile.mapPosition).X, ((Rectangle)tile.mapPosition).Y, ((Rectangle)tile.mapPosition).Width, ((Rectangle)tile.mapPosition).Height);
            return null;
        }

        internal TBAGW.Utilities.Characters.BaseCharacter ToEnemyDuringBattle(TBAGW.MapZone mz, List<BasicTile> playerZone)
        {
            TBAGW.EnemyAIInfo temp = mz.zoneEncounterInfo.enemies.Find(e => e.enemyName.Equals(name, StringComparison.OrdinalIgnoreCase)
            || name.Equals(e.enemyCharBase.CharacterName, StringComparison.OrdinalIgnoreCase)
            || name.Equals(e.enemyCharBase.displayName, StringComparison.OrdinalIgnoreCase));

            if (temp == null)
            {
                return null;
            }

            bool bGenerateLocation = true;
            Vector2 pos = new Vector2(location.x * 64, location.y * 64);
            BasicTile tile = new BasicTile();
            if (mz.Contains(pos))
            {
                tile = GameProcessor.loadedMap.possibleTilesGameZoneForEnemyINITIALIZATION(mz.zoneTiles).Except(playerZone).ToList().Find(t => t.positionGrid == new Vector2(location.x, location.y));
                if (tile != default(TBAGW.BasicTile))
                {
                    Console.WriteLine("From LuaScriptBattle, script enemy has correct spawn location!");
                    bGenerateLocation = false;

                    //int randomNum = GamePlayUtility.Randomize(0, temp.Count);
                    //var randomTile = temp[randomNum];


                }
            }

            if (bGenerateLocation)
            {
                Console.WriteLine("From LuaScriptBattle, script enemy has incorrect spawn location!");
                var tiles = GameProcessor.loadedMap.possibleTilesGameZoneForEnemyINITIALIZATION(mz.zoneTiles).Except(playerZone).ToList();
                if (tiles.Count < PlayerSaveData.heroParty.Count)
                {
                    return null;
                }
                int randomNum = GamePlayUtility.Randomize(0, tiles.Count);
                tile = tiles[randomNum];
            }

            TBAGW.Utilities.Characters.BaseCharacter bcTemp = temp.enemyCharBase;
            bcTemp.spriteGameSize = new Rectangle(((Rectangle)tile.mapPosition).X, ((Rectangle)tile.mapPosition).Y, ((Rectangle)tile.mapPosition).Width, ((Rectangle)tile.mapPosition).Height);
            bcTemp.position = CombatProcessor.encounterEnemies.Last().spriteGameSize.Location.ToVector2();
            bcTemp.rotationIndex = rot % 4;
            bcTemp.UpdatePosition();


            return bcTemp;
            //CombatProcessor.encounterEnemies.Add(temp.enemyCharBase);
            //CombatProcessor.encounterEnemies.Last().spriteGameSize = new Rectangle(((Rectangle)tile.mapPosition).X, ((Rectangle)tile.mapPosition).Y, ((Rectangle)tile.mapPosition).Width, ((Rectangle)tile.mapPosition).Height);

            //CombatProcessor.encounterEnemies.Last().position = CombatProcessor.encounterEnemies.Last().spriteGameSize.Location.ToVector2();
            //CombatProcessor.encounterEnemies.Last().rotationIndex = rot % 4;
            //CombatProcessor.encounterEnemies.Last().UpdatePosition();

            //temp.enemyCharBase.spriteGameSize = new Rectangle(((Rectangle)tile.mapPosition).X, ((Rectangle)tile.mapPosition).Y, ((Rectangle)tile.mapPosition).Width, ((Rectangle)tile.mapPosition).Height);
            //item.spriteGameSize.Width = 64;
            //item.spriteGameSize.Height = 64;
            //temp.enemyCharBase.spriteGameSize = new Rectangle(((Rectangle)tile.mapPosition).X, ((Rectangle)tile.mapPosition).Y, ((Rectangle)tile.mapPosition).Width, ((Rectangle)tile.mapPosition).Height);
            //temp.enemyCharBase.rotationIndex = rot % 4;
            //temp.enemyCharBase.position = temp.enemyCharBase.spriteGameSize.Location.ToVector2();
            //GameProcessor.encounterEnemies = enemies;
            //CombatProcessor.encounterEnemies.Add(temp.enemyCharBase);
            //CombatProcessor.encounterEnemies.Last().spriteGameSize = new Rectangle(((Rectangle)tile.mapPosition).X, ((Rectangle)tile.mapPosition).Y, ((Rectangle)tile.mapPosition).Width, ((Rectangle)tile.mapPosition).Height);
            return null;
        }
    }
}
