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
    public partial class User_Management : Form
    {
        // ⚠️ QUAN TRỌNG: Thay thế YOUR_SERVER_NAME bằng tên server SQL thực tế của bạn
        string connectionString = @"Data Source=YOUR_SERVER_NAME;Initial Catalog=FinalDB;Integrated Security=True";

        // Sửa lỗi: Tên hàm khởi tạo phải khớp với tên Class
        public User_Management()
        {
            InitializeComponent();
            // Cấu hình để ẩn mật khẩu
            txtPassWord.UseSystemPasswordChar = true;
            // Đảm bảo tên control DataGridView là dgvEmployee
        }

        // Sự kiện khi Form tải lên -> Load dữ liệu
        // ⚠️ Chú ý: Đổi tên hàm nếu tên hàm gốc trong Designer không phải là Employee_Management_Load
        private void User_Management_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // HÀM: Tải dữ liệu từ SQL lên DataGridView
        void LoadData(string searchKeyword = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Nếu có từ khóa tìm kiếm, áp dụng WHERE, nếu không thì lấy tất cả
                    string query = "SELECT UserID, FullName, Address, Role, Username, Password FROM Users ";
                    if (!string.IsNullOrEmpty(searchKeyword))
                    {
                        query += "WHERE FullName LIKE @search OR UserID LIKE @search";
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    if (!string.IsNullOrEmpty(searchKeyword))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@search", $"%{searchKeyword}%");
                    }

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvEmployee.DataSource = dt;

                    // Tự động điều chỉnh kích thước cột
                    dgvEmployee.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL hoặc tải dữ liệu. Vui lòng kiểm tra chuỗi kết nối và tên bảng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // HÀM: Làm sạch các ô nhập liệu
        void ClearControls()
        {
            txtEmployeeID.Clear();
            txtEmployeeName.Clear();
            txtLocation.Clear();
            txtRoleID.Clear();
            txtUserName.Clear();
            txtPassWord.Clear();
            txtEmployeeID.Focus();
        }

        // HÀM: Kiểm tra dữ liệu chung
        private bool ValidateInputs(bool isUpdate = false)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeID.Text) ||
                string.IsNullOrWhiteSpace(txtEmployeeName.Text) ||
                string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Employee ID, Tên và Username.", "Thiếu Thông Tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Chỉ cần nhập Password khi ADD
            if (!isUpdate && string.IsNullOrWhiteSpace(txtPassWord.Text))
            {
                MessageBox.Show("Vui lòng nhập Password khi thêm mới.", "Thiếu Thông Tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(txtRoleID.Text, out _))
            {
                MessageBox.Show("Role ID phải là một số nguyên hợp lệ.", "Lỗi Định Dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }


        // NÚT ADD (Thêm nhân viên)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Đảm bảo tên cột khớp: UserID, FullName, Address, Role, Username, Password
                    string query = "INSERT INTO Users (UserID, FullName, Address, Role, Username, Password) " +
                                   "VALUES (@id, @name, @loc, @role, @user, @pass)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtEmployeeID.Text);
                    cmd.Parameters.AddWithValue("@name", txtEmployeeName.Text);
                    cmd.Parameters.AddWithValue("@loc", txtLocation.Text);
                    // ⚠️ CHÚ Ý: Đảm bảo Role ID là kiểu INT/SmallINT trong CSDL
                    cmd.Parameters.AddWithValue("@role", int.Parse(txtRoleID.Text));
                    cmd.Parameters.AddWithValue("@user", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@pass", txtPassWord.Text); // ⚠️ Cần Hash mật khẩu trong ứng dụng thực tế

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm thành công!");
                        LoadData();
                        ClearControls();
                    }
                    else
                    {
                        MessageBox.Show("Thêm không thành công (có thể ID đã tồn tại).");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // NÚT UPDATE (Sửa thông tin)
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Sử dụng isUpdate = true để cho phép không cần nhập password nếu không thay đổi
            if (!ValidateInputs(isUpdate: true)) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // ⚠️ CẢI TIẾN: Nếu người dùng không nhập Password, không cập nhật Password
                    string passUpdate = string.IsNullOrEmpty(txtPassWord.Text) ? "" : ", Password=@pass";

                    string query = $"UPDATE Users SET FullName=@name, Address=@loc, Role=@role, Username=@user {passUpdate} WHERE UserID=@id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtEmployeeID.Text);
                    cmd.Parameters.AddWithValue("@name", txtEmployeeName.Text);
                    cmd.Parameters.AddWithValue("@loc", txtLocation.Text);
                    cmd.Parameters.AddWithValue("@role", int.Parse(txtRoleID.Text));
                    cmd.Parameters.AddWithValue("@user", txtUserName.Text);

                    if (!string.IsNullOrEmpty(txtPassWord.Text))
                    {
                        cmd.Parameters.AddWithValue("@pass", txtPassWord.Text);
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thành công!");
                        LoadData();
                        ClearControls();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật không thành công (không tìm thấy ID).");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // NÚT DELETE (Xóa nhân viên)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmployeeID.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên ID: {txtEmployeeID.Text} không?", "Xác nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Users WHERE UserID=@id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", txtEmployeeID.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa thành công!");
                            LoadData();
                            ClearControls();
                        }
                        else
                        {
                            MessageBox.Show("Xóa không thành công (không tìm thấy ID).");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // SỰ KIỆN: Click vào bảng để hiện thông tin lên ô nhập
        private void dgvEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra chỉ số hàng hợp lệ
            if (e.RowIndex >= 0 && e.RowIndex < dgvEmployee.RowCount)
            {
                DataGridViewRow row = dgvEmployee.Rows[e.RowIndex];

                // ⚠️ QUAN TRỌNG: Thứ tự Cells[0], Cells[1]... phải khớp với thứ tự cột trong câu lệnh SELECT của hàm LoadData (UserID, FullName, Address, Role, Username, Password)

                // Lấy giá trị an toàn (sử dụng ?.ToString())
                txtEmployeeID.Text = row.Cells[0].Value?.ToString(); // UserID
                txtEmployeeName.Text = row.Cells[1].Value?.ToString(); // FullName
                txtLocation.Text = row.Cells[2].Value?.ToString(); // Address
                txtRoleID.Text = row.Cells[3].Value?.ToString(); // Role
                txtUserName.Text = row.Cells[4].Value?.ToString(); // Username

                // KHÔNG hiển thị mật khẩu cũ trong ô nhập. Yêu cầu nhập lại nếu muốn thay đổi
                txtPassWord.Clear();
            }
        }

        // NÚT SEARCH (Tìm kiếm) - Gắn sự kiện này cho txtSearch.TextChanged
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        // NÚT EXIT (Thoát)
        private void btnExit_Click(object sender, EventArgs e)
        {
            // Nếu bạn muốn thoát hẳn ứng dụng
            // Application.Exit(); 

            // Nếu bạn muốn đóng form này và quay lại form khác (ví dụ: AdminDashboard)
            // ⚠️ Cần đảm bảo Class AdminDashboard đã tồn tại và import
            /*
            AdminDashboard proForm = new AdminDashboard(); // Sửa thành tên Class chính xác
            this.Hide();
            proForm.ShowDialog();
            this.Close(); // Đóng form hiện tại sau khi form AdminDashboard đóng
            */

            this.Close(); // Đơn giản là đóng form hiện tại
        }
    }
}