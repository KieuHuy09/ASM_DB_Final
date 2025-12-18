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
    public partial class Sale_Management : Form
    {
        string connectionString = @"Server=DESKTOP-TD5V49V\MSSQLSERVER01; Database=SE08201_Bookstore; Integrated Security=True";
        public Sale_Management()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // Hàm lấy dữ liệu từ SQL đổ vào DataGridView
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Câu lệnh SQL để lấy bảng hóa đơn (thay 'Sales' bằng tên bảng của bạn)
                    string query = "SELECT SaleID, CustomerName, TotalAmount, SaleDate FROM tblSales";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Đổ dữ liệu vào bảng bên phải
                    dgvSales.DataSource = dt;

                    // Chỉnh giao diện bảng cho đẹp
                    dgvSales.Columns["CustomerName"].HeaderText = "Tên Khách Hàng";
                    dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối SQL: " + ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO tblSales (SaleID, CustomerName, TotalAmount, SaleDate) " +
                             "VALUES (@id, @name, @total, @date)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", txtSaleID.Text);
                cmd.Parameters.AddWithValue("@name", txtCustomerName.Text);
                cmd.Parameters.AddWithValue("@total", txtTotal.Text);
                cmd.Parameters.AddWithValue("@date", dtpSaleDate.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Đã lưu vào SQL Server!");
                LoadData(); // Load lại bảng để thấy dòng mới
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        { 
             this.Close();

        }

        //private void textBox1_TextChanged(object sender, EventArgs e)
        // {

        // }
    }
}
