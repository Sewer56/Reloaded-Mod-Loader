using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Github_Updater.Github.Queue;
using Octokit;
using Reloaded_Plugin_System.Interfaces.Updaters;

namespace Github_Updater.Github
{
    public class GithubUpdater : IUpdateSourceV1
    {
        /// <summary>
        /// Cache this client for performance reasons.
        /// </summary>
        public GitHubClient GithubClient { get; private set; }

        /// <summary>
        /// The maximum number of mod version check requests still allowed.
        /// </summary>
        public int RateLimit { get; private set; }

        public QueueManager QueueManager { get; private set; }
        public GithubUpdater()
        {
            GithubClient = new GitHubClient(new ProductHeaderValue("Reloaded-Mod-Loader"));
            var rateLimits = GithubClient.Miscellaneous.GetRateLimits().Result;
            RateLimit = rateLimits.Resources.Core.Remaining;
            QueueManager = new QueueManager();
        }

        /*
             ------------------------
             Interface Implementation 
             ------------------------
        */
        public List<IUpdate> GetUpdates()
        {
            // Get all eligible mods from queue.
            // The QueueItem class already guarantees us the members have valid Github links; so we are good here.
            List<QueueItem> queueItems = QueueManager.GetQueue();

            // Get our releases. This function automatically changes the time on the individual elements we are checking.
            // We are practically done here and after this we can re-save our queue.
            // This operation works because QueueItem is a class, a pointer.
            var releases = GetReleases(queueItems);
            QueueManager.SaveQueue(queueItems);

            // Now we just have to process our potential individual releases and we are done.
            return ProcessReleases(releases.Result);
        }

        public string GetSourceName() => "Github";
        public string OnLinkDownload(string downloadLink, string downloadLocation) { return downloadLink; }
        public void OnModExtract(string[] directories) { }

        /*
            --------------------
            Class Implementation 
            --------------------
        */

        /// <summary>
        /// Retrieves the individual releases for a specific Github URI.
        /// </summary>
        /// <returns></returns>
        private async Task<List<QueueItemReleasePair>> GetReleases(List<QueueItem> queueItems)
        {
            // Here we map our releases to individual Queue Items; we want to try obtain all of the releases
            // at once; thus must do so asynchronously.
            var itemReleasePairs = new List<QueueItemReleasePair>(queueItems.Count);
            Task[] releaseTasks = new Task[queueItems.Count];

            // Process the release grabbing process for each of the items in the queue.
            for (int x = 0; (x < queueItems.Count && x < RateLimit); x++)
            {
                // Start with our individual item.
                var queueItem = queueItems[x];

                // Start processing the release check of every item
                var releaseTask = GetRelease(new Uri(queueItems[x].ModConfiguration.ModSource));
                itemReleasePairs.Add(new QueueItemReleasePair(queueItem, releaseTask));
                releaseTasks[x] = releaseTask;

                // Set our queue time here.
                queueItems[x].LastChecked = DateTime.UtcNow;
            }

            // Wait till all item queue grabs are complete.
            await Task.WhenAll(releaseTasks.Where(x => x != null).ToArray());

            return itemReleasePairs;
        }

        /// <summary>
        /// Retrieves the individual release for a specific Github URI.
        /// </summary>
        /// <returns>The result of this is null if the request fails. It can fail either due to an invalid link or hitting rate limit.</returns>
        private async Task<IReadOnlyList<Release>> GetRelease(Uri uri)
        {
            // Check rate limits
            var result = GithubClient.Miscellaneous.GetRateLimits().Result;
            if (result.Resources.Core.Remaining <= 0)
                return null;

            // Check segments, there should only be 3 "/" "username/" "repository"
            if (uri.Segments.Length != 3)
                return null;

            // Get username + repository name from URI
            string userName = uri.Segments[uri.Segments.Length - 2];
            userName = userName.Substring(0, userName.Length - 1);
            string repositoryName = uri.Segments[uri.Segments.Length - 1];

            // :3
            try { return await GithubClient.Repository.Release.GetAll(userName, repositoryName); }
            catch { return null; }
        }

        /// <summary>
        /// Compares the DateTime of the current Config.json and 
        /// looks for a download file in each of the individual files
        /// </summary>
        private List<IUpdate> ProcessReleases(List<QueueItemReleasePair> itemReleasePairs)
        {
            List<IUpdate> updates = new List<IUpdate>(itemReleasePairs.Count);

            // For each of the queue items, if the release is empty, do nothing, if it is not empty
            // try to determine some stuff.
            foreach (var itemReleasePair in itemReleasePairs)
            {
                IUpdate update = itemReleasePair.GetUpdate();
                if (update != null)
                    updates.Add(update);
            }

            return updates;
        }


    }
}
