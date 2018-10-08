using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW;
using TBAGW.Utilities.Characters;

namespace LUA
{
    public class LuaGameClass
    {
        public int ID = 0;
        public int level = 0;
        public LuaCharacterInfo character;
        public LuaStatEdit statAddition = new LuaStatEdit();
        public LuaClassPoints classPoints = new LuaClassPoints();
        public int quality = 0;

        internal STATChart additionStat = null;
        internal ClassPoints additionPoints = null;
        internal TBAGW.BaseClass parent;
        internal static List<GameText> titles = new List<GameText>();
        internal static bool bInitialize = false;

        public LuaGameClass()
        {
            if (!bInitialize)
            {
                Initialize();
            }
        }

        public LuaGameClass(TBAGW.BaseClass p, BaseCharacter bc)
        {
            if (!bInitialize)
            {
                Initialize();
            }
            parent = p;
            level = p.classEXP.classLevel;
            character = bc.toCharInfo();
            ID = p.classIdentifier;
        }

        private void Initialize()
        {
            bInitialize = true;
            titles.Clear();
            titles.Add(new GameText("Mediocre"));
            titles.Add(new GameText("Average"));
            titles.Add(new GameText("Great"));
            titles.Add(new GameText("Fantastic"));
            titles.Add(new GameText("Amazing"));
            titles.Add(new GameText("Legendary"));
        }

        static internal LuaGameClass editorSummon(TBAGW.BaseClass p)
        {
            LuaGameClass lgc = new LuaGameClass();
            lgc.parent = p;
            lgc.level = p.classEXP.classLevel;
            lgc.ID = p.classIdentifier;

            return lgc;
        }

        internal void HandleLevelUp(BaseClass equippedClass, BaseCharacter currentBC)
        {
            STATChart tempStatAddition = null;
            ClassPoints tempCP = null;

            #region
            if (statAddition != null)
            {
                try
                {
                    tempStatAddition = statAddition.ExtractStatChart();
                }
                catch (Exception)
                {

                }
            }

            if (classPoints != null)
            {
                try
                {
                    tempCP = ClassPoints.toClassPoints(classPoints);
                }
                catch (Exception)
                {

                }
            }
            #endregion
            #region
            if (tempStatAddition != null)
            {
                try
                {
                    equippedClass.classStats.AddStatChartWithoutActive(tempStatAddition);
                }
                catch (Exception)
                {

                }
            }

            if (tempCP != null)
            {
                try
                {
                    equippedClass.AddPoints(tempCP);
                }
                catch (Exception)
                {

                }
            }
            #endregion

            additionStat = tempStatAddition;
            additionPoints = tempCP;
            TBAGW.Utilities.ExpGainScreen.levelUpInfo = this;
        }

        internal GameText getQuality()
        {
            if (Math.Abs(quality)<titles.Count)
            {
                return titles[Math.Abs(quality)];
            }
            else if (Math.Abs(quality) >= titles.Count)
            {
                return titles.Last();
            }else
            {
                return titles[0];
            }
        }
    }
}
