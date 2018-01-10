using HeroesModLoaderConfig.Utilities.Fonts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SonicHeroes.Misc.Config;
using System.Threading;

namespace HeroesModLoaderConfig
{
    public partial class Base : Form
    {
        public Base()
        {
            InitializeComponent();
            InitializeTheme();
        }

        /// <summary>
        /// Initializes the current windows form theming properties.
        /// </summary>
        private void InitializeTheme()
        {
            // Add the form to the global forms list.
            Global.WindowsForms.Add(this);

            // Themes the current windows form.
            Styles.Themes.ApplyTheme.ThemeWindowsForm(this);
        }
    }
}
