using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Reloaded_Plugin_System.Utilities.Downloader
{
    /// <summary>
    /// Utility class around the WebClient to be used for downloading files.
    /// </summary>
    public static class FileDownloader
    {
        /*
            -------
            Methods
            -------
        */

        /// <summary>
        /// Retrieves the file name of a file behind a specific URL.
        /// Returns "UnknownFile.xxx" if the name could not be determined.
        /// </summary>
        public static string GetFileName(Uri url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    // Obtain the name of the file.
                    client.OpenRead(url);
                    return new ContentDisposition(client.ResponseHeaders["Content-Disposition"]).FileName;
                }
            }
            catch { return "UnknownFile.xxx"; }
        }

        public static async Task<byte[]> DownloadFile(Uri uri)
        {
            // Start the modification download.
            using (WebClient client = new WebClient())
            {
                return await client.DownloadDataTaskAsync(uri);
            }
        }

        public static async Task<byte[]> DownloadFile (
            Uri uri, 
            DownloadDataCompletedEventHandler downloadCompleted = null, 
            DownloadProgressChangedEventHandler downloadProgressChanged = null
        )
        {
            // Start the modification download.
            using (WebClient client = new WebClient())
            {
                client.DownloadDataCompleted += downloadCompleted;
                client.DownloadProgressChanged += downloadProgressChanged;
                return await client.DownloadDataTaskAsync(uri);
            }
        }

    }
}
