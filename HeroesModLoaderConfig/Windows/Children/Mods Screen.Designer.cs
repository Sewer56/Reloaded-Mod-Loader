using HeroesModLoaderConfig.Styles.Controls.Animated;
using HeroesModLoaderConfig.Styles.Controls.Enhanced;

namespace HeroesModLoaderConfig.Windows.Children
{
    partial class Mods_Screen
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
            HeroesModLoaderConfig.Styles.Animation.AnimProperties animProperties1 = new HeroesModLoaderConfig.Styles.Animation.AnimProperties();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage1 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage2 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            HeroesModLoaderConfig.Styles.Animation.AnimProperties animProperties2 = new HeroesModLoaderConfig.Styles.Animation.AnimProperties();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage3 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage4 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            this.box_ModList = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedDataGridView();
            this.modName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modDirectory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.box_ModListBox = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.item_LocationBoxDirectoryTitle = new HeroesModLoaderConfig.Styles.Controls.EnhancedLabel();
            ((System.ComponentModel.ISupportInitialize)(this.box_ModList)).BeginInit();
            this.SuspendLayout();
            // 
            // box_ModList
            // 
            this.box_ModList.AllowUserToAddRows = false;
            this.box_ModList.AllowUserToDeleteRows = false;
            this.box_ModList.AllowUserToResizeColumns = false;
            this.box_ModList.AllowUserToResizeRows = false;
            animMessage1.Control = this.box_ModList;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.box_ModList;
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
            this.box_ModList.AnimProperties = animProperties1;
            this.box_ModList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.box_ModList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.box_ModList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.box_ModList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.box_ModList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.box_ModList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.box_ModList.ColumnHeadersVisible = false;
            this.box_ModList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.modName,
            this.modDirectory});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Roboto Mono", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.box_ModList.DefaultCellStyle = dataGridViewCellStyle3;
            this.box_ModList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.box_ModList.EnableHeadersVisualStyles = false;
            this.box_ModList.GridColor = System.Drawing.Color.White;
            this.box_ModList.Location = new System.Drawing.Point(41, 40);
            this.box_ModList.MultiSelect = false;
            this.box_ModList.Name = "box_ModList";
            this.box_ModList.ReadOnly = true;
            this.box_ModList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.box_ModList.RowHeadersVisible = false;
            this.box_ModList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.box_ModList.RowTemplate.Height = 30;
            this.box_ModList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.box_ModList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.box_ModList.Size = new System.Drawing.Size(518, 330);
            this.box_ModList.StandardTab = true;
            this.box_ModList.TabIndex = 14;
            this.box_ModList.SelectionChanged += new System.EventHandler(this.ModList_SelectionChanged);
            // 
            // modName
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.modName.DefaultCellStyle = dataGridViewCellStyle1;
            this.modName.FillWeight = 152.2843F;
            this.modName.HeaderText = "Mod name";
            this.modName.Name = "modName";
            this.modName.ReadOnly = true;
            this.modName.Width = 200;
            // 
            // modDirectory
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.modDirectory.DefaultCellStyle = dataGridViewCellStyle2;
            this.modDirectory.FillWeight = 47.71573F;
            this.modDirectory.HeaderText = "Mod Directory";
            this.modDirectory.Name = "modDirectory";
            this.modDirectory.ReadOnly = true;
            this.modDirectory.Width = 318;
            // 
            // box_ModListBox
            // 
            animMessage3.Control = this.box_ModListBox;
            animMessage3.PlayAnimation = true;
            animProperties2.BackColorMessage = animMessage3;
            animMessage4.Control = this.box_ModListBox;
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
            this.box_ModListBox.AnimProperties = animProperties2;
            this.box_ModListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_ModListBox.CaptureChildren = false;
            this.box_ModListBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.box_ModListBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_ModListBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_ModListBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.box_ModListBox.IgnoreMouse = true;
            this.box_ModListBox.IgnoreMouseClicks = true;
            this.box_ModListBox.Location = new System.Drawing.Point(40, 39);
            this.box_ModListBox.Margin = new System.Windows.Forms.Padding(0);
            this.box_ModListBox.Name = "box_ModListBox";
            this.box_ModListBox.Size = new System.Drawing.Size(520, 332);
            this.box_ModListBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.box_ModListBox.TabIndex = 13;
            this.box_ModListBox.Text = null;
            this.box_ModListBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.box_ModListBox.UseVisualStyleBackColor = false;
            // 
            // item_LocationBoxDirectoryTitle
            // 
            this.item_LocationBoxDirectoryTitle.AutoSize = true;
            this.item_LocationBoxDirectoryTitle.BackColor = System.Drawing.Color.Transparent;
            this.item_LocationBoxDirectoryTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_LocationBoxDirectoryTitle.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.item_LocationBoxDirectoryTitle.ForeColor = System.Drawing.Color.White;
            this.item_LocationBoxDirectoryTitle.IgnoreMouse = false;
            this.item_LocationBoxDirectoryTitle.Location = new System.Drawing.Point(52, 331);
            this.item_LocationBoxDirectoryTitle.Margin = new System.Windows.Forms.Padding(0);
            this.item_LocationBoxDirectoryTitle.Name = "item_LocationBoxDirectoryTitle";
            this.item_LocationBoxDirectoryTitle.Size = new System.Drawing.Size(120, 24);
            this.item_LocationBoxDirectoryTitle.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_LocationBoxDirectoryTitle.TabIndex = 1;
            this.item_LocationBoxDirectoryTitle.Text = "DIRECTORY:";
            this.item_LocationBoxDirectoryTitle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // Mods_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(900, 512);
            this.Controls.Add(this.box_ModList);
            this.Controls.Add(this.box_ModListBox);
            this.Controls.Add(this.item_LocationBoxDirectoryTitle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Mods_Screen";
            this.Text = "Main_Screen";
            ((System.ComponentModel.ISupportInitialize)(this.box_ModList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Styles.Controls.EnhancedLabel item_LocationBoxDirectoryTitle;
        private Styles.Controls.Animated.AnimatedButton box_ModListBox;
        private AnimatedDataGridView box_ModList;
        private System.Windows.Forms.DataGridViewTextBoxColumn modName;
        private System.Windows.Forms.DataGridViewTextBoxColumn modDirectory;
    }
}