using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace await_asyncTEST
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Start waiting 3000 ms...\n");
            richTextBox1.Update();
            await Task.Run(() => Wait());
            richTextBox1.AppendText("Wait done...\n\n");
            richTextBox1.Update();
            String res = await Task.Run(() => GetString());
            richTextBox1.AppendText("Test Result is " + res);
        }

        private void Wait()
        {
            System.Diagnostics.Debug.WriteLine("Doing Wait...");
            Thread.Sleep(3000);
            System.Diagnostics.Debug.WriteLine("Wait done...");
        }

        private async Task Wait2()
        {
            System.Diagnostics.Debug.WriteLine("Doing Wait2...");
            Thread.Sleep(3000);
            System.Diagnostics.Debug.WriteLine("Wait2 done...");
        }

        private async Task<String> GetString()
        {
            System.Diagnostics.Debug.WriteLine("Doing GetString...");
            Thread.Sleep(3000);
            System.Diagnostics.Debug.WriteLine("GetString done...");
            return "GetString Result";
        }
    }
}
