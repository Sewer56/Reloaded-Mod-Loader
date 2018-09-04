using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Github_Updater.Github.Queue;
using Octokit;
using Reloaded_Plugin_System.Interfaces.Updaters;

namespace Github_Updater.Github
{
    public class QueueItemReleasePair
    {
        /// <summary>
        /// Returns the Releases assigned to this Queue Item and Release list tuple.
        /// </summary>
        public List<Release> Releases => _releaseList.Result.ToList();
        public QueueItem QueueItem { get; set; }
        public bool AreReleasesAvailable => _releaseList.Result != null;
        private Task<IReadOnlyList<Release>> _releaseList;

        public QueueItemReleasePair(QueueItem queueItem, Task<IReadOnlyList<Release>> releaseList)
        {
            QueueItem = queueItem;
            _releaseList = releaseList;
        }

        /// <summary>
        /// Retrieves an individual update item, if possible; else returns null.
        /// </summary>
        public IUpdate GetUpdate()
        {
            // No releases for this pair, continue on.
            if (!AreReleasesAvailable)
                return null;

            // Get the date of our mod's config file.
            DateTime configDateTime = File.GetLastWriteTime(QueueItem.ModConfiguration.ModLocation);
            configDateTime          = configDateTime.ToUniversalTime();

            foreach (var release in Releases)
            {
                if (!IsReleaseNewer(configDateTime, release))
                    return null;

                // Good, the release is newer than our files; let's look for some file name.
                foreach (var asset in release.Assets)
                {
                    if (asset.Name.StartsWith("Update-", StringComparison.OrdinalIgnoreCase))
                    {
                        var modConfig = QueueItem.ModConfiguration;
                        string gameModFolder = Path.GetDirectoryName(modConfig.GetModDirectory());

                        return new Update(modConfig.ModName, modConfig.ModId, modConfig.ModAuthor,$">{modConfig.ModVersion}", new Uri(asset.BrowserDownloadUrl), asset.Size, gameModFolder);
                    }    
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if a Github release is newer than the specified Date/Time combo.
        /// </summary>
        private bool IsReleaseNewer(DateTime dateTime, Release githubRelease)
        {
            // If the release is older, skip.
            var releaseDateTimeOffset = githubRelease.PublishedAt?.ToUniversalTime();
            if (!releaseDateTimeOffset.HasValue)    return false; // Check release publish date existence.
            if (releaseDateTimeOffset < dateTime)   return false; // Check release publish date.

            return true;
        }
    }
}
