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
        // Thay YOUR_SERVER_NAME bằng tên máy chủ SQL của bạn
        string connectionString = @"Data Source=YOUR_SERVER_NAME;Initial Catalog=FinalDB;Integrated Security=True";

        public WarehouseDashboard()
        {
            InitializeComponent();
            StylingInterface(); // Gọi hàm trang trí giao diện
        }

        private void WarehouseDashboard_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        // 1. Hàm trang trí màu sắc cho giống bản vẽ (Dark Mode)
        void StylingInterface()
        {
            this.BackColor = Color.FromArgb(30, 30, 30); // Màu nền tối

            // Chỉnh màu cho GridView
            dgvWarehouse.BackgroundColor = Color.FromArgb(45, 45, 48);
            dgvWarehouse.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvWarehouse.DefaultCellStyle.ForeColor = Color.White;
            dgvWarehouse.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dgvWarehouse.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvWarehouse.EnableHeadersVisualStyles = false;
        }

        // 2. Hàm tải dữ liệu thống kê và danh sách
        void LoadDashboardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // A. Tính tổng số sách trong kho (Total books in stock)
                    string queryTotal = "SELECT SUM(Quantity) FROM Books";
                    SqlCommand cmdTotal = new SqlCommand(queryTotal, conn);
                    object resultTotal = cmdTotal.ExecuteScalar();
                    // Nếu null (chưa có sách) thì bằng 0
                    lblTotalStock.Text = (resultTotal != DBNull.Value) ? resultTotal.ToString() : "0";

                    // B. Đếm số sách sắp hết hàng (Almost out of stock)
                    // Giả sử quy định dưới 10 cuốn là sắp hết
                    string queryLow = "SELECT COUNT(*) FROM Books WHERE Quantity < 10";
                    SqlCommand cmdLow = new SqlCommand(queryLow, conn);
                    int lowCount = (int)cmdLow.ExecuteScalar();
                    lblLowStock.Text = lowCount.ToString();

                    // Đổi màu cảnh báo nếu có sách sắp hết
                    if (lowCount > 0) lblLowStock.ForeColor = Color.Red;
                    else lblLowStock.ForeColor = Color.LightGreen;

                    // C. Hiển thị bảng "What to do" (Danh sách các sách cần nhập thêm)
                    // Chỉ hiện những sách có Quantity < 10
                    string queryGrid = "SELECT BookID AS [Code], Title AS [Book Name], Quantity FROM Books WHERE Quantity < 10";
                    SqlDataAdapter adapter = new SqlDataAdapter(queryGrid, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvWarehouse.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu kho: " + ex.Message);
            }
        }

        // Nút Refresh (hoặc nút Overview trong menu) để tải lại dữ liệu
        private void btnOverview_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        // Nút Enter Stock (Nhập kho - Chức năng mở rộng)
        private void btnEnter_Click(object sender, EventArgs e)
        {
            // Logic: Bạn có thể mở một form nhỏ để nhập thêm số lượng cho sách
            // Ví dụ: Update Quantity = Quantity + Số nhập
            MessageBox.Show("Tính năng nhập kho (Update Quantity) sẽ được phát triển ở Form nhập liệu!");
        }

        // Nút Exit
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}