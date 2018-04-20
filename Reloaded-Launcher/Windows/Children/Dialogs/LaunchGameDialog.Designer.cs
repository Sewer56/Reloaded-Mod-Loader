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

namespace ReloadedLauncher.Windows.Children.Dialogs
{
    partial class LaunchGameDialog
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
            System.Drawing.StringFormat stringFormat1 = new System.Drawing.StringFormat();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties2 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage3 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage4 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties3 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage5 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage6 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties4 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage7 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage8 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties5 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage9 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage10 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties6 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage11 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage12 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties7 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage13 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage14 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LaunchGameDialog));
            this.borderless_EnableLogs = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButtonPressIndicator();
            this.item_LaunchBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.item_CloseBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_LogLocation = new Reloaded_GUI.Styles.Controls.Animated.AnimatedTextbox();
            this.titleBar_Title = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.item_AttachBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_AttachExecutableName = new Reloaded_GUI.Styles.Controls.Animated.AnimatedTextbox();
            this.titleBar_Panel = new System.Windows.Forms.Panel();
            this.toolTipHelper = new System.Windows.Forms.ToolTip(this.components);
            this.box_LogLocation = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.box_AttachName = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.box_LogLocationSelect = new System.Windows.Forms.PictureBox();
            this.box_AttachExecutableNameSelect = new System.Windows.Forms.PictureBox();
            this.titleBar_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.box_LogLocationSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.box_AttachExecutableNameSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // borderless_EnableLogs
            // 
            animMessage1.Control = this.borderless_EnableLogs;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.borderless_EnableLogs;
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
            this.borderless_EnableLogs.AnimProperties = animProperties1;
            this.borderless_EnableLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.borderless_EnableLogs.BottomBorderColour = System.Drawing.Color.White;
            this.borderless_EnableLogs.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_EnableLogs.BottomBorderWidth = 1;
            this.borderless_EnableLogs.ButtonEnabled = false;
            this.borderless_EnableLogs.ForeColor = System.Drawing.Color.White;
            this.borderless_EnableLogs.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_EnableLogs.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_EnableLogs.LeftBorderWidth = 1;
            this.borderless_EnableLogs.Location = new System.Drawing.Point(19, 98);
            this.borderless_EnableLogs.Name = "borderless_EnableLogs";
            this.borderless_EnableLogs.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_EnableLogs.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_EnableLogs.RightBorderWidth = 1;
            this.borderless_EnableLogs.Size = new System.Drawing.Size(25, 25);
            stringFormat1.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat1.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_EnableLogs.StringFormat = stringFormat1;
            this.borderless_EnableLogs.TabIndex = 40;
            this.borderless_EnableLogs.Text = "-";
            this.toolTipHelper.SetToolTip(this.borderless_EnableLogs, "Enable to output logs to the specified user location.");
            this.borderless_EnableLogs.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_EnableLogs.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_EnableLogs.TopBorderWidth = 1;
            // 
            // item_LaunchBox
            // 
            animMessage3.Control = this.item_LaunchBox;
            animMessage3.PlayAnimation = true;
            animProperties2.BackColorMessage = animMessage3;
            animMessage4.Control = this.item_LaunchBox;
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
            this.item_LaunchBox.AnimProperties = animProperties2;
            this.item_LaunchBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.item_LaunchBox.CaptureChildren = true;
            this.item_LaunchBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.item_LaunchBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_LaunchBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_LaunchBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_LaunchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.item_LaunchBox.ForeColor = System.Drawing.Color.White;
            this.item_LaunchBox.IgnoreMouse = false;
            this.item_LaunchBox.IgnoreMouseClicks = false;
            this.item_LaunchBox.Location = new System.Drawing.Point(370, 184);
            this.item_LaunchBox.Name = "item_LaunchBox";
            this.item_LaunchBox.Size = new System.Drawing.Size(155, 43);
            this.item_LaunchBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_LaunchBox.TabIndex = 38;
            this.item_LaunchBox.Text = "Launch";
            this.item_LaunchBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.toolTipHelper.SetToolTip(this.item_LaunchBox, "Left click to launch game.\r\nHold CTRL/Control for advanced options.");
            this.item_LaunchBox.UseVisualStyleBackColor = false;
            this.item_LaunchBox.Click += new System.EventHandler(this.item_LaunchBox_Click);
            // 
            // item_CloseBox
            // 
            animMessage5.Control = this.item_CloseBox;
            animMessage5.PlayAnimation = true;
            animProperties3.BackColorMessage = animMessage5;
            animMessage6.Control = this.item_CloseBox;
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
            this.item_CloseBox.AnimProperties = animProperties3;
            this.item_CloseBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.item_CloseBox.CaptureChildren = true;
            this.item_CloseBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.item_CloseBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_CloseBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_CloseBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_CloseBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.item_CloseBox.ForeColor = System.Drawing.Color.White;
            this.item_CloseBox.IgnoreMouse = false;
            this.item_CloseBox.IgnoreMouseClicks = false;
            this.item_CloseBox.Location = new System.Drawing.Point(19, 184);
            this.item_CloseBox.Name = "item_CloseBox";
            this.item_CloseBox.Size = new System.Drawing.Size(155, 43);
            this.item_CloseBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_CloseBox.TabIndex = 37;
            this.item_CloseBox.Text = "Close";
            this.item_CloseBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.item_CloseBox.UseVisualStyleBackColor = false;
            this.item_CloseBox.Click += new System.EventHandler(this.item_CloseBox_Click);
            // 
            // borderless_LogLocation
            // 
            animMessage7.Control = this.borderless_LogLocation;
            animMessage7.PlayAnimation = true;
            animProperties4.BackColorMessage = animMessage7;
            animMessage8.Control = this.borderless_LogLocation;
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
            this.borderless_LogLocation.AnimProperties = animProperties4;
            this.borderless_LogLocation.BackColor = System.Drawing.Color.Gray;
            this.borderless_LogLocation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.borderless_LogLocation.BottomBorderColour = System.Drawing.Color.Empty;
            this.borderless_LogLocation.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_LogLocation.BottomBorderWidth = 55;
            this.borderless_LogLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_LogLocation.LeftBorderColour = System.Drawing.Color.Empty;
            this.borderless_LogLocation.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_LogLocation.LeftBorderWidth = 0;
            this.borderless_LogLocation.Location = new System.Drawing.Point(64, 98);
            this.borderless_LogLocation.Name = "borderless_LogLocation";
            this.borderless_LogLocation.RightBorderColour = System.Drawing.Color.Empty;
            this.borderless_LogLocation.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_LogLocation.RightBorderWidth = 0;
            this.borderless_LogLocation.Size = new System.Drawing.Size(415, 21);
            this.borderless_LogLocation.TabIndex = 35;
            this.borderless_LogLocation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTipHelper.SetToolTip(this.borderless_LogLocation, "Specifies the folder location, subdirectory of Reloaded-Mods directory to be used" +
        " for the current game configuration.");
            this.borderless_LogLocation.TopBorderColour = System.Drawing.Color.Empty;
            this.borderless_LogLocation.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_LogLocation.TopBorderWidth = 0;
            // 
            // titleBar_Title
            // 
            animMessage9.Control = this.titleBar_Title;
            animMessage9.PlayAnimation = true;
            animProperties5.BackColorMessage = animMessage9;
            animMessage10.Control = this.titleBar_Title;
            animMessage10.PlayAnimation = true;
            animProperties5.ForeColorMessage = animMessage10;
            animProperties5.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            animProperties5.MouseEnterDuration = 200F;
            animProperties5.MouseEnterForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(163)))), ((int)(((byte)(244)))));
            animProperties5.MouseEnterFramerate = 144F;
            animProperties5.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties5.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            animProperties5.MouseLeaveDuration = 200F;
            animProperties5.MouseLeaveForeColor = System.Drawing.Color.White;
            animProperties5.MouseLeaveFramerate = 144F;
            animProperties5.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.titleBar_Title.AnimProperties = animProperties5;
            this.titleBar_Title.CaptureChildren = false;
            this.titleBar_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBar_Title.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.titleBar_Title.FlatAppearance.BorderSize = 0;
            this.titleBar_Title.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.titleBar_Title.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.titleBar_Title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titleBar_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.titleBar_Title.ForeColor = System.Drawing.Color.White;
            this.titleBar_Title.IgnoreMouse = false;
            this.titleBar_Title.IgnoreMouseClicks = false;
            this.titleBar_Title.Location = new System.Drawing.Point(0, 0);
            this.titleBar_Title.Name = "titleBar_Title";
            this.titleBar_Title.Size = new System.Drawing.Size(544, 44);
            this.titleBar_Title.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.titleBar_Title.TabIndex = 3;
            this.titleBar_Title.Text = "Advanced Launch Options";
            this.titleBar_Title.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.titleBar_Title.UseVisualStyleBackColor = true;
            // 
            // item_AttachBox
            // 
            animMessage11.Control = this.item_AttachBox;
            animMessage11.PlayAnimation = true;
            animProperties6.BackColorMessage = animMessage11;
            animMessage12.Control = this.item_AttachBox;
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
            this.item_AttachBox.AnimProperties = animProperties6;
            this.item_AttachBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.item_AttachBox.CaptureChildren = true;
            this.item_AttachBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.item_AttachBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_AttachBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_AttachBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_AttachBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.item_AttachBox.ForeColor = System.Drawing.Color.White;
            this.item_AttachBox.IgnoreMouse = false;
            this.item_AttachBox.IgnoreMouseClicks = false;
            this.item_AttachBox.Location = new System.Drawing.Point(194, 184);
            this.item_AttachBox.Name = "item_AttachBox";
            this.item_AttachBox.Size = new System.Drawing.Size(156, 43);
            this.item_AttachBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_AttachBox.TabIndex = 41;
            this.item_AttachBox.Text = "Attach";
            this.item_AttachBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.item_AttachBox.UseVisualStyleBackColor = false;
            this.item_AttachBox.Click += new System.EventHandler(this.item_AttachBox_Click);
            // 
            // borderless_AttachExecutableName
            // 
            animMessage13.Control = this.borderless_AttachExecutableName;
            animMessage13.PlayAnimation = true;
            animProperties7.BackColorMessage = animMessage13;
            animMessage14.Control = this.borderless_AttachExecutableName;
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
            this.borderless_AttachExecutableName.AnimProperties = animProperties7;
            this.borderless_AttachExecutableName.BackColor = System.Drawing.Color.Gray;
            this.borderless_AttachExecutableName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.borderless_AttachExecutableName.BottomBorderColour = System.Drawing.Color.Empty;
            this.borderless_AttachExecutableName.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_AttachExecutableName.BottomBorderWidth = 55;
            this.borderless_AttachExecutableName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_AttachExecutableName.LeftBorderColour = System.Drawing.Color.Empty;
            this.borderless_AttachExecutableName.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_AttachExecutableName.LeftBorderWidth = 0;
            this.borderless_AttachExecutableName.Location = new System.Drawing.Point(271, 139);
            this.borderless_AttachExecutableName.Name = "borderless_AttachExecutableName";
            this.borderless_AttachExecutableName.RightBorderColour = System.Drawing.Color.Empty;
            this.borderless_AttachExecutableName.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_AttachExecutableName.RightBorderWidth = 0;
            this.borderless_AttachExecutableName.Size = new System.Drawing.Size(208, 21);
            this.borderless_AttachExecutableName.TabIndex = 42;
            this.borderless_AttachExecutableName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTipHelper.SetToolTip(this.borderless_AttachExecutableName, "Specifies the folder location, subdirectory of Reloaded-Mods directory to be used" +
        " for the current game configuration.");
            this.borderless_AttachExecutableName.TopBorderColour = System.Drawing.Color.Empty;
            this.borderless_AttachExecutableName.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_AttachExecutableName.TopBorderWidth = 0;
            // 
            // titleBar_Panel
            // 
            this.titleBar_Panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.titleBar_Panel.Controls.Add(this.titleBar_Title);
            this.titleBar_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar_Panel.Location = new System.Drawing.Point(0, 0);
            this.titleBar_Panel.Margin = new System.Windows.Forms.Padding(0);
            this.titleBar_Panel.Name = "titleBar_Panel";
            this.titleBar_Panel.Size = new System.Drawing.Size(544, 44);
            this.titleBar_Panel.TabIndex = 0;
            // 
            // box_LogLocation
            // 
            this.box_LogLocation.AutoSize = true;
            this.box_LogLocation.BackColor = System.Drawing.Color.Transparent;
            this.box_LogLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.box_LogLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.box_LogLocation.ForeColor = System.Drawing.Color.White;
            this.box_LogLocation.IgnoreMouse = false;
            this.box_LogLocation.Location = new System.Drawing.Point(15, 59);
            this.box_LogLocation.Margin = new System.Windows.Forms.Padding(0);
            this.box_LogLocation.Name = "box_LogLocation";
            this.box_LogLocation.Size = new System.Drawing.Size(170, 22);
            this.box_LogLocation.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.box_LogLocation.TabIndex = 39;
            this.box_LogLocation.Text = "Log File Location:";
            this.box_LogLocation.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.toolTipHelper.SetToolTip(this.box_LogLocation, "Specifies the name of the game as seen in Reloaded Launcher");
            // 
            // box_AttachName
            // 
            this.box_AttachName.AutoSize = true;
            this.box_AttachName.BackColor = System.Drawing.Color.Transparent;
            this.box_AttachName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.box_AttachName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.box_AttachName.ForeColor = System.Drawing.Color.White;
            this.box_AttachName.IgnoreMouse = false;
            this.box_AttachName.Location = new System.Drawing.Point(16, 140);
            this.box_AttachName.Margin = new System.Windows.Forms.Padding(0);
            this.box_AttachName.Name = "box_AttachName";
            this.box_AttachName.Size = new System.Drawing.Size(235, 22);
            this.box_AttachName.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.box_AttachName.TabIndex = 45;
            this.box_AttachName.Text = "Attach Executable Name:";
            this.box_AttachName.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.toolTipHelper.SetToolTip(this.box_AttachName, "Specifies the name of the game as seen in Reloaded Launcher");
            // 
            // box_LogLocationSelect
            // 
            this.box_LogLocationSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.box_LogLocationSelect.Location = new System.Drawing.Point(499, 98);
            this.box_LogLocationSelect.Name = "box_LogLocationSelect";
            this.box_LogLocationSelect.Size = new System.Drawing.Size(25, 25);
            this.box_LogLocationSelect.TabIndex = 36;
            this.box_LogLocationSelect.TabStop = false;
            this.box_LogLocationSelect.Click += new System.EventHandler(this.box_LogLocationSelect_Click);
            // 
            // box_AttachExecutableNameSelect
            // 
            this.box_AttachExecutableNameSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.box_AttachExecutableNameSelect.Location = new System.Drawing.Point(499, 139);
            this.box_AttachExecutableNameSelect.Name = "box_AttachExecutableNameSelect";
            this.box_AttachExecutableNameSelect.Size = new System.Drawing.Size(25, 25);
            this.box_AttachExecutableNameSelect.TabIndex = 43;
            this.box_AttachExecutableNameSelect.TabStop = false;
            this.box_AttachExecutableNameSelect.Click += new System.EventHandler(this.box_AttachExecutableNameSelect_Click);
            // 
            // LaunchGameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(544, 250);
            this.Controls.Add(this.box_AttachName);
            this.Controls.Add(this.box_AttachExecutableNameSelect);
            this.Controls.Add(this.borderless_AttachExecutableName);
            this.Controls.Add(this.item_AttachBox);
            this.Controls.Add(this.borderless_EnableLogs);
            this.Controls.Add(this.box_LogLocation);
            this.Controls.Add(this.item_LaunchBox);
            this.Controls.Add(this.item_CloseBox);
            this.Controls.Add(this.box_LogLocationSelect);
            this.Controls.Add(this.borderless_LogLocation);
            this.Controls.Add(this.titleBar_Panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LaunchGameDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doki Doki Modification Club!";
            this.Load += new System.EventHandler(this.Base_Load);
            this.titleBar_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.box_LogLocationSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.box_AttachExecutableNameSelect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton titleBar_Title;
        public System.Windows.Forms.Panel titleBar_Panel;
        private System.Windows.Forms.ToolTip toolTipHelper;
        public System.Windows.Forms.PictureBox box_LogLocationSelect;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedTextbox borderless_LogLocation;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton item_LaunchBox;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton item_CloseBox;
        private Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel box_LogLocation;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButtonPressIndicator borderless_EnableLogs;
        private AnimatedButton item_AttachBox;
        public System.Windows.Forms.PictureBox box_AttachExecutableNameSelect;
        private AnimatedTextbox borderless_AttachExecutableName;
        private Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel box_AttachName;
    }
}

