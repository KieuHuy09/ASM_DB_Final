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
    public partial class Dashboard : Form
    {
        string connectionString = @"Server=DESKTOP-TD5V49V\MSSQLSERVER01; Database=SE08201_Bookstore; Integrated Security=True";
        public Dashboard()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Tính Tổng giá trị kho sách (Total Sale)
                    SqlCommand cmdValue = new SqlCommand("SELECT SUM(Price) FROM Books", conn);
                    object totalValue = cmdValue.ExecuteScalar();
                    txtTotalSale.Text = totalValue != DBNull.Value ? string.Format("{0:N2}", totalValue) : "0";

                    // 2. Lấy tên cuốn sách đắt nhất (Best Selling - Giả định)
                    SqlCommand cmdBest = new SqlCommand("SELECT TOP 1 Title FROM Books ORDER BY Price DESC", conn);
                    object bestProduct = cmdBest.ExecuteScalar();
                    txtBestProduct.Text = bestProduct != null ? bestProduct.ToString() : "N/A";

                    // 3. Đếm tổng số tài khoản (Total Number of Customer)
                    SqlCommand cmdCustomer = new SqlCommand("SELECT COUNT(*) FROM tblAccounts", conn);
                    int customerCount = (int)cmdCustomer.ExecuteScalar();
                    txtTotalCustomer.Text = customerCount.ToString();

                    // 4. Hiển thị danh sách sách lên bảng DataGridView bên dưới
                    SqlDataAdapter da = new SqlDataAdapter("SELECT BookID, Title, Price, PublishYear FROM Books", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDashboard.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Dashboard: " + ex.Message);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Product_Management pm = new Product_Management();

            // Hiển thị Form dưới dạng hội thoại (người dùng xong mới quay lại Dashboard được)
            pm.ShowDialog();

            // Sau khi đóng Form Add, gọi lại hàm nạp dữ liệu để cập nhật số liệu mới nhất
            LoadDashboardData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Product_Management pm = new Product_Management();
            pm.ShowDialog();

            // Cập nhật lại các con số thống kê sau khi có thể đã xóa bớt sách
            LoadDashboardData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 cusForm = new Form1();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }
    }
}
