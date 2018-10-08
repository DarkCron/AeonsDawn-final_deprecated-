using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Spells.ShotPattern
{
    class StraightShot:BasicPattern
    {

        public StraightShot(Spell spell)
            : base(spell)
        {

        }

        public SpellMissile Update(GameTime gameTime, Shape shooter, Vector2 target, List<Spell> extraSpells = default(List<Spell>), bool bRandom = false)
        {
            base.Update(gameTime);

            if (spellTimer.millisecondTimer(gameTime, 1000))
            {
                spellTimer.elapsedMilliseconds = 0;
                if(!bRandom){
                    return CalculateSimpleShot(shooter, target);
                }else if(bRandom){
                    return CalculateRandomSimpleShot(shooter, target, extraSpells);
                }
                
            }

            return default(SpellMissile);
        }

        private SpellMissile CalculateRandomSimpleShot(Shape shooter, Vector2 target,List<Spell> extraSpells)
        {
            //Random rand2 = new Random();
            int temp = GamePlayUtility.Randomize(0,extraSpells.Count);

            float spellAngleNR = (float)(spellAngle / Math.PI * 180);
            float angle = 3.6f * 4;
            int divider = 15;

            SpellMissile tempMissile = new SpellMissile(extraSpells[temp],
extraSpells[temp].spellTextureSpriteSheet,
1,
shooter.position,
true,
default(Vector2),
extraSpells[temp].spellName,
extraSpells[temp].spellHitBox);


            float maxAngle = 0f;
            float minAngle = 0f;

            float deltaY = target.Y - tempMissile.centerPoint.Y;
            float deltaX = target.X - tempMissile.centerPoint.X;
            float angleVector = (float)(Math.Atan2(deltaY, deltaX) + Math.PI / 2);


            maxAngle = angleVector + (float)(Math.PI / 3);
            maxAngle = angleVector - (float)(Math.PI / 3);

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

        private SpellMissile CalculateSimpleShot(Shape shooter, Vector2 target)
        {
            
            float spellAngleNR = (float)(spellAngle / Math.PI * 180);
            float angle = 3.6f * 4;
            int divider = 15;

            SpellMissile tempMissile = new SpellMissile(spell,
spell.spellTextureSpriteSheet,
1,
shooter.position,
true,
default(Vector2),
spell.spellName,
spell.spellHitBox);


            float maxAngle = 0f;
            float minAngle = 0f;

            float deltaY = target.Y - tempMissile.centerPoint.Y;
            float deltaX = target.X - tempMissile.centerPoint.X;
            float angleVector = (float)(Math.Atan2(deltaY, deltaX) + Math.PI / 2);


            maxAngle = angleVector + (float)(Math.PI / 3);
            maxAngle = angleVector - (float)(Math.PI / 3);

            spellAngleNR = (float)(angleVector * 180 / Math.PI);
            if(spellAngleNR<0){
                spellAngleNR += 360;
            }

            if (spellAngleNR > 360 - 45 && spellAngleNR < 45)
            {
                tempMissile.Rotate(0);
            }
            else if (spellAngleNR < 360-45 && spellAngleNR > 360-45-90)
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
