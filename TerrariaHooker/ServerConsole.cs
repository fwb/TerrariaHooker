using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using Microsoft.Xna.Framework;
using Terraria;

namespace TerrariaHooker {

    
    public partial class ServerConsole : Form {

        public static SockHook sockHook;
        private static int selectedPlayer = -1;
        private static Properties.Settings settings = new Properties.Settings();


        private delegate void NCallback();

        [DllImport("kernel32.dll",
            EntryPoint = "WriteConsoleInput",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        static extern bool WriteConsoleInput(SafeFileHandle hConsoleInput,
           INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEventsWritten);

        [DllImport("user32.dll",
            EntryPoint = "MapVirtualKey",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);


        public class PlayerInfo : ListViewItem {
            public int Id { get; set; }
            public string PlayerName { get; set; }

            public PlayerInfo( int id, string name ) {
                Id = id;
                PlayerName = name;
            }

            public override string ToString( ) {
                return PlayerName;
            }

        }

        public class NPCInfo {
            public int Type { get; set; }
            public string Name { get; set; }
            public int Damage { get; set; }
            public int Defense { get; set; }
            public int LifeMax { get; set; }
            public Single DropValue { get; set; }

            public NPCInfo( int id, string name, int damage, int defense, 
                            int lifemax, Single dropvalue ) {
                Type = id;
                Name = name;
                Damage = damage;
                Defense = defense;
                LifeMax = lifemax;
                DropValue = dropvalue;
            }

            public override string ToString( ) {
                return Name;
            }
        }


        private TextWriter _writer = null;
        //private delegate void AddChatLineCallback( string text );
        private delegate void SpawnMeteorCallback();
        public static Stream _out;

        
        public ServerConsole() {

            InitializeComponent( );
            LoadLauncherSettings( );
            _writer = new StdOutRedirect(textBox1);
            // Redirect the out Console stream
            _out = Console.OpenStandardOutput();

            Console.SetOut(_writer);
            sockHook = new SockHook();

            check_wlEnabled.Checked = settings.EnableWhitelist;
            check_anonLoginEnabled.Checked = settings.EnableAnonLogin;

        }

        protected override void OnLoad( EventArgs e ) {
            base.OnLoad( e );
            sockHook.InitializeHooks( );
        }

        private void LoadLauncherSettings( ) {
            //enableInvasion.Checked = mih.InfiniteInvasion;
            //enableBloodMoon.Checked = mih.InfiniteBloodMoon;
            //enableEyeSpawn.Checked = mih.InfiniteEye;
        }

        private void LoadPlayerInfo( ) {
            playerList.Items.Clear( );
            for( var i = 0; i < Terraria.Main.maxNetPlayers; i++ ) {
                var p = Terraria.Main.player[i];
                if( p.active )
                {
                    playerList.Items.Add(new ListViewItem(new string[] {
                                                                        i.ToString(),
                                                                        p.name,
                                                                        Netplay.serverSock[i].tcpClient.Client.RemoteEndPoint.ToString(),
                                                                        Commands.whitelisted[i].ToString(),}) );
                }
            }
        }

        private void LoadNPCInfo( ) {
            npcPicker.Items.Clear( );
            var npc = new NPC( );
            for( var i=1; i<=45; i++ ) {
                try {
                    npc.SetDefaults( i );
                } catch {
                    MessageBox.Show( string.Format( "Error loading NPC type [{0}]", i ) );
                }

                if( npc.name != null ) {
                    npcPicker.Items.Add( new NPCInfo( npc.type, npc.name, npc.damage,
                                                      npc.defense, npc.lifeMax,
                                                      npc.value ) );
                }
            }
            npcPicker.SelectedIndex = 0;
        }

        private void SpawnNearPlayer( int playerNum, int npcType, bool useNearSpawn ) {
            //todo: i want to move implementation to Commands.cs
            //Calculate our own spawn location
            var position = Main.player[playerNum].position;
            NPC.NewNPC( (int)position.X, (int)position.Y, npcType );
        }

        //private void SendBroadcast( string msg ) {
        //    for( var i = 0; i < 8; i++ ) {
        //        if( Terraria.Main.player[i].active ) {
        //            // last 3 args are RGB color values
        //            Terraria.NetMessage.SendData( (int)MessageTypes.BROADCAST,
        //                                          i, -1, msg, 8, 204f, 153f, 0f );
        //        }
        //    }
        //}


        private void SpawnButtonClick( object sender, EventArgs e ) {
            //todo: i want to move implementation to Commands.cs
            if( npcPicker.SelectedItem != null && selectedPlayer != -1) {
                var npc = npcPicker.SelectedItem as NPCInfo;
                SpawnNearPlayer( selectedPlayer, npc.Type, false);
            }
        }

        private void CloseButtonClick( object sender, EventArgs e )
        {
            settings.EnableAnonLogin = Commands.allowUnwhiteLogin;
            settings.EnableWhitelist = Whitelist.IsActive;
            settings.Save();

            //save and quit. redirects console output to usual stdout first.
            var n = new StreamWriter(_out);
            Console.SetOut(n);
            Close();
            //send exit command.
            sendLineToConsole("exit");
            //this.Close();
        }

        private void LoadNPCsClick( object sender, EventArgs e ) {
            LoadNPCInfo( );
        }

        private void SpawnMeteorButtonClick( object sender, EventArgs e )
        {
            SpawnMeteor();
        }

        private void SpawnMeteor()
        {
            SpawnMeteorCallback d = new SpawnMeteorCallback(WorldEvents.SpawnMeteorCB);
            Invoke(d, null);
        }


        private void RefreshPlayersClick( object sender, EventArgs e )
        {
            LoadPlayerInfo( );
        }

        private void button3_Click(object sender, EventArgs e)
        {
       
            if (consoleInput.Text.Substring(0,1) == ".")
            {
                textBox1.AppendText(consoleInput.Text + Environment.NewLine);
                var finalBuf = new byte[consoleInput.Text.Length + 9];
                //0xFC is our player ID for the console. hopefully never going to really be used
                var buf = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x19, 0xFC, 0x00, 0x00, 0x00};

                var t = Encoding.ASCII.GetBytes(consoleInput.Text);
                Buffer.BlockCopy(buf,0,finalBuf,0,buf.Length);
                Buffer.BlockCopy(t, 0, finalBuf, 9, t.Length);
                consoleInput.Clear();
                Commands.HandleChatMsg(finalBuf);
                return;

            }
            textBox1.AppendText(consoleInput.Text + Environment.NewLine);
            //hooray!
            sendLineToConsole(consoleInput.Text);
            //UpdateForm();

        }

        private void sendLineToConsole(string text)
        {
            char[] c = text.ToCharArray();
            var n = new INPUT_RECORD[c.Length + 1];
            int index = 0;
            foreach (char i in c)
            {
                n[index].EventType = 0x0001;
                n[index].KeyEvent.bKeyDown = 1;
                n[index].KeyEvent.wRepeatCount = 1;
                n[index].KeyEvent.UnicodeChar = i;
                index++;
            }

            n[index].EventType = 0x0001;
            n[index].KeyEvent.bKeyDown = 1;
            n[index].KeyEvent.dwControlKeyState = 0;
            n[index].KeyEvent.wRepeatCount = 1;
            n[index].KeyEvent.wVirtualKeyCode = 0x0D;
            n[index].KeyEvent.UnicodeChar = (char)0x0D;
            n[index].KeyEvent.wVirtualScanCode = (ushort)MapVirtualKey(0x0D, 0x00);

            uint events;
            WriteConsoleInput(Program.STDIN_HANDLE, n, (uint)c.Length + 1, out events);
            consoleInput.Clear();
        }


        private void consoleInputKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            { // enter key pressed
                if (consoleInput.Text == "")
                    return;
                button3_Click(sender, e);
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sockHook.InitializeHooks();
        }

        private void consoleInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void playerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (playerList.FocusedItem == null)
            {
                selectedPlayer = -1;
                label2.Text = "Selected player: none";
                return;
            }
            int id;
            if (int.TryParse(playerList.FocusedItem.SubItems[0].Text, out id) == false)
                return;
            selectedPlayer = id;
            label2.Text = "Selected player: " + playerList.FocusedItem.SubItems[1].Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (playerList.FocusedItem == null)
                return;
            int id;
            if (int.TryParse(playerList.FocusedItem.SubItems[0].Text, out id) == false)
                return;

            Commands.kickUser(id);
            LoadPlayerInfo();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (playerList.FocusedItem == null)
                return;
            int id;
            if (int.TryParse(playerList.FocusedItem.SubItems[0].Text, out id) == false)
                return;

            Commands.banUser(id);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (playerList.FocusedItem == null)
                return;
            int id;
            if (int.TryParse(playerList.FocusedItem.SubItems[0].Text, out id) == false)
                return;

            /* TODO: now that anon login is out of the question, these buttons are probably
             * not necessary except for removing people that are being stupid, but ban beats
             * that outright.
             */
            var ip = Utils.ParseEndPointAddr(playerList.FocusedItem.SubItems[2].Text);
            Whitelist.AddEntry(ip);
            Commands.whitelisted[id] = true;
            Commands.SendChatMsg("You are now whitelisted.", id, Color.Yellow);
            LoadPlayerInfo();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (playerList.FocusedItem == null)
                return;
            int id;
            if (int.TryParse(playerList.FocusedItem.SubItems[0].Text, out id) == false)
                return;

            var ip = Utils.ParseEndPointAddr(playerList.FocusedItem.SubItems[2].Text);
            Whitelist.RemoveEntry(ip);
            Commands.whitelisted[id] = false;
            Commands.SendChatMsg("You are no longer whitelisted.", id, Color.Yellow);
            LoadPlayerInfo();
        }


        private void check_wlEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Whitelist.IsActive = check_wlEnabled.Checked;
        }

        private void check_anonLoginEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Commands.allowUnwhiteLogin = check_anonLoginEnabled.Checked;
        }
        private void tabControl1_SelectedIndexchanged(object sender, EventArgs e)
        {
            switch (((TabControl) sender).SelectedIndex)
            {
                case 4:
                    check_wlEnabled.Checked = Whitelist.IsActive;
                    check_anonLoginEnabled.Checked = Commands.allowUnwhiteLogin;
                    break;
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
                check_SpawnDisallowBreak.Enabled = check_ProtectSpawn.Checked;
                check_SpawnDisallowPlace.Enabled = check_ProtectSpawn.Checked;
                check_SpawnDisallowUse.Enabled = check_ProtectSpawn.Checked;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            int width;
            int height;

            if (int.TryParse(text_protectWidth.Text, out width) == false)
            {
                MessageBox.Show("Width and Height must be positive numbers");
                return;
            }

            if (int.TryParse(text_protectHeight.Text, out height) == false)
            {
                MessageBox.Show("Width and Height must be positive numbers");
                return;
            }

            Commands.SetProtectWidth = width;
            Commands.SetProtectHeight = height;

            Commands.protectSpawn = check_ProtectSpawn.Checked;
            Commands.spawnAllowBreak = check_SpawnDisallowBreak.Checked;
            Commands.spawnAllowUse = check_SpawnDisallowUse.Checked;
            Commands.spawnAllowPlace = check_SpawnDisallowPlace.Checked;

        }

        private void button_EnemyApply_Click(object sender, EventArgs e)
        {
            Commands.disableSpawns(check_DisableEnemies.Checked);
            button_EnemyApply.Enabled = false;
            if (check_enableCustomRates.Checked && !check_DisableEnemies.Checked)
            {
                int a = track_spawnRate.Value; //spawnrate
                int b; //maxspawns
                if (int.TryParse(text_maxSpawns.Text, out b) == false)
                {
                    MessageBox.Show("Max Spawns must be a number");
                    return;
                }
                
                Commands.setSpawnValues(a, b);
                label_customRateEnabled.Text = check_enableCustomRates.Checked ? "Custom Rates: ENABLED" : "Custom Rates: DISABLED";
                label_maxSpawns.Text = String.Format("Max Spawns: {0}", b);
                label_spawnInterval.Text = String.Format("Spawn Rate: {0} (ms)", track_spawnRate.Value); 

            }
            label_spawnsEnabled.Text = !check_DisableEnemies.Checked ? "Spawns: ENABLED" : "Spawns: DISABLED";

        }

        private void button10_Click(object sender, EventArgs e)
        {
            track_spawnRate.Value = 700;
            text_maxSpawns.Text = "4";
            button_EnemyApply.Enabled = true;
        }

        private void check_DisableEnemies_CheckedChanged(object sender, EventArgs e)
        {
            panel_customSpawn.Enabled = !check_DisableEnemies.Checked;
            button_EnemyApply.Enabled = true;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void check_enableCustomRates_CheckedChanged(object sender, EventArgs e)
        {
            panel_CustomInternal.Enabled = check_enableCustomRates.Checked;
            button_EnemyApply.Enabled = true;
        }

        private void text_maxSpawns_TextChanged(object sender, EventArgs e)
        {
            button_EnemyApply.Enabled = true;
        }

        private void track_spawnRate_Scroll(object sender, EventArgs e)
        {
            button_EnemyApply.Enabled = true;
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

    }

}
