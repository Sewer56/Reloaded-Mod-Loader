using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octokit;
using Reloaded.IO.Config;
using ReloadedUpdateChecker.Updaters;
using ReloadedUpdateChecker.Updaters.Implementations.Github.Queue;

namespace ReloadedUpdateChecker
{
    public static class UpdateChecker
    {
        /// <summary>
        /// Contains every interface implementation that defines a source mod updates may
        /// be received from.
        /// </summary>
        public static List<IUpdateSource> UpdateSources { get; private set; }

        static UpdateChecker()
        {
            UpdateSources = GetUpdateSources();
        }

        /// <summary>
        /// Retrieves all updates from 3rd party website update source classes which
        /// inherit from the <see cref="IUpdateSource"/> interface.
        /// </summary>
        public static async Task<List<Update>> GetAllUpdatesFromSources()
        {
            List<Update> updates = new List<Update>();

            Task[] getUpdateTasks = new Task[UpdateSources.Count];
            for (int x = 0; x < UpdateSources.Count; x++)
            {
                var index = x;
                getUpdateTasks[index] = Task.Run(() => { updates.AddRange(UpdateSources[index].GetUpdates()); });
            }

            await Task.WhenAll(getUpdateTasks);
            return updates;
        }

        /// <summary>
        /// Returns all implementations of <see cref="IUpdateSource"/> in loaded assemblies through
        /// the use of Reflection.
        /// </summary>
        private static List<IUpdateSource> GetUpdateSources()
        {
            var interfaceType = typeof(IUpdateSource);
            var all =   AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                        .Select(x => Activator.CreateInstance(x));
            return all.Cast<IUpdateSource>().ToList();
        }
    }
}
