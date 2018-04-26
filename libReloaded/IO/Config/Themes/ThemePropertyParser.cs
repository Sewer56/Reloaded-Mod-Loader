/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
    Copyright (C) 2018  Sewer. Sz (Sewer56)

    [Reloaded] is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    [Reloaded] is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>
*/

using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using Reloaded.Utilities;

namespace Reloaded.IO.Config.Themes
{
    public static class ThemePropertyParser
    {
        /// <summary>
        /// Defines the default theme for Reloaded GUI applications.
        /// </summary>
<<<<<<< refs/remotes/origin/master
        private static readonly ThemeConfig _defaultTheme = new ThemeConfig()
=======
        private readonly ThemeConfig _defaultTheme = new ThemeConfig()
>>>>>>> Fix a bunch of warnings
        {
            // [Title]
            TitleProperties = new TitleProperties()
            {
                LoaderTitle = "Reloaded",
                LoaderTitlePrefix = true,
                LoaderTitleDelimiter = "!"
            },

            // [Title Font]
            TitleFontStyle = new FontStyle()
            {
                Bold = false,
                Underlined = false,
                Italic = false,
                Striked = false
            },

            // [Category Font]
            CategoryFontStyle = new FontStyle()
            {
                Bold = false,
                Underlined = false,
                Italic = false,
                Striked = false
            },

            // [Text Font]
            TextFontStyle = new FontStyle()
            {
                Bold = false,
                Underlined = false,
                Italic = false,
                Striked = false
            },

            // [Border Properties]
            BorderProperties = new BorderProperties()
            {
                BorderColour = Color.FromArgb(0xff, 0x4c, 0x4c, 0x4c),
                BorderWidth = 1
            },

            // [Main Colours]
            MainColours = new BarColours()
            {
                BgColour = Color.FromArgb(0xff, 0x18, 0x18, 0x18),
                ButtonBgColour = Color.FromArgb(0xff, 0x79, 0x39, 0x39),
                TextColour = Color.FromArgb(0xff, 0xff, 0xff, 0xff)
            },

            // [Box Colours]
            BoxColours = new BarColours()
            {
                BgColour = Color.FromArgb(0xff, 0x18, 0x18, 0x18),
                ButtonBgColour = Color.FromArgb(0xff, 0x18, 0x18, 0x18),
                TextColour = Color.FromArgb(0xff, 0xff, 0xff, 0xff)
            },

            // [Title Colours]
            TitleColours = new BarColours()
            {
                BgColour = Color.FromArgb(0xff, 0x79, 0x39, 0x39),
                ButtonBgColour = Color.FromArgb(0xff, 0x79, 0x39, 0x39),
                TextColour = Color.FromArgb(0xff, 0xff, 0xff, 0xff)
            },

            // [Borderless Colours]
            BorderlessColours = new BarColours()
            {
                BgColour = Color.FromArgb(0xff, 0x79, 0x39, 0x39),
                ButtonBgColour = Color.FromArgb(0xff, 0x18, 0x18, 0x19),
                TextColour = Color.FromArgb(0xff, 0xff, 0xff, 0xff)
            },

            // [Category Colours]
            CategoryColours = new BarColours()
            {
                BgColour = Color.FromArgb(0xff, 0x4e, 0x27, 0x27),
                ButtonBgColour = Color.FromArgb(0xff, 0x4e, 0x27, 0x27),
                TextColour = Color.FromArgb(0xff, 0xc0, 0xc0, 0xc0)
            },

            // [Category Button Mouse Enter Animation]
            CategoryEnterAnimation = new ButtonMouseAnimation()
            {
                BlendFgColour = false,
                BlendBgColour = true,
                AnimationDuration = 100,
                AnimationFramerate = 144,
                BgTargetColour = Color.FromArgb(0xff, 0x62, 0x32, 0x32),
                FgTargetColour = Color.FromArgb(0x00, 0x00, 0x00, 0x00)
            },

            // [Category Button Mouse Leave Animation]
            CategoryLeaveAnimation = new ButtonMouseAnimation()
            {
                BlendBgColour = true,
                BlendFgColour = false,
                AnimationDuration = 150,
                AnimationFramerate = 144,
                BgTargetColour = Color.FromArgb(0xff, 0x4e, 0x27, 0x27),
                FgTargetColour = Color.FromArgb(0xff, 0x00, 0x00, 0x00),
            },

            // [Title Button Mouse Enter Animation]
            TitleEnterAnimation = new ButtonMouseAnimation()
            {
                BlendBgColour = false,
                BlendFgColour = false,
                AnimationDuration = 0,
                AnimationFramerate = 0,
                BgTargetColour = Color.FromArgb(0xff, 0x00, 0x00, 0x00),
                FgTargetColour = Color.FromArgb(0xff, 0xff, 0x00, 0x00),
            },

            // [Title Button Mouse Enter Animation]
            TitleLeaveAnimation = new ButtonMouseAnimation()
            {
                BlendBgColour = false,
                BlendFgColour = false,
                AnimationDuration = 0,
                AnimationFramerate = 0,
                BgTargetColour = Color.FromArgb(0xff, 0x00, 0x00, 0x00),
                FgTargetColour = Color.FromArgb(0xff, 0xff, 0xff, 0xff),
            },

            // [Main Button Mouse Enter Animation]
            MainEnterAnimation = new ButtonMouseAnimation()
            {
                BlendBgColour = true,
                BlendFgColour = false,
                AnimationDuration = 150,
                AnimationFramerate = 144,
                BgTargetColour = Color.FromArgb(0xff, 0x33, 0x33, 0x33),
                FgTargetColour = Color.FromArgb(0xff, 0x00, 0x00, 0x00),
            },

            // [Main Button Mouse Leave Animation]
            MainLeaveAnimation = new ButtonMouseAnimation()
            {
                BlendBgColour = true,
                BlendFgColour = false,
                AnimationDuration = 150,
                AnimationFramerate = 144,
                BgTargetColour = Color.FromArgb(0xff, 0x79, 0x39, 0x39),
                FgTargetColour = Color.FromArgb(0xff, 0x00, 0x00, 0x00),
            },

            // [Box Mouse Enter Animation]
            BoxEnterAnimation = new ButtonMouseAnimation()
            {
                BlendBgColour = true,
                BlendFgColour = false,
                AnimationDuration = 100,
                AnimationFramerate = 60,
                BgTargetColour = Color.FromArgb(0xff, 0x33, 0x33, 0x33),
                FgTargetColour = Color.FromArgb(0x00, 0x00, 0x00, 0x00),
            },

            // [Box Mouse Leave Animation]
            BoxLeaveAnimation = new ButtonMouseAnimation()
            {
                BlendBgColour = true,
                BlendFgColour = false,
                AnimationDuration = 100,
                AnimationFramerate = 60,
                BgTargetColour = Color.FromArgb(0xff, 0x18, 0x18, 0x18),
                FgTargetColour = Color.FromArgb(0x00, 0x00, 0x00, 0x00),
            },

            // [Borderless Mouse Enter Animation]
            BorderlessEnterAnimation = new ButtonMouseAnimation()
            {
                BlendBgColour = false,
                BlendFgColour = true,
                AnimationDuration = 200,
                AnimationFramerate = 60,
                BgTargetColour = Color.FromArgb(0x00, 0x00, 0x00, 0x00),
                FgTargetColour = Color.FromArgb(0xff, 0xb6, 0x63, 0x63),
            },

            // [Borderless Mouse Leave Animation]
            BorderlessLeaveAnimation = new ButtonMouseAnimation()
            {
                BlendBgColour = false,
                BlendFgColour = true,
                AnimationDuration = 100,
                AnimationFramerate = 60,
                BgTargetColour = Color.FromArgb(0xff, 0x00, 0x00, 0x00),
                FgTargetColour = Color.FromArgb(0xff, 0xff, 0xff, 0xff),
            },
        };

        /// <summary>
        /// Retrieves the Mod Loader Colour Configuration File.
        /// </summary>
        /// <param name="themeDirectory">The relative directory of the individual theme to Reloaded-Config/Themes. e.g. Default</param>
        public static ThemeConfig ParseConfig(string themeDirectory)
        {
            // Specifies the location of the current theme property file.
            string themeLocation = LoaderPaths.GetModLoaderThemeDirectory() + "/" + themeDirectory + $"/{Strings.Parsers.ThemeFile}";

            // Try parsing the config file, else return default one.
            ThemeConfig themeConfig;
            try
            {
<<<<<<< refs/remotes/origin/master
                themeConfig = File.Exists(themeLocation)
                    ? JsonConvert.DeserializeObject<ThemeConfig>(File.ReadAllText(themeLocation))
                    : ThemeConfig.GetDefaultThemeConfig();
            }
            catch { themeConfig = ThemeConfig.GetDefaultThemeConfig(); }
=======
                // Check if theme exists, else reset theme.
                if (!Directory.Exists(LoaderPaths.GetModLoaderThemeDirectory() + Path.DirectorySeparatorChar + themeDirectory))
                {
                    string themeDirectories = Directory.GetDirectories(LoaderPaths.GetModLoaderThemeDirectory())[0];
                    themeDirectory = Path.GetDirectoryName(themeDirectories);
                }

                // Instantiate a new configuration struct.
                ThemeConfig themeProperties = new ThemeConfig();

                // Read the mod loader configuration.
                themeProperties.ThemePropertyLocation = LoaderPaths.GetModLoaderThemeDirectory() + Path.DirectorySeparatorChar + themeDirectory + Path.DirectorySeparatorChar + Strings.Parsers.ThemeFile;
                _iniData = _iniParser.ReadFile(themeProperties.ThemePropertyLocation);

                // Parse the title properties .
                TitleProperties titleProperties = new TitleProperties();
                titleProperties.LoaderTitle = _iniData["Title"]["LoaderTitle"];
                titleProperties.LoaderTitleDelimiter = _iniData["Title"]["LoaderTitleDelimiter"];
                titleProperties.LoaderTitlePrefix = bool.Parse(_iniData["Title"]["LoaderTitlePrefix"]);
                titleProperties.DelimiterHasSpaces = bool.Parse(_iniData["Title"]["DelimiterHasSpaces"]);
                themeProperties.TitleProperties = titleProperties;

                // Parse font style.
                themeProperties.TitleFontStyle = ParseFontStyle("Title Font");
                themeProperties.CategoryFontStyle = ParseFontStyle("Category Font");
                themeProperties.TextFontStyle = ParseFontStyle("Text Font");

                // Parse the border properties.
                var borderProperties = new BorderProperties();
                borderProperties.BorderColour = ColourLoader.AARRGGBBToColor(_iniData["Border Properties"]["BorderColour"]);
                borderProperties.BorderWidth = Convert.ToInt32(_iniData["Border Properties"]["BorderWidth"]);
                themeProperties.BorderProperties = borderProperties;

                // Parse the Main, Title and Category Colours.
                themeProperties.MainColours = ParseColours("Main Colours");
                themeProperties.TitleColours = ParseColours("Title Colours");
                themeProperties.CategoryColours = ParseColours("Category Colours");
                themeProperties.BoxColours = ParseColours("Box Colours");
                themeProperties.BorderlessColours = ParseColours("Borderless Colours");

                // Parse the Main, Title and Category Enter and Leave Animations
                themeProperties.CategoryEnterAnimation = ParseAnimations("Category Button Mouse Enter Animation");
                themeProperties.CategoryLeaveAnimation = ParseAnimations("Category Button Mouse Leave Animation");

                themeProperties.MainEnterAnimation = ParseAnimations("Main Button Mouse Enter Animation");
                themeProperties.MainLeaveAnimation = ParseAnimations("Main Button Mouse Leave Animation");

                themeProperties.TitleEnterAnimation = ParseAnimations("Title Button Mouse Enter Animation");
                themeProperties.TitleLeaveAnimation = ParseAnimations("Title Button Mouse Leave Animation");

                themeProperties.BoxEnterAnimation = ParseAnimations("Box Mouse Enter Animation");
                themeProperties.BoxLeaveAnimation = ParseAnimations("Box Mouse Leave Animation");

                themeProperties.BorderlessEnterAnimation = ParseAnimations("Borderless Mouse Enter Animation");
                themeProperties.BorderlessLeaveAnimation = ParseAnimations("Borderless Mouse Leave Animation");

                // Return the config file.
                return themeProperties;
            }
            catch (Exception)
            {
                MessageBox.Show($"Failed to parse theme: {themeDirectory}, Reloaded will try to instead load its in-library default theme.");
                return _defaultTheme;
            }
        }

        /// <summary>
        /// Parses the BarColours struct of colours for a certain item/section inside the .ini file.
        /// BarColours defines button colours, text colours and background colours.
        /// </summary>
        /// <param name="iniCategoryTitleName">The category name inside the .ini file storing the properties for "X" type of objects. e.g. Main Colours, Category Colours</param>
        private BarColours ParseColours(string iniCategoryTitleName)
        {
            // Store the colours.
            BarColours colours = new BarColours();

            // Parse the colours.
            colours.BgColour = ColourLoader.AARRGGBBToColor(_iniData[iniCategoryTitleName]["BGColour"]);
            colours.ButtonBgColour = ColourLoader.AARRGGBBToColor(_iniData[iniCategoryTitleName]["ButtonBGColour"]);
            colours.TextColour = ColourLoader.AARRGGBBToColor(_iniData[iniCategoryTitleName]["TextColour"]);
>>>>>>> Fix a bunch of warnings

            themeConfig.ThemePropertyLocation = themeLocation;
            return themeConfig;
        }

        /// <summary>
        /// Writes out the config file to disk.
        /// </summary>
        /// <param name="themeConfig">The mod configuration structure defining the details of the individual mod.</param>
        public static void WriteConfig(ThemeConfig themeConfig)
        {
            // Convert structure to JSON
            string json = JsonConvert.SerializeObject(themeConfig, Strings.Parsers.SerializerSettings);

            // Write to disk
            File.WriteAllText(themeConfig.ThemePropertyLocation, json);
        }

        /// <summary>
        /// Stores all of the general theme colours and properties in question.
        /// </summary>
        public struct ThemeConfig
        {
            /// <summary>
            /// Specifies the title properties which declare the style of how the mod loader
            /// title is presented to the user.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public TitleProperties TitleProperties { get; set; }

            /// <summary>
            /// The style of title font.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public FontStyle TitleFontStyle { get; set; }

            /// <summary>
            /// The style of category font.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public FontStyle CategoryFontStyle { get; set; }

            /// <summary>
            /// The style of text font.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public FontStyle TextFontStyle { get; set; }

            /// <summary>
            /// Specifies the properties of the button and control borders used
            /// for the theme in question.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public BorderProperties BorderProperties;
=======
            public BorderProperties BorderProperties { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the main form colours.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public BarColours MainColours;
=======
            public BarColours MainColours { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the title bar colours.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public BarColours TitleColours;
=======
            public BarColours TitleColours { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the category bar colours.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public BarColours CategoryColours;
=======
            public BarColours CategoryColours { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the category bar colours.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public BarColours BorderlessColours;
=======
            public BarColours BorderlessColours { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the colours for the WinForm buttons used as decorations that serve no functionality
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public BarColours BoxColours;
=======
            public BarColours BoxColours { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the category bar.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation CategoryEnterAnimation;
=======
            public ButtonMouseAnimation CategoryEnterAnimation { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the category bar.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation CategoryLeaveAnimation;
=======
            public ButtonMouseAnimation CategoryLeaveAnimation { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the title bar.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation TitleEnterAnimation;
=======
            public ButtonMouseAnimation TitleEnterAnimation { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the title bar.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation TitleLeaveAnimation;
=======
            public ButtonMouseAnimation TitleLeaveAnimation { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items in the main forms.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation MainEnterAnimation;
=======
            public ButtonMouseAnimation MainEnterAnimation { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the mouse leave animation behaviour for the items in the main forms.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation MainLeaveAnimation;
=======
            public ButtonMouseAnimation MainLeaveAnimation { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation BoxEnterAnimation;
=======
            public ButtonMouseAnimation BoxEnterAnimation { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the mouse leave animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation BoxLeaveAnimation;
=======
            public ButtonMouseAnimation BoxLeaveAnimation { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation BorderlessEnterAnimation;
=======
            public ButtonMouseAnimation BorderlessEnterAnimation { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Specifies the mouse leave animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation BorderlessLeaveAnimation;
=======
            public ButtonMouseAnimation BorderlessLeaveAnimation { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// [DO NOT MODIFY] Specifies the location of this theme property configuration file.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonIgnore]
            public string ThemePropertyLocation;

            /// <summary>
            /// Retrieves the default theme configuration.
            /// </summary>
            /// <returns></returns>
            public static ThemeConfig GetDefaultThemeConfig()
            {
                return _defaultTheme;
            }
=======
            public string ThemePropertyLocation { get; internal set; }
>>>>>>> Fix a bunch of warnings
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
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public string LoaderTitle;
=======
            public string LoaderTitle { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Sets the character that is shown between the loader title
            /// and the current menu that the user is in.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public string LoaderTitleDelimiter;
=======
            public string LoaderTitleDelimiter { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Sets the whether the title delimiter and menu name is
            /// placed before or after the actual LoaderTitle field.
            /// false sets it after, true places it before.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public bool LoaderTitlePrefix;
=======
            public bool LoaderTitlePrefix { get; set; }

            /// <summary>
            /// Set to true if there should be a space before and after
            /// the delimiter character. (ini parser cannot read spaces)
            /// </summary>
            public bool DelimiterHasSpaces { get; set; }
>>>>>>> Fix a bunch of warnings
        }

        /// <summary>
        /// Defines the border styles used for buttons.
        /// </summary>
        public struct BorderProperties
        {
            /// <summary>
            /// Defines the background colour of the category/title bar.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public Color BorderColour;
=======
            public Color BorderColour { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// The colour of the category/title bar buttons with options such as e.g. Games, Mods
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public int BorderWidth;
=======
            public int BorderWidth { get; set; }
>>>>>>> Fix a bunch of warnings
        }

        /// <summary>
        /// Defines the colours used for controls and buttons on the category bar.
        /// </summary>
        public struct BarColours
        {
            /// <summary>
            /// Defines the background colour of the category/title bar.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public Color BgColour;
=======
            public Color BgColour { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// The colour of the category/title bar buttons with options such as e.g. Games, Mods
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public Color ButtonBgColour;
=======
            public Color ButtonBgColour { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// The colour of the category/title bar text. e.g. Games, Mods
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public Color TextColour;
=======
            public Color TextColour { get; set; }
>>>>>>> Fix a bunch of warnings
        }

        /// <summary>
        /// Defines the font style for fonts used to represent particular elements.
        /// </summary>
        public struct FontStyle
        {
            /// <summary>
            /// Declares whether the font is underlined (line under text).
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public bool Underlined { get; set; }

            /// <summary>
            /// Declares whether the font is striked (line through text).
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public bool Striked { get; set; }

            /// <summary>
            /// Declares whether the font is bold.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public bool Bold { get; set; }

            /// <summary>
            /// Declares whether the font is italic (tilted text).
            /// </summary>
            [JsonProperty(Required = Required.Default)]
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
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public bool BlendBgColour;
=======
            public bool BlendBgColour { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// Defines whether the forecolor (text colour) of a category bar button should be
            /// blended to a target colour when the mouse enters the control.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public bool BlendFgColour;
=======
            public bool BlendFgColour { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// The duration/length of the mouse enter event, specifies the duration of the
            /// background and/or foreground blend effects.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public int AnimationDuration;
=======
            public int AnimationDuration { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// The framerate at which the mouse enter animation to be performed.
            /// It does not affect the length of the animation.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public int AnimationFramerate;
=======
            public int AnimationFramerate { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// The target colour towards which the backcolor of a button is blended
            /// if BlendBGColour is set to true and the mouse enters the button area.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public Color BgTargetColour;
=======
            public Color BgTargetColour { get; set; }
>>>>>>> Fix a bunch of warnings

            /// <summary>
            /// The target colour towards which the text colour of a button is blended
            /// if BlendFGColour is set to true and the mouse enters the button area.
            /// </summary>
<<<<<<< refs/remotes/origin/master
            [JsonProperty(Required = Required.Default)]
            public Color FgTargetColour;
=======
            public Color FgTargetColour { get; set; }
>>>>>>> Fix a bunch of warnings
        }
    }
}
