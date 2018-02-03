using HeroesModLoaderConfig.Styles.Controls.Animated;

namespace HeroesModLoaderConfig.Windows.Children
{
    partial class Manage_Screen
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
            this.box_ComboBox = new HeroesModLoaderConfig.Styles.Controls.Enhanced.EnhancedComboBox();
            this.SuspendLayout();
            // 
            // box_ComboBox
            // 
            this.box_ComboBox.BackColor = System.Drawing.Color.Red;
            this.box_ComboBox.BottomBorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.box_ComboBox.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.box_ComboBox.BottomBorderWidth = 2;
            this.box_ComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.box_ComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.box_ComboBox.DropDownArrowColour = System.Drawing.Color.Red;
            this.box_ComboBox.DropDownButtonColour = System.Drawing.Color.Red;
            this.box_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.box_ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.box_ComboBox.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.box_ComboBox.ForeColor = System.Drawing.Color.White;
            this.box_ComboBox.FormattingEnabled = true;
            this.box_ComboBox.HighlightColor = System.Drawing.Color.Red;
            this.box_ComboBox.Items.AddRange(new object[] {
            "Ayy",
            "Lmao",
            "AyyMD",
            "Novideo"});
            this.box_ComboBox.LeftBorderColour = System.Drawing.Color.Empty;
            this.box_ComboBox.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.box_ComboBox.LeftBorderWidth = 0;
            this.box_ComboBox.Location = new System.Drawing.Point(372, 153);
            this.box_ComboBox.Name = "box_ComboBox";
            this.box_ComboBox.RightBorderColour = System.Drawing.Color.Empty;
            this.box_ComboBox.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.box_ComboBox.RightBorderWidth = 0;
            this.box_ComboBox.Size = new System.Drawing.Size(121, 32);
            this.box_ComboBox.TabIndex = 0;
            this.box_ComboBox.TopBorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.box_ComboBox.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.box_ComboBox.TopBorderWidth = 2;
            // 
            // Manage_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(900, 512);
            this.Controls.Add(this.box_ComboBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Manage_Screen";
            this.Text = "Main_Screen";
            this.VisibleChanged += new System.EventHandler(this.MenuVisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private Styles.Controls.Enhanced.EnhancedComboBox box_ComboBox;
    }
}