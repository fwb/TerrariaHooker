using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using EasyHook;
using System.Net.Sockets;
using Terraria;
using Microsoft.Xna.Framework;


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

    public partial class SockHook :  IEntryPoint
    {

        public LocalHook CreateWSARecvHook;
        public LocalHook CreateWSASendHook;

        public SockHook()
        {
            
        }

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
                    try
                    {
                        Marshal.Copy(Buffer.buf, newBuffer, 0, bytes);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                    //processData(newBuffer);
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
            return 0;
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

            //test code, triggers on chat messages OUT from the server. 
            //saves having to modify unmanaged code pointers and whatever other
            //bullshit was stopping sane breaks in WSARecv logic (because for 
            //WSASend we can just not call the original WSASend and return 0,
            //effectively jumping over the socket code).
            if (newBuffer[4] == 0x19)
            {
                Packet n = ProcessData(newBuffer);
                if (n.length == -1)
                {
                    return 0;
                }
                int i = 0;
                foreach (byte b in n.data)
                {
                    Marshal.WriteByte(Buffer.buf, i, n.data[i]);
                    i++;
                }
                // fake = GCHandle.Alloc(n.data, GCHandleType.Pinned);
                //IntPtr fakePtr = fake.AddrOfPinnedObject();
                //Marshal.WriteIntPtr(Buffer.buf, fakePtr);
                Buffer.len = n.length;
            }

            var result = WSASend(socketHandle, ref Buffer, BufferCount, bytesTransferred, socketFlags, overlapped,
                                 completionRoutine);
            return result;
        }

        /// <summary>
        /// Process a data packet, determining data type and where defined, 
        /// calling additional handlers to break apart data into a more workable
        /// format.
        /// </summary>
        /// <param name="data">The data.</param>
        public static Packet ProcessData(byte[] data)
        {
            byte type = data[4];

            string prefix;
            string details = null;
            var packet = new Packet(data, data.Length);

            switch (type)
            {
                case 0x01:
                    prefix = "USER LOGIN P1";
                    break;
                case 0x02:
                    prefix = "USER LOGIN RELATED";
                    break;
                case 0x04:
                    prefix = "USER LOGIN RELATED [CHARACTER DETAILS]";
                    break;
                case 0x05:
                    prefix = "PLAYER UPDATE INVENTORY/EQUIP";
                    break;
                case 0x06:
                    prefix = "PLAYER SEND COMPLETE [LOGIN]";
                    break;
                case 0x08:
                    prefix = "PLAYER REQUEST TILE DATA";
                    break;
                case 0x0C:
                    prefix = "PLAYER SYNC/GREET REQUEST";
                    break;
                case 0x0D:
                    prefix = "PLAYER STATE CHANGE";
                    //packet = new packet_PlayerState(data);
                    break;
                case 0x10:
                    prefix = "PLAYER CURRENT/MAX HEALTH UPDATE";
                    break;
                case 0x13:
                    prefix = "PLAYER USE DOOR";
                    break;
                case 0x16:
                    prefix = "DETERMINE ITEM OWNER";    //UPDATE: apparently item[x].active defines whether the item is
                                                        //active within the game world (outside of inventories). I assume
                                                        //the shit in FindOwner is to determine who the new owner of a newly
                                                        //dropped item is (owner is the current dropper, so they don't have
                                                        //have preference on the item by being closest). Also seems to be called
                                                        //when you try to pick up an item you don't have room for, which explains
                                                        //the erroneus captures I was recieving.

                    break;
                case 0x18:
                    prefix = "PLAYER HURT NPC";
                    break;
                case 0x19:
                    prefix = "CHAT MESSAGE";
                    packet = HandleChatMsg(data);   //pass data to handler. 
                                                    //ALL handlers assume a Packet struct as input.
                                                    //and ALL handlers return a Packet struct.

                    break;
                case 0x1A:
                    prefix = "PLAYER HURT PLAYER";
                    break;
                case 0x1B:
                    prefix = "PLAYER FIRED PROJECTILE";
                    break;
                case 0x1E:
                    prefix = "PLAYER TOGGLED PVP";
                    break;
                case 0x1F:
                    prefix = "PLAYER USE CHEST";
                    break;
                case 0x20:
                    prefix = "?PLAYER PLACED CHEST?";
                    break;
                case 0x23:
                    prefix = "PLAYER HEAL EFFECT";
                    break;
                case 0x24:
                    prefix = "PLAYER ZONE UPDATE";
                    break;
                case 0x26:
                    prefix = "SEND PASSWORD";
                    break;
                case 0x28:
                    prefix = "PLAYER INTERACT NPC";
                    break;
                case 0x2A:
                    prefix = "PLAYER CURRENT/MAX MANA UPDATE";
                    break;
                case 0x2c:
                    prefix = "PLAYER DIED";
                    break;
                case 0x2d:
                    prefix = "PLAYER PARTY UPDATE";
                    break;
                case 0x2F:
                    prefix = "PLAYER USE/PLACE SIGN";
                    break;
                case 0x31:
                    prefix = "PLAYER SPAWN";
                    break;

                default:
                    prefix = "UNDEFINED";
                    break;
            }

            var sBuffer = new StringBuilder();
            foreach (byte t in data)
            {
                sBuffer.Append(Convert.ToInt32(t).ToString("x").PadLeft(2, '0') + " ");
            }
            //string s = System.Text.Encoding.ASCII.GetString(newBuffer);
            //MakeItHarder.serverConsole.AddChatLine(sBuffer.ToString().ToUpper());

            MakeItHarder.serverConsole.AddChatLine(prefix + " : :" + sBuffer.ToString().ToUpper());
       
            return packet;

        }

        private static Packet HandleChatMsg(byte[] data)
        {
            var p = new packet_ChatMsg(data); //initialize packet class, populate fields from data
            //...
            //act on packet fields, generate new data etc.
            var match = false;
            if (p.text[0] == 0x2F) // match backslash
            {
                
                var commands = p.text.Split(' ');
                switch (commands[0])
                {
                    case ("/meteor"):
                        WorldEvents.SpawnMeteorCB();
                        match = true;
                        break;
                    case ("/broadcast"):
                        if (commands.Length > 1)
                        {
                            string msg = null;
                            int i = 1;
                            while (i<commands.Length)
                            {
                                msg = msg + " " + commands[i];
                                i++;
                            }
                            Terraria.NetMessage.SendData(25, -1, -1, msg, 8, 0xff, 0xcc, 0x66);
                        } else
                        {
                            Terraria.NetMessage.SendData(25, p.playerId, -1, "USAGE: /echo <word>", 8, 0x99, 0xff, 0x99);
                        }
                        match = true;
                        break;
                    default:
                        match = false;
                        break;
                }

            }
            //create new Packet(data, length). use a length of -1 to indicate the packet is to be dropped.
            //this will directly be used by WSASend so don't fuck it up!
            //p.packet[10] = 0x7E;
            if (match)
                return new Packet(new byte[] {}, 0);

            return new Packet(p.packet, p.packet.Length);
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

    #region packet definitions
    /// <summary>
    /// classes for packet structures.
    /// the packet parsing routines will create instances of these, with a base class of
    /// packet_Base implementing a default constructor getting at least the name of the sender
    /// (in the event it is called from WSARecv), or variations of SYSTEM or original_player_name
    /// in the event of WSASend calls. Typically the name won't be used unless the data is already
    /// known to be of a type where name is useful, so any irregularities are of no consequence.
    /// </summary>
    public class packet_Base
    {
        internal byte type;
        internal int playerId;
        internal string name;
        internal byte[] packet;

        public packet_Base(byte[] data)
        {
            packet = data;
            type = data[4];
            playerId = data[5];
            name = Main.player[playerId].name;
            if (name == "")
                name = "SYSTEM";
        }
    }

    /// <summary>
    /// Chat Message/Broadcast/Announce Information
    /// </summary>
    public class packet_ChatMsg : packet_Base
    {
        internal string text;
        internal byte R;
        internal byte G;
        internal byte B;

        internal packet_ChatMsg(byte[] data)
            : base(data)
        {
            R = data[6];
            G = data[7];
            B = data[8];
            text = Encoding.ASCII.GetString(data, 9, data.Length - 9);
        }

    }

    /// <summary>
    /// Player State Information
    /// </summary>
    public class packet_PlayerState : packet_Base
    {
        internal Vector2 position;
        internal Vector2 velocity;
        internal int selectedItemId;
        internal int buttonState;

        internal packet_PlayerState(byte[] data)
            : base(data)
        {
            buttonState = data[6];
            selectedItemId = data[7];
            position.X = BitConverter.ToSingle(data, 8);
            position.Y = BitConverter.ToSingle(data, 12);
            velocity.X = BitConverter.ToSingle(data, 16);
            velocity.Y = BitConverter.ToSingle(data, 20);

        }
    }

    /// <summary>
    /// Packet struct, used to pass around data during processing for modification,
    /// and passed back to WSASend. Requires marshalling before it can be used as 
    /// the buffer for the final WSASend call
    /// </summary>
    public struct Packet
    {
        internal byte[] data;
        internal int length;

        internal Packet(byte[] d, int l)
        {
            data = d;
            length = l;
        }
    }
    #endregion
}
