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
    partial class DownloadModDialog
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
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties3 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage5 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage6 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties4 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage7 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage8 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties5 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage9 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage10 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            this.borderless_SelectGame = new Reloaded_GUI.Styles.Controls.Animated.AnimatedComboBox();
            this.item_Update = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.categoryBar_Close = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.titleBar_Title = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_DownloadProgress = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.titleBar_Panel = new System.Windows.Forms.Panel();
            this.toolTipHelper = new System.Windows.Forms.ToolTip(this.components);
            this.borderless_UpdateProgressBar = new Reloaded_GUI.Styles.Controls.Custom.HorizontalProgressBar();
            this.titleBar_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // borderless_SelectGame
            // 
            animMessage1.Control = this.borderless_SelectGame;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.borderless_SelectGame;
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
            this.borderless_SelectGame.AnimProperties = animProperties1;
            this.borderless_SelectGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.borderless_SelectGame.BottomBorderColour = System.Drawing.Color.White;
            this.borderless_SelectGame.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_SelectGame.BottomBorderWidth = 2;
            this.borderless_SelectGame.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.borderless_SelectGame.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.borderless_SelectGame.DropDownArrowColour = System.Drawing.Color.White;
            this.borderless_SelectGame.DropDownButtonColour = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.borderless_SelectGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.borderless_SelectGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_SelectGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_SelectGame.ForeColor = System.Drawing.Color.White;
            this.borderless_SelectGame.FormattingEnabled = true;
            this.borderless_SelectGame.HighlightColor = System.Drawing.Color.Red;
            this.borderless_SelectGame.LeftBorderColour = System.Drawing.Color.Empty;
            this.borderless_SelectGame.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_SelectGame.LeftBorderWidth = 0;
            this.borderless_SelectGame.Location = new System.Drawing.Point(38, 64);
            this.borderless_SelectGame.Name = "borderless_SelectGame";
            this.borderless_SelectGame.RightBorderColour = System.Drawing.Color.Empty;
            this.borderless_SelectGame.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_SelectGame.RightBorderWidth = 0;
            this.borderless_SelectGame.Size = new System.Drawing.Size(298, 29);
            this.borderless_SelectGame.TabIndex = 32;
            this.toolTipHelper.SetToolTip(this.borderless_SelectGame, "Did you know you can use the scroll wheel to change games?");
            this.borderless_SelectGame.TopBorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_SelectGame.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_SelectGame.TopBorderWidth = 2;
            // 
            // item_Update
            // 
            animMessage3.Control = this.item_Update;
            animMessage3.PlayAnimation = true;
            animProperties2.BackColorMessage = animMessage3;
            animMessage4.Control = this.item_Update;
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
            this.item_Update.AnimProperties = animProperties2;
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
            this.item_Update.Location = new System.Drawing.Point(38, 153);
            this.item_Update.Name = "item_Update";
            this.item_Update.Size = new System.Drawing.Size(298, 35);
            this.item_Update.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_Update.TabIndex = 30;
            this.item_Update.Text = "Download Modification";
            this.item_Update.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.item_Update.UseVisualStyleBackColor = false;
            this.item_Update.Click += new System.EventHandler(this.item_Update_Click);
            // 
            // categoryBar_Close
            // 
            animMessage5.Control = this.categoryBar_Close;
            animMessage5.PlayAnimation = true;
            animProperties3.BackColorMessage = animMessage5;
            animMessage6.Control = this.categoryBar_Close;
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
            this.categoryBar_Close.AnimProperties = animProperties3;
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
            // titleBar_Title
            // 
            animMessage7.Control = this.titleBar_Title;
            animMessage7.PlayAnimation = true;
            animProperties4.BackColorMessage = animMessage7;
            animMessage8.Control = this.titleBar_Title;
            animMessage8.PlayAnimation = true;
            animProperties4.ForeColorMessage = animMessage8;
            animProperties4.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            animProperties4.MouseEnterDuration = 200F;
            animProperties4.MouseEnterForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(163)))), ((int)(((byte)(244)))));
            animProperties4.MouseEnterFramerate = 144F;
            animProperties4.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties4.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            animProperties4.MouseLeaveDuration = 200F;
            animProperties4.MouseLeaveForeColor = System.Drawing.Color.White;
            animProperties4.MouseLeaveFramerate = 144F;
            animProperties4.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.titleBar_Title.AnimProperties = animProperties4;
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
            this.titleBar_Title.Text = "Reloaded URL Detected";
            this.titleBar_Title.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.titleBar_Title.UseVisualStyleBackColor = true;
            // 
            // borderless_DownloadProgress
            // 
            animMessage9.Control = this.borderless_DownloadProgress;
            animMessage9.PlayAnimation = true;
            animProperties5.BackColorMessage = animMessage9;
            animMessage10.Control = this.borderless_DownloadProgress;
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
            this.borderless_DownloadProgress.AnimProperties = animProperties5;
            this.borderless_DownloadProgress.BackColor = System.Drawing.Color.Transparent;
            this.borderless_DownloadProgress.CaptureChildren = true;
            this.borderless_DownloadProgress.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_DownloadProgress.FlatAppearance.BorderSize = 0;
            this.borderless_DownloadProgress.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_DownloadProgress.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_DownloadProgress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_DownloadProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_DownloadProgress.ForeColor = System.Drawing.Color.White;
            this.borderless_DownloadProgress.IgnoreMouse = false;
            this.borderless_DownloadProgress.IgnoreMouseClicks = false;
            this.borderless_DownloadProgress.Location = new System.Drawing.Point(199, 98);
            this.borderless_DownloadProgress.Margin = new System.Windows.Forms.Padding(0);
            this.borderless_DownloadProgress.Name = "borderless_DownloadProgress";
            this.borderless_DownloadProgress.Size = new System.Drawing.Size(145, 48);
            this.borderless_DownloadProgress.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_DownloadProgress.TabIndex = 50;
            this.borderless_DownloadProgress.Text = "000.0/000.0MB";
            this.borderless_DownloadProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.borderless_DownloadProgress.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.borderless_DownloadProgress.UseVisualStyleBackColor = false;
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
            // borderless_UpdateProgressBar
            // 
            this.borderless_UpdateProgressBar.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_UpdateProgressBar.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_UpdateProgressBar.BottomBorderWidth = 2;
            this.borderless_UpdateProgressBar.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_UpdateProgressBar.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_UpdateProgressBar.LeftBorderWidth = 2;
            this.borderless_UpdateProgressBar.Location = new System.Drawing.Point(38, 111);
            this.borderless_UpdateProgressBar.Name = "borderless_UpdateProgressBar";
            this.borderless_UpdateProgressBar.ProgressColour = System.Drawing.Color.White;
            this.borderless_UpdateProgressBar.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_UpdateProgressBar.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_UpdateProgressBar.RightBorderWidth = 2;
            this.borderless_UpdateProgressBar.Size = new System.Drawing.Size(171, 23);
            this.borderless_UpdateProgressBar.TabIndex = 29;
            this.borderless_UpdateProgressBar.Text = "horizontalProgressBar1";
            this.borderless_UpdateProgressBar.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_UpdateProgressBar.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_UpdateProgressBar.TopBorderWidth = 2;
            this.borderless_UpdateProgressBar.Value = 0;
            // 
            // DownloadModDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(374, 206);
            this.Controls.Add(this.borderless_UpdateProgressBar);
            this.Controls.Add(this.borderless_DownloadProgress);
            this.Controls.Add(this.borderless_SelectGame);
            this.Controls.Add(this.item_Update);
            this.Controls.Add(this.titleBar_Panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DownloadModDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doki Doki Modification Club!";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Base_Load);
            this.titleBar_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton titleBar_Title;
        public System.Windows.Forms.Panel titleBar_Panel;
        private System.Windows.Forms.ToolTip toolTipHelper;
        private Reloaded_GUI.Styles.Controls.Custom.HorizontalProgressBar borderless_UpdateProgressBar;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton item_Update;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton categoryBar_Close;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedComboBox borderless_SelectGame;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton borderless_DownloadProgress;
    }
}

