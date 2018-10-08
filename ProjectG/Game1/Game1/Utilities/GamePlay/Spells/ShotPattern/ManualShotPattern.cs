using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Spells.ShotPattern
{
    class ManualShotPattern : BasicPattern
    {
        public SpellMissile Update(GameTime gameTime, Shape shooter, Vector2 target, Spell castSpell)
        {

         //   if (spellTimer.millisecondTimer(gameTime, 100))
           // {
               // spellTimer.elapsedMilliseconds = 0;
                return CalculateSimpleShot(shooter,target,castSpell);

            //}

            //return default(SpellMissile);
        }

        private SpellMissile CalculateSimpleShot(Shape shooter, Vector2 target, Spell castSpell)
        {

            float spellAngleNR = (float)(spellAngle / Math.PI * 180);

            SpellMissile tempMissile = new SpellMissile(castSpell,
castSpell.spellTextureSpriteSheet,
1,
shooter.position,
true,
default(Vector2),
castSpell.spellName,
castSpell.spellHitBox);

            float deltaY = target.Y - tempMissile.centerPoint.Y;
            float deltaX = target.X - tempMissile.centerPoint.X;
            float angleVector = (float)(Math.Atan2(deltaY, deltaX) + Math.PI / 2);



            spellAngleNR = (float)(angleVector * 180 / Math.PI);
            if (spellAngleNR < 0)
            {
                spellAngleNR += 360;
            }

            if (spellAngleNR > 360 - 45 && spellAngleNR < 45)
            {
                tempMissile.Rotate(0);
            }
            else if (spellAngleNR < 360 - 45 && spellAngleNR > 360 - 45 - 90)
            {
                tempMissile.Rotate(90);
            }
            else if (spellAngleNR < 360 - 45 - 90 && spellAngleNR > 45 + 90)
            {
                tempMissile.Rotate(180);
            }
            else if (spellAngleNR > 45 && spellAngleNR < 45 + 90)
            {

                tempMissile.Rotate(270);
            }

            //Console.Out.WriteLine(angleVector*180/Math.PI);
            tempMissile.speed = 3;
            tempMissile.bStopAtTarget = false;
            // shapes[shapes.Count - 1].SetVelocity(new Vector2((float)Math.Sin(spellAngle), (float)Math.Cos(spellAngle)) * speed);
            tempMissile.SetVelocity(angleVector);

            return tempMissile;
        }
    }
}
