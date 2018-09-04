using System;

namespace Reloaded_Plugin_System.Interfaces.Updaters
{
    public class Update : IUpdate
    {
        public string ModId             { get; set; }
        public string ModName           { get; set; }
        public string ModAuthor         { get; set; }
        public string ModVersion        { get; set; }
        public int    FileSize          { get; set; }
        public Uri    DownloadLink      { get; set; }
        public string GameModFolder     { get; set; }

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
