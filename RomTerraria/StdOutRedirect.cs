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
                if (value == (char)0x0A || value == (char)0x3A)
                {
                    //copy stringbuilder contents to local string (so the contents aren't lost
                    //if the invoke takes too long, or if multiple writes from multiple threads
                    //are processed).
                    string t = sb.ToString();
                    MethodInvoker action = delegate { _output.AppendText(t); };
                    _output.BeginInvoke(action);

                    //empty stringbuilder
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
