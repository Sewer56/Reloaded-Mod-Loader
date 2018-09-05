using System;

namespace GamebananaUpdater.GameBanana.Tracker
{
    public class GBModEntry
    {
        public string ItemType;
        public long   ItemId;
        public DateTime LastChecked;
        public string DownloadLocation;
        public string[] ModFolders = new string[0];

        public GBModEntry(string itemType, long itemId, DateTime lastChecked, string downloadLocation)
        {
            ItemType         = itemType;
            ItemId           = itemId;
            LastChecked      = lastChecked;
            DownloadLocation = downloadLocation;
        }

        public GameBananaItem GetItem()
        {
            return GameBananaItem.Load(ItemType, ItemId);
        }
    }
}
