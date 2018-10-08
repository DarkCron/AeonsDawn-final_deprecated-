using TBAGW.Utilities.GamePlay.Characters.Friendly.Team;
using TBAGW.Utilities.GamePlay.Characters.Hostile;
using TBAGW.Utilities.GamePlay.Spells;
using TBAGW.Utilities.GamePlay.Spells.ShotPattern;
using TBAGW.Utilities.GamePlay.Stats;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Characters.Friendly
{
    class BasicHero : BaseCharacter
    {
        public int teamIndex = (int)BasicTeamUtility.FriendlyParties.None;
        public List<Spell> spellList = new List<Spell>();
        public Spell[] activeSpells = new Spell[9];


        public BasicHero(Texture2D shapeTexture, int scale, Vector2 position, bool bCollision, Vector2 center = default(Vector2), String shapeName = "", Rectangle shapeTextureBounds = default(Rectangle))
            : base(shapeTexture, scale, position, bCollision, center, shapeName, shapeTextureBounds)
        {
            
        }

        public BasicHero(HeroAsset charAsset, int scale, Vector2 position, bool bCollision)
            : base(charAsset as BaseCharAsset, scale, position, bCollision)
        {

        }

        /*
        public BasicHero(HeroAsset heroAsset, int scale, Vector2 position, bool bCollision, Vector2 center = default(Vector2))
            : base(heroAsset.shapeTexture, scale, position, bCollision, center, heroAsset.heroName, heroAsset.shapeTextureBounds)
        {
            bInitializedAsHero = false;
            this.heroAsset = heroAsset;
        }

        public void InitializeHero(HeroAsset heroAsset, List<int> stats)
        {
            bInitializedAsHero = true;
            this.stats.AssignStats(stats);
            this.heroAsset = heroAsset;
        }*/

        public void AssignTeam(int teamIndex)
        {

        }

        public override void Update(GameTime gameTime, List<BaseCharacter> activeObjects)
        {

            base.Update(gameTime,activeObjects);

        }

        public void Shoot(GameTime gameTime, Spell castSpell)
        {
            if (bThisCharacterSelected)
            {
                var temp = manualShot.Update(gameTime, this,CursorUtility.trueCursorPos,castSpell);
                if (temp != default(SpellMissile))
                {
                    Console.Out.WriteLine("Just shot a "+castSpell.spellName+" bullet.");
                    bullets.Add(temp);
                    bHasBullets = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

        }
    }
}
