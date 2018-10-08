using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    public static class NiceText
    {
        static Effect fx = Game1.contentManager.Load<Effect>(@"FX\Text\TextLining");
        static List<TextInfo> niceTexts = new List<TextInfo>();
        static List<RenderTarget2D> renders = new List<RenderTarget2D>();
        static RenderTarget2D textRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

        public static void AddTextOrder(SpriteFont sf, String text, Vector2 pos, Color textColor, Color colorLining, int pixelLining)
        {
            niceTexts.Add(new TextInfo(sf, text, pos, textColor, colorLining, pixelLining));
        }

        public static RenderTarget2D DrawAll(SpriteBatch sb)
        {

            foreach (var item in niceTexts)
            {
                int width = (int)item.sf.MeasureString(item.text).X;
                int height = (int)item.sf.MeasureString(item.text).Y;
                renders.Add(new RenderTarget2D(sb.GraphicsDevice, width, height));
            }



            for (int i = 0; i < niceTexts.Count; i++)
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(renders[i]);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Immediate, null, SamplerState.LinearWrap);
                sb.DrawString(niceTexts[i].sf, niceTexts[i].text, Vector2.Zero, niceTexts[i].textColor);
                sb.End();
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(textRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.LinearWrap);
            //for (int i = 0; i < niceTexts.Count; i++)
            //{
            //    int width = renders[i].Width;
            //    int height = renders[i].Height;
            //    int lining = niceTexts[i].pixelLining;
            //    //float r = (float)colorLining.R / 255f;

            //    fx.Parameters["width"].SetValue(width);
            //    fx.Parameters["height"].SetValue(height);
            //    fx.Parameters["liningSize"].SetValue(lining);
            //    fx.Parameters["r"].SetValue((float)niceTexts[i].colorLining.R / 255f);
            //    fx.Parameters["g"].SetValue((float)niceTexts[i].colorLining.G / 255f);
            //    fx.Parameters["b"].SetValue((float)niceTexts[i].colorLining.B / 255f);
            //    fx.Parameters["a"].SetValue((float)niceTexts[i].colorLining.A / 255f);

            //    fx.CurrentTechnique.Passes[0].Apply();

            //    sb.Draw(renders[i], niceTexts[i].pos-new Vector2(lining,0), renders[i].Bounds, Color.White);
            //    //sb.DrawString(niceTexts[i].sf, niceTexts[i].text, Vector2.Zero, niceTexts[i].textColor);
            //}


            sb.End();

            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.LinearWrap);
            foreach (var item in niceTexts)
            {
                sb.DrawString(item.sf, item.text, item.pos + new Vector2(3), item.colorLining * .3f);
                sb.DrawString(item.sf, item.text, item.pos, item.textColor);
            }
            sb.Draw(textRender, textRender.Bounds, Color.White);
            sb.End();

            niceTexts.Clear();
            renders.Clear();

            return textRender;
        }

        public static void Draw(SpriteBatch sb, SpriteFont sf, String text, Vector2 pos, Color textColor, Color colorLining, int pixelLining)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Immediate);



            //int width = (int)sf.MeasureString(text).X;
            //int height = (int)sf.MeasureString(text).Y;
            //int lining = pixelLining;
            ////float r = (float)colorLining.R / 255f;

            //fx.Parameters["width"].SetValue(width);
            //fx.Parameters["height"].SetValue(height);
            //fx.Parameters["liningSize"].SetValue(lining);
            //fx.Parameters["r"].SetValue((float)colorLining.R / 256f);
            //fx.Parameters["g"].SetValue((float)colorLining.G / 256f);
            //fx.Parameters["b"].SetValue((float)colorLining.B / 256f);
            //fx.Parameters["a"].SetValue((float)colorLining.A / 256f);

            //fx.CurrentTechnique.Passes[0].Apply();


            sb.DrawString(sf, text, pos, textColor);

            sb.End();
        }
    }

    public class TextInfo
    {
        public SpriteFont sf;
        public String text;
        public Vector2 pos;
        public Color textColor;
        public Color colorLining;
        public int pixelLining;

        public TextInfo(SpriteFont sf, String text, Vector2 pos, Color textColor, Color colorLining, int pixelLining)
        {
            this.sf = sf;
            this.text = text;
            this.pos = pos;
            this.textColor = textColor;
            this.colorLining = colorLining;
            this.pixelLining = pixelLining;
        }
    }
}
