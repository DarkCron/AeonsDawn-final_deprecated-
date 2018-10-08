using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUA;
using TBAGW;
using TBAGW.Utilities;

namespace TBAGW.Utilities
{
    static class GamePlayUtility
    {
        static public Random randomizer = new Random();

        static public void Initiate(int seed)
        {
            randomizer = new Random(seed);
            Console.WriteLine("New randomizer assigned. With seed: " + seed);
            LUAUtilities.SetupLuaUtils(randomizer);
        }

        /// <summary>
        /// Generates a number between x and y via RNG
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static public int Randomize(int x, int y)
        {
            if (x < y)
            {
                return randomizer.Next(x, y);
            }
            else
            {
                return randomizer.Next(y, x);
            }

        }

        static public float ExpertRandomize(float x, float y)
        {
            if (x < y)
            {
                return (float)randomizer.NextDouble() * ((y + 1) - x) + x;
            }
            else
            {
                return (float)randomizer.NextDouble() * ((x + 1) - y) + y;
            }
        }

        static public float ExpertRandomizeSmallNumbers(float x, float y)
        {
            if (x < y)
            {
                return (float)randomizer.NextDouble() * ((y) - x) + x;
            }
            else
            {
                return (float)randomizer.NextDouble() * ((x) - y) + y;
            }
        }

        static public int randomChance()
        {
            return Randomize(0, 101);
        }

        /// <summary>
        /// example 50.5 is 50.5%
        /// </summary>
        /// <returns></returns>
        static public bool randomChance(float num)
        {
            float random = ExpertRandomizeSmallNumbers(0.0f, 1.0f);
            if (num / 100f > random)
            {
                return true;
            }
            return false;
        }
    }


}

namespace LUA
{
    public static class LuaHelp
    {
        public static int Random(int x, int y)
        {
            return TBAGW.Utilities.GamePlayUtility.Randomize(x, y);
        }

        public static BasicAbility.ABILITY_AFFINITY getAffinity(String name)
        {
            var temp = Enum.GetNames(typeof(BasicAbility.ABILITY_AFFINITY)).ToList().Find(aff => aff.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (temp != default(String))
            {
                return (BasicAbility.ABILITY_AFFINITY)Enum.GetNames(typeof(BasicAbility.ABILITY_AFFINITY)).ToList().IndexOf(temp);
            }
            else
            {
                return BasicAbility.ABILITY_AFFINITY.None;
            }
        }

        public static LuaPoint RandomMapPosition()
        {
            if (CombatProcessor.zoneTiles.Count != 0)
            {
                BasicTile randomTile = CombatProcessor.zoneTiles[GamePlayUtility.Randomize(0, CombatProcessor.zoneTiles.Count - 1)];
                LuaPoint p = new LuaPoint(randomTile.positionGrid.X, randomTile.positionGrid.Y);
                return p;
            }
            return null;
        }

        public static BasicTile PointToTile(LuaPoint lp)
        {
            if (CombatProcessor.zoneTiles.Count != 0)
            {
                BasicTile randomTile = CombatProcessor.zoneTiles.Find(t => t.positionGrid == lp.toVector2());
                return randomTile;
            }
            return null;
        }
    }
}
