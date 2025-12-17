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
    public partial class Author_Management : Form
    {
        string connectionString = @"Server=DESKTOP-TD5V49V\MSSQLSERVER01; Database=SE08201_Bookstore; Integrated Security=True";
        public Author_Management()
        {
            InitializeComponent();
            LoadAuthors();
        }
        private void LoadAuthors()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT AuthorID, AuthorName FROM Authors";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvAuthors.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAuthorID.Text) || string.IsNullOrWhiteSpace(txtAuthorName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ ID và Tên tác giả!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Authors (AuthorID, AuthorName) VALUES (@id, @name)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtAuthorID.Text);
                    cmd.Parameters.AddWithValue("@name", txtAuthorName.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm tác giả thành công!");
                    LoadAuthors();
                    ClearForm();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi thêm: " + ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAuthorID.Text))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập AuthorID cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa tác giả này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Authors WHERE AuthorID = @id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", txtAuthorID.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa thành công!");
                        LoadAuthors();
                        ClearForm();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Lỗi khi xóa: " + ex.Message); }
            }
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadAuthors();
        }
        private void ClearForm()
        {
            txtAuthorID.Clear();
            txtAuthorName.Clear();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            AdminDashboard cusForm = new AdminDashboard();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }

        private void dgvAuthors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAuthors.Rows[e.RowIndex];
                txtAuthorID.Text = row.Cells["AuthorID"].Value.ToString();
                txtAuthorName.Text = row.Cells["AuthorName"].Value.ToString();
            }
        }
    }
}
