using System;
using Newtonsoft.Json;
using Reloaded.IO.Config;

namespace Github_Updater.Github.Queue
{
    /// <summary>
    /// Maps an individual Mod ID to an individual date when an update was last checked.
    /// </summary>
    public class QueueItem
    {
        public string   ModId;
        public DateTime LastChecked;

        [JsonIgnore]
        public ModConfig ModConfiguration;

        public QueueItem(string modId, DateTime lastChecked, ModConfig modConfig)
        {
            ModId = modId;
            LastChecked = lastChecked;
            ModConfiguration = modConfig;
        }
    }
}
