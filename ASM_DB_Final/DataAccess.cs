using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM_DB_Final
{
    internal class DataAccess
    {
     
        private const string ConnectionString = "Server=DESKTOP-TD5V49V\\MSSQLSERVER01;Database=SE08201_Bookstore;Integrated Security=True;";

     
   

        private bool ExecuteNonQuery(string query, Product product = null, string idToDelete = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (product != null)
                    {
                        cmd.Parameters.AddWithValue("@id", product.ID);
                        cmd.Parameters.AddWithValue("@name", product.Name);
                        cmd.Parameters.AddWithValue("@price", product.Price);
                        cmd.Parameters.AddWithValue("@status", product.Status);
                        cmd.Parameters.AddWithValue("@title", product.Title);
                    }
                    else if (idToDelete != null)
                    {
                        cmd.Parameters.AddWithValue("@id", idToDelete);
                    }

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi CSDL Product: " + ex.Message);
                    }
                }
            }
        }

        public DataTable LoadProducts()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT ID, Name, Price, Status, Title FROM Products";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public bool AddProduct(Product product)
        {
            string query = "INSERT INTO Products (ID, Name, Price, Status, Title) VALUES (@id, @name, @price, @status, @title)";
            return ExecuteNonQuery(query, product);
        }

        public bool UpdateProduct(Product product)
        {
            string query = "UPDATE Products SET Name = @name, Price = @price, Status = @status, Title = @title WHERE ID = @id";
            return ExecuteNonQuery(query, product);
        }

        public bool DeleteProduct(string productId)
        {
            string query = "DELETE FROM Products WHERE ID = @id";
            return ExecuteNonQuery(query, null, productId);
        }


        

        // Hàm thực thi riêng cho Purchase để khớp với các tham số của bảng PurchaseHistory
        private bool ExecutePurchaseNonQuery(string query, PurchaseModel purchase = null, string idToDelete = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (purchase != null)
                    {
                        cmd.Parameters.AddWithValue("@purchaseId", purchase.PurchaseID);
                        cmd.Parameters.AddWithValue("@customerName", purchase.CustomerName);
                        cmd.Parameters.AddWithValue("@productCode", purchase.ProductCode);
                        cmd.Parameters.AddWithValue("@date", purchase.DateOfPurchase);
                        cmd.Parameters.AddWithValue("@quantity", purchase.Quantity);
                        cmd.Parameters.AddWithValue("@status", purchase.Status);
                    }
                    else if (idToDelete != null)
                    {
                        cmd.Parameters.AddWithValue("@purchaseId", idToDelete);
                    }

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi CSDL Purchase History: " + ex.Message);
                    }
                }
            }
        }

        // Tải toàn bộ lịch sử mua hàng
        public DataTable LoadPurchaseHistory()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // Đảm bảo tên bảng trong SQL của bạn là PurchaseHistory
                string query = "SELECT PurchaseID, CustomerName, ProductCode, DateOfPurchase, Quantity, Status FROM PurchaseHistory";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // Thêm mới giao dịch
        public bool AddPurchase(PurchaseModel purchase)
        {
            string query = "INSERT INTO PurchaseHistory (PurchaseID, CustomerName, ProductCode, DateOfPurchase, Quantity, Status) " +
                           "VALUES (@purchaseId, @customerName, @productCode, @date, @quantity, @status)";
            return ExecutePurchaseNonQuery(query, purchase);
        }

        // Cập nhật giao dịch
        public bool UpdatePurchase(PurchaseModel purchase)
        {
            string query = "UPDATE PurchaseHistory SET CustomerName=@customerName, ProductCode=@productCode, " +
                           "DateOfPurchase=@date, Quantity=@quantity, Status=@status WHERE PurchaseID=@purchaseId";
            return ExecutePurchaseNonQuery(query, purchase);
        }

        // Xóa giao dịch
        public bool DeletePurchase(string purchaseId)
        {
            string query = "DELETE FROM PurchaseHistory WHERE PurchaseID=@purchaseId";
            return ExecutePurchaseNonQuery(query, null, purchaseId);
        }
    }
}