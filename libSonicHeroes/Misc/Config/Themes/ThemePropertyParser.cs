using IniParser;
using IniParser.Model;
using System;
using System.Drawing;

namespace SonicHeroes.Misc.Config
{
    public class ThemePropertyParser
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
        /// Stores all of the general theme colours and properties in question.
        /// </summary>
        public struct ThemeConfig
        {
            /// <summary>
            /// Specifies the title properties which declare the style of how the mod loader
            /// title is presented to the user.
            /// </summary>
            public TitleProperties TitleProperties { get; set; }

            /// <summary>
            /// The style of title font.
            /// </summary>
            public FontStyle TitleFontStyle { get; set; }

            /// <summary>
            /// The style of category font.
            /// </summary>
            public FontStyle CategoryFontStyle { get; set; }

            /// <summary>
            /// The style of text font.
            /// </summary>
            public FontStyle TextFontStyle { get; set; }

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
            /// Specifies the category bar colours.
            /// </summary>
            public BarColours BorderlessColours;

            /// <summary>
            /// Specifies the colours for the WinForm buttons used as decorations that serve no functionality
            /// </summary>
            public BarColours BoxColours;

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
            /// Specifies the mouse enter animation behaviour for the items in the main forms.
            /// </summary>
            public ButtonMouseAnimation MainEnterAnimation;

            /// <summary>
            /// Specifies the mouse leave animation behaviour for the items in the main forms.
            /// </summary>
            public ButtonMouseAnimation MainLeaveAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
            public ButtonMouseAnimation BoxEnterAnimation;

            /// <summary>
            /// Specifies the mouse leave animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
            public ButtonMouseAnimation BoxLeaveAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
            public ButtonMouseAnimation BorderlessEnterAnimation;

            /// <summary>
            /// Specifies the mouse leave animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
            public ButtonMouseAnimation BorderlessLeaveAnimation;
        }

        /// <summary>
        /// Defines the common properties that define how the mod loader title is presented
        /// within the theme.
        /// </summary>
        public struct TitleProperties
        {
            /// <summary>
            /// Specifies the title of the Mod Loader Window.
            /// The name of the current menu is appended to the title, without space.
            /// </summary>
            public string LoaderTitle;

            /// <summary>
            /// Sets the character that is shown between the loader title
            /// and the current menu that the user is in.
            /// </summary>
            public string LoaderTitleDelimiter;

            /// <summary>
            /// Sets the whether the title delimiter and menu name is
            /// placed before or after the actual LoaderTitle field.
            /// false sets it after, true places it before.
            /// </summary>
            public bool LoaderTitlePrefix;

            /// <summary>
            /// Set to true if there should be a space before and after
            /// the delimiter character. (ini parser cannot read spaces)
            /// </summary>
            public bool DelimiterHasSpaces;
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
        /// Defines the font style for fonts used to represent particular elements.
        /// </summary>
        public struct FontStyle
        {
            /// <summary>
            /// Declares whether the font is underlined (line under text).
            /// </summary>
            public bool Underlined { get; set; }

            /// <summary>
            /// Declares whether the font is striked (line through text).
            /// </summary>
            public bool Striked { get; set; }

            /// <summary>
            /// Declares whether the font is bold.
            /// </summary>
            public bool Bold { get; set; }

            /// <summary>
            /// Declares whether the font is italic (tilted text).
            /// </summary>
            public bool Italic { get; set; }
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
        public ThemePropertyParser()
        {
            iniParser = new FileIniDataParser();
            iniParser.Parser.Configuration.CommentString = "#";
        }

        /// <summary>
        /// Retrieves the Mod Loader Colour Configuration File.
        /// </summary>
        /// <param name="themeDirectory">The relative directory of the individual theme to Mod-Loader-Config/Themes. e.g. Default</param>
        public ThemeConfig ParseConfig(string themeDirectory)
        {
            // Instantiate a new configuration struct.
            ThemeConfig colourConfig = new ThemeConfig();

            // Read the mod loader configuration.
            iniData = iniParser.ReadFile(LoaderPaths.GetModLoaderConfigDirectory() + "/Themes/" + themeDirectory + "/Theme.ini");

            // Parse the title properties .
            TitleProperties titleProperties = new TitleProperties();
            titleProperties.LoaderTitle = iniData["Title"]["LoaderTitle"];
            titleProperties.LoaderTitleDelimiter = iniData["Title"]["LoaderTitleDelimiter"];
            titleProperties.LoaderTitlePrefix = bool.Parse(iniData["Title"]["LoaderTitlePrefix"]);
            titleProperties.DelimiterHasSpaces = bool.Parse(iniData["Title"]["DelimiterHasSpaces"]);
            colourConfig.TitleProperties = titleProperties;

            // Parse font style.
            colourConfig.TitleFontStyle = ParseFontStyle("Title Font");
            colourConfig.CategoryFontStyle = ParseFontStyle("Category Font");
            colourConfig.TextFontStyle = ParseFontStyle("Text Font");

            // Parse the border properties.
            colourConfig.ButtonBorderProperties = new BorderProperties();
            colourConfig.ButtonBorderProperties.BorderColour = ColourLoader.AARRGGBBToColor(iniData["Border Properties"]["BorderColour"]);
            colourConfig.ButtonBorderProperties.BorderWidth = Convert.ToInt32(iniData["Border Properties"]["BorderWidth"]);

            // Parse the Main, Title and Category Colours.
            colourConfig.MainColours = ParseColours("Main Colours");
            colourConfig.TitleColours = ParseColours("Title Colours");
            colourConfig.CategoryColours = ParseColours("Category Colours");
            colourConfig.BoxColours = ParseColours("Box Colours");
            colourConfig.BorderlessColours = ParseColours("Borderless Colours");

            // Parse the Main, Title and Category Enter and Leave Animations
            colourConfig.CategoryEnterAnimation = ParseAnimations("Category Button Mouse Enter Animation");
            colourConfig.CategoryLeaveAnimation = ParseAnimations("Category Button Mouse Leave Animation");

            colourConfig.MainEnterAnimation = ParseAnimations("Main Button Mouse Enter Animation");
            colourConfig.MainLeaveAnimation = ParseAnimations("Main Button Mouse Leave Animation");

            colourConfig.TitleEnterAnimation = ParseAnimations("Title Button Mouse Enter Animation");
            colourConfig.TitleLeaveAnimation = ParseAnimations("Title Button Mouse Leave Animation");

            colourConfig.BoxEnterAnimation = ParseAnimations("Box Mouse Enter Animation");
            colourConfig.BoxLeaveAnimation = ParseAnimations("Box Mouse Leave Animation");

            colourConfig.BorderlessEnterAnimation = ParseAnimations("Borderless Mouse Enter Animation");
            colourConfig.BorderlessLeaveAnimation = ParseAnimations("Borderless Mouse Leave Animation");

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
        /// Parses the FontStyle struct of font style properties for a certain item/section inside the .ini file.
        /// FontStyle defines the style of the fonts.
        /// </summary>
        /// <param name="iniCategoryTitleName">The category inside the .ini file containing the font style. e.g. "Category Font"</param>
        /// <returns></returns>
        private FontStyle ParseFontStyle(string iniCategoryTitleName)
        {
            // Store font style
            FontStyle fontStyle = new FontStyle();

            // Parse font style.
            fontStyle.Underlined = bool.Parse(iniData[iniCategoryTitleName]["FontIsUnderlined"]);
            fontStyle.Bold = bool.Parse(iniData[iniCategoryTitleName]["FontIsBold"]);
            fontStyle.Italic = bool.Parse(iniData[iniCategoryTitleName]["FontIsItalic"]);
            fontStyle.Striked = bool.Parse(iniData[iniCategoryTitleName]["FontIsStriked"]);

            // Return font style.
            return fontStyle;
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
