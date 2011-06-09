namespace TerrariaHooker {
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.consoleInput = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.playerList = new System.Windows.Forms.ListView();
            this.col_ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_IP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Whitelist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RefreshPlayers = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.disableSpawns = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.loadNPCs = new System.Windows.Forms.Button();
            this.npcPicker = new System.Windows.Forms.ListBox();
            this.spawnButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.check_wlEnabled = new System.Windows.Forms.CheckBox();
            this.check_anonLoginEnabled = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button4 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(491, 365);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexchanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.consoleInput);
            this.tabPage5.Controls.Add(this.button3);
            this.tabPage5.Controls.Add(this.textBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(648, 339);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Console";
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
            this.consoleInput.Location = new System.Drawing.Point(3, 315);
            this.consoleInput.Name = "consoleInput";
            this.consoleInput.Size = new System.Drawing.Size(641, 23);
            this.consoleInput.TabIndex = 2;
            this.consoleInput.TextChanged += new System.EventHandler(this.consoleInput_TextChanged);
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
            this.textBox1.Size = new System.Drawing.Size(641, 312);
            this.textBox1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button8);
            this.tabPage4.Controls.Add(this.button7);
            this.tabPage4.Controls.Add(this.button6);
            this.tabPage4.Controls.Add(this.button5);
            this.tabPage4.Controls.Add(this.playerList);
            this.tabPage4.Controls.Add(this.RefreshPlayers);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(483, 339);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Clients";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Location = new System.Drawing.Point(412, 313);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(68, 23);
            this.button8.TabIndex = 5;
            this.button8.Text = "Remove";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button7.Location = new System.Drawing.Point(344, 313);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(62, 23);
            this.button7.TabIndex = 4;
            this.button7.Text = "Whitelist";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Location = new System.Drawing.Point(282, 313);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(47, 23);
            this.button6.TabIndex = 3;
            this.button6.Text = "Ban";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(229, 313);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(47, 23);
            this.button5.TabIndex = 2;
            this.button5.Text = "Kick";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // playerList
            // 
            this.playerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_ID,
            this.col_Name,
            this.col_IP,
            this.col_Whitelist});
            this.playerList.FullRowSelect = true;
            this.playerList.GridLines = true;
            this.playerList.Location = new System.Drawing.Point(3, 3);
            this.playerList.Name = "playerList";
            this.playerList.Size = new System.Drawing.Size(475, 304);
            this.playerList.TabIndex = 1;
            this.playerList.UseCompatibleStateImageBehavior = false;
            this.playerList.View = System.Windows.Forms.View.Details;
            this.playerList.SelectedIndexChanged += new System.EventHandler(this.playerList_SelectedIndexChanged);
            // 
            // col_ID
            // 
            this.col_ID.Text = "ID";
            this.col_ID.Width = 42;
            // 
            // col_Name
            // 
            this.col_Name.Text = "Name";
            this.col_Name.Width = 166;
            // 
            // col_IP
            // 
            this.col_IP.Text = "IP";
            this.col_IP.Width = 188;
            // 
            // col_Whitelist
            // 
            this.col_Whitelist.Text = "Whitelisted";
            this.col_Whitelist.Width = 120;
            // 
            // RefreshPlayers
            // 
            this.RefreshPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RefreshPlayers.Location = new System.Drawing.Point(3, 313);
            this.RefreshPlayers.Name = "RefreshPlayers";
            this.RefreshPlayers.Size = new System.Drawing.Size(75, 23);
            this.RefreshPlayers.TabIndex = 0;
            this.RefreshPlayers.Text = "Refresh";
            this.RefreshPlayers.UseVisualStyleBackColor = true;
            this.RefreshPlayers.Click += new System.EventHandler(this.RefreshPlayersClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.disableSpawns);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.loadNPCs);
            this.tabPage2.Controls.Add(this.npcPicker);
            this.tabPage2.Controls.Add(this.spawnButton);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(481, 339);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "NPCs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "Selected player: none";
            // 
            // disableSpawns
            // 
            this.disableSpawns.AutoSize = true;
            this.disableSpawns.Enabled = false;
            this.disableSpawns.Location = new System.Drawing.Point(305, 12);
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
            this.npcPicker.Location = new System.Drawing.Point(6, 49);
            this.npcPicker.Name = "npcPicker";
            this.npcPicker.Size = new System.Drawing.Size(275, 251);
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
            this.label3.Location = new System.Drawing.Point(6, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Choose a Mob:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(481, 339);
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
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(159, 92);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Survival";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(481, 339);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Text = "Options";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.check_wlEnabled);
            this.groupBox4.Controls.Add(this.check_anonLoginEnabled);
            this.groupBox4.Location = new System.Drawing.Point(13, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(234, 76);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Whitelist";
            // 
            // check_wlEnabled
            // 
            this.check_wlEnabled.AutoSize = true;
            this.check_wlEnabled.Location = new System.Drawing.Point(10, 19);
            this.check_wlEnabled.Name = "check_wlEnabled";
            this.check_wlEnabled.Size = new System.Drawing.Size(102, 17);
            this.check_wlEnabled.TabIndex = 0;
            this.check_wlEnabled.Text = "Enable Whitelist";
            this.check_wlEnabled.UseVisualStyleBackColor = true;
            this.check_wlEnabled.CheckedChanged += new System.EventHandler(this.check_wlEnabled_CheckedChanged);
            // 
            // check_anonLoginEnabled
            // 
            this.check_anonLoginEnabled.AutoSize = true;
            this.check_anonLoginEnabled.Location = new System.Drawing.Point(10, 42);
            this.check_anonLoginEnabled.Name = "check_anonLoginEnabled";
            this.check_anonLoginEnabled.Size = new System.Drawing.Size(199, 17);
            this.check_anonLoginEnabled.TabIndex = 1;
            this.check_anonLoginEnabled.Text = "Enable Anonymous (Restricted) login";
            this.check_anonLoginEnabled.UseVisualStyleBackColor = true;
            this.check_anonLoginEnabled.CheckedChanged += new System.EventHandler(this.check_anonLoginEnabled_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(418, 383);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Save and Quit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(304, 383);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(108, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "ReLoad Commands";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // ServerConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 413);
            this.ControlBox = false;
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Name = "ServerConsole";
            this.Text = "TerrariaHooker";
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button spawnButton;
        private System.Windows.Forms.ListBox npcPicker;
        private System.Windows.Forms.Button loadNPCs;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox disableSpawns;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox2;
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
        private System.Windows.Forms.ColumnHeader col_ID;
        private System.Windows.Forms.ColumnHeader col_Name;
        private System.Windows.Forms.ColumnHeader col_IP;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox consoleInput;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ColumnHeader col_Whitelist;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox check_wlEnabled;
        private System.Windows.Forms.CheckBox check_anonLoginEnabled;
    }
}