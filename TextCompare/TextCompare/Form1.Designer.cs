using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextCompare
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LeftRichBox = new System.Windows.Forms.RichTextBox();
            this.RightRichBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.LeftEncodingBox = new System.Windows.Forms.ComboBox();
            this.RightEncodingBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LeftBtnConvert = new System.Windows.Forms.Button();
            this.RightBtnConvert = new System.Windows.Forms.Button();
            this.LeftBtnReadBytes = new System.Windows.Forms.Button();
            this.RightBtnReadByte = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.LeftRichBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.RightRichBox, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 57);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(830, 483);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // LeftRichBox
            // 
            this.LeftRichBox.AllowDrop = true;
            this.LeftRichBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LeftRichBox.Location = new System.Drawing.Point(3, 3);
            this.LeftRichBox.Name = "LeftRichBox";
            this.LeftRichBox.Size = new System.Drawing.Size(394, 477);
            this.LeftRichBox.TabIndex = 0;
            this.LeftRichBox.Text = "";
            // 
            // RightRichBox
            // 
            this.RightRichBox.AllowDrop = true;
            this.RightRichBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RightRichBox.Location = new System.Drawing.Point(433, 3);
            this.RightRichBox.Name = "RightRichBox";
            this.RightRichBox.Size = new System.Drawing.Size(394, 477);
            this.RightRichBox.TabIndex = 1;
            this.RightRichBox.Text = "";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button1.Location = new System.Drawing.Point(392, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Compare";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LeftEncodingBox
            // 
            this.LeftEncodingBox.FormattingEnabled = true;
            this.LeftEncodingBox.Location = new System.Drawing.Point(15, 31);
            this.LeftEncodingBox.Name = "LeftEncodingBox";
            this.LeftEncodingBox.Size = new System.Drawing.Size(121, 20);
            this.LeftEncodingBox.TabIndex = 2;
            // 
            // RightEncodingBox
            // 
            this.RightEncodingBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RightEncodingBox.FormattingEnabled = true;
            this.RightEncodingBox.Location = new System.Drawing.Point(718, 31);
            this.RightEncodingBox.Name = "RightEncodingBox";
            this.RightEncodingBox.Size = new System.Drawing.Size(121, 20);
            this.RightEncodingBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "From Encoding:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(750, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "From Encoding:";
            // 
            // LeftBtnConvert
            // 
            this.LeftBtnConvert.Location = new System.Drawing.Point(142, 29);
            this.LeftBtnConvert.Name = "LeftBtnConvert";
            this.LeftBtnConvert.Size = new System.Drawing.Size(75, 23);
            this.LeftBtnConvert.TabIndex = 6;
            this.LeftBtnConvert.Text = "Convert";
            this.LeftBtnConvert.UseVisualStyleBackColor = true;
            this.LeftBtnConvert.Click += new System.EventHandler(this.LeftBtnConvert_Click);
            // 
            // RightBtnConvert
            // 
            this.RightBtnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RightBtnConvert.Location = new System.Drawing.Point(637, 31);
            this.RightBtnConvert.Name = "RightBtnConvert";
            this.RightBtnConvert.Size = new System.Drawing.Size(75, 23);
            this.RightBtnConvert.TabIndex = 7;
            this.RightBtnConvert.Text = "Convert";
            this.RightBtnConvert.UseVisualStyleBackColor = true;
            this.RightBtnConvert.Click += new System.EventHandler(this.RightBtnConvert_Click);
            // 
            // LeftBtnReadBytes
            // 
            this.LeftBtnReadBytes.Location = new System.Drawing.Point(223, 29);
            this.LeftBtnReadBytes.Name = "LeftBtnReadBytes";
            this.LeftBtnReadBytes.Size = new System.Drawing.Size(75, 23);
            this.LeftBtnReadBytes.TabIndex = 8;
            this.LeftBtnReadBytes.Text = "ReadBytes";
            this.LeftBtnReadBytes.UseVisualStyleBackColor = true;
            this.LeftBtnReadBytes.Click += new System.EventHandler(this.button2_Click);
            // 
            // RightBtnReadByte
            // 
            this.RightBtnReadByte.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RightBtnReadByte.Location = new System.Drawing.Point(556, 31);
            this.RightBtnReadByte.Name = "RightBtnReadByte";
            this.RightBtnReadByte.Size = new System.Drawing.Size(75, 23);
            this.RightBtnReadByte.TabIndex = 9;
            this.RightBtnReadByte.Text = "ReadBytes";
            this.RightBtnReadByte.UseVisualStyleBackColor = true;
            this.RightBtnReadByte.Click += new System.EventHandler(this.RightBtnReadByte_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 552);
            this.Controls.Add(this.RightBtnReadByte);
            this.Controls.Add(this.LeftBtnReadBytes);
            this.Controls.Add(this.RightBtnConvert);
            this.Controls.Add(this.LeftBtnConvert);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RightEncodingBox);
            this.Controls.Add(this.LeftEncodingBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        /// <summary>
        /// 初始化一些初始数据，例如编码的combobox
        /// </summary>
        private void InitializeData()
        {
            string defaultEncoding = Encoding.Default.WebName;
            string ASCIIEncoding = Encoding.ASCII.WebName;
            string bigEndianUnicode = Encoding.BigEndianUnicode.WebName;
            string unicode = Encoding.Unicode.WebName;
            string utf8 = Encoding.UTF8.WebName;
            string utf7 = Encoding.UTF7.WebName;
            string utf32 = Encoding.UTF32.WebName;
            // 初始化一组item controls
            List<ListItem> toAddItems = new List<ListItem>(7);
            toAddItems.Add(new ListItem(defaultEncoding, defaultEncoding));
            toAddItems.Add(new ListItem(ASCIIEncoding, ASCIIEncoding));
            toAddItems.Add(new ListItem(bigEndianUnicode, bigEndianUnicode));
            toAddItems.Add(new ListItem(unicode, unicode));
            toAddItems.Add(new ListItem(utf8, utf8));
            toAddItems.Add(new ListItem(utf7, utf7));
            toAddItems.Add(new ListItem(utf32, utf32));
            // 装载两个combobox
            LeftEncodingBox.Items.AddRange(toAddItems.ToArray());
            RightEncodingBox.Items.AddRange(toAddItems.ToArray());
            LeftEncodingBox.DisplayMember = "Display";
            LeftEncodingBox.ValueMember = "Value";
            RightEncodingBox.DisplayMember = "Display";
            RightEncodingBox.ValueMember = "Value";
            // 初始化默认选择utf-8
            LeftEncodingBox.SelectedIndex = 4;
            RightEncodingBox.SelectedIndex = 4;

            // 自己初始化一些东西
            this.Name = "TextCompare";
            this.Text = "TextCompare";
            this.LeftRichBox.AllowDrop = true;
            this.LeftRichBox.DragEnter += RichBox_DragEnter;
            this.LeftRichBox.DragDrop += RichBox_DragDrop;
            this.RightRichBox.AllowDrop = true;
            this.RightRichBox.DragEnter += RichBox_DragEnter;
            this.RightRichBox.DragDrop += RichBox_DragDrop;
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox LeftRichBox;
        private System.Windows.Forms.RichTextBox RightRichBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox LeftEncodingBox;
        private System.Windows.Forms.ComboBox RightEncodingBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button LeftBtnConvert;
        private System.Windows.Forms.Button RightBtnConvert;
        private System.Windows.Forms.Button LeftBtnReadBytes;
        private System.Windows.Forms.Button RightBtnReadByte;
    }
}

