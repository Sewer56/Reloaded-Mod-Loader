using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Reloaded.Paths;

namespace Reloaded_Steam_Shim
{
    /// <summary>
    /// Retrieves or writes the current shim configuration.
    /// </summary>
    public class ReloadedShimSettings
    {
        public static string ShimConfigLocation;

        /// <summary>
        /// If this is not empty, load the game config at this location by default.
        /// </summary>
        public string LoadByDefault { get; set; }

        /// <summary>
        /// Specifies the location of Reloaded's Launcher.
        /// </summary>
        public string LauncherLocation { get; set; }

        /// <summary>
        /// Sets the shim location when this class is first accessed.
        /// </summary>
        static ReloadedShimSettings()
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ShimConfigLocation      = currentDirectory + $"\\{Strings.Common.SteamShimFileName}";

            if (! File.Exists(ShimConfigLocation))
                File.WriteAllText(ShimConfigLocation, JsonConvert.SerializeObject(new ReloadedShimSettings()));
        }

        /// <summary>
        /// Retrieves the current shim config file in game directory.
        /// </summary>
        public static ReloadedShimSettings GetShim()
        {
            return JsonConvert.DeserializeObject<ReloadedShimSettings>(File.ReadAllText(ShimConfigLocation));
        }

        /// <summary>
        /// Saves the current shim config file back to game directory.
        /// </summary>
        public void SaveShim()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(ShimConfigLocation, json);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Saving shim settings failed, unauthorized access. Please run me as admin next time.");
            }
        }
    }
}
