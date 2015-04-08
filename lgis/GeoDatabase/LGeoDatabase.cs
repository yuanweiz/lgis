using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Lgis
{
    public class LGeoDatabase
    {

        #region methods
        public LGeoDatabase(string filename) {
            conn = new SQLiteConnection();
            cmd = new SQLiteCommand(conn);
            bool fileExists = File.Exists(filename);
            if (!fileExists)
                CreateDatabase(filename);
            try
            {
                connectStr = @"Data Source=" + filename;
                conn.ConnectionString = connectStr;
                conn.Open();
            }
            catch (IOException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            if (!fileExists)
                InitDbTables();
        }
        public void OpenDatabase(string filename){}
        public void SaveDatabase(string filename){}
        /// <summary>
        /// This method will overwrite the file to an empty 0-byte database
        /// </summary>
        /// <param name="filename"></param>
        public static void CreateDatabase(string filename) {
            SQLiteConnection.CreateFile(filename);
            
        }
        void InitDbTables( )
        {
            //SQLiteCommandBuilder cb = new SQLiteCommandBuilder();
            cmd = new SQLiteCommand(conn);
            try
            {
                cmd.Transaction = conn.BeginTransaction();
                cmd.CommandText =
                    @" create table _ras ( name varchar, type varchar );
                create table _vec (name varchar, type varchar );";
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
            }
            catch (Exception e){
                MessageBox.Show(e.ToString());
                cmd.Transaction.Rollback();
            }
        }

        public void AddVectorObject(LVectorObject vo)
        {
            SQLiteCommandBuilder cmdBuilder = new SQLiteCommandBuilder();
            if (vo.Name == null || vo.Name == "")
            {
                throw new IOException("No name is specified");
            }
            cmd.Transaction = conn.BeginTransaction();
            switch (vo.FeatureType)
            {
                case FeatureType.Polygon:
                    cmd.CommandText = @"insert into [_vec] values('" + vo.Name + "','polygon');" +
                        "create table [polygon_" + vo.Name + "](Data BLOB);";
                    break;
                case FeatureType.Point:
                    break;
                case FeatureType.Polyline:
                    break;
                default:
                    throw new IOException("not supported by LGeoDatabase");
            }
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                cmd.Transaction.Rollback();
            }
        }

        public void Close()
        {
            conn.Close();
        }
        #endregion

        #region properties and private fields
        SQLiteConnection conn ;
        SQLiteCommand cmd  ;
        SQLiteDataReader dr;
        SQLiteDataAdapter da;
        string connectStr;

        #endregion

        #region debug

        public void AddBlob()
        {
            cmd.Parameters.Add(new SQLiteParameter(DbType.Binary));
            cmd.Parameters[0].Value = new byte[13];
            cmd.CommandText = "insert into polygon_polygon1 values(?)";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        #endregion
    }

    public class LGeoRecord
    {
        
    }

    public class LGeoRecordSet  // IEnumerable <LGeoRecord>
    {
    }

    public class LWorkSpace
    {
    }
}
        /*
        private void btnConnect_Click(object sender, EventArgs e)
        {
            string cntstr = "Server="+txtHost.Text+
                ";Port="+txtPort.Text+
                ";User Id="+txtUser.Text+
                ";Password="+txtPasswd.Text+
                ";Database="+txtDbname.Text;
            NpgsqlConnection conn = new NpgsqlConnection(cntstr);
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(txtSQL.Text, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                    string o="";
                while (dr.Read())
                {
                    o += dr[0];
                }
                MessageBox.Show(o);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }

        */
