using System;
using System.IO;
using System.Windows.Forms;
using Terraria;

namespace TerrariaHooker
{
    public partial class Launcher : Form
    {
        private Terraria.Main terraria = new Terraria.Main();
        private TerrariaHooker.Properties.Settings settings = new Properties.Settings();

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
            Main.dedServ = true;
            Main.LoadWorlds( );
            selectWorld.Items.Clear( );

            var n = Main.loadWorld.Length;
            for( var i = 0; i < n; i++ ) {
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
