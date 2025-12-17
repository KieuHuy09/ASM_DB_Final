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
    public partial class Customer_Management : Form
    {
        string connectionString = @"Data Source=YOUR_SERVER_NAME;Initial Catalog=FinalDB;Integrated Security=True";
        public Customer_Management()
        {
            InitializeComponent();
        }
        // 1. Khi Form mở lên thì tải dữ liệu
        private void Customer_Management_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // Hàm tải dữ liệu từ SQL lên bảng
        void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Đảm bảo tên bảng trong SQL là Customers
                    string query = "SELECT * FROM Customers";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCustomer.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        // Hàm xóa trắng các ô nhập
        void ClearControls()
        {
            txtCusID.Clear();
            txtCusName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
        }

        // 2. Nút ADD (Thêm)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Customers (CustomerID, CustomerName, PhoneNumber, Address) VALUES (@id, @name, @phone, @addr)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtCusID.Text);
                    cmd.Parameters.AddWithValue("@name", txtCusName.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@addr", txtAddress.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm thành công!");
                    LoadData();
                    ClearControls();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        // 3. Nút UPDATE (Sửa)
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Customers SET CustomerName=@name, PhoneNumber=@phone, Address=@addr WHERE CustomerID=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtCusID.Text);
                    cmd.Parameters.AddWithValue("@name", txtCusName.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@addr", txtAddress.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thành công!");
                    LoadData();
                    ClearControls();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message);
            }
        }

        // 4. Nút DELETE (Xóa)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Customers WHERE CustomerID=@id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", txtCusID.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đã xóa!");
                        LoadData();
                        ClearControls();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
            }
        }

        // 5. Sự kiện Click vào bảng (Hiện thông tin lên ô nhập)
        // Nhớ vào Properties của DataGridView -> Sự kiện CellClick -> Chọn hàm này
        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomer.Rows[e.RowIndex];
                txtCusID.Text = row.Cells[0].Value.ToString();
                txtCusName.Text = row.Cells[1].Value.ToString();
                txtPhone.Text = row.Cells[2].Value.ToString();
                txtAddress.Text = row.Cells[3].Value.ToString();
            }
        }

        // 6. Tìm kiếm (Gán vào sự kiện TextChanged của ô txtSearch)
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Customers WHERE CustomerName LIKE @search OR PhoneNumber LIKE @search";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCustomer.DataSource = dt;
                }
            }
            catch { }
        }

        // 7. Nút Thoát
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
    