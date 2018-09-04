using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Console_Colourizer
{
    public class Settings
    {
        public static string FilePath;
        public bool PickRandom = false;

        static Settings()
        {
            FilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + $"\\Settings.json";

            // Write an empty file if file does not exist.
            if (! File.Exists(FilePath))
                new Settings().WriteSettings();
        }

        /*
            ---
            I/O
            ---
        */

        public static Settings ParseSettings()
        {
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FilePath));
        }

        public void WriteSettings()
        {
            
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);

            // Append message.
            json +=
@"
/*
    PickRandom: If set to false, the Colourizer will cycle colours instead of picking random colours.
*/";

            File.WriteAllText(FilePath, json);
        }
    }
}
