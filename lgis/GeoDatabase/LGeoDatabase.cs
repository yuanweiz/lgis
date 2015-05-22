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
            SQLiteParameter param = new SQLiteParameter();
            LLineseg ls = new LLineseg(new LPoint(1,2),new LPoint(2,3));
            param.Value = (LMapObject)ls;
            param.DbType = DbType.Binary;
            cmd.CommandText = "insert into [metadata] values(\"blob\",(?));";
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
        }

        

        #region Save/Load layertree structure
        public static string LayerInfo (LLayerGroup Layers)
        {
            string layerinfo = "";
            dfs(ref layerinfo, "/g" + Layers.Name , Layers);
            //dfs(ref layerinfo, "", Layers);
            if (layerinfo.Length > 0)
                layerinfo = layerinfo.Substring(0, layerinfo.Length - 1);
            return layerinfo;
        }
        private static void dfs(ref string layerinfo,string prefix, LLayerGroup lg)
        {
            if (lg==null)return;
            //show parent node first
            layerinfo = layerinfo + prefix + "\n";
            for (int i = 0; i < lg.Count; ++i)
            {
                if (lg[i].ObjectType == ObjectType.Layer)
                {
                    LayerType type = ((LLayer)lg[i]).LayerType;
                    if (type == LayerType.Raster)
                        layerinfo = layerinfo + prefix + "/r" + lg[i].Name + "\n";
                    else if (type == LayerType.Vector)
                        layerinfo = layerinfo + prefix + "/v" + lg[i].Name + "\n";
                }
                else
                {
                    dfs(ref layerinfo,
                        prefix + "/g" + lg[i].Name ,
                        (LLayerGroup)lg[i]);
                }
            }
        }

        static int Mycount(string s, char c)
        {
            int cnt=0;
            foreach (char ch in s)
                if (c == ch) cnt++;
            return cnt;
        }
        static LLayerGroup Insert(LLayerGroup lg, string item)
        {
            int i;
            LMapObject mo =new LMapObject();
            for (i = item.Length - 1; i >= 0; --i)
                if (item[i] == '/') break;
            string basename = item.Substring(i+1);
            bool isgroup = false;
            switch (basename[0])
            {
                case 'g':
                    mo = new LLayerGroup();
                    isgroup = true;
                    break;
                case 'v':
                    mo = new LVectorLayer();
                    break;
                case 'r':
                    mo = new LRasterObject();
                    break;
                default:
                    throw new Exception("unrecognized layertype");
            }
            mo.Name = basename.Substring(1);
            lg.Add(mo);
            if (isgroup)
                return (LLayerGroup)mo;
            else return null;
        }
        public static LLayerGroup RebuildLayerTree(string layerinfo)
        {
            /********************************************8
             * there should be at least one layergroup /root
             * suppose one layer item is as below
             * /root/a/b/c
             * if the program goes right, inside stack should be 
             * root, a, b ; current == c, parent == b
            *****************************************
             *in algorithm designing, I should follow some rules,
             * for instance, in this function, I assume :
             * before each loop begins, the current, parent, stack
             * should match the status of LAST iteration
             * 
             * */
            LLayerGroup root = new LLayerGroup();
            Stack<LLayerGroup> stack = new Stack<LLayerGroup>();
            LLayerGroup parent;
            //define root as depth=1; /root/layer1 as depth=2, etc
            int lastdepth = 1;
            string[] items = layerinfo.Split('\n');
            string item = items[0];
            LLayerGroup current;
            root.Name = item.Substring(2);

            //init 
            parent = null;
            current = root;
            //stack.Push(root);
            for (int i = 1; i < items.Length; ++i)
            {
                item = items[i];
                int depth = Mycount(items[i], '/');
                // /a/b and /a/c are in the same level
                if (depth == lastdepth)
                    //parent node remains unchanged
                    current = Insert(parent, item);

                /***************************
                 /a/b -> /a/b/c
                  | |       | |
                  p c       p c p:parent, c:current
                 * **************************/
                else if (depth > lastdepth)
                {
                    lastdepth = depth;
                    stack.Push(parent);
                    //FIXME:Really correct?
                    parent = current;
                    current = Insert(parent, item);
                }
                /***********************************
                 /a/b/c -> /a/d
                    | |     | |
                 *  p c     p c
                ****************************/
                else
                {
                    lastdepth = depth;
                    for (int j = 0; j < lastdepth - depth; ++j)
                    {
                        stack.Pop();
                    }
                    parent = stack.Peek();
                    current = Insert(parent, item);
                }
            }
            return root;
        }

        #endregion

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
                throw new SQLiteException("In InitDbTables() method:"+e.ToString());
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

}
