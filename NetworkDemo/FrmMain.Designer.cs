namespace NetworkDemo
{
    partial class FrmMain
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
            Lgis.LLayerGroup lLayerGroup1 = new Lgis.LLayerGroup();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnOpenShp = new System.Windows.Forms.Button();
            this.btnFullExtent = new System.Windows.Forms.Button();
            this.lblCoordinate = new System.Windows.Forms.Label();
            this.lWindow1 = new Lgis.LWindow();
            this.btnSetFont = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(505, 12);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 1;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnOpenShp
            // 
            this.btnOpenShp.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnOpenShp.Location = new System.Drawing.Point(505, 53);
            this.btnOpenShp.Name = "btnOpenShp";
            this.btnOpenShp.Size = new System.Drawing.Size(75, 41);
            this.btnOpenShp.TabIndex = 2;
            this.btnOpenShp.Text = "Open Shapefile";
            this.btnOpenShp.UseVisualStyleBackColor = true;
            this.btnOpenShp.Click += new System.EventHandler(this.btnOpenShp_Click);
            // 
            // btnFullExtent
            // 
            this.btnFullExtent.Location = new System.Drawing.Point(505, 100);
            this.btnFullExtent.Name = "btnFullExtent";
            this.btnFullExtent.Size = new System.Drawing.Size(75, 39);
            this.btnFullExtent.TabIndex = 3;
            this.btnFullExtent.Text = "Full Extent";
            this.btnFullExtent.UseVisualStyleBackColor = true;
            this.btnFullExtent.Click += new System.EventHandler(this.btnFullExtent_Click);
            // 
            // lblCoordinate
            // 
            this.lblCoordinate.AutoSize = true;
            this.lblCoordinate.Location = new System.Drawing.Point(503, 452);
            this.lblCoordinate.Name = "lblCoordinate";
            this.lblCoordinate.Size = new System.Drawing.Size(41, 12);
            this.lblCoordinate.TabIndex = 4;
            this.lblCoordinate.Text = "label1";
            // 
            // lWindow1
            // 
            this.lWindow1.BackColor = System.Drawing.Color.White;
            this.lWindow1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lLayerGroup1.Name = "NoName";
            this.lWindow1.Layers = lLayerGroup1;
            this.lWindow1.Location = new System.Drawing.Point(0, 0);
            this.lWindow1.Name = "lWindow1";
            this.lWindow1.Scale = 1D;
            this.lWindow1.Size = new System.Drawing.Size(495, 464);
            this.lWindow1.TabIndex = 0;
            this.lWindow1.Load += new System.EventHandler(this.lWindow1_Load);
            this.lWindow1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lWindow1_MouseMove);
            // 
            // btnSetFont
            // 
            this.btnSetFont.Location = new System.Drawing.Point(505, 145);
            this.btnSetFont.Name = "btnSetFont";
            this.btnSetFont.Size = new System.Drawing.Size(75, 23);
            this.btnSetFont.TabIndex = 5;
            this.btnSetFont.Text = "Set Font";
            this.btnSetFont.UseVisualStyleBackColor = true;
            this.btnSetFont.Click += new System.EventHandler(this.btnSetFont_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 476);
            this.Controls.Add(this.btnSetFont);
            this.Controls.Add(this.lblCoordinate);
            this.Controls.Add(this.btnFullExtent);
            this.Controls.Add(this.btnOpenShp);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.lWindow1);
            this.Name = "FrmMain";
            this.Text = "Network Demo";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Lgis.LWindow lWindow1;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnOpenShp;
        private System.Windows.Forms.Button btnFullExtent;
        private System.Windows.Forms.Label lblCoordinate;
        private System.Windows.Forms.Button btnSetFont;
    }
}

