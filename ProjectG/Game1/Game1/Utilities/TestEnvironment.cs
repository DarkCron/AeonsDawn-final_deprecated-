using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LUA;
using TBAGW.Utilities.Sprite;
using LUA;

namespace TBAGW.Utilities
{
    class TestEnvironment
    {
        public static bool bDoTest = true;
        public static void Test()
        {


            NLua.Lua state = new NLua.Lua();

            NLua.Lua state2 = new NLua.Lua();

            someData sd = new someData();
            sd.ID = 25;
            sd.sList.AddRange(new String[] { "this", " is", " a", " test", " list" });
            luaData ld = new luaData(sd);


            //var test = state.DoString("return 10 + 3*(5 + 2)")[0];
            //var type = test.GetType();

            //int value = 6;
            //state["test"] = value;
            //var result = state.DoString("return test*3+(test-3)")[0];

            //state.DoString(@"
            // function ScriptFunc (val1, val2)
            //     val1=val1+2
            //     val2=val2+3
            //     return val1, val2
            //  end");

            //var scriptFunc = state["ScriptFunc"] as NLua.LuaFunction;
            //var res = scriptFunc.Call(3, 5);

            testClass tc = new testClass("Some test string here");
            tc.list.Add(25);
            state["provision"] = "Random piece of information";



            testClass tc2 = new testClass("Indirect summon");
            tc2.data = ld;
            tc2.ProvideInfoToLua(state);
            //state["testObj"] = tc;

            state.LoadCLRPackage();
            // state.DoString(@" import ('The betrayer', 'TBAGW.Utilities') 
            //   import ('System.Web') 
            //    import ('System')");

            // var testStringFromObject = state.DoString("return TestObject.testList[0]")[0];
            // var elementType = testStringFromObject.GetType();
            // testClass testObjFromLUA = state.DoString("return TestObject")[0] as testClass;
            //bool bWhat = testObjFromLUA.GetType() == tc.GetType();

            state.DoString("print(\"Hello World\")");

            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "LUA files (*.lua)|*.lua";
            ofd.InitialDirectory = Game1.rootContent;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                state = new NLua.Lua();
                state.LoadCLRPackage();
                state.DoFile(ofd.FileName);
                var time = System.IO.File.GetLastWriteTime(ofd.FileName);
            }
            //var statsTest = ((state["generateSomeStats"] as NLua.LuaFunction).Call()[0] as LuaStatEdit).ExtractStatChart();

            //BaseSprite bs = new BaseSprite();
            //bs.position = new Vector2(136, 2556);
            //if (Scenes.Editor.MapBuilder.loadedMap != null)
            //{
            //    bs.UpdatePosition();
            //}
            // List<List<Object>> luaStuff = new List<List<object>>();
            //var ttt = (state["save"] as NLua.LuaFunction).Call()[0].GetType();
            (state["save"] as NLua.LuaFunction).Call();
            var st = LuaSaveData.getGlobalData("My Collection", "luaStuff");
            (state["loadProcess"] as NLua.LuaFunction).Call();
            //var dabdab = ((state["getShapeData"] as NLua.LuaFunction).Call(bs));
            //  NLua.LuaFunction savedFunction = state["e1.Test"] as NLua.LuaFunction;
            uic = ((state["createUI"] as NLua.LuaFunction).Call()[0] as LuaUICollection).ConvertFromLua(state);
            goto skip;
            var luaRect = ShapeAnimation.animFromLuaInfo(((state["generateAnimation"] as NLua.LuaFunction).Call()[0] as LuaShapeAnimationInfo));
            var resldo = (state["returnLuaSpecificData"] as NLua.LuaFunction).Call()[0];
            NLua.LuaTable lt = (state["returnLuaSpecificData"] as NLua.LuaFunction).Call()[0] as NLua.LuaTable;
            (state["GenerateSomeStuff"] as NLua.LuaFunction).Call();
            ListDataProvider.setLuaData(state);
            (state["publicStaticCall"] as NLua.LuaFunction).Call();
            (state["internalStaticCall"] as NLua.LuaFunction).Call();
            state["testObj"] = tc;
            var ele2 = (state["attempt"] as NLua.LuaFunction).Call()[0];
            var returnedData = (state["returnData"] as NLua.LuaFunction).Call()[0];
            var oldProvision = state["provision"];
            var value2 = state["testObj.i"];
            var function = state["MyFunc"] as NLua.LuaFunction;
            var funcRes = function.Call(state["testObj.i"])[0];
            var tableResult = (state["returnTable"] as NLua.LuaFunction).Call()[0];
            var bCompare1 = tableResult.GetType() == typeof(NLua.LuaTable);
            var tableToList = LUAUtilities.LUATableToListUtility(typeof(testClass), tableResult as NLua.LuaTable).Cast<testClass>().ToList();
            var tableToListGenericObject = LUAUtilities.LUATableToListUtility(typeof(testClass), tableResult as NLua.LuaTable);
            var listType = tableToList.GetType();
            var listTypeGO = tableToListGenericObject.GetType();



            state = new NLua.Lua();
            state.LoadCLRPackage();
            state.DoFile(ofd.FileName);
            List<testClass> tcl = new List<testClass>();
            tcl.Add(new testClass(""));
            tcl.Last().i = 25;
            tcl.Add(new testClass(""));
            tcl.Last().i = 0;
            tcl.Add(new testClass(""));
            tcl.Last().i = 10;
            foreach (var item in tcl)
            {
                (state["setStuff"] as NLua.LuaFunction).Call(item);
            }

            tcl = LUAUtilities.LUATableToListUtility(typeof(testClass), (state["doStuff"] as NLua.LuaFunction).Call()[0] as NLua.LuaTable).Cast<testClass>().ToList();
        skip: { }
            bDoTest = false;

        }

        static NLua.Lua npcState = null;
        public static void NPCTest(Object o)
        {
            NPC temp = o as NPC;
            if (npcState == null)
            {
                npcState = new NLua.Lua();
                npcState["NPC_Info"] = temp;
            }

        }

        public static UICollection uic;
        public static void Update(GameTime gt, Point trueMousePos)
        {

            if (uic != null)
            {
                // uic.startMainElement.position = new Point(150,70);
                trueMousePos = (Microsoft.Xna.Framework.Input.Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale).ToPoint();
                //  Console.WriteLine(trueMousePos);
                uic.Update(gt, trueMousePos);
            }
            else
            {
                if (Scenes.Editor.MapBuilder.gcDB.gameUICollections.Count >= 1)
                {
                    uic = Scenes.Editor.MapBuilder.gcDB.gameUICollections[0].parent;
                }

            }
        }

        public static void Draw(SpriteBatch sb)
        {
            if (uic != null)
            {
                if (Scenes.Editor.MapBuilder.gcDB.gameUICollections.Count == 0)
                {
                    UICollectionSave UICS = new UICollectionSave();
                    UICS.parent = uic;
                    Scenes.Editor.MapBuilder.gcDB.AddUICollection(UICS);
                }

                uic.Draw(sb);
            }

        }

        public static void UpdateRain(GameTime gt)
        {

            //Console.WriteLine(rain.Count);
            for (int i = 0; i < rain.Count; i++)
            {
                rain[i].Update(gt.ElapsedGameTime.Milliseconds);
            }
            rain.RemoveAll(rd => rd.Remove());

            for (int i = 0; i < GamePlayUtility.Randomize(8, 35) / GameProcessor.zoom / GameProcessor.zoom; i++)
            {
                if (rain.Count >= (RainParticle.limit / GameProcessor.zoom))
                {
                    break;
                }
                rain.Add(new RainParticle(new Point(GamePlayUtility.Randomize(-100, (int)(1366 / GameProcessor.zoom) + 100), GamePlayUtility.Randomize(-75, (int)(768 / GameProcessor.zoom) + 75)), new Point(GamePlayUtility.Randomize(8, 32))));
            }
        }

        static List<RainParticle> rain = new List<RainParticle>();
        static internal RenderTarget2D rainRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        public static void GenerateRainRender(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(rainRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
            for (int i = 0; i < rain.Count; i++)
            {
                rain[i].Draw(sb, Color.LightBlue);
            }
            sb.End();
        }

        internal static RenderTarget2D getRainRender()
        {
            return rainRender;
        }
    }

    class RainParticle
    {
        Texture2D t = Game1.contentManager.Load<Texture2D>(@"Graphics\Particles\rainTest");
        int fi = 0;
        static List<Rectangle> frames = new List<Rectangle>();
        int ftp = 0;
        int ftT = 150;
        public static int limit = 400;
        bool bRemove = false;
        Rectangle pos;

        public RainParticle(Point p, Point s = default(Point))
        {
            if (frames.Count == 0)
            {
                frames.Add(new Rectangle(0, 0, 16, 16));
                frames.Add(new Rectangle(16, 0, 16, 16));
                frames.Add(new Rectangle(32, 0, 16, 16));
                frames.Add(new Rectangle(48, 0, 16, 16));
            }

            Point size = s == default(Point) ? new Point(16) : s;
            pos = new Rectangle(p + new Point(-(GameProcessor.sceneCamera).ToPoint().X, -(GameProcessor.sceneCamera).ToPoint().Y), size);
        }

        public void Update(int t)
        {
            ftp += t;
            if (ftp >= ftT)
            {
                fi++;
                ftp = 0;
            }
            if (fi >= frames.Count)
            {
                bRemove = true;
            }
        }

        public void Draw(SpriteBatch sb, Color c)
        {
            sb.Draw(t, pos, frames[fi], c);
        }

        public bool Remove()
        {
            return bRemove;
        }
    }

    class testClass
    {
        public List<int> list = new List<int>();
        public String s;
        public int i;
        public double d;
        public luaData data;

        String luaProvider
        { get; set; }
        public String LuaProvider
        {
            get { if (luaProvider == null) { luaProvider = ""; } return luaProvider; }
            set { luaProvider = value; }
        }

        public testClass(String str)
        {
            s = str;
            i = 5;
            d = 22;

            LuaProvider =
            @"
            import ('The betrayer', 'TBAGW')
            
            provision = testClass('') 
            ";
        }


        public void doesNothing() { }

        public static void doSomething()
        {
            Console.WriteLine("static function called!");
        }

        public void ProvideInfoToLua(NLua.Lua L)
        {
            L.LoadCLRPackage();
            L.DoString("import ('The betrayer', 'TBAGW.Utilities') ");
            L["provisionState"] = this;
            L["provisionData"] = data;
        }
    }

    class luaData
    {
        Object obj;
        public luaData(Object o) { obj = o; }

        public Object getData()
        {
            return obj;
        }
    }

    class someData
    {
        public String name = "myName";
        public int ID = 56;
        public List<String> sList = new List<string>();
    }
}
