using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reloaded.IO.Config;

namespace Reloaded_Steam_Shim
{
    public partial class GameSelector : Form
    {
        private List<GameConfig> _gameConfigurations;
        private ReloadedShimSettings _shimSettings;

        /*
            ------------
            Constructors
            ------------ 
        */

        public GameSelector(List<GameConfig> gameConfigurations, ReloadedShimSettings shimSettings)
        {
            InitializeComponent();
            _gameConfigurations = gameConfigurations;
            _shimSettings = shimSettings;
        }

        /*
            -----------------
            UI Business Logic
            -----------------
        */

        /// <summary>
        /// When the loading finishes, populates the list of games to start.
        /// </summary>
        private void GameSelector_Load(object sender, EventArgs e)
        {
            foreach (var gameConfiguration in _gameConfigurations)
            {
                item_GameList.Items.Add(new ListViewItem(gameConfiguration.GameName));
            }
        }

        /// <summary>
        /// Start a user specified game profile.
        /// </summary>
        private void item_LaunchGame_Click(object sender, EventArgs e)
        {
            // Get directory of game config to pass to loader.
            string gameConfig       = _gameConfigurations[item_GameList.SelectedIndices[0]].ConfigLocation;
            string gameConfigFolder = Path.GetDirectoryName(gameConfig);

            // Remember choice and then launch game.
            if (item_RememberThisBox.Checked)
                _shimSettings.LoadByDefault = gameConfigFolder;
            else
                _shimSettings.LoadByDefault = "";

            // Save shim settings and launch game.
            _shimSettings.SaveShim();
            this.Close();
            Functions.LaunchGame(gameConfigFolder);
        }
    }
}
