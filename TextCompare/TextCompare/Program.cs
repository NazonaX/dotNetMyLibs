using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextCompare
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void AppendTextColorful(this RichTextBox rtBox, string text, Color color)
        {
            int start = rtBox.TextLength;
            rtBox.AppendText(text);
            int length = text.Length;
            rtBox.Select(start, length);
            //System.Diagnostics.Debug.WriteLine(rtBox.SelectedText);
            rtBox.SelectionBackColor = color;
            rtBox.Select(rtBox.Text.Length, 0);
        }

    }
}
