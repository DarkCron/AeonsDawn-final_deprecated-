using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    public class ChangeTurnEffect
    {
        public Texture2D textureSheet;
        public List<Rectangle> frames = new List<Rectangle>();
        public int frameTimer = 62;
        public int timePassed = 0;
        public int frameIndex = 0;
        public bool bMustShow = false;

        public ChangeTurnEffect(Texture2D textureSheet, List<Rectangle> frames)
        {
            this.textureSheet = textureSheet;
            this.frames = frames;
        }

        public void ShowEffect()
        {
            bMustShow = true;
            frameIndex = 0;
            timePassed = 0;
        }

        public void Update(GameTime gameTime)
        {
            timePassed += gameTime.ElapsedGameTime.Milliseconds;
            if (timePassed > frameTimer)
            {
                timePassed = 0;
                frameIndex++;
                if (frameIndex > frames.Count - 1)
                {
                    bMustShow = false;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (bMustShow)
            {
                sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                sb.Draw(textureSheet, new Rectangle(0, 0, 1366, 768), frames[frameIndex], Color.White);
                if (frameIndex == frames.Count - 1)
                {
                    Reset();
                }
                sb.End();
            }
        }

        private void Reset()
        {
            frameIndex = 0;
            timePassed = 0;
            bMustShow = false;
            EncounterInfo.TurnEffectCompletedAfterChangeTurn();
        }
    }
}
