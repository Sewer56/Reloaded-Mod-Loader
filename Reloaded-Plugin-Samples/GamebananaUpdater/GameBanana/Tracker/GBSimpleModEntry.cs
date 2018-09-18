using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reloaded.IO.Config;

namespace GamebananaUpdater.GameBanana.Tracker
{
    /// <summary>
    /// This is the variation of <see cref="GBModEntry"/> to be manually added as "GameBanana.json" into mod folders.
    /// </summary>
    public class GBSimpleModEntry
    {
        public string ItemType;
        public long   ItemId;

        [JsonIgnore]
        public ModConfig ModConfig;

        public GBSimpleModEntry(string itemType, long itemId, ModConfig modConfig)
        {
            ItemType = itemType;
            ItemId = itemId;
            ModConfig = modConfig;
        }
    }
}
