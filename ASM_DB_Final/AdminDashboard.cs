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
        string userRole = "";

        // Sửa lại Constructor để nhận tham số role
        public AdminDashboard(string role)
        {
            InitializeComponent();
            this.userRole = role.ToLower();
        }

        public AdminDashboard()
        {
            InitializeComponent();
            
        }





        // Sự kiện khi Form Main tải lên
        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            
            string check = userRole.ToLower().Trim();

            // CHỈ HIỆN nút theo đúng vai trò, còn lại ẨN HẾT
            if (check == "admin")
            {
                SetAllButtons(true); // Hiện tất cả
            }
            else if (check == "user") // Tài khoản Nguyen Van Min
            {
                SetAllButtons(false); // Ẩn hết trước
                btnCustomerManagement.Visible = true; // Hiện lại cái cần
                btnProductManagement.Visible = true;
                btnSale.Visible = true;
            }
            else if (check == "test") // Tài khoản warehouse
            {
                SetAllButtons(false);
                btnWarehouseDashboard.Visible = true;
            }
        }

        // Hàm phụ trợ để ẩn/hiện nhanh (đỡ phải viết từng dòng .Visible)
        private void SetAllButtons(bool status)
        {
            btnCustomerManagement.Visible = status;
            btnAuthorsManagement.Visible = status;
            btnProductManagement.Visible = status;
            btnWarehouseDashboard.Visible = status;
            btnDashboard.Visible = status;
            btnOrder.Visible = status;
            btnCreate.Visible = status;
            btnSale.Visible = status;
        }











        // 5. Nút LOGOUT (Đăng xuất)
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất");
        }

        private void btnWarehouseDashboard_Click(object sender, EventArgs e)
        {
            WarehouseDashboard f = new WarehouseDashboard();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard f = new Dashboard();
            this.Hide();
            f.ShowDialog();
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


            Author_Management f = new Author_Management();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnCustomerManagement_Click(object sender, EventArgs e)
        {
            Customer_Management f = new Customer_Management();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnProductManagement_Click_1(object sender, EventArgs e)
        {
            Product_Management f = new Product_Management();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất");
            Form1 Login = new Form1();
            this.Hide();
            Login.ShowDialog();
            this.Show();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            Order f = new Order();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Account_Management f = new Account_Management();
            this.Hide();
                        f.ShowDialog();
            this.Show();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            Sale_Management f = new Sale_Management();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
