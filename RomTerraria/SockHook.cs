using System;
using System.Runtime.InteropServices;
using EasyHook;
using System.Net.Sockets;


namespace RomTerraria
{
   

    /// <summary>
    /// Winsock Struct, implemented as an IntPtr to unmanaged memory.
    /// </summary>
    internal struct WSABuffer
    {
        internal int len;
        // Length of Buffer
        internal IntPtr buf;
        // Pointer to Buffer
    }

    /// <summary>
    /// The code in this class needs to be locked down, as any exception thrown here is completely
    /// unraised, will fault the socket and cause the hook to drop with no warning. Any logic should
    /// be in Commands.cs unless absolutely necessary.
    /// </summary>
    public class SockHook :  IEntryPoint
    {

        public LocalHook CreateWSARecvHook;
        public LocalHook CreateWSASendHook;

        #region dllimports
        [DllImport("Ws2_32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
        static extern int WSARecv(
            [In] IntPtr socketHandle,
            [In, Out] ref WSABuffer Buffer,
            [In] int BufferCount,
            [In] IntPtr bytesTransferred,
            [In, Out] ref SocketFlags socketFlags,
            [In] IntPtr overlapped,
            [In] IntPtr completionRoutine
            );

        [DllImport("Ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int WSASend(
            [In] IntPtr socketHandle,
            [In] ref WSABuffer Buffer,
            [In] int BufferCount,
            [In] IntPtr bytesTransferred,
            [In] SocketFlags socketFlags,
            [In] IntPtr overlapped,
            [In] IntPtr completionRoutine
            );
        #endregion

        #region function pointer delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        private delegate int DWSARecv(
            [In] IntPtr socketHandle,
            [In, Out] ref WSABuffer Buffer,
            [In] int BufferCount,
            [In] IntPtr bytesTransferred,
            [In, Out] ref SocketFlags socketFlags,
            [In] IntPtr overlapped,
            [In] IntPtr completionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        private delegate int DWSASend(
            [In] IntPtr socketHandle,
            [In] ref WSABuffer Buffer,
            [In] int BufferCount,
            [In] IntPtr bytesTransferred,
            [In] SocketFlags socketFlags,
            [In] IntPtr overlapped,
            [In] IntPtr completionRoutine
            );
        #endregion


        /// <summary>
        /// WSARecv Hook. Use this hook to modify incoming data, keeping length intact.
        /// Any changes to length apparently will cause a socket fault (unraised), though
        /// actual cause is unknown. It should be safe to rewrite incoming data and marshal
        /// it back to the original pointer, through Marshal.WriteByte(Buffer.buf, index);
        /// 
        /// There are possibly ways to modify packet data to be inert, if reducing a recieved
        /// packet to a stub is necessary.
        /// 
        /// To block typical incoming data, use WSASendHooked e.g. preventing lines of chat,
        /// undoing world changes (in this case, buffer changes in WSARecv, revert, and block
        /// outgoing updates in WSASend. It's a hacky method, but works.
        /// 
        /// NOTES: Buffer is a ref to an WSABuffer structure. To access packet data, it needs
        /// to be marshalled. Marshal.Copy(Buffer.buf, newbuffer, number_of_bytes) is the
        /// safest way. number_of_bytes can be retrieved by marshalling bytesTransferred to
        /// an Int32 type with Marshal.ReadInt32(bytesTransferred).
        /// </summary>
        /// <param name="socketHandle">The socket handle.</param>
        /// <param name="Buffer">The buffer pointer.</param>
        /// <param name="BufferCount">The buffer count.</param>
        /// <param name="bytesTransferred">The bytes transferred.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="overlapped">The overlapped data.</param>
        /// <param name="completionRoutine">The completion routine after overlap processing.</param>
        /// <returns></returns>
        static int WSARecvHooked(
                [In] IntPtr socketHandle,
                [In, Out] ref WSABuffer Buffer,
                [In] int BufferCount,
                [In] IntPtr bytesTransferred,
                [In, Out] ref SocketFlags socketFlags,
                [In] IntPtr overlapped,
                [In] IntPtr completionRoutine)
        {

            var result = WSARecv(socketHandle, ref Buffer, BufferCount, bytesTransferred, ref socketFlags,
                                    overlapped, completionRoutine);
            if (result != 0)
            {
                MakeItHarder.serverConsole.AddChatLine("Socket Error!");
                return result;
            }

            int bytes = Marshal.ReadInt32(bytesTransferred);
            if (bytes > 0)
            {
                var newBuffer = new byte[bytes];
                Marshal.Copy(Buffer.buf, newBuffer, 0, bytes);
                //processData(newBuffer); //processData calls on RECV will strictly be read-only.
                //fairly simple, since it's designed around the Packet struct and ignoring that
                //forces normal program flow.
#region old_WSARecv_blocking_code.
                    //ok, this is fucking ridiculous. modifying the bytesTransferred ref should prevent processing by the application
                    //however probably .net or even xNA is fucking that up. going to intercept WSASend, so that when it tries to reply
                    //with a 0x19 pid 0x00 0x00 0x00 bytes[] text message we'll block that, fairly trivially.
                    /*if (newBuffer[4] == 0x19)
                    {
                        var n = new byte[5] { 0x01, 0x00, 0x00, 0x00, 0x15 };
                        GCHandle fake = GCHandle.Alloc(n, GCHandleType.Pinned);
                        IntPtr fakePtr = fake.AddrOfPinnedObject();
                        //bytesTransferred = 5;
                        //Marshal.WriteInt32(bytesTransferred, 9);
                        int p = 0;
                        foreach (byte b in n)
                        {
                            Marshal.WriteByte(Buffer.buf,p,b);
                            p++;
                        }
                        Marshal.WriteInt32(bytesTransferred, p); 
                        //Marshal.WriteInt32(bytesTransferred,0x00);
                        //Marshal.WriteIntPtr(Buffer.buf, 0, IntPtr.Zero);
                        //Buffer.len = 0;

                        //bytesTransferred = 0;
                        //Marshal.WriteIntPtr(buffer.Pointer, fakePtr);
                        //buffer.len = 0;););

                        return 0;
                        //return SocketError.TryAgain;
                    }*/


                    /*
                    var sBuffer = new StringBuilder();
                    foreach (byte t in newBuffer)
                    {
                        sBuffer.Append(Convert.ToInt32(t).ToString("x").PadLeft(2, '0') + " ");
                    }
                
                    //string s = System.Text.Encoding.ASCII.GetString(newBuffer);
                
                    //MakeItHarder.serverConsole.AddChatLine(sBuffer.ToString().ToUpper());
                    */
#endregion

            }
            return result;
        }

        /// <summary>
        /// WSASend Hook. Use this to modify any particular outgoing data, or use
        /// outgoing data to modify internal structures (e.g. parse world update
        /// sends and use parsed data to change NPC locations, block types, etc.)
        /// 
        /// Also, currently, this is the only place to properly add /commands to the
        /// game, without having code split between WSARecv and here. Process data in
        /// Buffer.buf searching for:
        /// 0x?? 0x?? 0x?? 0x?? 0x19 PID, 0xRR 0xGG 0xBB byte[] message. Return 0 from
        /// the function without calling the original WSASend method and the game loop
        /// will continue unfussed.
        /// 
        /// WSASend doesn't suffer from the same issues as WSARecv as there are no
        /// callback structures, all data passed to WSASend is isolated and changes
        /// can be made without any effect on anything except the current invocation
        /// of WSASend. 
        /// 
        /// NOTES: Buffer is a ref to an WSABuffer structure. To access packet data, it 
        /// needs to be marshalled. Marshal.Copy(Buffer.buf, newbuffer, number_of_bytes) 
        /// is the safest way. number_of_bytes in WSASend calls is stored in Buffer.len, 
        /// as a plain int.
        /// </summary>
        /// <param name="socketHandle">The socket handle.</param>
        /// <param name="Buffer">The buffer pointer.</param>
        /// <param name="BufferCount">The buffer count.</param>
        /// <param name="bytesTransferred">The bytes transferred POST SEND.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="overlapped">The overlapped data.</param>
        /// <param name="completionRoutine">The completion routine after overlap processing.</param>
        /// <returns></returns>
        static int WSASendHooked(
            [In] IntPtr socketHandle,
            [In] ref WSABuffer Buffer,
            [In] int BufferCount,
            [In] IntPtr bytesTransferred,
            [In] SocketFlags socketFlags,
            [In] IntPtr overlapped,
            [In] IntPtr completionRoutine)
        {

            var newBuffer = new byte[Buffer.len]; //buffer to hold sent bytes
            Marshal.Copy(Buffer.buf,newBuffer,0,Buffer.len);

            //I absolutely wish this code wasn't here, but unfortunately it's necessary.
            if (newBuffer[4] == 0x19)
            {
                var n = Commands.ProcessData(newBuffer);
                if (n.Length == -1)
                {
                    return 0;
                }
                int i = 0;
                foreach (byte b in n.Data)
                {
                    Marshal.WriteByte(Buffer.buf, i, b);
                    i++;
                }
                Buffer.len = n.Length;
            }

            //call WSASend
            return WSASend(socketHandle, ref Buffer, BufferCount, bytesTransferred, socketFlags, overlapped,
                                 completionRoutine);

        }

        


        /// <summary>
        /// Loads and wakes WSARecv and WSASend hooks. Process-local.
        /// </summary>
        public void InitializeHooks()
        {
            try
            {
                CreateWSARecvHook = LocalHook.Create(LocalHook.GetProcAddress("Ws2_32.dll", "WSARecv"), new DWSARecv(WSARecvHooked), this);
                CreateWSARecvHook.ThreadACL.SetExclusiveACL(new[] { 0 });
                MakeItHarder.serverConsole.AddChatLine("WSARecv Hook succeeded.");

                CreateWSASendHook = LocalHook.Create(LocalHook.GetProcAddress("Ws2_32.dll", "WSASend"), new DWSASend(WSASendHooked), this);
                CreateWSASendHook.ThreadACL.SetExclusiveACL(new[] { 0 });
                MakeItHarder.serverConsole.AddChatLine("WSASend Hook succeeded.");
            }
            catch (Exception extInfo)
            {
                MakeItHarder.serverConsole.AddChatLine("Hook failed: " + extInfo);
                return;
            }
            RemoteHooking.WakeUpProcess();
        }


    }


}
