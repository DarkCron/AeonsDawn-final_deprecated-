using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Sprite;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBAGW
{
    [XmlRoot("Pick Up Entity")]
    public class PickUpEntity
    {
        [XmlArrayItem("Item IDs")]
        public List<int> itemIDs = new List<int>();
        [XmlElement("Map Location")]
        public Vector2 location = Vector2.Zero;

        [XmlIgnore]
        Rectangle mapLoc = new Rectangle();
        [XmlIgnore]
        public List<BaseItem> itemList = new List<BaseItem>();
        [XmlIgnore]
        public List<BaseSprite> sprites = new List<BaseSprite>();

        public PickUpEntity()
        {

        }

        internal PickUpEntity(List<BaseItem> lbi, Point loc)
        {
            location = loc.ToVector2() * 64;

            itemList = new List<BaseItem>(lbi);

            for (int i = 0; i < lbi.Count; i++)
            {
                itemIDs.Add(lbi[i].itemID);
                sprites.Add(new BaseSprite());
                sprites[i].bHasCollision = false;
                sprites[i].bHasShadow = false;
                sprites[i].baseAnimations.Add(lbi[i].itemTexAndAnimation);
            }

            if (lbi.Count > 3)
            {
                sprites[0].spriteGameSize = new Rectangle((int)location.X, (int)location.Y, 32, 32);
                sprites[1].spriteGameSize = new Rectangle((int)location.X + 32, (int)location.Y, 32, 32);
                sprites[2].spriteGameSize = new Rectangle((int)location.X, (int)location.Y + 32, 32, 32);
                sprites[3].spriteGameSize = new Rectangle((int)location.X + 32, (int)location.Y + 32, 32, 32);
            }
            else if (lbi.Count == 3)
            {
                sprites[0].spriteGameSize = new Rectangle((int)location.X, (int)location.Y, 32, 32);
                sprites[1].spriteGameSize = new Rectangle((int)location.X + 32, (int)location.Y, 32, 32);
                sprites[2].spriteGameSize = new Rectangle((int)location.X + 16, (int)location.Y + 32, 32, 32);
            }
            else if (lbi.Count == 2)
            {
                sprites[0].spriteGameSize = new Rectangle((int)location.X, (int)location.Y + 16, 32, 32);
                sprites[1].spriteGameSize = new Rectangle((int)location.X + 32, (int)location.Y + 16, 32, 32);
            }
            else if (lbi.Count == 1)
            {
                sprites[0].spriteGameSize = new Rectangle((int)location.X + 6, (int)location.Y + 6, 48, 48);
            }

        }

        internal virtual void Update(GameTime gt)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].baseAnimations[0].UpdateAnimationForItems(gt);
            }
        }

        internal virtual void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Draw(sb);
            }
        }

        internal bool CanPickUp(BaseSprite bs)
        {
            if(mapLoc == default(Rectangle) ){ mapLoc = new Rectangle(location.ToPoint(),new Point(64)); }
            if(bs.trueMapSize().Contains(mapLoc)|| bs.trueMapSize().Intersects(mapLoc)) { return true; }
            return false;
        }

        internal bool isInRegion(List<Rectangle> lrs)
        {
            if (mapLoc == default(Rectangle)) { mapLoc = new Rectangle(location.ToPoint(), new Point(64)); }
            for (int i = 0; i < lrs.Count; i++)
            {
                if(lrs[i].Contains(mapLoc)||lrs[i].Intersects(mapLoc))
                {
                    return true;
                }
            }
            return false;
        }

        internal void RemoveItemFromList(BaseItem bi)
        {
            int index = itemList.IndexOf(bi);
            itemList.RemoveAt(index);
            sprites.RemoveAt(index);
            GenerateDisplay();
        }

        internal void GenerateDisplay()
        {
            if (sprites.Count > 3)
            {
                sprites[0].spriteGameSize = new Rectangle((int)location.X, (int)location.Y, 32, 32);
                sprites[1].spriteGameSize = new Rectangle((int)location.X + 32, (int)location.Y, 32, 32);
                sprites[0].spriteGameSize = new Rectangle((int)location.X, (int)location.Y + 32, 32, 32);
                sprites[0].spriteGameSize = new Rectangle((int)location.X + 32, (int)location.Y + 32, 32, 32);
            }
            else if (sprites.Count == 3)
            {
                sprites[0].spriteGameSize = new Rectangle((int)location.X, (int)location.Y, 32, 32);
                sprites[1].spriteGameSize = new Rectangle((int)location.X + 32, (int)location.Y, 32, 32);
                sprites[2].spriteGameSize = new Rectangle((int)location.X + 16, (int)location.Y + 32, 32, 32);
            }
            else if (sprites.Count == 2)
            {
                sprites[0].spriteGameSize = new Rectangle((int)location.X, (int)location.Y + 16, 32, 32);
                sprites[1].spriteGameSize = new Rectangle((int)location.X + 32, (int)location.Y + 16, 32, 32);
            }
            else if (sprites.Count == 1)
            {
                sprites[0].spriteGameSize = new Rectangle((int)location.X + 6, (int)location.Y + 6, 48, 48);
            }
        }
    }
}
