using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.Input
{
    static class CursorUtility
    {
        public static Vector2 cursorPos;
        static String cursorTextureString = @"Graphics\Cursor\CursorV1";
        static Rectangle[] cursorTexture = new Rectangle[2];
        static Texture2D cursorTextureSheet;
        public static Cursor cursorShape;
        public static Vector2 trueCursorPos;
        public static Vector2 GUICursorPos;

        static public void Initialize(Game game)
        {
            cursorTextureSheet = game.Content.Load<Texture2D>(cursorTextureString);
            cursorTexture[0] = new Rectangle(0,0,64,64);
            cursorTexture[1] = new Rectangle(64, 0, 64, 64);
            cursorShape = new Cursor(cursorTextureSheet,1,cursorPos,true,default(Vector2),"Cursor",cursorTexture[0]);
            cursorShape.rectangleToDraw=cursorTexture[1];
        }        

        static public void update(GameTime gameTime)
        {
       //     GUICursorPos = ResolutionUtility.mousePos / 1 - (new Vector2(0, 32)) * 1;
       //     cursorPos = ResolutionUtility.mousePos/1 -(new Vector2(0,32)+new Vector2(SceneUtility.xAxis,SceneUtility.yAxis))*1;
        
            trueCursorPos = new Vector2(CursorUtility.cursorShape.shapeHitBox[0].Location.X, CursorUtility.cursorShape.shapeHitBox[0].Location.Y + CursorUtility.cursorShape.shapeHitBox[0].Height);
            GUICursorPos = ResolutionUtility.AdjustMousePosition(Game1.graphics, Mouse.GetState().Position.ToVector2()) ;
            cursorPos = ResolutionUtility.AdjustMousePosition(Game1.graphics, Mouse.GetState().Position.ToVector2()) - (new Vector2(0, 32) + new Vector2(SceneUtility.xAxis, SceneUtility.yAxis)) * 1;

            //This is for Editor ViewPort stuff
            trueCursorPos -= SceneUtility.EditorTransformTranslation;
            trueCursorPos /= SceneUtility.EditorTransformScale;

            cursorShape.position = ResolutionUtility.AdjustMousePosition(Game1.graphics, Mouse.GetState().Position.ToVector2()) - (new Vector2(0, 64)) - new Vector2(SceneUtility.xAxis, SceneUtility.yAxis);

            cursorShape.Update(gameTime);
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            cursorShape.Draw(spriteBatch);
        }
    }
}
