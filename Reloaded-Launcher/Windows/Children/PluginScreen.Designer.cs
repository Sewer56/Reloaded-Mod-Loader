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

using Reloaded_GUI.Styles.Controls.Animated;

namespace ReloadedLauncher.Windows.Children
{
    partial class PluginScreen
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
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties1 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage1 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage2 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginScreen));
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties2 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage3 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage4 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties3 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage5 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage6 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties4 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage7 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage8 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties5 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage9 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage10 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimProperties animProperties6 = new Reloaded_GUI.Styles.Animation.AnimProperties();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage11 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            Reloaded_GUI.Styles.Animation.AnimMessage animMessage12 = new Reloaded_GUI.Styles.Animation.AnimMessage();
            this.borderless_InfoBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_WebBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_ConfigBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.box_PluginList = new Reloaded_GUI.Styles.Controls.Animated.AnimatedDataGridView();
            this.Enabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modDirectory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.box_PluginListBox = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            this.borderless_LauncherRestartNote = new Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel();
            this.borderless_ModDescription = new Reloaded_GUI.Styles.Controls.Animated.AnimatedButton();
            ((System.ComponentModel.ISupportInitialize)(this.box_PluginList)).BeginInit();
            this.SuspendLayout();
            // 
            // borderless_InfoBox
            // 
            animMessage1.Control = this.borderless_InfoBox;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.borderless_InfoBox;
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
            this.borderless_InfoBox.AnimProperties = animProperties1;
            this.borderless_InfoBox.CaptureChildren = false;
            this.borderless_InfoBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_InfoBox.FlatAppearance.BorderSize = 0;
            this.borderless_InfoBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_InfoBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_InfoBox.ForeColor = System.Drawing.Color.White;
            this.borderless_InfoBox.IgnoreMouse = false;
            this.borderless_InfoBox.IgnoreMouseClicks = false;
            this.borderless_InfoBox.Image = ((System.Drawing.Image)(resources.GetObject("borderless_InfoBox.Image")));
            this.borderless_InfoBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_InfoBox.Location = new System.Drawing.Point(609, 429);
            this.borderless_InfoBox.Name = "borderless_InfoBox";
            this.borderless_InfoBox.Size = new System.Drawing.Size(253, 50);
            this.borderless_InfoBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_InfoBox.TabIndex = 20;
            this.borderless_InfoBox.Text = "Creating Plugins";
            this.borderless_InfoBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.borderless_InfoBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.borderless_InfoBox.UseVisualStyleBackColor = true;
            this.borderless_InfoBox.Click += new System.EventHandler(this.InfoBox_Click);
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
            animProperties2.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties2.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties2.MouseLeaveDuration = 0F;
            animProperties2.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties2.MouseLeaveFramerate = 0F;
            animProperties2.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_WebBox.AnimProperties = animProperties2;
            this.borderless_WebBox.CaptureChildren = false;
            this.borderless_WebBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_WebBox.FlatAppearance.BorderSize = 0;
            this.borderless_WebBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_WebBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_WebBox.ForeColor = System.Drawing.Color.White;
            this.borderless_WebBox.IgnoreMouse = false;
            this.borderless_WebBox.IgnoreMouseClicks = false;
            this.borderless_WebBox.Image = ((System.Drawing.Image)(resources.GetObject("borderless_WebBox.Image")));
            this.borderless_WebBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_WebBox.Location = new System.Drawing.Point(323, 429);
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
            animProperties3.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties3.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties3.MouseLeaveDuration = 0F;
            animProperties3.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties3.MouseLeaveFramerate = 0F;
            animProperties3.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_ConfigBox.AnimProperties = animProperties3;
            this.borderless_ConfigBox.CaptureChildren = false;
            this.borderless_ConfigBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_ConfigBox.FlatAppearance.BorderSize = 0;
            this.borderless_ConfigBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_ConfigBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_ConfigBox.ForeColor = System.Drawing.Color.White;
            this.borderless_ConfigBox.IgnoreMouse = false;
            this.borderless_ConfigBox.IgnoreMouseClicks = false;
            this.borderless_ConfigBox.Image = ((System.Drawing.Image)(resources.GetObject("borderless_ConfigBox.Image")));
            this.borderless_ConfigBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderless_ConfigBox.Location = new System.Drawing.Point(40, 429);
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
            // box_PluginList
            // 
            this.box_PluginList.AllowDrop = true;
            this.box_PluginList.AllowUserToAddRows = false;
            this.box_PluginList.AllowUserToDeleteRows = false;
            this.box_PluginList.AllowUserToResizeColumns = false;
            this.box_PluginList.AllowUserToResizeRows = false;
            animMessage7.Control = this.box_PluginList;
            animMessage7.PlayAnimation = true;
            animProperties4.BackColorMessage = animMessage7;
            animMessage8.Control = this.box_PluginList;
            animMessage8.PlayAnimation = true;
            animProperties4.ForeColorMessage = animMessage8;
            animProperties4.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties4.MouseEnterDuration = 0F;
            animProperties4.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties4.MouseEnterFramerate = 0F;
            animProperties4.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties4.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties4.MouseLeaveDuration = 0F;
            animProperties4.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties4.MouseLeaveFramerate = 0F;
            animProperties4.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.box_PluginList.AnimProperties = animProperties4;
            this.box_PluginList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.box_PluginList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.box_PluginList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.box_PluginList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.box_PluginList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.box_PluginList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.box_PluginList.ColumnHeadersVisible = false;
            this.box_PluginList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Enabled,
            this.modName,
            this.Description,
            this.Author,
            this.modDirectory});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.box_PluginList.DefaultCellStyle = dataGridViewCellStyle6;
            this.box_PluginList.DragRowIndex = 0;
            this.box_PluginList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.box_PluginList.EnableHeadersVisualStyles = false;
            this.box_PluginList.GridColor = System.Drawing.Color.White;
            this.box_PluginList.Location = new System.Drawing.Point(41, 30);
            this.box_PluginList.MultiSelect = false;
            this.box_PluginList.Name = "box_PluginList";
            this.box_PluginList.ReadOnly = true;
            this.box_PluginList.ReorderingEnabled = false;
            this.box_PluginList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.box_PluginList.RowHeadersVisible = false;
            this.box_PluginList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.box_PluginList.RowTemplate.Height = 30;
            this.box_PluginList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.box_PluginList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.box_PluginList.ShowCellToolTips = false;
            this.box_PluginList.Size = new System.Drawing.Size(820, 309);
            this.box_PluginList.StandardTab = true;
            this.box_PluginList.TabIndex = 14;
            this.box_PluginList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PluginList_CellClick);
            this.box_PluginList.SelectionChanged += new System.EventHandler(this.PluginList_SelectionChanged);
            // 
            // Enabled
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Enabled.DefaultCellStyle = dataGridViewCellStyle1;
            this.Enabled.HeaderText = "Enabled";
            this.Enabled.Name = "Enabled";
            this.Enabled.ReadOnly = true;
            this.Enabled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Enabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Enabled.Width = 50;
            // 
            // modName
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.modName.DefaultCellStyle = dataGridViewCellStyle2;
            this.modName.HeaderText = "Name";
            this.modName.Name = "modName";
            this.modName.ReadOnly = true;
            this.modName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.modName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.modName.Width = 225;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Description.DefaultCellStyle = dataGridViewCellStyle3;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Author
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Author.DefaultCellStyle = dataGridViewCellStyle4;
            this.Author.HeaderText = "Author";
            this.Author.Name = "Author";
            this.Author.ReadOnly = true;
            this.Author.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Author.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Author.Width = 160;
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
            this.modDirectory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.modDirectory.Width = 60;
            // 
            // box_PluginListBox
            // 
            animMessage9.Control = this.box_PluginListBox;
            animMessage9.PlayAnimation = true;
            animProperties5.BackColorMessage = animMessage9;
            animMessage10.Control = this.box_PluginListBox;
            animMessage10.PlayAnimation = true;
            animProperties5.ForeColorMessage = animMessage10;
            animProperties5.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties5.MouseEnterDuration = 0F;
            animProperties5.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties5.MouseEnterFramerate = 0F;
            animProperties5.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties5.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties5.MouseLeaveDuration = 0F;
            animProperties5.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties5.MouseLeaveFramerate = 0F;
            animProperties5.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.box_PluginListBox.AnimProperties = animProperties5;
            this.box_PluginListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_PluginListBox.CaptureChildren = false;
            this.box_PluginListBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.box_PluginListBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_PluginListBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.box_PluginListBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.box_PluginListBox.IgnoreMouse = true;
            this.box_PluginListBox.IgnoreMouseClicks = true;
            this.box_PluginListBox.Location = new System.Drawing.Point(40, 29);
            this.box_PluginListBox.Margin = new System.Windows.Forms.Padding(0);
            this.box_PluginListBox.Name = "box_PluginListBox";
            this.box_PluginListBox.Size = new System.Drawing.Size(822, 311);
            this.box_PluginListBox.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.box_PluginListBox.TabIndex = 13;
            this.box_PluginListBox.Text = null;
            this.box_PluginListBox.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.box_PluginListBox.UseVisualStyleBackColor = false;
            // 
            // borderless_LauncherRestartNote
            // 
            this.borderless_LauncherRestartNote.BackColor = System.Drawing.Color.Transparent;
            this.borderless_LauncherRestartNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_LauncherRestartNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_LauncherRestartNote.ForeColor = System.Drawing.Color.White;
            this.borderless_LauncherRestartNote.IgnoreMouse = false;
            this.borderless_LauncherRestartNote.Location = new System.Drawing.Point(37, 386);
            this.borderless_LauncherRestartNote.Margin = new System.Windows.Forms.Padding(0);
            this.borderless_LauncherRestartNote.Name = "borderless_LauncherRestartNote";
            this.borderless_LauncherRestartNote.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.borderless_LauncherRestartNote.Size = new System.Drawing.Size(826, 24);
            this.borderless_LauncherRestartNote.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_LauncherRestartNote.TabIndex = 21;
            this.borderless_LauncherRestartNote.Text = "Reloading Plugins Requires a Launcher Restart";
            this.borderless_LauncherRestartNote.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.borderless_LauncherRestartNote.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // borderless_ModDescription
            // 
            animMessage11.Control = this.borderless_ModDescription;
            animMessage11.PlayAnimation = true;
            animProperties6.BackColorMessage = animMessage11;
            animMessage12.Control = this.borderless_ModDescription;
            animMessage12.PlayAnimation = true;
            animProperties6.ForeColorMessage = animMessage12;
            animProperties6.MouseEnterBackColor = System.Drawing.Color.Empty;
            animProperties6.MouseEnterDuration = 0F;
            animProperties6.MouseEnterForeColor = System.Drawing.Color.Empty;
            animProperties6.MouseEnterFramerate = 0F;
            animProperties6.MouseEnterOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseEnterOverride.None;
            animProperties6.MouseLeaveBackColor = System.Drawing.Color.Empty;
            animProperties6.MouseLeaveDuration = 0F;
            animProperties6.MouseLeaveForeColor = System.Drawing.Color.Empty;
            animProperties6.MouseLeaveFramerate = 0F;
            animProperties6.MouseLeaveOverride = Reloaded_GUI.Styles.Animation.AnimOverrides.MouseLeaveOverride.None;
            this.borderless_ModDescription.AnimProperties = animProperties6;
            this.borderless_ModDescription.BackColor = System.Drawing.Color.Transparent;
            this.borderless_ModDescription.CaptureChildren = true;
            this.borderless_ModDescription.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_ModDescription.FlatAppearance.BorderSize = 0;
            this.borderless_ModDescription.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_ModDescription.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.borderless_ModDescription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_ModDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_ModDescription.ForeColor = System.Drawing.Color.White;
            this.borderless_ModDescription.IgnoreMouse = false;
            this.borderless_ModDescription.IgnoreMouseClicks = false;
            this.borderless_ModDescription.Location = new System.Drawing.Point(40, 351);
            this.borderless_ModDescription.Name = "borderless_ModDescription";
            this.borderless_ModDescription.Size = new System.Drawing.Size(822, 29);
            this.borderless_ModDescription.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.borderless_ModDescription.TabIndex = 48;
            this.borderless_ModDescription.Text = "Hello World!";
            this.borderless_ModDescription.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.borderless_ModDescription.UseVisualStyleBackColor = false;
            // 
            // PluginScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(900, 512);
            this.Controls.Add(this.borderless_ModDescription);
            this.Controls.Add(this.borderless_LauncherRestartNote);
            this.Controls.Add(this.borderless_InfoBox);
            this.Controls.Add(this.borderless_WebBox);
            this.Controls.Add(this.borderless_ConfigBox);
            this.Controls.Add(this.box_PluginList);
            this.Controls.Add(this.box_PluginListBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PluginScreen";
            this.Text = "Main_Screen";
            this.VisibleChanged += new System.EventHandler(this.MenuVisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.box_PluginList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Reloaded_GUI.Styles.Controls.Animated.AnimatedButton box_PluginListBox;
        private AnimatedDataGridView box_PluginList;
        public AnimatedButton borderless_ConfigBox;
        public AnimatedButton borderless_WebBox;
        public AnimatedButton borderless_InfoBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Enabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn modName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Author;
        private System.Windows.Forms.DataGridViewTextBoxColumn modDirectory;
        private Reloaded_GUI.Styles.Controls.Enhanced.EnhancedLabel borderless_LauncherRestartNote;
        private AnimatedButton borderless_ModDescription;
    }
}