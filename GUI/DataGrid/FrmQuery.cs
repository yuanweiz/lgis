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

    //Currently this program only supports the query on a single column(field)
    public partial class FrmQuery : Form
    {
        public LDataTable DataTable;

        public DataColumn Column;

        //FIXME:Bad design but I have no Idea
        public FrmMain frmMain;

        public FrmQuery(LDataTable t)
        {
            InitializeComponent();
            this.DataTable = t;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            double value;
            if (cmbQueryType.Items.Count > 1)//numeric
            {
                LNumericDataFilter numFilter = new LNumericDataFilter();
                try
                {
                    value = Convert.ToDouble(txtThresValue.Text);
                    numFilter.value = value;
                    numFilter.Column = Column;
                    if (Column == null)
                        throw new NullReferenceException("Column or threshold not specified");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                switch (cmbQueryType.SelectedIndex)
                {
                    case 0:
                        numFilter.CompareType = LNumericDataFilter.FilterType.EqualTo;
                        break;
                    case 1:
                        numFilter.CompareType = LNumericDataFilter.FilterType.LessThan;
                        break;
                    case 2:
                        numFilter.CompareType = LNumericDataFilter.FilterType.LargerThan;
                        break;
                    default:
                        return;
                }
                frmMain.RecordSet = numFilter.Filter(DataTable.AsEnumerable());
            }
            else
            {
                LStringDataFilter strFilter = new LStringDataFilter(txtThresValue.Text);
                strFilter.Column = Column;
                frmMain.RecordSet = strFilter.Filter(DataTable.AsEnumerable());
            }
            foreach (DataRow row in frmMain.RecordSet)
            {
                foreach (DataColumn col in DataTable.Columns)
                    Console.Write(row[col].ToString() + "");
                Console.WriteLine();
            }
        }

        private void FrmNumericQuery_Load(object sender, EventArgs e)
        {
            //Load Queryable fields
            cmbQueryField.Items.Clear();
            cmbQueryField.BeginUpdate();
            for (int i = 2; i < DataTable.Columns.Count; ++i)
            {
                cmbQueryField.Items.Add(DataTable.Columns[i].ColumnName);
            }
            cmbQueryField.EndUpdate();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbQueryField_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataColumn col = DataTable.Columns[cmbQueryField.SelectedIndex + 2];
            if (col.DataType == typeof(String))
            {
                ChangeCmbQueryType(false);
            }
            else
                ChangeCmbQueryType(true);
        }

        void ChangeCmbQueryType(bool numeric)
        {
            cmbQueryType.BeginUpdate();
            cmbQueryType.Items.Clear();
            cmbQueryType.Items.Add("EqualTo");
            if (numeric)
            {
                cmbQueryType.Items.AddRange(new object[] { "LessThan", "LargerThan" });
            }
            cmbQueryType.EndUpdate();
        }

    }
}
