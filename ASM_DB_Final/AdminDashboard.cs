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

        public AdminDashboard()
        {
            InitializeComponent();
        }

        // Sự kiện khi Form Main tải lên
        private void Main_Load(object sender, EventArgs e)
        {
            // --- PHÂN QUYỀN DỰA TRÊN USE CASE ---
            // Nếu là Staff (Nhân viên), ẩn chức năng quản lý User (vì Use Case Manage User chỉ nối với Admin)
            if (currentRole == "Staff")
            {
                //btnAuthorManagement.Enabled = false; // Hoặc .Visible = false để ẩn luôn
            }

            // Hiển thị tên người dùng hoặc quyền hạn lên tiêu đề form (Optional)
            this.Text = "Hệ thống quản lý nhà sách - Xin chào: " + currentRole;
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

        private void btnUserManagement_Click_1(object sender, EventArgs e)
        {
           // User_Management empForm = new User_Management();
            this.Hide();
            //empForm.ShowDialog();
            this.Show();
        }

        private void btnAuthorsManagement_Click(object sender, EventArgs e)
        {


            Author_Management empForm = new Author_Management();
            this.Hide();
            empForm.ShowDialog();
            this.Show();
        }

        private void btnCustomerManagement_Click(object sender, EventArgs e)
        {
            Customer_Management cusForm = new Customer_Management();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }

        private void btnProductManagement_Click_1(object sender, EventArgs e)
        {
            Product_Management cusForm = new Product_Management();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất");
            Form1 cusForm = new Form1();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            Order cusForm = new Order();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }
    }
}
