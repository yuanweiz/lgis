namespace GUI
{
    partial class FrmQuery
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtThresValue = new System.Windows.Forms.TextBox();
            this.cmbQueryType = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblQueryField = new System.Windows.Forms.Label();
            this.cmbQueryField = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "Threshold Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "Query Type";
            // 
            // txtThresValue
            // 
            this.txtThresValue.Location = new System.Drawing.Point(24, 142);
            this.txtThresValue.Name = "txtThresValue";
            this.txtThresValue.Size = new System.Drawing.Size(182, 21);
            this.txtThresValue.TabIndex = 10;
            // 
            // cmbQueryType
            // 
            this.cmbQueryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQueryType.FormattingEnabled = true;
            this.cmbQueryType.Items.AddRange(new object[] {
            "EqualTo",
            "LessThan",
            "LargerThan"});
            this.cmbQueryType.Location = new System.Drawing.Point(24, 88);
            this.cmbQueryType.Name = "cmbQueryType";
            this.cmbQueryType.Size = new System.Drawing.Size(182, 20);
            this.cmbQueryType.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(105, 190);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(24, 190);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblQueryField
            // 
            this.lblQueryField.AutoSize = true;
            this.lblQueryField.Location = new System.Drawing.Point(22, 21);
            this.lblQueryField.Name = "lblQueryField";
            this.lblQueryField.Size = new System.Drawing.Size(71, 12);
            this.lblQueryField.TabIndex = 14;
            this.lblQueryField.Text = "Query Field";
            // 
            // cmbQueryField
            // 
            this.cmbQueryField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQueryField.FormattingEnabled = true;
            this.cmbQueryField.Items.AddRange(new object[] {
            "EqualTo",
            "LessThan",
            "LargerThan"});
            this.cmbQueryField.Location = new System.Drawing.Point(24, 36);
            this.cmbQueryField.Name = "cmbQueryField";
            this.cmbQueryField.Size = new System.Drawing.Size(182, 20);
            this.cmbQueryField.TabIndex = 15;
            this.cmbQueryField.SelectedIndexChanged += new System.EventHandler(this.cmbQueryField_SelectedIndexChanged);
            // 
            // FrmQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.cmbQueryField);
            this.Controls.Add(this.lblQueryField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtThresValue);
            this.Controls.Add(this.cmbQueryType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "FrmQuery";
            this.Text = "Query";
            this.Load += new System.EventHandler(this.FrmNumericQuery_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtThresValue;
        private System.Windows.Forms.ComboBox cmbQueryType;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblQueryField;
        private System.Windows.Forms.ComboBox cmbQueryField;
    }
}