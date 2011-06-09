using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using System.Reflection;
using TerrariaHooker.AccountManagement;

namespace TerrariaHooker
{

    class Commands
    {
        private static Properties.Settings settings = new Properties.Settings();
        
        public static byte[] riskItems = new byte[] { 0xCF,   //lava bucket
                                                        0xA7 }; //dynamite

        private static bool itemRiskEnabled = true;
        //public static bool whitelistEnabled;
        public static bool allowUnwhiteLogin;
        public static bool[] whitelisted = new bool[255];

        //.star <player> related variables
        private static int[] justHostiled = new int[10];
        private static int _numberHostiled;

        private static Actions anonPrivs = Actions.NOBREAKBLOCK | Actions.NOUSEITEMS| Actions.NOCOMMANDS;
        
        private static Assembly terrariaAssembly;
        private static Type netplay;
        private static FieldInfo serverSock;
        private static TextWriter w = new StreamWriter(ServerConsole._out);


        internal static int MAX_SPAWNS = 50; //maximum number of spawns using .spawn (at a single time)


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

            Whitelist.IsActive = settings.EnableWhitelist;
            allowUnwhiteLogin = settings.EnableAnonLogin;
        }

        /// <summary>
        /// Process a data packet, determining data type and where defined,
        /// calling additional handlers to break apart data into a more workable
        /// format.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="direction">The direction of travel. 0 = RECV, 1 = SEND</param>
        /// <returns>A packet structure, complete with Data and Data Length</returns>
        public static Packet ProcessData(byte[] data, int direction)
        {
            byte type;
            try { type = data[4]; } 
            catch { return new Packet(data, data.Length); }

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
                    // Clear AccountManager refs in HandleGreeting( )
                    packet = HandleGreeting( data );
                    if( Whitelist.IsActive ) {
                        CheckWhitelist( data );
                    }
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
                    if (Whitelist.IsActive && !whitelisted[data[5]])
                        SendChatMsg("You are not whitelisted, some actions are restricted.", data[5], Color.Peru);
      
                    break;
                case 0x0D:
                    prefix = "PLAYER STATE CHANGE";
                    packet = HandlePlayerState(data);
                    break;
                case 0x10:
                    prefix = "PLAYER CURRENT/MAX HEALTH UPDATE";
                    break;
                case 0x11:
                    prefix = "PLAYER DESTROY BLOCK";
                    packet = HandleDestroyBlock(data);
                    break;
                case 0x13:
                    prefix = "PLAYER USE DOOR";
                    break;
                case 0x16:
                    prefix = "DETERMINE ITEM OWNER";    //UPDATE: apparently item[x].active defines whether the item is
                    //active within the game world (outside of inventories). I assume
                    //the shit in FindOwner is to determine who the new owner of a newly
                    //dropped item is (owner is the current dropper, so they don't have
                    //preference on the item by being closest). Also seems to be called
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
                    prefix = "PLAYER PROJECTILE UPDATE";
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

#if DEBUG   
            if( direction == 0 ) {
                var sBuffer = new StringBuilder( );
                foreach( byte t in data ) {
                    sBuffer.Append( Convert.ToInt32( t ).ToString( "x" ).PadLeft( 2, '0' ) + " " );
                }

               Console.WriteLine( "{0} :: {1}", prefix, sBuffer.ToString( ).ToUpper( ) );
            }
#endif
            return packet;

        }

        private static Packet HandleDestroyBlock(byte[] data)
        {
            var packet = new Packet(data, data.Length);
            #region NEW WHITELIST CODE
            if (Whitelist.IsActive && !whitelisted[data[5]])
            {
                if (anonPrivs.Has(Actions.NOBREAKBLOCK))
                {
                    //SendChatMsg("You cannot destroy blocks until whitelisted.", data[5], Color.Purple);
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
            //on them using .star.);
            var p = new packet_PlayerDied(data);

            if (Main.player[p.PlayerId].hostile)
            {
                for (var i = 0; i < _numberHostiled; i++)
                {
                    if (justHostiled[i] == data[5])
                    {
                        justHostiled[i] = 0x00;
                        _numberHostiled--;
                        Main.player[p.PlayerId].hostile = false;
                    }
                }
            }
            return new Packet(data, data.Length);
        }

        private static Packet HandlePlayerState(byte[] data)
        {
            var packet = new Packet(data, data.Length);
            if (!itemRiskEnabled) return packet;

            var p = new packet_PlayerState(data);
            
                //much, much more aggressive. no longer checks if the user is using an item since apparently that doesn't
                //much matter past the server doing some weird shit like telling other users they're swinging an axe.
                #region NEW WHITELIST CODE
                if (Whitelist.IsActive && !whitelisted[p.PlayerId])
                {
                    if (anonPrivs.Has(Actions.NOUSEITEMS))
                    {
                        //SendChatMsg("You cannot use items until whitelisted.", p.PlayerId, Color.Purple);
                        return CreateDummyPacket(data);
                    }
                }
                #endregion
               if (p.UsingItem)
               {
                foreach (byte i in riskItems)
                    {
                        var id = Main.player[p.PlayerId].inventory[p.SelectedItemId].type;
                        if (id == i)
                        {
                            //packet = CreateDummyPacket(data);
                            var itemName = Main.player[p.PlayerId].inventory[p.SelectedItemId].name;
                            //MakeItHarder.serverConsole.AddChatLine("Player: " + p.Name + " tried to use " + itemName);
                            Console.WriteLine("RISK: Player '{0}' used {1}", Main.player[p.PlayerId].name, itemName);

                        }
                    }
               }
        
            return packet;

        }

        private static void CheckWhitelist( byte[] data ) {
            try {
                var t = (ServerSock[])serverSock.GetValue( null );
                var playerId = data[5];
                var endpoint = t[playerId].tcpClient.Client.RemoteEndPoint.ToString( );
                var ip = Utils.ParseEndPointAddr( endpoint );
                if (!Whitelist.IsAllowed(ip))
                {
                    Console.WriteLine(String.Format("Player {0} connecting from {1} is not on whitelist.", Main.player[playerId].name, ip));
                    whitelisted[playerId] = false;
                    if (!allowUnwhiteLogin)
                        NetMessage.SendData( 2, playerId, -1, "Not on whitelist.", 0, 0f, 0f, 0f );
                    //t[playerId].kill = true;
                }
                else
                {
                    whitelisted[playerId] = true;
                }
            } catch( Exception e ) {
                Console.WriteLine( String.Format( "Exception during Whitelist processing: {0}", e.ToString( ) ) );
            }
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

        public static Packet HandleChatMsg(byte[] data)
        {
            var p = new packet_ChatMsg(data); //initialize packet class, populate fields from data
            //...
            //act on packet fields, generate new data etc.

            if (p.Text[0] == 0x2E) // match dot
            {
                //check if nocommands for anons is set, if playerid is not console id, and playerid is whitelisted
                if (anonPrivs.Has(Actions.NOCOMMANDS) && p.PlayerId != 0xFC && !whitelisted[p.PlayerId])
                {
                    SendChatMsg("Commands disabled until Whitelisted", p.PlayerId, Color.Salmon);
                    return CreateDummyPacket(data);
                }

                //split is probably no slower than substring, if the size of the strings
                //we are pulling from user-text is variable.
                var commands = p.Text.Split(' ');
                commands[0] = commands[0].ToLower();

                switch (commands[0])
                {
                    case( ".help" ):
                        cmdHelp( commands, p );
                        break;
                    case (".login"):
                        if (!cmdLogin(commands, p)) cmdUsage("USAGE: .login <username>",p.PlayerId);
                        break;
                    case (".meteor"):
                        cmdSpawnMeteor( commands, p ); //usage is .meteor, can't return false because it doesn't care about extra args
                        break;
                    case (".broadcast"):
                        if (!cmdBroadcast(commands, p)) cmdUsage("USAGE: .broadcast <message>",p.PlayerId);
                        break;
                    case (".kick"):
                        if (!cmdKick(commands, p)) cmdUsage("USAGE: .kick <player>",p.PlayerId);
                        break;
                    case (".itemrisk"):
                        cmdItemBanToggle( commands, p); //usage is .itemban, cares not for args
                        break;
                    case (".ban"):
                        if (!cmdBanUser(commands, p)) cmdUsage("USAGE: .ban <player>",p.PlayerId);
                        break;
                    case (".kickban"):
                        if (!cmdKickBan(commands, p)) cmdUsage("USAGE: .kickban <player>",p.PlayerId);
                        break;
                    case (".star"):
                        if (!cmdLaunchStar(commands, p)) cmdUsage("USAGE: .star <player>",p.PlayerId);
                        break;
                    case (".teleport"):
                        if (!cmdTeleport(commands, p)) cmdUsage("USAGE: .teleport <player> <xcoord:ycoord>; or .teleport <xcoord:ycoord>",p.PlayerId);
                        break;
                    case (".teleportto"):
                        if (!cmdTeleportTo(commands, p)) cmdUsage("USAGE: .teleportto <player>",p.PlayerId);
                        break;
                    case (".wl"):
                        if (!cmdWhitelist(commands, p)) cmdUsage("USAGE: .wl (a)dd | (d)el | (r)efresh | on | off", p.PlayerId);
                        break;
                    case (".landmark"):
                        if(!cmdLandMark(commands, p)) cmdUsage("USAGE: .landmark <name>", p.PlayerId);
                        break;
                    case (".spawn"):
                        if(!cmdSpawnNPC(commands, p)) cmdUsage("USAGE: .spawn <npcid> <player> [count]",p.PlayerId);
                        break;
                    default:
                        cmdUnknown(commands, p);
                        break;
                }
                return CreateDummyPacket(data);
            }

            return new Packet(p.Packet, p.Packet.Length);
        }

        private static void cmdHelp( string[] commands, packet_ChatMsg packetChatMsg ) {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
            SendChatMsg( String.Format( "Available Commands: .wl, .broadcast, .ban, .kick, .kickban, .itemban," ),
                         packetChatMsg.PlayerId, Color.GreenYellow );
            SendChatMsg( String.Format( ".meteor, .star, .spawn, .landmark, .teleport, .teleportto " ), packetChatMsg.PlayerId, Color.GreenYellow );
        }

        private static void cmdUsage(string usage, int pid)
        {
            SendChatMsg(usage, pid, Color.GreenYellow);
        }

        private static bool cmdSpawnNPC(string[] commands, packet_ChatMsg packetChatMsg)
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
            if (commands.Length < 3)
                return false;
                
            int npcId;
            int count = 0;
            bool multi = false;

            //first argument must be an integer
            if (int.TryParse(commands[1], out npcId) == false)
                return false;

            //final argument can be an integer, but is not required
            if (int.TryParse(commands[commands.Length - 1], out count))
                multi = true;

            //don't spawn retard amounts
            if (count > MAX_SPAWNS) count = MAX_SPAWNS;

            string targetName = GetParamsAsString(commands, " ", multi ? 1 : 0, 1);

            if (targetName == null)
                return false;

            var targetId = getPlayerIdFromName(targetName);
            if (targetId != -1)
            {
                if (!multi)
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
            //for now, landmark will be usable by all players. seems non-destructive enough.
            /*if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }*/
            var tag = GetParamsAsString(commands);
            //generate list of landmarks
            if (tag == null)
            {
                var locations = new string[20];
                int nLocs = 0;
                foreach (var sign in Main.sign)
                {
                    if (sign == null)
                        continue;

                    var lLoc = sign.text.IndexOf("<");
                    if (lLoc != -1)
                    {
                        int rLoc = sign.text.IndexOf(">");
                        if (rLoc != -1)
                        {
                            locations[nLocs] = sign.text.Substring(lLoc + 1, (rLoc-lLoc) -1).ToLower();
                            nLocs++;
                        }
                    }
                }
                string o = null;
                if (nLocs > 0)
                {
                    foreach (string l in locations)
                    {
                        if (l == null)
                            break;
                        o += l + ", ";
                    }

                    SendChatMsg("Available landmarks: "+ o.Substring(0,o.Length-2),packetChatMsg.PlayerId, Color.GreenYellow);
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
                if (found !=  -1)
                {
                    //get sign coords, teleport user using 0x0D
                    float x = n.x * 16;
                    float y = n.y * 16 - 20;
                   
                    Main.player[packetChatMsg.PlayerId].position.X = x;
                    Main.player[packetChatMsg.PlayerId].position.Y = y;
                    NetMessage.SendData(0x0D, -1, -1, "", packetChatMsg.PlayerId, 0f, 0f, 0f);
                    return true;
                }
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
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
            WorldEvents.SpawnMeteorCB();
            return true;
        }

        private static bool cmdTeleportTo(string[] commands, packet_ChatMsg packetChatMsg)
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
            var name = GetParamsAsString(commands);
            if (name == null)
                return false;

            var id = getPlayerIdFromName(name);
            if (id != -1)
            {
                Main.player[packetChatMsg.PlayerId].position.X = Main.player[id].position.X;
                Main.player[packetChatMsg.PlayerId].position.Y = Main.player[id].position.Y;
                NetMessage.SendData(0x0D, -1, -1, "", packetChatMsg.PlayerId, 0f, 0f, 0f);
                return true;
            }
            SendChatMsg(String.Format("Player '{0}' not found", name), packetChatMsg.PlayerId, Color.Red);
            return true;

        }

        private static bool cmdTeleport(string[] commands, packet_ChatMsg packetChatMsg)
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
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

            Main.player[targetId].position.X = finalCoords.X;
            Main.player[targetId].position.Y = finalCoords.Y;

            NetMessage.SendData(0x0D, -1, -1, "", targetId);
            return true;

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
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
            var name = GetParamsAsString(commands);
            if (name == null)
                return false;

            var id = getPlayerIdFromName(name);
            if (id == -1)
            {
                SendChatMsg(String.Format("Player '{0}' not found", name), packetChatMsg.PlayerId, Color.Red);
                return true;
            }

            //if the player is already hostile, don't do anything
            if (!Main.player[id].hostile)
            {
                Main.player[id].hostile = true;
                justHostiled[_numberHostiled] = id;
                _numberHostiled++;
            }

            Main.player[Main.myPlayer].hostile = true; //i'm not sure what this actually affects?
                                                       //as far as i can tell, main.myplayer only owns projectile
                                                       //12 [star] and everything else is either unowned, or self-owned
            Projectile.NewProjectile(Main.player[id].position.X, Main.player[id].position.Y-300, 0f, 5f, 12, 1000, 10f, Main.myPlayer);
            SendChatMsg("Sending death to " + name + ".", packetChatMsg.PlayerId, Color.Red);
            return true;

        }

        private static bool cmdKickBan(string[] commands, packet_ChatMsg packetChatMsg)
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
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
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
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
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
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
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
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
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
            if (commands.Length < 2)
                return false;
            switch( commands[1].ToLower( ) ) {
                case ("a"):
                case ("add"):
                    if (commands.Length < 3)
                        return false;
                    SendChatMsg( String.Format( "Adding {0} to whitelist.", commands[2] ), packetChatMsg.PlayerId, Color.GreenYellow );
                    Whitelist.AddEntry( commands[2] );
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

                    Whitelist.AddEntry(ip);
                    whitelisted[targetId] = true;
                    SendChatMsg("You are now whitelisted.", targetId, Color.Purple);
                    //
                    break;
                case ("d"):
                case ("del"):
                    if (commands.Length < 3)
                        return false;
                    SendChatMsg( String.Format( "Removing {0} from whitelist.", commands[2] ), packetChatMsg.PlayerId, Color.GreenYellow );
                    Whitelist.RemoveEntry( commands[2] );
                    break;
                case ("r"):
                case ("refresh"):
                    SendChatMsg( "Loading whitelist from file.", packetChatMsg.PlayerId, Color.GreenYellow );
                    Whitelist.Refresh( );
                    break;
                case ("on"):
                    Whitelist.IsActive = true;
                    SendChatMsg( "Server whitelist is on.", packetChatMsg.PlayerId, Color.GreenYellow );
                    settings.EnableWhitelist = Whitelist.IsActive;
                    settings.Save();
                    break;
                case ("off"):
                    Whitelist.IsActive = false;
                    SendChatMsg( "Server whitelist is off.", packetChatMsg.PlayerId, Color.GreenYellow );
                    break;
                case ("allowlogin"):
                    allowUnwhiteLogin = !allowUnwhiteLogin;
                    string state = allowUnwhiteLogin ? "enabled." : "disabled.";
                    SendChatMsg( "Allow un-whitelisted users login: " + state, packetChatMsg.PlayerId, Color.Green );
                    settings.EnableAnonLogin = allowUnwhiteLogin;
                    settings.Save();
                    break;
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
            if (commands.Length == 1)
                return null;

            string param = null;

            for (int i = (1 + posOffset); i < (commands.Length - negOffset); i++)
                param = param + delimiter + commands[i];

            //if theres no name (e.g. negative offset negates space for name), return null.
            return param != null ? param.Trim() : null;
        }

        private static void banUser(int id)
        {
            //built-in terraria ban method
            Netplay.AddBan(id);
            return;
        }

        private static void kickUser(int id)
        {
            NetMessage.SendData(2, id, -1, "Kicked from server.", 0, 0f, 0f, 0f);
            return;
        }

        private static int getPlayerIdFromName(string name)
        {
            for (int i = 0; i < Main.maxNetPlayers; i++)
            {
                if (Main.player[i].name.ToLower() == name.ToLower())
                {
                    if (Main.player[i].active)
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
        public static void SendChatMsg(string msg, int playerId, Color color)
        {
            if (playerId == 0xFC)
                Console.WriteLine(msg);
            else 
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
            SendChatMsg("ERROR: Unknown command " + commands[0], p.PlayerId, Color.Red);
            return;
        }

        /// <summary>
        /// Broadcast Command Handler
        /// </summary>
        /// <param name="commands">The array of space-delimited words</param>
        private static bool cmdBroadcast( string[] commands, packet_ChatMsg packetChatMsg )
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return true;
            }
            var msg = GetParamsAsString(commands);
            if (msg == null)
                return false;

            SendChatMsg(msg, -1, Color.Orange);
            return true;
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
