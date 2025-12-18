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
    public partial class Product_Management : Form


    {
        string connectionString = "Server=DESKTOP-TD5V49V\\MSSQLSERVER01;Database=SE08201_Bookstore;Integrated Security=True;";

        public Product_Management()
        {
            InitializeComponent();

            LoadData();

            dgvProducts.CellContentClick += new DataGridViewCellEventHandler(dgvProducts_CellContentClick);

            this.Load += Product_Management_Load;
        }

        private void Product_Management_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // --- HÀM TẢI DỮ LIỆU LÊN BẢNG ---
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-TD5V49V\\MSSQLSERVER01;Database=SE08201_Bookstore;Integrated Security=True;"))
                {
                    conn.Open();
                    // BookID -> ID | Title -> Name | PublishYear -> Title (khớp với giao diện của bạn)
                    string query = "SELECT BookID AS ID, Title AS Name, Price, PublishYear AS Title FROM Books";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Gán dữ liệu vào bảng dgvProducts
                    dgvProducts.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối SQL: " + ex.Message);
            }
        }

        

        

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Vui lòng chọn một dòng từ bảng để sửa!");
                return;
            }

            // 2. Ép kiểu số an toàn cho Price để tránh lỗi "Input string format"
            if (!decimal.TryParse(txtPrice.Text, out decimal priceValue))
            {
                MessageBox.Show("Giá tiền không hợp lệ! Vui lòng chỉ nhập số.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Lệnh SQL cập nhật bảng Books
                    // Chú ý: Cột năm xuất bản trong SQL của bạn là PublishYear
                    string query = "UPDATE Books SET Title=@name, Price=@price, PublishYear=@year WHERE BookID=@id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtID.Text);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@price", priceValue);
                    cmd.Parameters.AddWithValue("@year", txtTitle.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật dữ liệu thành công!");
                        LoadData(); // Tải lại bảng để thấy thay đổi
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống khi sửa: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Vui lòng nhập hoặc chọn ID cần xóa!");
                return;
            }

            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa sách có ID: " + txtID.Text + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        // Phải dùng đúng tên cột là BookID và tên bảng là Books
                        string query = "DELETE FROM Books WHERE BookID = @id";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", txtID.Text.Trim());

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Xóa thành công!");
                            LoadData(); // Tải lại bảng dgv để cập nhật danh sách
                            ClearForm(); // Xóa trắng các ô nhập liệu
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy ID này trong hệ thống!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi khóa ngoại (Foreign Key), nó sẽ báo ở đây
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                }
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadData();
        }

        private void ClearForm()
        {
            txtID.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txt.Clear();
            txtTitle.Clear();
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
                txtID.Text = row.Cells["ID"].Value.ToString();
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                
                txtTitle.Text = row.Cells["Title"].Value.ToString();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Books (BookID, Title, Price, PublishYear) VALUES (@id, @name, @price, @year)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtID.Text);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(txtPrice.Text));

                    cmd.Parameters.AddWithValue("@year", txtTitle.Text); // PublishYear từ SQL tương ứng txtTitle

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm sản phẩm thành công!");
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi thêm: " + ex.Message); }
        }
    }
}
    


