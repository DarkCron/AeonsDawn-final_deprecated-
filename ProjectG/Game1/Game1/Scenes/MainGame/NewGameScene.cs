using TBAGW.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Scenes.MainGame
{
    class NewGameScene:Scene
    {
        public override void Initialize(Game1 game)
        {
            base.bIsInitialized = true;
            base.Initialize(game);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Game1 game)
        {
            base.Update(gameTime, game);
            SceneUtility.ChangeScene((int)(Game1.Screens.OWGame));
        }

    }
}
