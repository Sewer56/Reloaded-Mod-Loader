using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reloaded_Plugin_System.Config;
using Reloaded_Plugin_System.Interfaces.Launcher;
using Reloaded_Plugin_System.Interfaces.Loader;

namespace Reloaded_Plugin_System
{
    public static class PluginLoader
    {
        /* Plugin Implementations */
        public static List<ILoaderEventsV1> LoaderEventPlugins  { get; private set; } = new List<ILoaderEventsV1>();
        public static List<ILauncherEventsV1> LauncherEventPlugins { get; private set; } = new List<ILauncherEventsV1>();
        public static List<ILoaderBehaviourV1> LoaderConfigPlugins { get; private set; } = new List<ILoaderBehaviourV1>();

        /// <summary>
        /// The constructor gets a list of enabled plugins and populates the individual interface lists.
        /// </summary>
        static PluginLoader()
        {
            Initialize();
        }

        /// <summary>
        /// Gets all plugins for Reloaded 
        /// </summary>
        public static void Initialize()
        {
            var pluginConfigs = PluginConfig.GetAllConfigs();
            List<ILoaderEventsV1> loaderEventPlugins  = new List<ILoaderEventsV1>();
            List<ILoaderBehaviourV1> loaderConfigPlugins = new List<ILoaderBehaviourV1>();
            List<ILauncherEventsV1> launcherEventPlugins = new List<ILauncherEventsV1>();

            // Load every DLL belonging to a plugin and populate the plugin list.
            foreach (var pluginConfig in pluginConfigs)
            {
                if (pluginConfig.Enabled)
                {
                    string dllPath = pluginConfig.GetDllPath();
                    if (File.Exists(dllPath))
                    {
                        var pluginDll = Assembly.LoadFrom(dllPath);

                        // Get all classes implementing interfaces, create them and populate lists.
                        foreach (var classType in pluginDll.GetTypes())
                        {
                            if (classType.GetInterfaces().Contains(typeof(ILoaderEventsV1))) { loaderEventPlugins.Add(Activator.CreateInstance(classType) as ILoaderEventsV1); }
                            if (classType.GetInterfaces().Contains(typeof(ILoaderBehaviourV1))) { loaderConfigPlugins.Add(Activator.CreateInstance(classType) as ILoaderBehaviourV1); }
                            if (classType.GetInterfaces().Contains(typeof(ILauncherEventsV1))) { launcherEventPlugins.Add(Activator.CreateInstance(classType) as ILauncherEventsV1); }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"The file path for the plugin {pluginConfig.Name} does not exist. Illegal path: {dllPath}");
                    }

                }
            }

            // Replace the new event and config plugins.
            LoaderEventPlugins.Clear();
            LoaderConfigPlugins.Clear();
            LauncherEventPlugins.Clear();

            LoaderEventPlugins = loaderEventPlugins;
            LoaderConfigPlugins = loaderConfigPlugins;
            LauncherEventPlugins = launcherEventPlugins;
        }
    }
}
