using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace TerrariaHooker {
    class Whitelist {
        public static bool IsActive { get; set; }
        private static HashSet<string> wl;

        static Whitelist( ) {
            wl = new HashSet<string>( );
            LoadFromDisk( );
        }

        public static void AddEntry( string ip ) {
            try {
                IPAddress.Parse( ip );
                wl.Add( ip );
            } catch( Exception e ) {
                Console.WriteLine( String.Format( "Exception caught in Whitelist.AddEntry( ): {0}", e.ToString( ) ) );
            }
            SaveToDisk( );
        }

        public static void RemoveEntry( string ip ) {
            wl.Remove( ip );
            SaveToDisk( );
        }

        public static bool IsAllowed( string ip ) {
            if( wl.Contains( ip ) ) {
                return true;
            }
            return false;
        }

        public static void Refresh( ) {
            LoadFromDisk( );
        }

        private static void LoadFromDisk( ) {
            try {
                using( var fs = new FileStream( @"whitelist.txt", FileMode.OpenOrCreate,
                                                FileAccess.Read, FileShare.ReadWrite ) ) {
                    using( var sr = new StreamReader( fs ) ) {
                        while( !sr.EndOfStream ) {
                            var line = sr.ReadLine( );
                            if( line != null ) {
                                try {
                                    IPAddress.Parse( line );
                                    wl.Add( line );
                                }
                                catch( Exception e ) {
                                    Console.WriteLine(
                                        String.Format( "Exception caught in Whitelist.LoadFromDisk( ): {0}",
                                                       e.ToString( ) ) );
                                }
                            }
                        }
                    }
                }
            } catch( Exception e ) {
                Console.WriteLine( String.Format( "Exception caught in Whitelist.LoadFromDisk( ): {0}", e.ToString( ) ) );
            }
        }

        private static void SaveToDisk( ) {
            try {
                using( var fs = new FileStream( @"whitelist.txt", FileMode.OpenOrCreate & FileMode.Truncate,
                                                FileAccess.Write, FileShare.None ) ) {
                    fs.SetLength( 0 ); 
                    using( var sw = new StreamWriter( fs ) ) {
                        using( var tw = TextWriter.Synchronized( sw ) ) {
                            foreach( var s in wl ) {
                                tw.WriteLine( s );
                            }
                        }
                    }
                }
            } catch( Exception e ) {
                Console.WriteLine( String.Format( "Exception caught in Whitelist.SaveToDisk( ): {0}", e.ToString( ) ) );
            }
        }
    }
}
