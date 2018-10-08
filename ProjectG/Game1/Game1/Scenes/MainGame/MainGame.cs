using TBAGW.Scenes;
using TBAGW.Utilities;
using TBAGW.Utilities.Animation;
using TBAGW.Utilities.GamePlay.Characters;
using TBAGW.Utilities.GamePlay.Characters.Friendly;
using TBAGW.Utilities.GamePlay.Characters.Friendly.Team;
using TBAGW.Utilities.GamePlay.Characters.Hostile;
using TBAGW.Utilities.GamePlay.Spells;
using TBAGW.Utilities.GamePlay.Spells.ShotPattern;
using TBAGW.Utilities.GamePlay.Stats;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen;
using TBAGW.Utilities.OnScreen.GUI;
using TBAGW.Utilities.OnScreen.Particles;
using TBAGW.Utilities.Sound.BG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace TBAGW
{
    class MainGame : Scene
    {
        String backGroundString = @"Graphics\MainGame\Background\Field";
        String popUpString = @"Graphics\MainGame\PopUp\PopUpTexture";
        String popUpFontString = @"Fonts\OptionsFont";
        String selectionIndicatorString = @"Graphics\MainMenu\TestRectangle";
        String complexSpriteString = @"Graphics\Particles\Effects\ComplexSprite1";
        String hitBoxHelpString = @"Graphics\HitBoxHelp";
        String testTextureString = @"Graphics\TestTexture";
        String coneString = @"Graphics\Particles\Effects\Cone";
        String arrowString = @"Graphics\Particles\Effects\Arrow";
        String tempCharString = @"Graphics\Characters\TempChar";
        Texture2D backGroundTexture;
        Texture2D popUpTexture;
        Texture2D selectionIndicatorTexture;
        Texture2D complexSpriteTexture;
        Texture2D hitBoxHelp;
        Texture2D testTexture;
        Texture2D coneTexture;
        Texture2D arrowTexture;
        Texture2D tempCharTexture;
        bool bShowPopUp = false;
        bool bPaused = false;
        WindowPopUp pauseScreen;
        SpriteFont popUpFont;
        SpriteFont pauseFont;
        int currentPauseChoice;

        List<Shape> shapes = new List<Shape>();
        List<BasicEnemy> enemies = new List<BasicEnemy>();
        List<BasicHero> heroes = new List<BasicHero>();
        List<BaseCharacter> activeObjects = new List<BaseCharacter>();

        List<Rectangle> borderTilesBounds = new List<Rectangle>();

        // Animation spikeBallAnimation;

        public override void Initialize(Game1 game)
        {
            base.Initialize(game);
            MusicBGPlayer.Start(MenuSongAssestLoader.BattleSongOminous);
            MusicBGPlayer.Repeat();
            BasicTeamUtility.Initialize();

            game.IsMouseVisible = false;
            base.bIsInitialized = true;
            backGroundTexture = game.Content.Load<Texture2D>(backGroundString);
            popUpTexture = game.Content.Load<Texture2D>(popUpString);
            popUpFont = game.Content.Load<SpriteFont>(popUpFontString);
            pauseFont = game.Content.Load<SpriteFont>(@"Fonts\PauseFont");
            selectionIndicatorTexture = game.Content.Load<Texture2D>(selectionIndicatorString);
            complexSpriteTexture = game.Content.Load<Texture2D>(complexSpriteString);
            hitBoxHelp = game.Content.Load<Texture2D>(hitBoxHelpString);
            testTexture = game.Content.Load<Texture2D>(testTextureString);
            arrowTexture = game.Content.Load<Texture2D>(arrowString);
            coneTexture = game.Content.Load<Texture2D>(coneString);

            tempCharTexture = game.Content.Load<Texture2D>(tempCharString);

            //MapBorderINitialization
            for (int i = -1; i < ((SceneUtility.currentMapSize.X / 64) + 2); i++)
            {
                borderTilesBounds.Add(new Rectangle(i * 64, -64, 64, 64));
                borderTilesBounds.Add(new Rectangle(i * 64, (int)SceneUtility.currentMapSize.Y + 64, 64, 64));
            }

            for (int i = 0; i < ((SceneUtility.currentMapSize.Y / 64) + 1); i++)
            {
                borderTilesBounds.Add(new Rectangle(-64, i * 64, 64, 64));
                borderTilesBounds.Add(new Rectangle((int)SceneUtility.currentMapSize.Y + 64, i * 64, 64, 64));
            }


            /*
            spikeBallTexture = game.Content.Load<Texture2D>(spikeBallString);
            List<Rectangle> tempList = new List<Rectangle>();
            tempList.Add((new Rectangle(0, 0, 64, 64)));
            tempList.Add(new Rectangle(0, 64, 64, 64));
            List<int> tempListFC = new List<int>() { 16, 16, 0, 0, 0 };
            //spikeBallAnimation = new Animation(tempList,tempListFC,spikeBallTexture);
            
            test = new Shape(spikeBallTexture, 1, new Vector2(500, 500), true, default(Vector2), "Spiked ball", new Rectangle(0, 0, 64, 64));
            test.InitializeAnimation(tempList, tempListFC, spikeBallTexture, 600 / 16);
            test.bPlayAnimation = true;
            test.DynamicHitBoxes();
            shapes.Add(test);

            test = new Shape(spikeBallTexture, 1, new Vector2(500, 400), true, default(Vector2), "Spiked ball", new Rectangle(0, 0, 64, 64));
            test.InitializeAnimation(tempList, tempListFC, spikeBallTexture, 600 / 16);
            test.bPlayAnimation = true;
            // test.DynamicHitBoxes();
            shapes.Add(test);

            test = new Shape(spikeBallTexture, 1, new Vector2(600, 500), true, default(Vector2), "Spiked ball", new Rectangle(0, 0, 64, 64));
            test.InitializeAnimation(tempList, tempListFC, spikeBallTexture, 600 / 16);
            test.bPlayAnimation = true;
            test.animationIndex = 1;
            test.DynamicHitBoxes();
            shapes.Add(test);

            test = new Shape(spikeBallTexture, 1, new Vector2(600, 400), true, default(Vector2), "Spiked ball", new Rectangle(0, 0, 64, 64));
            test.InitializeAnimation(tempList, tempListFC, spikeBallTexture, 600 / 16);
            test.bPlayAnimation = true;
            test.animationIndex = 1;
            //test.shapeAnimation.currentAnimation = 1;
            shapes.Add(test);
            */
            ScreenButton[] popUpButtons = { new ScreenButton(null, popUpFont, "Options", new Vector2(283 + 50, 279 + 50)), new ScreenButton(null, popUpFont, "Exit Game", new Vector2(283 + 550, 279 + 50)) };
            pauseScreen = new WindowPopUp(new Rectangle(0, 0, popUpTexture.Width, popUpTexture.Height), popUpTexture, popUpButtons);

            //^*************************************
            heroes.Add(new BasicHero((CharacterAssetsLoader.heroAssetsC1[(int)CharacterAssetsLoader.heroesC1.hero1]), 1, new Vector2(100, 50), true));
            heroes[heroes.Count - 1].rectangleToDraw = CharacterAssetsLoader.heroAssetsC1[(int)CharacterAssetsLoader.heroesC1.hero1].shapeTextureBounds;
            BasicTeamUtility.parties[(int)BasicTeamUtility.FriendlyParties.Party].Add(heroes[heroes.Count - 1]);
            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.MaxHealth] = 150;
            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.Health] = 150;
            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.MaxMana] = 60;
            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.Mana] = 60;
            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.Speed] = 4;
            heroes[heroes.Count - 1].stats.ResetCurrentStats();
            heroes[heroes.Count - 1].activeSpells = new Spell[9] {SpellsAssetLoader.basicMissileArcane, SpellsAssetLoader.basicMissileDark , SpellsAssetLoader.basicMissileEarth,
            SpellsAssetLoader.basicMissileFire,SpellsAssetLoader.basicMissileGrass,SpellsAssetLoader.basicMissileIce,
            default(Spell),default(Spell),default(Spell)};
            activeObjects.Clear();
            activeObjects.AddRange(enemies);
            activeObjects.AddRange(heroes);
        }

        public override void Reload()
        {
            // MusicBGPlayer.Start(MenuSongAssestLoader.BattleSongOminous);
            // MusicBGPlayer.Repeat();
        }

        InputControl spellTimer = new InputControl();

        // Random r1 = new Random();

        //For hero purposes temp
        int tempH = 1;
        //************************

        public override void Update(GameTime gameTime, Game1 game)
        {
            bool bHasTarget = SelectionUtility.HasMemberSelected();

            if (!bShowPopUp && !bPaused)
            {
                base.Update(gameTime, game);


                CameraUpdate();
                NormalRunUpdate(gameTime);

                if (bHasTarget)
                {
                    CommandUpdate();
                }

                SelectionUtility.Update(gameTime, activeObjects);
                MainGUI.Update(gameTime);
            }

            if (bShowPopUp && !bPaused)
            {
                ButtonControlPopUp(gameTime);
            }

            if (bPaused && !bShowPopUp)
            {
                SelectionUtility.Update(gameTime, activeObjects);
                MainGUI.Update(gameTime);
                PauseRunUpdate();
                CameraUpdate();

                if (bHasTarget)
                {
                    CommandUpdatePaused();
                }
            }

            if (bPaused && bShowPopUp)
            {
                ButtonControlPopUp(gameTime);
            }

            SceneUtility.xAxis = xAxis;
            SceneUtility.yAxis = yAxis;

            activeSceneObjects.Clear();
            activeSceneObjects.AddRange(shapes);
            activeSceneObjects.AddRange(enemies);
            activeSceneObjects.AddRange(heroes);

            activeSceneShapeCollections.Clear();
            activeSceneShapeCollections.Add(new IdentifiableShapeList("All Characters List", activeObjects));

            activeSceneButtonCollections.Clear();
            activeSceneButtonCollections.Add(new IdentifiableShapeList("Pop-up buttons List",pauseScreen.popUpButtons));

            activeSceneCharactersCollections.Clear();
            activeSceneCharactersCollections.Add(new IdentifiableShapeList("Enemies List",enemies));
            activeSceneCharactersCollections.Add(new IdentifiableShapeList("Heroes List", heroes));

        }

        /// <summary>
        /// Includes direct and indirect commands & movements
        /// </summary>
        private void CommandUpdate()
        {
            SelectionUtility.primarySelectedCharacter.velocity = Vector2.Zero;

            if (buttonPressUtility.isPressedSub(Game1.moveDownString) && !buttonPressUtility.isPressedSub(Game1.moveUpString))
            {
                SelectionUtility.primarySelectedCharacter.SetVelocityManual(SelectionUtility.primarySelectedCharacter.velocity + new Vector2(0, SelectionUtility.primarySelectedCharacter.stats.baseStats[(int)BasicStatChart.stats.Speed]));
            }

            if (buttonPressUtility.isPressedSub(Game1.moveUpString) && !buttonPressUtility.isPressedSub(Game1.moveDownString))
            {
                SelectionUtility.primarySelectedCharacter.SetVelocityManual(SelectionUtility.primarySelectedCharacter.velocity + new Vector2(0, -SelectionUtility.primarySelectedCharacter.stats.baseStats[(int)BasicStatChart.stats.Speed]));
            }

            if (buttonPressUtility.isPressedSub(Game1.moveLeftString) && !buttonPressUtility.isPressedSub(Game1.moveRightString))
            {
                SelectionUtility.primarySelectedCharacter.SetVelocityManual(SelectionUtility.primarySelectedCharacter.velocity + new Vector2(-SelectionUtility.primarySelectedCharacter.stats.baseStats[(int)BasicStatChart.stats.Speed], 0));
            }

            if (buttonPressUtility.isPressedSub(Game1.moveRightString) && !buttonPressUtility.isPressedSub(Game1.moveLeftString))
            {
                SelectionUtility.primarySelectedCharacter.SetVelocityManual(SelectionUtility.primarySelectedCharacter.velocity + new Vector2(SelectionUtility.primarySelectedCharacter.stats.baseStats[(int)BasicStatChart.stats.Speed], 0));
            }

            if (SelectionUtility.primarySelectedCharacter.velocity!=new Vector2(0))
            {
                xAxis = SceneUtility.xAxis;
                yAxis = SceneUtility.yAxis;
            }
            
        }

        /// <summary>
        /// Indirect commands and movement only
        /// </summary>
        private void CommandUpdatePaused()
        {

        }

        private void ClearNotRenderedShapeObjects()
        {
            List<int> j = new List<int>();
            int k = 0;
            foreach (Shape shape in shapes)
            {
                if (!shape.bRender)
                {

                    j.Add(k);
                }

                k++;
            }

            for (int l = j.Count - 1; l > -1; l--)
            {
                shapes[j[l]].Clear();
                shapes.RemoveAt(j[l]);
            }

        }

        private void ClearNotRenderedActiveObjects()
        {
            List<int> j = new List<int>();
            int k = 0;
            foreach (BaseCharacter shape in activeObjects)
            {
                if (!shape.bIsAlive && !shape.bHasBullets)
                {

                    j.Add(k);
                }

                k++;
            }

            for (int l = j.Count - 1; l > -1; l--)
            {
                activeObjects[j[l]].Clear();
                activeObjects.RemoveAt(j[l]);
            }

            heroes.Clear();
            enemies.Clear();
            foreach (Shape shape in activeObjects)
            {
                if (shape.GetType() == typeof(BasicEnemy))
                {
                    enemies.Add(shape as BasicEnemy);
                }

                if (shape.GetType() == typeof(BasicHero))
                {
                    heroes.Add(shape as BasicHero);
                }
            }
        }

        /// <summary>
        /// Handles general functionality, use CommandUpdate(Paused) for character controll
        /// </summary>
        /// <param name="gameTime"></param>
        private void NormalRunUpdate(GameTime gameTime)
        {
            if (buttonPressUtility.isPressed(Game1.openMenuString))
            {
                if (!bShowPopUp)
                {
                    bShowPopUp = true;
                    KeyboardMouseUtility.bPressed = true;
                    currentPauseChoice = 0;
                }

            }

            if (buttonPressUtility.isPressed(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                bFriendlyFire = !bFriendlyFire;

                foreach (var item in activeObjects)
                {
                    item.bFriendlyFire = bFriendlyFire;
                }
            }

            if (SelectionUtility.HasMemberSelected())
            {
                HandleHotBarKeys(gameTime);
            }


            if (buttonPressUtility.isPressedSub(Game1.moveUpString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {


            }

            //if (buttonPressUtility.isPressedSub(Game1.cameraMoveCenterString))
            //{
            //    Vector2 tempVec = SceneUtility.CenterCamera(xAxis,yAxis);
            //    xAxis = tempVec.X;
            //    yAxis = tempVec.Y;
            //}

            if (buttonPressUtility.isPressedSub(Game1.moveLeftString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                /*
                enemies.Add(new BasicEnemy(game.Content.Load<Texture2D>(@"Graphics\Characters\Hostile\BasicEnemy"), 1, CursorUtility.trueCursorPos, true));
                activeObjects.Clear();
                activeObjects.AddRange(enemies);
                activeObjects.AddRange(heroes);
                */
            }

            if (buttonPressUtility.isPressedSub(Game1.moveRightString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                /*
                heroes.Add(new BasicHero(game.Content.Load<Texture2D>(@"Graphics\Characters\Friendly\BasicHero"), 1, CursorUtility.trueCursorPos, true));
                activeObjects.Clear();
                activeObjects.AddRange(enemies);
                activeObjects.AddRange(heroes);
                 */
            }

            if (buttonPressUtility.isPressedSub(Game1.pauseString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                bPaused = true;
                KeyboardMouseUtility.bPressed = true;
            }


            if (buttonPressUtility.isPressedSub(Game1.debugInfoString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (SceneUtility.isWithinMap(CursorUtility.trueCursorPos))
                {
                    switch (tempH)
                    {
                        case 0:
                            heroes.Add(new BasicHero(CharacterAssetsLoader.heroAssetsC1[(int)CharacterAssetsLoader.heroesC1.hero1], 1, CursorUtility.trueCursorPos, true));
                            heroes[heroes.Count - 1].rectangleToDraw = CharacterAssetsLoader.heroAssetsC1[(int)CharacterAssetsLoader.heroesC1.hero1].shapeTextureBounds;
                            BasicTeamUtility.parties[(int)BasicTeamUtility.FriendlyParties.Party].Add(heroes[heroes.Count - 1]);
                            activeObjects.Clear();
                            activeObjects.AddRange(enemies);
                            activeObjects.AddRange(heroes);
                            break;
                        case 1:
                            heroes.Add(new BasicHero(CharacterAssetsLoader.heroAssetsC1[(int)CharacterAssetsLoader.heroesC1.hero2], 1, CursorUtility.trueCursorPos, true));
                            heroes[heroes.Count - 1].rectangleToDraw = CharacterAssetsLoader.heroAssetsC1[(int)CharacterAssetsLoader.heroesC1.hero2].shapeTextureBounds;
                            BasicTeamUtility.parties[(int)BasicTeamUtility.FriendlyParties.Party].Add(heroes[heroes.Count - 1]);
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.MaxHealth] = 200;
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.Health] = 200;
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.MaxMana] = 30;
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.Mana] = 30;
                            heroes[heroes.Count - 1].stats.ResetCurrentStats();
                            heroes[heroes.Count - 1].activeSpells = new Spell[9] {SpellsAssetLoader.basicMissileFire, SpellsAssetLoader.basicMissileIce , default(Spell),
            default(Spell),default(Spell),default(Spell),
            default(Spell),default(Spell),default(Spell)};
                            activeObjects.Clear();
                            activeObjects.AddRange(enemies);
                            activeObjects.AddRange(heroes);
                            break;
                        case 2:
                            heroes.Add(new BasicHero(CharacterAssetsLoader.heroAssetsC1[(int)CharacterAssetsLoader.heroesC1.hero3], 1, CursorUtility.trueCursorPos, true));
                            heroes[heroes.Count - 1].rectangleToDraw = CharacterAssetsLoader.heroAssetsC1[(int)CharacterAssetsLoader.heroesC1.hero3].shapeTextureBounds;
                            BasicTeamUtility.parties[(int)BasicTeamUtility.FriendlyParties.Party].Add(heroes[heroes.Count - 1]);
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.MaxHealth] = 450;
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.Health] = 450;
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.MaxMana] = 0;
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.Mana] = 0;
                            heroes[heroes.Count - 1].stats.ResetCurrentStats();
                            heroes[heroes.Count - 1].activeSpells = new Spell[9] {SpellsAssetLoader.basicMissileGrass, SpellsAssetLoader.basicMissileArcane , default(Spell),
            default(Spell),default(Spell),default(Spell),
            default(Spell),default(Spell),default(Spell)};
                            activeObjects.Clear();
                            activeObjects.AddRange(enemies);
                            activeObjects.AddRange(heroes);
                            break;
                        case 3:
                            heroes.Add(new BasicHero(CharacterAssetsLoader.heroAssetsC1[(int)CharacterAssetsLoader.heroesC1.hero4], 1, CursorUtility.trueCursorPos, true));
                            heroes[heroes.Count - 1].rectangleToDraw = CharacterAssetsLoader.heroAssetsC1[(int)CharacterAssetsLoader.heroesC1.hero4].shapeTextureBounds;
                            BasicTeamUtility.parties[(int)BasicTeamUtility.FriendlyParties.Party].Add(heroes[heroes.Count - 1]);
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.MaxHealth] = 75;
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.Health] = 75;
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.MaxMana] = 200;
                            heroes[heroes.Count - 1].stats.baseStats[(int)BasicStatChart.stats.Mana] = 200;
                            heroes[heroes.Count - 1].stats.ResetCurrentStats();
                            heroes[heroes.Count - 1].activeSpells = new Spell[9] {SpellsAssetLoader.basicMissileDark, default(Spell) , default(Spell),
            default(Spell),default(Spell),default(Spell),
            default(Spell),default(Spell),default(Spell)};
                            activeObjects.Clear();
                            activeObjects.AddRange(enemies);
                            activeObjects.AddRange(heroes);
                            break;
                    }

                    tempH++;
                }

            }

            foreach (var character in activeObjects)
            {
                character.Update(gameTime, activeObjects);
                character.bFriendlyFire = bFriendlyFire;
            }

            foreach (var character in activeObjects)
            {
                if (!character.bHasTarget)
                {
                    if (character.GetType() == typeof(BasicEnemy) && heroes.Count != 0)
                    {
                        character.setTarget(heroes[heroes.Count - 1]);
                    }
                    else if (character.GetType() == typeof(BasicHero) && enemies.Count != 0)
                    {
                        character.setTarget(enemies[enemies.Count - 1]);
                    }
                }
            }

            ClearNotRenderedActiveObjects();
            ClearNotRenderedShapeObjects();
        }

        private void HandleHotBarKeys(GameTime gameTime)
        {



            if (buttonPressUtility.isPressedSub(Game1.QKey1String) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if ((SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[0] != default(Spell))
                {
                    (SelectionUtility.primarySelectedCharacter as BasicHero).Shoot(gameTime, (SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[0]);
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.QKey2String) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if ((SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[1] != default(Spell))
                {
                    (SelectionUtility.primarySelectedCharacter as BasicHero).Shoot(gameTime, (SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[1]);
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.QKey3String) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if ((SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[2] != default(Spell))
                {
                    (SelectionUtility.primarySelectedCharacter as BasicHero).Shoot(gameTime, (SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[2]);
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.QKey4String) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if ((SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[3] != default(Spell))
                {
                    (SelectionUtility.primarySelectedCharacter as BasicHero).Shoot(gameTime, (SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[3]);
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.QKey5String) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if ((SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[4] != default(Spell))
                {
                    (SelectionUtility.primarySelectedCharacter as BasicHero).Shoot(gameTime, (SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[4]);
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.QKey6String) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if ((SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[5] != default(Spell))
                {
                    (SelectionUtility.primarySelectedCharacter as BasicHero).Shoot(gameTime, (SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[5]);
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.QKey7String) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if ((SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[6] != default(Spell))
                {
                    (SelectionUtility.primarySelectedCharacter as BasicHero).Shoot(gameTime, (SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[6]);
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.QKey8String) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if ((SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[7] != default(Spell))
                {
                    (SelectionUtility.primarySelectedCharacter as BasicHero).Shoot(gameTime, (SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[7]);
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.QKey9String) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if ((SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[8] != default(Spell))
                {
                    (SelectionUtility.primarySelectedCharacter as BasicHero).Shoot(gameTime, (SelectionUtility.primarySelectedCharacter as BasicHero).activeSpells[8]);

                }

            }
        }

        /// <summary>
        /// Handles open menu
        /// </summary>
        /// <param name="gameTime"></param>
        private void PopUpRunUpdate(GameTime gameTime)
        {
            pauseScreen.Update(gameTime);

            if ((buttonPressUtility.isPressed(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed()) || (buttonPressUtility.isMousePressed(Mouse.GetState().LeftButton)))
            {
                switch (currentPauseChoice)
                {
                    case 0:
                        SceneUtility.ChangeScene((int)Game1.Screens.Options);
                        break;
                    case 1:
                        SceneUtility.ChangeScene((int)Game1.Screens.ExitGame);
                        break;
                }
            }

            if (buttonPressUtility.isPressed(Game1.openMenuString) && !KeyboardMouseUtility.AnyButtonsPressed() && bShowPopUp)
            {
                bShowPopUp = false;
            }
        }
        private void ButtonControlPopUp(GameTime gameTime)
        {
            if (buttonPressUtility.isPressed(Game1.moveRightString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (currentPauseChoice < pauseScreen.popUpButtons.Length - 1)
                {
                    currentPauseChoice++;
                }
                else
                {
                    currentPauseChoice = 0;
                }
            }

            if (buttonPressUtility.isPressed(Game1.moveLeftString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (currentPauseChoice == 0)
                {
                    currentPauseChoice = pauseScreen.popUpButtons.Length - 1;
                }
                else
                {
                    currentPauseChoice--;
                }
            }

            int i = 0;
            foreach (var button in pauseScreen.popUpButtons)
            {
                if (button.ContainsMouse())
                {
                    currentPauseChoice = i;
                }
                i++;
            }

            PopUpRunUpdate(gameTime);
        }

        /// <summary>
        /// Handles general Pause Controll
        /// </summary>
        private void PauseRunUpdate()
        {
            if (buttonPressUtility.isPressed(Game1.pauseString))
            {
                bPaused = false;
            }

            if (buttonPressUtility.isPressed(Game1.openMenuString))
            {
                if (!bShowPopUp)
                {
                    bShowPopUp = true;
                    KeyboardMouseUtility.bPressed = true;
                    currentPauseChoice = 0;
                }

            }
        }

        private void CameraUpdate()
        {
            if (buttonPressUtility.isPressedSub(Game1.cameraMoveDownString))
            {
                yAxis -= 3;
            }

            if (buttonPressUtility.isPressedSub(Game1.cameraMoveUpString))
            {
                yAxis += 3;
            }

            if (buttonPressUtility.isPressedSub(Game1.cameraMoveLeftString))
            {
                xAxis += 3;
            }

            if (buttonPressUtility.isPressedSub(Game1.cameraMoveRightString))
            {
                xAxis -= 3;
            }
        }

        bool bFriendlyFire = true;

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            Game1.graphics.GraphicsDevice.Clear(Color.AliceBlue);

            spriteBatch.Draw(backGroundTexture, Vector2.Zero, backGroundTexture.Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //spriteBatch.Draw(arrowTexture, ResolutionUtility.mousePos - new Vector2(arrowTexture.Bounds.Height / 2, arrowTexture.Bounds.Width) * 1, arrowTexture.Bounds, sceneColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //  spriteBatch.DrawString(popUpFont, Game1.graphics.PreferredBackBufferWidth + " x " + Game1.graphics.PreferredBackBufferHeight, new Vector2(500, 100) * 1, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            //  spriteBatch.DrawString(popUpFont, "Complex sprites on screen: " + shapes.Count, new Vector2(300, 200) * 1, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            // spriteBatch.DrawString(popUpFont, "Press Space to quit.\nArrow keys to move camera.\nPress I to add up to 4 heroes.\nLMB to select a hero.\nEsc or bs to deselect selection. \nFriendly fire is: " + bFriendlyFire, new Vector2(300, 200) * 1, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

            DrawMapBorder(spriteBatch);

            if (bPaused)
            {
                spriteBatch.DrawString(pauseFont, "PAUSED", new Vector2(430, 20) - SceneUtility.Axis, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }

            foreach (var character in activeObjects)
            {
                character.Draw(spriteBatch);
            }


            foreach (var shape in shapes)
            {
                shape.Draw(spriteBatch);
            }

            if (SceneUtility.currentScene == (int)Game1.Screens.BGame)
            {
                MainGUI.Draw(spriteBatch);
            }


            if (bShowPopUp)
            {
                pauseScreen.Draw(spriteBatch);
                spriteBatch.Draw(selectionIndicatorTexture, pauseScreen.popUpButtons[currentPauseChoice].ButtonBox(), Color.White);
            }

            return spriteBatch;
        }

        private void DrawMapBorder(SpriteBatch spriteBatch)
        {
            foreach (var borderBlock in borderTilesBounds)
            {
                spriteBatch.Draw(Game1.mapBorderHelp, borderBlock, Game1.mapBorderHelp.Bounds, Color.White);
            }
        }

        public override void UnloadContent(Game1 game)
        {
            base.UnloadContent(game);
        }
    }
}
