using HeroesModLoaderConfig.Properties;

namespace HeroesModLoaderConfig
{
    partial class Base
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
            this.panel_TitleBar = new System.Windows.Forms.Panel();
            this.panel_CategoryBar = new System.Windows.Forms.Panel();
            this.categoryBar_About = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.categoryBar_Theme = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.categoryBar_Input = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.categoryBar_Mods = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.categoryBar_Manager = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.categoryBar_Games = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.titleBar_Title = new HeroesModLoaderConfig.Styles.Controls.EnhancedButton();
            this.panel_TitleBar.SuspendLayout();
            this.panel_CategoryBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_TitleBar
            // 
            this.panel_TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.panel_TitleBar.Controls.Add(this.titleBar_Title);
            this.panel_TitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_TitleBar.Location = new System.Drawing.Point(0, 0);
            this.panel_TitleBar.Margin = new System.Windows.Forms.Padding(0);
            this.panel_TitleBar.Name = "panel_TitleBar";
            this.panel_TitleBar.Size = new System.Drawing.Size(900, 44);
            this.panel_TitleBar.TabIndex = 0;
            // 
            // panel_CategoryBar
            // 
            this.panel_CategoryBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.panel_CategoryBar.Controls.Add(this.categoryBar_About);
            this.panel_CategoryBar.Controls.Add(this.categoryBar_Theme);
            this.panel_CategoryBar.Controls.Add(this.categoryBar_Input);
            this.panel_CategoryBar.Controls.Add(this.categoryBar_Mods);
            this.panel_CategoryBar.Controls.Add(this.categoryBar_Manager);
            this.panel_CategoryBar.Controls.Add(this.categoryBar_Games);
            this.panel_CategoryBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_CategoryBar.Location = new System.Drawing.Point(0, 44);
            this.panel_CategoryBar.Margin = new System.Windows.Forms.Padding(0);
            this.panel_CategoryBar.Name = "panel_CategoryBar";
            this.panel_CategoryBar.Size = new System.Drawing.Size(900, 44);
            this.panel_CategoryBar.TabIndex = 1;
            // 
            // categoryBar_About
            // 
            this.categoryBar_About.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_About.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.categoryBar_About.FlatAppearance.BorderSize = 0;
            this.categoryBar_About.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_About.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_About.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categoryBar_About.Font = new System.Drawing.Font("Roboto Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.categoryBar_About.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.categoryBar_About.Location = new System.Drawing.Point(750, 0);
            this.categoryBar_About.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(63)))), ((int)(((byte)(67)))));
            this.categoryBar_About.MouseEnterDuration = 150F;
            this.categoryBar_About.MouseEnterForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_About.MouseEnterFramerate = 144F;
            this.categoryBar_About.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.BackColorInterpolate;
            this.categoryBar_About.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_About.MouseLeaveDuration = 150F;
            this.categoryBar_About.MouseLeaveForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_About.MouseLeaveFramerate = 144F;
            this.categoryBar_About.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.BackColorInterpolate;
            this.categoryBar_About.Name = "categoryBar_About";
            this.categoryBar_About.Size = new System.Drawing.Size(150, 44);
            this.categoryBar_About.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.categoryBar_About.TabIndex = 6;
            this.categoryBar_About.Text = "About";
            this.categoryBar_About.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.categoryBar_About.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.categoryBar_About.UseVisualStyleBackColor = false;
            // 
            // categoryBar_Theme
            // 
            this.categoryBar_Theme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_Theme.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.categoryBar_Theme.FlatAppearance.BorderSize = 0;
            this.categoryBar_Theme.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_Theme.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_Theme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categoryBar_Theme.Font = new System.Drawing.Font("Roboto Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.categoryBar_Theme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.categoryBar_Theme.Location = new System.Drawing.Point(450, 0);
            this.categoryBar_Theme.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(63)))), ((int)(((byte)(67)))));
            this.categoryBar_Theme.MouseEnterDuration = 150F;
            this.categoryBar_Theme.MouseEnterForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_Theme.MouseEnterFramerate = 144F;
            this.categoryBar_Theme.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.BackColorInterpolate;
            this.categoryBar_Theme.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_Theme.MouseLeaveDuration = 150F;
            this.categoryBar_Theme.MouseLeaveForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_Theme.MouseLeaveFramerate = 144F;
            this.categoryBar_Theme.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.BackColorInterpolate;
            this.categoryBar_Theme.Name = "categoryBar_Theme";
            this.categoryBar_Theme.Size = new System.Drawing.Size(150, 44);
            this.categoryBar_Theme.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.categoryBar_Theme.TabIndex = 5;
            this.categoryBar_Theme.Text = "Theme";
            this.categoryBar_Theme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.categoryBar_Theme.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.categoryBar_Theme.UseVisualStyleBackColor = false;
            // 
            // categoryBar_Input
            // 
            this.categoryBar_Input.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_Input.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.categoryBar_Input.FlatAppearance.BorderSize = 0;
            this.categoryBar_Input.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_Input.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_Input.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categoryBar_Input.Font = new System.Drawing.Font("Roboto Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.categoryBar_Input.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.categoryBar_Input.Location = new System.Drawing.Point(300, 0);
            this.categoryBar_Input.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(63)))), ((int)(((byte)(67)))));
            this.categoryBar_Input.MouseEnterDuration = 150F;
            this.categoryBar_Input.MouseEnterForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_Input.MouseEnterFramerate = 144F;
            this.categoryBar_Input.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.BackColorInterpolate;
            this.categoryBar_Input.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_Input.MouseLeaveDuration = 150F;
            this.categoryBar_Input.MouseLeaveForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_Input.MouseLeaveFramerate = 144F;
            this.categoryBar_Input.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.BackColorInterpolate;
            this.categoryBar_Input.Name = "categoryBar_Input";
            this.categoryBar_Input.Size = new System.Drawing.Size(150, 44);
            this.categoryBar_Input.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.categoryBar_Input.TabIndex = 4;
            this.categoryBar_Input.Text = "Input";
            this.categoryBar_Input.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.categoryBar_Input.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.categoryBar_Input.UseVisualStyleBackColor = false;
            // 
            // categoryBar_Mods
            // 
            this.categoryBar_Mods.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_Mods.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.categoryBar_Mods.FlatAppearance.BorderSize = 0;
            this.categoryBar_Mods.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_Mods.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_Mods.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categoryBar_Mods.Font = new System.Drawing.Font("Roboto Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.categoryBar_Mods.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.categoryBar_Mods.Location = new System.Drawing.Point(150, 0);
            this.categoryBar_Mods.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(63)))), ((int)(((byte)(67)))));
            this.categoryBar_Mods.MouseEnterDuration = 150F;
            this.categoryBar_Mods.MouseEnterForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_Mods.MouseEnterFramerate = 144F;
            this.categoryBar_Mods.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.BackColorInterpolate;
            this.categoryBar_Mods.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_Mods.MouseLeaveDuration = 150F;
            this.categoryBar_Mods.MouseLeaveForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_Mods.MouseLeaveFramerate = 144F;
            this.categoryBar_Mods.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.BackColorInterpolate;
            this.categoryBar_Mods.Name = "categoryBar_Mods";
            this.categoryBar_Mods.Size = new System.Drawing.Size(150, 44);
            this.categoryBar_Mods.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.categoryBar_Mods.TabIndex = 3;
            this.categoryBar_Mods.Text = "Mods";
            this.categoryBar_Mods.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.categoryBar_Mods.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.categoryBar_Mods.UseVisualStyleBackColor = false;
            // 
            // categoryBar_Manager
            // 
            this.categoryBar_Manager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_Manager.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.categoryBar_Manager.FlatAppearance.BorderSize = 0;
            this.categoryBar_Manager.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_Manager.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_Manager.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categoryBar_Manager.Font = new System.Drawing.Font("Roboto Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.categoryBar_Manager.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.categoryBar_Manager.Location = new System.Drawing.Point(600, 0);
            this.categoryBar_Manager.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(63)))), ((int)(((byte)(67)))));
            this.categoryBar_Manager.MouseEnterDuration = 150F;
            this.categoryBar_Manager.MouseEnterForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_Manager.MouseEnterFramerate = 144F;
            this.categoryBar_Manager.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.BackColorInterpolate;
            this.categoryBar_Manager.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_Manager.MouseLeaveDuration = 150F;
            this.categoryBar_Manager.MouseLeaveForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_Manager.MouseLeaveFramerate = 144F;
            this.categoryBar_Manager.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.BackColorInterpolate;
            this.categoryBar_Manager.Name = "categoryBar_Manager";
            this.categoryBar_Manager.Size = new System.Drawing.Size(150, 44);
            this.categoryBar_Manager.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.categoryBar_Manager.TabIndex = 2;
            this.categoryBar_Manager.Text = "Manage";
            this.categoryBar_Manager.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.categoryBar_Manager.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.categoryBar_Manager.UseVisualStyleBackColor = false;
            // 
            // categoryBar_Games
            // 
            this.categoryBar_Games.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_Games.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.categoryBar_Games.FlatAppearance.BorderSize = 0;
            this.categoryBar_Games.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_Games.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.categoryBar_Games.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categoryBar_Games.Font = new System.Drawing.Font("Roboto Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.categoryBar_Games.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.categoryBar_Games.Location = new System.Drawing.Point(0, 0);
            this.categoryBar_Games.MouseEnterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(63)))), ((int)(((byte)(67)))));
            this.categoryBar_Games.MouseEnterDuration = 150F;
            this.categoryBar_Games.MouseEnterForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_Games.MouseEnterFramerate = 144F;
            this.categoryBar_Games.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.BackColorInterpolate;
            this.categoryBar_Games.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.categoryBar_Games.MouseLeaveDuration = 150F;
            this.categoryBar_Games.MouseLeaveForeColor = System.Drawing.Color.Transparent;
            this.categoryBar_Games.MouseLeaveFramerate = 144F;
            this.categoryBar_Games.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.BackColorInterpolate;
            this.categoryBar_Games.Name = "categoryBar_Games";
            this.categoryBar_Games.Size = new System.Drawing.Size(150, 44);
            this.categoryBar_Games.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.categoryBar_Games.TabIndex = 1;
            this.categoryBar_Games.Text = "Games";
            this.categoryBar_Games.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.categoryBar_Games.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.categoryBar_Games.UseVisualStyleBackColor = false;
            // 
            // titleBar_Title
            // 
            this.titleBar_Title.BackColor = System.Drawing.Color.Transparent;
            this.titleBar_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBar_Title.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.titleBar_Title.FlatAppearance.BorderSize = 0;
            this.titleBar_Title.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.titleBar_Title.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.titleBar_Title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titleBar_Title.Font = new System.Drawing.Font("Roboto Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.titleBar_Title.ForeColor = System.Drawing.Color.White;
            this.titleBar_Title.Location = new System.Drawing.Point(0, 0);
            this.titleBar_Title.Margin = new System.Windows.Forms.Padding(0);
            this.titleBar_Title.Name = "titleBar_Title";
            this.titleBar_Title.Size = new System.Drawing.Size(900, 44);
            this.titleBar_Title.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.titleBar_Title.TabIndex = 1;
            this.titleBar_Title.TabStop = false;
            this.titleBar_Title.Text = "Heroes Mod Loader MKII";
            this.titleBar_Title.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.titleBar_Title.UseVisualStyleBackColor = false;
            // 
            // Base
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.panel_CategoryBar);
            this.Controls.Add(this.panel_TitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Base";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doki Doki Modification Club!";
            this.Load += new System.EventHandler(this.Base_Load);
            this.panel_TitleBar.ResumeLayout(false);
            this.panel_CategoryBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_TitleBar;
        private Styles.Controls.EnhancedButton titleBar_Title;
        private System.Windows.Forms.Panel panel_CategoryBar;
        public Styles.Controls.Animated.AnimatedButton categoryBar_Games;
        public Styles.Controls.Animated.AnimatedButton categoryBar_Mods;
        public Styles.Controls.Animated.AnimatedButton categoryBar_Input;
        public Styles.Controls.Animated.AnimatedButton categoryBar_Theme;
        public Styles.Controls.Animated.AnimatedButton categoryBar_Manager;
        public Styles.Controls.Animated.AnimatedButton categoryBar_About;
    }
}

