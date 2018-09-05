using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Reloaded.Paths;

namespace GamebananaUpdater.GameBanana.Tracker
{
    public class GBModManager
    {
        /// <summary>
        /// Where our last update check date is stored.
        /// </summary>
        public static string SavePath = LoaderPaths.GetModLoaderConfigDirectory() + "\\GameBananaUpdateTracker.json";

        /*
            ----------
            Essentials
            ----------
        */

        public List<GBModEntry> GetMods()
        {
            // Simply return existing entries from file.
            try
            {
                var objects = JsonConvert.DeserializeObject<List<GBModEntry>>(File.ReadAllText(SavePath));
                objects = RemoveEntriesWithInvalidFolders(objects);
                return RemoveDuplicates(objects);
            }
            catch { return new List<GBModEntry>(); }
        }

        public void SaveMods(List<GBModEntry> entries)
        {
            entries = RemoveDuplicates(entries);
            entries = RemoveEntriesWithInvalidFolders(entries);
            string json = JsonConvert.SerializeObject(entries, Formatting.Indented);
            File.WriteAllText(SavePath, json);
        }

        /// <summary>
        /// From a list of mod entries, remove mod entries with duplicate ItemID and Item Type.
        /// </summary>
        /// <param name="modEntries"></param>
        public List<GBModEntry> RemoveDuplicates(List<GBModEntry> modEntries)
        {
            List<GBModEntry> entries = new List<GBModEntry>();
            foreach (var entry in modEntries)
            {
                bool hasDuplicate = false;
                foreach (var newEntry in entries)
                {
                    // Check for duplicate ID and type.
                    if (newEntry.ItemId != entry.ItemId || newEntry.ItemType != entry.ItemType)
                        continue;

                    hasDuplicate = true;
                    break;
                }

                if (! hasDuplicate)
                { entries.Add(entry); }
            }

            return entries;
        }

        /// <summary>
        /// Removes all mods which have had their folder directories removed.
        /// </summary>
        public List<GBModEntry> RemoveEntriesWithInvalidFolders(List<GBModEntry> modEntries)
        {
            List<GBModEntry> newModEntries = new List<GBModEntry>(modEntries.Count);

            foreach (var modEntry in modEntries)
            {
                // No folders, entry is out.
                if (modEntry.ModFolders.Length == 0)
                    continue;

                // Folder is missng, entry is out.
                bool folderIsMissing = false;
                foreach (var folder in modEntry.ModFolders)
                {
                    if (!Directory.Exists( $"{modEntry.DownloadLocation}\\{folder}"))
                    {
                        folderIsMissing = true;
                        break;
                    }
                }

                if (! folderIsMissing)
                    newModEntries.Add(modEntry);
            }

            return newModEntries;
        }
    }
}
