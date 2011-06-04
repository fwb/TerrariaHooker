using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ConsoleRedirection;

namespace RomTerraria {

    
    public partial class ServerConsole : Form {

        public static SockHook sockHook;

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

        
        public ServerConsole() {
            //mih = m;
            InitializeComponent( );
            LoadLauncherSettings( );
            _writer = new StdOutRedirect(textBox1);
            // Redirect the out Console stream
            Console.SetOut(_writer);


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

        private void SpawnMeteor( ) {
            WorldEvents.SpawnMeteor( );
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

        private void SpawnMeteorButtonClick( object sender, EventArgs e ) {
            SpawnMeteor( );
        }

        private void RefreshPlayersClick( object sender, EventArgs e ) {
            LoadPlayerInfo( );
        }

        private void msgText_TextChanged(object sender, EventArgs e)
        {

        }


        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
         
 
        }

        
    }

}
