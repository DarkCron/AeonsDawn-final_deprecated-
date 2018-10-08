using TBAGW.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Scenes.MainGame
{
    class ContinueGameScene:Scene
    {
        public override void Initialize(Game1 game)
        {
            base.Initialize(game);
            base.bIsInitialized = true;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Game1 game)
        {

            base.Update(gameTime, game);
            SceneUtility.ChangeScene((int)(Game1.Screens.BGame));
        }
    }
}
