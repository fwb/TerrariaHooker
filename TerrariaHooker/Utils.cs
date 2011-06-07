namespace TerrariaHooker {
    class Utils {

        public static string ParseEndPointAddr( string s ) {
            for( int i = 0; i < s.Length; i++ ) {
                if( s.Substring( i, 1 ) == ":" ) {
                    s = s.Substring( 0, i );
                }
            }

            return s;
        }

    }
}
