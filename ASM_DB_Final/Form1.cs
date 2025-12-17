using System;
using System.Data.SqlClient;
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


        // Simple handler for Register button so clicks are handled gracefully.
        private void btnRegister_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Register functionality not implemented yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ TextBox
            string user = txtUsername.Text;
            string pass = txtPassword.Text;

            // 1. Kiểm tra tài khoản ADMIN
            if (user == "admin" && pass == "123")
            {
                MessageBox.Show("Chào mừng Quản trị viên!", "Thông báo");
                this.Hide();
                // Mở AdminDashboard
                AdminDashboard adminForm = new AdminDashboard("Admin");
                adminForm.ShowDialog();
                this.Close();
            }
            // 2. Kiểm tra tài khoản NHÂN VIÊN BÁN HÀNG (User)
            else if (user == "staff" && pass == "123")
            {
                MessageBox.Show("Chào mừng Nhân viên bán hàng!", "Thông báo");
                this.Hide();
                // Mở Dashboard (Form nhân viên)
                Dashboard staffForm = new Dashboard();
                staffForm.ShowDialog();
                this.Close();
            }
            // 3. Kiểm tra tài khoản QUẢN LÝ KHO (Test)
            else if (user == "warehouse" && pass == "123")
            {
                MessageBox.Show("Chào mừng Quản lý kho!", "Thông báo");
                this.Hide();
                // Mở WarehouseDashboard
                WarehouseDashboard storageForm = new WarehouseDashboard();
                storageForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}