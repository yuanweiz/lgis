namespace GUI
{
    partial class FrmSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFillColor = new System.Windows.Forms.Label();
            this.lblShowFillColor = new System.Windows.Forms.Label();
            this.btnFillColorSelect = new System.Windows.Forms.Button();
            this.btnBoundaryColorSelect = new System.Windows.Forms.Button();
            this.lblShowBoundaryColor = new System.Windows.Forms.Label();
            this.lblBoundaryColor = new System.Windows.Forms.Label();
            this.btnTrackingColorSelect = new System.Windows.Forms.Button();
            this.lblShowTrackingColor = new System.Windows.Forms.Label();
            this.lblTrackingColor = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFillColor
            // 
            this.lblFillColor.AutoSize = true;
            this.lblFillColor.Location = new System.Drawing.Point(13, 14);
            this.lblFillColor.Name = "lblFillColor";
            this.lblFillColor.Size = new System.Drawing.Size(41, 12);
            this.lblFillColor.TabIndex = 0;
            this.lblFillColor.Text = "label1";
            // 
            // lblShowFillColor
            // 
            this.lblShowFillColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblShowFillColor.Location = new System.Drawing.Point(78, 9);
            this.lblShowFillColor.Name = "lblShowFillColor";
            this.lblShowFillColor.Size = new System.Drawing.Size(100, 23);
            this.lblShowFillColor.TabIndex = 1;
            this.lblShowFillColor.Click += new System.EventHandler(this.lblShowFillColor_Click);
            // 
            // btnFillColorSelect
            // 
            this.btnFillColorSelect.Location = new System.Drawing.Point(184, 9);
            this.btnFillColorSelect.Name = "btnFillColorSelect";
            this.btnFillColorSelect.Size = new System.Drawing.Size(75, 23);
            this.btnFillColorSelect.TabIndex = 2;
            this.btnFillColorSelect.Text = "Sellect";
            this.btnFillColorSelect.UseVisualStyleBackColor = true;
            this.btnFillColorSelect.Click += new System.EventHandler(this.btnFillColorSelect_Click);
            // 
            // btnBoundaryColorSelect
            // 
            this.btnBoundaryColorSelect.Location = new System.Drawing.Point(184, 51);
            this.btnBoundaryColorSelect.Name = "btnBoundaryColorSelect";
            this.btnBoundaryColorSelect.Size = new System.Drawing.Size(75, 23);
            this.btnBoundaryColorSelect.TabIndex = 5;
            this.btnBoundaryColorSelect.Text = "Sellect";
            this.btnBoundaryColorSelect.UseVisualStyleBackColor = true;
            // 
            // lblShowBoundaryColor
            // 
            this.lblShowBoundaryColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblShowBoundaryColor.Location = new System.Drawing.Point(78, 51);
            this.lblShowBoundaryColor.Name = "lblShowBoundaryColor";
            this.lblShowBoundaryColor.Size = new System.Drawing.Size(100, 23);
            this.lblShowBoundaryColor.TabIndex = 4;
            // 
            // lblBoundaryColor
            // 
            this.lblBoundaryColor.AutoSize = true;
            this.lblBoundaryColor.Location = new System.Drawing.Point(13, 56);
            this.lblBoundaryColor.Name = "lblBoundaryColor";
            this.lblBoundaryColor.Size = new System.Drawing.Size(41, 12);
            this.lblBoundaryColor.TabIndex = 3;
            this.lblBoundaryColor.Text = "label4";
            // 
            // btnTrackingColorSelect
            // 
            this.btnTrackingColorSelect.Location = new System.Drawing.Point(184, 96);
            this.btnTrackingColorSelect.Name = "btnTrackingColorSelect";
            this.btnTrackingColorSelect.Size = new System.Drawing.Size(75, 23);
            this.btnTrackingColorSelect.TabIndex = 8;
            this.btnTrackingColorSelect.Text = "Sellect";
            this.btnTrackingColorSelect.UseVisualStyleBackColor = true;
            this.btnTrackingColorSelect.Click += new System.EventHandler(this.btnTrackingColorSelect_Click);
            // 
            // lblShowTrackingColor
            // 
            this.lblShowTrackingColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblShowTrackingColor.Location = new System.Drawing.Point(78, 96);
            this.lblShowTrackingColor.Name = "lblShowTrackingColor";
            this.lblShowTrackingColor.Size = new System.Drawing.Size(100, 23);
            this.lblShowTrackingColor.TabIndex = 7;
            // 
            // lblTrackingColor
            // 
            this.lblTrackingColor.AutoSize = true;
            this.lblTrackingColor.Location = new System.Drawing.Point(13, 101);
            this.lblTrackingColor.Name = "lblTrackingColor";
            this.lblTrackingColor.Size = new System.Drawing.Size(41, 12);
            this.lblTrackingColor.TabIndex = 6;
            this.lblTrackingColor.Text = "label6";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(94, 196);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(184, 196);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnTrackingColorSelect);
            this.Controls.Add(this.lblShowTrackingColor);
            this.Controls.Add(this.lblTrackingColor);
            this.Controls.Add(this.btnBoundaryColorSelect);
            this.Controls.Add(this.lblShowBoundaryColor);
            this.Controls.Add(this.lblBoundaryColor);
            this.Controls.Add(this.btnFillColorSelect);
            this.Controls.Add(this.lblShowFillColor);
            this.Controls.Add(this.lblFillColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFillColor;
        private System.Windows.Forms.Label lblShowFillColor;
        private System.Windows.Forms.Button btnFillColorSelect;
        private System.Windows.Forms.Button btnBoundaryColorSelect;
        private System.Windows.Forms.Label lblShowBoundaryColor;
        private System.Windows.Forms.Label lblBoundaryColor;
        private System.Windows.Forms.Button btnTrackingColorSelect;
        private System.Windows.Forms.Label lblShowTrackingColor;
        private System.Windows.Forms.Label lblTrackingColor;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}