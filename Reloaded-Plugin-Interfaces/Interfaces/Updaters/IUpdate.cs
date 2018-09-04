using System;

namespace Reloaded_Plugin_System.Interfaces.Updaters
{
    /// <summary>
    /// The individual interface to be used by plugins when passing updates to the loaders.
    /// </summary>
    public interface IUpdate
    {
        Uri DownloadLink        { get; set; }
        int FileSize            { get; set; }
        string GameModFolder    { get; set; }
        string ModAuthor        { get; set; }
        string ModId            { get; set; }
        string ModName          { get; set; }
        string ModVersion       { get; set; }

        float FileSizeGB        { get; }
        string FileSizeGBString { get; }
        float FileSizeKB        { get; }
        string FileSizeKBString { get; }
        float FileSizeMB        { get; }
        string FileSizeMBString { get; }
    }
}