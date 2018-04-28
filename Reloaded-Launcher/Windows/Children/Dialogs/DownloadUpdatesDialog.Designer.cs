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

namespace ReloadedLauncher.Windows.Children.Dialogs
{
    partial class DownloadUpdatesDialog
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
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties2 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage3 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage4 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties8 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage15 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage16 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties1 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage1 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage2 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            this.titleBar_Panel = new System.Windows.Forms.Panel();
            this.toolTipHelper = new System.Windows.Forms.ToolTip(this.components);
            this.borderless_OldVersionNumber = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_NewVersionNumber = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_NewVersion = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_OldVersion = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.item_Changelog = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_UpdateProgressBar = new Reloaded_GUI.Styles.Controls.Custom.HorizontalProgressBar();
            this.titleBar_Title = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.item_Update = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.categoryBar_Close = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.titleBar_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleBar_Panel
            // 
            this.titleBar_Panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.titleBar_Panel.Controls.Add(this.categoryBar_Close);
            this.titleBar_Panel.Controls.Add(this.titleBar_Title);
            this.titleBar_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar_Panel.Location = new System.Drawing.Point(0, 0);
            this.titleBar_Panel.Margin = new System.Windows.Forms.Padding(0);
            this.titleBar_Panel.Name = "titleBar_Panel";
            this.titleBar_Panel.Size = new System.Drawing.Size(374, 44);
            this.titleBar_Panel.TabIndex = 0;
            // 
            // borderless_OldVersionNumber
            // 
            animMessage5.Control = this.borderless_OldVersionNumber;
            animMessage5.PlayAnimation = true;
            animProperties3.BackColorMessage = animMessage5;
            animMessage6.Control = this.borderless_OldVersionNumber;
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
            this.borderless_OldVersionNumber.AnimProperties = animProperties3;
            this.borderless_OldVersionNumber.BackColor = System.Drawing.Color.Transparent;
            this.borderless_OldVersionNumber.CaptureChildren = true;
            this.borderless_OldVersionNumber.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_OldVersionNumber.FlatAppearance.BorderSize = 0;
            this.borderless_OldVersionNumber.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_OldVersionNumber.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_OldVersionNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_OldVersionNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_OldVersionNumber.ForeColor = System.Drawing.Color.White;
            this.borderless_OldVersionNumber.IgnoreMouse = false;
            this.borderless_OldVersionNumber.IgnoreMouseClicks = false;
            this.borderless_OldVersionNumber.Location = new System.Drawing.Point(195, 94);
            this.borderless_OldVersionNumber.Name = "borderless_OldVersionNumber";
            this.borderless_OldVersionNumber.Size = new System.Drawing.Size(141, 27);
            this.borderless_OldVersionNumber.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_OldVersionNumber.TabIndex = 50;
            this.borderless_OldVersionNumber.Text = "TEMP";
            this.borderless_OldVersionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.borderless_OldVersionNumber.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.borderless_OldVersionNumber.UseVisualStyleBackColor = false;
            // 
            // borderless_NewVersionNumber
            // 
            animMessage7.Control = this.borderless_NewVersionNumber;
            animMessage7.PlayAnimation = true;
            animProperties4.BackColorMessage = animMessage7;
            animMessage8.Control = this.borderless_NewVersionNumber;
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
            this.borderless_NewVersionNumber.AnimProperties = animProperties4;
            this.borderless_NewVersionNumber.BackColor = System.Drawing.Color.Transparent;
            this.borderless_NewVersionNumber.CaptureChildren = true;
            this.borderless_NewVersionNumber.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_NewVersionNumber.FlatAppearance.BorderSize = 0;
            this.borderless_NewVersionNumber.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_NewVersionNumber.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_NewVersionNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_NewVersionNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_NewVersionNumber.ForeColor = System.Drawing.Color.White;
            this.borderless_NewVersionNumber.IgnoreMouse = false;
            this.borderless_NewVersionNumber.IgnoreMouseClicks = false;
            this.borderless_NewVersionNumber.Location = new System.Drawing.Point(195, 121);
            this.borderless_NewVersionNumber.Name = "borderless_NewVersionNumber";
            this.borderless_NewVersionNumber.Size = new System.Drawing.Size(141, 27);
            this.borderless_NewVersionNumber.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_NewVersionNumber.TabIndex = 49;
            this.borderless_NewVersionNumber.Text = "TEMP";
            this.borderless_NewVersionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.borderless_NewVersionNumber.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.borderless_NewVersionNumber.UseVisualStyleBackColor = false;
            // 
            // borderless_NewVersion
            // 
            animMessage9.Control = this.borderless_NewVersion;
            animMessage9.PlayAnimation = true;
            animProperties5.BackColorMessage = animMessage9;
            animMessage10.Control = this.borderless_NewVersion;
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
            this.borderless_NewVersion.AnimProperties = animProperties5;
            this.borderless_NewVersion.BackColor = System.Drawing.Color.Transparent;
            this.borderless_NewVersion.CaptureChildren = true;
            this.borderless_NewVersion.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_NewVersion.FlatAppearance.BorderSize = 0;
            this.borderless_NewVersion.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_NewVersion.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_NewVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_NewVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_NewVersion.ForeColor = System.Drawing.Color.White;
            this.borderless_NewVersion.IgnoreMouse = false;
            this.borderless_NewVersion.IgnoreMouseClicks = false;
            this.borderless_NewVersion.Location = new System.Drawing.Point(38, 121);
            this.borderless_NewVersion.Name = "borderless_NewVersion";
            this.borderless_NewVersion.Size = new System.Drawing.Size(177, 27);
            this.borderless_NewVersion.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_NewVersion.TabIndex = 48;
            this.borderless_NewVersion.Text = "New Version:";
            this.borderless_NewVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_NewVersion.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.borderless_NewVersion.UseVisualStyleBackColor = false;
            // 
            // borderless_OldVersion
            // 
            animMessage11.Control = this.borderless_OldVersion;
            animMessage11.PlayAnimation = true;
            animProperties6.BackColorMessage = animMessage11;
            animMessage12.Control = this.borderless_OldVersion;
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
            this.borderless_OldVersion.AnimProperties = animProperties6;
            this.borderless_OldVersion.BackColor = System.Drawing.Color.Transparent;
            this.borderless_OldVersion.CaptureChildren = true;
            this.borderless_OldVersion.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_OldVersion.FlatAppearance.BorderSize = 0;
            this.borderless_OldVersion.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_OldVersion.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_OldVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_OldVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_OldVersion.ForeColor = System.Drawing.Color.White;
            this.borderless_OldVersion.IgnoreMouse = false;
            this.borderless_OldVersion.IgnoreMouseClicks = false;
            this.borderless_OldVersion.Location = new System.Drawing.Point(38, 94);
            this.borderless_OldVersion.Name = "borderless_OldVersion";
            this.borderless_OldVersion.Size = new System.Drawing.Size(177, 27);
            this.borderless_OldVersion.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_OldVersion.TabIndex = 47;
            this.borderless_OldVersion.Text = "Current Version:";
            this.borderless_OldVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_OldVersion.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.borderless_OldVersion.UseVisualStyleBackColor = false;
            // 
            // item_Changelog
            // 
            animMessage13.Control = this.item_Changelog;
            animMessage13.PlayAnimation = true;
            animProperties7.BackColorMessage = animMessage13;
            animMessage14.Control = this.item_Changelog;
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
            this.item_Changelog.AnimProperties = animProperties7;
            this.item_Changelog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.item_Changelog.CaptureChildren = true;
            this.item_Changelog.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.item_Changelog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_Changelog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_Changelog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_Changelog.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.item_Changelog.ForeColor = System.Drawing.Color.White;
            this.item_Changelog.IgnoreMouse = false;
            this.item_Changelog.IgnoreMouseClicks = false;
            this.item_Changelog.Location = new System.Drawing.Point(38, 157);
            this.item_Changelog.Name = "item_Changelog";
            this.item_Changelog.Size = new System.Drawing.Size(177, 35);
            this.item_Changelog.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_Changelog.TabIndex = 31;
            this.item_Changelog.Text = "View Changelog";
            this.item_Changelog.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.item_Changelog.UseVisualStyleBackColor = false;
            this.item_Changelog.Click += new System.EventHandler(this.item_Changelog_Click);
            // 
            // borderless_UpdateProgressBar
            // 
            this.borderless_UpdateProgressBar.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_UpdateProgressBar.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_UpdateProgressBar.BottomBorderWidth = 2;
            this.borderless_UpdateProgressBar.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_UpdateProgressBar.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_UpdateProgressBar.LeftBorderWidth = 2;
            this.borderless_UpdateProgressBar.Location = new System.Drawing.Point(38, 63);
            this.borderless_UpdateProgressBar.Name = "borderless_UpdateProgressBar";
            this.borderless_UpdateProgressBar.ProgressColour = System.Drawing.Color.White;
            this.borderless_UpdateProgressBar.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_UpdateProgressBar.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_UpdateProgressBar.RightBorderWidth = 2;
            this.borderless_UpdateProgressBar.Size = new System.Drawing.Size(298, 23);
            this.borderless_UpdateProgressBar.TabIndex = 29;
            this.borderless_UpdateProgressBar.Text = "horizontalProgressBar1";
            this.borderless_UpdateProgressBar.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_UpdateProgressBar.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_UpdateProgressBar.TopBorderWidth = 2;
            this.borderless_UpdateProgressBar.Value = 0;
            // 
            // titleBar_Title
            // 
            animMessage3.Control = this.titleBar_Title;
            animMessage3.PlayAnimation = true;
            animProperties2.BackColorMessage = animMessage3;
            animMessage4.Control = this.titleBar_Title;
            animMessage4.PlayAnimation = true;
            animProperties2.ForeColorMessage = animMessage4;
            animProperties2.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            animProperties2.MouseEnterDuration = 200F;
            animProperties2.MouseEnterForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(163)))), ((int)(((byte)(244)))));
            animProperties2.MouseEnterFramerate = 144F;
            animProperties2.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties2.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            animProperties2.MouseLeaveDuration = 200F;
            animProperties2.MouseLeaveForeColor = System.Drawing.Color.White;
            animProperties2.MouseLeaveFramerate = 144F;
            animProperties2.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.titleBar_Title.AnimProperties = animProperties2;
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
            this.titleBar_Title.Size = new System.Drawing.Size(374, 44);
            this.titleBar_Title.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.titleBar_Title.TabIndex = 3;
            this.titleBar_Title.Text = "Update Available";
            this.titleBar_Title.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.titleBar_Title.UseVisualStyleBackColor = true;
            // 
            // item_Update
            // 
            animMessage15.Control = this.item_Update;
            animMessage15.PlayAnimation = true;
            animProperties8.BackColorMessage = animMessage15;
            animMessage16.Control = this.item_Update;
            animMessage16.PlayAnimation = true;
            animProperties8.ForeColorMessage = animMessage16;
            animProperties8.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties8.MouseEnterDuration = 0F;
            animProperties8.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties8.MouseEnterFramerate = 0F;
            animProperties8.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties8.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties8.MouseLeaveDuration = 0F;
            animProperties8.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties8.MouseLeaveFramerate = 0F;
            animProperties8.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.item_Update.AnimProperties = animProperties8;
            this.item_Update.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.item_Update.CaptureChildren = true;
            this.item_Update.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.item_Update.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_Update.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_Update.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_Update.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.item_Update.ForeColor = System.Drawing.Color.White;
            this.item_Update.IgnoreMouse = false;
            this.item_Update.IgnoreMouseClicks = false;
            this.item_Update.Location = new System.Drawing.Point(221, 157);
            this.item_Update.Name = "item_Update";
            this.item_Update.Size = new System.Drawing.Size(115, 35);
            this.item_Update.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_Update.TabIndex = 30;
            this.item_Update.Text = "Update";
            this.item_Update.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.item_Update.UseVisualStyleBackColor = false;
            this.item_Update.Click += new System.EventHandler(this.item_Update_Click);
            // 
            // categoryBar_Close
            // 
            animMessage1.Control = this.categoryBar_Close;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.categoryBar_Close;
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
            this.categoryBar_Close.AnimProperties = animProperties1;
            this.categoryBar_Close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.categoryBar_Close.CaptureChildren = false;
            this.categoryBar_Close.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.categoryBar_Close.FlatAppearance.BorderSize = 0;
            this.categoryBar_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.categoryBar_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.categoryBar_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categoryBar_Close.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.categoryBar_Close.ForeColor = System.Drawing.Color.White;
            this.categoryBar_Close.IgnoreMouse = false;
            this.categoryBar_Close.IgnoreMouseClicks = false;
            this.categoryBar_Close.Location = new System.Drawing.Point(345, -1);
            this.categoryBar_Close.Name = "categoryBar_Close";
            this.categoryBar_Close.Size = new System.Drawing.Size(30, 30);
            this.categoryBar_Close.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.categoryBar_Close.TabIndex = 51;
            this.categoryBar_Close.Text = "X";
            this.categoryBar_Close.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.categoryBar_Close.UseVisualStyleBackColor = false;
            this.categoryBar_Close.Click += new System.EventHandler(this.item_Close_Click);
            // 
            // DownloadUpdatesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(374, 210);
            this.Controls.Add(this.borderless_OldVersionNumber);
            this.Controls.Add(this.borderless_NewVersionNumber);
            this.Controls.Add(this.borderless_NewVersion);
            this.Controls.Add(this.borderless_OldVersion);
            this.Controls.Add(this.item_Changelog);
            this.Controls.Add(this.item_Update);
            this.Controls.Add(this.borderless_UpdateProgressBar);
            this.Controls.Add(this.titleBar_Panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DownloadUpdatesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doki Doki Modification Club!";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Base_Load);
            this.Shown += new System.EventHandler(this.DownloadUpdatesDialog_Shown);
            this.titleBar_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton titleBar_Title;
        public System.Windows.Forms.Panel titleBar_Panel;
        private System.Windows.Forms.ToolTip toolTipHelper;
        private Reloaded_GUI.Styles.Controls.Custom.HorizontalProgressBar borderless_UpdateProgressBar;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton item_Changelog;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton borderless_OldVersion;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton borderless_NewVersion;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton borderless_NewVersionNumber;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton borderless_OldVersionNumber;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton item_Update;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton categoryBar_Close;
    }
}

