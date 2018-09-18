using System;
using System.IO;

namespace GamebananaUpdater.GameBanana.Tracker
{
    public class GBModEntry
    {
        public string ItemType;
        public long   ItemId;
        public DateTime LastChecked;
        public string DownloadLocation;
        public string[] ModFolders = new string[0];

        private GBModEntry() { }

        public GBModEntry(string itemType, long itemId, DateTime lastChecked, string downloadLocation)
        {
            ItemType         = itemType;
            ItemId           = itemId;
            LastChecked      = lastChecked;
            DownloadLocation = downloadLocation;
        }

        public static GBModEntry FromSimpleModEntry(GBSimpleModEntry simpleModEntry)
        {
            return new GBModEntry()
            {
                // GetModDirectory(): C:\\Users\\sewer\\AppData\\Roaming\\Reloaded-Mod-Loader\\Reloaded-Mods\\Sonic-Heroes\\Midnight-Hill
                DownloadLocation = Path.GetDirectoryName(simpleModEntry.ModConfig.GetModDirectory()),

                ItemId      = simpleModEntry.ItemId,
                ItemType    = simpleModEntry.ItemType,
                LastChecked = DateTime.UtcNow,

                // GetModDirectoryName(): Midnight-Hill
                // Substring of GetModDirectory():
                ModFolders = new []{ simpleModEntry.ModConfig.GetModDirectoryName() }
            };
        }

        public GameBananaItem GetItem()
        {
            return GameBananaItem.Load(ItemType, ItemId);
        }


    }
}
