/*
    [Reloaded] Mod Loader Launcher
    The launcher for a universal, powerful, multi-game and multi-process mod loader
    based off of the concept of DLL Injection to execute arbitrary program code.
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

using Reloaded_GUI.Styles.Controls.Animated;
using Reloaded_GUI.Styles.Controls.Enhanced;

namespace ReloadedLauncher.Windows.Children
{
    partial class AboutScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties1 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage1 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage2 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties2 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage3 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage4 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            System.Drawing.StringFormat stringFormat1 = new System.Drawing.StringFormat();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties3 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage5 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage6 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            System.Drawing.StringFormat stringFormat2 = new System.Drawing.StringFormat();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties4 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage7 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage8 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties5 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage9 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage10 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            System.Drawing.StringFormat stringFormat3 = new System.Drawing.StringFormat();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties6 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage11 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage12 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            System.Drawing.StringFormat stringFormat4 = new System.Drawing.StringFormat();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutScreen));
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties7 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage13 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage14 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            this.borderless_Author = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_AllowPreReleasesBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButtonPressIndicator();
            this.borderless_CloseOnLaunchBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButtonPressIndicator();
            this.borderless_GPLV3 = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_SilentUpdatesBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButtonPressIndicator();
            this.borderless_AutoUpdatesBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButtonPressIndicator();
            this.box_ReloadedLogo = new System.Windows.Forms.PictureBox();
            this.toolTipHelper = new System.Windows.Forms.ToolTip(this.components);
            this.borderless_CloseOnLaunch = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.borderless_AllowPreReleases = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.borderless_SilentUpdates = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.borderless_AutoUpdates = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.borderless_Documentation = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            ((System.ComponentModel.ISupportInitialize)(this.box_ReloadedLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // borderless_Author
            // 
            animMessage1.Control = this.borderless_Author;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.borderless_Author;
            animMessage2.PlayAnimation = true;
            animProperties1.ForeColorMessage = animMessage2;
            animProperties1.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties1.MouseEnterDuration = 0F;
            animProperties1.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties1.MouseEnterFramerate = 0F;
            animProperties1.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties1.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties1.MouseLeaveDuration = 0F;
            animProperties1.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties1.MouseLeaveFramerate = 0F;
            animProperties1.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_Author.AnimProperties = animProperties1;
            this.borderless_Author.BackColor = System.Drawing.Color.Transparent;
            this.borderless_Author.CaptureChildren = true;
            this.borderless_Author.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_Author.FlatAppearance.BorderSize = 0;
            this.borderless_Author.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_Author.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_Author.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_Author.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_Author.ForeColor = System.Drawing.Color.White;
            this.borderless_Author.IgnoreMouse = false;
            this.borderless_Author.IgnoreMouseClicks = false;
            this.borderless_Author.Location = new System.Drawing.Point(166, 287);
            this.borderless_Author.Name = "borderless_Author";
            this.borderless_Author.Size = new System.Drawing.Size(568, 30);
            this.borderless_Author.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_Author.TabIndex = 49;
            this.borderless_Author.Text = "Written by Sewer56 ~ (C) 2018 | Compiled on 01/02/2018 20:57";
            this.borderless_Author.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.borderless_Author.UseVisualStyleBackColor = false;
            // 
            // borderless_AllowPreReleasesBox
            // 
            animMessage3.Control = this.borderless_AllowPreReleasesBox;
            animMessage3.PlayAnimation = true;
            animProperties2.BackColorMessage = animMessage3;
            animMessage4.Control = this.borderless_AllowPreReleasesBox;
            animMessage4.PlayAnimation = true;
            animProperties2.ForeColorMessage = animMessage4;
            animProperties2.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties2.MouseEnterDuration = 0F;
            animProperties2.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties2.MouseEnterFramerate = 0F;
            animProperties2.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties2.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties2.MouseLeaveDuration = 0F;
            animProperties2.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties2.MouseLeaveFramerate = 0F;
            animProperties2.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_AllowPreReleasesBox.AnimProperties = animProperties2;
            this.borderless_AllowPreReleasesBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.borderless_AllowPreReleasesBox.BottomBorderColour = System.Drawing.Color.White;
            this.borderless_AllowPreReleasesBox.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_AllowPreReleasesBox.BottomBorderWidth = 1;
            this.borderless_AllowPreReleasesBox.ButtonEnabled = false;
            this.borderless_AllowPreReleasesBox.ForeColor = System.Drawing.Color.White;
            this.borderless_AllowPreReleasesBox.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_AllowPreReleasesBox.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_AllowPreReleasesBox.LeftBorderWidth = 1;
            this.borderless_AllowPreReleasesBox.Location = new System.Drawing.Point(479, 405);
            this.borderless_AllowPreReleasesBox.Name = "borderless_AllowPreReleasesBox";
            this.borderless_AllowPreReleasesBox.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_AllowPreReleasesBox.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_AllowPreReleasesBox.RightBorderWidth = 1;
            this.borderless_AllowPreReleasesBox.Size = new System.Drawing.Size(25, 25);
            stringFormat1.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat1.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_AllowPreReleasesBox.StringFormat = stringFormat1;
            this.borderless_AllowPreReleasesBox.TabIndex = 48;
            this.borderless_AllowPreReleasesBox.Text = "-";
            this.toolTipHelper.SetToolTip(this.borderless_AllowPreReleasesBox, "Allows for downloading of early builds tagged as Pre-release on Github.\r\nPre-rele" +
        "ase builds will likely contain new features, default mod updates or internal imp" +
        "rovements but may introduce bugs.\r\n");
            this.borderless_AllowPreReleasesBox.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_AllowPreReleasesBox.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_AllowPreReleasesBox.TopBorderWidth = 1;
            // 
            // borderless_CloseOnLaunchBox
            // 
            animMessage5.Control = this.borderless_CloseOnLaunchBox;
            animMessage5.PlayAnimation = true;
            animProperties3.BackColorMessage = animMessage5;
            animMessage6.Control = this.borderless_CloseOnLaunchBox;
            animMessage6.PlayAnimation = true;
            animProperties3.ForeColorMessage = animMessage6;
            animProperties3.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties3.MouseEnterDuration = 0F;
            animProperties3.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties3.MouseEnterFramerate = 0F;
            animProperties3.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties3.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties3.MouseLeaveDuration = 0F;
            animProperties3.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties3.MouseLeaveFramerate = 0F;
            animProperties3.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_CloseOnLaunchBox.AnimProperties = animProperties3;
            this.borderless_CloseOnLaunchBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.borderless_CloseOnLaunchBox.BottomBorderColour = System.Drawing.Color.White;
            this.borderless_CloseOnLaunchBox.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_CloseOnLaunchBox.BottomBorderWidth = 1;
            this.borderless_CloseOnLaunchBox.ButtonEnabled = false;
            this.borderless_CloseOnLaunchBox.ForeColor = System.Drawing.Color.White;
            this.borderless_CloseOnLaunchBox.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_CloseOnLaunchBox.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_CloseOnLaunchBox.LeftBorderWidth = 1;
            this.borderless_CloseOnLaunchBox.Location = new System.Drawing.Point(479, 455);
            this.borderless_CloseOnLaunchBox.Name = "borderless_CloseOnLaunchBox";
            this.borderless_CloseOnLaunchBox.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_CloseOnLaunchBox.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_CloseOnLaunchBox.RightBorderWidth = 1;
            this.borderless_CloseOnLaunchBox.Size = new System.Drawing.Size(25, 25);
            stringFormat2.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat2.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat2.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat2.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_CloseOnLaunchBox.StringFormat = stringFormat2;
            this.borderless_CloseOnLaunchBox.TabIndex = 47;
            this.borderless_CloseOnLaunchBox.Text = "-";
            this.toolTipHelper.SetToolTip(this.borderless_CloseOnLaunchBox, "Does not close Reloaded-Launcher when a game is started.\r\nUseful for some develop" +
        "ers.");
            this.borderless_CloseOnLaunchBox.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_CloseOnLaunchBox.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_CloseOnLaunchBox.TopBorderWidth = 1;
            // 
            // borderless_GPLV3
            // 
            animMessage7.Control = this.borderless_GPLV3;
            animMessage7.PlayAnimation = true;
            animProperties4.BackColorMessage = animMessage7;
            animMessage8.Control = this.borderless_GPLV3;
            animMessage8.PlayAnimation = true;
            animProperties4.ForeColorMessage = animMessage8;
            animProperties4.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties4.MouseEnterDuration = 0F;
            animProperties4.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties4.MouseEnterFramerate = 0F;
            animProperties4.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties4.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties4.MouseLeaveDuration = 0F;
            animProperties4.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties4.MouseLeaveFramerate = 0F;
            animProperties4.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_GPLV3.AnimProperties = animProperties4;
            this.borderless_GPLV3.BackColor = System.Drawing.Color.Transparent;
            this.borderless_GPLV3.CaptureChildren = true;
            this.borderless_GPLV3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_GPLV3.FlatAppearance.BorderSize = 0;
            this.borderless_GPLV3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_GPLV3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_GPLV3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_GPLV3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_GPLV3.ForeColor = System.Drawing.Color.White;
            this.borderless_GPLV3.IgnoreMouse = false;
            this.borderless_GPLV3.IgnoreMouseClicks = false;
            this.borderless_GPLV3.Location = new System.Drawing.Point(166, 323);
            this.borderless_GPLV3.Name = "borderless_GPLV3";
            this.borderless_GPLV3.Size = new System.Drawing.Size(282, 73);
            this.borderless_GPLV3.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_GPLV3.TabIndex = 46;
            this.borderless_GPLV3.Text = "Reloaded is Free Software\r\nLicensed under GNU GPL V3\r\n";
            this.borderless_GPLV3.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.borderless_GPLV3.UseVisualStyleBackColor = false;
            // 
            // borderless_SilentUpdatesBox
            // 
            animMessage9.Control = this.borderless_SilentUpdatesBox;
            animMessage9.PlayAnimation = true;
            animProperties5.BackColorMessage = animMessage9;
            animMessage10.Control = this.borderless_SilentUpdatesBox;
            animMessage10.PlayAnimation = true;
            animProperties5.ForeColorMessage = animMessage10;
            animProperties5.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties5.MouseEnterDuration = 0F;
            animProperties5.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties5.MouseEnterFramerate = 0F;
            animProperties5.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties5.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties5.MouseLeaveDuration = 0F;
            animProperties5.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties5.MouseLeaveFramerate = 0F;
            animProperties5.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_SilentUpdatesBox.AnimProperties = animProperties5;
            this.borderless_SilentUpdatesBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.borderless_SilentUpdatesBox.BottomBorderColour = System.Drawing.Color.White;
            this.borderless_SilentUpdatesBox.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_SilentUpdatesBox.BottomBorderWidth = 1;
            this.borderless_SilentUpdatesBox.ButtonEnabled = false;
            this.borderless_SilentUpdatesBox.ForeColor = System.Drawing.Color.White;
            this.borderless_SilentUpdatesBox.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_SilentUpdatesBox.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_SilentUpdatesBox.LeftBorderWidth = 1;
            this.borderless_SilentUpdatesBox.Location = new System.Drawing.Point(166, 455);
            this.borderless_SilentUpdatesBox.Name = "borderless_SilentUpdatesBox";
            this.borderless_SilentUpdatesBox.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_SilentUpdatesBox.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_SilentUpdatesBox.RightBorderWidth = 1;
            this.borderless_SilentUpdatesBox.Size = new System.Drawing.Size(25, 25);
            stringFormat3.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat3.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat3.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat3.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_SilentUpdatesBox.StringFormat = stringFormat3;
            this.borderless_SilentUpdatesBox.TabIndex = 43;
            this.borderless_SilentUpdatesBox.Text = "-";
            this.toolTipHelper.SetToolTip(this.borderless_SilentUpdatesBox, "Updates silently and transparently in Google Chrome style.\r\nIf an update is downl" +
        "oaded, it will be applied the next time you start up Reloaded.");
            this.borderless_SilentUpdatesBox.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_SilentUpdatesBox.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_SilentUpdatesBox.TopBorderWidth = 1;
            // 
            // borderless_AutoUpdatesBox
            // 
            animMessage11.Control = this.borderless_AutoUpdatesBox;
            animMessage11.PlayAnimation = true;
            animProperties6.BackColorMessage = animMessage11;
            animMessage12.Control = this.borderless_AutoUpdatesBox;
            animMessage12.PlayAnimation = true;
            animProperties6.ForeColorMessage = animMessage12;
            animProperties6.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties6.MouseEnterDuration = 0F;
            animProperties6.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties6.MouseEnterFramerate = 0F;
            animProperties6.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties6.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties6.MouseLeaveDuration = 0F;
            animProperties6.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties6.MouseLeaveFramerate = 0F;
            animProperties6.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_AutoUpdatesBox.AnimProperties = animProperties6;
            this.borderless_AutoUpdatesBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.borderless_AutoUpdatesBox.BottomBorderColour = System.Drawing.Color.White;
            this.borderless_AutoUpdatesBox.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_AutoUpdatesBox.BottomBorderWidth = 1;
            this.borderless_AutoUpdatesBox.ButtonEnabled = false;
            this.borderless_AutoUpdatesBox.ForeColor = System.Drawing.Color.White;
            this.borderless_AutoUpdatesBox.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_AutoUpdatesBox.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_AutoUpdatesBox.LeftBorderWidth = 1;
            this.borderless_AutoUpdatesBox.Location = new System.Drawing.Point(166, 405);
            this.borderless_AutoUpdatesBox.Name = "borderless_AutoUpdatesBox";
            this.borderless_AutoUpdatesBox.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_AutoUpdatesBox.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_AutoUpdatesBox.RightBorderWidth = 1;
            this.borderless_AutoUpdatesBox.Size = new System.Drawing.Size(25, 25);
            stringFormat4.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat4.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat4.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat4.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_AutoUpdatesBox.StringFormat = stringFormat4;
            this.borderless_AutoUpdatesBox.TabIndex = 41;
            this.borderless_AutoUpdatesBox.Text = "-";
            this.toolTipHelper.SetToolTip(this.borderless_AutoUpdatesBox, "Automatically starts the update process if Silent Updates are not enabled.\r\nHas n" +
        "o effect if Silent Updates are enabled.");
            this.borderless_AutoUpdatesBox.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_AutoUpdatesBox.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_AutoUpdatesBox.TopBorderWidth = 1;
            // 
            // box_ReloadedLogo
            // 
            this.box_ReloadedLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("box_ReloadedLogo.BackgroundImage")));
            this.box_ReloadedLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.box_ReloadedLogo.Location = new System.Drawing.Point(106, 24);
            this.box_ReloadedLogo.Name = "box_ReloadedLogo";
            this.box_ReloadedLogo.Size = new System.Drawing.Size(688, 223);
            this.box_ReloadedLogo.TabIndex = 0;
            this.box_ReloadedLogo.TabStop = false;
            // 
            // borderless_CloseOnLaunch
            // 
            this.borderless_CloseOnLaunch.BackColor = System.Drawing.Color.Transparent;
            this.borderless_CloseOnLaunch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_CloseOnLaunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.borderless_CloseOnLaunch.ForeColor = System.Drawing.Color.White;
            this.borderless_CloseOnLaunch.IgnoreMouse = false;
            this.borderless_CloseOnLaunch.Location = new System.Drawing.Point(518, 455);
            this.borderless_CloseOnLaunch.Margin = new System.Windows.Forms.Padding(0);
            this.borderless_CloseOnLaunch.Name = "borderless_CloseOnLaunch";
            this.borderless_CloseOnLaunch.Size = new System.Drawing.Size(290, 24);
            this.borderless_CloseOnLaunch.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_CloseOnLaunch.TabIndex = 51;
            this.borderless_CloseOnLaunch.Text = "Close on Game Launch";
            this.borderless_CloseOnLaunch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_CloseOnLaunch.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.toolTipHelper.SetToolTip(this.borderless_CloseOnLaunch, "Does not close Reloaded-Launcher when a game is started.\r\nUseful for some develop" +
        "ers.");
            // 
            // borderless_AllowPreReleases
            // 
            this.borderless_AllowPreReleases.BackColor = System.Drawing.Color.Transparent;
            this.borderless_AllowPreReleases.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_AllowPreReleases.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.borderless_AllowPreReleases.ForeColor = System.Drawing.Color.White;
            this.borderless_AllowPreReleases.IgnoreMouse = false;
            this.borderless_AllowPreReleases.Location = new System.Drawing.Point(518, 405);
            this.borderless_AllowPreReleases.Margin = new System.Windows.Forms.Padding(0);
            this.borderless_AllowPreReleases.Name = "borderless_AllowPreReleases";
            this.borderless_AllowPreReleases.Size = new System.Drawing.Size(290, 24);
            this.borderless_AllowPreReleases.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_AllowPreReleases.TabIndex = 50;
            this.borderless_AllowPreReleases.Text = "Get Prerelease/Beta Builds";
            this.borderless_AllowPreReleases.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_AllowPreReleases.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.toolTipHelper.SetToolTip(this.borderless_AllowPreReleases, "Allows for downloading of early builds tagged as Pre-release on Github.\r\nPre-rele" +
        "ase builds will likely contain new features, default mod updates or internal imp" +
        "rovements but may introduce bugs.");
            // 
            // borderless_SilentUpdates
            // 
            this.borderless_SilentUpdates.BackColor = System.Drawing.Color.Transparent;
            this.borderless_SilentUpdates.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_SilentUpdates.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.borderless_SilentUpdates.ForeColor = System.Drawing.Color.White;
            this.borderless_SilentUpdates.IgnoreMouse = false;
            this.borderless_SilentUpdates.Location = new System.Drawing.Point(205, 455);
            this.borderless_SilentUpdates.Margin = new System.Windows.Forms.Padding(0);
            this.borderless_SilentUpdates.Name = "borderless_SilentUpdates";
            this.borderless_SilentUpdates.Size = new System.Drawing.Size(256, 24);
            this.borderless_SilentUpdates.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_SilentUpdates.TabIndex = 44;
            this.borderless_SilentUpdates.Text = "Enable Silent Updates";
            this.borderless_SilentUpdates.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_SilentUpdates.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.toolTipHelper.SetToolTip(this.borderless_SilentUpdates, "Updates silently and transparently in Google Chrome style.\r\nIf an update is downl" +
        "oaded, it will be applied the next time you start up Reloaded.");
            // 
            // borderless_AutoUpdates
            // 
            this.borderless_AutoUpdates.BackColor = System.Drawing.Color.Transparent;
            this.borderless_AutoUpdates.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_AutoUpdates.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.borderless_AutoUpdates.ForeColor = System.Drawing.Color.White;
            this.borderless_AutoUpdates.IgnoreMouse = false;
            this.borderless_AutoUpdates.Location = new System.Drawing.Point(205, 405);
            this.borderless_AutoUpdates.Margin = new System.Windows.Forms.Padding(0);
            this.borderless_AutoUpdates.Name = "borderless_AutoUpdates";
            this.borderless_AutoUpdates.Size = new System.Drawing.Size(256, 24);
            this.borderless_AutoUpdates.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_AutoUpdates.TabIndex = 42;
            this.borderless_AutoUpdates.Text = "Enable Automatic Updates";
            this.borderless_AutoUpdates.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_AutoUpdates.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.toolTipHelper.SetToolTip(this.borderless_AutoUpdates, "Automatically starts the update process if Silent Updates are not enabled.\r\nHas n" +
        "o effect if Silent Updates are enabled.");
            // 
            // borderless_Documentation
            // 
            animMessage13.Control = this.borderless_Documentation;
            animMessage13.PlayAnimation = true;
            animProperties7.BackColorMessage = animMessage13;
            animMessage14.Control = this.borderless_Documentation;
            animMessage14.PlayAnimation = true;
            animProperties7.ForeColorMessage = animMessage14;
            animProperties7.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties7.MouseEnterDuration = 0F;
            animProperties7.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties7.MouseEnterFramerate = 0F;
            animProperties7.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties7.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties7.MouseLeaveDuration = 0F;
            animProperties7.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties7.MouseLeaveFramerate = 0F;
            animProperties7.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_Documentation.AnimProperties = animProperties7;
            this.borderless_Documentation.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("borderless_Documentation.BackgroundImage")));
            this.borderless_Documentation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.borderless_Documentation.CaptureChildren = false;
            this.borderless_Documentation.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_Documentation.FlatAppearance.BorderSize = 0;
            this.borderless_Documentation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_Documentation.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_Documentation.ForeColor = System.Drawing.Color.White;
            this.borderless_Documentation.IgnoreMouse = false;
            this.borderless_Documentation.IgnoreMouseClicks = false;
            this.borderless_Documentation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_Documentation.Location = new System.Drawing.Point(479, 339);
            this.borderless_Documentation.Name = "borderless_Documentation";
            this.borderless_Documentation.Size = new System.Drawing.Size(270, 45);
            this.borderless_Documentation.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_Documentation.TabIndex = 58;
            this.borderless_Documentation.Text = "User Guide + Docs";
            this.borderless_Documentation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.borderless_Documentation.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.borderless_Documentation.UseVisualStyleBackColor = true;
            this.borderless_Documentation.Click += new System.EventHandler(this.borderless_Documentation_Click);
            // 
            // AboutScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(900, 512);
            this.Controls.Add(this.borderless_Documentation);
            this.Controls.Add(this.borderless_CloseOnLaunch);
            this.Controls.Add(this.borderless_AllowPreReleases);
            this.Controls.Add(this.borderless_Author);
            this.Controls.Add(this.borderless_AllowPreReleasesBox);
            this.Controls.Add(this.borderless_CloseOnLaunchBox);
            this.Controls.Add(this.borderless_GPLV3);
            this.Controls.Add(this.borderless_SilentUpdates);
            this.Controls.Add(this.borderless_SilentUpdatesBox);
            this.Controls.Add(this.borderless_AutoUpdates);
            this.Controls.Add(this.borderless_AutoUpdatesBox);
            this.Controls.Add(this.box_ReloadedLogo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AboutScreen";
            this.Text = "Main_Screen";
            this.VisibleChanged += new System.EventHandler(this.MenuVisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.box_ReloadedLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox box_ReloadedLogo;
        private System.Windows.Forms.ToolTip toolTipHelper;
        private AnimatedButtonPressIndicator borderless_AutoUpdatesBox;
        private EnhancedLabel borderless_AutoUpdates;
        private AnimatedButtonPressIndicator borderless_SilentUpdatesBox;
        private EnhancedLabel borderless_SilentUpdates;
        private AnimatedButton borderless_GPLV3;
        private AnimatedButtonPressIndicator borderless_CloseOnLaunchBox;
        private AnimatedButtonPressIndicator borderless_AllowPreReleasesBox;
        private AnimatedButton borderless_Author;
        private EnhancedLabel borderless_AllowPreReleases;
        private EnhancedLabel borderless_CloseOnLaunch;
        public AnimatedButton borderless_Documentation;
    }
}