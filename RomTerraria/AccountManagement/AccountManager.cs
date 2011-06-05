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
        }
        
        public static bool Login( int playerId, string username, string password ) {
            var ip = IPAddress.Parse( Netplay.serverSock[playerId].tcpClient.Client.RemoteEndPoint.ToString( ) );
            var account = FindAccount( username, ip );
                //Netplay.CheckBan(Netplay.serverSock[this.whoAmI].tcpClient.Client.RemoteEndPoint.ToString()
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

        public static void AddIP( Account a, IPAddress ip ) {
            
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
            var account = activeAccounts[playerId];
            if( account != null ) {
                return account.CheckRights( r );
            }
            return false;
        }
    }

}