using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesModLoaderConfig.Styles.Animation
{
    /// <summary>
    /// Specifies enumerables, methods and general structs, variables, etc. used for event overrides.
    /// e.g. diverting OnEnter and OnLeave events for mouse buttons.
    /// </summary>
    class EventOverrides
    {
        

        /// <summary>
        /// Specifies overrides for the mouse enter events for custom animated windows forms
        /// controls. 
        /// </summary>
        [Flags]
        public enum MouseEnterOverride
        {
            /// <summary>
            /// Run default mouse enter events.
            /// </summary>
            None,
            /// <summary>
            /// Interpolates the BackColor from the current original BackColor to a new custom specified BackColor.
            /// </summary>
            BackColorInterpolate,
            /// <summary>
            /// Interpolates the ForeColor from the current original ForeColor to a new custom specified ForeColor.
            /// </summary>
            ForeColorInterpolate
        }

        /// <summary>
        /// Specifies overrides for the mouse leave events for custom animated windows forms
        /// controls. 
        /// </summary>
        [Flags]
        public enum MouseLeaveOverride
        {
            /// <summary>
            /// Run default mouse enter events.
            /// </summary>
            None,
            /// <summary>
            /// Interpolates the BackColor from the current original BackColor to a new custom specified BackColor.
            /// </summary>
            BackColorInterpolate,
            /// <summary>
            /// Interpolates the ForeColor from the current original ForeColor to a new custom specified ForeColor.
            /// </summary>
            ForeColorInterpolate
        }
    }
}
