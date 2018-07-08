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

namespace ReloadedLauncher.Windows.Children.Dialogs.Tutorial
{
    partial class WelcomeScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeScreen));
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties4 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage7 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage8 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            this.titleBar_Title = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.item_ok = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_SourceBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_UserGuide = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.titleBar_Panel = new System.Windows.Forms.Panel();
            this.toolTipHelper = new System.Windows.Forms.ToolTip(this.components);
            this.borderless_Text1 = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.borderless_Text2 = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.borderless_Text3 = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.titleBar_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleBar_Title
            // 
            animMessage1.Control = this.titleBar_Title;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.titleBar_Title;
            animMessage2.PlayAnimation = true;
            animProperties1.ForeColorMessage = animMessage2;
            animProperties1.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            animProperties1.MouseEnterDuration = 200F;
            animProperties1.MouseEnterForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(163)))), ((int)(((byte)(244)))));
            animProperties1.MouseEnterFramerate = 144F;
            animProperties1.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties1.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            animProperties1.MouseLeaveDuration = 200F;
            animProperties1.MouseLeaveForeColor = System.Drawing.Color.White;
            animProperties1.MouseLeaveFramerate = 144F;
            animProperties1.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.titleBar_Title.AnimProperties = animProperties1;
            this.titleBar_Title.CaptureChildren = false;
            this.titleBar_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBar_Title.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.titleBar_Title.FlatAppearance.BorderSize = 0;
            this.titleBar_Title.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.titleBar_Title.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.titleBar_Title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titleBar_Title.Font = new System.Drawing.Font("Roboto Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.titleBar_Title.ForeColor = System.Drawing.Color.White;
            this.titleBar_Title.IgnoreMouse = false;
            this.titleBar_Title.IgnoreMouseClicks = false;
            this.titleBar_Title.Location = new System.Drawing.Point(0, 0);
            this.titleBar_Title.Name = "titleBar_Title";
            this.titleBar_Title.Size = new System.Drawing.Size(496, 44);
            this.titleBar_Title.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.titleBar_Title.TabIndex = 3;
            this.titleBar_Title.Text = "Welcome to Reloaded";
            this.titleBar_Title.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.titleBar_Title.UseVisualStyleBackColor = true;
            // 
            // item_ok
            // 
            animMessage3.Control = this.item_ok;
            animMessage3.PlayAnimation = true;
            animProperties2.BackColorMessage = animMessage3;
            animMessage4.Control = this.item_ok;
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
            this.item_ok.AnimProperties = animProperties2;
            this.item_ok.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.item_ok.CaptureChildren = true;
            this.item_ok.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.item_ok.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_ok.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.item_ok.ForeColor = System.Drawing.Color.White;
            this.item_ok.IgnoreMouse = false;
            this.item_ok.IgnoreMouseClicks = false;
            this.item_ok.Location = new System.Drawing.Point(45, 285);
            this.item_ok.Margin = new System.Windows.Forms.Padding(15, 20, 30, 20);
            this.item_ok.Name = "item_ok";
            this.item_ok.Size = new System.Drawing.Size(410, 32);
            this.item_ok.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_ok.TabIndex = 52;
            this.item_ok.Text = "Ok";
            this.item_ok.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.item_ok.UseVisualStyleBackColor = false;
            this.item_ok.Click += new System.EventHandler(this.item_ok_Click);
            // 
            // borderless_SourceBox
            // 
            animMessage5.Control = this.borderless_SourceBox;
            animMessage5.PlayAnimation = true;
            animProperties3.BackColorMessage = animMessage5;
            animMessage6.Control = this.borderless_SourceBox;
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
            this.borderless_SourceBox.AnimProperties = animProperties3;
            this.borderless_SourceBox.CaptureChildren = false;
            this.borderless_SourceBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_SourceBox.FlatAppearance.BorderSize = 0;
            this.borderless_SourceBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_SourceBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_SourceBox.ForeColor = System.Drawing.Color.White;
            this.borderless_SourceBox.IgnoreMouse = false;
            this.borderless_SourceBox.IgnoreMouseClicks = false;
            this.borderless_SourceBox.Image = ((System.Drawing.Image)(resources.GetObject("borderless_SourceBox.Image")));
            this.borderless_SourceBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_SourceBox.Location = new System.Drawing.Point(45, 172);
            this.borderless_SourceBox.Name = "borderless_SourceBox";
            this.borderless_SourceBox.Size = new System.Drawing.Size(198, 50);
            this.borderless_SourceBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_SourceBox.TabIndex = 56;
            this.borderless_SourceBox.Text = "Documentation";
            this.borderless_SourceBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.borderless_SourceBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.borderless_SourceBox.UseVisualStyleBackColor = true;
            this.borderless_SourceBox.Click += new System.EventHandler(this.borderless_SourceBox_Click);
            // 
            // borderless_UserGuide
            // 
            animMessage7.Control = this.borderless_UserGuide;
            animMessage7.PlayAnimation = true;
            animProperties4.BackColorMessage = animMessage7;
            animMessage8.Control = this.borderless_UserGuide;
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
            this.borderless_UserGuide.AnimProperties = animProperties4;
            this.borderless_UserGuide.CaptureChildren = false;
            this.borderless_UserGuide.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_UserGuide.FlatAppearance.BorderSize = 0;
            this.borderless_UserGuide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_UserGuide.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_UserGuide.ForeColor = System.Drawing.Color.White;
            this.borderless_UserGuide.IgnoreMouse = false;
            this.borderless_UserGuide.IgnoreMouseClicks = false;
            this.borderless_UserGuide.Image = ((System.Drawing.Image)(resources.GetObject("borderless_UserGuide.Image")));
            this.borderless_UserGuide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_UserGuide.Location = new System.Drawing.Point(257, 172);
            this.borderless_UserGuide.Name = "borderless_UserGuide";
            this.borderless_UserGuide.Size = new System.Drawing.Size(198, 50);
            this.borderless_UserGuide.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_UserGuide.TabIndex = 57;
            this.borderless_UserGuide.Text = "User Guide";
            this.borderless_UserGuide.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.borderless_UserGuide.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.borderless_UserGuide.UseVisualStyleBackColor = true;
            this.borderless_UserGuide.Click += new System.EventHandler(this.borderless_UserGuide_Click);
            // 
            // titleBar_Panel
            // 
            this.titleBar_Panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.titleBar_Panel.Controls.Add(this.titleBar_Title);
            this.titleBar_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar_Panel.Location = new System.Drawing.Point(0, 0);
            this.titleBar_Panel.Margin = new System.Windows.Forms.Padding(0);
            this.titleBar_Panel.Name = "titleBar_Panel";
            this.titleBar_Panel.Size = new System.Drawing.Size(496, 44);
            this.titleBar_Panel.TabIndex = 0;
            // 
            // borderless_Text1
            // 
            this.borderless_Text1.BackColor = System.Drawing.Color.Transparent;
            this.borderless_Text1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_Text1.Font = new System.Drawing.Font("Roboto Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.borderless_Text1.ForeColor = System.Drawing.Color.White;
            this.borderless_Text1.IgnoreMouse = false;
            this.borderless_Text1.Location = new System.Drawing.Point(45, 65);
            this.borderless_Text1.Margin = new System.Windows.Forms.Padding(0);
            this.borderless_Text1.Name = "borderless_Text1";
            this.borderless_Text1.Size = new System.Drawing.Size(410, 28);
            this.borderless_Text1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_Text1.TabIndex = 51;
            this.borderless_Text1.Text = "It\'s (probably) your first time using Reloaded.";
            this.borderless_Text1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // borderless_Text2
            // 
            this.borderless_Text2.BackColor = System.Drawing.Color.Transparent;
            this.borderless_Text2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_Text2.Font = new System.Drawing.Font("Roboto Light", 14.25F);
            this.borderless_Text2.ForeColor = System.Drawing.Color.White;
            this.borderless_Text2.IgnoreMouse = false;
            this.borderless_Text2.Location = new System.Drawing.Point(45, 107);
            this.borderless_Text2.Margin = new System.Windows.Forms.Padding(0);
            this.borderless_Text2.Name = "borderless_Text2";
            this.borderless_Text2.Size = new System.Drawing.Size(410, 51);
            this.borderless_Text2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_Text2.TabIndex = 54;
            this.borderless_Text2.Text = "Before getting started, you might consider\r\nchecking out the following links:";
            this.borderless_Text2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // borderless_Text3
            // 
            this.borderless_Text3.BackColor = System.Drawing.Color.Transparent;
            this.borderless_Text3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_Text3.Font = new System.Drawing.Font("Roboto Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.borderless_Text3.ForeColor = System.Drawing.Color.White;
            this.borderless_Text3.IgnoreMouse = false;
            this.borderless_Text3.Location = new System.Drawing.Point(45, 237);
            this.borderless_Text3.Margin = new System.Windows.Forms.Padding(0);
            this.borderless_Text3.Name = "borderless_Text3";
            this.borderless_Text3.Size = new System.Drawing.Size(410, 28);
            this.borderless_Text3.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_Text3.TabIndex = 58;
            this.borderless_Text3.Text = "You may later also find these links in [About].";
            this.borderless_Text3.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // WelcomeScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(496, 346);
            this.Controls.Add(this.borderless_Text3);
            this.Controls.Add(this.borderless_UserGuide);
            this.Controls.Add(this.borderless_SourceBox);
            this.Controls.Add(this.item_ok);
            this.Controls.Add(this.borderless_Text1);
            this.Controls.Add(this.titleBar_Panel);
            this.Controls.Add(this.borderless_Text2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WelcomeScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doki Doki Modification Club!";
            this.Load += new System.EventHandler(this.Base_Load);
            this.titleBar_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton titleBar_Title;
        public System.Windows.Forms.Panel titleBar_Panel;
        private System.Windows.Forms.ToolTip toolTipHelper;
        private Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel borderless_Text1;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton item_ok;
        private Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel borderless_Text2;
        public Reloaded_GUI.Styles.Controls.Animated.AnimatedButton borderless_SourceBox;
        public Reloaded_GUI.Styles.Controls.Animated.AnimatedButton borderless_UserGuide;
        private Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel borderless_Text3;
    }
}

