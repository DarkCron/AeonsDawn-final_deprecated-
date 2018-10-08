using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    [XmlRoot("object group")]
    public class ObjectGroup
    {
        [XmlArrayItem("Items in group IDs")]
        public List<int> groupItemsIDs = new List<int>();
        //[XmlArrayItem("Items in group")]
        //public List<BaseSprite> groupItems = new List<BaseSprite>();
        [XmlElement("Group Name")]
        public String groupName = "";
        [XmlElement("Group Map Name")]
        public String groupMapName = "";
        [XmlElement("Group ID")]
        public int groupID = -1;
        [XmlArrayItem("relative positions")]
        public List<Vector2> relativeOffSet = new List<Vector2>();
        [XmlElement("Object group size")]
        public Point groupSize = new Point();
        [XmlElement("Scale")]
        public Vector2 scaleVector = new Vector2(1);
        [XmlElement("Is visible")]
        public bool bIsVisible = true;
        [XmlElement("Collision")]
        public bool bHasCollision = false;
        [XmlElement("position")]
        public Vector2 position = Vector2.Zero;
        [XmlElement("Water reflection")]
        public bool bNeedsWaterReflection = false;
        [XmlIgnore]
        public int heightIndicator;
        [XmlIgnore]
        public Rectangle trueMapSize = new Rectangle();
        [XmlIgnore]
        public List<BaseSprite> groupItems = new List<BaseSprite>();
        [XmlIgnore]
        public bool bGenerateRender = true;

        RenderTarget2D fxRender;
        RenderTarget2D shadowRender;
        Rectangle fxDrawLoc = new Rectangle();
        bool bHasMoved = true;
        bool bUpdateHitboxes = true;

        internal void AssignShadowRender(RenderTarget2D r2d)
        {
            if (shadowRender != null && !shadowRender.IsDisposed)
            {
                shadowRender.Dispose();
            }
            shadowRender = r2d;
        }

        internal RenderTarget2D getShadowFX()
        {
            return shadowRender;
        }

        public ObjectGroup()
        {

        }

        public BaseSprite mainController()
        {
            return groupItems[0];
        }

        public void Move(Vector2 move, BaseSprite baseSprite)
        {
            baseSprite.position += move;
            PlaceGroup(baseSprite.position);
            bHasMoved = true;
            bUpdateHitboxes = true;
        }

        public void Merge(ref ObjectGroup objg)
        {
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure you want to add both groups as-is? The groups relative locations will be saved as they are now. The group you're copying from will disappear forever (reverb).", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                groupItems.AddRange(objg.groupItems);
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                //do something else
            }

        }

        public void AddObject(BaseSprite bs)
        {
            if (!groupItems.Contains(bs))
            {
                if (groupItems.Count == 0)
                {
                    groupItems.Add(bs);
                    groupItemsIDs.Add(bs.shapeID);
                    relativeOffSet.Add(new Vector2(0, 0));
                }
                else
                {
                    groupItems.Add(bs);
                    groupItemsIDs.Add(bs.shapeID);
                    relativeOffSet.Add((bs.position - groupItems[0].position));
                }
            }
            else
            {
                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Seems like the current selected object group already contains that item, do you still wish to add it?", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    if (groupItems.Count == 0)
                    {
                        groupItems.Add(bs);
                        groupItemsIDs.Add(bs.shapeID);
                        relativeOffSet.Add(new Vector2(0, 0));
                    }
                    else
                    {
                        groupItems.Add(bs);
                        groupItemsIDs.Add(bs.shapeID);
                        relativeOffSet.Add((bs.position - groupItems[0].position));
                    }
                }
                else if (dialogResult == System.Windows.Forms.DialogResult.No)
                {
                }
            }
        }

        public void RemoveObject(BaseSprite bs)
        {
            if (groupItems.Contains(bs))
            {
                int index = groupItems.IndexOf(bs);
                groupItems.RemoveAt(index);
                relativeOffSet.RemoveAt(index);
            }

        }

        public void Reload(GameContentDataBase gcDB)
        {
            //if (groupItemsIDs.Count != groupItems.Count)
            //{
            //    groupItemsIDs.Clear();
            //    groupItems.ForEach(obj => groupItemsIDs.Add(obj.shapeID));
            //}

            //List<Vector2> positions = new List<Vector2>();
            //groupItems.ForEach(obj => positions.Add(obj.position));

            ////foreach (var item in groupItems)
            ////{
            ////    item.ReloadTextures();
            ////}
            //groupItems.Clear();
            //for (int i = 0; i < groupItemsIDs.Count; i++)
            //{
            //    groupItems.Add(gcDB.gameObjectObjects.Find(obj => obj.shapeID == groupItemsIDs[i]).ShallowCopy());
            //    groupItems[i].position = positions[i];
            //    groupItems[i].UpdatePositioNonMovement();
            //}
            groupItems.Clear();
            foreach (var item in groupItemsIDs)
            {
                groupItems.Add(gcDB.gameObjectObjects.Find(obj => obj.shapeID == item).ShallowCopy());
            }

        }

        public void PlaceGroupFromReload(Vector2 loc)
        {
            foreach (var item in groupItems)
            {
                item.position = loc + relativeOffSet[groupItems.IndexOf(item)];
                item.UpdatePositioNonMovement();
            }
        }

        public void PlaceGroup(Vector2 loc)
        {
            foreach (var item in groupItems)
            {
                item.position = loc + relativeOffSet[groupItems.IndexOf(item)];
                item.UpdatePosition();
            }
            bGenerateRender = true;
        }

        public void CalculateGroupSize()
        {
            GenerateBoundingRectangleForMapSize();

            //int width = groupItems[0].trueMapSize().Width + Math.Abs((int)relativeOffSet[0].X);
            //int height = groupItems[0].trueMapSize().Height + Math.Abs((int)relativeOffSet[0].Y);

            //for (int i = 1; i < groupItems.Count; i++)
            //{
            //    int widthTemp = groupItems[i].trueMapSize().Width + Math.Abs((int)relativeOffSet[i].X);
            //    int heightTemp = groupItems[i].trueMapSize().Height + Math.Abs((int)relativeOffSet[i].Y);
            //    if (widthTemp > width)
            //    {
            //        width = widthTemp;
            //    }
            //    if (heightTemp > height)
            //    {
            //        height = heightTemp;
            //    }
            //}

            //groupSize = new Point(width, height);
        }

        public void Update(GameTime gt)
        {
            if (bHasMoved)
            {
                GenerateTrueMapSize();
                CalculateHeightIndicator();
                bHasMoved = false;
            }

            if (groupSize == new Point() && groupItems.Count != 0)
            {
                CalculateGroupSize();
            }

            if(bUpdateHitboxes)
            {
                for (int i = 0; i < groupItems.Count; i++)
                {
                    groupItems[i].AssignScaleVectorGroupItems(this);
                }
                bUpdateHitboxes = false;
            }
          

            foreach (var item in groupItems)
            {
                item.Update(gt);
            }
        }

        public void MinimalUpdate(GameTime gt)
        {
            groupItems.Where(gi => gi.bMustUpdateHitBoxes).ToList().ForEach(gi => gi.AssignScaleVectorGroupItems(this));

            foreach (var item in groupItems)
            {
                item.MinimalUpdate(gt);
            }
        }

        public void Draw(SpriteBatch sb, Color sColor = new Color(), int index = -1)
        {
            if (bIsVisible && fxRender != null)
            {
                if (groupSize.X == 213)
                { }
                if (sColor == default(Color))
                {
                    sColor = Color.White;
                }
                Rectangle r = new Rectangle(fxDrawLoc.Location, (fxDrawLoc.Size.ToVector2() * scaleVector).ToPoint());
                sb.Draw(fxRender, r, new Rectangle(groupSize.Y, 0, groupSize.X, groupSize.Y), sColor);
            }
            else if (fxRender == null)
            {
            }
        }

        public void DrawAtOrigin(SpriteBatch sb, Color sColor = new Color(), int index = -1)
        {
            if (sColor == default(Color))
            {
                sColor = Color.White;
            }
            //Rectangle r = new Rectangle(Point.Zero,fxDrawLoc.Size);

            if (fxRender != null)
            {
                Rectangle r = new Rectangle(Point.Zero, (getRendeSize().ToVector2() * 0.5f).ToPoint());
                sb.Draw(fxRender, r, new Rectangle(groupSize.Y, 0, groupSize.X, groupSize.Y), sColor);
            }

        }

        public void DrawAtOrigin2(SpriteBatch sb, Color sColor = new Color(), int index = -1)
        {
            if (bIsVisible)
            {
                if (sColor == default(Color))
                {
                    sColor = Color.White;
                }
                Rectangle r = new Rectangle(Point.Zero, (fxDrawLoc.Size.ToVector2() * scaleVector).ToPoint());
                sb.Draw(fxRender, r, new Rectangle(groupSize.Y, 0, groupSize.X, groupSize.Y), sColor);
            }
        }

        public bool Contains(ObjectGroup objg)
        {
            foreach (var item in objg.groupItems)
            {
                var temp = groupItems.Find(x => x.bHasCollision && (x.spriteGameSize.Intersects(item.spriteGameSize) || x.spriteGameSize.Contains(item.spriteGameSize)));
                if (temp != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Contains(BaseSprite bs)
        {
            var temp = groupItems.Find(x => x.bHasCollision && (x.spriteGameSize.Intersects(bs.spriteGameSize) || x.spriteGameSize.Contains(bs.spriteGameSize)));
            if (temp != null)
            {
                return true;
            }
            return false;
        }

        public bool Contains(Rectangle r)
        {
            var temp = groupItems.Find(x => (x.spriteGameSize.Intersects(r) || x.spriteGameSize.Contains(r)));
            if (temp != null)
            {
                return true;
            }
            return false;
        }

        public bool Contains(Vector2 r)
        {
            var temp = groupItems.Find(x => x.trueMapSize().Contains(r));
            if (temp != null)
            {
                return true;
            }
            return false;
        }

        public bool ContainsInCamera(Rectangle r)
        {
            var temp = groupItems.Find(x => (x.spriteGameSize.Intersects(r) || x.spriteGameSize.Contains(r)));
            if (temp != null)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            String n = "";
            if (!groupMapName.Equals(""))
            {
                n += "Map name: " + groupMapName + ", ";
            }
            n += groupName + "  ID: " + groupID;
            return n;
        }

        internal ObjectGroup ShallowCopy()
        {
            ObjectGroup temp = new ObjectGroup();
            temp.groupID = groupID;
            temp.groupName = groupName;
            foreach (var item in groupItems)
            {
                temp.groupItems.Add(item.ShallowCopy());

            }
            temp.groupItemsIDs = new List<int>(groupItemsIDs);

            temp.relativeOffSet = new List<Vector2>(relativeOffSet);
            return temp;
        }

        internal void UpdateMovement()
        {
            groupItems.ForEach(i => i.UpdatePosition());
        }

        public void GenerateRender(SpriteBatch spriteBatch, Color sColor = new Color(), int index = -1)
        {
            spriteBatch.End();
            if (fxRender == null)
            {
                if (groupSize == new Point() && groupItems.Count != 0)
                {
                    CalculateGroupSize();
                }
                fxRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, groupSize.Y * 3, groupSize.Y);
            }
            else
            {
                CalculateGroupSize();
                fxRender.Dispose();
                fxRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, groupSize.Y * 3, groupSize.Y);
            }

            List<BaseSprite> tempList = new List<BaseSprite>();
            groupItems.ForEach(gi => tempList.Add(gi.ShallowCopy()));

            spriteBatch.GraphicsDevice.SetRenderTarget(fxRender);
            spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            int i = 0;
            var offSet = new Point((int)relativeOffSet.Min(v => v.X), (int)relativeOffSet.Min(v => v.Y));
            foreach (var item in tempList)
            {
                item.position = relativeOffSet[i] + new Vector2(groupSize.Y, 0) - offSet.ToVector2();
                item.UpdatePositioNonMovement();
                i++;

                item.DrawFX(spriteBatch, sColor, index);
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);

            bGenerateRender = false;
            fxDrawLoc = new Rectangle(mainController().position.ToPoint() + new Point((int)relativeOffSet.Min(v => v.X), (int)relativeOffSet.Min(v => v.Y)), groupSize);
            //fxDrawLoc = new Rectangle(mainController().position.ToPoint(), groupSize);
            //bool test = Utilities.Map.MapChunk.consideredSprites.FindAll(cs => cs.objectType == Utilities.Map.BasicMap.objectInfo.type.ObjectGroup).Select(cs => cs.obj).Cast<ObjectGroup>().ToList().Contains(this);
        }

        public void CalculateFxDrawLocBeforeFirstDraw()
        {
            List<BaseSprite> tempList = new List<BaseSprite>();
            groupItems.ForEach(gi => tempList.Add(gi.ShallowCopy()));
            int i = 0;
            var offSet = new Point((int)relativeOffSet.Min(v => v.X), (int)relativeOffSet.Min(v => v.Y));
            foreach (var item in tempList)
            {
                item.position = relativeOffSet[i] + new Vector2(groupSize.Y, 0) - offSet.ToVector2();
                item.UpdatePositioNonMovement();
                i++;
            }
            fxDrawLoc = new Rectangle(mainController().position.ToPoint() + new Point((int)relativeOffSet.Min(v => v.X), (int)relativeOffSet.Min(v => v.Y)), groupSize);
        }

        public void CalculateHeightIndicator()
        {
            heightIndicator = (int)(fxDrawLoc.Y + (fxDrawLoc.Height * scaleVector.Y));
        }

        public Point getRendeSize()
        {
            if (fxRender == null)
            {
                return new Point(64);
            }
            return fxRender.Bounds.Size;
        }

        public RenderTarget2D getRender()
        {
            return fxRender;
        }

        public Rectangle getFXRenderDawLoc()
        {
            return fxDrawLoc;
        }

        public void GenerateTrueMapSize()
        {
            trueMapSize = new Rectangle(fxDrawLoc.Location, (fxDrawLoc.Size.ToVector2() * scaleVector).ToPoint());
        }

        internal void AssignScaleVector(Vector2 scaleVector)
        {
            this.scaleVector = scaleVector;
            groupItems.ForEach(i => i.AssignScaleVectorGroupItems(this));
        }

        internal void GenerateBoundingRectangleForMapSize()
        {
            Rectangle br = groupItems[0].trueMapSize();

            for (int i = 0; i < groupItems.Count; i++)
            {
                br = Rectangle.Union(br, groupItems[i].trueMapSize());
            }

            fxDrawLoc = br;
            groupSize = new Point(br.Width, br.Height);
            bGenerateRender = true;
        }
    }
}
