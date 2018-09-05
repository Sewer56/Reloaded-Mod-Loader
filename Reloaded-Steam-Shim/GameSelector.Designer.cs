namespace Reloaded_Steam_Shim
{
    partial class GameSelector
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
            this.item_GameList = new System.Windows.Forms.ListView();
            this.item_LaunchGame = new System.Windows.Forms.Button();
            this.item_RememberThisBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // item_GameList
            // 
            this.item_GameList.Location = new System.Drawing.Point(12, 12);
            this.item_GameList.MultiSelect = false;
            this.item_GameList.Name = "item_GameList";
            this.item_GameList.Size = new System.Drawing.Size(260, 164);
            this.item_GameList.TabIndex = 0;
            this.item_GameList.UseCompatibleStateImageBehavior = false;
            this.item_GameList.View = System.Windows.Forms.View.List;
            // 
            // item_LaunchGame
            // 
            this.item_LaunchGame.Location = new System.Drawing.Point(13, 212);
            this.item_LaunchGame.Name = "item_LaunchGame";
            this.item_LaunchGame.Size = new System.Drawing.Size(259, 29);
            this.item_LaunchGame.TabIndex = 1;
            this.item_LaunchGame.Text = "Launch via Reloaded";
            this.item_LaunchGame.UseVisualStyleBackColor = true;
            this.item_LaunchGame.Click += new System.EventHandler(this.item_LaunchGame_Click);
            // 
            // item_RememberThisBox
            // 
            this.item_RememberThisBox.AutoSize = true;
            this.item_RememberThisBox.Location = new System.Drawing.Point(16, 184);
            this.item_RememberThisBox.Name = "item_RememberThisBox";
            this.item_RememberThisBox.Size = new System.Drawing.Size(136, 17);
            this.item_RememberThisBox.TabIndex = 2;
            this.item_RememberThisBox.Text = "Remember This Choice";
            this.item_RememberThisBox.UseVisualStyleBackColor = true;
            // 
            // GameSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 253);
            this.Controls.Add(this.item_RememberThisBox);
            this.Controls.Add(this.item_LaunchGame);
            this.Controls.Add(this.item_GameList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GameSelector";
            this.Text = "Reloaded Steam Shim";
            this.Load += new System.EventHandler(this.GameSelector_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView item_GameList;
        private System.Windows.Forms.Button item_LaunchGame;
        private System.Windows.Forms.CheckBox item_RememberThisBox;
    }
}

