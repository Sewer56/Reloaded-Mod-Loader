using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Reloaded.IO.Config;
using Reloaded.Paths;
using Reloaded_Plugin_System.Utilities;

namespace GamebananaUpdater.GameBanana.Tracker
{
    public class GBModManager
    {
        /// <summary>
        /// Where our last update check date is stored.
        /// </summary>
        public static string SavePath = LoaderPaths.GetModLoaderConfigDirectory() + "\\GameBananaUpdateTracker.json";

        /// <summary>
        /// Specifies the file name that the users can use to define their custom mod type and ID.
        /// </summary>
        public const string ModManualEntryFileName = "GameBanana.json";

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
                objects     = GetEntriesFromExistingMods(objects);
                objects     = RemoveEntriesWithInvalidFolders(objects);
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

        /// <summary>
        /// Retrieves a list of all mod configurations for each game and adds the ones with GameBananaUpdater.json
        /// to the list. 
        /// </summary>
        /// <param name="gbModEntries"></param>
        /// <returns></returns>
        public List<GBModEntry> GetEntriesFromExistingMods(List<GBModEntry> gbModEntries)
        {
            // Get all of the individual mods, and then if a manual config file exists, try parsing
            // it and adding a new GBModEntry.
            List<ModConfig> allExistingMods = ModUtilities.GetAllMods();

            foreach (ModConfig existingMod in allExistingMods)
            {
                string potentialFileLocation = $"{existingMod.GetModDirectory()}\\{ModManualEntryFileName}";
                if (File.Exists(potentialFileLocation))
                {
                    try
                    {
                        var simpleModEntry = JsonConvert.DeserializeObject<GBSimpleModEntry>(File.ReadAllText(potentialFileLocation));
                        simpleModEntry.ModConfig = existingMod;
                        gbModEntries.Add(GBModEntry.FromSimpleModEntry(simpleModEntry));
                    }
                    catch(Exception ex)
                    {
                        // Write correct template.
                        string templateFileName = potentialFileLocation.Replace(".json", "Example.json");
                        string json = JsonConvert.SerializeObject(new GBSimpleModEntry("Example", 123456, null), Formatting.Indented);
                        json += $"\r\n\r\n/*You did something wrong in your GameBanana.json, so here's a template to help you.\r\nHere's a tip about what you did wrong {ex.Message}*/";

                        File.WriteAllText(templateFileName, json);
                    }
                }
            }

            return gbModEntries;
        }
    }
}
