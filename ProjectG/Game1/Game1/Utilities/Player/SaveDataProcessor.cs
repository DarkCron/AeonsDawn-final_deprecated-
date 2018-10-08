using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.ReadWrite;
using static TBAGW.Utilities.Characters.BaseCharacter;

namespace TBAGW
{
    public static class SaveDataProcessor
    {
        internal static String saveFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Aeon's Dawn");

        static public void Launch()
        {
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
        }

        static public void AttemptSave()
        {
            PlayerSaveData tempPSD = new PlayerSaveData();
            tempPSD.GenerateSave();

            EditorFileWriter.SaveFileWriter(tempPSD);
        }

        static public void OverwriteSave(String fn)
        {
            PlayerSaveData tempPSD = new PlayerSaveData();
            tempPSD.GenerateSave();

            EditorFileWriter.OverwriteSaveFileWriter(tempPSD, fn);
        }

        static public String GenerateMGSavePath(String n)
        {
            return Path.Combine(saveFolder, n + "-MG.sfc");
            try
            {
                string[] filePaths = Directory.GetFiles(saveFolder, "*MG.sfc");
                if (filePaths.Length < 30)
                {
                    return Path.Combine(saveFolder, filePaths.Length + "MG.sfc");
                }
                else
                {
                    FileSystemInfo fileInfo = new DirectoryInfo(saveFolder).GetFileSystemInfos("*MG.sfc").OrderByDescending(fi => fi.LastWriteTime).Last();
                    String tempFileName = fileInfo.Name;
                    tempFileName = tempFileName.Replace("MG.sfc", "");
                    return Path.Combine(saveFolder, tempFileName + "MG.sfc");
                }
            }
            catch (Exception)
            {
                return Path.Combine(saveFolder, "ERROR" + "MG.sfc");
            }
        }

        static public String GenerateMGLoadPath(int i)
        {
            try
            {
                return Path.Combine(saveFolder, i + "MG.sfc");
            }
            catch (Exception)
            {
                return Path.Combine(saveFolder, "ERROR" + "MG.sfc");
            }
        }

        static public void HandleLoad(PlayerSaveData PSD)
        {
            PlayerSaveData.heroParty.Clear();
            PlayerSaveData.heroParty = PSD.activePartySave;
            PlayerSaveData.playerInventory = PSD.playerInventorySave;


            foreach (var item in PSD.heroTeamActive)
            {
                BaseCharacter tempBC = GameProcessor.gcDB.gameCharacters.Find(c => item.charID == c.shapeID).Clone();
                tempBC.ReloadFromSaveFile(item);
                PlayerSaveData.heroParty.Add(tempBC);
            }

            BaseCharacter newController = (PlayerSaveData.heroParty.Find(hero => hero.shapeID == PSD.mainControllerID));
            PlayerController.selectedSprite = newController;
            Utilities.Map.BasicMap.HandleLoadGame(newController);
            PlayerController.selectedSprite = newController;
            PlayerController.selectedSprite.position = PSD.mainControllerPos;
            PlayerController.selectedSprite.rotationIndex = (int)BaseCharacter.Rotation.Down;
            PlayerController.selectedSprite.animationIndex = (int)CharacterAnimations.Idle;
            GameProcessor.cameraFollowTarget = PlayerController.selectedSprite;
            NonCombatCtrl.Reset();
            PlayerSaveData.HandleReload(PSD);
        }

        static internal List<PlayerSaveData> saveFilesIn(String loc)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<PlayerSaveData> saves = new List<PlayerSaveData>();
            List<Task<PlayerSaveData>> aSyncReadList = new List<Task<PlayerSaveData>>();


            DirectoryInfo info = new DirectoryInfo(loc);
            var files = info.GetFiles("*.sfc").OrderBy(p => p.CreationTime).ToList();
            files.Reverse();

            List<String> tempList = new List<string>();

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].FullName.EndsWith(".sfc"))
                {
                    tempList.Add(files[i].FullName);
                }
            }


            for (int i = 0; i < tempList.Count; i++)
            {
                int temp = i;
                aSyncReadList.Add(Task<PlayerSaveData>.Factory.StartNew(() => EditorFileWriter.SaveFileReaderSpawn(tempList[temp])));
                // saves.Add(EditorFileWriter.SaveFileReader(tempList[i]));
            }

            while (tempList.Count != 0 && aSyncReadList.FindAll(t => t.IsCompleted).Count == tempList.Count)
            {

            }

            aSyncReadList.ForEach(t => saves.Add(t.Result));
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Time elapsed : " + elapsedMs);

            return saves;
        }

        internal static void QuickSave()
        {
            PlayerSaveData tempPSD = new PlayerSaveData();
            tempPSD.GenerateSave();
            tempPSD.bAutoSave = true;

            EditorFileWriter.SaveFileWriter(tempPSD);
        }
    }
}
