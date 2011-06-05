using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RomTerraria.AccountManagement;
using Terraria;
using System.Reflection;

namespace RomTerraria
{
    class Commands
    {
        public static byte[] bannedItems = new byte[] { 0xCF,   //lava bucket
                                                        0xA7 }; //dynamite
    
        private static bool itemBanEnabled;

        private static int[] justHostiled = new int[10];
        private static int numberHostiled;
        private static Assembly terrariaAssembly;
        private static Type netplay;
        private static FieldInfo serverSock;

        static Commands()
        {

            terrariaAssembly = Assembly.GetAssembly(typeof(Main));
            if (terrariaAssembly == null)
            {
                return;
            }
            netplay = terrariaAssembly.GetType("Terraria.Netplay");

            foreach (var f in netplay.GetFields())
            {
                if (f.Name == "serverSock")
                {
                    serverSock = f;
                }
            }
        }

        /// <summary>
        /// Process a data packet, determining data type and where defined, 
        /// calling additional handlers to break apart data into a more workable
        /// format.
        /// </summary>
        /// <param name="data">The data.</param>
        public static Packet ProcessData(byte[] data, int direction)
        {
            byte type = data[4];

            string prefix;
            //string details = null;
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
                    // Clear AccountManager refs in HandleGreeting( )
                    packet = HandleGreeting( data );        
                    break;
                case 0x0D:
                    prefix = "PLAYER STATE CHANGE";
                    packet = HandlePlayerState(data);
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
                    //and ALL handlers return a Packet struct as output.

                    break;
                case 0x1A:
                    prefix = "PLAYER HURT PLAYER";
                    break;
                case 0x1B:
                    prefix = "PLAYER FIRED PROJECTILE";
                    break;
                case 0x1C:
                    prefix = "?PLAYER PROJECTILE HIT NPC";
                    break;
                case 0x1D:
                    prefix = "PLAYER DESTROY PROJECTILE";
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
                    packet = handleDeath(data);
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

            //this code is currently unreachable, as the hook is on direction 0 only.
            //if( direction == 0 ) {
            //    var sBuffer = new StringBuilder( );
            //    foreach( byte t in data ) {
            //        sBuffer.Append( Convert.ToInt32( t ).ToString( "x" ).PadLeft( 2, '0' ) + " " );
            //    }

            //    Console.WriteLine( "{0} ::{1}\n", prefix, sBuffer.ToString( ).ToUpper( ) );
            //}
            return packet;

        }

        //basic death handler.
        private static Packet handleDeath(byte[] data)
        {
            //right now, death handler just fixes up hostile state if a star was dropped
            //on them using .star.);
            var p = new packet_PlayerDied(data);

            if (Main.player[data[5]].hostile)
            {
                for (var i = 0; i < numberHostiled; i++)
                {
                    if (justHostiled[i] == data[5])
                    {
                        justHostiled[i] = 0x00;
                        numberHostiled--;
                        Main.player[data[5]].hostile = false;
                    }
                }
            }
            return new Packet(data, data.Length);
        }

        private static Packet HandlePlayerState(byte[] data)
        {
            var packet = new Packet(data, data.Length);
            if (!itemBanEnabled) return packet;

            var p = new packet_PlayerState(data);
            if (p.UsingItem)
            {
                foreach (byte i in bannedItems)
                {
                    var id = Main.player[p.PlayerId].inventory[p.SelectedItemId].type;
                    if (id == i)
                    {
                        packet = CreateDummyPacket(data);
                        var itemName = Main.player[p.PlayerId].inventory[p.SelectedItemId].name;
                        //MakeItHarder.serverConsole.AddChatLine("Player: " + p.Name + " tried to use " + itemName);
                        Console.WriteLine("Player: {0} tried to use {1}\n", Main.player[p.PlayerId].name, itemName);

                    }
                }
            }
            return packet;

        }

        private static Packet HandleGreeting( byte[] data ) {
            AccountManager.Logout( data[5] );
            return new Packet( data, data.Length );
        }

        private static Packet CreateDummyPacket(byte[] data)
        {
            int msgLength = 1;
            var newPacket = new Packet();
            //info on message header
            //bytes 0-3 are payload length
            //byte 4 is message type
            //payload is bytes 5-end of packet
            //payload length must match expected length since C# uses Streams to handle data
            //and EndRead retrieves the expected data length before WSARecv is invoked.

            var msgHeader = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x02 }; //new data, type 0x02 is ignored by the server
            var finalPacket = new byte[data.Length];
            int lenDiff = 0;
            if (msgHeader.Length < data.Length)
                lenDiff = data.Length - msgHeader.Length;

            var msgContents = new byte[lenDiff];
            for (int b = 0; b < lenDiff; b++)
            {
                msgContents[b] = 0x2e; //just fill with periods, it's ignored anyway
                msgLength++;
            }

            var msgLenBytes = BitConverter.GetBytes(msgLength);
            Buffer.BlockCopy(msgLenBytes, 0, msgHeader, 0, 4);

            Buffer.BlockCopy(msgHeader, 0, finalPacket, 0, msgHeader.Length);
            Buffer.BlockCopy(msgContents, 0, finalPacket, msgHeader.Length, msgContents.Length);
            newPacket.Data = finalPacket;
            newPacket.Length = finalPacket.Length;
            return newPacket;


        }

        private static Packet HandleChatMsg(byte[] data)
        {
            var p = new packet_ChatMsg(data); //initialize packet class, populate fields from data
            //...
            //act on packet fields, generate new data etc.
            var match = false;
            if (p.Text[0] == 0x2E) // match backslash
            {
                match = true;

                //split is probably no slower than substring, if the size of the strings
                //we are pulling from user-text is variable.
                var commands = p.Text.Split(' ');
                commands[0] = commands[0].ToLower();

                switch (commands[0])
                {
                    case( ".login" ):
                        cmdLogin( commands, p );
                        break;
                    case (".meteor"):
                        WorldEvents.SpawnMeteorCB();
                        break;
                    case (".broadcast"):
                        cmdBroadcast(commands, p);
                        break;
                    case (".kick"):
                        if( AccountManager.CheckRights( p.PlayerId, Rights.ADMIN ) ) {
                            cmdKick( commands, p );
                        }
                        break;
                    case (".itemban"):
                        cmdItemBanToggle(p);
                        break;
                    case (".ban"):
                        cmdBanUser(commands, p);
                        break;
                    case (".kickban"):
                        cmdKickBan(commands, p);
                        break;
                    case (".star"):
                        cmdLaunchStar(commands, p);
                        break;
                    default:
                        cmdUnknown(commands, p);
                        break;
                }
            }

            if (match)
                return CreateDummyPacket(data);

            return new Packet(p.Packet, p.Packet.Length);
        }

        private static void cmdLogin( string[] commands, packet_ChatMsg p ) {
            if( commands.Length == 2 ) {
                if( AccountManager.Login( p.PlayerId, commands[1] ) ) {
                   return;
                }
            }

            SendChatMsg( "USAGE: .login <username> <password>", p.PlayerId, Color.GreenYellow );
        }

        private static void cmdLaunchStar(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var name = GetParamsAsString(commands);
            if (name == null)
            {
                SendChatMsg("USAGE: .star <player>", packetChatMsg.PlayerId, Color.GreenYellow);
                return;
            }
            var id = getPlayerIdFromName(name);

            //if the player is already hostile, don't do anything
            if (!Main.player[id].hostile)
            {
                Main.player[id].hostile = true;
                justHostiled[numberHostiled] = id;
                numberHostiled++;
            }

            Main.player[Main.myPlayer].hostile = true; //i'm not sure what this actually affects?
                                                       //as far as i can tell, main.myplayer only owns projectile
                                                       //12 [star] and everything else is either unowned, or self-owned
            Projectile.NewProjectile(Main.player[id].position.X, Main.player[id].position.Y-300, 0f, 5f, 12, 1000, 10f, Main.myPlayer);
            SendChatMsg("Sending death to " + name + ".", packetChatMsg.PlayerId, Color.Red);

        }

        private static void cmdKickBan(string[] commands, packet_ChatMsg packetChatMsg)
        {
            //NOTE: much more efficient to use string.Substring(x); to get the name from the command
            var name = GetParamsAsString(commands);
            if (name == null)
            {
                SendChatMsg("USAGE: .kickban <player>", packetChatMsg.PlayerId, Color.GreenYellow);
                return;
            }

            var id = getPlayerIdFromName(name);
            if (id != -1)
            {
                kickUser(id);
                banUser(id);
                SendChatMsg("Player " + name + " kicked and banned.", packetChatMsg.PlayerId, Color.Red);
            }
        }

        private static void cmdBanUser(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var name = GetParamsAsString(commands);
            if (name == null)
            {
                SendChatMsg("USAGE: .ban <player>", packetChatMsg.PlayerId, Color.GreenYellow);
                return;
            }

            var id = getPlayerIdFromName(name);
            if (id != -1) {
                    Netplay.AddBan(id);
                    SendChatMsg("Banned user: " + name, packetChatMsg.PlayerId, Color.Red);
                    return;
            }
        }


        private static void cmdItemBanToggle(packet_ChatMsg p)
        {
            itemBanEnabled = !itemBanEnabled;
            string state = itemBanEnabled ? "enabled." : "disabled.";
            SendChatMsg("Item ban " + state, p.PlayerId, Color.Green);
        }

        /// <summary>
        /// Kick a named player from the server
        /// </summary>
        /// <param name="commands">The commands array</param>
        /// <param name="packetChatMsg">Data class for ChatMsg</param>
        private static void cmdKick(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var name = GetParamsAsString(commands);
            if (name == null)
            {
                SendChatMsg("USAGE: .kick <player>", packetChatMsg.PlayerId, Color.GreenYellow);
                return;
            }

            var t = (ServerSock[]) serverSock.GetValue(null);
            
            //now that this can have up to 255 sockets, maybe it's best to just iterate up to max.
            //nb: this might not even be necessary, if user[i] maps directly to socket[i] which i think it might.
            for (var i = 0; i < Main.maxNetPlayers;i++ )
            {
                if (t[i].name == name)
                {
                    kickUser(i);
                    SendChatMsg("Player " + name + " kicked.", packetChatMsg.PlayerId, Color.Red);
                    return;
                }
            }

            /*foreach (ServerSock b in t)
            {
                if (b.name == name)
                {
                    b.kill = true;
                    return;
                }
            }*/

        }

        /// <summary>
        /// Helper function, returns all arguments to a command as a single, delimited string.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        private static string GetParamsAsString(string[] commands, string delimiter = " ")
        {
            if (commands.Length == 1)
                return null;

            string param = null;

            for (int i = 1; i < commands.Length; i++)
                param = param + delimiter + commands[i];

            return param.Trim();
        }

        private static void banUser(int id)
        {
            Netplay.AddBan(id);
            return;
        }

        private static void kickUser(int id)
        {
            var t = (ServerSock[])serverSock.GetValue(null);
            t[id].kill = true;
            return;
        }

        private static int getPlayerIdFromName(string name)
        {
            for (int i = 0; i < Main.maxNetPlayers; i++)
            {
                if (Main.player[i].name.ToLower() == name.ToLower())
                {
                    return i;
                }
            }
            return -1;
        }
        //END OF HELPERS

        /// <summary>
        /// Helper function to send chat messages
        /// </summary>
        /// <param name="msg">Message to send</param>
        /// <param name="playerId">Player index to send to, or -1 for broadcast</param>
        /// <param name="color">The color, as a Color type.</param>
        private static void SendChatMsg(string msg, int playerId, Color color)
        {
            NetMessage.SendData(25, playerId, -1, msg, 255, color.R, color.G, color.B);
            return;
        }

        /// <summary>
        /// Unknown Command Handler
        /// </summary>
        /// <param name="commands">TArray of space-delimited words</param>
        /// <param name="p">Data class for ChatMsg</param>
        private static void cmdUnknown(string[] commands, packet_ChatMsg p)
        {
            SendChatMsg("ERROR: Unknown command " + commands[0], p.PlayerId, Color.GreenYellow);
            return;
        }

        /// <summary>
        /// Broadcast Command Handler
        /// </summary>
        /// <param name="commands">The array of space-delimited words</param>
        /// <param name="p">Data class for ChatMsg</param>
        private static void cmdBroadcast(string[] commands, packet_ChatMsg p)
        {
            var msg = GetParamsAsString(commands);
            if (msg == null)
            {
                SendChatMsg("USAGE: .broadcast <message>", p.PlayerId, Color.GreenYellow);
                return;
            }

            SendChatMsg(msg, -1, Color.Orange);
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
    //USE A CLASS INHERITING PACKET_BASE UNLESS YOU KNOW THAT PLAYERID IS AT INDEX 5
    //CHILD CLASSES OVERRIDE PLAYERID IF THE PACKET THEY ARE DEFINING DIFFERS FROM BASE
    public class packet_Base
    {
        internal byte Type;
        internal int PlayerId;
        internal byte[] Packet;

        public packet_Base(byte[] data)
        {
            Packet = data;
            Type = data[4];
        }
    }

    /// <summary>
    /// Chat Message/Broadcast/Announce Information
    /// </summary>
    public class packet_ChatMsg : packet_Base
    {
        internal string Text;
        internal byte R;
        internal byte G;
        internal byte B;

        internal packet_ChatMsg(byte[] data)
            : base(data)
        {
            PlayerId = data[5];
            R = data[6];
            G = data[7];
            B = data[8];
            Text = Encoding.ASCII.GetString(data, 9, data.Length - 9);
        }

    }

    public class packet_FireProjectile : packet_Base
    {
        internal short Identity;
        internal int ProjectileType;
        internal int Owner;
        internal Vector2 Position;
        internal Vector2 Velocity;
        internal float KnockBack;
        internal short Damage;

        internal packet_FireProjectile(byte[] data)
            : base(data)
        {

            Identity = BitConverter.ToInt16(data, 5);
            Position.X = BitConverter.ToSingle(data, 7);
            Position.Y = BitConverter.ToSingle(data, 11);
            Velocity.X = BitConverter.ToSingle(data, 15);
            Velocity.Y = BitConverter.ToSingle(data, 19);
            KnockBack = BitConverter.ToSingle(data, 23);
            Damage = BitConverter.ToInt16(data, 27);

            PlayerId = data[29]; //overriding base class constructor

            Owner = data[29];
            ProjectileType = data[30];

            //rest of the bytes in the packet are AI related
            //it would be a fair performance hit to get human-readable
            //data from it, since it's set when the packet is received
            //but only read when it needs to be acted on.

        }
    }
    //

    public class packet_PlayerDied : packet_Base
    {
        internal int HitDirection;
        internal double DamageTaken;
        internal bool WasPVP;

        internal packet_PlayerDied(byte[] data) 
            : base(data)
        {
            PlayerId = data[5];
            HitDirection = data[6] - 1;
            DamageTaken = BitConverter.ToInt16(data, 7);
            WasPVP = data[9] != 0;
        }
    }

    /// <summary>
    /// Player State Information
    /// </summary>
    public class packet_PlayerState : packet_Base
    {
        internal Vector2 Position;
        internal Vector2 Velocity;
        internal int SelectedItemId;
        internal int ButtonState;
        internal bool KeyUp;
        internal bool KeyDown;
        internal bool KeyLeft;
        internal bool KeyRight;
        internal bool KeyJump;
        internal bool UsingItem;
        internal int Direction = -1;

        internal packet_PlayerState(byte[] data)
            : base(data)
        {
            PlayerId = data[5];
            ButtonState = data[6];
            SelectedItemId = data[7];
            Position.X = BitConverter.ToSingle(data, 8);
            Position.Y = BitConverter.ToSingle(data, 12);
            Velocity.X = BitConverter.ToSingle(data, 16);
            Velocity.Y = BitConverter.ToSingle(data, 20);

            //advanced state
            if ((ButtonState & 1) == 1) KeyUp = true;
            if ((ButtonState & 2) == 2) KeyDown = true;
            if ((ButtonState & 4) == 4) KeyLeft = true;
            if ((ButtonState & 8) == 8) KeyRight = true;
            if ((ButtonState & 16) == 16) KeyJump = true;
            if ((ButtonState & 32) == 32) UsingItem = true;
            if ((ButtonState & 64) == 64) Direction = 1;
            //
        }
    }

    /// <summary>
    /// Packet struct, used to pass around data during processing for modification,
    /// and passed back to WSASend. Requires marshalling before it can be used as 
    /// the buffer for the final WSASend call
    /// </summary>
    public struct Packet
    {
        internal byte[] Data;
        internal int Length;

        internal Packet(byte[] d, int l)
        {
            Data = d;
            Length = l;
        }
    }
    #endregion
}
