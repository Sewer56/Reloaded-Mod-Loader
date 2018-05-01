using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded_Mod_Template
{
    /// <summary>
    /// Stores properties of the individual game window, or any native window for that matter.
    /// Used for calculations to be performed in the future.
    /// </summary>
    public class GameWindowProperties
    {
        public int width { get; set; }
        public int height { get; set; }
        public int xPosition { get; set; }
        public int yPosition { get; set; }
    }
}
