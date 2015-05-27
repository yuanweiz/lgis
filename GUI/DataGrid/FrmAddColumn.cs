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
    public partial class FrmAddColumn : Form
    {

        public LDataTable DataTable;
        public FrmAddColumn()
        {
            InitializeComponent();
        }

        private void FrmAddColumn_Load(object sender, EventArgs e)
        {

        }

        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AddColumn();
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            AddColumn();
        }
        private bool AddColumn()
        {
            Type t;
            string strtype = cmbDataType.SelectedItem.ToString();
            if (DataTable == null || txtColumnName.Text == "")
                return false;
            switch (strtype){
                case "Double":
                    t = typeof(Double);
                    break;
                case "Integer":
                    t = typeof(int);
                    break;
                case "String":
                    t= typeof(String);
                    break;
                default:
                    return false;
            }
            try
            {
                DataTable.Columns.Add(new DataColumn(txtColumnName.Text, t));
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failure:Exception occurs during AddColumn():" + ex.Message);
                return false;
            }
            return true;
        }
    }
}
