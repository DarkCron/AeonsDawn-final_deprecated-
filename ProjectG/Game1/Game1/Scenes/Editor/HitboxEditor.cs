using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    static public class HitboxEditor
    {
        static internal int hitboxWidth = 64;
        static internal int hitboxHeight = 64;
        static internal Point cameraPosition = new Point(0, 0);
        static RenderTarget2D editorRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static Matrix cameraMatrix;
        static List<Rectangle> hitboxList;
        static ShapeAnimation sa;
        static int saIndex = 0;
        static public bool bIsRunning = false;
        static List<Rectangle> onScreenBoxes = new List<Rectangle>();
        static internal Rectangle drawArea;
        static internal int widthHB = 4;
        static internal int heightHB = 4;

        static public void Start(Object obj, int width = 64, int height = 64)
        {
            widthHB = 4;
            heightHB = 4;
            MapBuilder.controls.currentType = Scenes.Editor.MapEditorSub.MapEditorControls.controlType.HitboxEditor;
            bIsRunning = true;
            hitboxWidth = width;
            hitboxHeight = height;
            saIndex = 0;
            scale = 8 - (width / 64 + 1);
            drawArea = new Rectangle(0, 0, width * scale, height * scale);
            cameraPosition = Point.Zero;
            if (obj is BaseSprite)
            {
                if ((obj as BaseSprite).shapeHitBox.Count == 0)
                {
                    (obj as BaseSprite).shapeHitBox.Add(new List<Rectangle>());
                }
                hitboxList = (obj as BaseSprite).shapeHitBox[0];

                if (obj is BaseCharacter)
                {
                    sa = (obj as BaseCharacter).charAnimations[0];
                }
                else if (obj is BaseSprite && (obj as BaseSprite).baseAnimations.Count != 0)
                {
                    sa = (obj as BaseSprite).baseAnimations[0].Clone();
                }
                else if (obj is BaseSprite && (obj as BaseSprite).baseAnimations.Count == 0)
                {
                   (obj as BaseSprite).baseAnimations.Add(new ShapeAnimation());
                    sa = (obj as BaseSprite).baseAnimations[0].Clone();
                }
            }
            else if (obj is NPC)
            {
                hitboxList = (obj as NPC).baseCharacter.shapeHitBox[0];
                sa = (obj as NPC).baseCharacter.charAnimations[0].Clone();
            }
            else if (obj is TileSource)
            {
                hitboxList = (obj as TileSource).tileHitBoxes;
                sa = (obj as TileSource).tileAnimation;
            }



            onScreenBoxes.Clear();
            foreach (var item in hitboxList)
            {
                onScreenBoxes.Add(new Rectangle(item.X * scale, item.Y * scale, item.Width * scale, item.Height * scale));
            }
        }

        static public void Update()
        {

            if (hitboxList.Count != onScreenBoxes.Count)
            {
                onScreenBoxes.Clear();
                foreach (var item in hitboxList)
                {
                    onScreenBoxes.Add(new Rectangle(item.X * scale, item.Y * scale, item.Width * scale, item.Height * scale));
                }

            }

            cameraMatrix = Matrix.CreateTranslation(cameraPosition.X, cameraPosition.Y, 0);
        }

        static internal void LMBFunction()
        {

            Vector2 trueMousePos = Mouse.GetState().Position.ToVector2();
            trueMousePos -= cameraPosition.ToVector2();

            if (drawArea.Contains(trueMousePos))
            {
                if (onScreenBoxes.Find(r => r.Contains(trueMousePos)) == default(Rectangle))
                {

                    Rectangle r = new Rectangle((int)trueMousePos.X / scale, (int)trueMousePos.Y / scale, widthHB, heightHB);
                    if (r.X + r.Width <= hitboxWidth && r.Y + r.Height <= hitboxHeight)
                    {
                        hitboxList.Add(r);
                    }


                }
            }
        }

        static internal void RMBFunction()
        {
            Vector2 trueMousePos = Mouse.GetState().Position.ToVector2();
            trueMousePos -= cameraPosition.ToVector2();

            if (onScreenBoxes.Find(r => r.Contains(trueMousePos)) != default(Rectangle))
            {
                var r2 = onScreenBoxes.Find(r => r.Contains(trueMousePos));
                var r3 = new Rectangle(r2.X / scale, r2.Y / scale, r2.Width / scale, r2.Height / scale);
                hitboxList.Remove(r3);
                onScreenBoxes.Remove(r2);
            }
        }

        static int scale = 8;
        static public void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cameraMatrix);


            sa.Draw(sb, new Rectangle(0, 0, hitboxWidth * scale, hitboxHeight * scale));
            onScreenBoxes.ForEach(r => sb.Draw(Game1.hitboxHelp, r, Color.White));

            Vector2 trueMousePos = Mouse.GetState().Position.ToVector2();
            trueMousePos -= cameraPosition.ToVector2();
            sb.Draw(Game1.hitboxHelp, new Rectangle((int)trueMousePos.X, (int)trueMousePos.Y, widthHB * scale, heightHB * scale), Game1.hitboxHelp.Bounds, Color.White);
            if (hitboxHeight > 64)
            {
                sb.Draw(Game1.hitboxHelp, new Rectangle(0, hitboxHeight * scale - 64 * scale, hitboxWidth * scale, (64-0) * scale), Color.Red);
            }

            sb.End();
            //bIsRunning = false;
        }
    }
}
