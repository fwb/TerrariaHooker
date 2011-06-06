using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ConsoleRedirection;
using Microsoft.Win32.SafeHandles;
using Terraria;

namespace RomTerraria {

    
    public partial class ServerConsole : Form {

        public static SockHook sockHook;

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
        private MakeItHarder mih;
        private delegate void AddChatLineCallback( string text );
        private delegate void SpawnMeteorCallback();
        public static Stream _out;

        
        public ServerConsole() {
            //mih = m;
            InitializeComponent( );
            LoadLauncherSettings( );
            _writer = new StdOutRedirect(textBox1);
            // Redirect the out Console stream
            _out = Console.OpenStandardOutput();

            Console.SetOut(_writer);
            sockHook = new SockHook();

            //LoadTerrariaAssembly( );
        }

        private void LoadLauncherSettings( ) {
            //enableInvasion.Checked = mih.InfiniteInvasion;
            //enableBloodMoon.Checked = mih.InfiniteBloodMoon;
            //enableEyeSpawn.Checked = mih.InfiniteEye;
        }

        private void LoadPlayerInfo( ) {
            playerList.Items.Clear( );
            for( var i = 0; i < 8; i++ ) {
                var p = Terraria.Main.player[i];
                if( p.active ) {
                    playerList.Items.Add( new PlayerInfo( i, p.name ) );
                }
            }
        }

        private void LoadNPCInfo( ) {
            npcPicker.Items.Clear( );
            var npc = new Terraria.NPC( );
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

        // delegates, wtf? -- amckhome@tpg.com.au
        public void AddChatLine( string msg ) {
            if( ConsoleText.InvokeRequired ) {
                var d = new AddChatLineCallback(AddChatLine);
                BeginInvoke( d, new object[] { msg } );
            } else {
                ConsoleText.AppendText( Environment.NewLine + msg );
            }
        }

        private void SpawnNearPlayer( int playerNum, int npcType, bool useNearSpawn ) {
            if( useNearSpawn ) {
                Terraria.NPC.SpawnOnPlayer( playerSelectSlider.Value, npcType );
            } else {
                //Calculate our own spawn location
                var position = Terraria.Main.player[playerNum].position;
                Terraria.NPC.NewNPC( (int)position.X, (int)position.Y, npcType );
            }
        }

        private void SendBroadcast( string msg ) {
            for( var i = 0; i < 8; i++ ) {
                if( Terraria.Main.player[i].active ) {
                    // last 3 args are RGB color values
                    Terraria.NetMessage.SendData( (int)MessageTypes.BROADCAST,
                                                  i, -1, msg, 8, 204f, 153f, 0f );
                }
            }
        }

        private void SendMessageClick( object sender, EventArgs e ) {
            if( msgText.Text != null ) {
                ConsoleText.AppendText( Environment.NewLine + msgText.Text );
                SendBroadcast( msgText.Text );
                msgText.Clear( );
            }
        }

        private void MsgTextKeyPress( object sender, KeyPressEventArgs e ) {
            if( e.KeyChar == (char)13 ) { // enter key pressed
                SendMessageClick( sender, e );
            }
        }

        
        private void PlayerSelectSliderScroll( object sender, EventArgs e ) {
            var playerNum = playerSelectSlider.Value;
            string playerName;
            if( Terraria.Main.player[playerNum].active ) {
                playerName = Terraria.Main.player[playerNum].name;
            } else {
                playerName = "(Inactive)";
            }
            playerSelectLabel.Text = playerName;
        }

        private void SpawnButtonClick( object sender, EventArgs e ) {
            //MessageBox.Show( String.Format( "Index [{0}], PlayerNum [{1}]", mobPicker.SelectedIndex,
            //                                playerSelectSlider.Value ) );
            //if( npcPicker.SelectedIndex > 0 ) {
            if( npcPicker.SelectedItem != null ) {
                var npc = npcPicker.SelectedItem as NPCInfo;
                SpawnNearPlayer( playerSelectSlider.Value, npc.Type, useNearSpawn.Checked );
            }
        }

        private void CloseButtonClick( object sender, EventArgs e ) {
            Hide( );
        }

        private void LoadNPCsClick( object sender, EventArgs e ) {
            LoadNPCInfo( );
        }

        private void EnableInvasionCheckedChanged( object sender, EventArgs e ) {
            mih.InfiniteInvasion = enableInvasion.Checked;
        }

        private void EnableBloodMoonCheckedChanged( object sender, EventArgs e ) {
            mih.InfiniteBloodMoon = enableBloodMoon.Checked;
        }

        private void EnableEyeSpawnCheckedChanged( object sender, EventArgs e ) {
            mih.InfiniteEye = enableEyeSpawn.Checked;
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
       
            textBox1.AppendText(consoleInput.Text + Environment.NewLine);
            //hooray!
            char[] c = consoleInput.Text.ToCharArray();
            var n = new INPUT_RECORD[c.Length+1];
            int index = 0;
            foreach (char i in c)
            {
                n[index].EventType = 0x0001;
                //n[index].KeyEvent = new KEY_EVENT_RECORD();
                n[index].KeyEvent.bKeyDown = 1;
                n[index].KeyEvent.wRepeatCount = 1;
                n[index].KeyEvent.UnicodeChar = i;
                index++;
            }

            n[index].EventType = 0x0001;
            //n[index].KeyEvent = new KEY_EVENT_RECORD();
            n[index].KeyEvent.bKeyDown = 1;
            n[index].KeyEvent.dwControlKeyState = 0;
            n[index].KeyEvent.wRepeatCount = 1;
            n[index].KeyEvent.wVirtualKeyCode = 0x0D;
            n[index].KeyEvent.UnicodeChar = (char) 0x0D;
            n[index].KeyEvent.wVirtualScanCode = (ushort)MapVirtualKey(0x0D, 0x00);

            uint events;
            //WriteConsoleInput(Program.STDIN_HANDLE, n, (uint) c.Length + 1, out events);
            WriteConsoleInput(Program.STDIN_HANDLE, n, (uint)c.Length + 1, out events);
            consoleInput.Clear();
            
        }


        private void consoleInputKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            { // enter key pressed
                button3_Click(sender, e);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sockHook.InitializeHooks();
        }

        
    }

}
