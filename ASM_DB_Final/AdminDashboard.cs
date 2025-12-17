using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_DB_Final
{
    public partial class AdminDashboard : Form
    {
        // Biến để lưu quyền hạn (Admin/Staff) nhận từ form Login
        private string currentRole;

        // Sửa lại Constructor để nhận tham số role
        public AdminDashboard(string role)
        {
            InitializeComponent();
            this.currentRole = role;
        }

        // Sự kiện khi Form Main tải lên
        private void Main_Load(object sender, EventArgs e)
        {
            // --- PHÂN QUYỀN DỰA TRÊN USE CASE ---
            // Nếu là Staff (Nhân viên), ẩn chức năng quản lý User (vì Use Case Manage User chỉ nối với Admin)
            if (currentRole == "Staff")
            {
                btnUserManagement.Enabled = false; // Hoặc .Visible = false để ẩn luôn
            }

            // Hiển thị tên người dùng hoặc quyền hạn lên tiêu đề form (Optional)
            this.Text = "Hệ thống quản lý nhà sách - Xin chào: " + currentRole;
        }

        // 1. Nút USER MANAGEMENT (Quản lý nhân viên)
        // Mở form Employee Management.cs (Trong code là Employee_Management)
        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            User_Management empForm = new User_Management();
            this.Hide();
            empForm.ShowDialog();
            this.Show();
        }

        // 2. Nút PRODUCT MANAGEMENT (Quản lý sách)
        // Mở form Product Management.cs
        private void btnProductManagement_Click(object sender, EventArgs e)
        {
            Form1 proForm = new Form1();
            this.Hide();
            proForm.ShowDialog();
            this.Show();
        }

        // 3. Nút CUSTOMER MANAGEMENT (Quản lý khách hàng)
        // Mở form Customer Management.cs
        private void btnCustomerMgt_Click(object sender, EventArgs e)
        {
            Customer_Management cusForm = new Customer_Management();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }

        // 4. Nút AUTHORS MANAGEMENT (Quản lý tác giả)
        // Hiện tại chưa có file Form này trong Solution Explorer của bạn
        private void btnAuthorMgt_Click(object sender, EventArgs e)
        {
            // Khi nào bạn tạo xong form AuthorManagement thì mở comment dòng dưới ra:
            // AuthorManagement auForm = new AuthorManagement();
            // this.Hide();
            // auForm.ShowDialog();
            // this.Show();

            MessageBox.Show("Chức năng đang phát triển (Bạn cần tạo thêm Form AuthorManagement)!", "Thông báo");
        }

        // 5. Nút LOGOUT (Đăng xuất)
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất");
        }

        private void btnWarehouseDashboard_Click(object sender, EventArgs e)
        {
            WarehouseDashboard cusForm = new WarehouseDashboard();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard cusForm = new Dashboard();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }
    }
}
