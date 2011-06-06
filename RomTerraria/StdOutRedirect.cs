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
        public StringBuilder sb = new StringBuilder();      

        public StdOutRedirect(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            try
            {
                base.Write(value);
                //using a stringbuilder here now, since STDOUT apparently sends data character-at-a-time
                //so it was using an enormous amount of processor to append text to a textbox.
                sb.Append(value);
                if (value == (char)0x0A)
                {
                    MethodInvoker action = delegate { _output.AppendText(sb.ToString()); };
                    //this is iffy. it was originally begininvoke but an IAsyncResult was hanging
                    //when the console was written to cross-thread. This works now, but it probably
                    //causes some slowdown. I guess it can be switched back by making a delegate
                    //on ServerConsole so that running AppendText as part of the threadpool doesn't
                    //have issues, but then I suppose threads completing out of order would produce
                    //console text out of order.
                    _output.Invoke(action);
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
