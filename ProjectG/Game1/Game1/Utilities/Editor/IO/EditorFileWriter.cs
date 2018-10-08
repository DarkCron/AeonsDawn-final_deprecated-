using TBAGW.Utilities.Actions;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.OnScreen.Particles;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;
using TBAGW.Utilities.SriptProcessing.ScriptTriggers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Utilities.Editor.IO;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;
using System.Xml.Linq;

namespace TBAGW.Utilities.ReadWrite
{
    static class EditorFileWriter
    {
        static String baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        static String fileLoc;
        static TestSerialize test = new TestSerialize(new Vector2(56, 2));

        internal static void Writer(String locationFolder, String fileName, IdentifiableShapeList objectList)
        {
            String folderLoc = Path.Combine(baseFolder, @"Cronix Games/TheBetrayer/" + locationFolder);

            if (!Directory.Exists(folderLoc))
                Directory.CreateDirectory(folderLoc);

            fileLoc = Path.Combine(folderLoc, fileName);

            if (File.Exists(fileLoc))
                File.Delete(fileLoc);


            FileStream stream = File.Open(fileLoc, FileMode.OpenOrCreate);
            XmlSerializer xmlSer = new XmlSerializer(typeof(IdentifiableShapeList));
            xmlSer.Serialize(stream, objectList);
            stream.Close();
        }



        //Only used for map creation
        internal static void MapWriter(String locationFolder, BasicMap map)
        {
            fileLoc = locationFolder;

            if (Game1.bIsDebug)
            {
                map.mapLocation = locationFolder.Replace(Game1.rootTBAGW, "");
            }
            else
            {
                map.mapLocation = locationFolder.Replace(Game1.rootContentExtra, "");
            }

            if (Game1.bIsDebug)
            {
                if (map.mapLocation.EndsWith(".cgmap"))
                {
                    map.mapLocation = map.mapLocation.Replace(".cgmap", ".cgmapc");
                }
            }

            if (!Directory.Exists(Path.GetDirectoryName(fileLoc)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileLoc));

            if (File.Exists(fileLoc))
                File.Delete(fileLoc);


            FileStream stream = File.Open(fileLoc, FileMode.OpenOrCreate);
            XmlSerializer xmlSer = new XmlSerializer(typeof(BasicMap));
            xmlSer.Serialize(stream, map);
            stream.Close();
            if (Game1.bIsDebug)
            {
                Encrypter.EncryptFile(locationFolder, locationFolder.Replace(".cgmap", ".cgmapc"));
            }
        }

        internal static void MapWriter(BasicMap map)
        {
            MapInfoChart MIC = new MapInfoChart();
            MIC.mapName = map.mapName;
            MIC.mapId = map.identifier;
            MIC.subMapIDs.Clear();
            foreach (var item in map.subMaps)
            {
                MIC.subMapIDs.Add(item.identifier);
            }

            String mapLoc = map.mapLocation;
            if (Game1.bIsDebug)
            {
                if (map.mapLocation.StartsWith(@"\"))
                {
                    fileLoc = Path.Combine(Game1.rootTBAGW, map.mapLocation.Substring(1));
                    MIC.mapLocation = map.mapLocation.Substring(1);
                }
                else
                {
                    fileLoc = Path.Combine(Game1.rootTBAGW, map.mapLocation);
                    MIC.mapLocation = map.mapLocation;
                }

                if (File.Exists(fileLoc))
                {
                    if (File.Exists(fileLoc + "BU"))
                    {
                        File.Delete(Path.Combine(fileLoc, fileLoc + "BU"));
                    }
                    File.Copy(fileLoc, fileLoc + "BU");
                }
                if (File.Exists(fileLoc.Replace(".cgmapc", ".cgmicc")))
                {
                    if (File.Exists(fileLoc.Replace(".cgmapc", ".cgmicc") + "BU"))
                    {
                        File.Delete(fileLoc.Replace(".cgmapc", ".cgmicc") + "BU");
                    }
                    File.Copy(fileLoc.Replace(".cgmapc", ".cgmicc"), fileLoc.Replace(".cgmapc", ".cgmicc") + "BU");
                }
                if (File.Exists(fileLoc.Replace(".cgmapc", ".cgdbc")))
                {
                    if (File.Exists(fileLoc.Replace(".cgmapc", ".cgdbc") + "BU"))
                    {
                        File.Delete(fileLoc.Replace(".cgmapc", ".cgdbc") + "BU");
                    }
                    File.Copy(fileLoc.Replace(".cgmapc", ".cgdbc"), fileLoc.Replace(".cgmapc", ".cgdbc") + "BU");
                }
                else if (!File.Exists(fileLoc.Replace(".cgmapc", ".cgdbc")))
                {
                    //MapBuilder.gcDB.gameAbilities = MapBuilder.loadedMap.gameAbilities;
                }

                if (fileLoc.EndsWith(".cgmapc")) //Create a temp map in temp, after that put the decrypted map in 
                {
                    String tempLoc = Path.Combine(Path.GetTempPath(), "TBAGW.cgmap");
                    String tempLocMIC = Path.Combine(Path.GetTempPath(), "TBAGW.cgmicc");
                    String tempLocDBC = Path.Combine(Path.GetTempPath(), "TBAGW.cgdbc");

                    FileStream loadStreamCGMAPC = File.Open(tempLoc, FileMode.OpenOrCreate);
                    XmlSerializer xmlSerCGMAPC = new XmlSerializer(typeof(BasicMap));
                    xmlSerCGMAPC.Serialize(loadStreamCGMAPC, map);
                    loadStreamCGMAPC.Close();

                    loadStreamCGMAPC = File.Open(tempLocMIC, FileMode.OpenOrCreate);
                    xmlSerCGMAPC = new XmlSerializer(typeof(MapInfoChart));
                    xmlSerCGMAPC.Serialize(loadStreamCGMAPC, MIC);
                    loadStreamCGMAPC.Close();

                    loadStreamCGMAPC = File.Open(tempLocDBC, FileMode.OpenOrCreate);
                    xmlSerCGMAPC = new XmlSerializer(typeof(GameContentDataBase));
                    xmlSerCGMAPC.Serialize(loadStreamCGMAPC, MapBuilder.gcDB);
                    loadStreamCGMAPC.Close();

                    MapBuilder.functionForm.Progress(50);
                    Encrypter.EncryptFile(tempLoc, fileLoc);
                    Encrypter.EncryptFile(tempLocMIC, fileLoc.Replace(".cgmapc", ".cgmicc"));
                    Encrypter.EncryptFile(tempLocDBC, fileLoc.Replace(".cgmapc", ".cgdbc"));

                    //DELETES TEMP FILE
                    if (File.Exists(tempLoc))
                        File.Delete(tempLoc);
                    if (File.Exists(tempLocMIC))
                        File.Delete(tempLocMIC);
                    if (File.Exists(tempLocDBC))
                        File.Delete(tempLocDBC);
                    if (File.Exists(fileLoc + "BU"))
                        File.Delete(fileLoc + "BU");
                    if (File.Exists(fileLoc.Replace(".cgmapc", ".cgmicc") + "BU"))
                        File.Delete(fileLoc.Replace(".cgmapc", ".cgmicc") + "BU");
                    if (File.Exists(fileLoc.Replace(".cgmapc", ".cgdbc") + "BU"))
                        File.Delete(fileLoc.Replace(".cgmapc", ".cgdbc") + "BU");
                    MapBuilder.functionForm.Progress(100);
                }
                else if (fileLoc.EndsWith(".cgmap")) //save cgmap then convert in cgmapc
                {
                    map.mapLocation = map.mapLocation.Replace(".cgmap", ".cgmapc");
                    FileStream streamCGMAP = File.Open(fileLoc, FileMode.OpenOrCreate);
                    XmlSerializer xmlSerCGMAP = new XmlSerializer(typeof(BasicMap));
                    xmlSerCGMAP.Serialize(streamCGMAP, map);
                    streamCGMAP.Close();
                    MapBuilder.functionForm.Progress(50);
                    Encrypter.EncryptFile(fileLoc, fileLoc.Replace(".cgmap", ".cgmapc"));
                    System.Windows.Forms.MessageBox.Show("Map was CGMAP converted to CGMAPC @" + fileLoc.Replace(".cgmap", ".cgmapc"));
                    MapBuilder.functionForm.Progress(100);
                    if (File.Exists(fileLoc + "BU"))
                        File.Delete(fileLoc + "BU");
                }
            }
            else
            {
                if (map.mapLocation.EndsWith(".cgmapc")) //Marks TBAGW MAP
                {
                    /*
                        if (map.mapLocation.StartsWith(@"\"))
                        {
                            fileLoc = Path.Combine(Game1.rootTBAGW, map.mapLocation.Substring(1));
                        }
                        else
                        {
                            fileLoc = Path.Combine(Game1.rootTBAGW, map.mapLocation);
                        }

                        if (File.Exists(fileLoc))
                        {
                            File.Copy(fileLoc, fileLoc + "BU");
                        }

                        if (fileLoc.EndsWith(".cgmapc")) //Create a temp map in temp, after that put the decrypted map in 
                        {
                            String tempLoc = Path.Combine(Path.GetTempPath(), "TBAGW.cgmap");

                            FileStream loadStreamCGMAPC = File.Open(tempLoc, FileMode.OpenOrCreate);
                            XmlSerializer xmlSerCGMAPC = new XmlSerializer(typeof(BasicMap));
                            xmlSerCGMAPC.Serialize(loadStreamCGMAPC, map);
                            loadStreamCGMAPC.Close();
                            MapBuilder.functionForm.Progress(50);
                            Encrypter.EncryptFile(tempLoc, fileLoc);
                            //DELETES TEMP FILE
                            if (File.Exists(tempLoc))
                                File.Delete(tempLoc);
                            if (File.Exists(fileLoc + "BU"))
                                File.Delete(fileLoc + "BU");
                            MapBuilder.functionForm.Progress(100);
                        }*/
                }
                else //everything else
                {
                    if (map.mapLocation.StartsWith(@"\"))
                    {
                        fileLoc = Path.Combine(Game1.rootContentExtra, map.mapLocation.Substring(1));
                    }
                    else
                    {
                        fileLoc = Path.Combine(Game1.rootContentExtra, map.mapLocation);
                    }

                    if (fileLoc.EndsWith(".cgmap")) //Create a temp map in temp, after that put the decrypted map in 
                    {
                        FileStream loadStreamCGMAP = File.Open(fileLoc, FileMode.OpenOrCreate);
                        XmlSerializer xmlSerCGMAP = new XmlSerializer(typeof(BasicMap));
                        xmlSerCGMAP.Serialize(loadStreamCGMAP, map);
                        loadStreamCGMAP.Close();

                        loadStreamCGMAP = File.Open(fileLoc.Replace(".cgmap", ".cgmic"), FileMode.OpenOrCreate);
                        xmlSerCGMAP = new XmlSerializer(typeof(MapInfoChart));
                        xmlSerCGMAP.Serialize(loadStreamCGMAP, MIC);
                        loadStreamCGMAP.Close();

                        loadStreamCGMAP = File.Open(fileLoc.Replace(".cgmap", ".cgdb"), FileMode.OpenOrCreate);
                        xmlSerCGMAP = new XmlSerializer(typeof(GameContentDataBase));
                        xmlSerCGMAP.Serialize(loadStreamCGMAP, MapBuilder.gcDB);
                        loadStreamCGMAP.Close();


                        MapBuilder.functionForm.Progress(50);
                        MapBuilder.functionForm.Progress(100);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Error wrong file extension, only CGMAP please file @" + map.mapLocation);
                    }
                }

            }
        }

        internal static void TileSheetWriter(String locationFolder, TileSheet tileSheet)
        {
            if (Game1.bIsDebug)
            {
                if (!locationFolder.StartsWith(Game1.rootTBAGW))
                {
                    if (tileSheet.tileSheetLocation.StartsWith(@"\"))
                    {
                        fileLoc = Path.Combine(Game1.rootTBAGW, tileSheet.tileSheetLocation.Substring(1).Replace(".cgtsc", ".cgts"));
                    }
                    else
                    {
                        fileLoc = Path.Combine(Game1.rootTBAGW, tileSheet.tileSheetLocation);
                    }
                }
                else
                {
                    fileLoc = locationFolder;
                }
            }
            else
            {
                if (!locationFolder.StartsWith(Game1.rootContentExtra))
                {
                    if (tileSheet.tileSheetLocation.StartsWith(@"\"))
                    {
                        fileLoc = Path.Combine(Game1.rootContentExtra, tileSheet.tileSheetLocation.Substring(1));
                    }
                    else
                    {
                        fileLoc = Path.Combine(Game1.rootContentExtra, tileSheet.tileSheetLocation);
                    }
                }
                else
                {
                    fileLoc = locationFolder;
                }
            }


            if (tileSheet.tileSheetLocation.Equals(""))
            {
                tileSheet.tileSheetLocation = fileLoc.Replace(Game1.root, "");
            }

            if (Game1.bIsDebug)
            {
                if (tileSheet.tileSheetLocation.EndsWith(".cgts"))
                {
                    tileSheet.tileSheetLocation = tileSheet.tileSheetLocation.Replace(".cgts", ".cgtsc");
                }
            }
            else
            {
                tileSheet.tileSheetLocation = fileLoc.Replace(Game1.rootContentExtra, "");
            }


            if (!Directory.Exists(Path.GetDirectoryName(fileLoc)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileLoc));
            }


            if (File.Exists(fileLoc))
                File.Delete(fileLoc);

            // tileSheet.tileSheetLocation = fileLoc.Replace(Game1.root, "");
            FileStream stream = File.Open(fileLoc, FileMode.OpenOrCreate);
            XmlSerializer xmlSer = new XmlSerializer(typeof(TileSheet));
            xmlSer.Serialize(stream, tileSheet);
            stream.Close();

            if (Game1.bIsDebug)
            {
                String input = fileLoc;
                if (fileLoc.EndsWith(".cgtsc"))
                {
                    String tempLoc = Path.Combine(Path.GetTempPath(), "TBAGW.cgts");

                    FileStream loadStreamCGTSC = File.Open(tempLoc, FileMode.OpenOrCreate);
                    XmlSerializer xmlSerCGTSC = new XmlSerializer(typeof(TileSheet));
                    xmlSerCGTSC.Serialize(loadStreamCGTSC, tileSheet);
                    loadStreamCGTSC.Close();

                    Encrypter.EncryptFile(tempLoc, fileLoc);
                    //DELETES TEMP FILE
                    if (File.Exists(tempLoc))
                        File.Delete(tempLoc);
                    if (File.Exists(fileLoc + "BU"))
                        File.Delete(fileLoc + "BU");
                }
                else if (fileLoc.EndsWith(".cgts"))
                {
                    String output = fileLoc.Replace(".cgts", ".cgtsc");
                    Encrypter.EncryptFile(fileLoc, output);
                }
            }
        }

        internal static void TileSheetEcnryptWrite(TileSheet ts, String otherFolder = "")
        {
            if (otherFolder.Equals(""))
            {
                String location = ts.tileSheetLocation;
                String fileLoc = "";
                if (location.StartsWith(@"\"))
                {
                    fileLoc = Path.Combine(Game1.rootTBAGW, location.Substring(1));
                }
                else
                {
                    fileLoc = Path.Combine(Game1.rootTBAGW, location);
                }


            }
            else
            {

            }

        }

        internal static TileSheet TileSheetReader(String location)
        {
            location = @"Maps\TileSheets\IDTestTS.cgtsc";
            String tileSheetLoc = location;
            if (Game1.bIsDebug)
            {
                #region debug region
                if (location.EndsWith(".cgts") || location.EndsWith(".cgtsc"))
                {
                    if (!location.StartsWith(Game1.rootTBAGW))
                    {
                        if (!location.StartsWith(@"\"))
                        {
                            fileLoc = Path.Combine(Game1.rootTBAGW, location);
                        }
                        else
                        {
                            fileLoc = Path.Combine(Game1.rootTBAGW, location.Substring(1));
                        }
                    }
                    else if (location.StartsWith(Game1.rootTBAGW))
                    {
                        fileLoc = location;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Please check tilesheet location, error with content location @" + location);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please check tilesheet location, error with file extension @" + location);
                }

                tileSheetLoc = fileLoc.Replace(Game1.rootTBAGW, "");
                if (tileSheetLoc.EndsWith(".cgts"))
                {
                    tileSheetLoc = tileSheetLoc.Replace(".cgts", ".cgtsc");
                }

                if (location.EndsWith(".cgts"))
                {
                    FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                    XmlSerializer xmlSer = new XmlSerializer(typeof(TileSheet));
                    TileSheet loadedTileSheet = (TileSheet)(xmlSer.Deserialize(loadStream));

                    if (loadedTileSheet.tileSheetLocation != tileSheetLoc)
                    {
                        loadStream.Close();
                        loadedTileSheet.tileSheetLocation = tileSheetLoc;
                        TileSheetWriter(tileSheetLoc, loadedTileSheet);
                    }

                    loadStream.Close();
                    loadedTileSheet.ReloadTextures();


                    return loadedTileSheet;
                }
                else if (location.EndsWith(".cgtsc"))
                {
                    fileLoc = Encrypter.DecryptFile(fileLoc);
                    FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                    XmlSerializer xmlSer = new XmlSerializer(typeof(TileSheet));
                    TileSheet loadedTileSheet = (TileSheet)(xmlSer.Deserialize(loadStream));

                    if (loadedTileSheet.tileSheetLocation != tileSheetLoc)
                    {
                        loadStream.Close();
                        loadedTileSheet.tileSheetLocation = tileSheetLoc;
                        TileSheetWriter(fileLoc, loadedTileSheet);
                    }

                    loadStream.Close();
                    loadedTileSheet.ReloadTextures();

                    //DELETES TEMP FILE
                    if (File.Exists(fileLoc))
                        File.Delete(fileLoc);
                    return loadedTileSheet;
                }
                #endregion debug reader
            }
            else
            {
                if (location.EndsWith(".cgts") || location.EndsWith(".cgtsc"))
                {
                    if (location.EndsWith(".cgtsc")) //MMARKS TBAGW CGTS
                    {
                        if (location.StartsWith(@"\TBAGW\"))
                        {
                            location = location.Replace(@"\TBAGW\", "");
                        }
                        else if (location.StartsWith(@"TBAGW\"))
                        {
                            location = location.Replace(@"TBAGW\", "");
                        }
                        if (!location.StartsWith(Game1.rootTBAGW))
                        {
                            if (!location.StartsWith(@"\"))
                            {
                                fileLoc = Path.Combine(Game1.rootTBAGW, location);
                            }
                            else
                            {
                                fileLoc = Path.Combine(Game1.rootTBAGW, location.Substring(1));
                            }
                        }
                        else if (location.StartsWith(Game1.rootTBAGW))
                        {
                            fileLoc = location;
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Please check tilesheet location, error with content location Must end with .cgtsc @" + location);
                        }

                        fileLoc = Encrypter.DecryptFile(fileLoc);
                        FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                        XmlSerializer xmlSer = new XmlSerializer(typeof(TileSheet));
                        TileSheet loadedTileSheet = (TileSheet)(xmlSer.Deserialize(loadStream));
                        loadStream.Close();

                        loadedTileSheet.ReloadTextures();

                        //DELETES TEMP FILE
                        if (File.Exists(fileLoc))
                            File.Delete(fileLoc);

                        return loadedTileSheet;

                    }
                    else if (location.EndsWith(".cgts"))
                    {
                        if (!location.StartsWith(Game1.rootContentExtra))
                        {
                            if (!location.StartsWith(@"\"))
                            {
                                fileLoc = Path.Combine(Game1.rootContentExtra, location);
                            }
                            else
                            {
                                fileLoc = Path.Combine(Game1.rootContentExtra, location.Substring(1));
                            }
                        }
                        else if (location.StartsWith(Game1.rootContentExtra))
                        {
                            fileLoc = location;
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Please check tilesheet location, error with content location, must end with .cgts @" + location);
                        }

                        FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                        XmlSerializer xmlSer = new XmlSerializer(typeof(TileSheet));
                        TileSheet loadedTileSheet = (TileSheet)(xmlSer.Deserialize(loadStream));
                        loadStream.Close();

                        loadedTileSheet.ReloadTextures();

                        return loadedTileSheet;
                    }

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please check tilesheet location, error with file extension must be either .cgts or .cgtsc @" + location);
                }
            }

            System.Windows.Forms.MessageBox.Show("Something went wrong loading: " + location);
            return default(TileSheet);


        }

        internal static BasicMap MapReader(String location)
        {
            BaseItem.itemIDLatest = 0;
            BaseCharacter.characterIDLatest = 0;

            if (Game1.bIsDebug)
            {
                MapInfoReader(location);
                if (!location.StartsWith(Game1.rootTBAGW))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootTBAGW, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootTBAGW, location);
                    }
                }
            }
            else
            {
                if (!location.StartsWith(Game1.rootContentExtra))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootContentExtra, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootContentExtra, location);
                    }
                }
            }

            if (!location.EndsWith(".cgmapc"))
            {
                fileLoc = Path.Combine(location);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(BasicMap));
                BasicMap loadedMap = (BasicMap)(xmlSer.Deserialize(loadStream));
                loadStream.Close();

                try
                {
                    loadStream = File.Open(fileLoc.Replace(".cgmap", ".cgdb"), FileMode.OpenOrCreate, FileAccess.Read);
                    xmlSer = new XmlSerializer(typeof(GameContentDataBase));
                    var tempddddf = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                    tempddddf.contentDatabaseLoc = fileLoc.Replace(".cgmap", ".cgdb");
                    MapBuilder.gcDB = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                    loadStream.Close();
                }
                catch
                {
                    Console.WriteLine("Error loading game content database");
                }

                GameProcessor.gcDB = MapBuilder.gcDB;
                loadedMap.ReloadMap();

                MapBuilder.gcDB.PostLoadCheck();
                loadedMap.PostSerializationReload(MapBuilder.gcDB);
                GameProcessor.loadedMap = loadedMap;
                return loadedMap;
            }
            else
            {
                //fileLoc = Encrypter.DecryptFile(location);
                String fileLocDB = location.Replace(".cgmapc", ".cgdbc");

                // FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                //var xDoc =  XDocument.Load(loadStream);
                //if (xDoc.Descendants("Equipment").Count() != 0)
                //{
                //    try
                //    {
                //        Console.WriteLine(xDoc.Descendants("Equipment").Count());
                //        xDoc.Descendants("Equipment").Remove();
                //    }
                //    catch (Exception)
                //    {

                //        throw;
                //    }
                //    Console.WriteLine("Removed some stuff, have a nice day!");
                //}
                //else
                //{
                //    xDoc = null;
                //}



                XmlSerializer xmlSer = new XmlSerializer(typeof(BasicMap));

                if (true)//(xDoc == null)
                {
                    // loadStream.Position = 0;
                    var esr = new EncrypterSpawn();
                    BasicMap loadedMap = (BasicMap)(xmlSer.Deserialize(esr.DecryptFileToStream(location)));
                    //loadStream.Close();
                    loadedMap.ReloadMap();


                    try
                    {
                        //loadStream = File.Open(fileLocDB, FileMode.OpenOrCreate, FileAccess.Read);
                        xmlSer = new XmlSerializer(typeof(GameContentDataBase));
                        // var tempddddf = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                        MapBuilder.gcDB = (GameContentDataBase)(xmlSer.Deserialize(esr.DecryptFileToStream(fileLocDB)));
                        MapBuilder.gcDB.contentDatabaseLoc = fileLocDB;
                        GameProcessor.gcDB = MapBuilder.gcDB;
                        GameProcessor.gcDB.contentDatabaseLoc = fileLocDB;
                        //loadStream.Close();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error loading game content database: \n" + e);
                    }

                    try
                    {
                        MapBuilder.gcDB.Reload();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error reloading game content database resources");
                    }

                    //DELETES TEMP FILE
                    //if (File.Exists(fileLoc))
                    //    File.Delete(fileLoc);
                    //if (File.Exists(fileLocDB))
                    //    File.Delete(fileLocDB);

                    MapBuilder.gcDB.PostLoadCheck();

                    MapBuilder.parentMap = loadedMap;
                    MapBuilder.loadedMap = loadedMap;
                    GameProcessor.parentMap = loadedMap;
                    GameProcessor.loadedMap = loadedMap;
                    loadedMap.PostSerializationReload(MapBuilder.gcDB);

                    return loadedMap;
                }


            }

        }

        internal static KeyValuePair<BasicMap, GameContentDataBase> GameMapReader(String location)
        {
            BaseItem.itemIDLatest = 0;
            BaseCharacter.characterIDLatest = 0;

            {
                //fileLoc = Encrypter.DecryptFile(location);
                //String fileLocDB = Encrypter.DecryptFile(location.Replace(".cgmapc", ".cgdbc"));

                //FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                //XmlSerializer xmlSer = new XmlSerializer(typeof(BasicMap));
                //GameContentDataBase gcdb = new GameContentDataBase();

                //loadStream.Close();
                BasicMap loadedMap = MapReader(location);


                //try
                //{
                //    loadStream = File.Open(fileLocDB, FileMode.OpenOrCreate, FileAccess.Read);
                //    xmlSer = new XmlSerializer(typeof(GameContentDataBase));
                //    // var tempddddf = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                //    gcdb = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                //    loadStream.Close();

                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine("Error loading game content database: \n" + e);
                //}

                //try
                //{
                //    gcdb.Reload();
                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("Error reloading game content database resources");
                //}

                //DELETES TEMP FILE
                //if (File.Exists(fileLoc))
                //    File.Delete(fileLoc);

                //loadedMap.ReloadMap();
                //gcdb.PostLoadCheck();
                //loadedMap.PostSerializationReload(gcdb);
                return new KeyValuePair<BasicMap, GameContentDataBase>(loadedMap, GameProcessor.gcDB);

                //else
                //{
                //    BasicMap loadedMap = (BasicMap)(xmlSer.Deserialize(xDoc.CreateReader()));
                //    loadStream.Close();
                //    loadedMap.ReloadMap();

                //    try
                //    {
                //        loadStream = File.Open(fileLocDB, FileMode.OpenOrCreate, FileAccess.Read);
                //        xmlSer = new XmlSerializer(typeof(GameContentDataBase));
                //        MapBuilder.gcDB = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                //        loadStream.Close();

                //    }
                //    catch
                //    {
                //        Console.WriteLine("Error loading game content database");
                //    }

                //    try
                //    {
                //        MapBuilder.gcDB.Reload();
                //        GameContentDataBase.characterIDLatest = MapBuilder.gcDB.characterID;
                //        GameContentDataBase.itemIDLatest = MapBuilder.gcDB.itemID;
                //    }
                //    catch (Exception)
                //    {
                //        Console.WriteLine("Error reloading game content database resources");
                //    }

                //    //DELETES TEMP FILE
                //    if (File.Exists(fileLoc))
                //        File.Delete(fileLoc);

                //    GameContentDataBase.characterIDLatest = MapBuilder.gcDB.characterID;
                //    GameContentDataBase.itemIDLatest = MapBuilder.gcDB.itemID;
                //    GameContentDataBase.objectIDLatest = MapBuilder.gcDB.objectID;
                //    MapBuilder.gcDB.PostLoadCheck();
                //    loadedMap.PostSerializationReload(MapBuilder.gcDB);
                //    return loadedMap;
                //}

            }

        }

        internal static BasicMap MapReaderSimple(String location)
        {
            BaseItem.itemIDLatest = 0;
            BaseCharacter.characterIDLatest = 0;

            if (Game1.bIsDebug)
            {
                //MapInfoReader(location);
                if (!location.StartsWith(Game1.rootTBAGW))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootTBAGW, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootTBAGW, location);
                    }
                }
            }
            else
            {
                if (!location.StartsWith(Game1.rootContentExtra))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootContentExtra, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootContentExtra, location);
                    }
                }
            }

            if (!location.EndsWith(".cgmapc"))
            {
                fileLoc = Path.Combine(location);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(BasicMap));
                BasicMap loadedMap = (BasicMap)(xmlSer.Deserialize(loadStream));
                loadStream.Close();

                try
                {
                    loadStream = File.Open(fileLoc.Replace(".cgmap", ".cgdb"), FileMode.OpenOrCreate, FileAccess.Read);
                    xmlSer = new XmlSerializer(typeof(GameContentDataBase));
                    var tempddddf = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                    MapBuilder.gcDB = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                    loadStream.Close();
                }
                catch
                {
                    Console.WriteLine("Error loading game content database");
                }


                loadedMap.ReloadMap();

                MapBuilder.gcDB.PostLoadCheck();
                loadedMap.PostSerializationReload(MapBuilder.gcDB);
                GameProcessor.gcDB = MapBuilder.gcDB;
                GameProcessor.loadedMap = loadedMap;
                return loadedMap;
            }
            else
            {
                fileLoc = Encrypter.DecryptFile(location);
                // String fileLocDB = Encrypter.DecryptFile(location.Replace(".cgmapc", ".cgdbc"));

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var xDoc = XDocument.Load(loadStream);
                if (xDoc.Descendants("Equipment").Count() != 0)
                {
                    try
                    {
                        Console.WriteLine(xDoc.Descendants("Equipment").Count());
                        xDoc.Descendants("Equipment").Remove();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    Console.WriteLine("Removed some stuff, have a nice day!");
                }
                else
                {
                    xDoc = null;
                }



                XmlSerializer xmlSer = new XmlSerializer(typeof(BasicMap));

                if (xDoc == null)
                {
                    loadStream.Position = 0;
                    BasicMap loadedMap = (BasicMap)(xmlSer.Deserialize(loadStream));
                    loadStream.Close();
                    loadedMap.ReloadMap();


                    //try
                    //{
                    //    loadStream = File.Open(fileLocDB, FileMode.OpenOrCreate, FileAccess.Read);
                    //    xmlSer = new XmlSerializer(typeof(GameContentDataBase));
                    //    // var tempddddf = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                    //    MapBuilder.gcDB = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                    //    loadStream.Close();

                    //}
                    //catch (Exception e)
                    //{
                    //    Console.WriteLine("Error loading game content database: \n" + e);
                    //}

                    //try
                    //{
                    //    MapBuilder.gcDB.Reload();
                    //    GameContentDataBase.characterIDLatest = MapBuilder.gcDB.characterID;
                    //    GameContentDataBase.itemIDLatest = MapBuilder.gcDB.itemID;
                    //}
                    //catch (Exception)
                    //{
                    //    Console.WriteLine("Error reloading game content database resources");
                    //}

                    //DELETES TEMP FILE
                    if (File.Exists(fileLoc))
                        File.Delete(fileLoc);

                    MapBuilder.gcDB.PostLoadCheck();
                    loadedMap.PostSerializationReload(MapBuilder.gcDB);
                    return loadedMap;
                }
                else
                {
                    BasicMap loadedMap = (BasicMap)(xmlSer.Deserialize(xDoc.CreateReader()));
                    loadStream.Close();
                    loadedMap.ReloadMap();

                    //try
                    //{
                    //    loadStream = File.Open(fileLocDB, FileMode.OpenOrCreate, FileAccess.Read);
                    //    xmlSer = new XmlSerializer(typeof(GameContentDataBase));
                    //    MapBuilder.gcDB = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                    //    loadStream.Close();

                    //}
                    //catch
                    //{
                    //    Console.WriteLine("Error loading game content database");
                    //}

                    //try
                    //{
                    //    MapBuilder.gcDB.Reload();
                    //    GameContentDataBase.characterIDLatest = MapBuilder.gcDB.characterID;
                    //    GameContentDataBase.itemIDLatest = MapBuilder.gcDB.itemID;
                    //}
                    //catch (Exception)
                    //{
                    //    Console.WriteLine("Error reloading game content database resources");
                    //}

                    //DELETES TEMP FILE
                    if (File.Exists(fileLoc))
                        File.Delete(fileLoc);

                    //GameContentDataBase.characterIDLatest = MapBuilder.gcDB.characterID;
                    //GameContentDataBase.itemIDLatest = MapBuilder.gcDB.itemID;
                    //GameContentDataBase.objectIDLatest = MapBuilder.gcDB.objectID;
                    //MapBuilder.gcDB.PostLoadCheck();
                    //loadedMap.PostSerializationReload(MapBuilder.gcDB);
                    return loadedMap;
                }

            }

        }

        internal static GameContentDataBase gcdbReader(String location)
        {
            BaseItem.itemIDLatest = 0;
            BaseCharacter.characterIDLatest = 0;

            if (Game1.bIsDebug || true)
            {
                if (!location.StartsWith(Game1.rootTBAGW))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootTBAGW, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootTBAGW, location);
                    }
                }
            }
            else
            {
                if (!location.StartsWith(Game1.rootContentExtra))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootContentExtra, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootContentExtra, location);
                    }
                }
            }

            if (!location.EndsWith(".cgdbc"))
            {
                fileLoc = Path.Combine(location);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(GameContentDataBase));
                GameContentDataBase gcdb = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                loadStream.Close();

                gcdb.PostLoadCheck();
                return gcdb;
            }
            else
            {
                fileLoc = Encrypter.DecryptFile(location);
                String fileLocDB = Encrypter.DecryptFile(location.Replace(".cgmapc", ".cgdbc"));

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.ReadWrite);


                XmlSerializer xmlSer = new XmlSerializer(typeof(GameContentDataBase));

                loadStream.Position = 0;
                GameContentDataBase gcdb = (GameContentDataBase)(xmlSer.Deserialize(loadStream));
                loadStream.Close();

                try
                {
                    gcdb.Reload();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error reloading game content database resources");
                }

                //DELETES TEMP FILE
                if (File.Exists(fileLoc))
                    File.Delete(fileLoc);

                gcdb.PostLoadCheck();
                return gcdb;
            }

        }

        private static void MapInfoReader(string location)
        {
            if (Game1.bIsDebug)
            {
                if (!location.StartsWith(Game1.rootTBAGW))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootTBAGW, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootTBAGW, location);
                    }
                }
            }
            else
            {
                if (!location.StartsWith(Game1.rootContentExtra))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootContentExtra, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootContentExtra, location);
                    }
                }
            }

            if (!location.EndsWith(".cgmapc")) //Non debug map
            {
                fileLoc = Path.Combine(location.Replace(".cgmap", ".cgmic"));

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(MapInfoChart));
                try
                {
                    MapInfoChart loadedMapInfo = (MapInfoChart)(xmlSer.Deserialize(loadStream));
                }
                catch
                {
                    Console.WriteLine("Error found retrieving map info, do not be alarmed! This error gets thrown on first map creation.");
                }

                loadStream.Close();
            }
            else
            {
                fileLoc = Encrypter.DecryptFile(location.Replace(".cgmapc", ".cgmicc"));

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(MapInfoChart));
                try
                {
                    MapInfoChart loadedMapInfo = (MapInfoChart)(xmlSer.Deserialize(loadStream));
                }
                catch
                {
                    Console.WriteLine("Error found retrieving map info, do not be alarmed! This error gets thrown on first map creation.");
                }
                loadStream.Close();
                Console.WriteLine("Map info correctly loaded!");

                //DELETES TEMP FILE
                if (File.Exists(fileLoc))
                    File.Delete(fileLoc);
            }
        }

        internal static IdentifiableShapeList Reader(String locationFolder, String fileName)
        {
            String folderLoc = Path.Combine(baseFolder, locationFolder);

            fileLoc = Path.Combine(folderLoc, fileName);

            FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
            XmlSerializer xmlSer = new XmlSerializer(typeof(IdentifiableShapeList));
            IdentifiableShapeList idList = (IdentifiableShapeList)(xmlSer.Deserialize(loadStream));
            loadStream.Close();
            return idList;
        }

        internal static void WriteActionKeyFile()
        {
            IdentifiableShapeList actionKeyList = new IdentifiableShapeList("Action Key List", Game1.actionKeyList);
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

        internal static void ScriptObjectWriter(String location, ScriptTestObject obj)
        {

            if (!Directory.Exists(location))
                Directory.CreateDirectory(location);

            fileLoc = Path.Combine(location, obj.script.identifier + ".cgscript");

            if (File.Exists(fileLoc))
                File.Delete(fileLoc);

            FileStream stream = File.Open(fileLoc, FileMode.OpenOrCreate);
            XmlSerializer xmlSer = new XmlSerializer(typeof(ScriptTestObject));
            xmlSer.Serialize(stream, obj);
            stream.Close();
        }

        internal static ScriptTestObject ScriptObjectReader(String location)
        {
            fileLoc = Path.Combine(location);

            FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
            XmlSerializer xmlSer = new XmlSerializer(typeof(ScriptTestObject));
            ScriptTestObject obj = (ScriptTestObject)(xmlSer.Deserialize(loadStream));
            loadStream.Close();
            return obj;
        }

        internal static void TriggerWriter(String location, BasicTrigger obj)
        {

            if (!Directory.Exists(location))
                Directory.CreateDirectory(location);

            fileLoc = Path.Combine(location, obj.script.identifier + ".cgt");

            if (File.Exists(fileLoc))
                File.Delete(fileLoc);

            FileStream stream = File.Open(fileLoc, FileMode.OpenOrCreate);
            XmlSerializer xmlSer = new XmlSerializer(typeof(BasicTrigger));
            xmlSer.Serialize(stream, obj);
            stream.Close();
        }

        internal static BasicTrigger TriggerReader(String location)
        {
            fileLoc = Path.Combine(location);

            FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
            XmlSerializer xmlSer = new XmlSerializer(typeof(BasicTrigger));
            BasicTrigger obj = (BasicTrigger)(xmlSer.Deserialize(loadStream));
            loadStream.Close();
            return obj;
        }

        internal static void BasicSpriteWriter(String location, BaseSprite obj)
        {

            if (Game1.bIsDebug)
            {
                obj.spriteLocation = location.Replace(Game1.rootTBAGW, "");
            }
            else
            {
                obj.spriteLocation = location.Replace(Game1.rootContentExtra, "");
            }

            if (File.Exists(location))
                File.Delete(location);

            if (location.EndsWith(".cgbsc"))
            {
                fileLoc = location;
                fileLoc = location.Replace(".cgbsc", ".cgbs");
            }
            else
            {
                fileLoc = location;
            }

            Console.WriteLine("==? " + (obj.GetType() == typeof(BaseCharacter)));
            Console.WriteLine("EQUALS? " + (obj.GetType().Equals(typeof(BaseCharacter))));
            XmlSerializer xmlSer = default(XmlSerializer);
            if (obj.GetType() == typeof(BaseSprite))
            {
                xmlSer = new XmlSerializer(typeof(BaseSprite));
            }
            else if (obj.GetType() == typeof(BaseCharacter))
            {

                xmlSer = new XmlSerializer(typeof(BaseCharacter));
                fileLoc = fileLoc.Replace(".cgbcc", ".cgbc");
                Console.WriteLine("FileLoc: " + fileLoc + " Location: " + location);
            }

            FileStream stream = File.Open(fileLoc, FileMode.OpenOrCreate);
            xmlSer.Serialize(stream, obj);
            stream.Close();

            if (Game1.bIsDebug)
            {
                Encrypter.EncryptFile(fileLoc, location);
                File.Delete(fileLoc);
            }
        }

        internal static BaseSprite BasicSpriteReader(String location)
        {
            if (location.EndsWith(".cgbsc"))
            {
                location = Path.Combine(Game1.rootTBAGW, location);
                fileLoc = Encrypter.DecryptFile(location);
            }
            else
            {
                location = Path.Combine(Game1.rootContentExtra, location);
                fileLoc = Path.Combine(location);
            }

            FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
            XmlSerializer xmlSer = new XmlSerializer(typeof(BaseSprite));
            BaseSprite obj = (BaseSprite)(xmlSer.Deserialize(loadStream));
            loadStream.Close();
            obj.ReloadTextures();
            return obj;
        }

        internal static BaseCharacter BaseCharacterReader(String location)
        {
            if (location.EndsWith(".cgbcc"))
            {
                location = Path.Combine(Game1.rootTBAGW, location);
                fileLoc = Encrypter.DecryptFile(location);
            }
            else
            {
                location = Path.Combine(Game1.rootContentExtra, location);
                fileLoc = Path.Combine(location);
            }

            FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var xDoc = XDocument.Load(loadStream);
            if (xDoc.Descendants("Equipment").Count() != 0)
            {
                try
                {
                    Console.WriteLine(xDoc.Descendants("Equipment").Count());
                    xDoc.Descendants("Equipment").Remove();
                }
                catch (Exception)
                {

                    throw;
                }
                Console.WriteLine("Removed some stuff, have a nice day!");

                XmlSerializer xmlSer = new XmlSerializer(typeof(BaseCharacter));
                BaseCharacter obj = (BaseCharacter)(xmlSer.Deserialize(xDoc.CreateReader()));
                loadStream.Close();
                obj.ReloadTextures();
                return obj;
            }
            else
            {
                loadStream.Position = 0;
                XmlSerializer xmlSer = new XmlSerializer(typeof(BaseCharacter));
                BaseCharacter obj = (BaseCharacter)(xmlSer.Deserialize(loadStream));
                loadStream.Close();
                obj.ReloadTextures();
                return obj;
            }


        }

        internal static void ClassWriter(BaseClass baseClass)
        {
            if (Game1.bIsDebug)
            {
                if (baseClass.ClassLoc.StartsWith(@"\"))
                {
                    fileLoc = Path.Combine(Game1.rootTBAGW, baseClass.ClassLoc.Substring(1));
                }
                else
                {
                    fileLoc = Path.Combine(Game1.rootTBAGW, baseClass.ClassLoc);
                }

                if (File.Exists(fileLoc))
                {
                    File.Copy(fileLoc, fileLoc + "BU");
                }

                if (fileLoc.EndsWith(".cgclc")) //Create a temp map in temp, after that put the decrypted map in 
                {
                    String tempLoc = Path.Combine(Path.GetTempPath(), "TBAGW.cgclc");

                    FileStream loadStreamCGCLC = File.Open(tempLoc, FileMode.OpenOrCreate);
                    XmlSerializer xmlSerCGCLC = new XmlSerializer(typeof(BaseClass));
                    xmlSerCGCLC.Serialize(loadStreamCGCLC, baseClass);
                    loadStreamCGCLC.Close();
                    Encrypter.EncryptFile(tempLoc, fileLoc);
                    //DELETES TEMP FILE
                    if (File.Exists(tempLoc))
                        File.Delete(tempLoc);
                    if (File.Exists(fileLoc + "BU"))
                        File.Delete(fileLoc + "BU");
                }
                else if (fileLoc.EndsWith(".cgcl")) //save cgmap then convert in cgmapc
                {
                    baseClass.ClassLoc = baseClass.ClassLoc.Replace(".cgcl", ".cgclc");
                    FileStream streamCGCL = File.Open(fileLoc, FileMode.OpenOrCreate);
                    XmlSerializer xmlSerCGCL = new XmlSerializer(typeof(BaseClass));
                    xmlSerCGCL.Serialize(streamCGCL, baseClass);
                    streamCGCL.Close();
                    Encrypter.EncryptFile(fileLoc, fileLoc.Replace(".cgcl", ".cgclc"));
                    if (File.Exists(fileLoc + "BU"))
                        File.Delete(fileLoc + "BU");
                }
            }
            else //When not in debug mode
            {
                {
                    if (baseClass.ClassLoc.StartsWith(@"\"))
                    {
                        fileLoc = Path.Combine(Game1.rootContentExtra, baseClass.ClassLoc.Substring(1));
                    }
                    else
                    {
                        fileLoc = Path.Combine(Game1.rootContentExtra, baseClass.ClassLoc);
                    }

                    if (fileLoc.EndsWith(".cgcl"))
                    {
                        FileStream loadStreamCGCL = File.Open(fileLoc, FileMode.OpenOrCreate);
                        XmlSerializer xmlSerCGCL = new XmlSerializer(typeof(BaseClass));
                        xmlSerCGCL.Serialize(loadStreamCGCL, baseClass);
                        loadStreamCGCL.Close();
                    }
                }
            }
        }

        internal static BaseClass ClassReader(String location)
        {
            if (Game1.bIsDebug)
            {
                if (!location.StartsWith(Game1.rootTBAGW))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootTBAGW, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootTBAGW, location);
                    }
                }
            }
            else
            {
                if (!location.StartsWith(Game1.rootContentExtra))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootContentExtra, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootContentExtra, location);
                    }
                }
            }

            if (!location.EndsWith(".cgclc"))
            {
                fileLoc = Path.Combine(location);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(BaseClass));
                BaseClass loadedClass = (BaseClass)(xmlSer.Deserialize(loadStream));
                loadStream.Close();
                return loadedClass;
            }
            else
            {
                fileLoc = Encrypter.DecryptFile(location);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(BaseClass));
                BaseClass loadedClass = (BaseClass)(xmlSer.Deserialize(loadStream));
                loadStream.Close();

                //DELETES TEMP FILE
                if (File.Exists(fileLoc))
                    File.Delete(fileLoc);

                return loadedClass;
            }

        }

        internal static void BGWriter(BGInfo bgi, String f)
        {
            if (Game1.bIsDebug)
            {
                fileLoc = f + ".cgbgic";

                if (File.Exists(fileLoc))
                {
                    File.Copy(fileLoc, fileLoc + "BU");
                }

                if (fileLoc.EndsWith(".cgbgic")) //Create a temp map in temp, after that put the decrypted map in 
                {
                    String tempLoc = Path.Combine(Path.GetTempPath(), "TBAGW.cgbgic");

                    FileStream loadStreamCGCLC = File.Open(tempLoc, FileMode.OpenOrCreate);
                    XmlSerializer xmlSerCGCLC = new XmlSerializer(typeof(BGInfo));
                    xmlSerCGCLC.Serialize(loadStreamCGCLC, bgi);
                    loadStreamCGCLC.Close();
                    Encrypter.EncryptFile(tempLoc, fileLoc);
                    //DELETES TEMP FILE
                    if (File.Exists(tempLoc))
                        File.Delete(tempLoc);
                    if (File.Exists(fileLoc + "BU"))
                        File.Delete(fileLoc + "BU");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Change save ending please, in '.cgbgic' please");
                }
            }
            else //When not in debug mode
            {
                {
                    if (bgi.songELoc.StartsWith(@"\"))
                    {
                        fileLoc = Path.Combine(Game1.rootContentExtra, bgi.songELoc.Substring(1));
                    }
                    else
                    {
                        fileLoc = Path.Combine(Game1.rootContentExtra, bgi.songELoc);
                    }

                    if (fileLoc.EndsWith(".cgbgi"))
                    {
                        FileStream loadStreamCGCL = File.Open(fileLoc, FileMode.OpenOrCreate);
                        XmlSerializer xmlSerCGCL = new XmlSerializer(typeof(BGInfo));
                        xmlSerCGCL.Serialize(loadStreamCGCL, bgi);
                        loadStreamCGCL.Close();
                    }
                }
            }
        }

        internal static BGInfo BGReader(String location)
        {
            if (Game1.bIsDebug)
            {
                if (!location.StartsWith(Game1.rootContent))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootContent, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootContent, location);
                    }
                }
            }
            else
            {
                if (!location.StartsWith(Game1.rootContent))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootContent, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootContent, location);
                    }
                }
            }

            if (!location.EndsWith(".cgbgic"))
            {
                fileLoc = Path.Combine(location);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(BGInfo));
                BGInfo bgi = (BGInfo)(xmlSer.Deserialize(loadStream));
                loadStream.Close();
                return bgi;
            }
            else
            {
                fileLoc = Encrypter.DecryptFile(location);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(BGInfo));
                BGInfo bgi = (BGInfo)(xmlSer.Deserialize(loadStream));
                loadStream.Close();

                //DELETES TEMP FILE
                if (File.Exists(fileLoc))
                    File.Delete(fileLoc);

                return bgi;
            }

        }

        internal static void SFXWriter(SFXInfo sfxi)
        {
            if (Game1.bIsDebug)
            {
                if (sfxi.sfxLoc.StartsWith(@"\"))
                {
                    fileLoc = Path.Combine(Game1.rootContent, sfxi.sfxLoc.Substring(1));
                }
                else
                {
                    fileLoc = Path.Combine(Game1.rootContent, sfxi.sfxLoc);
                }

                if (File.Exists(fileLoc))
                {
                    File.Copy(fileLoc, fileLoc + "BU");
                }

                if (fileLoc.EndsWith(".cgsfxc")) //Create a temp map in temp, after that put the decrypted map in 
                {
                    String tempLoc = Path.Combine(Path.GetTempPath(), "TBAGW.cgsfxc");

                    FileStream loadStreamCGCLC = File.Open(tempLoc, FileMode.OpenOrCreate);
                    XmlSerializer xmlSerCGCLC = new XmlSerializer(typeof(SFXInfo));
                    xmlSerCGCLC.Serialize(loadStreamCGCLC, sfxi);
                    loadStreamCGCLC.Close();
                    Encrypter.EncryptFile(tempLoc, fileLoc);
                    //DELETES TEMP FILE
                    if (File.Exists(tempLoc))
                        File.Delete(tempLoc);
                    if (File.Exists(fileLoc + "BU"))
                        File.Delete(fileLoc + "BU");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Change save ending please, in '.cgsfxc' please");
                }
            }
            else //When not in debug mode
            {
                {
                    if (sfxi.sfxLoc.StartsWith(@"\"))
                    {
                        fileLoc = Path.Combine(Game1.rootContentExtra, sfxi.sfxLoc.Substring(1));
                    }
                    else
                    {
                        fileLoc = Path.Combine(Game1.rootContentExtra, sfxi.sfxLoc);
                    }

                    if (fileLoc.EndsWith(".cgsfx"))
                    {
                        FileStream loadStreamCGCL = File.Open(fileLoc, FileMode.OpenOrCreate);
                        XmlSerializer xmlSerCGCL = new XmlSerializer(typeof(SFXInfo));
                        xmlSerCGCL.Serialize(loadStreamCGCL, sfxi);
                        loadStreamCGCL.Close();
                    }
                }
            }
        }

        internal static SFXInfo SFXReader(String location)
        {
            if (Game1.bIsDebug)
            {
                if (!location.StartsWith(Game1.rootContent))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootContent, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootContent, location);
                    }
                }
            }
            else
            {
                if (!location.StartsWith(Game1.rootContent))
                {
                    if (location.StartsWith(@"\"))
                    {
                        location = Path.Combine(Game1.rootContent, location.Substring(1));
                    }
                    else
                    {
                        location = Path.Combine(Game1.rootContent, location);
                    }
                }
            }

            if (!location.EndsWith(".cgsfxc"))
            {
                fileLoc = Path.Combine(location);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(SFXInfo));
                SFXInfo sfx = (SFXInfo)(xmlSer.Deserialize(loadStream));
                loadStream.Close();
                return sfx;
            }
            else
            {
                fileLoc = Encrypter.DecryptFile(location);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(SFXInfo));
                SFXInfo sfx = (SFXInfo)(xmlSer.Deserialize(loadStream));
                loadStream.Close();

                //DELETES TEMP FILE
                if (File.Exists(fileLoc))
                    File.Delete(fileLoc);

                return sfx;
            }

        }

        internal static void PreFabWriter(ObjectGroup objg, string loc)
        {
            if (Game1.bIsDebug)
            {

                fileLoc = loc;


                if (File.Exists(fileLoc))
                {
                    File.Copy(fileLoc, fileLoc + "BU");
                }

                if (fileLoc.EndsWith(".cgprefabc")) //Create a temp map in temp, after that put the decrypted map in 
                {
                    String tempLoc = Path.Combine(Path.GetTempPath(), "TBAGW.cgprefabc");

                    FileStream loadStreamCGCLC = File.Open(tempLoc, FileMode.OpenOrCreate);
                    XmlSerializer xmlSerCGCLC = new XmlSerializer(typeof(ObjectGroup));
                    xmlSerCGCLC.Serialize(loadStreamCGCLC, objg);
                    loadStreamCGCLC.Close();
                    Encrypter.EncryptFile(tempLoc, fileLoc);
                    //DELETES TEMP FILE
                    if (File.Exists(tempLoc))
                        File.Delete(tempLoc);
                    if (File.Exists(fileLoc + "BU"))
                        File.Delete(fileLoc + "BU");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Change save ending please, in '.cgprefabc' please");
                }
            }
            else //When not in debug mode
            {
                {

                    fileLoc = loc;


                    if (fileLoc.EndsWith(".cgprefab"))
                    {
                        FileStream loadStreamCGCL = File.Open(fileLoc, FileMode.OpenOrCreate);
                        XmlSerializer xmlSerCGCL = new XmlSerializer(typeof(ObjectGroup));
                        xmlSerCGCL.Serialize(loadStreamCGCL, objg);
                        loadStreamCGCL.Close();
                    }
                }
            }
        }

        internal static ObjectGroup PreFabReader(String location)
        {
            if (!location.EndsWith(".cgprefabc"))
            {
                fileLoc = location;

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(ObjectGroup));
                ObjectGroup objg = (ObjectGroup)(xmlSer.Deserialize(loadStream));
                loadStream.Close();
                return objg;
            }
            else if (Game1.bIsDebug)
            {
                fileLoc = Encrypter.DecryptFile(location);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(ObjectGroup));
                ObjectGroup objg = (ObjectGroup)(xmlSer.Deserialize(loadStream));
                loadStream.Close();

                //DELETES TEMP FILE
                if (File.Exists(fileLoc))
                    File.Delete(fileLoc);

                return objg;
            }
            return null;

        }

        internal static void SaveFileWriter(PlayerSaveData psd, string saveFileName = null)
        {
            if (saveFileName == null)
            {
                saveFileName = "";
                if (psd.bAutoSave) { saveFileName += "Autosave-"; }
                saveFileName += DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToShortTimeString();
                saveFileName = saveFileName.Replace(@"\", "-");
                saveFileName = saveFileName.Replace(@":", "-");
                saveFileName = saveFileName.Replace(@"/", "-");
                saveFileName += "-" + DateTime.Now.Second + "-" + DateTime.Now.Millisecond; ;
            }
            String tempLoc = Path.Combine(Path.GetTempPath(), "MG.sf");
            psd.saveDataName = SaveDataProcessor.GenerateMGSavePath(saveFileName);

            FileStream loadStreamCGCLC = File.Open(tempLoc, FileMode.OpenOrCreate);
            XmlSerializer xmlSerCGCLC = new XmlSerializer(typeof(PlayerSaveData));
            xmlSerCGCLC.Serialize(loadStreamCGCLC, psd);
            loadStreamCGCLC.Close();

            String sfn = SaveDataProcessor.GenerateMGSavePath(saveFileName);
            if (File.Exists(sfn))
            {
                File.Delete(sfn);
            }

            //psd.saveDataName = SaveDataProcessor.GenerateMGSavePath(saveFileName);
            Encrypter.EncryptFile(tempLoc, SaveDataProcessor.GenerateMGSavePath(saveFileName));
            //DELETES TEMP FILE
            if (File.Exists(tempLoc))
                File.Delete(tempLoc);


        }

        internal static void OverwriteSaveFileWriter(PlayerSaveData psd, string fn)
        {
            String saveFileName = null;
            if (saveFileName == null)
            {
                saveFileName = "";
                if (psd.bAutoSave) { saveFileName += "Autosave-"; }
                saveFileName += DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToShortTimeString();
                saveFileName = saveFileName.Replace(@"\", "-");
                saveFileName = saveFileName.Replace(@":", "-");
                saveFileName = saveFileName.Replace(@"/", "-");
                saveFileName += "-" + DateTime.Now.Second + "-" + DateTime.Now.Millisecond; ;
            }
            String tempLoc = Path.Combine(Path.GetTempPath(), "MG.sf");
            psd.saveDataName = SaveDataProcessor.GenerateMGSavePath(saveFileName);

            if (File.Exists(Path.Combine(SaveDataProcessor.saveFolder, fn)))
            {
                File.Delete((Path.Combine(SaveDataProcessor.saveFolder, fn)));
            }

            FileStream loadStreamCGCLC = File.Open(tempLoc, FileMode.OpenOrCreate);
            XmlSerializer xmlSerCGCLC = new XmlSerializer(typeof(PlayerSaveData));
            xmlSerCGCLC.Serialize(loadStreamCGCLC, psd);
            loadStreamCGCLC.Close();

            String sfn = SaveDataProcessor.GenerateMGSavePath(saveFileName);
            if (File.Exists(sfn))
            {
                File.Delete(sfn);
            }

            // psd.saveDataName = SaveDataProcessor.GenerateMGSavePath(saveFileName);
            Encrypter.EncryptFile(tempLoc, SaveDataProcessor.GenerateMGSavePath(saveFileName));

            //DELETES TEMP FILE
            if (File.Exists(tempLoc))
                File.Delete(tempLoc);


        }

        internal static void SaveSettingsWriter()
        {

            SettingsFile sf = SettingsFile.generateFile();

            XmlSerializer xmlSerCGCLC = new XmlSerializer(typeof(SettingsFile));
            if (File.Exists(SettingsFile.GenerateSettingsSavePath()))
            {
                File.Delete(SettingsFile.GenerateSettingsSavePath());
            }
            FileStream loadStreamCGCLC = File.Open(SettingsFile.GenerateSettingsSavePath(), FileMode.OpenOrCreate);
            xmlSerCGCLC.Serialize(loadStreamCGCLC, sf);
            loadStreamCGCLC.Close();

        }

        internal static void LoadSaveSettings()
        {
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Aeon's Dawn")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Aeon's Dawn"));
            }

            if (File.Exists(SettingsFile.GenerateSettingsSavePath()))
            {
                FileStream loadStream = File.Open(SettingsFile.GenerateSettingsSavePath(), FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(SettingsFile));
                SettingsFile sf = (SettingsFile)(xmlSer.Deserialize(loadStream));
                loadStream.Close();
                sf.Load();
            }
            else
            {
                SaveSettingsWriter();
            }

        }

        internal static PlayerSaveData SaveFileReader(int i)
        {
            if (Game1.bMainGame)
            {
                fileLoc = Encrypter.DecryptFile(SaveDataProcessor.GenerateMGLoadPath(i));

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(PlayerSaveData));
                PlayerSaveData objg = (PlayerSaveData)(xmlSer.Deserialize(loadStream));
                loadStream.Close();

                //DELETES TEMP FILE
                if (File.Exists(fileLoc))
                    File.Delete(fileLoc);

                return objg;
            }
            return null;

        }

        internal static PlayerSaveData SaveFileReader(String fileLoc)
        {

            var loc = fileLoc;
            loc = loc.Replace(SaveDataProcessor.saveFolder, "");
            loc = loc.Replace(@"\", "");

            if (Game1.bMainGame)
            {
                fileLoc = Encrypter.DecryptFile(fileLoc);

                FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(PlayerSaveData));
                PlayerSaveData objg = (PlayerSaveData)(xmlSer.Deserialize(loadStream));
                loadStream.Close();

                //DELETES TEMP FILE
                if (File.Exists(fileLoc))
                    File.Delete(fileLoc);

                return objg;
            }
            return null;

        }

        internal static PlayerSaveData SaveFileReaderSpawn(String fileLoc)
        {
            {
                EncrypterSpawn esp = new EncrypterSpawn();
                //fileLoc = esp.DecryptFile(fileLoc);
                Console.WriteLine("Attempting readStream");
                // FileStream loadStream = File.Open(fileLoc, FileMode.OpenOrCreate, FileAccess.Read);
                XmlSerializer xmlSer = new XmlSerializer(typeof(PlayerSaveData));
                //PlayerSaveData objg = (PlayerSaveData)(xmlSer.Deserialize(loadStream));
                Stream s = (esp.DecryptFileToStream(fileLoc));
                PlayerSaveData objg = (PlayerSaveData)(xmlSer.Deserialize(s));
                s.Close();
                //loadStream.Close();

                //DELETES TEMP FILE
                //if (File.Exists(fileLoc))
                //    File.Delete(fileLoc);

                Console.WriteLine("Done readStream");
                return objg;
            }
            return null;

        }

        internal static SpriteExportObjectCollection TileSheetProcessor(String loc)
        {
            SpriteExportObjectCollection data = null;
            using (FileStream loadstream = File.Open(loc, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer xSer = new XmlSerializer(typeof(SpriteExportObjectCollection));
                data = (SpriteExportObjectCollection)(xSer.Deserialize(loadstream));
                loadstream.Close();
            }

            return data;
        }

        internal static void SongEncrypter(String inF, String outF)
        {
            Encrypter.EncryptFile(inF, outF);
        }
    }
}

