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
        #region properties and private fields
        SQLiteConnection conn ; // once constructed, the connection remains open
        SQLiteCommand cmd  ;
        SQLiteDataReader dr;
        SQLiteDataAdapter da;
        string filename;
        string connectStr;
        public string Description
        {
            get
            {
                return GetMetadata("description");
            }
            set
            {
                ResetMetadata("description", value);
            }
        }

        public LLayerGroup Layers = new LLayerGroup();

        //if a column with key doesn't exist , this method will create one 
        private void ResetMetadata(string key, string value)
        {
            try
            {
                cmd.Transaction = conn.BeginTransaction();
                cmd.CommandText = "delete from [metadata] where key = \"" + key + "\";" +
                    "insert into [metadata] values(\"description\",\"" + value + "\" );";
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
            }
            catch (SQLiteException e)
            {
                cmd.Transaction.Rollback();
                throw e;
            }
        }
        private string GetMetadata(string key )
        {
            string s;
            try
            {
                cmd.Transaction = conn.BeginTransaction();
                cmd.CommandText = "select value from metadata where key=\"" + key+"\";";
                s= (string)cmd.ExecuteScalar();
                cmd.Transaction.Commit();
                return s;
            }
            catch (SQLiteException e)
            {
                cmd.Transaction.Rollback();
                throw e;
            }
        }
        #endregion

        #region methods
        public LGeoDatabase(string filename , bool overwrite = false) {
            this.filename = filename;
            conn = new SQLiteConnection();
            cmd = new SQLiteCommand(conn);
            bool newfile =( !File.Exists(filename) || overwrite);
            try
            {
                if (newfile)
                    SQLiteConnection.CreateFile(filename);
                connectStr = @"Data Source=" + filename;
                conn.ConnectionString = connectStr;
                conn.Open();
                if (newfile)
                    InitDbTables();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            //for new dbfile , init
        }

        public void OpenDatabase(string filename,bool overwrite = false){}

        //even if the file already exists, overwrite it 
        public void SaveDatabase(string filename){
            
        }
        //TODO
        private void RestoreLayer (string layerstr){
            LLayerGroup pwd, oldpwd;
            pwd = oldpwd = Layers;
            string[] layernames = layerstr.Split(';');
            foreach (string layername in layernames)
            {

            }
        }

        /// <summary>
        /// This method will overwrite the file to an empty 0-byte database
        /// </summary>
        /// <param name="filename"></param>
        void InitDbTables( )
        {
            try
            {
                cmd.Transaction = conn.BeginTransaction();
                cmd.CommandText =
                    " create table [layers] ( name varchar, type varchar );" +
                "create table [metadata] (key varchar, value varchar );";
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                ResetMetadata("Description", "default");
            }
            catch (SQLiteException e)
            {
                cmd.Transaction.Rollback();
                throw new SQLiteException("In InitDbTables() method");
            }
        }
        public void Close()
        {
            conn.Close();
        }
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
