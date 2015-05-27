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
                return gridView.DataSource as LDataTable;
            }
            set
            {
                gridView.DataSource = value;
            }
        }
        
        public FrmGridView()
        {
            InitializeComponent();
        }

        private void FrmGridView_Load(object sender, EventArgs e)
        {
            gridView.AllowUserToAddRows = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            if (gridView.DataSource == null)
                return;
            gridView.Columns["Geometry"].ReadOnly = true;
            gridView.Columns["FID"].ReadOnly = true;
        }

        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            FrmAddColumn frmAddColumn = new FrmAddColumn();
            frmAddColumn.DataTable = this.DataTable;
            frmAddColumn.ShowDialog(this);
            gridView.Refresh();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (gridView.SelectedCells.Count != 1)
            {
                MessageBox.Show("Please Select one cell at a certain column");
                return;
            }
            int idx =  gridView.SelectedCells[0].ColumnIndex;
            DataColumn col = DataTable.Columns[idx];
            if (col.DataType.GetInterface("IComparable") != null)
            {
                FrmNumericQuery frmNumericQuery = new FrmNumericQuery();
                frmNumericQuery.Column = col;
                frmNumericQuery.DataTable = DataTable;
                frmNumericQuery.ShowDialog(this);
            }
        }
    }
}
