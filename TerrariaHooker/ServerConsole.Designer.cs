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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.text_protectHeight = new System.Windows.Forms.TextBox();
            this.text_protectWidth = new System.Windows.Forms.TextBox();
            this.check_SpawnDisallowUse = new System.Windows.Forms.CheckBox();
            this.check_ProtectSpawn = new System.Windows.Forms.CheckBox();
            this.check_SpawnDisallowPlace = new System.Windows.Forms.CheckBox();
            this.check_SpawnDisallowBreak = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.check_wlEnabled = new System.Windows.Forms.CheckBox();
            this.check_anonLoginEnabled = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.check_DisableEnemies = new System.Windows.Forms.CheckBox();
            this.button_EnemyApply = new System.Windows.Forms.Button();
            this.check_enableCustomRates = new System.Windows.Forms.CheckBox();
            this.text_maxSpawns = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.panel_customSpawn = new System.Windows.Forms.Panel();
            this.track_spawnRate = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.panel_CustomInternal = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label_spawnsEnabled = new System.Windows.Forms.Label();
            this.label_customRateEnabled = new System.Windows.Forms.Label();
            this.label_maxSpawns = new System.Windows.Forms.Label();
            this.label_spawnInterval = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.panel_customSpawn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.track_spawnRate)).BeginInit();
            this.panel_CustomInternal.SuspendLayout();
            this.groupBox7.SuspendLayout();
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
            this.tabPage5.Size = new System.Drawing.Size(483, 339);
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
            this.textBox1.Size = new System.Drawing.Size(477, 312);
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
            this.tabPage2.Size = new System.Drawing.Size(483, 339);
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
            this.tabPage3.Size = new System.Drawing.Size(483, 339);
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
            this.groupBox3.Enabled = false;
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
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(483, 339);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Text = "Options";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.button9);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.text_protectHeight);
            this.groupBox5.Controls.Add(this.text_protectWidth);
            this.groupBox5.Controls.Add(this.check_SpawnDisallowUse);
            this.groupBox5.Controls.Add(this.check_ProtectSpawn);
            this.groupBox5.Controls.Add(this.check_SpawnDisallowPlace);
            this.groupBox5.Controls.Add(this.check_SpawnDisallowBreak);
            this.groupBox5.Location = new System.Drawing.Point(253, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(217, 195);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Spawn";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Box Width:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button9
            // 
            this.button9.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button9.Location = new System.Drawing.Point(6, 166);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(204, 23);
            this.button9.TabIndex = 8;
            this.button9.Text = "Apply";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(109, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Box Height:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // text_protectHeight
            // 
            this.text_protectHeight.Location = new System.Drawing.Point(112, 138);
            this.text_protectHeight.Name = "text_protectHeight";
            this.text_protectHeight.Size = new System.Drawing.Size(98, 20);
            this.text_protectHeight.TabIndex = 9;
            this.text_protectHeight.Text = "10";
            // 
            // text_protectWidth
            // 
            this.text_protectWidth.Location = new System.Drawing.Point(6, 138);
            this.text_protectWidth.Name = "text_protectWidth";
            this.text_protectWidth.Size = new System.Drawing.Size(96, 20);
            this.text_protectWidth.TabIndex = 4;
            this.text_protectWidth.Text = "10";
            // 
            // check_SpawnDisallowUse
            // 
            this.check_SpawnDisallowUse.AutoSize = true;
            this.check_SpawnDisallowUse.Enabled = false;
            this.check_SpawnDisallowUse.Location = new System.Drawing.Point(6, 88);
            this.check_SpawnDisallowUse.Name = "check_SpawnDisallowUse";
            this.check_SpawnDisallowUse.Size = new System.Drawing.Size(120, 17);
            this.check_SpawnDisallowUse.TabIndex = 7;
            this.check_SpawnDisallowUse.Text = "Disallow using items";
            this.check_SpawnDisallowUse.UseVisualStyleBackColor = true;
            this.check_SpawnDisallowUse.Visible = false;
            // 
            // check_ProtectSpawn
            // 
            this.check_ProtectSpawn.AutoSize = true;
            this.check_ProtectSpawn.Location = new System.Drawing.Point(6, 19);
            this.check_ProtectSpawn.Name = "check_ProtectSpawn";
            this.check_ProtectSpawn.Size = new System.Drawing.Size(188, 17);
            this.check_ProtectSpawn.TabIndex = 4;
            this.check_ProtectSpawn.Text = "Protect Spawn [EXPERIMENTAL]";
            this.check_ProtectSpawn.UseVisualStyleBackColor = true;
            this.check_ProtectSpawn.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // check_SpawnDisallowPlace
            // 
            this.check_SpawnDisallowPlace.AutoSize = true;
            this.check_SpawnDisallowPlace.Enabled = false;
            this.check_SpawnDisallowPlace.Location = new System.Drawing.Point(6, 65);
            this.check_SpawnDisallowPlace.Name = "check_SpawnDisallowPlace";
            this.check_SpawnDisallowPlace.Size = new System.Drawing.Size(136, 17);
            this.check_SpawnDisallowPlace.TabIndex = 6;
            this.check_SpawnDisallowPlace.Text = "Disallow placing blocks";
            this.check_SpawnDisallowPlace.UseVisualStyleBackColor = true;
            this.check_SpawnDisallowPlace.Visible = false;
            // 
            // check_SpawnDisallowBreak
            // 
            this.check_SpawnDisallowBreak.AutoSize = true;
            this.check_SpawnDisallowBreak.Enabled = false;
            this.check_SpawnDisallowBreak.Location = new System.Drawing.Point(6, 42);
            this.check_SpawnDisallowBreak.Name = "check_SpawnDisallowBreak";
            this.check_SpawnDisallowBreak.Size = new System.Drawing.Size(143, 17);
            this.check_SpawnDisallowBreak.TabIndex = 5;
            this.check_SpawnDisallowBreak.Text = "Disallow breaking blocks";
            this.check_SpawnDisallowBreak.UseVisualStyleBackColor = true;
            this.check_SpawnDisallowBreak.Visible = false;
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
            this.check_wlEnabled.Location = new System.Drawing.Point(6, 19);
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
            this.check_anonLoginEnabled.Enabled = false;
            this.check_anonLoginEnabled.Location = new System.Drawing.Point(6, 42);
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
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.panel_customSpawn);
            this.groupBox6.Controls.Add(this.button_EnemyApply);
            this.groupBox6.Controls.Add(this.check_DisableEnemies);
            this.groupBox6.Controls.Add(this.button10);
            this.groupBox6.Location = new System.Drawing.Point(13, 96);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(234, 204);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Enemies";
            // 
            // check_DisableEnemies
            // 
            this.check_DisableEnemies.AutoSize = true;
            this.check_DisableEnemies.Location = new System.Drawing.Point(7, 20);
            this.check_DisableEnemies.Name = "check_DisableEnemies";
            this.check_DisableEnemies.Size = new System.Drawing.Size(104, 17);
            this.check_DisableEnemies.TabIndex = 0;
            this.check_DisableEnemies.Text = "Disable Enemies";
            this.check_DisableEnemies.UseVisualStyleBackColor = true;
            this.check_DisableEnemies.CheckedChanged += new System.EventHandler(this.check_DisableEnemies_CheckedChanged);
            // 
            // button_EnemyApply
            // 
            this.button_EnemyApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_EnemyApply.Enabled = false;
            this.button_EnemyApply.Location = new System.Drawing.Point(7, 175);
            this.button_EnemyApply.Name = "button_EnemyApply";
            this.button_EnemyApply.Size = new System.Drawing.Size(142, 23);
            this.button_EnemyApply.TabIndex = 1;
            this.button_EnemyApply.Text = "Apply";
            this.button_EnemyApply.UseVisualStyleBackColor = true;
            this.button_EnemyApply.Click += new System.EventHandler(this.button_EnemyApply_Click);
            // 
            // check_enableCustomRates
            // 
            this.check_enableCustomRates.AutoSize = true;
            this.check_enableCustomRates.Location = new System.Drawing.Point(0, 3);
            this.check_enableCustomRates.Name = "check_enableCustomRates";
            this.check_enableCustomRates.Size = new System.Drawing.Size(164, 17);
            this.check_enableCustomRates.TabIndex = 2;
            this.check_enableCustomRates.Text = "Enable Custom Spawn Rates";
            this.check_enableCustomRates.UseVisualStyleBackColor = true;
            this.check_enableCustomRates.CheckedChanged += new System.EventHandler(this.check_enableCustomRates_CheckedChanged);
            // 
            // text_maxSpawns
            // 
            this.text_maxSpawns.Location = new System.Drawing.Point(0, 19);
            this.text_maxSpawns.Name = "text_maxSpawns";
            this.text_maxSpawns.Size = new System.Drawing.Size(75, 20);
            this.text_maxSpawns.TabIndex = 4;
            this.text_maxSpawns.Text = "4";
            this.text_maxSpawns.TextChanged += new System.EventHandler(this.text_maxSpawns_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(171, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Slower";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(-2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Max Spawns";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(155, 175);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(71, 23);
            this.button10.TabIndex = 7;
            this.button10.Text = "Reset";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // panel_customSpawn
            // 
            this.panel_customSpawn.BackColor = System.Drawing.Color.Transparent;
            this.panel_customSpawn.Controls.Add(this.panel_CustomInternal);
            this.panel_customSpawn.Controls.Add(this.check_enableCustomRates);
            this.panel_customSpawn.Location = new System.Drawing.Point(7, 38);
            this.panel_customSpawn.Name = "panel_customSpawn";
            this.panel_customSpawn.Size = new System.Drawing.Size(222, 131);
            this.panel_customSpawn.TabIndex = 5;
            // 
            // track_spawnRate
            // 
            this.track_spawnRate.Location = new System.Drawing.Point(-2, 58);
            this.track_spawnRate.Maximum = 1400;
            this.track_spawnRate.Minimum = 10;
            this.track_spawnRate.Name = "track_spawnRate";
            this.track_spawnRate.Size = new System.Drawing.Size(216, 42);
            this.track_spawnRate.TabIndex = 6;
            this.track_spawnRate.TickFrequency = 50;
            this.track_spawnRate.Value = 700;
            this.track_spawnRate.Scroll += new System.EventHandler(this.track_spawnRate_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Faster";
            // 
            // panel_CustomInternal
            // 
            this.panel_CustomInternal.Controls.Add(this.label9);
            this.panel_CustomInternal.Controls.Add(this.label11);
            this.panel_CustomInternal.Controls.Add(this.label8);
            this.panel_CustomInternal.Controls.Add(this.text_maxSpawns);
            this.panel_CustomInternal.Controls.Add(this.track_spawnRate);
            this.panel_CustomInternal.Enabled = false;
            this.panel_CustomInternal.Location = new System.Drawing.Point(1, 31);
            this.panel_CustomInternal.Name = "panel_CustomInternal";
            this.panel_CustomInternal.Size = new System.Drawing.Size(220, 100);
            this.panel_CustomInternal.TabIndex = 5;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label_spawnInterval);
            this.groupBox7.Controls.Add(this.label_maxSpawns);
            this.groupBox7.Controls.Add(this.label_customRateEnabled);
            this.groupBox7.Controls.Add(this.label_spawnsEnabled);
            this.groupBox7.Location = new System.Drawing.Point(254, 215);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(216, 100);
            this.groupBox7.TabIndex = 5;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "STATUS";
            // 
            // label_spawnsEnabled
            // 
            this.label_spawnsEnabled.AutoSize = true;
            this.label_spawnsEnabled.Location = new System.Drawing.Point(7, 20);
            this.label_spawnsEnabled.Name = "label_spawnsEnabled";
            this.label_spawnsEnabled.Size = new System.Drawing.Size(101, 13);
            this.label_spawnsEnabled.TabIndex = 0;
            this.label_spawnsEnabled.Text = "Spawns: ENABLED";
            // 
            // label_customRateEnabled
            // 
            this.label_customRateEnabled.AutoSize = true;
            this.label_customRateEnabled.Location = new System.Drawing.Point(7, 37);
            this.label_customRateEnabled.Name = "label_customRateEnabled";
            this.label_customRateEnabled.Size = new System.Drawing.Size(132, 13);
            this.label_customRateEnabled.TabIndex = 1;
            this.label_customRateEnabled.Text = "Custom Rates: DISABLED";
            // 
            // label_maxSpawns
            // 
            this.label_maxSpawns.AutoSize = true;
            this.label_maxSpawns.Location = new System.Drawing.Point(7, 54);
            this.label_maxSpawns.Name = "label_maxSpawns";
            this.label_maxSpawns.Size = new System.Drawing.Size(80, 13);
            this.label_maxSpawns.TabIndex = 2;
            this.label_maxSpawns.Text = "Max Spawns: 4";
            // 
            // label_spawnInterval
            // 
            this.label_spawnInterval.AutoSize = true;
            this.label_spawnInterval.Location = new System.Drawing.Point(7, 71);
            this.label_spawnInterval.Name = "label_spawnInterval";
            this.label_spawnInterval.Size = new System.Drawing.Size(124, 13);
            this.label_spawnInterval.TabIndex = 3;
            this.label_spawnInterval.Text = "Spawn Interval: 700 (ms)";
            this.label_spawnInterval.Click += new System.EventHandler(this.label13_Click);
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
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panel_customSpawn.ResumeLayout(false);
            this.panel_customSpawn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.track_spawnRate)).EndInit();
            this.panel_CustomInternal.ResumeLayout(false);
            this.panel_CustomInternal.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox check_SpawnDisallowUse;
        private System.Windows.Forms.CheckBox check_ProtectSpawn;
        private System.Windows.Forms.CheckBox check_SpawnDisallowPlace;
        private System.Windows.Forms.CheckBox check_SpawnDisallowBreak;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox text_protectHeight;
        private System.Windows.Forms.TextBox text_protectWidth;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button_EnemyApply;
        private System.Windows.Forms.CheckBox check_DisableEnemies;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox text_maxSpawns;
        private System.Windows.Forms.CheckBox check_enableCustomRates;
        private System.Windows.Forms.Panel panel_customSpawn;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar track_spawnRate;
        private System.Windows.Forms.Panel panel_CustomInternal;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label_spawnInterval;
        private System.Windows.Forms.Label label_maxSpawns;
        private System.Windows.Forms.Label label_customRateEnabled;
        private System.Windows.Forms.Label label_spawnsEnabled;
    }
}