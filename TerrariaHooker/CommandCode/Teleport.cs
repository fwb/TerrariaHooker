using System.Collections.Generic;
using System.Timers;
using Microsoft.Xna.Framework;
using Terraria;

namespace TerrariaHooker.CommandCode
{
    /// <summary>
    /// Class to hold details for queued teleports
    /// </summary>
    public class TP
    {
        public int targetId;
        public int x;
        public int y;
    }

    /// <summary>
    /// Code for teleporting a player from one location to another.
    /// </summary>
    class Teleport
    {
        //teleport timer related
        private static LinkedList<TP> teleQueue = new LinkedList<TP>();
        private static System.Timers.Timer tTimer;
        //

        /// <summary>
        /// Handles calling and queuing requests to teleport a player to X,Y coords.
        /// </summary>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        /// <param name="targetId">The target Player ID</param>
        public static void teleportPlayer(int x, int y, int targetId, bool old = true)
        {

            while (x % 2 != 0) x++;
            while (y % 2 != 0) y++;

            if (x > Main.maxTilesX - 2 || y > Main.maxTilesY - 2)
            {
                Commands.SendChatMsg("Landmark out of range, or coords off-map.", targetId, Color.Purple);
                return;
            }

            int sectionX = Netplay.GetSectionX(x);
            int sectionY = Netplay.GetSectionY(y + 1);

            /*
            //does player already have the tilesection available to it? if yes, just teleport without queue
            if (Netplay.serverSock[targetId].tileSection[sectionX, sectionY])
            {
                TeleportToLocation(targetId, x, y + 1);
                return;
            }*/

            Commands.SendChatMsg("Preparing teleport!", targetId, Color.Bisque);

            for (int m = sectionX - 1; m < sectionX + 2; m++)
            {
                for (int n = sectionY - 1; n < sectionY + 1; n++)
                {
                    NetMessage.SendSection(targetId, m, n);
                }
            }

            //additional section update details, 11 requests client determines tile frames and walls.
            NetMessage.SendData(11, targetId, -1, "", sectionX - 2, (float)(sectionY - 1), (float)(sectionX + 2), (float)(sectionY + 1), 0);

            teleQueue.AddLast(new TP {targetId = targetId, x = x, y = y + 1 });

            //if timer hasn't been created, create and initialize
            if (tTimer == null)
            {
                tTimer = new System.Timers.Timer(3000);
                tTimer.Elapsed += new ElapsedEventHandler(TeleportQueueProcess);
                tTimer.Enabled = true;
            }
            else
            {   //else just enable it
                tTimer.Enabled = true;
            }


        }

        /// <summary>
        /// Method called by teleport timer, to teleport a player to their destination.
        /// Prereqs: LinkedList<teleQueue> with a list of clients that requested teleports
        /// to map locations, or landmarks.
        /// 
        /// Logic: checks teleQueue, if there are no queued teleports it will disable the timer and return.
        /// If queued items exist, it will get the details of the first client and process that client only.
        /// That entry is then removed from the queue, and the next tick will process the next in line.
        /// </summary>
        private static void TeleportQueueProcess(object source, ElapsedEventArgs e)
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
            }
            else
            {
                tTimer.Enabled = false;
                return;
            }

            TeleportToLocation(targetId, x, y);

        }

        /// <summary>
        /// Actual code to teleport the player to their destination.
        /// </summary>
        /// <param name="targetId">The target id.</param>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        private static void TeleportToLocation(int targetId, int x, int y)
        {
            //change server spawn tile.
            var oldSpawnTileX = Main.spawnTileX;
            var oldSpawnTileY = Main.spawnTileY;

            var oldPlayerSpawnX = Main.player[targetId].SpawnX;
            var oldPlayerSpawnY = Main.player[targetId].SpawnY;

            //send(0x0C) sends server stored player SpawnX and SpawnY
            //so lets revert them to -1, so they will be ignored by the client
            Main.player[targetId].SpawnX = -1;
            Main.player[targetId].SpawnY = -1;

            Main.spawnTileX = x;
            Main.spawnTileY = y;

            //dummy world name
            var n = Main.worldName;
            Main.worldName = "TH-v001";

            //int randomNumber = random.Next(0, 100000000);
            //Main.worldName = "TH-"+randomNumber;


            //0x07: update spawntilex, worldname clientside
            NetMessage.SendData(0x07, targetId, -1, "", targetId);

            //client respawn
            NetMessage.SendData(0x0C, targetId, -1, "", targetId);

            //reset forged data (Serverside)
            Main.worldName = n;

            Main.spawnTileX = oldSpawnTileX;
            Main.spawnTileY = oldSpawnTileY;

            Main.player[targetId].SpawnX = oldPlayerSpawnX;
            Main.player[targetId].SpawnY = oldPlayerSpawnY;

            Main.player[targetId].position.X = x;
            Main.player[targetId].position.Y = y;
            //

            //restore original values to client
            NetMessage.SendData(0x07, targetId, -1, "", targetId);

        }
    }
}
