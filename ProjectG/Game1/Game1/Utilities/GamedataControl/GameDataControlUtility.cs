using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW.Utilities.GamedataControl
{
    static public class GameDataControlUtility
    {
        public enum ControlledDataType1 { Player_Money, Amount, ItemAmount }
        public enum ControlledDataType1Names { Money, Amount, Item_Amount }

        public enum ContolType { Is_Equal, Is_Greater, Is_Lesser, Is_Greater_Or_Equal, Is_Lesser_Or_Equal }
        public enum CheckType { AND, OR }
    }

    public class DataControlCondition
    {
        
        public DataControlCondition() { }
    }
}
