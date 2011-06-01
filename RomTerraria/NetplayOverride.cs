using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using Terraria;

namespace RomTerraria {

    class NetplayOverride {

        private static Assembly terrariaAssembly;
        private static Type worldGen;
        // genRand, saveLock, saveWorld
        private static FieldInfo genRand;
        private static FieldInfo saveLock;
        private static MethodInfo saveWorld;

        //public static ClientSock clientSock;
        //public static bool disconnect;
        //public static string password;
        //public static IPAddress serverIP;
        //public static IPAddress serverListenIP;
        //public static int serverPort;
        public static FilteredServerSock[] serverSock;
        //public static bool stopListen;
        //public static TcpListener tcpListener;

        static NetplayOverride( ) {
            Netplay.stopListen = false;
            Netplay.serverSock = new FilteredServerSock[9];
            serverSock = (FilteredServerSock[])Netplay.serverSock;
            Netplay.clientSock = new ClientSock( );
            Netplay.serverPort = 7777;
            Netplay.disconnect = false;
            Netplay.password = "";
        }

        public static void ListenForClients( object threadContext ) {
            while( !Netplay.disconnect && !Netplay.stopListen ) {
                int index = -1;
                for( int i = 0; i < 8; i++ ) {
                    if( !Netplay.serverSock[i].tcpClient.Connected ) {
                        index = i;
                        break;
                    }
                }
                if( index >= 0 ) {
                    try {
                        Netplay.serverSock[index].tcpClient = Netplay.tcpListener.AcceptTcpClient( );
                        Netplay.serverSock[index].tcpClient.NoDelay = true;
                    } catch( Exception exception ) {
                        if( !Netplay.disconnect ) {
                            Main.menuMode = 15;
                            Main.statusText = exception.ToString( );
                            Netplay.disconnect = true;
                        }
                    }
                } else {
                    Netplay.stopListen = true;
                    Netplay.tcpListener.Stop( );
                }
            }

        }

        public static void StartServer( ) {
            ThreadPool.QueueUserWorkItem( new WaitCallback( ServerLoop ), 1 );
        }

        public static void ServerLoop( object threadContext ) {
            LoadTerrariaAssembly( );
            if( Main.rand == null ) {
                Main.rand = new Random( (int)DateTime.Now.Ticks );
            }
            if( genRand.GetValue( null ) == null ) {
                genRand.SetValue( null, new Random( (int)DateTime.Now.Ticks ) );
            }
            Main.myPlayer = 8;
            Netplay.serverIP = IPAddress.Any;
            Netplay.serverListenIP = Netplay.serverIP;
            Main.menuMode = 14;
            Main.statusText = "Starting server...";
            Main.netMode = 2;
            Netplay.disconnect = false;
            for( int i = 0; i < 9; i++ ) {
                Netplay.serverSock[i] = new FilteredServerSock( );
                Netplay.serverSock[i].Reset( );
                Netplay.serverSock[i].whoAmI = i;
                Netplay.serverSock[i].tcpClient = new TcpClient( );
                Netplay.serverSock[i].tcpClient.NoDelay = true;
                Netplay.serverSock[i].readBuffer = new byte[0x400];
                Netplay.serverSock[i].writeBuffer = new byte[0x400];
            }
            Netplay.tcpListener = new TcpListener( Netplay.serverListenIP, Netplay.serverPort );
            try {
                Netplay.tcpListener.Start( );
            } catch( Exception exception ) {
                Main.menuMode = 15;
                Main.statusText = exception.ToString( );
                Netplay.disconnect = true;
            }
            if( !Netplay.disconnect ) {
                ThreadPool.QueueUserWorkItem( new WaitCallback( ListenForClients ), 1 );
                Main.statusText = "Server started";
            }
            while( !Netplay.disconnect ) {
                if( Netplay.stopListen ) {
                    int num2 = -1;
                    for( int m = 0; m < 8; m++ ) {
                        if( !Netplay.serverSock[m].tcpClient.Connected ) {
                            num2 = m;
                            break;
                        }
                    }
                    if( num2 >= 0 ) {
                        Netplay.tcpListener.Start( );
                        Netplay.stopListen = false;
                        ThreadPool.QueueUserWorkItem( new WaitCallback( ListenForClients ), 1 );
                    }
                }
                int num4 = 0;
                for( int k = 0; k < 9; k++ ) {
                    if( NetMessage.buffer[k].checkBytes ) {
                        NetMessage.CheckBytes( k );
                    }
                    if( Netplay.serverSock[k].kill ) {
                        Netplay.serverSock[k].Reset( );
                        NetMessage.syncPlayers( );
                    } else if( Netplay.serverSock[k].tcpClient.Connected ) {
                        if( !Netplay.serverSock[k].active ) {
                            Netplay.serverSock[k].state = 0;
                        }
                        Netplay.serverSock[k].active = true;
                        num4++;
                        if( !Netplay.serverSock[k].locked ) {
                            try {
                                Netplay.serverSock[k].networkStream = Netplay.serverSock[k].tcpClient.GetStream( );
                                if( Netplay.serverSock[k].networkStream.DataAvailable ) {
                                    Netplay.serverSock[k].locked = true;
                                    Netplay.serverSock[k].networkStream.BeginRead( Netplay.serverSock[k].readBuffer, 0, Netplay.serverSock[k].readBuffer.Length, new AsyncCallback( NetplayOverride.serverSock[k].FilteredReadCallBack ), Netplay.serverSock[k].networkStream );
                                }
                            } catch {
                                Netplay.serverSock[k].kill = true;
                            }
                        }
                        if( ( Netplay.serverSock[k].statusMax > 0 ) && ( Netplay.serverSock[k].statusText2 != "" ) ) {
                            if( Netplay.serverSock[k].statusCount >= Netplay.serverSock[k].statusMax ) {
                                Netplay.serverSock[k].statusText = string.Concat( new object[] { "(", Netplay.serverSock[k].tcpClient.Client.RemoteEndPoint, ") ", Netplay.serverSock[k].name, " ", Netplay.serverSock[k].statusText2, ": Complete!" } );
                                Netplay.serverSock[k].statusText2 = "";
                                Netplay.serverSock[k].statusMax = 0;
                                Netplay.serverSock[k].statusCount = 0;
                            } else {
                                Netplay.serverSock[k].statusText = string.Concat( new object[] { "(", Netplay.serverSock[k].tcpClient.Client.RemoteEndPoint, ") ", Netplay.serverSock[k].name, " ", Netplay.serverSock[k].statusText2, ": ", (int)( ( ( (float)Netplay.serverSock[k].statusCount ) / ( (float)Netplay.serverSock[k].statusMax ) ) * 100f ), "%" } );
                            }
                        } else if( Netplay.serverSock[k].state == 0 ) {
                            Netplay.serverSock[k].statusText = string.Concat( new object[] { "(", Netplay.serverSock[k].tcpClient.Client.RemoteEndPoint, ") ", Netplay.serverSock[k].name, " is connecting..." } );
                        } else if( Netplay.serverSock[k].state == 1 ) {
                            Netplay.serverSock[k].statusText = string.Concat( new object[] { "(", Netplay.serverSock[k].tcpClient.Client.RemoteEndPoint, ") ", Netplay.serverSock[k].name, " is sending player data..." } );
                        } else if( Netplay.serverSock[k].state == 2 ) {
                            Netplay.serverSock[k].statusText = string.Concat( new object[] { "(", Netplay.serverSock[k].tcpClient.Client.RemoteEndPoint, ") ", Netplay.serverSock[k].name, " requested world information" } );
                        } else if( ( Netplay.serverSock[k].state != 3 ) && ( Netplay.serverSock[k].state == 10 ) ) {
                            Netplay.serverSock[k].statusText = string.Concat( new object[] { "(", Netplay.serverSock[k].tcpClient.Client.RemoteEndPoint, ") ", Netplay.serverSock[k].name, " is playing" } );
                        }
                    } else if( Netplay.serverSock[k].active ) {
                        Netplay.serverSock[k].kill = true;
                    } else {
                        Netplay.serverSock[k].statusText2 = "";
                        if( k < 8 ) {
                            Main.player[k].active = false;
                        }
                    }
                    Thread.Sleep( 1 );
                }
                if( !(bool)saveLock.GetValue( null ) ) {
                    if( num4 == 0 ) {
                        Main.statusText = "Waiting for clients...";
                    } else {
                        Main.statusText = num4 + " clients connected";
                    }
                }
            }
            Netplay.tcpListener.Stop( );
            for( int j = 0; j < 9; j++ ) {
                Netplay.serverSock[j].Reset( );
            }
            if( Main.menuMode != 15 ) {
                Main.netMode = 0;
                Main.menuMode = 10;
                saveWorld.Invoke( null, new object[] { false } ); 
                //WorldGen.saveWorld( false );
                while( (bool)saveLock.GetValue( null ) ) {
                    // continue
                }
                Main.menuMode = 0;
            } else {
                Main.netMode = 0;
            }
            Main.myPlayer = 0;
        }

        private static void LoadTerrariaAssembly( ) {
            //private FieldInfo genRand;
            //private FieldInfo saveLock;
            //private MethodInfo saveWorld;

            terrariaAssembly = Assembly.GetAssembly( typeof( Terraria.Main ) );
            if( terrariaAssembly != null ) {
                worldGen = terrariaAssembly.GetType( "Terraria.WorldGen" );

                foreach( var f in worldGen.GetFields( ) ) {
                    if( f.Name == "genRand" ) {
                        genRand = f;
                    }

                    if( f.Name == "saveLock" ) {
                        saveLock = f;
                    }
                }

                foreach( var f in worldGen.GetMethods( ) ) {
                    if( f.Name == "saveWorld" ) {
                        saveWorld = f;
                    }
                }
            }
        }

        //public static void InterceptNetwork( ) {
        //    var Netplay.serverSock = Terraria.Netplay.Netplay.serverSock;

        //    for( var i =0; i < 10; i++ ) {
        //        if( i < 9 ) {
        //            Netplay.serverSock[i] = new FilteredNetplay.serverSock( );
        //            Netplay.serverSock[i].tcpClient.NoDelay = true;
        //        }
        //        Terraria.NetMessage.buffer[i] = new messageBuffer( );
        //        Terraria.NetMessage.buffer[i].whoAmI = i;
        //    }
        //}

    }

}
