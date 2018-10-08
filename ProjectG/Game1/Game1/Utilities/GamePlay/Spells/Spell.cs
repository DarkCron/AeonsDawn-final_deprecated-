using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Spells
{
    public class Spell
    {
        public String spellName = "";
        public int spellIndexOnSpellBar = -1;
        public int spellIndexOnList = -1;
        public Rectangle spellTextureBounds;
        public Texture2D spellTextureSpriteSheet;
        public Rectangle spellTextureSpellBarBounds;
        public Texture2D spellTextureSpellBarSheet;
        SoundEffect spellSound;
        //int causesStatusEffect = (int)(StatusEffects.StatusEffects.statusEffects.Normal);
        public int spellType = (int)(SpellType.statusEffects.Melee);
        public int spellStatusEffect = (int)(StatusEffects.StatusEffects.statusEffects.Normal);
        public int spellStatusModifier = (int)(StatusEffects.StatusEffects.statusModifiers.Normal);

        public Rectangle spellHitBox;

        public int effectChanceUB = 100;
        public int effectChance = 0;
        public int modifierChanceUB = 100;
        public int modifierChance = 0;

        public bool bHasOverlay = false;
        public Color overlayColor;
        public Texture2D overlayTexture;
        public Rectangle overlayBounds = new Rectangle();

        public Spell(String spellName, Texture2D spellTextureSpriteSheet, Rectangle spellTextureBounds,
        Texture2D spellTextureSpellBarSheet, Rectangle spellTextureSpellBarBounds, Rectangle spellHitBox = default(Rectangle)
        )
        {
            this.spellName = spellName;
            this.spellTextureSpriteSheet = spellTextureSpriteSheet;
            this.spellTextureBounds = spellTextureBounds;
            this.spellTextureSpellBarSheet = spellTextureSpellBarSheet;
            this.spellTextureSpellBarBounds = spellTextureSpellBarBounds;
            this.spellHitBox = spellHitBox;
        }

        public void SetOverlay(Color overlayColor, Texture2D overlayTexture,Rectangle overlayBounds)
        {
            bHasOverlay = true;

            if(overlayColor == default(Color)){
                overlayColor = new Color(255,255,255,100);
            }
            else
            {
                this.overlayColor = overlayColor;
            }

            this.overlayTexture = overlayTexture;
            this.overlayBounds = overlayBounds;
        }

        public void setChances(int effectChance, int modifierChance, int effectChanceUB = 100, int modifierChanceUB=100)
        {
            this.effectChance = effectChance;
            this.modifierChance = modifierChance;
            this.effectChanceUB = effectChanceUB;
            this.modifierChanceUB = modifierChanceUB;
        }

        public bool CauseEffect()
        {
            if(effectChance!=0){
                int temp = GamePlayUtility.Randomize(0,effectChanceUB);
                if(temp<=effectChance){
                    return true;
                }
            }
            return false;
        }

        public bool CauseModifier()
        {
            if (modifierChance != 0)
            {
                int temp = GamePlayUtility.Randomize(0, modifierChanceUB);
                if (temp <= modifierChance)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
