using System.Collections.Generic;

namespace Reloaded.IO.Config.Interfaces
{
    /// <summary>
    /// The following fixed interface is designed for the purpose of directly passing individual
    /// game configurations between plugins without the need of manual serialization.
    ///
    /// The existence of this interface ensures that plugins could remain compatible as different
    /// versions of Reloaded are introduced.
    /// </summary>
    public interface IGameConfigV1
    {
        string CommandLineArgs          { get; set; }
        string ConfigLocation           { get; set; }
        List<string> EnabledMods        { get; set; }
        string ExecutableLocation       { get; set; }
        string GameDirectory            { get; set; }
        string GameName                 { get; set; }
        string GameVersion              { get; set; }
        string ModDirectory             { get; set; }
    }
}