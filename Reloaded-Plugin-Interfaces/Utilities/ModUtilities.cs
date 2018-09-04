using System.Collections.Generic;
using Reloaded.IO.Config;

namespace Reloaded_Plugin_System.Utilities
{
    public static class ModUtilities
    {
        /// <summary>
        /// Retrieves a list of all Reloaded mods for all games.
        /// </summary>
        /// <returns>A list of Reloaded mods... unsurprisingly.</returns>
        public static List<ModConfig> GetAllMods()
        {
            List<ModConfig> modConfigurations   = new List<ModConfig>(100);
            List<GameConfig> gameConfigurations = ConfigManager.GetAllGameConfigs();

            // Iterate all mods over all games.
            foreach (var gameConfig in gameConfigurations)
            {
                modConfigurations.AddRange(ConfigManager.GetAllModsForGame(gameConfig));
            }

            return modConfigurations;
        }
    }
}
