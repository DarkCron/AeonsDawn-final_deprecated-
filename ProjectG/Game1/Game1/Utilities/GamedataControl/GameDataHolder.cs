using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    internal abstract class GameDataHolder
    {
        int id { get; set; }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        String dataHolderName { get; set; }
        public String DataHolderName
        {
            get { if(dataHolderName==null) { dataHolderName = ""; } return dataHolderName; }
            set { dataHolderName = value; }
        }

        String dataHolderDescription { get; set; }
        public String DataHolderDescription
        {
            get { if (dataHolderDescription == null) { dataHolderDescription = ""; } return dataHolderDescription; }
            set { dataHolderDescription = value; }
        }

        public virtual List<Object> getData()
        {
            return new List<object>();
        }


    }

    [XmlRoot("Game data holder save info")]
    [XmlInclude(typeof(BaseShop))]
    public abstract class GameDataHolderSaveInfo
    {
        [XmlElement("Data Holder ID")]
        public int ID = 0;
        [XmlElement("Name")]
        public String name = "";
        [XmlElement("Description")]
        public String description = "";

        public GameDataHolderSaveInfo() { }
    }
}
