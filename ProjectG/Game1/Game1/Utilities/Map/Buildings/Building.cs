using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Map;

namespace TBAGW
{
    [XmlRoot("Building")]
    public class Building
    {
        [XmlArrayItem("Building tiles")]
        public List<List<Vector2>> buildingTilesPositions = new List<List<Vector2>>();
        [XmlElement("Bounds")]
        public Rectangle boundingZone = new Rectangle();
        [XmlElement("Draw zone")]
        public Rectangle boundingDrawZone = new Rectangle();
        [XmlElement("Location")]
        public Vector2 location = new Vector2();
        [XmlElement("Render Location")]
        public Vector2 renderLocation = new Vector2();
        [XmlElement("Collision")]
        public bool bHasCollision = true;

        [XmlIgnore]
        public List<List<BasicTile>> buildingTiles = new List<List<BasicTile>>();
        [XmlIgnore]
        public RenderTarget2D buildingCompleteRender = null;
        [XmlIgnore]
        public RenderTarget2D buildingRender = null;
        [XmlIgnore]
        public bool reGenerate = true;
        RenderTarget2D shadowRenderRef;

        internal void AssignShadowRender(RenderTarget2D r2d)
        {
            if (shadowRenderRef != null && !shadowRenderRef.IsDisposed && shadowRenderRef != r2d)
            {
                shadowRenderRef.Dispose();
            }
            shadowRenderRef = r2d;
        }

        internal RenderTarget2D getShadowFX()
        {
            return shadowRenderRef;
        }

        public Building()
        {

        }

        public void Reload(BasicMap m, GameContentDataBase gcdb)
        {
            buildingCompleteRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, boundingZone.Height * 3, boundingZone.Height);
            buildingRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, boundingZone.Width, boundingZone.Height);


            int layer = 0;
            //location.Y *= -1;
            foreach (var item in buildingTilesPositions)
            {
                buildingTiles.Add(new List<BasicTile>());
                foreach (var position in item)
                {
                    // var chunk = m.testChunk[layer].Find(kv => kv.Key.Contains(position * 64 + location));
                    var chunk = m.Chunks.Find(c => c.region.Contains(position * 64 + location));
                    BasicTile tempTIle = chunk.returnLayer(layer).Find(tile => tile.positionGrid == position + location / 64);
                    tempTIle.Reload(gcdb);
                    tempTIle = tempTIle.BClone();
                    tempTIle.positionGrid = position;
                    tempTIle.BReload();
                    buildingTiles[layer].Add(tempTIle);
                }
                layer++;
            }
        }

        public void Generate()
        {
            if (buildingTiles.Count == 0)
            {
                goto stop;
            }

            float maxX = 0;
            float maxY = 0;

            int layerIndex = 0;
            foreach (var layer in buildingTiles)
            {
                float tempMaxX = layer.Max(tile => tile.positionGrid.X);
                float tempMaxY = layer.Max(tile => tile.positionGrid.Y);

                if (maxX < tempMaxX)
                {
                    maxX = tempMaxX;
                }


                if (maxY < tempMaxY)
                {
                    maxY = tempMaxY;
                }

                buildingTilesPositions.Add(new List<Vector2>());
                foreach (var item in layer)
                {
                    item.BReload();
                    buildingTilesPositions[layerIndex].Add(item.positionGrid);
                }
                layerIndex++;
            }

            maxX++;
            maxY++;

            boundingZone = new Rectangle((int)location.X, (int)location.Y, (int)maxX * 64, (int)maxY * 64);
            boundingDrawZone = new Rectangle(boundingZone.Height, 0, boundingZone.Width, boundingZone.Height);

            buildingCompleteRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, boundingZone.Height * 3, boundingZone.Height);
            buildingRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, boundingZone.Width, boundingZone.Height);


        stop: { }
        }

        public void Update(GameTime gt)
        {

        }

        public bool Contains(Rectangle camera, float zoom = 1)
        {
            Rectangle trueCamera = new Rectangle((int)(camera.X) - (int)(500 / zoom), (int)(camera.Y) - (int)(500 / zoom), (int)(1366 / zoom) + (int)(500 / zoom), (int)(768 / zoom) + (int)(500 / zoom));

            if (trueCamera.Contains(boundingZone) || trueCamera.Intersects(boundingZone))
            {
                return true;
            }


            return false;
        }

        public bool IsIn(Rectangle region)
        {
            if (region.Contains(boundingZone) || region.Intersects(boundingZone))
            {
                return true;
            }


            return false;
        }

        public RenderTarget2D Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(buildingRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            foreach (var layer in buildingTiles)
            {
                foreach (var tile in layer)
                {
                    tile.Draw(sb);
                }
            }
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(buildingCompleteRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            sb.Draw(buildingRender, boundingDrawZone, buildingRender.Bounds, Color.White);
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            reGenerate = false;
            return buildingCompleteRender;
        }

        internal int heightIndicator()
        {
            return boundingZone.Height + boundingZone.Y;
        }

        internal Rectangle MapSize()
        {
            return boundingZone;
        }
    }
}
