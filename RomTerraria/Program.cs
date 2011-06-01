using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RomTerraria
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles( );
            Launcher l = new Launcher();
            l.ShowDialog(); // You have to use ShowDialog() here or it creates a second message pump and that makes XNA very unhappy.

            if( l.DialogResult == DialogResult.Yes ) {
                using( var s = new ServerOverride( ) ) {
                    s.ServerWorldID = l.LaunchWorldID;
                    s.ServerPassword = l.LaunchWorldPassword;

                    if( l.InstallInfiniteInvasion ||
                        l.InstallUncheckShadowOrb ||
                        l.InstallBloodMoon ||
                        l.InstallEye ||
                        l.EnableServerConsole ) {
                        s.Components.Add( 
                            new MakeItHarder( s ) {
                                InfiniteInvasion = l.InstallInfiniteInvasion,
                                TurnOffInvasion = l.InstallUncheckShadowOrb,
                                InfiniteBloodMoon = l.InstallBloodMoon,
                                HardcoreMode = l.InstallHardcore,
                                InfiniteEye = l.InstallEye,
                                EnableConsole = l.EnableServerConsole
                            } );
                        }

                    s.Run( );
                }
            }  else if (l.DialogResult == DialogResult.OK) {
#if DEBUG
#else
                try
                {
#endif
                    using (MainOverride m = new MainOverride())
                    {

                        System.IO.Directory.CreateDirectory(Terraria.Main.SavePath); // Used to work around a bug where settings won't be saved.

                        // Access to the buffers that contain the pixel data aren't available in the Reach profile.  Switching to HiDef to allow it.
                        if (l.InstallHiDef)
                        {
                            var gdm = m.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
                            if (gdm != null)
                            {
                                gdm.GraphicsProfile = GraphicsProfile.HiDef;
                            }
                        }

                        // Are we interested in the time?
                        m.DrawClock = l.InstallClock;

                        // Do we want a pony (yes), are we getting a pony (no)
                        m.SpawnPony = l.InstallPony;

                        // Modify resolution
                        Terraria.Main.screenHeight = l.NewHeight;
                        Terraria.Main.screenWidth = l.NewWidth;

                        // Set up the health/mana trainer
                        if (l.InstallHealthTrainer ||
                            l.InstallManaTrainer)
                        {
                            m.Components.Add(new Trainer(m)
                                {
                                    RestoreHealth = l.InstallHealthTrainer,
                                    RestoreMana = l.InstallManaTrainer,
                                    RestoreTimer = l.RefreshTimer
                                }
                            );
                        }

                        // Some people want stuff harder...this is for them.
                        if ( l.InstallInfiniteInvasion ||
                             l.InstallUncheckShadowOrb ||
                             l.InstallBloodMoon ||
                             l.InstallEye ||
                             l.InstallHardcore ||
                             l.EnableServerConsole )
                        {
                            m.Components.Add(new MakeItHarder(m)
                                {
                                    InfiniteInvasion = l.InstallInfiniteInvasion,
                                    TurnOffInvasion = l.InstallUncheckShadowOrb,
                                    InfiniteBloodMoon = l.InstallBloodMoon,
                                    InfiniteEye = l.InstallEye,
                                    HardcoreMode = l.InstallHardcore,
                                    EnableConsole = l.EnableServerConsole
                                }
                            );
                        }

                        // This is pretty much ready to go, but because he has his base.Draw() call at the start of Draw, this is getting called before
                        // he draws everything so nobody can add extra data.  If he'd move his base.Draw() call to the end of the Draw(GameTime) method,
                        // we could do stuff like this without hacks.
                        if (l.InstallMinimap ||
                            l.InstallSurroundingAreaMinimap)
                        {
                            m.MinimapOverlay = new MapOverlay(m)
                            {
                                ShowOverworld = l.InstallMinimap,
                                ShowRealtime = l.InstallSurroundingAreaMinimap
                            };
                            m.Components.Add(m.MinimapOverlay);
                            m.DrawMinimap = true;
                        }

                        m.Run();
                    }
#if DEBUG
#else

                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("[{0}]\n{1}\n\n{2}", 
                                        ex.GetType().ToString(), 
                                        ex.Message, 
                                        ex.StackTrace), 
                                    "Error", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Stop);
                }
#endif
            }
        }
    }
}
