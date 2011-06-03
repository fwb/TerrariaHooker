using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using System.Reflection;

namespace RomTerraria
{
    class Commands
    {
        public static string[] ignoreList = new string[100];
        public static byte[] bannedItems = new byte[] { 0xCF, 0xA7 }; //banned items.0xCF = lava bucket, 0xA7 = dynomite
        public static int numberIgnored;

        private static bool itemBanEnabled;


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
            if (direction == 0)
            {
                var ignored = checkIgnores(data);
                if (ignored)
                    return CreateDummyPacket(data);

            }

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

            if (direction == 1) {
                var sBuffer = new StringBuilder();
                foreach (byte t in data)
                {
                    sBuffer.Append(Convert.ToInt32(t).ToString("x").PadLeft(2, '0') + " ");
                }

                MakeItHarder.serverConsole.AddChatLine(prefix + " : :" + sBuffer.ToString().ToUpper());
                }
            return packet;

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
                        MakeItHarder.serverConsole.AddChatLine("Player: " + p.Name + " tried to use " + itemName);
                    }
                }
            }
            return packet;

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

        private static bool checkIgnores(byte[] data)
        {
            byte playerId = data[5];
            if (playerId > 8)
                return false;

            string playerName = Main.player[playerId].name;
            return ignoreList.Any(p => p == playerName);
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
                var commands = p.Text.Split(' ');
                commands[0] = commands[0].ToLower();

                switch (commands[0])
                {
                    case (".meteor"):
                        WorldEvents.SpawnMeteorCB();
                        break;
                    case (".broadcast"):
                        cmdBroadcast(commands, p);
                        break;
                    case (".ignore"):
                        cmdIgnore(commands, p);
                        break;
                    case (".kick"):
                        cmdKick(commands, p);
                        break;
                    case (".itemban"):
                        cmdItemBanToggle(p);
                        break;
                    case (".unignore"):
                        cmdUnIgnore(commands, p);
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

        private static void cmdUnIgnore(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var name = GetParamsAsString(commands);
            if (name == null)
            {
                SendChatMsg("USAGE: .ignore <player>", packetChatMsg.PlayerId, Color.GreenYellow);
                return;
            }

            for (int i = 0; i < numberIgnored;i++ )
            {
                if (ignoreList[i] == name)
                {
                    ignoreList[i] = null;
                    SendChatMsg("Player " + name + " unignored.", packetChatMsg.PlayerId, Color.Red);
                }
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
            foreach (ServerSock b in t)
            {
                if (b.name == name)
                {
                    b.kill = true;
                    return;
                }
            }

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

            int i = 1;
            string param = null;
            while (i < commands.Length)
            {
                param = param + delimiter + commands[i];
                i++;
            }
            param = param.Trim();
            return param;
        }

        private static void cmdIgnore(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var name = GetParamsAsString(commands);
            if (name == null)
            {
                SendChatMsg("USAGE: .ignore <player>", packetChatMsg.PlayerId, Color.GreenYellow);
                return;
            }

            ignoreList[numberIgnored] = name;
            numberIgnored++;
            SendChatMsg("Player " + name + " ignored.", packetChatMsg.PlayerId, Color.Red);
        }

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
            SendChatMsg("ERROR: Unknown command " + commands[0], -1, Color.GreenYellow);
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
    public class packet_Base
    {
        internal byte Type;
        internal int PlayerId;
        internal string Name;
        internal byte[] Packet;

        public packet_Base(byte[] data)
        {
            Packet = data;
            Type = data[4];
            PlayerId = data[5];
            Name = Main.player[PlayerId].name;
            if (Name == "")
                Name = "SYSTEM";
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
            R = data[6];
            G = data[7];
            B = data[8];
            Text = Encoding.ASCII.GetString(data, 9, data.Length - 9);
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
