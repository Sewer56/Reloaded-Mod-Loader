using System.Collections.Generic;
using System.Threading.Tasks;
using Reloaded_Plugin_System.Interfaces.Updaters;

namespace Reloaded_Plugin_System
{
    public static class UpdateChecker
    {
        /// <summary>
        /// Retrieves all updates from 3rd party website update source classes which
        /// inherit from the <see cref="IUpdateSourceV1"/> interface.
        /// </summary>
        public static async Task<List<IUpdate>> GetAllUpdatesFromSources()
        {
            List<IUpdate> updates = new List<IUpdate>();

            Task[] getUpdateTasks = new Task[PluginLoader.UpdateSourcePlugins.Count];
            for (int x = 0; x < PluginLoader.UpdateSourcePlugins.Count; x++)
            {
                var index = x;
                getUpdateTasks[index] = Task.Run(() => { updates.AddRange(PluginLoader.UpdateSourcePlugins[index].GetUpdates()); });
            }

            await Task.WhenAll(getUpdateTasks);
            return updates;
        }
    }
}
