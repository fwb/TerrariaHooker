using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ConsoleRedirection
{
    public class StdOutRedirect : TextWriter
    {
        TextBox _output = null;

        public StdOutRedirect(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            MethodInvoker action = delegate { _output.AppendText(value.ToString()); };
            _output.BeginInvoke(action);

        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
