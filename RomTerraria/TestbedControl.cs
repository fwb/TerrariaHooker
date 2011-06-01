using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RomTerraria
{
    public partial class TestbedControl : UserControl
    {
        public TestbedControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerConsole.sockHook.InitializeHooks();
        }

        private void TestbedControl_Load(object sender, EventArgs e)
        {

        }
    }
}
