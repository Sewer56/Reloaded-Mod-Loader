using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Reloaded.IO.Config;
using static Reloaded.Paths.LoaderPaths;

namespace ReloadedUpdateChecker.Updaters.Implementations.Github.Queue
{
    /// <summary>
    /// An utility to work with Github update mappings.
    /// </summary>
    public class QueueManager
    {
        /// <summary>
        /// Where our last update check date is stored.
        /// </summary>
        public static string SavePath = GetModLoaderConfigDirectory() + "\\GithubUpdateTracker.json";

        /*
            ----------
            Essentials
            ----------
        */

        public List<QueueItem> GetQueue()
        {
            // Get all mods with valid Github URLs.
            List<ModConfig> allMods = ModUtilities.GetAllMods();
            allMods                 = GetConfigsWithValidGithubUrl(allMods);

            // Get mappings from all mods.
            List<QueueItem> mappings     = FromModList(allMods);

            // Get mappings already stored on disk.
            List<QueueItem> fileMappings = new List<QueueItem>();
            try   { fileMappings = JsonConvert.DeserializeObject< List<QueueItem> >( File.ReadAllText(SavePath) ); }
            catch { }

            // Assign each mapping's proper last checked date.
            for (int x = 0; x < mappings.Count; x++)
            {
                foreach (var fileMapping in fileMappings)
                {
                    if (mappings[x].ModId == fileMapping.ModId)
                    {
                        var updateMapping         = mappings[x];
                        updateMapping.LastChecked = fileMapping.LastChecked;
                        mappings[x]               = updateMapping;
                        break;
                    }
                }

            }

            // Sort mappings.
            return SortQueue(mappings);
        }

        public void SaveQueue(List<QueueItem> mappings)
        {
            string json = JsonConvert.SerializeObject(mappings, Formatting.Indented);
            File.WriteAllText(SavePath, json);
        }

        /// <summary>
        /// Chronologically sorts queue, returning the updates not checked at the beginning of the list.
        /// </summary>
        public List<QueueItem> SortQueue(List<QueueItem> mappings)
        {
            mappings.Sort((x, y) => DateTime.Compare(x.LastChecked, y.LastChecked));
            return mappings;
        }

        /// <summary>
        /// Returns the original supplied list of mods, minus the mods which do not have a valid Github URL
        /// </summary>
        public List<ModConfig> GetConfigsWithValidGithubUrl(List<ModConfig> modConfigs)
        {
            List<ModConfig> modsWithValidUrl = GetConfigsWithValidUrl(modConfigs);
            List<ModConfig> modsWithValidGithubUrl = new List<ModConfig>(modsWithValidUrl.Count);

            foreach (var mod in modsWithValidUrl)
            {
                Uri uri = GetUri(mod);
                if (FilterGithubUri(uri) != null)
                    modsWithValidGithubUrl.Add(mod);
            }

            return modsWithValidGithubUrl;
        }

        /*
            -------------------
            Important Functions
            -------------------
        */

        /// <summary>
        /// Returns configs that have a valid URL from a supplied list of configs.
        /// </summary>
        public List<ModConfig> GetConfigsWithValidUrl(List<ModConfig> modConfigs)
        {
            List<ModConfig> validUrlConfigs = new List<ModConfig>(modConfigs.Count);

            foreach (var modConfig in modConfigs)
            {
                if (GetUri(modConfig) != null)
                    validUrlConfigs.Add(modConfig);
            }

            return validUrlConfigs;
        }

        /// <summary>
        /// Retrieves an URL used to check updates for an individual mod.
        /// </summary>
        public Uri GetUri(ModConfig modConfig)
        {
            if (modConfig.ModSource != "")
            {
                try   { return new Uri(modConfig.ModSource); }
                catch { return null; }
            }

            return null;
        }

        /// <summary>
        /// Tries to check an individual supplied URI and returns the URI if the URI is a valid
        /// link to an individual Github repository.
        /// </summary>
        /// <returns>Null if the URI is not a valid repository link.</returns>
        public Uri FilterGithubUri(Uri uri)
        {
            if (uri == null)
                return null;

            if (String.Equals("github.com", uri.Host, StringComparison.OrdinalIgnoreCase))
            {
                // Check segments, there should only be 3 "/" "username/" "repository"
                if (uri.Segments.Length != 3)
                    return null;

                if (uri.Segments.Length > 0 && uri.Segments[0] != "/")
                    return null;

                return uri;
            }

            return null;
        }

        /*
            ---------------
            Extra Functions
            ---------------
        */

        /// <summary>
        /// Retrieves a list of URLs used to check updates for multiple individual mods.
        /// </summary>
        /// <param name="modConfigs">Mod configurations potentially having URLs.</param>
        public List<Uri> GetUris(List<ModConfig> modConfigs)
        {
            List<Uri> uriList = new List<Uri>(modConfigs.Count);
            foreach (var modConfig in modConfigs)
            {
                var result = GetUri(modConfig);
                if (result != null)
                    uriList.Add(result);
            }

            return uriList;
        }

        /// <summary>
        /// From a list of URIs, try return only the URIs that are valid repository links
        /// where we can search updates from.
        /// </summary>
        public List<Uri> FilterGithubUris(List<Uri> uriList)
        {
            List<Uri> uris = new List<Uri>(uriList.Count);

            foreach (var uri in uriList)
            {
                FilterGithubUri(uri);
            }

            return uris.Where(x => x != null).ToList();
        }

        /*
            -----------------
            Utility Functions
            -----------------
        */

        /// <summary>
        /// Generates a list of Update UpdateMappings from a mod list.
        /// </summary>
        public List<QueueItem> FromModList(List<ModConfig> modConfigurations)
        {
            List<QueueItem> mappings = new List<QueueItem>(modConfigurations.Count);

            foreach (var configuration in modConfigurations)
                mappings.Add(new QueueItem(configuration.ModId, DateTime.MinValue, configuration));

            return mappings;
        }

        /// <summary>
        /// Obtains a set of mod configurations from the list of update mappings.
        /// </summary>
        public List<ModConfig> FromMappings(List<QueueItem> updateMappings)
        {
            // Get all of the mods.
            List<ModConfig> modConfigurations = new List<ModConfig>(updateMappings.Count);

            foreach (var mapping in updateMappings)
            {
                modConfigurations.Add(mapping.ModConfiguration);
            }

            return modConfigurations;
        }

        /// <summary>
        /// Merges a list of update mappings. Returns the update mappings supplied in updateMappingsA, with any new entries
        /// not present in updateMappingsA from updateMappingsB.
        /// </summary>
        /// <returns></returns>
        public List<QueueItem> MergeMappings(List<QueueItem> updateMappingsA, List<QueueItem> updateMappingsB)
        {
            // Start with a copy of mappings from A.
            List<QueueItem> newUpdateMappings = new List<QueueItem>(updateMappingsA.Count + updateMappingsB.Count);
            newUpdateMappings.AddRange(updateMappingsA);

            // Then iterate over mappings in B, if A does not contain the ModId, add it.
            foreach (var updateMappingB in updateMappingsB)
            {
                if (! MappingsContainModId(updateMappingsA, updateMappingB.ModId)) // Add if mapping not present.
                    newUpdateMappings.Add(updateMappingB);
            }

            return newUpdateMappings;
        }

        /// <summary>
        /// Returns true if the supplied list of mappings contains a specific ModId, else false.
        /// </summary>
        public bool MappingsContainModId(List<QueueItem> mappings, string modId)
        {
            foreach (var mapping in mappings)
            {
                if (mapping.ModId == modId)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the supplied list of mod configs contains a specific ModId, else false.
        /// </summary>
        public bool ModConfigsContainModId(List<ModConfig> modConfig, string modId)
        {
            foreach (var mapping in modConfig)
            {
                if (mapping.ModId == modId)
                    return true;
            }

            return false;
        }
    }
}
