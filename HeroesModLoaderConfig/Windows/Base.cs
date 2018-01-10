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
        /// <summary>
        /// Constructor.
        /// </summary>
        public Base()
        {
            InitializeComponent();
            Global.WindowsForms.Add(this);
        }

        /// <summary>
        /// Load the global theme once the base form has finished loading (all MDI children should also have finished loading)
        /// by then, as they are loaded in the constructor, pretty convenient.
        /// </summary>
        private void Base_Load(object sender, EventArgs e)
        {
            // Load the global theme.
            Global.Theme.LoadTheme();
        }
    }
}
