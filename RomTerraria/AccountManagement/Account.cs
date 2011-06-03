using System;
using System.Collections.Generic;
using System.Net;

namespace RomTerraria.AccountManagement {

    class Account {
        public bool IsLoggedIn { get; set; }
        public bool IsBanned { get; set; }
        
        private Dictionary<Rights, bool> rights;
        private Dictionary<String, ReuseTimer> timers; 
        private List<IPAddress> ips;
        private List<String> usernames;
        private string accountName;
        private string steamName;

        public Account(  ) {
            
        }

        public bool CheckRights( Rights r ) {
            return rights[r];
        }

        public void GetIPs( ) {
            
        }

        public void GetUsernames( ) {
            
        }
        
        public void StartTimer( ReuseTimer t ) {
            
        }
        
        public void ResetTimer( ReuseTimer t ) {
            
        }
        
        public bool CheckTimer( ReuseTimer t ) {
            return true;
        }
        
        public void Kick( ) {
            
        }
    }

}
