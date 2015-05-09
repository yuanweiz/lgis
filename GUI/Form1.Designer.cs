namespace GUI
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            Lgis.LLayerGroup lLayerGroup1 = new Lgis.LLayerGroup();
            Lgis.LLayerGroup lLayerGroup2 = new Lgis.LLayerGroup();
            Lgis.LLayerGroup lLayerGroup3 = new Lgis.LLayerGroup();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomToLayer = new System.Windows.Forms.Button();
            this.btnStartEditing = new System.Windows.Forms.Button();
            this.btnStopEditing = new System.Windows.Forms.Button();
            this.lblCoordinate = new System.Windows.Forms.Label();
            this.lblScale = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnRotate = new System.Windows.Forms.Button();
            this.lLayerComboBox1 = new Lgis.LLayerComboBox();
            this.lLayerView1 = new Lgis.LLayerTreeView();
            this.lWindow1 = new Lgis.LWindow();
            this.SuspendLayout();
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Location = new System.Drawing.Point(515, 24);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(75, 23);
            this.btnZoomIn.TabIndex = 1;
            this.btnZoomIn.Text = "Zoom In";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Location = new System.Drawing.Point(515, 63);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(75, 23);
            this.btnZoomOut.TabIndex = 2;
            this.btnZoomOut.Text = "Zoom Out";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomToLayer
            // 
            this.btnZoomToLayer.Location = new System.Drawing.Point(515, 107);
            this.btnZoomToLayer.Name = "btnZoomToLayer";
            this.btnZoomToLayer.Size = new System.Drawing.Size(75, 34);
            this.btnZoomToLayer.TabIndex = 3;
            this.btnZoomToLayer.Text = "Zoom To Layer";
            this.btnZoomToLayer.UseVisualStyleBackColor = true;
            this.btnZoomToLayer.Click += new System.EventHandler(this.btnZoomToLayer_Click);
            // 
            // btnStartEditing
            // 
            this.btnStartEditing.Location = new System.Drawing.Point(515, 160);
            this.btnStartEditing.Name = "btnStartEditing";
            this.btnStartEditing.Size = new System.Drawing.Size(75, 36);
            this.btnStartEditing.TabIndex = 6;
            this.btnStartEditing.Text = "Start Editing";
            this.btnStartEditing.UseVisualStyleBackColor = true;
            this.btnStartEditing.Click += new System.EventHandler(this.btnStartEditting_Click);
            // 
            // btnStopEditing
            // 
            this.btnStopEditing.Location = new System.Drawing.Point(515, 211);
            this.btnStopEditing.Name = "btnStopEditing";
            this.btnStopEditing.Size = new System.Drawing.Size(75, 38);
            this.btnStopEditing.TabIndex = 7;
            this.btnStopEditing.Text = "Stop Editing";
            this.btnStopEditing.UseVisualStyleBackColor = true;
            this.btnStopEditing.Click += new System.EventHandler(this.btnStopEditing_Click);
            // 
            // lblCoordinate
            // 
            this.lblCoordinate.AutoSize = true;
            this.lblCoordinate.Location = new System.Drawing.Point(157, 400);
            this.lblCoordinate.Name = "lblCoordinate";
            this.lblCoordinate.Size = new System.Drawing.Size(41, 12);
            this.lblCoordinate.TabIndex = 5;
            this.lblCoordinate.Text = "label1";
            this.lblCoordinate.Click += new System.EventHandler(this.lblCoordinate_Click);
            this.lblCoordinate.Paint += new System.Windows.Forms.PaintEventHandler(this.lblCoordinate_Paint);
            // 
            // lblScale
            // 
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new System.Drawing.Point(12, 400);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(41, 12);
            this.lblScale.TabIndex = 4;
            this.lblScale.Text = "Scale:";
            this.lblScale.Click += new System.EventHandler(this.lblScale_Click);
            this.lblScale.Paint += new System.Windows.Forms.PaintEventHandler(this.lblScale_Paint);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(515, 267);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(515, 306);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(75, 23);
            this.btnSetting.TabIndex = 10;
            this.btnSetting.Text = "Settings";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnRotate
            // 
            this.btnRotate.Location = new System.Drawing.Point(515, 348);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(75, 23);
            this.btnRotate.TabIndex = 12;
            this.btnRotate.Text = "Rotate";
            this.btnRotate.UseVisualStyleBackColor = true;
            this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
            // 
            // lLayerComboBox1
            // 
            this.lLayerComboBox1.Layers = lLayerGroup1;
            this.lLayerComboBox1.Location = new System.Drawing.Point(32, 341);
            this.lLayerComboBox1.Name = "lLayerComboBox1";
            this.lLayerComboBox1.Size = new System.Drawing.Size(143, 37);
            this.lLayerComboBox1.TabIndex = 13;
            this.lLayerComboBox1.SelectedItemChanged += new Lgis.LLayerComboBox.SelectedItemChangedHandler(this.lLayerComboBox1_SelectedItemChanged);
            // 
            // lLayerView1
            // 
            this.lLayerView1.Layers = lLayerGroup2;
            this.lLayerView1.Location = new System.Drawing.Point(12, 24);
            this.lLayerView1.Name = "lLayerView1";
            this.lLayerView1.Size = new System.Drawing.Size(189, 295);
            this.lLayerView1.TabIndex = 11;
            this.lLayerView1.Load += new System.EventHandler(this.lLayerView1_Load);
            this.lLayerView1.AfterCheck += new Lgis.LLayerTreeView.AfterCheckEventHandler(this.lLayerView1_AfterCheck);
            // 
            // lWindow1
            // 
            this.lWindow1.BackColor = System.Drawing.Color.White;
            this.lWindow1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lWindow1.Cursor = System.Windows.Forms.Cursors.Default;
            this.lWindow1.Layers = lLayerGroup3;
            this.lWindow1.Location = new System.Drawing.Point(211, 12);
            this.lWindow1.Name = "lWindow1";
            this.lWindow1.Scale = 1D;
            this.lWindow1.Size = new System.Drawing.Size(298, 359);
            this.lWindow1.TabIndex = 8;
            this.lWindow1.Load += new System.EventHandler(this.lWindow1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 413);
            this.Controls.Add(this.lLayerComboBox1);
            this.Controls.Add(this.btnRotate);
            this.Controls.Add(this.lLayerView1);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lWindow1);
            this.Controls.Add(this.btnStopEditing);
            this.Controls.Add(this.btnStartEditing);
            this.Controls.Add(this.lblCoordinate);
            this.Controls.Add(this.lblScale);
            this.Controls.Add(this.btnZoomToLayer);
            this.Controls.Add(this.btnZoomOut);
            this.Controls.Add(this.btnZoomIn);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomToLayer;
        private System.Windows.Forms.Button btnStartEditing;
        private System.Windows.Forms.Button btnStopEditing;
        private Lgis.LWindow lWindow1;
        private System.Windows.Forms.Label lblCoordinate;
        private System.Windows.Forms.Label lblScale;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSetting;
        private Lgis.LLayerTreeView lLayerView1;
        private System.Windows.Forms.Button btnRotate;
        private Lgis.LLayerComboBox lLayerComboBox1;
    }
}

