using TBAGW.Utilities.GamePlay.Spells;
using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Characters.Hostile
{
    class Target:Shape
    {

        int statusModifier = (int)(StatusEffects.StatusEffects.statusModifiers.Normal);
        int statusEffect = (int)(StatusEffects.StatusEffects.statusEffects.Normal);

         public Target(Texture2D shapeTexture, int scale, Vector2 position, bool bCollision, Vector2 center = default(Vector2), String shapeName = "", Rectangle shapeTextureBounds = default(Rectangle))
             :base(shapeTexture, scale, position, bCollision,  center,  shapeName,  shapeTextureBounds ){

         }

         int temp;
         int temp2;
         public override bool Contains(SpellMissile missile)
         {
             if (proximityIndicator.Intersects(missile.proximityIndicator))
             {

                 foreach (var hitbox in shapeHitBox)
                 {
                     foreach (var target in missile.shapeHitBox)
                     {
                         if (hitbox.Intersects(target))
                         {
                             hitString = "You've been hit by,\nyou've been hit by,\na smooth " + missile.shapeName;

                             AdjustModifiers(missile);
                             return true;

                         }
                     }
                 }

             }
             return false;
         }

         private void AdjustModifiers(SpellMissile missile)
         {
             
             if(missile.baseSpell.CauseEffect()){
                 statusEffect = missile.baseSpell.spellStatusEffect;
                 temp = missile.baseSpell.effectChance;
                 
             }else if(missile.baseSpell.CauseModifier()){
                 statusModifier = missile.baseSpell.spellStatusModifier;
                 temp2 = missile.baseSpell.modifierChance;
             }
         }

         public override void Draw(SpriteBatch spriteBatch)
         {
             base.Draw(spriteBatch);


             
             if(statusEffect!=(int)(StatusEffects.StatusEffects.statusEffects.Normal)){
                 spriteBatch.DrawString(Game1.defaultFont, "Oh dear, seems you got "+Enum.GetName(typeof(StatusEffects.StatusEffects.statusEffects),statusEffect)
                     +"\nThis is a status effect with a chance of "+temp+"%\nUnlucky you!", position + new Vector2(0, 64+64+32), Color.Black);
             }

             if (statusModifier != (int)(StatusEffects.StatusEffects.statusModifiers.Normal))
             {
                 spriteBatch.DrawString(Game1.defaultFont, "Oh dear, seems you got " + Enum.GetName(typeof(StatusEffects.StatusEffects.statusModifiers), statusModifier)
                     + "\nThis is a modifier effect with a chance of " + temp2 + "%\nUnlucky you!", position + new Vector2(64, -64), Color.Black);
             }

         }
    }
}
