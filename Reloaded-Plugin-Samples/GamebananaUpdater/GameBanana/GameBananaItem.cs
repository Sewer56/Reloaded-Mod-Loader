using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace GamebananaUpdater.GameBanana
{
    /* Disclaimer: Class snippet taken from SADX-Mod-Loader. */
    public class GameBananaItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("Owner().name")]
        public string OwnerName { get; set; }

        [JsonProperty("Url().sGetProfileUrl()")]
        public string ProfileUrl { get; set; }

        [JsonProperty("Updates().bSubmissionHasUpdates()")]
        public bool HasUpdates { get; set; }

        [JsonProperty("Updates().aGetLatestUpdates()")]
        public GameBananaItemUpdate[] Updates { get; set; }

        [JsonProperty("Files().aFiles()")]
        public Dictionary<string, GameBananaItemFile> Files { get; set; }

        public static GameBananaItem Load(string itemType, long itemId)
        {
            try
            {
                string response;
                using (var client = new WebClient())
                    response = client.DownloadString($"https://api.gamebanana.com/Core/Item/Data?itemtype={itemType}&itemid={itemId}&fields=name%2COwner().name%2CUrl().sGetProfileUrl()%2CUpdates().bSubmissionHasUpdates()%2CUpdates().aGetLatestUpdates()%2CFiles().aFiles()&return_keys=1");
                return JsonConvert.DeserializeObject<GameBananaItem>(response);
            }
            catch
            { return null; }
        }
    }

    public class GameBananaItemUpdate
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1);

        [JsonProperty("_sTitle")]
        public string Title { get; set; }

        [JsonProperty("_aChangeLog")]
        public GameBananaItemUpdateChange[] Changes { get; set; }

        [JsonProperty("_sText")]
        public string Text { get; set; }

        [JsonProperty("_tsDateAdded")]
        public long DateAddedInt { get; set; }

        [JsonIgnore]
        public DateTime DateAdded => Epoch.AddSeconds(DateAddedInt);
    }

    public class GameBananaItemUpdateChange
    {
        [JsonProperty("cat")]
        public string Category { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class GameBananaItemFile
    {
        [JsonProperty("_nFilesize")]
        public long Filesize { get; set; }

        [JsonProperty("_sDownloadUrl")]
        public string DownloadUrl { get; set; }
    }
}
