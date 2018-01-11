namespace HeroesModLoaderConfig.Windows.Children
{
    partial class Main_Screen
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
            this.item_LocationBox = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.SuspendLayout();
            // 
            // item_LocationBox
            // 
            this.item_LocationBox.Location = new System.Drawing.Point(192, 314);
            this.item_LocationBox.MouseEnterBackColor = System.Drawing.Color.Empty;
            this.item_LocationBox.MouseEnterDuration = 0F;
            this.item_LocationBox.MouseEnterForeColor = System.Drawing.Color.Empty;
            this.item_LocationBox.MouseEnterFramerate = 0F;
            this.item_LocationBox.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            this.item_LocationBox.MouseLeaveBackColor = System.Drawing.Color.Empty;
            this.item_LocationBox.MouseLeaveDuration = 0F;
            this.item_LocationBox.MouseLeaveForeColor = System.Drawing.Color.Empty;
            this.item_LocationBox.MouseLeaveFramerate = 0F;
            this.item_LocationBox.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.item_LocationBox.Name = "item_LocationBox";
            this.item_LocationBox.Size = new System.Drawing.Size(553, 213);
            this.item_LocationBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_LocationBox.TabIndex = 0;
            this.item_LocationBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.item_LocationBox.UseVisualStyleBackColor = true;
            // 
            // Main_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(900, 512);
            this.Controls.Add(this.item_LocationBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Main_Screen";
            this.Text = "Main_Screen";
            this.ResumeLayout(false);

        }

        #endregion

        private Styles.Controls.Animated.AnimatedButton item_LocationBox;
    }
}