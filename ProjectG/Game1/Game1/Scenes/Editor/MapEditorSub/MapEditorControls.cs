using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;

namespace TBAGW.Scenes.Editor.MapEditorSub
{
    class MapEditorControls
    {
        MapBuilder refMB;
        internal enum controlType { None, TileLayer, ObjectLayerEditing, ObjectLayerAdding, HitboxEditor, TileSelect }
        internal enum objectLayerType { normal, ObjectGroup, MultiSelect }
        internal enum tileLayerType { normal, smartBrush }
        internal objectLayerType currentObjectEditType = objectLayerType.normal;
        internal controlType currentType = controlType.None;
        internal tileLayerType tileType = tileLayerType.normal;
        internal bool bIgnore = false;
        internal controlType previousType = controlType.None;
        

        internal delegate void tileSelectFunction(BasicTile bt);
        tileSelectFunction tsf = null;

        internal void startTileSelect(tileSelectFunction tsf)
        {
            previousType = currentType;
            currentType = controlType.TileSelect;
            this.tsf = tsf;
        }

        internal void Assign(MapBuilder mb)
        {
            refMB = mb;
        }

        internal void Update(GameTime gt, Vector2 EditorCursorPos)
        {

            if (!MapBuilder.mainG.IsActive)
            {
                goto end;
            }
            if (bIgnore && currentType != controlType.HitboxEditor)
            {
                goto end;
            }

            switch (currentType)
            {
                case controlType.HitboxEditor:
                    break;
                default:
                    GlobalControls(gt, EditorCursorPos);
                    break;
            }
            switch (currentType)
            {
                case controlType.None:
                    NoSpecificControls(gt, EditorCursorPos);
                    break;
                case controlType.TileLayer:
                    DrawTileLayerControls(gt, EditorCursorPos);
                    break;
                case controlType.ObjectLayerEditing:
                    ObjectLayerControls(gt, EditorCursorPos);
                    break;
                case controlType.ObjectLayerAdding:
                    ObjectLayerAddingControls(gt, EditorCursorPos);
                    break;
                case controlType.HitboxEditor:
                    HitboxEditorControls(gt, EditorCursorPos);
                    break;
                case controlType.TileSelect:
                    TileSelectControls(gt, EditorCursorPos);
                    break;
                default:
                    break;
            }

        end: { }
        }

        private void TileSelectControls(GameTime gt, Vector2 editorCursorPos)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                var tempList = MapBuilder.loadedMap.tilesOnPositionClick(editorCursorPos);
                if (tempList.Count != 0)
                {
                    if (tempList.Count == 1 && tempList[0] != null)
                    {
                        tsf(tempList[0]);
                    }
                    else
                    {
                        Console.WriteLine("Check tile select controls");
                    }
                }
            }
        }

        private void HitboxEditorControls(GameTime gt, Vector2 editorCursorPos)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                HitboxEditor.cameraPosition.Y -= 3;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                HitboxEditor.cameraPosition.Y += 3;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                HitboxEditor.cameraPosition.X += 3;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                HitboxEditor.cameraPosition.X -= 3;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && HitboxEditor.heightHB != HitboxEditor.hitboxHeight)
            {
                HitboxEditor.heightHB++;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && HitboxEditor.heightHB > 1)
            {
                HitboxEditor.heightHB--;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) && HitboxEditor.widthHB > 1)
            {
                HitboxEditor.widthHB--;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && HitboxEditor.widthHB != HitboxEditor.hitboxWidth)
            {
                HitboxEditor.widthHB++;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                HitboxEditor.bIsRunning = false;
                KeyboardMouseUtility.bPressed = true;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && MapBuilder.mainG.IsActive)
            {
                HitboxEditor.LMBFunction();
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed && MapBuilder.mainG.IsActive)
            {
                HitboxEditor.RMBFunction();
            }
        }

        private void NoSpecificControls(GameTime gt, Vector2 editorCursorPos)
        {
            if (currentType == controlType.None && Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                var tempList = MapBuilder.loadedMap.allObjectsAtLocation(editorCursorPos);
                if (tempList.Count == 1)
                {
                    currentType = controlType.ObjectLayerEditing;
                    MapBuilder.objSelection = new Utilities.Sprite.EditorObjectSelection();
                    MapBuilder.objSelection.Assign(tempList.First());

                }
                else if (tempList.Count > 1)
                {
                    int index = tempList.IndexOf(MapBuilder.objSelection.selectedObject);
                    index++;
                    if (index >= tempList.Count)
                    {
                        index = 0;
                    }

                    currentType = controlType.ObjectLayerEditing;
                    MapBuilder.objSelection = new Utilities.Sprite.EditorObjectSelection();
                    MapBuilder.objSelection.Assign(tempList[index]);
                }
            }
        }

        private void ObjectLayerAddingControls(GameTime gt, Vector2 editorCursorPos)
        {
            if (MapBuilder.objAddition != null && MapBuilder.objAddition.IsCorrect())
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    KeyboardMouseUtility.bMouseButtonPressed = true;
                    MapBuilder.objAddition.HandleCopyAtMousePos(editorCursorPos);
                }

                if (!KeyboardMouseUtility.AnyButtonsPressed() && KeyboardMouseUtility.ScrollingUp())
                {
                    MapBuilder.objAddition.Scale(0.05f);
                }

                if (!KeyboardMouseUtility.AnyButtonsPressed() && KeyboardMouseUtility.ScrollingDown())
                {
                    MapBuilder.objAddition.Scale(-0.05f);
                }
            }

        }

        internal int objectMoveSpeed = 1;
        private void ObjectLayerControls(GameTime gt, Vector2 editorCursorPos)
        {
            //Console.WriteLine(editorCursorPos);

            if (MapBuilder.objSelection != null && MapBuilder.objSelection.IsCorrect() && currentObjectEditType != objectLayerType.MultiSelect)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !KeyboardMouseUtility.AnyButtonsPressed() && currentObjectEditType != objectLayerType.ObjectGroup)
                {
                    MapBuilder.objSelection.OpenEditWindow();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    MapBuilder.objSelection.Move(new Vector2(0, -1));
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    MapBuilder.objSelection.Move(new Vector2(0, 1));
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    MapBuilder.objSelection.Move(new Vector2(-1, 0));
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    MapBuilder.objSelection.Move(new Vector2(1, 0));
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    MapBuilder.objSelection.Move(new Vector2(0, -objectMoveSpeed));
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    MapBuilder.objSelection.Move(new Vector2(0, objectMoveSpeed));
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    MapBuilder.objSelection.Move(new Vector2(-objectMoveSpeed, 0));
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    MapBuilder.objSelection.Move(new Vector2(objectMoveSpeed, 0));
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Q) && !KeyboardMouseUtility.AnyButtonsPressed() && currentObjectEditType != objectLayerType.ObjectGroup)
                {
                    MapBuilder.objSelection.HandleCopyAtMousePos(editorCursorPos);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    MapBuilder.objSelection.HandleMoveToMousePos(editorCursorPos);
                }


                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && KeyboardMouseUtility.ScrollingUp())
                {
                    objectMoveSpeed++;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && KeyboardMouseUtility.ScrollingDown() && objectMoveSpeed > 1)
                {
                    objectMoveSpeed--;
                }

            }

            //if (MapBuilder.objSelection != null && MapBuilder.objSelection.IsCorrect() && MapBuilder.objSelection.selectedType == typeof(ObjectGroup))
            //{
            //    if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed() && currentObjectEditType != objectLayerType.ObjectGroup)
            //    {
            //        currentObjectEditType = objectLayerType.ObjectGroup;
            //        KeyboardMouseUtility.bPressed = true;
            //        MapBuilder.objSelection.extraArg = MapBuilder.objSelection.selectedObject;
            //    }
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed() && currentObjectEditType == objectLayerType.ObjectGroup)
            //{
            //    currentObjectEditType = objectLayerType.normal;
            //    KeyboardMouseUtility.bPressed = true;
            //}

            if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                //MapBuilder.objSelection = new Utilities.Sprite.EditorObjectSelection();
                KeyboardMouseUtility.bPressed = true;
                currentObjectEditType++;
                if ((int)currentObjectEditType >= Enum.GetNames(typeof(objectLayerType)).Length)
                {
                    currentObjectEditType = 0;
                }

                switch (currentObjectEditType)
                {
                    case objectLayerType.normal:
                        break;
                    case objectLayerType.ObjectGroup:
                        MapBuilder.objSelection.extraArg = MapBuilder.objSelection.selectedObject;
                        if (MapBuilder.objSelection.extraArg == null)
                        {
                            currentObjectEditType++;
                        }
                        break;
                    case objectLayerType.MultiSelect:
                        break;

                    default:
                        break;
                }
               // currentObjectEditType = objectLayerType.normal;
                KeyboardMouseUtility.bPressed = true;
            }

            switch (currentObjectEditType)
            {
                case objectLayerType.normal:
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        var tempList = MapBuilder.loadedMap.allObjectsAtLocation(editorCursorPos);
                        if (tempList.Count == 1)
                        {
                            MapBuilder.objSelection = new Utilities.Sprite.EditorObjectSelection();
                            MapBuilder.objSelection.Assign(tempList.First());
                            Form1.MakeSureFormClosed(MapBuilder.oiw);
                        }
                        else if (tempList.Count > 1)
                        {
                            int index = tempList.IndexOf(MapBuilder.objSelection.selectedObject);
                            index++;
                            if (index >= tempList.Count)
                            {
                                index = 0;
                            }

                            MapBuilder.objSelection = new Utilities.Sprite.EditorObjectSelection();
                            MapBuilder.objSelection.Assign(tempList[index]);
                            Form1.MakeSureFormClosed(MapBuilder.oiw);
                        }
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && KeyboardMouseUtility.ScrollingUp())
                    {
                        MapBuilder.objSelection.Scale(0.05f);
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && KeyboardMouseUtility.ScrollingDown())
                    {
                        MapBuilder.objSelection.Scale(-0.05f);
                    }

                    break;
                case objectLayerType.ObjectGroup:
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed() && (MapBuilder.objSelection.extraArg is ObjectGroup) && MapBuilder.objSelection.extraArg!=null)
                    {
                        var tempList = (MapBuilder.objSelection.extraArg as ObjectGroup).groupItems.Cast<Object>().ToList();
                        if (tempList.Count == 1)
                        {
                            MapBuilder.objSelection.Assign(tempList.First());
                            Form1.MakeSureFormClosed(MapBuilder.oiw);
                        }
                        else if (tempList.Count > 1)
                        {
                            int index = tempList.IndexOf(MapBuilder.objSelection.selectedObject);
                            index++;
                            if (index >= tempList.Count)
                            {
                                index = 0;
                            }

                            MapBuilder.objSelection.Assign(tempList[index]);
                            Form1.MakeSureFormClosed(MapBuilder.oiw);
                        }
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && KeyboardMouseUtility.ScrollingUp())
                    {
                        MapBuilder.objSelection.Scale(0.05f);
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && KeyboardMouseUtility.ScrollingDown())
                    {
                        MapBuilder.objSelection.Scale(-0.05f);
                    }

                    break;
                case objectLayerType.MultiSelect:
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        var tempList = MapBuilder.loadedMap.allObjectsAtLocation(editorCursorPos);
                        if (tempList.Count == 1)
                        {
                            // MapBuilder.objSelection = new Utilities.Sprite.EditorObjectSelection();
                            MapBuilder.objSelection.Assign(tempList.First());
                            Form1.MakeSureFormClosed(MapBuilder.oiw);
                        }
                        else if (tempList.Count > 1)
                        {
                            int index = tempList.IndexOf(MapBuilder.objSelection.selectedObject);
                            index++;
                            if (index >= tempList.Count)
                            {
                                index = 0;
                            }

                            // MapBuilder.objSelection = new Utilities.Sprite.EditorObjectSelection();
                            MapBuilder.objSelection.Assign(tempList[index]);
                            Form1.MakeSureFormClosed(MapBuilder.oiw);
                        }
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.O))
                    {
                        MapBuilder.objSelection.CreateObjectGroupFromMultiSelect();
                    }
                    break;
                default:
                    break;
            }

        }

        private void DrawTileLayerControls(GameTime gt, Vector2 cp)
        {
            if (MapBuilder.paintTile != default(BasicTile) && MapBuilder.bAllowEditing)
            {
                refMB.PaintLogic(cp);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                switch (tileType)
                {
                    case tileLayerType.normal:
                        tileType = tileLayerType.smartBrush;
                        break;
                    case tileLayerType.smartBrush:
                        tileType = tileLayerType.normal;
                        break;
                }
            }

            switch (tileType)
            {
                case tileLayerType.normal:
                    if (MapBuilder.paintTile != default(BasicTile))
                    {

                        if (Keyboard.GetState().IsKeyDown(Keys.F) && MapBuilder.paintTile.HasDifferentSizeSource() && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            if (MapBuilder.paintTile.drawSize == new Point(64))
                            {
                                MapBuilder.paintTile.drawSize = MapBuilder.paintTile.tileSource.tileAnimation.AnimSourceSize();
                            }
                            else
                            {
                                MapBuilder.paintTile.drawSize = new Point(64);
                            }
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Space) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            MapBuilder.paintTile.offset = Point.Zero;
                        }

                        var t = MapBuilder.paintTile.offset;
                        var tSource = MapBuilder.paintTile;
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Up) && t.Y > -tSource.drawSize.Y  && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                t.Y--;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Down) && t.Y < 64 && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                t.Y++;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Left) && t.X > -tSource.drawSize.X && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                t.X--;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Right) && t.X < 64 && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                t.X++;
                            }
                        }
                        else if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Up) && t.Y > -tSource.drawSize.Y )
                            {
                                t.Y--;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Down) && t.Y < 64)
                            {
                                t.Y++;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Left) && t.X > -tSource.drawSize.X )
                            {
                                t.X--;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Right) && t.X < 64)
                            {
                                t.X++;
                            }
                        }

                        MapBuilder.paintTile.offset = t;
                    }

                    break;
                case tileLayerType.smartBrush:
                    if (Keyboard.GetState().IsKeyDown(Keys.L) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        MapBuilder.memoryBrush.bLearnFromAllLayers = !MapBuilder.memoryBrush.bLearnFromAllLayers;
                    }
                    break;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                MapBuilder.paintTile.rotation++;
                if ((int)MapBuilder.paintTile.rotation >= Enum.GetNames(typeof(BasicTile.Rotation)).Length)
                {
                    MapBuilder.paintTile.rotation = 0;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Add) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                MapBuilder.brushSize++;
                MapBuilder.CalculateBrushSize();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Subtract) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (MapBuilder.brushSize > 1)
                {
                    MapBuilder.brushSize--;
                    MapBuilder.CalculateBrushSize();
                }
            }
        }

        private void GlobalControls(GameTime gt, Vector2 EditorCursorPos)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad7) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && GameScreenEffect.worldBrightness < 1)
            {
                GameScreenEffect.worldBrightness += 0.01f;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && GameScreenEffect.worldBrightness > 0)
            {
                GameScreenEffect.worldBrightness -= 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.S) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                Form1.MakeSureFormClosed(Form1.songEncrypt);
                Form1.songEncrypt = new Forms.Sound.SongEncrypt();
                Form1.songEncrypt.Show();
                KeyboardMouseUtility.bPressed = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                MapBuilder.cameraPosY -= (int)(MapBuilder.cameraSpeed / MapBuilder.cameraZoomX);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                MapBuilder.cameraPosY += (int)(MapBuilder.cameraSpeed / MapBuilder.cameraZoomX);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                MapBuilder.cameraPosX -= (int)(MapBuilder.cameraSpeed / MapBuilder.cameraZoomX);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                MapBuilder.cameraPosX += (int)(MapBuilder.cameraSpeed / MapBuilder.cameraZoomX);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && MapBuilder.cameraZoomX > .2f)
            {
                MapBuilder.cameraZoomX -= 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.C) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                MapBuilder.cameraZoomX += 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                MapBuilder.cameraZoomX = 1f;
            }

            if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.Tab) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                //if (currentType == controlType.ObjectLayerEditing)
                //{
                //    currentType = controlType.None;
                //}
                //else
                //{
                //    currentType = controlType.ObjectLayerEditing;
                //}

            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.Tab) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                MapBuilder.testLight = !MapBuilder.testLight;
                MapBuilder.TurnLightsOn();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.O) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (MapBuilder.functionForm.Visible)
                {
                    MapBuilder.functionForm.Hide();
                }
                else
                {
                    MapBuilder.functionForm.Show();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !KeyboardMouseUtility.AnyButtonsPressed() && MapBuilder.mainG.IsActive)
            {
                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("You sure want to exit Map editing? NOTE: be sure to save first!", "Leaving", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    MapEditor.bIsRunning = true;
                    MapBuilder.functionForm.Close();
                }
                else if (dialogResult == System.Windows.Forms.DialogResult.No)
                {
                    //do something else
                }

            }


        }
    }
}
