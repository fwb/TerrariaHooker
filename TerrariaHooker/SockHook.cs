using System;
using System.Runtime.InteropServices;
using EasyHook;
using System.Net.Sockets;

namespace TerrariaHooker
{
   
    /// <summary>
    /// Winsock Struct, implemented as an IntPtr to unmanaged memory.
    /// </summary>
    internal struct WSABuffer
    {
        internal Int32 len;
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
            [In,Out] IntPtr bytesTransferred,
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
                [In,Out] IntPtr bytesTransferred,
                [In, Out] ref SocketFlags socketFlags,
                [In] IntPtr overlapped,
                [In] IntPtr completionRoutine)
        {
            
            var result = WSARecv(socketHandle, ref Buffer, BufferCount, bytesTransferred, ref socketFlags,
                                     overlapped, completionRoutine);

            if (result != 0)
            {
                Console.WriteLine("Socket Error!\n");
#if DEBUG
                Console.WriteLine("WSARecv (original) returned errorno: " + result);
#endif
                return result;
            }

            int bytes = Marshal.ReadInt32(bytesTransferred);
            if (bytes > 0)
            {
                var newBuffer = new byte[bytes];
                Marshal.Copy(Buffer.buf, newBuffer, 0, bytes);
                
                //wrap ProcessData call, so an exception in it shouldn't cause
                //easyhook to detach.
                try
                {
                    var packet = Commands.ProcessData(newBuffer, 0);
                    //write packet data to buffer. keep it in the try/catch because if
                    //processdata fails, this will fail because packet is null.
                    Marshal.Copy(packet.Data, 0, Buffer.buf, packet.Length);
                } catch (Exception e)
                {
                    Console.WriteLine("Serious error in Commands.cs: " + e + "\n");
#if DEBUG
                    Console.WriteLine("HOOK MAY BE DETACHED\n");
#endif
                }

            }
            return result;
        }

        /// <summary>
        /// WSASend Hook. Use this to modify any particular outgoing data, or use
        /// outgoing data to modify internal structures (e.g. parse world update
        /// sends and use parsed data to change NPC locations, block types, etc.
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
            //var newBuffer = new byte[Buffer.len]; //buffer to hold sent bytes
            //Marshal.Copy(Buffer.buf,newBuffer,0,Buffer.len);

            /*if (newBuffer[4] == 0x19)
            {
                var n = Commands.ProcessData(newBuffer, 1);
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
            }*/

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
                //this should set the ACL to try to hook on all threads.
                LocalHook.GlobalThreadACL.SetExclusiveACL(new int[] { });
                CreateWSARecvHook = LocalHook.Create(LocalHook.GetProcAddress("Ws2_32.dll", "WSARecv"), new DWSARecv(WSARecvHooked), this);
                CreateWSARecvHook.ThreadACL.SetExclusiveACL(new int[] {});
                Console.WriteLine("WSARecv Hook succeeded.");

                /*
                CreateWSASendHook = LocalHook.Create(LocalHook.GetProcAddress("Ws2_32.dll", "WSASend"), new DWSASend(WSASendHooked), this);
                CreateWSASendHook.ThreadACL.SetExclusiveACL(new int[] { });
                Console.WriteLine("WSASend Hook succeeded.");*/

            }
            catch (Exception extInfo)
            {
                Console.WriteLine("Hook failed: " + extInfo);
                return;
            }
            RemoteHooking.WakeUpProcess();
            
        }


    }


}
