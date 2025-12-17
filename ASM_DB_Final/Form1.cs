using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_DB_Final
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        public Form1()
        {
            InitializeComponent();
           
            conn = new SqlConnection(@"Server=DESKTOP-TD5V49V\MSSQLSERVER01; Database=SE08201_Bookstore; Integrated Security=True");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do u want to exit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
                //Environment.Exit(0);
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            conn.Open();
            //string username = txtUsername.Text;
            //string password = txtPassword.Text;
            string query = "select * from tblAccounts where username = @u and user_password = @p";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.Add("@u", SqlDbType.VarChar);
            cmd.Parameters["@u"].Value = txtUsername.Text;

            cmd.Parameters.Add("@p", SqlDbType.VarChar);
            cmd.Parameters["@p"].Value = txtPassword.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read() == true)
            {
                string role = reader["user_role"].ToString();

                if (role == "admin")
                {
                    MessageBox.Show(this, "Welcome Admin", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    global::AdminDashboard adminDashboard = new global::AdminDashboard();
                    adminDashboard.ShowDialog();
                    this.Dispose();
                }

                if (role == "user")
                {
                    MessageBox.Show(this, "Welcome Sales staff", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Dashboard saleDashboard = new Dashboard();
                    saleDashboard.ShowDialog();
                    this.Dispose();
                }

                if (role == "test")
                {
                    MessageBox.Show(this, "Welcome Warehouse staff", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    WarehouseDashboard warehouseDashboard = new WarehouseDashboard();
                    warehouseDashboard.ShowDialog();
                    this.Dispose();
                }
            }
            conn.Close();
        }
    }
}

internal class AdminDashboard
{
    internal void Show()
    {
        throw new NotImplementedException();
    }

    internal void ShowDialog()
    {
        throw new NotImplementedException();
    }
}
