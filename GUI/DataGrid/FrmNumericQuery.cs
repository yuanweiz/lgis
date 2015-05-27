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
    public partial class FrmNumericQuery : Form
    {
        public LDataTable DataTable;
        public DataColumn Column;
        public IEnumerable<DataRow> RecordSet;
        public FrmNumericQuery()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            double value;
            LNumericDataFilter filter = new LNumericDataFilter();

            try
            {
                value = Convert.ToDouble(txtThresValue.Text);
                filter.value = value ;
                filter.Column = Column;
                if (Column == null || value == null)
                    throw new NullReferenceException("Column or threshold not specified");
                if (Column.DataType.GetInterface("IComparable") == null || value as IComparable == null)
                    throw new LTypeMismatchException("Not Comparable value type");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            switch (cmbQueryType.SelectedIndex){
                case 0:
                    filter.CompareType = LNumericDataFilter.FilterType.EqualTo;
                    break;
                case 1:
                    filter.CompareType = LNumericDataFilter.FilterType.LessThan;
                    break;
                case 2:
                    filter.CompareType = LNumericDataFilter.FilterType.LargerThan;
                    break;
                default:
                    return;
            }
            RecordSet = filter.Filter(DataTable.AsEnumerable());
        }

        private void FrmNumericQuery_Load(object sender, EventArgs e)
        {

        }
    }
}
