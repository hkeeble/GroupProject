﻿using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace VOiD.Components
{
    /*
    public class testing
    {
        internal static int[] thing(ulong ID)
        {
            string temp = ID.ToString();
            int[] rtn = new int[temp.Length];
            for(int i = 0; i < rtn.Length; i++) {
                rtn[i] = int.Parse(temp[i].ToString());
            }

            foreach (int value in rtn)
                Console.WriteLine("the digit is {0}\n", value);
            return rtn;
        }
    }*/

    public class DebugLog : GameComponent
    {
        private static int errorCount = 0;

        public DebugLog(Game game)
            : base(game)
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            string temp = ("----- Program executed on " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString() + " -----");
            Console.WriteLine(temp);
            tw.WriteLine(temp);
            tw.Close();
            //testing.thing(3456898254673474675);
        }

        public static void Close()
        {
            StreamWriter tw = File.AppendText("debugLog.txt");
            string temp = ("----- Program execution finished on " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString() + " -----");
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
