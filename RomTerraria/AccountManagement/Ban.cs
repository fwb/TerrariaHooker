namespace RomTerraria.AccountManagement {

    class Ban {
        public string Reason { get; private set; }

        private ReuseTimer banTimer;

        public Ban( string reason ) : this( 5, reason ) {
            // default 5 minute ban
        }

        public Ban( double duration, string reason ) {
            banTimer = new ReuseTimer( duration );
            Reason = reason;
        }

        public bool HasExpired( ) {
            return banTimer.HasExpired( );
        }
    }

}