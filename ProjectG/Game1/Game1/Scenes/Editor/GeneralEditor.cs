using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TBAGW.Scenes.Editor
{
    class GeneralEditor : Scene
    {
        public List<Scene> scenes = new List<Scene>();

        private Vector2 EditorFormat = new Vector2(600, 400);
        private Vector2 EditorScale;
        private Vector2 EditorCursorPos;
        private Vector2 EditorGameWindowPos = new Vector2(0);
        private Matrix transformRightBox;
        private Matrix transformDownBox;
        private float transformStep = 5f;

        private Texture2D EditorBG;

        private Rectangle rightBox = new Rectangle();
        private Rectangle lowerBox = new Rectangle();
        private Rectangle gameBox = new Rectangle();

        private int yTranslateRightBoxMin = 0;
        private int yTranslateDownBoxMin = 0;

        private int yTranslateRightBox = 0;
        private int yTranslateDownBox = 0;

        private int yTranslateRightStep = 5;
        private int yTranslateDownStep = 5;

        private int yTranslateRightBoxMax = 100;
        private int yTranslateDownBoxMax = 0;

        private bool bRightBoxSelected = false;
        private bool bDownBoxSelected = false;
        private bool bGameBoxSelected = false;
        private bool bUpdateGameViewPort = false;

        private float zoomScale = 1f;
        private float zoomScaleMin = 0.5f;
        private float zoomScaleMax = 3f;
        private float zoomScaleStep = 0.1f;

        List<Type> types = new List<Type>();
        List<String> typeNames = new List<String>();

        List<ScreenButton> downTabButtons = new List<ScreenButton>();

        public override void Initialize(Game1 game)
        {
            base.Initialize(game);


            downTabButtons.Add(new ScreenButton(null, Game1.defaultFont, "Allow Viewport Update          ", new Vector2(lowerBox.X + 100, lowerBox.Y + 100)));
        }

        public override void Update(GameTime gameTime, Game1 game)
        {
            transformRightBox = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(0, -yTranslateRightBox, 0));
            transformDownBox = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(0, -yTranslateDownBox, 0));
            EditorCursorPos = Mouse.GetState().Position.ToVector2();

            WhatWindowsSelected();

            if (bRightBoxSelected)
            {
                MoveRightBox();
                UpdateRightBox(gameTime);
            }
            else if (bDownBoxSelected)
            {
                MoveDownBox();
                UpdateDownBox(gameTime);
            }
            else if (bGameBoxSelected)
            {
                MoveGameBox();
                ZoomGameBox();
                UpdateGameBox(gameTime, game);
            }

            if (!bGameBoxSelected)
            {
                FinalUpdate();
            }




            SceneUtility.EditorTransformScale = EditorScale * zoomScale;
            SceneUtility.EditorTransformTranslation = EditorGameWindowPos;


        }

        private void FinalUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
            {
                Console.Out.WriteLine("Going to Sprite Editor");
            }
        }

        private void UpdateGameBox(GameTime gameTime, Game1 game)
        {
            if (bUpdateGameViewPort)
            {
                scenes[SceneUtility.prevScene].Update(gameTime, game);
            }
        }

        private void UpdateDownBox(GameTime gameTime)
        {
            foreach (var button in downTabButtons)
            {
                button.Update(gameTime);
            }


            if (downTabButtons[0].ButtonBox().Contains(EditorCursorPos) && buttonPressUtility.isMousePressed(Mouse.GetState().LeftButton))
            {
                bUpdateGameViewPort = !bUpdateGameViewPort;
            }

        }

        private void UpdateRightBox(GameTime gameTime)
        {
        }

        private void MoveGameBox()
        {
            if (buttonPressUtility.isPressedSub(Game1.moveDownString))
            {
                EditorGameWindowPos.Y -= (transformStep / zoomScale);
            }

            if (buttonPressUtility.isPressedSub(Game1.moveUpString))
            {
                EditorGameWindowPos.Y += (transformStep / zoomScale);
            }

            if (buttonPressUtility.isPressedSub(Game1.moveLeftString))
            {
                EditorGameWindowPos.X += (transformStep / zoomScale);
            }

            if (buttonPressUtility.isPressedSub(Game1.moveRightString))
            {
                EditorGameWindowPos.X -= (transformStep / zoomScale);
            }

        }

        private void MoveDownBox()
        {
            if (buttonPressUtility.isPressedSub(Game1.moveDownString))
            {
                if (yTranslateDownBox + yTranslateDownStep < yTranslateDownBoxMax)
                {
                    yTranslateDownBox += yTranslateDownStep;
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.moveUpString))
            {
                if (yTranslateDownBox - yTranslateDownStep > yTranslateDownBoxMin)
                {
                    yTranslateDownBox -= yTranslateDownStep;
                }
            }
        }

        private void WhatWindowsSelected()
        {
            if (rightBox.Contains(EditorCursorPos))
            {
                if (buttonPressUtility.isMousePressed(Mouse.GetState().LeftButton))
                {
                    bRightBoxSelected = true;
                    bDownBoxSelected = false;
                    bGameBoxSelected = false;
                }
            }

            if (lowerBox.Contains(EditorCursorPos))
            {
                if (buttonPressUtility.isMousePressed(Mouse.GetState().LeftButton))
                {
                    bRightBoxSelected = false;
                    bDownBoxSelected = true;
                    bGameBoxSelected = false;
                }
            }

            if (gameBox.Contains(EditorCursorPos))
            {
                if (buttonPressUtility.isMousePressed(Mouse.GetState().LeftButton))
                {
                    bRightBoxSelected = false;
                    bDownBoxSelected = false;
                    bGameBoxSelected = true;
                }
            }

            if (bRightBoxSelected)
            {
                EditorCursorPos.Y += yTranslateRightBox;
            }
            else if (bDownBoxSelected)
            {
                EditorCursorPos.Y += yTranslateDownBox;
            }
            else if (bGameBoxSelected)
            {

            }
        }

        private void MoveRightBox()
        {
            if (buttonPressUtility.isPressedSub(Game1.moveDownString))
            {
                if (yTranslateRightBox + yTranslateRightStep < yTranslateRightBoxMax)
                {
                    yTranslateRightBox += yTranslateRightStep;
                }
            }

            if (buttonPressUtility.isPressedSub(Game1.moveUpString))
            {
                if (yTranslateRightBox - yTranslateRightStep > yTranslateRightBoxMin)
                {
                    yTranslateRightBox -= yTranslateRightStep;
                }
            }
        }

        private void ZoomGameBox()
        {
            //Up Arrow Key Control
            if ((KeyboardMouseUtility.mouseScrollValue > 0) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (zoomScale < zoomScaleMax)
                {
                    zoomScale += zoomScaleStep;
                }
            }
            //Down Arrow Key Control
            if ((KeyboardMouseUtility.mouseScrollValue < 0) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (zoomScale > zoomScaleMin)
                {
                    zoomScale -= zoomScaleStep;
                }
            }

        }

        public override void UnloadContent(Game1 game)
        {

        }

        public void Start(Game game)
        {
            activeSceneObjects = scenes[SceneUtility.prevScene].activeSceneObjects;
            activeSceneButtonCollections = scenes[SceneUtility.prevScene].activeSceneButtonCollections;
            activeSceneShapeCollections = scenes[SceneUtility.prevScene].activeSceneShapeCollections;
            activeSceneCharactersCollections = scenes[SceneUtility.prevScene].activeSceneCharactersCollections;

            xAxis = scenes[SceneUtility.prevScene].xAxis;
            yAxis = scenes[SceneUtility.prevScene].yAxis;

            EditorScale = EditorFormat / new Vector2(1366, 768);
            lowerBox = new Rectangle(0, (int)EditorFormat.Y, (int)EditorFormat.X, 768 - (int)EditorFormat.Y);
            rightBox = new Rectangle((int)EditorFormat.X, 0, 1366 - (int)EditorFormat.X, 768);
            gameBox = new Rectangle(0, 0, (int)EditorFormat.X, (int)EditorFormat.Y);
            SceneUtility.EditorTransformTranslation = EditorScale;
            if (EditorBG == null)
            {
                EditorBG = game.Content.Load<Texture2D>(@"Editor\EditorBG");
            }
            game.IsMouseVisible = true;

            AnalyzeActiveObjectList();
        }

        public void ResetBackToGame(Game game)
        {
            SceneUtility.EditorTransformScale = new Vector2(1);
            SceneUtility.EditorTransformTranslation = new Vector2(0);
            yTranslateDownBox = 0;
            yTranslateRightBox = 0;
            bRightBoxSelected = false;
            bDownBoxSelected = false;
            bGameBoxSelected = false;
            zoomScale = 1f;
            EditorGameWindowPos = new Vector2(0);
            game.IsMouseVisible = false;
            activeSceneObjects.Clear();
            types.Clear();
            typeNames.Clear();
            bUpdateGameViewPort = false;
        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            Game1.graphics.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
            scenes[SceneUtility.prevScene].Draw(gametime, spriteBatch);
            spriteBatch.Draw(Game1.hitboxHelp, new Rectangle((int)CursorUtility.trueCursorPos.X, (int)CursorUtility.trueCursorPos.Y, 16, 16), Color.Red);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(EditorBG, rightBox, EditorBG.Bounds, Color.IndianRed);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(EditorBG, lowerBox, EditorBG.Bounds, Color.Green);
            spriteBatch.Draw(Game1.hitboxHelp, new Rectangle((int)EditorCursorPos.X, (int)EditorCursorPos.Y, 16, 16), Color.PapayaWhip);

            spriteBatch.End();

            DrawRightEditorBox(spriteBatch);
            DrawDownEditorBox(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
            return spriteBatch;
        }

        private void DrawDownEditorBox(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, transformDownBox);

            if ((50 - yTranslateDownBox) > 0)
            {
                spriteBatch.DrawString(Game1.defaultFont, "Down Box", new Vector2(lowerBox.X + 50, lowerBox.Y + 50), Color.Black);
            }

            foreach (var button in downTabButtons)
            {
                button.Draw(spriteBatch);
            }


            spriteBatch.DrawString(Game1.defaultFont, bUpdateGameViewPort.ToString(), new Vector2(lowerBox.X + 400, lowerBox.Y + 100), Color.Black);


            spriteBatch.End();
        }

        private void DrawRightEditorBox(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, transformRightBox);

            spriteBatch.DrawString(Game1.defaultFont, "Right Box", new Vector2(rightBox.X + 50, rightBox.Y + 50), Color.Black);

            if (activeSceneObjects.Count == 0)
            {
                spriteBatch.DrawString(Game1.defaultFont, "You haven't initialized any\nactive Scene objects for the current scene.", new Vector2(rightBox.X + 50, rightBox.Y + 100), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(Game1.defaultFont, "Found " + types.Count + " type(s) in activeSceneObjects list:", new Vector2(rightBox.X + 50, rightBox.Y + 100), Color.Black);
                int i = 0;
                foreach (var typeName in typeNames)
                {
                    spriteBatch.DrawString(Game1.defaultFont, "-" + typeName, new Vector2(rightBox.X + 50, rightBox.Y + 130 + i * 50), Color.Black);
                    i++;
                }

                spriteBatch.DrawString(Game1.defaultFont, "Found Active object lists:", new Vector2(rightBox.X + 50, rightBox.Y + 120 + i * 50 + 75), Color.Black);

                int j = 0;
                if (activeSceneButtonCollections.Count != 0)
                {
                    spriteBatch.DrawString(Game1.defaultFont, "BUTTONS:", new Vector2(rightBox.X + 50, rightBox.Y + 230 + i * 50 + j * 50), Color.Black);
                    j++;
                }
                foreach (var list in activeSceneButtonCollections)
                {
                    spriteBatch.DrawString(Game1.defaultFont, "-" + list.name, new Vector2(rightBox.X + 50, rightBox.Y + 230 + i * 50 + j * 50), Color.Black);
                    j++;
                }

                if (activeSceneCharactersCollections.Count != 0)
                {
                    spriteBatch.DrawString(Game1.defaultFont, "CHARACTERS:", new Vector2(rightBox.X + 50, rightBox.Y + 230 + i * 50 + j * 50), Color.Black);
                    j++;
                }
                foreach (var list in activeSceneCharactersCollections)
                {
                    spriteBatch.DrawString(Game1.defaultFont, "-" + list.name, new Vector2(rightBox.X + 50, rightBox.Y + 230 + i * 50 + j * 50), Color.Black);
                    j++;
                }

                if (activeSceneShapeCollections.Count != 0)
                {
                    spriteBatch.DrawString(Game1.defaultFont, "BASIC SHAPES:", new Vector2(rightBox.X + 50, rightBox.Y + 230 + i * 50 + j * 50), Color.Black);
                    j++;
                }
                foreach (var list in activeSceneShapeCollections)
                {
                    spriteBatch.DrawString(Game1.defaultFont, "-" + list.name, new Vector2(rightBox.X + 50, rightBox.Y + 230 + i * 50 + j * 50), Color.Black);
                    j++;
                }
                /*
                int j = 0;
                foreach (var listname in namesOfButtonLists)
                {
                    spriteBatch.DrawString(Game1.defaultFont, "-"+listname, new Vector2(rightBox.X + 50, rightBox.Y + 230 + i * 50+j*50), Color.Black);
                    j++;
                }

                foreach (var listname in namesOfShapeLists)
                {
                    spriteBatch.DrawString(Game1.defaultFont, "-"+listname, new Vector2(rightBox.X + 50, rightBox.Y + 230 + i * 50 + j * 50), Color.Black);
                    j++;
                }*/


            }

            spriteBatch.End();
        }

        private void AnalyzeActiveObjectList()
        {
            foreach (var activeSceneObject in activeSceneObjects)
            {
                bool bHasType = false;
                foreach (var type in types)
                {
                    if (activeSceneObject.GetType().Equals(type))
                    {
                        bHasType = true;
                    }
                }

                if (!bHasType)
                {
                    types.Add(activeSceneObject.GetType());
                }

            }

            Console.Out.WriteLine("Found " + types.Count + " type(s) in activeSceneObjects list");

            foreach (var type in types)
            {
                typeNames.Add(type.ToString().Substring(type.ToString().LastIndexOf('.') + 1));
            }
        }
    }
}
