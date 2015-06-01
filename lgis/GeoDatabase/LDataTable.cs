using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public class LDataTable:DataTable
    {
        LLayer Layer;
        internal int maxfid = 0;
        public LDataTable(LLayer Layer)
            : base()
        {
            this.Layer = Layer;
            this.TableNewRow += new DataTableNewRowEventHandler(OnRowAdded);
            Columns.Add(new DataColumn("FID", typeof(int)));
            Columns.Add(new DataColumn("Geometry", typeof(LVectorObject)));
        }
        public void Print()
        {
            foreach (DataRow row in Rows)
            {
                foreach (DataColumn col in Columns)
                {
                    Console.Write(row[col].ToString() + " ");
                }
                Console.WriteLine();
            }
        }
        void OnRowAdded(object sender ,DataTableNewRowEventArgs e)
        {
            e.Row["FID"] = maxfid;
            maxfid++;
        }
        
    }
}
