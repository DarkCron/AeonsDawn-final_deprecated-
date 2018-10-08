using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    public class BaseMaterials:BaseItem
    {
        public BaseMaterials():base() {

        }

        public BaseMaterials(bool b) : base(true)
        {
            itemType = ITEM_TYPES.Materials;
            itemStackSize = 999;
            itemMaxAmount = -1;
        }
    }
}
