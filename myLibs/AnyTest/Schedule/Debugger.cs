using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AnyTest.Schedule
{
    public class Debugger
    {
        private static bool IsDebugMode = true;
        private static bool WriteConsole = true;
        private StringBuilder _sb = new StringBuilder();

        public static void WriteLine(String msg, params Object[] objs)
        {
            if (IsDebugMode)
            {
                System.Diagnostics.Debug.Write(msg);
                foreach(Object o in objs)
                {
                    System.Diagnostics.Debug.Write(objs.ToString());
                }
                System.Diagnostics.Debug.WriteLine("");
                if (WriteConsole)
                {
                    Console.Write(msg);
                    foreach (Object o in objs)
                    {
                        Console.Write(objs.ToString());
                    }
                   Console.WriteLine("");
                }
            }
        }

        public static void SetDebugMode(bool debugMode)
        {
            IsDebugMode = debugMode;
        }
        public static bool GetDebugMode()
        {
            return IsDebugMode;
        }
        public static void SetWriteConsole(bool writeConsole)
        {
            WriteConsole = writeConsole;
        }

        /// <summary>
        /// must be used withd BuildString
        /// </summary>
        public Debugger StringAppend(Object str)
        {
            _sb.Append(str);
            return this;
        }
        public String BuildString()
        {
            String str = _sb.ToString();
            _sb.Clear();
            return str;
        }
        public void WriteTofile()
        {
            using(FileStream fs = File.Create("output-" + DateTime.Now.ToFileTimeUtc() + ".txt"))
            {
                byte[] data = Encoding.UTF8.GetBytes(this.BuildString());
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
        }

    }
}
