namespace Lgis
{
    partial class MainForm
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPasswd = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPasswd = new System.Windows.Forms.TextBox();
            this.txtDbname = new System.Windows.Forms.TextBox();
            this.txtSQL = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(189, 290);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(189, 55);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(40, 21);
            this.txtPort.TabIndex = 2;
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(52, 55);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(113, 21);
            this.txtHost.TabIndex = 1;
            this.txtHost.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(50, 40);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(29, 12);
            this.lblHost.TabIndex = 3;
            this.lblHost.Text = "Host";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(187, 40);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 12);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "port";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(50, 106);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(29, 12);
            this.lblUser.TabIndex = 5;
            this.lblUser.Text = "user";
            // 
            // lblPasswd
            // 
            this.lblPasswd.AutoSize = true;
            this.lblPasswd.Location = new System.Drawing.Point(176, 106);
            this.lblPasswd.Name = "lblPasswd";
            this.lblPasswd.Size = new System.Drawing.Size(53, 12);
            this.lblPasswd.TabIndex = 6;
            this.lblPasswd.Text = "password";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(52, 126);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(100, 21);
            this.txtUser.TabIndex = 7;
            // 
            // txtPasswd
            // 
            this.txtPasswd.Location = new System.Drawing.Point(172, 126);
            this.txtPasswd.Name = "txtPasswd";
            this.txtPasswd.Size = new System.Drawing.Size(100, 21);
            this.txtPasswd.TabIndex = 8;
            // 
            // txtDbname
            // 
            this.txtDbname.Location = new System.Drawing.Point(52, 179);
            this.txtDbname.Name = "txtDbname";
            this.txtDbname.Size = new System.Drawing.Size(100, 21);
            this.txtDbname.TabIndex = 9;
            this.txtDbname.TextChanged += new System.EventHandler(this.txtDbname_TextChanged);
            // 
            // txtSQL
            // 
            this.txtSQL.Location = new System.Drawing.Point(178, 179);
            this.txtSQL.Multiline = true;
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.Size = new System.Drawing.Size(225, 105);
            this.txtSQL.TabIndex = 10;
            this.txtSQL.TextChanged += new System.EventHandler(this.txtSQL_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 325);
            this.Controls.Add(this.txtSQL);
            this.Controls.Add(this.txtDbname);
            this.Controls.Add(this.txtPasswd);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lblPasswd);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblHost);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.btnConnect);
            this.Name = "MainForm";
            this.Text = "LGIS";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPasswd;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPasswd;
        private System.Windows.Forms.TextBox txtDbname;
        private System.Windows.Forms.TextBox txtSQL;
    }
}

