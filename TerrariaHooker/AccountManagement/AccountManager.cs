using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Terraria;

namespace TerrariaHooker.AccountManagement {
    internal class AccountManager {
        private const string AccountFile = @"accounts.xml";
        private static List<Account> accounts;
        private static Dictionary<int, Account> activeAccounts;

        private static XDocument xmldoc;

        public static bool WhitelistActive;

        static AccountManager( ) {
            accounts = new List<Account>( );
            activeAccounts = new Dictionary<int, Account>( );
            LoadAccounts( );
        }

        public static bool IsAllowed( string ip ) {
            try {
                var a = FindAccount( IPAddress.Parse( ip ) );
                if( a != null ) {
                    return true;
                }
            } catch {
                return false;
            }
            return false;
        }

        public static void RemoveAccount( string ip ) {
            try {
                var a = FindAccount( IPAddress.Parse( ip ) );
                if( a != null ) {
                    accounts.Remove( a );
                    SaveAccounts( );
                }
            } catch( Exception e ) {
                Console.WriteLine( String.Format( "Exception in AccountManager.RemoveAccount( ): {0}", e ) );
            }
        }

        public static void Refresh( ) {
            LoadAccounts( );
        }

        public static void SaveAccounts( ) {
            ConvertToXml( );
            try {
                using( var fs = new FileStream( AccountFile, FileMode.OpenOrCreate & FileMode.Truncate,
                                                FileAccess.Write, FileShare.None ) ) {
                    xmldoc.Save( fs );
                }
            }
            catch( Exception e ) {
                Console.WriteLine( String.Format( "Exception in AccountManager.SaveAccounts( ): {0}", e ) );
            }
        }

        public static void LoadAccounts( ) {
            ConvertFromXml( );
            try {
                using( var fs = new FileStream( AccountFile, FileMode.Open,
                                                FileAccess.Read, FileShare.None ) ) {
                    xmldoc = XDocument.Load( fs );
                }
            }
            catch( Exception e ) {
                Console.WriteLine( String.Format( "Exception in AccountManager.LoadAccounts( ): {0}", e ) );
                Console.WriteLine( "Loading default accounts" );
                CreateAccount( "Forfeit", "192.168.1.242", true );
                var a = FindAccount( "Forfeit" );
                a.AddIP( IPAddress.Parse( "173.66.80.83" ) );

                CreateAccount( "ass", "192.168.0.90", true );
                var b = FindAccount( "ass" );
                b.AddIP( IPAddress.Parse( "123.243.252.109" ) );

                CreateAccount( "Zubu", "192.168.1.201", true );
                var c = FindAccount( "Zubu" );
                c.AddIP( IPAddress.Parse( "76.119.218.206" ) );

                CreateAccount( "console", "127.0.0.1", true );
            }
        }

        private static void ConvertToXml( ) {
            try {
                xmldoc = new XDocument( );
                var xAccountManager = new XElement( "AccountManager" );

                foreach( var account in accounts ) {
                    var xAccount = new XElement( "Account" );

                    // List of usernames
                    foreach( var username in account.GetUsernames( ) ) {
                        xAccount.Add( new XElement( "Username", username ) );
                    }
                    // List of IPs
                    foreach( var ip in account.GetIPs( ) ) {
                        xAccount.Add( new XElement( "Ip", ip.ToString( ) ) );
                    }
                    // Rights
                    xAccount.Add( new XElement( "Rights", (int)account.CheckRights( ) ) );

                    // account name (future)
                    // steam name (future)
                    xAccountManager.Add( xAccount );
                }

                xmldoc.Add( xAccountManager );
            }
            catch( Exception e ) {
                Console.WriteLine( String.Format( "Exception in AccountManager.ConvertToXml( ): {0}", e ) );
            }
        }

        private static void ConvertFromXml( ) {
            try {
                foreach( var account in xmldoc.Elements( "Account" ) ) {
                    var usernames = account.Elements( "Username" ).Select( item => item.ToString( ) ).ToList( );
                    var ips = account.Elements( "Ip" ).Select( item => IPAddress.Parse( item.ToString( ) ) ).ToList( );
                    var rights = (Rights)int.Parse( account.Element( "Rights" ).Value );

                    CreateAccount( usernames, rights, ips );
                }
            }
            catch( Exception e ) {
                Console.WriteLine( String.Format( "Exception in AccountManager.ConvertFromXml( ): {0}", e ) );
            }
        }

        public static bool Login( int playerId, string username ) {
            string endpoint;
            if( playerId == 0xFC )
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
            if( admin ) {
                rights = Rights.ADMIN | Rights.EVENTS;
            }
            rights = rights | Rights.USEITEMS | Rights.TELEPORT;
            ips.Add( IPAddress.Parse( ip ) );
            //ips.Add( new IPAddress( new byte[] { 192, 168, 1, 242 } ) );

            accounts.Add( new Account( usernames, rights, ips ) );
        }

        private static void CreateAccount( List<String> usernames, Rights rights, List<IPAddress> ips ) {
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

        private static Account FindAccount( IPAddress ip ) {
            foreach( var account in accounts ) {
                foreach( var i in account.GetIPs(  ) ) {
                    if( ip.Equals( i ) ) {
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

        public static Rights GetRights( int playerId ) {
            if( activeAccounts.ContainsKey( playerId ) ) {
                return activeAccounts[playerId].CheckRights( );
            }
            return Rights.NONE;
        }
    }

}