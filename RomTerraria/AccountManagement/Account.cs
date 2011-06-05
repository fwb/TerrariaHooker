using System;
using System.Collections.Generic;
using System.Net;

namespace RomTerraria.AccountManagement {

    class Account {
        public bool IsLoggedIn { get; set; }
        public bool IsBanned { get; set; }

        private List<String> usernames;
        private Dictionary<Rights, bool> rights;
        private Dictionary<String, ReuseTimer> timers; 
        private List<IPAddress> ips;
        private List<String> ignored; 
        private string accountName;
        private string steamName;

        public Account(  ) {
            usernames = new List<string>( );
            rights = new Dictionary<Rights, bool>( );
            ips = new List<IPAddress>( );

            timers = new Dictionary<string, ReuseTimer>( );
            ignored = new List<string>( );
        }

        public Account( List<string> usernames, Dictionary<Rights, bool> rights, 
                        List<IPAddress> ips ) {
            this.usernames = usernames;
            this.rights = rights;
            this.ips = ips;

            timers = new Dictionary<string, ReuseTimer>( );
            ignored = new List<string>( );
        }

        public bool CheckRights( Rights r ) {
            return rights[r];
        }

        public void SetRights( Rights r, bool b ) {
            rights[r] = b;
        }

        public List<IPAddress> GetIPs( ) {
            return ips;
        }

        public void AddIP( IPAddress ip ) {
            if( !ips.Contains( ip ) ) {
                ips.Add( ip );
            }
        }

        public void AddUsername( string s ) {
            if( !usernames.Contains( s ) ) {
                usernames.Add( s );
            }
        }

        public List<String> GetUsernames( ) {
            return usernames;
        } 

        public bool IsIgnored( string player ) {
            return ignored.Contains( player );
        }
        
        public void StartTimer( string s ) {
            timers.Add( s, new ReuseTimer( ) );
        }

        public void StartTimer( string s, double duration ) {
            timers.Add( s, new ReuseTimer( duration ) );
        }

        public void ResetTimer( string s ) {
            timers[s].Reset( );
        }

        public bool HasExpired( string s ) {
            return timers[s].HasExpired( );
        }
        
        public void Kick( ) {
            
        }
    }

}
