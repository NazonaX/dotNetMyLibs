using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapAndSimulation.Utils
{
    public static class Logger
    {
        private static string LogPath = "log.txt";
        public static void WriteMsgAndLog(string msg, RichTextBox txtbox)
        {
            System.Diagnostics.Debug.WriteLine(msg);
            txtbox.AppendText(msg + "\n");
            txtbox.Select(txtbox.TextLength, 0);
            txtbox.ScrollToCaret();
            IOOps.AppendTxt(LogPath, GetNowTime() + msg);
        }

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
