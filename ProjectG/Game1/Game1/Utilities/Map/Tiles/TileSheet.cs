using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("TileSheet")]
    public class TileSheet
    {
        [XmlElement("TileCollection")]
        public List<BasicTile> tiles = new List<BasicTile>();
        [XmlElement("TileSheetName")]
        public String tileSheetName="";
        [XmlElement("TileSheetLoc")]
        public String tileSheetLocation = "";
        [XmlElement("TileSheetTexLoc")]
        public String tileSheetTextureLocation = "";
        [XmlElement("Editable")]
        public bool bIsEditable = true;
        [XmlElement("TileSheet ID")]
        public int tsID = 0;

        [XmlIgnore]
        public Texture2D tileSheetTexture;

        public TileSheet()
        {

        }

        public void ReloadTextures(Game game)
        {
            tileSheetTextureLocation = "Graphics\\Tiles\\CF\\Castle";
            tileSheetTexture = Game1.contentManager.Load<Texture2D>(tileSheetTextureLocation);
        }

        internal void ReloadTextures()
        {
            tileSheetTextureLocation = "Graphics\\Tiles\\CF\\Castle";
           tileSheetTexture = Game1.contentManager.Load<Texture2D>(tileSheetTextureLocation);
        }

        public override string ToString()
        {
            String toString = tileSheetLocation;
            if(tileSheetName.Equals("")) {
                return "tsID" + toString;
            }
            else {
                return "tsID" + tileSheetName;
            }
           
        }
    }
}
