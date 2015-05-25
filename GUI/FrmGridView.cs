using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lgis;

namespace GUI
{
    public partial class FrmGridView : Form
    {
        public LDataTable DataTable
        {
            get
            {
                return dataGridView1.DataSource as LDataTable;
            }
            set
            {
                dataGridView1.DataSource = value;
            }
        }
        public FrmGridView()
        {
            InitializeComponent();
        }

        private void FrmGridView_Load(object sender, EventArgs e)
        {
        }
    }
}
