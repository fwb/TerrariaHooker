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

        public static void sendLineToConsole(string text)
        {
            char[] c = text.ToCharArray();
            var n = new INPUT_RECORD[c.Length + 1];
            int index = 0;
            foreach (char i in c)
            {
                n[index].EventType = 0x0001;
                n[index].KeyEvent.bKeyDown = 1;
                n[index].KeyEvent.wRepeatCount = 1;
                n[index].KeyEvent.UnicodeChar = i;
                index++;
            }

            n[index].EventType = 0x0001;
            n[index].KeyEvent.bKeyDown = 1;
            n[index].KeyEvent.dwControlKeyState = 0;
            n[index].KeyEvent.wRepeatCount = 1;
            n[index].KeyEvent.wVirtualKeyCode = 0x0D;
            n[index].KeyEvent.UnicodeChar = (char)0x0D;
            n[index].KeyEvent.wVirtualScanCode = (ushort)ServerConsole.MapVirtualKey(0x0D, 0x00);

            uint events;
            ServerConsole.WriteConsoleInput(Program.STDIN_HANDLE, n, (uint)c.Length + 1, out events);

        }

    }
}
