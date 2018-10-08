using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Scenes.Editor;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    public static class TalentGridEditor
    {
        static public bool bIsRunning = false;
        static List<KeyValuePair<Rectangle, String>> gridCamera = new List<KeyValuePair<Rectangle, String>>();
        static List<KeyValuePair<Rectangle, String>> grid = new List<KeyValuePair<Rectangle, String>>();
        static Rectangle camera = new Rectangle();
        static bool bInitialize = true;
        static internal TalentGrid talentGrid = null;
        static Matrix m = Matrix.CreateTranslation(0, 0, 1);
        static internal bool bShowTalentTree = true;
        static CharacterClassCollection CCCRef;

        static void Initialize()
        {
            bInitialize = false;
            grid = new List<KeyValuePair<Rectangle, String>>();
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    grid.Add(new KeyValuePair<Rectangle, string>(new Rectangle(i * 64 + i * 32, j * 64 + j * 32, 64, 64), i + "," + j));
                    grid.Add(new KeyValuePair<Rectangle, string>(new Rectangle(-i * 64 - i * 32, -j * 64 - j * 32, 64, 64), -i + "," + -j));
                    grid.Add(new KeyValuePair<Rectangle, string>(new Rectangle(-i * 64 - i * 32, j * 64 + j * 32, 64, 64), -i + "," + j));
                    grid.Add(new KeyValuePair<Rectangle, string>(new Rectangle(i * 64 + i * 32, -j * 64 - j * 32, 64, 64), i + "," + -j));
                }
            }
        }

        static public void Start(BaseCharacter bc)
        {
            if (bInitialize) { Initialize(); }
            TalentGrid.mPos = new Point(0, 0);
            TalentGrid.mScale = 1f;
            talentGrid = new TalentGrid(bc.CCC.getEditorTalentNodesForGrid());
            bIsRunning = true;
            TalentGrid.bUpdateMatrix = true;
            CCCRef = bc.CCC;
        }

        static public void Update(GameTime gt)
        {
            camera = new Rectangle((int)((-1366 / 2 + TalentGrid.mPos.X + 32) * (1f / TalentGrid.mScale)), (int)((-768 / 2 + TalentGrid.mPos.Y + 32) * (1f / TalentGrid.mScale)), (int)(1366 * (1f / TalentGrid.mScale)), (int)(768 * (1f / TalentGrid.mScale)));
            UpdateController();

            if (TalentGrid.bUpdateMatrix)
            {
                gridCamera = grid.FindAll(r => camera.Contains(r.Key) || camera.Intersects(r.Key));
                m = Matrix.CreateTranslation(1366 / 2 - TalentGrid.mPos.X - 32, 768 / 2 - TalentGrid.mPos.Y - 32, 1);
            }
            talentGrid.Update(gt);
        }

        private static void UpdateController()
        {
            KeyboardState kbs = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            if (kbs.IsKeyDown(Keys.W))
            {
                TalentGrid.mPos.Y -= 3;
                TalentGrid.bUpdateMatrix = true;
            }

            if (kbs.IsKeyDown(Keys.S))
            {
                TalentGrid.mPos.Y += 3;
                TalentGrid.bUpdateMatrix = true;
            }

            if (kbs.IsKeyDown(Keys.A))
            {
                TalentGrid.mPos.X -= 3;
                TalentGrid.bUpdateMatrix = true;
            }

            if (kbs.IsKeyDown(Keys.D))
            {
                TalentGrid.mPos.X += 3;
                TalentGrid.bUpdateMatrix = true;
            }

            if (kbs.IsKeyDown(Keys.Space))
            {
                TalentGrid.mPos = new Point(0, 0);
                TalentGrid.mScale = 1f;
                TalentGrid.bUpdateMatrix = true;
                CCCRef.baseTalentSlot.Clear();
                CCCRef.actualTalentSlots.Clear();
                talentGrid = new TalentGrid(CCCRef.getEditorTalentNodesForGrid());
            }

            if (kbs.IsKeyDown(Keys.Tab))
            {
                bShowTalentTree = false;
            }
            else
            {
                bShowTalentTree = true;
            }

            if (ms.LeftButton == ButtonState.Pressed && kbs.IsKeyDown(Keys.LeftShift) && Game1.gameRef.GameHasMouse())
            {
                HandleLB();
            }

            if (ms.RightButton == ButtonState.Pressed && kbs.IsKeyDown(Keys.LeftShift) && Game1.gameRef.GameHasMouse())
            {
                HandleRB();
            }

            if (kbs.IsKeyDown(Keys.Escape))
            {
                bIsRunning = false;
                talentGrid.Close();
            }

        }

        private static void HandleLB()
        {
            Point p = Mouse.GetState().Position;
            p -= new Point(1366 / 2 - 32, 768 / 2 - 32);
            p += TalentGrid.mPos;

            var item = gridCamera.Find(gc => gc.Key.Contains(p));
            if (item.Value == null) { return; }
            var splitString = item.Value.Split(',');
            int x = int.Parse(splitString[0]);
            int y = int.Parse(splitString[1]);

            if (CCCRef.actualTalentSlots.Find(tn => tn.talentNode.nodePos == new Point(x, y)) == default(BaseTalentSlot))
            {
                CCCRef.baseTalentSlot.Add(new ClassUnlockTalent(new Point(x, y), CCCRef));
                CCCRef.actualTalentSlots.Add(new ClassUnlockTalent(new Point(x, y), CCCRef));
                talentGrid = new TalentGrid(CCCRef.getEditorTalentNodesForGrid());
                Console.WriteLine("Added talent node slot at : x: " + x + ", y: " + y);
            }

        }

        private static void HandleRB()
        {
            Point p = Mouse.GetState().Position;
            p -= new Point(1366 / 2 - 32, 768 / 2 - 32);
            p += TalentGrid.mPos;

            var item = gridCamera.Find(gc => gc.Key.Contains(p));
            if (item.Value == null) { return; }
            var splitString = item.Value.Split(',');
            int x = int.Parse(splitString[0]);
            int y = int.Parse(splitString[1]);

            if (CCCRef.actualTalentSlots.Find(tn => tn.talentNode.nodePos == new Point(x, y)) != default(BaseTalentSlot))
            {
                CCCRef.baseTalentSlot.Remove(CCCRef.baseTalentSlot.Find(tn => tn.talentNode.nodePos == new Point(x, y)));
                CCCRef.actualTalentSlots.Remove(CCCRef.actualTalentSlots.Find(tn => tn.talentNode.nodePos == new Point(x, y)));
                talentGrid = new TalentGrid(CCCRef.getEditorTalentNodesForGrid());
            }

        }

        internal static void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(Game1.gameRender);
            sb.GraphicsDevice.Clear(Color.Black);

            if (bShowTalentTree)
            {
                talentGrid.GenerateRenderEditor(sb);
                sb.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                sb.GraphicsDevice.Clear(Color.Black);
                sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                sb.Draw(talentGrid.getRender(), new Vector2(0), Color.White);
                sb.End();
                return;
            }

            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, m);


            foreach (var item in gridCamera)
            {


                sb.Draw(Game1.WhiteTex, item.Key, Color.Green);
                sb.DrawString(Game1.defaultFont, item.Value, item.Key.Location.ToVector2(), Color.White);

            }

            sb.End();
        }
    }
}
