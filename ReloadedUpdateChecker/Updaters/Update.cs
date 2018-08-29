using System;
using System.Globalization;
using Reloaded.IO.Config;

namespace ReloadedUpdateChecker.Updaters
{
    public class Update
    {
        public string ModId;
        public string ModName;
        public string ModAuthor;
        public string ModVersion;
        public int    FileSize;
        public Uri    DownloadLink;
        public string GameModFolder;

        public float FileSizeKB => (FileSize / 1000F);
        public float FileSizeMB => (FileSize / 1000F / 1000F);
        public float FileSizeGB => (FileSize / 1000F / 1000F / 1000F);
        public string FileSizeKBString => (FileSize/1000F).ToString("000.00");
        public string FileSizeMBString => (FileSize/1000F/1000F).ToString("000.00");
        public string FileSizeGBString => (FileSize/1000F/1000F/1000F).ToString("000.00");

        public Update(string modName, string modId, string modAuthor, string modVersion, Uri download, int fileSize, string gameModFolder)
        {
            ModName = modName;
            ModId = modId;
            ModAuthor = modAuthor;
            ModVersion = modVersion;
            DownloadLink = download;
            FileSize = fileSize;
            GameModFolder = gameModFolder;
        }


    }
}
