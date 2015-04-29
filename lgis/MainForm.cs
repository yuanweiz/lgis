#define DEBUG
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Npgsql;

namespace Lgis
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
#if DEBUG
            btnConnect.Enabled = false;
#endif
            txtHost.Text = "127.0.0.1";
            txtPort.Text = "5432";
            txtUser.Text = "ywz";
            txtPasswd.Text = "668877";
            txtPasswd.PasswordChar = '*';
            txtDbname.Text = "student";
            txtSQL.Text = "select * from student;";
        }
        public void Connect(){
            //string connectStr = "Server=127.0.0.1;Port=5432;User Id=joe;Password=secret;Database=joedata;";

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

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

        private void txtHost_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDbname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSQL_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
