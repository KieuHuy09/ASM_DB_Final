using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_DB_Final
{
    public partial class Order : Form
    {
        SqlConnection conn;

        string connectionString = "Server=DESKTOP-TD5V49V\\MSSQLSERVER01;Database=SE08201_Bookstore;Integrated Security=True;";
        public Order()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }
        
        private void FillDataToComboBox()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // 1. Nạp khách hàng
                SqlDataAdapter daCus = new SqlDataAdapter("SELECT CustomerID, CustomerName FROM Customers", conn);
                DataTable dtCus = new DataTable();
                daCus.Fill(dtCus);
                cmbCustomer.DataSource = dtCus;
                cmbCustomer.DisplayMember = "CustomerName";
                cmbCustomer.ValueMember = "CustomerID";

                // 2. Nạp sách (PHẢI CÓ CỘT PRICE Ở ĐÂY)
                SqlDataAdapter daBook = new SqlDataAdapter("SELECT BookID, Title, Price FROM Books", conn);
                DataTable dtBook = new DataTable();
                daBook.Fill(dtBook);
                cmbBook.DataSource = dtBook;
                cmbBook.DisplayMember = "Title";
                cmbBook.ValueMember = "BookID";

                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void LoadCustomers()
        {
            string query = "SELECT CustomerID, CustomerName FROM Customers";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbCustomer.DataSource = dt;
            cmbCustomer.DisplayMember = "CustomerName";
            cmbCustomer.ValueMember = "CustomerID";
        }

        // Lấy danh sách sách và giá tiền đổ vào ComboBox
        private void LoadBooks()
        {
            string query = "SELECT BookID, Title, Price FROM Books";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbBook.DataSource = dt;
            cmbBook.DisplayMember = "Title";
            cmbBook.ValueMember = "BookID";
        }

        // Tự động hiện giá khi chọn sách (Bổ sung thêm tính năng này)
        private void cmbBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBook.SelectedItem != null)
            {
                DataRowView drv = (DataRowView)cmbBook.SelectedItem;
                lblTotal.Text = drv["Price"].ToString(); // txtPrice là ô hiện đơn giá
            }
        }
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (cmbBook.SelectedValue == null || string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("Vui lòng chọn sách và nhập số lượng!");
                return;
            }

            // 2. Lấy thông tin từ các điều khiển giao diện
            string bookID = cmbBook.SelectedValue.ToString();
            string bookName = cmbBook.Text;
            int qty = int.Parse(txtQuantity.Text);
            decimal price = decimal.Parse(lblTotal.Text); // Giả sử bạn có ô hiện giá sách
            decimal subTotal = qty * price;

            // 3. Thêm một dòng mới vào DataGridView (Giỏ hàng tạm thời)
            // Cấu trúc cột dgv: BookID, BookName, Quantity, Price, SubTotal
            dgvOrderDetails.Rows.Add(bookID, bookName, qty, price, subTotal);

            // 4. Cập nhật tổng số tiền hiển thị trên Label
            UpdateTotalAmount();
        }
        private void UpdateTotalAmount()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvOrderDetails.Rows)
            {
                if (row.Cells["SubTotal"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value);
                }
            }
            lblTotal.Text = total.ToString(); // Hiển thị tổng tiền
        }
        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                // Bắt đầu một Transaction để đảm bảo nếu lưu chi tiết lỗi thì hóa đơn tổng cũng không được lưu
                SqlTransaction trans = conn.BeginTransaction();

                // Bước 1: Lưu vào bảng Orders
                string sqlOrder = "INSERT INTO Orders (CustomerID, TotalAmount) OUTPUT INSERTED.OrderID VALUES (@cusID, @total)";
                SqlCommand cmdOrder = new SqlCommand(sqlOrder, conn, trans);
                cmdOrder.Parameters.AddWithValue("@cusID", cmbCustomer.SelectedValue);
                cmdOrder.Parameters.AddWithValue("@total", decimal.Parse(lblTotal.Text));

                int newOrderID = (int)cmdOrder.ExecuteScalar(); // Lấy mã hóa đơn vừa tạo

                // Bước 2: Lưu từng dòng trong DataGridView vào bảng OrderDetails
                foreach (DataGridViewRow row in dgvOrderDetails.Rows)
                {
                    if (row.Cells["BookID"].Value != null)
                    {
                        string sqlDetail = "INSERT INTO OrderDetails (OrderID, BookID, Quantity, Price) VALUES (@oid, @bid, @qty, @price)";
                        SqlCommand cmdDetail = new SqlCommand(sqlDetail, conn, trans);
                        cmdDetail.Parameters.AddWithValue("@oid", newOrderID);
                        cmdDetail.Parameters.AddWithValue("@bid", row.Cells["BookID"].Value);
                        cmdDetail.Parameters.AddWithValue("@qty", row.Cells["Quantity"].Value);
                        cmdDetail.Parameters.AddWithValue("@price", row.Cells["Price"].Value);
                        cmdDetail.ExecuteNonQuery();
                    }
                }

                trans.Commit(); // Hoàn tất giao dịch
                MessageBox.Show("Thanh toán thành công! Đã lưu hóa đơn mã: " + newOrderID);
                dgvOrderDetails.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            AdminDashboard cusForm = new AdminDashboard();
            this.Hide();
            cusForm.ShowDialog();
            this.Show();
        }

        private void Order_Load_1(object sender, EventArgs e)
        {
            FillDataToComboBox();
        }
    }
}
