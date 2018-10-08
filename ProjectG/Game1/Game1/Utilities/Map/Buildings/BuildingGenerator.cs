using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Map;

namespace TBAGW
{
    public static class BuildingGenerator
    {

        public static List<Building> Generate(BasicMap bm)
        {
            var test = AnyBuildingTilesLeft(bm, new List<BasicTile>());

            bool bHasBuildingTile = test.Key;
            if (!test.Key)
            {
                return new List<Building>();
            }

            List<Building> final = new List<Building>();

            List<BasicTile> allBuildingTiles = FindBuildingTiles(bm);
            List<List<BasicTile>> groupedTiles = new List<List<BasicTile>>();
            if (allBuildingTiles.Count > 1)
            {
                groupedTiles = GroupProximityTiles(allBuildingTiles);
            }
            else
            {
                groupedTiles.Add(new List<BasicTile>());
                groupedTiles[0].Add(allBuildingTiles[0]);
            }

            int index = 0;
            foreach (var group in groupedTiles)
            {
                final.Add(new Building());
                float firstX = group.Min(t => t.positionGrid.X);
                float firstY = group.Min(t => t.positionGrid.Y);
                Vector2 firstPos = new Vector2(firstX, firstY);
                final[index].location = firstPos * 64;
                var temp = group.OrderBy(t => t.tileLayer).Select(tile => tile.tileLayer).Distinct();
                int layer = 0;
                foreach (var item in temp)
                {
                    final[index].buildingTiles.Add(new List<BasicTile>());
                    foreach (var t in group.FindAll(tile => tile.tileLayer == item))
                    {
                        BasicTile tempTIle = t.BClone();
                        tempTIle.positionGrid -= firstPos;
                        final[index].buildingTiles[layer].Add(tempTIle);
                    }

                    layer++;
                }
                index++;
            }

            final.ForEach(b => b.Generate());

            return final;
        }

        private static List<List<BasicTile>> GroupProximityTiles(List<BasicTile> allBuildingTiles)
        {
            List<List<BasicTile>> tempList = new List<List<BasicTile>>();

            int tilesLeftToCheck = allBuildingTiles.Count;
            int groupIndex = 0;

            while (allBuildingTiles.Count != 0)
            {
                tempList.Add(new List<BasicTile>());
                List<BasicTile> handledTiles = new List<BasicTile>();
                BasicTile selectedTile = allBuildingTiles[0];
                handledTiles.Add(selectedTile);
                tempList[groupIndex].Add(selectedTile);
                allBuildingTiles.Remove(selectedTile);
                if (allBuildingTiles.Count != 0)
                {
                    var temp = allBuildingTiles.FindAll(bt => bt.TileDistance(selectedTile) <= 1);
                    tempList[groupIndex].AddRange(temp);
                    foreach (var item in temp)
                    {
                        allBuildingTiles.Remove(item);
                    }
                }
                if (allBuildingTiles.Count != 0)
                {
                    while (handledTiles.Count != tempList[groupIndex].Count)
                    {
                        selectedTile = tempList[groupIndex].Except(handledTiles).First();
                        handledTiles.Add(selectedTile);

                        if (allBuildingTiles.Count != 0)
                        {
                            var temp = allBuildingTiles.FindAll(bt => bt.TileDistance(selectedTile) <= 1);
                            tempList[groupIndex].AddRange(temp);
                            foreach (var item in temp)
                            {
                                allBuildingTiles.Remove(item);
                            }
                        }
                    }
                }

                groupIndex++;
            }

            return tempList;
        }

        private static List<BasicTile> FindBuildingTiles(BasicMap bm)
        {
            List<BasicTile> tempList = new List<BasicTile>();
            foreach (var chunk in bm.Chunks)
            {
                foreach (var layer in chunk.lbt)
                {
                    var temp = layer.FindAll(tile => tile.tileSource.tileType == TileSource.TileType.Building);
                    if (temp.Count != 0)
                    {
                        tempList.AddRange(temp);
                    }
                }
            }

            return tempList;
        }

        static KeyValuePair<bool, BasicTile> AnyBuildingTilesLeft(BasicMap bm, List<BasicTile> exceptions)
        {
            foreach (var chunk in bm.Chunks)
            {
                foreach (var layer in chunk.lbt)
                {
                    var temp2 = layer.Except(exceptions).ToList().FindAll(tile => tile.tsID == 14);
                    var temp = layer.Except(exceptions).ToList().Find(tile => tile.tileSource.tileType == TileSource.TileType.Building);
                    if (temp != default(BasicTile))
                    {
                        return new KeyValuePair<bool, BasicTile>(true, temp);
                    }
                }
            }

            return new KeyValuePair<bool, BasicTile>(false, null);
        }


    }
}
