using System;
using System.Data;
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
            string user = txtUsername.Text;
            string pass = txtPassword.Text;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!");
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Truy vấn lấy quyền (user_role) dựa vào tài khoản mật khẩu
                string sql = "SELECT user_role FROM tblAccounts WHERE username = @user AND user_password = @pass";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@pass", pass);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    // Lấy role và xóa khoảng trắng thừa để so sánh chính xác 100%
                    string role = result.ToString().Trim();
                    MessageBox.Show($"Đăng nhập thành công! Quyền hạn: {role}", "Thông báo");

                    this.Hide();
                    // TRUYỀN BIẾN role VÀO ĐÂY
                    AdminDashboard mainForm = new AdminDashboard(role);
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}