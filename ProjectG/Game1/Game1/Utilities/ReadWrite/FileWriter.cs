using TBAGW.Utilities.Actions;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TBAGW.Utilities.ReadWrite
{
    static class FileWriter
    {
        static String baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        static String fileLoc;
        static TestSerialize test = new TestSerialize(new Vector2(56,2));

        public static void Writer(String locationFolder, String fileName, IdentifiableShapeList objectList)
        {
            String folderLoc = Path.Combine(baseFolder, locationFolder);

            if (!Directory.Exists(folderLoc))
                Directory.CreateDirectory(folderLoc);

            fileLoc = Path.Combine(folderLoc, fileName);

            if (File.Exists(fileLoc))
                File.Delete(fileLoc);


            FileStream stream = File.Open(fileLoc, FileMode.OpenOrCreate);
            XmlSerializer xmlSer = new XmlSerializer(typeof(IdentifiableShapeList));
            xmlSer.Serialize(stream, objectList);
            stream.Close();

            FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);

            IdentifiableShapeList testIDList = (IdentifiableShapeList)(xmlSer.Deserialize(loadStream));
            if (testIDList.name.Equals("Controls Choice Buttons"))
            {
                foreach (InputButton item in testIDList.objectList)
                {
                    Console.Out.WriteLine(item.actionIndentifierString);
                }
            }

            loadStream.Close();
        }

        public static IdentifiableShapeList Reader(String locationFolder, String fileName)
        {
            String folderLoc = Path.Combine(baseFolder, locationFolder);

            fileLoc = Path.Combine(folderLoc, fileName);

            FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
            XmlSerializer xmlSer = new XmlSerializer(typeof(IdentifiableShapeList));
            IdentifiableShapeList idList = (IdentifiableShapeList)(xmlSer.Deserialize(loadStream));
            loadStream.Close();
            return idList;
        }

        public static void WriteActionKeyFile()
        {
            IdentifiableShapeList actionKeyList = new IdentifiableShapeList("Action Key List",Game1.actionKeyList);
            String locationFolder = @"My Games\The Betrayer\prefs\";
            String fileName = "actionKeys.xml";
            String folderLoc = Path.Combine(baseFolder, locationFolder);

            if (!Directory.Exists(folderLoc))
                Directory.CreateDirectory(folderLoc);

            fileLoc = Path.Combine(folderLoc, fileName);

            if (File.Exists(fileLoc))
                File.Delete(fileLoc);

            FileStream stream = File.Open(fileLoc, FileMode.OpenOrCreate);
            XmlSerializer xmlSer = new XmlSerializer(typeof(IdentifiableShapeList));
            xmlSer.Serialize(stream, actionKeyList);
            stream.Close();
        }

    }
}
