using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_DB_Final
{
    public partial class Purchase_History : Form
    {
        DataAccess db = new DataAccess();

        public Purchase_History()
        {
            InitializeComponent();
        }

        // Sự kiện khi Form vừa mở lên
        private void Purchase_History_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // Hàm tải dữ liệu lên DataGridView
        private void LoadData()
        {
            try
            {
                // dgvPurchase là tên DataGridView của bạn
                dgvPurchase.DataSource = db.LoadPurchaseHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // Nút ADD (Thêm mới)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra định dạng số lượng
                if (!int.TryParse(txtQuantity.Text, out int qty))
                {
                    MessageBox.Show("Số lượng phải là số!");
                    return;
                }

                // Kiểm tra định dạng ngày tháng
                if (!DateTime.TryParse(txtDate.Text, out DateTime pDate))
                {
                    MessageBox.Show("Ngày tháng không hợp lệ (Dùng: YYYY-MM-DD)!");
                    return;
                }

                PurchaseModel p = new PurchaseModel
                {
                    PurchaseID = txtPurchaseID.Text,
                    CustomerName = txtCustomerName.Text,
                    ProductCode = txtProductCode.Text,
                    DateOfPurchase = pDate, // biến DateTime đã parse
                    Quantity = qty,         // biến int đã parse
                    Status = txtStatus.Text
                };

                if (db.AddPurchase(p)) // Bây giờ p sẽ không còn lỗi
                {
                    MessageBox.Show("Thành công!");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        // Nút DELETE (Xóa)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = txtPurchaseID.Text;
            if (string.IsNullOrEmpty(id)) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (db.DeletePurchase(id))
                {
                    MessageBox.Show("Đã xóa giao dịch!");
                    LoadData();
                    ClearFields();
                }
            }
        }

        // Nút UPDATE (Cập nhật)
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                PurchaseModel p = new PurchaseModel
                {
                    PurchaseID = txtPurchaseID.Text,
                    CustomerName = txtCustomerName.Text,
                    ProductCode = txtProductCode.Text,
                    DateOfPurchase = DateTime.Parse(txtDate.Text),
                    Quantity = int.Parse(txtQuantity.Text),
                    Status = txtStatus.Text
                };

                // 2. Gọi hàm Update từ DataAccess
                if (db.UpdatePurchase(p))
                {
                    MessageBox.Show("Cập nhật thành công!");
                    LoadData(); // Tải lại bảng để thấy dữ liệu mới
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại! Kiểm tra lại ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        // Sự kiện Click vào một dòng trong bảng để hiện lên các ô nhập
        private void dgvPurchase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPurchase.Rows[e.RowIndex];
                txtPurchaseID.Text = row.Cells[0].Value.ToString();
                txtCustomerName.Text = row.Cells[1].Value.ToString();
                txtProductCode.Text = row.Cells[2].Value.ToString();
                txtDate.Text = row.Cells[3].Value.ToString();
                txtQuantity.Text = row.Cells[4].Value.ToString();
                txtStatus.Text = row.Cells[5].Value.ToString();
            }
        }

        private void ClearFields()
        {
            txtPurchaseID.Clear();
            txtCustomerName.Clear();
            txtProductCode.Clear();
            txtDate.Clear();
            txtQuantity.Clear();
            txtStatus.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
