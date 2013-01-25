using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CreatureGame
{
    public static class DebugLog
    {
        private static int errorCount = 0;

        public static void Init()
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            tw.WriteLine(("----- Program executed on " + Convert.ToString(DateTime.Now.Date) + " at " + Convert.ToString(DateTime.Now.TimeOfDay) + " -----"));
            tw.Close();
        }

        public static void Close()
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            tw.WriteLine(("----- Program execution finished on " + Convert.ToString(DateTime.Now.Date) + " at " + Convert.ToString(DateTime.Now.TimeOfDay) + " -----"));
            tw.Close();
        }

        public static void WriteLine(string text)
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            errorCount++;
            tw.WriteLine("\t" + errorCount + ":" + text + "\n");
            tw.Close();
        }

        public static void Write(string text)
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            errorCount++;
            tw.Write(text);
            tw.Close();
        }
    }
}
