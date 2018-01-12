using HeroesModLoaderConfig.Styles.Animation;
using SonicHeroes.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Styles.Controls.Enhanced
{
    [Serializable]
    class AnimatedDataGridView : DataGridView, IAnimatedControl
    {
        /// <summary>
        /// Stores the animation properties for backcolor and forecolor blending.
        /// </summary>
        public AnimProperties AnimProperties { get; set; }

        public AnimatedDataGridView()
        {
            // Instantiate all of the animation messages.
            this.AnimProperties = new AnimProperties();
            this.AnimProperties.ForeColorMessage = new AnimMessage(this);
            this.AnimProperties.BackColorMessage = new AnimMessage(this);
        }

        // //////////////////////////
        // Common Animation Redirects
        // //////////////////////////
        protected override void OnMouseEnter(EventArgs e) { AnimHandler.AnimateMouseEnter(e, this, AnimProperties); }
        protected override void OnMouseLeave(EventArgs e) { AnimHandler.AnimateMouseLeave(e, this, AnimProperties); }
        public void OnMouseEnterWrapper(EventArgs e) { base.OnMouseEnter(e); }
        public void OnMouseLeaveWrapper(EventArgs e) { base.OnMouseEnter(e); }
    }
}
