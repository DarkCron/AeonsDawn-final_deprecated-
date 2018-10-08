using TBAGW;
using TBAGW.Utilities.Actions;
using TBAGW.Utilities.GamePlay.Characters;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;
using TBAGW.Utilities;

namespace TBAGW.Scenes
{
    class Scene
    {
        public bool bIsInitialized = false;
        public List<object> activeSceneObjects = new List<object>();

        public List<IdentifiableShapeList> activeSceneButtonCollections = new List<IdentifiableShapeList>();
        public List<IdentifiableShapeList> activeSceneShapeCollections = new List<IdentifiableShapeList>();
        public List<IdentifiableShapeList> activeSceneCharactersCollections = new List<IdentifiableShapeList>();

        public float xAxis = 0;
        public float yAxis = 0;

        public virtual void Initialize(Game1 game)
        {
            bIsInitialized = true;
        }

        public virtual void Reload()
        {

        }

        public virtual void Update(GameTime gameTime, Game1 game)
        {
            SceneUtility.xAxis = xAxis;
            SceneUtility.yAxis = yAxis;
        }

        public virtual void UnloadContent(Game1 game)
        {

        }

        public virtual SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            return spriteBatch;
        }

    }
}
