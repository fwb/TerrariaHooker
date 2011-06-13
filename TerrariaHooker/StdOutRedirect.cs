using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TerrariaHooker
{
    public class StdOutRedirect : TextWriter
    {
        TextBox _output = null;
        public StringBuilder sb = new StringBuilder();
        public static string current;
        public static int tick = 0;
        

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
                    //copy stringbuilder contents to local string (so the contents aren't lost
                    //if the invoke takes too long, or if multiple writes from multiple threads
                    //are processed).
                    string t = sb.ToString();
                    var n = t.Trim(); //don't want newlines in our shit

                    //<x>ing <object>: xx% = 3
                    if (n.Length > 2)
                    {
                        //get last character
                        var k = n.Substring(n.Length - 1);
                        if (k == "%")
                        {
                            //if we're not already dotting up a data reporting msg, start
                            if (current == null)
                            {
                                current = n.Split(' ')[0];
                                t = current;
                            } else if (current != n.Split(' ')[0]) //if the current is a new one, restart + leading newline
                            {
                                current = n.Split(' ')[0];
                                t = Environment.NewLine + current;
                            } else //we're currently dotting one, and it's old.
                            {
                                if (tick % 2 == 0)
                                {
                                    t = ".";
                                    tick++;
                                }
                                else
                                {
                                    tick++;
                                    sb.Clear();
                                    return;
                                }
                            }
                        } else if (current != null)
                        {
                            current = null;
                            t = Environment.NewLine + t;
                        }
                    }

                    MethodInvoker action = delegate { _output.AppendText(t); };
                    try
                    {
                        _output.BeginInvoke(action);
                    } catch
                    {
                        //.. don't care! the only time this excepts is if something tries to write to the console and it's 
                        //not available i.e. we've closed it.
                    }

                    //empty stringbuilder
                    sb.Clear();
                    return;
                }

                //handle lines ending with a semicolon instead of a newline, for input.
                if (value == (char)0x3A)
                {
                    string p = sb.ToString();
                    var r = p.Trim().Split(' ');

                    //if the lines are data related, pass so it can be handled when the percentage and newline is reached
                    if (r[0] == "Resetting" || r[0] == "Settling" || r[0] == "Loading" || r[0] == "Saving")
                        return;

                    //reset current
                    current = null;

                    MethodInvoker action = delegate { _output.AppendText(p); };
                    _output.BeginInvoke(action);

                    //empty stringbuilder
                    sb.Clear();
                    return;

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
