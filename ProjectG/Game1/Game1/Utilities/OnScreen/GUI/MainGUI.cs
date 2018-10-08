using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBAGW.Utilities;
using TBAGW.Scenes;
using TBAGW.Utilities.Actions;
using System;
using System.Collections.Generic;
using TBAGW.Scenes.MainGame;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.GamePlay.Characters.Friendly.Team;
using TBAGW.Utilities.GamePlay.Characters.Friendly;
using TBAGW.Utilities.GamePlay.Characters;
using TBAGW.Utilities.GamePlay.Spells;

namespace TBAGW.Utilities.OnScreen.GUI
{
    public static class MainGUI
    {
        static Texture2D GUIBigPic;
        public static Rectangle GUIBigPicBounds;

        //Right sidebar
        static Texture2D GUISmallPic;
        static Rectangle GUISmallPicBounds;

        static Texture2D GUIBigPicBG;
        static Texture2D GUIBigFrame;
        public static Texture2D GUISpellBar;

        static SpriteFont smallGUIFont;

        static List<Texture2D> spells = new List<Texture2D>();

        static int selectedHeroIndex = -1;
        static BaseCharacter targetedCharacter;
        static BasicHero selectedHero;

        static Texture2D bigGUIPicTarget;
        static Rectangle GUIBigPicBoundsTarget;
        static Vector2 HBarSLocTarget;
        static Vector2 HBarLocTarget;
        static Vector2 MBarSLocTarget;
        static Vector2 MBarLocTarget;

        public static void Initialize(Game game)
        {
            smallGUIFont = game.Content.Load<SpriteFont>(@"Fonts\GUIFontSmall");
            GUIBigPic = game.Content.Load<Texture2D>(@"Graphics\GUI\BigGUIPics\TestBigProf"); //
            GUIBigPicBG = game.Content.Load<Texture2D>(@"Graphics\GUI\Layout\TopLeftBG");//128 x 96
            GUIBigFrame = game.Content.Load<Texture2D>(@"Graphics\GUI\Layout\BigFrame");// 134 x 102
            GUISpellBar = game.Content.Load<Texture2D>(@"Graphics\GUI\Layout\SpellBar");//752 x 96
            GUIBigPicBounds = new Rectangle(0, 0, 128, 96);
        }

        public static void Update(GameTime gameTime)
        {
            if (selectedHeroIndex != SelectionUtility.currentSelectedPartyMember && SelectionUtility.currentSelectedPartyMember != -1)
            {
                selectedHeroIndex = SelectionUtility.currentSelectedPartyMember;
                selectedHero = (BasicTeamUtility.parties[(int)BasicTeamUtility.FriendlyParties.Party][selectedHeroIndex] as BasicHero);
                GUIBigPic = selectedHero.charAsset.bigGUIPic;
                GUIBigPicBounds = selectedHero.charAsset.bigGUIPicBounds;
                SpellBarHandler.bGenerateBounds = true;

                int i = 0;
                SpellBarHandler.activeSpellsBounds.Clear();
                SpellBarHandler.activeSpells.Clear();
                foreach (var spell in selectedHero.activeSpells)
                {
                    if (spell != default(Spell))
                    {
                        if (SpellBarHandler.bGenerateBounds)
                        {
                            SpellBarHandler.activeSpells.Add(spell);
                            SpellBarHandler.activeSpellsBounds.Add(new Rectangle(315 + 16 + i * (64 + 2 + 16), 672 + 15, 64, 64));
                            
                        }
                    }
                    i++;
                }

                SpellBarHandler.bGenerateBounds = false;
                SpellBarHandler.Update(gameTime);
            }else if (SelectionUtility.currentSelectedPartyMember!=-1)
            {
                SpellBarHandler.Update(gameTime);
            }

            if (targetedCharacter != (SelectionUtility.secondSelectedCharacter))
            {
                targetedCharacter = SelectionUtility.secondSelectedCharacter;
                if (SelectionUtility.secondSelectedCharacter != default(BaseCharacter))
                {
                    bigGUIPicTarget = SelectionUtility.secondSelectedCharacter.charAsset.bigGUIPic;
                    GUIBigPicBoundsTarget = SelectionUtility.secondSelectedCharacter.charAsset.bigGUIPicBounds;
                    HBarSLocTarget = new Vector2(-(128 + 16 + 16 + 16) + 1366 - GUIBigPicBounds.Width, 48 + 16);
                    HBarLocTarget = new Vector2(-(HBarLoc.X) + 1366 - GUIBigPicBounds.Width, 48 + 16 + 16 + fontMargin);
                    MBarSLocTarget = new Vector2(-(128 + 16 + 16 + 16) + 1366 - GUIBigPicBounds.Width, 80 + 16);
                    MBarLocTarget = new Vector2(-(MBarLoc.X) + 1366 - GUIBigPicBounds.Width, 80 + 16 + 16 + fontMargin);

                }
            }

            
            selectedHeroIndex = SelectionUtility.currentSelectedPartyMember;
        }

        private static int fontMargin = 3;
        private static Vector2 HBarLoc = new Vector2(128 + 16 + 16 + 16, 48 + 16 + 16);
        private static Vector2 MBarLoc = new Vector2(128 + 16 + 16 + 16, 80 + 16 + 16);
        private static Vector2 CBarLoc = new Vector2(16 + 16, 112 + 16 + 16 + 16);

        private static Vector2 GUIBigPicBGLoc = new Vector2(16, 16);


        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.GUITransform);





            //Top left corner
            if (selectedHeroIndex != -1)
            {
                spriteBatch.Draw(GUIBigPicBG, GUIBigPicBGLoc * 1, new Rectangle(0, 0, 304, 176), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                spriteBatch.Draw(GUIBigFrame, new Vector2(32 - 3, 32 - 3) * 1, new Rectangle(0, 0, 134, 102), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(GUIBigPic, new Vector2(32, 32) * 1, GUIBigPicBounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(smallGUIFont, selectedHero.shapeName.ToUpper(), new Vector2(128 + 16 + 16 + 16, 16 + 16 - fontMargin) * 1, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(smallGUIFont, selectedHero.stats.HealthAsString(), new Vector2(HBarLoc.X, HBarLoc.Y - fontMargin - 16) * 1, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.Draw(Game1.hitboxHelp, HBarLoc * 1, new Rectangle(0, 0, 128, 16), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(smallGUIFont, selectedHero.stats.ManaAsString(), new Vector2(MBarLoc.X, MBarLoc.Y - fontMargin - 16) * 1, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.Draw(Game1.hitboxHelp, MBarLoc * 1, new Rectangle(0, 0, 128, 16), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(smallGUIFont, "Default spell name", new Vector2(CBarLoc.X, CBarLoc.Y - fontMargin - 16) * 1, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.Draw(Game1.hitboxHelp, CBarLoc * 1, new Rectangle(0, 0, 256 + 16, 16), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                //spellbar

                spriteBatch.Draw(GUISpellBar, new Rectangle(315, 672, 752, 96), Color.White);

                int i = 0;
                foreach (var spell in selectedHero.activeSpells)
                {
                    if (spell != default(Spell))
                    {
                        spriteBatch.Draw(spell.spellTextureSpellBarSheet, new Vector2(315 + 16 + i * (64 + 2 + 16), 672 + 15), spell.spellTextureSpellBarBounds, Color.White);
                    }

                    i++;
                }



            }

            //Top right corner
            if (SelectionUtility.secondSelectedCharacter != default(BaseCharacter))
            {


                spriteBatch.Draw(GUIBigPicBG, GUIBigPicBGLoc + new Vector2(1046 - 16, 0), new Rectangle(0, 0, 304, 176), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                spriteBatch.Draw(GUIBigFrame, new Vector2(-35 - GUIBigPicBounds.Width, 32 - 3) + new Vector2(1366, 0), new Rectangle(0, 0, 134, 102), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(bigGUIPicTarget, new Vector2(-32 - GUIBigPicBounds.Width, 32) + new Vector2(1366, 0), GUIBigPicBoundsTarget, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(smallGUIFont, SelectionUtility.secondSelectedCharacter.shapeName.ToUpper(), HBarSLocTarget - new Vector2(0, 32), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(smallGUIFont, SelectionUtility.secondSelectedCharacter.stats.HealthAsString(), HBarSLocTarget, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.Draw(Game1.hitboxHelp, HBarLocTarget * 1, new Rectangle((int)HBarLocTarget.X, (int)HBarLocTarget.Y, 128, 16), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(smallGUIFont, SelectionUtility.secondSelectedCharacter.stats.ManaAsString(), MBarSLocTarget, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.Draw(Game1.hitboxHelp, MBarLocTarget * 1, new Rectangle((int)MBarLocTarget.X, (int)MBarLocTarget.Y, 128, 16), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
        }
    }

    public static class SpellBarHandler
    {
        public static List<Rectangle> activeSpellsBounds = new List<Rectangle>();
        public static List<Spell> activeSpells = new List<Spell>();
        public static bool bGenerateBounds = false;
        public static bool bHasClickedOnSpell = false;
        public static Spell clickedSpell;
        public static int spellBarIndex = 0;

        public static void Update(GameTime gameTime)
        {
            if (new Rectangle(315, 672, 752, 96).Contains(CursorUtility.GUICursorPos))
            {
                bool temp = false;
                int i = 0;
                foreach (var activeSpell in activeSpellsBounds)
                {
                    if (activeSpell.Contains(CursorUtility.GUICursorPos))
                    {
                        clickedSpell = activeSpells[i];
                        spellBarIndex = i + 1;
                        temp = true;
                        break;
                    }

                    i++;
                }
                bHasClickedOnSpell = temp;
            }
        }

    }
}
