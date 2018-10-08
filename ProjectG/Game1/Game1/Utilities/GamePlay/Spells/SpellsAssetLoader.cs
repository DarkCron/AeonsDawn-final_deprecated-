using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Spells
{
    static class SpellsAssetLoader
    {
        static public Texture2D SpriteSheetL1;
        static public Texture2D SpriteSheetL1Overlay;
        static public List<Rectangle> FrameStartL1 = new List<Rectangle>();
        static public List<int> FrameCountL1 = new List<int>();



        static public void loadAssets(Game game)
        {
            //TEMPANIMATION
            SpriteSheetL1 = game.Content.Load<Texture2D>(@"Graphics\Test\SpriteSheet\spritesheet");
            
            FrameStartL1 = new List<Rectangle>();
            FrameCountL1 = new List<int>();

            FrameStartL1.Add(new Rectangle(64, 0 + 1 * 64, 64, 64));
            FrameStartL1.Add(new Rectangle(64, 0 + 1 * 64, 64, 64));
            FrameStartL1.Add(new Rectangle(64, 0 + 1 * 64, 64, 64));
            FrameStartL1.Add(new Rectangle(64, 0 + 0 * 64, 64, 64));
            FrameStartL1.Add(new Rectangle(64, 0 + 2 * 64, 64, 64));

            FrameCountL1.Add(4);
            FrameCountL1.Add(0);
            FrameCountL1.Add(0);
            FrameCountL1.Add(4);
            FrameCountL1.Add(4);
            //TEMPANIMATION

            //BASICMISSILES
            LoadBasicMissiles(game);
        }

        static public Spell basicMissileArcane;
        static public Spell basicMissileFire;
        static public Spell basicMissileDark;
        static public Spell basicMissileEarth;
        static public Spell basicMissileIce;
        static public Spell basicMissileGrass;
        static public Spell basicMissileWind;
        static public String basicMissileTextureString = @"Graphics\Particles\Spells\Missile\basicMissileSpriteSheet";
        static public Texture2D basicMissileTexture;
        private static void LoadBasicMissiles(Game game)
        {
            basicMissileTexture = game.Content.Load<Texture2D>(basicMissileTextureString);
            SpriteSheetL1Overlay = game.Content.Load<Texture2D>(@"Graphics\Particles\Spells\Missile\SpellOverlay");
           
            basicMissileArcane = new Spell("Arcane Missile", basicMissileTexture, new Rectangle(64, 0, 64, 64), basicMissileTexture, new Rectangle(64, 0, 64, 64), new Rectangle(0, 0, 64, 64));
            basicMissileFire = new Spell("Fire Missile", basicMissileTexture, new Rectangle(192, 0, 64, 64), basicMissileTexture, new Rectangle(192, 0, 64, 64), new Rectangle(0, 0, 64, 64));
            basicMissileDark = new Spell("Dark Missile", basicMissileTexture, new Rectangle(64, 64, 64, 64), basicMissileTexture, new Rectangle(64, 64, 64, 64), new Rectangle(0, 0, 64, 64));
            basicMissileEarth = new Spell("Earth Missile", basicMissileTexture, new Rectangle(64, 128, 64, 64), basicMissileTexture, new Rectangle(64, 128, 64, 64), new Rectangle(0, 0, 64, 64));
            basicMissileIce = new Spell("Ice Missile", basicMissileTexture, new Rectangle(192, 128, 64, 64), basicMissileTexture, new Rectangle(192, 128, 64, 64), new Rectangle(0, 0, 64, 64));
            basicMissileGrass = new Spell("Grass Missile", basicMissileTexture, new Rectangle(192, 64, 64, 64), basicMissileTexture, new Rectangle(192, 64, 64, 64), new Rectangle(0, 0, 64, 64));
            basicMissileWind = new Spell("Wind Missile", basicMissileTexture, new Rectangle(64, 192, 64, 64), basicMissileTexture, new Rectangle(64, 192, 64, 64), new Rectangle(0, 0, 64, 64));
           
            basicMissileArcane.spellType = (int)(SpellType.statusEffects.Arcane);
            basicMissileFire.spellType = (int)(SpellType.statusEffects.Fire);
            basicMissileDark.spellType = (int)(SpellType.statusEffects.Dark);
            basicMissileEarth.spellType = (int)(SpellType.statusEffects.Earth);
            basicMissileIce.spellType = (int)(SpellType.statusEffects.Ice);
            basicMissileGrass.spellType = (int)(SpellType.statusEffects.Grass);
            basicMissileWind.spellType = (int)(SpellType.statusEffects.Wind);

            basicMissileArcane.spellStatusModifier = (int)(StatusEffects.StatusEffects.statusModifiers.Normal);
            basicMissileFire.spellStatusModifier = (int)(StatusEffects.StatusEffects.statusModifiers.Normal);
            basicMissileDark.spellStatusModifier = (int)(StatusEffects.StatusEffects.statusModifiers.Blind);
            basicMissileEarth.spellStatusModifier = (int)(StatusEffects.StatusEffects.statusModifiers.Slow);
            basicMissileIce.spellStatusModifier = (int)(StatusEffects.StatusEffects.statusModifiers.Normal);
            basicMissileGrass.spellStatusModifier = (int)(StatusEffects.StatusEffects.statusModifiers.Normal);
            basicMissileWind.spellStatusModifier = (int)(StatusEffects.StatusEffects.statusModifiers.Slow);

            basicMissileArcane.spellStatusEffect = (int)(StatusEffects.StatusEffects.statusEffects.ArcaneBurn);
            basicMissileFire.spellStatusEffect = (int)(StatusEffects.StatusEffects.statusEffects.Burned);
            basicMissileDark.spellStatusEffect = (int)(StatusEffects.StatusEffects.statusEffects.Normal);
            basicMissileEarth.spellStatusEffect = (int)(StatusEffects.StatusEffects.statusEffects.Normal);
            basicMissileIce.spellStatusEffect = (int)(StatusEffects.StatusEffects.statusEffects.Frozen);
            basicMissileGrass.spellStatusEffect = (int)(StatusEffects.StatusEffects.statusEffects.Poision);
            basicMissileWind.spellStatusEffect = (int)(StatusEffects.StatusEffects.statusEffects.Normal);

           
            basicMissileArcane.setChances(20,0);
            basicMissileFire.setChances(20, 0);
            basicMissileDark.setChances(0, 20);
            basicMissileEarth.setChances(0, 20);
            basicMissileIce.setChances(20, 0);
            basicMissileGrass.setChances(20, 0);
            basicMissileWind.setChances(0, 20);

            int opacity = 1;
            basicMissileArcane.SetOverlay(new Color(190, 85, 226, opacity), SpriteSheetL1Overlay, new Rectangle(0, 0, 64, 96));
            basicMissileFire.SetOverlay(new Color(255, 65, 72, opacity), SpriteSheetL1Overlay, new Rectangle(0, 0, 64, 96));
            basicMissileDark.SetOverlay(new Color(74, 49, 87, opacity), SpriteSheetL1Overlay, new Rectangle(0, 0, 64, 96));
            basicMissileEarth.SetOverlay(new Color(161, 110, 34, opacity), SpriteSheetL1Overlay, new Rectangle(0, 0, 64, 96));
            basicMissileIce.SetOverlay(new Color(128, 229, 229, opacity), SpriteSheetL1Overlay, new Rectangle(0, 0, 64, 96));
            basicMissileGrass.SetOverlay(new Color(128, 229, 135, opacity), SpriteSheetL1Overlay, new Rectangle(0, 0, 64, 96));
            basicMissileWind.SetOverlay(new Color(178, 226, 221, opacity), SpriteSheetL1Overlay, new Rectangle(0, 0, 64, 96));
        }
    }
}
