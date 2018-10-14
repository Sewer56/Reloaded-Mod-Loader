/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
    Copyright (C) 2018  Sewer. Sz (Sewer56)

    [Reloaded] is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    [Reloaded] is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Reloaded.IO.Config.Interfaces;
using Reloaded.Paths;

namespace Reloaded.IO.Config
{
    /// <summary>
    /// Defines a general struct for the loader mod configuration file.
    /// </summary>
    public class ModConfig : IModConfigV1
    {
        /// <summary>
        /// Contains a unique name assigned to the modification which
        /// can be used for enabling the modification as a dependency.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ModId { get; set; } = "";

        /// <summary>
        /// The name of the mod as it appears in the mod loader configuration tool.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ModName { get; set; } = "Modification Name Undefined";

        /// <summary>
        /// The description of the mod.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ModDescription { get; set; } = "Modification Description Undefined";

        /// <summary>
        /// The version of the mod. (Recommended Format: 1.XX)
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ModVersion { get; set; } = "Undefined";

        /// <summary>
        /// The author of the specific mod.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ModAuthor { get; set; } = "Undefined";

        /// <summary>
        /// The site shown in the hyperlink on the loader for the mod.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ModSite { get; set; } = "N/A";

        /// <summary>
        /// Used for self-updates from source code.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ModSource { get; set; } = "N/A";

        /// <summary>
        /// Specifies an executable or file in the same directory to be ran for configuration purposes.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ConfigurationFile { get; set; } = "N/A";

        /// <summary>
        /// [Optional] Specifies the DLL File name to load if the mod is being loaded into a 32 bit application.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string DllFile32 { get; set; } = "";

        /// <summary>
        /// [Optional] Specifies the DLL File name to load if the mod is being loaded into a 64 bit application.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string DllFile64 { get; set; } = "";

        /// <summary>
        /// Specifies a list of other mod ids to be loaded if the current mod is enabled.
        /// This is not specifically enforced, but is a hint to the Reloaded Launcher.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public List<string> Dependencies { get; set; } = new List<string>();

        /// <summary>
        /// [DO NOT MODIFY] Stores the physical file location of the mod configuration's Config.json for re-save purposes.
        /// </summary>
        [JsonIgnore]
        public string ModLocation { get; set; }

        /// <summary>
        /// Allows you to stores and/or backup the individual game entry to which this modification belongs to.
        /// </summary>
        [JsonIgnore]
        public GameConfig ParentGame { get; set; }

        /// <summary>
        /// Retrieves the Mod Loader mod configuration file struct and assigns a parent game
        /// to this individual mod loader mod.
        /// </summary>
        /// <param name="modDirectory">The absolute directory of the individual mod in question.</param>
        /// <param name="parentGame">The parent to be assigned to this individual mod in question.</param>
        public static ModConfig ParseConfig(string modDirectory, GameConfig parentGame)
        {
            ModConfig modConfiguration = ParseConfig(modDirectory);
            modConfiguration.ParentGame = parentGame;
            return modConfiguration;
        }

        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// This function exists only for preserving the legacy interface, consider using <see cref="ParseConfig(string,Reloaded.IO.Config.GameConfig)"/> instead.
        /// </summary>
        /// <param name="modDirectory">The absolute directory of the individual mod in question.</param>
        public static ModConfig ParseConfig(string modDirectory)
        {
            // Read the mod loader configuration.
            string modConfigurationLocation = modDirectory + $"/{Strings.Parsers.ConfigFile}";

            // Try parsing the config file, else return default one.
            ModConfig modConfiguration;

            try
            {
                string jsonText = File.ReadAllText(modConfigurationLocation);
                modConfiguration =
                    File.Exists(modConfigurationLocation)
                        ? JsonConvert.DeserializeObject<ModConfig>(jsonText)
                        : new ModConfig();
            }
            catch { modConfiguration = new ModConfig(); }

            modConfiguration.ModLocation = modConfigurationLocation;
            return modConfiguration;
        }

        /// <summary>
        /// Writes out the config file to disk.
        /// </summary>
        /// <param name="modConfig">The mod configuration structure defining the details of the individual mod.</param>
        public static void WriteConfig(ModConfig modConfig)
        {
            // Convert structure to JSON
            string json = JsonConvert.SerializeObject(modConfig, Formatting.Indented);

            // Write to disk
            File.WriteAllText(modConfig.ModLocation, json);
        }

        /// <summary>
        /// <see cref="ParentGame"/> should be set before calling this method.
        /// Returns true if all of the individual dependencies for this mod exist either in the current game
        /// configuration or the global mod list. This only checks if all of the individual mod IDs match, not
        /// whether they are enabled or not.
        /// </summary>
        /// <returns>True if all dependencies exist, else false.</returns>
        public bool CheckDependencies()
        {
            // Get all mods for current game and global game.
            GameConfig globalGameConfig = GameConfig.GetGlobalConfig();
            GameConfig localGameConfig = ParentGame;

            // Stores the individual mod configurations for the current game in question.
            List<ModConfig> modConfigurations = ConfigManager.GetAllModsForGame(globalGameConfig);
            modConfigurations.AddRange(ConfigManager.GetAllModsForGame(localGameConfig));

            // Populate a list of mod IDs.
            List<string> modIDs = new List<string>();
            foreach (var modConfig in modConfigurations)
            { modIDs.Add(modConfig.ModId); }

            // Iterate over all dependencies and return false if a mod ID does not exist in the pre-populated list of IDs.
            foreach (var modDependencyId in Dependencies)
            {
                if (!modIDs.Contains(modDependencyId))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// <see cref="ParentGame"/> should be set before calling this method.
        /// Returns a list of individual dependencies for the mod (other mods) that cannot be found or are not installed.
        /// The list of dependencies is a list of all individual missing mod IDs.
        /// </summary>
        /// <returns>A list of all individual missing/not installed mod IDs.</returns>
        public List<string> GetMissingDependencies()
        {
            // Get all mods for current game and global game.
            GameConfig globalGameConfig = GameConfig.GetGlobalConfig();
            GameConfig localGameConfig = ParentGame;

            // Stores the individual mod configurations for the current game in question.
            List<ModConfig> modConfigurations = ConfigManager.GetAllModsForGame(globalGameConfig);
            modConfigurations.AddRange(ConfigManager.GetAllModsForGame(localGameConfig));

            // Populate a list of mod IDs.
            List<string> modIDs = new List<string>();
            foreach (var modConfig in modConfigurations)
            { modIDs.Add(modConfig.ModId); }

            // Missing mods.
            List<string> missingModIDs = new List<string>();

            // Iterate over all dependencies and return false if a mod ID does not exist in the pre-populated list of IDs.
            foreach (var modDependencyId in Dependencies)
            {
                if (!modIDs.Contains(modDependencyId))
                    missingModIDs.Add(modDependencyId);
            }

            return missingModIDs;
        }

        /// <summary>
        /// Returns true if the individual modification is enabled, else false.
        /// You are required to ensure <see cref="ParentGame"/> is assigned before calling this.
        /// </summary>
        /// <returns></returns>
        public bool IsEnabled()
        {
            // Stores the directory name for the current mod, mod loader enables mods by directory.
            string folderName = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(ModLocation));
            return ParentGame.EnabledMods.Contains(folderName);
        }

        /// <summary>
        /// Retrieves the directory name of the individual mod, used for enabling or disabling the mod for
        /// the Mod Loader.
        /// </summary>
        public string GetModDirectoryName()
        {
            return Path.GetFileNameWithoutExtension(Path.GetDirectoryName(ModLocation));
        }

        /// <summary>
        /// Returns the directory whereby the individual mod is contained, i.e. storing Config.json.
        /// </summary>
        /// <returns>The directory containing the individual mod in question.</returns>
        public string GetModDirectory()
        {
            return Path.GetDirectoryName(ModLocation);
        }

        /// <summary>
        /// Goes through a list of this individual mod's dependencies and returns a list of all
        /// dependencies for the current modification.
        /// Note: Consider using <see cref="GetMissingDependencies"/> if you cannot guarantee all dependencies are installed, this method does not throw
        /// if a dependency is not installed.
        /// </summary>
        /// <returns>A list of depdendent mod configurations used for this mod that are disabled.</returns>
        public List<ModConfig> GetAllDependencies()
        {
            // Get all mods for current game and global game.
            GameConfig globalGameConfig = GameConfig.GetGlobalConfig();
            GameConfig localGameConfig = ParentGame;

            // Stores the individual mod configurations for the current game in question.
            List<ModConfig> modConfigurations = ConfigManager.GetAllModsForGame(globalGameConfig);
            modConfigurations.AddRange(ConfigManager.GetAllModsForGame(localGameConfig));

            // Filter down all of the mod configurations down to our dependencies.
            return modConfigurations.Where(x => this.Dependencies.Contains(x.ModId)).ToList();
        }

        /// <summary>
        /// Goes through a list of this individual mod's dependencies and returns a list of disabled
        /// dependencies for the current modification.
        /// </summary>
        /// <returns>A list of depdendent mod configurations used for this mod that are disabled.</returns>
        public List<ModConfig> GetDisabledDependencies()
        {
            // Filter down to all disabled mods.
            return GetAllDependencies().Where(x => ! x.IsEnabled()).ToList();
        }

        /// <summary>
        /// Goes through a list of this individual mod's dependencies and returns a list of enabled
        /// dependencies for the current modification.
        /// </summary>
        /// <returns>A list of depdendent mod configurations used for this mod that are disabled.</returns>
        public List<ModConfig> GetEnabledDependencies()
        {
            // Filter down to all disabled mods.
            return GetAllDependencies().Where(x => x.IsEnabled()).ToList();
        }

        /// <summary>
        /// Returns true if the config file for the mod exists, else false.
        /// </summary>
        public bool IsValidModConfig()
        {
            return File.Exists(ModLocation);
        }
    }
}
