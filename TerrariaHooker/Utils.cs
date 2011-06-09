using System.Collections.Generic;

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

        public static string concat(List<string> n, string delimiter=", ")
        {
            string o = null;
            foreach (var i in n)
            {
                o += i + delimiter;
            }
            return o.Substring(0, o.Length - 2);
        }

        public static List<string> GetLines( string s, int maxLineLength ) {
            var list = new List<string>( );

            if( maxLineLength >= s.Length ) {
                list.Add( s );
            } else {
                var curLine = "";
                foreach( var word in s.Split( ' ' ) ) {
                    if( curLine.Length + word.Length + 1 > maxLineLength ) {
                        list.Add( curLine );
                        curLine = "";
                    }
                    curLine += word + " ";
                }
                if( curLine != "" ) {
                    list.Add( curLine );
                }
            }

            return list;
        }

    }
}
