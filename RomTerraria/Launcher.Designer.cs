namespace RomTerraria
{
    partial class Launcher
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
            this.enableTrainer = new System.Windows.Forms.CheckBox();
            this.enableMinimap = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.turnOffShadowOrbSmashed = new System.Windows.Forms.CheckBox();
            this.enableManaRecharge = new System.Windows.Forms.CheckBox();
            this.enableRealTimeMinimap = new System.Windows.Forms.CheckBox();
            this.enablePony = new System.Windows.Forms.CheckBox();
            this.howOftenToRecharge = new System.Windows.Forms.TrackBar();
            this.cheatContainer = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.maxNetPlayers = new System.Windows.Forms.TextBox();
            this.launchServer = new System.Windows.Forms.Button();
            this.serverPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.selectWorld = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.resetPort = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.networkPort = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.letMeMakeItEasier = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.howOftenToRecharge)).BeginInit();
            this.cheatContainer.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // enableTrainer
            // 
            this.enableTrainer.AutoSize = true;
            this.enableTrainer.Location = new System.Drawing.Point(6, 19);
            this.enableTrainer.Name = "enableTrainer";
            this.enableTrainer.Size = new System.Drawing.Size(210, 17);
            this.enableTrainer.TabIndex = 0;
            this.enableTrainer.Text = "Enable Rapid Health Recharge Trainer";
            this.toolTip1.SetToolTip(this.enableTrainer, "Single-player only. Causes you to recover 1HP per frame.");
            this.enableTrainer.UseVisualStyleBackColor = true;
            // 
            // enableMinimap
            // 
            this.enableMinimap.AutoSize = true;
            this.enableMinimap.Location = new System.Drawing.Point(6, 115);
            this.enableMinimap.Name = "enableMinimap";
            this.enableMinimap.Size = new System.Drawing.Size(198, 17);
            this.enableMinimap.TabIndex = 2;
            this.enableMinimap.Text = "Enable Inventory Full World Minimap";
            this.toolTip1.SetToolTip(this.enableMinimap, "Draws a minimap in the upper-right corner of the screen.  Best at 1280x720 or hig" +
                    "her.");
            this.enableMinimap.UseVisualStyleBackColor = true;
            // 
            // turnOffShadowOrbSmashed
            // 
            this.turnOffShadowOrbSmashed.AutoSize = true;
            this.turnOffShadowOrbSmashed.ForeColor = System.Drawing.SystemColors.Highlight;
            this.turnOffShadowOrbSmashed.Location = new System.Drawing.Point(6, 161);
            this.turnOffShadowOrbSmashed.Name = "turnOffShadowOrbSmashed";
            this.turnOffShadowOrbSmashed.Size = new System.Drawing.Size(207, 17);
            this.turnOffShadowOrbSmashed.TabIndex = 4;
            this.turnOffShadowOrbSmashed.Text = "Turn Off \"Shadow Orb Smashed\" Flag";
            this.toolTip1.SetToolTip(this.turnOffShadowOrbSmashed, "Once a shadow orb is smashed, goblin invasions will become common place.  This te" +
                    "lls the game you haven\'t smashed one.");
            this.turnOffShadowOrbSmashed.UseVisualStyleBackColor = true;
            // 
            // enableManaRecharge
            // 
            this.enableManaRecharge.AutoSize = true;
            this.enableManaRecharge.Location = new System.Drawing.Point(6, 92);
            this.enableManaRecharge.Name = "enableManaRecharge";
            this.enableManaRecharge.Size = new System.Drawing.Size(206, 17);
            this.enableManaRecharge.TabIndex = 1;
            this.enableManaRecharge.Text = "Enable Rapid Mana Recharge Trainer";
            this.toolTip1.SetToolTip(this.enableManaRecharge, "Single player only. Restores 1MP per frame.");
            this.enableManaRecharge.UseVisualStyleBackColor = true;
            // 
            // enableRealTimeMinimap
            // 
            this.enableRealTimeMinimap.AutoSize = true;
            this.enableRealTimeMinimap.Location = new System.Drawing.Point(6, 138);
            this.enableRealTimeMinimap.Name = "enableRealTimeMinimap";
            this.enableRealTimeMinimap.Size = new System.Drawing.Size(186, 17);
            this.enableRealTimeMinimap.TabIndex = 3;
            this.enableRealTimeMinimap.Text = "Enable Surrounding Area Minimap";
            this.toolTip1.SetToolTip(this.enableRealTimeMinimap, "This enables a real-time \"surrounding area\" minimap.  Doesn\'t work on some videoc" +
                    "ards.");
            this.enableRealTimeMinimap.UseVisualStyleBackColor = true;
            // 
            // enablePony
            // 
            this.enablePony.AutoSize = true;
            this.enablePony.Location = new System.Drawing.Point(6, 184);
            this.enablePony.Name = "enablePony";
            this.enablePony.Size = new System.Drawing.Size(86, 17);
            this.enablePony.TabIndex = 5;
            this.enablePony.Text = "Enable Pony";
            this.toolTip1.SetToolTip(this.enablePony, "Pretty much what it says.");
            this.enablePony.UseVisualStyleBackColor = true;
            // 
            // howOftenToRecharge
            // 
            this.howOftenToRecharge.Location = new System.Drawing.Point(7, 43);
            this.howOftenToRecharge.Maximum = 1000;
            this.howOftenToRecharge.Name = "howOftenToRecharge";
            this.howOftenToRecharge.Size = new System.Drawing.Size(231, 42);
            this.howOftenToRecharge.TabIndex = 6;
            this.howOftenToRecharge.TickFrequency = 100;
            this.toolTip1.SetToolTip(this.howOftenToRecharge, "How quickly you recover health/mana");
            // 
            // cheatContainer
            // 
            this.cheatContainer.Controls.Add(this.label5);
            this.cheatContainer.Controls.Add(this.label4);
            this.cheatContainer.Controls.Add(this.howOftenToRecharge);
            this.cheatContainer.Controls.Add(this.enablePony);
            this.cheatContainer.Controls.Add(this.enableRealTimeMinimap);
            this.cheatContainer.Controls.Add(this.enableManaRecharge);
            this.cheatContainer.Controls.Add(this.turnOffShadowOrbSmashed);
            this.cheatContainer.Controls.Add(this.enableTrainer);
            this.cheatContainer.Controls.Add(this.enableMinimap);
            this.cheatContainer.Location = new System.Drawing.Point(118, 99);
            this.cheatContainer.Name = "cheatContainer";
            this.cheatContainer.Size = new System.Drawing.Size(244, 208);
            this.cheatContainer.TabIndex = 5;
            this.cheatContainer.TabStop = false;
            this.cheatContainer.Text = "Make It Easier";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(199, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Slower";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Faster";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.maxNetPlayers);
            this.groupBox4.Controls.Add(this.launchServer);
            this.groupBox4.Controls.Add(this.serverPassword);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.selectWorld);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(457, 119);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Server Settings";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Max Players";
            // 
            // maxNetPlayers
            // 
            this.maxNetPlayers.Location = new System.Drawing.Point(90, 83);
            this.maxNetPlayers.Name = "maxNetPlayers";
            this.maxNetPlayers.Size = new System.Drawing.Size(54, 20);
            this.maxNetPlayers.TabIndex = 6;
            this.maxNetPlayers.Text = "8";
            // 
            // launchServer
            // 
            this.launchServer.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.launchServer.Location = new System.Drawing.Point(299, 20);
            this.launchServer.Name = "launchServer";
            this.launchServer.Size = new System.Drawing.Size(152, 79);
            this.launchServer.TabIndex = 5;
            this.launchServer.Text = "Start Dedicated Server";
            this.launchServer.UseVisualStyleBackColor = true;
            this.launchServer.Click += new System.EventHandler(this.launchServer_Click);
            // 
            // serverPassword
            // 
            this.serverPassword.Location = new System.Drawing.Point(90, 56);
            this.serverPassword.Name = "serverPassword";
            this.serverPassword.Size = new System.Drawing.Size(202, 20);
            this.serverPassword.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Server World";
            // 
            // selectWorld
            // 
            this.selectWorld.FormattingEnabled = true;
            this.selectWorld.Location = new System.Drawing.Point(90, 25);
            this.selectWorld.Name = "selectWorld";
            this.selectWorld.Size = new System.Drawing.Size(202, 21);
            this.selectWorld.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(478, 367);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(470, 341);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.resetPort);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.networkPort);
            this.groupBox6.Location = new System.Drawing.Point(3, 131);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(250, 52);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Network Settings (Client and Server)";
            // 
            // resetPort
            // 
            this.resetPort.Location = new System.Drawing.Point(169, 15);
            this.resetPort.Name = "resetPort";
            this.resetPort.Size = new System.Drawing.Size(75, 23);
            this.resetPort.TabIndex = 2;
            this.resetPort.Text = "Default";
            this.resetPort.UseVisualStyleBackColor = true;
            this.resetPort.Click += new System.EventHandler(this.ResetPortClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Port";
            // 
            // networkPort
            // 
            this.networkPort.Location = new System.Drawing.Point(45, 17);
            this.networkPort.Name = "networkPort";
            this.networkPort.Size = new System.Drawing.Size(118, 20);
            this.networkPort.TabIndex = 0;
            this.networkPort.Text = "7777";
            this.networkPort.TextChanged += new System.EventHandler(this.networkPort_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.letMeMakeItEasier);
            this.tabPage2.Controls.Add(this.cheatContainer);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(470, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cheats";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // letMeMakeItEasier
            // 
            this.letMeMakeItEasier.AutoSize = true;
            this.letMeMakeItEasier.Location = new System.Drawing.Point(163, 48);
            this.letMeMakeItEasier.Name = "letMeMakeItEasier";
            this.letMeMakeItEasier.Size = new System.Drawing.Size(144, 17);
            this.letMeMakeItEasier.TabIndex = 6;
            this.letMeMakeItEasier.Text = "Let Me Make This Easier";
            this.letMeMakeItEasier.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label6.Location = new System.Drawing.Point(230, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(256, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Options in blue may also affect your dedicated server";
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 382);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Launcher";
            this.Text = "RomTerraria Launcher - http://www.romsteady.net/";
            this.Load += new System.EventHandler(this.Launcher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.howOftenToRecharge)).EndInit();
            this.cheatContainer.ResumeLayout(false);
            this.cheatContainer.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox enableTrainer;
        private System.Windows.Forms.CheckBox enableMinimap;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox cheatContainer;
        private System.Windows.Forms.CheckBox turnOffShadowOrbSmashed;
        private System.Windows.Forms.CheckBox enableManaRecharge;
        private System.Windows.Forms.CheckBox enableRealTimeMinimap;
        private System.Windows.Forms.CheckBox enablePony;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button launchServer;
        private System.Windows.Forms.TextBox serverPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox selectWorld;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button resetPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox networkPort;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox letMeMakeItEasier;
        private System.Windows.Forms.TrackBar howOftenToRecharge;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox maxNetPlayers;
    }
}

