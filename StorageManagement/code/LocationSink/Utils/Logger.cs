using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Logger
    {
        public static string LogPath = "log.txt";

        public static void WriteMsgAndLog(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
            IOOps.AppendTxt(LogPath, GetNowTime() + msg);
        }

        private static string GetNowTime()
        {
            return DateTime.Now.ToShortDateString() + " "
                + DateTime.Now.ToLongTimeString() + ": ";
        }
    }
}
