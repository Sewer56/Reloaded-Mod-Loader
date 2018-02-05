using ReloadedLauncher.Styles.Controls.Animated;

namespace ReloadedLauncher.Windows.Children
{
    partial class Theme_Screen
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
            ReloadedLauncher.Styles.Animation.AnimProperties animProperties1 = new ReloadedLauncher.Styles.Animation.AnimProperties();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage1 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage2 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Theme_Screen));
            ReloadedLauncher.Styles.Animation.AnimProperties animProperties2 = new ReloadedLauncher.Styles.Animation.AnimProperties();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage3 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage4 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            ReloadedLauncher.Styles.Animation.AnimProperties animProperties3 = new ReloadedLauncher.Styles.Animation.AnimProperties();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage5 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage6 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            ReloadedLauncher.Styles.Animation.AnimProperties animProperties4 = new ReloadedLauncher.Styles.Animation.AnimProperties();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage7 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage8 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            ReloadedLauncher.Styles.Animation.AnimProperties animProperties5 = new ReloadedLauncher.Styles.Animation.AnimProperties();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage9 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage10 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            this.borderless_SourceBox = new ReloadedLauncher.Styles.Controls.Animated.AnimatedButton();
            this.borderless_WebBox = new ReloadedLauncher.Styles.Controls.Animated.AnimatedButton();
            this.borderless_ConfigBox = new ReloadedLauncher.Styles.Controls.Animated.AnimatedButton();
            this.box_ThemeList = new ReloadedLauncher.Styles.Controls.Animated.AnimatedDataGridView();
            this.modName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.separator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modDirectory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.box_ThemeListBox = new ReloadedLauncher.Styles.Controls.Animated.AnimatedButton();
            ((System.ComponentModel.ISupportInitialize)(this.box_ThemeList)).BeginInit();
            this.SuspendLayout();
            // 
            // borderless_SourceBox
            // 
            animMessage1.Control = this.borderless_SourceBox;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.borderless_SourceBox;
            animMessage2.PlayAnimation = true;
            animProperties1.ForeColorMessage = animMessage2;
            animProperties1.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties1.MouseEnterDuration = 0F;
            animProperties1.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties1.MouseEnterFramerate = 0F;
            animProperties1.MouseEnterOverride = ReloadedLauncher.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties1.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties1.MouseLeaveDuration = 0F;
            animProperties1.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties1.MouseLeaveFramerate = 0F;
            animProperties1.MouseLeaveOverride = ReloadedLauncher.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_SourceBox.AnimProperties = animProperties1;
            this.borderless_SourceBox.CaptureChildren = false;
            this.borderless_SourceBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_SourceBox.FlatAppearance.BorderSize = 0;
            this.borderless_SourceBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_SourceBox.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_SourceBox.ForeColor = System.Drawing.Color.White;
            this.borderless_SourceBox.IgnoreMouse = false;
            this.borderless_SourceBox.IgnoreMouseClicks = false;
            this.borderless_SourceBox.Image = ((System.Drawing.Image)(resources.GetObject("borderless_SourceBox.Image")));
            this.borderless_SourceBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_SourceBox.Location = new System.Drawing.Point(609, 439);
            this.borderless_SourceBox.Name = "borderless_SourceBox";
            this.borderless_SourceBox.Size = new System.Drawing.Size(253, 50);
            this.borderless_SourceBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_SourceBox.TabIndex = 20;
            this.borderless_SourceBox.Text = "Github";
            this.borderless_SourceBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.borderless_SourceBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.borderless_SourceBox.UseVisualStyleBackColor = true;
            this.borderless_SourceBox.Click += new System.EventHandler(this.SourceBox_Click);
            // 
            // borderless_WebBox
            // 
            animMessage3.Control = this.borderless_WebBox;
            animMessage3.PlayAnimation = true;
            animProperties2.BackColorMessage = animMessage3;
            animMessage4.Control = this.borderless_WebBox;
            animMessage4.PlayAnimation = true;
            animProperties2.ForeColorMessage = animMessage4;
            animProperties2.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties2.MouseEnterDuration = 0F;
            animProperties2.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties2.MouseEnterFramerate = 0F;
            animProperties2.MouseEnterOverride = ReloadedLauncher.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties2.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties2.MouseLeaveDuration = 0F;
            animProperties2.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties2.MouseLeaveFramerate = 0F;
            animProperties2.MouseLeaveOverride = ReloadedLauncher.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_WebBox.AnimProperties = animProperties2;
            this.borderless_WebBox.CaptureChildren = false;
            this.borderless_WebBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_WebBox.FlatAppearance.BorderSize = 0;
            this.borderless_WebBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_WebBox.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_WebBox.ForeColor = System.Drawing.Color.White;
            this.borderless_WebBox.IgnoreMouse = false;
            this.borderless_WebBox.IgnoreMouseClicks = false;
            this.borderless_WebBox.Image = ((System.Drawing.Image)(resources.GetObject("borderless_WebBox.Image")));
            this.borderless_WebBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_WebBox.Location = new System.Drawing.Point(323, 439);
            this.borderless_WebBox.Name = "borderless_WebBox";
            this.borderless_WebBox.Size = new System.Drawing.Size(253, 50);
            this.borderless_WebBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_WebBox.TabIndex = 19;
            this.borderless_WebBox.Text = "Webpage";
            this.borderless_WebBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.borderless_WebBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.borderless_WebBox.UseVisualStyleBackColor = true;
            this.borderless_WebBox.Click += new System.EventHandler(this.WebBox_Click);
            // 
            // borderless_ConfigBox
            // 
            animMessage5.Control = this.borderless_ConfigBox;
            animMessage5.PlayAnimation = true;
            animProperties3.BackColorMessage = animMessage5;
            animMessage6.Control = this.borderless_ConfigBox;
            animMessage6.PlayAnimation = true;
            animProperties3.ForeColorMessage = animMessage6;
            animProperties3.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties3.MouseEnterDuration = 0F;
            animProperties3.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties3.MouseEnterFramerate = 0F;
            animProperties3.MouseEnterOverride = ReloadedLauncher.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties3.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties3.MouseLeaveDuration = 0F;
            animProperties3.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties3.MouseLeaveFramerate = 0F;
            animProperties3.MouseLeaveOverride = ReloadedLauncher.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_ConfigBox.AnimProperties = animProperties3;
            this.borderless_ConfigBox.CaptureChildren = false;
            this.borderless_ConfigBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_ConfigBox.FlatAppearance.BorderSize = 0;
            this.borderless_ConfigBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_ConfigBox.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_ConfigBox.ForeColor = System.Drawing.Color.White;
            this.borderless_ConfigBox.IgnoreMouse = false;
            this.borderless_ConfigBox.IgnoreMouseClicks = false;
            this.borderless_ConfigBox.Image = ((System.Drawing.Image)(resources.GetObject("borderless_ConfigBox.Image")));
            this.borderless_ConfigBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_ConfigBox.Location = new System.Drawing.Point(40, 439);
            this.borderless_ConfigBox.Name = "borderless_ConfigBox";
            this.borderless_ConfigBox.Size = new System.Drawing.Size(253, 50);
            this.borderless_ConfigBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_ConfigBox.TabIndex = 17;
            this.borderless_ConfigBox.Text = "Configuration";
            this.borderless_ConfigBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.borderless_ConfigBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.borderless_ConfigBox.UseVisualStyleBackColor = true;
            this.borderless_ConfigBox.Click += new System.EventHandler(this.ConfigBox_Click);
            // 
            // box_ThemeList
            // 
            this.box_ThemeList.AllowDrop = true;
            this.box_ThemeList.AllowUserToAddRows = false;
            this.box_ThemeList.AllowUserToDeleteRows = false;
            this.box_ThemeList.AllowUserToResizeColumns = false;
            this.box_ThemeList.AllowUserToResizeRows = false;
            animMessage7.Control = this.box_ThemeList;
            animMessage7.PlayAnimation = true;
            animProperties4.BackColorMessage = animMessage7;
            animMessage8.Control = this.box_ThemeList;
            animMessage8.PlayAnimation = true;
            animProperties4.ForeColorMessage = animMessage8;
            animProperties4.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties4.MouseEnterDuration = 0F;
            animProperties4.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties4.MouseEnterFramerate = 0F;
            animProperties4.MouseEnterOverride = ReloadedLauncher.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties4.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties4.MouseLeaveDuration = 0F;
            animProperties4.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties4.MouseLeaveFramerate = 0F;
            animProperties4.MouseLeaveOverride = ReloadedLauncher.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.box_ThemeList.AnimProperties = animProperties4;
            this.box_ThemeList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.box_ThemeList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.box_ThemeList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.box_ThemeList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.box_ThemeList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.box_ThemeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.box_ThemeList.ColumnHeadersVisible = false;
            this.box_ThemeList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.modName,
            this.Description,
            this.Author,
            this.separator,
            this.modDirectory});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Roboto Mono", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.box_ThemeList.DefaultCellStyle = dataGridViewCellStyle5;
            this.box_ThemeList.DragRowIndex = 0;
            this.box_ThemeList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.box_ThemeList.EnableHeadersVisualStyles = false;
            this.box_ThemeList.GridColor = System.Drawing.Color.White;
            this.box_ThemeList.Location = new System.Drawing.Point(41, 40);
            this.box_ThemeList.MultiSelect = false;
            this.box_ThemeList.Name = "box_ThemeList";
            this.box_ThemeList.ReadOnly = true;
            this.box_ThemeList.ReorderingEnabled = false;
            this.box_ThemeList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.box_ThemeList.RowHeadersVisible = false;
            this.box_ThemeList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.box_ThemeList.RowTemplate.Height = 30;
            this.box_ThemeList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.box_ThemeList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.box_ThemeList.Size = new System.Drawing.Size(820, 377);
            this.box_ThemeList.StandardTab = true;
            this.box_ThemeList.TabIndex = 14;
            this.box_ThemeList.SelectionChanged += new System.EventHandler(this.ModList_SelectionChanged);
            // 
            // modName
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.modName.DefaultCellStyle = dataGridViewCellStyle1;
            this.modName.FillWeight = 192.7388F;
            this.modName.HeaderText = "Name";
            this.modName.Name = "modName";
            this.modName.ReadOnly = true;
            this.modName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.modName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.modName.Width = 280;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Description.Width = 330;
            // 
            // Author
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.Author.DefaultCellStyle = dataGridViewCellStyle2;
            this.Author.FillWeight = 126.5651F;
            this.Author.HeaderText = "Author";
            this.Author.Name = "Author";
            this.Author.ReadOnly = true;
            this.Author.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Author.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Author.Width = 136;
            // 
            // separator
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.separator.DefaultCellStyle = dataGridViewCellStyle3;
            this.separator.HeaderText = "Separator";
            this.separator.Name = "separator";
            this.separator.ReadOnly = true;
            this.separator.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.separator.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.separator.Width = 20;
            // 
            // modDirectory
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.modDirectory.DefaultCellStyle = dataGridViewCellStyle4;
            this.modDirectory.FillWeight = 60.39148F;
            this.modDirectory.HeaderText = "Version";
            this.modDirectory.Name = "modDirectory";
            this.modDirectory.ReadOnly = true;
            this.modDirectory.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.modDirectory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.modDirectory.Width = 60;
            // 
            // box_ThemeListBox
            // 
            animMessage9.Control = this.box_ThemeListBox;
            animMessage9.PlayAnimation = true;
            animProperties5.BackColorMessage = animMessage9;
            animMessage10.Control = this.box_ThemeListBox;
            animMessage10.PlayAnimation = true;
            animProperties5.ForeColorMessage = animMessage10;
            animProperties5.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties5.MouseEnterDuration = 0F;
            animProperties5.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties5.MouseEnterFramerate = 0F;
            animProperties5.MouseEnterOverride = ReloadedLauncher.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties5.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties5.MouseLeaveDuration = 0F;
            animProperties5.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties5.MouseLeaveFramerate = 0F;
            animProperties5.MouseLeaveOverride = ReloadedLauncher.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.box_ThemeListBox.AnimProperties = animProperties5;
            this.box_ThemeListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_ThemeListBox.CaptureChildren = false;
            this.box_ThemeListBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.box_ThemeListBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_ThemeListBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_ThemeListBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.box_ThemeListBox.IgnoreMouse = true;
            this.box_ThemeListBox.IgnoreMouseClicks = true;
            this.box_ThemeListBox.Location = new System.Drawing.Point(40, 39);
            this.box_ThemeListBox.Margin = new System.Windows.Forms.Padding(0);
            this.box_ThemeListBox.Name = "box_ThemeListBox";
            this.box_ThemeListBox.Size = new System.Drawing.Size(822, 379);
            this.box_ThemeListBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.box_ThemeListBox.TabIndex = 13;
            this.box_ThemeListBox.Text = null;
            this.box_ThemeListBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.box_ThemeListBox.UseVisualStyleBackColor = false;
            // 
            // Theme_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(900, 512);
            this.Controls.Add(this.borderless_SourceBox);
            this.Controls.Add(this.borderless_WebBox);
            this.Controls.Add(this.borderless_ConfigBox);
            this.Controls.Add(this.box_ThemeList);
            this.Controls.Add(this.box_ThemeListBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Theme_Screen";
            this.Text = "Main_Screen";
            this.VisibleChanged += new System.EventHandler(this.MenuVisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.box_ThemeList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Styles.Controls.Animated.AnimatedButton box_ThemeListBox;
        private AnimatedDataGridView box_ThemeList;
        public AnimatedButton borderless_ConfigBox;
        public AnimatedButton borderless_WebBox;
        public AnimatedButton borderless_SourceBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn modName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Author;
        private System.Windows.Forms.DataGridViewTextBoxColumn separator;
        private System.Windows.Forms.DataGridViewTextBoxColumn modDirectory;
    }
}