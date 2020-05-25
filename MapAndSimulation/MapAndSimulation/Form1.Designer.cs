namespace MapAndSimulation
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
            this.LayerControlPanel = new System.Windows.Forms.TabControl();
            this.MsgWindow = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.GoodName = new System.Windows.Forms.TextBox();
            this.GoodOrderNo = new System.Windows.Forms.TextBox();
            this.GoodSpecification = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Mode_Select = new System.Windows.Forms.RadioButton();
            this.Mode_Add = new System.Windows.Forms.RadioButton();
            this.Mode_Delete = new System.Windows.Forms.RadioButton();
            this.Btn_Evalutate = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Txt_NumOfAGVs = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LayerControlPanel
            // 
            this.LayerControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerControlPanel.Location = new System.Drawing.Point(1, 2);
            this.LayerControlPanel.Name = "LayerControlPanel";
            this.LayerControlPanel.SelectedIndex = 0;
            this.LayerControlPanel.Size = new System.Drawing.Size(904, 419);
            this.LayerControlPanel.TabIndex = 1;
            // 
            // MsgWindow
            // 
            this.MsgWindow.Location = new System.Drawing.Point(1, 427);
            this.MsgWindow.Name = "MsgWindow";
            this.MsgWindow.Size = new System.Drawing.Size(478, 166);
            this.MsgWindow.TabIndex = 2;
            this.MsgWindow.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(485, 430);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Good\'s Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(485, 457);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Good\'s Order No.:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(485, 483);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Good\'s Specification:";
            // 
            // GoodName
            // 
            this.GoodName.Location = new System.Drawing.Point(568, 427);
            this.GoodName.Name = "GoodName";
            this.GoodName.Size = new System.Drawing.Size(220, 21);
            this.GoodName.TabIndex = 6;
            // 
            // GoodOrderNo
            // 
            this.GoodOrderNo.Location = new System.Drawing.Point(598, 454);
            this.GoodOrderNo.Name = "GoodOrderNo";
            this.GoodOrderNo.Size = new System.Drawing.Size(190, 21);
            this.GoodOrderNo.TabIndex = 7;
            // 
            // GoodSpecification
            // 
            this.GoodSpecification.Location = new System.Drawing.Point(622, 480);
            this.GoodSpecification.Name = "GoodSpecification";
            this.GoodSpecification.Size = new System.Drawing.Size(166, 21);
            this.GoodSpecification.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(485, 504);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Mode";
            // 
            // Mode_Select
            // 
            this.Mode_Select.AutoSize = true;
            this.Mode_Select.Checked = true;
            this.Mode_Select.Location = new System.Drawing.Point(520, 502);
            this.Mode_Select.Name = "Mode_Select";
            this.Mode_Select.Size = new System.Drawing.Size(59, 16);
            this.Mode_Select.TabIndex = 10;
            this.Mode_Select.TabStop = true;
            this.Mode_Select.Text = "Select";
            this.Mode_Select.UseVisualStyleBackColor = true;
            // 
            // Mode_Add
            // 
            this.Mode_Add.AutoSize = true;
            this.Mode_Add.Location = new System.Drawing.Point(585, 502);
            this.Mode_Add.Name = "Mode_Add";
            this.Mode_Add.Size = new System.Drawing.Size(41, 16);
            this.Mode_Add.TabIndex = 11;
            this.Mode_Add.TabStop = true;
            this.Mode_Add.Text = "Add";
            this.Mode_Add.UseVisualStyleBackColor = true;
            // 
            // Mode_Delete
            // 
            this.Mode_Delete.AutoSize = true;
            this.Mode_Delete.Location = new System.Drawing.Point(632, 502);
            this.Mode_Delete.Name = "Mode_Delete";
            this.Mode_Delete.Size = new System.Drawing.Size(59, 16);
            this.Mode_Delete.TabIndex = 12;
            this.Mode_Delete.TabStop = true;
            this.Mode_Delete.Text = "Delete";
            this.Mode_Delete.UseVisualStyleBackColor = true;
            // 
            // Btn_Evalutate
            // 
            this.Btn_Evalutate.Location = new System.Drawing.Point(485, 524);
            this.Btn_Evalutate.Name = "Btn_Evalutate";
            this.Btn_Evalutate.Size = new System.Drawing.Size(75, 23);
            this.Btn_Evalutate.TabIndex = 13;
            this.Btn_Evalutate.Text = "StartTest";
            this.Btn_Evalutate.UseVisualStyleBackColor = true;
            this.Btn_Evalutate.Click += new System.EventHandler(this.Btn_Evalutate_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(566, 529);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "with num of AGVs";
            // 
            // Txt_NumOfAGVs
            // 
            this.Txt_NumOfAGVs.Location = new System.Drawing.Point(673, 526);
            this.Txt_NumOfAGVs.Name = "Txt_NumOfAGVs";
            this.Txt_NumOfAGVs.Size = new System.Drawing.Size(115, 21);
            this.Txt_NumOfAGVs.TabIndex = 15;
            this.Txt_NumOfAGVs.Text = "4";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 600);
            this.Controls.Add(this.Txt_NumOfAGVs);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Btn_Evalutate);
            this.Controls.Add(this.Mode_Delete);
            this.Controls.Add(this.Mode_Add);
            this.Controls.Add(this.Mode_Select);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.GoodSpecification);
            this.Controls.Add(this.GoodOrderNo);
            this.Controls.Add(this.GoodName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MsgWindow);
            this.Controls.Add(this.LayerControlPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl LayerControlPanel;
        private System.Windows.Forms.RichTextBox MsgWindow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox GoodName;
        private System.Windows.Forms.TextBox GoodOrderNo;
        private System.Windows.Forms.TextBox GoodSpecification;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton Mode_Select;
        private System.Windows.Forms.RadioButton Mode_Add;
        private System.Windows.Forms.RadioButton Mode_Delete;
        private System.Windows.Forms.Button Btn_Evalutate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Txt_NumOfAGVs;
    }
}

