using System;
using System.IO;
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
        private static bool whitelistEnabled;

        private static int[] justHostiled = new int[10];
        private static int numberHostiled;
        private static Assembly terrariaAssembly;
        private static Type netplay;
        private static FieldInfo serverSock;
        private static TextWriter w = new StreamWriter(ServerConsole._out);


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
                    if( whitelistEnabled ) {
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

               //Console.WriteLine( "{0} :: {1}", prefix, sBuffer.ToString( ).ToUpper( ) );
            }
#endif
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

        private static void CheckWhitelist( byte[] data ) {
            try {
                var t = (ServerSock[])serverSock.GetValue( null );
                var playerId = data[5];
                var endpoint = t[playerId].tcpClient.Client.RemoteEndPoint.ToString( );
                var ip = Utils.ParseEndPointAddr( endpoint );
                if( !Whitelist.IsAllowed( ip ) ) {
                    Console.WriteLine( String.Format( "Player {0} connecting from {1} is not on whitelist.", Main.player[playerId].name, ip ) );
                    NetMessage.SendData( 2, playerId, -1, "Not on whitelist.", 0, 0f, 0f, 0f );
                    //t[playerId].kill = true;
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
                    case (".login"):
                        cmdLogin(commands, p);
                        break;
                    case (".meteor"):
                        cmdSpawnMeteor( commands, p );
                        break;
                    case (".broadcast"):
                        cmdBroadcast(commands, p);
                        break;
                    case (".kick"):
                        cmdKick( commands, p );
                        break;
                    case (".itemban"):
                        cmdItemBanToggle( commands, p);
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
                    case (".teleport"):
                        cmdTeleport(commands, p);
                        break;
                    case (".teleportto"):
                        cmdTeleportTo(commands, p);
                        break;
                    case (".wl"):
                        cmdWhitelist( commands, p );
                        break;
                    case (".landmark"):
                        cmdLandMark(commands, p);
                        break;
                    default:
                        cmdUnknown(commands, p);
                        break;
                }
                return CreateDummyPacket(data);
            }

            return new Packet(p.Packet, p.Packet.Length);
        }

        private static void cmdLandMark(string[] commands, packet_ChatMsg packetChatMsg)
        {
            var tag = GetParamsAsString(commands);
            if (tag == null)
            {
                string[] locations = new string[20];
                int nLocs = 0;
                foreach (var sign in Main.sign)
                {
                    if (sign == null)
                        continue;

                    int rLoc;
                    int lLoc = sign.text.IndexOf("<");
                    if (lLoc != -1)
                    {
                        rLoc = sign.text.IndexOf(">");
                        if (rLoc != -1)
                        {
                            locations[nLocs] = sign.text.Substring(lLoc + 1, (rLoc-lLoc) -1);
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
                    return;
                }

                //fallthru, no landmarks set (nLocs = 0)
                SendChatMsg("No landmarks set.", packetChatMsg.PlayerId, Color.GreenYellow);
                return;
            }
            foreach (var n in Main.sign)
            {
                int found = n.text.IndexOf("<" + tag + ">");
                if (found !=  -1)
                {
                    //get sign coords, teleport user using 0x0D
                    float x = n.x * 16;
                    float y = n.y * 16 - 20;
                   
                    Main.player[packetChatMsg.PlayerId].position.X = x;
                    Main.player[packetChatMsg.PlayerId].position.Y = y;
                    NetMessage.SendData(0x0D, -1, -1, "", packetChatMsg.PlayerId, 0f, 0f, 0f);
                    return;
                }
                SendChatMsg("Landmark not found.", packetChatMsg.PlayerId, Color.GreenYellow);
                return;
            }
        }

        private static void SendAccessDeniedMsg( int pid, string cmd ) {
            SendChatMsg( "Access denied. Please login.", pid, Color.GreenYellow );
            Console.WriteLine( String.Format( "Player {0} attempted to use command {1}", pid, cmd ) );
        }

        public static void cmdSpawnMeteor( string[] commands, packet_ChatMsg packetChatMsg )
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
            WorldEvents.SpawnMeteorCB();
            return;
        }

        private static void cmdTeleportTo(string[] commands, packet_ChatMsg packetChatMsg)
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
            var name = GetParamsAsString(commands);
            if (name == null)
            {
                SendChatMsg("USAGE: .teleportto <player>", packetChatMsg.PlayerId, Color.GreenYellow);
                return;
            }

            var id = getPlayerIdFromName(name);
            if (id != -1)
            {
                Main.player[packetChatMsg.PlayerId].position.X = Main.player[id].position.X;
                Main.player[packetChatMsg.PlayerId].position.Y = Main.player[id].position.Y;
                NetMessage.SendData(0x0D, -1, -1, "", packetChatMsg.PlayerId, 0f, 0f, 0f);
            }

        }

        private static void cmdTeleport(string[] commands, packet_ChatMsg packetChatMsg)
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
            Vector2 finalCoords = new Vector2(0f, 0f);
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
            //i dont like using a lambda here.
            //var name = GetParamsAsString(Array.FindAll(commands, val => val != coords).ToArray());
            var name = GetParamsAsString(commands," ", 1);

            if (name == null && invalid) //if the name is bad, but the vector is fine, we can continue
            {
                SendChatMsg("USAGE: .teleport <player> <xcoord:ycoord>; or .teleport <xcoord:ycoord>", packetChatMsg.PlayerId, Color.GreenYellow);
                return;
            }

            //if our name was null, but the vector was valid, our playerid is self:
            int targetId = name == null ? packetChatMsg.PlayerId : getPlayerIdFromName(name);

            Main.player[targetId].position.X = finalCoords.X;
            Main.player[targetId].position.Y = finalCoords.Y;

            NetMessage.SendData(0x0D, -1, -1, "", targetId, 0f, 0f, 0f);

        }

        private static void cmdLogin( string[] commands, packet_ChatMsg p )
        {
            var name = GetParamsAsString(commands);
            if (name != null && AccountManager.Login(p.PlayerId, name))
            {
                SendChatMsg("Login successful.", p.PlayerId, Color.ForestGreen);
                return;
            }

            SendChatMsg( "USAGE: .login <username>", p.PlayerId, Color.GreenYellow );
        }

        private static void cmdLaunchStar(string[] commands, packet_ChatMsg packetChatMsg)
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
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
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
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
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
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


        private static void cmdItemBanToggle( string[] commands, packet_ChatMsg packetChatMsg )
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
            itemBanEnabled = !itemBanEnabled;
            string state = itemBanEnabled ? "enabled." : "disabled.";
            SendChatMsg( "Item ban " + state, packetChatMsg.PlayerId, Color.Green );
        }

        /// <summary>
        /// Kick a named player from the server
        /// </summary>
        /// <param name="commands">The commands array</param>
        /// <param name="packetChatMsg">Data class for ChatMsg</param>
        private static void cmdKick(string[] commands, packet_ChatMsg packetChatMsg)
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
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

        private static void cmdWhitelist( string[] commands, packet_ChatMsg packetChatMsg ) {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
            if( commands.Length < 2 ) {
                cmdWhitelistUsage( packetChatMsg.PlayerId );
            } else {
                switch( commands[1].ToLower( ) ) {
                    case ("a"):
                    case ("add"):
                        if( commands.Length < 3 ) {
                            cmdWhitelistUsage( packetChatMsg.PlayerId );
                        }
                        else {
                            SendChatMsg( String.Format( "Adding {0} to whitelist.", commands[2] ), packetChatMsg.PlayerId, Color.GreenYellow );
                            Whitelist.AddEntry( commands[2] );
                        }

                        break;
                    case ("d"):
                    case ("del"):
                        if( commands.Length < 3 ) {
                            cmdWhitelistUsage( packetChatMsg.PlayerId );
                        }
                        else {
                            SendChatMsg( String.Format( "Removing {0} from whitelist.", commands[2] ), packetChatMsg.PlayerId, Color.GreenYellow );
                            Whitelist.RemoveEntry( commands[2] );
                        }
                        break;
                    case ("r"):
                    case ("refresh"):
                        SendChatMsg( "Loading whitelist from file.", packetChatMsg.PlayerId, Color.GreenYellow );
                        Whitelist.Refresh( );
                        break;
                    case ("on"):
                        whitelistEnabled = true;
                        SendChatMsg( "Server whitelist is on.", packetChatMsg.PlayerId, Color.GreenYellow );
                        break;
                    case ("off"):
                        whitelistEnabled = false;
                        SendChatMsg( "Server whitelist is off.", packetChatMsg.PlayerId, Color.GreenYellow );
                        break;
                    default:
                        cmdWhitelistUsage( packetChatMsg.PlayerId );
                        break;
                }
            }
        }

        private static void cmdWhitelistUsage( int pid ) {
            SendChatMsg( "USAGE: .wl (a)dd | (d)el | (r)efresh | on | off", pid,
                         Color.GreenYellow );
        }

        /// <summary>
        /// Helper function, returns all arguments to a command as a single, delimited string.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="negOffset">Negative offset for name. Used if your commands have .command [name here] [additional parameters]</param>
        /// <returns></returns>
        private static string GetParamsAsString(string[] commands, string delimiter = " ", int negOffset = 0)
        {
            if (commands.Length == 1)
                return null;

            string param = null;

            for (int i = 1; i < (commands.Length - negOffset); i++)
                param = param + delimiter + commands[i];

            //if theres no name (e.g. negative offset negates space for name), return null.
            return param != null ? param.Trim() : null;
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
        private static void cmdBroadcast( string[] commands, packet_ChatMsg packetChatMsg )
        {
            if( ( AccountManager.GetRights( packetChatMsg.PlayerId ) & Rights.ADMIN ) != Rights.ADMIN ) {
                SendAccessDeniedMsg( packetChatMsg.PlayerId, commands[0] );
                return;
            }
            var msg = GetParamsAsString(commands);
            if (msg == null)
            {
                SendChatMsg( "USAGE: .broadcast <message>", packetChatMsg.PlayerId, Color.GreenYellow );
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
