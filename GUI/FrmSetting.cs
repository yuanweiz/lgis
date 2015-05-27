using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmSetting : Form
    {
        #region properties

        public Color BoundaryColor { get; set; }
        public Color FillColor { get; set; }
        public Color TrackingColor { get; set; }

        #endregion
        public FrmSetting()
        {
            InitializeComponent();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            lblBoundaryColor.BackColor = BoundaryColor;
            lblTrackingColor.BackColor = TrackingColor;
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnFillColorSelect_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = FillColor;
            if (colorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                FillColor = colorDialog.Color;
            }
        }

        private void btnTrackingColorSelect_Click(object sender, EventArgs e)
        {

        }

        private void lblShowFillColor_Click(object sender, EventArgs e)
        {

        }
    }
}
