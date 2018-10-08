using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Spells.ShotPattern
{
    class BasicPattern
    {
        protected Spell spell;
        protected InputControl spellTimer = new InputControl();
        protected float spellAngle = 0.0f;

        public BasicPattern(Spell spell=default(Spell))
        {
            this.spell = spell;
        }

        public SpellMissile Update(GameTime gameTime)
        {
            return default(SpellMissile);
        }
    }
}
