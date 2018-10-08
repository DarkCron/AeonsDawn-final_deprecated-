using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;
using TBAGW.Utilities.Control.Player;

namespace TBAGW
{
    public static class LootScreen
    {
        static RenderTarget2D lootScreenRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static bool bIsRunning = false;
        static List<BaseItem> loot = new List<BaseItem>();
        static int amountOfScreens = 1;
        static int currentScreen = 0;
        static bool bHandleNextScreen = false;
        static List<lootDisplay> lootToDisplay = new List<lootDisplay>();
        static int amountOfNonRares;
        static int amountOfRares;
        static bool bShowingNormals = true;
        static bool bAwaitingKeyPress = false;

        static float lootDisplayTimer = 1.0f * 60;
        static int lootDisplayFramesPassed = 0;
        static int itemsPerScreen = 4;
        static int regionLvl = 0;
        static bool bInitializeVictoryAnim = false;

        public static void Start(int regionLevel = 0)
        {
            bInitializeVictoryAnim = true;
            regionLvl = regionLevel;
            bAwaitingKeyPress = false;
            lootDisplayFramesPassed = 0;
            bShowingNormals = true;
            lootToDisplay.Clear();
            bHandleNextScreen = true;
            amountOfScreens = 1;
            currentScreen = 0;
            loot = LootGenerator.GenerateLoot(regionLevel);
            var temp = loot.FindAll(l => !l.bMarkedAsRare);
            temp.AddRange(loot.FindAll(l => l.bMarkedAsRare));
            loot = temp;
            bIsRunning = true;
            GenerateLogic();
            lootDisplay.GenerateOffSets(4, 96);
            if (AddMoreDisplayLoot())
            {
                AddDisplay();
            }
        }

        private static void GenerateLogic()
        {
            amountOfNonRares = loot.FindAll(l => !l.bMarkedAsRare).Count;
            amountOfRares = loot.FindAll(l => l.bMarkedAsRare).Count;

            int sum = 0;
            foreach (var item in loot)
            {
                sum += item.itemAmount;
            }

            if (sum > 48)
            {

            }

            // bShowingNormals = amountOfNonRares > 1 ? true : false;

            amountOfScreens = (amountOfNonRares / itemsPerScreen + 1) + (amountOfRares / itemsPerScreen + 1);
        }

        public static void Update(GameTime gt)
        {
            if (AddMoreDisplayLoot())
            {
                lootDisplayFramesPassed++;
                if (lootDisplayFramesPassed > lootDisplayTimer)
                {
                    AddDisplay();
                }
            }
            else if (!AddMoreDisplayLoot())
            {
                bAwaitingKeyPress = true;

            }

            foreach (var item in lootToDisplay)
            {
                item.Update(gt);
            }
        }

        public static void AddDisplay()
        {
            int amountShown = currentScreen * itemsPerScreen + lootToDisplay.Count;
            lootToDisplay.Add(new lootDisplay(loot[amountShown], lootToDisplay.Count));
            lootDisplayFramesPassed = 0;
        }

        static bool AddMoreDisplayLoot()
        {
            if (bShowingNormals)
            {
                int amountShown = currentScreen * itemsPerScreen + lootToDisplay.Count;
                if (amountShown == amountOfNonRares)
                {
                    return false;
                }
            }
            else
            {
                int amountShown = currentScreen * itemsPerScreen + lootToDisplay.Count - amountOfNonRares;
                if (amountShown == amountOfRares || amountOfRares == 0)
                {
                    return false;
                }
            }

            if (lootToDisplay.Count < itemsPerScreen)
            {
                return true;
            }
            return false;
        }

        public static void Stop()
        {
            bIsRunning = false;
            foreach (var item in loot)
            {
                InventoryManager.AddItemToInventory(item);
            }
            PlayerSaveData.playerInventory.ManageStackableItems();

            ExpGainScreen.Start(LootGenerator.GenerateExp(regionLvl));
            CombatProcessor.currentAfterBattleScreen = CombatProcessor.afterBattleScreen.Exp;
            PlayerController.currentController = PlayerController.Controllers.EXPGainScreen;
        }

        public static void HandleConfirmPress()
        {
            bAwaitingKeyPress = false;


            if (bShowingNormals)
            {
                int amountShown = currentScreen * itemsPerScreen + lootToDisplay.Count;
                if (amountShown >= amountOfNonRares)
                {
                    bShowingNormals = false;
                }else
                {
                    currentScreen++;
                }

                if (AddMoreDisplayLoot())
                {
                    AddDisplay();
                }
            }

            if (!bShowingNormals)
            {
                int amountShown = currentScreen * itemsPerScreen + lootToDisplay.Count - amountOfNonRares;
                if (amountShown != amountOfRares && amountOfRares != 0)
                {
                    if (AddMoreDisplayLoot())
                    {
                        AddDisplay();
                    }

                }
                else
                {
                    Stop();
                }
            }

            lootToDisplay.Clear();
        }

        public static bool IsRunning()
        {
            return bIsRunning;
        }

        public static bool WaitingForKeyPress()
        {
            return bAwaitingKeyPress;
        }

        public static RenderTarget2D Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(lootScreenRender);
            sb.GraphicsDevice.Clear(Color.DarkBlue * .6f);
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

            Vector2 pos = new Vector2(1366 / 2 - BattleGUI.testSF48.MeasureString("VICTORY").X / 2, 100);
            sb.DrawString(BattleGUI.testSF48, "VICTORY", pos, Color.Gold);

            foreach (var item in lootToDisplay)
            {
                item.Draw(sb);
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            return lootScreenRender;
        }
    }

    public class lootDisplay
    {
        enum Rarity { Normal = 0, Rare }
        Rarity itemDropRarity = Rarity.Normal;
        BaseItem.ITEM_RARITY ItemRarity = BaseItem.ITEM_RARITY.Common;

        static Point SizeItemIcon = new Point();
        static Vector2 startPos = new Vector2(195, 285);
        static float verticalOffSet = 20;
        static float endXpos = 1366 / 2 - 32;
        static float horizontalSpeed = 1.0f;
        static float framesToFade = 1.2f * 60;
        static float framesToMove = 1.5f * 60;



        String displayName = "";
        Color itemRarityColour = Color.White;
        Vector2 displayPositionIcon = new Vector2();
        float fade = 0f;
        int framesFadePassed = 0;
        int framesMovePassed = 0;
        BaseItem biRef;

        public lootDisplay(BaseItem bi, int order)
        {
            itemDropRarity = bi.bMarkedAsRare ? Rarity.Normal : Rarity.Rare;
            ItemRarity = bi.ItemRarity;
            displayName = bi.itemName;
            biRef = bi;

            ColourForRarity();
            GenerateLocations(order);
        }

        static public void GenerateOffSets(int amountItems, int heightItem)
        {
            SizeItemIcon = new Point(heightItem);
            float height = 768 - startPos.Y;
            verticalOffSet = (height - (amountItems + 1) * heightItem) / (amountItems + 1);
            horizontalSpeed = (endXpos - startPos.X - SizeItemIcon.X) / framesToMove;
        }

        public void Update(GameTime gt)
        {
            if (framesFadePassed < framesToFade)
            {
                framesFadePassed++;
                fade = 1.0f * (framesFadePassed / framesToFade);
            }

            if (framesMovePassed < framesToMove)
            {
                framesMovePassed++;
                displayPositionIcon.X += horizontalSpeed;
            }

            biRef.UpdateAnimation(gt);
        }

        private void GenerateLocations(int order)
        {
            int index = order + 1;
            displayPositionIcon = startPos + new Vector2(0, index * (SizeItemIcon.X + verticalOffSet));
        }

        private void ColourForRarity()
        {
            switch (ItemRarity)
            {
                case BaseItem.ITEM_RARITY.Junk:
                    itemRarityColour = Color.DarkGray;
                    break;
                case BaseItem.ITEM_RARITY.Common:
                    itemRarityColour = Color.Silver;
                    break;
                case BaseItem.ITEM_RARITY.Uncommon:
                    itemRarityColour = Color.Green;
                    break;
                case BaseItem.ITEM_RARITY.Rare:
                    itemRarityColour = Color.Blue;
                    break;
                case BaseItem.ITEM_RARITY.Epic:
                    itemRarityColour = Color.Purple;
                    break;
                case BaseItem.ITEM_RARITY.Legendary:
                    itemRarityColour = Color.Orange;
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            biRef.itemTexAndAnimation.Draw(sb, new Rectangle(displayPositionIcon.ToPoint(), SizeItemIcon), fade);

            sb.DrawString(BattleGUI.testSF20, "x" + biRef.itemAmount.ToString(), displayPositionIcon + new Vector2(SizeItemIcon.X - 8, SizeItemIcon.Y - 32), itemRarityColour * fade);

            int verticalOffSet = SizeItemIcon.Y / 2 - (int)BattleGUI.testSF25.MeasureString(displayName).Y / 2;
            sb.DrawString(BattleGUI.testSF25, displayName, new Vector2(1366 - SizeItemIcon.X - displayPositionIcon.X, displayPositionIcon.Y + verticalOffSet), itemRarityColour * fade);
        }
    }
}
