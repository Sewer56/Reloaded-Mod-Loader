using ReloadedLauncher.Styles.Controls.Animated;

namespace ReloadedLauncher.Windows.Children
{
    partial class Input_Screen
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
            this.components = new System.ComponentModel.Container();
            ReloadedLauncher.Styles.Animation.AnimProperties animProperties1 = new ReloadedLauncher.Styles.Animation.AnimProperties();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage1 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            ReloadedLauncher.Styles.Animation.AnimMessage animMessage2 = new ReloadedLauncher.Styles.Animation.AnimMessage();
            System.Drawing.StringFormat stringFormat1 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat2 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat3 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat4 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat5 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat6 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat7 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat8 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat9 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat10 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat11 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat12 = new System.Drawing.StringFormat();
            this.borderless_CurrentController = new ReloadedLauncher.Styles.Controls.Animated.AnimatedComboBox();
            this.toolTipHelper = new System.Windows.Forms.ToolTip(this.components);
            this.borderless_RightStick = new ReloadedLauncher.Styles.Controls.Custom.CustomAnalogStickIndicator();
            this.borderless_LeftStick = new ReloadedLauncher.Styles.Controls.Custom.CustomAnalogStickIndicator();
            this.borderless_LeftTrigger = new ReloadedLauncher.Styles.Controls.Custom.CustomVerticalProgressBar();
            this.borderless_RightTrigger = new ReloadedLauncher.Styles.Controls.Custom.CustomVerticalProgressBar();
            this.borderless_ButtonA = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonB = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonX = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonR = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonY = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonL = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonRIGHT = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonLEFT = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonDOWN = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonUP = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonSELECT = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.borderless_ButtonSTART = new ReloadedLauncher.Styles.Controls.Custom.CustomControllerButtonPressIndicator();
            this.SuspendLayout();
            // 
            // borderless_CurrentController
            // 
            animMessage1.Control = this.borderless_CurrentController;
            animMessage1.PlayAnimation = true;
            animProperties1.BackColorMessage = animMessage1;
            animMessage2.Control = this.borderless_CurrentController;
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
            this.borderless_CurrentController.AnimProperties = animProperties1;
            this.borderless_CurrentController.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.borderless_CurrentController.BottomBorderColour = System.Drawing.Color.White;
            this.borderless_CurrentController.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_CurrentController.BottomBorderWidth = 2;
            this.borderless_CurrentController.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.borderless_CurrentController.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.borderless_CurrentController.DropDownArrowColour = System.Drawing.Color.White;
            this.borderless_CurrentController.DropDownButtonColour = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.borderless_CurrentController.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.borderless_CurrentController.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.borderless_CurrentController.Font = new System.Drawing.Font("Roboto Mono", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.borderless_CurrentController.ForeColor = System.Drawing.Color.White;
            this.borderless_CurrentController.FormattingEnabled = true;
            this.borderless_CurrentController.HighlightColor = System.Drawing.Color.Red;
            this.borderless_CurrentController.Items.AddRange(new object[] {
            "Sonic Heroes",
            "Sonic Adventure 2",
            "Sonic Adventure DX",
            "Sonic Riders",
            "Sora no Kiseki SC"});
            this.borderless_CurrentController.LeftBorderColour = System.Drawing.Color.Empty;
            this.borderless_CurrentController.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_CurrentController.LeftBorderWidth = 0;
            this.borderless_CurrentController.Location = new System.Drawing.Point(591, 40);
            this.borderless_CurrentController.Name = "borderless_CurrentController";
            this.borderless_CurrentController.RightBorderColour = System.Drawing.Color.Empty;
            this.borderless_CurrentController.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            this.borderless_CurrentController.RightBorderWidth = 0;
            this.borderless_CurrentController.Size = new System.Drawing.Size(271, 32);
            this.borderless_CurrentController.TabIndex = 3;
            this.borderless_CurrentController.TopBorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.borderless_CurrentController.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_CurrentController.TopBorderWidth = 2;
            // 
            // borderless_RightStick
            // 
            this.borderless_RightStick.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_RightStick.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_RightStick.BottomBorderWidth = 1;
            this.borderless_RightStick.ForeColor = System.Drawing.Color.White;
            this.borderless_RightStick.IndicatorColour = System.Drawing.Color.White;
            this.borderless_RightStick.IndicatorWidth = 4;
            this.borderless_RightStick.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_RightStick.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_RightStick.LeftBorderWidth = 1;
            this.borderless_RightStick.Location = new System.Drawing.Point(772, 91);
            this.borderless_RightStick.Name = "borderless_RightStick";
            this.borderless_RightStick.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_RightStick.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_RightStick.RightBorderWidth = 1;
            this.borderless_RightStick.Size = new System.Drawing.Size(90, 90);
            this.borderless_RightStick.TabIndex = 5;
            this.borderless_RightStick.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_RightStick.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_RightStick.TopBorderWidth = 1;
            this.borderless_RightStick.ValueX = 500;
            this.borderless_RightStick.ValueY = 500;
            // 
            // borderless_LeftStick
            // 
            this.borderless_LeftStick.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_LeftStick.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_LeftStick.BottomBorderWidth = 1;
            this.borderless_LeftStick.ForeColor = System.Drawing.Color.White;
            this.borderless_LeftStick.IndicatorColour = System.Drawing.Color.White;
            this.borderless_LeftStick.IndicatorWidth = 4;
            this.borderless_LeftStick.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_LeftStick.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_LeftStick.LeftBorderWidth = 1;
            this.borderless_LeftStick.Location = new System.Drawing.Point(591, 91);
            this.borderless_LeftStick.Name = "borderless_LeftStick";
            this.borderless_LeftStick.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_LeftStick.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_LeftStick.RightBorderWidth = 1;
            this.borderless_LeftStick.Size = new System.Drawing.Size(90, 90);
            this.borderless_LeftStick.TabIndex = 4;
            this.borderless_LeftStick.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_LeftStick.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_LeftStick.TopBorderWidth = 1;
            this.borderless_LeftStick.ValueX = 500;
            this.borderless_LeftStick.ValueY = 500;
            // 
            // borderless_LeftTrigger
            // 
            this.borderless_LeftTrigger.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_LeftTrigger.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_LeftTrigger.BottomBorderWidth = 1;
            this.borderless_LeftTrigger.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_LeftTrigger.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_LeftTrigger.LeftBorderWidth = 1;
            this.borderless_LeftTrigger.Location = new System.Drawing.Point(696, 91);
            this.borderless_LeftTrigger.Name = "borderless_LeftTrigger";
            this.borderless_LeftTrigger.ProgressColour = System.Drawing.Color.White;
            this.borderless_LeftTrigger.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_LeftTrigger.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_LeftTrigger.RightBorderWidth = 1;
            this.borderless_LeftTrigger.Size = new System.Drawing.Size(15, 90);
            this.borderless_LeftTrigger.TabIndex = 6;
            this.borderless_LeftTrigger.Text = "customVerticalProgressBar1";
            this.borderless_LeftTrigger.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_LeftTrigger.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_LeftTrigger.TopBorderWidth = 1;
            this.borderless_LeftTrigger.Value = 333;
            // 
            // borderless_RightTrigger
            // 
            this.borderless_RightTrigger.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_RightTrigger.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_RightTrigger.BottomBorderWidth = 1;
            this.borderless_RightTrigger.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_RightTrigger.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_RightTrigger.LeftBorderWidth = 1;
            this.borderless_RightTrigger.Location = new System.Drawing.Point(742, 91);
            this.borderless_RightTrigger.Name = "borderless_RightTrigger";
            this.borderless_RightTrigger.ProgressColour = System.Drawing.Color.White;
            this.borderless_RightTrigger.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_RightTrigger.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_RightTrigger.RightBorderWidth = 1;
            this.borderless_RightTrigger.Size = new System.Drawing.Size(15, 90);
            this.borderless_RightTrigger.TabIndex = 7;
            this.borderless_RightTrigger.Text = "customVerticalProgressBar1";
            this.borderless_RightTrigger.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_RightTrigger.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_RightTrigger.TopBorderWidth = 1;
            this.borderless_RightTrigger.Value = 666;
            // 
            // borderless_ButtonA
            // 
            this.borderless_ButtonA.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonA.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonA.BottomBorderWidth = 1;
            this.borderless_ButtonA.ButtonEnabled = false;
            this.borderless_ButtonA.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonA.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonA.LeftBorderWidth = 1;
            this.borderless_ButtonA.Location = new System.Drawing.Point(591, 200);
            this.borderless_ButtonA.Name = "borderless_ButtonA";
            this.borderless_ButtonA.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonA.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonA.RightBorderWidth = 1;
            this.borderless_ButtonA.Size = new System.Drawing.Size(37, 37);
            stringFormat1.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat1.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonA.StringFormat = stringFormat1;
            this.borderless_ButtonA.TabIndex = 8;
            this.borderless_ButtonA.Text = "A";
            this.borderless_ButtonA.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonA.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonA.TopBorderWidth = 1;
            // 
            // borderless_ButtonB
            // 
            this.borderless_ButtonB.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonB.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonB.BottomBorderWidth = 1;
            this.borderless_ButtonB.ButtonEnabled = false;
            this.borderless_ButtonB.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonB.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonB.LeftBorderWidth = 1;
            this.borderless_ButtonB.Location = new System.Drawing.Point(637, 200);
            this.borderless_ButtonB.Name = "borderless_ButtonB";
            this.borderless_ButtonB.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonB.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonB.RightBorderWidth = 1;
            this.borderless_ButtonB.Size = new System.Drawing.Size(37, 37);
            stringFormat2.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat2.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat2.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat2.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonB.StringFormat = stringFormat2;
            this.borderless_ButtonB.TabIndex = 9;
            this.borderless_ButtonB.Text = "B";
            this.borderless_ButtonB.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonB.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonB.TopBorderWidth = 1;
            // 
            // borderless_ButtonX
            // 
            this.borderless_ButtonX.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonX.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonX.BottomBorderWidth = 1;
            this.borderless_ButtonX.ButtonEnabled = false;
            this.borderless_ButtonX.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonX.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonX.LeftBorderWidth = 1;
            this.borderless_ButtonX.Location = new System.Drawing.Point(683, 200);
            this.borderless_ButtonX.Name = "borderless_ButtonX";
            this.borderless_ButtonX.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonX.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonX.RightBorderWidth = 1;
            this.borderless_ButtonX.Size = new System.Drawing.Size(37, 37);
            stringFormat3.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat3.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat3.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat3.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonX.StringFormat = stringFormat3;
            this.borderless_ButtonX.TabIndex = 11;
            this.borderless_ButtonX.Text = "X";
            this.borderless_ButtonX.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonX.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonX.TopBorderWidth = 1;
            // 
            // borderless_ButtonR
            // 
            this.borderless_ButtonR.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonR.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonR.BottomBorderWidth = 1;
            this.borderless_ButtonR.ButtonEnabled = false;
            this.borderless_ButtonR.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonR.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonR.LeftBorderWidth = 1;
            this.borderless_ButtonR.Location = new System.Drawing.Point(825, 200);
            this.borderless_ButtonR.Name = "borderless_ButtonR";
            this.borderless_ButtonR.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonR.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonR.RightBorderWidth = 1;
            this.borderless_ButtonR.Size = new System.Drawing.Size(37, 37);
            stringFormat4.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat4.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat4.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat4.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonR.StringFormat = stringFormat4;
            this.borderless_ButtonR.TabIndex = 10;
            this.borderless_ButtonR.Text = "R";
            this.borderless_ButtonR.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonR.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonR.TopBorderWidth = 1;
            // 
            // borderless_ButtonY
            // 
            this.borderless_ButtonY.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonY.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonY.BottomBorderWidth = 1;
            this.borderless_ButtonY.ButtonEnabled = false;
            this.borderless_ButtonY.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonY.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonY.LeftBorderWidth = 1;
            this.borderless_ButtonY.Location = new System.Drawing.Point(729, 200);
            this.borderless_ButtonY.Name = "borderless_ButtonY";
            this.borderless_ButtonY.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonY.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonY.RightBorderWidth = 1;
            this.borderless_ButtonY.Size = new System.Drawing.Size(37, 37);
            stringFormat5.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat5.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat5.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat5.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonY.StringFormat = stringFormat5;
            this.borderless_ButtonY.TabIndex = 12;
            this.borderless_ButtonY.Text = "Y";
            this.borderless_ButtonY.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonY.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonY.TopBorderWidth = 1;
            // 
            // borderless_ButtonL
            // 
            this.borderless_ButtonL.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonL.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonL.BottomBorderWidth = 1;
            this.borderless_ButtonL.ButtonEnabled = false;
            this.borderless_ButtonL.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonL.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonL.LeftBorderWidth = 1;
            this.borderless_ButtonL.Location = new System.Drawing.Point(779, 200);
            this.borderless_ButtonL.Name = "borderless_ButtonL";
            this.borderless_ButtonL.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonL.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonL.RightBorderWidth = 1;
            this.borderless_ButtonL.Size = new System.Drawing.Size(37, 37);
            stringFormat6.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat6.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat6.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat6.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonL.StringFormat = stringFormat6;
            this.borderless_ButtonL.TabIndex = 13;
            this.borderless_ButtonL.Text = "L";
            this.borderless_ButtonL.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonL.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonL.TopBorderWidth = 1;
            // 
            // borderless_ButtonRIGHT
            // 
            this.borderless_ButtonRIGHT.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonRIGHT.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonRIGHT.BottomBorderWidth = 1;
            this.borderless_ButtonRIGHT.ButtonEnabled = false;
            this.borderless_ButtonRIGHT.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonRIGHT.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonRIGHT.LeftBorderWidth = 1;
            this.borderless_ButtonRIGHT.Location = new System.Drawing.Point(729, 256);
            this.borderless_ButtonRIGHT.Name = "borderless_ButtonRIGHT";
            this.borderless_ButtonRIGHT.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonRIGHT.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonRIGHT.RightBorderWidth = 1;
            this.borderless_ButtonRIGHT.Size = new System.Drawing.Size(37, 37);
            stringFormat7.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat7.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat7.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat7.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonRIGHT.StringFormat = stringFormat7;
            this.borderless_ButtonRIGHT.TabIndex = 17;
            this.borderless_ButtonRIGHT.Text = "RHT";
            this.borderless_ButtonRIGHT.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonRIGHT.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonRIGHT.TopBorderWidth = 1;
            // 
            // borderless_ButtonLEFT
            // 
            this.borderless_ButtonLEFT.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonLEFT.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonLEFT.BottomBorderWidth = 1;
            this.borderless_ButtonLEFT.ButtonEnabled = false;
            this.borderless_ButtonLEFT.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonLEFT.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonLEFT.LeftBorderWidth = 1;
            this.borderless_ButtonLEFT.Location = new System.Drawing.Point(683, 256);
            this.borderless_ButtonLEFT.Name = "borderless_ButtonLEFT";
            this.borderless_ButtonLEFT.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonLEFT.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonLEFT.RightBorderWidth = 1;
            this.borderless_ButtonLEFT.Size = new System.Drawing.Size(37, 37);
            stringFormat8.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat8.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat8.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat8.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonLEFT.StringFormat = stringFormat8;
            this.borderless_ButtonLEFT.TabIndex = 16;
            this.borderless_ButtonLEFT.Text = "LFT";
            this.borderless_ButtonLEFT.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonLEFT.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonLEFT.TopBorderWidth = 1;
            // 
            // borderless_ButtonDOWN
            // 
            this.borderless_ButtonDOWN.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonDOWN.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonDOWN.BottomBorderWidth = 1;
            this.borderless_ButtonDOWN.ButtonEnabled = false;
            this.borderless_ButtonDOWN.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonDOWN.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonDOWN.LeftBorderWidth = 1;
            this.borderless_ButtonDOWN.Location = new System.Drawing.Point(637, 256);
            this.borderless_ButtonDOWN.Name = "borderless_ButtonDOWN";
            this.borderless_ButtonDOWN.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonDOWN.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonDOWN.RightBorderWidth = 1;
            this.borderless_ButtonDOWN.Size = new System.Drawing.Size(37, 37);
            stringFormat9.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat9.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat9.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat9.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonDOWN.StringFormat = stringFormat9;
            this.borderless_ButtonDOWN.TabIndex = 15;
            this.borderless_ButtonDOWN.Text = "DWN";
            this.borderless_ButtonDOWN.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonDOWN.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonDOWN.TopBorderWidth = 1;
            // 
            // borderless_ButtonUP
            // 
            this.borderless_ButtonUP.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonUP.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonUP.BottomBorderWidth = 1;
            this.borderless_ButtonUP.ButtonEnabled = false;
            this.borderless_ButtonUP.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonUP.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonUP.LeftBorderWidth = 1;
            this.borderless_ButtonUP.Location = new System.Drawing.Point(591, 256);
            this.borderless_ButtonUP.Name = "borderless_ButtonUP";
            this.borderless_ButtonUP.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonUP.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonUP.RightBorderWidth = 1;
            this.borderless_ButtonUP.Size = new System.Drawing.Size(37, 37);
            stringFormat10.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat10.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat10.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat10.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonUP.StringFormat = stringFormat10;
            this.borderless_ButtonUP.TabIndex = 14;
            this.borderless_ButtonUP.Text = "UP";
            this.borderless_ButtonUP.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonUP.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonUP.TopBorderWidth = 1;
            // 
            // borderless_ButtonSELECT
            // 
            this.borderless_ButtonSELECT.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonSELECT.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonSELECT.BottomBorderWidth = 1;
            this.borderless_ButtonSELECT.ButtonEnabled = false;
            this.borderless_ButtonSELECT.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonSELECT.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonSELECT.LeftBorderWidth = 1;
            this.borderless_ButtonSELECT.Location = new System.Drawing.Point(779, 256);
            this.borderless_ButtonSELECT.Name = "borderless_ButtonSELECT";
            this.borderless_ButtonSELECT.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonSELECT.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonSELECT.RightBorderWidth = 1;
            this.borderless_ButtonSELECT.Size = new System.Drawing.Size(37, 37);
            stringFormat11.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat11.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat11.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat11.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonSELECT.StringFormat = stringFormat11;
            this.borderless_ButtonSELECT.TabIndex = 18;
            this.borderless_ButtonSELECT.Text = "SEL";
            this.borderless_ButtonSELECT.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonSELECT.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonSELECT.TopBorderWidth = 1;
            // 
            // borderless_ButtonSTART
            // 
            this.borderless_ButtonSTART.BottomBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonSTART.BottomBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonSTART.BottomBorderWidth = 1;
            this.borderless_ButtonSTART.ButtonEnabled = false;
            this.borderless_ButtonSTART.LeftBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonSTART.LeftBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonSTART.LeftBorderWidth = 1;
            this.borderless_ButtonSTART.Location = new System.Drawing.Point(825, 256);
            this.borderless_ButtonSTART.Name = "borderless_ButtonSTART";
            this.borderless_ButtonSTART.RightBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonSTART.RightBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonSTART.RightBorderWidth = 1;
            this.borderless_ButtonSTART.Size = new System.Drawing.Size(37, 37);
            stringFormat12.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat12.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat12.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat12.Trimming = System.Drawing.StringTrimming.Character;
            this.borderless_ButtonSTART.StringFormat = stringFormat12;
            this.borderless_ButtonSTART.TabIndex = 19;
            this.borderless_ButtonSTART.Text = "STA";
            this.borderless_ButtonSTART.TopBorderColour = System.Drawing.Color.Gray;
            this.borderless_ButtonSTART.TopBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.borderless_ButtonSTART.TopBorderWidth = 1;
            // 
            // Input_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(900, 512);
            this.Controls.Add(this.borderless_ButtonSTART);
            this.Controls.Add(this.borderless_ButtonSELECT);
            this.Controls.Add(this.borderless_ButtonRIGHT);
            this.Controls.Add(this.borderless_ButtonLEFT);
            this.Controls.Add(this.borderless_ButtonDOWN);
            this.Controls.Add(this.borderless_ButtonUP);
            this.Controls.Add(this.borderless_ButtonL);
            this.Controls.Add(this.borderless_ButtonY);
            this.Controls.Add(this.borderless_ButtonX);
            this.Controls.Add(this.borderless_ButtonR);
            this.Controls.Add(this.borderless_ButtonB);
            this.Controls.Add(this.borderless_ButtonA);
            this.Controls.Add(this.borderless_RightTrigger);
            this.Controls.Add(this.borderless_LeftTrigger);
            this.Controls.Add(this.borderless_RightStick);
            this.Controls.Add(this.borderless_LeftStick);
            this.Controls.Add(this.borderless_CurrentController);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Input_Screen";
            this.Text = "Main_Screen";
            this.VisibleChanged += new System.EventHandler(this.MenuVisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTipHelper;
        private AnimatedComboBox borderless_CurrentController;
        private Styles.Controls.Custom.CustomAnalogStickIndicator borderless_LeftStick;
        private Styles.Controls.Custom.CustomAnalogStickIndicator borderless_RightStick;
        private Styles.Controls.Custom.CustomVerticalProgressBar borderless_LeftTrigger;
        private Styles.Controls.Custom.CustomVerticalProgressBar borderless_RightTrigger;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonA;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonB;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonX;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonR;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonY;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonL;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonRIGHT;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonLEFT;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonDOWN;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonUP;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonSELECT;
        private Styles.Controls.Custom.CustomControllerButtonPressIndicator borderless_ButtonSTART;
    }
}