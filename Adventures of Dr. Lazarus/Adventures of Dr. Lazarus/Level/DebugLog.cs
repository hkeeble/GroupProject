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

        public static void WriteLine(string text)
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            errorCount++;
            tw.WriteLine(errorCount + ":" + text + "\n");
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
