using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTTPTest
{
    public partial class Form1 : Form
    {
        List<TextBox> names = new List<TextBox>();
        List<TextBox> values = new List<TextBox>();

        public Form1()
        {
            InitializeComponent();
            TextBox tb_ = this.Controls.Find("tb_name1", true)[0] as TextBox;
            if(tb_ != null)
                names.Add(tb_);
            tb_ = this.Controls.Find("tb_value1", true)[0] as TextBox;
            if (tb_ != null)
                values.Add(tb_);
            this.flp_paras.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseWheel);
        }

        private void Panel_MouseWheel(object sender, MouseEventArgs e)
        {
            flp_paras.VerticalScroll.Value += 2;
            flp_paras.Refresh();
            flp_paras.Invalidate();
            flp_paras.Update();
        }

        private void bt_addp_Click(object sender, EventArgs e)
        {
            Label l1 = new Label();
            l1.Font = new Font("宋体", 12);
            l1.Text = "Name:";
            l1.Size = new System.Drawing.Size(48, 16);
            flp_paras.Controls.Add(l1);
            TextBox tb1 = new TextBox();
            tb1.Name = "tb_name" + (names.Count + 1);
            names.Add(tb1);
            flp_paras.Controls.Add(tb1);

            Label l2 = new Label();
            l2.Font = new Font("宋体", 12);
            l2.Text = "Value:";
            l2.Size = new System.Drawing.Size(56, 16);
            flp_paras.Controls.Add(l2);
            TextBox tb2 = new TextBox();
            tb2.Name = "tb_value" + (values.Count + 1);
            values.Add(tb2);
            flp_paras.Controls.Add(tb2);
        }

        private async void bt_send_Click(object sender, EventArgs e)
        {
            string address = tb_domain.Text;
            bool get = rb_get.Checked;
            if (String.IsNullOrEmpty(address))
                return;
            StringBuilder sb = new StringBuilder();
            bool first = true;
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            for(int i = 0; i < names.Count; i++)
            {
                if(String.IsNullOrEmpty(names[i].Text) || String.IsNullOrEmpty(values[i].Text))
                    continue;
                if (first)
                {
                    first = false;
                    sb.Append(names[i].Text).Append("=").Append(values[i].Text);
                }
                else
                {
                    sb.Append("&").Append(names[i].Text).Append("=").Append(values[i].Text);
                }
                paraList.Add(new KeyValuePair<string, string>(names[i].Text, values[i].Text));
            }
            string parameters = sb.ToString();
            sb.Clear();
            System.Diagnostics.Debug.WriteLine("Address-->" + address);
            if(get)
                System.Diagnostics.Debug.WriteLine("Method-->GET" );
            else
                System.Diagnostics.Debug.WriteLine("Method-->POST");
            System.Diagnostics.Debug.WriteLine("Parameters-->" + parameters);
            if (get)
            {
                //Get
                HttpClient httpClient = new HttpClient();
                if (!String.IsNullOrEmpty(parameters))
                    address += "?" + parameters;
                HttpResponseMessage hrm = new HttpResponseMessage();
                hrm.StatusCode = System.Net.HttpStatusCode.RequestTimeout;
                try
                {
                    hrm = await httpClient.GetAsync(address);
                }
                catch (Exception exc)
                {
                    AppendExceptionRecursion(exc);
                }
                if (hrm.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    rtb_result.Text = await hrm.Content.ReadAsStringAsync();
                }
                else
                {
                    rtb_result.AppendText("Failed...\n");
                    rtb_result.AppendText("Address-->" + address + "\n");
                    if (get)
                        rtb_result.AppendText("Method-->GET" + "\n");
                    else
                        rtb_result.AppendText("Method-->POST" + "\n");
                    rtb_result.AppendText("Parameters-->" + parameters + "\n");
                }
            }
            else
            {
                //Post
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage hrm = new HttpResponseMessage();
                hrm.StatusCode = System.Net.HttpStatusCode.RequestTimeout;
                try
                {
                    hrm = await httpClient.PostAsync(address, new FormUrlEncodedContent(paraList));
                }
                catch (Exception exc)
                {
                    AppendExceptionRecursion(exc);
                }
                if (hrm.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    rtb_result.Text = await hrm.Content.ReadAsStringAsync();
                }
                else
                {
                    rtb_result.AppendText("Failed...\n");
                    rtb_result.AppendText("Address-->" + address + "\n");
                    if (get)
                        rtb_result.AppendText("Method-->GET" + "\n");
                    else
                        rtb_result.AppendText("Method-->POST" + "\n");
                    rtb_result.AppendText("Parameters-->" + parameters + "\n");
                }
            }
        }

        private void AppendExceptionRecursion(Exception exc)
        {
            rtb_result.Text = "";
            while(exc != null)
            {
                rtb_result.AppendText(exc.Message + "\n");
                exc = exc.InnerException;
            }
        }
    }
}
