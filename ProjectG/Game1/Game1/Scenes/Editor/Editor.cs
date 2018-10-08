using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TBAGW.Scenes.Editor
{
    class Editor : Scene
    {
        List<Scene> editors = new List<Scene>();

        GeneralEditor generalEditor = new GeneralEditor();
        SpriteEditor spriteEditor = new SpriteEditor();
        MapEditor mapEditor = new MapEditor();
        

        public enum EditorsCollection { GeneralEditor = 0 , SpriteEditor, MapEditor};

        static public int currentEditor = (int)(EditorsCollection.MapEditor);

        public int preScreen;

        //Needed by generalEditor
        public List<Scene> scenes = new List<Scene>();

        public override void Initialize(Game1 game)
        {
            base.Initialize(game);


            editors[currentEditor].Initialize(game);
        }

        public void Start(Game game)
        {
            generalEditor.scenes = scenes;
            spriteEditor.scenes = scenes;

            generalEditor.Start(game);
            editors.Add(generalEditor);
            editors.Add(spriteEditor);
            editors.Add(mapEditor);
        }

        public void ResetBackToGame(Game game) {
            generalEditor.ResetBackToGame(game);
        }

        public override void Update(GameTime gameTime, Game1 game)
        {
            editors[currentEditor].Update(gameTime,game);
        }


        
        public override void UnloadContent(Game1 game)
        {
            foreach (var editor in editors)
            {
                editor.UnloadContent(game);
            }
        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            editors[currentEditor].Draw(gametime,spriteBatch);

            return spriteBatch;
        }
        
    }
}
