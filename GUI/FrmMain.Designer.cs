namespace GUI
{
    partial class FrmMain
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
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRotate = new System.Windows.Forms.Button();
            this.btnPan = new System.Windows.Forms.Button();
            this.btnGridView = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openShapefileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lLayerComboBox1 = new Lgis.LLayerComboBox();
            this.lLayerView1 = new Lgis.LLayerTreeView();
            this.lWindow1 = new Lgis.LWindow();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Location = new System.Drawing.Point(696, 24);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(75, 23);
            this.btnZoomIn.TabIndex = 1;
            this.btnZoomIn.Text = "Zoom In";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Location = new System.Drawing.Point(696, 63);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(75, 23);
            this.btnZoomOut.TabIndex = 2;
            this.btnZoomOut.Text = "Zoom Out";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomToLayer
            // 
            this.btnZoomToLayer.Location = new System.Drawing.Point(696, 100);
            this.btnZoomToLayer.Name = "btnZoomToLayer";
            this.btnZoomToLayer.Size = new System.Drawing.Size(75, 34);
            this.btnZoomToLayer.TabIndex = 3;
            this.btnZoomToLayer.Text = "Zoom To Layer";
            this.btnZoomToLayer.UseVisualStyleBackColor = true;
            this.btnZoomToLayer.Click += new System.EventHandler(this.btnZoomToLayer_Click);
            // 
            // btnStartEditing
            // 
            this.btnStartEditing.Location = new System.Drawing.Point(696, 169);
            this.btnStartEditing.Name = "btnStartEditing";
            this.btnStartEditing.Size = new System.Drawing.Size(75, 36);
            this.btnStartEditing.TabIndex = 6;
            this.btnStartEditing.Text = "Start Editing";
            this.btnStartEditing.UseVisualStyleBackColor = true;
            this.btnStartEditing.Click += new System.EventHandler(this.btnStartEditting_Click);
            // 
            // btnStopEditing
            // 
            this.btnStopEditing.Location = new System.Drawing.Point(696, 211);
            this.btnStopEditing.Name = "btnStopEditing";
            this.btnStopEditing.Size = new System.Drawing.Size(75, 38);
            this.btnStopEditing.TabIndex = 7;
            this.btnStopEditing.Text = "Stop Editing";
            this.btnStopEditing.UseVisualStyleBackColor = true;
            this.btnStopEditing.Click += new System.EventHandler(this.btnStopEditing_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(696, 267);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRotate
            // 
            this.btnRotate.Location = new System.Drawing.Point(696, 348);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(75, 23);
            this.btnRotate.TabIndex = 12;
            this.btnRotate.Text = "Rotate";
            this.btnRotate.UseVisualStyleBackColor = true;
            this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
            // 
            // btnPan
            // 
            this.btnPan.Location = new System.Drawing.Point(696, 140);
            this.btnPan.Name = "btnPan";
            this.btnPan.Size = new System.Drawing.Size(75, 23);
            this.btnPan.TabIndex = 14;
            this.btnPan.Text = "Pan";
            this.btnPan.UseVisualStyleBackColor = true;
            this.btnPan.Click += new System.EventHandler(this.btnPan_Click);
            // 
            // btnGridView
            // 
            this.btnGridView.Location = new System.Drawing.Point(696, 386);
            this.btnGridView.Name = "btnGridView";
            this.btnGridView.Size = new System.Drawing.Size(75, 23);
            this.btnGridView.TabIndex = 15;
            this.btnGridView.Text = "GridView";
            this.btnGridView.UseVisualStyleBackColor = true;
            this.btnGridView.Click += new System.EventHandler(this.btnGridView_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(774, 25);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openShapefileToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.openToolStripMenuItem.Text = "File";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openShapefileToolStripMenuItem
            // 
            this.openShapefileToolStripMenuItem.Name = "openShapefileToolStripMenuItem";
            this.openShapefileToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.openShapefileToolStripMenuItem.Text = "Open Shapefile";
            this.openShapefileToolStripMenuItem.Click += new System.EventHandler(this.openShapefileToolStripMenuItem_Click);
            // 
            // lLayerComboBox1
            // 
            this.lLayerComboBox1.LabelText = "Active Layer";
            lLayerGroup1.Name = "NoName";
            this.lLayerComboBox1.Layers = lLayerGroup1;
            this.lLayerComboBox1.Location = new System.Drawing.Point(14, 526);
            this.lLayerComboBox1.Name = "lLayerComboBox1";
            this.lLayerComboBox1.Size = new System.Drawing.Size(188, 48);
            this.lLayerComboBox1.TabIndex = 13;
            this.lLayerComboBox1.SelectedItemChanged += new Lgis.LLayerComboBox.SelectedItemChangedHandler(this.lLayerComboBox1_SelectedItemChanged);
            // 
            // lLayerView1
            // 
            lLayerGroup2.Name = "NoName";
            this.lLayerView1.Layers = lLayerGroup2;
            this.lLayerView1.Location = new System.Drawing.Point(12, 24);
            this.lLayerView1.Name = "lLayerView1";
            this.lLayerView1.Size = new System.Drawing.Size(189, 496);
            this.lLayerView1.TabIndex = 11;
            this.lLayerView1.AfterCheck += new Lgis.LLayerTreeView.AfterCheckEventHandler(this.lLayerView1_AfterCheck);
            this.lLayerView1.Load += new System.EventHandler(this.lLayerView1_Load);
            // 
            // lWindow1
            // 
            this.lWindow1.BackColor = System.Drawing.Color.White;
            this.lWindow1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lWindow1.Cursor = System.Windows.Forms.Cursors.Default;
            lLayerGroup3.Name = "NoName";
            this.lWindow1.Layers = lLayerGroup3;
            this.lWindow1.Location = new System.Drawing.Point(207, 24);
            this.lWindow1.Name = "lWindow1";
            this.lWindow1.Scale = 1D;
            this.lWindow1.Size = new System.Drawing.Size(483, 509);
            this.lWindow1.TabIndex = 8;
            this.lWindow1.Load += new System.EventHandler(this.lWindow1_Load);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 586);
            this.Controls.Add(this.btnGridView);
            this.Controls.Add(this.btnPan);
            this.Controls.Add(this.lLayerComboBox1);
            this.Controls.Add(this.btnRotate);
            this.Controls.Add(this.lLayerView1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lWindow1);
            this.Controls.Add(this.btnStopEditing);
            this.Controls.Add(this.btnStartEditing);
            this.Controls.Add(this.btnZoomToLayer);
            this.Controls.Add(this.btnZoomOut);
            this.Controls.Add(this.btnZoomIn);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "bu";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.Button btnClear;
        private Lgis.LLayerTreeView lLayerView1;
        private System.Windows.Forms.Button btnRotate;
        private Lgis.LLayerComboBox lLayerComboBox1;
        private System.Windows.Forms.Button btnPan;
        private System.Windows.Forms.Button btnGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openShapefileToolStripMenuItem;
    }
}

