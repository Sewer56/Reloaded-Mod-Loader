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

namespace HeroesModLoaderConfig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SonicHeroes.Misc.Config.LoaderConfigParser loaderConfigParser = new LoaderConfigParser();
            loaderConfigParser.ParseConfig();
        }
    }
}
