using TBAGW.Utilities.GamePlay.Characters.Friendly;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Characters
{
    static class CharacterAssetsLoader
    {
        public enum heroesC1 { hero1 = 0, hero2, hero3, hero4 }
        static public List<HeroAsset> heroAssetsC1 = new List<HeroAsset>();

        static public void loadAssets(Game game)
        {
            LoadC1(game);
        }

        static public Texture2D basicCharTexture;

        private static void LoadC1(Game game)
        {
            basicCharTexture = game.Content.Load<Texture2D>(@"Graphics\Characters\Friendly\BasicTeam\BasicTeam");

            heroAssetsC1.Add(new HeroAsset(basicCharTexture, new Rectangle(0, 0, 128, 96), new Rectangle(), new Rectangle(512, 0, 64, 64)));
            heroAssetsC1.Add(new HeroAsset(basicCharTexture, new Rectangle(128, 0, 128, 96), new Rectangle(), new Rectangle( 576, 0, 64, 64)));
            heroAssetsC1.Add(new HeroAsset(basicCharTexture, new Rectangle(256, 0, 128, 96), new Rectangle(), new Rectangle( 640, 0, 64, 64)));
            heroAssetsC1.Add(new HeroAsset(basicCharTexture, new Rectangle(384, 0, 128, 96), new Rectangle(), new Rectangle( 704, 0, 64, 64)));
            heroAssetsC1[(int)heroesC1.hero1].heroName = "Hero 1";
            heroAssetsC1[(int)heroesC1.hero2].heroName = "Hero 2";
            heroAssetsC1[(int)heroesC1.hero3].heroName = "Hero 3";
            heroAssetsC1[(int)heroesC1.hero4].heroName = "Hero 4";
        }



    }
}
