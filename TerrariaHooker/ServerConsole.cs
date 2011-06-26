using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using Microsoft.Xna.Framework;
using Terraria;
using TerrariaHooker.AccountManagement;

namespace TerrariaHooker {


    public partial class ServerConsole : Form
    {

        public static SockHook sockHook;
        private static int selectedPlayer = -1;
        private static Properties.Settings settings = new Properties.Settings();


        //private delegate void NCallback();

        [DllImport("kernel32.dll",
            EntryPoint = "WriteConsoleInput",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool WriteConsoleInput(SafeFileHandle hConsoleInput,
           INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEventsWritten);

        [DllImport("user32.dll",
            EntryPoint = "MapVirtualKey",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);


        public class PlayerInfo : ListViewItem
        {
            public int Id { get; set; }
            public string PlayerName { get; set; }

            public PlayerInfo(int id, string name)
            {
                Id = id;
                PlayerName = name;
            }

            public override string ToString()
            {
                return PlayerName;
            }

        }

        public class NPCInfo
        {
            public int Type { get; set; }
            public string Name { get; set; }
            public int Damage { get; set; }
            public int Defense { get; set; }
            public int LifeMax { get; set; }
            public Single DropValue { get; set; }

            public NPCInfo(int id, string name, int damage, int defense,
                            int lifemax, Single dropvalue)
            {
                Type = id;
                Name = name;
                Damage = damage;
                Defense = defense;
                LifeMax = lifemax;
                DropValue = dropvalue;
            }

            public override string ToString()
            {
                return Name;
            }
        }


        private TextWriter _writer = null;
        private delegate void SpawnMeteorCallback();
        public static Stream _out;


        public ServerConsole()
        {

            InitializeComponent();
            LoadLauncherSettings();
            _writer = new StdOutRedirect(textBox1);
            // Redirect the out Console stream
            _out = Console.OpenStandardOutput();

            Console.SetOut( _writer );
            sockHook = new SockHook();

            check_wlEnabled.Checked = settings.EnableWhitelist;
            check_anonLoginEnabled.Checked = false;

            //initialize spawn defaults.
            InitSpawns();

        }
        private void InitSpawns()
        {
            track_spawnRate.Value = Data.DEFAULT_SPAWN_RATE;
            text_maxSpawns.Text = Data.DEFAULT_MAX_SPAWNS.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            sockHook.InitializeHooks();
        }

        private void LoadLauncherSettings()
        {
            //enableInvasion.Checked = mih.InfiniteInvasion;
            //enableBloodMoon.Checked = mih.InfiniteBloodMoon;
            //enableEyeSpawn.Checked = mih.InfiniteEye;
        }

        private void LoadPlayerInfo()
        {
            playerList.Items.Clear();
            for (var i = 0; i < Terraria.Main.maxNetPlayers; i++)
            {
                var p = Terraria.Main.player[i];
                if (p.active)
                {
                    playerList.Items.Add(new ListViewItem(new[] {
                                                          i.ToString(),
                                                          p.name,
                                                          Netplay.serverSock[i].tcpClient.Client.RemoteEndPoint.ToString(),
                                                          Commands.player[i].Whitelisted.ToString(),}));
                }
            }
        }

        private void LoadNPCInfo()
        {
            npcPicker.Items.Clear();
            var npc = new NPC();
            for (var i = 1; i <= 69; i++)
            {
                try
                {
                    npc.SetDefaults(i);
                }
                catch
                {
                    MessageBox.Show(string.Format("Error loading NPC type [{0}]", i));
                }

                if (npc.name != null)
                {
                    npcPicker.Items.Add(new NPCInfo(npc.type, npc.name+ " [" + npc.type + "]", npc.damage,
                                                      npc.defense, npc.lifeMax,
                                                      npc.value));
                }
            }
            npcPicker.SelectedIndex = 0;
        }

        private void SpawnNearPlayer(int playerNum, int npcType, bool useNearSpawn)
        {
            //todo: i want to move implementation to Commands.cs
            //Calculate our own spawn location
            var position = Main.player[playerNum].position;
            NPC.NewNPC((int)position.X, (int)position.Y, npcType);
        }

        private void SpawnButtonClick(object sender, EventArgs e)
        {
            //todo: i want to move implementation to Commands.cs
            if (npcPicker.SelectedItem != null && selectedPlayer != -1)
            {
                var npc = npcPicker.SelectedItem as NPCInfo;
                SpawnNearPlayer(selectedPlayer, npc.Type, false);
            }
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadNPCsClick(object sender, EventArgs e)
        {
            LoadNPCInfo();
        }

        private void SpawnMeteorButtonClick(object sender, EventArgs e)
        {
            SpawnMeteor();
        }

        private void SpawnMeteor()
        {
            SpawnMeteorCallback d = new SpawnMeteorCallback(WorldEvents.SpawnMeteorCB);
            Invoke(d, null);
        }


        private void RefreshPlayersClick(object sender, EventArgs e)
        {
            LoadPlayerInfo();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (consoleInput.Text.Substring(0, 1) == ".")
            {
                textBox1.AppendText(consoleInput.Text + Environment.NewLine);
                var finalBuf = new byte[consoleInput.Text.Length + 9];
                //0xFC is our player ID for the console. hopefully never going to really be used
                var buf = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x19, 0xFC, 0x00, 0x00, 0x00 };

                var t = Encoding.ASCII.GetBytes(consoleInput.Text);
                Buffer.BlockCopy(buf, 0, finalBuf, 0, buf.Length);
                Buffer.BlockCopy(t, 0, finalBuf, 9, t.Length);
                consoleInput.Clear();
                Commands.HandleChatMsg(finalBuf, 0);
                return;

            }
            textBox1.AppendText(consoleInput.Text + Environment.NewLine);
            //hooray!
            Utils.sendLineToConsole(consoleInput.Text);
            consoleInput.Clear();


        }


        private void consoleInputKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            { // enter key pressed
                if (consoleInput.Text == "")
                    return;
                button3_Click(sender, e);
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sockHook.InitializeHooks();
        }


        private void playerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (playerList.FocusedItem == null)
            {
                selectedPlayer = -1;
                label2.Text = "Selected player: none";
                return;
            }
            int id;
            if (int.TryParse(playerList.FocusedItem.SubItems[0].Text, out id) == false)
                return;
            selectedPlayer = id;
            label2.Text = "Selected player: " + playerList.FocusedItem.SubItems[1].Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (playerList.FocusedItem == null)
                return;
            int id;
            if (int.TryParse(playerList.FocusedItem.SubItems[0].Text, out id) == false)
                return;

            Commands.kickUser(id);
            LoadPlayerInfo();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (playerList.FocusedItem == null)
                return;
            int id;
            if (int.TryParse(playerList.FocusedItem.SubItems[0].Text, out id) == false)
                return;

            Commands.banUser(id);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (playerList.FocusedItem == null)
                return;
            int id;
            if (int.TryParse(playerList.FocusedItem.SubItems[0].Text, out id) == false)
                return;

            var ip = Utils.ParseEndPointAddr(playerList.FocusedItem.SubItems[2].Text);
            var username = playerList.FocusedItem.SubItems[1].Text;
            AccountManager.CreateAccount( username, ip );
            AccountManager.SaveAccounts( );
            Commands.player[id].Whitelisted = true;
            Commands.SendChatMsg("You are now whitelisted.", id, Color.Yellow);
            LoadPlayerInfo();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (playerList.FocusedItem == null)
                return;
            int id;
            if (int.TryParse(playerList.FocusedItem.SubItems[0].Text, out id) == false)
                return;

            var ip = Utils.ParseEndPointAddr(playerList.FocusedItem.SubItems[2].Text);
            AccountManager.RemoveAccount( ip );
            Commands.player[id].Whitelisted = false;
            Commands.SendChatMsg("You are no longer whitelisted.", id, Color.Yellow);
            LoadPlayerInfo();
        }


        private void check_wlEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AccountManager.WhitelistActive = check_wlEnabled.Checked;
        }

        private void check_anonLoginEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Commands.allowUnwhiteLogin = check_anonLoginEnabled.Checked;
        }
        private void tabControl1_SelectedIndexchanged(object sender, EventArgs e)
        {
            switch (((TabControl)sender).SelectedIndex)
            {
                case 4:
                    check_wlEnabled.Checked = AccountManager.WhitelistActive;
                    check_anonLoginEnabled.Checked = Commands.allowUnwhiteLogin;
                    break;
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            check_SpawnDisallowBreak.Enabled = check_ProtectSpawn.Checked;
            check_SpawnDisallowPlace.Enabled = check_ProtectSpawn.Checked;
            check_SpawnDisallowUse.Enabled = check_ProtectSpawn.Checked;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            int width;
            int height;

            if (int.TryParse(text_protectWidth.Text, out width) == false)
            {
                MessageBox.Show("Width and Height must be positive numbers");
                return;
            }

            if (int.TryParse(text_protectHeight.Text, out height) == false)
            {
                MessageBox.Show("Width and Height must be positive numbers");
                return;
            }

            Commands.SetProtectWidth = width;
            Commands.SetProtectHeight = height;

            Commands.protectSpawn = check_ProtectSpawn.Checked;
            Commands.spawnAllowBreak = check_SpawnDisallowBreak.Checked;
            Commands.spawnAllowUse = check_SpawnDisallowUse.Checked;
            Commands.spawnAllowPlace = check_SpawnDisallowPlace.Checked;

        }

        private void button_EnemyApply_Click(object sender, EventArgs e)
        {
            Commands.disableSpawns(check_DisableEnemies.Checked);
            button_EnemyApply.Enabled = false;
            if (check_enableCustomRates.Checked && !check_DisableEnemies.Checked)
            {
                int a = track_spawnRate.Value; //spawnrate
                int b; //maxspawns
                if (int.TryParse(text_maxSpawns.Text, out b) == false)
                {
                    MessageBox.Show("Max Spawns must be a number");
                    return;
                }

                Commands.setSpawnValues(a, b);
                label_customRateEnabled.Text = check_enableCustomRates.Checked ? "Custom Rates: ENABLED" : "Custom Rates: DISABLED";
                label_maxSpawns.Text = String.Format("Max Spawns: {0}", b);
                label_spawnInterval.Text = String.Format("Spawn Rate: {0} (ms)", track_spawnRate.Value);

            }
            else
            {
                //if it's unchecked, reset to defaults.
                if (!check_DisableEnemies.Checked)
                    Commands.disableSpawns(false);  //method name is deceptive, using disablespawns(false)
                                                    //resets spawn rate to defaults.
            }
            label_spawnsEnabled.Text = !check_DisableEnemies.Checked ? "Spawns: ENABLED" : "Spawns: DISABLED";

        }

        private void button10_Click(object sender, EventArgs e)
        {
            InitSpawns();
            button_EnemyApply.Enabled = true;
        }

        private void check_DisableEnemies_CheckedChanged(object sender, EventArgs e)
        {
            panel_customSpawn.Enabled = !check_DisableEnemies.Checked;
            button_EnemyApply.Enabled = true;
        }

        private void check_enableCustomRates_CheckedChanged(object sender, EventArgs e)
        {
            panel_CustomInternal.Enabled = check_enableCustomRates.Checked;
            button_EnemyApply.Enabled = true;
        }

        private void text_maxSpawns_TextChanged(object sender, EventArgs e)
        {
            button_EnemyApply.Enabled = true;
        }

        private void track_spawnRate_Scroll(object sender, EventArgs e)
        {
            button_EnemyApply.Enabled = true;
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void ExitSave()
        {
            settings.EnableAnonLogin = Commands.allowUnwhiteLogin;
            settings.EnableWhitelist = AccountManager.WhitelistActive;
            settings.Save();
            Utils.sendLineToConsole("exit");
        }

        private void ExitNoSave()
        {
            settings.EnableAnonLogin = Commands.allowUnwhiteLogin;
            settings.EnableWhitelist = AccountManager.WhitelistActive;
            settings.Save();
            Utils.sendLineToConsole("exit-nosave");
        }

        private void ServerConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("Do you want to save the world?", "Exiting TerrariaHooker",
                                       MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            switch (res)
            {
                case DialogResult.Yes:
                    ExitSave();
                    break;
                case DialogResult.No:
                    ExitNoSave();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            int x;
            int y;
            int sx = Main.spawnTileX;
            int sy = Main.spawnTileY;
            int r = 50;

            for (int i = 0; i < 1000; i++)
            {
                if (Main.chest[i] != null)
                {
                    x = Main.chest[i].x;
                    y = Main.chest[i].y;
                    if (!(x > sx - r && x < sx + r && y > sy - r && y < sy + r))
                        continue;

                    listBox1.Items.Add(i);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            if (listBox1.SelectedItem == null)
                return;

            int c = (int)listBox1.SelectedItem;
            for (int i = 0; i < Chest.maxItems; i++)
            {
                if (Main.chest[c].item[i].name == null)
                    continue;

                listBox2.Items.Add(Main.chest[c].item[i].name);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            int c = (int)listBox1.SelectedItem;
            Commands.MAGIC_CHEST_ID = c;



            for (int i = 0; i < Chest.maxItems; i++)
                Main.chest[c].item[i] = new Item();


            Main.chest[c].item[0] = new Item();
            Main.chest[c].item[0].SetDefaults("Copper Axe");
            Main.chest[c].item[0].name = "TEST ITEM 1";

            Main.chest[c].item[1] = new Item();
            Main.chest[c].item[1].SetDefaults("Gold Pickaxe");
            Main.chest[c].item[1].name = "TEST ITEM 2";

        }

        private void npcPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (npcPicker.SelectedItem != null)
            {
                var npc = npcPicker.SelectedItem as NPCInfo;
                label10.Text = String.Format("ID: {0} \nDamage: {1} \nHealth: {2} \nDefense: {3}", npc.Type, npc.Damage, npc.LifeMax, npc.Defense);
            }
        }

        private void Save_Click( object sender, EventArgs e ) {
            AccountManager.SaveAccounts( );
        }

        private void Load_Click( object sender, EventArgs e ) {
            AccountManager.LoadAccounts( );
        }


    }

}
