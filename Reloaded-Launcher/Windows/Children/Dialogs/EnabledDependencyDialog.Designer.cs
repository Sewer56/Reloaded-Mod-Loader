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
    partial class EnabledDependencyDialog
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
            this.item_yes = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.titleBar_Title = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.box_DisabledDependenciesBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.titleBar_Panel = new System.Windows.Forms.Panel();
            this.toolTipHelper = new System.Windows.Forms.ToolTip(this.components);
            this.borderless_DependencyConfirm = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.borderless_EnabledDependencies = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.borderless_DependencyWarningTitle = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.titleBar_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // item_yes
            // 
            animMessage1.Control = this.item_yes;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.item_yes;
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
            this.item_yes.AnimProperties = animProperties1;
            this.item_yes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.item_yes.CaptureChildren = true;
            this.item_yes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.item_yes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_yes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.item_yes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_yes.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.item_yes.ForeColor = System.Drawing.Color.White;
            this.item_yes.IgnoreMouse = false;
            this.item_yes.IgnoreMouseClicks = false;
            this.item_yes.Location = new System.Drawing.Point(193, 403);
            this.item_yes.Margin = new System.Windows.Forms.Padding(15, 20, 30, 20);
            this.item_yes.Name = "item_yes";
            this.item_yes.Size = new System.Drawing.Size(177, 32);
            this.item_yes.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_yes.TabIndex = 23;
            this.item_yes.Text = "Ok";
            this.item_yes.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.item_yes.UseVisualStyleBackColor = false;
            this.item_yes.Click += new System.EventHandler(this.item_yes_Click);
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
            this.titleBar_Title.Font = new System.Drawing.Font("Roboto Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.titleBar_Title.ForeColor = System.Drawing.Color.White;
            this.titleBar_Title.IgnoreMouse = false;
            this.titleBar_Title.IgnoreMouseClicks = false;
            this.titleBar_Title.Location = new System.Drawing.Point(0, 0);
            this.titleBar_Title.Name = "titleBar_Title";
            this.titleBar_Title.Size = new System.Drawing.Size(561, 44);
            this.titleBar_Title.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.titleBar_Title.TabIndex = 3;
            this.titleBar_Title.Text = "Dependency Warning";
            this.titleBar_Title.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.titleBar_Title.UseVisualStyleBackColor = true;
            // 
            // box_DisabledDependenciesBox
            // 
            animMessage5.Control = this.box_DisabledDependenciesBox;
            animMessage5.PlayAnimation = true;
            animProperties3.BackColorMessage = animMessage5;
            animMessage6.Control = this.box_DisabledDependenciesBox;
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
            this.box_DisabledDependenciesBox.AnimProperties = animProperties3;
            this.box_DisabledDependenciesBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_DisabledDependenciesBox.CaptureChildren = false;
            this.box_DisabledDependenciesBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.box_DisabledDependenciesBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_DisabledDependenciesBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_DisabledDependenciesBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.box_DisabledDependenciesBox.IgnoreMouse = true;
            this.box_DisabledDependenciesBox.IgnoreMouseClicks = true;
            this.box_DisabledDependenciesBox.Location = new System.Drawing.Point(39, 130);
            this.box_DisabledDependenciesBox.Margin = new System.Windows.Forms.Padding(0);
            this.box_DisabledDependenciesBox.Name = "box_DisabledDependenciesBox";
            this.box_DisabledDependenciesBox.Size = new System.Drawing.Size(483, 125);
            this.box_DisabledDependenciesBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.box_DisabledDependenciesBox.TabIndex = 26;
            this.box_DisabledDependenciesBox.Text = null;
            this.box_DisabledDependenciesBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.box_DisabledDependenciesBox.UseVisualStyleBackColor = false;
            // 
            // titleBar_Panel
            // 
            this.titleBar_Panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.titleBar_Panel.Controls.Add(this.titleBar_Title);
            this.titleBar_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar_Panel.Location = new System.Drawing.Point(0, 0);
            this.titleBar_Panel.Margin = new System.Windows.Forms.Padding(0);
            this.titleBar_Panel.Name = "titleBar_Panel";
            this.titleBar_Panel.Size = new System.Drawing.Size(561, 44);
            this.titleBar_Panel.TabIndex = 0;
            // 
            // borderless_DependencyConfirm
            // 
            this.borderless_DependencyConfirm.BackColor = System.Drawing.Color.Transparent;
            this.borderless_DependencyConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_DependencyConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_DependencyConfirm.ForeColor = System.Drawing.Color.White;
            this.borderless_DependencyConfirm.IgnoreMouse = false;
            this.borderless_DependencyConfirm.Location = new System.Drawing.Point(39, 277);
            this.borderless_DependencyConfirm.Margin = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.borderless_DependencyConfirm.Name = "borderless_DependencyConfirm";
            this.borderless_DependencyConfirm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.borderless_DependencyConfirm.Size = new System.Drawing.Size(483, 106);
            this.borderless_DependencyConfirm.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_DependencyConfirm.TabIndex = 25;
            this.borderless_DependencyConfirm.Text = "Please note that disabling the mod may cause the list of mods above to stop funct" +
    "ioning.\r\n\r\nShould you experience adverse efffects such as crashing, consider man" +
    "ually disabling the mod(s) above.";
            this.borderless_DependencyConfirm.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // borderless_EnabledDependencies
            // 
            this.borderless_EnabledDependencies.BackColor = System.Drawing.Color.Transparent;
            this.borderless_EnabledDependencies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_EnabledDependencies.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_EnabledDependencies.ForeColor = System.Drawing.Color.White;
            this.borderless_EnabledDependencies.IgnoreMouse = false;
            this.borderless_EnabledDependencies.Location = new System.Drawing.Point(40, 131);
            this.borderless_EnabledDependencies.Margin = new System.Windows.Forms.Padding(30, 0, 30, 20);
            this.borderless_EnabledDependencies.Name = "borderless_EnabledDependencies";
            this.borderless_EnabledDependencies.Padding = new System.Windows.Forms.Padding(5);
            this.borderless_EnabledDependencies.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.borderless_EnabledDependencies.Size = new System.Drawing.Size(481, 122);
            this.borderless_EnabledDependencies.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_EnabledDependencies.TabIndex = 5;
            this.borderless_EnabledDependencies.Text = "Reloaded File Redirector [reloaded.global.fileredirector]";
            this.borderless_EnabledDependencies.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // borderless_DependencyWarningTitle
            // 
            this.borderless_DependencyWarningTitle.BackColor = System.Drawing.Color.Transparent;
            this.borderless_DependencyWarningTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_DependencyWarningTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_DependencyWarningTitle.ForeColor = System.Drawing.Color.White;
            this.borderless_DependencyWarningTitle.IgnoreMouse = false;
            this.borderless_DependencyWarningTitle.Location = new System.Drawing.Point(39, 67);
            this.borderless_DependencyWarningTitle.Margin = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.borderless_DependencyWarningTitle.Name = "borderless_DependencyWarningTitle";
            this.borderless_DependencyWarningTitle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.borderless_DependencyWarningTitle.Size = new System.Drawing.Size(483, 43);
            this.borderless_DependencyWarningTitle.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_DependencyWarningTitle.TabIndex = 4;
            this.borderless_DependencyWarningTitle.Text = "The following mods are enabled which depend on the mod you are about to disable:";
            this.borderless_DependencyWarningTitle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // EnabledDependencyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(561, 463);
            this.Controls.Add(this.borderless_DependencyConfirm);
            this.Controls.Add(this.item_yes);
            this.Controls.Add(this.borderless_EnabledDependencies);
            this.Controls.Add(this.borderless_DependencyWarningTitle);
            this.Controls.Add(this.titleBar_Panel);
            this.Controls.Add(this.box_DisabledDependenciesBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EnabledDependencyDialog";
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
        private Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel borderless_DependencyWarningTitle;
        private Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel borderless_EnabledDependencies;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton item_yes;
        private Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel borderless_DependencyConfirm;
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton box_DisabledDependenciesBox;
    }
}

