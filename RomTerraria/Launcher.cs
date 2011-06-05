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

        public bool InstallHealthTrainer
        {
            get
            {
                return enableTrainer.Checked;
            }
        }

        public bool InstallManaTrainer
        {
            get
            {
                return enableManaRecharge.Checked;
            }
        }

        public bool InstallMinimap
        {
            get
            {
                return enableMinimap.Checked;
            }
        }

        public bool InstallSurroundingAreaMinimap
        {
            get
            {
                return enableRealTimeMinimap.Checked;
            }
        }

        public bool InstallUncheckShadowOrb
        {
            get
            {
                return turnOffShadowOrbSmashed.Checked;
            }
        }


        public bool InstallPony
        {
            get
            {
                return enablePony.Checked;
            }
        }


        public int RefreshTimer {
            get { return howOftenToRecharge.Value; }
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

        private void launchTerraria_Click(object sender, EventArgs e)
        {

        }


        private void createCraftables_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();


            Terraria.Recipe.SetupRecipes();
            var recipes = Terraria.Main.recipe;


            foreach (var r in recipes)
            {
                if (r != null)
                {
                    string s = r.createItem.name + ":";
                    foreach (var i in r.requiredItem)
                    {
                        if (i != null && i.stack > 0)
                        {
                            s += String.Format(" {0} ({1}),", i.name, i.stack);
                        }
                    }
                    if (r.requiredTile[0] >= 0)
                    {
                        string tile = r.requiredTile[0].ToString();
                        switch (r.requiredTile[0])
                        {
                            case 13: tile = "Alchemy Station"; break;
                            case 14: tile = "Table"; break;
                            case 16: tile = "Anvil"; break;
                            case 17: tile = "Furnace"; break;
                            case 18: tile = "Workbench"; break;
                            case 26: tile = "Demon Altar"; break;
                        }
                        s += String.Format(" required to be by {0}", tile);
                    }
                    sb.AppendLine(s.TrimEnd(','));
                }
            }

            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\TerrariaCraftables.txt"))
            {
                sw.Write(sb.ToString());
            }

            for (int i = 0; i <= Terraria.Main.recipe.GetUpperBound(0); i++ )
            {
                Terraria.Main.recipe[i] = null;
            }
            Terraria.Main.recipe[0] = new Terraria.Recipe();
            Terraria.Recipe.numRecipes = 0;
            MessageBox.Show("TerrariaCraftables.txt is in your documents folder.");
        }


        private void letMeMakeItEasier_CheckChanged( object sender, EventArgs e ) {
            if( letMeMakeItEasier.Checked ) {
                var answer = MessageBox.Show( "Are you sure you want to enable cheats?", 
                                              "Warning",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Warning );
                if( answer == System.Windows.Forms.DialogResult.No ) {
                    letMeMakeItEasier.Checked = false;
                }
            }

            foreach( Control c in cheatContainer.Controls ) c.Visible = letMeMakeItEasier.Checked;
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
