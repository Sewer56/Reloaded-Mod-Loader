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
using Reloaded.Paths;

namespace Reloaded.IO.Config
{
    public static class ThemeProperties
    {
        /// <summary>
        /// Stores all of the general theme colours and properties in question.
        /// </summary>
        public struct Theme
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
            [JsonProperty(Required = Required.Default)]
            public BorderProperties BorderProperties;

            /// <summary>
            /// Specifies the main form colours.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public BarColours MainColours;

            /// <summary>
            /// Specifies the title bar colours.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public BarColours TitleColours;

            /// <summary>
            /// Specifies the category bar colours.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public BarColours CategoryColours;

            /// <summary>
            /// Specifies the category bar colours.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public BarColours BorderlessColours;

            /// <summary>
            /// Specifies the colours for the WinForm buttons used as decorations that serve no functionality
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public BarColours BoxColours;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the category bar.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation CategoryEnterAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the category bar.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation CategoryLeaveAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the title bar.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation TitleEnterAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items on the title bar.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation TitleLeaveAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the items in the main forms.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation MainEnterAnimation;

            /// <summary>
            /// Specifies the mouse leave animation behaviour for the items in the main forms.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation MainLeaveAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation BoxEnterAnimation;

            /// <summary>
            /// Specifies the mouse leave animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation BoxLeaveAnimation;

            /// <summary>
            /// Specifies the mouse enter animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation BorderlessEnterAnimation;

            /// <summary>
            /// Specifies the mouse leave animation behaviour for the WinForm buttons used for decorations that serve no functionality.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public ButtonMouseAnimation BorderlessLeaveAnimation;

            /// <summary>
            /// [DO NOT MODIFY] Specifies the location of this theme property configuration file.
            /// </summary>
            [JsonIgnore]
            public string ThemePropertyLocation;

            /// <summary>
            /// Retrieves the Mod Loader Colour Configuration File.
            /// </summary>
            /// <param name="themeDirectory">The relative directory of the individual theme to Reloaded-Config/Themes. e.g. Default</param>
            public static Theme ParseConfig(string themeDirectory)
            {
                // Specifies the location of the current theme property file.
                string themeLocation = LoaderPaths.GetModLoaderThemeDirectory() + "/" + themeDirectory + $"/{Strings.Parsers.ThemeFile}";

                // Try parsing the config file, else return default one.
                Theme theme;
                try
                {
                    theme = File.Exists(themeLocation)
                        ? JsonConvert.DeserializeObject<Theme>(File.ReadAllText(themeLocation))
                        : Theme.GetDefaultThemeConfig();
                }
                catch { theme = Theme.GetDefaultThemeConfig(); }

                theme.ThemePropertyLocation = themeLocation;
                return theme;
            }

            /// <summary>
            /// Writes out the config file to disk.
            /// </summary>
            /// <param name="theme">The mod configuration structure defining the details of the individual mod.</param>
            public static void WriteConfig(Theme theme)
            {
                // Convert structure to JSON
                string json = JsonConvert.SerializeObject(theme, Formatting.Indented);

                // Write to disk
                File.WriteAllText(theme.ThemePropertyLocation, json);
            }

            /// <summary>
            /// Retrieves the default theme configuration.
            /// </summary>
            /// <returns></returns>
            public static Theme GetDefaultThemeConfig()
            {
                return new Theme()
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
            }
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
            [JsonProperty(Required = Required.Default)]
            public string LoaderTitle;

            /// <summary>
            /// Sets the character that is shown between the loader title
            /// and the current menu that the user is in.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string LoaderTitleDelimiter;

            /// <summary>
            /// Sets the whether the title delimiter and menu name is
            /// placed before or after the actual LoaderTitle field.
            /// false sets it after, true places it before.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public bool LoaderTitlePrefix;
        }

        /// <summary>
        /// Defines the border styles used for buttons.
        /// </summary>
        public struct BorderProperties
        {
            /// <summary>
            /// Defines the background colour of the category/title bar.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public Color BorderColour;

            /// <summary>
            /// The colour of the category/title bar buttons with options such as e.g. Games, Mods
            /// </summary>
            [JsonProperty(Required = Required.Default)]
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
            [JsonProperty(Required = Required.Default)]
            public Color BgColour;

            /// <summary>
            /// The colour of the category/title bar buttons with options such as e.g. Games, Mods
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public Color ButtonBgColour;

            /// <summary>
            /// The colour of the category/title bar text. e.g. Games, Mods
            /// </summary>
            [JsonProperty(Required = Required.Default)]
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
            [JsonProperty(Required = Required.Default)]
            public bool BlendBgColour;

            /// <summary>
            /// Defines whether the forecolor (text colour) of a category bar button should be
            /// blended to a target colour when the mouse enters the control.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public bool BlendFgColour;

            /// <summary>
            /// The duration/length of the mouse enter event, specifies the duration of the
            /// background and/or foreground blend effects.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public int AnimationDuration;

            /// <summary>
            /// The framerate at which the mouse enter animation to be performed.
            /// It does not affect the length of the animation.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public int AnimationFramerate;

            /// <summary>
            /// The target colour towards which the backcolor of a button is blended
            /// if BlendBGColour is set to true and the mouse enters the button area.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public Color BgTargetColour;

            /// <summary>
            /// The target colour towards which the text colour of a button is blended
            /// if BlendFGColour is set to true and the mouse enters the button area.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public Color FgTargetColour;
        }
    }
}
