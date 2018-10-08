using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TBAGW.Utilities;
using System.Collections.Generic;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.Sound.BG;
using TBAGW.Utilities.ReadWrite;

namespace TBAGW
{
    class Options : Scenes.Scene
    {

        SpriteFont mainOptionsDefaultFont;
        SpriteFont controlsOptionsDefaultFont;
        String MainMenuFont = "Fonts\\OptionsFont";
        String ControlsMenuFont = "Fonts\\InputOptionsFont";
        String selectionIndicatorLoc = "Graphics\\MainMenu\\TestRectangle";
        String sliderBaseTextureLoc = "Graphics\\OptionsMenu\\sliderBaseTexture";
        String sliderButtonTextureLoc = "Graphics\\OptionsMenu\\SliderButtonTexture";
        Texture2D selectionWindow;
        Texture2D selectionIndicator;
        Texture2D sliderBase;
        Texture2D sliderButton;
        String selectionWindowLoc = "Graphics\\OptionsMenu\\SelectionWindow";
        bool bSubMenu = false;
        bool bEditingSubMenu = false;
        bool bDrawOptionsScreen = true;
        Vector2 cursorPosition = new Vector2(0, 0);
        bool bShowMouse = false;
        InputControl mouseControl = new InputControl();//for Mouse activity
        List<ScreenButton> mainOptionButtons = new List<ScreenButton>();
        List<ScreenButton> videoOptionButtons = new List<ScreenButton>();
        List<ScreenButton> musicOptionButtons = new List<ScreenButton>();
        List<ScreenButton> controlsOptionButtons = new List<ScreenButton>();
        List<ScreenButton> miscOptionButtons = new List<ScreenButton>();
        List<List<ScreenButton>> subMenuArray = new List<List<ScreenButton>>();

        int currentChoiceMainOptions = 1;

        public override void Initialize(Game1 game)
        {
            MusicBGPlayer.Start(MenuSongAssestLoader.OptionsSong);
            MusicBGPlayer.Repeat();

            base.bIsInitialized = true;
            mainOptionsDefaultFont = game.Content.Load<SpriteFont>(MainMenuFont);
            controlsOptionsDefaultFont = game.Content.Load<SpriteFont>(ControlsMenuFont);
            selectionWindow = game.Content.Load<Texture2D>(selectionWindowLoc);
            selectionIndicator = game.Content.Load<Texture2D>(selectionIndicatorLoc);
            sliderBase = game.Content.Load<Texture2D>(sliderBaseTextureLoc);
            sliderButton = game.Content.Load<Texture2D>(sliderButtonTextureLoc);

            mainOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "VIDEO", new Vector2(50,50)));
            mainOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "SOUND", new Vector2(50, 250)));
            mainOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "CONTROLS", new Vector2(50, 450)));
            mainOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "MISC", new Vector2(50, 650)));

            GenerateVideoButtons(game);
            GenerateMusicButtons(game);
            GenerateControlsButtons(game);
            GenerateMiscButtons(game);


            subMenuArray.Add(videoOptionButtons);
            subMenuArray.Add(musicOptionButtons);
            subMenuArray.Add(controlsOptionButtons);
            subMenuArray.Add(miscOptionButtons);
            //subMenuArray[1] = musicOptionButtons;
          //  subMenuArray[2] = controlsOptionButtons;
         //   subMenuArray[3] = miscOptionButtons;


        }

        public override void Reload()
        {
            MusicBGPlayer.Start(MenuSongAssestLoader.OptionsSong);
            MusicBGPlayer.Repeat();
        }

        private void GenerateMiscButtons(Game1 game)
        {
            miscOptionButtons.Add( new ScreenButton(null, mainOptionsDefaultFont, "Enable narration mode*", new Vector2(648,50)));
        }

        private void GenerateControlsButtons(Game1 game)
        {
            for (int j = 0; j < Game1.actionList.Count; j++)
            {
                controlsOptionButtons.Add(new InputButton(null, mainOptionsDefaultFont, Game1.actionList[j].actionIndentifierString,new Vector2(628,110+50*j)));
                (controlsOptionButtons[j] as InputButton).setIndentifierString(Game1.actionList[j].actionIndentifierString);
                (controlsOptionButtons[j] as InputButton).controlsFont = controlsOptionsDefaultFont;
                (controlsOptionButtons[j] as InputButton).PositionButton(30 + 598, 110 + 50 * j);
                /*
                controlsOptionButtons[j] = new InputButton();
                controlsOptionButtons[j].Initialize(game, null, mainOptionsDefaultFont, Game1.actionList[j].actionIndentifierString);
                controlsOptionButtons[j].controlsFont = controlsOptionsDefaultFont;
                controlsOptionButtons[j].PositionButton(30 + 598, 110 + 50 * j);
                controlsOptionButtons[j].setIndentifierString(Game1.actionList[j].actionIndentifierString);*/
            }
        }

        private void GenerateMusicButtons(Game1 game)
        {
            SliderButton slider1 = new SliderButton(sliderBase, null, null, sliderButton, new Vector2(898,50));
            SliderButton slider2 = new SliderButton(sliderBase, null, null, sliderButton, new Vector2(898, 300));
            SliderButton slider3 = new SliderButton(sliderBase, null, null, sliderButton, new Vector2(898, 550));
            

            musicOptionButtons.Add(slider1);
            musicOptionButtons.Add(slider2);
            musicOptionButtons.Add(slider3);

        }

        private void GenerateVideoButtons(Game1 game)
        {
            videoOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "1024x768",new Vector2(630,20)));
            videoOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "1280x800", new Vector2(1000, 95)));
            videoOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "1280x1024", new Vector2(630, 170)));
            videoOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "1366x768", new Vector2(1000, 245)));
            videoOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "1440x900", new Vector2(630, 320)));
            videoOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "1600x900", new Vector2(1000, 395)));
            videoOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "1920x1080", new Vector2(630, 470)));
            videoOptionButtons.Add(new ScreenButton(null, mainOptionsDefaultFont, "Fullscreen", new Vector2(630, 650)));
        }

        public override void Update(GameTime gameTime, Game1 game)
        {
            KeyBoardControl(gameTime, game);
            MouseControl(gameTime, game);
            


            foreach (ScreenButton mainOptionButton in mainOptionButtons)
            {
                mainOptionButton.Update(gameTime);
            }

            switch (currentChoiceMainOptions)
            {
                case 1:
                    foreach (ScreenButton videoOptionButton in videoOptionButtons)
                    {
                        if (SceneUtility.prevScene == (int)(Game1.Screens.BGame))
                        {
                            if (cursorPosition.X!=0)
                            {
                                cursorPosition.X = 8;
                            }
                            
                        }

                        videoOptionButton.Update(gameTime);


                    }
                    break;
                case 2:
                    int i = 0;
                    foreach (SliderButton musicButton in musicOptionButtons)
                    {
                        musicButton.Update(gameTime);
                        switch (i)
                        {
                            case 0:
                                musicButton.sliderValue = SceneUtility.masterVolume;
                                break;
                            case 1:
                                musicButton.sliderValue = SceneUtility.musicVolume;
                                break;
                            case 2:
                                musicButton.sliderValue = SceneUtility.soundEffectsVolume;
                                break;
                        }
                        i++;
                    }
                    break;
                case 3:
                    foreach (InputButton controlOptionButton in controlsOptionButtons)
                    {
                        controlOptionButton.Update(gameTime);
                    }

                    break;
                case 4:
                    foreach (ScreenButton miscOptionButton in miscOptionButtons)
                    {
                        miscOptionButton.Update(gameTime);
                    }
                    break;
            }

            base.Update(gameTime, game);

            activeSceneObjects.Clear();
            activeSceneObjects.AddRange(mainOptionButtons);
            activeSceneObjects.AddRange(videoOptionButtons);
            activeSceneObjects.AddRange(musicOptionButtons);
            activeSceneObjects.AddRange(controlsOptionButtons);
            activeSceneObjects.AddRange(miscOptionButtons);

            activeSceneButtonCollections.Clear();
            activeSceneButtonCollections.Add(new IdentifiableShapeList("Main Choice Buttons" ,mainOptionButtons));
            activeSceneButtonCollections.Add(new IdentifiableShapeList("Video Choice Buttons", videoOptionButtons));
            activeSceneButtonCollections.Add(new IdentifiableShapeList("Music Choice Buttons", musicOptionButtons));
            activeSceneButtonCollections.Add(new IdentifiableShapeList("Controls Choice Buttons", controlsOptionButtons));
            activeSceneButtonCollections.Add(new IdentifiableShapeList("Misc Choice Buttons", miscOptionButtons));

        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.GUITransform);
            if (bDrawOptionsScreen)
            {
                Game1.graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 1.0f, 0);

                spriteBatch.Draw(selectionWindow, new Rectangle((int)(598 * 1), 0, (int)(selectionWindow.Width * 1), (int)(selectionWindow.Height * 1)), Color.Black);

                int i = 0;
                foreach (ScreenButton mainOptionButton in mainOptionButtons)
                {
                    mainOptionButton.Draw(spriteBatch);

                    if (currentChoiceMainOptions - 1 == i && !bSubMenu)
                    {
                        spriteBatch.Draw(selectionIndicator, mainOptionButton.ButtonBox(), Color.Black);
                    }
                    i++;
                }

                int j = 0;

                switch (currentChoiceMainOptions)
                {
                    case 1:
                        j = 0;
                        if (SceneUtility.prevScene != (int)(Game1.Screens.BGame))
                        {

                            foreach (ScreenButton videoOptionButton in videoOptionButtons)
                            {
                                videoOptionButton.Draw(spriteBatch);

                                if (ResolutionUtility.bIsFullScreen)
                                {
                                    spriteBatch.DrawString(mainOptionsDefaultFont, "On", new Vector2((600 + 400) * 1, 650 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                }
                                else
                                {
                                    spriteBatch.DrawString(mainOptionsDefaultFont, "Off", new Vector2((600 + 400) * 1, 650 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                }

                                if (j == cursorPosition.X - 1)
                                {
                                    spriteBatch.Draw(selectionIndicator, videoOptionButton.ButtonBox(), Color.Black);
                                }
                                j++;

                            }
                        }
                        else
                        {
                            videoOptionButtons[7].Draw(spriteBatch);

                            spriteBatch.DrawString(mainOptionsDefaultFont, "Resolution can't be \n changed in battle.", new Vector2((600 + 100) * 1, 200 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                            if (ResolutionUtility.bIsFullScreen)
                            {
                                spriteBatch.DrawString(mainOptionsDefaultFont, "On", new Vector2((600 + 400) * 1, 650 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                            }
                            else
                            {
                                spriteBatch.DrawString(mainOptionsDefaultFont, "Off", new Vector2((600 + 400) * 1, 650 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                            }

                            if (cursorPosition.X - 1==7)
                            {
                                spriteBatch.Draw(selectionIndicator, videoOptionButtons[7].ButtonBox(), Color.Black);
                            }
                        }

                        break;
                    case 2:

                        spriteBatch.DrawString(mainOptionsDefaultFont, "Master\nVolume", new Vector2((598 + 50) * 1, 50 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                        spriteBatch.DrawString(mainOptionsDefaultFont, "Music\nVolume", new Vector2((598 + 50) * 1, 300 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                        spriteBatch.DrawString(mainOptionsDefaultFont, "Sound\nEffects", new Vector2((598 + 50) * 1, 550 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                        spriteBatch.DrawString(mainOptionsDefaultFont, SceneUtility.masterVolume.ToString(), new Vector2((1040) * 1, 175 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                        spriteBatch.DrawString(mainOptionsDefaultFont, SceneUtility.musicVolume.ToString(), new Vector2((1040) * 1, 425 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                        spriteBatch.DrawString(mainOptionsDefaultFont, SceneUtility.soundEffectsVolume.ToString(), new Vector2((1040) * 1, 675 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                        j = 0;
                        foreach (ScreenButton musicOptionButton in musicOptionButtons)
                        {
                            musicOptionButton.Draw(spriteBatch);


                            if (j == cursorPosition.X - 1)
                            {
                                spriteBatch.Draw(selectionIndicator, musicOptionButton.ButtonBox(), Color.Black);
                            }
                            j++;

                        }
                        break;
                    case 3:
                        j = 0;
                        foreach (InputButton controlOptionsButton in controlsOptionButtons)
                        {
                            controlOptionsButton.Draw(spriteBatch);


                            if (j == cursorPosition.X - 1)
                            {
                                spriteBatch.Draw(selectionIndicator, controlOptionsButton.ButtonBox(), Color.White);
                                spriteBatch.Draw(selectionIndicator, controlOptionsButton.keysBoxes[(int)cursorPosition.Y], Color.White);
                            }
                            j++;

                        }
                        break;
                    case 4:
                        j = 0;
                        foreach (ScreenButton miscOptionButton in miscOptionButtons)
                        {
                            miscOptionButton.Draw(spriteBatch);

                            if (SceneUtility.bNarrativeMode)
                            {
                                spriteBatch.DrawString(mainOptionsDefaultFont, "On", new Vector2((50 + 598 + 150) * 1, 100 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                            }
                            else
                            {
                                spriteBatch.DrawString(mainOptionsDefaultFont, "Off", new Vector2((50 + 598 + 150) * 1, 100 * 1), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                            }

                            if (j == cursorPosition.X - 1)
                            {
                                spriteBatch.Draw(selectionIndicator, miscOptionButton.ButtonBox(), Color.White);
                            }
                            j++;
                        }

                        break;
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
 

            return spriteBatch;
        }

        private void KeyBoardControl(GameTime gametime, Game1 game)
        {
            if (buttonPressUtility.isPressed(Game1.moveRightString))
            {
                if (!bEditingSubMenu)
                {
                    if (!bSubMenu)
                    {
                        bSubMenu = true;
                        cursorPosition.X = 1;
                    }
                    else if (currentChoiceMainOptions == 3 && cursorPosition.Y == 0)
                    {
                        cursorPosition.Y++;
                    }
                    else if (currentChoiceMainOptions == 3 && cursorPosition.Y == 1)
                    {
                        cursorPosition.Y = 1;

                    }
                    else
                    {

                    }
                }
                else
                {
                    buttonPressUtility.bPressed = false;
                }
            }

            if (buttonPressUtility.isPressed(Game1.debugInfoString))
            {
                int i = 1;
                foreach (var item in activeSceneButtonCollections)
                {
                    FileWriter.Writer(@"My Games\The Betrayer\Options\", "Buttons" + i + ".xml", item);
                    i++;
                }

                i = 1;
                foreach (var item in activeSceneShapeCollections)
                {
                    FileWriter.Writer(@"My Games\The Betrayer\Options\", "Shapes" + i + ".xml", item);
                    i++;
                }

                i = 1;
                foreach (var item in activeSceneCharactersCollections)
                {
                    FileWriter.Writer(@"My Games\The Betrayer\Options\", "Characters" + i + ".xml", item);
                    i++;
                }

            }

            if (buttonPressUtility.isPressed(Game1.moveLeftString) && cursorPosition.Y != 1)
            {
                if (!bEditingSubMenu)
                {
                    if (!bSubMenu)
                    {

                    }
                    else if (currentChoiceMainOptions == 2 && cursorPosition.Y == 1 && currentChoiceMainOptions == 3)
                    {
                        cursorPosition.Y--;
                    }
                    else
                    {
                        bSubMenu = false;
                        cursorPosition.X = 0;
                        cursorPosition.Y = 0;
                    }
                }
                else
                {
                    buttonPressUtility.bPressed = false;
                }
            }
            else if (buttonPressUtility.isPressedSub(Game1.moveLeftString) && cursorPosition.Y == 1)
            {
                cursorPosition.Y--;
            }


            //Down Arrow Key Control
            if (buttonPressUtility.isPressed(Game1.moveDownString) && !bEditingSubMenu)
            {
                if (!bSubMenu)
                {
                    if (currentChoiceMainOptions < mainOptionButtons.Count)
                    {
                        currentChoiceMainOptions++;

                    }
                    else
                    {
                        currentChoiceMainOptions = 1;
                    }
                }
                else if (bSubMenu)
                {
                    if (cursorPosition.X < subMenuArray[currentChoiceMainOptions - 1].Count)
                    {
                        cursorPosition.X++;
                    }
                    else
                    {
                        cursorPosition.X = 1;
                    }

                }
            }

            if (buttonPressUtility.isPressed(Game1.moveUpString) && !bEditingSubMenu)
            {
                if (!bSubMenu)
                {
                    if (currentChoiceMainOptions > 1)
                    {
                        currentChoiceMainOptions--;

                    }
                    else
                    {
                        currentChoiceMainOptions = mainOptionButtons.Count;
                    }
                }
                else if (bSubMenu)
                {
                    if (cursorPosition.X > 1)
                    {
                        cursorPosition.X--;

                    }
                    else
                    {
                        cursorPosition.X = subMenuArray[currentChoiceMainOptions - 1].Count;
                    }
                }
            }



            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {

            }

            if ((((buttonPressUtility.isPressed(Game1.cancelString))) && !KeyboardMouseUtility.AnyButtonsPressed() || (Game1.bIsActive && Mouse.GetState().RightButton == ButtonState.Pressed&&SceneUtility.currentScene!=(int)Game1.Screens.Editor)) && !bEditingSubMenu)
            {
                SwitchScenes();


            }


            if (bSubMenu)
            {
                switch (currentChoiceMainOptions)
                {
                    //Video options
                    case 1:
                        videoOptionHandle();
                        break;
                    //Music options
                    case 2:
                        musicOptionHandle();
                        break;
                    //Control options
                    case 3:
                        controlOptionHandle();
                        break;
                    //Misc. options
                    case 4:
                        miscOptionHandle();
                        break;
                }
            }

        }

        private void SwitchScenes()
        {
        //    Console.Out.WriteLine((int)(Game1.Screens.MainMenu));
            SceneUtility.currentScene = SceneUtility.prevScene;
            SceneUtility.prevScene = (int)(Game1.Screens.Options);
            bEditingSubMenu = false;
            bSubMenu = false;
            currentChoiceMainOptions = 1;
            cursorPosition = Vector2.Zero;
        }

        private void MouseControl(GameTime gametime, Game1 game)
        {
            //Mouse Controll, only when mouse is visible
            CurrentMouseSelection();

            //Up Arrow Key Control
            if ((KeyboardMouseUtility.mouseScrollValue > 0) && Keyboard.GetState().GetPressedKeys().Length == 0 && !bSubMenu)
            {
                if (currentChoiceMainOptions > 1)
                {
                    currentChoiceMainOptions--;

                }
                else
                {
                    currentChoiceMainOptions = mainOptionButtons.Count;
                }
            }
            //Down Arrow Key Control
            if ((KeyboardMouseUtility.mouseScrollValue < 0) && Keyboard.GetState().GetPressedKeys().Length == 0 && !bSubMenu)
            {
                if (currentChoiceMainOptions < mainOptionButtons.Count)
                {
                    currentChoiceMainOptions++;

                }
                else
                {
                    currentChoiceMainOptions = 1;
                }
            }


            if (bSubMenu)
            {
                if ((KeyboardMouseUtility.mouseScrollValue > 0) && Keyboard.GetState().GetPressedKeys().Length == 0)
                {
                    if (cursorPosition.X > 0)
                    {
                        cursorPosition.X--; ;

                    }
                    else
                    {
                        cursorPosition.X = subMenuArray[currentChoiceMainOptions - 1].Count;
                    }
                }
                //Down Arrow Key Control
                if ((KeyboardMouseUtility.mouseScrollValue < 0) && Keyboard.GetState().GetPressedKeys().Length == 0)
                {
                    if (cursorPosition.X < subMenuArray[currentChoiceMainOptions - 1].Count)
                    {
                        cursorPosition.X++;

                    }
                    else
                    {
                        cursorPosition.X = 1;
                    }
                }
            }

        }

        private int CurrentMouseSelection()
        {
            int i = 1;
            foreach (ScreenButton mainMenuButton in mainOptionButtons)
            {
                if (mainMenuButton.ContainsMouse())
                {
                    currentChoiceMainOptions = i;
                    bSubMenu = false;
                    cursorPosition.X = 0;
                    cursorPosition.Y = 0;
                }
                i++;
            }

            switch (currentChoiceMainOptions)
            {
                case 1:
                    i = 1;
                    foreach (ScreenButton videoOptionButton in videoOptionButtons)
                    {
                        if (videoOptionButton.ContainsMouse())
                        {

                            bSubMenu = true;
                            cursorPosition.X = i;
                        }
                        i++;
                    }
                    break;
                case 2:
                    i = 1;
                    foreach (ScreenButton musicOptionButton in musicOptionButtons)
                    {
                        if (musicOptionButton.ContainsMouse())
                        {

                            bSubMenu = true;
                            cursorPosition.X = i;
                        }
                        i++;
                    }
                    break;
                case 3:
                    i = 1;
                    foreach (ScreenButton controlOptionButton in controlsOptionButtons)
                    {
                        if (controlOptionButton.ContainsMouse())
                        {

                            bSubMenu = true;
                            cursorPosition.X = i;
                        }
                        i++;
                    }
                    break;
                case 4:
                    i = 1;
                    foreach (ScreenButton miscOptionButton in miscOptionButtons)
                    {
                        if (miscOptionButton.ContainsMouse())
                        {

                            bSubMenu = true;
                            cursorPosition.X = i;
                        }
                        i++;
                    }
                    break;
            }



            return currentChoiceMainOptions;
        }

        private void videoOptionHandle()
        {
            bDrawOptionsScreen = true;
            if ((buttonPressUtility.isPressed(Game1.confirmString) || (Game1.bIsActive&&Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.bMousePressed&&Game1.graphics.GraphicsDevice.Viewport.Bounds.Contains(CursorUtility.trueCursorPos))))
            {
                switch ((int)cursorPosition.X)
                {
                    case 8:
                        ResolutionUtility.toggleFullscreen();
                        bSubMenu = false;
                        cursorPosition.X = 0;
                        cursorPosition.Y = 0;
                        break;
                }
            }
            if (SceneUtility.prevScene != 5)
            {
                bDrawOptionsScreen = true;
                if ((buttonPressUtility.isPressed(Game1.confirmString) || (Game1.bIsActive && Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.bMousePressed)))
                {
                    switch ((int)cursorPosition.X)
                    {
                        case 1:
                            ResolutionUtility.AdjustResolution(1024, 768, Game1.graphics);
                            break;
                        case 2:
                            ResolutionUtility.AdjustResolution(1280, 800, Game1.graphics);
                            break;
                        case 3:
                            ResolutionUtility.AdjustResolution(1280, 1024, Game1.graphics);
                            break;
                        case 4:
                            ResolutionUtility.AdjustResolution(1366, 768, Game1.graphics);
                            break;
                        case 5:
                            ResolutionUtility.AdjustResolution(1440, 900, Game1.graphics);
                            break;
                        case 6:
                            ResolutionUtility.AdjustResolution(1600, 900, Game1.graphics);
                            break;
                        case 7:
                            ResolutionUtility.AdjustResolution(1920, 1080, Game1.graphics);
                            break;
                        case 8:
                            ResolutionUtility.toggleFullscreen();
                            bSubMenu = false;
                            cursorPosition.X = 0;
                            cursorPosition.Y = 0;
                            break;
                    }
                }
                if (buttonPressUtility.isPressed(Game1.moveLeftString))
                { bEditingSubMenu = false; }
            }
        }

        private void musicOptionHandle()
        {
            bDrawOptionsScreen = true;

            if (buttonPressUtility.isPressed(Game1.confirmString))
            {
                if (!bEditingSubMenu)
                {
                    bEditingSubMenu = true;
                }
                else
                {

                    buttonPressUtility.bPressed = false;
                }

            }

            if (buttonPressUtility.isPressedSub(Game1.moveLeftString) && bEditingSubMenu)
            {


                switch ((int)cursorPosition.X)
                {
                    case 1:
                        SceneUtility.adjustMasterVolume(SceneUtility.masterVolume - 1);
                        break;
                    case 2:
                        SceneUtility.adjustMusicVolume(SceneUtility.musicVolume - 1);
                        break;
                    case 3:
                        SceneUtility.adjustSEVolume(SceneUtility.soundEffectsVolume - 1);
                        break;
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.moveRightString) && bEditingSubMenu)
            {
                switch ((int)cursorPosition.X)
                {
                    case 1:
                        SceneUtility.adjustMasterVolume(SceneUtility.masterVolume + 1);
                        break;
                    case 2:
                        SceneUtility.adjustMusicVolume(SceneUtility.musicVolume + 1);
                        break;
                    case 3:
                        SceneUtility.adjustSEVolume(SceneUtility.soundEffectsVolume + 1);
                        break;
                }
            }

            if (KeyboardMouseUtility.bMousePressed)
            {

                bEditingSubMenu = false;

                switch ((int)cursorPosition.X)
                {
                    case 1:
                        SceneUtility.masterVolume = (musicOptionButtons[0] as SliderButton).Slide(SceneUtility.masterVolume);
                        break;
                    case 2:
                        SceneUtility.musicVolume = (musicOptionButtons[1] as SliderButton).Slide(SceneUtility.musicVolume);
                        break;
                    case 3:
                        SceneUtility.soundEffectsVolume = (musicOptionButtons[2] as SliderButton).Slide(SceneUtility.soundEffectsVolume);
                        break;
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.cancelString) || buttonPressUtility.isPressed(Game1.confirmString) && bEditingSubMenu)
            {
                bEditingSubMenu = false;

            }

        }

        private void controlOptionHandle()
        {
            bDrawOptionsScreen = true;
            Game1.graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.WhiteSmoke, 1.0f, 0);
        }

        private void miscOptionHandle()
        {
            if ((buttonPressUtility.isPressed(Game1.confirmString) || (Game1.bIsActive&&Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.bMousePressed)))
            {
                switch ((int)cursorPosition.X)
                {
                    case 1:
                        if (SceneUtility.bNarrativeMode)
                        {
                            SceneUtility.bNarrativeMode = false;
                        }
                        else
                        {
                            SceneUtility.bNarrativeMode = true;
                        }
                        break;
                }
            }
        }

        public override void UnloadContent(Game1 game)
        {
            base.UnloadContent(game);
        }

    }
}
