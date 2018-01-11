using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicHeroes.Misc.Config
{
    public class ThemeColourParser
    {
        /// <summary>
        /// Stores the ini data read by the ini-parser.
        /// </summary>
        private IniData iniData;

        /// <summary>
        /// Holds an instance of ini-parser used for parsing INI files.
        /// </summary>
        private FileIniDataParser iniParser;

        /// <summary>
        /// Stores all of the colour and animation details of the theme in question.
        /// </summary>
        public struct ColourConfig
        {
            /// <summary>
            /// Specifies the title of the Mod Loader Window.
            /// The name of the current menu is appended to the title, without space.
            /// </summary>
            public string LoaderTitle;

            /// <summary>
            /// Specifies the properties of the button and control borders used
            /// for the theme in question.
            /// </summary>
            public BorderProperties ButtonBorderProperties;

            /// <summary>
            /// Specifies the main form colours.
            /// </summary>
            public BarColours MainColours;

            /// <summary>
            /// Specifies the title bar colours.
            /// </summary>
            public BarColours TitleColours;

            /// <summary>
            /// Specifies the category bar colours.
            /// </summary>
            public BarColours CategoryColours;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the category bar.
            /// </summary>
            public ButtonMouseAnimation CategoryEnterAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the category bar.
            /// </summary>
            public ButtonMouseAnimation CategoryLeaveAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the title bar.
            /// </summary>
            public ButtonMouseAnimation TitleEnterAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the title bar.
            /// </summary>
            public ButtonMouseAnimation TitleLeaveAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the category bar.
            /// </summary>
            public ButtonMouseAnimation MainEnterAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the category bar.
            /// </summary>
            public ButtonMouseAnimation MainLeaveAnimation;
        }

        /// <summary>
        /// Defines the border styles used for buttons.
        /// </summary>
        public struct BorderProperties
        {
            /// <summary>
            /// Defines the background colour of the category/title bar.
            /// </summary>
            public Color BorderColour;

            /// <summary>
            /// The colour of the category/title bar buttons with options such as e.g. Games, Mods
            /// </summary>
            public int BorderWidth;
        }

        /// <summary>
        /// Defines the colours used for controls and buttons on the category bar.
        /// </summary>
        public struct BarColours
        {
            /// <summary>
            /// Defines the background colour of the category/title bar.
            /// </summary>
            public Color BGColour;

            /// <summary>
            /// The colour of the category/title bar buttons with options such as e.g. Games, Mods
            /// </summary>
            public Color ButtonBGColour;

            /// <summary>
            /// The colour of the category/title bar text. e.g. Games, Mods
            /// </summary>
            public Color TextColour;
        }

        /// <summary>
        /// Defines the colours used for controls and buttons on the category bar.
        /// </summary>
        public struct ButtonMouseAnimation
        {
            /// <summary>
            /// Defines whether the backcolor (BG Colour) of a category bar button should be
            /// blended to a target colour when the mouse enters the control.
            /// </summary>
            public bool BlendBGColour;

            /// <summary>
            /// Defines whether the forecolor (text colour) of a category bar button should be
            /// blended to a target colour when the mouse enters the control.
            /// </summary>
            public bool BlendFGColour;

            /// <summary>
            /// The duration/length of the mouse enter event, specifies the duration of the
            /// background and/or foreground blend effects.
            /// </summary>
            public int AnimationDuration;

            /// <summary>
            /// The framerate at which the mouse enter animation to be performed.
            /// It does not affect the length of the animation.
            /// </summary>
            public int AnimationFramerate;

            /// <summary>
            /// The target colour towards which the backcolor of a button is blended
            /// if BlendBGColour is set to true and the mouse enters the button area.
            /// </summary>
            public Color BGTargetColour;

            /// <summary>
            /// The target colour towards which the text colour of a button is blended
            /// if BlendFGColour is set to true and the mouse enters the button area.
            /// </summary>
            public Color FGTargetColour;
        }

        /// <summary>
        /// Initiates the Theme Colour Parser.
        /// </summary>
        public ThemeColourParser()
        {
            iniParser = new FileIniDataParser();
            iniParser.Parser.Configuration.CommentString = "#";
        }

        /// <summary>
        /// Retrieves the Mod Loader Colour Configuration File.
        /// </summary>
        /// <param name="themeDirectory">The relative directory of the individual theme to Mod-Loader-Config/Themes. e.g. Default</param>
        public ColourConfig ParseConfig(string themeDirectory)
        {
            // Instantiate a new configuration struct.
            ColourConfig colourConfig = new ColourConfig();

            // Read the mod loader configuration.
            iniData = iniParser.ReadFile(LoaderPaths.GetModLoaderConfigDirectory() + "/Themes/" + themeDirectory + "/Theme.ini");

            // Parse the title.
            colourConfig.LoaderTitle = iniData["Title"]["LoaderTitle"];

            // Parse the border properties.
            colourConfig.ButtonBorderProperties = new BorderProperties();
            colourConfig.ButtonBorderProperties.BorderColour = ColourLoader.AARRGGBBToColor(iniData["Border Properties"]["BorderColour"]);
            colourConfig.ButtonBorderProperties.BorderWidth = Convert.ToInt32(iniData["Border Properties"]["BorderWidth"]);

            // Parse the Main, Title and Category Colours.
            colourConfig.MainColours = ParseColours("Main Colours");
            colourConfig.TitleColours = ParseColours("Title Colours");
            colourConfig.CategoryColours = ParseColours("Category Colours");

            // Parse the Main, Title and Category Enter and Leave Animations
            colourConfig.CategoryEnterAnimation = ParseAnimations("Category Button Mouse Enter Animation");
            colourConfig.CategoryLeaveAnimation = ParseAnimations("Category Button Mouse Leave Animation");

            colourConfig.MainEnterAnimation = ParseAnimations("Main Button Mouse Enter Animation");
            colourConfig.MainLeaveAnimation = ParseAnimations("Main Button Mouse Leave Animation");

            colourConfig.TitleEnterAnimation = ParseAnimations("Title Button Mouse Enter Animation");
            colourConfig.TitleLeaveAnimation = ParseAnimations("Title Button Mouse Leave Animation");

            // Return the config file.
            return colourConfig;
        }

        /// <summary>
        /// Parses the BarColours struct of colours for a certain item/section inside the .ini file.
        /// BarColours defines button colours, text colours and background colours.
        /// </summary>
        /// <param name="iniCategoryTitleName">The category name inside the .ini file storing the properties for <X> type of objects. e.g. Main Colours, Category Colours</param>
        private BarColours ParseColours(string iniCategoryTitleName)
        {
            // Store the colours.
            BarColours colours = new BarColours();

            // Parse the colours.
            colours.BGColour = ColourLoader.AARRGGBBToColor(iniData[iniCategoryTitleName]["BGColour"]);
            colours.ButtonBGColour = ColourLoader.AARRGGBBToColor(iniData[iniCategoryTitleName]["ButtonBGColour"]);
            colours.TextColour = ColourLoader.AARRGGBBToColor(iniData[iniCategoryTitleName]["TextColour"]);

            // Return
            return colours;
        }

        /// <summary>
        /// Parses the ButtonMouseAnimation struct of mouse enter/exit kanimations 
        /// for a certain item/section inside the .ini file.
        /// </summary>
        /// <param name="iniCategoryTitleName">The category name inside the .ini file storing the properties for <X> type of objects. e.g. Main Colours, Category Colours</param>
        private ButtonMouseAnimation ParseAnimations(string iniCategoryTitleName)
        {
            // Store the animations.
            ButtonMouseAnimation animations = new ButtonMouseAnimation();

            // Parse the animation.
            animations.BlendBGColour = bool.Parse(iniData[iniCategoryTitleName]["BlendBGColour"]);
            animations.BlendFGColour = bool.Parse(iniData[iniCategoryTitleName]["BlendFGColour"]);

            animations.AnimationDuration = Convert.ToInt32(iniData[iniCategoryTitleName]["AnimationDuration"]);
            animations.AnimationFramerate = Convert.ToInt32(iniData[iniCategoryTitleName]["AnimationFramerate"]);

            animations.BGTargetColour = ColourLoader.AARRGGBBToColor(iniData[iniCategoryTitleName]["BGTargetColour"]);
            animations.FGTargetColour = ColourLoader.AARRGGBBToColor(iniData[iniCategoryTitleName]["FGTargetColour"]);

            // Return
            return animations;
        }
    }
}
