using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace RomTerraria
{
    public class MainOverride : Terraria.Main
    {
        public bool DrawMinimap { get; set; }
        public bool DrawClock { get; set; }
        public bool SpawnPony { get; set; }
        public bool AllowScreenshots { get; set; }
        public bool DisplayHardcore { get; set; }

        public MapOverlay MinimapOverlay { get; set; }
        private SpriteBatch spriteBatch;

        private int nextScreenshotNumber = -1;
        private bool screenshotKeyWasPressed = false;

        private const Keys screenshotKey = Keys.PrintScreen;

        private bool oldHideUI = false;
        private int ponyPosition = -873;

        internal int playerToDelete = -1;
        internal System.Reflection.MethodInfo deleteMethod = null;

        public MainOverride() : base() {
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            base.Window.Title += " (using RomTerraria Launcher - http://www.romsteady.net)";
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            AllowScreenshots = this.GraphicsDevice.GraphicsProfile == GraphicsProfile.HiDef;
        }

        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
            if (DrawMinimap && (MinimapOverlay != null))
                MinimapOverlay.Draw();

            //spriteBatch.Begin();
            //int y = 0;
            //foreach (var c in this.Components)
            //{
            //    spriteBatch.DrawString(Terraria.Main.fontItemStack, 
            //        String.Format("{0}: {1}", c.GetType().ToString(), 
            //                    (c as GameComponent) == null ? "" : (c as GameComponent).Enabled.ToString()), 
            //        new Microsoft.Xna.Framework.Vector2(0, y), Color.White);
            //    y += 16;
            //}
            //spriteBatch.DrawString(Terraria.Main.fontItemStack,
            //        this.GraphicsDevice.GraphicsProfile.ToString(),
            //        new Microsoft.Xna.Framework.Vector2(0, y), Color.White);
            //spriteBatch.End();

            if (AllowScreenshots) CheckScreenshot(); // Hate having logic in draw, but only way to do it.
            spriteBatch.Begin();

            if (DrawClock)
            {
                
                string s =  String.Format("{0:g}", DateTime.Now);
                var v = fontItemStack.MeasureString(s);
                spriteBatch.DrawString(Terraria.Main.fontItemStack, s, new Vector2(Terraria.Main.screenWidth - v.X, 0), Color.White);
                
            }

            if (SpawnPony && Terraria.Main.netMode == 0 && !hideUI && !gameMenu)
            {
                var p = Terraria.Main.player[Terraria.Main.myPlayer];

                int px = (int)(p.position.X / 16.0) - ponyPosition;

                string s = String.Format("Pony {0} blocks to west", px);
                if (px < 400)
                    s = "Sorry, pony ran away.";
                var v = fontItemStack.MeasureString(s);
                spriteBatch.DrawString(Terraria.Main.fontItemStack, s, new Vector2(Terraria.Main.screenWidth / 2 - v.X / 2, Terraria.Main.screenHeight - v.Y), Color.White);
                
            }

            if( DisplayHardcore ) {
                spriteBatch.DrawString( Terraria.Main.fontItemStack, "Hardcore Mode", 
                                        Vector2.Zero, Color.White );
            }

            spriteBatch.End();
        }

        private TimeSpan waitToDeleteTime = TimeSpan.Zero;

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        private void CheckScreenshot()
        {

            KeyboardState ks = Terraria.Main.keyState; // Use their keystate so we don't have to poll ourselves
            if (!screenshotKeyWasPressed && ks.IsKeyDown(screenshotKey))
            {
                screenshotKeyWasPressed = true;
                oldHideUI = Terraria.Main.hideUI;
                Terraria.Main.hideUI = true;
                return;
            } else if( screenshotKeyWasPressed && ks.IsKeyUp( screenshotKey ) ) {
                screenshotKeyWasPressed = false;
                Terraria.Main.hideUI = oldHideUI;

                if( nextScreenshotNumber < 0 ) {
                    nextScreenshotNumber = 0;
                    while( File.Exists( String.Format( "ss{0}.jpg", nextScreenshotNumber ) ) ) {
                        nextScreenshotNumber++;
                    }
                }

                var gd = this.GraphicsDevice;

                // Code from http://blog.raufan.com/2011/05/16/screenshot-xna/
                Color[] colors = new Color[gd.Viewport.Width * gd.Viewport.Height];
                gd.GetBackBufferData<Color>( colors );

                Texture2D tex2D = new Texture2D( gd, gd.Viewport.Width, gd.Viewport.Height );
                tex2D.SetData<Color>( colors );

                String filename = String.Format( "ss{0}.jpg", nextScreenshotNumber++ );

                FileStream stream = File.Create( filename );

                tex2D.SaveAsPng( stream, gd.Viewport.Width, gd.Viewport.Height );

                stream.Flush( );
                stream.Close( );

            }
        }
    }
}
