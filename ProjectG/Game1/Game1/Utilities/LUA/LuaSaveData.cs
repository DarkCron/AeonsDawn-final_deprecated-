using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUA
{
    public static class LuaSaveData
    {
        static List<LuaSaveCollection> globalSaveCollection = new List<LuaSaveCollection>();
        static List<LuaSaveCollection> battleSaveCollection = new List<LuaSaveCollection>();

        static public LuaSaveCollection CreateGlobalSave(String name, String typeName)
        {
            LuaSaveCollection temp = globalSaveCollection.Find(sc => sc.dataName.Equals(typeName, StringComparison.OrdinalIgnoreCase) && sc.name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (temp == default(LuaSaveCollection))
            {
                globalSaveCollection.Add(new LuaSaveCollection(name, typeName));
                temp = globalSaveCollection.Last();
            }

            return temp;
        }

        static public LuaSaveCollection getGlobalData(String Name, String typeName)
        {
            LuaSaveCollection temp = globalSaveCollection.Find(sd => sd.name.Equals(Name, StringComparison.OrdinalIgnoreCase) && sd.dataName.Equals(typeName, StringComparison.OrdinalIgnoreCase));
            if (temp == null)
            {
                temp = new LuaSaveCollection();
            }
            return temp;
        }

        static public void assignGlobalData(LuaSaveCollection lsc, NLua.LuaTable data)
        {
            if (!lsc.data.Contains(data))
            {
                lsc.data.Add(data);
            }
        }
    }

    public class LuaSaveCollection
    {
        public String dataName = "Type";
        public String name = "Collection";
        public List<NLua.LuaTable> data = new List<NLua.LuaTable>();

        internal LuaSaveCollection()
        {
        }

        internal LuaSaveCollection(String name, String typeName)
        {
            this.name = name;
            this.dataName = typeName;
        }

        public void assignData(NLua.LuaTable ele)
        {
            if (!data.Contains(ele))
            {
                data.Add(ele);
            }
        }
    }

}
