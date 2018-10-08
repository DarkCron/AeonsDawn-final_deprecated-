using TBAGW.Scenes;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.GamePlay.Characters.Friendly.Team;
using TBAGW.Utilities.Sound.BG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities
{
    static class SceneUtility
    {
        public static List<Scene> scenes;

        public static int currentScene = (int)(Game1.Screens.MainMenu);
        public static int prevScene = (int)(Game1.Screens.MainMenu);

        public static int masterVolume = 80;
        public static int musicVolume = 80;
        public static int soundEffectsVolume = 80;

        public static bool bNarrativeMode = true;

        public static float GUIScale = 1.0f;
        public static Vector2 currentMapSize = new Vector2(64 * 100, 64 * 100);

        /// <summary>
        /// Only battle animations
        /// </summary>
        public static int animationSpeed = 1;

        /// <summary>
        /// Handles big scene draws
        /// </summary>
        public static float xAxis = 0;
        public static float yAxis = 0;
        public static Vector2 Axis = new Vector2(0);
        public static Matrix transform = Matrix.CreateScale(1, 1, 0) * Matrix.CreateTranslation(new Vector3(xAxis, yAxis, 0));
        public static Matrix GUITransform = Matrix.CreateScale(1, 1, 0);
        public static Vector2 EditorTransformScale = new Vector2(1);
        public static Vector2 EditorTransformTranslation = new Vector2(0);

        public static void UpdateTransform()
        {
            Axis = new Vector2(xAxis,yAxis);
            transform = Matrix.CreateScale(1 * ResolutionUtility.stdScale.X, 1 * ResolutionUtility.stdScale.Y, 0) *
                Matrix.CreateScale(EditorTransformScale.X, EditorTransformScale.Y, 0) * Matrix.CreateTranslation(new Vector3(xAxis * ResolutionUtility.stdScale.X, yAxis * ResolutionUtility.stdScale.Y, 0))
                * Matrix.CreateTranslation(new Vector3(EditorTransformTranslation.X, EditorTransformTranslation.Y, 0));

            transform = Matrix.CreateScale(1 * 1, 1 * 1, 0) *
    Matrix.CreateScale(EditorTransformScale.X, EditorTransformScale.Y, 0) * Matrix.CreateTranslation(new Vector3(xAxis * 1, yAxis * 1, 0))
    * Matrix.CreateTranslation(new Vector3(EditorTransformTranslation.X, EditorTransformTranslation.Y, 0));

            GUITransform = Matrix.CreateScale(1 * ResolutionUtility.stdScale.X, 1 * ResolutionUtility.stdScale.Y, 0)
                * Matrix.CreateScale(EditorTransformScale.X, EditorTransformScale.Y, 0)
                * Matrix.CreateTranslation(new Vector3(EditorTransformTranslation.X, EditorTransformTranslation.Y, 0));
        }

        public static void Update(GameTime gameTime)
        {
            try
            {
                MediaPlayer.Volume = (float)((float)musicVolume * (float)masterVolume / 100 / 100);

            }
            catch
            {

            }
                
           
            
            //  MediaPlayer.Volume = 0.2f;
        }

        public static Vector2 CenterCamera(float tempX,float tempY)
        {
            if (SelectionUtility.HasMemberSelected())
            {
                Vector2 centerOfScreen = new Vector2(1366 / 2 - SelectionUtility.primarySelectedCharacter.proximityIndicator.Width / 2, 768 / 2 - SelectionUtility.primarySelectedCharacter.proximityIndicator.Height / 2);
                int x = (int)(SelectionUtility.primarySelectedCharacter.position.X + SceneUtility.xAxis - centerOfScreen.X);
                int y = (int)(SelectionUtility.primarySelectedCharacter.position.Y + SceneUtility.yAxis - centerOfScreen.Y);

                SceneUtility.xAxis -= x;
                SceneUtility.yAxis -= y;

                tempX = SceneUtility.xAxis;
                tempY = SceneUtility.yAxis;
                return new Vector2(tempX, tempY);
            }

            return new Vector2(tempX,tempY);
        }

        public static void ChangeScene(int sceneInt)
        {
            if ((int)Game1.Screens.Editor != currentScene)
            {
                prevScene = currentScene;
                currentScene = sceneInt;
            }
        }

        public static void adjustMasterVolume(int newVolume)
        {
            if (newVolume < 101 && newVolume > -1)
            {
                masterVolume = newVolume;
            }
        }

        public static void adjustMusicVolume(int newVolume)
        {
            if (newVolume < 101 && newVolume > -1)
            {
                musicVolume = newVolume;
            }
        }

        public static void adjustSEVolume(int newVolume)
        {
            if (newVolume < 101 && newVolume > -1)
            {
                soundEffectsVolume = newVolume;
            }
        }

        public static void adjustAnimationSpeed(int newSpeed)
        {
            if (newSpeed < 5 && newSpeed > -1)
            {
                animationSpeed = newSpeed;
            }
        }

        public static bool isWithinMapWidth(float x)
        {
            if (!(x < 0) && x <= currentMapSize.X)
            {
                return true;
            }
            return false;
        }

        public static bool isWithinMapHeight(float y)
        {
            if (!(y < 0) && y <= currentMapSize.Y)
            {
                return true;
            }
            return false;
        }

        public static bool isWithinMap(Vector2 xy)
        {
            if (isWithinMapWidth(xy.X) && isWithinMapHeight(xy.Y))
            {
                return true;
            }
            return false;
        }
    }
}
