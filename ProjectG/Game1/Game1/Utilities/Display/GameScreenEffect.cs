using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities;
using TBAGW.Utilities.Map;

namespace TBAGW
{
    static public class GameScreenEffect
    {
        public enum Effects { None = 0, Conversation, RegionName, Shake }
        public enum Transform { None = 0, Zoom = 1 }
        static public Effects currentEffect = Effects.None;
        static Transform currentTransform = Transform.None;

        #region conversation effect fields
        static int zoomTimer = 10;
        static int zoomTimePassed = 0;
        static int maxZoomHorizontalModifier = 150;
        static int ZoomHorizontalModifier = 0;
        static bool bDoneZooming = false;
        #endregion

        #region Region Name effect fields
        static String regionName = "";
        static float regionNameOpacity = 0f;
        static float regionNameOpacityModifier = 0.01f;
        static int regionOpacityTimer = 50;
        static int regionOpacityTimePassed = 0;
        static int regionMaxOpacityTimer = 2000;
        static int regionMaxOpacityTimePassed = 0;
        static Vector2 regionNamePosition = new Vector2(1366 / 2, 50);
        static bool bReachedMax = false;
        #endregion

        static public bool bWeatherEffect = false;
        static public ParticleSystemSource WeatherEffect;

        static SpriteFont testSF;
        static SpriteFont testSF20;
        static SpriteFont testSF25;
        static SpriteFont testSF32;
        static SpriteFont testSF48;


        static float combatCritShakeTimer = 0.27f * 1000f;
        static int combatCritShakeTimePassed = 0;
        static int combatCritShakeAmount = 20;
        static int combatCritShakeAmountDefault = 20;

        static float zoomTo = 1.0f;
        static float zoomStep = 0.0f;
        static int frameZoomTimer = 60;
        static int frameZoomTimePassed = 0;
        static public bool bZoomIsComplete = false;
        static Vector2 adjustZoomPosRender = new Vector2(0);
        static float zoomStepAmountDone = 0f;

        static public void InitiateZoom(float seconds, float endZoom, float gameZoom, float currentZoom = 1.0f)
        {
            zoomTo = currentZoom;
            zoomTimer = (int)(seconds * 60);
            zoomStep = (float)(endZoom - gameZoom) / (float)zoomTimer;
            zoomTimePassed = 0;
            currentTransform = Transform.Zoom;
            adjustZoomPosRender = Vector2.Zero;
            zoomStepAmountDone = 0f;
        }

        static public void InitiateCombatCritShake(float combatShakeMod)
        {
            Reset();
            currentEffect = Effects.Shake;
            combatCritShakeAmount = (int)(combatCritShakeAmountDefault * combatShakeMod);
            combatCritShakeTimePassed = 0;
        }

        static public void InitializeResources()
        {
            testSF = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test");
            testSF20 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test20");
            testSF25 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test25");
            testSF32 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");
            testSF48 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test48");
        }

        static int opacitySteps = 0;
        static int opacityStepsTaken = 0;
        static float opacityStep = 0.01f;
        static float opacityDetailed = 1f;

        static public void GenerateOpacitySteps(int opacityEnd, float time)
        {
            int spriteOpacity = (int)(worldBrightness * 100);
            if (time != 0)
            {
                opacitySteps = (int)(time * 60);
                opacityStep = (float)(((float)(opacityEnd - spriteOpacity)) / ((float)opacitySteps)) / 100;
                opacityDetailed = ((float)spriteOpacity) / 100f;

            }
            else
            {
                worldBrightness = opacityEnd / 100f;
                opacityDetailed = opacityEnd / 100f;
                opacityStepsTaken = 0;
                opacitySteps = 0;
                opacityStep = 0f;
            }

            opacityStepsTaken = 0;

        }

        static public bool CombatZoomAlmostDone()
        {
            if (zoomTimer < zoomTimePassed + 30 && zoomTimer > zoomTimePassed)
            {
                return true;
            }
            return false;
        }

        static public bool CombatZoomDone()
        {
            if (GameProcessor.bStartCombatZoom && zoomTimePassed == 0)
            {
                return true;
            }
            if (zoomTimer <= zoomTimePassed)
            {
                return true;
            }
            return false;
        }

        static public void Update(GameTime gt, Vector2 cameraPos)
        {
            if (opacityStepsTaken < opacitySteps)
            {
                opacityStepsTaken++;
                opacityDetailed += opacityStep;
                worldBrightness = opacityDetailed;
            }

            if (currentTransform == Transform.Zoom && !LUA.LuaExecutionList.DemandOverride())
            {
                if (zoomTimePassed < zoomTimer)
                {
                    zoomTimePassed++;
                    zoomTo += zoomStep;
                    zoomStepAmountDone += zoomStep;
                    adjustZoomPosRender = new Vector2(1366 / 2, 768 / 2) * zoomStepAmountDone;
                }
            }

            //worldColor.R += (byte)GamePlayUtility.Randomize(-3, 4);
            //worldColor.G += (byte)GamePlayUtility.Randomize(-3, 4);
            //worldColor.B += (byte)GamePlayUtility.Randomize(-3, 4);

            if (worldBrightness > 1f)
            {
                worldBrightness = 1f;
            }

            if (worldBrightness < 0f)
            {
                worldBrightness = 0f;
            }

            switch (currentEffect)
            {
                case Effects.None:
                    break;
                case Effects.Conversation:
                    if (ZoomHorizontalModifier < maxZoomHorizontalModifier)
                    {
                        zoomTimePassed += gt.ElapsedGameTime.Milliseconds;
                        if (zoomTimePassed > zoomTimer)
                        {
                            ZoomHorizontalModifier += 2;
                            zoomTimePassed = 0;
                        }
                    }
                    else
                    {
                        bDoneZooming = true;
                    }
                    break;
                case Effects.RegionName:
                    if (!bReachedMax)
                    {
                        regionOpacityTimePassed += gt.ElapsedGameTime.Milliseconds;
                        if (regionOpacityTimePassed > regionOpacityTimer)
                        {
                            regionOpacityTimePassed = 0;
                            regionNameOpacity += regionNameOpacityModifier * 8;
                        }
                        if (regionNameOpacity > 1)
                        {
                            bReachedMax = true;
                        }
                    }
                    else
                    {
                        regionMaxOpacityTimePassed += gt.ElapsedGameTime.Milliseconds;
                        if (regionMaxOpacityTimePassed > regionMaxOpacityTimer)
                        {
                            regionOpacityTimePassed += gt.ElapsedGameTime.Milliseconds;
                            if (regionOpacityTimePassed > regionOpacityTimer)
                            {
                                regionOpacityTimePassed = 0;
                                regionNameOpacity -= regionNameOpacityModifier * 3;
                            }
                            if (regionNameOpacity < 0)
                            {
                                Reset();
                            }
                        }
                    }
                    break;
                case Effects.Shake:
                    combatCritShakeTimePassed += gt.ElapsedGameTime.Milliseconds;

                    if (combatCritShakeTimePassed > combatCritShakeTimer)
                    {
                        Reset();
                    }

                    break;
                default:
                    break;
            }



            if (bWeatherEffect)
            {

            }

            timePassedScale += gt.ElapsedGameTime.Milliseconds;
            if (timePassedScale > 50)
            {
                timePassedScale = 0;
                if (goingUp)
                {
                    scale += 0.1f;
                    if (scale > 7)
                    {
                        goingUp = false;
                    }
                }
                else
                {
                    scale -= 0.1f;
                    if (scale < 3.5)
                    {
                        goingUp = true;
                    }
                }


            }
        }

        static public void Draw(SpriteBatch sb, RenderTarget2D r2d)
        {

            switch (currentEffect)
            {
                case Effects.None:
                    //sb.Draw(r2d, r2d.Bounds, Color.White);
                    break;
                case Effects.Conversation:
                    // Rectangle renderBounds = new Rectangle(r2d.Bounds.X, r2d.Bounds.Y + ZoomHorizontalModifier, r2d.Bounds.Width, r2d.Bounds.Height - 2 * ZoomHorizontalModifier);
                    //sb.Draw(r2d, renderBounds, renderBounds, Color.White);
                    break;
                case Effects.RegionName:
                    // sb.Draw(r2d, r2d.Bounds, Color.White);
                    Vector2 regionNamePos = regionNamePosition;
                    regionNamePos.X = regionNamePosition.X - (testSF48.MeasureString(regionName).X / 2);
                    sb.DrawString(testSF48, regionName, regionNamePos, Color.AntiqueWhite * regionNameOpacity);
                    break;
                case Effects.Shake:

                    break;
            }


        }

        static Texture2D lightTex;
        static RenderTarget2D lightMask = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D trueLightMask = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static Effect lightEffect = Game1.contentManager.Load<Effect>(@"FX\LightMask");
        static float scale = 5;
        static bool goingUp = true;
        static int timePassedScale = 0;
        public static float worldBrightness = .50f;
        public static float worldBrightnessLM = .50f;
        static public Color worldColor = Color.White;
        static public void DrawWithEffects(SpriteBatch sb, RenderTarget2D r2d)
        {
            if (lightTex == null)
            {
                //lightTex = Game1.contentManager.Load<Texture2D>("light");
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            sb.GraphicsDevice.SetRenderTarget(trueLightMask);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
            foreach (var item in GameProcessor.loadedMap.activeLights.FindAll(l => l.bIsLightOn))
            {
                if (item.bIsLightOn)
                {
                    Color c = item.lightColor;
                    item.lightMask.DrawLight(sb, item, item.spriteGameSize.Center.ToVector2() - (new Vector2(item.lightMask.animationFrames[0].Width / 2, item.lightMask.animationFrames[0].Height / 2)) * item.lightScale, item.lightScale, c);
                }
            }
            sb.End();



            sb.GraphicsDevice.SetRenderTarget(null);
            sb.GraphicsDevice.SetRenderTarget(lightMask);
            if (GameProcessor.bIsOverWorldOutsideGame)
            {
                sb.GraphicsDevice.Clear(worldColor * worldBrightnessLM);
            }
            else if (true)
            {
                sb.GraphicsDevice.Clear(Color.White);
            }


            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            if (ThunderEffect.IsShowing())
            {
                var c = ThunderEffect.GetColor();
                sb.Draw(Game1.WhiteTex, new Rectangle(0, 0, 1366, 768), c * .95f);
            }

            sb.End();



            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
            foreach (var item in GameProcessor.loadedMap.activeLights)
            {
                // sb.Draw(lightTex, item.spriteGameSize.Center.ToVector2() - (item.spriteGameSize.Center.ToVector2() - item.spriteGameSize.Location.ToVector2()) * scale, lightTex.Bounds, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
                // item.Draw(sb);
                // item.lightColor = Color.White;
                if (item.bIsLightOn)
                {
                    item.lightMask.DrawLight(sb, item, item.spriteGameSize.Center.ToVector2() - (new Vector2(item.lightMask.animationFrames[0].Width / 2, item.lightMask.animationFrames[0].Height / 2)) * item.lightScale, item.lightScale, item.lightColor);
                }
            }
            sb.End();



            sb.End();




            sb.GraphicsDevice.SetRenderTarget(Game1.gameRender);

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
            lightEffect.Parameters["lightMask"].SetValue(lightMask);
            //  lightEffect.Parameters["truelightMask"].SetValue(trueLightMask);
            //  lightEffect.Parameters["heightMask"].SetValue(GameProcessor.loadedMap.GetHeightMask());
            lightEffect.Parameters["mod"].SetValue((float)(1.0f - worldBrightnessLM));
            //      lightEffect.Parameters["mod2"].SetValue((float)(1f));
            //    lightEffect.Parameters["brightness"].SetValue((float)(worldBrightnessLM));
            //lightEffect.CurrentTechnique.Passes[0].Apply();
            lightEffect.CurrentTechnique.Passes[0].Apply();
            Rectangle renderBounds = new Rectangle(r2d.Bounds.X, r2d.Bounds.Y + ZoomHorizontalModifier, r2d.Bounds.Width, r2d.Bounds.Height - 2 * ZoomHorizontalModifier);
            //  sb.Draw(r2d, renderBounds, renderBounds, Color.White);
            //   sb.Draw(GameProcessor.objectRender, renderBounds, renderBounds, Color.Red);

            switch (currentEffect)
            {
                case Effects.None:

                    renderBounds = new Rectangle(r2d.Bounds.X, r2d.Bounds.Y + ZoomHorizontalModifier, r2d.Bounds.Width, r2d.Bounds.Height - 2 * ZoomHorizontalModifier);
                    if (currentTransform == Transform.None)
                    {
                        sb.Draw(r2d, renderBounds, renderBounds, Color.White);
                    }
                    else if (currentTransform == Transform.Zoom)
                    {
                        sb.Draw(r2d, -adjustZoomPosRender, renderBounds, Color.White, 0f, Vector2.Zero, zoomTo, SpriteEffects.None, 0);
                    }

                    break;
                case Effects.Conversation:
                    sb.Draw(r2d, renderBounds, renderBounds, Color.White);
                    break;
                case Effects.RegionName:
                    sb.Draw(r2d, renderBounds, renderBounds, Color.White);
                    break;
                case Effects.Shake:
                    renderBounds.X += GamePlayUtility.Randomize(-35, 35);
                    renderBounds.Y += GamePlayUtility.Randomize(-35, 35);

                    sb.Draw(r2d, renderBounds, new Rectangle(r2d.Bounds.X, r2d.Bounds.Y + ZoomHorizontalModifier, r2d.Bounds.Width, r2d.Bounds.Height - 2 * ZoomHorizontalModifier), Color.White);
                    break;
                default:
                    break;
            }
            //sb.Draw(GameProcessor.loadedMap.GetHeightMask(), renderBounds, Color.White);
            sb.End();


        }

        static public void DrawLightEditor(SpriteBatch sb, RenderTarget2D r2d, RenderTarget2D targetRender, BasicMap map)
        {
            if (lightTex == null)
            {
                //     lightTex = Game1.contentManager.Load<Texture2D>("light");
            }
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            sb.GraphicsDevice.SetRenderTarget(trueLightMask);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);

            Matrix m = Matrix.CreateTranslation(-MapBuilder.cameraPosX, MapBuilder.cameraPosY, 1) * Matrix.CreateScale(MapBuilder.cameraZoomX, MapBuilder.cameraZoomX, 1);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, m);
            foreach (var item in MapBuilder.loadedMap.activeLights)
            {
                Color c = item.lightColor;
                item.lightMask.DrawLight(sb, item, item.spriteGameSize.Center.ToVector2() - (new Vector2(item.lightMask.animationFrames[0].Width / 2, item.lightMask.animationFrames[0].Height / 2)) * item.lightScale, item.lightScale, c);
            }
            if (MapBuilder.objAddition.selectedObject != null && MapBuilder.objAddition.selectedObject.GetType() == typeof(SpriteLight))
            {
                Color c = (MapBuilder.objAddition.selectedObject as SpriteLight).lightColor;
                var item = (MapBuilder.objAddition.selectedObject as SpriteLight);
                item.lightMask.DrawLight(sb, item, item.spriteGameSize.Center.ToVector2() - (new Vector2(item.lightMask.animationFrames[0].Width / 2, item.lightMask.animationFrames[0].Height / 2)) * item.lightScale, item.lightScale, c);
            }
            sb.End();

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            sb.GraphicsDevice.SetRenderTarget(lightMask);
            sb.GraphicsDevice.Clear(worldColor * worldBrightness);

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, m);
            foreach (var item in MapBuilder.loadedMap.activeLights)
            {

                item.lightMask.DrawLight(sb, item, item.spriteGameSize.Center.ToVector2() - (new Vector2(item.lightMask.animationFrames[0].Width / 2, item.lightMask.animationFrames[0].Height / 2)) * item.lightScale, item.lightScale, item.lightColor);

            }
            if (MapBuilder.objAddition.selectedObject != null && MapBuilder.objAddition.selectedObject.GetType() == typeof(SpriteLight))
            {
                var item = (MapBuilder.objAddition.selectedObject as SpriteLight);
                item.lightMask.DrawLight(sb, item, item.spriteGameSize.Center.ToVector2() - (new Vector2(item.lightMask.animationFrames[0].Width / 2, item.lightMask.animationFrames[0].Height / 2)) * item.lightScale, item.lightScale, item.lightColor);
            }
            sb.End();

            sb.GraphicsDevice.SetRenderTarget(targetRender);
            //sb.GraphicsDevice.Clear(worldColor * MapBuilder.testWorldBrightness);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
            lightEffect.Parameters["lightMask"].SetValue(lightMask);
            //    lightEffect.Parameters["truelightMask"].SetValue(trueLightMask);
            //   lightEffect.Parameters["heightMask"].SetValue(map.GetHeightMask());
            lightEffect.Parameters["mod"].SetValue((float)(1.0f - worldBrightness) / 1.5f);
            //lightEffect.Parameters["mod2"].SetValue((float)(1.8f));
            //lightEffect.Parameters["brightness"].SetValue((float)(worldBrightness));
            lightEffect.CurrentTechnique.Passes[0].Apply();
            Rectangle renderBounds = new Rectangle(r2d.Bounds.X, r2d.Bounds.Y + ZoomHorizontalModifier, r2d.Bounds.Width, r2d.Bounds.Height - 2 * ZoomHorizontalModifier);
            //  sb.Draw(r2d, renderBounds, renderBounds, Color.White);
            //   sb.Draw(GameProcessor.objectRender, renderBounds, renderBounds, Color.Red);

            sb.Draw(r2d, renderBounds, Color.White);

            sb.End();
            sb.End();


            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            //   sb.Draw(trueLightMask, renderBounds, Color.White);

            sb.End();
        }

        static public void DrawWithNoEffects(SpriteBatch sb, RenderTarget2D r2d)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);

            Rectangle renderBounds = new Rectangle(r2d.Bounds.X, r2d.Bounds.Y + ZoomHorizontalModifier, r2d.Bounds.Width, r2d.Bounds.Height - 2 * ZoomHorizontalModifier);
            switch (currentEffect)
            {
                case Effects.None:


                    sb.Draw(r2d, renderBounds, renderBounds, Color.White);
                    break;
                case Effects.Conversation:
                    sb.Draw(r2d, renderBounds, renderBounds, Color.White);
                    break;
                case Effects.RegionName:
                    sb.Draw(r2d, renderBounds, renderBounds, Color.White);
                    break;
                case Effects.Shake:
                    renderBounds.X += GamePlayUtility.Randomize(-35, 35);
                    renderBounds.Y += GamePlayUtility.Randomize(-35, 35);

                    sb.Draw(r2d, renderBounds, new Rectangle(r2d.Bounds.X, r2d.Bounds.Y + ZoomHorizontalModifier, r2d.Bounds.Width, r2d.Bounds.Height - 2 * ZoomHorizontalModifier), Color.White);
                    break;
                default:
                    break;
            }

            sb.End();


        }

        static public void InitializeConversationEffect()
        {
            Reset();
            currentEffect = Effects.Conversation;
        }

        static public void InitializeRegionName(String rn)
        {
            Reset();
            currentEffect = Effects.RegionName;
            // regionNameOpacity = 1.0f;
            regionName = rn;
        }

        static public void TurnOnWeather(int id)
        {
            bWeatherEffect = true;
            WeatherEffect = GameProcessor.gcDB.gameParticleSystems.Find(ps => ps.identifier == id).Clone();

        }

        static public void TurnOffWeather()
        {
            bWeatherEffect = false;
        }

        public static void Reset()
        {
            currentTransform = Transform.None;
            currentEffect = Effects.None;
            zoomTimePassed = 0;
            bDoneZooming = false;
            ZoomHorizontalModifier = 0;

            regionNameOpacity = 0f;
            regionOpacityTimePassed = 0;
            regionMaxOpacityTimePassed = 0;
            bReachedMax = false;
        }

        public static bool DoneZooming()
        {
            if (currentEffect == Effects.Conversation)
            {
                if (bDoneZooming)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
