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
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ TextBox
            string user = txtUsername.Text;
            string pass = txtPassword.Text;

            // Kiểm tra tài khoản/mật khẩu cố định
            // Bạn có thể đổi 'admin' và '123' thành bất cứ gì bạn muốn
            if (user == "admin" && pass == "123")
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide(); // Ẩn form đăng nhập

                // Mở Form chính (AdminDashboard)
                // Lưu ý: Truyền vào một chuỗi bất kỳ vì Form của bạn đang yêu cầu tham số
                AdminDashboard mainForm = new AdminDashboard("Admin");
                mainForm.ShowDialog();

                this.Close(); // Đóng hẳn ứng dụng khi thoát form chính
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Simple handler for Register button so clicks are handled gracefully.
        private void btnRegister_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Register functionality not implemented yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}