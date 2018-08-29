using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ReloadedUpdateChecker.Updaters.Implementations.GameBanana.Tracker;

namespace ReloadedUpdateChecker.Updaters.Implementations.GameBanana
{
    public class GameBananaUpdater : IUpdateSource
    {
        /// <summary>
        /// A shared copy of the Mod Manager; in case if needed.
        /// </summary>
        private GBModManager _modManager;

        /// <summary>
        /// Holds a copy of all of the mod entries to be stored between <see cref="ProcessGamebananaLink"/> and auto call to <see cref="OnModExtract"/>
        /// </summary>
        private List<GBModEntry> _modEntries;

        /*
             ------------------------
             Interface Implementation 
             ------------------------
        */

        public List<Update> GetUpdates()
        {
            // Go over the list of mods and grab individual updates.
            var mods = new GBModManager().GetMods();
            List<Update> updates = new List<Update>();

            // Iterate every single mod and find ones that have been updated.
            foreach (var mod in mods)
            {
                var modItem = mod.GetItem();
                if (modItem != null && modItem.HasUpdates)
                {
                    // Get latest update and compare the download times.
                    GameBananaItemUpdate latestUpdate = modItem.Updates[0];
                    var file = modItem.Files.First().Value;

                    // Determine the last write time of config file (if exists) - and write last check time.
                    // Or if it does not exist, we will not update the last check time.
                    var modTime = mod.LastChecked.ToUniversalTime();
                    mod.LastChecked = DateTime.Now.ToUniversalTime();

                    // Conditionally alter mod time and store flag of whether to update last checked time.
                    string potentialConfigLocation = mod.DownloadLocation + $"\\{mod.ModFolders[0]}\\Config.json";
                    if (File.Exists(potentialConfigLocation))
                        modTime = File.GetLastWriteTime(potentialConfigLocation).ToUniversalTime();
                    else
                    {
                        // Oldest file in directory.
                        // This is a workaround for non-Reloaded mods, or if the structure ever changes.
                        string firstFolder = mod.DownloadLocation + $"\\{mod.ModFolders[0]}";
                        modTime = Directory.GetFiles(firstFolder)
                                                .OrderBy(File.GetLastWriteTimeUtc)
                                                .Select(File.GetLastWriteTimeUtc)
                                                .First();
                    }

                    if (latestUpdate.DateAdded > modTime)
                    {
                        updates.Add(new Update(modItem.Name, "Not Supported", modItem.OwnerName, latestUpdate.Title, new Uri(file.DownloadUrl), (int)file.Filesize, mod.DownloadLocation));
                    }
                }
            }

            // Save mod list so last update time was remembered.
            new GBModManager().SaveMods(mods);
            return updates;
        }

        public string OnLinkDownload(string downloadLink, string downloadLocation)
        {
            // Check if our link is a gamebanana link.
            string gbLinkPattern = @"^https?:\/\/gamebanana[.]com";
            Regex gbLinkMatcher  = new Regex(gbLinkPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var gbLinkMatch      = gbLinkMatcher.Match(downloadLink);

            // Process link if GB link, else not.
            return gbLinkMatch.Success ? ProcessGamebananaLink(downloadLink, downloadLocation) : downloadLink;
        }

        public void OnModExtract(string[] directories)
        {
            // If the last link was not ours - dp nothing.
            if (_modManager == null || _modEntries == null)
                return;

            // Complete the last set _modEntries member with a directory set and save the mod entry list.
            var lastEntry = _modEntries[_modEntries.Count - 1];
            lastEntry.ModFolders = directories;
            _modEntries[_modEntries.Count - 1] = lastEntry;
            _modManager.SaveMods(_modEntries);
        }

        public string GetSourceName() => "GameBanana";


        /*
            --------------------
            Class Implementation 
            --------------------
        */

        /// <summary>
        /// Contains the actual behind the scenes implementation of <see cref="OnLinkDownload"/>.
        /// </summary>
        private string ProcessGamebananaLink(string downloadLink, string downloadLocation)
        {
            // Set our regular expressions for string matching.
            string itemTypePattern = @"^.*gb_itemtype:(\w+).*$";
            string itemidPattern = @"^.*gb_itemid:(\w+).*$";
            Regex itemTypeMatcher = new Regex(itemTypePattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex itemIdMatcher = new Regex(itemidPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var itemTypeMatch = itemTypeMatcher.Match(downloadLink);
            var itemIdMatch = itemIdMatcher.Match(downloadLink);

            if (itemTypeMatch.Success && itemIdMatch.Success)
            {
                // Get the list of GB mods, append to it and resave.
                _modManager = new GBModManager();
                _modEntries = _modManager.GetMods();

                long itemId = Convert.ToInt64(itemIdMatch.Groups[1].Value);
                string itemType = itemTypeMatch.Groups[1].Value;
                _modEntries.Add(new GBModEntry(itemType, itemId, DateTime.Now.ToUniversalTime(), downloadLocation));

                // Now strip the download link.
                string downloadLinkPattern = @"(^.*?),";
                var downloadLinkmatcher = new Regex(downloadLinkPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                return downloadLinkmatcher.Match(downloadLink).Groups[1].Value;
            }

            return downloadLink;
        }
    }
}
