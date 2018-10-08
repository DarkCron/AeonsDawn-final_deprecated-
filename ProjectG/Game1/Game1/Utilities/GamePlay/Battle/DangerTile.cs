using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LUA;

namespace TBAGW
{
    internal class DangerTile
    {

        NLua.LuaFunction callFunction;
        BasicTile parentLocation;
        int timer = 2;
        int timePassed = 0;
        bool ticksEveryTurn = false;
        bool bRemove = false;
        int groupCallIndex = -1;

        static String texLoc = @"Graphics\GUI\Warning";
        static Texture2D tex = Game1.contentManager.Load<Texture2D>(texLoc);
        static bool bBuildUp = true;

        internal static TimingUtility texTimer;

        internal DangerTile(BasicTile tile, NLua.LuaFunction f)
        {
            parentLocation = tile;
            callFunction = f;
        }

        internal void Tick(int index)
        {
            if (groupCallIndex == index)
            {
                timePassed++;
            }
        }

        internal bool IsReady()
        {
            if (ticksEveryTurn) { return timePassed <= timer; }
            return timePassed >= timer;
        }

        internal void Execute()
        {
            bool bDone = false;
            for (int i = 0; i < EncounterInfo.encounterGroups.Count; i++)
            {
                for (int j = 0; j < EncounterInfo.encounterGroups[i].charactersInGroup.Count; j++)
                {
                    if (parentLocation.positionGrid == EncounterInfo.encounterGroups[i].charactersInGroup[j].positionToMapCoords())
                    {
                        bDone = true;
                        if (callFunction != null)
                        {
                            try
                            {
                                callFunction.Call(EncounterInfo.encounterGroups[i].charactersInGroup[j].toCharInfo());
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                        break;
                    }
                }
                if (bDone) { break; }
            }

            if (!ticksEveryTurn)
            {
                bRemove = true;
            }
            else if (timePassed >= timer)
            {
                bRemove = true;
            }
        }

        internal bool Remove()
        {
            return bRemove;
        }

        static internal bool timerEnd()
        {
            if (texTimer.percentageDone() >= 1.0f)
            {
                return true;
            }
            return false;
        }

        static internal void Reset()
        {
            bBuildUp = true;
            texTimer = new TimingUtility(16, true, timerEnd);
            texTimer.SetStepTimer(120);
        }

        static internal void Update(GameTime gt)
        {
            texTimer.Tick(gt);

            if (texTimer.IsDone())
            {
                texTimer.SetStepTimer(120, 0);
                texTimer.Start();
                bBuildUp = !bBuildUp;
            }
        }

        static internal void Draw(SpriteBatch sb)
        {
            float opacity = 0.0f;
            if (bBuildUp) { opacity = texTimer.percentageDone(); } else { opacity = 1.0f - texTimer.percentageDone(); }

            if (opacity != 0.0f)
            {
                for (int i = 0; i < EncounterInfo.dangerTiles.Count; i++)
                {
                    sb.Draw(Game1.WhiteTex, EncounterInfo.dangerTiles[i].parentLocation.mapPosition, Color.Orange * (opacity));
                    sb.Draw(tex, EncounterInfo.dangerTiles[i].parentLocation.mapPosition, Color.White * opacity);
                }
            }
        }

        internal static DangerTile Convert(LuaDangerTile luaDangerTile)
        {
            DangerTile temp = null;
            try
            {
                BasicTile t = LuaHelp.PointToTile(luaDangerTile.tile);
                temp = new DangerTile(t, luaDangerTile.function);

            }
            catch (Exception)
            {
                return null;
            }
            if (temp != null)
            {
                if (temp.callFunction == null)
                {
                    //return null
                }
                return temp;
            }
            return null;
        }

        internal NLua.LuaFunction getFunction()
        {
            return callFunction;
        }

        internal void SetGroupIndex(int v)
        {
            groupCallIndex = v;
            if (groupCallIndex == -1)
            {
                groupCallIndex = 0;
            }
        }
    }
}

namespace LUA
{
    public class LuaDangerTile
    {
        public NLua.LuaFunction function;
        public LuaPoint tile = new LuaPoint();
        public int timer = 2;
        public int groupCallIndex = -1;

        public LuaDangerTile() { }

        public LuaDangerTile(LuaPoint p)
        {
            tile = p;

        }

        public LuaDangerTile(LuaPoint p, NLua.LuaFunction f)
        {
            tile = p;
            SetFunction(f);
        }

        public void SetFunction(NLua.LuaFunction f)
        {
            function = f;
        }

        internal TBAGW.DangerTile TryConvert()
        {
            return TBAGW.DangerTile.Convert(this);
        }

        public void Initialize()
        {
            TBAGW.DangerTile dt = this.TryConvert();
            if (dt != null)
            {
                if (dt.getFunction() != null)
                {
                    if (groupCallIndex == -1)
                    {
                        dt.SetGroupIndex(TBAGW.EncounterInfo.encounterGroups.IndexOf(TBAGW.EncounterInfo.currentTurn()));
                    }

                    TBAGW.EncounterInfo.AddDangerTile(dt);
                }
            }
        }
    }
}
