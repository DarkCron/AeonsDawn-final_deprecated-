#region Using Statements
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
#endregion

namespace TBAGW
{
#if WINDOWS || LINUX||STEAMWORKS_WIN||DRMFREE

    /// <summary>
    /// The main class.
    /// </summary>
    /// 
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// dsfs

        public static Thread mainThread;
        [STAThread]
        static void Main()
        {
        
#if DEBUG
            using (var game = new Game1())
            {
             
                game.Run();
                //if (Game1.bExited) { game.Exit(); }
            }
               

#elif !DEBUG
            using (var game = new Game1())
                try
                {
                    game.Run();
                }
                catch (Exception e)
                {
                    String docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\TBAGW\LOGS\";

                    docs += System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year + "-" + System.DateTime.Now.Minute + "-" + System.DateTime.Now.Hour + ".cglog";

                    if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\TBAGW\LOGS\"))
                    {
                        System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\TBAGW\LOGS\");
                    }

                    Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\TBAGW\LOGS\");

                    System.IO.StreamWriter sw = new System.IO.StreamWriter(docs);
                    sw.WriteLine(e.Message);
                    sw.Close();
                    game.Exit();
                }
#endif
            //CleanUpResources();
        }


    }
#endif
}
