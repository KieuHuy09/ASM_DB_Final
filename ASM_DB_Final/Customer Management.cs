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
        string connectionString = "Server=DESKTOP-TD5V49V\\MSSQLSERVER01;Database=SE08201_Bookstore;Integrated Security=True;";
        public Customer_Management()
        {
            InitializeComponent();
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Customers";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvCustomers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
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
                    dgvCustomers.DataSource = dt;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCusID.Text)) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Customers VALUES (@id, @name, @phone, @address, @email)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtCusID.Text);
                    cmd.Parameters.AddWithValue("@name", txtCusName.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm khách hàng thành công!");
                    LoadCustomerData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Customers SET CusName=@name, PhoneNumber=@phone, Address=@address, Email=@email WHERE CusID=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtCusID.Text);
                    cmd.Parameters.AddWithValue("@name", txtCusName.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thành công!");
                    LoadCustomerData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
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
                        LoadCustomerData();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                txtCusID.Text = row.Cells["CustomerID"].Value.ToString();
                txtCusName.Text = row.Cells["CustomerName"].Value.ToString();
                txtPhone.Text = row.Cells["PhoneNumber"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
            }
        }

        private void txtExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng cần tìm!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Câu lệnh SQL tìm kiếm theo tên khách hàng (dùng LIKE để tìm kiếm tương đối)
                    string query = "SELECT * FROM Customers WHERE CustomerName LIKE @name";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    // Thêm dấu % vào hai đầu để tìm kiếm các tên có chứa từ khóa đó
                    cmd.Parameters.AddWithValue("@name", "%" + txtSearch.Text + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Hiển thị kết quả tìm được lên bảng dgvCustomers
                    dgvCustomers.DataSource = dt;

                    // Thông báo nếu không tìm thấy ai
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy khách hàng nào có tên này!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }
    }
}

