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

namespace ReloadedLauncher.Windows.Children.Dialogs.Input_Screen
{
    partial class GetNumberDialog
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
            this.borderless_OKButton = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_ValueBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedTextbox();
            this.titleBar_Title = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.titleBar_Panel = new System.Windows.Forms.Panel();
            this.toolTipHelper = new System.Windows.Forms.ToolTip(this.components);
            this.titleBar_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // borderless_OKButton
            // 
            animMessage1.Control = this.borderless_OKButton;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.borderless_OKButton;
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
            this.borderless_OKButton.AnimProperties = animProperties1;
            this.borderless_OKButton.CaptureChildren = false;
            this.borderless_OKButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_OKButton.FlatAppearance.BorderSize = 0;
            this.borderless_OKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_OKButton.Font = new System.Drawing.Font("Roboto Mono", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_OKButton.ForeColor = System.Drawing.Color.White;
            this.borderless_OKButton.IgnoreMouse = false;
            this.borderless_OKButton.IgnoreMouseClicks = false;
            this.borderless_OKButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_OKButton.Location = new System.Drawing.Point(286, 63);
            this.borderless_OKButton.Name = "borderless_OKButton";
            this.borderless_OKButton.Size = new System.Drawing.Size(36, 24);
            this.borderless_OKButton.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_OKButton.TabIndex = 28;
            this.borderless_OKButton.Text = "OK";
            this.borderless_OKButton.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.toolTipHelper.SetToolTip(this.borderless_OKButton, "Double click an entry to bind a new joystick button to the \"virtual\" XBOX 360 con" +
        "troller used by Reloaded.");
            this.borderless_OKButton.UseVisualStyleBackColor = true;
            this.borderless_OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // borderless_ValueBox
            // 
            animMessage3.Control = this.borderless_ValueBox;
            animMessage3.PlayAnimation = true;
            animProperties2.BackColorMessage = animMessage3;
            animMessage4.Control = this.borderless_ValueBox;
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
            this.borderless_ValueBox.AnimProperties = animProperties2;
            this.borderless_ValueBox.BackColor = System.Drawing.Color.Gray;
            this.borderless_ValueBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.borderless_ValueBox.BottomBorderColour = System.Drawing.Color.Red;
            this.borderless_ValueBox.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ValueBox.BottomBorderWidth = 55;
            this.borderless_ValueBox.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_ValueBox.ForeColor = System.Drawing.Color.Red;
            this.borderless_ValueBox.LeftBorderColour = System.Drawing.Color.Red;
            this.borderless_ValueBox.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_ValueBox.LeftBorderWidth = 0;
            this.borderless_ValueBox.Location = new System.Drawing.Point(52, 63);
            this.borderless_ValueBox.Name = "borderless_ValueBox";
            this.borderless_ValueBox.RightBorderColour = System.Drawing.Color.Red;
            this.borderless_ValueBox.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_ValueBox.RightBorderWidth = 0;
            this.borderless_ValueBox.Size = new System.Drawing.Size(228, 24);
            this.borderless_ValueBox.TabIndex = 2;
            this.borderless_ValueBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTipHelper.SetToolTip(this.borderless_ValueBox, "Specifies the name of the game as seen in Reloaded Launcher");
            this.borderless_ValueBox.TopBorderColour = System.Drawing.Color.Red;
            this.borderless_ValueBox.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_ValueBox.TopBorderWidth = 0;
            this.borderless_ValueBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.borderless_ValueBox_KeyUp);
            // 
            // titleBar_Title
            // 
            animMessage5.Control = this.titleBar_Title;
            animMessage5.PlayAnimation = true;
            animProperties3.BackColorMessage = animMessage5;
            animMessage6.Control = this.titleBar_Title;
            animMessage6.PlayAnimation = true;
            animProperties3.ForeColorMessage = animMessage6;
            animProperties3.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            animProperties3.MouseEnterDuration = 200F;
            animProperties3.MouseEnterForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(163)))), ((int)(((byte)(244)))));
            animProperties3.MouseEnterFramerate = 144F;
            animProperties3.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties3.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            animProperties3.MouseLeaveDuration = 200F;
            animProperties3.MouseLeaveForeColor = System.Drawing.Color.White;
            animProperties3.MouseLeaveFramerate = 144F;
            animProperties3.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.titleBar_Title.AnimProperties = animProperties3;
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
            this.titleBar_Title.Size = new System.Drawing.Size(374, 44);
            this.titleBar_Title.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.titleBar_Title.TabIndex = 3;
            this.titleBar_Title.Text = "Reloaded Sample Dialog";
            this.titleBar_Title.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.titleBar_Title.UseVisualStyleBackColor = true;
            // 
            // titleBar_Panel
            // 
            this.titleBar_Panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.titleBar_Panel.Controls.Add(this.titleBar_Title);
            this.titleBar_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar_Panel.Location = new System.Drawing.Point(0, 0);
            this.titleBar_Panel.Margin = new System.Windows.Forms.Padding(0);
            this.titleBar_Panel.Name = "titleBar_Panel";
            this.titleBar_Panel.Size = new System.Drawing.Size(374, 44);
            this.titleBar_Panel.TabIndex = 0;
            // 
            // GetNumberDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(374, 105);
            this.Controls.Add(this.borderless_OKButton);
            this.Controls.Add(this.borderless_ValueBox);
            this.Controls.Add(this.titleBar_Panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GetNumberDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doki Doki Modification Club!";
            this.Load += new System.EventHandler(this.Base_Load);
            this.titleBar_Panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton titleBar_Title;
        public System.Windows.Forms.Panel titleBar_Panel;
        private System.Windows.Forms.ToolTip toolTipHelper;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedTextbox borderless_ValueBox;
        public Reloaded_GUI.Styles.Controls.Animated.AnimatedButton borderless_OKButton;
    }
}

