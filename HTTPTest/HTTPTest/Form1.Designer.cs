namespace HTTPTest
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
        //private void InitializeComponent(System.Collections.Generic.List<System.Windows.Forms.TextBox> names,
        //    System.Collections.Generic.List<System.Windows.Forms.TextBox> values)
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tb_domain = new System.Windows.Forms.TextBox();
            this.rb_get = new System.Windows.Forms.RadioButton();
            this.rb_post = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bt_addp = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flp_paras = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_name1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_value1 = new System.Windows.Forms.TextBox();
            this.bt_send = new System.Windows.Forms.Button();
            this.rtb_result = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flp_paras.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Domain Or IP:Port";
            // 
            // tb_domain
            // 
            this.tb_domain.Location = new System.Drawing.Point(13, 29);
            this.tb_domain.Name = "tb_domain";
            this.tb_domain.Size = new System.Drawing.Size(344, 21);
            this.tb_domain.TabIndex = 1;
            // 
            // rb_get
            // 
            this.rb_get.AutoSize = true;
            this.rb_get.BackColor = System.Drawing.SystemColors.Control;
            this.rb_get.Checked = true;
            this.rb_get.Location = new System.Drawing.Point(6, 20);
            this.rb_get.Name = "rb_get";
            this.rb_get.Size = new System.Drawing.Size(41, 16);
            this.rb_get.TabIndex = 2;
            this.rb_get.TabStop = true;
            this.rb_get.Text = "Get";
            this.rb_get.UseVisualStyleBackColor = false;
            // 
            // rb_post
            // 
            this.rb_post.AutoSize = true;
            this.rb_post.Location = new System.Drawing.Point(58, 20);
            this.rb_post.Name = "rb_post";
            this.rb_post.Size = new System.Drawing.Size(47, 16);
            this.rb_post.TabIndex = 3;
            this.rb_post.Text = "Post";
            this.rb_post.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.rb_get);
            this.groupBox1.Controls.Add(this.rb_post);
            this.groupBox1.Location = new System.Drawing.Point(15, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 56);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select your mehtod";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.bt_addp);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Location = new System.Drawing.Point(15, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(349, 221);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Set your parameters";
            // 
            // bt_addp
            // 
            this.bt_addp.Location = new System.Drawing.Point(6, 20);
            this.bt_addp.Name = "bt_addp";
            this.bt_addp.Size = new System.Drawing.Size(337, 23);
            this.bt_addp.TabIndex = 1;
            this.bt_addp.Text = "Add Parameters";
            this.bt_addp.UseVisualStyleBackColor = true;
            this.bt_addp.Click += new System.EventHandler(this.bt_addp_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.flp_paras);
            this.panel1.Location = new System.Drawing.Point(7, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 161);
            this.panel1.TabIndex = 0;
            // 
            // flp_paras
            // 
            this.flp_paras.AutoScroll = true;
            this.flp_paras.Controls.Add(this.label2);
            this.flp_paras.Controls.Add(this.tb_name1);
            this.flp_paras.Controls.Add(this.label3);
            this.flp_paras.Controls.Add(this.tb_value1);
            this.flp_paras.Location = new System.Drawing.Point(-1, 4);
            this.flp_paras.Name = "flp_paras";
            this.flp_paras.Size = new System.Drawing.Size(336, 157);
            this.flp_paras.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name:";
            // 
            // tb_name1
            // 
            this.tb_name1.Location = new System.Drawing.Point(57, 3);
            this.tb_name1.Name = "tb_name1";
            this.tb_name1.Size = new System.Drawing.Size(100, 21);
            this.tb_name1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(163, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Value:";
            // 
            // tb_value1
            // 
            this.tb_value1.Location = new System.Drawing.Point(225, 3);
            this.tb_value1.Name = "tb_value1";
            this.tb_value1.Size = new System.Drawing.Size(100, 21);
            this.tb_value1.TabIndex = 3;
            // 
            // bt_send
            // 
            this.bt_send.Location = new System.Drawing.Point(364, 13);
            this.bt_send.Name = "bt_send";
            this.bt_send.Size = new System.Drawing.Size(75, 23);
            this.bt_send.TabIndex = 6;
            this.bt_send.Text = "Send";
            this.bt_send.UseVisualStyleBackColor = true;
            this.bt_send.Click += new System.EventHandler(this.bt_send_Click);
            // 
            // rtb_result
            // 
            this.rtb_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_result.Location = new System.Drawing.Point(364, 43);
            this.rtb_result.Name = "rtb_result";
            this.rtb_result.Size = new System.Drawing.Size(343, 277);
            this.rtb_result.TabIndex = 7;
            this.rtb_result.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 332);
            this.Controls.Add(this.rtb_result);
            this.Controls.Add(this.bt_send);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tb_domain);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.flp_paras.ResumeLayout(false);
            this.flp_paras.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_domain;
        private System.Windows.Forms.RadioButton rb_get;
        private System.Windows.Forms.RadioButton rb_post;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bt_send;
        private System.Windows.Forms.RichTextBox rtb_result;
        private System.Windows.Forms.Button bt_addp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flp_paras;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_name1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_value1;
    }
}

