using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    [XmlRoot("Tile Source")]
    public class TileSource
    {
        public enum TileType { Ground = 0, Building, Fluid, Stairs }

        [XmlElement("Tile Source ID")]
        public int tileID = 0;
        [XmlElement("Tile Texture")]
        public ShapeAnimation tileAnimation = new ShapeAnimation();
        [XmlElement("Tile Source Name")]
        public String tileName = "Tile";
        [XmlElement("HitBoxes")]
        public List<Rectangle> tileHitBoxes = new List<Rectangle>();
        [XmlElement("Default layer")]
        public int defaultLayer = 0;
        [XmlElement("Tile type")]
        public TileType tileType = TileType.Ground;

        public TileSource()
        {

        }

        public void ReloadTextures()
        {
            tileAnimation.ReloadTexture();
            ResetAnimation();
        }

        private void ResetAnimation()
        {
            tileAnimation.frameIndex = 0;

        }

        public List<Rectangle> getHitBoxes(Point coord)
        {
            var temp = new List<Rectangle>(tileHitBoxes);
            temp.ForEach(r => { r.Location = new Point(coord.X * 64, coord.Y * 64); });
            return temp;
        }

        public void Update(GameTime gt)
        {
            tileAnimation.UpdateAnimationForItems(gt);
        }

        public override string ToString()
        {
            return tileName + ", ID: " + tileID;
        }

        public TileSource Clone()
        {
            TileSource temp = (TileSource)MemberwiseClone();
            temp.tileAnimation = tileAnimation.Clone();
            return temp;
        }
    }
}
