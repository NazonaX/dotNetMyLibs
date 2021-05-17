using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextCompare
{
    public partial class Form1 : Form
    {
        // 左右两边富文本的内容存储对象
        private RichText LeftRichText = new RichText();
        private RichText RightRichText = new RichText();

        public Form1()
        {
            InitializeComponent();
            InitializeData();
        }

        /// <summary>
        /// 进行左右文本对比
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            string leftLine = string.Empty;
            string rightLine = string.Empty;
            LeftRichText.ResetLineIndex();
            LeftRichBox.Text = "";
            RightRichText.ResetLineIndex();
            RightRichBox.Text = "";
            bool leftConsumed = true;
            bool rightConsumed = true;
            do
            {
                if (leftConsumed)
                {
                    leftLine = LeftRichText.NextLine();
                    leftConsumed = false;
                }
                if (rightConsumed)
                {
                    rightLine = RightRichText.NextLine();
                    rightConsumed = false;
                }
                // 任何一边读完的就出去，剩下的标红
                if (leftLine == null || rightLine == null)
                {
                    break;
                }
                if (leftLine.Trim() == "")
                {
                    LeftRichBox.AppendText(leftLine);
                    LeftRichBox.AppendText("\n");
                    leftConsumed = true;
                }
                if (rightLine.Trim() == "")
                {
                    RightRichBox.AppendText(rightLine);
                    RightRichBox.AppendText("\n");
                    rightConsumed = true;
                }
                // 对两边的非空白文本进行对比
                if (!leftConsumed && !rightConsumed)
                {
                    // 附加空白头
                    string lefttmp = leftLine.Trim();
                    LeftRichBox.AppendText(leftLine.Substring(0, leftLine.IndexOf(lefttmp)));
                    string righttmp = rightLine.Trim();
                    RightRichBox.AppendText(rightLine.Substring(0, rightLine.IndexOf(righttmp)));
                    // 对比正文
                    CompareTwoLines(lefttmp, righttmp);
                    // 附加空白尾
                    LeftRichBox.AppendText(leftLine.Substring(leftLine.IndexOf(lefttmp) + lefttmp.Length) + "\n");
                    RightRichBox.AppendText(rightLine.Substring(rightLine.IndexOf(righttmp) + righttmp.Length) + "\n");
                    leftConsumed = true;
                    rightConsumed = true;
                }
            } while (true);
            // 补齐剩下的信息
            while (leftLine != null)
            {
                LeftRichBox.AppendTextColorful(leftLine, Color.Red);
                leftLine = LeftRichText.NextLine();
            }
            while (rightLine != null)
            {
                RightRichBox.AppendTextColorful(rightLine, Color.Red);
                rightLine = RightRichText.NextLine();
            }
        }

        /// <summary>
        /// 富文本框的拖拽进入事件
        /// </summary>
        private void RichBox_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        /// <summary>
        /// 富文本框的拖拽松放事件
        /// </summary>
        private void RichBox_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            RichTextBox target = sender as RichTextBox;
            string filePath = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            // 读取文本十六进制内容
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            Button btnConvert = null;
            RichText richText = null;
            if (target == LeftRichBox)
            {
                btnConvert = LeftBtnConvert;
                richText = LeftRichText;
            }
            else
            {
                btnConvert = RightBtnConvert;
                richText = RightRichText;
            }
            // 可能会不够需注意，因此只适合小文本
            fs.Read(buffer, 0, (int)fs.Length);
            richText.ByteData = buffer.ToList();
            fs.Close();
            btnConvert.PerformClick();
        }
        /// <summary>
        /// 左侧文本编码转换按钮
        /// </summary>
        private void LeftBtnConvert_Click(object sender, EventArgs e)
        {
            byte[] content = LeftRichText.ByteData.ToArray();
            ListItem li = LeftEncodingBox.SelectedItem as ListItem;
            LeftRichBox.Text = "";
            LeftRichBox.AppendTextColorful(ConvertToUTF8(Encoding.GetEncoding(li.Value), content), Color.White);
            LeftRichText.Lines = LeftRichBox.Text.Split('\n', '\r').ToList();
        }
        /// <summary>
        /// 右侧文本编码转换按钮
        /// </summary>
        private void RightBtnConvert_Click(object sender, EventArgs e)
        {
            byte[] content = RightRichText.ByteData.ToArray();
            ListItem li = RightEncodingBox.SelectedItem as ListItem;
            RightRichBox.Text = "";
            RightRichBox.AppendTextColorful(ConvertToUTF8(Encoding.GetEncoding(li.Value), content), Color.White);
            RightRichText.Lines = RightRichBox.Text.Split('\n', '\r').ToList();
        }

        private string ConvertToUTF8(Encoding encoding, byte[] content)
        {
            return Encoding.UTF8.GetString(Encoding.Convert(encoding, Encoding.UTF8, content));
        }

        /// <summary>
        /// 比较左右两边的内容行，
        /// </summary>
        private void CompareTwoLines(string left, string right)
        {
            int i = 0;
            for (i = 0; i < left.Length && i < right.Length; i++)
            {
                if (left[i] == right[i])
                {
                    LeftRichBox.AppendTextColorful(left[i].ToString(), Color.LightGray);
                    RightRichBox.AppendTextColorful(right[i].ToString(), Color.LightGray);
                }
                else
                {
                    LeftRichBox.AppendTextColorful(left[i].ToString(), Color.Red);
                    RightRichBox.AppendTextColorful(right[i].ToString(), Color.Red);
                }
            }
            for (; i < left.Length; i++)
            {
                LeftRichBox.AppendTextColorful(left[i].ToString(), Color.Red);
            }
            for (; i < right.Length; i++)
            {
                RightRichBox.AppendTextColorful(right[i].ToString(), Color.Red);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string content = LeftRichBox.Text;
            ListItem li = LeftEncodingBox.SelectedItem as ListItem;
            Encoding encoding = Encoding.GetEncoding(li.Value);
            LeftRichText.ByteData = encoding.GetBytes(content).ToList();
            LeftBtnConvert.PerformClick();
        }

        private void RightBtnReadByte_Click(object sender, EventArgs e)
        {
            string content = RightRichBox.Text;
            ListItem li = RightEncodingBox.SelectedItem as ListItem;
            Encoding encoding = Encoding.GetEncoding(li.Value);
            RightRichText.ByteData = encoding.GetBytes(content).ToList();
            RightBtnConvert.PerformClick();
        }
    }
}
