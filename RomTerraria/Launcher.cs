using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;

namespace RomTerraria
{
    public partial class Launcher : Form
    {
        private Terraria.Main terraria = new Terraria.Main();
        private RomTerraria.Properties.Settings settings = new Properties.Settings();

        public Launcher()
        {
            InitializeComponent();
        }

        public int LaunchWorldID {
            get {
                if( selectWorld.SelectedItem == null ) return -1;
                return ((WorldData)selectWorld.SelectedItem).ID;
            }
        }

        public string LaunchWorldPassword {
            get { return (serverPassword.Text ?? "").Trim( ); }
        }

        public int LaunchWorldPort
        {
            get
            {
                int port = 7777;
                int.TryParse(networkPort.Text, out port);
                return port;
            }

        }
        public int LaunchWorldPlayers
        {
            get { int players = 8;
                int.TryParse(maxNetPlayers.Text, out players);
                return players;
            }
        }


        public class WorldData {
            public int ID;
            public string Name;
            public override string ToString( ) {
                return Name;
            }
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            networkPort.Text = settings.LoadWorldPort;
            serverPassword.Text = settings.LoadWorldPassword;
            maxNetPlayers.Text = settings.LoadWorldMaxNetPlayers;

            //Server stuff
            Terraria.Main.LoadWorlds( );
            selectWorld.Items.Clear( );
            
            for( int i = 0; i < 5; i++ ) {
                if( !String.IsNullOrWhiteSpace( Main.loadWorld[i] ) ) {
                    selectWorld.Items.Add( new WorldData( ) { ID = i, Name = Main.loadWorld[i] } );
                }
            }
        }

        private void ResetPortClick( object sender, EventArgs e ) {
            networkPort.Text = "7777";
        }

        private void launchServer_Click(object sender, EventArgs e)
        {
            settings.LoadWorldPassword = serverPassword.Text;
            settings.LoadWorldPort = networkPort.Text;
            settings.LoadWorldMaxNetPlayers = maxNetPlayers.Text;
            settings.Save();
        }

        private void networkPort_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
