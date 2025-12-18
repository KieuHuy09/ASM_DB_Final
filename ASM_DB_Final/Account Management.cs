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
    public partial class Account_Management : Form
    {
        SqlConnection conn;
        // Chuỗi kết nối từ cấu trúc server của bạn
        string strConn = @"Server=DESKTOP-TD5V49V\MSSQLSERVER01; Database=SE08201_Bookstore; Integrated Security=True";
        public Account_Management()
        {
            InitializeComponent();
            conn = new SqlConnection(strConn);
        }

        private void Account_Management_Load(object sender, EventArgs e)
        {
            LoadAccountList();
        }

        // Hàm nạp danh sách tài khoản từ database
        private void LoadAccountList()
        {
            try
            {
                conn.Open();
                string sql = "SELECT username, user_password, user_role FROM tblAccounts";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAccounts.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi nạp dữ liệu: " + ex.Message); }
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string newUser = txtUsername.Text.Trim();

            if (string.IsNullOrEmpty(newUser))
            {
                MessageBox.Show("Vui lòng nhập Username!");
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // 1. Kiểm tra Username đã tồn tại chưa
                string checkSql = "SELECT COUNT(*) FROM tblAccounts WHERE username = @user";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@user", newUser);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Tên đăng nhập '" + newUser + "' đã tồn tại. Vui lòng chọn tên khác!");
                    conn.Close();
                    return;
                }

                // 2. Nếu chưa tồn tại thì mới tiến hành INSERT
                string sql = "INSERT INTO tblAccounts (username, user_password, user_role) VALUES (@user, @pass, @role)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user", newUser);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);
                cmd.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());

                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Tạo tài khoản thành công!");
                LoadAccountList();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn dòng nào trên GridView chưa
            if (dgvAccounts.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản muốn xóa từ danh sách!");
                return;
            }

            // 2. Lấy username của dòng đang chọn (Giả sử cột username là cột đầu tiên - index 0)
            string userToDelete = dgvAccounts.CurrentRow.Cells["username"].Value.ToString();

            // Không cho phép tự xóa tài khoản chính mình (nếu cần bảo mật)
            if (userToDelete == "admin")
            {
                MessageBox.Show("Không thể xóa tài khoản Admin hệ thống!");
                return;
            }

            // 3. Xác nhận lại với người dùng trước khi xóa
            DialogResult confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản '{userToDelete}' không?",
                                                   "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    // 4. Lệnh SQL xóa tài khoản
                    string sql = "DELETE FROM tblAccounts WHERE username = @user";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user", userToDelete);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Xóa tài khoản thành công!");
                        LoadAccountList(); // Gọi lại hàm nạp dữ liệu để cập nhật GridView
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                }
            }
        }
    }
}
