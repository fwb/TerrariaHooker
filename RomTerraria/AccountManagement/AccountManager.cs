using System;
using System.Collections.Generic;
using System.Net;
using Terraria;

namespace RomTerraria.AccountManagement {

    class AccountManager {
        private static List<Account> accounts;
        private static Dictionary<int, Account> activeAccounts; 

        static AccountManager( ) {
            accounts = new List<Account>( );
            activeAccounts = new Dictionary<int, Account>( );
            LoadAccounts( );
        }

        private static void LoadAccounts( ) {
            CreateAccount( "Forfeit", "192.168.1.242", true );
            var a = FindAccount( "Forfeit" );
            a.AddIP( IPAddress.Parse( "173.66.80.83" ) );  

            CreateAccount( "ass", "192.168.0.90", true );
            var b = FindAccount( "ass" );
            b.AddIP( IPAddress.Parse( "123.243.252.109" ) );
        }
        
        public static bool Login( int playerId, string username ) {
            var endpoint = Netplay.serverSock[playerId].tcpClient.Client.RemoteEndPoint.ToString( );
            var ipstr = endpoint;
            for( int i = 0; i < endpoint.Length; i++ ) {
                if( endpoint.Substring( i, 1 ) == ":" ) {
                    ipstr = endpoint.Substring( 0, i );
                }
            }
            var ip = IPAddress.Parse( ipstr );
            var account = FindAccount( username, ip );
            if( account != null ) {
                activeAccounts.Add( playerId, account );
                return true;
            }

            return false;
        }

        public static void Logout( int playerId ) {
            activeAccounts.Remove( playerId );
        }

        public static void CreateAccount( string username, string ip, bool admin = false ) {
            var usernames = new List<string>( );
            var rights = new Dictionary<Rights, bool>( );
            var ips = new List<IPAddress>( );

            usernames.Add( username );
            if( admin ) {
                rights.Add( Rights.ADMIN, true );
                rights.Add( Rights.EVENTS, true );
            } else {
                rights.Add( Rights.ADMIN, false );
                rights.Add( Rights.EVENTS, false );
            }
            rights.Add( Rights.TELEPORT, true );
            rights.Add( Rights.USEITEMS, true );
            ips.Add( IPAddress.Parse( ip ) );  
            //ips.Add( new IPAddress( new byte[] { 192, 168, 1, 242 } ) );

            accounts.Add( new Account( usernames, rights, ips ) );
        }

        private static Account FindAccount( string username ) {
            foreach( var account in accounts ) {
                foreach( var u in account.GetUsernames( ) ) {
                    if( u.Equals( username ) ) {
                        return account;
                    }
                }
            }
            return null;
        }

        private static Account FindAccount( string username, IPAddress ip ) {
            foreach( var account in accounts ) {
                foreach( var i in account.GetIPs( ) ) {
                    if( ip.Equals( i ) ) {
                        foreach( var u in account.GetUsernames( ) ) {
                            if( u.Equals( username ) ) {
                                return account;
                            }
                        }

                    }
                }
            }
            return null;
        }

        public static bool CheckRights( int playerId, Rights r ) {
            if( activeAccounts.ContainsKey( playerId ) ) {
                return activeAccounts[playerId].CheckRights( r );
            }
            return false;
        }
    }

}