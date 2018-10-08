using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW;

namespace LUA
{
    public class LuaBScriptEvent
    {
        public enum EventType { postCT = 0, postPT, postET, startPT, startET, startCT, None , updateEV}
        public String functionName = "";
        public bool bNoParameter = false;
        public EventType eventType = EventType.None;
        public String fileNameLoc = "";
        internal static int msTime = 0;

        internal NLua.Lua state = null;

        public LuaBScriptEvent() { }

        public LuaBScriptEvent(LuaBScriptEvent lbse)
        {
            fileNameLoc = lbse.fileNameLoc;
        }

        internal void Execute(LuaTurnSetInfo ltsi)
        {
            if (state == null)
            {
                state = new NLua.Lua();
                state.LoadCLRPackage();
                if (fileNameLoc.StartsWith(@"\"))
                {
                    fileNameLoc = fileNameLoc.Remove(0, 1);
                }
                String comp = System.IO.Path.Combine(TBAGW.Game1.rootContent, fileNameLoc);
                state.DoFile(comp);
            }
            if (state.GetFunction(functionName) != null)
            {
                if (bNoParameter)
                {
                    (state[functionName] as NLua.LuaFunction).Call();
                }
                else
                if (!bNoParameter)
                {
                    //var temp = ltsi.otherGroups[0].RandomMember();
                    if (eventType == EventType.updateEV)
                    {
                        (state[functionName] as NLua.LuaFunction).Call(ltsi,msTime);
                    }
                    else
                    {
                        (state[functionName] as NLua.LuaFunction).Call(ltsi);
                    }
                  
                }
            }
            else
            {
                Console.WriteLine("Lua error from " + fileNameLoc + " , @function-name " + functionName);
                eventType = EventType.None;
            }
        }

        static public void Add(LuaBScriptEvent lbse)
        {
            LuaBScriptHandle.AddBScript(lbse);
        }
    }
}
