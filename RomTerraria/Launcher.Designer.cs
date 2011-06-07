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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
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
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(478, 216);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(470, 190);
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
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 238);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Launcher";
            this.Text = "RomTerraria Launcher - http://www.romsteady.net/";
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
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
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox maxNetPlayers;
    }
}

