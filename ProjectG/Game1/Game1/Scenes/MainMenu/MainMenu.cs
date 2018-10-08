using TBAGW;
using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using TBAGW.Scenes;
using TBAGW.Utilities.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;
using TBAGW.Utilities.Animation;
using TBAGW.Utilities.Sound.BG;
using TBAGW.Utilities.OnScreen.Particles;
using System.Collections;
using TBAGW.Utilities.ReadWrite;

namespace TBAGW
{
    class MainMenu : Scene
    {

        SpriteFont mainMenuDefaultFont;
        Texture2D testTexture;
        String MainMenuFont = "Fonts\\MainMenuFont";
        String[] MainMenuBG = { @"Graphics\MainMenu\BackGroundMM1", @"Graphics\MainMenu\BackGroundMM2", @"Graphics\MainMenu\BackGroundMM3" };
        String MainMenuBGTex = @"Graphics\MainMenu\BackGroundMMBG";
        String MainMenuFGTex = @"Graphics\MainMenu\BackGroundMMFG";
        String MainMenuCloudsTex = @"Graphics\MainMenu\MMClouds";
        Texture2D[] mainMenuTex = new Texture2D[2];
        Texture2D[] mainMenuBGTexture = new Texture2D[3];
        Texture2D mainMenuClouds;
        // Animation mainMenuAnimation;
        List<ScreenButton> mainMenuButtons = new List<ScreenButton>();
        List<Shape> clouds = new List<Shape>();



        int currentChoice = 0; //0 no choice , 1 Start Story, 2 Continue Story, 3 Options, 4 Exitgame
        int nextChoice = 0; //What choice has player clicked on?

        String startStoryString = "Start story";
        String continueStoryString = "Continue story";
        String optionsString = "Options";
        String exitGameString = "Exit game";

        InputControl mouseControl = new InputControl();//for Mouse activity
        InputControl buttonControl = new InputControl();//for keyboard keys

        Vector2 cloudsPos1 = new Vector2(-1366, 320);
        Vector2 cloudsPos2 = new Vector2(500, 305);
        Vector2 cloudsPos3 = new Vector2(100, 270);
        Vector2 cloudsPos4 = new Vector2(-300, 265);
        Vector2 cloudsPos5 = new Vector2(-700, 263);
        Vector2 cloudsPos6 = new Vector2(-1000, 260);
        /*Also contains graphic resources to load.
         * 
         * 
         */
        public override void Initialize(Game1 game)
        {
            MusicBGPlayer.Start(MenuSongAssestLoader.MMSong);
            MusicBGPlayer.Repeat();

            base.bIsInitialized = true;
            mainMenuDefaultFont = game.Content.Load<SpriteFont>(MainMenuFont);
            testTexture = game.Content.Load<Texture2D>("Graphics\\MainMenu\\TestRectangle");

            mainMenuTex[0] = game.Content.Load<Texture2D>(MainMenuFGTex);
            mainMenuTex[1] = game.Content.Load<Texture2D>(MainMenuBGTex);
            mainMenuClouds = game.Content.Load<Texture2D>(MainMenuCloudsTex);

            for (int i = 0; i < 3; i++)
            {
                mainMenuBGTexture[i] = game.Content.Load<Texture2D>(MainMenuBG[i]);
            }
            // mainMenuAnimation = new Animation(mainMenuBGTexture);



            mainMenuButtons.Add(new ScreenButton(null, mainMenuDefaultFont, startStoryString, new Vector2(370, 220)));
            mainMenuButtons.Add(new ScreenButton(null, mainMenuDefaultFont, continueStoryString, new Vector2(300, 350)));
            mainMenuButtons.Add(new ScreenButton(null, mainMenuDefaultFont, optionsString, new Vector2(470, 480)));
            mainMenuButtons.Add(new ScreenButton(null, mainMenuDefaultFont, exitGameString, new Vector2(410, 610)));

            clouds.Add(new Shape(mainMenuClouds, 1, cloudsPos1, false, default(Vector2), "Cloud 1"));
            clouds.Add(new Shape(mainMenuClouds, 1, cloudsPos2, false, default(Vector2), "Cloud 2"));
            clouds.Add(new Shape(mainMenuClouds, 1, cloudsPos3, false, default(Vector2), "Cloud 3"));
            clouds.Add(new Shape(mainMenuClouds, 1, cloudsPos4, false, default(Vector2), "Cloud 4"));
            clouds.Add(new Shape(mainMenuClouds, 1, cloudsPos5, false, default(Vector2), "Cloud 5"));
            clouds.Add(new Shape(mainMenuClouds, 1, cloudsPos6, false, default(Vector2), "Cloud 6"));

            
        }

        public override void Reload()
        {
            MusicBGPlayer.Start(MenuSongAssestLoader.MMSong);
            MusicBGPlayer.Repeat();
        }

        public override void Update(GameTime gametime, Game1 game)
        {
            //mainMenuAnimation.Update(gametime);
            KeyBoardControl(gametime, game);
            MouseControl(gametime, game);

            foreach (ScreenButton mainMenuButton in mainMenuButtons)
            {
                mainMenuButton.Update(gametime);
            }

            UpdateClouds();

            base.Update(gametime,game);

            activeSceneObjects.Clear();
            activeSceneObjects.AddRange(clouds);
            activeSceneObjects.AddRange(mainMenuButtons);

            activeSceneButtonCollections.Clear();
            activeSceneButtonCollections.Add(new IdentifiableShapeList("Main Menu Buttons", mainMenuButtons));

            activeSceneShapeCollections.Clear();
            activeSceneShapeCollections.Add(new IdentifiableShapeList("Clouds",clouds));

        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.GUITransform);

            //Game1.fx.CurrentTechnique.Passes[0].Apply();
            //mainMenuAnimation.Draw(spriteBatch);

            spriteBatch.Draw(mainMenuTex[1], Vector2.Zero, mainMenuBGTexture[0].Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            foreach (var item in clouds)
            {
                item.Draw(spriteBatch);
            }

            spriteBatch.Draw(mainMenuTex[0], Vector2.Zero, mainMenuBGTexture[1].Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);



            foreach (ScreenButton mainMenuButton in mainMenuButtons)
            {
                mainMenuButton.Draw(spriteBatch);
            }

            //This shows the "selection indicator"
            switch (currentChoice)
            {
                case 0:
                    break;
                case 1:
                    spriteBatch.Draw(testTexture, mainMenuButtons[currentChoice - 1].ButtonBox(), Color.Black);
                    break;
                case 2:
                    spriteBatch.Draw(testTexture, mainMenuButtons[currentChoice - 1].ButtonBox(), Color.Black);
                    break;
                case 3:
                    spriteBatch.Draw(testTexture, mainMenuButtons[currentChoice - 1].ButtonBox(), Color.Black);
                    break;
                case 4:
                    spriteBatch.Draw(testTexture, mainMenuButtons[currentChoice - 1].ButtonBox(), Color.Black);
                    break;
            }


            return spriteBatch;
        }

        private void UpdateClouds()
        {
            if (clouds[1].position.X < 1366)
            {
                clouds[1].position.X += 3;
            }
            else
            {
                clouds[1].position.X = -1366;
            }

            if (clouds[0].position.X < 1366)
            {
                clouds[0].position.X += 2;
            }
            else
            {
                clouds[0].position.X = -1366;
            }

            if (clouds[2].position.X < 1366)
            {
                clouds[2].position.X += 1;
            }
            else
            {
                clouds[2].position.X = -1366;
            }

            if (clouds[3].position.X < 1366)
            {
                clouds[3].position.X += 0.85f;
            }
            else
            {
                clouds[3].position.X = -1366;
            }

            if (clouds[4].position.X < 1366)
            {
                clouds[4].position.X += 0.75f;
            }
            else
            {
                clouds[4].position.X = -1366;
            }

            if (clouds[5].position.X < 1366)
            {
                clouds[5].position.X += 0.7f;
            }
            else
            {
                clouds[5].position.X = -1366;
            }
        }

        private void KeyBoardControl(GameTime gametime, Game1 game)
        {
            if (!buttonControl.secondTimer(gametime,2))
            {
                if (KeyboardMouseUtility.AnyButtonsPressed())
                {
                    buttonControl.elapsedSeconds = 0;
                }
            }
            else
            {
                currentChoice = 0;
            }
            //Up Arrow Key Control
            if (buttonPressUtility.isPressed(Game1.moveUpString))
            {
                buttonControl.elapsedMilliseconds = 0;
                if (currentChoice > 1)
                {
                    currentChoice--;

                }
                else
                {
                    currentChoice = mainMenuButtons.Count;
                }
            }
            //Down Arrow Key Control
            if (buttonPressUtility.isPressed(Game1.moveDownString))
            {
                buttonControl.elapsedMilliseconds = 0;
                if (currentChoice < mainMenuButtons.Count)
                {
                    currentChoice++;

                }
                else
                {
                    currentChoice = 1;
                }
            }

            if (buttonPressUtility.isPressed(Game1.confirmString))
            {
                SceneSelect(game);
            }



            if (buttonPressUtility.isPressed(Game1.debugInfoString))
            {
                int i = 1;
                foreach (var item in activeSceneButtonCollections)
                {
                    FileWriter.Writer(@"My Games\The Betrayer\MM\","MMButtons" + item.name.Replace(" ",String.Empty)+ ".xml", item);
                    i++;
                }

                i = 1;
                foreach (var item in activeSceneShapeCollections)
                {
                    FileWriter.Writer(@"My Games\The Betrayer\MM\", "MMShapes" + item.name.Replace(" ", String.Empty) + ".xml", item);
                    i++;
                }

                i = 1;
                foreach (var item in activeSceneCharactersCollections)
                {
                    FileWriter.Writer(@"My Games\The Betrayer\MM\", "MMCharacters" + item.name.Replace(" ", String.Empty) + ".xml", item);
                    i++;
                }

                FileWriter.WriteActionKeyFile();

            }

            if (Keyboard.GetState().IsKeyDown(Keys.L)&&!KeyboardMouseUtility.AnyButtonsPressed())
            {
                IdentifiableShapeList temp = FileWriter.Reader(@"My Games\The Betrayer\MM\", "MMShapes" + activeSceneShapeCollections[0].name.Replace(" ", String.Empty) + ".xml");
                clouds.Clear();
                foreach (Shape item in temp.objectList)
                {
                    item.shapeTexture = mainMenuClouds; 
                    clouds.Add(item);
                    
                }

            }


            if (buttonPressUtility.isPressed(Game1.cancelString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                game.Exit();
            }

            

        }

        private void MouseControl(GameTime gametime, Game1 game)
        {

            CurrentMouseSelection();

            //Up Arrow Key Control
            if ((KeyboardMouseUtility.mouseScrollValue > 0) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                buttonPressUtility.bPressed = true;
                if (currentChoice > 1)
                {
                    currentChoice--;

                }
                else
                {
                    currentChoice = mainMenuButtons.Count;
                }
            }
            //Down Arrow Key Control
            if ((KeyboardMouseUtility.mouseScrollValue < 0) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                buttonPressUtility.bPressed = true;
                if (currentChoice < mainMenuButtons.Count)
                {
                    currentChoice++;

                }
                else
                {
                    currentChoice = 1;
                }
            }


            if (buttonPressUtility.isMousePressed(Mouse.GetState().LeftButton))
            {
                SceneSelect(game);
                buttonPressUtility.bPressed = true;
            }

        }

        private void CurrentMouseSelection()
        {
            int i = 1;
            foreach (ScreenButton mainMenuButton in mainMenuButtons)
            {
                if (mainMenuButton.ContainsMouse())
                {
                    currentChoice = i;
                    break;
                }
                i++;
            }

        }

        public override void UnloadContent(Game1 game)
        {
            game.Content.Unload();

        }

        private void SceneSelect(Game1 game)
        {
            switch (currentChoice)
            {
                case 1:
                    nextChoice = (int)(Game1.Screens.MainGameStart);
                    break;
                case 2:
                    nextChoice = (int)(Game1.Screens.MainGameContinue);
                    break;
                case 3:
                    nextChoice = (int)(Game1.Screens.Options);
                    break;
                case 4:
                    nextChoice = (int)(Game1.Screens.ExitGame);
                    break;
            }


            SceneUtility.ChangeScene(nextChoice);

        }

    }
}

