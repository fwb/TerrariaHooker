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
            this.components = new System.ComponentModel.Container( );
            this.launchTerraria = new System.Windows.Forms.Button( );
            this.createCraftables = new System.Windows.Forms.Button( );
            this.enableTrainer = new System.Windows.Forms.CheckBox( );
            this.width = new System.Windows.Forms.TextBox( );
            this.x = new System.Windows.Forms.Label( );
            this.height = new System.Windows.Forms.TextBox( );
            this.enableMinimap = new System.Windows.Forms.CheckBox( );
            this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
            this.enableInvasion = new System.Windows.Forms.CheckBox( );
            this.turnOffShadowOrbSmashed = new System.Windows.Forms.CheckBox( );
            this.enableManaRecharge = new System.Windows.Forms.CheckBox( );
            this.enableBloodMoon = new System.Windows.Forms.CheckBox( );
            this.enableRealTimeMinimap = new System.Windows.Forms.CheckBox( );
            this.enableClock = new System.Windows.Forms.CheckBox( );
            this.enablePony = new System.Windows.Forms.CheckBox( );
            this.enableEyeSpawn = new System.Windows.Forms.CheckBox( );
            this.enableServerConsole = new System.Windows.Forms.CheckBox( );
            this.howOftenToRecharge = new System.Windows.Forms.TrackBar( );
            this.cheatContainer = new System.Windows.Forms.GroupBox( );
            this.label5 = new System.Windows.Forms.Label( );
            this.label4 = new System.Windows.Forms.Label( );
            this.groupBox2 = new System.Windows.Forms.GroupBox( );
            this.enableHardcore = new System.Windows.Forms.CheckBox( );
            this.groupBox3 = new System.Windows.Forms.GroupBox( );
            this.enableHiDef = new System.Windows.Forms.CheckBox( );
            this.groupBox4 = new System.Windows.Forms.GroupBox( );
            this.launchServer = new System.Windows.Forms.Button( );
            this.serverPassword = new System.Windows.Forms.TextBox( );
            this.label1 = new System.Windows.Forms.Label( );
            this.label2 = new System.Windows.Forms.Label( );
            this.selectWorld = new System.Windows.Forms.ComboBox( );
            this.tabControl1 = new System.Windows.Forms.TabControl( );
            this.tabPage1 = new System.Windows.Forms.TabPage( );
            this.groupBox6 = new System.Windows.Forms.GroupBox( );
            this.resetPort = new System.Windows.Forms.Button( );
            this.label3 = new System.Windows.Forms.Label( );
            this.networkPort = new System.Windows.Forms.TextBox( );
            this.groupBox5 = new System.Windows.Forms.GroupBox( );
            this.tabPage2 = new System.Windows.Forms.TabPage( );
            this.letMeMakeItEasier = new System.Windows.Forms.CheckBox( );
            this.label6 = new System.Windows.Forms.Label( );
            ( (System.ComponentModel.ISupportInitialize)( this.howOftenToRecharge ) ).BeginInit( );
            this.cheatContainer.SuspendLayout( );
            this.groupBox2.SuspendLayout( );
            this.groupBox3.SuspendLayout( );
            this.groupBox4.SuspendLayout( );
            this.tabControl1.SuspendLayout( );
            this.tabPage1.SuspendLayout( );
            this.groupBox6.SuspendLayout( );
            this.groupBox5.SuspendLayout( );
            this.tabPage2.SuspendLayout( );
            this.SuspendLayout( );
            // 
            // launchTerraria
            // 
            this.launchTerraria.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.launchTerraria.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.launchTerraria.Location = new System.Drawing.Point( 0, 409 );
            this.launchTerraria.Name = "launchTerraria";
            this.launchTerraria.Size = new System.Drawing.Size( 499, 43 );
            this.launchTerraria.TabIndex = 0;
            this.launchTerraria.Text = "Launch";
            this.toolTip1.SetToolTip( this.launchTerraria, "Launches Terraria." );
            this.launchTerraria.UseVisualStyleBackColor = true;
            this.launchTerraria.Click += new System.EventHandler( this.launchTerraria_Click );
            // 
            // createCraftables
            // 
            this.createCraftables.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.createCraftables.Location = new System.Drawing.Point( 0, 386 );
            this.createCraftables.Name = "createCraftables";
            this.createCraftables.Size = new System.Drawing.Size( 499, 23 );
            this.createCraftables.TabIndex = 8;
            this.createCraftables.Text = "Create Craftable List";
            this.toolTip1.SetToolTip( this.createCraftables, "Creates the file TerrariaCraftables.txt in your My Documents folder." );
            this.createCraftables.UseVisualStyleBackColor = true;
            this.createCraftables.Click += new System.EventHandler( this.createCraftables_Click );
            // 
            // enableTrainer
            // 
            this.enableTrainer.AutoSize = true;
            this.enableTrainer.Location = new System.Drawing.Point( 6, 19 );
            this.enableTrainer.Name = "enableTrainer";
            this.enableTrainer.Size = new System.Drawing.Size( 210, 17 );
            this.enableTrainer.TabIndex = 0;
            this.enableTrainer.Text = "Enable Rapid Health Recharge Trainer";
            this.toolTip1.SetToolTip( this.enableTrainer, "Single-player only. Causes you to recover 1HP per frame." );
            this.enableTrainer.UseVisualStyleBackColor = true;
            // 
            // width
            // 
            this.width.Location = new System.Drawing.Point( 6, 19 );
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size( 100, 20 );
            this.width.TabIndex = 2;
            this.width.Text = "1024";
            // 
            // x
            // 
            this.x.AutoSize = true;
            this.x.Location = new System.Drawing.Point( 113, 21 );
            this.x.Name = "x";
            this.x.Size = new System.Drawing.Size( 12, 13 );
            this.x.TabIndex = 3;
            this.x.Text = "x";
            // 
            // height
            // 
            this.height.Location = new System.Drawing.Point( 131, 19 );
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size( 100, 20 );
            this.height.TabIndex = 4;
            this.height.Text = "768";
            // 
            // enableMinimap
            // 
            this.enableMinimap.AutoSize = true;
            this.enableMinimap.Location = new System.Drawing.Point( 6, 115 );
            this.enableMinimap.Name = "enableMinimap";
            this.enableMinimap.Size = new System.Drawing.Size( 198, 17 );
            this.enableMinimap.TabIndex = 2;
            this.enableMinimap.Text = "Enable Inventory Full World Minimap";
            this.toolTip1.SetToolTip( this.enableMinimap, "Draws a minimap in the upper-right corner of the screen.  Best at 1280x720 or hig" +
                    "her." );
            this.enableMinimap.UseVisualStyleBackColor = true;
            // 
            // enableInvasion
            // 
            this.enableInvasion.AutoSize = true;
            this.enableInvasion.ForeColor = System.Drawing.SystemColors.Highlight;
            this.enableInvasion.Location = new System.Drawing.Point( 6, 17 );
            this.enableInvasion.Name = "enableInvasion";
            this.enableInvasion.Size = new System.Drawing.Size( 136, 17 );
            this.enableInvasion.TabIndex = 0;
            this.enableInvasion.Text = "Enable Infinite Invasion";
            this.toolTip1.SetToolTip( this.enableInvasion, "This will attempt to force a new invasion every frame." );
            this.enableInvasion.UseVisualStyleBackColor = true;
            // 
            // turnOffShadowOrbSmashed
            // 
            this.turnOffShadowOrbSmashed.AutoSize = true;
            this.turnOffShadowOrbSmashed.ForeColor = System.Drawing.SystemColors.Highlight;
            this.turnOffShadowOrbSmashed.Location = new System.Drawing.Point( 6, 161 );
            this.turnOffShadowOrbSmashed.Name = "turnOffShadowOrbSmashed";
            this.turnOffShadowOrbSmashed.Size = new System.Drawing.Size( 207, 17 );
            this.turnOffShadowOrbSmashed.TabIndex = 4;
            this.turnOffShadowOrbSmashed.Text = "Turn Off \"Shadow Orb Smashed\" Flag";
            this.toolTip1.SetToolTip( this.turnOffShadowOrbSmashed, "Once a shadow orb is smashed, goblin invasions will become common place.  This te" +
                    "lls the game you haven\'t smashed one." );
            this.turnOffShadowOrbSmashed.UseVisualStyleBackColor = true;
            // 
            // enableManaRecharge
            // 
            this.enableManaRecharge.AutoSize = true;
            this.enableManaRecharge.Location = new System.Drawing.Point( 6, 92 );
            this.enableManaRecharge.Name = "enableManaRecharge";
            this.enableManaRecharge.Size = new System.Drawing.Size( 206, 17 );
            this.enableManaRecharge.TabIndex = 1;
            this.enableManaRecharge.Text = "Enable Rapid Mana Recharge Trainer";
            this.toolTip1.SetToolTip( this.enableManaRecharge, "Single player only. Restores 1MP per frame." );
            this.enableManaRecharge.UseVisualStyleBackColor = true;
            // 
            // enableBloodMoon
            // 
            this.enableBloodMoon.AutoSize = true;
            this.enableBloodMoon.ForeColor = System.Drawing.SystemColors.Highlight;
            this.enableBloodMoon.Location = new System.Drawing.Point( 6, 41 );
            this.enableBloodMoon.Name = "enableBloodMoon";
            this.enableBloodMoon.Size = new System.Drawing.Size( 151, 17 );
            this.enableBloodMoon.TabIndex = 1;
            this.enableBloodMoon.Text = "Enable Infinte Blood Moon";
            this.toolTip1.SetToolTip( this.enableBloodMoon, "Every moon is a blood moon with this checked." );
            this.enableBloodMoon.UseVisualStyleBackColor = true;
            // 
            // enableRealTimeMinimap
            // 
            this.enableRealTimeMinimap.AutoSize = true;
            this.enableRealTimeMinimap.Location = new System.Drawing.Point( 6, 138 );
            this.enableRealTimeMinimap.Name = "enableRealTimeMinimap";
            this.enableRealTimeMinimap.Size = new System.Drawing.Size( 186, 17 );
            this.enableRealTimeMinimap.TabIndex = 3;
            this.enableRealTimeMinimap.Text = "Enable Surrounding Area Minimap";
            this.toolTip1.SetToolTip( this.enableRealTimeMinimap, "This enables a real-time \"surrounding area\" minimap.  Doesn\'t work on some videoc" +
                    "ards." );
            this.enableRealTimeMinimap.UseVisualStyleBackColor = true;
            // 
            // enableClock
            // 
            this.enableClock.AutoSize = true;
            this.enableClock.Location = new System.Drawing.Point( 7, 44 );
            this.enableClock.Name = "enableClock";
            this.enableClock.Size = new System.Drawing.Size( 254, 17 );
            this.enableClock.TabIndex = 1;
            this.enableClock.Text = "Enable Real-World Time Clock (Not Game Time)";
            this.toolTip1.SetToolTip( this.enableClock, "Show the current time unobstrusively in the upper-right corner of the screen." );
            this.enableClock.UseVisualStyleBackColor = true;
            // 
            // enablePony
            // 
            this.enablePony.AutoSize = true;
            this.enablePony.Location = new System.Drawing.Point( 6, 184 );
            this.enablePony.Name = "enablePony";
            this.enablePony.Size = new System.Drawing.Size( 86, 17 );
            this.enablePony.TabIndex = 5;
            this.enablePony.Text = "Enable Pony";
            this.toolTip1.SetToolTip( this.enablePony, "Pretty much what it says." );
            this.enablePony.UseVisualStyleBackColor = true;
            // 
            // enableEyeSpawn
            // 
            this.enableEyeSpawn.AutoSize = true;
            this.enableEyeSpawn.ForeColor = System.Drawing.SystemColors.Highlight;
            this.enableEyeSpawn.Location = new System.Drawing.Point( 6, 64 );
            this.enableEyeSpawn.Name = "enableEyeSpawn";
            this.enableEyeSpawn.Size = new System.Drawing.Size( 142, 17 );
            this.enableEyeSpawn.TabIndex = 2;
            this.enableEyeSpawn.Text = "Enable Daily Eye Spawn";
            this.toolTip1.SetToolTip( this.enableEyeSpawn, "Eye of Cthulhu spawns every evening" );
            this.enableEyeSpawn.UseVisualStyleBackColor = true;
            // 
            // enableServerConsole
            // 
            this.enableServerConsole.AutoSize = true;
            this.enableServerConsole.Location = new System.Drawing.Point( 9, 82 );
            this.enableServerConsole.Name = "enableServerConsole";
            this.enableServerConsole.Size = new System.Drawing.Size( 134, 17 );
            this.enableServerConsole.TabIndex = 0;
            this.enableServerConsole.Text = "Enable Server Console";
            this.toolTip1.SetToolTip( this.enableServerConsole, "This will attempt to force a new invasion every frame." );
            this.enableServerConsole.UseVisualStyleBackColor = true;
            // 
            // howOftenToRecharge
            // 
            this.howOftenToRecharge.Location = new System.Drawing.Point( 7, 43 );
            this.howOftenToRecharge.Maximum = 1000;
            this.howOftenToRecharge.Name = "howOftenToRecharge";
            this.howOftenToRecharge.Size = new System.Drawing.Size( 231, 45 );
            this.howOftenToRecharge.TabIndex = 6;
            this.howOftenToRecharge.TickFrequency = 100;
            this.toolTip1.SetToolTip( this.howOftenToRecharge, "How quickly you recover health/mana" );
            // 
            // cheatContainer
            // 
            this.cheatContainer.Controls.Add( this.label5 );
            this.cheatContainer.Controls.Add( this.label4 );
            this.cheatContainer.Controls.Add( this.howOftenToRecharge );
            this.cheatContainer.Controls.Add( this.enablePony );
            this.cheatContainer.Controls.Add( this.enableRealTimeMinimap );
            this.cheatContainer.Controls.Add( this.enableManaRecharge );
            this.cheatContainer.Controls.Add( this.turnOffShadowOrbSmashed );
            this.cheatContainer.Controls.Add( this.enableTrainer );
            this.cheatContainer.Controls.Add( this.enableMinimap );
            this.cheatContainer.Location = new System.Drawing.Point( 118, 99 );
            this.cheatContainer.Name = "cheatContainer";
            this.cheatContainer.Size = new System.Drawing.Size( 244, 208 );
            this.cheatContainer.TabIndex = 5;
            this.cheatContainer.TabStop = false;
            this.cheatContainer.Text = "Make It Easier";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 199, 74 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 39, 13 );
            this.label5.TabIndex = 8;
            this.label5.Text = "Slower";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 7, 74 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 36, 13 );
            this.label4.TabIndex = 7;
            this.label4.Text = "Faster";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add( this.enableHardcore );
            this.groupBox2.Controls.Add( this.enableEyeSpawn );
            this.groupBox2.Controls.Add( this.enableBloodMoon );
            this.groupBox2.Controls.Add( this.enableInvasion );
            this.groupBox2.Location = new System.Drawing.Point( 286, 135 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 181, 112 );
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Make It Harder";
            // 
            // enableHardcore
            // 
            this.enableHardcore.AutoSize = true;
            this.enableHardcore.Location = new System.Drawing.Point( 6, 88 );
            this.enableHardcore.Name = "enableHardcore";
            this.enableHardcore.Size = new System.Drawing.Size( 169, 17 );
            this.enableHardcore.TabIndex = 3;
            this.enableHardcore.Text = "Hardcore Mode (Experimental)";
            this.enableHardcore.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add( this.enableClock );
            this.groupBox3.Controls.Add( this.enableHiDef );
            this.groupBox3.Location = new System.Drawing.Point( 6, 267 );
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size( 457, 69 );
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Make It Faster";
            // 
            // enableHiDef
            // 
            this.enableHiDef.AutoSize = true;
            this.enableHiDef.Location = new System.Drawing.Point( 7, 20 );
            this.enableHiDef.Name = "enableHiDef";
            this.enableHiDef.Size = new System.Drawing.Size( 356, 17 );
            this.enableHiDef.TabIndex = 0;
            this.enableHiDef.Text = "Enable XNA HiDef Graphics Profile (Also Maps PrtSc To Screenshots)";
            this.enableHiDef.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add( this.launchServer );
            this.groupBox4.Controls.Add( this.serverPassword );
            this.groupBox4.Controls.Add( this.label1 );
            this.groupBox4.Controls.Add( this.label2 );
            this.groupBox4.Controls.Add( this.selectWorld );
            this.groupBox4.Controls.Add( this.enableServerConsole );
            this.groupBox4.Location = new System.Drawing.Point( 6, 6 );
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size( 457, 107 );
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Server Settings";
            // 
            // launchServer
            // 
            this.launchServer.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.launchServer.Location = new System.Drawing.Point( 299, 20 );
            this.launchServer.Name = "launchServer";
            this.launchServer.Size = new System.Drawing.Size( 152, 79 );
            this.launchServer.TabIndex = 5;
            this.launchServer.Text = "Start Dedicated Server";
            this.launchServer.UseVisualStyleBackColor = true;
            // 
            // serverPassword
            // 
            this.serverPassword.Location = new System.Drawing.Point( 90, 56 );
            this.serverPassword.Name = "serverPassword";
            this.serverPassword.Size = new System.Drawing.Size( 202, 20 );
            this.serverPassword.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 6, 56 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 53, 13 );
            this.label1.TabIndex = 3;
            this.label1.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 6, 28 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 69, 13 );
            this.label2.TabIndex = 2;
            this.label2.Text = "Server World";
            // 
            // selectWorld
            // 
            this.selectWorld.FormattingEnabled = true;
            this.selectWorld.Location = new System.Drawing.Point( 90, 25 );
            this.selectWorld.Name = "selectWorld";
            this.selectWorld.Size = new System.Drawing.Size( 202, 21 );
            this.selectWorld.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add( this.tabPage1 );
            this.tabControl1.Controls.Add( this.tabPage2 );
            this.tabControl1.Location = new System.Drawing.Point( 12, 12 );
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size( 478, 367 );
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add( this.groupBox6 );
            this.tabPage1.Controls.Add( this.groupBox5 );
            this.tabPage1.Controls.Add( this.groupBox3 );
            this.tabPage1.Controls.Add( this.groupBox4 );
            this.tabPage1.Controls.Add( this.groupBox2 );
            this.tabPage1.Location = new System.Drawing.Point( 4, 22 );
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPage1.Size = new System.Drawing.Size( 470, 341 );
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add( this.resetPort );
            this.groupBox6.Controls.Add( this.label3 );
            this.groupBox6.Controls.Add( this.networkPort );
            this.groupBox6.Location = new System.Drawing.Point( 3, 131 );
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size( 250, 52 );
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Network Settings (Client and Server)";
            // 
            // resetPort
            // 
            this.resetPort.Location = new System.Drawing.Point( 169, 15 );
            this.resetPort.Name = "resetPort";
            this.resetPort.Size = new System.Drawing.Size( 75, 23 );
            this.resetPort.TabIndex = 2;
            this.resetPort.Text = "Default";
            this.resetPort.UseVisualStyleBackColor = true;
            this.resetPort.Click += new System.EventHandler( this.ResetPortClick );
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 7, 20 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 26, 13 );
            this.label3.TabIndex = 1;
            this.label3.Text = "Port";
            // 
            // networkPort
            // 
            this.networkPort.Location = new System.Drawing.Point( 45, 17 );
            this.networkPort.Name = "networkPort";
            this.networkPort.Size = new System.Drawing.Size( 118, 20 );
            this.networkPort.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add( this.width );
            this.groupBox5.Controls.Add( this.height );
            this.groupBox5.Controls.Add( this.x );
            this.groupBox5.Location = new System.Drawing.Point( 6, 201 );
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size( 239, 46 );
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Resolution";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add( this.letMeMakeItEasier );
            this.tabPage2.Controls.Add( this.cheatContainer );
            this.tabPage2.Location = new System.Drawing.Point( 4, 22 );
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPage2.Size = new System.Drawing.Size( 470, 341 );
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cheats";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // letMeMakeItEasier
            // 
            this.letMeMakeItEasier.AutoSize = true;
            this.letMeMakeItEasier.Location = new System.Drawing.Point( 163, 48 );
            this.letMeMakeItEasier.Name = "letMeMakeItEasier";
            this.letMeMakeItEasier.Size = new System.Drawing.Size( 144, 17 );
            this.letMeMakeItEasier.TabIndex = 6;
            this.letMeMakeItEasier.Text = "Let Me Make This Easier";
            this.letMeMakeItEasier.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label6.Location = new System.Drawing.Point( 230, 12 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 256, 13 );
            this.label6.TabIndex = 10;
            this.label6.Text = "Options in blue may also affect your dedicated server";
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 499, 452 );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.tabControl1 );
            this.Controls.Add( this.createCraftables );
            this.Controls.Add( this.launchTerraria );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Launcher";
            this.Text = "RomTerraria Launcher - http://www.romsteady.net/";
            this.Load += new System.EventHandler( this.Launcher_Load );
            ( (System.ComponentModel.ISupportInitialize)( this.howOftenToRecharge ) ).EndInit( );
            this.cheatContainer.ResumeLayout( false );
            this.cheatContainer.PerformLayout( );
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout( );
            this.groupBox3.ResumeLayout( false );
            this.groupBox3.PerformLayout( );
            this.groupBox4.ResumeLayout( false );
            this.groupBox4.PerformLayout( );
            this.tabControl1.ResumeLayout( false );
            this.tabPage1.ResumeLayout( false );
            this.groupBox6.ResumeLayout( false );
            this.groupBox6.PerformLayout( );
            this.groupBox5.ResumeLayout( false );
            this.groupBox5.PerformLayout( );
            this.tabPage2.ResumeLayout( false );
            this.tabPage2.PerformLayout( );
            this.ResumeLayout( false );
            this.PerformLayout( );

        }

        #endregion

        private System.Windows.Forms.Button launchTerraria;
        private System.Windows.Forms.Button createCraftables;
        private System.Windows.Forms.CheckBox enableTrainer;
        private System.Windows.Forms.TextBox width;
        private System.Windows.Forms.Label x;
        private System.Windows.Forms.TextBox height;
        private System.Windows.Forms.CheckBox enableMinimap;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox enableInvasion;
        private System.Windows.Forms.GroupBox cheatContainer;
        private System.Windows.Forms.CheckBox turnOffShadowOrbSmashed;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox enableManaRecharge;
        private System.Windows.Forms.CheckBox enableBloodMoon;
        private System.Windows.Forms.CheckBox enableRealTimeMinimap;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox enableHiDef;
        private System.Windows.Forms.CheckBox enableClock;
        private System.Windows.Forms.CheckBox enablePony;
        private System.Windows.Forms.CheckBox enableEyeSpawn;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox enableServerConsole;
        private System.Windows.Forms.CheckBox enableHardcore;
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
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox letMeMakeItEasier;
        private System.Windows.Forms.TrackBar howOftenToRecharge;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
    }
}

