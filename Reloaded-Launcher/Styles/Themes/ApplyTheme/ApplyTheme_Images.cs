using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReloadedLauncher.Styles.Themes
{
    /// <summary>
    /// Applies a theme to the current form.
    /// This file handles the image aspect of the form.
    /// </summary>
    public static partial class ApplyTheme
    {
        /// <summary>
        /// Loads all of the theme images from storage onto the relevant base form buttons.
        /// </summary>
        /// <param name="imagesFolder">Dictates the folder where the images are supposed to be loaded from.</param>
        public static void LoadImages(string imagesFolder)
        {
            // If the baseform is instantiated
            if (Global.BaseForm != null)
            {
                // Load the images from HDD.
                Global.BaseForm.categoryBar_About.Image = Image.FromFile(imagesFolder + "\\About-Icon.png");
                Global.BaseForm.categoryBar_Manager.Image = Image.FromFile(imagesFolder + "\\Entry-Icon.png");
                Global.BaseForm.categoryBar_Theme.Image = Image.FromFile(imagesFolder + "\\Paint-Icon.png");
                Global.BaseForm.categoryBar_Input.Image = Image.FromFile(imagesFolder + "\\Controller-Icon.png");
                Global.BaseForm.categoryBar_Mods.Image = Image.FromFile(imagesFolder + "\\Tweaks-Icon.png");
                Global.BaseForm.categoryBar_Games.Image = Image.FromFile(imagesFolder + "\\Main-Icon.png");

                // If the mods form is created.
                if (Global.BaseForm.MDIChildren.ModsMenu != null)
                {
                    // Load the images from HDD.
                    Global.BaseForm.MDIChildren.ModsMenu.borderless_ConfigBox.Image = Image.FromFile(imagesFolder + "\\Tweaks2-Icon.png");
                    Global.BaseForm.MDIChildren.ModsMenu.borderless_SourceBox.Image = Image.FromFile(imagesFolder + "\\Github-Icon.png");
                    Global.BaseForm.MDIChildren.ModsMenu.borderless_WebBox.Image = Image.FromFile(imagesFolder + "\\World-Icon.png");
                }

                // If the theme form is created.
                if (Global.BaseForm.MDIChildren.ThemeMenu != null)
                {
                    // Load the images from HDD.
                    Global.BaseForm.MDIChildren.ThemeMenu.borderless_ConfigBox.Image = Image.FromFile(imagesFolder + "\\Tweaks2-Icon.png");
                    Global.BaseForm.MDIChildren.ThemeMenu.borderless_SourceBox.Image = Image.FromFile(imagesFolder + "\\Github-Icon.png");
                    Global.BaseForm.MDIChildren.ThemeMenu.borderless_WebBox.Image = Image.FromFile(imagesFolder + "\\World-Icon.png");
                }
            }
        }
    }
}
