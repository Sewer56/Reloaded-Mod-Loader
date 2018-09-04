using System.Collections.Generic;

namespace Reloaded_Plugin_System.Interfaces.Updaters
{
    public interface IUpdateSourceV1
    {
        /// <summary>
        /// Retrieves a list of updates from the update source.
        /// The update source must itself know or determine which mods must be checked and
        /// how to check those mods. The other methods (such as the 1 click link events) are
        /// designed to help you do this.
        /// </summary>
        /// <returns></returns>
        List<IUpdate> GetUpdates();

        /// <summary>
        /// Returns the name of where the update is being sourced from.
        /// This is purely for the GUI.
        /// </summary>
        /// <returns></returns>
        string GetSourceName();

        /*
            Pseudo-events
        */

        /// <summary>
        /// This method gets automatically called in your updater
        /// implementation right before downloading a link.
        /// The download location is where the file will be downloaded to before extraction.
        /// The return value is the new link to download from, in the case it needs to be altered.
        /// </summary>
        string OnLinkDownload(string downloadLink, string downloadLocation);

        /// <summary>
        /// This method gets called after the link is downloaded and the
        /// mod is extracted; it returns the directory names from the inside of the mod.
        /// </summary>
        void OnModExtract(string[] directories);
    }
}
