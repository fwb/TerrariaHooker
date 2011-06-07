using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using Terraria;

namespace TerrariaHooker
{
    static class Program
    {

        [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        /*
         [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();*/


        //private const int STD_OUTPUT_HANDLE = -11;
        private const int STD_INPUT_HANDLE = -10;
        //private const int MY_CODE_PAGE = 437;
        //public static FileStream fileStreamSTDIN = null;
        public static IntPtr STDIN_HANDLE1;
        public static SafeFileHandle STDIN_HANDLE;


        static void showServerConsole()
        {
            //run new form in thread, otherwise the console window/main process
            //will be locked
            Application.Run(new ServerConsole());
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Launcher l = new Launcher();
            l.ShowDialog();
            // You have to use ShowDialog() here or it creates a second message pump and that makes XNA very unhappy.

            if (l.DialogResult == DialogResult.Yes)
            {
                STDIN_HANDLE1 = GetStdHandle(STD_INPUT_HANDLE);
                STDIN_HANDLE = new SafeFileHandle(STDIN_HANDLE1, false);

                using (Main s = new Main())
                {

                   //NEW 1.0.3
                    var sf = new Thread(showServerConsole);
                    sf.Start();

                    Netplay.serverPort = l.LaunchWorldPort;
                    s.SetNetPlayers(l.LaunchWorldPlayers);
                    Netplay.password = l.LaunchWorldPassword;
                    if (l.LaunchWorldID != -1)
                        s.SetWorld(Terraria.Main.loadWorldPath[l.LaunchWorldID]);

                    try
                    {
                        s.DedServ();  
                    }
                    catch (Exception ext)
                    {
                        MessageBox.Show(ext.ToString());
                    }

                }
            }
        }
    }
}
