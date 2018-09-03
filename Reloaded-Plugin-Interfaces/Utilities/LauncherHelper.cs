using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Reloaded.IO.Config;

namespace Reloaded_Plugin_System.Utilities
{
    public static class LauncherHelper
    {
        /*
            -----------
            Serializers
            -----------
        */

        /// <summary>
        /// Deserializes a list of individual mod configurations passed into the plugin.
        /// </summary>
        public static List<ModConfig> DeserializeModConfigurations(string modConfigurations)
        {
            // Allows JsonConvert to ignore the ignore attribute.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new LauncherHelper.JsonIgnoreAttributeIgnorerContractResolver();

            return JsonConvert.DeserializeObject<List<ModConfig>>(modConfigurations, settings);
        }

        /// <summary>
        /// Serializes a list of individual mod configurations for passing back to the caller.
        /// </summary>
        /// <param name="aa"></param>
        /// <returns></returns>
        public static string SerializeModConfigurations(List<ModConfig> modConfigurations)
        {
            // Allows JsonConvert to ignore the ignore attribute.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new LauncherHelper.JsonIgnoreAttributeIgnorerContractResolver();

            return JsonConvert.SerializeObject(modConfigurations, settings);
        }

        /// <summary>
        /// Deserializes a list of individual theme configurations passed into the plugin.
        /// </summary>
        public static List<ThemeConfig> DeserializeThemeConfigurations(string themeConfigurations)
        {
            // Allows JsonConvert to ignore the ignore attribute.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new LauncherHelper.JsonIgnoreAttributeIgnorerContractResolver();

            return JsonConvert.DeserializeObject<List<ThemeConfig>>(themeConfigurations, settings);
        }

        /// <summary>
        /// Serializes a list of individual theme configurations for passing back to the caller.
        /// </summary>
        public static string SerializeThemeConfigurations(List<ThemeConfig> themeConfigurations)
        {
            // Allows JsonConvert to ignore the ignore attribute.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new LauncherHelper.JsonIgnoreAttributeIgnorerContractResolver();

            return JsonConvert.SerializeObject(themeConfigurations, settings);
        }

        /// <summary>
        /// Deserializes a list of individual game configurations passed into the plugin.
        /// </summary>
        public static List<GameConfig> DeserializeGameConfigurations(string gameConfigurations)
        {
            // Allows JsonConvert to ignore the ignore attribute.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new LauncherHelper.JsonIgnoreAttributeIgnorerContractResolver();

            return JsonConvert.DeserializeObject<List<GameConfig>>(gameConfigurations, settings);
        }

        /// <summary>
        /// Serializes a list of individual game configurations for passing back to the caller.
        /// </summary>
        public static string SerializeGameConfigurations(List<GameConfig> gameConfigurations)
        {
            // Allows JsonConvert to ignore the ignore attribute.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new LauncherHelper.JsonIgnoreAttributeIgnorerContractResolver();

            return JsonConvert.SerializeObject(gameConfigurations, settings);
        }

        /*
            -------
            Helpers
            -------
        */

        /// <summary>
        /// A contract Resolver that causes the JSON serializer to ignore the JsonIgnore attribute.
        /// </summary>
        public class JsonIgnoreAttributeIgnorerContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.Ignored = false;
                return property;
            }
        }
    }
}
