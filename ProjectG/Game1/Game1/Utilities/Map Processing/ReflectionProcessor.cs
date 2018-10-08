using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    internal static class ReflectionProcessor
    {
        static int count = 0;

        internal static void ProcessReflection(BasicMap map)
        {
            count = 0;
            #region Prepare chunks
            foreach (var chunk in map.Chunks)
            {
                map.CheckChunksToConsider(chunk.region);
            }

            Console.WriteLine("Chunks correctly prepared for logic.");
            #endregion

            List<BasicTile> waterTiles = new List<BasicTile>();
            foreach (var chunk in map.Chunks)
            {
                chunk.waterTiles.Clear();
                foreach (var layer in chunk.lbt)
                {
                    chunk.waterTiles.Add(new List<BasicTile>());
                    if (layer.Find(t => t.tileSource.tileType == TileSource.TileType.Fluid) != null)
                    {
                        foreach (var tile in layer.FindAll(t => t.tileSource.tileType == TileSource.TileType.Fluid))
                        {
                            waterTiles.Add(tile);
                            chunk.waterTiles.Last().Add(tile);
                        }
                    }
                }
            }

            foreach (var chunk in map.Chunks)
            {
                if (waterTiles.Count != 0)
                {
                    waterTiles = waterTiles.Distinct().ToList();

                    foreach (var obj in chunk.objectsInChunk)
                    {
                        Rectangle reflectionPosition;

                        if (obj is BaseSprite)
                        {
                            reflectionPosition = (obj as BaseSprite).trueMapSize();
                            reflectionPosition.Location = reflectionPosition.Location + new Point(0, reflectionPosition.Height);

                            if (CheckReflection(reflectionPosition, waterTiles))
                            {
                                (obj as BaseSprite).bNeedsWaterReflection = true;
                            }
                        }
                        else if (obj is ObjectGroup)
                        {
                            (obj as ObjectGroup).CalculateFxDrawLocBeforeFirstDraw();
                            reflectionPosition = (obj as ObjectGroup).trueMapSize;
                            reflectionPosition.Location = reflectionPosition.Location + new Point(0, reflectionPosition.Height);

                            if (CheckReflection(reflectionPosition, waterTiles))
                            {
                                (obj as ObjectGroup).bNeedsWaterReflection = true;
                            }
                        }
                    }
                }
            }

            

            Console.WriteLine("Found "+count + " Objects for water reflection.");
        }

        private static bool CheckReflection(Rectangle reflectionPosition, List<BasicTile> waterTiles)
        {
            foreach (var tile in waterTiles)
            {
                if (reflectionPosition.Contains(tile.mapPosition)||reflectionPosition.Intersects(tile.mapPosition))
                {
                    count++;
                    return true;
                }
            }

            return false;
        }
    }
}
