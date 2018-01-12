using HeroesModLoaderConfig.Styles.Themes;
using HeroesModLoaderConfig.Utilities.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Windows.Children
{
    public partial class Main_Screen : Form
    {
        /// <summary>
        /// Gets the creation parameters.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x02000000; // WS_EX_TRANSPARENT
                return cp;
            }
        }

        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="MDIParent">The MDI Parent form, an instance of Base.cs</param>
        public Main_Screen(Form MDIParent)
        {
            // Standard Winforms Initialization
            InitializeComponent();

            // Set the MDI parent
            MdiParent = MDIParent;

            // Add to the window list.
            Global.WindowsForms.Add(this);

            // Add Box Controls
            SetupDecorationBoxes.FindDecorationControls(this);
        }

        ///
        private void Main_Screen_Load(object sender, EventArgs e)
        {
            item_GameList.Rows.Add("Sonic Heroes", "Mod-Loader-Mods\\Sonic-Heroes");
            item_GameList.Rows.Add("Sonic Riders", "Mod-Loader-Mods\\Sonic-Riders");
            item_GameList.Rows.Add("Sonic Adventure DX", "Mod-Loader-Mods\\SADX");
            item_GameList.Rows.Add("Sonic Adventure II", "Mod-Loader-Mods\\SA2B");
            item_GameList.Rows.Add("Sonic Generations", "Mod-Loader-Mods\\Sonic-Generations");
            item_GameList.Rows.Add("Sonic Forces", "Mod-Loader-Mods\\Sonic-Forces");
            item_GameList.Rows.Add("Sora no Kiseki SC", "Mod-Loader-Mods\\Sora-SC");
            item_GameList.Rows.Add("Sora no Kiseki 3rd", "Mod-Loader-Mods\\Sora-3rd");
            item_GameList.Rows.Add("Zero no Kiseki", "Mod-Loader-Mods\\Zero-no-Kiseki");
            item_GameList.Rows.Add("Sen no Kiseki", "Mod-Loader-Mods\\Sen-I");
            item_GameList.Rows.Add("Oki Doki Lit Club", "Mod-Loader-Mods\\Just-Monika");
        }
    }
}
