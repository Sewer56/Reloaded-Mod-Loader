using HeroesModLoaderConfig.Styles.Controls.Animated;

namespace HeroesModLoaderConfig.Windows.Children
{
    partial class About_Screen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About_Screen));
            HeroesModLoaderConfig.Styles.Animation.AnimProperties animProperties1 = new HeroesModLoaderConfig.Styles.Animation.AnimProperties();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage1 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage2 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            HeroesModLoaderConfig.Styles.Animation.AnimProperties animProperties2 = new HeroesModLoaderConfig.Styles.Animation.AnimProperties();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage3 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage4 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            this.box_ReloadedLogo = new System.Windows.Forms.PictureBox();
            this.item_Author = new HeroesModLoaderConfig.Styles.Controls.EnhancedLabel();
            this.box_AuthorBox = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.item_ReloadedModLoader = new HeroesModLoaderConfig.Styles.Controls.EnhancedLabel();
            this.box_ReloadedModLoaderBox = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            ((System.ComponentModel.ISupportInitialize)(this.box_ReloadedLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // box_ReloadedLogo
            // 
            this.box_ReloadedLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("box_ReloadedLogo.BackgroundImage")));
            this.box_ReloadedLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.box_ReloadedLogo.Location = new System.Drawing.Point(106, 20);
            this.box_ReloadedLogo.Name = "box_ReloadedLogo";
            this.box_ReloadedLogo.Size = new System.Drawing.Size(688, 223);
            this.box_ReloadedLogo.TabIndex = 0;
            this.box_ReloadedLogo.TabStop = false;
            // 
            // item_Author
            // 
            this.item_Author.BackColor = System.Drawing.Color.Transparent;
            this.item_Author.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_Author.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.item_Author.ForeColor = System.Drawing.Color.White;
            this.item_Author.IgnoreMouse = false;
            this.item_Author.Location = new System.Drawing.Point(250, 327);
            this.item_Author.Margin = new System.Windows.Forms.Padding(0);
            this.item_Author.Name = "item_Author";
            this.item_Author.Size = new System.Drawing.Size(400, 24);
            this.item_Author.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_Author.TabIndex = 24;
            this.item_Author.Text = "Written by Sewer56 ~ (C) 2018";
            this.item_Author.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.item_Author.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // box_AuthorBox
            // 
            animMessage1.Control = this.box_AuthorBox;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.box_AuthorBox;
            animMessage2.PlayAnimation = true;
            animProperties1.ForeColorMessage = animMessage2;
            animProperties1.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties1.MouseEnterDuration = 0F;
            animProperties1.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties1.MouseEnterFramerate = 0F;
            animProperties1.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties1.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties1.MouseLeaveDuration = 0F;
            animProperties1.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties1.MouseLeaveFramerate = 0F;
            animProperties1.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.box_AuthorBox.AnimProperties = animProperties1;
            this.box_AuthorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_AuthorBox.CaptureChildren = true;
            this.box_AuthorBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.box_AuthorBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_AuthorBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_AuthorBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.box_AuthorBox.IgnoreMouse = false;
            this.box_AuthorBox.IgnoreMouseClicks = true;
            this.box_AuthorBox.Location = new System.Drawing.Point(248, 318);
            this.box_AuthorBox.Name = "box_AuthorBox";
            this.box_AuthorBox.Size = new System.Drawing.Size(404, 43);
            this.box_AuthorBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            this.box_AuthorBox.TabIndex = 23;
            this.box_AuthorBox.Text = null;
            this.box_AuthorBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.box_AuthorBox.UseVisualStyleBackColor = false;
            // 
            // item_ReloadedModLoader
            // 
            this.item_ReloadedModLoader.BackColor = System.Drawing.Color.Transparent;
            this.item_ReloadedModLoader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_ReloadedModLoader.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.item_ReloadedModLoader.ForeColor = System.Drawing.Color.White;
            this.item_ReloadedModLoader.IgnoreMouse = false;
            this.item_ReloadedModLoader.Location = new System.Drawing.Point(326, 265);
            this.item_ReloadedModLoader.Margin = new System.Windows.Forms.Padding(0);
            this.item_ReloadedModLoader.Name = "item_ReloadedModLoader";
            this.item_ReloadedModLoader.Size = new System.Drawing.Size(248, 24);
            this.item_ReloadedModLoader.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_ReloadedModLoader.TabIndex = 22;
            this.item_ReloadedModLoader.Text = "Reloaded Mod Loader";
            this.item_ReloadedModLoader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.item_ReloadedModLoader.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // box_ReloadedModLoaderBox
            // 
            animMessage3.Control = this.box_ReloadedModLoaderBox;
            animMessage3.PlayAnimation = true;
            animProperties2.BackColorMessage = animMessage3;
            animMessage4.Control = this.box_ReloadedModLoaderBox;
            animMessage4.PlayAnimation = true;
            animProperties2.ForeColorMessage = animMessage4;
            animProperties2.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties2.MouseEnterDuration = 0F;
            animProperties2.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties2.MouseEnterFramerate = 0F;
            animProperties2.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties2.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties2.MouseLeaveDuration = 0F;
            animProperties2.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties2.MouseLeaveFramerate = 0F;
            animProperties2.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.box_ReloadedModLoaderBox.AnimProperties = animProperties2;
            this.box_ReloadedModLoaderBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_ReloadedModLoaderBox.CaptureChildren = true;
            this.box_ReloadedModLoaderBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.box_ReloadedModLoaderBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_ReloadedModLoaderBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_ReloadedModLoaderBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.box_ReloadedModLoaderBox.IgnoreMouse = false;
            this.box_ReloadedModLoaderBox.IgnoreMouseClicks = true;
            this.box_ReloadedModLoaderBox.Location = new System.Drawing.Point(324, 256);
            this.box_ReloadedModLoaderBox.Name = "box_ReloadedModLoaderBox";
            this.box_ReloadedModLoaderBox.Size = new System.Drawing.Size(252, 43);
            this.box_ReloadedModLoaderBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            this.box_ReloadedModLoaderBox.TabIndex = 21;
            this.box_ReloadedModLoaderBox.Text = null;
            this.box_ReloadedModLoaderBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.box_ReloadedModLoaderBox.UseVisualStyleBackColor = false;
            // 
            // About_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(900, 512);
            this.Controls.Add(this.item_Author);
            this.Controls.Add(this.box_AuthorBox);
            this.Controls.Add(this.item_ReloadedModLoader);
            this.Controls.Add(this.box_ReloadedModLoaderBox);
            this.Controls.Add(this.box_ReloadedLogo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "About_Screen";
            this.Text = "Main_Screen";
            this.VisibleChanged += new System.EventHandler(this.MenuVisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.box_ReloadedLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox box_ReloadedLogo;
        private Styles.Controls.EnhancedLabel item_ReloadedModLoader;
        private AnimatedButton box_ReloadedModLoaderBox;
        private Styles.Controls.EnhancedLabel item_Author;
        private AnimatedButton box_AuthorBox;
    }
}