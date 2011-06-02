using System;

namespace RomTerraria.AccountManagement {

    class ReuseTimer {

        private DateTime startTime = DateTime.MinValue;
        private readonly TimeSpan duration;
        private bool isRunning;

        public ReuseTimer( ) : this( 5, true ){
            // default constructor sets duration to 5 minutes and starts the timer
        }

        public ReuseTimer( double minutes, bool runNow ) {
            duration = TimeSpan.FromMinutes( minutes );
            if( runNow ) {
                Reset( );
            }
        }

        public void Reset( ) {
            startTime = DateTime.Now;            
        }

        public bool HasExpired( ) {
            return DateTime.Compare( DateTime.Now, startTime.Add( duration ) ) >= 0;
        }
    }

}