using LUA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TBAGW;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW
{
    static public class BattleGUI
    {
        static Rectangle normalScreen = new Rectangle(0, 0, 1366, 768);

        #region GUI Resources
        static public Texture2D BattleGUITexture;
        static public Texture2D BattleGUIBG;
        static public Texture2D GUIElementTurn1;
        static public Texture2D GUIElementTurn2;

        static SpriteFont HugeAbiFont;
        static SpriteFont BigAbiFont;
        static SpriteFont MedAbiFont;
        static SpriteFont SmallAbiFont;
        static SpriteFont TinyAbiFont;

        static SpriteFont ActiveBarsFont;

        static SpriteFont ChanceFont;

        static SpriteFont ModifiersFont;

        static SpriteFont BigNameFont;
        static SpriteFont MediumNameFont;
        static SpriteFont SmallNameFont;

        public enum TurnEffects { PlayerTurn = 0, EnemyTurn, AssistTurn }
        static public List<ChangeTurnEffect> TurnEffectsList = new List<ChangeTurnEffect>();
        #endregion
        #region GUI ELEMENTS GRAPHIC
        static Rectangle LNameBoxSource = new Rectangle(14, 0, 342, 100);
        static Rectangle RNameBoxSource = new Rectangle(450, 0, 342, 100);

        static Rectangle LNameBox = new Rectangle(9, 2, 342, 100);
        static Rectangle RNameBox = new Rectangle(1015, 2, 342, 100);

        static Rectangle HBoxSource = new Rectangle(16, 101, 546, 46);
        static Rectangle MBoxSource = new Rectangle(16, 182, 370, 46);

        static Rectangle LHBox = new Rectangle(11, 106, 546, 46);
        static Rectangle LMBox = new Rectangle(11, 144, 370, 46);

        static Rectangle HGreenBarSource = new Rectangle(20, 157, 538, 14);
        static Rectangle MBlueBarSource = new Rectangle(20, 244, 362, 14);

        static Rectangle LHGreenBar = new Rectangle(15, 120, 538, 14);
        static Rectangle LMBlueBar = new Rectangle(15, 159, 362, 14);

        static Rectangle LHGreenBarActualSource = new Rectangle(25, 112, 538, 14);
        static Rectangle LMBlueBarActualSource = new Rectangle(20, 244, 362, 14);

        static Rectangle RHGreenBarActualSource = new Rectangle(25, 112, 538, 14);
        static Rectangle RMBlueBarActualSource = new Rectangle(20, 244, 362, 14);

        static Color LHBarColor = new Color(0, 200, 0);
        static Color RHBarColor = new Color(0, 200, 0);
        static Color LMBarColor = Color.White;
        static Color RMBarColor = Color.White;

        static Rectangle RHGreenBar = new Rectangle(813, 120, 538, 14);
        static Rectangle RMBlueBar = new Rectangle(989, 159, 362, 14);

        static Rectangle RHBox = new Rectangle(809, 106, 546, 46);
        static Rectangle RMBox = new Rectangle(985, 144, 370, 46);

        static Rectangle LAbilityIcon = new Rectangle(203, 576, 96, 96);
        static Rectangle RAbilityIcon = new Rectangle(1067, 576, 96, 96);

        static Rectangle IsometricSource = new Rectangle(0, 768, 1014, 128);
        static Rectangle Isometric = new Rectangle(176, 448, 1014, 128);

        static List<Rectangle> LShieldIcons = new List<Rectangle>();
        static List<Rectangle> RShieldIcons = new List<Rectangle>();
        static Rectangle ShieldIconsSource = new Rectangle(356, 0, 94, 100);
        static Rectangle LShieldIconFirstPos = new Rectangle(352, 2, 94, 100);
        static Rectangle RShieldIconFirstPos = new Rectangle(920, 2, 94, 100);

        static Rectangle LowerHalfScreen = new Rectangle(0, 552, 1366, 216);
        static Rectangle LowerHalfScreenSource = new Rectangle(0, 496, 1366, 216);

        //TODO Ability Panels and frames
        static Rectangle AbilitySelectionPanelSource = new Rectangle(18, 280, 432, 86);

        static Rectangle AbilityFrameSource = new Rectangle(204, 398, 116, 92);
        static Rectangle AbilityNameSource = new Rectangle(324, 398, 362, 92);

        static Rectangle LAbilityFrame = new Rectangle(212, 545, 92, 92);
        static Rectangle LAbilityName = new Rectangle(322, 545, 362, 92);

        static Rectangle RAbilityFrame = new Rectangle(730, 545, 92, 92);
        static Rectangle RAbilityName = new Rectangle(840, 545, 362, 92);

        static Rectangle BlueMovementTileSource = new Rectangle(0, 1171, 64, 64);
        static Rectangle RedAttackTileSource = new Rectangle(64, 1171, 64, 64);
        static Rectangle GreenFriendlyTileSource = new Rectangle(128, 1171, 64, 64);

        static Rectangle PopUpMenuAttackSource = new Rectangle(0, 840, 265, 331);
        static Rectangle PopUpMenuItemSource = new Rectangle(265, 840, 265, 331);
        static Rectangle PopUpMenuDefendSource = new Rectangle(530, 840, 265, 331);
        static Rectangle PopUpMenuInteractSource = new Rectangle(795, 840, 265, 331);

        static Rectangle HoverCursorSource = new Rectangle(1078, 853, 32, 40);



        static Rectangle HoverPointerHorSource = new Rectangle(1067, 918, 50, 37);
        static Rectangle HoverPointerVerSource = new Rectangle(1073, 976, 37, 50);

        public enum PopUpChoices { Attack = 0, Item, Defend, Interact }
        static int popUpMenuSelection = (int)PopUpChoices.Attack;
        #endregion
        #region GUI ELEMENTS TEXT
        static Rectangle LNameText = new Rectangle(20, 10, 320, 89);
        static String GName = "";

        static Rectangle LHBarText = new Rectangle(435, 27, 112, 16);
        static String LHBarString = "";
        static Rectangle LMBarText = new Rectangle(435, 59, 112, 16);
        static String LMBarString = "";
        static Rectangle LEBarText = new Rectangle(435, 91, 112, 16);
        static String LEBarString = "";
        static Rectangle LSBarText = new Rectangle(435, 127, 112, 16);
        static String LSBarString = "";

        static Rectangle LModText = new Rectangle(7, 464, 188, 152);
        static String LModTextString = "";



        static Rectangle LChanceText = new Rectangle(3, 648, 192, 104);
        static String LChanceString = "";
        static Rectangle LWeaponText = new Rectangle(335, 550, 300, 80);
        static String LWeaponString = "";
        static Rectangle LAbiText = new Rectangle(223, 660, 444, 80);
        static String LAbiString = "";


        static Rectangle RNameText = new Rectangle(1335, 28, 320, 89);
        static String RName = "";

        static Rectangle RHBarText = new Rectangle(930, 27, 112, 16);
        static String RHBarString = "";
        static Rectangle RMBarText = new Rectangle(930, 59, 112, 16);
        static String RMBarString = "";
        static Rectangle REBarText = new Rectangle(930, 91, 112, 16);
        static String REBarString = "";
        static Rectangle RSBarText = new Rectangle(930, 127, 112, 16);
        static String RSBarString = "";

        static Rectangle RModText = new Rectangle(1358, 464, 188, 152);
        static String RModTextString = "";
        static Rectangle RChanceText = new Rectangle(1362, 648, 192, 104);
        static String RChanceString = "";
        static Rectangle RWeaponText = new Rectangle(855, 550, 300, 80);
        static String RWeaponString = "";
        static Rectangle RAbiText = new Rectangle(1142, 660, 444, 80);
        static String RAbiString = "";

        static SpriteFont usedLNameFont = default(SpriteFont);
        static SpriteFont usedRNameFont = default(SpriteFont);
        static SpriteFont usedLAbiFont = default(SpriteFont);
        static SpriteFont usedRAbiFont = default(SpriteFont);
        static SpriteFont usedLWpFont = default(SpriteFont);
        static SpriteFont usedRWpFont = default(SpriteFont);

        static Vector2 LHitChanceLoc = new Vector2(30, 610);
        static String LHitChanceString = "HIT:";
        static String LHitChanceNum = "HIT:";

        static Vector2 LDMGLoc = new Vector2(30, 650);
        static String LDMGString = "DMG:";
        static String LDMGNum = "DMG:";

        static Vector2 LCRITChanceLoc = new Vector2(30, 690);
        static String LCRITChanceString = "CRIT:";
        static String LCRITChanceNum = "CRIT:";

        static Vector2 RHitChanceLoc = new Vector2(500, 50);
        static String RHitChanceString = "HIT:";
        static String RHitChanceNum = "HIT:";

        static Vector2 RDMGLoc = new Vector2(500, 100);
        static String RDMGString = "DMG:";
        static String RDMGNum = "DMG:";

        static Vector2 RCRITChanceLoc = new Vector2(500, 150);
        static String RCRITChanceString = "CRIT:";
        static String RCRITChanceNum = "CRIT:";

        #endregion
        #region GUI logic
        static bool bGDrawMana = false;
        static bool bGDrawEnergy = false;
        static bool bGDrawShield = false;

        static bool bRDrawMana = false;
        static bool bRDrawEnergy = false;
        static bool bRDrawShield = false;
        static BasicAbility gbAbi;
        static BasicAbility rbAbi;
        static BasicAbility rbAbiAssist;
        static public BaseCharacter gbc;
        static public BaseCharacter rbc;

        static public bool bDrawFrames = true;
        static public bool bIsRunning = false;

        static Vector2 jumpPosition = Vector2.Zero;

        static int overrideGAnimation = -1;
        static int overrideRAnimation = -1;
        static int attackTimerPassed = 0;
        static int attackStunTimerPassed = 0;

        static bool bPerformJumpBack = false;
        static bool bPerformPushBack = false;

        static public bool bChooseAbility = false;
        static public int abilityIndex = 0;
        static int abilityPrev = 0;
        static int abilityNext = 0;

        static public bool bIsAttacking = false;
        #endregion
        #region Selection Logic
        static public BaseCharacter selectedTarget = null;
        static public List<BaseCharacter> selectableTargetsWithinRange = new List<BaseCharacter>();
        static public int selectTargetIndex = 0;
        static public bool bCanExecuteMove = false;
        #endregion
        #region Small Bars Stuff
        static Texture2D smallBarsTex;
        static Rectangle smallHealthBarSource = new Rectangle(49, 0, 48, 6);
        static Rectangle smallHealthBarPosition = new Rectangle(8, 40, 48, 6);
        static Rectangle smallManaBarSource = new Rectangle(49, 16, 48, 6);
        static Rectangle smallManaBarPosition = new Rectangle(8, 50, 48, 6);
        static Rectangle smallHealthIndicatorSource = new Rectangle(1, 1, 46, 4);
        static Rectangle smallManaIndicatorSource = new Rectangle(1, 17, 46, 4);
        static Rectangle smallHealthIndicatorPosition = new Rectangle(1, 1, 46, 4);
        static Rectangle smallManaIndicatorPosition = new Rectangle(1, 1, 46, 4);
        #endregion
        static public SpriteFont testSF;
        static public SpriteFont testSF20;
        static public SpriteFont testSF25;
        static public SpriteFont testSF32;
        static public SpriteFont testSF48;
        static public bool bCounterAttack = false;
        static List<BasicAbility> gAbilityList;
        static List<BasicAbility> rAbilityList;
        static bool bTargetCanCounter = false;
        static bool bIsAICombat = false;
        static bool bDisplayCounterInfo = false;
        static bool bHasAttacked = false;
        static bool bHandledCounter = false;
        internal static bool bMissedAttack = false;
        static bool bCritAttack = false;

        static int delayBetweenAttackAndHealthAdjust = 2000;
        static int delayBetweenAttackAndHealthAdjustTimePassed = 0;
        static bool bStartAttackDelayTimer = false;
        static bool bPlayAbilitySoundOnce = true;
        static bool bPlayMissSoundOnce = true;
        static bool receiverWillDie = false;
        static bool giverWillDie = false;
        static List<BGUIPopUp> GUIPopUps = new List<BGUIPopUp>();
        static int gDMGDone = 0;
        static int rDMGCounter = 0;
        static bool bShowedGDmg = false;
        static bool bShowedRDmg = false;
        static bool bMustSeeIfCanCouter = true;
        static bool bPerformLastCounter = true;
        static public bool bStartZoom = false;
        static internal List<KeyValuePair<BaseCharacter, bool>> surroundingRbcChars = new List<KeyValuePair<BaseCharacter, bool>>();
        static KeyValuePair<BaseCharacter, bool> charRbcLeft = default(KeyValuePair<BaseCharacter, bool>);
        static KeyValuePair<BaseCharacter, bool> charRbcRight = default(KeyValuePair<BaseCharacter, bool>);
        static KeyValuePair<BaseCharacter, bool> charRbcBehind = default(KeyValuePair<BaseCharacter, bool>);
        static internal List<KeyValuePair<BaseCharacter, bool>> surroundingGbcChars = new List<KeyValuePair<BaseCharacter, bool>>();
        static KeyValuePair<BaseCharacter, bool> charGbcLeft = default(KeyValuePair<BaseCharacter, bool>);
        static KeyValuePair<BaseCharacter, bool> charGbcRight = default(KeyValuePair<BaseCharacter, bool>);
        static KeyValuePair<BaseCharacter, bool> charGbcBehind = default(KeyValuePair<BaseCharacter, bool>);
        static internal BasicAbility castAbilityGBC = null;
        static bool bHandleAttack = true;
        internal static bool bHandleAreaAttack = false;
        internal static List<BaseCharacter> charsForHandleArea = new List<BaseCharacter>();
        static internal int damageDoneByDealer = 0;
        static internal bool bShowAttackNumbers = true;
        static internal bool bShowCounterAttackNumbers = true;
        static TexPanel descriptionPanel;
        static Texture2D texPanelSource;
        static TexPanel battleSpeedButton;
        static TexPanel battleCameraSpeedButton;

        static SoundEffectInstance magicCounterSound;
        static SoundEffectInstance meleeCounterSound;
        static SoundEffectInstance rangedCounterSound;

        internal static void PlayerChoiceUpDown(int v)
        {
            abilityIndex += v;

            if (bChooseAbility)
            {
                if (abilityIndex < 0)
                {
                    abilityIndex = gAbilityList.Count - 1;
                }
                if (abilityIndex > gAbilityList.Count - 1)
                {
                    abilityIndex = 0;
                }
                abilityPrev = abilityIndex - 1;
                abilityNext = abilityIndex + 1;
                if (abilityIndex == 0)
                {
                    abilityPrev = gAbilityList.Count - 1;
                    abilityNext = abilityIndex + 1; ;
                }

                if (abilityIndex == gAbilityList.Count - 1)
                {
                    abilityPrev = abilityIndex - 1;
                    abilityNext = 0;
                }

                if (gAbilityList.Count == 1)
                {
                    abilityNext = 0;
                    abilityNext = 0;
                    abilityPrev = 0;
                }

                GetCasterTextReady();
            }


            if ((bChooseAbility || bIsAICombat || bHasAttacked) && bMustSeeIfCanCouter && gbc.IsAlive() && !bIsAttacking && gbc.AbilityList()[abilityIndex].abilityType != (int)BasicAbility.ABILITY_TYPE.SUPPORT)
            {
                CanTargetCounter();
            }
        }

        static Rectangle positionForHealthLost = new Rectangle(RHGreenBar.X, RHGreenBar.Y, RHGreenBar.Width, RHGreenBar.Height);

        static internal void PlayerChoiceMade()
        {
            //castAbilityGBC = EncounterInfo.currentTurn().selectedCharTurn.character.AbilityList()[BattleGUI.abilityIndex];
            BasicAbility focus = gAbilityList[BattleGUI.abilityIndex];

            if (focus.IsAbilityAvailable(gbc.trueSTATChart()))
            {
                castAbilityGBC = focus;

                if (castAbilityGBC.PAanim != null)
                {
                    castAbilityGBC.PAanim.Reset();
                }

                bChooseAbility = false;
            }

        }

        static public void InitializeResources()
        {
            #region initialization
            if (BattleGUI.BattleGUITexture == null)
            {
                texPanelSource = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
                descriptionPanel = new TexPanel(texPanelSource, new Rectangle(11, 200, 300, 400), new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

                battleSpeedButton = descriptionPanel.positionCopy(new Rectangle(115, 550, 100, 50));
                battleCameraSpeedButton = descriptionPanel.positionCopy(new Rectangle(240, 550, 100, 50));

                smallBarsTex = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\HealthBars");
                BattleGUITexture = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\GUI_Sheet_2048x2048");
                BattleGUIBG = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\CastleBBG");
                GUIElementTurn1 = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\Turn_shields_animations_sheet_Heroes_Enemies_4096x4096");
                GUIElementTurn2 = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\Turn_shields_animations_sheet_Allies_4096x4096");
                InitializeTurnEffects();

                magicCounterSound = Game1.contentManager.Load<SoundEffect>(@"SFX\Abilities\Reverse_Twinkle").CreateInstance();
                meleeCounterSound = Game1.contentManager.Load<SoundEffect>(@"SFX\Abilities\Swoosh_Heavy_Fluid").CreateInstance();
                rangedCounterSound = Game1.contentManager.Load<SoundEffect>(@"SFX\Abilities\Swoosh_Heavy_Fluid").CreateInstance();



                testSF = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test");
                testSF20 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test20");
                testSF25 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test25");
                testSF32 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");
                testSF48 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test48");

                HugeAbiFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Ability\HugeAbiFont_70");
                BigAbiFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Ability\BigAbiFont_64");
                MedAbiFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Ability\MedAbiFont_56");
                SmallAbiFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Ability\SmallAbiFont_48");
                TinyAbiFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Ability\TinyAbiFont_32");

                ActiveBarsFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Bars\BarsFont_22");

                ChanceFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Chances\ChanceFont_32");

                ModifiersFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Modifiers\ModifierFont_32");

                BigNameFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Name\BigNameFont_58");
                MediumNameFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Name\MediumNameFont_48");
                SmallNameFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Name\SmallNameFont_32");
            }
            #endregion
        }

        static internal void PlayCounterSound()
        {
            SoundEffectInstance cSE = null;
            switch (castAbilityGBC.abilityFightStyle)
            {
                case (int)BasicAbility.ABILITY_CAST_TYPE.MELEE:
                    cSE = meleeCounterSound;
                    break;
                case (int)BasicAbility.ABILITY_CAST_TYPE.RANGED:
                    cSE = rangedCounterSound;
                    break;
                case (int)BasicAbility.ABILITY_CAST_TYPE.MAGIC:
                    cSE = magicCounterSound;
                    break;
            }

            cSE.Volume = SceneUtility.masterVolume / 100f * SceneUtility.soundEffectsVolume / 100f;

            cSE.Play();
        }

        private static void InitializeTurnEffects()
        {
            List<Rectangle> temp = new List<Rectangle>();
            for (int i = 1; i != 10; i++)
            {
                temp.Add(new Rectangle(0, 384 * (i - 1), 683, 384));
            }
            for (int i = 1; i != 8; i++)
            {
                temp.Add(new Rectangle(683, 384 * (i - 1), 683, 384));
            }
            TurnEffectsList.Add(new ChangeTurnEffect(GUIElementTurn1, temp));

            temp = new List<Rectangle>();
            for (int i = 1; i != 10; i++)
            {
                temp.Add(new Rectangle(1366, 384 * (i - 1), 683, 384));
            }
            for (int i = 1; i != 8; i++)
            {
                temp.Add(new Rectangle(2049, 384 * (i - 1), 683, 384));
            }
            TurnEffectsList.Add(new ChangeTurnEffect(GUIElementTurn1, temp));

            temp = new List<Rectangle>();
            for (int i = 1; i != 10; i++)
            {
                temp.Add(new Rectangle(0, 384 * (i - 1), 683, 384));
            }
            for (int i = 1; i != 8; i++)
            {
                temp.Add(new Rectangle(683, 384 * (i - 1), 683, 384));
            }
            TurnEffectsList.Add(new ChangeTurnEffect(GUIElementTurn2, temp));

        }

        private static void ResetElements()
        {
            bShowCounterAttackNumbers = true;
            bShowAttackNumbers = true;
            charsForHandleArea = new List<BaseCharacter>();
            bHandleAreaAttack = false;
            TurnSet.bSelectingArea = false;
            bHandleAttack = true;
            castAbilityGBC = null;
            charGbcLeft = default(KeyValuePair<BaseCharacter, bool>);
            charGbcRight = default(KeyValuePair<BaseCharacter, bool>);
            charGbcBehind = default(KeyValuePair<BaseCharacter, bool>);
            charRbcLeft = default(KeyValuePair<BaseCharacter, bool>);
            charRbcRight = default(KeyValuePair<BaseCharacter, bool>);
            charRbcBehind = default(KeyValuePair<BaseCharacter, bool>);

            if (gbc != rbc)
            {
                GenerateNearRbcCharacters();
            }

            GenerateNearGbcCharacters();
            bStartZoom = false;
            zoom = 1.5f;
            GameScreenEffect.Reset();
            bPerformLastCounter = true;
            bMustSeeIfCanCouter = true;
            bShowedGDmg = false;
            bShowedRDmg = false;
            GUIPopUps.Clear();
            giverWillDie = false;
            receiverWillDie = false;
            bPlayMissSoundOnce = true;
            bPlayAbilitySoundOnce = true;
            bHandledCounter = false;
            delayBetweenAttackAndHealthAdjustTimePassed = 0;
            bIsAICombat = false;
            bTargetCanCounter = false;
            bCounterAttack = false;
            bStartAttackDelayTimer = false;
            attackTimerPassed = 0;
            attackStunTimerPassed = 0;
            bPerformJumpBack = false;
            bPerformPushBack = false;
            abilityIndex = 0;
            jumpPosition = Vector2.Zero;
            counterDMGs = new KeyValuePair<int, int>();
            // GameProcessor.bIsOverWorldOutsideGame = false;
            bHasAttacked = false;
            bMissedAttack = false;
            bCritAttack = false;

            bStartAttackDelayTimer = false;
            delayBetweenAttackAndHealthAdjustTimePassed = 0;
            positionForHealthLost.X = 0;
            positionForHealthLost.Width = 0;
            bDisplayCounterInfo = false;
            positionForHealthLost = new Rectangle(0, RHGreenBar.Y, 0, RHGreenBar.Height);
            bIsAICombat = false;
            LCRITChanceNum = "";
            RCRITChanceNum = "";
            LHitChanceNum = "";
            RHitChanceNum = "";
            LDMGNum = "";
            RDMGNum = "";
            LAbiString = "";
            RAbiString = "";
            var gts = gbc.trueSTATChart();
            //gAbilityList = gbc.AbilityList().FindAll(ba => ba.abilityCanHitTargetInRange(gbc, rbc) && ba.IsAbilityAvailable(gts));
            gAbilityList = gbc.AbilityList().FindAll(ba => ba.abilityCanHitTargetInRange(gbc, rbc));

            if (gbc != null)
            {
                gbc.charBattleAnimations[0].frameIndex = 0;
                gbc.charBattleAnimations[0].elapsedFrameTime = 0;
                gbc.charBattleAnimations[0].bAnimationFinished = false;

                for (int i = 1; i < gbc.charBattleAnimations.Count; i++)
                {
                    gbc.charBattleAnimations[i].bSimplePlayOnce = true;
                    gbc.charBattleAnimations[i].frameIndex = 0;
                    gbc.charBattleAnimations[i].elapsedFrameTime = 0;
                    gbc.charBattleAnimations[i].bAnimationFinished = false;

                }
            }


            if (rbc != null)
            {
                rbc.charBattleAnimations[0].frameIndex = 0;
                rbc.charBattleAnimations[0].elapsedFrameTime = 0;
                rbc.charBattleAnimations[0].bAnimationFinished = false;

                for (int i = 1; i < rbc.charBattleAnimations.Count; i++)
                {
                    rbc.charBattleAnimations[i].bSimplePlayOnce = true;
                    rbc.charBattleAnimations[i].frameIndex = 0;
                    rbc.charBattleAnimations[i].elapsedFrameTime = 0;
                    rbc.charBattleAnimations[i].bAnimationFinished = false;

                }
            }
        }

        private static void GenerateNearGbcCharacters()
        {
            var temp = (BaseCharacter.Rotation)rbc.rotationIndex;


            switch (temp)
            {
                case Utilities.Sprite.BaseSprite.Rotation.Up:
                    var charFurthestAway = surroundingGbcChars.Find(c => c.Key.pointDistanceFrom(rbc) == new Point(0, 2));
                    var tempList = new List<KeyValuePair<BaseCharacter, bool>>(surroundingGbcChars);
                    tempList.Remove(charFurthestAway);

                    charGbcBehind = charFurthestAway.Key == default(BaseCharacter) ? default(KeyValuePair<BaseCharacter, bool>) : charFurthestAway;
                    if (tempList.Count == 2)
                    {
                        charGbcRight = tempList[0];
                        charGbcLeft = tempList[1];
                    }
                    else if (tempList.Count == 1)
                    {
                        charGbcRight = tempList[0];
                    }

                    break;
                case Utilities.Sprite.BaseSprite.Rotation.Right:
                    charFurthestAway = surroundingGbcChars.Find(c => c.Key.pointDistanceFrom(rbc) == new Point(2, 0));
                    tempList = new List<KeyValuePair<BaseCharacter, bool>>(surroundingGbcChars);
                    tempList.Remove(charFurthestAway);

                    charGbcBehind = charFurthestAway.Key == default(BaseCharacter) ? default(KeyValuePair<BaseCharacter, bool>) : charFurthestAway;
                    if (tempList.Count == 2)
                    {
                        charGbcRight = tempList[0];
                        charGbcLeft = tempList[1];
                    }
                    else if (tempList.Count == 1)
                    {
                        charGbcRight = tempList[0];
                    }
                    break;
                case Utilities.Sprite.BaseSprite.Rotation.Down:
                    charFurthestAway = surroundingGbcChars.Find(c => c.Key.pointDistanceFrom(rbc) == new Point(0, 2));
                    tempList = new List<KeyValuePair<BaseCharacter, bool>>(surroundingGbcChars);
                    tempList.Remove(charFurthestAway);

                    charGbcBehind = charFurthestAway.Key == default(BaseCharacter) ? default(KeyValuePair<BaseCharacter, bool>) : charFurthestAway;
                    if (tempList.Count == 2)
                    {
                        charGbcRight = tempList[0];
                        charGbcLeft = tempList[1];
                    }
                    else if (tempList.Count == 1)
                    {
                        charGbcLeft = tempList[0];
                    }
                    break;
                case Utilities.Sprite.BaseSprite.Rotation.Left:
                    charFurthestAway = surroundingGbcChars.Find(c => c.Key.pointDistanceFrom(rbc) == new Point(2, 0));
                    tempList = new List<KeyValuePair<BaseCharacter, bool>>(surroundingGbcChars);
                    tempList.Remove(charFurthestAway);

                    charGbcBehind = charFurthestAway.Key == default(BaseCharacter) ? default(KeyValuePair<BaseCharacter, bool>) : charFurthestAway;
                    if (tempList.Count == 2)
                    {
                        charGbcRight = tempList[0];
                        charGbcLeft = tempList[1];
                    }
                    else if (tempList.Count == 1)
                    {
                        charGbcLeft = tempList[0];
                    }
                    break;
                default:
                    break;
            }
        }

        private static void GenerateNearRbcCharacters()
        {
            var temp = (BaseCharacter.Rotation)gbc.rotationIndex;


            switch (temp)
            {
                case Utilities.Sprite.BaseSprite.Rotation.Up:
                    var charFurthestAway = surroundingRbcChars.Find(c => c.Key.pointDistanceFrom(gbc) == new Point(0, 2));
                    var tempList = new List<KeyValuePair<BaseCharacter, bool>>(surroundingRbcChars);
                    tempList.Remove(charFurthestAway);

                    charRbcBehind = charFurthestAway.Key == default(BaseCharacter) ? default(KeyValuePair<BaseCharacter, bool>) : charFurthestAway;
                    if (tempList.Count == 2)
                    {
                        charRbcLeft = tempList[0];
                        charRbcRight = tempList[1];
                    }
                    else if (tempList.Count == 1)
                    {
                        charRbcLeft = tempList[0];
                    }

                    break;
                case Utilities.Sprite.BaseSprite.Rotation.Right:
                    charFurthestAway = surroundingRbcChars.Find(c => c.Key.pointDistanceFrom(gbc) == new Point(2, 0));
                    tempList = new List<KeyValuePair<BaseCharacter, bool>>(surroundingRbcChars);
                    tempList.Remove(charFurthestAway);

                    charRbcBehind = charFurthestAway.Key == default(BaseCharacter) ? default(KeyValuePair<BaseCharacter, bool>) : charFurthestAway;
                    if (tempList.Count == 2)
                    {
                        charRbcLeft = tempList[0];
                        charRbcRight = tempList[1];
                    }
                    else if (tempList.Count == 1)
                    {
                        charRbcLeft = tempList[0];
                    }
                    break;
                case Utilities.Sprite.BaseSprite.Rotation.Down:
                    charFurthestAway = surroundingRbcChars.Find(c => c.Key.pointDistanceFrom(gbc) == new Point(0, 2));
                    tempList = new List<KeyValuePair<BaseCharacter, bool>>(surroundingRbcChars);
                    tempList.Remove(charFurthestAway);

                    charRbcBehind = charFurthestAway.Key == default(BaseCharacter) ? default(KeyValuePair<BaseCharacter, bool>) : charFurthestAway;
                    if (tempList.Count == 2)
                    {
                        charRbcLeft = tempList[0];
                        charRbcRight = tempList[1];
                    }
                    else if (tempList.Count == 1)
                    {
                        charRbcRight = tempList[0];
                    }
                    break;
                case Utilities.Sprite.BaseSprite.Rotation.Left:
                    charFurthestAway = surroundingRbcChars.Find(c => c.Key.pointDistanceFrom(gbc) == new Point(2, 0));
                    tempList = new List<KeyValuePair<BaseCharacter, bool>>(surroundingRbcChars);
                    tempList.Remove(charFurthestAway);

                    charRbcBehind = charFurthestAway.Key == default(BaseCharacter) ? default(KeyValuePair<BaseCharacter, bool>) : charFurthestAway;
                    if (tempList.Count == 2)
                    {
                        charRbcLeft = tempList[0];
                        charRbcRight = tempList[1];
                    }
                    else if (tempList.Count == 1)
                    {
                        charRbcRight = tempList[0];
                    }
                    break;
                default:
                    break;
            }
        }

        internal static void HandleLowerTabSelect()
        {
            var clicked = playerOverlaySlabs.Find(r => r.Contains(KeyboardMouseUtility.uiMousePos));
            if (clicked != default(Rectangle))
            {
                int index = playerOverlaySlabs.IndexOf(clicked);
                KeyboardMouseUtility.bPressed = true;
                BaseCharacter c = PlayerSaveData.heroParty[index];
                if (c.IsAlive() && EncounterInfo.currentTurn().charactersInGroup.Contains(c))
                {
                    EncounterInfo.currentTurn().SelectCharacter(c);
                }
            }

            if (battleSpeedButton.ContainsMouse(KeyboardMouseUtility.uiMousePos))
            {
                KeyboardMouseUtility.bPressed = true;
                SettingsFile.speedMod += 2;
                if (SettingsFile.speedMod > 8)
                {
                    SettingsFile.speedMod = 1;
                }
            }

            if (battleCameraSpeedButton.ContainsMouse(KeyboardMouseUtility.uiMousePos))
            {
                KeyboardMouseUtility.bPressed = true;
                SettingsFile.speedModCamera += 2;
                if (SettingsFile.speedModCamera > 8)
                {
                    SettingsFile.speedModCamera = 1;
                }
            }
        }

        static public void Start(STATChart gAS, STATChart rAS, BaseCharacter g, BaseCharacter r, BasicAbility gba, BasicAbility rba)
        {
            #region initialization
            if (BattleGUI.BattleGUITexture == null)
            {
                InitializeResources();
            }
            #endregion

            LShieldIcons.Clear();
            RShieldIcons.Clear();
            gbc = g;
            rbc = r;

            gbAbi = gba;
            rbAbi = rba;

            GName = CalculateNameSizeLeft(g.displayName);
            RName = CalculateNameSizeRight(r.displayName);

            // LWeaponString = CalculateWpNameSizeLeft(g.charEquipment[(int)BaseItem.EQUIP_TYPES.Weapon].itemName);
            //RWeaponString = CalculateWpNameSizeRight(r.charEquipment[(int)BaseItem.EQUIP_TYPES.Weapon].itemName);

            try
            {
                // LWeaponString = CalculateWpNameSizeLeft(g.charEquipment[(int)BaseItem.EQUIP_TYPES.Weapon].itemName);
                LWeaponString = CalculateWpNameSizeLeft(g.weapon.itemName);
            }
            catch (Exception)
            {
                LWeaponString = CalculateWpNameSizeLeft("Unarmed");
            }

            try
            {
                RWeaponString = CalculateWpNameSizeRight(r.weapon.itemName);
            }
            catch (Exception)
            {
                RWeaponString = CalculateWpNameSizeRight("Unarmed");
            }

            LAbiString = CalculateAbilitySizeLeft(gba.abilityName);
            RAbiString = CalculateAbilitySizeRight(rba.abilityName);

            bGDrawMana = g.CCC.equippedClass.bUsesMagic;
            bGDrawEnergy = g.CCC.equippedClass.bUsesMagic;
            bGDrawShield = false;

            bRDrawMana = r.CCC.equippedClass.bUsesMagic;
            bRDrawEnergy = r.CCC.equippedClass.bUsesMagic;
            bRDrawShield = false;

            if (gAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] > 0 && gAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] < 10)
            {
                bGDrawShield = true;
                LShieldIcons.Add(LShieldIconFirstPos);

                for (int i = 1; i < gAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]; i++)
                {
                    LShieldIcons.Add(new Rectangle(LShieldIcons[i - 1].X + LShieldIcons[i - 1].Width, LShieldIcons[i - 1].Y, LShieldIcons[i - 1].Width, LShieldIcons[i - 1].Height));
                }
            }

            if (rAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] > 0 && rAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] < 10)
            {
                bRDrawShield = true;
                RShieldIcons.Add(RShieldIconFirstPos);

                for (int i = 1; i < rAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]; i++)
                {
                    RShieldIcons.Add(new Rectangle(RShieldIcons[i - 1].X - RShieldIcons[i - 1].Width, RShieldIcons[i - 1].Y, RShieldIcons[i - 1].Width, RShieldIcons[i - 1].Height));
                }
            }
        }

        static public void Start(BaseCharacter g, BaseCharacter r, int abilityInd = 0)
        {
            #region initialization
            if (BattleGUI.BattleGUITexture == null)
            {
                InitializeResources();
            }
            #endregion

            gbc = g;
            rbc = r;
            ResetElements();
            BattleAnimationInfo.Initialize(g, r);
            g.bSaveAP = false;



            g.charBattleAnimations.ForEach(a => a.SimpleReset());
            r.charBattleAnimations.ForEach(a => a.SimpleReset());
            g.battleAnimationTaskList.Clear();
            r.battleAnimationTaskList.Clear();
            g.animationBattleIndex = (int)BaseCharacter.CharacterBattleAnimations.Idle;
            r.animationBattleIndex = (int)BaseCharacter.CharacterBattleAnimations.Idle;

            bCanExecuteMove = false;
            abilityIndex = abilityInd;
            PlayerChoiceUpDown(0);
            abilityNext = 0;
            abilityPrev = 0;
            bChooseAbility = false;
            bIsRunning = true;
            if (g.AbilityList().Count == 1)
            {
                abilityNext = 0;
                abilityPrev = 0;
            }

            LShieldIcons.Clear();
            RShieldIcons.Clear();


            GName = CalculateNameSizeLeft(g.displayName);
            RName = CalculateNameSizeRight(r.displayName);

            try
            {
                LWeaponString = CalculateWpNameSizeLeft(g.weapon.itemName);
            }
            catch (Exception)
            {
                LWeaponString = CalculateWpNameSizeLeft("Unarmed");
            }

            try
            {
                RWeaponString = CalculateWpNameSizeRight(r.weapon.itemName);
            }
            catch (Exception)
            {
                RWeaponString = CalculateWpNameSizeRight("Unarmed");
            }



            bGDrawMana = g.CCC.equippedClass.bUsesMagic;
            bGDrawEnergy = g.CCC.equippedClass.bUsesMagic;
            bGDrawShield = false;
            bRDrawMana = r.CCC.equippedClass.bUsesMagic;
            bRDrawEnergy = r.CCC.equippedClass.bUsesMagic;
            bRDrawShield = false;

            var gAS = g.trueSTATChart();
            var rAS = r.trueSTATChart();

            if (gAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] > 0 && gAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] < 10)
            {
                bGDrawShield = true;
                LShieldIcons.Add(LShieldIconFirstPos);

                for (int i = 1; i < gAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]; i++)
                {
                    LShieldIcons.Add(new Rectangle(LShieldIcons[i - 1].X + LShieldIcons[i - 1].Width, LShieldIcons[i - 1].Y, LShieldIcons[i - 1].Width, LShieldIcons[i - 1].Height));
                }
            }

            if (rAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] > 0 && rAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] < 10)
            {
                bRDrawShield = true;
                RShieldIcons.Add(RShieldIconFirstPos);

                for (int i = 1; i < rAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]; i++)
                {
                    RShieldIcons.Add(new Rectangle(RShieldIcons[i - 1].X - RShieldIcons[i - 1].Width, RShieldIcons[i - 1].Y, RShieldIcons[i - 1].Width, RShieldIcons[i - 1].Height));
                }
            }
            LogicUpdate();

            bStartZoom = true;

            //ScriptProcessor.ChangeActiveScript(g.GenerateCombatScript(), true);
            //ScriptProcessor.AssignCombatScript(g.shapeID,r.shapeID,g.shapeID);
        }

        static public void AIStart(BaseCharacter g, BaseCharacter r, int abilityInd = 0)
        {
            #region initialization
            if (BattleGUI.BattleGUITexture == null)
            {
                BattleGUITexture = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\GUI_Sheet_2048x2048");
                BattleGUIBG = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\CastleBBG");

                HugeAbiFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Ability\HugeAbiFont_70");
                BigAbiFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Ability\BigAbiFont_64");
                MedAbiFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Ability\MedAbiFont_56");
                SmallAbiFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Ability\SmallAbiFont_48");
                TinyAbiFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Ability\TinyAbiFont_32");

                ActiveBarsFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Bars\BarsFont_22");

                ChanceFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Chances\ChanceFont_32");

                ModifiersFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Modifiers\ModifierFont_32");

                BigNameFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Name\BigNameFont_58");
                MediumNameFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Name\MediumNameFont_48");
                SmallNameFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\Name\SmallNameFont_32");
            }
            #endregion

            g.attackedAsAI = true;
            g.bAIExecuteDefend = false;

            g.charBattleAnimations.ForEach(a => a.SimpleReset());
            r.charBattleAnimations.ForEach(a => a.SimpleReset());
            g.battleAnimationTaskList.Clear();
            r.battleAnimationTaskList.Clear();
            g.animationBattleIndex = (int)BaseCharacter.CharacterBattleAnimations.Idle;
            r.animationBattleIndex = (int)BaseCharacter.CharacterBattleAnimations.Idle;
            g.bSaveAP = false;

            bCanExecuteMove = false;
            abilityIndex = abilityInd;
            abilityNext = 0;
            abilityPrev = 0;
            bChooseAbility = false;
            bIsRunning = true;


            if (g.AbilityList().Count == 1)
            {
                abilityNext = 0;
                abilityPrev = 0;
            }

            LShieldIcons.Clear();
            RShieldIcons.Clear();
            gbc = g;
            rbc = r;
            ResetElements();
            gAbilityList = gbc.CCC.equippedClass.classAbilities;
            gAbilityList = gbc.CCC.possibleAbilities();
            gbc.CCC.abiEquipList.abilities.Clear();
            gbc.CCC.abiEquipList.abilities.AddRange(gAbilityList);

            if (abilityInd == -1)
            {
                castAbilityGBC = g.CCC.AIDefaultAttack;
            }
            else
            {
                castAbilityGBC = gAbilityList[abilityInd];
            }

            if (castAbilityGBC != null)
            {
                PlayerChoiceUpDown(0);
                if (castAbilityGBC.PAanim != null)
                {
                    castAbilityGBC.PAanim.Reset();
                }
            }
            else
            {
                throw new Exception("GBC ability is null error");
            }

            abilityIndex = gAbilityList.IndexOf(castAbilityGBC);

            BattleAnimationInfo.Initialize(g, r);
            bIsAICombat = true;

            GName = CalculateNameSizeLeft(g.displayName);
            RName = CalculateNameSizeRight(r.displayName);

            //LWeaponString = CalculateWpNameSizeLeft(g.charEquipment[(int)BaseItem.EQUIP_TYPES.Weapon].itemName);
            //RWeaponString = CalculateWpNameSizeRight(r.charEquipment[(int)BaseItem.EQUIP_TYPES.Weapon].itemName);

            try
            {
                LWeaponString = CalculateWpNameSizeLeft(g.weapon.itemName);
            }
            catch (Exception)
            {
                LWeaponString = CalculateWpNameSizeLeft("Unarmed");
            }

            try
            {
                RWeaponString = CalculateWpNameSizeRight(r.weapon.itemName);
            }
            catch (Exception)
            {
                RWeaponString = CalculateWpNameSizeRight("Unarmed");
            }

            bGDrawMana = g.CCC.equippedClass.bUsesMagic;
            bGDrawEnergy = g.CCC.equippedClass.bUsesMagic;
            bGDrawShield = false;

            bRDrawMana = r.CCC.equippedClass.bUsesMagic;
            bRDrawEnergy = r.CCC.equippedClass.bUsesMagic;
            bRDrawShield = false;

            var gAS = g.trueSTATChart();
            var rAS = r.trueSTATChart();

            LAbiString = CalculateAbilitySizeLeft(castAbilityGBC.abilityName);

            if (gAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] > 0 && gAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] < 10)
            {
                bGDrawShield = true;
                LShieldIcons.Add(LShieldIconFirstPos);

                for (int i = 1; i < gAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]; i++)
                {
                    LShieldIcons.Add(new Rectangle(LShieldIcons[i - 1].X + LShieldIcons[i - 1].Width, LShieldIcons[i - 1].Y, LShieldIcons[i - 1].Width, LShieldIcons[i - 1].Height));
                }
            }

            if (rAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] > 0 && rAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] < 10)
            {
                bRDrawShield = true;
                RShieldIcons.Add(RShieldIconFirstPos);

                for (int i = 1; i < rAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]; i++)
                {
                    RShieldIcons.Add(new Rectangle(RShieldIcons[i - 1].X - RShieldIcons[i - 1].Width, RShieldIcons[i - 1].Y, RShieldIcons[i - 1].Width, RShieldIcons[i - 1].Height));
                }
            }
            GetCasterTextReady();
            LogicUpdate();

            //AI STUFF
            bStartZoom = true;
            //AI

            //AttemptAttack();

        }

        static public void End()
        {
            CombatProcessor.GetRegion().regionBGinfo.SwitchLayer();

            TurnSet.bSelectingArea = false;
            if (bHandleAreaAttack)
            {
                castAbilityGBC.PAanim.Reset();
            }
            //bHandleAreaAttack = false;
            GUIPopUps.Clear();
            //ResetElements();
            bIsRunning = false;
            bChooseAbility = false;
            selectedTarget = null;
            abilityIndex = 0;
            selectTargetIndex = 0;
            selectableTargetsWithinRange.Clear();
            bIsAttacking = false;
            bCanExecuteMove = false;
            abilityIndex = -1;
            UpdateGUIElements();
            GameProcessor.bIsOverWorldOutsideGame = true;
            if (bTargetCanCounter && gbc != null && !bHandledCounter)
            {
                CanTargetCounter();
                ApplyAbilityDamageCounter(counterDMGs);
            }

            if (rbc != null)
            {
                rbc.MakeSureActiveStatsAreInOrder();
            }
            if (gbc != null)
            {
                gbc.MakeSureActiveStatsAreInOrder();
            }

            if (castAbilityGBC != null)
            {
                castAbilityGBC.SetCoolDown();
            }

        }

        static public void EndWithoutAttacking()
        {
            CombatProcessor.GetRegion().regionBGinfo.SwitchLayer();
            GUIPopUps.Clear();
            //ResetElements();
            bIsRunning = false;
            bChooseAbility = false;
            selectedTarget = null;
            abilityIndex = 0;
            selectTargetIndex = 0;
            selectableTargetsWithinRange.Clear();
            bIsAttacking = false;
            bCanExecuteMove = false;
            abilityIndex = -1;
            UpdateGUIElements();
            GameProcessor.bIsOverWorldOutsideGame = true;

        }

        private static string CalculateWpNameSizeLeft(string wpn)
        {
            if (BigNameFont.MeasureString(wpn).X <= LWeaponText.Width)
            {
                usedLWpFont = BigNameFont;
            }
            else if (MediumNameFont.MeasureString(wpn).X <= LWeaponText.Width)
            {
                usedLWpFont = MediumNameFont;
            }
            else if (SmallNameFont.MeasureString(wpn).X <= LWeaponText.Width)
            {
                usedLWpFont = SmallNameFont;
            }

            return wpn;
        }
        private static string CalculateWpNameSizeRight(string wpn)
        {
            if (BigNameFont.MeasureString(wpn).X <= RWeaponText.Width)
            {
                usedRWpFont = BigNameFont;
            }
            else if (MediumNameFont.MeasureString(wpn).X <= RWeaponText.Width)
            {
                usedRWpFont = MediumNameFont;
            }
            else if (SmallNameFont.MeasureString(wpn).X <= RWeaponText.Width)
            {
                usedRWpFont = SmallNameFont;
            }

            return wpn;
        }
        private static String CalculateAbilitySizeLeft(string an)
        {
            if (HugeAbiFont.MeasureString(an).X <= LAbiText.Width)
            {
                usedLAbiFont = HugeAbiFont;
            }
            else if (BigAbiFont.MeasureString(an).X <= LAbiText.Width)
            {
                usedLAbiFont = BigAbiFont;
            }
            else if (MedAbiFont.MeasureString(an).X <= LAbiText.Width)
            {
                usedLAbiFont = MedAbiFont;
            }
            else if (SmallAbiFont.MeasureString(an).X <= LAbiText.Width)
            {
                usedLAbiFont = SmallAbiFont;
            }
            else if (TinyAbiFont.MeasureString(an).X <= LAbiText.Width)
            {
                usedLAbiFont = TinyAbiFont;
            }

            return an;
        }
        private static String CalculateAbilitySizeRight(string an)
        {
            if (HugeAbiFont.MeasureString(an).X <= RAbiText.Width)
            {
                usedRAbiFont = HugeAbiFont;
            }
            else if (BigAbiFont.MeasureString(an).X <= RAbiText.Width)
            {
                usedRAbiFont = BigAbiFont;
            }
            else if (MedAbiFont.MeasureString(an).X <= RAbiText.Width)
            {
                usedRAbiFont = MedAbiFont;
            }
            else if (SmallAbiFont.MeasureString(an).X <= RAbiText.Width)
            {
                usedRAbiFont = SmallAbiFont;
            }
            else if (TinyAbiFont.MeasureString(an).X <= RAbiText.Width)
            {
                usedRAbiFont = TinyAbiFont;
            }

            return an;
        }
        private static String CalculateNameSizeRight(string cn)
        {
            if (BigNameFont.MeasureString(cn).X <= RNameText.Width)
            {
                usedRNameFont = BigNameFont;
            }
            else if (MediumNameFont.MeasureString(cn).X <= RNameText.Width)
            {
                usedRNameFont = MediumNameFont;
            }
            else if (SmallNameFont.MeasureString(cn).X <= RNameText.Width)
            {
                usedRNameFont = SmallNameFont;
            }

            return cn;
        }
        private static String CalculateNameSizeLeft(string cn)
        {
            if (BigNameFont.MeasureString(cn).X <= LNameText.Width)
            {
                usedLNameFont = BigNameFont;
            }
            else if (MediumNameFont.MeasureString(cn).X <= LNameText.Width)
            {
                usedLNameFont = MediumNameFont;
            }
            else if (SmallNameFont.MeasureString(cn).X <= LNameText.Width)
            {
                usedLNameFont = SmallNameFont;
            }

            return cn;
        }

        static List<Vector2> locs = new List<Vector2>();
        static LuaAbilityInfo currentAttack = null;

        static public void AttemptAttack()
        {
            bHasAttacked = true;
            bIsAttacking = true;
            //bCounterAttack = true;
            gbc.charBattleAnimations.ForEach(sa => { sa.frameIndex = 0; });
            bool bCanAttack = false;

            if (EncounterInfo.currentTurn().bIsPlayerTurnSet && !EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden)
            {
                if (gbc.RemainingMana() >= gAbilityList[abilityIndex].currentMPCost())
                {
                    if (gbc.RemainingAP() >= gAbilityList[abilityIndex].currentAPCost() || gAbilityList[abilityIndex].currentAPCost() == 0)
                    {
                        bCanAttack = true;
                    }
                }
            }
            else if (!EncounterInfo.currentTurn().bIsPlayerTurnSet || EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden)
            {
                if (abilityIndex != -1 && gbc.CCC.AIDefaultAttack != gAbilityList[abilityIndex])
                {
                    if (gbc.RemainingMana() >= gAbilityList[abilityIndex].currentMPCost())
                    {
                        if (gbc.RemainingAP() >= gAbilityList[abilityIndex].currentAPCost() || gAbilityList[abilityIndex].currentAPCost() == 0)
                        {
                            bCanAttack = true;
                        }
                    }
                }
                else if (gbc.CCC.AIDefaultAttack == gAbilityList[abilityIndex])
                {
                    bCanAttack = true;
                }

                if (abilityIndex == -1)
                {
                    abilityIndex = gAbilityList.IndexOf(gAbilityList.Find(abi => abi.abilityIdentifier == gbc.CCC.AIDefaultAttack.abilityIdentifier));
                    bCanAttack = true;
                }
            }

            if (bCanAttack)
            {
                BasicAbility abiUsed = gAbilityList[abilityIndex];
                int hitChance = (int)abiUsed.ReturnAbilityHitChanceNum(gbc);
                hitChance += ExtraHitChanceFromNearbyChars();


                int critChance = abiUsed.ReturnAbilityCritChanceNum(gbc);
                critChance += ExtraCritChanceFromNearbyChars();

                try
                {
                    if (EncounterInfo.currentTurn().bIsPlayerTurnSet && !EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden)
                    {
                        EncounterInfo.currentTurn().selectedCharTurn.abiUsedInfo = LuaAbilityInfo.abiToLuaAbilityInfo(gAbilityList[abilityIndex], gbc.toCharInfo(), rbc.toCharInfo());
                        currentAttack = EncounterInfo.currentTurn().selectedCharTurn.abiUsedInfo;
                    }
                    else
                    {
                        EncounterInfo.currentTurn().currentEnemy.abiUsedInfo = LuaAbilityInfo.abiToLuaAbilityInfo(gAbilityList[abilityIndex], gbc.toCharInfo(), rbc.toCharInfo());
                        currentAttack = EncounterInfo.currentTurn().currentEnemy.abiUsedInfo;
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                currentAttack.hitChance = hitChance;
                currentAttack.critChance = critChance;

                currentAttack.ExecuteScript();

                LHitChanceNum = currentAttack.hitChance.ToString();
                LCRITChanceNum = currentAttack.critChance.ToString();


                int randomChance = GamePlayUtility.Randomize(0, 101);

                if (randomChance > currentAttack.hitChance)
                {
                    bMissedAttack = true;
                }

                if (!bMissedAttack)
                {
                    randomChance = GamePlayUtility.Randomize(0, 101);

                    if (randomChance < currentAttack.critChance)
                    {
                        bCritAttack = true;
                    }

                    GenerateGiverDMG(currentAttack);

                    if (bMustSeeIfCanCouter)
                    {
                        CanTargetCounter();
                    }
                    LuaPreAttackLogic();

                    ApplyAbilityDamage();
                    if (!receiverWillDie)
                    {
                        PerformCounterLogic();
                    }

                    currentAttack.HandleAbilityMod();
                    currentAttack.parent.InitializeAbilityBeforeAttack();
                }


                gbc.jumpSpeed = 23;

                //gbc.battleAnimScale = 4;
                bChooseAbility = false;
                if (receiverWillDie)
                {
                    bCounterAttack = false;
                }


                if (abiUsed.abilityFightStyle == (int)BasicAbility.ABILITY_CAST_TYPE.MELEE)
                {
                    var jumpDestination = new Vector2(1366 - (rbc.jumpEnd.X + rbc.charBattleAnimations[rbc.animationBattleIndex].animationFrames[0].Width * rbc.battleAnimScale), gbc.battleAnimLocs[rbc.animationBattleIndex].Y);
                    jumpDestination = new Vector2(1366 - (rbc.battleAnimLocs[rbc.animationBattleIndex].X + 100 + gbc.charBattleAnimations[0].animationFrames[0].Width * gbc.battleAnimScale * 6 / 5), gbc.battleAnimLocs[rbc.animationBattleIndex].Y);
                    var anim1 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Jump_Brace, BaseCharacter.CharacterBattleAnimations.Idle, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                    BattleAnimationInfo.completeAnimationInfo.Add(anim1);
                    var anim2 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Jump_MidAir, BaseCharacter.CharacterBattleAnimations.Idle, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, jumpDestination, rbc.battleAnimLocs[0]);
                    BattleAnimationInfo.completeAnimationInfo.Add(anim2);
                    var anim3 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Jump_Landing, BaseCharacter.CharacterBattleAnimations.Idle, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, jumpDestination, rbc.battleAnimLocs[0]);
                    BattleAnimationInfo.completeAnimationInfo.Add(anim3);
                    BaseCharacter.CharacterBattleAnimations endAnimationRBC = BaseCharacter.CharacterBattleAnimations.Idle;
                    if (bCounterAttack)// || true)
                    {
                        var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Attack, BaseCharacter.CharacterBattleAnimations.Counter_Attack, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, jumpDestination, rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Hurt, BaseCharacter.CharacterBattleAnimations.Attack, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, BattleAnimationInfo.AnimationTimingType.PlayAtStart, jumpDestination, rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        BattleAnimationInfo.SetHasCounterAttack(true);
                        //anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Hurt, BaseCharacter.CharacterBattleAnimations.Attack, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, BattleAnimationInfo.AnimationTimingType.PlayAtStart, jumpDestination, rbc.battleAnimLocs[0]);
                        //BattleAnimationInfo.completeAnimationInfo.Add(anim);
                    }
                    else if (!bCounterAttack)
                    {
                        if (!bMissedAttack)
                        {
                            var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Attack, BaseCharacter.CharacterBattleAnimations.Hurt, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, jumpDestination, rbc.battleAnimLocs[0]);
                            BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        }
                        else if (bMissedAttack)
                        {
                            var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Attack, BaseCharacter.CharacterBattleAnimations.Idle, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, jumpDestination, rbc.battleAnimLocs[0]);
                            BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        }

                    }

                    if (receiverWillDie)
                    {
                        var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Idle, BaseCharacter.CharacterBattleAnimations.Death, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, jumpDestination, rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        endAnimationRBC = BaseCharacter.CharacterBattleAnimations.Death_State;
                    }

                    if (!giverWillDie)
                    {
                        var anim4 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Jump_Brace, endAnimationRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, jumpDestination, rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim4);
                        anim4 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Jump_MidAir, endAnimationRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim4);
                        anim4 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Jump_Landing, endAnimationRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim4);
                        anim4 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Idle, endAnimationRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim4);
                    }
                    else
                    {
                        var anim4 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Death, endAnimationRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, jumpDestination, rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim4);
                        anim4 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Death_State, endAnimationRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, jumpDestination, rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim4);
                    }

                }
                if (abiUsed.abilityFightStyle == (int)BasicAbility.ABILITY_CAST_TYPE.RANGED && abiUsed.abilityType != (int)BasicAbility.ABILITY_TYPE.SUPPORT)
                {
                    BaseCharacter.CharacterBattleAnimations endAnimationRBC = BaseCharacter.CharacterBattleAnimations.Idle;
                    BaseCharacter.CharacterBattleAnimations reactionToAttackRBC = BaseCharacter.CharacterBattleAnimations.Hurt;
                    if (bMissedAttack) { reactionToAttackRBC = BaseCharacter.CharacterBattleAnimations.Idle; }
                    BaseCharacter.CharacterBattleAnimations reactionToAttackGBC = BaseCharacter.CharacterBattleAnimations.Hurt;
                    if (bCounterAttack && abiUsed.abilityType != (int)BasicAbility.ABILITY_TYPE.SUPPORT)
                    {

                        var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Attack, reactionToAttackRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        anim = new BattleAnimationInfo(reactionToAttackGBC, BaseCharacter.CharacterBattleAnimations.Attack, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        BattleAnimationInfo.SetHasCounterAttack(true);
                    }
                    else if (!bCounterAttack && abiUsed.abilityType != (int)BasicAbility.ABILITY_TYPE.SUPPORT)
                    {
                        var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Attack, reactionToAttackRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                    }
                    else if (abiUsed.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT)
                    {
                        var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Attack, BaseCharacter.CharacterBattleAnimations.Idle, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                    }

                    if (receiverWillDie)
                    {
                        var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Idle, BaseCharacter.CharacterBattleAnimations.Death, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        endAnimationRBC = BaseCharacter.CharacterBattleAnimations.Death_State;
                    }

                    if (!giverWillDie)
                    {
                        var animEND = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Idle, endAnimationRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(animEND);
                    }
                    else
                    {
                        var animEND = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Death, endAnimationRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(animEND);
                    }
                }
                if (abiUsed.abilityFightStyle == (int)BasicAbility.ABILITY_CAST_TYPE.MAGIC || abiUsed.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT)
                {
                    BaseCharacter.CharacterBattleAnimations endAnimationRBC = BaseCharacter.CharacterBattleAnimations.Idle;
                    BaseCharacter.CharacterBattleAnimations reactionToAttackRBC = abiUsed.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT ? BaseCharacter.CharacterBattleAnimations.Idle : BaseCharacter.CharacterBattleAnimations.Hurt;
                    BaseCharacter.CharacterBattleAnimations reactionToAttackGBC = BaseCharacter.CharacterBattleAnimations.Hurt;
                    if (bCounterAttack)
                    {
                        reactionToAttackRBC = BaseCharacter.CharacterBattleAnimations.Counter_Attack;
                        var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Attack, reactionToAttackRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        anim = new BattleAnimationInfo(reactionToAttackGBC, BaseCharacter.CharacterBattleAnimations.Attack, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        BattleAnimationInfo.SetHasCounterAttack(true);
                    }
                    else if (!bCounterAttack)
                    {
                        if (bMissedAttack) { reactionToAttackRBC = BaseCharacter.CharacterBattleAnimations.Idle; }
                        var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Attack, reactionToAttackRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtClimax, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                    }

                    if (receiverWillDie)
                    {
                        var anim = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Idle, BaseCharacter.CharacterBattleAnimations.Death, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(anim);
                        endAnimationRBC = BaseCharacter.CharacterBattleAnimations.Death_State;
                    }

                    if (!giverWillDie)
                    {
                        var animEND = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Idle, endAnimationRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(animEND);
                    }
                    else
                    {
                        var animEND = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Death, endAnimationRBC, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
                        BattleAnimationInfo.completeAnimationInfo.Add(animEND);
                    }

                }

                BattleAnimationInfo.Start(BattleAnimationInfo.completeAnimationInfo[0]);


                if (!bMissedAttack)
                {

                }
                else
                {
                    Vector2 startPos = Vector2.Zero;
                    if (jumpPosition != Vector2.Zero)
                    {
                        startPos = jumpPosition + new Vector2(rbc.charBattleAnimations[0].animationFrames[0].Width * rbc.battleAnimScale, 0) + new Vector2(popUpTextLocationHMod, 150);
                    }
                    else
                    {
                        startPos = gbc.battleAnimLocs[gbc.animationBattleIndex] + new Vector2(rbc.charBattleAnimations[0].animationFrames[0].Width * rbc.battleAnimScale, 0) + new Vector2(popUpTextLocationHMod, 150);
                    }

                    GUIPopUps.Add(new BGUIPopUpText(testSF48, "MISS", Color.Black, Color.White, 2, startPos, modTextTime, modTextFadeTime, deltaMovMod));
                }

                bIsAttacking = true;
            }


            PlayAttackSound();
            ApplyAbilityEffects();
        }

        private static void GenerateGiverDMG(LuaAbilityInfo currentAttack)
        {
            if (gAbilityList[abilityIndex].abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK)
            {
                BattleProcessor.giverDMG = BattleProcessor.CalculateDamage(currentAttack);
            }
            else
            {
                BattleProcessor.giverDMG = BattleProcessor.CalculateHealing(currentAttack);
            }
        }

        private static void LuaPreAttackLogic()
        {
            LuaAttackInfo.attackInfo.attack = gAbilityList[abilityIndex];
            LuaAttackInfo.attackInfo.attacker = gbc;
            LuaAttackInfo.attackInfo.defender = rbc;

            LuaAttackInfo.attackInfo.charBehind = charGbcBehind.Key == default(BaseCharacter) ? null : charGbcBehind.Key;
            LuaAttackInfo.attackInfo.charSide1 = charGbcLeft.Key == default(BaseCharacter) ? null : charGbcLeft.Key;
            LuaAttackInfo.attackInfo.charSide2 = charGbcRight.Key == default(BaseCharacter) ? null : charGbcRight.Key;

            LuaAttackInfo.attackInfo.defBehind = charRbcBehind.Key == default(BaseCharacter) ? null : charRbcBehind.Key;
            LuaAttackInfo.attackInfo.defSide1 = charRbcLeft.Key == default(BaseCharacter) ? null : charRbcLeft.Key;
            LuaAttackInfo.attackInfo.defSide2 = charRbcRight.Key == default(BaseCharacter) ? null : charRbcRight.Key;

            LuaAttackInfo.attackInfo.bCounter = bCounterAttack;
            LuaAttackInfo.attackInfo.bCrit = bCritAttack;
            LuaAttackInfo.attackInfo.bMiss = bMissedAttack;
            LuaAttackInfo.attackInfo.bIsHeal = !(gAbilityList[abilityIndex].abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK);

            LuaAttackInfo.attackInfo.minDmg = BattleProcessor.giverDMG.Key[0];
            LuaAttackInfo.attackInfo.maxDmg = BattleProcessor.giverDMG.Key[1];

            if (bCounterAttack)
            {
                LuaAttackInfo.attackInfo.counterMinDmg = counterDMGs.Key;
                LuaAttackInfo.attackInfo.counterMaxDmg = counterDMGs.Value;
            }

            if (!gbc.bIsAI)
            {
                if (gbc.HasProperLua())
                {
                    if (gbc.luaState.GetFunction("Action") != null)
                    {
                        (gbc.luaState["Action"] as NLua.LuaFunction).Call(LuaAttackInfo.attackInfo);
                    }
                }
            }
            else
            {
                if (gbc.eai.HasProperLua())
                {
                    if (gbc.eai.luaState.GetFunction("Action") != null)
                    {
                        (gbc.eai.luaState["Action"] as NLua.LuaFunction).Call(LuaAttackInfo.attackInfo);
                    }
                }
            }

            if (!rbc.bIsAI)
            {
                if (rbc.HasProperLua())
                {
                    if (rbc.luaState.GetFunction("Reaction") != null)
                    {
                        (rbc.luaState["Reaction"] as NLua.LuaFunction).Call(LuaAttackInfo.attackInfo);
                    }
                }
            }
            else
            {
                if (rbc.eai.HasProperLua())
                {
                    if (rbc.eai.luaState.GetFunction("Reaction") != null)
                    {
                        (rbc.eai.luaState["Reaction"] as NLua.LuaFunction).Call(LuaAttackInfo.attackInfo);
                    }
                }
            }
        }

        private static int ExtraCritChanceFromNearbyChars()
        {
            int extraChance = 0;

            foreach (var item in surroundingRbcChars)
            {
                if (!item.Value)
                {
                    extraChance += 3;
                }
            }

            if (castAbilityGBC != null && castAbilityGBC.abilityType != (int)BasicAbility.ABILITY_TYPE.SUPPORT)
            {
                foreach (var item in surroundingGbcChars)
                {
                    if (item.Value)
                    {
                        extraChance -= 3;
                    }
                }
            }


            return extraChance;
        }

        private static int ExtraHitChanceFromNearbyChars()
        {
            int extraChance = 0;
            foreach (var item in surroundingRbcChars)
            {
                if (!item.Value)
                {
                    extraChance += 5;
                }
            }

            if (bChooseAbility || castAbilityGBC != null && castAbilityGBC.abilityType != (int)BasicAbility.ABILITY_TYPE.SUPPORT)
            {
                foreach (var item in surroundingGbcChars)
                {
                    if (item.Value)
                    {
                        extraChance -= 3;
                    }
                }
            }

            return extraChance;
        }

        private static void PlayAttackSound()
        {

        }

        public enum characterType { Hero, Enemy, Other }

        internal static void DrawSmallBars(SpriteBatch spriteBatch, BaseCharacter bc, int index, characterType ct)
        {
            //int widthHP = (int)(smallHealthIndicatorSource.Width * ((float)(bc.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP])) / (float)(bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP]));
            //int widthMP = (int)(smallManaIndicatorSource.Width * ((float)(bc.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.MANA])) / (float)(bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXMANA]));
            int widthHP = smallHealthIndicatorSource.Width;
            int widthMP = smallManaIndicatorSource.Width;
            if (CombatProcessor.heroCharacters.Count != healthOfAllHeroes.Count)
            {
                UpdateGUIElements();
            }
            switch (ct)
            {
                case characterType.Hero:
                    widthHP = (int)(smallHealthIndicatorSource.Width * ((float)(healthOfAllHeroes[index])) / (float)(maxHealthOfAllHeroes[index]));
                    widthMP = (int)(smallManaIndicatorSource.Width * ((float)(manaOfAllHeroes[index])) / (float)(maxManaOfAllHeroes[index]));

                    break;
                case characterType.Enemy:
                    widthHP = (int)(smallHealthIndicatorSource.Width * ((float)(healthOfAllEnemies[index])) / (float)(maxHealthOfAllEnemies[index]));
                    widthMP = (int)(smallManaIndicatorSource.Width * ((float)(manaOfAllEnemies[index])) / (float)(maxManaOfAllEnemies[index]));
                    break;
                case characterType.Other:
                    widthHP = (int)(smallHealthIndicatorSource.Width * ((float)(healthOfAllOthers[index])) / (float)(maxHealthOfAllOthers[index]));
                    widthMP = (int)(smallManaIndicatorSource.Width * ((float)(manaOfAllOthers[index])) / (float)(maxManaOfAllOthers[index]));
                    break;
                default:
                    break;
            }

            Rectangle HPdrawLocation = new Rectangle((int)bc.position.X + smallHealthBarPosition.X, (int)bc.position.Y + smallHealthBarPosition.Y, smallHealthBarPosition.Width, smallHealthBarPosition.Height);
            Rectangle MPdrawLocation = new Rectangle((int)bc.position.X + smallManaBarPosition.X, (int)bc.position.Y + smallManaBarPosition.Y, smallManaBarPosition.Width, smallManaBarPosition.Height);
            Rectangle HPIndicatordrawLocation = new Rectangle(HPdrawLocation.X + smallHealthIndicatorPosition.X, HPdrawLocation.Y + smallHealthIndicatorPosition.Y, widthHP, smallHealthIndicatorPosition.Height);
            Rectangle MPIndicatordrawLocation = new Rectangle(MPdrawLocation.X + smallManaIndicatorPosition.X, MPdrawLocation.Y + smallManaIndicatorPosition.Y, widthMP, smallManaIndicatorPosition.Height);
            Rectangle HPIndicatorSource = new Rectangle(smallHealthIndicatorSource.X, smallHealthIndicatorSource.Y, widthHP, smallHealthIndicatorSource.Height);
            Rectangle MPIndicatorSource = new Rectangle(smallManaIndicatorSource.X, smallManaIndicatorSource.Y, widthMP, smallManaIndicatorSource.Height);

            spriteBatch.Draw(smallBarsTex, HPdrawLocation, smallHealthBarSource, Color.White);
            spriteBatch.Draw(smallBarsTex, MPdrawLocation, smallManaBarSource, Color.White);
            spriteBatch.Draw(smallBarsTex, HPIndicatordrawLocation, HPIndicatorSource, Color.White);
            spriteBatch.Draw(smallBarsTex, MPIndicatordrawLocation, MPIndicatorSource, Color.White);
        }

        static List<int> healthOfAllEnemies = new List<int>();
        static List<int> healthOfAllHeroes = new List<int>();
        static List<int> healthOfAllOthers = new List<int>();

        static List<int> manaOfAllEnemies = new List<int>();
        static List<int> manaOfAllHeroes = new List<int>();
        static List<int> manaOfAllOthers = new List<int>();

        static List<int> maxHealthOfAllEnemies = new List<int>();
        static List<int> maxHealthOfAllHeroes = new List<int>();
        static List<int> maxHealthOfAllOthers = new List<int>();

        static List<int> maxManaOfAllEnemies = new List<int>();
        static List<int> maxManaOfAllHeroes = new List<int>();
        static List<int> maxManaOfAllOthers = new List<int>();

        public static void UpdateGUIElements()
        {
            healthOfAllEnemies.Clear();
            healthOfAllHeroes.Clear();
            healthOfAllOthers.Clear();
            manaOfAllEnemies.Clear();
            manaOfAllHeroes.Clear();
            manaOfAllOthers.Clear();
            maxHealthOfAllEnemies.Clear();
            maxHealthOfAllHeroes.Clear();
            maxHealthOfAllOthers.Clear();
            maxManaOfAllEnemies.Clear();
            maxManaOfAllHeroes.Clear();
            maxManaOfAllOthers.Clear();

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                healthOfAllEnemies.Add(item.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP]);
                maxHealthOfAllEnemies.Add(item.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP]);
                manaOfAllEnemies.Add(item.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.MANA]);
                maxManaOfAllEnemies.Add(item.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXMANA]);
            }

            foreach (var item in CombatProcessor.heroCharacters)
            {
                healthOfAllHeroes.Add(item.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP]);
                maxHealthOfAllHeroes.Add(item.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP]);
                manaOfAllHeroes.Add(item.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.MANA]);
                maxManaOfAllHeroes.Add(item.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXMANA]);
            }
        }

        private static void GenerateGiverDMG()
        {
            if (gAbilityList[abilityIndex].abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK)
            {
                BattleProcessor.giverDMG = BattleProcessor.CalculateDamage(gbc, rbc, gAbilityList[abilityIndex], new BasicAbility());
            }
            else
            {
                BattleProcessor.giverDMG = BattleProcessor.CalculateHealing(gbc, rbc, gAbilityList[abilityIndex], new BasicAbility());
            }
        }

        private static void ApplyAbilityDamage()
        {
            //var dmg = new KeyValuePair<List<int>, List<int>>(new List<int>(), new List<int>()); ;
            //if (gAbilityList[abilityIndex].abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK)
            //{
            //    dmg = BattleProcessor.CalculateDamage(gbc, rbc, gAbilityList[abilityIndex], new BasicAbility());
            //}
            //else
            //{
            //    dmg = BattleProcessor.CalculateHealing(gbc, rbc, gAbilityList[abilityIndex], new BasicAbility());
            //    dmg.Key[0] = -dmg.Key[0];
            //    dmg.Key[1] = -dmg.Key[1];
            //}

            var dmg = BattleProcessor.giverDMG;
            if (gAbilityList[abilityIndex].abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK)
            {
            }
            else
            {

            }

            if (true)
            {
                bStartAttackDelayTimer = true;

                KeyValuePair<int, float> extraDamageAndModifierFromSurroundingChars = generateExtraDamageAndModifierFromSurroundingChars();
                //  randomDamage += extraDamageAndModifierFromSurroundingChars.Key;
                if (extraDamageAndModifierFromSurroundingChars.Value == 0.0f) { extraDamageAndModifierFromSurroundingChars.Value = 1.0f; }
                int min = dmg.Key[0] + extraDamageAndModifierFromSurroundingChars.Key;
                int max = (int)((dmg.Key[1] + extraDamageAndModifierFromSurroundingChars.Key) * extraDamageAndModifierFromSurroundingChars.Value);
                int randomDamage = GamePlayUtility.Randomize(dmg.Key[0] + extraDamageAndModifierFromSurroundingChars.Key, (int)((dmg.Key[1] + extraDamageAndModifierFromSurroundingChars.Key) * extraDamageAndModifierFromSurroundingChars.Value));

                if (gAbilityList[abilityIndex].abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT)
                {
                    randomDamage = -randomDamage;
                }

                if (bCritAttack)
                {
                    randomDamage = (int)((float)randomDamage * (GamePlayUtility.ExpertRandomize(1.4f, 2.0f)));
                }

                rbc.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] -= randomDamage;
                damageDoneByDealer = randomDamage;
                if (randomDamage != 0)
                {
                    int xDiffernce = (int)(new Vector2(-RHGreenBarActualSource.Width + HGreenBarSource.Width, 0)).X;
                    positionForHealthLost.X = RHGreenBar.X + xDiffernce;
                }

                CalculateHBar();
                CalculateMBar();

                if (CombatProcessor.heroCharacters.Contains(gbc))
                {
                    BattleStats.AddDamageDoneByHero(gbc, randomDamage);
                    if (!rbc.IsAlive())
                    {
                        BattleStats.AddKBByHero(gbc);
                        receiverWillDie = true;
                    }
                }

                gDMGDone = randomDamage;

                if (rbc != null && !rbc.IsAlive())
                {
                    receiverWillDie = true;
                }

                try
                {
                    if (EncounterInfo.currentTurn().bIsPlayerTurnSet && !EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden)
                    {
                        EncounterInfo.currentTurn().selectedCharTurn.bAttackedThisCT = true;
                    }
                    else
                    {
                        EncounterInfo.currentTurn().currentEnemy.bAttackedThisCT = true;
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        private static KeyValuePair<int, float> generateExtraDamageAndModifierFromSurroundingChars()
        {
            KeyValuePair<int, float> extras = new KeyValuePair<int, float>();
            if (castAbilityGBC != null && castAbilityGBC.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT)
            {
                return new KeyValuePair<int, float>();
            }

            extras.Value = 1.0f;

            int amountOfAllies = 0;
            foreach (var item in surroundingRbcChars)
            {
                if (!item.Value)
                {
                    amountOfAllies++;
                }
            }

            switch (amountOfAllies)
            {
                case 1:
                    extras.Key += 1;
                    break;
                case 2:
                    extras.Key += 2;
                    break;
                case 3:
                    extras.Key += 3;
                    break;
            }

            if (charRbcBehind.Key != null && !charRbcBehind.Value)
            {
                extras.Key++;
            }

            amountOfAllies = 0;
            foreach (var item in surroundingGbcChars)
            {
                if (!item.Value)
                {
                    amountOfAllies++;
                }
            }

            switch (amountOfAllies)
            {
                case 1:
                    extras.Value = 1.2f;
                    break;
                case 2:
                    extras.Value = 1.5f;
                    break;
                case 3:
                    extras.Value = 2.3f;
                    break;
            }


            return extras;
        }

        internal struct surroundInfo
        {
            internal int rbcAllies;
            internal int rbcEnemies;
            internal int gbcAllies;
            internal int gbcEnemies;
            internal int hitChance;
            internal int critChance;

            internal surroundInfo(int rbcAllies, int rbcEnemies, int gbcAllies, int gbcEnemies, int hitChance, int critChance)
            {
                this.rbcAllies = rbcAllies;
                this.rbcEnemies = rbcEnemies;
                this.gbcAllies = gbcAllies;
                this.gbcEnemies = gbcEnemies;
                this.hitChance = hitChance;
                this.critChance = critChance;
            }
        }

        internal static surroundInfo GetSurroundInfo()
        {

            int amountOfAlliesRBC = 0;
            foreach (var item in surroundingRbcChars)
            {
                if (!item.Value)
                {
                    amountOfAlliesRBC++;
                }
            }

            int amountOfAlliesGBC = 0;
            foreach (var item in surroundingGbcChars)
            {
                if (item.Value)
                {
                    amountOfAlliesGBC++;
                }
            }



            return new BattleGUI.surroundInfo(amountOfAlliesRBC, surroundingRbcChars.Count - amountOfAlliesRBC, amountOfAlliesGBC, surroundingGbcChars.Count - amountOfAlliesGBC, ExtraHitChanceFromNearbyChars(), ExtraCritChanceFromNearbyChars());
        }

        private static KeyValuePair<int, float> generateExtraCounterDamageAndModifierFromSurroundingChars()
        {
            KeyValuePair<int, float> extras = new KeyValuePair<int, float>();

            int amountOfAllies = 0;
            foreach (var item in surroundingRbcChars)
            {
                if (item.Value)
                {
                    amountOfAllies++;
                }
            }

            switch (amountOfAllies)
            {
                case 1:
                    extras.Key += 1;
                    break;
                case 2:
                    extras.Key += 3;
                    break;
                case 3:
                    extras.Key += 5;
                    break;
            }

            amountOfAllies = 0;
            foreach (var item in surroundingGbcChars)
            {
                if (item.Value)
                {
                    amountOfAllies++;
                }
            }

            switch (amountOfAllies)
            {
                case 1:
                    extras.Value = 1.2f;
                    break;
                case 2:
                    extras.Value = 1.5f;
                    break;
                case 3:
                    extras.Value = 2.3f;
                    break;
            }

            return extras;
        }

        public static void ApplyAbilityEffects()
        {
            //CalculateMBar();
            //CalculateHBar();
            //bIsAttacking = false;
            gAbilityList[abilityIndex].ApplyAbilityCosts(gbc);

            rbc.modifierList.Add(gbc.AbilityList()[abilityIndex].AppliedStatModifier());


        }

        static int healthTickTimer = 30;
        static int healthTickTimePassed = 0;

        static public void LogicUpdate(GameTime gt)
        {
            CalculateHBar();
            CalculateMBar();

        }

        static public void LogicUpdate()
        {
            CalculateHBar();
            CalculateMBar();
        }

        private static void CalculateHBar()
        {
            int gHP = gbc.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP];
            int rHP = rbc.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP];

            int gMaxHP = gbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP];
            int rMaxHP = rbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP];

            float gHpercentage;
            if (gHP != 0)
            {
                gHpercentage = (float)gHP / (float)gMaxHP;
            }
            else
            {
                gHpercentage = 0f;
            }

            float rHpercentage;
            if (rHP != 0)
            {
                rHpercentage = (float)rHP / (float)rMaxHP;
            }
            else
            {
                rHpercentage = 0f;
            }

            if (gHpercentage <= 1f && gHpercentage >= 0.75f)
            {
                LHBarColor = new Color(0, 200, 0);
            }
            else if (gHpercentage >= 0.50f && gHpercentage < 0.75f)
            {
                int step = (int)((0.5f - ((0.75f - gHpercentage))) * 25);
                step = 25 - step;
                LHBarColor = new Color((int)((255 / 25) * step), (int)255, (int)0);
            }
            else if (gHpercentage >= 0.25f && gHpercentage < 0.50f)
            {
                int step = (int)((0.25f - ((0.50f - gHpercentage))) * 50);
                step = 25 - step;
                LHBarColor = new Color(255, 255 - ((128 / 25) * step), 0);
            }
            else if (gHpercentage > 0f && gHpercentage < 0.25f)
            {
                int step = (int)((0f - ((0.25f - gHpercentage))) * 25);
                step = 25 - step;
                LHBarColor = new Color(255, 128 - (128 / 25) * step, 0);
            }


            if (rHpercentage <= 1f && rHpercentage >= 0.75f)
            {
                RHBarColor = new Color(0, 200, 0);
            }
            else if (rHpercentage >= 0.50f && rHpercentage < 0.75f)
            {
                int step = (int)((0.5f - ((0.75f - rHpercentage))) * 25);
                step = 25 - step;
                RHBarColor = new Color((int)((255 / 25) * step), (int)255, (int)0);
            }
            else if (rHpercentage >= 0.25f && rHpercentage < 0.50f)
            {
                int step = (int)((0.25f - ((0.50f - rHpercentage))) * 50);
                step = 25 - step;
                RHBarColor = new Color(255, 255 - ((128 / 25) * step), 0);
            }
            else if (rHpercentage > 0f && rHpercentage < 0.25f)
            {
                int step = (int)((0f - ((0.25f - rHpercentage))) * 25);
                step = 25 - step;
                RHBarColor = new Color(255, 128 - (128 / 25) * step, 0);
            }

            int LGreenBarWidth = (int)(HGreenBarSource.Width * gHpercentage);
            LHGreenBarActualSource = new Rectangle(HGreenBarSource.X, HGreenBarSource.Y, LGreenBarWidth, HGreenBarSource.Height);
            int RGreenBarWidth = (int)(HGreenBarSource.Width * rHpercentage);
            RHGreenBarActualSource = new Rectangle(HGreenBarSource.X, HGreenBarSource.Y, RGreenBarWidth, HGreenBarSource.Height);

            if (damageDoneByDealer != 0)
            {
                int xDiffernce = (int)(new Vector2(-RHGreenBarActualSource.Width + HGreenBarSource.Width, 0)).X;
                //xDiffernce = (int)(new Vector2(positionForHealthLost.Width - RHGreenBarActualSource.Width, 0)).X;
                //positionForHealthLost.X = RHGreenBar.X + RHGreenBarActualSource.Width;
                positionForHealthLost.Width = Math.Abs(xDiffernce);
                if (positionForHealthLost.Width > RHGreenBarActualSource.Width && RHGreenBarActualSource.Width == 0)
                {
                    positionForHealthLost.Width = RHGreenBarActualSource.Width;
                }
            }


        }

        private static void CalculateMBar()
        {
            int gMP = gbc.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA];
            int rMP = rbc.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA];

            int gMaxMP = gbc.statChart.currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXMANA];
            int rMaxMP = rbc.statChart.currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXMANA];

            float gMpercentage;
            if (gMP != 0)
            {
                gMpercentage = (float)gMP / (float)gMaxMP;
            }
            else
            {
                gMpercentage = 0f;
            }

            float rMpercentage;
            if (rMP != 0)
            {
                rMpercentage = (float)rMP / (float)rMaxMP;
            }
            else
            {
                rMpercentage = 0f;
            }
            /*
            if (gMpercentage < 1f && gMpercentage >= 0.75f)
            {
                LHBarColor = new Color(0, 255, 0);
            }
            else if (gMpercentage >= 0.50f && gMpercentage < 0.75f)
            {
                int step = (int)((0.5f - ((0.75f - gMpercentage))) * 25);
                step = 25 - step;
                LHBarColor = new Color((int)((255 / 25) * step), (int)255, (int)0);
            }
            else if (gMpercentage >= 0.25f && gMpercentage < 0.50f)
            {
                int step = (int)((0.25f - ((0.50f - gMpercentage))) * 50);
                step = 25 - step;
                LHBarColor = new Color(255, 255 - ((128 / 25) * step), 0);
            }
            else if (gMpercentage > 0f && gMpercentage < 0.25f)
            {
                int step = (int)((0f - ((0.25f - gMpercentage))) * 25);
                step = 25 - step;
                LHBarColor = new Color(255, 128 - (128 / 25) * step, 0);
            }


            if (rMpercentage < 1f && rMpercentage >= 0.75f)
            {
                RHBarColor = new Color(0, 255, 0);
            }
            else if (rMpercentage >= 0.50f && rMpercentage < 0.75f)
            {
                int step = (int)((0.5f - ((0.75f - rMpercentage))) * 25);
                step = 25 - step;
                RHBarColor = new Color((int)((255 / 25) * step), (int)255, (int)0);
            }
            else if (rMpercentage >= 0.25f && rMpercentage < 0.50f)
            {
                int step = (int)((0.25f - ((0.50f - rMpercentage))) * 50);
                step = 25 - step;
                RHBarColor = new Color(255, 255 - ((128 / 25) * step), 0);
            }
            else if (rMpercentage > 0f && rMpercentage < 0.25f)
            {
                int step = (int)((0f - ((0.25f - rMpercentage))) * 25);
                step = 25 - step;
                RHBarColor = new Color(255, 128 - (128 / 25) * step, 0);
            }*/

            int LBlueBarWidth = (int)(MBlueBarSource.Width * gMpercentage);
            LMBlueBarActualSource = new Rectangle(MBlueBarSource.X, MBlueBarSource.Y, LBlueBarWidth, MBlueBarSource.Height);
            int RBlueBarWidth = (int)(MBlueBarSource.Width * rMpercentage);
            RMBlueBarActualSource = new Rectangle(MBlueBarSource.X, MBlueBarSource.Y, RBlueBarWidth, MBlueBarSource.Height);


        }

        static public void UpdateBattleLogic(GameTime gt)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.OemPlus) && zoom <= 1.5f)
            //{
            //    zoom += 0.015f;
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.OemMinus) && zoom >= 1.0f)
            //{
            //    zoom -= 0.015f;
            //}
            //castAbilityGBC
            //if (castAbilityGBC.bPlayedSE)
            //{
            //    castAbilityGBC.bPlayedSE = false;
            //    castAbilityGBC.playSoundEffect();
            //}

            if (charRbcBehind.Key != default(BaseCharacter))
            {
                charRbcBehind.Key.charBattleAnimations[0].BAnimUpdate(gt, charRbcBehind.Key);
            }
            if (charRbcLeft.Key != default(BaseCharacter))
            {
                charRbcLeft.Key.charBattleAnimations[0].BAnimUpdate(gt, charRbcLeft.Key);
            }
            if (charRbcRight.Key != default(BaseCharacter))
            {
                charRbcRight.Key.charBattleAnimations[0].BAnimUpdate(gt, charRbcRight.Key);
            }

            if (charGbcBehind.Key != default(BaseCharacter))
            {
                charGbcBehind.Key.charBattleAnimations[0].BAnimUpdate(gt, charRbcBehind.Key);
            }
            if (charGbcLeft.Key != default(BaseCharacter))
            {
                charGbcLeft.Key.charBattleAnimations[0].BAnimUpdate(gt, charRbcLeft.Key);
            }
            if (charGbcRight.Key != default(BaseCharacter))
            {
                charGbcRight.Key.charBattleAnimations[0].BAnimUpdate(gt, charRbcRight.Key);
            }

            if (bHandleAreaAttack)
            {
                castAbilityGBC.PAanim.UpdateAnimationForItems(gt);
            }

            if (bHandleAttack && bIsRunning)
            {
                if (bStartZoom && zoom >= 1.0f)
                {
                    zoom -= 0.015f;
                }
                else if (!(zoom >= 1.0f))
                {
                    if (bStartZoom && EncounterInfo.currentTurn().bIsPlayerTurnSet && !EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden && !bChooseAbility)
                    {
                        bChooseAbility = true;
                        bStartZoom = false;
                    }
                    else if (!EncounterInfo.currentTurn().bIsPlayerTurnSet || EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden)
                    {
                        bStartZoom = false;
                    }
                    if (!bStartZoom && !bChooseAbility)
                    {
                        PreAttackCall();

                        bHandleAttack = false;
                    }
                }
            }


            try
            {
                if (castAbilityGBC != null)
                {
                    if (!castAbilityGBC.abilityIcon.texFileLoc.Equals(""))
                    {
                        if (castAbilityGBC.abilityIcon.animationTexture != null)
                        {
                            castAbilityGBC.abilityIcon.UpdateAnimationForItems(gt);
                        }
                        else
                        {
                            castAbilityGBC.abilityIcon.ReloadTexture();
                        }
                    }
                }

            }
            catch
            {
                Console.WriteLine("Error updating " + castAbilityGBC + " icon at BattleGUI");
            }




            CalculateHBar();
            CalculateMBar();

            if (delayBetweenAttackAndHealthAdjustTimePassed <= delayBetweenAttackAndHealthAdjust)
            {
                delayBetweenAttackAndHealthAdjustTimePassed += gt.ElapsedGameTime.Milliseconds;
            }


            if (delayBetweenAttackAndHealthAdjustTimePassed > delayBetweenAttackAndHealthAdjust)
            {
                if (positionForHealthLost.Width > 0)
                {
                    healthTickTimePassed += gt.ElapsedGameTime.Milliseconds;
                }
            }
            // GameScreenEffect.InitiateCombatCritShake(1.0f);
            if (rbc != null)
            {
                if (rbc.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Counter_Attack || rbc.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Shield_Block || rbc.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Hurt)
                {
                    if (bCritAttack)
                    {
                        GameScreenEffect.InitiateCombatCritShake(1.0f);
                        bCritAttack = false;
                    }
                }


            }

            if (gbc != null)
            {

                if (bMissedAttack && bPlayMissSoundOnce)
                {
                    if (gbc.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack && gbc.charBattleAnimations[gbc.animationBattleIndex].NearlyFinished())
                    {
                        bPlayMissSoundOnce = false;
                        PlayMissSound();
                    }
                }

                if (!bMissedAttack && bPlayAbilitySoundOnce)
                {
                    if (gbc.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack && gbc.charBattleAnimations[gbc.animationBattleIndex].NearlyFinished())
                    {
                        bPlayAbilitySoundOnce = false;
                        PlayAbilitySound();
                    }
                }
            }

            if (BattleAnimationInfo.completeAnimationInfo.Count != 0 && bIsAttacking)
            {
                BattleAnimationInfo.Update(gt);
            }
            else if (BattleAnimationInfo.completeAnimationInfo.Count == 0 && bIsAttacking)
            {
                gbc.currentBattleAnimation().bPause = false;
                rbc.currentBattleAnimation().bPause = false;
                if (rbc.animationBattleIndex == 0)
                {
                    rbc.currentBattleAnimation().bSimplePlayOnce = false;
                    rbc.currentBattleAnimation().bAnimationFinished = false;
                }
                if (gbc.animationBattleIndex == 0)
                {
                    gbc.currentBattleAnimation().bSimplePlayOnce = false;
                    gbc.currentBattleAnimation().bAnimationFinished = false;
                }
            }

            if (BattleAnimationInfo.completeAnimationInfo.Count != 0 && bHandleAreaAttack)
            {
                BattleAnimationInfo.Update(gt);
            }
            else if (BattleAnimationInfo.completeAnimationInfo.Count == 0 && bIsAttacking)
            {
                gbc.currentBattleAnimation().bPause = false;
                rbc.currentBattleAnimation().bPause = false;
                if (rbc.animationBattleIndex == 0)
                {
                    rbc.currentBattleAnimation().bSimplePlayOnce = false;
                    rbc.currentBattleAnimation().bAnimationFinished = false;
                }
                if (gbc.animationBattleIndex == 0)
                {
                    gbc.currentBattleAnimation().bSimplePlayOnce = false;
                    gbc.currentBattleAnimation().bAnimationFinished = false;
                }
            }


            if (rbc.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Death && rbc.currentBattleAnimation().bAnimationFinished)
            {
                rbc.animationBattleIndex = (int)BaseCharacter.CharacterBattleAnimations.Death_State;
            }


            if (bIsAttacking && gbc.battleAnimationTaskList.Count == 0 && !bPerformJumpBack)
            {
                bCanExecuteMove = true;
            }

            if (bPerformPushBack)
            {

            }

            if (gbc != rbc)
            {
                if (overrideRAnimation != -1)
                {
                    rbc.charBattleAnimations[overrideRAnimation].BAnimUpdate(gt, rbc);
                }
                else
                {
                    rbc.charBattleAnimations[rbc.animationBattleIndex].BAnimUpdate(gt, rbc);
                }
            }


            if (overrideGAnimation != -1)
            {
                gbc.charBattleAnimations[overrideGAnimation].BAnimUpdate(gt, gbc);
            }
            else
            {
                if (gbc.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Idle && (gbc.currentBattleAnimation().bAnimationFinished || gbc.currentBattleAnimation().bSimplePlayOnce))
                {
                    gbc.ChangeBattleAnimation(BaseCharacter.CharacterBattleAnimations.Idle);
                }
                gbc.charBattleAnimations[gbc.animationBattleIndex].BAnimUpdate(gt, gbc);
            }


            foreach (var item in GUIPopUps)
            {
                item.Update(gt);
            }
            GUIPopUps.RemoveAll(gp => gp.Remove());

            if (LHitChanceNum.Equals("", StringComparison.OrdinalIgnoreCase))
            {
                GetCasterTextReady();
            }

            //if (gbc.battleAnimationTaskList.Count == 0)
            //{
            //    switch (gbc.animationBattleIndex)
            //    {
            //        case (int)BaseCharacter.CharacterBattleAnimations.Attack:
            //            if (gbc.animationBattleIndex != (int)BaseCharacter.CharacterBattleAnimations.Idle && gbc.currentBattleAnimation().bAnimationFinished)
            //            {
            //                gbc.battleAnimationTaskList.Clear();
            //                gbc.animationBattleIndex = (int)BaseCharacter.CharacterBattleAnimations.Idle;
            //                gbc.charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].bSimplePlayOnce = false;
            //                gbc.charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].elapsedFrameTime = 0;
            //                gbc.charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].frameIndex = 0;
            //            }
            //            break;
            //    }
            //}


            //switch (rbc.animationBattleIndex)
            //{
            //    case (int)BaseCharacter.CharacterBattleAnimations.Death:
            //        if (gbc.animationBattleIndex != (int)BaseCharacter.CharacterBattleAnimations.Idle && gbc.currentBattleAnimation().bAnimationFinished)
            //        {
            //            gbc.battleAnimationTaskList.Clear();
            //            gbc.animationBattleIndex = (int)BaseCharacter.CharacterBattleAnimations.Idle;
            //            gbc.charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].bSimplePlayOnce = false;
            //            gbc.charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].elapsedFrameTime = 0;
            //            gbc.charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].frameIndex = 0;
            //        }
            //        break;
            //    case (int)BaseCharacter.CharacterBattleAnimations.Death_State:
            //        if (gbc.animationBattleIndex != (int)BaseCharacter.CharacterBattleAnimations.Idle && gbc.currentBattleAnimation().bAnimationFinished)
            //        {
            //            gbc.battleAnimationTaskList.Clear();
            //            gbc.animationBattleIndex = (int)BaseCharacter.CharacterBattleAnimations.Idle;
            //            gbc.charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].bSimplePlayOnce = false;
            //            gbc.charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].elapsedFrameTime = 0;
            //            gbc.charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].frameIndex = 0;
            //        }
            //        break;
            //}

            if (bIsAttacking && BattleAnimationInfo.completeAnimationInfo.Count != 0)
            {
                if (castAbilityGBC != null && (gbc.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack))
                {
                    //  castAbilityGBC.PAanim.UpdateAnimationForItems(gt);
                }

            }
        }


        enum abiType { Cast, GroundSelect }

        private static void PreAttackCall()
        {
            abiType type = abiType.Cast;
            type = castAbilityGBC.bIsAOE ? abiType.GroundSelect : abiType.Cast;
            //type = abiType.GroundSelect;
            switch (type)
            {
                case abiType.Cast:
                    AttemptAttack();
                    break;
                case abiType.GroundSelect:
                    rbc = gbc;
                    LUA.LuaAbilityInfo lai = LuaAbilityInfo.abiToLuaAbilityInfo(castAbilityGBC, gbc.toCharInfo(), new LuaCharacterInfo(), LuaAbilityInfo.CallType.Area);
                    lai.ExecuteScript();
                    bIsRunning = false;
                    TurnSet.bSelectingArea = true;

                    break;
                default:
                    break;
            }
        }

        internal static void HandleAreaConfirm()
        {
            List<BaseCharacter> allCharactersInArea = new List<BaseCharacter>();
            for (int i = 0; i < EncounterInfo.encounterGroups.Count; i++)
            {
                for (int j = 0; j < EncounterInfo.encounterGroups[i].charactersInGroup.Count; j++)
                {
                    for (int k = 0; k < castAbilityGBC.lamaTiles.Count; k++)
                    {
                        if (castAbilityGBC.lamaTiles[k].positionGrid == EncounterInfo.encounterGroups[i].charactersInGroup[j].positionToMapCoords())
                        {
                            allCharactersInArea.Add(EncounterInfo.encounterGroups[i].charactersInGroup[j]);
                            break;
                        }
                    }
                }
            }

            BattleGUI.surroundingRbcChars = GameProcessor.GetSurroundingChars(gbc);
            BattleGUI.surroundingGbcChars.Clear();


            bIsRunning = true;
            //bIsRunning = false;
            bHandleAttack = false;
            bHandleAreaAttack = true;
            charsForHandleArea = new List<BaseCharacter>(allCharactersInArea);
            BattleAnimationInfo.Initialize(gbc, gbc);
            BattleAnimationInfo.completeAnimationInfo.Clear();
            var anim3 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Attack, BaseCharacter.CharacterBattleAnimations.Idle, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
            BattleAnimationInfo.completeAnimationInfo.Add(anim3);
            anim3 = new BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations.Idle, BaseCharacter.CharacterBattleAnimations.Idle, BattleAnimationInfo.AnimationTimingType.PlayAtStart, BattleAnimationInfo.AnimationTimingType.PlayAtStart, gbc.battleAnimLocs[0], rbc.battleAnimLocs[0]);
            BattleAnimationInfo.completeAnimationInfo.Add(anim3);
            BattleAnimationInfo.Start(BattleAnimationInfo.completeAnimationInfo[0]);
            castAbilityGBC.PAanim.Reset();
            castAbilityGBC.PAanim.AssignRefCharSafe(null);

            gbc.CCC.battleStatistics.AddAoE();

            for (int i = 0; i < allCharactersInArea.Count; i++)
            {
                LUA.LuaAbilityInfo lai = LuaAbilityInfo.abiToLuaAbilityInfo(castAbilityGBC, gbc.toCharInfo(), allCharactersInArea[i].toCharInfo(), LuaAbilityInfo.CallType.Attack);
                lai.ExecuteScript();
                int randomDMG = (int)(GamePlayUtility.Randomize(lai.minDmg, lai.maxDmg));
                if (lai.bIsAttack)
                {
                    randomDMG *= -1;
                    //CombatProcessor.tacTextManager.AddText(randomDMG.ToString(), allCharactersInArea[i], new Vector2(32, 16), new Point(32), 60);
                    //CombatProcessor.tacTextManager.texts.Last().InitializeTextColor(Color.Red, Color.Gray);
                    gbc.CCC.battleStatistics.AddDMG(Math.Abs(randomDMG));
                }
                else
                {
                    //CombatProcessor.tacTextManager.AddText(randomDMG.ToString(), allCharactersInArea[i], new Vector2(32, 16), new Point(32), 60);
                    //CombatProcessor.tacTextManager.texts.Last().InitializeTextColor(Color.Green, Color.Wheat);
                    gbc.CCC.battleStatistics.AddHealing(randomDMG);
                }
                gbc.CCC.battleStatistics.AddHit();

                allCharactersInArea[i].statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] += randomDMG;


                lai.HandleAbilityMod();

                if (!allCharactersInArea[i].IsAlive())
                {
                    gbc.CCC.battleStatistics.AddKO();
                }
            }

        }

        internal static void HandleAreaCancel()
        {
            Start(gbc, rbc);
            bHandleAreaAttack = false;
            TurnSet.bSelectingArea = false;
        }

        private static void PlayAbilitySound()
        {

        }

        private static void PlayMissSound()
        {

        }

        private static void PerformCounterLogic()
        {
            if (!bMustSeeIfCanCouter)
            {
                CanTargetCounter();
            }

            if (bCounterAttack && !bHandledCounter)
            {
                ApplyAbilityDamageCounter(counterDMGs);
            }

        }

        private static void ApplyAbilityDamageCounter(KeyValuePair<int, int> counterDMG)
        {

            if (true)
            {
                //  bStartAttackDelayTimer = true;
                KeyValuePair<int, float> extraDamageAndModifierFromSurroundingChars = generateExtraCounterDamageAndModifierFromSurroundingChars();
                int randomDamage = GamePlayUtility.Randomize(counterDMG.Key + extraDamageAndModifierFromSurroundingChars.Key / 2, counterDMG.Value + extraDamageAndModifierFromSurroundingChars.Key / 2);
                gbc.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] -= randomDamage;
                // damageDoneByDealer = randomDamage;
                //if (randomDamage != 0)
                //{
                //    int xDiffernce = (int)(new Vector2(-RHGreenBarActualSource.Width + HGreenBarSource.Width, 0)).X;
                //    positionForHealthLost.X = RHGreenBar.X + xDiffernce;
                //}

                CalculateHBar();
                CalculateMBar();

                if (CombatProcessor.heroCharacters.Contains(rbc))
                {
                    BattleStats.AddDamageDoneByHero(rbc, randomDamage);
                    if (!gbc.IsAlive())
                    {
                        BattleStats.AddKBByHero(rbc);
                        gbc.battleAnimationTaskList.Clear();
                        //gbc.animationBattleIndex = (int)BaseCharacter.CharacterBattleAnimations.Death;
                        //gbc.battleAnimationTaskList.Add((int)BaseCharacter.CharacterBattleAnimations.Death_State);

                        //if (!bShowedGDmg)
                        //{
                        //    {
                        //        Vector2 startPos = Vector2.Zero;
                        //        if (jumpPosition != Vector2.Zero)
                        //        {
                        //            startPos = jumpPosition + new Vector2(100, 150);
                        //        }
                        //        else
                        //        {
                        //            startPos = gbc.battleAnimLocs[gbc.animationBattleIndex] * gbc.battleAnimScale + new Vector2(popUpTextLocationHMod, 150);
                        //        }

                        //        GUIPopUps.Add(new BGUIPopUpText(testSF32, rDMGCounter.ToString(), Color.Red, Color.White, 2, startPos, dmgTextTime, dmgTextFadeTime, deltaMovDmg));
                        //    }
                        //    bShowedGDmg = true;
                        //}
                    }
                }

                if (!gbc.IsAlive())
                {
                    giverWillDie = true;
                }

                bHandledCounter = true;

                rDMGCounter = randomDamage;
                //{
                //    Vector2 startPos = Vector2.Zero;
                //    if (jumpPosition != Vector2.Zero)
                //    {
                //        startPos = jumpPosition + new Vector2(100, 150);
                //    }
                //    else
                //    {
                //        startPos = gbc.battleAnimLocs[gbc.animationBattleIndex] * gbc.battleAnimScale + new Vector2(popUpTextLocationHMod, 150);
                //    }

                //    GUIPopUps.Add(new BGUIPopUpText(testSF32, randomDamage.ToString(), Color.Red, Color.White, 2, startPos, dmgTextTime, dmgTextFadeTime, deltaMovDmg));
                //}

            }


        }

        static float dmgTextTime = 3f;
        static float dmgTextFadeTime = 0.5f;

        static float modTextTime = 2f;
        static float modTextFadeTime = 0.5f;

        static Vector2 deltaMovDmg = new Vector2(0, -100);
        static Vector2 deltaMovMod = new Vector2(0, -180);

        static int popUpTextLocationHMod = 150;

        static KeyValuePair<int, int> counterDMGs = new KeyValuePair<int, int>();

        private static void CanTargetCounter()
        {
            bMustSeeIfCanCouter = false;

            BasicAbility abi = abilityIndex == -1 ? castAbilityGBC : gAbilityList[abilityIndex];
            //if (gbc.CCC.equippedClass.classType == rbc.CCC.equippedClass.classType && rbc.counterRange >= rbc.distanceFrom(gbc) && gbc != rbc)
            if (abi.CanCounterClass(rbc.CCC.equippedClass) && gbc != rbc && EncounterInfo.AreEnemies(gbc, rbc))
            {


                bHandledCounter = false;
                bTargetCanCounter = true;
                bCounterAttack = true;
                bDisplayCounterInfo = true;
                int minDMG = 0;
                switch (rbc.CCC.equippedClass.classType)
                {
                    case BaseClass.CLASSType.MELEE:
                        minDMG = rbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.STR] - gbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.DEF];
                        break;
                    case BaseClass.CLASSType.RANGED:
                        minDMG = rbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AGI] - (gbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.DEF] + gbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AGI]) / 2;
                        break;
                    case BaseClass.CLASSType.CASTER:
                        minDMG = rbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.INT] - gbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.INT];
                        break;
                    default:
                        break;
                }
                minDMG = (int)((float)minDMG / 3 * 2);
                if (minDMG < 0)
                {
                    minDMG = 0;
                }
                minDMG += rbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY];
                int maxDMG = minDMG + rbc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY];
                if (rbc.weapon != null)
                {
                    //rbc.weapon.statModifier.currentPassiveStats.ForEach(sm => maxDMG += sm);
                    switch (rbc.CCC.equippedClass.classType)
                    {
                        case BaseClass.CLASSType.MELEE:
                            maxDMG += rbc.weapon.statModifier.currentPassiveStats[(int)STATChart.PASSIVESTATS.STR];
                            break;
                        case BaseClass.CLASSType.RANGED:
                            maxDMG += rbc.weapon.statModifier.currentPassiveStats[(int)STATChart.PASSIVESTATS.AGI];
                            break;
                        case BaseClass.CLASSType.CASTER:
                            maxDMG += rbc.weapon.statModifier.currentPassiveStats[(int)STATChart.PASSIVESTATS.INT];
                            break;
                        default:
                            break;
                    }
                }

                if (minDMG * 2 < maxDMG)
                {
                    maxDMG = minDMG * 2;
                }

                counterDMGs = new KeyValuePair<int, int>(minDMG, maxDMG);
                KeyValuePair<int, float> extraDamageAndModifierFromSurroundingChars = generateExtraCounterDamageAndModifierFromSurroundingChars();

                RDMGNum = (minDMG + extraDamageAndModifierFromSurroundingChars.Key / 2).ToString() + " - " + (maxDMG + extraDamageAndModifierFromSurroundingChars.Key / 2).ToString();
            }
        }

        private static bool ShouldCounter()
        {
            return true;
        }

        private static void GetCasterTextReady()
        {
            try
            {
                var currentAttackTemp = LuaAbilityInfo.abiToLuaAbilityInfo(gAbilityList[abilityIndex], gbc.toCharInfo(), rbc.toCharInfo());
                currentAttackTemp.ExecuteScript();

                KeyValuePair<List<int>, List<int>> dmg = new KeyValuePair<List<int>, List<int>>();
                if (gAbilityList[abilityIndex].abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK)
                {
                    dmg = BattleProcessor.CalculateDamage(currentAttackTemp);

                    KeyValuePair<int, float> extraDamageAndModifierFromSurroundingChars = generateExtraDamageAndModifierFromSurroundingChars();
                    //int randomDamage = GamePlayUtility.Randomize(dmg.Key[0] + extraDamageAndModifierFromSurroundingChars.Key, dmg.Key[1] + (int)(extraDamageAndModifierFromSurroundingChars.Key * extraDamageAndModifierFromSurroundingChars.Value));


                    LDMGNum = (dmg.Key[0] + extraDamageAndModifierFromSurroundingChars.Key).ToString() + "-" + ((int)((dmg.Key[1] + extraDamageAndModifierFromSurroundingChars.Key) * extraDamageAndModifierFromSurroundingChars.Value)).ToString();

                    LHitChanceNum = (currentAttackTemp.hitChance).ToString();
                    LCRITChanceNum = (currentAttackTemp.critChance).ToString();
                }
                else if (gAbilityList[abilityIndex].abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT)
                {
                    dmg = BattleProcessor.CalculateHealing(currentAttackTemp);
                    //dmg.Key[0] = -dmg.Key[0];
                    //dmg.Key[1] = -dmg.Key[1];

                    KeyValuePair<int, float> extraDamageAndModifierFromSurroundingChars = generateExtraDamageAndModifierFromSurroundingChars();
                    LDMGNum = (dmg.Key[0] + extraDamageAndModifierFromSurroundingChars.Key).ToString() + "-" + ((int)((dmg.Key[1] + extraDamageAndModifierFromSurroundingChars.Key) * extraDamageAndModifierFromSurroundingChars.Value)).ToString();

                    LHitChanceNum = (currentAttackTemp.hitChance).ToString();
                    LCRITChanceNum = (currentAttackTemp.critChance).ToString();
                }

            }
            catch
            {


            }
        }

        static float zoom = 1.0f;
        static RenderTarget2D stageLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D BGLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D UILayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D finalBattleScreen = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

        static public void QuickDraw(SpriteBatch sb)
        {
            //sb.End();
            //sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            //LogicUpdate();

            #region 2nd Layer
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(stageLayer);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            sb.Draw(BattleGUIBG, Isometric, IsometricSource, Color.White);
            DrawBattleAnimation(sb);
            sb.End();
            #endregion

            #region Draw BG
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(BGLayer);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            sb.Draw(BattleGUIBG, new Rectangle(0, 0, 1366, 768), new Rectangle(0, 0, 1366, 768), Color.White);
            sb.End();
            #endregion

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(UILayer);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            #region Draw Upper Part Graphics
            sb.Draw(BattleGUITexture, LNameBox, LNameBoxSource, Color.White);

            if (rbc != gbc)
            {
                sb.Draw(BattleGUITexture, RNameBox, RNameBoxSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }


            // sb.Draw(BattleGUITexture, LHBox, HBoxSource, Color.White);
            // sb.Draw(BattleGUITexture, RHBox, HBoxSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);


            // sb.Draw(BattleGUITexture, LMBox, MBoxSource, Color.White);
            //sb.Draw(BattleGUITexture, RMBox, MBoxSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);




            sb.Draw(BattleGUITexture, LHBox, HBoxSource, Color.White);
            if (rbc != gbc)
            {
                sb.Draw(BattleGUITexture, RHBox, HBoxSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }


            sb.Draw(BattleGUITexture, LMBox, MBoxSource, Color.White);
            if (rbc != gbc)
            {
                sb.Draw(BattleGUITexture, RMBox, MBoxSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }


            if (healthTickTimePassed > healthTickTimer && positionForHealthLost.Width > 0)
            {
                healthTickTimePassed = 0;
                int modifier = 6;
                positionForHealthLost.X += modifier;
                positionForHealthLost.Width -= modifier * 2;

            }

            sb.Draw(Game1.hitboxHelp, positionForHealthLost, RHBarColor * .6f);

            sb.Draw(BattleGUITexture, LHGreenBar.Location.ToVector2(), LHGreenBarActualSource, LHBarColor);
            if (rbc != gbc)
            {
                sb.Draw(BattleGUITexture, RHGreenBar.Location.ToVector2() + new Vector2(-RHGreenBarActualSource.Width + HGreenBarSource.Width, 0), RHGreenBarActualSource, RHBarColor, 0f, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);

            }

            sb.Draw(BattleGUITexture, LMBlueBar.Location.ToVector2(), LMBlueBarActualSource, LMBarColor);
            if (rbc != gbc)
            {
                sb.Draw(BattleGUITexture, RMBlueBar.Location.ToVector2() + new Vector2(-RMBlueBarActualSource.Width + MBlueBarSource.Width, 0), RMBlueBarActualSource, RMBarColor);

            }




            foreach (var item in LShieldIcons)
            {
                sb.Draw(BattleGUITexture, item, ShieldIconsSource, Color.White);
            }

            if (gbc != rbc)
            {
                foreach (var item in RShieldIcons)
                {
                    sb.Draw(BattleGUITexture, item, ShieldIconsSource, Color.White);
                }
            }

            #endregion
            #region Draw Upper Text
            var lLoc = new Vector2(Math.Abs(testSF32.MeasureString(GName).X / 2 - 185), 28); //1335,28

            //gbc.displayName = "Knight";
            GName = gbc.displayName;

            {
                Vector2 lMeasure = testSF32.MeasureString(GName);
                int nameBoxOffset = 20;
                Rectangle lNameTextBox = new Rectangle(LNameBox.X + nameBoxOffset, LNameBox.Y + nameBoxOffset, LNameBox.Width - nameBoxOffset * 2, LNameBox.Height - nameBoxOffset * 2);
                Vector2 scale = new Vector2(lMeasure.X / (float)lNameTextBox.Width, lMeasure.Y / (float)lNameTextBox.Height);
                float neededScale = 1.0f;
                if (scale.X >= scale.Y)
                {
                    neededScale = scale.X;
                }
                else
                {
                    neededScale = scale.Y;
                }

                lLoc = new Vector2(Math.Abs(testSF32.MeasureString(GName).X / 2 - 185), 28); //1335,28
                if (scale.X < 1)
                {
                    scale.X = 1;
                    TextUtility.DrawString(sb, testSF32, GName, new Vector2(lLoc.X, LNameBox.Y + nameBoxOffset), new Vector2(1f / scale.X, 1f / scale.Y), Color.White, Color.Gray); //32,28

                }
                else
                {
                    TextUtility.DrawString(sb, testSF32, GName, new Vector2(LNameBox.X + nameBoxOffset, LNameBox.Y + nameBoxOffset), new Vector2(1f / scale.X, 1f / scale.Y), Color.White, Color.Gray); //32,28

                }
            }

            //*******************************************************

            {
                Vector2 lMeasure = testSF32.MeasureString(RName);
                int nameBoxOffset = 20;
                Rectangle lNameTextBox = new Rectangle(LNameBox.X + nameBoxOffset, LNameBox.Y + nameBoxOffset, LNameBox.Width - nameBoxOffset * 2, LNameBox.Height - nameBoxOffset * 2);
                Vector2 scale = new Vector2(lMeasure.X / (float)lNameTextBox.Width, lMeasure.Y / (float)lNameTextBox.Height);
                float neededScale = 1.0f;
                if (scale.X >= scale.Y)
                {
                    neededScale = scale.X;
                }
                else
                {
                    neededScale = scale.Y;
                }

                if (scale.X < 1)
                {
                    scale.X = 1;

                    Vector2 rLoc2 = new Vector2(Math.Abs(RNameText.Location.X - 150 - testSF32.MeasureString(RName).X / 2), RNameText.Location.ToVector2().Y); //1335,28
                    if (rbc != gbc)
                    {
                        TextUtility.DrawString(sb, testSF32, RName, new Vector2(rLoc2.X, LNameBox.Y + nameBoxOffset), new Vector2(1f / scale.X, 1f / scale.Y), Color.White, Color.Gray); //32,28
                    }
                }
                else
                {
                    TextUtility.DrawString(sb, testSF32, RName, new Vector2(RNameBox.X + nameBoxOffset, RNameBox.Y + nameBoxOffset), new Vector2(1f / scale.X, 1f / scale.Y), Color.White, Color.Gray); //32,28

                }

            }


            //var rLoc = new Vector2(Math.Abs(RNameText.Location.X - 150 - testSF32.MeasureString(RName).X / 2), RNameText.Location.ToVector2().Y); //1335,28
            //if (rbc != gbc)
            //{
            //    sb.DrawString(testSF32, RName, rLoc, Color.White);
            //}

            Vector2 rLoc = Vector2.Zero;


            //*******************************************************
            TextUtility.DrawString(sb, ActiveBarsFont, LHBarString, LHBarText.Location.ToVector2(), new Vector2(1), Color.Black, Color.Silver);
            if (bGDrawMana)
            {
                TextUtility.DrawString(sb, ActiveBarsFont, LMBarString, LMBarText.Location.ToVector2(), new Vector2(1), Color.Black, Color.Silver);
            }
            if (bGDrawEnergy)
            {
                TextUtility.DrawString(sb, ActiveBarsFont, LEBarString, LEBarText.Location.ToVector2(), new Vector2(1), Color.Black, Color.Silver);
            }
            //TODO SHIELD

            if (rbc != gbc)
            {
                rLoc = new Vector2(RHBarText.Location.ToVector2().X - ActiveBarsFont.MeasureString(RHBarString).X, RHBarText.Location.ToVector2().Y);
                TextUtility.DrawString(sb, ActiveBarsFont, RHBarString, rLoc, new Vector2(1), Color.Black, Color.Silver);
                if (bGDrawMana)
                {
                    rLoc = new Vector2(RMBarText.Location.ToVector2().X - ActiveBarsFont.MeasureString(RMBarString).X, RMBarText.Location.ToVector2().Y);
                    TextUtility.DrawString(sb, ActiveBarsFont, RMBarString, rLoc, new Vector2(1), Color.Black, Color.Silver);
                }
                if (bGDrawEnergy)
                {
                    rLoc = new Vector2(REBarText.Location.ToVector2().X - ActiveBarsFont.MeasureString(REBarString).X, REBarText.Location.ToVector2().Y);
                    TextUtility.DrawString(sb, ActiveBarsFont, REBarString, rLoc, new Vector2(1), Color.Black, Color.Silver);
                }
            }

            //TODO SHIELD


            #endregion
            #region Draw Lower Part Graphics
            if (true)
            {
                sb.Draw(BattleGUITexture, LowerHalfScreen, LowerHalfScreenSource, Color.White);
            }
            else
            {
                sb.Draw(BattleGUITexture, new Rectangle(LowerHalfScreen.X, LowerHalfScreen.Y, LowerHalfScreen.Width / 2, LowerHalfScreen.Height), new Rectangle(LowerHalfScreenSource.X, LowerHalfScreenSource.Y, LowerHalfScreenSource.Width / 2, LowerHalfScreenSource.Height), Color.White);
            }
            //  sb.Draw(BattleGUITexture, LowerHalfScreen, LowerHalfScreenSource, Color.White);
            try
            {
                sb.Draw(BattleGUITexture, LAbilityFrame, AbilityFrameSource, Color.White);
                sb.Draw(BattleGUITexture, LAbilityName, AbilityNameSource, Color.White);


                if (true)
                {
                    sb.Draw(BattleGUITexture, RAbilityFrame, AbilityFrameSource, Color.White);
                    sb.Draw(BattleGUITexture, RAbilityName, AbilityNameSource, Color.White);
                }
                //sb.Draw(Game1.hitboxHelp, LAbilityIcon, Color.White);
                //sb.Draw(Game1.hitboxHelp, RAbilityIcon, Color.White);
                if (gbc.weapon != null)
                {
                    gbc.weapon.itemTexAndAnimation.Draw(sb, LAbilityFrame);
                }

                if (rbc.weapon != null && rbc != gbc)
                {
                    rbc.weapon.itemTexAndAnimation.Draw(sb, RAbilityFrame);
                }
                // sb.Draw(gbAbi.abilityIcon, LAbilityFrame, gbAbi.iconTexBox, Color.White);
                // sb.Draw(rbAbi.abilityIcon, RAbilityFrame, rbAbi.iconTexBox, Color.White);
            }
            catch
            {

            }
            #endregion
            #region Draw Lower Text

            try
            {
                //   sb.DrawString(usedLAbiFont, LAbiString, LAbiText.Location.ToVector2(), Color.Black);
                // rLoc = new Vector2(RAbiText.Location.ToVector2().X - usedRAbiFont.MeasureString(RAbiString).X, RAbiText.Location.ToVector2().Y);
                // sb.DrawString(usedRAbiFont, RAbiString, rLoc, Color.Black);

                //sb.DrawString(usedLWpFont, LAbiString, LAbiText.Location.ToVector2(), Color.Black);
                rLoc = new Vector2(RAbiText.Location.ToVector2().X - usedRWpFont.MeasureString(RAbiString).X, RAbiText.Location.ToVector2().Y);
                //   sb.DrawString(usedRWpFont, RAbiString, rLoc, Color.Black);

                // sb.DrawString(testSF32, LWeaponString, LWeaponText.Location.ToVector2(), Color.Black);


                //*******************************************************
                //sb.Draw(Game1.hitboxHelp, LWeaponText, Color.Red);

                {
                    Vector2 lMeasure = testSF32.MeasureString(LWeaponString);
                    int nameBoxOffset = 16;
                    Rectangle lNameTextBox = new Rectangle(LWeaponText.X + nameBoxOffset, LWeaponText.Y + nameBoxOffset, LWeaponText.Width - nameBoxOffset * 2, LWeaponText.Height - nameBoxOffset * 2);
                    Vector2 scale = new Vector2(lMeasure.X / (float)lNameTextBox.Width, lMeasure.Y / (float)lNameTextBox.Height);
                    float neededScale = 1.0f;
                    if (scale.X >= scale.Y)
                    {
                        neededScale = scale.X;
                    }
                    else
                    {
                        neededScale = scale.Y;
                    }

                    if (scale.X < 1)
                    {
                        scale.X = 1;

                        Vector2 rLoc2 = new Vector2(Math.Abs(LWeaponText.Location.X + 150 - testSF32.MeasureString(LWeaponString).X / 2), LWeaponText.Location.ToVector2().Y); //1335,28
                        if (rbc != gbc)
                        {
                            TextUtility.DrawString(sb, testSF32, LWeaponString, new Vector2(rLoc2.X, LWeaponText.Y + nameBoxOffset), new Vector2(1f / scale.X, 1f / scale.Y), Color.White, Color.Gray); //32,28
                        }
                    }
                    else
                    {
                        TextUtility.DrawString(sb, testSF32, LWeaponString, new Vector2(LWeaponText.X + nameBoxOffset, LWeaponText.Y + nameBoxOffset), new Vector2(1f / scale.X, 1f / scale.Y), Color.White, Color.Gray); //32,28

                    }

                }




                if (rbc != gbc)
                {
                    //  rLoc = new Vector2(RWeaponText.Location.ToVector2().X - usedRWpFont.MeasureString(RWeaponString).X, RWeaponText.Location.ToVector2().Y);
                    //sb.DrawString(testSF32, RWeaponString, rLoc, Color.Black);

                    {
                        Vector2 lMeasure = testSF32.MeasureString(RWeaponString);
                        int nameBoxOffset = 16;
                        Rectangle lNameTextBox = new Rectangle(RWeaponText.X + nameBoxOffset, RWeaponText.Y + nameBoxOffset, RWeaponText.Width - nameBoxOffset * 2, RWeaponText.Height - nameBoxOffset * 2);
                        Vector2 scale = new Vector2(lMeasure.X / (float)lNameTextBox.Width, lMeasure.Y / (float)lNameTextBox.Height);
                        float neededScale = 1.0f;
                        if (scale.X >= scale.Y)
                        {
                            neededScale = scale.X;
                        }
                        else
                        {
                            neededScale = scale.Y;
                        }

                        if (scale.X < 1)
                        {
                            scale.X = 1;

                            Vector2 rLoc2 = new Vector2(Math.Abs(RWeaponText.Location.X + 150 - testSF32.MeasureString(RWeaponString).X / 2), RWeaponText.Location.ToVector2().Y); //1335,28
                            if (rbc != gbc)
                            {
                                TextUtility.DrawString(sb, testSF32, RWeaponString, new Vector2(rLoc2.X, RWeaponText.Y + nameBoxOffset), new Vector2(1f / scale.X, 1f / scale.Y), Color.White, Color.Gray); //32,28
                            }
                        }
                        else
                        {
                            TextUtility.DrawString(sb, testSF32, RWeaponString, new Vector2(RWeaponText.X + nameBoxOffset, RWeaponText.Y + nameBoxOffset), new Vector2(1f / scale.X, 1f / scale.Y), Color.White, Color.Gray); //32,28

                        }

                    }
                }

            }
            catch
            {

            }

            #endregion

            TextUtility.DrawString(sb, testSF25, LHitChanceString + " " + LHitChanceNum, new Vector2(30, 610), new Vector2(1), Color.Gold, Color.Silver); //30,610
            if (EncounterInfo.currentTurn().bIsPlayerTurnSet && !EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden)
            {
                TextUtility.DrawString(sb, testSF20, "Total AP: " + gbc.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP], new Vector2(250, 717), new Vector2(1), Color.Green, Color.LightYellow);
                if (abilityIndex == -1)
                {

                }
                else
                {
                    TextUtility.DrawString(sb, testSF20, " - " + gAbilityList[abilityIndex].currentAPCost(), new Vector2(250 + testSF20.MeasureString("Total AP: " + gbc.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP]).X, 717), new Vector2(1), Color.Red, Color.Black);
                }

            }
            TextUtility.DrawString(sb, testSF20, LDMGString + " " + LDMGNum, new Vector2(450, 717), new Vector2(1), Color.Gold, Color.Silver); //450,717
            TextUtility.DrawString(sb, testSF25, LCRITChanceString + " " + LCRITChanceNum, new Vector2(30, 690), new Vector2(1), Color.Gold, Color.Silver); //30,690
            try
            {
                TextUtility.DrawString(sb, testSF32, gAbilityList[abilityIndex].abilityName, new Vector2(250, 670), new Vector2(1), Color.LightGoldenrodYellow, Color.Silver);
            }
            catch
            {
            }

            if (bDisplayCounterInfo || bChooseAbility)
            {
                TextUtility.DrawString(sb, testSF32, "COUNTER", new Vector2(750, 670), new Vector2(1), Color.LightGoldenrodYellow, Color.Silver);
                TextUtility.DrawString(sb, testSF20, RDMGString + " " + RDMGNum, new Vector2(900, 717), new Vector2(1), Color.Gold, Color.Silver); //450,717
            }



            if (bChooseAbility)
            {
                sb.Draw(BattleGUITexture, new Rectangle(1025, 250, 302, 60), AbilitySelectionPanelSource, Color.White * .7f);
                sb.Draw(BattleGUITexture, new Vector2(900, 340), AbilitySelectionPanelSource, Color.White * .7f);
                sb.Draw(BattleGUITexture, new Rectangle(1025, 456, 302, 60), AbilitySelectionPanelSource, Color.White * .7f);
                TextUtility.DrawString(sb, testSF25, gAbilityList[abilityPrev].abilityName, new Vector2(1100, 258), new Vector2(1), Color.Silver * .7f, Color.Silver * 0.7f);
                TextUtility.DrawString(sb, testSF25, gAbilityList[abilityNext].abilityName, new Vector2(1100, 464), new Vector2(1), Color.Silver * .7f, Color.Silver * 0.7f);
                //sb.Draw(Game1.hitboxHelp, new Rectangle(903, 343, 80, 80), Color.Purple * .7f);
                gAbilityList[abilityIndex].abilityIcon.Draw(sb, new Rectangle(903, 343, 80, 80), .4f);
                if (!gAbilityList[abilityIndex].abilityCanHitTargetInRange(gbc, rbc))
                {
                    TextUtility.DrawString(sb, TinyAbiFont, "Not in range!", new Vector2(990, 410), new Vector2(1), Color.Red * .7f,Color.Gray*.7f);
                }

                TextUtility.DrawString(sb, TinyAbiFont, gAbilityList[abilityIndex].abilityName, new Vector2(990, 340), new Vector2(1), Color.Silver * .7f, Color.Gold * .7f);
                TextUtility.DrawString(sb, TinyAbiFont, "MP " + gAbilityList[abilityIndex].currentMPCost().ToString(), new Vector2(990, 380), new Vector2(1), Color.Blue * .7f, Color.Gray * .7f);
                TextUtility.DrawString(sb, TinyAbiFont, "AP " + gAbilityList[abilityIndex].currentAPCost().ToString(), new Vector2(1200, 380), new Vector2(1), Color.Green * .7f, Color.Gray * .7f);

                var r = new Rectangle(11, 200, 300, 400);
                var r2 = new Rectangle(31, 220, 260, 360);
                String t = gAbilityList[abilityIndex].abilityDescription;
                // String testText = TextUtility.bestMatchStringForBox(t, testSF20, r2);
                descriptionPanel.Draw(sb, Color.White * .5f);
                float opac = 0.4f;
                //TextUtility.ClearCache();
                TextUtility.DrawComplex(sb, t, testSF32, r2, TextUtility.OutLining.Center, Color.White * opac, 1f, true, default(Matrix), Color.DarkGray * opac);

                BasicAbility focus = gAbilityList[abilityIndex];
                r2 = new Rectangle(31, 220, 260, 50);
                if (focus.IsOnCoolDown())
                {
                    String cdString = "On cooldown: " + focus.abilityCooldownTimer + " turns";
                    TextUtility.Draw(sb, cdString, testSF32, r2, TextUtility.OutLining.Center, Color.White * opac, 1f, true, default(Matrix), Color.DarkGray * opac, false);
                }
                else if (focus.abilityCooldown > 1)
                {
                    String cdString = "Cooldown: " + focus.abilityCooldown + " turns";
                    TextUtility.Draw(sb, cdString, testSF32, r2, TextUtility.OutLining.Center, Color.White * opac, 1f, true, default(Matrix), Color.DarkGray * opac, false);
                }

                //sb.Draw(BattleGUITexture, r, AbilitySelectionPanelSource, Color.White * .5f);
                //sb.Draw(Game1.hitboxHelp, r2, Color.Green);
                //sb.DrawString(testSF20, testText, r2.Location.ToVector2(), Color.Silver * .7f);
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(finalBattleScreen);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            float zoomAmount = -(1.0f - zoom);
            float parallaxDifference = 0.6f;
            Vector2 posBG = new Vector2(1366 / 2, 768 / 2) * zoomAmount;

            float parallaxZoom = (1.0f + (parallaxDifference * zoomAmount * 2) * 1.5f);
            Vector2 scaledCenter = new Vector2(1366 / 2, 768 / 2) * (parallaxZoom - 1.0f);

            sb.Draw(BGLayer, -posBG, BGLayer.Bounds, Color.White, 0f, Vector2.Zero, zoom, SpriteEffects.None, 0);

            sb.Draw(stageLayer, -scaledCenter, stageLayer.Bounds, Color.White, 0f, Vector2.Zero, parallaxZoom, SpriteEffects.None, 0);
            sb.Draw(UILayer, BGLayer.Bounds, Color.White);
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(GameProcessor.gameRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            GameProcessor.UIRenders.Add(finalBattleScreen);
        }

        static public void QuickDrawInEditor(SpriteBatch sb, GameTime gt)
        {
            LogicUpdate(gt);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
            #region Draw BG
            sb.Draw(BattleGUIBG, new Rectangle(0, 0, 1366, 768), new Rectangle(0, 0, 1366, 768), Color.White);
            sb.Draw(BattleGUIBG, Isometric, IsometricSource, Color.White);
            #endregion
            #region Draw Upper Part Graphics
            sb.Draw(BattleGUITexture, LNameBox, LNameBoxSource, Color.White);
            sb.Draw(BattleGUITexture, RNameBox, RNameBoxSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

            sb.Draw(BattleGUITexture, LHBox, HBoxSource, Color.White);
            sb.Draw(BattleGUITexture, RHBox, HBoxSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

            sb.Draw(BattleGUITexture, LMBox, MBoxSource, Color.White);
            sb.Draw(BattleGUITexture, RMBox, MBoxSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

            sb.Draw(BattleGUITexture, LHGreenBar.Location.ToVector2(), LHGreenBarActualSource, LHBarColor);
            sb.Draw(BattleGUITexture, RHGreenBar.Location.ToVector2() + new Vector2(-RHGreenBarActualSource.Width + HGreenBarSource.Width, 0), RHGreenBarActualSource, RHBarColor, 0f, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);

            sb.Draw(BattleGUITexture, LMBlueBar.Location.ToVector2(), LMBlueBarActualSource, LMBarColor);
            sb.Draw(BattleGUITexture, RMBlueBar.Location.ToVector2() + new Vector2(-RMBlueBarActualSource.Width + MBlueBarSource.Width, 0), RMBlueBarActualSource, RMBarColor);
            // sb.Draw(BattleGUITexture, RMBox, HGreenBarSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

            foreach (var item in LShieldIcons)
            {
                sb.Draw(BattleGUITexture, item, ShieldIconsSource, Color.White);
            }

            foreach (var item in RShieldIcons)
            {
                sb.Draw(BattleGUITexture, item, ShieldIconsSource, Color.White);
            }

            #endregion
            /* #region Draw Upper Text
             sb.DrawString(usedLNameFont, GName, LNameText.Location.ToVector2(), Color.White);

             var rLoc = new Vector2(RNameText.Location.X - usedRNameFont.MeasureString(RName).X, RNameText.Location.ToVector2().Y);
             sb.DrawString(usedRNameFont, RName, rLoc, Color.White);

             sb.DrawString(ActiveBarsFont, LHBarString, LHBarText.Location.ToVector2(), Color.Black);
             if (bGDrawMana)
             {
                 sb.DrawString(ActiveBarsFont, LMBarString, LMBarText.Location.ToVector2(), Color.Black);
             }
             if (bGDrawEnergy)
             {
                 sb.DrawString(ActiveBarsFont, LEBarString, LEBarText.Location.ToVector2(), Color.Black);
             }
             //TODO SHIELD

             rLoc = new Vector2(RHBarText.Location.ToVector2().X - ActiveBarsFont.MeasureString(RHBarString).X, RHBarText.Location.ToVector2().Y);
             sb.DrawString(ActiveBarsFont, RHBarString, rLoc, Color.Black);
             if (bGDrawMana)
             {
                 rLoc = new Vector2(RMBarText.Location.ToVector2().X - ActiveBarsFont.MeasureString(RMBarString).X, RMBarText.Location.ToVector2().Y);
                 sb.DrawString(ActiveBarsFont, RMBarString, rLoc, Color.Black);
             }
             if (bGDrawEnergy)
             {
                 rLoc = new Vector2(REBarText.Location.ToVector2().X - ActiveBarsFont.MeasureString(REBarString).X, REBarText.Location.ToVector2().Y);
                 sb.DrawString(ActiveBarsFont, REBarString, rLoc, Color.Black);
             }
             //TODO SHIELD


             #endregion*/
            #region Draw Lower Part Graphics
            sb.Draw(BattleGUITexture, LowerHalfScreen, LowerHalfScreenSource, Color.White);
            try
            {
                LAbilityFrame.Y = 550;
                RAbilityFrame.Y = 550;
                LAbilityName.Y = 550;
                RAbilityName.Y = 550;
                sb.Draw(BattleGUITexture, LAbilityFrame, AbilityFrameSource, Color.White);
                sb.Draw(BattleGUITexture, RAbilityFrame, AbilityFrameSource, Color.White);
                sb.Draw(BattleGUITexture, LAbilityName, AbilityNameSource, Color.White);
                sb.Draw(BattleGUITexture, RAbilityName, AbilityNameSource, Color.White);
                //sb.Draw(gbAbi.abilityIcon, new Rectangle(LAbilityFrame.X + 8, LAbilityFrame.Y + 8, LAbilityFrame.Width - 16, LAbilityFrame.Height - 16), gbAbi.iconTexBox, Color.White);
                //sb.Draw(rbAbi.abilityIcon, new Rectangle(RAbilityFrame.X + 8, RAbilityFrame.Y + 8, RAbilityFrame.Width - 16, RAbilityFrame.Height - 16), rbAbi.iconTexBox, Color.White);
            }
            catch
            {

            }
            #endregion
            /*   #region Draw Lower Text
               sb.DrawString(usedLAbiFont, LAbiString, LAbiText.Location.ToVector2(), Color.Black);
               rLoc = new Vector2(RAbiText.Location.ToVector2().X - usedRAbiFont.MeasureString(RAbiString).X, RAbiText.Location.ToVector2().Y);
               sb.DrawString(usedRAbiFont, RAbiString, rLoc, Color.Black);
               try
               {
                   sb.DrawString(usedLWpFont, LWeaponString, LWeaponText.Location.ToVector2(), Color.Black);
                   rLoc = new Vector2(RWeaponText.Location.ToVector2().X - usedRWpFont.MeasureString(RWeaponString).X, RWeaponText.Location.ToVector2().Y);
                   sb.DrawString(usedRWpFont, RWeaponString, rLoc, Color.Black);
               }
               catch
               {

               }

               #endregion*/

            if (rbc != null)
            {
                rbc.charBattleAnimations[rbc.animationBattleIndex].BAnimUpdate(gt, rbc);
                MapBuilder.gcDB.gameParticleAnimations[0].UpdateAnimationForItems(gt);
            }
            if (gbc != null && gbc != rbc)
            {
                gbc.charBattleAnimations[gbc.animationBattleIndex].BAnimUpdate(gt, gbc);
            }
            DrawBattleAnimation(sb);
            sb.End();
        }

        static void DrawBattleAnimation(SpriteBatch sb, GameTime gt)
        {
            #region Draw and Update Battle Animations
            try
            {

                if (overrideRAnimation != -1)
                {
                    rbc.charBattleAnimations[overrideRAnimation].BAnimUpdate(gt, rbc);
                    Vector2 rLoc = new Vector2(1366 - (rbc.battleAnimLocs[overrideRAnimation].X + rbc.charBattleAnimations[overrideRAnimation].animationFrames[0].Width * rbc.battleAnimScale), rbc.battleAnimLocs[overrideRAnimation].Y);
                    rbc.charBattleAnimations[overrideRAnimation].BattleAnimationDraw(sb, rLoc, rbc.battleAnimScale, true);

                    //MapBuilder.gcDB.gameParticleAnimations[0].UpdateAnimationForItems(gt);
                    //MapBuilder.gcDB.gameParticleAnimations[0].BattleAnimationDrawWOShadow(sb, rLoc, rbc.battleAnimScale, true);
                }
                else
                {
                    rbc.charBattleAnimations[rbc.animationBattleIndex].BAnimUpdate(gt, rbc);
                    Vector2 rLoc = new Vector2(1366 - (rbc.battleAnimLocs[rbc.animationBattleIndex].X + rbc.charBattleAnimations[rbc.animationBattleIndex].animationFrames[0].Width * rbc.battleAnimScale), rbc.battleAnimLocs[rbc.animationBattleIndex].Y);
                    rbc.charBattleAnimations[rbc.animationBattleIndex].BattleAnimationDraw(sb, rLoc, rbc.battleAnimScale, true);

                    MapBuilder.gcDB.gameParticleAnimations[0].BattleAnimationDrawWOShadow(sb, rLoc, rbc.battleAnimScale, true);
                    //rLoc += new Vector2((rbc.charBattleAnimations[rbc.animationBattleIndex].animationFrames[0].Width * rbc.battleAnimScale) / 2 - (100 * (rbc.battleAnimScale - 2)), 0);
                    //rLoc.Y = rbc.battleAnimLocs[overrideRAnimation].Y - (rbc.charBattleAnimations[rbc.animationBattleIndex].animationFrames[0].Width * rbc.battleAnimScale);
                    //MapBuilder.gcDB.gameParticleAnimations[0].UpdateAnimationForItems(gt);
                    //MapBuilder.gcDB.gameParticleAnimations[0].BattleAnimationDrawWOShadow(sb, rLoc, rbc.battleAnimScale - 2, true);
                }



                if (overrideGAnimation != -1)
                {
                    gbc.charBattleAnimations[overrideGAnimation].BAnimUpdate(gt, gbc);
                    if (jumpPosition != Vector2.Zero)
                    {
                        gbc.charBattleAnimations[overrideGAnimation].BattleAnimationDraw(sb, jumpPosition, gbc.battleAnimScale);
                    }
                    else
                    {
                        gbc.charBattleAnimations[overrideGAnimation].BattleAnimationDraw(sb, gbc.battleAnimLocs[overrideGAnimation], gbc.battleAnimScale);
                    }
                }
                else
                {
                    gbc.charBattleAnimations[gbc.animationBattleIndex].BAnimUpdate(gt, gbc);
                    if (jumpPosition != Vector2.Zero)
                    {
                        gbc.charBattleAnimations[gbc.animationBattleIndex].BattleAnimationDraw(sb, jumpPosition, gbc.battleAnimScale);
                    }
                    else
                    {
                        gbc.charBattleAnimations[gbc.animationBattleIndex].BattleAnimationDraw(sb, gbc.battleAnimLocs[gbc.animationBattleIndex], gbc.battleAnimScale);
                    }
                }
            }
            catch
            {

            }

            #endregion
        }

        static Vector2 assistPosition = new Vector2();

        static bool bHasAssist = false;

        static void DrawBattleAnimation(SpriteBatch sb)
        {
            #region Draw and Update Battle Animations
            try
            {
                if (rbc != gbc || !GameProcessor.bIsInGame)
                {
                    if (!bHasAssist)
                    {
                        if (overrideRAnimation != -1)
                        {
                            Vector2 rLoc = new Vector2(1366 - (rbc.battleAnimLocs[overrideRAnimation].X + rbc.charBattleAnimations[overrideRAnimation].animationFrames[0].Width * rbc.battleAnimScale), rbc.battleAnimLocs[overrideRAnimation].Y);
                            rbc.charBattleAnimations[overrideRAnimation].BattleAnimationDraw(sb, rLoc, rbc.battleAnimScale, true);
                        }
                        else
                        {
                            Vector2 rLoc = new Vector2(1366 - (rbc.battleAnimLocs[rbc.animationBattleIndex].X + rbc.charBattleAnimations[rbc.animationBattleIndex].animationFrames[0].Width * rbc.battleAnimScale), rbc.battleAnimLocs[rbc.animationBattleIndex].Y);
                            //  rbc.charBattleAnimations[rbc.animationBattleIndex].BattleAnimationDraw(sb, rLoc, rbc.battleAnimScale, true);
                            var charLoc = rLoc;

                            if (gbc.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack && castAbilityGBC != null && castAbilityGBC.PAanim != null && castAbilityGBC.PAanim != default(ParticleAnimation) && !castAbilityGBC.PAanim.bAnimationFinished)
                            {

                                rLoc = new Vector2(630, -35);
                                castAbilityGBC.PAanim.BattleAnimationDrawWOShadowTrue(sb, rLoc, 3.6f - 0.5f, charLoc, rbc.battleAnimScale, true);
                            }
                            else
                            {
                                if (rbc.animationBattleIndex != (int)BaseCharacter.CharacterBattleAnimations.Death_State)
                                {
                                    rbc.currentBattleAnimation().BattleAnimationDraw(sb, charLoc, rbc.battleAnimScale, true);
                                }
                                else
                                {
                                    rbc.currentBattleAnimation().BattleAnimationDrawWOShadow(sb, charLoc, rbc.battleAnimScale, true);
                                }
                            }

                        }
                    }
                    else
                    {

                        if (overrideRAnimation != -1)
                        {
                            Vector2 rLoc = new Vector2(1366 - (rbc.battleAnimLocs[overrideRAnimation].X + rbc.charBattleAnimations[overrideRAnimation].animationFrames[0].Width * rbc.battleAnimScale), rbc.battleAnimLocs[overrideRAnimation].Y);
                            assistPosition = rLoc + new Vector2(200, 0);
                            rbc.charBattleAnimations[overrideRAnimation].BattleAnimationDraw(sb, rLoc, rbc.battleAnimScale, true);
                        }
                        else
                        {
                            Vector2 rLoc = new Vector2(1366 - (rbc.battleAnimLocs[rbc.animationBattleIndex].X + rbc.charBattleAnimations[rbc.animationBattleIndex].animationFrames[0].Width * rbc.battleAnimScale), rbc.battleAnimLocs[rbc.animationBattleIndex].Y);
                            rbc.charBattleAnimations[rbc.animationBattleIndex].BattleAnimationDraw(sb, rLoc, rbc.battleAnimScale, true);
                        }
                    }

                    if (charGbcBehind.Key != default(BaseCharacter))
                    {
                        Vector2 rLoc = new Vector2(0, charGbcBehind.Key.battleAnimLocs[0].Y);
                        charGbcBehind.Key.charBattleAnimations[0].BattleAnimationDraw(sb, rLoc + new Vector2(50, 0), charGbcBehind.Key.battleAnimScale, !true, Color.White * .35f);
                    }

                    if (charRbcBehind.Key != default(BaseCharacter))
                    {
                        Vector2 rLoc = new Vector2(1366 - (charRbcBehind.Key.battleAnimLocs[0].X + charRbcBehind.Key.charBattleAnimations[0].animationFrames[0].Width * charRbcBehind.Key.battleAnimScale), charRbcBehind.Key.battleAnimLocs[0].Y);
                        charRbcBehind.Key.charBattleAnimations[0].BattleAnimationDraw(sb, rLoc + new Vector2(200, 0), charRbcBehind.Key.battleAnimScale, true, Color.White * .35f);
                    }
                }

                if (charRbcLeft.Key != default(BaseCharacter))
                {
                    Vector2 rLoc = new Vector2(1366 / 2 - (charRbcLeft.Key.battleAnimLocs[0].X + charRbcLeft.Key.charBattleAnimations[0].animationFrames[0].Width * charRbcLeft.Key.battleAnimScale) / 2, charRbcLeft.Key.battleAnimLocs[0].Y);
                    charRbcLeft.Key.charBattleAnimations[0].BattleAnimationDraw(sb, rLoc + new Vector2(200, -50), charRbcLeft.Key.battleAnimScale, charRbcLeft.Value, Color.White * .35f);
                }

                if (charGbcLeft.Key != default(BaseCharacter))
                {
                    Vector2 rLoc = new Vector2(0 - (charGbcLeft.Key.battleAnimLocs[0].X + charGbcLeft.Key.charBattleAnimations[0].animationFrames[0].Width * charGbcLeft.Key.battleAnimScale) / 2, charGbcLeft.Key.battleAnimLocs[0].Y);
                    charGbcLeft.Key.charBattleAnimations[0].BattleAnimationDraw(sb, rLoc + new Vector2(550, -50), charGbcLeft.Key.battleAnimScale, !charGbcLeft.Value, Color.White * .35f);
                }

                if (overrideGAnimation != -1)
                {
                    if (jumpPosition != Vector2.Zero)
                    {
                        gbc.charBattleAnimations[overrideGAnimation].BattleAnimationDraw(sb, jumpPosition, gbc.battleAnimScale);
                    }
                    else
                    {
                        gbc.charBattleAnimations[overrideGAnimation].BattleAnimationDraw(sb, gbc.battleAnimLocs[overrideGAnimation], gbc.battleAnimScale);
                    }
                }
                else
                {
                    //gbc.charBattleAnimations[gbc.animationBattleIndex].BattleAnimationDraw(sb, BattleAnimationInfo.GBCPosition(), gbc.battleAnimScale);
                    //if (jumpPosition != Vector2.Zero)
                    //{
                    //    gbc.charBattleAnimations[gbc.animationBattleIndex].BattleAnimationDraw(sb, jumpPosition, gbc.battleAnimScale);
                    //}
                    //else
                    //{
                    //    gbc.charBattleAnimations[gbc.animationBattleIndex].BattleAnimationDraw(sb, gbc.battleAnimLocs[gbc.animationBattleIndex], gbc.battleAnimScale);
                    //}

                    if (gbc.animationBattleIndex != (int)BaseCharacter.CharacterBattleAnimations.Death_State)
                    {
                        var v2 = BattleAnimationInfo.GBCPosition();
                        gbc.currentBattleAnimation().BattleAnimationDraw(sb, BattleAnimationInfo.GBCPosition(), gbc.battleAnimScale);
                    }
                    else
                    {
                        gbc.currentBattleAnimation().BattleAnimationDrawWOShadow(sb, BattleAnimationInfo.GBCPosition(), gbc.battleAnimScale);
                    }
                }

                if (charRbcRight.Key != default(BaseCharacter))
                {
                    Vector2 rLoc = new Vector2(1366 / 2 - (charRbcRight.Key.battleAnimLocs[0].X + charRbcRight.Key.charBattleAnimations[0].animationFrames[0].Width * charRbcRight.Key.battleAnimScale) / 2, charRbcRight.Key.battleAnimLocs[0].Y);
                    charRbcRight.Key.charBattleAnimations[0].BattleAnimationDraw(sb, rLoc + new Vector2(150, 40), charRbcRight.Key.battleAnimScale, charRbcRight.Value, Color.White * .35f);
                }

                if (charGbcRight.Key != default(BaseCharacter))
                {
                    Vector2 rLoc = new Vector2(0 - (charGbcRight.Key.battleAnimLocs[0].X + charGbcRight.Key.charBattleAnimations[0].animationFrames[0].Width * charGbcRight.Key.battleAnimScale) / 2, charGbcRight.Key.battleAnimLocs[0].Y);
                    charGbcRight.Key.charBattleAnimations[0].BattleAnimationDraw(sb, rLoc + new Vector2(650, 40), charGbcRight.Key.battleAnimScale, !charGbcRight.Value, Color.White * .35f);
                }
            }
            catch
            {

            }

            if (bHandleAreaAttack && !castAbilityGBC.PAanim.bAnimationFinished)
            {
                Vector2 rLoc = new Vector2(630, -35);
                castAbilityGBC.PAanim.BattleAnimationDrawWOShadowTrue(sb, rLoc, 3.6f - 0.5f, Vector2.Zero, 6f, true);
            }

            #endregion
        }

        static public void DrawCursor(SpriteBatch sb, Vector2 location, float zoom)
        {
            sb.Draw(BattleGUITexture, location, HoverCursorSource, Color.White);
        }

        static public void DrawBlueTiles(SpriteBatch sb, List<BasicTile> bt)
        {
            foreach (var item in bt)
            {
                sb.Draw(BattleGUITexture, item.mapPosition, BlueMovementTileSource, Color.White);
            }

        }

        static public void DrawCharacterArea(SpriteBatch sb, CharacterTurn ct, float opacity = 1f)
        {
            var l = CombatProcessor.radiusTiles;
            //foreach (var list in ct.characterArea)
            //{
            //    foreach (var item in list)
            //    {
            //        sb.Draw(BattleGUITexture, item.mapPosition, BlueMovementTileSource, Color.White * (1f - 0.3f * ct.characterArea.IndexOf(list)));
            //    }
            //}
            //foreach (var item in l)
            //{

            //    // var MaxAP = ct.character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP];
            //    var MaxAP = ct.characterAP;
            //    var expendedAP = ct.characterArea.IndexOf(ct.characterArea.Find(area => area.Contains(item))) + 1;
            //    //selectedCharTurn.character.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP] += MaxAP - expendedAP;

            //    sb.Draw(BattleGUITexture, item.mapPosition, BlueMovementTileSource, Color.White);
            //    sb.DrawString(testSF, "+" + (MaxAP - expendedAP).ToString() + "AP", item.mapPosition.Location.ToVector2() + (new Vector2(40, 45)), Color.White, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0);
            //}

            for (int i = 0; i < ct.characterArea.Count; i++)
            {
                for (int j = 0; j < ct.characterArea[i].Count; j++)
                {
                    if (CombatProcessor.radiusTiles.Contains(ct.characterArea[i][j]))
                    {
                        sb.Draw(BattleGUITexture, ct.characterArea[i][j].mapPosition, BlueMovementTileSource, Color.White);
                        sb.DrawString(testSF, "+" + (ct.characterAP - i).ToString() + "AP", ct.characterArea[i][j].mapPosition.Location.ToVector2() + (new Vector2(40, 45)), Color.White, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0);
                    }
                }
            }
        }

        static public void DrawCharacterArea(SpriteBatch sb, List<BasicTile> lbt, float opacity = 1f)
        {
            foreach (var item in lbt)
            {
                sb.Draw(BattleGUITexture, item.mapPosition, BlueMovementTileSource, Color.White * opacity);
            }
        }

        static public void DrawCharacterAttackArea(SpriteBatch sb, List<BasicTile> bt, float opacity = 1f)
        {
            foreach (var item in bt)
            {
                sb.Draw(BattleGUITexture, item.mapPosition, RedAttackTileSource, Color.White * opacity);
            }
        }

        internal static void DrawCharacterSupportArea(SpriteBatch sb, List<BasicTile> bt, float opacity = 1.0f)
        {
            foreach (var item in bt)
            {
                sb.Draw(BattleGUITexture, item.mapPosition, GreenFriendlyTileSource, Color.White * opacity);
            }
        }


        static public void InitializePopUpMenu()
        {
            popUpMenuSelection = (int)PopUpChoices.Attack;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="location"> Relative to screen, so between 1366x768</param>
        static public void DrawPopUpMenu(SpriteBatch sb, bool bShowRight = true)
        {
            //192 25%
            //575 75%
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

            if (selectedTarget != null)
            {
                Rectangle targetPositionOnScreen = new Rectangle((int)(selectedTarget.spriteGameSize.X * GameProcessor.zoom), (int)(selectedTarget.spriteGameSize.Y * GameProcessor.zoom), (int)(selectedTarget.spriteGameSize.Width * GameProcessor.zoom), (int)(selectedTarget.spriteGameSize.Height * GameProcessor.zoom));
                Rectangle menuScreenSize = new Rectangle((int)new Vector2(1366 * .75f - 130 - GameProcessor.sceneCamera.X * GameProcessor.zoom, 766 / 2 - 135 - GameProcessor.sceneCamera.Y * GameProcessor.zoom).X, (int)new Vector2(1366 * .75f - 130 - GameProcessor.sceneCamera.X * GameProcessor.zoom, 766 / 2 - 135 - GameProcessor.sceneCamera.Y * GameProcessor.zoom).Y, 265, 331);
                if (targetPositionOnScreen.Contains(menuScreenSize) || targetPositionOnScreen.Intersects(menuScreenSize))
                {
                    bShowRight = false;
                }

            }

            if (bShowRight)
            {
                Vector2 location = new Vector2(1366 * .75f - 130, 766 / 2 - 135);
                switch (popUpMenuSelection)
                {
                    case (int)PopUpChoices.Attack:
                        AttackPopUpInfo(sb, location);
                        sb.Draw(BattleGUITexture, location, PopUpMenuAttackSource, Color.White);
                        break;
                    case (int)PopUpChoices.Item:
                        sb.Draw(BattleGUITexture, location, PopUpMenuItemSource, Color.White);
                        break;
                    case (int)PopUpChoices.Defend:
                        sb.Draw(BattleGUITexture, location, PopUpMenuDefendSource, Color.White);
                        break;
                    case (int)PopUpChoices.Interact:
                        sb.Draw(BattleGUITexture, location, PopUpMenuInteractSource, Color.White);
                        break;
                }
            }
            else
            {
                Vector2 location = new Vector2(1366 * .25f - 130, 766 / 2 - 135);
                switch (popUpMenuSelection)
                {
                    case (int)PopUpChoices.Attack:
                        sb.Draw(BattleGUITexture, location, PopUpMenuAttackSource, Color.White);
                        break;
                    case (int)PopUpChoices.Item:
                        sb.Draw(BattleGUITexture, location, PopUpMenuItemSource, Color.White);
                        break;
                    case (int)PopUpChoices.Defend:
                        sb.Draw(BattleGUITexture, location, PopUpMenuDefendSource, Color.White);
                        break;
                    case (int)PopUpChoices.Interact:
                        sb.Draw(BattleGUITexture, location, PopUpMenuInteractSource, Color.White);
                        break;
                }
            }


            sb.End();
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
        }

        private static void AttackPopUpInfo(SpriteBatch sb, Vector2 location)
        {
            GameText attackHelp = new GameText("    Select a target using the,\n  movement keys left and right.\nSelect yourself to cast AoE abilities\n(if available).");
            sb.Draw(Game1.WhiteTex, new Rectangle(location.ToPoint() - new Point(0, 120), new Point(265, 120)), Color.DarkOrange * .6f);
            TextUtility.Draw(sb, attackHelp.getText(), testSF20, new Rectangle(location.ToPoint() - new Point(0, 120), new Point(265, 120)), TextUtility.OutLining.Center, Color.LightYellow, 1f, true, default(Matrix), Color.Black, false);
        }


        static public void PopUpNextChoice()
        {
            popUpMenuSelection++;
            if (popUpMenuSelection > Enum.GetNames(typeof(PopUpChoices)).Length - 1)
            {
                popUpMenuSelection = 0;
            }
        }

        static public void PopUpPreviousChoice()
        {
            popUpMenuSelection--;
            if (popUpMenuSelection < 0)
            {
                popUpMenuSelection = Enum.GetNames(typeof(PopUpChoices)).Length - 1;
            }
        }

        static public int returnCurrentPopUpMenuSelection()
        {
            return popUpMenuSelection;
        }

        static public void DrawVerticalPointerAbove(SpriteBatch sb, List<BaseCharacter> bcs)
        {
            Vector2 offset = new Vector2(-14, 55);
            foreach (var item in bcs)
            {
                sb.Draw(BattleGUITexture, item.position - offset, HoverPointerVerSource, Color.White);
            }
        }

        static Vector2 offsetOverTime = new Vector2(0, 16);
        static Vector2 deltaOffsetVA = new Vector2(0, 0);
        static int framesPassedVA = 0;
        static int framesUntillTop = (int)(2f / 2 * 60);
        static int framesStillTop = (int)(1f / 2 * 60);
        static bool bArrowIsAtTop = true;
        static bool bArrowGoesUp = false;
        private static void VericalArrowHoverLogic()
        {
            framesPassedVA++;
            if (bArrowIsAtTop)
            {
                if (framesPassedVA > framesStillTop)
                {
                    bArrowIsAtTop = false;
                    bArrowGoesUp = false;
                    framesPassedVA = 0;
                }
            }
            else
            {
                if (!bArrowGoesUp)
                {
                    deltaOffsetVA = offsetOverTime * (float)((float)framesPassedVA / (float)framesUntillTop);
                    if (framesPassedVA > framesUntillTop)
                    {
                        bArrowGoesUp = true;
                    }
                }
                else
                {
                    framesPassedVA += -2;
                    deltaOffsetVA = offsetOverTime * (float)((float)framesPassedVA / (float)framesUntillTop);
                    if (framesPassedVA < 0)
                    {
                        framesPassedVA = 0;
                        bArrowGoesUp = false;
                        bArrowIsAtTop = true;
                        deltaOffsetVA = new Vector2(0, 0);
                    }
                }
            }

        }

        static public void DrawVerticalPointerAbove(SpriteBatch sb, BaseCharacter bc)
        {
            Vector2 offset = new Vector2(-14, 55);

            sb.Draw(BattleGUITexture, bc.position - offset + deltaOffsetVA, HoverPointerVerSource, Color.White);

        }

        static public void DrawVerticalPointerAbove(SpriteBatch sb)
        {
            Vector2 offset = new Vector2(-14, 55);
            VericalArrowHoverLogic();
            if (selectedTarget != null)
            {
                if (!bArrowIsAtTop)
                {
                    sb.Draw(BattleGUITexture, selectedTarget.position - offset + (deltaOffsetVA * (1 + ((float)framesPassedVA / (float)framesUntillTop))), HoverPointerVerSource, Color.White, 0f, Vector2.Zero, new Vector2(1, 1f - 0.3f * ((float)framesPassedVA / (float)framesUntillTop)), SpriteEffects.None, 0);

                }
                else
                {
                    sb.Draw(BattleGUITexture, selectedTarget.position - offset + deltaOffsetVA, HoverPointerVerSource, Color.White, 0f, Vector2.Zero, new Vector2(1, 1f), SpriteEffects.None, 0);

                }
            }


        }

        static public void SelectNextCharacter()
        {
            if (selectableTargetsWithinRange.Count != 0)
            {
                selectTargetIndex++;
                if (selectTargetIndex > selectableTargetsWithinRange.Count - 1)
                {
                    selectTargetIndex = 0;
                }
                selectedTarget = selectableTargetsWithinRange[selectTargetIndex];
            }
        }

        static public void SelectPreviousCharacter()
        {
            if (selectableTargetsWithinRange.Count != 0)
            {
                selectTargetIndex--;
                if (selectTargetIndex < 0)
                {
                    selectTargetIndex = selectableTargetsWithinRange.Count - 1;
                }
                selectedTarget = selectableTargetsWithinRange[selectTargetIndex];
            }
        }

        internal static void DefendOption(BaseCharacter character, BaseCharacter selectedTarget)
        {
            BaseModifier defendMod = new BaseModifier();
            defendMod.abilityModifierLength = 1;
            defendMod.statModifier.currentPassiveStats[(int)STATChart.PASSIVESTATS.DEF] = 2;
            defendMod.statModifier.currentPassiveStats[(int)STATChart.PASSIVESTATS.INT] = 2;
            defendMod.statModifier.currentPassiveStats[(int)STATChart.PASSIVESTATS.AGI] = 2;
            defendMod.modifierName = "Defend";
            character.modifierList.Add(defendMod);

        }

        internal static void ItemsOption()
        {
            GameProcessor.bInGameMenu = true;
            GameMenuHandler.Start();
            KeyboardMouseUtility.bPressed = true;
            GameProcessor.EnableMenuStage();
        }

        static public bool ExecutableAbilitySelected()
        {
            try
            {
                BasicAbility test = gAbilityList[abilityIndex];
                return true;
            }
            catch
            {
                return false;
            }
        }

        static Rectangle overlayFlagSource = new Rectangle(0, 1235, 108, 348);
        static Rectangle overlaySlabUnselectedSource = new Rectangle(108, 1235, 92, 121);
        static Rectangle overlaySlabSelectedSource = new Rectangle(200, 1235, 92, 121);
        static Vector2 overlaySlabPortraitOffSet = new Vector2(9, 22);//74,74
        static List<Rectangle> playerOverlaySlabs = new List<Rectangle>();
        static Rectangle overlayFlagPosition = new Rectangle(0, 768 - 348, 108, 348);

        internal static void DrawMapOverlay(SpriteBatch spriteBatch)
        {
            playerOverlaySlabs.Clear();
            for (int i = 0; i < PlayerSaveData.heroParty.Count; i++)
            {
                playerOverlaySlabs.Add(new Rectangle(120 + i * 92 + 20 * i, 768 - 121, 92, 121));
            }

            spriteBatch.Draw(BattleGUITexture, overlayFlagPosition, overlayFlagSource, Color.White);
            int index = 0;
            foreach (var item in PlayerSaveData.heroParty)
            {

                spriteBatch.Draw(BattleGUITexture, playerOverlaySlabs[index], overlaySlabSelectedSource, Color.Gray);
                Rectangle r = new Rectangle(playerOverlaySlabs[index].X + (int)overlaySlabPortraitOffSet.X, playerOverlaySlabs[index].Y + (int)overlaySlabPortraitOffSet.Y, 74, 74);
                item.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(spriteBatch, r);

                if (item.IsAlive() && !EncounterInfo.encounterGroups.Find(group => group.bIsPlayerTurnSet).groupTurnSet.Find(ct => ct.character == item).bIsCompleted)
                {
                    spriteBatch.Draw(BattleGUITexture, playerOverlaySlabs[index], overlaySlabSelectedSource, Color.White);
                    r = new Rectangle(playerOverlaySlabs[index].X + (int)overlaySlabPortraitOffSet.X, playerOverlaySlabs[index].Y + (int)overlaySlabPortraitOffSet.Y, 74, 74);
                    item.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(spriteBatch, r);
                }
                else if (item.IsAlive())
                {
                    spriteBatch.Draw(BattleGUITexture, playerOverlaySlabs[index], overlaySlabSelectedSource, Color.Gray);
                    r = new Rectangle(playerOverlaySlabs[index].X + (int)overlaySlabPortraitOffSet.X, playerOverlaySlabs[index].Y + (int)overlaySlabPortraitOffSet.Y, 74, 74);
                    item.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(spriteBatch, r, 0.6f);
                }
                else if (!item.IsAlive())
                {
                    spriteBatch.Draw(BattleGUITexture, playerOverlaySlabs[index], overlaySlabSelectedSource, Color.DarkRed);
                    r = new Rectangle(playerOverlaySlabs[index].X + (int)overlaySlabPortraitOffSet.X, playerOverlaySlabs[index].Y + (int)overlaySlabPortraitOffSet.Y, 74, 74);
                    item.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(spriteBatch, r, 0.6f);
                }

                index++;
            }

            //foreach (var item in playerOverlaySlabs)
            //{
            //    spriteBatch.Draw(Game1.WhiteTex,item, Color.Red);
            //}

            index = 0;
            foreach (var item in PlayerSaveData.heroParty.FindAll(h => h.IsAlive()))
            {
                int playerIndex = PlayerSaveData.heroParty.IndexOf(item);
                int widthHP = widthHP = (int)(smallHealthIndicatorSource.Width * ((float)(healthOfAllHeroes[index])) / (float)(maxHealthOfAllHeroes[index]));
                int widthMP = widthMP = (int)(smallManaIndicatorSource.Width * ((float)(manaOfAllHeroes[index])) / (float)(maxManaOfAllHeroes[index]));

                int xPos = playerOverlaySlabs[playerIndex].X + 14;
                int yPos = playerOverlaySlabs[playerIndex].Y + 60;

                Rectangle HPdrawLocation = new Rectangle(xPos + smallHealthBarPosition.X, yPos + smallHealthBarPosition.Y, smallHealthBarPosition.Width, smallHealthBarPosition.Height);
                Rectangle MPdrawLocation = new Rectangle(xPos + smallManaBarPosition.X, yPos + smallManaBarPosition.Y, smallManaBarPosition.Width, smallManaBarPosition.Height);
                Rectangle HPIndicatordrawLocation = new Rectangle(HPdrawLocation.X + smallHealthIndicatorPosition.X, HPdrawLocation.Y + smallHealthIndicatorPosition.Y, widthHP, smallHealthIndicatorPosition.Height);
                Rectangle MPIndicatordrawLocation = new Rectangle(MPdrawLocation.X + smallManaIndicatorPosition.X, MPdrawLocation.Y + smallManaIndicatorPosition.Y, widthMP, smallManaIndicatorPosition.Height);
                Rectangle HPIndicatorSource = new Rectangle(smallHealthIndicatorSource.X, smallHealthIndicatorSource.Y, widthHP, smallHealthIndicatorSource.Height);
                Rectangle MPIndicatorSource = new Rectangle(smallManaIndicatorSource.X, smallManaIndicatorSource.Y, widthMP, smallManaIndicatorSource.Height);

                spriteBatch.Draw(smallBarsTex, HPdrawLocation, smallHealthBarSource, Color.White);
                spriteBatch.Draw(smallBarsTex, MPdrawLocation, smallManaBarSource, Color.White);
                spriteBatch.Draw(smallBarsTex, HPIndicatordrawLocation, HPIndicatorSource, Color.White);
                spriteBatch.Draw(smallBarsTex, MPIndicatordrawLocation, MPIndicatorSource, Color.White);

                index++;
            }

            battleSpeedButton.Draw(spriteBatch, Color.White);
            battleCameraSpeedButton.Draw(spriteBatch, Color.White);
            TextUtility.Draw(spriteBatch, "Speed x" + SettingsFile.speedMod, testSF20, battleSpeedButton.Position(), TextUtility.OutLining.Center, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);
            battleCameraSpeedButton.Draw(spriteBatch, Color.White);
            TextUtility.Draw(spriteBatch, "Camera x" + SettingsFile.speedModCamera, testSF20, battleCameraSpeedButton.Position(), TextUtility.OutLining.Center, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);

        }






        internal static bool bBGUISoloModifierRunning = false;
        internal static List<ActiveStatModifier> modifiersThatHurt = new List<ActiveStatModifier>();
        static ActiveStatModifier modifierCurrentlyBeingHandled = new ActiveStatModifier();
        static float timeBetweenModifierEffects = 1.0f * 60;
        static int modifierTimeFramesPassed = 0;
        internal static bool bSoloCompleted = false;
        internal static bool bSoloSkipTimer = false;
        internal static bool bHasModifier = false;
        static BaseModifier bm = null;
        static int currentModLine = 0;

        internal static void CharacterSoloStart(BaseCharacter bc, List<ActiveStatModifier> mth)
        {
            bm = null;
            bDrawFrames = false;
            GUIPopUps.Clear();
            gbc = bc;
            modifierTimeFramesPassed = 0;
            RName = bc.displayName;
            bBGUISoloModifierRunning = true;
            modifiersThatHurt = new List<ActiveStatModifier>(mth);
            bc.ResetAllBattleAnims();

            if (mth.Count == 0)
            {
                CharacterSoloStop();
            }
            else
            {
                modifierCurrentlyBeingHandled = modifiersThatHurt[0];
            }
            PlayerController.currentController = PlayerController.Controllers.SoloCombat;
        }

        internal static void CharacterSoloStart(BaseCharacter bc, BaseConsumable bi)
        {
            bHasModifier = bi.HasModifier();
            currentModLine = 0;
            bDrawFrames = false;
            GUIPopUps.Clear();
            gbc = bc;
            modifierTimeFramesPassed = 0;
            RName = bc.displayName;
            bBGUISoloModifierRunning = true;
            modifiersThatHurt = new List<ActiveStatModifier>();
            modifiersThatHurt.Add(bi.ConsumableActiveStatModifier);
            modifiersThatHurt[0].displayName = bi.itemName;
            bc.ResetAllBattleAnims();

            bm = bi.ConsumableModifier;


            if (modifiersThatHurt.Count == 0)
            {
                CharacterSoloStop();
            }
            else
            {
                modifierCurrentlyBeingHandled = modifiersThatHurt[0];
            }
            PlayerController.currentController = PlayerController.Controllers.SoloCombat;
        }

        internal static void CharacterSoloUpdate(GameTime gt)
        {
            if (modifiersThatHurt.Count > 0)
            {
                modifierTimeFramesPassed++;
                if (modifierTimeFramesPassed > timeBetweenModifierEffects || bSoloSkipTimer)
                {
                    bSoloSkipTimer = false;
                    modifierTimeFramesPassed = 0;
                    int xMin = 1366 / 2 - 200;
                    int xMax = 1366 / 2 + 200;
                    int yMin = 768 / 2 - 000;
                    int yMax = 768 / 2 + 000;


                    int index = 0;
                    foreach (var item in modifierCurrentlyBeingHandled.activeStatModifier)
                    {
                        if (item != 0)
                        {
                            Vector2 randomPosNearMiddle = new Vector2(GamePlayUtility.Randomize(xMin, xMax), GamePlayUtility.Randomize(yMin, yMax));

                            if (item > 0)
                            {
                                if ((int)STATChart.ACTIVESTATS.HP == index)
                                {
                                    GUIPopUps.Add(new BGUIPopUpText(testSF48, Enum.GetNames(typeof(STATChart.ACTIVESTATS))[index], Color.Green, Color.Black, 1, randomPosNearMiddle, 3.0f, 0f, deltaMovMod));
                                    GUIPopUps.Add(new BGUIPopUpText(testSF32, "+" + item.ToString(), Color.Green, Color.Black, 1, randomPosNearMiddle + new Vector2(0, 64), 3.0f, 1f, deltaMovMod));
                                }
                                else if ((int)STATChart.ACTIVESTATS.MANA == index)
                                {
                                    GUIPopUps.Add(new BGUIPopUpText(testSF48, Enum.GetNames(typeof(STATChart.ACTIVESTATS))[index], Color.Blue, Color.Black, 1, randomPosNearMiddle, 3.0f, 0f, deltaMovMod));
                                    GUIPopUps.Add(new BGUIPopUpText(testSF32, "+" + item.ToString(), Color.Blue, Color.Black, 1, randomPosNearMiddle + new Vector2(0, 64), 3.0f, 1f, deltaMovMod));
                                }
                            }
                            else
                            {
                                GUIPopUps.Add(new BGUIPopUpText(testSF48, Enum.GetNames(typeof(STATChart.ACTIVESTATS))[index], Color.Red, Color.Black, 1, randomPosNearMiddle, 3.0f, 0f, deltaMovMod));
                                GUIPopUps.Add(new BGUIPopUpText(testSF32, item.ToString(), Color.Red, Color.Black, 1, randomPosNearMiddle + new Vector2(0, 64), 3.0f, 1f, deltaMovMod));
                            }



                        }
                        index++;
                    }

                    gbc.ChangeBattleAnimation((int)BaseCharacter.CharacterBattleAnimations.Idle);
                    modifiersThatHurt.Remove(modifierCurrentlyBeingHandled);
                    if (modifiersThatHurt.Count > 0)
                    {
                        modifierCurrentlyBeingHandled = modifiersThatHurt[0];
                    }
                }
            }
            else if (modifiersThatHurt.Count == 0 && bm != null && currentModLine != (Enum.GetNames(typeof(STATChart.PASSIVESTATS)).Length + Enum.GetNames(typeof(STATChart.SPECIALSTATS)).Length))
            {
                modifierTimeFramesPassed++;
                if (modifierTimeFramesPassed > timeBetweenModifierEffects)
                {
                    bSoloSkipTimer = false;
                    modifierTimeFramesPassed = 0;
                    int xMin = 1366 / 2 - 200;
                    int xMax = 1366 / 2 + 200;
                    int yMin = 768 / 2 + 000;
                    int yMax = 768 / 2 + 000;

                    int value = 0;
                    int finalIndex = 0;
                    bool inPassiveChart = true;

                    while (value == 0 && currentModLine != (Enum.GetNames(typeof(STATChart.PASSIVESTATS)).Length + Enum.GetNames(typeof(STATChart.SPECIALSTATS)).Length))
                    {
                        int index = currentModLine;
                        if (currentModLine < Enum.GetNames(typeof(STATChart.PASSIVESTATS)).Length)
                        {
                            index = currentModLine;
                            value = bm.statModifier.currentPassiveStats[index];
                            finalIndex = index;
                        }
                        else
                        {
                            index = currentModLine - Enum.GetNames(typeof(STATChart.PASSIVESTATS)).Length;
                            value = bm.statModifier.currentSpecialStats[index];
                            inPassiveChart = false;
                        }

                        currentModLine++;
                    }

                    if (value != 0)
                    {
                        Vector2 randomPosNearMiddle = new Vector2(GamePlayUtility.Randomize(xMin, xMax), GamePlayUtility.Randomize(yMin, yMax));

                        if (inPassiveChart)
                        {
                            GUIPopUps.Add(new BGUIPopUpText(testSF48, Enum.GetNames(typeof(STATChart.PASSIVESTATSNames))[finalIndex], Color.Gold, Color.White, 1, randomPosNearMiddle, 3.0f / 2, 1f / 2, deltaMovMod));
                            if (value > 0)
                            {
                                GUIPopUps.Add(new BGUIPopUpText(testSF32, "+" + value.ToString(), Color.DarkGreen, Color.Black, 1, randomPosNearMiddle + new Vector2(0, 64), 3.0f / 2, 1f / 2, deltaMovMod));
                            }
                            else
                            {
                                GUIPopUps.Add(new BGUIPopUpText(testSF32, value.ToString(), Color.Red, Color.Black, 1, randomPosNearMiddle + new Vector2(0, 64), 3.0f / 2, 1f / 2, deltaMovMod));
                            }

                        }
                        else
                        {
                            GUIPopUps.Add(new BGUIPopUpText(testSF48, Enum.GetNames(typeof(STATChart.SPECIALSTATSNames))[finalIndex], Color.Gold, Color.White, 1, randomPosNearMiddle, 3.0f / 2, 1f / 2, deltaMovMod));
                            if (value > 0)
                            {
                                GUIPopUps.Add(new BGUIPopUpText(testSF32, "+" + value.ToString(), Color.DarkGreen, Color.Black, 1, randomPosNearMiddle + new Vector2(0, 64), 3.0f / 2, 1f / 2, deltaMovMod));
                            }
                            else
                            {
                                GUIPopUps.Add(new BGUIPopUpText(testSF32, value.ToString(), Color.Red, Color.Black, 1, randomPosNearMiddle + new Vector2(0, 64), 3.0f / 2, 1f / 2, deltaMovMod));
                            }
                        }
                    }

                    if (bHasModifier && currentModLine == (Enum.GetNames(typeof(STATChart.PASSIVESTATS)).Length + Enum.GetNames(typeof(STATChart.SPECIALSTATS)).Length))
                    {
                        Vector2 randomPosNearMiddle = new Vector2(GamePlayUtility.Randomize(xMin, xMax), GamePlayUtility.Randomize(yMin, yMax));
                        if (bm.abilityModifierLength == 1)
                        {
                            GUIPopUps.Add(new BGUIPopUpText(testSF48, "Lasts " + bm.abilityModifierLength + " round!", Color.Gold, Color.White, 1, randomPosNearMiddle, 3.0f / 2, 1f / 2, deltaMovMod));
                        }
                        else
                        {
                            GUIPopUps.Add(new BGUIPopUpText(testSF48, "Lasts " + bm.abilityModifierLength + " rounds!", Color.Gold, Color.White, 1, randomPosNearMiddle, 3.0f / 2, 1f / 2, deltaMovMod));
                        }
                    }
                }
            }
            else if (bm == null || (bm != null && currentModLine == (Enum.GetNames(typeof(STATChart.PASSIVESTATS)).Length + Enum.GetNames(typeof(STATChart.SPECIALSTATS)).Length)) || bSoloSkipTimer)
            {
                bSoloCompleted = true;
            }

            if (gbc.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Hurt && gbc.currentBattleAnimation().bAnimationFinished)
            {
                gbc.ChangeBattleAnimation((int)BaseCharacter.CharacterBattleAnimations.Idle);
            }

            gbc.charBattleAnimations[gbc.animationBattleIndex].BAnimUpdate(gt, gbc);

            foreach (var item in GUIPopUps)
            {
                item.Update(gt);
                Console.WriteLine(item.fadePer);
            }
            GUIPopUps.RemoveAll(gp => gp.Remove());
        }

        internal static void CharacterSoloDraw(SpriteBatch sb)
        {
            #region 2nd Layer
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(stageLayer);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            sb.Draw(BattleGUIBG, Isometric, IsometricSource, Color.White);
            CharacterSoloBattleAnimation(sb);
            sb.End();
            #endregion

            #region Draw BG
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(BGLayer);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            sb.Draw(BattleGUIBG, new Rectangle(0, 0, 1366, 768), new Rectangle(0, 0, 1366, 768), Color.White);
            sb.End();
            #endregion

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(UILayer);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            #region Draw Upper Part Graphics
            sb.Draw(BattleGUITexture, LNameBox, LNameBoxSource, Color.White);
            sb.Draw(BattleGUITexture, LHBox, HBoxSource, Color.White);
            sb.Draw(BattleGUITexture, LMBox, MBoxSource, Color.White);
            sb.Draw(BattleGUITexture, LHGreenBar.Location.ToVector2(), LHGreenBarActualSource, LHBarColor);
            sb.Draw(BattleGUITexture, LMBlueBar.Location.ToVector2(), LMBlueBarActualSource, LMBarColor);
            foreach (var item in LShieldIcons)
            {
                sb.Draw(BattleGUITexture, item, ShieldIconsSource, Color.White);
            }
            #endregion
            #region Draw Upper Text
            var lLoc = new Vector2(Math.Abs(testSF32.MeasureString(GName).X / 2 - 185), 28); //1335,28
            {
                Vector2 lMeasure = testSF32.MeasureString(GName);
                int nameBoxOffset = 20;
                Rectangle lNameTextBox = new Rectangle(LNameBox.X + nameBoxOffset, LNameBox.Y + nameBoxOffset, LNameBox.Width - nameBoxOffset * 2, LNameBox.Height - nameBoxOffset * 2);
                Vector2 scale = new Vector2(lMeasure.X / (float)lNameTextBox.Width, lMeasure.Y / (float)lNameTextBox.Height);
                float neededScale = 1.0f;
                if (scale.X >= scale.Y)
                {
                    neededScale = scale.X;
                }
                else
                {
                    neededScale = scale.Y;
                }

                lLoc = new Vector2(Math.Abs(testSF32.MeasureString(GName).X / 2 - 185), 28); //1335,28
                if (scale.X < 1)
                {
                    scale.X = 1;
                    sb.DrawString(testSF32, GName, new Vector2(lLoc.X, LNameBox.Y + nameBoxOffset), Color.White, 0f, Vector2.Zero, new Vector2(1f / scale.X, 1f / scale.Y), SpriteEffects.None, 0); //32,28

                }
                else
                {
                    sb.DrawString(testSF32, GName, new Vector2(LNameBox.X + nameBoxOffset, LNameBox.Y + nameBoxOffset), Color.White, 0f, Vector2.Zero, new Vector2(1f / scale.X, 1f / scale.Y), SpriteEffects.None, 0); //32,28

                }
            }

            sb.DrawString(ActiveBarsFont, LHBarString, LHBarText.Location.ToVector2(), Color.Black);
            if (bGDrawMana)
            {
                sb.DrawString(ActiveBarsFont, LMBarString, LMBarText.Location.ToVector2(), Color.Black);
            }
            if (bGDrawEnergy)
            {
                sb.DrawString(ActiveBarsFont, LEBarString, LEBarText.Location.ToVector2(), Color.Black);
            }
            //TODO SHIELD

            //TODO SHIELD


            #endregion
            #region Draw Lower Part Graphics
            if (true)
            {
                sb.Draw(BattleGUITexture, LowerHalfScreen, LowerHalfScreenSource, Color.White);
            }

            try
            {
                sb.Draw(BattleGUITexture, LAbilityFrame, AbilityFrameSource, Color.White);
                sb.Draw(BattleGUITexture, LAbilityName, AbilityNameSource, Color.White);


                if (true)
                {
                    sb.Draw(BattleGUITexture, RAbilityFrame, AbilityFrameSource, Color.White);
                    sb.Draw(BattleGUITexture, RAbilityName, AbilityNameSource, Color.White);
                }
                if (gbc.weapon != null)
                {
                    gbc.weapon.itemTexAndAnimation.Draw(sb, LAbilityFrame);
                }
            }
            catch
            {

            }
            #endregion
            #region Draw Lower Text

            try
            {
                {
                    Vector2 lMeasure = testSF32.MeasureString(LWeaponString);
                    int nameBoxOffset = 16;
                    Rectangle lNameTextBox = new Rectangle(LWeaponText.X + nameBoxOffset, LWeaponText.Y + nameBoxOffset, LWeaponText.Width - nameBoxOffset * 2, LWeaponText.Height - nameBoxOffset * 2);
                    Vector2 scale = new Vector2(lMeasure.X / (float)lNameTextBox.Width, lMeasure.Y / (float)lNameTextBox.Height);
                    float neededScale = 1.0f;
                    if (scale.X >= scale.Y)
                    {
                        neededScale = scale.X;
                    }
                    else
                    {
                        neededScale = scale.Y;
                    }

                    if (scale.X < 1)
                    {
                        scale.X = 1;

                        Vector2 rLoc2 = new Vector2(Math.Abs(LWeaponText.Location.X + 150 - testSF32.MeasureString(LWeaponString).X / 2), LWeaponText.Location.ToVector2().Y); //1335,28
                        if (rbc != gbc)
                        {
                            sb.DrawString(testSF32, LWeaponString, new Vector2(rLoc2.X, LWeaponText.Y + nameBoxOffset), Color.White, 0f, Vector2.Zero, new Vector2(1f / scale.X, 1f / scale.Y), SpriteEffects.None, 0); //32,28
                        }
                    }
                    else
                    {
                        sb.DrawString(testSF32, LWeaponString, new Vector2(LWeaponText.X + nameBoxOffset, LWeaponText.Y + nameBoxOffset), Color.White, 0f, Vector2.Zero, new Vector2(1f / scale.X, 1f / scale.Y), SpriteEffects.None, 0); //32,28

                    }

                }
            }
            catch
            {

            }

            #endregion

            // sb.DrawString(testSF25, LHitChanceString + " " + LHitChanceNum, new Vector2(30, 610), Color.Gold); //30,610
            //if (EncounterInfo.currentTurn().bIsPlayerTurnSet)
            //{
            //    sb.DrawString(testSF20, "Total AP: " + gbc.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP], new Vector2(250, 717), Color.Green);
            //}

            //sb.DrawString(testSF20, LDMGString + " " + LDMGNum, new Vector2(450, 717), Color.Gold); //450,717
            //sb.DrawString(testSF25, LCRITChanceString + " " + LCRITChanceNum, new Vector2(30, 690), Color.Gold); //30,690

            try
            {
                sb.DrawString(testSF32, modifierCurrentlyBeingHandled.displayName, new Vector2(250, 670), Color.LightGoldenrodYellow);
            }
            catch
            {
            }

            sb.End();

            sb.GraphicsDevice.SetRenderTarget(finalBattleScreen);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            float zoomAmount = -(1.0f - zoom);
            float parallaxDifference = 0.6f;
            Vector2 posBG = new Vector2(1366 / 2, 768 / 2) * zoomAmount;

            float parallaxZoom = (1.0f + (parallaxDifference * zoomAmount * 2) * 1.5f);
            Vector2 scaledCenter = new Vector2(1366 / 2, 768 / 2) * (parallaxZoom - 1.0f);

            sb.Draw(BGLayer, -posBG, BGLayer.Bounds, Color.White, 0f, Vector2.Zero, zoom, SpriteEffects.None, 0);

            sb.Draw(stageLayer, -scaledCenter, stageLayer.Bounds, Color.White, 0f, Vector2.Zero, parallaxZoom, SpriteEffects.None, 0);
            sb.Draw(UILayer, BGLayer.Bounds, Color.White);
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            GameProcessor.UIRenders.Clear();
            GameProcessor.UIRenders.Add(finalBattleScreen);

        }

        static void CharacterSoloBattleAnimation(SpriteBatch sb)
        {
            #region Draw and Update Battle Animations
            try
            {
                Vector2 rLoc = gbc.battleAnimLocs[gbc.animationBattleIndex] + new Vector2(gbc.charBattleAnimations[gbc.animationBattleIndex].animationFrames[0].Width * gbc.battleAnimScale / 2, 0);

                gbc.charBattleAnimations[gbc.animationBattleIndex].BattleAnimationDraw(sb, rLoc, gbc.battleAnimScale, true);
            }
            catch (Exception)
            {

                throw;
            }
            #endregion
        }

        internal static void CharacterSoloStop()
        {
            bSoloCompleted = false;
            bBGUISoloModifierRunning = false;
            GUIPopUps.Clear();

            if (CombatProcessor.bIsRunning)
            {
                EncounterInfo.currentTurn().FinalizeCharacterRound();
                GameProcessor.DisableMenuStage();
                PlayerController.currentController = PlayerController.Controllers.Combat;
            }
            else
            {
                PlayerController.currentController = PlayerController.Controllers.Menu;
            }

        }

        internal static void ShowAttackNumbers()
        {
            Vector2 startPos = Vector2.Zero;
            if (jumpPosition != Vector2.Zero)
            {
                startPos = jumpPosition + new Vector2(rbc.charBattleAnimations[0].animationFrames[0].Width * rbc.battleAnimScale, 0) + new Vector2(popUpTextLocationHMod, 150);
            }
            else
            {
                startPos = gbc.battleAnimLocs[gbc.animationBattleIndex] + new Vector2(rbc.charBattleAnimations[0].animationFrames[0].Width * rbc.battleAnimScale, 0) + new Vector2(popUpTextLocationHMod, 150);
            }

            if (damageDoneByDealer > 0)
            {
                GUIPopUps.Add(new BGUIPopUpText(testSF48, damageDoneByDealer.ToString(), Color.Red, Color.Orange, 2, startPos, modTextTime, modTextFadeTime, deltaMovMod));
            }
            else if (damageDoneByDealer < 0)
            {
                GUIPopUps.Add(new BGUIPopUpText(testSF48, (-damageDoneByDealer).ToString(), Color.Green, Color.LightCoral, 2, startPos, modTextTime, modTextFadeTime, deltaMovMod));
            }


            bShowAttackNumbers = false;
        }

        internal static void ShowCounterAttackNumbers()
        {
            Vector2 startPos = Vector2.Zero;
            if (jumpPosition != Vector2.Zero)
            {
                startPos = jumpPosition + new Vector2(rbc.charBattleAnimations[0].animationFrames[0].Width * rbc.battleAnimScale, 0) + new Vector2(popUpTextLocationHMod - 200, 150);
            }
            else
            {
                startPos = gbc.battleAnimLocs[gbc.animationBattleIndex] + new Vector2(rbc.charBattleAnimations[0].animationFrames[0].Width * rbc.battleAnimScale, 0) + new Vector2(popUpTextLocationHMod - 200, 150);
            }
            startPos = gbc.battleAnimLocs[gbc.animationBattleIndex] + new Vector2(300, 100);// + new Vector2(gbc.charBattleAnimations[0].animationFrames[0].Width * gbc.battleAnimScale, 0);

            // if (rDMGCounter != 0)
            {
                GUIPopUps.Add(new BGUIPopUpText(testSF48, rDMGCounter.ToString(), Color.Red, Color.White, 2, startPos, modTextTime, modTextFadeTime, deltaMovMod));
            }


            bShowCounterAttackNumbers = false;
        }
    }

    public class BGUIPopUp
    {
        public enum PopUpType { Text = 0, Texture }
        public PopUpType type = PopUpType.Text;

        internal float fadePer = 1.0f;
        int frameTimer = 60;
        int framesPassed = 0;

        int secondsLiftTime = 3;
        int frameTimeLife = 60 * 3;
        bool bMustFadeOut = false;
        int frameTimeLifePassed = 0;

        internal Vector2 elementPositon = Vector2.Zero;
        Vector2 velocity = Vector2.Zero;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seconds">Screen time element</param>
        /// <param name="secondsFadeInOut"></param>
        /// <param name="deltaMovement"></param>
        public BGUIPopUp(Vector2 beginPos = default(Vector2), float seconds = 3f, float secondsFadeInOut = 1f, Vector2 deltaMovement = default(Vector2))
        {
            frameTimeLife = (int)(60 * seconds);
            frameTimer = (int)(60 * secondsFadeInOut);
            if (deltaMovement != default(Vector2))
            {
                velocity = deltaMovement / frameTimeLife;
            }

            if (beginPos != default(Vector2))
            {
                elementPositon = beginPos;
            }
        }

        public virtual void Update(GameTime gt)
        {
            if (!bMustFadeOut && framesPassed < frameTimer)
            {
                framesPassed++;
                fadePer = (float)framesPassed / (float)frameTimer;
            }

            frameTimeLifePassed++;

            if (!bMustFadeOut && frameTimeLife - frameTimeLifePassed < frameTimer)
            {
                bMustFadeOut = true;
            }

            if (bMustFadeOut && framesPassed > 0)
            {
                framesPassed--;
                fadePer = (float)framesPassed / (float)frameTimer;
            }

            elementPositon += velocity;
        }

        public bool Remove()
        {
            if (frameTimeLifePassed > frameTimeLife)
            {
                return true;
            }

            return false;
        }
    }

    public class BGUIPopUpText : BGUIPopUp
    {
        SpriteFont sf;
        String text = "";
        Color textColour = Color.Red;
        Color textLiningColour = Color.Black;
        int pixelLining = 1;

        public BGUIPopUpText(SpriteFont sf, String text, Color textColour, Color textLiningColour, int pixelLining
        , Vector2 beginPos = default(Vector2), float seconds = 3f, float secondsFadeInOut = 1f, Vector2 deltaMovement = default(Vector2))
         : base(beginPos, seconds, secondsFadeInOut, deltaMovement)
        {
            type = PopUpType.Text;

            this.sf = sf;
            this.text = text;
            this.textColour = textColour;
            this.textLiningColour = textLiningColour;
            this.pixelLining = pixelLining;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            NiceText.AddTextOrder(sf, text, elementPositon, textColour * fadePer, textLiningColour * fadePer, pixelLining);
        }
    }

    public class BGUIPopUpTexture : BGUIPopUp
    {
        Texture2D tex;
        Rectangle gameSize;

        public BGUIPopUpTexture(Texture2D tex, Rectangle gameSize
        , Vector2 beginPos = default(Vector2), float seconds = 3f, float secondsFadeInOut = 1f, Vector2 deltaMovement = default(Vector2))
         : base(beginPos, seconds, secondsFadeInOut, deltaMovement)
        {
            type = PopUpType.Texture;
            this.tex = tex;
            this.gameSize = gameSize;

        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }
    }

    internal class BattleAnimationInfo
    {
        internal enum AnimationTimingType { PlayAtStart, PlayAtClimax, PlayAtEnd }


        static BattleAnimationInfo currentAnimation = null;

        static BaseCharacter GBC;
        static BaseCharacter RBC;

        static Vector2 positionAnimationGBC = Vector2.Zero;
        static Vector2 positionAnimationRBC = Vector2.Zero;

        static internal List<BattleAnimationInfo> completeAnimationInfo = new List<BattleAnimationInfo>();

        static bool bHasCounterAttack = false;

        Vector2 destinationGBC = Vector2.Zero;
        Vector2 destinationRBC = Vector2.Zero;

        int deltaGBC = 0;
        int deltaRBC = 0;

        bool bFinishedCommand = false;

        BaseCharacter.CharacterBattleAnimations GiverAnimation = BaseCharacter.CharacterBattleAnimations.Idle;
        BaseCharacter.CharacterBattleAnimations ReceiverAnimation = BaseCharacter.CharacterBattleAnimations.Idle;

        AnimationTimingType giverAnimationTimingType = AnimationTimingType.PlayAtStart;
        AnimationTimingType receiverAnimationTimingType = AnimationTimingType.PlayAtEnd;

        bool SpecialCaseWaitForPAToFinishGBC = false;

        static internal void SetHasCounterAttack(bool HasCounter)
        {
            bHasCounterAttack = HasCounter;
        }

        static internal Vector2 GBCPosition()
        {
            return positionAnimationGBC;
        }

        static internal Vector2 RBCPosition()
        {
            return positionAnimationGBC;
        }

        static internal void Initialize(BaseCharacter g, BaseCharacter c)
        {
            completeAnimationInfo.Clear();
            GBC = g;
            RBC = c;
            currentAnimation = null;
            bHasCounterAttack = false;
            positionAnimationGBC = g.battleAnimLocs[0];
            positionAnimationRBC = c.battleAnimLocs[0];
        }

        internal BattleAnimationInfo(BaseCharacter.CharacterBattleAnimations GA, BaseCharacter.CharacterBattleAnimations RA, AnimationTimingType GAT, AnimationTimingType RAT, Vector2 destinationGBC, Vector2 destinationRBC)
        {
            GiverAnimation = GA;
            ReceiverAnimation = RA;
            giverAnimationTimingType = GAT;
            receiverAnimationTimingType = RAT;
            this.destinationGBC = destinationGBC;
            this.destinationRBC = destinationRBC;
            Vector2 previousPosGBC = positionAnimationGBC;
            Vector2 previousPosRBC = positionAnimationRBC;
            if (completeAnimationInfo.Count >= 1)
            {
                previousPosGBC = completeAnimationInfo.Last().destinationGBC;
                previousPosRBC = completeAnimationInfo.Last().destinationRBC;
            }
            deltaGBC = (int)(previousPosGBC.X - destinationGBC.X);
            deltaRBC = (int)(previousPosRBC.X - destinationRBC.X);
        }

        static void InitialCheck()
        {
            if (currentAnimation != null)
            {
                currentAnimation.bFinishedCommand = IsDone();
                if (currentAnimation.bFinishedCommand)
                {
                    completeAnimationInfo.Remove(currentAnimation);

                    if (completeAnimationInfo.Count > 0)
                    {
                        currentAnimation = completeAnimationInfo[0];
                    }
                    else
                    {
                        currentAnimation = null;
                        if (BattleGUI.bHandleAreaAttack)
                        {
                            EncounterInfo.currentTurn().FinalizeCharacterRound();
                            BattleGUI.End();
                        }
                    }
                }
            }
        }

        static bool IsDone()
        {
            if (currentAnimation.bFinishedCommand)
            {
                return true;
            }

            if ((GBC.animationBattleIndex == (int)currentAnimation.GiverAnimation || currentAnimation.SpecialCaseWaitForPAToFinishGBC) && (GBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Idle || GBC.currentBattleAnimation().bAnimationFinished) && currentAnimation.deltaGBC == 0)
            {

                int destinationAnimation = (int)currentAnimation.ReceiverAnimation;
                if ((GBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack || currentAnimation.SpecialCaseWaitForPAToFinishGBC) && bHasCounterAttack)
                {
                    destinationAnimation = (int)BaseCharacter.CharacterBattleAnimations.Attack;
                    var PA = BattleGUI.castAbilityGBC.PAanim;
                    if (PA != null && PA != default(ParticleAnimation) && PA.bAnimationFinished)
                    {
                        return true;
                    }
                    else if (PA == null && RBC.currentBattleAnimation().bAnimationFinished)
                    {
                        return true;
                    }
                }
                else if (!bHasCounterAttack && (GBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack || currentAnimation.SpecialCaseWaitForPAToFinishGBC))
                {
                    return true;
                }

                if (RBC.animationBattleIndex == destinationAnimation && (RBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Idle || RBC.currentBattleAnimation().bAnimationFinished) && currentAnimation.deltaRBC == 0)
                {
                    return true;
                }
            }

            if (GBC.animationBattleIndex == 0 && RBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Death && RBC.currentBattleAnimation().bAnimationFinished && completeAnimationInfo.Count > 1)
            {
                if (!completeAnimationInfo[1].bFinishedCommand)
                {
                    if (completeAnimationInfo[1].GiverAnimation == (BaseCharacter.CharacterBattleAnimations)GBC.animationBattleIndex && completeAnimationInfo[1].ReceiverAnimation == (BaseCharacter.CharacterBattleAnimations)RBC.animationBattleIndex)
                    {
                        completeAnimationInfo[0].bFinishedCommand = true;
                        completeAnimationInfo[1].bFinishedCommand = true;
                        currentAnimation.bFinishedCommand = true;
                        return true;
                    }
                }
            }

            var tempAnim = GBC.currentBattleAnimation();
            if (RBC != null && RBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Death_State && GBC.currentBattleAnimation().bAnimationFinished)
            {
                return true;
            }

            if (RBC != null && !RBC.IsAlive() && currentAnimation.ReceiverAnimation == BaseCharacter.CharacterBattleAnimations.Death)
            {
                RBC.ChangeBattleAnimation((int)BaseCharacter.CharacterBattleAnimations.Death);
            }
            return false;
        }

        static internal void Start(BattleAnimationInfo bai)
        {
            currentAnimation = bai;
        }

        static internal void Update(GameTime gt)
        {
            if (currentAnimation != null && GBC != null)
            {
                if (currentAnimation.GiverAnimation == BaseCharacter.CharacterBattleAnimations.Attack && !BattleGUI.bMissedAttack && BattleGUI.bShowAttackNumbers)
                {
                    BattleGUI.ShowAttackNumbers();
                }
            }
            if (RBC != null)
            {
                if ((currentAnimation.ReceiverAnimation == BaseCharacter.CharacterBattleAnimations.Attack) && BattleGUI.bShowCounterAttackNumbers)
                {
                    BattleGUI.ShowCounterAttackNumbers();
                }
            }

            if (GBC == RBC)
            {
                if (GBC.currentBattleAnimation().bAnimationFinished)
                {
                    completeAnimationInfo.Remove(currentAnimation);

                    if (completeAnimationInfo.Count > 0)
                    {
                        currentAnimation = completeAnimationInfo[0];
                    }
                    else
                    {
                        currentAnimation = null;
                    }
                }

                if (currentAnimation != null)
                {
                    if (GBC.animationBattleIndex != (int)currentAnimation.GiverAnimation)
                    {
                        GBC.ChangeBattleAnimation((int)currentAnimation.GiverAnimation);
                    }
                }

                if (BattleGUI.bHandleAreaAttack)
                {
                    InitialCheck();
                }

            }
            else
            {
                InitialCheck();

                if (BattleGUI.castAbilityGBC.PAanim != null && BattleGUI.castAbilityGBC.PAanim != default(ParticleAnimation))
                {
                    BattleGUI.castAbilityGBC.PAanim.AssignRefCharSafe(RBC);
                }


                if (currentAnimation != null)
                {
                    if (GBC.animationBattleIndex != (int)currentAnimation.GiverAnimation && CanChangeGBCAnim() && !currentAnimation.SpecialCaseWaitForPAToFinishGBC)
                    {
                        GBC.ChangeBattleAnimation(currentAnimation.GiverAnimation);
                    }
                    if (RBC.animationBattleIndex != (int)currentAnimation.ReceiverAnimation && CanChangeRBCAnim())
                    {
                        //RBC.ChangeBattleAnimation(currentAnimation.ReceiverAnimation);

                        if (GBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack || currentAnimation.SpecialCaseWaitForPAToFinishGBC)
                        {
                            var PA = BattleGUI.castAbilityGBC.PAanim;
                            if (PA != null && PA != default(ParticleAnimation) && !PA.bAnimationFinished)
                            {
                                if (bHasCounterAttack)
                                {
                                    PA.GenerateLogic(RBC, currentAnimation.ReceiverAnimation, BaseCharacter.CharacterBattleAnimations.Attack);
                                }
                                else
                                {
                                    PA.GenerateLogic(RBC, currentAnimation.ReceiverAnimation, completeAnimationInfo[1].ReceiverAnimation);
                                }

                            }
                            else if (PA == null)
                            {
                                RBC.ChangeBattleAnimation(currentAnimation.ReceiverAnimation);
                                RBC.currentBattleAnimation().SimpleReset();
                            }


                        }
                        else
                        {
                            RBC.ChangeBattleAnimation(currentAnimation.ReceiverAnimation);
                        }
                    }



                    if (GBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack && GBC.currentBattleAnimation().bAnimationFinished)
                    {
                        var PA = BattleGUI.castAbilityGBC.PAanim;
                        if (PA != null && PA != default(ParticleAnimation) && !PA.bAnimationFinished)
                        {
                            GBC.ChangeBattleAnimation(BaseCharacter.CharacterBattleAnimations.Idle);
                            currentAnimation.SpecialCaseWaitForPAToFinishGBC = true;
                        }
                    }

                    if (GBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack || currentAnimation.SpecialCaseWaitForPAToFinishGBC)
                    {
                        var PA = BattleGUI.castAbilityGBC.PAanim;
                        if (PA != null && PA != default(ParticleAnimation))
                        {
                            PA.UpdateAnimationForItems(gt);
                        }

                    }

                    if (currentAnimation.deltaGBC != 0)
                    {
                        if (currentAnimation.destinationGBC != positionAnimationGBC)
                        {
                            MoveAnimation(GBC.jumpSpeed, ref currentAnimation.deltaGBC, ref currentAnimation.destinationGBC, ref positionAnimationGBC);
                        }
                        else
                        {
                            currentAnimation.deltaGBC = 0;
                        }
                    }

                    if (currentAnimation.deltaRBC != 0)
                    {
                        if (currentAnimation.destinationRBC != positionAnimationRBC)
                        {
                            MoveAnimation(RBC.jumpSpeed, ref currentAnimation.deltaRBC, ref currentAnimation.destinationRBC, ref positionAnimationRBC);
                        }
                        else
                        {
                            currentAnimation.deltaGBC = 0;
                        }
                    }
                }
            }

            if (GBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack)
            {
                if (GBC.currentBattleAnimation().OnClimax() || GBC.currentBattleAnimation().HasPassedClimax())
                {
                    BattleGUI.castAbilityGBC.playSoundEffect();
                }
            }

            if (GBC != RBC && RBC.animationBattleIndex == (int)BaseCharacter.CharacterBattleAnimations.Attack)
            {
                if (RBC.currentBattleAnimation().OnClimax() || RBC.currentBattleAnimation().HasPassedClimax())
                {
                    BattleGUI.PlayCounterSound();
                }
            }

        }

        private static void MoveAnimation(int jumpSpeed, ref int delta, ref Vector2 destination, ref Vector2 positionAnimation)
        {
            bool movingRight = delta < 0 ? true : false;
            int currentX = (int)(positionAnimation.X);
            if (movingRight)
            {
                if (positionAnimation.X + jumpSpeed < destination.X)
                {
                    positionAnimation.X += jumpSpeed;
                }
                else
                {
                    positionAnimation = destination;
                }
            }
            else
            {
                if (positionAnimation.X - jumpSpeed > destination.X)
                {
                    positionAnimation.X -= jumpSpeed;
                }
                else
                {
                    positionAnimation = destination;
                }
            }

            delta = (int)(positionAnimation.X - destination.X);
        }

        private static bool CanChangeGBCAnim()
        {

            switch (currentAnimation.giverAnimationTimingType)
            {
                case AnimationTimingType.PlayAtStart:
                    if (RBC == GBC && GBC.currentBattleAnimation().bAnimationFinished)
                    {
                        return true;
                    }
                    if (RBC.animationBattleIndex == (int)currentAnimation.ReceiverAnimation && RBC.currentBattleAnimation().bAnimationFinished)
                    {
                        return true;
                    }
                    return true;
                case AnimationTimingType.PlayAtClimax:
                    if (RBC == GBC && GBC.currentBattleAnimation().bAnimationFinished)
                    {
                        return true;
                    }
                    if (RBC.animationBattleIndex == (int)currentAnimation.ReceiverAnimation && RBC.currentBattleAnimation().HasPassedClimax())
                    {
                        return true;
                    }
                    break;
                case AnimationTimingType.PlayAtEnd:
                    if (RBC == GBC && GBC.currentBattleAnimation().bAnimationFinished)
                    {
                        return true;
                    }
                    if (RBC.animationBattleIndex == (int)currentAnimation.ReceiverAnimation && RBC.currentBattleAnimation().bAnimationFinished)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        private static bool CanChangeRBCAnim()
        {
            if (GBC == RBC)
            {
                return false;
            }
            switch (currentAnimation.receiverAnimationTimingType)
            {
                case AnimationTimingType.PlayAtStart:
                    if (GBC.animationBattleIndex == (int)currentAnimation.GiverAnimation && GBC.currentBattleAnimation().bAnimationFinished)
                    {
                        return true;
                    }
                    return true;
                case AnimationTimingType.PlayAtClimax:
                    if (GBC.animationBattleIndex == (int)currentAnimation.GiverAnimation && GBC.currentBattleAnimation().HasPassedClimax())
                    {
                        return true;
                    }
                    break;
                case AnimationTimingType.PlayAtEnd:
                    if (GBC.animationBattleIndex == (int)currentAnimation.GiverAnimation && GBC.currentBattleAnimation().bAnimationFinished)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
    }
}
