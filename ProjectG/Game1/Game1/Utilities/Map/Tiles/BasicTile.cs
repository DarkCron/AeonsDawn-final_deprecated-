using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Scenes.Editor;

namespace TBAGW
{
    [XmlRoot("Tile")]
    public class BasicTile
    {
        public enum Rotation { Zero, Ninety }

        [XmlElement("Draw Size")]
        public Point drawSize = new Point(64);
        [XmlElement("offset Grid")]
        public Point offset = new Point();
        [XmlElement("PositionGrid")]
        public Vector2 positionGrid = new Vector2();
        [XmlElement("TileLayer")]
        public int tileLayer = 1;
        [XmlElement("TileSource ID")]
        public int tsID = 0;
        [XmlElement("Tile Sprite effect")]
        public SpriteEffects spriteEffect = SpriteEffects.None;
        [XmlElement("Rotation")]
        public Rotation rotation = Rotation.Zero;

        [XmlIgnore]
        public Rectangle drawPosition = new Rectangle();
        [XmlIgnore]
        public Rectangle mapPosition = new Rectangle();
        [XmlIgnore]
        public TileSource tileSource = new TileSource();
        [XmlIgnore]
        public List<Rectangle> hitboxes = new List<Rectangle>();
        [XmlIgnore]
        public bool bMustReload = true;

        public void Reload(GameContentDataBase gcdb)
        {
            tileSource = gcdb.gameSourceTiles.Find(gst => gst.tileID == tsID);
            hitboxes = tileSource.getHitBoxes(positionGrid.ToPoint());
            bMustReload = false;
        }

        public void Reload()
        {
            reloadMapPosition();
            tileSource = GameProcessor.gcDB.gameSourceTiles.Find(gst => gst.tileID == tsID);
            hitboxes = tileSource.getHitBoxes(positionGrid.ToPoint());
            bMustReload = false;
        }

        public void reloadMapPosition()
        {
            mapPosition = new Rectangle((int)(positionGrid.X * 64), (int)(positionGrid.Y * 64), 64, 64);
            drawPosition = new Rectangle((int)(positionGrid.X * 64) + offset.X, (int)(positionGrid.Y * 64) + offset.Y, drawSize.X, drawSize.Y);
            hitboxes = new List<Rectangle>();
            tileSource.tileHitBoxes.ForEach(hb => hitboxes.Add(new Rectangle(hb.Location + (positionGrid * 64).ToPoint(), hb.Size)));
        }

        public BasicTile()
        {

        }

        public int TileDistance(BasicTile other)
        {
            int deltaX = (int)(other.positionGrid.X - positionGrid.X);
            int deltaY = (int)(other.positionGrid.Y - positionGrid.Y);
            return (Math.Abs(deltaX) + Math.Abs(deltaY));
        }

        public BasicTile(int tsID, Point mapCoord)
        {
            this.tsID = tsID;
            this.positionGrid = mapCoord.ToVector2();
        }

        public bool Contains(Vector2 target)
        {

            if (hitboxes.Find(r => r.Contains(target)) != default(Rectangle))
            {
                return true;
            }


            return false;
        }

        public bool Contains(Rectangle re)
        {
            Rectangle r = new Rectangle(positionGrid.ToPoint().X * 64, positionGrid.ToPoint().Y * 64, 64, 64);
            return (r.Contains(re) || r.Intersects(re));
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameContentDataBase gcdb, float opacity = 1f, Color c = default(Color))
        {
            if (c == default(Color))
            {
                c = Color.White;
            }

            if (mapPosition == default(Rectangle) || (positionGrid != new Vector2(0, 0) && mapPosition.Location == new Point(0)))
            {
                reloadMapPosition();
            }
            gcdb.gameSourceTiles.Find(gst => gst.tileID == tsID).tileAnimation.DrawTile(spriteBatch, drawPosition, rotation, opacity, spriteEffect, c);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            tileSource.tileAnimation.DrawTile(spriteBatch, mapPosition, rotation, 1f, spriteEffect);
        }

        public virtual void DrawOnDemand(SpriteBatch spriteBatch, Vector2 coord, Color c)
        {
            var r = new Rectangle((int)(coord.X * 64) + offset.X, (int)(coord.Y * 64) + offset.Y, drawSize.X, drawSize.Y);
            tileSource.tileAnimation.DrawTile(spriteBatch, r, rotation,1f, spriteEffect, c);
        }

        public override string ToString()
        {
            return positionGrid.ToPoint().ToString() + " tID: " + tsID;
        }

        public int distanceCoordsFrom(Vector2 v)
        {
            Point p = (v / 64).ToPoint();
            Point p2 = positionGrid.ToPoint();
            int distance = (Math.Abs(p2.X - p.X) + Math.Abs(p2.Y - p.Y));
            return distance;
        }

        public BasicTile Clone()
        {
            var temp = (BasicTile)MemberwiseClone();
            temp.offset = offset;
            temp.tileSource = MapBuilder.gcDB.gameSourceTiles.Find(gts => gts.tileID == tsID);
            temp.hitboxes = new List<Rectangle>(hitboxes);
            return temp;
        }

        public BasicTile BClone()
        {
            var temp = (BasicTile)MemberwiseClone();
            temp.positionGrid = new Vector2(positionGrid.X, positionGrid.Y);
            temp.tileSource = MapBuilder.gcDB.gameSourceTiles.Find(gts => gts.tileID == tsID);
            temp.hitboxes = new List<Rectangle>();
            return temp;
        }

        public void BReload()
        {
            mapPosition = new Rectangle((int)positionGrid.X * 64, (int)positionGrid.Y * 64, 64, 64);
        }

        public bool HasDifferentSizeSource()
        {
            if (drawSize != tileSource.tileAnimation.AnimSourceSize() || drawSize != new Point(64))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// True pos
        /// </summary>
        /// <returns></returns>
        internal bool isPointDistanceFrom(Vector2 pos, Point offset)
        {
            Point temp = (pos / 64).ToPoint();
            if (positionGrid.ToPoint() == temp + offset)
            {
                return true;
            }
            return false;
        }
    }
}
