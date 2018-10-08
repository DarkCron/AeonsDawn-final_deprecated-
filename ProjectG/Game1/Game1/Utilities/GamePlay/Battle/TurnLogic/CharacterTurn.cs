using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    public class CharacterTurn
    {
        public BaseCharacter character;
        public List<TurnInfo> turns = new List<TurnInfo>();
        public List<List<BasicTile>> characterArea = new List<List<BasicTile>>();
        public bool bIsCompleted = false;
        internal bool bAttackedThisCT = false;
        internal LUA.LuaAbilityInfo abiUsedInfo = null;
        public int stepsSet = 0;
        internal TurnSet parentTS = null;
        internal LUA.LuaTurnSetInfo.SideTurnType sideTurnType = LUA.LuaTurnSetInfo.SideTurnType.Normal;
        internal bool bSideTurned = false;
        internal int characterAP = 0;

        public void Start()
        {
            bIsCompleted = false;
        }

        public CharacterTurn(BaseCharacter bs, MapZone zone, List<BasicTile> bt, TurnSet parent)
        {
            characterAP = bs.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP];
            parentTS = parent;
            character = bs;
            turns.Clear();
            characterArea.Clear();
            int temp = bs.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP];
            int maxMoves = bs.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB];
            for (int i = 1; i < temp + 1; i++)
            {
                turns.Add(new TurnInfo(maxMoves));
                //     characterArea.Add(MapListUtility.returnMapRadius((i - 1) * maxMoves, i * maxMoves, bt, bs));
                if (i > 1)
                {
                    for (int j = 1; j < 2; j++)
                    {
                        List<BasicTile> list = MapListUtility.returnValidMapRadius((i - 1) * maxMoves + maxMoves, bt, bs.position).Except(characterArea[i - 1 - j]).ToList();
                        characterArea.Add(list);
                    }

                }
                else
                {
                    characterArea.Add(MapListUtility.returnValidMapRadius((i - 1) * maxMoves + maxMoves, bt, bs.position));
                }

            }
        }

        public void ReGenerateTurn(List<BasicTile> bt)
        {
            bAttackedThisCT = false;
            abiUsedInfo = null;
            turns.Clear();
            characterArea.Clear();
            int temp = character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP];
            int maxMoves = character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB];
            for (int i = 1; i < temp + 1; i++)
            {
                turns.Add(new TurnInfo(maxMoves));
                if (i > 1)
                {
                    for (int j = 1; j < 2; j++)
                    {
                        List<BasicTile> list = MapListUtility.returnValidMapRadius((i - 1) * maxMoves + maxMoves, bt, character.position).Except(characterArea[i - 1 - j]).ToList();
                        characterArea.Add(list);
                    }
                }
                else
                {
                    characterArea.Add(MapListUtility.returnValidMapRadius((i - 1) * maxMoves + maxMoves, bt, character.position));
                }
            }



            try
            {
                var tempT = bt.Find(t => t.positionGrid.ToPoint() == (character.position / 64).ToPoint());
                if (tempT != null && !characterArea[0].Contains(tempT))
                {
                    characterArea[0].Add(tempT);
                }

                if (characterArea[0].Count == 0)
                {
                    characterArea[0].Add(bt.Find(t => t.mapPosition.Location.ToVector2() == character.position));
                }
            }
            catch
            {
            }

            bool bStuffToAdjust = false;
            List<KeyValuePair<BasicTile, int>> allUniqueTiles = new List<KeyValuePair<BasicTile, int>>();
            List<KeyValuePair<BasicTile, int>> duplicates = new List<KeyValuePair<BasicTile, int>>();
            for (int i = 0; i < characterArea.Count; i++)
            {
                for (int j = 0; j < characterArea[i].Count; j++)
                {
                    if (allUniqueTiles.Find(t => t.Key == characterArea[i][j]).Equals(default(KeyValuePair<BasicTile, int>)))
                    {
                        allUniqueTiles.Add(new KeyValuePair<BasicTile, int>(characterArea[i][j], i));
                    }else
                    {
                        bStuffToAdjust = true;
                        duplicates.Add(new KeyValuePair<BasicTile, int>(characterArea[i][j], i));
                    }
                  
                }
            }

            if (bStuffToAdjust)
            {
                for (int i = 0; i < duplicates.Count; i++)
                {
                    characterArea[duplicates[i].Value].Remove(duplicates[i].Key);
                }
            }

        }

        /// <summary>
        /// For enemy radius preview
        /// </summary>
        /// <param name="bt"></param>
        public void ReGenerateTurn2(List<BasicTile> bt)
        {
            bAttackedThisCT = false;
            abiUsedInfo = null;
            turns.Clear();
            characterArea.Clear();
            int temp = character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP];
            int maxMoves = character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB];
            for (int i = 1; i < temp + 1; i++)
            {
                turns.Add(new TurnInfo(maxMoves));
                if (i > 1)
                {
                    for (int j = 1; j < 2; j++)
                    {
                        List<BasicTile> list = MapListUtility.returnValidMapRadius2((i - 1) * maxMoves + maxMoves, bt, character.position).Except(characterArea[i - 1 - j]).ToList();
                        characterArea.Add(list);
                    }
                }
                else
                {
                    characterArea.Add(MapListUtility.returnValidMapRadius2((i - 1) * maxMoves + maxMoves, bt, character.position));
                }
            }



            try
            {
                var tempT = bt.Find(t => t.positionGrid.ToPoint() == (character.position / 64).ToPoint());
                if (tempT != null && !characterArea[0].Contains(tempT))
                {
                    characterArea[0].Add(tempT);
                }

                if (characterArea[0].Count == 0)
                {
                    characterArea[0].Add(bt.Find(t => t.mapPosition.Location.ToVector2() == character.position));
                }
            }
            catch
            {
            }

        }

        public void GenerateTurn(BaseCharacter bs, MapZone zone, List<BasicTile> bt)
        {
            bAttackedThisCT = false;
            abiUsedInfo = null;
            character = bs;
            turns.Clear();
            characterArea.Clear();
            int temp = bs.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP];
            int maxMoves = bs.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB];
            for (int i = 1; i < temp; i++)
            {
                turns.Add(new TurnInfo(maxMoves));
                characterArea.Add(MapListUtility.returnMapRadius((i - 1) * maxMoves, i * maxMoves, bt, bs));
            }
        }

        /// <summary>
        /// Can, well should, only be used for player selected turns
        /// </summary>
        public List<List<BasicTile>> SoftGenerateTurn(List<BasicTile> bt, Vector2 charPos)
        {
            List<List<BasicTile>> temp = new List<List<BasicTile>>();

            int AP = character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP];
            int maxMoves = character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB];
            int currentMoved = turns[0].movesTaken;
            temp.Add(MapListUtility.returnMapRadius(maxMoves - currentMoved, maxMoves, bt, charPos));
            for (int i = 2; i < AP; i++)
            {
                temp.Add(MapListUtility.returnMapRadius((i - 1) * maxMoves, i * maxMoves, bt, charPos));
            }

            return temp;
        }

        public List<BasicTile> returnCompleteArea()
        {
            List<BasicTile> completeArea = new List<BasicTile>();
            foreach (var item in characterArea)
            {
                completeArea.AddRange(item);
            }
            return completeArea;
        }

        public void Reset()
        {
            bIsCompleted = false;
            turns.Clear();
            characterArea.Clear();
        }

        public void EndTurn()
        {
            foreach (var item in turns)
            {
                item.bEnded = true;
            }
        }

        internal LUA.LuaCharacterTurnInfo toLuaCharacterTurnInfo()
        {
            LUA.LuaCharacterTurnInfo lcti = new LUA.LuaCharacterTurnInfo();
            lcti.charInfo = character.toCharInfo();
            lcti.parent = this;
            lcti.bUsedAbi = bAttackedThisCT;
            if (lcti.bUsedAbi) { lcti.abiInfo = abiUsedInfo; }

            return lcti;
        }
    }
}

namespace LUA
{
    public class LuaCharacterTurnInfo
    {
        public bool bUsedAbi = false;
        public LuaAbilityInfo abiInfo = new LuaAbilityInfo();
        public LuaCharacterInfo charInfo = new LuaCharacterInfo();
        internal TBAGW.CharacterTurn parent;

        public LuaCharacterTurnInfo() { }
    }
}