using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sodoku_9_9
{
    public partial class Form1 : Form
    {
        private int[,] data = new int[9,9];
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void OutputData(int[,] answer)
        {
            //System.Diagnostics.Debug.WriteLine("Using OutputData...");
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Control ctr = Controls.Find("textBox" + i + j, true)[0];
                    TextBox tb = ctr as TextBox;
                    tb.Text = "" + answer[i, j];
                    
                }
            }
        }

        private void btn_check_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    Control ctr = Controls.Find("textBox" + i + j, true)[0];
                    TextBox tb = ctr as TextBox;
                    if (tb == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Control as TextBox failed...");
                        return;
                    }
                    else
                    {
                        if ("".Equals(tb.Text))
                            continue;
                        else
                        {
                            data[i, j] = Int32.Parse(tb.Text);
                            tb.Enabled = false;
                        }
                    }
                }
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            data = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Control ctr = Controls.Find("textBox" + i + j, true)[0];
                    TextBox tb = ctr as TextBox;
                    if (tb == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Control as TextBox failed...");
                        return;
                    }
                    else
                    {
                        tb.Enabled = true;
                        tb.Text = "";
                    }
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    System.Diagnostics.Debug.Write(data[i, j] + " ");
                }
                System.Diagnostics.Debug.Write("\n");
            }
            //send to do the calculation
            Control.CheckForIllegalCrossThreadCalls = false;
            Sodoku sodoku = new Sodoku(data);
            sodoku.outPutplz += this.OutputData;
            int[,] answer = sodoku.DoCalculate();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Control ctr = Controls.Find("textBox" + i + j, true)[0];
                    TextBox tb = ctr as TextBox;
                    tb.Text = "" + answer[i, j];
                }
            }
        }
    }
}
