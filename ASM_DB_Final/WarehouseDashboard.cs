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
    public partial class WarehouseDashboard : Form
    {
        string connectionString = @"Server=DESKTOP-TD5V49V\MSSQLSERVER01; Database=SE08201_Bookstore; Integrated Security=True";

        public WarehouseDashboard()
        {
            InitializeComponent();
            LoadWarehouseStats();
        }


        private void LoadWarehouseStats()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Tính tổng số đầu sách trong kho (Total books in stock)
                    SqlCommand cmdTotal = new SqlCommand("SELECT COUNT(*) FROM Books", conn);
                    int totalBooks = (int)cmdTotal.ExecuteScalar();
                    // Giả sử bạn đặt tên Label/TextBox hiển thị là lblTotalStock
                    lblTotalStock.Text = totalBooks.ToString();

                    // 2. Thống kê sách sắp hết hàng (Almost out of stock)
                    // Vì bảng không có cột Quantity, ta đếm theo Status = 'Out of Stock'
                    SqlCommand cmdLow = new SqlCommand("SELECT COUNT(*) FROM Books WHERE Status = 'Out of Stock'", conn);
                    int lowStock = (int)cmdLow.ExecuteScalar();
                    lblLowStock.Text = lowStock.ToString();

                    // 3. Hiển thị danh sách kho hàng lên DataGridView
                    SqlDataAdapter da = new SqlDataAdapter("SELECT BookID, Title, Price, Status, PublishYear FROM Books", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvWarehouse.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu kho: " + ex.Message);
            }
        }
        
        private void btnEnter_Click(object sender, EventArgs e)
        {
            // Logic: Bạn có thể mở một form nhỏ để nhập thêm số lượng cho sách
            // Ví dụ: Update Quantity = Quantity + Số nhập
            MessageBox.Show("Tính năng nhập kho (Update Quantity) sẽ được phát triển ở Form nhập liệu!");
        }

        
        

        private void btnOverview_Click_1(object sender, EventArgs e)
        {
            
            MessageBox.Show("Đã cập nhật số liệu thống kê mới nhất!");
        }

        private void btnWarehouse_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đang mở danh sách kho chi tiết...");
        }

        private void btnEnter_Click_1(object sender, EventArgs e)
        {
            Product_Management pm = new Product_Management();
            pm.ShowDialog();
            LoadWarehouseStats();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 cusForm = new Form1();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }
    }
}