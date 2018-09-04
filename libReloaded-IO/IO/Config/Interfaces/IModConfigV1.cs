using System.Collections.Generic;

namespace Reloaded.IO.Config.Interfaces
{
    /// <summary>
    /// The following fixed interface is designed for the purpose of directly passing individual
    /// mod configurations between plugins without the need of manual serialization.
    ///
    /// The existence of this interface ensures that plugins could remain compatible as different
    /// versions of Reloaded are introduced.
    /// </summary>
    public interface IModConfigV1
    {
        string ConfigurationFile    { get; set; }
        List<string> Dependencies   { get; set; }
        string DllFile32            { get; set; }
        string DllFile64            { get; set; }
        string ModAuthor            { get; set; }
        string ModDescription       { get; set; }
        string ModId                { get; set; }
        string ModLocation          { get; set; }
        string ModName              { get; set; }
        string ModSite              { get; set; }
        string ModSource            { get; set; }
        string ModVersion           { get; set; }
        GameConfig ParentGame       { get; set; }

        bool CheckDependencies();
        List<ModConfig> GetAllDependencies();
        List<ModConfig> GetDisabledDependencies();
        List<ModConfig> GetEnabledDependencies();
        List<string> GetMissingDependencies();
        string GetModDirectory();
        string GetModDirectoryName();
        bool IsEnabled();
        bool IsValidModConfig();
    }
}