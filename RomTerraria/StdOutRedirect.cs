using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;
using RomTerraria;

namespace ConsoleRedirection
{
    public class StdOutRedirect : TextWriter
    {
        TextBox _output = null;
        private static StringBuilder sb = new StringBuilder();
        private static string line;

        public StdOutRedirect(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            try
            {
                //base.Write(value);
                //using a stringbuilder here now, since STDOUT apparently sends data character-at-a-time
                //so it was using an enormous amount of processor to append text to a textbox.
                sb.Append(value);
                if (value == (char)0x0A)
                {
                    MethodInvoker action = delegate { _output.AppendText(sb.ToString()); };
                    IAsyncResult ar = _output.BeginInvoke(action);
                    //wait for waithandle.
                    ar.AsyncWaitHandle.WaitOne();
                    //clear stringbuffer.
                    sb.Clear();
                }
                
            }
            catch (Exception ext)
            {
                MessageBox.Show(ext.ToString());
            }


        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }

}
