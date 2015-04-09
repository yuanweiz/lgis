using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lgis
{
    public partial class LLayerView : UserControl
    {
        #region Properties
        public LLayerGroup Layers = new LLayerGroup();
        #endregion
        public LLayerView()
        {
            InitializeComponent();
        }

        private void LLayerView_Load(object sender, EventArgs e)
        {

        }
    }
}
