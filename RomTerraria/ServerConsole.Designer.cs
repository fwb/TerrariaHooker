namespace RomTerraria {
    partial class ServerConsole {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && ( components != null ) ) {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( ) {
            this.msgText = new System.Windows.Forms.TextBox();
            this.sendMessage = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ConsoleText = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.playerList = new System.Windows.Forms.ListView();
            this.Player = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Actions = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RefreshPlayers = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.useNearSpawn = new System.Windows.Forms.CheckBox();
            this.disableSpawns = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.loadNPCs = new System.Windows.Forms.Button();
            this.npcPicker = new System.Windows.Forms.ListBox();
            this.spawnButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.playerSelectLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.playerSelectSlider = new System.Windows.Forms.TrackBar();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SpawnMeteorButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.meteorGetPosition = new System.Windows.Forms.Button();
            this.customMeteorSpawn = new System.Windows.Forms.CheckBox();
            this.meteorPlayerList = new System.Windows.Forms.ComboBox();
            this.meteorSpawnX = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.meteorSpawnY = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.enableEyeSpawn = new System.Windows.Forms.CheckBox();
            this.enableBloodMoon = new System.Windows.Forms.CheckBox();
            this.enableInvasion = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.consoleInput = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button4 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playerSelectSlider)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // msgText
            // 
            this.msgText.Location = new System.Drawing.Point(6, 307);
            this.msgText.Name = "msgText";
            this.msgText.Size = new System.Drawing.Size(376, 20);
            this.msgText.TabIndex = 1;
            this.msgText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MsgTextKeyPress);
            // 
            // sendMessage
            // 
            this.sendMessage.Location = new System.Drawing.Point(403, 305);
            this.sendMessage.Name = "sendMessage";
            this.sendMessage.Size = new System.Drawing.Size(75, 23);
            this.sendMessage.TabIndex = 2;
            this.sendMessage.Text = "Send";
            this.sendMessage.UseVisualStyleBackColor = true;
            this.sendMessage.Click += new System.EventHandler(this.SendMessageClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 291);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Broadcast Message:";
            // 
            // ConsoleText
            // 
            this.ConsoleText.Location = new System.Drawing.Point(6, 6);
            this.ConsoleText.Multiline = true;
            this.ConsoleText.Name = "ConsoleText";
            this.ConsoleText.ReadOnly = true;
            this.ConsoleText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConsoleText.Size = new System.Drawing.Size(469, 275);
            this.ConsoleText.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(489, 362);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ConsoleText);
            this.tabPage1.Controls.Add(this.msgText);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.sendMessage);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(481, 336);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Console";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.playerList);
            this.tabPage4.Controls.Add(this.RefreshPlayers);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(481, 336);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Clients";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // playerList
            // 
            this.playerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Player,
            this.IP,
            this.Actions});
            this.playerList.FullRowSelect = true;
            this.playerList.Location = new System.Drawing.Point(3, 3);
            this.playerList.Name = "playerList";
            this.playerList.Size = new System.Drawing.Size(475, 301);
            this.playerList.TabIndex = 1;
            this.playerList.UseCompatibleStateImageBehavior = false;
            this.playerList.View = System.Windows.Forms.View.Details;
            // 
            // Player
            // 
            this.Player.Text = "Player";
            this.Player.Width = 134;
            // 
            // IP
            // 
            this.IP.Text = "IP";
            this.IP.Width = 149;
            // 
            // Actions
            // 
            this.Actions.Text = "Actions";
            this.Actions.Width = 187;
            // 
            // RefreshPlayers
            // 
            this.RefreshPlayers.Location = new System.Drawing.Point(211, 310);
            this.RefreshPlayers.Name = "RefreshPlayers";
            this.RefreshPlayers.Size = new System.Drawing.Size(75, 23);
            this.RefreshPlayers.TabIndex = 0;
            this.RefreshPlayers.Text = "Refresh";
            this.RefreshPlayers.UseVisualStyleBackColor = true;
            this.RefreshPlayers.Click += new System.EventHandler(this.RefreshPlayersClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.useNearSpawn);
            this.tabPage2.Controls.Add(this.disableSpawns);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.loadNPCs);
            this.tabPage2.Controls.Add(this.npcPicker);
            this.tabPage2.Controls.Add(this.spawnButton);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.playerSelectLabel);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.playerSelectSlider);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(481, 336);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "NPCs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // useNearSpawn
            // 
            this.useNearSpawn.AutoSize = true;
            this.useNearSpawn.Location = new System.Drawing.Point(291, 43);
            this.useNearSpawn.Name = "useNearSpawn";
            this.useNearSpawn.Size = new System.Drawing.Size(154, 17);
            this.useNearSpawn.TabIndex = 13;
            this.useNearSpawn.Text = "Use Built-in Spawn Locator";
            this.useNearSpawn.UseVisualStyleBackColor = true;
            // 
            // disableSpawns
            // 
            this.disableSpawns.AutoSize = true;
            this.disableSpawns.Location = new System.Drawing.Point(291, 19);
            this.disableSpawns.Name = "disableSpawns";
            this.disableSpawns.Size = new System.Drawing.Size(102, 17);
            this.disableSpawns.TabIndex = 12;
            this.disableSpawns.Text = "Disable Spawns";
            this.disableSpawns.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(399, 306);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(287, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 212);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(302, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Customize:";
            // 
            // loadNPCs
            // 
            this.loadNPCs.Location = new System.Drawing.Point(6, 307);
            this.loadNPCs.Name = "loadNPCs";
            this.loadNPCs.Size = new System.Drawing.Size(75, 23);
            this.loadNPCs.TabIndex = 8;
            this.loadNPCs.Text = "Load NPCs";
            this.loadNPCs.UseVisualStyleBackColor = true;
            this.loadNPCs.Click += new System.EventHandler(this.LoadNPCsClick);
            // 
            // npcPicker
            // 
            this.npcPicker.FormattingEnabled = true;
            this.npcPicker.Location = new System.Drawing.Point(6, 88);
            this.npcPicker.Name = "npcPicker";
            this.npcPicker.Size = new System.Drawing.Size(275, 212);
            this.npcPicker.TabIndex = 6;
            // 
            // spawnButton
            // 
            this.spawnButton.Location = new System.Drawing.Point(206, 307);
            this.spawnButton.Name = "spawnButton";
            this.spawnButton.Size = new System.Drawing.Size(75, 23);
            this.spawnButton.TabIndex = 5;
            this.spawnButton.Text = "Spawn!";
            this.spawnButton.UseVisualStyleBackColor = true;
            this.spawnButton.Click += new System.EventHandler(this.SpawnButtonClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Choose a Mob:";
            // 
            // playerSelectLabel
            // 
            this.playerSelectLabel.AutoSize = true;
            this.playerSelectLabel.Location = new System.Drawing.Point(130, 51);
            this.playerSelectLabel.Name = "playerSelectLabel";
            this.playerSelectLabel.Size = new System.Drawing.Size(51, 13);
            this.playerSelectLabel.TabIndex = 2;
            this.playerSelectLabel.Text = "(Inactive)";
            this.playerSelectLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose a Victim:";
            // 
            // playerSelectSlider
            // 
            this.playerSelectSlider.LargeChange = 1;
            this.playerSelectSlider.Location = new System.Drawing.Point(6, 19);
            this.playerSelectSlider.Maximum = 7;
            this.playerSelectSlider.Name = "playerSelectSlider";
            this.playerSelectSlider.Size = new System.Drawing.Size(278, 42);
            this.playerSelectSlider.TabIndex = 0;
            this.playerSelectSlider.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.playerSelectSlider.Scroll += new System.EventHandler(this.PlayerSelectSliderScroll);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(481, 336);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Events";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SpawnMeteorButton);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.meteorGetPosition);
            this.groupBox3.Controls.Add(this.customMeteorSpawn);
            this.groupBox3.Controls.Add(this.meteorPlayerList);
            this.groupBox3.Controls.Add(this.meteorSpawnX);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.meteorSpawnY);
            this.groupBox3.Location = new System.Drawing.Point(230, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 132);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Meteors";
            // 
            // SpawnMeteorButton
            // 
            this.SpawnMeteorButton.Location = new System.Drawing.Point(76, 97);
            this.SpawnMeteorButton.Name = "SpawnMeteorButton";
            this.SpawnMeteorButton.Size = new System.Drawing.Size(96, 23);
            this.SpawnMeteorButton.TabIndex = 9;
            this.SpawnMeteorButton.Text = "Spawn Meteor";
            this.SpawnMeteorButton.UseVisualStyleBackColor = true;
            this.SpawnMeteorButton.Click += new System.EventHandler(this.SpawnMeteorButtonClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "X";
            // 
            // meteorGetPosition
            // 
            this.meteorGetPosition.Location = new System.Drawing.Point(143, 68);
            this.meteorGetPosition.Name = "meteorGetPosition";
            this.meteorGetPosition.Size = new System.Drawing.Size(75, 23);
            this.meteorGetPosition.TabIndex = 16;
            this.meteorGetPosition.Text = "Get Position";
            this.meteorGetPosition.UseVisualStyleBackColor = true;
            // 
            // customMeteorSpawn
            // 
            this.customMeteorSpawn.AutoSize = true;
            this.customMeteorSpawn.Location = new System.Drawing.Point(36, 19);
            this.customMeteorSpawn.Name = "customMeteorSpawn";
            this.customMeteorSpawn.Size = new System.Drawing.Size(182, 17);
            this.customMeteorSpawn.TabIndex = 10;
            this.customMeteorSpawn.Text = "Use Custom Meteor Spawn Point";
            this.customMeteorSpawn.UseVisualStyleBackColor = true;
            // 
            // meteorPlayerList
            // 
            this.meteorPlayerList.FormattingEnabled = true;
            this.meteorPlayerList.Location = new System.Drawing.Point(36, 70);
            this.meteorPlayerList.Name = "meteorPlayerList";
            this.meteorPlayerList.Size = new System.Drawing.Size(96, 21);
            this.meteorPlayerList.TabIndex = 15;
            // 
            // meteorSpawnX
            // 
            this.meteorSpawnX.Location = new System.Drawing.Point(53, 44);
            this.meteorSpawnX.Name = "meteorSpawnX";
            this.meteorSpawnX.Size = new System.Drawing.Size(60, 20);
            this.meteorSpawnX.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(138, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Y";
            // 
            // meteorSpawnY
            // 
            this.meteorSpawnY.Location = new System.Drawing.Point(158, 41);
            this.meteorSpawnY.Name = "meteorSpawnY";
            this.meteorSpawnY.Size = new System.Drawing.Size(60, 20);
            this.meteorSpawnY.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.enableEyeSpawn);
            this.groupBox2.Controls.Add(this.enableBloodMoon);
            this.groupBox2.Controls.Add(this.enableInvasion);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(159, 92);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Survival";
            // 
            // enableEyeSpawn
            // 
            this.enableEyeSpawn.AutoSize = true;
            this.enableEyeSpawn.Location = new System.Drawing.Point(6, 66);
            this.enableEyeSpawn.Name = "enableEyeSpawn";
            this.enableEyeSpawn.Size = new System.Drawing.Size(142, 17);
            this.enableEyeSpawn.TabIndex = 2;
            this.enableEyeSpawn.Text = "Enable Daily Eye Spawn";
            this.enableEyeSpawn.UseVisualStyleBackColor = true;
            this.enableEyeSpawn.CheckedChanged += new System.EventHandler(this.EnableEyeSpawnCheckedChanged);
            // 
            // enableBloodMoon
            // 
            this.enableBloodMoon.AutoSize = true;
            this.enableBloodMoon.Location = new System.Drawing.Point(6, 43);
            this.enableBloodMoon.Name = "enableBloodMoon";
            this.enableBloodMoon.Size = new System.Drawing.Size(151, 17);
            this.enableBloodMoon.TabIndex = 1;
            this.enableBloodMoon.Text = "Enable Infinte Blood Moon";
            this.enableBloodMoon.UseVisualStyleBackColor = true;
            this.enableBloodMoon.CheckedChanged += new System.EventHandler(this.EnableBloodMoonCheckedChanged);
            // 
            // enableInvasion
            // 
            this.enableInvasion.AutoSize = true;
            this.enableInvasion.Location = new System.Drawing.Point(6, 19);
            this.enableInvasion.Name = "enableInvasion";
            this.enableInvasion.Size = new System.Drawing.Size(136, 17);
            this.enableInvasion.TabIndex = 0;
            this.enableInvasion.Text = "Enable Infinite Invasion";
            this.enableInvasion.UseVisualStyleBackColor = true;
            this.enableInvasion.CheckedChanged += new System.EventHandler(this.EnableInvasionCheckedChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.consoleInput);
            this.tabPage5.Controls.Add(this.button3);
            this.tabPage5.Controls.Add(this.textBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(481, 336);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Testbed";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // consoleInput
            // 
            this.consoleInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.consoleInput.BackColor = System.Drawing.Color.Black;
            this.consoleInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.consoleInput.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consoleInput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.consoleInput.Location = new System.Drawing.Point(3, 312);
            this.consoleInput.Name = "consoleInput";
            this.consoleInput.Size = new System.Drawing.Size(474, 23);
            this.consoleInput.TabIndex = 2;
            this.consoleInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.consoleInputKeyPress);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(25, 22);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(71, 21);
            this.button3.TabIndex = 1;
            this.button3.Text = "Send";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(474, 309);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(426, 380);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Hide";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(318, 380);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(102, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "Load Commands";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // ServerConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 414);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Name = "ServerConsole";
            this.Text = "ServerConsole";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playerSelectSlider)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox msgText;
        private System.Windows.Forms.Button sendMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ConsoleText;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label playerSelectLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar playerSelectSlider;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button spawnButton;
        private System.Windows.Forms.ListBox npcPicker;
        private System.Windows.Forms.Button loadNPCs;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox disableSpawns;
        private System.Windows.Forms.CheckBox useNearSpawn;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox enableEyeSpawn;
        private System.Windows.Forms.CheckBox enableBloodMoon;
        private System.Windows.Forms.CheckBox enableInvasion;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button RefreshPlayers;
        private System.Windows.Forms.Button SpawnMeteorButton;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button meteorGetPosition;
        private System.Windows.Forms.CheckBox customMeteorSpawn;
        private System.Windows.Forms.ComboBox meteorPlayerList;
        private System.Windows.Forms.TextBox meteorSpawnX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox meteorSpawnY;
        private System.Windows.Forms.ListView playerList;
        private System.Windows.Forms.ColumnHeader Player;
        private System.Windows.Forms.ColumnHeader IP;
        private System.Windows.Forms.ColumnHeader Actions;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox consoleInput;
        private System.Windows.Forms.Button button4;
    }
}