using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    static internal class BattleScriptHandler
    {
        static List<LUA.LuaBScriptEvent> BScripts = new List<LUA.LuaBScriptEvent>();

        internal static void Reset() {
            BScripts.Clear();
            
        }

        internal static void AddBScript(LUA.LuaBScriptEvent lbse)
        {
            BScripts.Add(lbse);
        }

        internal static void Execute(LUA.LuaBScriptEvent.EventType et, LUA.LuaTurnSetInfo ltsi)
        {
            switch (et)
            {
                case LUA.LuaBScriptEvent.EventType.postCT:
                    BScripts.Where(s => s.eventType == et).ToList().ForEach(s=>s.Execute(ltsi));
                    EncounterInfo.bGenerateLuaTurnInfo = true;
                    break;
                case LUA.LuaBScriptEvent.EventType.postPT:
                    BScripts.Where(s => s.eventType == et).ToList().ForEach(s => s.Execute(ltsi));
                    EncounterInfo.bGenerateLuaTurnInfo = true;
                    break;
                case LUA.LuaBScriptEvent.EventType.postET:
                    BScripts.Where(s => s.eventType == et).ToList().ForEach(s => s.Execute(ltsi));
                    EncounterInfo.bGenerateLuaTurnInfo = true;
                    break;
                case LUA.LuaBScriptEvent.EventType.startPT:
                    BScripts.Where(s => s.eventType == et).ToList().ForEach(s => s.Execute(ltsi));
                    EncounterInfo.bGenerateLuaTurnInfo = true;
                    break;
                case LUA.LuaBScriptEvent.EventType.startET:
                    BScripts.Where(s => s.eventType == et).ToList().ForEach(s => s.Execute(ltsi));
                    EncounterInfo.bGenerateLuaTurnInfo = true;
                    break;
                case LUA.LuaBScriptEvent.EventType.startCT:
                    BScripts.Where(s => s.eventType == et).ToList().ForEach(s => s.Execute(ltsi));
                    EncounterInfo.bGenerateLuaTurnInfo = true;
                    break;
                case LUA.LuaBScriptEvent.EventType.None:
                    break;
                case LUA.LuaBScriptEvent.EventType.updateEV:
                   // BScripts.Where(s => s.eventType == et).ToList().ForEach(s => s.Execute(ltsi));
                    break;
                default:
                    break;
            }
        }
    }
}

namespace LUA
{
    static public class LuaBScriptHandle
    {
        public static void AddBScript(LUA.LuaBScriptEvent lbse)
        {
            TBAGW.BattleScriptHandler.AddBScript(lbse);
        }
    }
}
