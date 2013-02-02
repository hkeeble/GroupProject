using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace VOiD.Components
{
    public class DebugLog : GameComponent
    {
        private static int errorCount = 0;

        public DebugLog(Game game)
            : base(game)
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            string temp = ("----- Program executed on " + Convert.ToString(DateTime.Now.Date) + " at " + Convert.ToString(DateTime.Now.TimeOfDay) + " -----");
            Console.WriteLine(temp);
            tw.WriteLine(temp);
            tw.Close();
        }

        public static void Close()
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            string temp = ("----- Program execution finished on " + Convert.ToString(DateTime.Now.Date) + " at " + Convert.ToString(DateTime.Now.TimeOfDay) + " -----");
            Console.WriteLine(temp);
            tw.WriteLine(temp);
            tw.Close();
        }

        public static void WriteLine(string text)
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            errorCount++;
            string temp = ("\t" + errorCount + ":" + text + "\n");
            Console.WriteLine(temp);
            tw.WriteLine(temp);
            tw.Close();
        }

        public static void Write(string text)
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            errorCount++;
            Console.WriteLine(text);
            tw.Write(text);
            tw.Close();
        }

        protected override void Dispose(bool disposing)
        {
            Close();
            base.Dispose(disposing);
        }
    }
}
