using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    public class StatDisplayChart
    {
        static public bool bInitialized = false;
        static SpriteFont sf32;
        static SpriteFont sf48;

        static Texture2D bigFrameSourceTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
        static Rectangle bigFrameSourceSize = new Rectangle(88, 679, 68, 68);
        static Texture2D smallFrameSourceTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
        static Rectangle smallFrameSourceSize = new Rectangle(156, 679, 148, 57);

        static RenderTarget2D r2d = new RenderTarget2D(Game1.graphics.GraphicsDevice, 352, 224);


        static public void Initialize()
        {
            if (!bInitialized)
            {
                BattleGUI.InitializeResources();
                bInitialized = true;

                sf32 = BattleGUI.testSF32;
                sf48 = BattleGUI.testSF48;
            }
        }

        Vector2 Location = Vector2.Zero;
        BaseCharacter charToDisplay;
        String HPString = "";
        String MPString = "";
        String STRString = "";
        String AGIString = "";
        String DEFString = "";
        String INTString = "";
        String MASString = "";

        //public StatDisplayChart() { }

        public StatDisplayChart(BaseCharacter bc, Vector2 Location)
        {
            Initialize();
            this.Location = Location + GameProcessor.sceneCamera;
            this.Location *= GameProcessor.zoom;

            if (this.Location.X + 352 > 1366)
            {
                this.Location.X += -(352 + 64 * 2);
            }

            if (this.Location.Y + 224 > 768)
            {
                this.Location.Y = 768 - 224 - 16;
            }

            this.charToDisplay = bc;
            HPString = bc.RemainingHP() + "HP" + " / " + bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP] + "HP";
            MPString = bc.RemainingMana() + "MP" + " / " + bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXMANA] + "MP";
            STRString = "STR: " + bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.STR];
            AGIString = "AGI: " + bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AGI];
            DEFString = "DEF: " + bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.DEF];
            INTString = "INT: " + bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.INT];
            MASString = "MAS: " + bc.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY];
        }


        public void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(r2d);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp);
            //TextUtility.ClearCache();
            sb.Draw(bigFrameSourceTex, new Rectangle(Point.Zero, r2d.Bounds.Size), bigFrameSourceSize, Color.White);

            int offsetX = 5;
            int offsetY = 5;
            Rectangle frameNameTextBox = new Rectangle(offsetX, offsetY, 352 - offsetX * 2, 64 - offsetY * 2);
            sb.Draw(smallFrameSourceTex, frameNameTextBox, smallFrameSourceSize, new Color(200, 200, 200, 255));

            Rectangle frameNameText = new Rectangle(offsetX, offsetY-2, 352 - offsetX * 2, 64 - offsetY * 2);
            TextUtility.Draw(sb, charToDisplay.displayName, sf48, frameNameText, TextUtility.OutLining.Center, Color.Black, 1f, false, default(Matrix), Color.White);

            int offsetYPos = 5;
            TextUtility.Draw(sb, HPString, sf32, new Rectangle(0, 64 - offsetYPos, 192, 32), TextUtility.OutLining.Right, Color.DarkGray, 1f, false);
            TextUtility.Draw(sb, MPString, sf32, new Rectangle(0, 82 + 50 - offsetYPos, 192, 32), TextUtility.OutLining.Right, Color.DarkGray, 1f, false);

            TextUtility.Draw(sb, STRString, sf32, new Rectangle(256, 64 - offsetYPos, 96, 32), TextUtility.OutLining.Left, Color.DarkGray, 1f, false);
            TextUtility.Draw(sb, DEFString, sf32, new Rectangle(256, 96 - offsetYPos, 96, 32), TextUtility.OutLining.Left, Color.DarkGray, 1f, false);
            TextUtility.Draw(sb, AGIString, sf32, new Rectangle(256, 128 - offsetYPos, 96, 32), TextUtility.OutLining.Left, Color.DarkGray, 1f, false);
            TextUtility.Draw(sb, INTString, sf32, new Rectangle(256, 160 - offsetYPos, 96, 32), TextUtility.OutLining.Left, Color.DarkGray, 1f, false);
            TextUtility.Draw(sb, MASString, sf32, new Rectangle(256, 192 - offsetYPos, 96, 32), TextUtility.OutLining.Left, Color.DarkGray, 1f, false);

            sb.End();
        }

        public void FinalDraw(SpriteBatch sb, Matrix m)
        {
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null);
            //     sb.Draw(r2d, new Rectangle(Point.Zero, r2d.Bounds.Size), Color.White);
            sb.Draw(r2d, new Rectangle((Location).ToPoint(), r2d.Bounds.Size), Color.White * 1f);
            sb.End();
        }
    }
}
