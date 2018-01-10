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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Base));
            this.panel_TitleBar = new System.Windows.Forms.Panel();
            this.titleBar_Test = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.Minimize = new HeroesModLoaderConfig.Styles.Controls.EnhancedButton();
            this.titleBar_Title = new HeroesModLoaderConfig.Styles.Controls.EnhancedButton();
            this.panel_TitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_TitleBar
            // 
            this.panel_TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.panel_TitleBar.Controls.Add(this.Minimize);
            this.panel_TitleBar.Controls.Add(this.titleBar_Title);
            this.panel_TitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_TitleBar.Location = new System.Drawing.Point(0, 0);
            this.panel_TitleBar.Margin = new System.Windows.Forms.Padding(0);
            this.panel_TitleBar.Name = "panel_TitleBar";
            this.panel_TitleBar.Size = new System.Drawing.Size(884, 44);
            this.panel_TitleBar.TabIndex = 0;
            // 
            // titleBar_Test
            // 
            this.titleBar_Test.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.titleBar_Test.FlatAppearance.BorderSize = 0;
            this.titleBar_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titleBar_Test.Font = new System.Drawing.Font("Racon Basic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleBar_Test.ForeColor = System.Drawing.Color.White;
            this.titleBar_Test.Location = new System.Drawing.Point(314, 260);
            this.titleBar_Test.MouseEnterBackColor = System.Drawing.Color.White;
            this.titleBar_Test.MouseEnterDuration = 250F;
            this.titleBar_Test.MouseEnterForeColor = System.Drawing.Color.Black;
            this.titleBar_Test.MouseEnterFramerate = 144F;
            this.titleBar_Test.MouseEnterOverride = ((HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride)((HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.BackColorInterpolate | HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.ForeColorInterpolate)));
            this.titleBar_Test.MouseLeaveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(64)))));
            this.titleBar_Test.MouseLeaveDuration = 250F;
            this.titleBar_Test.MouseLeaveForeColor = System.Drawing.Color.White;
            this.titleBar_Test.MouseLeaveFramerate = 144F;
            this.titleBar_Test.MouseLeaveOverride = ((HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride)((HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.BackColorInterpolate | HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.ForeColorInterpolate)));
            this.titleBar_Test.Name = "titleBar_Test";
            this.titleBar_Test.Size = new System.Drawing.Size(256, 42);
            this.titleBar_Test.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.titleBar_Test.TabIndex = 0;
            this.titleBar_Test.Text = "Test Button";
            this.titleBar_Test.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.titleBar_Test.UseVisualStyleBackColor = false;
            // 
            // Minimize
            // 
            this.Minimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Minimize.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Minimize.BackgroundImage")));
            this.Minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Minimize.FlatAppearance.BorderSize = 0;
            this.Minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Minimize.Location = new System.Drawing.Point(845, 0);
            this.Minimize.Name = "Minimize";
            this.Minimize.Size = new System.Drawing.Size(39, 24);
            this.Minimize.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.Minimize.TabIndex = 1;
            this.Minimize.Text = null;
            this.Minimize.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.Minimize.UseVisualStyleBackColor = false;
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
            this.titleBar_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleBar_Title.ForeColor = System.Drawing.Color.White;
            this.titleBar_Title.Location = new System.Drawing.Point(0, 0);
            this.titleBar_Title.Margin = new System.Windows.Forms.Padding(0);
            this.titleBar_Title.Name = "titleBar_Title";
            this.titleBar_Title.Size = new System.Drawing.Size(884, 44);
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
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.titleBar_Test);
            this.Controls.Add(this.panel_TitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Base";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doki Doki Modification Club!";
            this.panel_TitleBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_TitleBar;
        private Styles.Controls.EnhancedButton titleBar_Title;
        private Styles.Controls.EnhancedButton Minimize;
        private Styles.Controls.Animated.AnimatedButton titleBar_Test;
    }
}

