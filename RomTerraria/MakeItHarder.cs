using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace RomTerraria
{
    public class MakeItHarder : GameComponent
    {
        public bool InfiniteInvasion { get; set; } // Sets shadow orb flag to "smashed" and calls StartInvasion every frame

        public bool TurnOffInvasion { get; set; } // Sets shadow orb flag to "unsmashed"

        public bool InfiniteBloodMoon { get; set; } // Sets blood moon to true

        public bool InfiniteEye { get; set; }

        public bool EnableConsole { get; set; }

        public bool HardcoreMode { get; set; }

        Assembly terrariaAssembly;

        Type worldGen;
        Type main;
        Type netplay;
        //Type npc;

        FieldInfo bloodMoon;
        FieldInfo invasionSize;
        FieldInfo shadowOrbSmashed;
        FieldInfo spawnEye;
        FieldInfo time;
        public FieldInfo serverSock;

        public MethodInfo dropMeteor;
        MethodInfo erasePlayer;
        MethodInfo saveAndQuit;
        MethodInfo spawnOnPlayer;
        MethodInfo startInvasion;
        
        bool eyeSpawnedToday = false;
        bool consoleToggled;

        public static ServerConsole serverConsole;
        

        public MakeItHarder(Game g) : base(g) 
        {
            terrariaAssembly = Assembly.GetAssembly(typeof(Terraria.Main));
            if (terrariaAssembly != null)
            {
                worldGen = terrariaAssembly.GetType("Terraria.WorldGen");
                main = terrariaAssembly.GetType("Terraria.Main");
                netplay = terrariaAssembly.GetType("Terraria.NetPlay");
                //npc = terrariaAssembly.GetType( "Terraria.NPC" );

                foreach (var f in worldGen.GetFields())
                {
                    if (f.Name == "shadowOrbSmashed")
                    {
                        shadowOrbSmashed = f;
                    }
                    if( f.Name == "spawnEye" ) {
                        spawnEye = f;
                    }
                }
 
                foreach( var f in worldGen.GetMethods( BindingFlags.Static | BindingFlags.NonPublic ) ) {
                    if( f.Name == "dropMeteor" ) {
                        dropMeteor = f;
                    }
                    if( f.Name=="SaveAndQuit" ) {
                        saveAndQuit = f;
                    }
                }

                foreach (var f in main.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
                {
                    if (f.Name == "StartInvasion")
                    {
                        startInvasion = f;
                    }
                    if( f.Name == "SpawnOnPlayer" ) {
                        spawnOnPlayer = f;
                    }
                    if( f.Name == "ErasePlayer" ) {
                        erasePlayer = f;
                    }
                }

                //for Commands.cs
             

                foreach (var f in main.GetFields())
                {
                    if (f.Name == "bloodMoon")
                    {
                        bloodMoon = f;
                    }
                    if (f.Name == "invasionSize")
                    {
                        invasionSize = f;
                    }
                    if( f.Name == "time" ) {
                        time = f;
                    }
                }

                

            }
        }

        public override void Update(GameTime gameTime)
        {
            if( HardcoreMode && !Terraria.Main.gameMenu && saveAndQuit != null && erasePlayer != null ) {
                var p = Terraria.Main.player[Terraria.Main.myPlayer];
                if( p.dead ) {
                    p.statLifeMax = -1000;
                    p.statManaMax = -1000;
                    if( !p.name.EndsWith( "(deceased") ) {
                        p.name += " (deceased)";
                    }
                    for( var i = 0; i < p.inventory.Length; i++ ) {
                        p.inventory[i] = new Terraria.Item( );
                    }
                    p.eyeColor.A = 63;
                    p.hairColor.A = 63;
                    p.pantsColor.A = 63;
                    p.shirtColor.A = 63;
                    p.shoeColor.A = 63;
                    p.skinColor.A = 63;
                    p.underShirtColor.A = 63;

                    saveAndQuit.Invoke( null, null );
                }
            }

            //if (Terraria.Main.netMode != 0) return; // No trainer in MP
            if( Terraria.Main.netMode == 1 ) return; // 0 = SP, 1 = MP Client, 2 = MP Server    

            #region MP_Server_Specific
            if( Terraria.Main.netMode == 2 ) {
                var keyState = Terraria.Main.keyState;
                if( keyState.IsKeyDown( Keys.F7 ) && !consoleToggled ) {
                    consoleToggled = true;
                } else if (keyState.IsKeyUp( Keys.F7) && consoleToggled ) {
                    consoleToggled = false;
                    if( serverConsole.Visible == true ) {
                        serverConsole.Hide( );
                    } else {
                        serverConsole.Show( );
                    }
                }
                /**
                 * Append chat lines to server console - amckhome@tpg.com.au
                 * Not a 100% solution as the Terraria code to handle NetMessage processing
                 * resides in the ServerLoop thread. There will be sync issues and we won't
                 * catch all messages unless this is addressed.
                 **/
                //foreach( var n in Terraria.NetMessage.buffer ) {
                //    if( (n.checkBytes) && (n.readBuffer[4] == 0x19) ) {
                //        lock( n ) {
                //            if( n.messageLength == 0 ) {
                //                n.messageLength = BitConverter.ToInt32( n.readBuffer, 0 ) + 4;
                //            }

                //            var start = 4;

                //            var length = n.messageLength - 4;
                //            var player = n.readBuffer[start + 1];
                //            var text = Encoding.ASCII.GetString( n.readBuffer, start + 5, length - 5 );
                //            serverConsole.AddChatLine( string.Format( "<{0}> {1}", Terraria.Main.player[player].name, text ) );
                //            n.totalData = 0;
                //            n.messageLength = 0;
                //            n.checkBytes = false;
                //        }
                //    }
                //}
            }
            #endregion MP_Server_Specific


            if (TurnOffInvasion && shadowOrbSmashed != null && invasionSize != null)
            {
                shadowOrbSmashed.SetValue(null, false);
                invasionSize.SetValue(null, 0);
            }

            if (InfiniteInvasion && shadowOrbSmashed != null && startInvasion != null) // Some defensive coding
            {
                Terraria.Main.invasionDelay = 0;
                bool wasOrbSmashed = (bool)shadowOrbSmashed.GetValue(null);
                shadowOrbSmashed.SetValue(null, true);
                startInvasion.Invoke(null, null);
                shadowOrbSmashed.SetValue(null, wasOrbSmashed);
            }

            if (InfiniteBloodMoon && bloodMoon != null)
            {
                bloodMoon.SetValue(null, true);
            }

            if( InfiniteEye && !Terraria.Main.gameMenu 
                && spawnEye != null && time != null ) {
                if( !eyeSpawnedToday ) {
                    spawnEye.SetValue( null, true );
                    eyeSpawnedToday = true;
                } else {
                    if( (double)time.GetValue( null ) > 32400.0 ) {
                        eyeSpawnedToday = false;
                    }
                }

            }

            base.Update(gameTime);
        }

        //public override void Initialize( ) {
        //    base.Initialize( );
        //    NetplayOverride.InterceptNetwork( );
        //}

        public static void Debug( string msg ) {
            serverConsole.AddChatLine( msg );
        }
    }
}
