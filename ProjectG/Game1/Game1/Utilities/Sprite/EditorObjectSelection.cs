using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;

namespace TBAGW.Utilities.Sprite
{
    public class EditorObjectSelection
    {
        public object selectedObject;
        public Type selectedType;
        public object extraArg;

        public void Assign(object refObj)
        {
            if (MapBuilder.controls.currentObjectEditType == Scenes.Editor.MapEditorSub.MapEditorControls.objectLayerType.MultiSelect)
            {
                HandleMultiSelect(refObj);
            }
            else if (MapBuilder.controls.currentObjectEditType != Scenes.Editor.MapEditorSub.MapEditorControls.objectLayerType.MultiSelect)
            {



                if (refObj != null)
                {
                    if (refObj.GetType() == typeof(BaseSprite))
                    {
                        if (MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerAdding)
                        {
                            selectedObject = (refObj as BaseSprite).ShallowCopy();
                        }
                        else
                        {
                            selectedObject = refObj;
                        }
                        selectedType = typeof(BaseSprite);
                    }
                    else if (refObj.GetType() == typeof(ObjectGroup))
                    {
                        if (MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerAdding)
                        {
                            selectedObject = (refObj as ObjectGroup).ShallowCopy();
                        }
                        else
                        {
                            selectedObject = refObj;
                        }
                        selectedType = typeof(ObjectGroup);
                    }
                    else if (refObj.GetType() == typeof(SpriteLight))
                    {
                        if (MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerAdding)
                        {
                            selectedObject = (refObj as SpriteLight).Clone();
                        }
                        else
                        {
                            selectedObject = refObj;
                        }
                        selectedType = typeof(SpriteLight);
                    }
                    else if (refObj.GetType() == typeof(BaseCharacter))
                    {
                        if (MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerAdding)
                        {
                            selectedObject = (refObj as BaseCharacter).Clone();
                        }
                        else
                        {
                            selectedObject = refObj;
                        }
                        selectedType = typeof(BaseCharacter);
                        if ((selectedObject as BaseCharacter).spriteGameSize.Size == Point.Zero)
                        {
                            (selectedObject as BaseCharacter).rotationIndex = (int)BaseCharacter.Rotation.Down;
                            (selectedObject as BaseCharacter).spriteGameSize.Size = new Point(64);
                            (selectedObject as BaseCharacter).UpdatePosition();
                        }
                    }
                    else if (refObj.GetType() == typeof(NPC))
                    {


                        selectedObject = refObj;

                        selectedType = typeof(NPC);
                    }
                }
            }
        }

        private void HandleMultiSelect(object refObj)
        {
            if (selectedObject == null || selectedObject.GetType() != typeof(List<Object>))
            {
                selectedObject = new List<Object>();
            }



            if (refObj != null)
            {
                if (refObj.GetType() == typeof(BaseSprite))
                {
                    if (!(selectedObject as List<Object>).Contains(refObj))
                    {
                        (selectedObject as List<Object>).Add(refObj);
                    }
                }
                else if (refObj.GetType() == typeof(ObjectGroup))
                {
                    foreach (var item in (refObj as ObjectGroup).groupItems)
                    {
                        if (!(selectedObject as List<Object>).Contains(item))
                        {
                            (selectedObject as List<Object>).Add(item);
                        }
                    }
                }
                else if (refObj.GetType() == typeof(SpriteLight))
                {
                    if (!(selectedObject as List<Object>).Contains(refObj))
                    {
                        (selectedObject as List<Object>).Add(refObj);
                    }
                }
            }
        }

        public void Move(Vector2 movement)
        {
            if (selectedObject.GetType() == typeof(BaseSprite))
            {
                switch (MapBuilder.controls.currentObjectEditType)
                {
                    case Scenes.Editor.MapEditorSub.MapEditorControls.objectLayerType.normal:
                        var temp = (selectedObject as BaseSprite);
                        temp.position += movement;
                        temp.UpdatePosition();
                        break;
                    case Scenes.Editor.MapEditorSub.MapEditorControls.objectLayerType.ObjectGroup:
                        if ((extraArg as ObjectGroup).groupItems.IndexOf(selectedObject as BaseSprite) != 0)
                        {
                            (extraArg as ObjectGroup).relativeOffSet[(extraArg as ObjectGroup).groupItems.IndexOf(selectedObject as BaseSprite)] += movement;
                            (extraArg as ObjectGroup).groupItems[(extraArg as ObjectGroup).groupItems.IndexOf(selectedObject as BaseSprite)].position += movement;
                            (extraArg as ObjectGroup).groupItems[(extraArg as ObjectGroup).groupItems.IndexOf(selectedObject as BaseSprite)].UpdatePosition();
                            (extraArg as ObjectGroup).CalculateGroupSize();
                            (extraArg as ObjectGroup).bGenerateRender = true;
                        }
                        else if ((extraArg as ObjectGroup).groupItems.IndexOf(selectedObject as BaseSprite) == 0)
                        {
                            var tempOG = (extraArg as ObjectGroup);
                            var main = tempOG.mainController();
                            tempOG.Move(movement, main);
                            tempOG.UpdateMovement();
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (selectedObject.GetType() == typeof(ObjectGroup))
            {
                var temp = (selectedObject as ObjectGroup);
                var main = temp.mainController();
                temp.Move(movement, main);
                temp.UpdateMovement();
            }
            else if (selectedObject.GetType() == typeof(SpriteLight))
            {
                var temp = (selectedObject as BaseSprite);
                temp.position += movement;
                temp.UpdatePosition();
            }
        }

        internal void MarkedDraw(SpriteBatch sb)
        {
            if (MapBuilder.controls.currentObjectEditType != Scenes.Editor.MapEditorSub.MapEditorControls.objectLayerType.MultiSelect)
            {
                if (selectedObject.GetType() == typeof(BaseSprite))
                {
                    var temp = (selectedObject as BaseSprite);
                    temp.Draw(sb, Color.Red);
                }
                else if (selectedObject.GetType() == typeof(ObjectGroup))
                {
                    
                    var temp = (selectedObject as ObjectGroup);
                    temp.Draw(sb, Color.Red);
                }
                else if (selectedObject.GetType() == typeof(SpriteLight))
                {
                    var temp = (selectedObject as BaseSprite);
                    temp.Draw(sb, Color.Red);
                }
                else if (selectedObject.GetType() == typeof(BaseCharacter))
                {
                    var temp = (selectedObject as BaseCharacter);
                    temp.Draw(sb, Color.Red);
                }
                else if (selectedObject.GetType() == typeof(NPC))
                {
                    if (Game1.bIsDebug || false)
                    {
                        TestEnvironment.NPCTest(selectedObject);
                    }

                    var temp = (selectedObject as NPC).baseCharacter;
                    temp.Draw(sb, Color.Red);
                    foreach (var item in (selectedObject as NPC).npcCommands.FindAll(command => command.mapID == MapBuilder.loadedMap.identifier))
                    {
                        Vector2 offset = Vector2.Zero;
                        int index = (selectedObject as NPC).npcCommands.IndexOf(item);
                        for (int i = 0; i < index; i++)
                        {
                            if ((selectedObject as NPC).npcCommands[i].destination == item.destination)
                            {
                                offset += new Vector2(12, 20);
                            }
                        }
                        if (offset == Vector2.Zero)
                        {
                            sb.Draw(Game1.hitboxHelp, new Rectangle(item.destination.ToPoint(), new Point(64)), Color.Gold);
                        }
                        sb.DrawString(Game1.defaultFont, item.ToString(), item.destination + new Vector2(0, 12) + offset, Color.Gold);
                    }
                }
            }
            else if (MapBuilder.controls.currentObjectEditType == Scenes.Editor.MapEditorSub.MapEditorControls.objectLayerType.MultiSelect && selectedObject.GetType() == typeof(List<object>))
            {
                for (int i = 0; i < (selectedObject as List<Object>).Count; i++)
                {
                    if ((selectedObject as List<Object>)[i].GetType() == typeof(BaseSprite))
                    {
                        var temp = ((selectedObject as List<Object>)[i] as BaseSprite);
                        temp.Draw(sb, Color.Red);
                    }
                    else if ((selectedObject as List<Object>)[i].GetType() == typeof(ObjectGroup))
                    {
                        var temp = ((selectedObject as List<Object>)[i] as ObjectGroup);
                        temp.Draw(sb, Color.Red);
                    }
                    else if ((selectedObject as List<Object>)[i].GetType() == typeof(SpriteLight))
                    {
                        var temp = ((selectedObject as List<Object>)[i] as BaseSprite);
                        temp.Draw(sb, Color.Red);
                    }
                }
            }

        }

        internal bool IsCorrect()
        {
            if (selectedObject != null)
            {
                return true;
            }
            return false;
        }

        internal void HandleCopyAtMousePos(Vector2 editorCursorPos)
        {
            if (selectedObject.GetType() == typeof(BaseSprite))
            {
                var temp = (selectedObject as BaseSprite).ShallowCopy();
                temp.position = editorCursorPos - (temp.spriteGameSize.Center.ToVector2() - temp.position);
                temp.UpdatePosition();
                MapBuilder.loadedMap.AddSpriteToMap(temp);
                if (MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing)
                {
                    this.Assign(temp);
                }

            }
            else if (selectedObject.GetType() == typeof(ObjectGroup))
            {
                var temp = (selectedObject as ObjectGroup).ShallowCopy();
                temp.PlaceGroup(editorCursorPos - (temp.mainController().spriteGameSize.Center.ToVector2() - temp.mainController().position));
                MapBuilder.loadedMap.AddObjectGroupToMap(temp);
                if (MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing)
                {
                    this.Assign(temp);
                }
            }
            else if (selectedObject.GetType() == typeof(SpriteLight))
            {
                var temp = (selectedObject as SpriteLight).Clone();
                temp.position = editorCursorPos - (temp.spriteGameSize.Center.ToVector2() - (selectedObject as SpriteLight).position);
                temp.UpdatePosition();
                MapBuilder.loadedMap.AddSpriteLightToMap(temp);
                if (MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing)
                {
                    this.Assign(temp);
                }
            }
            else if (selectedObject.GetType() == typeof(BaseCharacter))
            {
                var temp = (selectedObject as BaseCharacter).Clone();
                temp.position = editorCursorPos - (temp.spriteGameSize.Center.ToVector2() - (selectedObject as BaseCharacter).position);
                temp.rotationIndex = (int)BaseCharacter.Rotation.Down;
                temp.spriteGameSize.Size = new Point(64);
                temp.UpdatePosition();
                temp.UpdateHitBoxes();
                // temp.UpdatePosition();
                MapBuilder.loadedMap.AddSpriteToMap(temp);
                if (MapBuilder.controls.currentType == Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing)
                {
                    this.Assign(temp);
                }
            }
        }

        internal void HandleMoveToMousePos(Vector2 editorCursorPos)
        {
            if (selectedObject.GetType() == typeof(BaseSprite))
            {
                switch (MapBuilder.controls.currentType)
                {
                    case Scenes.Editor.MapEditorSub.MapEditorControls.controlType.None:
                        break;
                    case Scenes.Editor.MapEditorSub.MapEditorControls.controlType.TileLayer:
                        break;
                    case Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerEditing:
                        switch (MapBuilder.controls.currentObjectEditType)
                        {
                            case Scenes.Editor.MapEditorSub.MapEditorControls.objectLayerType.normal:
                                var temp2 = (selectedObject as BaseSprite);
                                temp2.position = editorCursorPos - (temp2.spriteGameSize.Center.ToVector2() - temp2.position);

                                temp2.UpdatePosition();
                                break;
                            case Scenes.Editor.MapEditorSub.MapEditorControls.objectLayerType.ObjectGroup:

                                break;
                            default:
                                break;
                        }
                        break;
                    case Scenes.Editor.MapEditorSub.MapEditorControls.controlType.ObjectLayerAdding:
                        var temp = (selectedObject as BaseSprite);
                        temp.position = editorCursorPos - (temp.spriteGameSize.Center.ToVector2() - temp.position);

                        temp.UpdatePosition();
                        break;
                    case Scenes.Editor.MapEditorSub.MapEditorControls.controlType.HitboxEditor:
                        break;
                    default:
                        break;
                }


            }
            else if (selectedObject.GetType() == typeof(ObjectGroup))
            {
                var temp = (selectedObject as ObjectGroup);
                temp.PlaceGroup(editorCursorPos - (temp.mainController().spriteGameSize.Center.ToVector2() - temp.mainController().position));

            }
            else if (selectedObject.GetType() == typeof(SpriteLight))
            {
                var temp = (selectedObject as SpriteLight);
                temp.position = editorCursorPos - (temp.trueMapSize().Center.ToVector2() - temp.position);

                temp.UpdatePosition();
            }
            else if (selectedObject.GetType() == typeof(BaseCharacter))
            {
                var temp = (selectedObject as BaseCharacter);
                temp.position = editorCursorPos - (temp.trueMapSize().Center.ToVector2() - temp.position);

                temp.UpdatePosition();
                temp.UpdateHitBoxes();
            }
        }

        internal void Scale(float deltaScale)
        {
            if (selectedObject != null)
            {
                if (selectedObject.GetType() == typeof(BaseSprite))
                {
                    var temp = selectedObject as BaseSprite;
                    if ((temp.scaleVector + new Vector2(deltaScale)).X > 0 && (temp.scaleVector + new Vector2(deltaScale)).Y > 0)
                    {
                        temp.AssignScaleVector(temp.scaleVector + new Vector2(deltaScale));
                    }
                }
                else if (selectedObject.GetType() == typeof(ObjectGroup))
                {
                    var temp = selectedObject as ObjectGroup;
                    if ((temp.scaleVector + new Vector2(deltaScale)).X > 0 && (temp.scaleVector + new Vector2(deltaScale)).Y > 0)
                    {
                        temp.AssignScaleVector(temp.scaleVector + new Vector2(deltaScale));
                    }
                }
                else if (selectedObject.GetType() == typeof(SpriteLight))
                {
                    var temp = selectedObject as SpriteLight;
                    if ((temp.scaleVector + new Vector2(deltaScale)).X > 0 && (temp.scaleVector + new Vector2(deltaScale)).Y > 0)
                    {
                        temp.AssignScaleVector(temp.scaleVector + new Vector2(deltaScale));
                    }
                }
                else if (selectedObject.GetType() == typeof(BaseCharacter))
                {
                    var temp = selectedObject as BaseCharacter;
                    if ((temp.scaleVector + new Vector2(deltaScale)).X > 0 && (temp.scaleVector + new Vector2(deltaScale)).Y > 0)
                    {
                        temp.AssignScaleVector(temp.scaleVector + new Vector2(deltaScale));
                    }
                }
            }
        }

        internal void CreateObjectGroupFromMultiSelect()
        {
            if (selectedObject != null && selectedObject.GetType() == typeof(List<object>) && (selectedObject as List<Object>).Count != 0)
            {
                ObjectGroup og = new ObjectGroup();
                foreach (var item in selectedObject as List<Object>)
                {
                    og.AddObject(item as BaseSprite);
                }
                og.GenerateBoundingRectangleForMapSize();

                MapBuilder.gcDB.AddObjectGroup(og);

                selectedObject = null;
            }


        }

        internal void OpenEditWindow()
        {
            Form1.MakeSureFormClosed(MapBuilder.oiw);
            Form1.MakeSureFormClosed(Form1.npcEditor);

            if (selectedObject.GetType() == typeof(BaseSprite))
            {
                Form1.MakeSureFormClosed(MapBuilder.oiw);
                MapBuilder.oiw = new Forms.GameObjects.ObjectInfoWindow();
                MapBuilder.oiw.Start(selectedObject);
            }
            else if (selectedObject.GetType() == typeof(ObjectGroup))
            {
                Form1.MakeSureFormClosed(MapBuilder.oiw);
                MapBuilder.oiw = new Forms.GameObjects.ObjectInfoWindow();
                MapBuilder.oiw.Start(selectedObject);
            }
            else if (selectedObject.GetType() == typeof(SpriteLight))
            {
                Form1.MakeSureFormClosed(MapBuilder.oiw);
                MapBuilder.oiw = new Forms.GameObjects.ObjectInfoWindow();
                MapBuilder.oiw.Start(selectedObject as BaseSprite);
            }
            else if (selectedObject.GetType() == typeof(BaseCharacter))
            {
                Form1.MakeSureFormClosed(MapBuilder.oiw);
                MapBuilder.oiw = new Forms.GameObjects.ObjectInfoWindow();
                MapBuilder.oiw.Start(selectedObject as BaseCharacter);
            }
            else if (selectedObject.GetType() == typeof(NPC))
            {
                Form1.MakeSureFormClosed(MapBuilder.oiw);
                Form1.MakeSureFormClosed(Form1.npcEditor);
                Form1.npcEditor = new Forms.NPC_Editor.NPC_Editor();
                Form1.npcEditor.Start(selectedObject as NPC);
            }
        }
    }
}
