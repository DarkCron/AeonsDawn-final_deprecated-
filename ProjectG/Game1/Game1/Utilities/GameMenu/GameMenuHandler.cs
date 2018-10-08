using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.GameMenu;
using static TBAGW.GameMenuHandler;

namespace TBAGW
{
    public static class GameMenuHandler
    {
        internal static Texture2D menuTextureSheet;
        static String menuTextureSource = @"Graphics\GUI\Inventory_sheet_4096x4096";
        public enum GameMenuPages { EquipmentPage = 0, ItemsPage, QuestPage, CharactersPage, MapPage }
        static List<Rectangle> bigMenuBGs = new List<Rectangle> { new Rectangle(0, 0, 683, 384), new Rectangle(683, 0, 683, 384), new Rectangle(1366, 0, 683, 384), new Rectangle(2049, 0, 683, 384), new Rectangle(2732, 0, 683, 384) };
        static public GameMenuPages currentPage = GameMenuPages.EquipmentPage;

        #region Global menu stuff
        static List<Rectangle> menuClickSwitches = new List<Rectangle> { new Rectangle(28 * 2, 15 * 2, 127 * 2, 35 * 2), new Rectangle(155 * 2, 15 * 2, 127 * 2, 35 * 2), new Rectangle(282 * 2, 15 * 2, 125 * 2, 35 * 2), new Rectangle(407 * 2, 15 * 2, 125 * 2, 34 * 2), new Rectangle(531 * 2, 15 * 2, 127 * 2, 35 * 2) };
        #endregion

        #region EQUIPMENT PAGE STUFF
        static internal BaseCharacter selectedCharacterEquipment = null;
        static List<characterPagePanel> lcpp = new List<characterPagePanel>();
        static RenderTarget2D characterPanelRight = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static bool bMovingPanelsUpDown = true;
        static internal int verticalModifier = 0;
        static internal int maxVertical = 0;
        static int verticalPositionOffset = 0;
        static Rectangle upperScrollBarSource = new Rectangle(24, 384, 14, 16);
        static Rectangle lowerScrollBarSource = new Rectangle(24, 582, 14, 16);
        static Rectangle lengthScrollBarSource = new Rectangle(18, 384, 6, 214);
        static Rectangle middleScrollBarSource = new Rectangle(0, 384, 18, 46);

        static Rectangle upperScrollBarPosition = new Rectangle(338 * 2, 85 * 2, 14 * 2, 16 * 2);
        static Rectangle lowerScrollBarPosition = new Rectangle(338 * 2, 321 * 2, 14 * 2, 16 * 2);
        static Rectangle lengthScrollBarPosition = new Rectangle(342 * 2, 104 * 2, 6 * 2, 214 * 2);
        static Rectangle middleScrollBarPosition = new Rectangle(336 * 2, 104 * 2, 18 * 2, 46 * 2);
        static bool WeaponPanel = true;
        static Rectangle equipmentWeaponSelectionSource = new Rectangle(38, 384, 248, 245);
        static Rectangle equipmentArmourSelectionSource = new Rectangle(286, 384, 248, 245);
        static Rectangle equipmentSelectionPosition = new Rectangle(375 * 2, 71 * 2, 248 * 2, 245 * 2);
        static Rectangle equipmentLowerStatPanel = new Rectangle(375 * 2, 211 * 2, 248 * 2, 132 * 2);
        static bool bMovingScrollWheel = false;

        static internal BaseEquipment selectedEquipmentPieceCharacterPanel = null;
        static Rectangle selectedEquipmentPanel = new Rectangle(542 * 2, 197 * 2, 79 * 2, 79 * 2);

        static internal Rectangle WeaponSelectTabBox = new Rectangle(376 * 2, 71 * 2, 123 * 2, 34 * 2);
        static internal Rectangle ArmourSelectTabBox = new Rectangle(499 * 2, 71 * 2, 123 * 2, 34 * 2);
        static internal bool bWeaponTab = true;
        static internal List<Rectangle> equipmentItemBoxes = new List<Rectangle> { };
        static internal BaseEquipment selectedEquipmentPiece = null;
        static Rectangle EquipmentPagePanelPosition = new Rectangle(589 * 2, 183 * 2, 43 * 2, 28 * 2);
        static Rectangle EquipmentPagePanelSource = new Rectangle(1228, 384, 43, 28);
        static Vector2 EquipmentPagePanelTextPosition = new Vector2(602 * 2, 378);
        static String EquipmentPagePanelTest = "";
        static internal int EquipmentCurrentPage = 1;
        static internal int EquipmentMaxPage = 1;
        static internal List<BaseEquipment> equipmentItems = new List<BaseEquipment> { };
        static internal int equipmentOptionSelectionIndex = 0;
        static int maxEquipItemsRightPanel = 12;
        #endregion

        #region ITEMS PAGE STUFF
        static Rectangle redBannerSource = new Rectangle(683, 384, 127, 20);
        static Rectangle blueBannerSource = new Rectangle(683, 404, 127, 20);
        static Rectangle upperTraySource = new Rectangle(810, 384, 418, 131);
        static Rectangle lowerTraySource = new Rectangle(810, 515, 418, 90);

        static Rectangle redBannerPosition = new Rectangle(55 * 2, 75 * 2, 127 * 2, 20 * 2);
        static Rectangle blueBannerPosition = new Rectangle(55 * 2, 225 * 2, 127 * 2, 20 * 2);
        static Rectangle upperTrayPosition = new Rectangle(52 * 2, 88 * 2, 418 * 2, 131 * 2);
        static Rectangle lowerTrayPosition = new Rectangle(52 * 2, 238 * 2, 418 * 2, 90 * 2);

        static int currentUpperPage = 0;
        static List<Rectangle> upperItemPositions = new List<Rectangle>();
        static internal int maxUpperPage = 0;
        static internal int upperCurrentIndex = 0;

        static Rectangle ItemNamePosition = new Rectangle(501 * 2, 80 * 2, 118 * 2, 29 * 2);
        static Vector2 itemNameSpecificPosition = new Vector2(501 * 2, 82 * 2);
        static internal BaseItem itemPageSelectedItem = null;
        static String selectedItemName = "";
        static String selectedItemDescription = "";
        static String selectedItemShortDescription = "";
        static Rectangle selectedItemDescriptionPosition = new Rectangle(500 * 2, 181 * 2, 115 * 2, 170);
        static Rectangle selectedItemShortDescriptionPosition = new Rectangle(500 * 2, 520, 225, 32 * 2);
        static Rectangle selectedItemFramePosition = new Rectangle(530 * 2, 116 * 2, 60 * 2, 60 * 2);

        static Rectangle selectedItemHighlighterSource = new Rectangle(772, 424, 38, 38);
        static internal Rectangle selectedItemHighlighterPosition = new Rectangle(772, 424, 38 * 2, 38 * 2);

        static SpriteFont itemDescriptionFont = Game1.defaultFont;
        static SpriteFont itemNameFont = Game1.defaultFont;
        static SpriteFont itemOptionsFont = Game1.defaultFont;
        static internal bool bDisplayOptions = false;
        static internal int itemOptionSelectionIndex = 0;

        static internal bool bChooseCharacterToUseOn = false;
        static Rectangle charPanelBGSource = new Rectangle(60, 68, 255, 275);
        static Rectangle charPanelBGPosition = new Rectangle(60 * 2, 70 * 2, 255 * 2, 275 * 2);

        static Rectangle ItemPagePanelSource = new Rectangle(1228, 384, 43, 28);
        internal static BaseCharacter selectedCharacterItems;
        static internal List<BaseItem> onlyTheseItemsToConsider = new List<BaseItem>();
        #endregion

        #region CHARACTER PAGE

        static internal CharacterContextMenu selectedCharacterContext = null;

        static Rectangle BigPanelSource = new Rectangle(2048, 560, 560, 108);
        static Rectangle PortraitPanelSource = new Rectangle(2151, 492, 68, 68);
        static Rectangle NamePanelSource = new Rectangle(2049, 492, 102, 28);
        static Rectangle LevelBoxSource = new Rectangle(2208, 400, 35, 19);
        static Rectangle InfoBoxSource = new Rectangle(2049, 520, 75, 18);

        static internal Point firstTabLocation = new Point(0, 30);
        static Point portraitOffSet = new Point(55, 20);
        static Point nameOffSet = new Point(0, 155);

        static internal Matrix characterTabAdjustedMatrix = new Matrix();
        static internal float characterTabVerticalModifier = 0f;
        static internal int characterSelectIndex = 0;
        static internal List<CharacterTabdisplay> charTabList = new List<CharacterTabdisplay>();
        static internal CharacterTabdisplay selectedCharacterPanel = default(CharacterTabdisplay);
        #endregion

        static internal void ResetCharContextMenu()
        {
            selectedCharacterContext = null;
        }

        static void ReloadResources()
        {

            menuTextureSheet = Game1.contentManager.Load<Texture2D>(menuTextureSource);
            itemDescriptionFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\GameMenu\Items\ItemDescription");
            itemNameFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\GameMenu\Items\ItemName");
            itemOptionsFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\GameMenu\Items\ItemOptions");
            characterPagePanel.font = itemOptionsFont;
            upperItemPositions.Clear();
            equipmentItemBoxes.Clear();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    upperItemPositions.Add(new Rectangle((59 + (3 + 38) * j) * 2, (92 + (3 + 38) * i) * 2, 38 * 2, 38 * 2));
                }
            }

            //new Rectangle(378 * 2, 109 * 2, 38 * 2, 38 * 2);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    equipmentItemBoxes.Add(new Rectangle(378 * 2 + j * (3 + 38) * 2, 109 * 2 + i * (3 + 38) * 2, 38 * 2, 38 * 2));
                }
            }
        }

        public static void Start()
        {
            if (menuTextureSheet == null)
            {
                Console.WriteLine("Loading Menu resources");
                ReloadResources();
            }
            TextUtility.ClearCache();

            bMovingPanelsUpDown = true;
            currentPage = GameMenuPages.EquipmentPage;
            PlayerSaveData.playerInventory.ReloadAllItemsResources();
            PlayerSaveData.playerInventory.SoftResetItemAnimations();
            itemPageSelectedItem = null;
            maxUpperPage = 0;
            upperCurrentIndex = 0;
            selectedCharacterEquipment = null;
            bDisplayOptions = false;
            ItemOptionDisplay.Start(menuTextureSheet, itemOptionsFont);
            EquipmentOptionDisplay.Start(menuTextureSheet, itemOptionsFont);
            lcpp.Clear();
            verticalModifier = 0;
            WeaponPanel = true;
            for (int i = 0; i < PlayerSaveData.heroParty.Count; i++)
            {
                PlayerSaveData.heroParty[i].CCC.equippedClass.classEXP.CalculateResetExpAndLevels();
            }
        }

        static void ResetPages()
        {
            ResetCharContextMenu();
            charTabList.ForEach(tab => tab.GetRender().Dispose());
            charTabList.Clear();
            characterTabVerticalModifier = 0f;
            characterTabAdjustedMatrix = Matrix.CreateTranslation(new Vector3(0, GameMenuHandler.characterTabVerticalModifier, 1));
            equipmentItems.Clear();
            EquipmentMaxPage = 1;
            EquipmentCurrentPage = 1;
            EquipmentPagePanelTest = "";
            bChooseCharacterToUseOn = false;
            verticalModifier = 0;
            maxVertical = 0;
            verticalPositionOffset = 0;
            bDisplayOptions = false;
            itemOptionSelectionIndex = 0;
            itemPageSelectedItem = null;
            maxUpperPage = 0;
            bMovingScrollWheel = false;
            WeaponPanel = true;
            selectedEquipmentPieceCharacterPanel = null;
            bWeaponTab = true;
            selectedEquipmentPiece = null;
        }

        static internal void TabClick()
        {
            Vector2 beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale;
            if (menuClickSwitches.Find(r => r.Contains(beep)) != default(Rectangle) && Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                try
                {
                    currentPage = (GameMenuPages)(menuClickSwitches.IndexOf(menuClickSwitches.Find(r => r.Contains(beep))));
                    ResetPages();
                }
                catch
                {
                }

            }
            else if (Mouse.GetState().LeftButton != ButtonState.Pressed)
            {
                int index = (int)currentPage;
                index++;
                if (index == Enum.GetNames(typeof(GameMenuPages)).Length)
                {
                    index = 0;
                }

                currentPage = (GameMenuPages)(index);
                ResetPages();
            }
        }

        static internal void EquipmentTabRightClick()
        {
            Vector2 beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale;
            if (equipmentItemBoxes.Find(r => r.Contains(beep)) != default(Rectangle))
            {
                try
                {
                    int index = equipmentItemBoxes.IndexOf(equipmentItemBoxes.Find(r => r.Contains(beep)));
                    selectedEquipmentPiece = equipmentItems[index];

                    selectedItemHighlighterPosition.Location = equipmentItemBoxes.Find(r => r.Contains(beep)).Location;
                    bDisplayOptions = true;
                    if (index < 4)
                    {
                        EquipmentOptionDisplay.GenerateLocations(selectedItemHighlighterPosition.Location + new Point(-15, 70), selectedEquipmentPiece);
                    }
                    else
                    {
                        EquipmentOptionDisplay.GenerateLocations(selectedItemHighlighterPosition.Location + new Point(0, 70), selectedEquipmentPiece);
                    }

                    equipmentOptionSelectionIndex = 0;
                    GameMenuHandler.equipmentDisplay.Generate(index);
                }
                catch
                {
                }

            }

        }
        static internal void EquipmentTabLeftClick()
        {
            Vector2 beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale;
            if (WeaponSelectTabBox.Contains(beep))
            {
                bWeaponTab = true;
                EquipmentCurrentPage = 1;
                bDisplayOptions = false;
                //RegenerateEquipList();
            }
            else if (ArmourSelectTabBox.Contains(beep))
            {
                bWeaponTab = false;
                EquipmentCurrentPage = 1;
                bDisplayOptions = false;
                //  RegenerateEquipList();
            }




            if (!bMovingScrollWheel)
            {
                if (EquipmentOptionDisplay.choiceBoxes.Find(r => r.Contains(beep)) == default(Rectangle))
                {
                    bDisplayOptions = false;
                    foreach (var item in lcpp)
                    {
                        Point adjustedMouse = new Point(beep.ToPoint().X, beep.ToPoint().Y);
                        if (item.Contains(beep) && selectedCharacterEquipment != PlayerSaveData.heroParty[lcpp.IndexOf(item)])
                        {
                            selectedCharacterEquipment = PlayerSaveData.heroParty[lcpp.IndexOf(item)];
                            selectedEquipmentPiece = null;
                            GameMenuHandler.verticalModifier = (-GameMenuHandler.maxVertical / PlayerSaveData.heroParty.Count) * lcpp.IndexOf(item);
                            RegenerateEquipList();
                        }

                        if (item.charPanelPositions[(int)characterPagePanel.characterPanelPositions.WeaponPanel].Contains(adjustedMouse))
                        {
                            selectedEquipmentPieceCharacterPanel = PlayerSaveData.heroParty[lcpp.IndexOf(item)].weapon;
                            bWeaponTab = true;
                            selectedEquipmentPiece = null;
                            RegenerateEquipList();
                        }

                        if (item.charPanelPositions[(int)characterPagePanel.characterPanelPositions.ArmourPanel].Contains(adjustedMouse))
                        {
                            selectedEquipmentPieceCharacterPanel = PlayerSaveData.heroParty[lcpp.IndexOf(item)].armour;
                            bWeaponTab = false;
                            selectedEquipmentPiece = null;
                            RegenerateEquipList();
                        }

                        if (selectedEquipmentPieceCharacterPanel != null && selectedEquipmentPieceCharacterPanel.EquipType == BaseEquipment.EQUIP_TYPES.Armor)
                        {
                            bWeaponTab = false;

                        }
                        else if (selectedEquipmentPieceCharacterPanel != null && selectedEquipmentPieceCharacterPanel.EquipType != BaseEquipment.EQUIP_TYPES.Armor)
                        {
                            bWeaponTab = true;
                        }
                    }

                    if (lcpp.Find(panel => panel.Contains(beep)) == default(characterPagePanel) && !WeaponSelectTabBox.Contains(beep) && !ArmourSelectTabBox.Contains(beep) && equipmentItemBoxes.Find(r => r.Contains(beep)) == default(Rectangle))
                    {
                        selectedCharacterEquipment = null;
                    }
                }
            }




            if (equipmentItemBoxes.Find(b => b.Contains(beep)) != default(Rectangle))
            {
                int index = equipmentItemBoxes.IndexOf(equipmentItemBoxes.Find(b => b.Contains(beep)));
                if (index < equipmentItems.Count)
                {
                    selectedEquipmentPiece = equipmentItems[index];
                    selectedItemHighlighterPosition.Location = equipmentItemBoxes.Find(r => r.Contains(beep)).Location;
                    equipmentDisplay.Generate(index);
                    bDisplayOptions = false;
                    // RegenerateEquipList();
                }
                else if (!bDisplayOptions)
                {
                    selectedCharacterEquipment = null;
                    selectedEquipmentPiece = null;
                    bDisplayOptions = false;
                    //    RegenerateEquipList();
                }
            }





            #region choicebox
            if (selectedCharacterEquipment != null && EquipmentOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)) != default(Rectangle) && equipmentOptionSelectionIndex == 0)
            {
                switch (selectedEquipmentPiece.EquipType)
                {
                    case BaseEquipment.EQUIP_TYPES.Weapon:
                        var equippedWeapon = selectedCharacterEquipment.weapon;
                        if (equippedWeapon != null)
                        {
                            PlayerSaveData.playerInventory.localInventory.Add(equippedWeapon.Clone());
                        }
                        selectedCharacterEquipment.weapon = selectedEquipmentPiece.Clone() as BaseEquipment;
                        PlayerSaveData.playerInventory.localInventory.Remove(selectedEquipmentPiece);
                        RegenerateEquipList();
                        break;
                    case BaseEquipment.EQUIP_TYPES.Armor:
                        var equippedArmour = selectedCharacterEquipment.armour;
                        if (equippedArmour != null)
                        {
                            PlayerSaveData.playerInventory.localInventory.Add(equippedArmour.Clone());
                        }
                        selectedCharacterEquipment.armour = selectedEquipmentPiece.Clone() as BaseEquipment;
                        PlayerSaveData.playerInventory.localInventory.Remove(selectedEquipmentPiece);
                        RegenerateEquipList();
                        break;
                    default:
                        break;
                }
                selectedEquipmentPiece = null;
                bDisplayOptions = false;
                equipmentOptionSelectionIndex = 1;
            }
            else if (EquipmentOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)) != default(Rectangle) && equipmentOptionSelectionIndex == 1)
            {
                bDisplayOptions = false;
            }
            #endregion
        }
        static internal void RegenerateEquipList()
        {
            equipmentItems.Clear();
            List<BaseEquipment> allEquipment = new List<BaseEquipment>();
            foreach (var item in PlayerSaveData.playerInventory.localInventory.FindAll(w => w.itemType == BaseItem.ITEM_TYPES.Equipment))
            {
                allEquipment.Add(item as BaseEquipment);
            }
            List<BaseEquipment> allWeapons = new List<BaseEquipment>();
            List<BaseEquipment> allArmour = new List<BaseEquipment>();
            foreach (var item in allEquipment)
            {
                if (item.EquipType == BaseEquipment.EQUIP_TYPES.Weapon)
                {
                    allWeapons.Add(item);
                }
                else
                {
                    allArmour.Add(item);
                }
            }

            List<BaseEquipment> allEquipWeapons = new List<BaseEquipment>();
            List<BaseEquipment> allEquipArmour = new List<BaseEquipment>();

            if (bWeaponTab)
            {
                foreach (var item in allWeapons)
                {
                    if (selectedCharacterEquipment.CCC.equippedClass.classType == item.WeaponUseType)
                    {
                        allEquipWeapons.Add(item);
                    }
                }

                int maxPages = (allEquipWeapons.Count + 1) / maxEquipItemsRightPanel + 1;
                if (EquipmentCurrentPage > maxPages)
                {
                    EquipmentCurrentPage = maxPages;
                }
                EquipmentPagePanelTest = EquipmentCurrentPage.ToString() + @"/" + maxPages.ToString();

                if (maxPages > 1)
                {
                    allEquipWeapons.RemoveRange(0, EquipmentCurrentPage * maxEquipItemsRightPanel - 1);
                }

                int hhh = 0;
                while (equipmentItems.Count < maxEquipItemsRightPanel && equipmentItems.Count != allEquipWeapons.Count)
                {

                    equipmentItems.Add(allEquipWeapons[hhh]);
                    hhh++;
                }
            }
            else
            {
                foreach (var item in allArmour)
                {
                    if (selectedCharacterEquipment.CCC.equippedClass.classTypeArmour >= item.ArmourUseType)
                    {
                        allEquipArmour.Add(item);
                    }
                }

                int maxPages = (allEquipArmour.Count + 1) / maxEquipItemsRightPanel + 1;
                if (EquipmentCurrentPage > maxPages)
                {
                    EquipmentCurrentPage = maxPages;
                }
                EquipmentPagePanelTest = EquipmentCurrentPage.ToString() + @"/" + maxPages.ToString();


                if (maxPages > 1)
                {
                    allEquipArmour.RemoveRange(0, EquipmentCurrentPage * maxEquipItemsRightPanel - 1);
                }

                int hhh = 0;
                while (equipmentItems.Count < maxEquipItemsRightPanel && equipmentItems.Count != allEquipArmour.Count)
                {

                    equipmentItems.Add(allEquipArmour[hhh]);
                    hhh++;
                }
            }

            EquipmentMaxPage = allEquipment.Count / maxEquipItemsRightPanel;
        }
        static internal void EquipChoiceConfirm()
        {
            #region choicebox
            if (selectedCharacterEquipment != null && equipmentOptionSelectionIndex == 0)
            {
                switch (selectedEquipmentPiece.EquipType)
                {
                    case BaseEquipment.EQUIP_TYPES.Weapon:
                        var equippedWeapon = selectedCharacterEquipment.weapon;
                        if (equippedWeapon != null)
                        {
                            PlayerSaveData.playerInventory.localInventory.Add(equippedWeapon.Clone());
                        }
                        selectedCharacterEquipment.weapon = selectedEquipmentPiece.Clone() as BaseEquipment;
                        PlayerSaveData.playerInventory.localInventory.Remove(selectedEquipmentPiece);
                        RegenerateEquipList();
                        break;
                    case BaseEquipment.EQUIP_TYPES.Armor:
                        var equippedArmour = selectedCharacterEquipment.armour;
                        if (equippedArmour != null)
                        {
                            PlayerSaveData.playerInventory.localInventory.Add(equippedArmour.Clone());
                        }
                        selectedCharacterEquipment.armour = selectedEquipmentPiece.Clone() as BaseEquipment;
                        PlayerSaveData.playerInventory.localInventory.Remove(selectedEquipmentPiece);
                        RegenerateEquipList();
                        break;
                    default:
                        break;
                }
                selectedEquipmentPiece = null;
                bDisplayOptions = false;
                equipmentOptionSelectionIndex = 1;
            }
            else if (equipmentOptionSelectionIndex == 1)
            {
                bDisplayOptions = false;
            }
            #endregion
        }
        static internal void EquipmentShowChoices(int indexTemp)
        {

            try
            {
                int index = indexTemp;
                selectedEquipmentPiece = equipmentItems[index];

                selectedItemHighlighterPosition.Location = equipmentItemBoxes[index].Location;
                bDisplayOptions = true;
                if (index < 4)
                {
                    EquipmentOptionDisplay.GenerateLocations(selectedItemHighlighterPosition.Location + new Point(-15, 70), selectedEquipmentPiece);
                }
                else
                {
                    EquipmentOptionDisplay.GenerateLocations(selectedItemHighlighterPosition.Location + new Point(0, 70), selectedEquipmentPiece);
                }

                equipmentOptionSelectionIndex = 0;
                GameMenuHandler.equipmentDisplay.Generate(index);
            }
            catch
            {
            }



        }

        static internal void itemsTabLeftClickCharacter()
        {
            Vector2 beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale;


            Point testPosition = new Point(69 * 2, 82 * 2);
            lcpp.Clear();
            foreach (var item in PlayerSaveData.heroParty)
            {
                Point point = new Point(testPosition.X, (1 + PlayerSaveData.heroParty.IndexOf(item)) * testPosition.Y + (PlayerSaveData.heroParty.IndexOf(item)) * 100 + verticalModifier);
                lcpp.Add(new characterPagePanel(item as BaseCharacter, menuTextureSheet, point));
            }

            if (!bMovingScrollWheel)
            {

                if (lcpp.Find(cp => cp.Contains(beep)) != default(characterPagePanel))
                {
                    PlayerSaveData.heroParty[lcpp.IndexOf(lcpp.Find(cp => cp.Contains(beep)))].ProcessConsumable(itemPageSelectedItem as BaseConsumable);
                    ResetPages();
                    if (itemPageSelectedItem != null && itemPageSelectedItem.itemAmount == 0)
                    {
                        bDisplayOptions = false;
                    }
                }
            }

        }
        static internal void itemsTabLeftClick()
        {
            Vector2 beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale;


            if (!bDisplayOptions)
            {
                if (upperItemPositions.Find(r => r.Contains(beep)) != default(Rectangle))
                {
                    if (ItemOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)) == default(Rectangle))
                    {
                        try
                        {
                            itemPageSelectedItem = onlyTheseItemsToConsider[upperItemPositions.IndexOf(upperItemPositions.Find(r => r.Contains(beep)))];
                            selectedItemHighlighterPosition.Location = upperItemPositions.Find(r => r.Contains(beep)).Location;
                            selectedItemName = itemPageSelectedItem.itemName;
                            selectedItemDescription = TextUtility.bestMatchStringForBox(itemPageSelectedItem.itemDescription, itemDescriptionFont, selectedItemDescriptionPosition);
                            selectedItemShortDescription = TextUtility.bestMatchStringForBox(itemPageSelectedItem.itemShortDescription, itemDescriptionFont, selectedItemShortDescriptionPosition);
                            bDisplayOptions = false;
                        }
                        catch
                        {
                            itemPageSelectedItem = null;
                            bDisplayOptions = false;
                        }
                    }
                }
            }
            else if (bDisplayOptions)
            {
                if (ItemOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)) == default(Rectangle))
                {
                    if (upperItemPositions.Find(r => r.Contains(beep)) != default(Rectangle))
                    {

                        try
                        {
                            itemPageSelectedItem = onlyTheseItemsToConsider[upperItemPositions.IndexOf(upperItemPositions.Find(r => r.Contains(beep)))];
                            selectedItemHighlighterPosition.Location = upperItemPositions.Find(r => r.Contains(beep)).Location;
                            selectedItemName = itemPageSelectedItem.itemName;
                            selectedItemDescription = TextUtility.bestMatchStringForBox(itemPageSelectedItem.itemDescription, itemDescriptionFont, selectedItemDescriptionPosition);
                            selectedItemShortDescription = TextUtility.bestMatchStringForBox(itemPageSelectedItem.itemShortDescription, itemDescriptionFont, selectedItemShortDescriptionPosition);
                            bDisplayOptions = false;
                        }
                        catch
                        {
                            itemPageSelectedItem = null;
                            bDisplayOptions = false;
                        }

                    }
                }
                else if (ItemOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)) != default(Rectangle))
                {
                    itemOptionSelectionIndex = ItemOptionDisplay.choiceBoxes.IndexOf(ItemOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)));
                }
            }

            if (CombatProcessor.bMainCombat && bDisplayOptions)
            {
                if (ItemOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)) != default(Rectangle))
                {
                    if (itemOptionSelectionIndex == 0 && itemPageSelectedItem != null && itemPageSelectedItem.GetType() == typeof(BaseConsumable))
                    {
                        var c = EncounterInfo.currentTurn().selectedCharTurn.character;
                        c.ProcessConsumable(itemPageSelectedItem as BaseConsumable);
                        if (itemPageSelectedItem != null && itemPageSelectedItem.itemAmount == 0)
                        {
                            bDisplayOptions = false;
                        }
                    }

                    if (itemOptionSelectionIndex == 1 && itemPageSelectedItem != null && itemPageSelectedItem.GetType() == typeof(BaseConsumable))
                    {
                        itemPageSelectedItem.itemAmount = 0;
                        itemPageSelectedItem.ConsumeItem();
                        itemPageSelectedItem = null;

                        bDisplayOptions = false;

                    }
                    else if (itemOptionSelectionIndex == 0 && itemPageSelectedItem != null && (itemPageSelectedItem.GetType() == typeof(BaseMaterials) || itemPageSelectedItem.GetType() == typeof(BaseEquipment)))
                    {
                        itemPageSelectedItem.itemAmount = 0;
                        itemPageSelectedItem.ConsumeItem();
                        itemPageSelectedItem = null;

                        bDisplayOptions = false;

                    }

                    if (itemOptionSelectionIndex == ItemOptionDisplay.choiceBoxes.Count - 1)
                    {
                        bDisplayOptions = false;
                    }
                }
            }
            else if (!CombatProcessor.bMainCombat && bDisplayOptions)
            {
                if (ItemOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)) != default(Rectangle))
                {
                    if (itemOptionSelectionIndex == 0 && itemPageSelectedItem != null && itemPageSelectedItem.GetType() == typeof(BaseConsumable))
                    {
                        bChooseCharacterToUseOn = true;
                        KeyboardMouseUtility.bPressed = true;
                    }

                    if (itemOptionSelectionIndex == 1 && itemPageSelectedItem != null && itemPageSelectedItem.GetType() == typeof(BaseConsumable))
                    {
                        itemPageSelectedItem.itemAmount = 0;
                        itemPageSelectedItem.ConsumeItem();
                        itemPageSelectedItem = null;

                        bDisplayOptions = false;

                    }
                    else if (itemOptionSelectionIndex == 0 && itemPageSelectedItem != null && (itemPageSelectedItem.GetType() == typeof(BaseMaterials) || itemPageSelectedItem.GetType() == typeof(BaseEquipment)))
                    {
                        itemPageSelectedItem.itemAmount = 0;
                        itemPageSelectedItem.ConsumeItem();
                        itemPageSelectedItem = null;

                        bDisplayOptions = false;

                    }

                    if (itemOptionSelectionIndex == ItemOptionDisplay.choiceBoxes.Count - 1)
                    {
                        bDisplayOptions = false;
                    }
                }
            }
        }
        static internal void itemsTabRightClick()
        {
            Vector2 beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale;

            if (upperItemPositions.Find(r => r.Contains(beep)) != default(Rectangle))
            {
                try
                {
                    itemPageSelectedItem = onlyTheseItemsToConsider[upperItemPositions.IndexOf(upperItemPositions.Find(r => r.Contains(beep)))];
                    selectedItemHighlighterPosition.Location = upperItemPositions.Find(r => r.Contains(beep)).Location;
                    selectedItemName = itemPageSelectedItem.itemName;
                    selectedItemDescription = TextUtility.bestMatchStringForBox(itemPageSelectedItem.itemDescription, itemDescriptionFont, selectedItemDescriptionPosition);
                    selectedItemShortDescription = TextUtility.bestMatchStringForBox(itemPageSelectedItem.itemShortDescription, itemDescriptionFont, selectedItemShortDescriptionPosition);
                    bDisplayOptions = true;
                    ItemOptionDisplay.GenerateLocations(selectedItemHighlighterPosition.Location + new Point(35, 70), itemPageSelectedItem);
                    itemOptionSelectionIndex = 0;
                }
                catch
                {
                    itemPageSelectedItem = null;
                }

            }
        }
        static internal void SelectItemViaKeyboard(BaseItem bi, int index)
        {
            try
            {
                itemPageSelectedItem = bi;
                selectedItemHighlighterPosition.Location = upperItemPositions[index].Location;
                selectedItemName = itemPageSelectedItem.itemName;
                selectedItemDescription = TextUtility.bestMatchStringForBox(itemPageSelectedItem.itemDescription, itemDescriptionFont, selectedItemDescriptionPosition);
                selectedItemShortDescription = TextUtility.bestMatchStringForBox(itemPageSelectedItem.itemShortDescription, itemDescriptionFont, selectedItemShortDescriptionPosition);
                bDisplayOptions = false;
            }
            catch
            {
                itemPageSelectedItem = null;
                bDisplayOptions = false;
            }
        }
        static internal void SelectItemViaKeyboardDisplayOptions()
        {
            try
            {
                bDisplayOptions = true;
                ItemOptionDisplay.GenerateLocations(selectedItemHighlighterPosition.Location + new Point(35, 70), itemPageSelectedItem);
                itemOptionSelectionIndex = 0;
            }
            catch
            {
                itemPageSelectedItem = null;
            }
        }
        static internal void SelectItemOptionViaKeyboardDisplay()
        {
            if (CombatProcessor.bMainCombat && bDisplayOptions)
            {
                if (itemOptionSelectionIndex == 0 && itemPageSelectedItem.GetType() == typeof(BaseConsumable))
                {
                    var c = EncounterInfo.currentTurn().selectedCharTurn.character;
                    c.ProcessConsumable(itemPageSelectedItem as BaseConsumable);
                    if (itemPageSelectedItem != null && itemPageSelectedItem.itemAmount == 0)
                    {
                        bDisplayOptions = false;
                    }
                }

                if (itemOptionSelectionIndex == 1 && itemPageSelectedItem != null && itemPageSelectedItem.GetType() == typeof(BaseConsumable))
                {
                    itemPageSelectedItem.itemAmount = 0;
                    itemPageSelectedItem.ConsumeItem();
                    itemPageSelectedItem = null;

                    bDisplayOptions = false;

                }
                else if (itemOptionSelectionIndex == 0 && itemPageSelectedItem != null && (itemPageSelectedItem.GetType() == typeof(BaseMaterials) || itemPageSelectedItem.GetType() == typeof(BaseEquipment)))
                {
                    itemPageSelectedItem.itemAmount = 0;
                    itemPageSelectedItem.ConsumeItem();
                    itemPageSelectedItem = null;

                    bDisplayOptions = false;

                }


                if (itemOptionSelectionIndex == ItemOptionDisplay.choiceBoxes.Count - 1)
                {
                    bDisplayOptions = false;
                }

            }
            else if (!CombatProcessor.bMainCombat && bDisplayOptions)
            {
                if (itemOptionSelectionIndex == 0 && itemPageSelectedItem.GetType() == typeof(BaseConsumable))
                {
                    bChooseCharacterToUseOn = true;
                    KeyboardMouseUtility.bPressed = true;
                    selectedCharacterItems = null;
                }

                if (itemOptionSelectionIndex == 1 && itemPageSelectedItem != null && itemPageSelectedItem.GetType() == typeof(BaseConsumable))
                {
                    itemPageSelectedItem.itemAmount = 0;
                    itemPageSelectedItem.ConsumeItem();
                    itemPageSelectedItem = null;

                    bDisplayOptions = false;

                }
                else if (itemOptionSelectionIndex == 0 && itemPageSelectedItem != null && (itemPageSelectedItem.GetType() == typeof(BaseMaterials) || itemPageSelectedItem.GetType() == typeof(BaseEquipment)))
                {
                    itemPageSelectedItem.itemAmount = 0;
                    itemPageSelectedItem.ConsumeItem();
                    itemPageSelectedItem = null;

                    bDisplayOptions = false;

                }


                if (itemOptionSelectionIndex == ItemOptionDisplay.choiceBoxes.Count - 1)
                {
                    bDisplayOptions = false;
                }
            }

        }
        static internal void RegenerateItemList()
        {
            var upperItems = PlayerSaveData.playerInventory.localInventory;
            onlyTheseItemsToConsider = new List<BaseItem>();
            onlyTheseItemsToConsider.AddRange(upperItems.FindAll(i => upperItems.IndexOf(i) >= upperCurrentIndex * 30 && upperItems.IndexOf(i) < (upperCurrentIndex + 1) * 30));
        }
        static internal void UseItemOnViaKeyboard()
        {
            selectedCharacterItems.ProcessConsumable(itemPageSelectedItem as BaseConsumable);
            ResetPages();
            if (itemPageSelectedItem != null && itemPageSelectedItem.itemAmount == 0)
            {
                bDisplayOptions = false;
            }
        }


        static public void Update(GameTime gt, Vector2 beep)
        {

            PlayerController.Update(gt);




            switch (currentPage)
            {
                case GameMenuPages.EquipmentPage:
                    #region Equipment page

                    #region Manages equipment item boxes
                    if (selectedCharacterEquipment != null && equipmentItems.Count == 0 && PlayerSaveData.playerInventory.localInventory.FindAll(w => w.itemType == BaseItem.ITEM_TYPES.Equipment).Count != 0)
                    {
                        RegenerateEquipList();
                    }
                    #endregion

                    Point testPosition = new Point(69 * 2, 82 * 2);
                    maxVertical = (PlayerSaveData.heroParty.Count) * testPosition.Y + (PlayerSaveData.heroParty.Count) * 0;
                    lcpp.Clear();
                    foreach (var item in PlayerSaveData.heroParty)
                    {
                        Point point = new Point(testPosition.X, (1 + PlayerSaveData.heroParty.IndexOf(item)) * testPosition.Y + (PlayerSaveData.heroParty.IndexOf(item)) * 100 + verticalModifier);
                        lcpp.Add(new characterPagePanel(item as BaseCharacter, menuTextureSheet, point));
                    }

                    Rectangle scrollMiddleLocation = new Rectangle(middleScrollBarPosition.X, middleScrollBarPosition.Y + verticalPositionOffset, middleScrollBarPosition.Width, middleScrollBarPosition.Height);
                    if (scrollMiddleLocation.Contains(beep) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        bMovingScrollWheel = true;
                        bDisplayOptions = false;
                    }

                    if (bMovingScrollWheel && Mouse.GetState().LeftButton == ButtonState.Released)
                    {
                        bMovingScrollWheel = false;
                    }

                    if (bMovingScrollWheel && lcpp.Count > 2)
                    {
                        int offSet = beep.ToPoint().Y - scrollMiddleLocation.Y;
                        if (beep.ToPoint().Y > lengthScrollBarPosition.Y && beep.ToPoint().Y < 104 * 2 + 214 * 2)
                        {
                            verticalModifier = -(beep.ToPoint().Y - 100 * 2) - offSet;
                        }
                        else if (beep.ToPoint().Y < lengthScrollBarPosition.Y)
                        {
                            verticalModifier = 0;
                        }
                        else if (beep.ToPoint().Y >= 104 * 2 + 214 * 2)
                        {
                            verticalModifier = -maxVertical;
                        }
                    }

                    verticalPositionOffset = (int)(336 * ((float)((float)-verticalModifier / (float)maxVertical)));

                    var s = ((float)((float)-verticalModifier / (float)maxVertical));

                    //if (s <= 0)
                    //{
                    //    verticalModifier = 0;
                    //}
                    //else if (s >= 1)
                    //{
                    //    verticalModifier = -maxVertical;
                    //}

                    if (bDisplayOptions)
                    {
                        if (EquipmentOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)) != default(Rectangle))
                        {
                            equipmentOptionSelectionIndex = EquipmentOptionDisplay.choiceBoxes.IndexOf(EquipmentOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)));
                        }
                    }

                    if (selectedCharacterEquipment != null && equipmentItems.Count != 0)
                    {
                        foreach (var item in equipmentItems)
                        {
                            item.UpdateAnimation(gt);
                        }
                    }

                    ////////////////////////////////////////////////////////////

                    #endregion
                    break;
                case GameMenuPages.ItemsPage:
                    #region Items page
                    if (!bChooseCharacterToUseOn)
                    {
                        maxUpperPage = PlayerSaveData.playerInventory.localInventory.Count / 30;
                        var upperItems = PlayerSaveData.playerInventory.localInventory;
                        onlyTheseItemsToConsider = new List<BaseItem>();
                        onlyTheseItemsToConsider.AddRange(upperItems.FindAll(i => upperItems.IndexOf(i) >= upperCurrentIndex * 30 && upperItems.IndexOf(i) < (upperCurrentIndex + 1) * 30));
                        foreach (var item in onlyTheseItemsToConsider)
                        {
                            item.UpdateAnimation(gt);
                        }

                        if (ItemOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)) != default(Rectangle))
                        {
                            itemOptionSelectionIndex = ItemOptionDisplay.choiceBoxes.IndexOf(ItemOptionDisplay.choiceBoxes.Find(b => b.Contains(beep)));
                        }
                    }


                    if (bChooseCharacterToUseOn)
                    {
                        itemPageSelectedItem.UpdateAnimation(gt);

                        testPosition = new Point(69 * 2, 82 * 2);
                        maxVertical = (PlayerSaveData.heroParty.Count) * testPosition.Y + (PlayerSaveData.heroParty.Count) * 0;
                        lcpp.Clear();
                        foreach (var item in PlayerSaveData.heroParty)
                        {
                            Point point = new Point(testPosition.X, (1 + PlayerSaveData.heroParty.IndexOf(item)) * testPosition.Y + (PlayerSaveData.heroParty.IndexOf(item)) * 100 + verticalModifier);
                            lcpp.Add(new characterPagePanel(item as BaseCharacter, menuTextureSheet, point));
                        }

                        scrollMiddleLocation = new Rectangle(middleScrollBarPosition.X, middleScrollBarPosition.Y + verticalPositionOffset, middleScrollBarPosition.Width, middleScrollBarPosition.Height);
                        if (scrollMiddleLocation.Contains(beep) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            bMovingScrollWheel = true;
                        }

                        if (bMovingScrollWheel && Mouse.GetState().LeftButton == ButtonState.Released)
                        {
                            bMovingScrollWheel = false;
                        }

                        if (bMovingScrollWheel && lcpp.Count > 2)
                        {
                            int offSet = beep.ToPoint().Y - scrollMiddleLocation.Y;
                            if (beep.ToPoint().Y > lengthScrollBarPosition.Y && beep.ToPoint().Y < 104 * 2 + 214 * 2)
                            {
                                verticalModifier = -(beep.ToPoint().Y - 100 * 2) - offSet;
                            }
                            else if (beep.ToPoint().Y < lengthScrollBarPosition.Y)
                            {
                                verticalModifier = 0;
                            }
                            else if (beep.ToPoint().Y >= 104 * 2 + 214 * 2)
                            {
                                verticalModifier = -maxVertical;
                            }
                        }

                        verticalPositionOffset = (int)(336 * ((float)((float)-verticalModifier / (float)maxVertical)));

                        s = ((float)((float)-verticalModifier / (float)maxVertical));

                        //if (s <= 0)
                        //{
                        //    verticalModifier = 0;
                        //}
                        //else if (s >= 1)
                        //{
                        //    verticalModifier = -maxVertical;
                        //}


                    }
                    #endregion
                    break;
                case GameMenuPages.QuestPage:
                    #region Quest page
                    #endregion
                    break;
                case GameMenuPages.CharactersPage:
                    #region Characters page
                    if (selectedCharacterContext == null)
                    {
                        if (charTabList.Count == 0 && charTabList.Count != PlayerSaveData.heroParty.Count)
                        {
                            int index = 0;
                            foreach (var item in PlayerSaveData.heroParty)
                            {
                                charTabList.Add(new CharacterTabdisplay(index, item));
                                index++;
                            }
                        }
                    }
                    else
                    {
                        selectedCharacterContext.Update(gt);
                    }
                    #endregion
                    break;
                case GameMenuPages.MapPage:
                    #region Map page
                    #endregion
                    break;
                default:
                    break;
            }
        }

        static RenderTarget2D menuRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D characterPageRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 571 * 2, 272 * 2);

        static public RenderTarget2D Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(menuRender);
            sb.GraphicsDevice.Clear(Color.Black);
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

            sb.Draw(GameProcessor.lastDrawnMapRender, new Rectangle(0, 0, 1366, 768), Color.White * .5f);
            sb.Draw(menuTextureSheet, new Rectangle(0, 0, 1366, 768), bigMenuBGs[(int)currentPage], Color.White);
            switch (currentPage)
            {
                case GameMenuPages.EquipmentPage:
                    #region Equipment page
                    sb.End();
                    sb.GraphicsDevice.SetRenderTarget(characterPanelRight);
                    sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                    sb.Draw(menuTextureSheet, new Rectangle(0, 0, 1366, 768), bigMenuBGs[(int)currentPage], Color.White);
                    foreach (var item in lcpp)
                    {
                        if (selectedCharacterEquipment == null)
                        {
                            item.Draw(sb);
                        }
                        else if (PlayerSaveData.heroParty.IndexOf(selectedCharacterEquipment) == lcpp.IndexOf(item))
                        {
                            item.Draw(sb);
                        }
                        else if (PlayerSaveData.heroParty.IndexOf(selectedCharacterEquipment) != lcpp.IndexOf(item))
                        {
                            item.Draw(sb, .3f);
                        }

                    }
                    sb.End();
                    sb.GraphicsDevice.SetRenderTarget(menuRender);
                    sb.GraphicsDevice.Clear(Color.Black);
                    sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                    sb.Draw(GameProcessor.lastDrawnMapRender, new Rectangle(0, 0, 1366, 768), Color.White * .5f);

                    sb.Draw(menuTextureSheet, new Rectangle(0, 0, 1366, 768), bigMenuBGs[(int)currentPage], Color.White);
                    sb.Draw(characterPanelRight, new Rectangle(60 * 2, 70 * 2, 255 * 2 - 4, 275 * 2 - 4), new Rectangle(60 * 2, 70 * 2, 255 * 2 - 4, 275 * 2 - 4), Color.White);
                    sb.Draw(menuTextureSheet, upperScrollBarPosition, upperScrollBarSource, Color.White);
                    sb.Draw(menuTextureSheet, lengthScrollBarPosition, lengthScrollBarSource, Color.White);
                    sb.Draw(menuTextureSheet, new Rectangle(middleScrollBarPosition.X, middleScrollBarPosition.Y + verticalPositionOffset, middleScrollBarPosition.Width, middleScrollBarPosition.Height), middleScrollBarSource, Color.White);
                    sb.Draw(menuTextureSheet, lowerScrollBarPosition, lowerScrollBarSource, Color.White);

                    sb.Draw(menuTextureSheet, EquipmentPagePanelPosition, EquipmentPagePanelSource, Color.White);
                    sb.DrawString(itemNameFont, EquipmentPagePanelTest, EquipmentPagePanelTextPosition, Color.White);

                    if (bWeaponTab)
                    {
                        sb.Draw(menuTextureSheet, equipmentSelectionPosition, equipmentWeaponSelectionSource, Color.White);
                    }
                    else if (!bWeaponTab)
                    {
                        sb.Draw(menuTextureSheet, equipmentSelectionPosition, equipmentArmourSelectionSource, Color.White);
                    }

                    if (selectedCharacterEquipment != null && equipmentItems.Count != 0)
                    {
                        int temp = 0;
                        foreach (var item in equipmentItems)
                        {
                            item.itemTexAndAnimation.Draw(sb, equipmentItemBoxes[temp]);
                            temp++;
                        }

                        if (selectedEquipmentPiece != null && equipmentItems.IndexOf(selectedEquipmentPiece) != -1)
                        {
                            sb.Draw(menuTextureSheet, equipmentItemBoxes[equipmentItems.IndexOf(selectedEquipmentPiece)], selectedItemHighlighterSource, Color.White);
                        }


                    }

                    if (selectedEquipmentPiece != null && selectedCharacterEquipment != null)
                    {
                        equipmentDisplay.Draw(sb);
                    }

                    if (bDisplayOptions)
                    {
                        EquipmentOptionDisplay.Draw(sb);
                        sb.Draw(menuTextureSheet, EquipmentOptionDisplay.choiceBoxes[equipmentOptionSelectionIndex], selectedItemHighlighterSource, Color.White);
                    }



                    #endregion
                    break;
                case GameMenuPages.ItemsPage:
                    #region Items page
                    if (!bChooseCharacterToUseOn)
                    {
                       
                        sb.Draw(menuTextureSheet, upperTrayPosition, upperTraySource, Color.White);
                        sb.Draw(menuTextureSheet, lowerTrayPosition, lowerTraySource, Color.White);

                        var upperItems = PlayerSaveData.playerInventory.localInventory;
                        List<BaseItem> onlyTheseItemsToConsider = new List<BaseItem>();
                        onlyTheseItemsToConsider.AddRange(upperItems.FindAll(i => upperItems.IndexOf(i) >= upperCurrentIndex * 30 && upperItems.IndexOf(i) < (upperCurrentIndex + 1) * 30));
                        foreach (var item in onlyTheseItemsToConsider)
                        {
                            if (item.itemTexAndAnimation.animationTexture != null)
                            {
                                item.itemTexAndAnimation.Draw(sb, upperItemPositions[onlyTheseItemsToConsider.IndexOf(item)]);
                                sb.DrawString(Game1.defaultFont, "x" + item.itemAmount, upperItemPositions[onlyTheseItemsToConsider.IndexOf(item)].Location.ToVector2() + new Vector2(40, 50), Color.White);
                            }
                            else
                            {
                            }
                        }

                        sb.Draw(menuTextureSheet, redBannerPosition, redBannerSource, Color.White);


                        sb.Draw(menuTextureSheet, blueBannerPosition, blueBannerSource, Color.White);

                        if (bDisplayOptions)
                        {
                            ItemOptionDisplay.Draw(sb);
                            sb.Draw(menuTextureSheet, ItemOptionDisplay.choiceBoxes[itemOptionSelectionIndex], selectedItemHighlighterSource, Color.White);
                        }
                        if (itemPageSelectedItem != null)
                        {
                            sb.Draw(menuTextureSheet, selectedItemHighlighterPosition, selectedItemHighlighterSource, Color.White);
                        }
                    }
                    else
                    {
                        sb.End();
                        sb.GraphicsDevice.SetRenderTarget(characterPanelRight);
                        sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                        sb.Draw(menuTextureSheet, new Rectangle(0, 0, 1366, 768), bigMenuBGs[(int)currentPage], Color.White);
                        sb.Draw(menuTextureSheet, charPanelBGPosition, charPanelBGSource, Color.White);
                        foreach (var item in lcpp)
                        {
                            if (selectedCharacterItems == null)
                            {
                                item.Draw(sb);
                            }
                            else if (PlayerSaveData.heroParty.IndexOf(selectedCharacterItems) == lcpp.IndexOf(item))
                            {
                                item.Draw(sb);
                            }
                            else if (PlayerSaveData.heroParty.IndexOf(selectedCharacterItems) != lcpp.IndexOf(item))
                            {
                                item.Draw(sb, .3f);
                            }
                        }
                        sb.End();

                        sb.GraphicsDevice.SetRenderTarget(menuRender);
                        sb.GraphicsDevice.Clear(Color.Black);
                        sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                        sb.Draw(GameProcessor.lastDrawnMapRender, new Rectangle(0, 0, 1366, 768), Color.White * .5f);

                        sb.Draw(menuTextureSheet, new Rectangle(0, 0, 1366, 768), bigMenuBGs[(int)currentPage], Color.White);
                        sb.Draw(characterPanelRight, new Rectangle(60 * 2, 70 * 2, 255 * 2 - 4, 275 * 2 - 4), new Rectangle(60 * 2, 70 * 2, 255 * 2 - 4, 275 * 2 - 4), Color.White);
                        sb.Draw(menuTextureSheet, upperScrollBarPosition, upperScrollBarSource, Color.White);
                        sb.Draw(menuTextureSheet, lengthScrollBarPosition, lengthScrollBarSource, Color.White);
                        sb.Draw(menuTextureSheet, new Rectangle(middleScrollBarPosition.X, middleScrollBarPosition.Y + verticalPositionOffset, middleScrollBarPosition.Width, middleScrollBarPosition.Height), middleScrollBarSource, Color.White);
                        sb.Draw(menuTextureSheet, lowerScrollBarPosition, lowerScrollBarSource, Color.White);
                    }


                    if (itemPageSelectedItem != null)
                    {

                        itemPageSelectedItem.itemTexAndAnimation.Draw(sb, selectedItemFramePosition);
                        // sb.DrawString(itemNameFont, selectedItemName, itemNameSpecificPosition, Color.White); 
                        TextUtility.Draw(sb, selectedItemName, itemNameFont, ItemNamePosition, TextUtility.OutLining.Center, Color.White, 0f);
                        TextUtility.Draw(sb, selectedItemDescription, itemDescriptionFont, selectedItemDescriptionPosition, TextUtility.OutLining.Center, Color.White, 0f, false);
                        TextUtility.Draw(sb, selectedItemShortDescription, itemNameFont, selectedItemShortDescriptionPosition, TextUtility.OutLining.Center, Color.White, 0f, false);
                        //        sb.DrawString(itemDescriptionFont, selectedItemDescription, selectedItemDescriptionPosition.Location.ToVector2(), Color.White);
                        //      sb.DrawString(itemNameFont, selectedItemShortDescription, selectedItemShortDescriptionPosition.Location.ToVector2(), Color.White);
                    }
                    #endregion
                    break;
                case GameMenuPages.QuestPage:
                    #region Quest page
                    #endregion
                    break;
                case GameMenuPages.CharactersPage:
                    #region Characters page

                    if (selectedCharacterContext == null)
                    {
                        sb.End();
                        sb.GraphicsDevice.SetRenderTarget(characterPageRender);
                        sb.GraphicsDevice.Clear(Color.TransparentBlack);
                        sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, characterTabAdjustedMatrix);

                        var tempRect = new Rectangle(100, 140, 571 * 2, 272 * 2);
                        foreach (var item in charTabList.FindAll(ct => ct.Contains(new Rectangle(0, -(int)characterTabVerticalModifier, 571 * 2, 272 * 2))))
                        {
                            item.Draw(sb, characterTabAdjustedMatrix);
                        }


                        sb.End();
                        sb.GraphicsDevice.SetRenderTarget(menuRender);
                        sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                        sb.Draw(GameProcessor.lastDrawnMapRender, new Rectangle(0, 0, 1366, 768), Color.White * .5f);
                        sb.Draw(menuTextureSheet, new Rectangle(0, 0, 1366, 768), bigMenuBGs[(int)currentPage], Color.White);
                        sb.Draw(characterPageRender, tempRect, Color.White);
                    }
                    else
                    {
                        selectedCharacterContext.Draw(sb);
                        sb.End();
                        sb.GraphicsDevice.SetRenderTarget(menuRender);
                        sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                        sb.Draw(GameProcessor.lastDrawnMapRender, new Rectangle(0, 0, 1366, 768), Color.White * .5f);
                        sb.Draw(menuTextureSheet, new Rectangle(0, 0, 1366, 768), bigMenuBGs[(int)currentPage], Color.White);
                        sb.Draw(selectedCharacterContext.getRender(), new Rectangle(0, 0, 1366, 768), Color.White);
                    }

                    #endregion
                    break;
                case GameMenuPages.MapPage:
                    #region Map page
                    #endregion
                    break;
                default:
                    break;
            }
            BattleGUI.DrawCursor(sb, KeyboardMouseUtility.uiMousePos.ToVector2(), GameProcessor.zoom);
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            return menuRender;
        }

        internal struct equipmentDisplay
        {
            static Vector2 location = Vector2.Zero;
            static Point frameSize = new Point(76 * 2 + 10, 76 * 2 + 50);
            static Rectangle frameSource = new Rectangle(88, 679, 68, 68);
            static Rectangle position = new Rectangle();

            static Point equipmentNameBoxOffset = new Point(10);
            static Point equipmentNameBoxSize = new Point(76 * 2 - equipmentNameBoxOffset.X, 32);
            static Rectangle equipmentNamePosition = new Rectangle();

            static Point statsNameBoxOffset = new Point(10, 60);
            static Point statsNameBoxSize = new Point(76, 28);
            static Rectangle statsNamePosition = new Rectangle();

            static Point statsBoxOffset = new Point(10, 95);
            static Point statsBoxSize = new Point(76 * 2 - equipmentNameBoxOffset.X, 97);
            static Rectangle statsPosition = new Rectangle();

            static float verticalOffSet = 0f;
            static RenderTarget2D statboxRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 76 * 2 - equipmentNameBoxOffset.X, 97);
            static RenderTarget2D tempRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

            static List<statChange> statChanges = new List<statChange>();

            struct statChange
            {
                String statChangeString;
                enum valueChange { Increase = 0, Decrease, Equal }
                ValueType valueChangeType;
                int statDifference;

                Point valueSize;
                Point valueLocation;
                Rectangle valuePosition;

                Point valueDiffSize;
                Point valueDiffLocation;
                Rectangle valueDiffPosition;

                Point valueDiffNumSize;
                Point valueDiffNumLocation;
                Rectangle valueDiffNumPosition;

                static List<Rectangle> valueChangeTypeSource = new List<Rectangle> { new Rectangle(3415, 64, 9, 13), new Rectangle(3415, 77, 9, 13), new Rectangle(3415, 90, 9, 13) };

                public statChange(int index, String text, int difference)
                {
                    statChangeString = "";
                    valueChangeType = valueChange.Increase;
                    statDifference = 0;

                    valueSize = new Point(60, 28);
                    valueLocation = new Point(60, 28);
                    valuePosition = new Rectangle();

                    valueDiffSize = new Point(20, 26);
                    valueDiffLocation = new Point(60, 28);
                    valueDiffPosition = new Rectangle();

                    valueDiffNumSize = new Point(25, 28);
                    valueDiffNumLocation = new Point(20, 20);
                    valueDiffNumPosition = new Rectangle();

                    Generate(index, text, difference);
                }

                public void Generate(int index, String text, int difference)
                {
                    statChangeString = text;
                    statDifference = difference;
                    if (difference == 0)
                    {
                        valueChangeType = valueChange.Equal;
                    }
                    else if (difference > 0)
                    {
                        valueChangeType = valueChange.Increase;
                    }
                    else if (difference < 0)
                    {
                        valueChangeType = valueChange.Decrease;
                    }

                    valueLocation = new Point(10, 10) + (new Point(0, (10 + 20) * index));
                    valuePosition = new Rectangle(valueLocation, valueSize);

                    valueDiffLocation = new Point(10 + 60 + 15, 12) + new Point(0, (10 + 20) * index);
                    valueDiffPosition = new Rectangle(valueDiffLocation, valueDiffSize);

                    valueDiffNumLocation = new Point(10 + 60 + 15 + 10 + 15, 10) + new Point(0, (10 + 20) * index);
                    valueDiffNumPosition = new Rectangle(valueDiffNumLocation, valueDiffNumSize);
                    //sb.Draw(Game1.hitboxHelp, new Rectangle(position.Location + new Point(20, 105), new Point(80, 20)), Color.White);
                    //sb.Draw(Game1.hitboxHelp, new Rectangle(position.Location + new Point(20, 105) + new Point(0, 10 + 20), new Point(80, 20)), Color.White);
                    //sb.Draw(Game1.hitboxHelp, new Rectangle(position.Location + new Point(20, 105) + new Point(0, 10 * 2 + 20 * 2), new Point(80, 20)), Color.White);

                    //sb.Draw(menuTextureSheet, new Rectangle(position.Location + new Point(20 + 80 + 15, 105), new Point(14, 20)), new Rectangle(3415, 64, 9, 13), Color.White);
                    //sb.Draw(menuTextureSheet, new Rectangle(position.Location + new Point(20 + 80 + 15, 105) + new Point(0, 10 + 20), new Point(14, 20)), new Rectangle(3415, 64, 9, 13), Color.White);
                    //sb.Draw(menuTextureSheet, new Rectangle(position.Location + new Point(20 + 80 + 15, 105) + new Point(0, 10 * 2 + 20 * 2), new Point(14, 20)), new Rectangle(3415, 64, 9, 13), Color.White);
                }

                public void Draw(SpriteBatch sb)
                {
                    // sb.Draw(Game1.hitboxHelp, new Rectangle(valuePosition.X, valuePosition.Y + (int)verticalOffSet, valuePosition.Width, valuePosition.Height), Color.White);
                    TextUtility.Draw(sb, statChangeString, BattleGUI.testSF25, new Rectangle(valuePosition.X, valuePosition.Y, valuePosition.Width, valuePosition.Height), TextUtility.OutLining.Right, Color.White, 0f, false);
                    sb.Draw(menuTextureSheet, new Rectangle(valueDiffPosition.X, valueDiffPosition.Y + (int)verticalOffSet, valueDiffPosition.Width, valueDiffPosition.Height), valueChangeTypeSource[(int)valueChangeType], Color.White);

                    //  sb.Draw(Game1.hitboxHelp, new Rectangle(valueDiffNumPosition.X, valueDiffNumPosition.Y + (int)verticalOffSet, valueDiffNumPosition.Width, valueDiffNumPosition.Height), Color.White);
                    if (statDifference > 0)
                    {
                        TextUtility.Draw(sb, "+" + statDifference.ToString(), BattleGUI.testSF25, new Rectangle(valueDiffNumPosition.X, valueDiffNumPosition.Y, valueDiffNumPosition.Width, valueDiffNumPosition.Height), TextUtility.OutLining.Right, Color.Green, 0f, false);
                    }
                    else if (statDifference < 0)
                    {
                        TextUtility.Draw(sb, statDifference.ToString(), BattleGUI.testSF25, new Rectangle(valueDiffNumPosition.X, valueDiffNumPosition.Y, valueDiffNumPosition.Width, valueDiffNumPosition.Height), TextUtility.OutLining.Right, Color.Red, 0f, false);
                    }
                    else if (statDifference == 0)
                    {
                        TextUtility.Draw(sb, "=", BattleGUI.testSF25, new Rectangle(valueDiffPosition.X, valueDiffPosition.Y + (int)verticalOffSet, valueDiffPosition.Width, valueDiffPosition.Height), TextUtility.OutLining.Center, Color.White, 0f, true);
                        TextUtility.Draw(sb, statDifference.ToString(), BattleGUI.testSF25, new Rectangle(valueDiffNumPosition.X, valueDiffNumPosition.Y, valueDiffNumPosition.Width, valueDiffNumPosition.Height), TextUtility.OutLining.Right, Color.White, 0f, false);
                    }
                }
            }

            static public void Generate(int index)
            {
                statChanges.Clear();
                verticalOffSet = 0f;
                int test = index % 6;
                if (test <= 3)
                {
                    position = new Rectangle(selectedItemHighlighterPosition.Location + new Point(76, 5), frameSize);
                }
                else
                {
                    position = new Rectangle(selectedItemHighlighterPosition.Location + new Point(-frameSize.X, 5), frameSize);
                }

                equipmentNamePosition = new Rectangle(position.Location + equipmentNameBoxOffset, equipmentNameBoxSize);
                statsNamePosition = new Rectangle(position.Location + statsNameBoxOffset, statsNameBoxSize);
                statsPosition = new Rectangle(statsBoxOffset, statsBoxSize);

                var temp = selectedEquipmentPiece.Differencewith(selectedEquipmentPieceCharacterPanel);
                int index2 = 0;
                foreach (var item in temp)
                {
                    statChanges.Add(new statChange(index2, item.Key, item.Value));
                    index2++;
                }
            }

            static public void Draw(SpriteBatch sb)
            {
                sb.End();
                //statboxRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 76 * 2 - equipmentNameBoxOffset.X, 97);
                sb.GraphicsDevice.SetRenderTarget(statboxRender);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                //sb.Draw(menuTextureSheet, new Rectangle(0, 0, 142, 97), frameSource, Color.Silver);

                if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                {
                    verticalOffSet -= 0.5f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                {
                    verticalOffSet += 0.5f;
                }

                foreach (var item in statChanges)
                {
                    item.Draw(sb);
                }

                sb.End();
                sb.GraphicsDevice.SetRenderTarget(tempRender);
                sb.GraphicsDevice.SetRenderTarget(tempRender);
                sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                sb.Draw(menuRender, menuRender.Bounds, Color.White);

                sb.Draw(menuTextureSheet, position, frameSource, Color.White);
                // sb.Draw(Game1.hitboxHelp, equipmentNamePosition, Color.White);
                TextUtility.Draw(sb, selectedEquipmentPiece.itemName, BattleGUI.testSF32, equipmentNamePosition, TextUtility.OutLining.Center, Color.White, 0f, false);
                // sb.Draw(Game1.hitboxHelp, statsNamePosition, Color.White);
                TextUtility.Draw(sb, "Stats:", BattleGUI.testSF32, statsNamePosition, TextUtility.OutLining.Left, Color.White, 0f, false);

                sb.Draw(Game1.hitboxHelp, new Rectangle(position.Location + new Point(20, 105), new Point(80, 20)), Color.White);
                sb.Draw(Game1.hitboxHelp, new Rectangle(position.Location + new Point(20, 105) + new Point(0, 10 + 20), new Point(80, 20)), Color.White);
                sb.Draw(Game1.hitboxHelp, new Rectangle(position.Location + new Point(20, 105) + new Point(0, 10 * 2 + 20 * 2), new Point(80, 20)), Color.White);

                sb.Draw(menuTextureSheet, new Rectangle(position.Location + new Point(20 + 80 + 15, 105), new Point(14, 20)), new Rectangle(3415, 64, 9, 13), Color.White);
                sb.Draw(menuTextureSheet, new Rectangle(position.Location + new Point(20 + 80 + 15, 105) + new Point(0, 10 + 20), new Point(14, 20)), new Rectangle(3415, 64, 9, 13), Color.White);
                sb.Draw(menuTextureSheet, new Rectangle(position.Location + new Point(20 + 80 + 15, 105) + new Point(0, 10 * 2 + 20 * 2), new Point(14, 20)), new Rectangle(3415, 64, 9, 13), Color.White);

                sb.Draw(menuTextureSheet, new Rectangle(statsPosition.Location + position.Location - new Point(3), new Point(142, 97) + new Point(3 * 2)), frameSource, Color.Silver);
                sb.Draw(statboxRender, new Rectangle(statsPosition.Location + position.Location, statsPosition.Size), statboxRender.Bounds, Color.White);

                sb.End();
                sb.GraphicsDevice.SetRenderTarget(menuRender);
                sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                sb.Draw(tempRender, tempRender.Bounds, Color.White);
            }


        }

        internal class CharacterTabdisplay
        {
            internal Rectangle tabLocation;
            Rectangle portraitFrameLocation;
            Rectangle portraitLocation;
            Rectangle nameLocation;
            BaseCharacter bc;
            static Matrix defaultCamera = Matrix.CreateTranslation(new Vector3(0, 0, 1));

            Rectangle classLocation;
            List<statNameAmount> lsa;
            Rectangle specialStats;
            RenderTarget2D tabRender;
            int vertHeightOffset;

            class statNameAmount
            {
                String name;
                String amount;
                internal Rectangle positionName;
                internal Rectangle positionAmount;
                static Point startPos = new Point(210, 20);
                static Point nameSize = new Point(90, 24);
                static Point amountSize = new Point(50, 24);



                internal statNameAmount(int index, Point tabPos, STATChart sc, bool bIsPassive)
                {
                    if (bIsPassive)
                    {
                        name = ((STATChart.PASSIVESTATSNames)index).ToString();
                        amount = sc.currentPassiveStats[index].ToString();

                        if (index < 5)
                        {
                            positionName = new Rectangle(tabPos + startPos + new Point(0, index * (nameSize.Y + 10)), nameSize);
                            positionAmount = new Rectangle(tabPos + startPos + new Point(nameSize.X + 10, index * (nameSize.Y + 10)), nameSize);
                        }
                        else if (index < 10)
                        {
                            index -= 5;
                            positionName = new Rectangle(tabPos + startPos + new Point(150, 0) + new Point(0, index * (nameSize.Y + 10)), nameSize);
                            positionAmount = new Rectangle(tabPos + startPos + new Point(150, 0) + new Point(nameSize.X + 10, index * (nameSize.Y + 10)), nameSize);
                        }
                        else
                        {
                            index -= 10;
                            positionName = new Rectangle(tabPos + startPos + new Point(150 * 2 + 70, 0) + new Point(0, index * (nameSize.Y + 10)), nameSize);
                            positionAmount = new Rectangle(tabPos + startPos + new Point(150 * 2 + 70, 0) + new Point(nameSize.X + 10, index * (nameSize.Y + 10)), nameSize);
                        }
                    }
                    else
                    {
                        name = ((STATChart.SPECIALSTATSNames)(index - 12)).ToString();
                        amount = sc.currentSpecialStats[(index - 12)].ToString();

                        if (index < 15)
                        {
                            index -= 10;
                            positionName = new Rectangle(tabPos + startPos + new Point(150 * 2 + 70, 0) + new Point(0, index * (nameSize.Y + 10)), nameSize);
                            positionAmount = new Rectangle(tabPos + startPos + new Point(150 * 2 + 70, 0) + new Point(nameSize.X + 10, index * (nameSize.Y + 10)), nameSize);
                        }
                        else
                        {
                            index -= 15;
                            positionName = new Rectangle(tabPos + startPos + new Point(150 * 3 + 70 * 2, 0) + new Point(0, index * (nameSize.Y + 10)), nameSize);
                            positionAmount = new Rectangle(tabPos + startPos + new Point(150 * 3 + 70 * 2, 0) + new Point(nameSize.X + 10, index * (nameSize.Y + 10)), nameSize);
                        }
                    }
                }

                internal void Draw(SpriteBatch sb, Matrix camera)
                {
                    //sb.Draw(Game1.hitboxHelp, positionName, Color.Red);
                    TextUtility.Draw(sb, name, BattleGUI.testSF25, positionName, TextUtility.OutLining.Right, Color.White, 0f, false, camera, Color.Black, false);

                    //sb.Draw(Game1.hitboxHelp, positionAmount, Color.Blue);
                    sb.Draw(menuTextureSheet, positionAmount, InfoBoxSource, Color.White);
                    TextUtility.Draw(sb, amount, BattleGUI.testSF25, positionAmount, TextUtility.OutLining.Center, Color.White, 0f, false, camera, Color.Black, false);
                }

            }

            internal void RecalculateStats()
            {
                lsa = new List<statNameAmount>();

                int index2 = 0;
                var temp = bc.trueSTATChartOutsideCombat();
                foreach (var item in temp.currentPassiveStats)
                {

                    lsa.Add(new statNameAmount(index2, tabLocation.Location, temp, true));
                    index2++;
                }

                specialStats = new Rectangle(lsa[lsa.Count - 1].positionName.Location + new Point(0, (index2 % 5) * (24 + 10)), new Point(190, 24));
                //positionName = new Rectangle(tabPos + startPos + new Point(150 * 2 + 70, 0) + new Point(0, index * (nameSize.Y + 10)), nameSize);

                index2++;
                foreach (var item in temp.currentSpecialStats)
                {

                    lsa.Add(new statNameAmount(index2, tabLocation.Location, temp, false));
                    index2++;
                }

            }

            internal CharacterTabdisplay(int index, BaseCharacter c)
            {
                bc = c;
                int offSet = BigPanelSource.Height * 2 * index + 50 * index;
                vertHeightOffset = offSet;
                tabLocation = new Rectangle(firstTabLocation + new Point(0, offSet), ((BigPanelSource.Size).ToVector2() * 2).ToPoint());
                portraitFrameLocation = new Rectangle(tabLocation.Location + portraitOffSet, ((PortraitPanelSource.Size).ToVector2() * 2).ToPoint());
                portraitLocation = new Rectangle(tabLocation.Location + portraitOffSet + new Point(6), ((PortraitPanelSource.Size - new Point(6)).ToVector2() * 2).ToPoint());
                nameLocation = new Rectangle(tabLocation.Location + nameOffSet, ((NamePanelSource.Size).ToVector2() * 2).ToPoint());
                classLocation = new Rectangle(portraitFrameLocation.Location + new Point(0, 100), portraitFrameLocation.Size + new Point(0, -100));
                lsa = new List<statNameAmount>();

                int index2 = 0;
                var temp = c.trueSTATChartOutsideCombat();
                foreach (var item in temp.currentPassiveStats)
                {

                    lsa.Add(new statNameAmount(index2, tabLocation.Location, temp, true));
                    index2++;
                }

                specialStats = new Rectangle(lsa[lsa.Count - 1].positionName.Location + new Point(0, (index2 % 5) * (24 + 10)), new Point(190, 24));
                //positionName = new Rectangle(tabPos + startPos + new Point(150 * 2 + 70, 0) + new Point(0, index * (nameSize.Y + 10)), nameSize);

                index2++;
                foreach (var item in temp.currentSpecialStats)
                {

                    lsa.Add(new statNameAmount(index2, tabLocation.Location, temp, false));
                    index2++;
                }

                tabRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, tabLocation.Width, tabLocation.Height + 50);
            }

            internal void Draw(SpriteBatch sb, Matrix camera = default(Matrix))
            {
                if (camera == default(Matrix))
                {
                    camera = defaultCamera;
                }

                sb.Draw(menuTextureSheet, tabLocation, BigPanelSource, Color.White);
                sb.Draw(menuTextureSheet, portraitFrameLocation, PortraitPanelSource, Color.White);
                bc.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(sb, portraitLocation);
                sb.Draw(menuTextureSheet, nameLocation, NamePanelSource, Color.White);
                TextUtility.Draw(sb, bc.CCC.equippedClass.ClassName + " Lvl." + (bc.CCC.equippedClass.classEXP.classLevel + 1).ToString(), BattleGUI.testSF25, nameLocation, TextUtility.OutLining.Center, Color.White, 0f, false, camera);
                sb.Draw(menuTextureSheet, classLocation, InfoBoxSource, Color.White);
                TextUtility.Draw(sb, bc.displayName, BattleGUI.testSF25, classLocation, TextUtility.OutLining.Center, Color.White, 0f, false, camera);

                foreach (var item in lsa)
                {
                    item.Draw(sb, camera);
                }
                //sb.Draw(Game1.hitboxHelp, specialStats, Color.Blue);
                TextUtility.Draw(sb, "SPECIAL STATS:", BattleGUI.testSF25, specialStats, TextUtility.OutLining.Center, Color.SaddleBrown, 0f, false, camera,Color.Gold,false);

                // sb.Draw(Game1.WhiteTex, new Rectangle(800, 130, 180, 24),Color.White);
                Rectangle r = new Rectangle(lsa.Last().positionName.X, lsa.Last().positionName.Y + 28, lsa.Last().positionName.Width * 2, lsa.Last().positionName.Height);
                TextUtility.Draw(sb, "Experience until level up:", BattleGUI.testSF25, r, TextUtility.OutLining.Left, Color.SaddleBrown, 0f, false, camera, Color.Gold, false);
                r = new Rectangle(r.Location + new Point(0, 28), r.Size);
                TextUtility.Draw(sb, (bc.CCC.equippedClass.classEXP.ExpRequirementCurrentLevel() - bc.CCC.equippedClass.classEXP.expTillNextLevel) + @"/" + bc.CCC.equippedClass.classEXP.ExpRequirementCurrentLevel().ToString(), BattleGUI.testSF25, r, TextUtility.OutLining.Center, Color.SaddleBrown, 0f, false, camera, Color.Gold, false);
            }

            internal bool Contains(Rectangle r)
            {
                if (tabLocation.Contains(r) || tabLocation.Intersects(r))
                {
                    return true;
                }
                return false;
            }

            internal void GenerateRender(SpriteBatch sb)
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(tabRender);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

                Matrix m = Matrix.CreateTranslation(new Vector3(0, -vertHeightOffset, 1));

                Rectangle r = new Rectangle(tabLocation.X, tabLocation.Y - vertHeightOffset, tabLocation.Width, tabLocation.Height);
                sb.Draw(menuTextureSheet, r, BigPanelSource, Color.White);
                r = new Rectangle(portraitFrameLocation.X, portraitFrameLocation.Y - vertHeightOffset, portraitFrameLocation.Width, portraitFrameLocation.Height);
                sb.Draw(menuTextureSheet, r, PortraitPanelSource, Color.White);
                r = new Rectangle(portraitLocation.X, portraitLocation.Y - vertHeightOffset, portraitLocation.Width, portraitLocation.Height);
                bc.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(sb, r);
                r = new Rectangle(nameLocation.X, nameLocation.Y - vertHeightOffset, nameLocation.Width, nameLocation.Height);
                sb.Draw(menuTextureSheet, r, NamePanelSource, Color.White);
                TextUtility.Draw(sb, bc.CCC.equippedClass.ClassName + " Lvl." + (bc.CCC.equippedClass.classEXP.classLevel + 1).ToString(), BattleGUI.testSF25, nameLocation, TextUtility.OutLining.Center, Color.White, 0f, false, m);
                r = new Rectangle(classLocation.X, classLocation.Y - vertHeightOffset, classLocation.Width, classLocation.Height);
                sb.Draw(menuTextureSheet, classLocation, InfoBoxSource, Color.White);
                TextUtility.Draw(sb, bc.displayName, BattleGUI.testSF25, classLocation, TextUtility.OutLining.Center, Color.White, 0f, false, m);

                foreach (var item in lsa)
                {
                    item.Draw(sb, m);
                }
                //sb.Draw(Game1.hitboxHelp, specialStats, Color.Blue);
                TextUtility.Draw(sb, "SPECIAL STATS:", BattleGUI.testSF25, specialStats, TextUtility.OutLining.Center, Color.SaddleBrown, 0f, false, m,Color.Gold,false);


                r = new Rectangle(lsa.Last().positionName.X, lsa.Last().positionName.Y + 28, lsa.Last().positionName.Width * 2, lsa.Last().positionName.Height);
                TextUtility.Draw(sb, "Experience until level up:", BattleGUI.testSF25, r, TextUtility.OutLining.Left, Color.SaddleBrown, 0f, false, m, Color.Gold, false);
                r = new Rectangle(r.Location + new Point(0, 28), r.Size);
                TextUtility.Draw(sb, (bc.CCC.equippedClass.classEXP.ExpRequirementCurrentLevel() - bc.CCC.equippedClass.classEXP.expTillNextLevel) + @"/" + bc.CCC.equippedClass.classEXP.ExpRequirementCurrentLevel().ToString(), BattleGUI.testSF25, r, TextUtility.OutLining.Center, Color.SaddleBrown, 0f, false, m, Color.Gold, false);


                sb.End();
                sb.GraphicsDevice.SetRenderTarget(null);
            }

            internal RenderTarget2D GetRender()
            {
                return tabRender;
            }

            internal bool ContainsMouse(Point p)
            {
                if (!new Rectangle(100, 140, 571 * 2, 272 * 2).Contains(KeyboardMouseUtility.uiMousePos))
                {
                    return false;
                }
                if (tabLocation.Contains(p))
                {
                    return true;
                }
                return false;
            }

            internal int horizontalActualOffSet()
            {
                return -(0 + vertHeightOffset + (int)GameMenuHandler.characterTabVerticalModifier);
            }

            internal BaseCharacter getCharacter()
            {
                return bc;
            }
        }

        static internal bool CharacterTabContainsMouse()
        {
            var tempMouse = new Point(KeyboardMouseUtility.uiMousePos.X - 100, (int)(KeyboardMouseUtility.uiMousePos.Y - 140 - characterTabVerticalModifier));
            for (int i = 0; i < charTabList.Count; i++)
            {
                if (charTabList[i].ContainsMouse(tempMouse))
                {
                    return true;
                }
            }
            return false;
        }

        static internal CharacterTabdisplay selectedCharacterTab()
        {
            var tempMouse = new Point(KeyboardMouseUtility.uiMousePos.X - 100, (int)(KeyboardMouseUtility.uiMousePos.Y - 140 - characterTabVerticalModifier));
            for (int i = 0; i < charTabList.Count; i++)
            {
                if (charTabList[i].ContainsMouse(tempMouse))
                {
                    return charTabList[i];
                }
            }
            return default(CharacterTabdisplay);
        }
    }

    internal class CharacterContextMenu
    {
        internal enum CharacterContextMenuType { None, Select, AbilityLineUp, ClassLineUp, ClassPointLineUp }

        internal CharacterContextMenuType currentCharContext = CharacterContextMenuType.None;
        bool bStartingCharacterContextSelect = false;
        static TimingUtility transitionTimer;
        int steps = 60;
        int stepsTaken = 0;
        Vector2 drawingPos = new Vector2();
        Vector2 deltaV = Vector2.Zero;
        BaseCharacter bc;
        CharacterTabdisplay tabDisplay;
        internal AbilityLineupInfo abilityLineupInfo;
        internal ClassLineupInfo classLineupInfo;
        internal ClassPointLineupInfo classPointLineupInfo;

        static Rectangle RightPanelRenderDrawOnScreen;
        static RenderTarget2D RightPanelRender;
        static RenderTarget2D tempRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D render = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static SpriteFont ButtonFont = BattleGUI.testSF48;

        static Texture2D guiTex = GameMenuHandler.menuTextureSheet;
        static Rectangle bgPanelSource = new Rectangle(88, 679, 68, 68);

        static TexPanel bigBGPanel;
        static Rectangle bigBGPabelLoc;

        static TextTexPanel bgPanelRight;
        static TextTexPanel bgPanelLevelUp;
        static TextTexPanel bgPanelClassEdit;
        static TextTexPanel bgPanelAbilityEdit;
        static Rectangle rightPanelPosition = new Rectangle(700, 300, 616, 418);
        static Rectangle levelUpPanelPosition = new Rectangle(50, 570, 600, 148);
        static Rectangle abilityPanelPosition = new Rectangle(50, 300, 275, 220);
        static Rectangle classPanelPosition = new Rectangle(375, 300, 275, 220);
        static GameText levelUpButtonText;
        static GameText levelUpButtonTextAddition;
        static GameText abilityButtonText;
        static GameText classButtonText;

        static TexPanel selectedPanel = null;
        static bool initialize = false;
        static TimingUtility abilityLineUpStartTimer;
        static TimingUtility rightPanelTimerUnselect;

        static void Initialize()
        {
            BattleGUI.InitializeResources();
            initialize = true;
            guiTex = GameMenuHandler.menuTextureSheet;
            int bigPanelOffset = 35;
            bigBGPabelLoc = new Rectangle(bigPanelOffset, bigPanelOffset, 1366 - bigPanelOffset * 2, 768 - bigPanelOffset * 2);
            bigBGPanel = new TextTexPanel(guiTex, bigBGPabelLoc, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

            bgPanelRight = new TextTexPanel(guiTex, rightPanelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
            bgPanelLevelUp = new TextTexPanel(guiTex, levelUpPanelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
            bgPanelClassEdit = new TextTexPanel(guiTex, classPanelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
            bgPanelAbilityEdit = new TextTexPanel(guiTex, abilityPanelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
           // guiTex = GameMenuHandler.menuTextureSheet;
            ButtonFont = BattleGUI.testSF25;
            levelUpButtonText = new GameText();
            levelUpButtonText.AddText("Check talent customization", GameText.Language.English);

            levelUpButtonTextAddition = new GameText();
            levelUpButtonTextAddition.AddText("Points to spend: ", GameText.Language.English);

            abilityButtonText = new GameText();
            abilityButtonText.AddText("Check/Change\n ability lineup", GameText.Language.English);

            classButtonText = new GameText();
            classButtonText.AddText("Check/Change\n Class", GameText.Language.English);

            bgPanelLevelUp.Setup(levelUpButtonText, ButtonFont, Color.Silver, Color.DarkGray, TextUtility.OutLining.Center, true);
            bgPanelClassEdit.Setup(classButtonText, ButtonFont, Color.Silver, Color.DarkGray, TextUtility.OutLining.Center, true);
            bgPanelAbilityEdit.Setup(abilityButtonText, ButtonFont, Color.Silver, Color.DarkGray, TextUtility.OutLining.Center, true);

            int offsetPanel = 2;
            RightPanelRenderDrawOnScreen = new Rectangle(rightPanelPosition.Location + new Point(offsetPanel), rightPanelPosition.Size - new Point(offsetPanel * 2));
            RightPanelRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, RightPanelRenderDrawOnScreen.Width, RightPanelRenderDrawOnScreen.Height);

        }

        internal CharacterContextMenu(float vertMod, Point pos, Point newPos, CharacterTabdisplay tab)
        {

            if (!initialize)
            {
                Initialize();
            }



            selectedPanel = null;
            drawingPos = new Vector2(pos.X - 30, (float)(tab.tabLocation.Y + 140 + (int)GameMenuHandler.characterTabVerticalModifier));
            Point delta = new Point(-(newPos.X - (int)drawingPos.X), pos.Y - 20 - (int)(drawingPos.Y));
            deltaV = delta.ToVector2() / 60;
            bStartingCharacterContextSelect = true;
            transitionTimer = new TimingUtility(10, true, StopTimerWhen);
            tabDisplay = tab;
            int count = 0;
            for (int i = 0; i < tabDisplay.getCharacter().CCC.charClassList.Count; i++)
            {
                count += tabDisplay.getCharacter().CCC.charClassList[i].classPoints.points;
            }

            bgPanelLevelUp.SetTextAddition("\n" + levelUpButtonTextAddition.getText() + count.ToString());
        }

        internal void Update(GameTime gt)
        {
            switch (currentCharContext)
            {
                case CharacterContextMenuType.None:
                    transitionTimer.Tick(gt);

                    while (transitionTimer.Ding())
                    {
                        drawingPos += deltaV;
                        stepsTaken++;
                    }
                    break;
                case CharacterContextMenuType.Select:

                    bgPanelLevelUp.Update(gt);
                    bgPanelAbilityEdit.Update(gt);
                    bgPanelClassEdit.Update(gt);
                    break;
                case CharacterContextMenuType.AbilityLineUp:
                    if (abilityLineUpStartTimer != null && abilityLineUpStartTimer.IsActive())
                    {
                        abilityLineUpStartTimer.Tick(gt);
                    }
                    abilityLineupInfo.Update(gt);
                    break;
                case CharacterContextMenuType.ClassLineUp:
                    if (abilityLineUpStartTimer != null && !abilityLineUpStartTimer.IsDone())
                    {
                        abilityLineUpStartTimer.Tick(gt);
                    }
                    classLineupInfo.Update(gt);

                    break;
                case CharacterContextMenuType.ClassPointLineUp:
                    if (abilityLineUpStartTimer != null && !abilityLineUpStartTimer.IsDone())
                    {
                        abilityLineUpStartTimer.Tick(gt);
                    }
                    classPointLineupInfo.Update(gt);
                    break;
                default:
                    break;
            }

        }

        internal bool StopTimerWhen()
        {
            if (stepsTaken >= steps)
            {
                bStartingCharacterContextSelect = false;
                currentCharContext = CharacterContextMenuType.Select;
                return true;
            }
            return false;
        }

        internal bool StopAbilityLineupStartTimerWhen()
        {
            if (abilityLineUpStartTimer.percentageDone() == 1.0f)
            {
                return true;
            }
            return false;
        }

        internal void Draw(SpriteBatch sb)
        {
            tabDisplay.GenerateRender(sb);
            GenerateRightPanelRender(sb);
            switch (currentCharContext)
            {
                case CharacterContextMenuType.None:
                    sb.End();
                    sb.GraphicsDevice.SetRenderTarget(render);
                    sb.GraphicsDevice.Clear(Color.TransparentBlack);
                    sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                    sb.Draw(Game1.WhiteTex, new Rectangle(0, 0, 1366, 768), Color.DarkBlue);


                    DrawFadeInBGElements(sb);
                    sb.Draw(tabDisplay.GetRender(), drawingPos, Color.White);

                    sb.End();

                    break;
                case CharacterContextMenuType.Select:
                    DrawSelectContext(sb);
                    break;
                case CharacterContextMenuType.AbilityLineUp:
                    abilityLineupInfo.GenerateRenders(sb);
                    DrawAbilityLineup(sb);

                    break;
                case CharacterContextMenuType.ClassLineUp:
                    classLineupInfo.Draw(sb);
                    DrawClassLineup(sb);

                    break;
                case CharacterContextMenuType.ClassPointLineUp:
                    classPointLineupInfo.Draw(sb);
                    DrawClassPointLineup(sb);
                    break;
                default:
                    break;
            }
        }

        private void DrawClassPointLineup(SpriteBatch sb)
        {
            if (abilityLineUpStartTimer.IsActive())
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(tempRender);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                sb.Draw(Game1.WhiteTex, new Rectangle(0, 0, 1366, 768), Color.DarkBlue);
                DrawFadeInBGElements(sb);
                // sb.Draw(tabDisplay.GetRender(), drawingPos, Color.White);

                sb.End();
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            sb.Draw(Game1.WhiteTex, new Rectangle(0, 0, 1366, 768), Color.CornflowerBlue);
            bigBGPanel.Draw(sb, Color.LightSkyBlue);

            if (abilityLineUpStartTimer.IsActive() && abilityLineUpStartTimer.percentageDone() != 1.0f)
            {
                sb.Draw(tempRender, new Rectangle(0, 0, 1366, 768), Color.White * (1.0f - abilityLineUpStartTimer.percentageDone()));
            }

            //sb.Draw(tabDisplay.GetRender(), drawingPos, Color.White);

            sb.Draw(classPointLineupInfo.getRender(), Vector2.Zero, Color.White);

            sb.End();
        }

        private void DrawClassLineup(SpriteBatch sb)
        {
            if (abilityLineUpStartTimer.IsActive())
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(tempRender);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                sb.Draw(Game1.WhiteTex, new Rectangle(0, 0, 1366, 768), Color.DarkBlue);
                DrawFadeInBGElements(sb);
                // sb.Draw(tabDisplay.GetRender(), drawingPos, Color.White);

                sb.End();
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            sb.Draw(Game1.WhiteTex, new Rectangle(0, 0, 1366, 768), Color.CornflowerBlue);
            bigBGPanel.Draw(sb, Color.LightSkyBlue);

            if (abilityLineUpStartTimer.IsActive() && abilityLineUpStartTimer.percentageDone() != 1.0f)
            {
                sb.Draw(tempRender, new Rectangle(0, 0, 1366, 768), Color.White * (1.0f - abilityLineUpStartTimer.percentageDone()));
            }

            sb.Draw(tabDisplay.GetRender(), drawingPos, Color.White);

            sb.Draw(classLineupInfo.getRender(), Vector2.Zero, Color.White);

            sb.End();
        }

        internal void DrawSelectContext(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            sb.Draw(Game1.WhiteTex, new Rectangle(0, 0, 1366, 768), Color.DarkBlue);
            bigBGPanel.Draw(sb, Color.LightSkyBlue);

            DrawFadeInBGElements(sb);
            sb.Draw(tabDisplay.GetRender(), drawingPos, Color.White);

            sb.End();
        }

        internal void DrawAbilityLineup(SpriteBatch sb)
        {
            if (abilityLineUpStartTimer.IsActive())
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(tempRender);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                sb.Draw(Game1.WhiteTex, new Rectangle(0, 0, 1366, 768), Color.DarkBlue);
                DrawFadeInBGElements(sb);
                //sb.Draw(tabDisplay.GetRender(), drawingPos, Color.White);

                sb.End();
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            sb.Draw(Game1.WhiteTex, new Rectangle(0, 0, 1366, 768), Color.CornflowerBlue);
            bigBGPanel.Draw(sb, Color.LightSkyBlue);

            if (abilityLineUpStartTimer.IsActive() && abilityLineUpStartTimer.percentageDone() != 1.0f)
            {
                sb.Draw(tempRender, new Rectangle(0, 0, 1366, 768), Color.White * (1.0f - abilityLineUpStartTimer.percentageDone()));
            }

            sb.Draw(tabDisplay.GetRender(), drawingPos, Color.White);

            if (abilityLineUpStartTimer.IsActive() && abilityLineUpStartTimer.percentageDone() != 1.0f)
            {
                sb.Draw(abilityLineupInfo.getRender(), Vector2.Zero, Color.White * abilityLineUpStartTimer.percentageDone());
            }
            else
            {
                sb.Draw(abilityLineupInfo.getRender(), Vector2.Zero, Color.White);
            }



            sb.End();
        }

        private void DrawFadeInBGElements(SpriteBatch sb)
        {
            float opacity = (float)(stepsTaken / (float)steps);
            bigBGPanel.Draw(sb, Color.LightSkyBlue * opacity);
            bgPanelRight.Draw(sb, Color.White * (float)(stepsTaken / (float)steps));


            sb.Draw(RightPanelRender, RightPanelRenderDrawOnScreen, Color.White);



            bgPanelLevelUp.Draw(sb, Color.White * (float)(stepsTaken / (float)steps), opacity);
            // TextUtility.Draw(sb, levelUpButtonText.getText() + "\n" + levelUpButtonTextAddition.getText() + "0", ButtonFont, bgPanelLevelUp.Position(), TextUtility.OutLining.Center, Color.Silver * opacity, 1f, false, default(Matrix), Color.DarkGray * opacity, false);
            bgPanelAbilityEdit.Draw(sb, Color.White * (float)(stepsTaken / (float)steps), opacity);
            // TextUtility.Draw(sb, abilityButtonText.getText(), ButtonFont, bgPanelAbilityEdit.Position(), TextUtility.OutLining.Center, Color.Silver * opacity, 1f, false, default(Matrix), Color.DarkGray * opacity, false);
            bgPanelClassEdit.Draw(sb, Color.White * (float)(stepsTaken / (float)steps), opacity);
            // TextUtility.Draw(sb, classButtonText.getText(), ButtonFont, bgPanelClassEdit.Position(), TextUtility.OutLining.Center, Color.Silver * opacity, 1f, false, default(Matrix), Color.DarkGray * opacity, false);
        }

        private void DrawRightPanelStuff(SpriteBatch sb)
        {
            if (selectedPanel == bgPanelAbilityEdit)
            {

            }
            else if (selectedPanel == bgPanelClassEdit)
            {
                DrawClassStatOverview(sb);
            }
            else if (selectedPanel == bgPanelLevelUp)
            {
                DrawClassPointOverview(sb);
            }
        }

        struct statDisplay
        {
            internal String s;
            internal Rectangle pos;

            internal statDisplay(String s, Rectangle pos)
            {
                this.s = s;
                this.pos = pos;
            }
        }

        private void DrawClassStatOverview(SpriteBatch sb)
        {
            int renderOffSet = 15;
            int offsetWidth = 10;
            Rectangle tab = new Rectangle(renderOffSet, 64, (RightPanelRender.Width / 2) - offsetWidth - renderOffSet, 32);
            tabDisplay.getCharacter().CCC.equippedClass.GenerateStatUp();
            var sc = tabDisplay.getCharacter().CCC.equippedClass.statUp;
            List<statDisplay> statsToShow = new List<statDisplay>();

            statsToShow.Add(new statDisplay("Carried over stats: ", new Rectangle(renderOffSet, 0, RightPanelRender.Width, 48)));

            //for (int i = 0; i < sc.currentActiveStats.Count; i++)
            //{
            //    statsToShow.Add(new statDisplay(((STATChart.ACTIVESTATSNames)i).ToString() + ": " + sc.currentActiveStats[i].ToString(), new Rectangle(tab.Location + new Point(0, tab.Height * i), tab.Size)));
            //}

            for (int i = 0; i < sc.currentPassiveStats.Count; i++)
            {
                statsToShow.Add(new statDisplay(((STATChart.PASSIVESTATSNames)i).ToString() + ": " + sc.currentPassiveStats[i].ToString(), new Rectangle(tab.Location + new Point(0, tab.Height * i), tab.Size)));
            }

            for (int i = 0; i < sc.currentSpecialStats.Count; i++)
            {
                statsToShow.Add(new statDisplay(((STATChart.SPECIALSTATSNames)i).ToString() + ": " + sc.currentSpecialStats[i].ToString(), new Rectangle(tab.Location + new Point((tab.Width + offsetWidth), tab.Height * i), tab.Size)));
            }

            for (int i = 0; i < statsToShow.Count; i++)
            {
                TextUtility.Draw(sb, statsToShow[i].s, ButtonFont, statsToShow[i].pos, TextUtility.OutLining.Left, Color.White, 1f, true, default(Matrix), Color.Silver, false);
            }
        }

        private void DrawClassPointOverview(SpriteBatch sb)
        {
            var temp = tabDisplay.getCharacter().CCC.getAvailableClasses();
            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].classPoints.parentClass = temp[i];
                String s = temp[i].classPoints.ToString();
                int offSet = 10;
                int distance = 15;
                Rectangle r = new Rectangle(offSet, offSet + (distance + 48) * i, RightPanelRender.Width - offSet * 2, 48);
                TextUtility.Draw(sb, s, ButtonFont, r, TextUtility.OutLining.Left, Color.Silver, 1f, true, default(Matrix), Color.Gray, false);
            }
        }

        private void GenerateRightPanelRender(SpriteBatch sb)
        {
            if (selectedPanel != null)
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(RightPanelRender);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

                DrawRightPanelStuff(sb);

                sb.End();
                sb.GraphicsDevice.SetRenderTarget(null);
            }

        }

        internal RenderTarget2D getRender()
        {
            return render;
        }

        internal void HandleMouseOver()
        {
            switch (currentCharContext)
            {
                case CharacterContextMenuType.None:
                    break;
                case CharacterContextMenuType.Select:
                    bgPanelLevelUp.ContainsMouse(KeyboardMouseUtility.uiMousePos, Color.White);
                    bgPanelAbilityEdit.ContainsMouse(KeyboardMouseUtility.uiMousePos, Color.White);
                    bgPanelClassEdit.ContainsMouse(KeyboardMouseUtility.uiMousePos, Color.White);

                    if (bgPanelLevelUp.IsSelected())
                    {
                        selectedPanel = bgPanelLevelUp;
                    }
                    else if (bgPanelAbilityEdit.IsSelected())
                    {
                        selectedPanel = bgPanelAbilityEdit;
                    }
                    else if (bgPanelClassEdit.IsSelected())
                    {
                        selectedPanel = bgPanelClassEdit;
                    }
                    else
                    {
                        selectedPanel = null;
                    }
                    break;
                case CharacterContextMenuType.AbilityLineUp:
                    break;
                case CharacterContextMenuType.ClassPointLineUp:
                    classPointLineupInfo.HandleMouseMove();
                    break;
                default:
                    break;
            }

        }

        internal void HandleConfirm()
        {
            switch (currentCharContext)
            {
                case CharacterContextMenuType.None:
                    break;
                case CharacterContextMenuType.Select:
                    if (selectedPanel != null)
                    {
                        SelectButtonsConfirm();
                    }

                    break;
                case CharacterContextMenuType.AbilityLineUp:
                    break;
                default:
                    break;
            }
        }

        private void SelectButtonsConfirm()
        {
            if (selectedPanel == bgPanelAbilityEdit)
            {
                currentCharContext = CharacterContextMenuType.AbilityLineUp;
                abilityLineUpStartTimer = new TimingUtility(30, true, StopAbilityLineupStartTimerWhen);
                abilityLineUpStartTimer.SetStepTimer(45, 0);
                abilityLineupInfo = new AbilityLineupInfo(tabDisplay.getCharacter());
            }

            if (selectedPanel == bgPanelClassEdit)
            {
                currentCharContext = CharacterContextMenuType.ClassLineUp;
                abilityLineUpStartTimer = new TimingUtility(30, true, StopAbilityLineupStartTimerWhen);
                abilityLineUpStartTimer.SetStepTimer(45, 0);
                classLineupInfo = new ClassLineupInfo(tabDisplay.getCharacter());
            }

            if (selectedPanel == bgPanelLevelUp)
            {
                currentCharContext = CharacterContextMenuType.ClassPointLineUp;
                abilityLineUpStartTimer = new TimingUtility(30, true, StopAbilityLineupStartTimerWhen);
                abilityLineUpStartTimer.SetStepTimer(45, 0);
                classPointLineupInfo = new ClassPointLineupInfo(tabDisplay.getCharacter());
            }
        }

        internal void Close()
        {
            switch (currentCharContext)
            {
                case CharacterContextMenuType.None:
                    break;
                case CharacterContextMenuType.Select:
                    //GameMenuHandler.selectedCharacterContext.currentCharContext = CharacterContextMenu.CharacterContextMenuType.Select;
                    break;
                case CharacterContextMenuType.AbilityLineUp:
                    GameMenuHandler.selectedCharacterContext.currentCharContext = CharacterContextMenu.CharacterContextMenuType.Select;
                    abilityLineupInfo.Close();
                    break;
                case CharacterContextMenuType.ClassLineUp:
                    GameMenuHandler.selectedCharacterContext.currentCharContext = CharacterContextMenu.CharacterContextMenuType.Select;
                    classLineupInfo.Close();
                    break;
                default:
                    break;
            }
        }

    }

    static public class ItemOptionDisplay
    {
        static Texture2D displayTex;
        enum parts
        {
            LUC = 0, UW, RUC,
            LH, RH,
            LLC, LW, RLC,
            MIDDLE
        }
        static List<Rectangle> sources = new List<Rectangle> { new Rectangle(3424, 64, 3, 3), new Rectangle(3428,64,32,3),new Rectangle(3461,64,3,3),
        new Rectangle(3424,68,3,32),new Rectangle(3424,68,3,32),
        new Rectangle(3424,101,3,3), new Rectangle(3428,101,32,3), new Rectangle(3461,101,3,3),
        new Rectangle(3428,68,32,32)};
        static int neededWidth = 32;
        static int neededHeight = 32;
        static List<Rectangle> originalPositions = new List<Rectangle> { new Rectangle(0, 0, 3*2, 3*2), new Rectangle(3*2,0,neededWidth*2,3*2),new Rectangle(3*2+neededWidth*2,0,3*2,3*2),
        new Rectangle(0,3*2,3*2,neededHeight*2),new Rectangle(3*2+neededWidth*2,3*2,3*2,neededHeight*2),
        new Rectangle(0,3*2+neededHeight*2,3*2,3*2), new Rectangle(3*2,3*2+neededHeight*2,neededWidth*2,3*2), new Rectangle(3*2+neededWidth*2,3*2+neededHeight*2,3*2,3*2),
        new Rectangle(3*2,3*2,neededWidth*2,neededHeight*2)};

        static List<Rectangle> newPositions = new List<Rectangle> { new Rectangle(0, 0, 3*2, 3*2), new Rectangle(3*2,0,neededWidth*2,3*2),new Rectangle(3*2+neededWidth*2,0,3*2,3*2),
        new Rectangle(0,3*2,3*2,neededHeight*2),new Rectangle(3*2+neededWidth*2,3*2,3*2,neededHeight*2),
        new Rectangle(0,3*2+neededHeight*2,3*2,3*2), new Rectangle(3*2,3*2+neededHeight*2,neededWidth*2,3*2), new Rectangle(3*2+neededWidth*2,3*2+neededHeight*2,3*2,3*2),
        new Rectangle(3*2,3*2,neededWidth*2,neededHeight*2)};

        static List<String> wordsToShow = new List<string>();
        static int distanceBetweenWordsVertical = 25;

        static SpriteFont itemOptionsFont;

        public static List<Rectangle> choiceBoxes = new List<Rectangle>();

        public static void Start(Texture2D tex, SpriteFont optionsFont)
        {
            displayTex = tex;
            itemOptionsFont = optionsFont;
        }

        public static void GenerateLocations(Point location, BaseItem bi)
        {
            wordsToShow.Clear();
            choiceBoxes.Clear();

            switch (bi.itemType)
            {
                case BaseItem.ITEM_TYPES.Consumables:
                    wordsToShow = new List<string> { "Use", "Drop", "Back" };
                    break;
                case BaseItem.ITEM_TYPES.Equipment:
                    wordsToShow = new List<string> { "Drop", "Back" };
                    break;
                case BaseItem.ITEM_TYPES.Quest_Item:
                    wordsToShow = new List<string> { "Back" };
                    break;
                case BaseItem.ITEM_TYPES.Generic:
                    break;
                case BaseItem.ITEM_TYPES.Materials:
                    wordsToShow = new List<string> { "Drop", "Back" };
                    break;
                default:
                    break;
            }

            neededWidth = itemOptionsFont.MeasureString("Drop").ToPoint().X + 32;
            neededHeight = itemOptionsFont.MeasureString("Drop").ToPoint().Y + 16 * 2 + (wordsToShow.Count - 1) * 16;



            newPositions[(int)parts.LUC] = new Rectangle(location.X, location.Y, 3 * 2, 3 * 2);
            newPositions[(int)parts.UW] = new Rectangle(location.X + 3 * 2, location.Y, neededWidth, 3 * 2);
            newPositions[(int)parts.RUC] = new Rectangle(location.X + 3 * 2 + neededWidth, location.Y, 3 * 2, 3 * 2);

            newPositions[(int)parts.LH] = new Rectangle(location.X, location.Y + 3 * 2, 3 * 2, neededHeight);
            newPositions[(int)parts.RH] = new Rectangle(location.X + 3 * 2 + neededWidth, location.Y + 3 * 2, 3 * 2, neededHeight);

            newPositions[(int)parts.LLC] = new Rectangle(location.X, 3 * 2 + location.Y + neededHeight, 3 * 2, 3 * 2);
            newPositions[(int)parts.LW] = new Rectangle(location.X + 3 * 2, 3 * 2 + location.Y + neededHeight, neededWidth, 3 * 2);
            newPositions[(int)parts.RLC] = new Rectangle(location.X + 3 * 2 + neededWidth, 3 * 2 + location.Y + neededHeight, 3 * 2, 3 * 2);

            newPositions[(int)parts.MIDDLE] = new Rectangle(location.X + 3 * 2, location.Y + 3 * 2, neededWidth, neededHeight);


            foreach (var item in wordsToShow)
            {
                choiceBoxes.Add(new Rectangle(newPositions[(int)parts.UW].X, newPositions[(int)parts.UW].Y + 1 * 16 + (wordsToShow.IndexOf(item)) * distanceBetweenWordsVertical, neededWidth, distanceBetweenWordsVertical));
                //sb.DrawString(itemOptionsFont, item, new Vector2(newPositions[(int)parts.UW].X, newPositions[(int)parts.UW].Y + 1 * 16 + (wordsToShow.IndexOf(item)) * distanceBetweenWordsVertical), Color.White);
            }
        }

        public static void Draw(SpriteBatch sb, float opacityModifier = 1f)
        {
            foreach (var item in newPositions)
            {
                sb.Draw(displayTex, item, sources[newPositions.IndexOf(item)], Color.White * opacityModifier);
            }

            foreach (var item in wordsToShow)
            {
                if (wordsToShow.IndexOf(item) == GameMenuHandler.itemOptionSelectionIndex)
                {
                    sb.Draw(displayTex, choiceBoxes[GameMenuHandler.itemOptionSelectionIndex], sources[(int)parts.MIDDLE], Color.Gray * opacityModifier);
                }
                int modifier = 16;
                if (item.Equals("use", StringComparison.OrdinalIgnoreCase))
                {
                    modifier = 20;
                }
                sb.DrawString(itemOptionsFont, item, new Vector2(newPositions[(int)parts.UW].X + modifier, newPositions[(int)parts.UW].Y + 1 * 16 + (wordsToShow.IndexOf(item)) * distanceBetweenWordsVertical), Color.White * opacityModifier);
            }
        }
    }

    static public class EquipmentOptionDisplay
    {
        static Texture2D displayTex;
        enum parts
        {
            LUC = 0, UW, RUC,
            LH, RH,
            LLC, LW, RLC,
            MIDDLE
        }
        static List<Rectangle> sources = new List<Rectangle> { new Rectangle(3424, 64, 3, 3), new Rectangle(3428,64,32,3),new Rectangle(3461,64,3,3),
        new Rectangle(3424,68,3,32),new Rectangle(3424,68,3,32),
        new Rectangle(3424,101,3,3), new Rectangle(3428,101,32,3), new Rectangle(3461,101,3,3),
        new Rectangle(3428,68,32,32)};
        static int neededWidth = 32;
        static int neededHeight = 32;
        static List<Rectangle> originalPositions = new List<Rectangle> { new Rectangle(0, 0, 3*2, 3*2), new Rectangle(3*2,0,neededWidth*2,3*2),new Rectangle(3*2+neededWidth*2,0,3*2,3*2),
        new Rectangle(0,3*2,3*2,neededHeight*2),new Rectangle(3*2+neededWidth*2,3*2,3*2,neededHeight*2),
        new Rectangle(0,3*2+neededHeight*2,3*2,3*2), new Rectangle(3*2,3*2+neededHeight*2,neededWidth*2,3*2), new Rectangle(3*2+neededWidth*2,3*2+neededHeight*2,3*2,3*2),
        new Rectangle(3*2,3*2,neededWidth*2,neededHeight*2)};

        static List<Rectangle> newPositions = new List<Rectangle> { new Rectangle(0, 0, 3*2, 3*2), new Rectangle(3*2,0,neededWidth*2,3*2),new Rectangle(3*2+neededWidth*2,0,3*2,3*2),
        new Rectangle(0,3*2,3*2,neededHeight*2),new Rectangle(3*2+neededWidth*2,3*2,3*2,neededHeight*2),
        new Rectangle(0,3*2+neededHeight*2,3*2,3*2), new Rectangle(3*2,3*2+neededHeight*2,neededWidth*2,3*2), new Rectangle(3*2+neededWidth*2,3*2+neededHeight*2,3*2,3*2),
        new Rectangle(3*2,3*2,neededWidth*2,neededHeight*2)};

        static List<String> wordsToShow = new List<string>();
        static int distanceBetweenWordsVertical = 25;

        static SpriteFont itemOptionsFont;

        public static List<Rectangle> choiceBoxes = new List<Rectangle>();

        public static void Start(Texture2D tex, SpriteFont optionsFont)
        {
            displayTex = tex;
            itemOptionsFont = optionsFont;
        }

        public static void GenerateLocations(Point location, BaseEquipment bi)
        {
            wordsToShow.Clear();
            choiceBoxes.Clear();

            switch (bi.itemType)
            {
                case BaseItem.ITEM_TYPES.Equipment:
                    wordsToShow = new List<string> { "Equip", "Back" };
                    break;
            }

            neededWidth = itemOptionsFont.MeasureString("Drop").ToPoint().X + 32;
            neededHeight = itemOptionsFont.MeasureString("Drop").ToPoint().Y + 16 * 2 + (wordsToShow.Count - 1) * 16;



            newPositions[(int)parts.LUC] = new Rectangle(location.X, location.Y, 3 * 2, 3 * 2);
            newPositions[(int)parts.UW] = new Rectangle(location.X + 3 * 2, location.Y, neededWidth, 3 * 2);
            newPositions[(int)parts.RUC] = new Rectangle(location.X + 3 * 2 + neededWidth, location.Y, 3 * 2, 3 * 2);

            newPositions[(int)parts.LH] = new Rectangle(location.X, location.Y + 3 * 2, 3 * 2, neededHeight);
            newPositions[(int)parts.RH] = new Rectangle(location.X + 3 * 2 + neededWidth, location.Y + 3 * 2, 3 * 2, neededHeight);

            newPositions[(int)parts.LLC] = new Rectangle(location.X, 3 * 2 + location.Y + neededHeight, 3 * 2, 3 * 2);
            newPositions[(int)parts.LW] = new Rectangle(location.X + 3 * 2, 3 * 2 + location.Y + neededHeight, neededWidth, 3 * 2);
            newPositions[(int)parts.RLC] = new Rectangle(location.X + 3 * 2 + neededWidth, 3 * 2 + location.Y + neededHeight, 3 * 2, 3 * 2);

            newPositions[(int)parts.MIDDLE] = new Rectangle(location.X + 3 * 2, location.Y + 3 * 2, neededWidth, neededHeight);


            foreach (var item in wordsToShow)
            {
                choiceBoxes.Add(new Rectangle(newPositions[(int)parts.UW].X, newPositions[(int)parts.UW].Y + 1 * 16 + (wordsToShow.IndexOf(item)) * distanceBetweenWordsVertical, neededWidth, distanceBetweenWordsVertical));
                //sb.DrawString(itemOptionsFont, item, new Vector2(newPositions[(int)parts.UW].X, newPositions[(int)parts.UW].Y + 1 * 16 + (wordsToShow.IndexOf(item)) * distanceBetweenWordsVertical), Color.White);
            }
        }

        public static void Draw(SpriteBatch sb, float opacityModifier = 1f)
        {
            foreach (var item in newPositions)
            {
                sb.Draw(displayTex, item, sources[newPositions.IndexOf(item)], Color.White * opacityModifier);
            }

            foreach (var item in wordsToShow)
            {
                if (wordsToShow.IndexOf(item) == GameMenuHandler.equipmentOptionSelectionIndex)
                {
                    sb.Draw(displayTex, choiceBoxes[GameMenuHandler.equipmentOptionSelectionIndex], sources[(int)parts.MIDDLE], Color.Gray * opacityModifier);
                }
                int modifier = 16;
                if (item.Equals("use", StringComparison.OrdinalIgnoreCase))
                {
                    modifier = 20;
                }
                sb.DrawString(itemOptionsFont, item, new Vector2(newPositions[(int)parts.UW].X + modifier, newPositions[(int)parts.UW].Y + 1 * 16 + (wordsToShow.IndexOf(item)) * distanceBetweenWordsVertical), Color.White * opacityModifier);
            }
        }
    }

    public class characterPagePanel
    {
        enum characterPanelElements { Portrait = 0, Name, ActiveStatsPanel, EquipmentPanel, HPBG, MPBG }
        List<Rectangle> charPanelSources = new List<Rectangle> { new Rectangle(88, 679, 68, 68), new Rectangle(0, 679, 88, 28), new Rectangle(156, 679, 148, 57), new Rectangle(304, 679, 232, 86), new Rectangle(232, 737, 48, 6), new Rectangle(232, 753, 48, 6) };
        public enum characterPanelPositions { PortraitPanel = 0, Portrait, NamePanel, ActiveStatPanel, HealthBar, ManaBar, LowerPanel, ArmourPanel, WeaponPanel }
        public List<Rectangle> charPanelPositions = new List<Rectangle> { new Rectangle(10 * 2, 0, 68 * 2, 68 * 2), new Rectangle(12 * 2, 2 * 2, 64 * 2, 64 * 2), new Rectangle(77 * 2, 2 * 2, 88 * 2, 28 * 2), new Rectangle(77 * 2, 12 * 2, 148 * 2, 57 * 2), new Rectangle(106 * 2, 38 * 2, 48 * 2, 6 * 2), new Rectangle(106 * 2, 54 * 2, 48 * 2, 6 * 2), new Rectangle(0, 38 * 2, 232 * 2, 86 * 2), new Rectangle(104 * 2, 58 * 2, 61 * 2, 61 * 2), new Rectangle(167 * 2, 58 * 2, 61 * 2, 61 * 2) };
        enum characterPanelTextElements
        {
            Name = 0, Level,
            HP, MP,
            Health, Mana,
            BaseHit, Crit, ClassName
        }
        List<Point> textPositionOffSet = new List<Point> { new Point(96 * 2, 7 * 2), new Point(175 * 2, 17 * 2),
         new Point(84 * 2, 35 * 2), new Point(84 * 2, 51 * 2),
          new Point(159 * 2, 35 * 2), new Point(159 * 2, 51 * 2),
           new Point(19 * 2, 70 * 2), new Point(19 * 2, 87 * 2), new Point(19 * 2, 101 * 2) };
        List<Rectangle> textPosition = new List<Rectangle> { new Rectangle(96 * 2, 7 * 2,50,50), new Rectangle(55 * 2, 17 * 2,50,50),
        new Rectangle(84 * 2, 32 * 2,50,50), new Rectangle(84 * 2, 48 * 2,50,50),
         new Rectangle(159 * 2, 32 * 2,50,50), new Rectangle(159 * 2, 48 * 2,50,50),
          new Rectangle(19 * 2, 70 * 2,50,50), new Rectangle(19 * 2, 87 * 2,50,50), new Rectangle(19 * 2, 101 * 2,50,50) };
        List<String> panelText = new List<string> { "Name", "Level ", "H.P.", "M.P.", "1/1", "1/1", "Hit %", "Crit %", "CLASS" };

        List<Rectangle> charPanelPositionsOffSet = new List<Rectangle> { new Rectangle(10 * 2, 0, 68 * 2, 68 * 2), new Rectangle(12 * 2, 2 * 2, 64 * 2, 64 * 2), new Rectangle(77 * 2, 2 * 2, 88 * 2, 28 * 2), new Rectangle(77 * 2, 12 * 2, 148 * 2, 57 * 2), new Rectangle(106 * 2, 38 * 2, 48 * 2, 6 * 2), new Rectangle(106 * 2, 54 * 2, 48 * 2, 6 * 2), new Rectangle(0, 38 * 2, 232 * 2, 86 * 2), new Rectangle(104 * 2, 58 * 2, 61 * 2, 61 * 2), new Rectangle(167 * 2, 58 * 2, 61 * 2, 61 * 2) };

        internal Vector2 Location = new Vector2();
        Vector2 characterFrameOffSet = new Vector2(0 * 2, 10 * 2);
        Vector2 characterLowerPanelOffSet = new Vector2(0 * 2, 38 * 2);
        Vector2 characterNameOffSet = new Vector2(77 * 2, 2 * 2);
        Vector2 characterMiddlePanelOffSet = new Vector2(77 * 2, 12 * 2);

        Rectangle characterPortrait = new Rectangle(12 * 2, 2 * 2, 64 * 2, 64 * 2);
        Vector2 characterNamePosition = new Vector2(78 * 2, 6 * 2);
        Vector2 characterLowerStatsPosition = new Vector2(20 * 2, 71 * 2);
        Rectangle armourPanel = new Rectangle(104 * 2, 58 * 2, 61 * 2, 61 * 2);
        Rectangle weaponPanel = new Rectangle(167 * 2, 58 * 2, 61 * 2, 61 * 2);

        BaseCharacter character;
        Texture2D panelTex;
        static public SpriteFont font;

        public characterPagePanel(BaseCharacter bc, Texture2D panelT, Point testPosition)
        {
            panelTex = panelT;
            character = bc;

            charPanelPositions.Clear();
            textPosition.Clear();
            foreach (var r in charPanelPositionsOffSet)
            {
                charPanelPositions.Add(new Rectangle(r.X + testPosition.X, r.Y + testPosition.Y, r.Width, r.Height));
            }
            int index = 0;
            foreach (var p in textPositionOffSet)
            {
                switch (index)
                {
                    case 0:
                        textPosition.Add(new Rectangle(p.X + testPosition.X, p.Y + testPosition.Y, 125, 32));
                        break;
                    case 1:
                        textPosition.Add(new Rectangle(p.X + testPosition.X, p.Y + testPosition.Y, 85, 32));
                        break;
                    case 2:
                        textPosition.Add(new Rectangle(p.X + testPosition.X, p.Y + testPosition.Y, 43, 25));
                        break;
                    case 3:
                        textPosition.Add(new Rectangle(p.X + testPosition.X, p.Y + testPosition.Y, 43, 25));
                        break;
                    case 4:
                        textPosition.Add(new Rectangle(p.X + testPosition.X, p.Y + testPosition.Y, 117, 25));
                        break;
                    case 5:
                        textPosition.Add(new Rectangle(p.X + testPosition.X, p.Y + testPosition.Y, 117, 25));
                        break;
                    case 6:
                        textPosition.Add(new Rectangle(p.X + testPosition.X, p.Y + testPosition.Y, 117, 25));
                        break;
                    case 7:
                        textPosition.Add(new Rectangle(p.X + testPosition.X, p.Y + testPosition.Y, 117, 25));
                        break;
                    case 8:
                        textPosition.Add(new Rectangle(p.X + testPosition.X, p.Y + testPosition.Y, 117, 25));
                        break;

                    default:
                        textPosition.Add(new Rectangle(p.X + testPosition.X, p.Y + testPosition.Y, 50, 32));
                        break;
                }

                index++;
            }

            panelText[(int)characterPanelTextElements.Name] = bc.displayName;
            panelText[(int)characterPanelTextElements.ClassName] = bc.CCC.equippedClass.ClassName;
            panelText[(int)characterPanelTextElements.Health] = bc.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP].ToString() + @"/" + bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP].ToString();
            panelText[(int)characterPanelTextElements.Mana] = bc.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.MANA].ToString() + @"/" + bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXMANA].ToString();
            panelText[(int)characterPanelTextElements.Level] += bc.CCC.equippedClass.currentLevel() + 1;
            //     charPanelPositions.ForEach(r=> { r = new Rectangle(r.X + testPosition.X, r.Y + testPosition.Y, r.Width, r.Height); });
        }

        public void Draw(SpriteBatch sb, float opacity = 1f)
        {


            sb.Draw(panelTex, charPanelPositions[(int)characterPanelPositions.LowerPanel], charPanelSources[(int)characterPanelElements.EquipmentPanel], Color.White * opacity);

            if (GameMenuHandler.selectedCharacterEquipment != null && GameMenuHandler.selectedCharacterEquipment == character)
            {
                if (GameMenuHandler.selectedEquipmentPieceCharacterPanel != null)
                {
                    switch (GameMenuHandler.selectedEquipmentPieceCharacterPanel.EquipType)
                    {
                        case BaseEquipment.EQUIP_TYPES.Weapon:
                            sb.Draw(Game1.hitboxHelp, new Rectangle(charPanelPositions[(int)characterPanelPositions.WeaponPanel].Location + new Point(0, 16), charPanelPositions[(int)characterPanelPositions.WeaponPanel].Size - new Point(-3, 16)), Color.White * .7f);
                            break;
                        case BaseEquipment.EQUIP_TYPES.Armor:
                            sb.Draw(Game1.hitboxHelp, new Rectangle(charPanelPositions[(int)characterPanelPositions.ArmourPanel].Location + new Point(0, 16), charPanelPositions[(int)characterPanelPositions.ArmourPanel].Size - new Point(-3, 16)), Color.White * .7f);
                            break;
                        default:
                            break;
                    }

                }
            }


            if (character.weapon != null)
            {
                character.weapon.itemTexAndAnimation.Draw(sb, charPanelPositions[(int)characterPanelPositions.WeaponPanel], opacity);
            }

            if (character.armour != null)
            {
                character.armour.itemTexAndAnimation.Draw(sb, charPanelPositions[(int)characterPanelPositions.ArmourPanel], opacity);
            }

            sb.Draw(panelTex, charPanelPositions[(int)characterPanelPositions.ActiveStatPanel], charPanelSources[(int)characterPanelElements.ActiveStatsPanel], Color.White * opacity);
            sb.Draw(panelTex, charPanelPositions[(int)characterPanelPositions.HealthBar], charPanelSources[(int)characterPanelElements.HPBG], Color.White * opacity);
            sb.Draw(panelTex, charPanelPositions[(int)characterPanelPositions.ManaBar], charPanelSources[(int)characterPanelElements.MPBG], Color.White * opacity);


            sb.Draw(panelTex, charPanelPositions[(int)characterPanelPositions.NamePanel], charPanelSources[(int)characterPanelElements.Name], Color.White * opacity);

            sb.Draw(panelTex, charPanelPositions[(int)characterPanelPositions.PortraitPanel], charPanelSources[(int)characterPanelElements.Portrait], Color.White * opacity);
            character.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(sb, charPanelPositions[(int)characterPanelPositions.Portrait], opacity);


            foreach (var item in textPosition)
            {
                // sb.Draw(Game1.hitboxHelp, item, Color.White);
                // sb.DrawString(font, panelText[textPosition.IndexOf(item)], item.Location.ToVector2(), Color.White * opacity);
                TextUtility.Draw(sb, panelText[textPosition.IndexOf(item)], BattleGUI.testSF20, item, TextUtility.OutLining.Center, Color.White * opacity, 0f);
                //TextUtility.Draw(sb, "Knight", BattleGUI.testSF20, item, true, Color.White * opacity, 0f);
            }
        }

        public bool Contains(Vector2 mouse)
        {
            foreach (var item in charPanelPositions)
            {
                if (item.Contains(mouse))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
