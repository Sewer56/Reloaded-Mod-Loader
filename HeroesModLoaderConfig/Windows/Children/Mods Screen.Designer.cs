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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            HeroesModLoaderConfig.Styles.Animation.AnimProperties animProperties2 = new HeroesModLoaderConfig.Styles.Animation.AnimProperties();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage3 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage4 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            HeroesModLoaderConfig.Styles.Animation.AnimProperties animProperties3 = new HeroesModLoaderConfig.Styles.Animation.AnimProperties();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage5 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage6 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            HeroesModLoaderConfig.Styles.Animation.AnimProperties animProperties4 = new HeroesModLoaderConfig.Styles.Animation.AnimProperties();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage7 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            HeroesModLoaderConfig.Styles.Animation.AnimMessage animMessage8 = new HeroesModLoaderConfig.Styles.Animation.AnimMessage();
            this.box_ModList = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedDataGridView();
            this.modEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.separator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modDirectory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.box_ModListBox = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.box_DescriptionBox = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
            this.item_LocationBoxDirectoryTitle = new HeroesModLoaderConfig.Styles.Controls.EnhancedLabel();
            this.item_ModDescription = new HeroesModLoaderConfig.Styles.Controls.EnhancedLabel();
            this.item_QuitBox = new HeroesModLoaderConfig.Styles.Controls.Animated.AnimatedButton();
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
            this.modEnabled,
            this.modName,
            this.Author,
            this.separator,
            this.modDirectory});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Roboto Mono", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.box_ModList.DefaultCellStyle = dataGridViewCellStyle6;
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
            this.box_ModList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Box_ModList_CellContentClick);
            this.box_ModList.SelectionChanged += new System.EventHandler(this.ModList_SelectionChanged);
            // 
            // modEnabled
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.modEnabled.DefaultCellStyle = dataGridViewCellStyle1;
            this.modEnabled.FillWeight = 20.30457F;
            this.modEnabled.HeaderText = "Enabled";
            this.modEnabled.Name = "modEnabled";
            this.modEnabled.ReadOnly = true;
            this.modEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.modEnabled.Width = 50;
            // 
            // modName
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.modName.DefaultCellStyle = dataGridViewCellStyle2;
            this.modName.FillWeight = 192.7388F;
            this.modName.HeaderText = "Name";
            this.modName.Name = "modName";
            this.modName.ReadOnly = true;
            this.modName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.modName.Width = 250;
            // 
            // Author
            // 
            this.Author.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.Author.DefaultCellStyle = dataGridViewCellStyle3;
            this.Author.FillWeight = 126.5651F;
            this.Author.HeaderText = "Author";
            this.Author.Name = "Author";
            this.Author.ReadOnly = true;
            this.Author.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // separator
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.separator.DefaultCellStyle = dataGridViewCellStyle4;
            this.separator.HeaderText = "Separator";
            this.separator.Name = "separator";
            this.separator.ReadOnly = true;
            this.separator.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.separator.Width = 20;
            // 
            // modDirectory
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.modDirectory.DefaultCellStyle = dataGridViewCellStyle5;
            this.modDirectory.FillWeight = 60.39148F;
            this.modDirectory.HeaderText = "Version";
            this.modDirectory.Name = "modDirectory";
            this.modDirectory.ReadOnly = true;
            this.modDirectory.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.modDirectory.Width = 60;
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
            // box_DescriptionBox
            // 
            animMessage5.Control = this.box_DescriptionBox;
            animMessage5.PlayAnimation = true;
            animProperties3.BackColorMessage = animMessage5;
            animMessage6.Control = this.box_DescriptionBox;
            animMessage6.PlayAnimation = true;
            animProperties3.ForeColorMessage = animMessage6;
            animProperties3.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties3.MouseEnterDuration = 0F;
            animProperties3.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties3.MouseEnterFramerate = 0F;
            animProperties3.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties3.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties3.MouseLeaveDuration = 0F;
            animProperties3.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties3.MouseLeaveFramerate = 0F;
            animProperties3.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.box_DescriptionBox.AnimProperties = animProperties3;
            this.box_DescriptionBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_DescriptionBox.CaptureChildren = true;
            this.box_DescriptionBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.box_DescriptionBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_DescriptionBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_DescriptionBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.box_DescriptionBox.IgnoreMouse = false;
            this.box_DescriptionBox.IgnoreMouseClicks = true;
            this.box_DescriptionBox.Location = new System.Drawing.Point(40, 407);
            this.box_DescriptionBox.Name = "box_DescriptionBox";
            this.box_DescriptionBox.Size = new System.Drawing.Size(520, 77);
            this.box_DescriptionBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            this.box_DescriptionBox.TabIndex = 15;
            this.box_DescriptionBox.Text = null;
            this.box_DescriptionBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.box_DescriptionBox.UseVisualStyleBackColor = false;
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
            // item_ModDescription
            // 
            this.item_ModDescription.BackColor = System.Drawing.Color.Transparent;
            this.item_ModDescription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_ModDescription.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.item_ModDescription.ForeColor = System.Drawing.Color.White;
            this.item_ModDescription.IgnoreMouse = false;
            this.item_ModDescription.Location = new System.Drawing.Point(59, 418);
            this.item_ModDescription.Margin = new System.Windows.Forms.Padding(0);
            this.item_ModDescription.Name = "item_ModDescription";
            this.item_ModDescription.Size = new System.Drawing.Size(482, 55);
            this.item_ModDescription.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_ModDescription.TabIndex = 16;
            this.item_ModDescription.Text = "Description";
            this.item_ModDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.item_ModDescription.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // item_QuitBox
            // 
            animMessage7.Control = this.item_QuitBox;
            animMessage7.PlayAnimation = true;
            animProperties4.BackColorMessage = animMessage7;
            animMessage8.Control = this.item_QuitBox;
            animMessage8.PlayAnimation = true;
            animProperties4.ForeColorMessage = animMessage8;
            animProperties4.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties4.MouseEnterDuration = 0F;
            animProperties4.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties4.MouseEnterFramerate = 0F;
            animProperties4.MouseEnterOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties4.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties4.MouseLeaveDuration = 0F;
            animProperties4.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties4.MouseLeaveFramerate = 0F;
            animProperties4.MouseLeaveOverride = HeroesModLoaderConfig.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.item_QuitBox.AnimProperties = animProperties4;
            this.item_QuitBox.CaptureChildren = false;
            this.item_QuitBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.item_QuitBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.item_QuitBox.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.item_QuitBox.ForeColor = System.Drawing.Color.White;
            this.item_QuitBox.IgnoreMouse = false;
            this.item_QuitBox.IgnoreMouseClicks = false;
            this.item_QuitBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.item_QuitBox.Location = new System.Drawing.Point(601, 407);
            this.item_QuitBox.Name = "item_QuitBox";
            this.item_QuitBox.Size = new System.Drawing.Size(287, 77);
            this.item_QuitBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.item_QuitBox.TabIndex = 17;
            this.item_QuitBox.Text = "Configuration";
            this.item_QuitBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.item_QuitBox.UseVisualStyleBackColor = true;
            // 
            // Mods_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(900, 512);
            this.Controls.Add(this.item_QuitBox);
            this.Controls.Add(this.item_ModDescription);
            this.Controls.Add(this.box_DescriptionBox);
            this.Controls.Add(this.box_ModList);
            this.Controls.Add(this.box_ModListBox);
            this.Controls.Add(this.item_LocationBoxDirectoryTitle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Mods_Screen";
            this.Text = "Main_Screen";
            this.Load += new System.EventHandler(this.Mods_Screen_Load);
            this.VisibleChanged += new System.EventHandler(this.Mods_Screen_VisibleChanged);
            this.Leave += new System.EventHandler(this.Mods_Screen_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.box_ModList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Styles.Controls.EnhancedLabel item_LocationBoxDirectoryTitle;
        private Styles.Controls.Animated.AnimatedButton box_ModListBox;
        private AnimatedDataGridView box_ModList;
        private System.Windows.Forms.DataGridViewTextBoxColumn modEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn modName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Author;
        private System.Windows.Forms.DataGridViewTextBoxColumn separator;
        private System.Windows.Forms.DataGridViewTextBoxColumn modDirectory;
        private Styles.Controls.EnhancedLabel item_ModDescription;
        private AnimatedButton box_DescriptionBox;
        private AnimatedButton item_QuitBox;
    }
}