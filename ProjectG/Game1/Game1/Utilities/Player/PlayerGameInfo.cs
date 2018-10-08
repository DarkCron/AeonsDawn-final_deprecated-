using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    static public class PlayerGameInfo
    {
        static public PlayerInventory playerInventory = new PlayerInventory();
        static public List<BaseCharacter> heroParty = new List<BaseCharacter>();
        public static int playerID = 4444;

    }
}
