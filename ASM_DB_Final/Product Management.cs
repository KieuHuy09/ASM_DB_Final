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
        private DataAccess db = new DataAccess();

        // Tên DataGridView: dgvProducts (Giả định)
        // Tên TextBoxes: txtID, txtName, txtPrice, txtStatus, txtTitle (Giả định)

        public Product_Management()
        {
            InitializeComponent();
            this.Load += new EventHandler(Product_Management_Load);
            // Gán sự kiện CellClick cho DataGridView (DataGridView lớn màu xám)
            dgvProducts.CellClick += new DataGridViewCellEventHandler(dgvProducts_CellClick);
        }

        private void Product_Management_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // --- Tải Dữ liệu ---
        private void LoadData()
        {
            try
            {
                // dgvProducts là tên Control DataGridView lớn ở dưới
                dgvProducts.DataSource = db.LoadProducts();
                dgvProducts.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu. Vui lòng kiểm tra chuỗi kết nối và Database: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Các Hàm Hỗ Trợ Giao Diện và Validation ---

        private void ClearFormControls()
        {
            txtID.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtStatus.Clear();
            txtTitle.Clear();
            txtID.Focus();
        }

        private Product GetProductDataFromForm()
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ ID, Name và Price.", "Thiếu Thông Tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            // Validation cho Price
            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Price phải là một số hợp lệ.", "Lỗi Định Dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return new Product
            {
                ID = txtID.Text.Trim(),
                Name = txtName.Text.Trim(),
                Price = price,
                Status = txtStatus.Text.Trim(),
                Title = txtTitle.Text.Trim()
            };
        }

        // --- Xử Lý Sự Kiện Nút Bấm ---

        // Nút ADD
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Product product = GetProductDataFromForm();
            if (product == null) return;

            try
            {
                if (db.AddProduct(product))
                {
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFormControls();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Lỗi: Không thể thêm sản phẩm. Có thể ID đã tồn tại.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút UPDATE
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Product product = GetProductDataFromForm();
            if (product == null) return;

            try
            {
                if (db.UpdateProduct(product))
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFormControls();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Lỗi: Không tìm thấy ID để cập nhật.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút DELETE
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Vui lòng nhập ID sản phẩm cần xóa.", "Thiếu ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm có ID: {txtID.Text}?", "Xác nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    if (db.DeleteProduct(txtID.Text.Trim()))
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFormControls();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Không tìm thấy ID để xóa.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Nút REFRESH
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearFormControls();
            LoadData();
        }

        // Nút EXIT
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- Xử Lý DataGridView ---

        // Sự kiện khi click vào một hàng trong DataGridView
        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvProducts.Rows[e.RowIndex];

                // Điền dữ liệu từ hàng được chọn vào các TextBox.
                // Thứ tự cells[] phải khớp với thứ tự cột trong câu lệnh SELECT của LoadProducts()
                txtID.Text = row.Cells["ID"].Value?.ToString();
                txtName.Text = row.Cells["Name"].Value?.ToString();
                txtPrice.Text = row.Cells["Price"].Value?.ToString();
                txtStatus.Text = row.Cells["Status"].Value?.ToString();
                txtTitle.Text = row.Cells["Title"].Value?.ToString();
            }
        }
    }
}