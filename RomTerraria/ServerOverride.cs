using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace RomTerraria
{
    public class ServerOverride : Terraria.Main
    {
        public int ServerWorldID { get; set; }
        public string ServerPassword { get; set; }

        Assembly terrariaAssembly;
        Type worldGen;
        MethodInfo serverLoadWorld;
        private static MethodInfo worldGenLoadWorld;
        private static FieldInfo worldGenLoadFailed;
        private static FieldInfo worldGenTempDayTime;
        private static FieldInfo worldGenTempTime;
        private static FieldInfo worldGenTempMoonPhase;
        private static FieldInfo worldGenTempBloodMoon;
        SpriteBatch spriteBatch;
        //RSN.IP ipService = new RSN.IP();
        //bool asked = false;

        //private ConcurrentBag<string> ips = new ConcurrentBag<string>();
        //private List<string> ips = new List<string>();

        protected override void LoadContent()
        {
            //this.Window.Title += " (using RomTerraria Launcher - http://www.romsteady.net)";
            terrariaAssembly = Assembly.GetAssembly(typeof(Terraria.Main));
            if (terrariaAssembly != null)
            {
                worldGen = terrariaAssembly.GetType("Terraria.WorldGen");

                foreach (var f in worldGen.GetMethods())
                {
                    if (f.Name == "serverLoadWorld")
                    {
                        serverLoadWorld = f;
                    }
                    if( f.Name == "loadWorld" ) {
                        worldGenLoadWorld = f;
                    }
                }

                foreach( var f in worldGen.GetFields( ) ) {
                    if( f.Name == "loadFailed" ) {
                        worldGenLoadFailed = f;
                    }
                }

                foreach( var f in worldGen.GetFields( BindingFlags.Static | BindingFlags.NonPublic ) ) {
                    if( f.Name == "tempDayTime" ) {
                        worldGenTempDayTime = f;
                    }
                    if( f.Name == "tempTime" ) {
                        worldGenTempTime = f;
                    }
                    if( f.Name == "tempMoonPhase" ) {
                        worldGenTempMoonPhase = f;
                    }
                    if( f.Name == "tempBloodMoon" ) {
                        worldGenTempBloodMoon = f;
                    }
                }
            }
            /*base.LoadContent();
            Terraria.Main.showSplash = false;
            Terraria.Main.menuMultiplayer = true;
            Terraria.Main.worldPathName = Terraria.Main.loadWorldPath[ServerWorldID];
            Terraria.Netplay.password = ServerPassword;
            //Terraria.Netplay.serverPort = 7778;
            //serverLoadWorld.Invoke(null, null);

            ServerLoadWorld( );

            Terraria.Main.menuMode = 10;*/

            //foreach (var ip in Dns.GetHostAddresses(""))
            //{
            //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        ips.Add(String.Format("{0}:{1}", ip, Terraria.Netplay.serverPort));
            //    }
            //}

            //ipService.GetIPCompleted += new RSN.GetIPCompletedEventHandler(ipService_GetIPCompleted);
            //ipService.GetIPAsync();

            //spriteBatch = new SpriteBatch(this.GraphicsDevice);
        }



        public static void ServerLoadWorld( ) {
            ThreadPool.QueueUserWorkItem( new WaitCallback( ServerLoadWorldCallBack ), 1 );
        }

        public static void ServerLoadWorldCallBack( object threadContext ) {
            worldGenLoadWorld.Invoke( null, null );
            if( !(bool)worldGenLoadFailed.GetValue( null ) ) {
                Terraria.Main.PlaySound( 10, -1, -1, 1 );
                //Terraria.Netplay.StartServer( );
                NetplayOverride.StartServer( );
                Terraria.Main.dayTime = (bool)worldGenTempDayTime.GetValue( null );
                Terraria.Main.time = (double)worldGenTempTime.GetValue( null );
                Terraria.Main.moonPhase = (int)worldGenTempMoonPhase.GetValue( null );
                Terraria.Main.bloodMoon = (bool)worldGenTempBloodMoon.GetValue( null );
            }
            //MessageBox.Show( "World Loaded." );
        }

        //void ipService_GetIPCompleted(object sender, RSN.GetIPCompletedEventArgs e)
        //{
        //    if (e.Error == null)
        //    {
        //        ips.Add(String.Format("External: {0}:7777", e.Result));
        //    }
        //    else
        //    {
        //        ips.Add(String.Format("Error getting your IP Address: {0}", e.Error.Message));
        //    }
        //}

        private void DrawString(string s, int x, int y)
        {
            spriteBatch.DrawString(fontItemStack, s, new Vector2(x+1, y + 1), Color.Black);
            spriteBatch.DrawString(fontItemStack, s, new Vector2(x, y), Color.White);
        }

        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
            
            //int y = 16;
            //spriteBatch.Begin();

            //DrawString("IP Addresses:", 0, 0);
            //foreach (string s in ips)
            //{
            //    DrawString(s, 0, y);
            //    y += 16;
            //}

            //spriteBatch.End();
        }

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
