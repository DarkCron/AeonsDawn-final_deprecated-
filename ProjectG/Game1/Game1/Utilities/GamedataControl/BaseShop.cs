using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    internal class BaseShop:GameDataHolder
    {

    }

    [XmlRoot("Base shop data save")]
    public class BaseShopSaveInfo:GameDataHolderSaveInfo
    {
        public BaseShopSaveInfo():base(){ }
    }
}
