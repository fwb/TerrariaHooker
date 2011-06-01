using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RomTerraria
{
    public class MapOverlay : DrawableGameComponent
    {
        private const int surroundingAreaWidth = 400;
        private const int surroundingAreaHeight = 300;
        private Terraria.Main TerrariaGame = null;
        private Texture2D OverworldMap, OverworldMapNextTexture;
        private Texture2D SurroundingAreaMap, SurroundingAreaMapNextTexture;
        private int width, height;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private TimeSpan updateTime = TimeSpan.Zero;
        private TimeSpan realtimeElapsedTime = TimeSpan.Zero;
        private TimeSpan realtimeUpdateTime = TimeSpan.Zero;
        private Color[] textureColors;
        private SpriteBatch spriteBatch;
        private bool textureColorsInitialized = false;
        private Color[] surroundingAreaTexture = new Color[surroundingAreaWidth * surroundingAreaHeight];

        public bool ShowOverworld { get; set; }
        public bool ShowRealtime { get; set; }

        private void InitializeTextureColors()
        {
            if (textureColorsInitialized) return;
            textureColorsInitialized = true;
            for (int i = 0; i < Terraria.Main.tileTexture.Length; i++)
            {
                if (Terraria.Main.tileTexture[i] == null)
                {
                    textureColors[i] = Color.Transparent;
                }
                else
                {
                    Color[] retrievedColor = new Color[1];
                    Terraria.Main.tileTexture[i].GetData<Color>(0, new Rectangle(4, 4, 1, 1), retrievedColor, 0, 1);
                    textureColors[i] = retrievedColor[0];
                }
            }
        }


        public MapOverlay(Game game) : base(game)
        {
            TerrariaGame = (Terraria.Main)game;
        }

        public override void Initialize()
        {
            base.Initialize();
            base.Visible = true;
            //DrawOrder = 99999;
        }

        protected override void LoadContent()
        {
            width = Terraria.Main.screenWidth / 2;
            height = Terraria.Main.screenHeight / 2;
            OverworldMap = new Texture2D(TerrariaGame.GraphicsDevice, width, height);
            OverworldMapNextTexture = new Texture2D(TerrariaGame.GraphicsDevice, width, height);
            SurroundingAreaMap = new Texture2D(TerrariaGame.GraphicsDevice, surroundingAreaWidth, surroundingAreaHeight);
            SurroundingAreaMapNextTexture = new Texture2D(TerrariaGame.GraphicsDevice, surroundingAreaWidth, surroundingAreaHeight);
            textureColors = new Color[Terraria.Main.tileTexture.Length];
            spriteBatch = new SpriteBatch(TerrariaGame.GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            OverworldMap.Dispose();
            OverworldMap = null;
        }

        public void Draw()
        {
            if (Terraria.Main.gameMenu || Terraria.Main.hideUI) return; // If we're in the menu, we shouldn't draw
            InitializeTextureColors(); // Has to happen in draw, otherwise the textures haven't necessarily been loaded yet


            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            int mapHeight = (int)(height * ((float)Terraria.Main.maxTilesY / Terraria.Main.maxTilesX));

            if (Terraria.Main.playerInventory)
            { // Only going to draw full map if inventory is up
                if (ShowOverworld)
                {
                    spriteBatch.Draw(Terraria.Main.fadeTexture,
        new Rectangle(width / 2, Terraria.Main.screenHeight - mapHeight, width, mapHeight), Color.FromNonPremultiplied(0, 0, 0, 224));
                    var p = Terraria.Main.player[Terraria.Main.myPlayer];
                    spriteBatch.Draw(OverworldMap, new Rectangle(width / 2, Terraria.Main.screenHeight - mapHeight, width, mapHeight), Color.White);
                    if (p != null && p.active)
                    {
                        int x = (int)((p.position.X + (p.width * 0.5)) / 16.0) / (int)((float)Terraria.Main.maxTilesX / width);
                        int y = (int)((p.position.Y + (p.height * 0.5)) / 16.0) / (int)((float)Terraria.Main.maxTilesY / mapHeight);
                        Rectangle playerLoc = new Rectangle((width / 2) + x, (Terraria.Main.screenHeight - mapHeight) + y, 12, 12);
                        spriteBatch.Draw(Terraria.Main.cursorTexture, playerLoc, Terraria.Main.cursorColor);
                    }
                }
            }
            else
            {
                if (ShowRealtime)
                {
                    spriteBatch.Draw(Terraria.Main.fadeTexture,
        new Rectangle(width / 2, Terraria.Main.screenHeight - mapHeight, width, mapHeight), Color.FromNonPremultiplied(0, 0, 0, 224));
                    // Draw surrounding area map
                    //spriteBatch.Draw(SurroundingAreaMap, new Rectangle(width / 2, Terraria.Main.screenHeight - mapHeight, width, mapHeight), Color.White);

                    spriteBatch.Draw(SurroundingAreaMap, new Rectangle(width / 2, Terraria.Main.screenHeight - mapHeight, width, mapHeight),
                        new Rectangle(0, surroundingAreaHeight / 4, surroundingAreaWidth, surroundingAreaHeight / 2), Color.White);
                }
            }
            spriteBatch.End();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            elapsedTime += gameTime.ElapsedGameTime;
            realtimeElapsedTime += gameTime.ElapsedGameTime;
            if (Terraria.Main.gameMenu) return; // If we're in the menu, we shouldn't update
            if (!textureColorsInitialized) return; // Nothing to draw yet to texture

            // Working on a real-time smaller map section
            if (ShowRealtime && realtimeUpdateTime < realtimeElapsedTime)
            {
                realtimeUpdateTime = TimeSpan.FromMilliseconds(500);
                realtimeElapsedTime = TimeSpan.Zero;

                var p = Terraria.Main.player[Terraria.Main.myPlayer];
                int mapHeight = (int)(height * ((float)Terraria.Main.maxTilesY / Terraria.Main.maxTilesX));

                int px = (int)(p.position.X / 16.0);
                int py = (int)(p.position.Y / 16.0);

                int left = Math.Max(px - surroundingAreaWidth / 2, 0);
                int top = Math.Max(py - surroundingAreaHeight / 2, 0);
                for (int y = top, yofs = 0; yofs < surroundingAreaHeight && y < Terraria.Main.maxTilesY; y++, yofs++)
                {
                    for (int x = left, xofs = 0; xofs < surroundingAreaWidth && x < Terraria.Main.maxTilesX; x++, xofs++)
                    {
                        float lightLevel = 0.5f; // 0.25f + (Terraria.Lighting.Brightness(w, h) * 0.75f);
                        if (x < 0 || y < 0)
                        {
                            surroundingAreaTexture[xofs + (yofs * surroundingAreaWidth)] = Color.Transparent;
                        }
                        else
                        {
                            Terraria.Tile t = Terraria.Main.tile[x, y];
                            surroundingAreaTexture[xofs + (yofs * surroundingAreaWidth)] = (t != null) ?
                                (t.active ?
                                    Color.FromNonPremultiplied(
                                        new Vector4(textureColors[t.type].R * lightLevel / 256,
                                                    textureColors[t.type].G * lightLevel / 256,
                                                    textureColors[t.type].B * lightLevel / 256, 1.0f))
                                    : Color.Transparent) :
                                Color.Transparent;
                        }
                    }
                }


                surroundingAreaTexture[surroundingAreaWidth * (py - top) + (px - left)] = Color.Magenta;
                try
                {
                    SurroundingAreaMapNextTexture.SetData<Color>(surroundingAreaTexture, 0, surroundingAreaWidth * surroundingAreaHeight);
                    Texture2D temp2 = SurroundingAreaMap;
                    SurroundingAreaMap = SurroundingAreaMapNextTexture;
                    SurroundingAreaMapNextTexture = temp2;
                }
                catch (InvalidOperationException)
                { } // Ignore...means FPS is too low
            }
            if (elapsedTime < updateTime) return; // Only want to do occasionally to keep perf up

            if (ShowOverworld)
            {
                updateTime = TimeSpan.FromMinutes(Terraria.Main.netMode == 0 ? 5 : 1); // Update every five minutes in single player, one in multiplayer
                elapsedTime = TimeSpan.Zero;

                int worldwidth = Terraria.Main.maxTilesX;
                int worldheight = Terraria.Main.maxTilesY;
                int widthstep = (int)((float)worldwidth / width);
                int heightstep = (int)((float)worldheight / height);
                Color[] newTexture = new Color[width * height];
                for (int h = 0, hofs = 0; h < worldheight && hofs < height; h += heightstep, hofs++)
                {
                    for (int w = 0, wofs = 0; w < worldwidth && wofs < width; w += widthstep, wofs++)
                    {
                        // Terraria.Lighting.Brightness is calculated only for the current screen.  D'oh.
                        float lightLevel = 0.5f; // 0.25f + (Terraria.Lighting.Brightness(w, h) * 0.75f);
                        Terraria.Tile t = Terraria.Main.tile[w, h];
                        newTexture[wofs + (hofs * width)] = (t != null) ?
                            (t.active ?
                                Color.FromNonPremultiplied(
                                    new Vector4(textureColors[t.type].R * lightLevel / 256,
                                                textureColors[t.type].G * lightLevel / 256,
                                                textureColors[t.type].B * lightLevel / 256, 1.0f))
                                : Color.Transparent) :
                            Color.Transparent;
                    }
                }
                try
                {
                    OverworldMapNextTexture.SetData<Color>(newTexture, 0, width * height);
                    Texture2D temp = OverworldMap;
                    OverworldMap = OverworldMapNextTexture;
                    OverworldMapNextTexture = temp;
                }
                catch (InvalidOperationException) { } // Ignore...means FPS is too low
                // Was only using this to test the overlay creation piece.  You will quickly get an exception if you enable this.
                //using (var fs = new System.IO.FileStream("overlay.jpg", System.IO.FileMode.Create, System.IO.FileAccess.Write))
                //{
                //    Overlay.SaveAsJpeg(fs, width, height);
                //}
            }
        }
    }
}
