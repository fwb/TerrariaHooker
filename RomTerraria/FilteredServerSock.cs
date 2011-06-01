using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RomTerraria {

    class FilteredServerSock : Terraria.ServerSock {

        public FilteredServerSock( ) {
            tcpClient = new TcpClient( );
            tileSection = new bool[Terraria.Main.maxTilesX / 200,Terraria.Main.maxTilesY / 150];
            statusText = "";
            name = "Anonymous";
            oldName = "";
        }

        public void FilteredReadCallBack( IAsyncResult ar ) {
            var streamLength = 0;
            if( !Terraria.Netplay.disconnect ) {
                try {
                    streamLength = networkStream.EndRead( ar );
                }
                catch {
                    //
                }
                if( streamLength == 0 ) {
                    kill = true;
                }
                else {
                    if( Terraria.Main.ignoreErrors ) {
                        try {
                            CopyAndForward( readBuffer, streamLength, whoAmI );
                            goto Label_wtf;
                        } catch {
                            goto Label_wtf;
                        }
                    }
                    CopyAndForward( readBuffer, streamLength, whoAmI );
                }
            }
            Label_wtf:
            locked = false;
        }

        private static void CopyAndForward( byte[] bytes, int streamLength, [Optional, DefaultParameterValue(9)] int i ) {
            var enc = new ASCIIEncoding( );
            //MessageBox.Show( "CopyAndForward( )" );  
            MakeItHarder.serverConsole.AddChatLine( enc.GetString( bytes ) );
            Terraria.NetMessage.RecieveBytes( bytes, streamLength, i );
        }
    }
}
