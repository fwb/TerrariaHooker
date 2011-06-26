using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Terraria;
using System.Reflection;
using TerrariaHooker.AccountManagement;
using System.Timers;

namespace TerrariaHooker
{
    public class PlayerInfo
    {
        public bool Whitelisted;
        public bool ForcedHostile;
        public bool Teleported; //was teleported by star death
        public PlayerInfo()
        {
        }

    }

    /// <summary>
    /// Just a class to hold details of queued teleports
    /// </summary>
    public class TP
    {
        public int targetId;
        public int x;
        public int y;
    }

    public class ChatCommand
    {
        public Commands.ChatCommandHandler Handler;
        public string Trigger;
        public string UsageString;
        public Rights PermissionsRequired;

        public ChatCommand(string trigger, Commands.ChatCommandHandler handler, Rights permissions, string usage)
        {
            Trigger = trigger;
            Handler = handler;
            PermissionsRequired = permissions;
            UsageString = usage;
        }

    }

    public class Commands
    {
        
        private static Properties.Settings settings = new Properties.Settings();
        
        public static byte[] riskItems = new byte[] { 0xCF,   //lava bucket
                                                      0xA7 }; //dynamite

        private static bool itemRiskEnabled = false;
        //public static bool whitelistEnabled;
        public static bool allowUnwhiteLogin;
        public static PlayerInfo[] player = new PlayerInfo[255];


        public static bool protectSpawn;
        public static bool spawnAllowUse;
        public static bool spawnAllowBreak;
        public static bool spawnAllowPlace;

        //REVOKED privileges for unwhitelisted/anonymous users.
        private const Actions anonPrivs = Actions.NOBREAKBLOCK | Actions.NOUSEITEMS| Actions.NOCOMMANDS;
        
        private static Assembly terrariaAssembly;
        private static Type netplay;
        private static FieldInfo serverSock;
        private static Type npc;
        private static FieldInfo defaultMaxSpawns;
        private static FieldInfo defaultSpawnRate;
        //private static TextWriter w = new StreamWriter(ServerConsole._out);

        //teleport timer related
        private static LinkedList<TP> teleQueue = new LinkedList<TP>();
        private static System.Timers.Timer tTimer;
        //
        private static Random random = new Random();

        private static LinkedList<ChatCommand> chatCommands = new LinkedList<ChatCommand>();
        public delegate bool ChatCommandHandler(string[] command, packet_ChatMsg p);
        
        internal static int MAX_SPAWNS = 50; //maximum number of spawns using .spawn (at a single time)
        internal const int MAX_LINE_LENGTH = 70;
        internal const int MAX_LANDMARK_LENGTH = 20;

        //10x10 box around spawn
        internal static int SPAWN_PROTECT_WIDTH = 5;
        internal static int SPAWN_PROTECT_HEIGHT = 5;

        //chest used to contain MAGICAL ITEMS OF MYSTERY!
        public static int MAGIC_CHEST_ID = -1;

        public static int SetProtectWidth
        {
            set { SPAWN_PROTECT_WIDTH = value;  }
        }

        public static int SetProtectHeight
        {
            set { SPAWN_PROTECT_HEIGHT = value; }
        }

        static Commands()
        {
            //COMMAND DEFINITION BLOCK
            //non-admin commands
            chatCommands.AddLast(new ChatCommand(".login", cmdLogin, Rights.NONE, ".login <username>"));
            chatCommands.AddLast(new ChatCommand(".landmark", cmdLandMark, Rights.NONE, ".landmark <name>"));
            chatCommands.AddLast(new ChatCommand(".coords", cmdCoords, Rights.NONE, null));
            chatCommands.AddLast(new ChatCommand(".help", cmdHelp, Rights.NONE, null));

            chatCommands.AddLast(new ChatCommand(".version", cmdVersion, Rights.NONE, null));

            //admin commands
            //user control commands
            chatCommands.AddLast(new ChatCommand(".kick", cmdKick, Rights.ADMIN, ".kick <player>"));
            chatCommands.AddLast(new ChatCommand(".ban", cmdBanUser, Rights.ADMIN, ".ban <player>"));
            chatCommands.AddLast(new ChatCommand(".kickban", cmdKickBan, Rights.ADMIN, ".kickban <player>"));

            //general commands
            chatCommands.AddLast(new ChatCommand(".broadcast", cmdBroadcast, Rights.ADMIN, ".broadcast <message>"));

            //teleport commands
            chatCommands.AddLast(new ChatCommand(".teleport", cmdTeleport, Rights.ADMIN, ".teleport <player> <xcoord:ycoord>; or .teleport <xcoord:ycoord>"));
            chatCommands.AddLast(new ChatCommand(".teleportto", cmdTeleportTo, Rights.ADMIN, ".teleportto <player>"));

            //item, npc and object spawn commands
            chatCommands.AddLast(new ChatCommand(".spawn", cmdSpawnNPC, Rights.ADMIN, ".spawn <player> <npcid>x[count]"));
            chatCommands.AddLast(new ChatCommand(".itemat", cmdCreateItem, Rights.ADMIN, ".itemat <player> <itemid>x[quantity]"));
            chatCommands.AddLast(new ChatCommand(".meteor", cmdSpawnMeteor, Rights.ADMIN, null));
            chatCommands.AddLast(new ChatCommand(".star", cmdLaunchStar, Rights.ADMIN, ".star <target>"));

            //settings
            chatCommands.AddLast(new ChatCommand(".noenemy", cmdDisableSpawns, Rights.ADMIN, ".noenemy on|off"));
            chatCommands.AddLast(new ChatCommand(".itemrisk", cmdItemBanToggle, Rights.ADMIN, null));

            //whitelist related
            chatCommands.AddLast(new ChatCommand(".wl", cmdWhitelist, Rights.ADMIN, ".wl (a)dd | (d)el | (r)efresh | on | off"));
            
            
            //


            terrariaAssembly = Assembly.GetAssembly(typeof(Main));
            if (terrariaAssembly == null)
            {
                return;
            }
            netplay = terrariaAssembly.GetType("Terraria.Netplay");
            npc = terrariaAssembly.GetType("Terraria.NPC");

            foreach (var f in netplay.GetFields())
            {
                if (f.Name == "serverSock")
                {
                    serverSock = f;
                }
            }

            foreach (var f in npc.GetFields(BindingFlags.Static | BindingFlags.NonPublic))
            {
                
                
                if (f.Name == "defaultMaxSpawns")
                {
                    defaultMaxSpawns = f;
                    continue;
                }
                if (f.Name == "defaultSpawnRate")
                {
                    defaultSpawnRate = f;
                    continue;
                }
            }

            AccountManager.WhitelistActive = settings.EnableWhitelist;
            //allowUnwhiteLogin = settings.EnableAnonLogin;
            allowUnwhiteLogin = false;
        }

        private static bool cmdVersion(string[] command, packet_ChatMsg p)
        {
            SendChatMsg("Version WOOP", p.PlayerId, Color.Maroon);
            return true;
        }

        /// <summary>
        /// Process a data packet, determining data type and where defined,
        /// calling additional handlers to break apart data into a more workable
        /// format.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="direction">The direction of travel. 0 = RECV, 1 = SEND</param>
        /// <param name="handle">The socket handle.</param>
        /// <param name="offset">The offset in the data to start reading from.</param>
        /// <returns>
        /// A packet structure, complete with Data and Data Length
        /// </returns>
        public static Packet ProcessData(byte[] data, int direction, int handle, int offset = 0)
        {
            //get playerid from socket handle
            int pid = getPlayerIdFromHandle(handle);

            byte type;
            try { type = data[offset+4]; } 
            catch { return new Packet(data, data.Length); }
 
            int length = data[offset] + 4;

            //some preemptive terribleness. if the detected length of the packet is greater
            //than the entire remaining length of the packet, return because the packet is 
            //split and unable to be acted on. also return if the length is less than 6, because 6 is the
            //minimum required for playerId.
            if (length > data.Length - offset || length < 6)
                return new Packet(data, data.Length);

            var nData = new byte[length]; //buffer containing just the packet we're working om

            //there's some cases where so much data is pushed to the server it exceeds the buffer length
            //(1024) and splits into multiples, leaving one partial packet at the end of the buffer and additional
            //broken data in the next buffer. I don't think it's worth creating edge-cases for this as it's still
            //being recieved by the server, just
            try
            {
                Buffer.BlockCopy(data, offset, nData, 0, length);
            } catch
            {
                //OutputPacket(data, "BLOCKCOPY ERROR");
                return new Packet(data,data.Length);
            }

            //packet is JUST the current packet.
            var packet = new Packet(nData, nData.Length);
            string prefix;

            switch (type)
            {
                case 0x01:
                    prefix = "USER LOGIN P1";
                    // Clear AccountManager refs in HandleGreeting( )
                    packet = HandleGreeting( nData, pid );
                    if( AccountManager.WhitelistActive ) {
                        CheckWhitelist( pid );
                    }
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
                    if( AccountManager.WhitelistActive && !player[pid].Whitelisted )
                        SendChatMsg("You are not whitelisted, some actions are restricted.", pid, Color.Peru);
      
                    break;
                case 0x0D:
                    prefix = "PLAYER STATE CHANGE";
                    packet = HandlePlayerState(nData, pid);
                    
                    break;
                case 0x10:
                    prefix = "PLAYER CURRENT/MAX HEALTH UPDATE";
                    break;
                case 0x11:
                    prefix = "PLAYER DESTROY/CREATE BLOCK";
                    packet = HandleBlockChange(nData, pid);

                    break;
                case 0x13:
                    prefix = "PLAYER USE DOOR";
                    break;
                case 0x15:
                    prefix = "PLAYER PICKUP/DROP ITEM";
                    break;
                case 0x16:
                    prefix = "DETERMINE ITEM OWNER"; 
                    break;
                case 0x18:
                    prefix = "PLAYER HURT NPC";
                    break;
                case 0x19:
                    prefix = "CHAT MESSAGE";
                    packet = HandleChatMsg(nData); 

                    break;
                case 0x1A:
                    prefix = "PLAYER HURT PLAYER";
                    break;
                case 0x1B:
                    prefix = "PLAYER PROJECTILE UPDATE";
                    break;
                case 0x1C:
                    prefix = "?PLAYER PROJECTILE HIT NPC?";
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
                case 0x29:  //hard to give a short description, but this packet
                            //seems to set the player-following item's animation
                            //and rotation details. e.g. those floating balls of light
                    prefix = "PLAYER ITEM ANIMATION/ROTATION";
                    break;
                case 0x2A:
                    prefix = "PLAYER CURRENT/MAX MANA UPDATE";
                    break;
                case 0x2c:
                    prefix = "PLAYER DIED";
                    packet = handleDeath(nData);

                    break;
                case 0x2d:
                    prefix = "PLAYER PARTY UPDATE";
                    break;
                case 0x2E:
                    prefix = "PLAYER READ SIGN";
                    break;
                case 0x31:
                    prefix = "PLAYER SPAWN";
                    break;

                default:
                    prefix = "UNDEFINED";
                    break;
            }

            //write the returned packet back into the original data buffer. packet can either be
            //the original data,  modified original data, or a dummy packet created.
            Buffer.BlockCopy(packet.Data, 0, data, offset, packet.Length);

            //now, fpacket is a clone of the updated packet
            var fpacket = new Packet(data, data.Length);

            #if DEBUG
            //OutputPacket(nData, prefix);
            #endif

            //if theres more data in the buffer than the packet just read references, process data
            //again with the offset covering the just-parsed packet. 
            //logic: ProcessData returns a new Packet(fullpacket, length);
            //last-updated packet should cascade down to the parent.
            if (offset + length < data.Length)
                fpacket = ProcessData(data, 0, handle, offset + length);

            //the updated packet data is in fpacket, either becasue we're at the end of the packet,
            //or because we weren't and fpacket was passed back to us from the child call.
            return new Packet(fpacket.Data, fpacket.Length);

        }

        /// <summary>
        /// Retrieves ID of player sending data from the socket handle data was recieved on.
        /// </summary>
        /// <param name="handle">The socket handle.</param>
        /// <returns>player id</returns>
        private static int getPlayerIdFromHandle(int handle)
        {
            int pid = -1;
            int h;
            for (int i = 0; i <= Main.maxNetPlayers; i++)
            {
                h = Netplay.serverSock[i].tcpClient.Client.Handle.ToInt32();
                if (h == handle)
                {
                    //ensure a client is connected on this socket, and not return the pid of
                    //a disconnected, disposed/disposing socket.
                    if (Netplay.serverSock[i].tcpClient.Client.Connected == false)
                        continue;

                    pid = i;
                    break;
                }
            }
            return pid;
        }

        /// <summary>
        /// Output the recieved/sent packet to console, as hexadecimal with prefix
        /// </summary>
        /// <param name="data">The packet data</param>
        /// <param name="prefix">The human-readable prefix</param>
        private static void OutputPacket(byte[] data, string prefix)
        {
                var sBuffer = new StringBuilder( );
                foreach( byte t in data ) {
                    sBuffer.Append( Convert.ToInt32( t ).ToString( "x" ).PadLeft( 2, '0' ) + " " );
                }

               Console.WriteLine( "{0} :: {1}", prefix, sBuffer.ToString( ).ToUpper( ) );
        }

        private static Packet HandleBlockChange(byte[] data, int pid)
        {
            var p = new packet_BlockChange(data);
            p.PlayerId = pid;

            var packet = new Packet(data, data.Length);
            #region PROTECT SPAWN
            //create a rect around the spawn, check if the point being acted on is part of the spawn rect.
            if (protectSpawn)
            {
                bool isIn = false;
                var d = new Vector2(p.Position.X, p.Position.Y);
                var rect = new Rectangle(Main.spawnTileX - (SPAWN_PROTECT_WIDTH * 16),
                                         Main.spawnTileY - (SPAWN_PROTECT_HEIGHT * 16),
                                         SPAWN_PROTECT_WIDTH * 2* 16,
                                         SPAWN_PROTECT_HEIGHT * 2 * 16);

                if ((d.X > rect.Left && d.X < rect.Right) && (d.Y > rect.Top && d.Y < rect.Bottom))
                    isIn = true;

                //destroying and creating blocks
                if ((p.ActionType == 0 || p.ActionType == 4 || p.ActionType == 1) && (isIn))
                {
                    //SendChatMsg("You can not make block changes here.", p.PlayerId, Color.Red);
                    return CreateDummyPacket(data);
                }
            }
            #endregion

            #region ANON LOGIN CODE

            if( AccountManager.WhitelistActive && !player[p.PlayerId].Whitelisted )
            {
                if (anonPrivs.Has(Actions.NOBREAKBLOCK))
                {
                    SendChatMsg("You cannot destroy blocks until whitelisted. x" + pid, pid, Color.Purple);
                    return CreateDummyPacket(data);
                }
            }
            
            #endregion
            return packet;
        }

        //basic death handler.
        private static Packet handleDeath(byte[] data)
        {
            //right now, death handler just fixes up hostile state if a star was dropped
            //on them using .star, or teleported by being killed by a star);
            var p = new packet_PlayerDied(data);

            FixHostileFlag(p.PlayerId);

            if (Main.player[p.PlayerId].hostile)
            {
                if (player[p.PlayerId].Teleported) //teleported by being killed by star
                {
                    player[p.PlayerId].Teleported = false;
                    NetMessage.SendData(0x0C, p.PlayerId, -1, "", p.PlayerId); //immediate respawn to teleported people
                    NetMessage.SendData(0x07, p.PlayerId, -1, "", p.PlayerId); //reset server details (to client)
                }

            }
            return new Packet(data, data.Length);
        }

        private static void FixHostileFlag(int playerId)
        {
            if (player[playerId].ForcedHostile)
            {
                player[playerId].ForcedHostile = false;
                Main.player[playerId].hostile = false;
            }
        }

        private static Packet HandlePlayerState(byte[] data, int pid)
        {
            var packet = new Packet(data, data.Length);

            FixHostileFlag(pid);

            var p = new packet_PlayerState(data);
            p.PlayerId = pid;

            if (p.UsingItem)
               {
                #region ANON LOGIN CODE
                   if( AccountManager.WhitelistActive && !player[p.PlayerId].Whitelisted )
                {
                    if (anonPrivs.Has(Actions.NOUSEITEMS))
                    {
                        SendChatMsg("You cannot use items until whitelisted. x" + pid, pid, Color.Purple);
                        return CreateDummyPacket(data);
                    }
                }
                #endregion
                if (itemRiskEnabled)
                {
                    foreach (byte i in riskItems)
                    {
                        var id = Main.player[p.PlayerId].inventory[p.SelectedItemId].type;
                        if (id != i) continue;
                        //packet = CreateDummyPacket(data);
                        var itemName = Main.player[p.PlayerId].inventory[p.SelectedItemId].name;
                        Console.WriteLine("RISK: Player '{0}' used {1}", Main.player[p.PlayerId].name, itemName);
                    }
                }
               }
        
            return packet;

        }

        private static void CheckWhitelist( int pid )
        {
            player[pid].Whitelisted = false;

            try {
                //var t = (ServerSock[])serverSock.GetValue( null );
                var playerId = pid;
                var endpoint = Netplay.serverSock[playerId].tcpClient.Client.RemoteEndPoint.ToString( );
                var ip = Utils.ParseEndPointAddr( endpoint );
                if( !AccountManager.IsAllowed( ip ) ) {
                    Console.WriteLine( String.Format( "Player {0} connecting from {1} is not on whitelist.", Main.player[playerId].name, ip ) );
                    if( !allowUnwhiteLogin ) {
                        NetMessage.SendData( 2, playerId, -1, "Not on whitelist." );
                    }
                } else {
                    player[playerId].Whitelisted = true;
                }
            } catch( Exception e ) {
                Console.WriteLine( String.Format( "Exception during Whitelist processing: {0}", e ) );
            }
        }

        private static Packet HandleGreeting( byte[] data, int pid ) {
            //reset playerinfo for player when a new player logs in with this socket.
            player[pid] = new PlayerInfo();
            AccountManager.Logout( pid );
            return new Packet( data, data.Length );
        }

        /// <summary>
        /// Creates a dummy packet the same length as [data], that has no effect
        /// when recieved by the server. Used to prevent an incoming packet from
        /// being processed by the server.
        /// </summary>
        /// <param name="data">The original packet data</param>
        /// <returns></returns>
        private static Packet CreateDummyPacket(byte[] data)
        {
            int msgLength = 1;
            var newPacket = new Packet();
            //info on message header
            //bytes 0-3 are payload length
            //byte 4 is message type
            //payload is bytes 5-end of packet
            //payload length must match expected length since XNA uses Streams to handle data
            //and the length is pre-computed.

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

        public static Packet HandleChatMsg(byte[] data)
        {
            var p = new packet_ChatMsg(data); //initialize packet class, populate fields from data
         
            //built-in commands (prefixed with %).
            if (p.Text[0] == 0x25) //match %, for built-in commands.
            {
                var n = p.Text.Split(' ');
                n[0] = n[0].ToLower();

                basicCommand(n,p);

                return CreateDummyPacket(data);
            }


            if (p.Text[0] == 0x2E) // match dot
            {
                //check if nocommands for anons is set, if playerid is not console id, and playerid is whitelisted
                if( AccountManager.WhitelistActive && anonPrivs.Has( Actions.NOCOMMANDS ) && p.PlayerId != 0xFC && !player[p.PlayerId].Whitelisted )
                {
                    SendChatMsg("Commands disabled until Whitelisted", p.PlayerId, Color.Salmon);
                    return CreateDummyPacket(data);
                }

                var commands = p.Text.Split(' ');
                commands[0] = commands[0].ToLower();

                
                foreach (ChatCommand c in chatCommands)
                {
                    if (commands[0] == c.Trigger)
                    {
                        if ((AccountManager.GetRights(p.PlayerId) & c.PermissionsRequired) == c.PermissionsRequired)
                        {
                            if (!c.Handler(commands, p))
                            {
                                if (c.UsageString != null)
                                    cmdUsage(String.Format("Usage: {0}", c.UsageString), p.PlayerId);
                            }
                        } else
                        {
                            SendAccessDeniedMsg(p.PlayerId, commands[0]);
                        }

                        //no matching command found
                        return CreateDummyPacket(data);
                    }
                }
                cmdUnknown(commands, p);
                return CreateDummyPacket(data);
            }
            return new Packet(p.Packet, p.Packet.Length);
        }

        /// <summary>
        /// Handles basic, built-in terraria server commands.
        /// </summary>
        /// <param name="n">The line entered.</param>
        /// <param name="packetChatMsg">The chat message packet</param>
        private static void basicCommand(string[] n, packet_ChatMsg packetChatMsg)
        {

            if (n[0] == "%dawn" || n[0] == "%noon" || n[0] == "%dusk" || n[0] == "%midnight" || n[0] == "%settle")
            {
                var b = GetParamsAsString(n, " ", 0, -1);
                var cmd = b.Substring(1, b.Length - 1);

                Utils.sendLineToConsole(cmd);
                return;
            }
            SendChatMsg("Available Commands: %dawn, %noon, %dusk, %midnight, %settle", packetChatMsg.PlayerId, Color.IndianRed);
        }

        private static bool cmdCreateItem(string[] commands, packet_ChatMsg packetChatMsg)
        {

            if (commands.Length < 3)
                return false;

            int item;
            int stack = 1;

            var z = commands[commands.Length - 1].Split('x');
            if (z.Length == 2)
            {
                if (int.TryParse(z[1], out stack) == false)
                    return false;
            }

            if (int.TryParse(z[0], out item) == false)
                return false;
            
            var name = GetParamsAsString(commands, " ", 1);
            var pid = getPlayerIdFromName(name);
            if (pid != -1)
                Item.NewItem((int) Main.player[pid].position.X, (int) Main.player[pid].position.Y, 16, 16, item, stack);

            return true;
        }

        private static bool cmdDisableSpawns(string[] commands, packet_ChatMsg packetChatMsg)
        {
            if (commands.Length < 2)
                return false;

            switch(commands[1])
            {
                case ("on"):
                    disableSpawns(true);
                    SendChatMsg("Spawns Disabled.", packetChatMsg.PlayerId, Color.Green);
                    break;
                case ("off"):
                    disableSpawns(false);
                    SendChatMsg("Spawns Enabled.", packetChatMsg.PlayerId, Color.Green);
                    break;
                default:
                    return false;
            }
            return true;

        }


        /// <summary>
        /// Enable/Disable spawns (public)
        /// not invoked from WSARecv.
        /// </summary>
        /// <param name="mode">true/false whether to disable or enable spawns.</param>
        public static void disableSpawns(bool mode)
        {
            if (mode)
            {
                setSpawnValues(0,0);
            } else
            {
                setSpawnValues(Data.DEFAULT_SPAWN_RATE,Data.DEFAULT_MAX_SPAWNS);
            }
            return;
        }

        /// <summary>
        /// Sets the default values for Terraria NPC/Enemy spawns.
        /// </summary>
        /// <param name="spawnrate">The default spawnrate.</param>
        /// <param name="maxspawns">The default maximum spawns at an instant (per player).</param>
        public static void setSpawnValues(int spawnrate, int maxspawns)
        {
            defaultSpawnRate.SetValue(null, spawnrate);
            defaultMaxSpawns.SetValue(null, maxspawns);
        }

        private static bool cmdCoords(string[] c, packet_ChatMsg packetChatMsg)
        {
            string o = String.Format("Coords: [X:{0} Y:{1}]", (int)Main.player[packetChatMsg.PlayerId].position.X / 16,
                                     (int)Main.player[packetChatMsg.PlayerId].position.Y / 16);
            SendChatMsg(o,packetChatMsg.PlayerId,Color.Green);
            return true;
        }

        private static bool cmdHelp( string[] commands, packet_ChatMsg packetChatMsg )
        {
            List<string> commandNames = new List<string>();
            foreach (ChatCommand c in chatCommands)
            {
                if ((AccountManager.GetRights(packetChatMsg.PlayerId) & c.PermissionsRequired) == c.PermissionsRequired)
                    commandNames.Add(c.Trigger);
            }
            string msg = Utils.concat(commandNames);

            SendChatMsgSafe(String.Format("Available Commands: {0}", msg), packetChatMsg.PlayerId, Color.GreenYellow);

            return true;
        }

        private static void cmdUsage(string usage, int pid)
        {
            SendChatMsg(usage, pid, Color.GreenYellow);
        }

        private static bool cmdSpawnNPC(string[] commands, packet_ChatMsg packetChatMsg)
        {
            if (commands.Length < 3)
                return false;
                
            int npcId;
            int count = 1;

            var z = commands[commands.Length - 1].Split('x');
            if (z.Length == 2)
            {
                if (int.TryParse(z[1], out count) == false)
                    return false;
            }

            if (int.TryParse(z[0], out npcId) == false)
                return false;

            //don't spawn retard amounts
            if (count > MAX_SPAWNS) count = MAX_SPAWNS;

            string targetName = GetParamsAsString(commands, " ", 1);

            if (targetName == null)
                return false;

            var targetId = getPlayerIdFromName(targetName);
            if (targetId != -1)
            {
                if (count == 1)
                {
                    NPC.NewNPC((int) Main.player[targetId].position.X, (int) Main.player[targetId].position.Y, npcId);
                } else
                {
                    for (int i=0;i<count;i++)
                        NPC.NewNPC((int)Main.player[targetId].position.X, (int)Main.player[targetId].position.Y, npcId);
                }
                return true;
            }
            return true;
        }

        private static bool cmdLandMark(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var tag = GetParamsAsString(commands);
            //generate list of landmarks
            if (tag == null)
            {
                var locations = new List<string>();
                foreach (var sign in Main.sign)
                {
                    if (sign == null)
                        continue;

                    var lLoc = sign.text.IndexOf("<");
                    if (lLoc == -1) continue;
                    int rLoc = sign.text.IndexOf(">");
                    if (rLoc == -1) continue;
                    if ((rLoc - lLoc) -1 <= MAX_LANDMARK_LENGTH)
                        locations.Add(sign.text.Substring(lLoc + 1, (rLoc-lLoc)-1).ToLower());
                }
                
                if (locations.Count > 0)
                {
                    //remove duplicates from list (since the code will only ever teleport to the first found instance)
                    locations = locations.Distinct().ToList();
                    string r = Utils.concat(locations);

                    SendChatMsgSafe("Available landmarks: "+ r,packetChatMsg.PlayerId, Color.GreenYellow);
                    return true;
                }

                //fallthru, no landmarks set (nLocs = 0)
                SendChatMsg("No landmarks set.", packetChatMsg.PlayerId, Color.GreenYellow);
                return true;
            } //


            //this will return true on the first instance of the requested
            //tag on a sign.
            foreach (var n in Main.sign)
            {
                if (n == null)
                    continue;

                int found = n.text.ToLower().IndexOf("<" + tag + ">");
                if (found == -1) continue;
                float x = n.x;
                float y = n.y;
                   
                teleportPlayer((int)x,(int)y,packetChatMsg.PlayerId);

                return true;
            }
            SendChatMsg("Landmark not found.", packetChatMsg.PlayerId, Color.GreenYellow);
            return true;
        }

        private static void SendAccessDeniedMsg( int pid, string cmd ) {
            SendChatMsg( "Access denied. Please login.", pid, Color.GreenYellow );
            Console.WriteLine( String.Format( "Player {0} attempted to use command {1}", pid, cmd ) );
        }

        public static bool cmdSpawnMeteor( string[] commands, packet_ChatMsg packetChatMsg )
        {
            WorldEvents.SpawnMeteorCB();
            return true;
        }

        private static bool cmdTeleportTo(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var name = GetParamsAsString(commands);
            if (name == null)
                return false;

            var id = getPlayerIdFromName(name);
            if (id != -1)
            {
                teleportPlayer((int)Main.player[id].position.X/16, (int)Main.player[id].position.Y/16,packetChatMsg.PlayerId);
                return true;
            }
            SendChatMsg(String.Format("Player '{0}' not found", name), packetChatMsg.PlayerId, Color.Red);
            return true;

        }

        private static bool cmdTeleport(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var finalCoords = new Vector2(0f, 0f);
            var invalid = false;

            //check if the last word in the message is an x:y coord pair
            var coords = commands[commands.Length - 1];
            var t = coords.Split(':');
            if (t.Length != 2)
            {
                invalid = true;
            }
            else
            {
                if (float.TryParse(t[0], out finalCoords.X) == false)
                    invalid = true;
                if (float.TryParse(t[1], out finalCoords.Y) == false)
                    invalid = true;
            }
            //
           
            //get the name, without the coords because well, they're not part of the name.
            var name = GetParamsAsString(commands," ", 1);

            if (name == null && invalid) 
                return false;

            //if our name was null, but the vector was valid, our playerid is self:
            int targetId = name == null ? packetChatMsg.PlayerId : getPlayerIdFromName(name);
            if (targetId == -1)
            {
                SendChatMsg(String.Format("Player '{0}' not found", name), packetChatMsg.PlayerId, Color.Red);
                return true;
            }

            var x = (int)finalCoords.X;
            var y = (int)finalCoords.Y;

            teleportPlayer(x, y, targetId);
            return true;

        }

        /// <summary>
        /// Teleports a player to X,Y coords
        /// </summary>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        /// <param name="targetId">The target Player ID</param>
        /// <param name="old">if set to <c>true</c> run [old] code.</param>
        private static void teleportPlayer(int x, int y, int targetId, bool old = true)
        {

            if (old)
            {
                while (x % 2 != 0) x++;
                while (y % 2 != 0) y++;

                if (x > Main.maxTilesX - 2 || y > Main.maxTilesY - 2)
                {
                    SendChatMsg("Landmark out of range, or coords off-map.", targetId, Color.Purple);
                    return;
                }
                SendChatMsg("Preparing teleport!", targetId, Color.Bisque);

                int sectionX = Netplay.GetSectionX(x);
                int sectionY = Netplay.GetSectionY(y);

                for (int m = sectionX - 1; m < sectionX + 2; m++)
                {
                    for (int n = sectionY - 1; n < sectionY + 1; n++)
                    {
                        NetMessage.SendSection(targetId, m, n);
                    }
                }

                //additional section update details, 11 requests client determines tile frames and walls.
                NetMessage.SendData(11, targetId, -1, "", sectionX - 2, (float)(sectionY - 1), (float)(sectionX + 2), (float)(sectionY + 1), 0);

                teleQueue.AddLast(new TP() {targetId = targetId, x = x, y = y + 2});

               
                //if timer hasn't been created, create and initialize
                if (tTimer == null)
                {
                    tTimer = new System.Timers.Timer(3000);
                    tTimer.Elapsed += new ElapsedEventHandler(TeleportTrigger);
                    tTimer.Enabled = true;
                } else
                {   //else just enable it
                    tTimer.Enabled = true;
                }


            }
            else
            {
                player[targetId].Teleported = true;
                //kill player, should respawn at the forged spawnpoint
                killWithStar(Main.player[targetId].position.X, Main.player[targetId].position.Y, targetId);
            }
        }

        /// <summary>
        /// Trigger called from timer, to teleport a player to their destination.
        /// Prereqs: LinkedList<teleQueue> with a list of clients that requested teleports
        /// to map locations, or landmarks.
        /// 
        /// Logic: checks teleQueue, if there are no queued teleports it will disable the timer and return.
        /// If queued items exist, it will get the details of the first client and process that client only.
        /// That entry is then removed from the queue, and the next tick will process the next in line.
        /// </summary>
        private static void TeleportTrigger(object source, ElapsedEventArgs e)
        {
            
            int x;
            int y;
            int targetId;
            if (teleQueue.Count > 0)
            {
                x = teleQueue.First.Value.x;
                y = teleQueue.First.Value.y;
                targetId = teleQueue.First.Value.targetId;
                teleQueue.RemoveFirst();
                if (teleQueue.Count == 0)
                    tTimer.Enabled = false;
            } else
            {
                tTimer.Enabled = false;
                return;
            }


            //change server spawn tile.
            var oldSpawnTileX = Main.spawnTileX;
            var oldSpawnTileY = Main.spawnTileY;
            Main.spawnTileX = x;
            Main.spawnTileY = y;

            //dummy world name
            var n = Main.worldName;
            
            int randomNumber = random.Next(0, 100000000);

            Main.worldName = "assassass-" + randomNumber;

            //0x07: update spawntilex, worldname
            NetMessage.SendData(0x07, targetId, -1, "", targetId);

            //reset forged data (Serverside)
            Main.worldName = n;
            Main.spawnTileX = oldSpawnTileX;
            Main.spawnTileY = oldSpawnTileY;

            Main.player[targetId].position.X = x;
            Main.player[targetId].position.Y = y;

            NetMessage.SendData(0x0C, targetId, -1, "", targetId); //client respawn
            NetMessage.SendData(0x07, targetId, -1, "", targetId); //restore original values to client

        }

        private static bool cmdLogin( string[] commands, packet_ChatMsg p )
        {
            var name = GetParamsAsString(commands);
            if (name != null && AccountManager.Login(p.PlayerId, name))
            {
                SendChatMsg("Login successful.", p.PlayerId, Color.ForestGreen);
                return true;
            }

            return false;
        }

        private static bool cmdLaunchStar(string[] commands, packet_ChatMsg packetChatMsg)
        {

            var name = GetParamsAsString(commands);
            if (name == null)
                return false;

            var id = getPlayerIdFromName(name);
            if (id == -1)
            {
                SendChatMsg(String.Format("Player '{0}' not found", name), packetChatMsg.PlayerId, Color.Red);
                return true;
            }

            float x = Main.player[id].position.X;
            float y = Main.player[id].position.Y - (14 * 16); //spawn star 14 tiles above player

            SendChatMsg("Sending death to " + name + ".", packetChatMsg.PlayerId, Color.Red);
            killWithStar(x, y, id);
           

            return true;

        }

        private static void killWithStar(float x, float y, int id)
        {
            //if the player is already hostile, don't do anything
            if (!Main.player[id].hostile)
            {
                Main.player[id].hostile = true;
                player[id].ForcedHostile = true;
            }

            Main.player[Main.myPlayer].hostile = true; //i'm not sure what this actually affects?
            //as far as i can tell, main.myplayer only owns projectile
            //12 [star] and everything else is either unowned, or self-owned
            Projectile.NewProjectile(x+8, y, 0f, 5f, 12, 500, 10f, Main.myPlayer);
            return;

        }

        private static bool cmdKickBan(string[] commands, packet_ChatMsg packetChatMsg)
        {
            //NOTE: much more efficient to use string.Substring(x); to get the name from the command
            var name = GetParamsAsString(commands);
            if (name == null)
                return false;

            var id = getPlayerIdFromName(name);
            if (id != -1)
            {
                banUser(id);
                kickUser(id);
                SendChatMsg("Player " + name + " kicked and banned.", packetChatMsg.PlayerId, Color.Green);
                return true;
            }
            SendChatMsg(String.Format("Player '{0}' not found", name), packetChatMsg.PlayerId, Color.Red);
            return true;
        }

        private static bool cmdBanUser(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var name = GetParamsAsString(commands);
            if (name == null)
                return false;

            var id = getPlayerIdFromName(name);
            if (id != -1) {
                    Netplay.AddBan(id);
                    SendChatMsg("Banned user: " + name, packetChatMsg.PlayerId, Color.Red);
                    return true;
            }
            SendChatMsg(String.Format("Player '{0}' not found", name), packetChatMsg.PlayerId, Color.Red);
            return true;
        }


        private static bool cmdItemBanToggle( string[] commands, packet_ChatMsg packetChatMsg )
        {
            itemRiskEnabled = !itemRiskEnabled;
            string state = itemRiskEnabled ? "enabled." : "disabled.";
            SendChatMsg( "Risky item warning " + state, packetChatMsg.PlayerId, Color.Green );
            return true;
        }

        /// <summary>
        /// Kick a named player from the server
        /// </summary>
        /// <param name="commands">The commands array</param>
        /// <param name="packetChatMsg">Data class for ChatMsg</param>
        private static bool cmdKick(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var name = GetParamsAsString(commands);
            if (name == null)
                return false;

            var targetid = getPlayerIdFromName(name);
            if (targetid != -1)
            {
                kickUser(targetid);
                SendChatMsg("Player " + name + " kicked.", packetChatMsg.PlayerId, Color.Green);
                return true;
            }
            SendChatMsg(String.Format("Player '{0}' not found", name), packetChatMsg.PlayerId, Color.Red);
            return true;

        }

        private static bool cmdWhitelist( string[] commands, packet_ChatMsg packetChatMsg ) {

            if (commands.Length < 2)
                return false;
            switch( commands[1].ToLower( ) ) {
                case ("a"):
                case ("add"):
                    if (commands.Length < 3)
                        return false;
                    SendChatMsg( String.Format( "Adding {0} to whitelist.", commands[2] ), packetChatMsg.PlayerId, Color.GreenYellow );
                    AccountManager.CreateAccount( "(Unknown)", commands[2] );
                    AccountManager.SaveAccounts( );
                    break;
                case ("addplayer"):
                    //NEW CODE: by ass
                    if (commands.Length < 3)
                        return false;
                    var name = GetParamsAsString(commands, " ", 0, 1);
                    if (name == null)
                        return false;

                    var targetId = getPlayerIdFromName(name);
                    if (targetId == -1)
                    {
                        SendChatMsg("Player not found.", packetChatMsg.PlayerId, Color.Red);
                        return true;
                    }
                    
                    var t = (ServerSock[])serverSock.GetValue(null);
                    var endpoint = t[targetId].tcpClient.Client.RemoteEndPoint.ToString( );
                    var ip = Utils.ParseEndPointAddr( endpoint );
                    SendChatMsg(String.Format("Adding {0} [{1}] to whitelist.", name, ip), packetChatMsg.PlayerId, Color.GreenYellow);

                    AccountManager.CreateAccount( name, ip );
                    AccountManager.SaveAccounts( );
                    player[targetId].Whitelisted = true;
                    SendChatMsg("You are now whitelisted.", targetId, Color.Purple);
                    //
                    break;
                case ("d"):
                case ("del"):
                    if (commands.Length < 3)
                        return false;
                    SendChatMsg( String.Format( "Removing {0} from whitelist.", commands[2] ), packetChatMsg.PlayerId, Color.GreenYellow );
                    AccountManager.RemoveAccount( commands[2] ); 
                    break;
                case ("r"):
                case ("refresh"):
                    SendChatMsg( "Loading whitelist from file.", packetChatMsg.PlayerId, Color.GreenYellow );
                    AccountManager.Refresh( );
                    break;
                case ("on"):
                    AccountManager.WhitelistActive = true;
                    SendChatMsg( "Server whitelist is on.", packetChatMsg.PlayerId, Color.GreenYellow );
                    settings.EnableWhitelist = AccountManager.WhitelistActive;
                    settings.Save();
                    break;
                case ("off"):
                    AccountManager.WhitelistActive = false;
                    SendChatMsg( "Server whitelist is off.", packetChatMsg.PlayerId, Color.GreenYellow );
                    break;
                /*case ("allowlogin"):
                    allowUnwhiteLogin = !allowUnwhiteLogin;
                    string state = allowUnwhiteLogin ? "enabled." : "disabled.";
                    SendChatMsg( "Allow un-whitelisted users login: " + state, packetChatMsg.PlayerId, Color.Green );
                    settings.EnableAnonLogin = allowUnwhiteLogin;
                    settings.Save();
                    break;*/
                default:
                    return false;
                        
            }
            return true;
        }

        /// <summary>
        /// Helper function, returns all arguments to a command as a single, delimited string.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="negOffset">Negative offset for name. Use to omit array entries from end of commands</param>
        /// <param name="posOffset">Positive offset for name. Use to omit array entries from beginning of command.</param>
        /// <returns></returns>
        private static string GetParamsAsString(string[] commands, string delimiter = " ", int negOffset = 0, int posOffset = 0)
        {
            if (commands.Length == 1 && posOffset != -1)
                return null;

            string param = null;

            for (int i = (1 + posOffset); i < (commands.Length - negOffset); i++)
                param = param + delimiter + commands[i];

            //if theres no name (e.g. negative offset negates space for name), return null.
            return param != null ? param.Trim() : null;
        }

        /// <summary>
        /// Bans a player from the server
        /// </summary>
        /// <param name="id">Player ID</param>
        public static void banUser(int id)
        {
            //built-in terraria ban method
            Netplay.AddBan(id);
            return;
        }

        /// <summary>
        /// Kicks a user from the server
        /// </summary>
        /// <param name="id">Player ID</param>
        public static void kickUser(int id)
        {
            NetMessage.SendData(2, id, -1, "Kicked from server.");
            return;
        }

        /// <summary>
        /// Given a player Name, returns their Player ID
        /// </summary>
        /// <param name="name">The players Name</param>
        /// <returns>The players ID, or -1 if user is not found.</returns>
        private static int getPlayerIdFromName(string name)
        {
            for (int i = 0; i < Main.maxNetPlayers; i++)
            {
                if (Main.player[i].name.ToLower() != name.ToLower()) continue;
                if (Main.player[i].active)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Helper function to send chat messages
        /// </summary>
        /// <param name="msg">Message to send</param>
        /// <param name="playerId">Player index to send to, or -1 for broadcast</param>
        /// <param name="color">The color, as a Color type.</param>
        public static void SendChatMsg(string msg, int playerId, Color color)
        {
            if (playerId == 0xFC)
                Console.WriteLine(msg);
            else 
                NetMessage.SendData(25, playerId, -1, msg, 255, color.R, color.G, color.B);

            return;
        }

        /// <summary>
        /// Sends a chat message with content exceeding MAX_LINE_LENGTH split
        /// into multiple messages.
        /// </summary>
        /// <param name="msg">The message to be sent</param>
        /// <param name="playerId">The target Player ID</param>
        /// <param name="color">The color of the Text</param>
        private static void SendChatMsgSafe( string msg, int playerId, Color color ) {
            if( playerId == 0xFC ) {
                Console.WriteLine( msg );
            } else {
                SendDataSafe( 25, playerId, -1, msg, 255, color.R, color.G, color.B );
            }
        }

        /// <summary>
        /// Unknown Command Handler
        /// </summary>
        /// <param name="commands">TArray of space-delimited words</param>
        /// <param name="p">Data class for ChatMsg</param>
        private static void cmdUnknown(string[] commands, packet_ChatMsg p)
        {
            SendChatMsg("ERROR: Unknown command " + commands[0], p.PlayerId, Color.Red);
            return;
        }

        /// <summary>
        /// Broadcast Command Handler
        /// </summary>
        /// <param name="commands">The array of space-delimited words</param>
        /// <param name="packetChatMsg">The chat message packet</param>
        /// <returns></returns>
        private static bool cmdBroadcast( string[] commands, packet_ChatMsg packetChatMsg )
        {
            var msg = GetParamsAsString(commands);
            if (msg == null)
                return false;

            SendChatMsg(msg, -1, Color.Orange);
            return true;
        }

        private static void SendDataSafe( int msgId, int playerId, int j, string msg, int k, byte r, byte g, byte b ) {
            var msgList = Utils.GetLines( msg, MAX_LINE_LENGTH );
            foreach( var m in msgList ) {
                NetMessage.SendData( msgId, playerId, j, m, k, r, g, b );
            }
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

    //stub
    public class packet_BlockChange : packet_Base
    {
        internal Vector2 Position;
        internal int ActionType;

        internal packet_BlockChange(byte[] data)
            : base(data)
        {
            PlayerId = data[5];
            Position.X = BitConverter.ToInt32(data, 6);
            Position.Y = BitConverter.ToInt32(data, 10);
            ActionType = data[11];
            /* 0 = Destroy Tile
             * 1 = Place Tile
             * 2 = Kill Wall
             * 3 = Place Wall
             * 4 = Destroy Tile (drop no item)
             */
            //TODO: need to change this to include the byte for fail
            //so can effectively include blocking for actions that break
            //blocks, skipping actions that merely touch blocks.
        }
    }

    public class packet_UpdateProjectile : packet_Base
    {
        internal short Identity;
        internal int ProjectileType;
        internal int Owner;
        internal Vector2 Position;
        internal Vector2 Velocity;
        internal float KnockBack;
        internal short Damage;

        internal packet_UpdateProjectile(byte[] data)
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
