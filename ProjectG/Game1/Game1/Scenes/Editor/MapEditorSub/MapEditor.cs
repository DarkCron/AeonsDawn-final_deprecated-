using TBAGW.Scenes.Editor.SpriteEditorSub;
using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.OnScreen.Particles;
using TBAGW.Utilities.ReadWrite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TBAGW.Scenes.Editor
{
    class MapEditor : Scene
    {
        #region
        List<Scene> scenes = new List<Scene>();
        static public List<Scene> localScenes = new List<Scene>();
        public enum MapEditorScenes { MapEditor = 0, MapBuilder };
        public int currentScene = (int)MapEditorScenes.MapEditor;

        bool bShowed = false;
        bool firstRun = true;

        Matrix MapEditorMatrix;
        const int cameraSpeed = 5;
        int cameraPosX = 0;
        int cameraPosY = 0;

        MapBuilder mapBuilder = new MapBuilder();

        String completeContentDir;

        private ActiveInput mapXSizeInput;
        private ActiveInput mapYSizeInput;
        private ActiveInput mapNameInput;
        public List<ActiveInput> activeInputButtons = new List<ActiveInput>();
        bool bRerunFileLoader = false;

        private ScreenButton CreateMapButton = new ScreenButton(null, Game1.defaultFont, "Create Map", Vector2.Zero);
        BasicMap loadedMap = default(BasicMap);
        public static bool bIsRunning = true;
        System.Windows.Forms.OpenFileDialog openMap = new System.Windows.Forms.OpenFileDialog();
        System.Windows.Forms.SaveFileDialog saveMap = new System.Windows.Forms.SaveFileDialog();
        ScreenButton loadMapButton = new ScreenButton(null, Game1.defaultFont, "Load Map", new Vector2(100, 100));
        #endregion
        public override void Initialize(Game1 game)
        {

        }

        public void ResetCamera()
        {
            cameraPosX = 0;
            cameraPosY = 0;
        }

        public override void Update(GameTime gameTime, Game1 game)
        {


            if (MapEditor.bIsRunning)
            {


                #region firstrun
                if (firstRun)
                {
                    mapXSizeInput = new ActiveInput("X-Size map: ", "0", Vector2.Zero, game);
                    mapYSizeInput = new ActiveInput("Y-Size map: ", "0", Vector2.Zero, game);
                    mapNameInput = new ActiveInput("Map name: ", "Map", Vector2.Zero, game);
                    mapXSizeInput.bNumericInputOnly = true;
                    mapYSizeInput.bNumericInputOnly = true;
                    activeInputButtons.Add(mapXSizeInput);
                    activeInputButtons.Add(mapYSizeInput);
                    activeInputButtons.Add(mapNameInput);
                    mapXSizeInput.bHasMinValue = true;
                    mapXSizeInput.minValue = 1;
                    mapYSizeInput.bHasMinValue = true;
                    mapYSizeInput.minValue = 1;

                    firstRun = false;

                    localScenes.Add(mapBuilder);
                    

                    

                }
                #endregion

                if (Game1.bIsDebug)
                {
                    openMap.Filter = "CGMAPC Files (.cgmapc)|*.cgmapc";
                    openMap.InitialDirectory = Game1.rootTBAGW;
                }
                else
                {
                    openMap.Filter = "CGMAP Files (.cgmap)|*.cgmap";
                    openMap.InitialDirectory = Game1.rootContentExtra;
                }
                openMap.FilterIndex = 1;
                openMap.Multiselect = false;
                saveMap.Filter = "CGMAP Files (.cgmap)|*.cgmap";
                saveMap.Title = "Save a CGMAP File";
                if (Game1.bIsDebug)
                {
                    saveMap.InitialDirectory = Game1.rootTBAGW;
                }
                else
                {
                    saveMap.InitialDirectory = Game1.rootContentExtra;
                }

                bool usingActiveInput = false;

                Vector2 EditorCursorPos = Mouse.GetState().Position.ToVector2() + new Vector2(cameraPosX, -cameraPosY);
                loadMapButton.Update(gameTime);
                if (loadMapButton.buttonBox.Contains(EditorCursorPos))
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed&&Game1.bIsActive  && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        System.Windows.Forms.DialogResult testDia = openMap.ShowDialog();

                        if (testDia == System.Windows.Forms.DialogResult.OK&&openMap.FileName.Contains(openMap.InitialDirectory))
                        {
                          //  Console.WriteLine("You selected: " + openMap.FileName);
                            loadedMap = EditorFileWriter.MapReader(openMap.FileName);
                        }
                    }
                }

                #region MapEditor only logic

                foreach (var item in activeInputButtons)
                {
                    item.Update(gameTime);
                    item.Contains(EditorCursorPos);
                }

                foreach (var item in activeInputButtons)
                {
                    if (item.bEnableInput)
                    {
                        usingActiveInput = true;
                        break;
                    }
                }

                int i = 0;
                foreach (var item in activeInputButtons)
                {

                    item.position = new Vector2(100, 200 + 75 * i);
                    i++;
                }

                CreateMapButton.position = new Vector2(activeInputButtons[activeInputButtons.Count - 1].position.X, activeInputButtons[activeInputButtons.Count - 1].position.Y + 75);

                CreateMapButton.Update(gameTime);
                if (CreateMapButton.ButtonBox().Contains(EditorCursorPos) && Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    bool bCorrectSave = false;
                    while (!bCorrectSave)
                    {
                        try
                        {
                            if (Game1.bIsDebug)
                            {
                                BasicMap newMap = new BasicMap();
                                newMap.mapName = mapNameInput.ReturnInputString();
                                saveMap.InitialDirectory = Game1.rootTBAGW;
                                System.Windows.Forms.DialogResult dia = saveMap.ShowDialog();
                                if (saveMap.FileName != "" && dia == System.Windows.Forms.DialogResult.OK && saveMap.FileName.StartsWith(saveMap.InitialDirectory))
                                {
                                    newMap.mapName = Path.GetFileNameWithoutExtension(saveMap.FileName);
                                    mapNameInput.AssignInput(Path.GetFileNameWithoutExtension(saveMap.FileName));
                                    //  Console.WriteLine("Line= " + Path.GetDirectoryName(saveMap.FileName));
                                    EditorFileWriter.MapWriter(saveMap.FileName, newMap);
                                    Console.WriteLine(newMap.mapLocation);
                                    loadedMap = EditorFileWriter.MapReader(saveMap.FileName); ;
                                    bCorrectSave = true;
                                }
                                else if (dia == System.Windows.Forms.DialogResult.Cancel)
                                {
                                    bCorrectSave = true;
                                }
                                else
                                {
                                    //  Console.WriteLine("Yow");
                                    System.Windows.Forms.MessageBox.Show("Make sure you save the map within the application folder.");
                                }
                            }
                            else
                            {
                                BasicMap newMap = new BasicMap();
                                newMap.mapName = mapNameInput.ReturnInputString();
                                saveMap.InitialDirectory = Game1.rootContentExtra;
                                System.Windows.Forms.DialogResult dia = saveMap.ShowDialog();
                                if (saveMap.FileName != "" && dia == System.Windows.Forms.DialogResult.OK && saveMap.FileName.StartsWith(Game1.rootContentExtra))
                                {
                                    newMap.mapName = Path.GetFileNameWithoutExtension(saveMap.FileName);
                                    mapNameInput.AssignInput(Path.GetFileNameWithoutExtension(saveMap.FileName));
                                    //  Console.WriteLine("Line= " + Path.GetDirectoryName(saveMap.FileName));
                                    EditorFileWriter.MapWriter(saveMap.FileName, newMap);
                                    Console.WriteLine(newMap.mapLocation);
                                    loadedMap = EditorFileWriter.MapReader(saveMap.FileName); ;
                                    bCorrectSave = true;
                                }
                                else if (dia == System.Windows.Forms.DialogResult.Cancel)
                                {
                                    bCorrectSave = true;
                                }
                                else
                                {
                                    //  Console.WriteLine("Yow");
                                    System.Windows.Forms.MessageBox.Show("Make sure you save the map within the application folder.");
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            Console.Out.WriteLine("Error:\n" + e);
                        }
                    }


                }

                if (currentScene == 0 && !usingActiveInput)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        cameraPosY -= cameraSpeed;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Z))
                    {
                        cameraPosY += cameraSpeed;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Q))
                    {
                        cameraPosX -= cameraSpeed;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        cameraPosX += cameraSpeed;
                    }
                }
                #endregion

                if (loadedMap != default(BasicMap))
                {
                    currentScene = (int)MapEditor.MapEditorScenes.MapBuilder;
                    bRerunFileLoader = true;
                    mapBuilder.Start(game, loadedMap);
                    loadedMap = default(BasicMap);
                    bIsRunning = false;
                }




                if (Mouse.GetState().RightButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed() && currentScene > 0)
                {
                    switch (currentScene)
                    {
                        case (int)MapEditorScenes.MapEditor:
                            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("You sure want to exit Map editing? NOTE: be sure to save first!", "Leaving", System.Windows.Forms.MessageBoxButtons.YesNo);
                            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                            {
                                currentScene--;
                            }
                            else if (dialogResult == System.Windows.Forms.DialogResult.No)
                            {
                                //do something else
                            }
                            
                            break;
                    }
                }
            }
            else if (!MapEditor.bIsRunning)
            {
                mapBuilder.Update(gameTime, game);
            }

            MapEditorMatrix = Matrix.CreateTranslation(-cameraPosX, cameraPosY, 1);

        }

        internal void ChangeScene()
        {

        }

        public override void UnloadContent(Game1 game)
        {
        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, MapEditorMatrix);

            Game1.graphics.GraphicsDevice.Clear(Color.White);

            if (bIsRunning)
            {
                #region MapEditor-only drawing
                spriteBatch.DrawString(Game1.defaultFont, "Select map to load for editing:", new Vector2(50, 50), Color.Black);
                spriteBatch.DrawString(Game1.defaultFont, loadMapButton.buttonText, loadMapButton.buttonBox.Location.ToVector2(), Color.Black);
                spriteBatch.DrawString(Game1.defaultFont, "Or create a new map:", new Vector2(100, 50 + 100), Color.Black);


                foreach (var item in activeInputButtons)
                {
                    item.Draw(spriteBatch);
                }

                CreateMapButton.Draw(spriteBatch);
                #endregion
            }
            else
            {
                mapBuilder.Draw(gametime, spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);

            return spriteBatch;
        }

    }

}
