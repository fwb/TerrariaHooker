using System.Collections.Generic;
using System.Net;
using Terraria;

namespace TerrariaHooker.AccountManagement {

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

            CreateAccount( "Zubu", "192.168.1.201", true );
            var c = FindAccount( "Zubu" );
            c.AddIP( IPAddress.Parse( "76.119.218.206" ) );

            CreateAccount( "console", "127.0.0.1", true);
        }
        
        public static bool Login( int playerId, string username ) {
            string endpoint;
            if (playerId == 0xFC)
                endpoint = "127.0.0.1:1";
            else
                endpoint = Netplay.serverSock[playerId].tcpClient.Client.RemoteEndPoint.ToString( );

            var ipstr = Utils.ParseEndPointAddr( endpoint );
            var ip = IPAddress.Parse( ipstr );
            var account = FindAccount( username, ip );
            if( account != null ) {
                if( activeAccounts.ContainsKey( playerId ) ) {
                    Logout( playerId );
                }
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
            var rights = Rights.NONE;
            var ips = new List<IPAddress>( );

            usernames.Add( username );
            if( admin )
            {
                rights = Rights.ADMIN | Rights.EVENTS;
            } else
            {
                rights = Rights.NONE;
            }
            rights = rights | Rights.USEITEMS | Rights.TELEPORT;
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

        public static Rights GetRights( int playerId) {
            if( activeAccounts.ContainsKey( playerId ) ) {
                return activeAccounts[playerId].CheckRights();
            }
            return Rights.NONE;
        }
    }

}