using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Collections.Generic; // Added for List
using ClothingStoreManager.Models; // Added for Models
using System; // Added for DateTime

namespace ClothingStoreManager.Helpers
{
    public static class DatabaseHelper
    {
        private static string dbFile = "clothing_store.db";
        private static string connectionString = $"Data Source={dbFile};Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
                using var conn = new SQLiteConnection(connectionString);
                conn.Open();
                using var cmd = conn.CreateCommand();
                // إنشاء الجداول الأساسية
                cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Items (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT,
    Type TEXT,
    Size TEXT,
    Color TEXT,
    Brand TEXT,
    ImagePath TEXT,
    Quantity INTEGER,
    PurchasePrice REAL,
    SalePrice REAL
);
CREATE TABLE IF NOT EXISTS Customers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT,
    Phone TEXT,
    Address TEXT
);
CREATE TABLE IF NOT EXISTS Suppliers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT,
    Phone TEXT,
    Address TEXT
);
CREATE TABLE IF NOT EXISTS Invoices (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT,
    CustomerId INTEGER,
    Total REAL,
    Type TEXT
);
CREATE TABLE IF NOT EXISTS InvoiceItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    InvoiceId INTEGER,
    ItemId INTEGER,
    Name TEXT,
    Quantity INTEGER,
    Price REAL
);
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT,
    Password TEXT,
    Role TEXT
);
";
                cmd.ExecuteNonQuery();
                // إضافة مستخدم مدير افتراضي
                cmd.CommandText = @"INSERT INTO Users (Username, Password, Role) VALUES ('admin', '1234', 'مدير');";
                cmd.ExecuteNonQuery();
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        // إضافة صنف
        public static void AddItem(Models.Item item)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Items (Name, Type, Size, Color, Brand, ImagePath, Quantity, PurchasePrice, SalePrice) VALUES (@n, @t, @s, @c, @b, @img, @q, @pp, @sp)";
            cmd.Parameters.AddWithValue("@n", item.Name);
            cmd.Parameters.AddWithValue("@t", item.Type);
            cmd.Parameters.AddWithValue("@s", item.Size);
            cmd.Parameters.AddWithValue("@c", item.Color);
            cmd.Parameters.AddWithValue("@b", item.Brand);
            cmd.Parameters.AddWithValue("@img", item.ImagePath ?? "");
            cmd.Parameters.AddWithValue("@q", item.Quantity);
            cmd.Parameters.AddWithValue("@pp", item.PurchasePrice);
            cmd.Parameters.AddWithValue("@sp", item.SalePrice);
            cmd.ExecuteNonQuery();
        }
        // تحديث صنف
        public static void UpdateItem(Models.Item item)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Items SET Name=@n, Type=@t, Size=@s, Color=@c, Brand=@b, ImagePath=@img, Quantity=@q, PurchasePrice=@pp, SalePrice=@sp WHERE Id=@id";
            cmd.Parameters.AddWithValue("@n", item.Name);
            cmd.Parameters.AddWithValue("@t", item.Type);
            cmd.Parameters.AddWithValue("@s", item.Size);
            cmd.Parameters.AddWithValue("@c", item.Color);
            cmd.Parameters.AddWithValue("@b", item.Brand);
            cmd.Parameters.AddWithValue("@img", item.ImagePath ?? "");
            cmd.Parameters.AddWithValue("@q", item.Quantity);
            cmd.Parameters.AddWithValue("@pp", item.PurchasePrice);
            cmd.Parameters.AddWithValue("@sp", item.SalePrice);
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.ExecuteNonQuery();
        }
        // حذف صنف
        public static void DeleteItem(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Items WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        // جلب جميع الأصناف
        public static List<Models.Item> GetAllItems()
        {
            var list = new List<Models.Item>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Items";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Models.Item
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Type = reader.GetString(2),
                    Size = reader.GetString(3),
                    Color = reader.GetString(4),
                    Brand = reader.GetString(5),
                    ImagePath = reader.GetString(6),
                    Quantity = reader.GetInt32(7),
                    PurchasePrice = reader.GetDecimal(8),
                    SalePrice = reader.GetDecimal(9)
                });
            }
            return list;
        }
        // إضافة عميل
        public static void AddCustomer(Models.Customer customer)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Customers (Name, Phone, Address) VALUES (@n, @p, @a)";
            cmd.Parameters.AddWithValue("@n", customer.Name);
            cmd.Parameters.AddWithValue("@p", customer.Phone);
            cmd.Parameters.AddWithValue("@a", customer.Address);
            cmd.ExecuteNonQuery();
        }
        // تحديث عميل
        public static void UpdateCustomer(Models.Customer customer)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Customers SET Name=@n, Phone=@p, Address=@a WHERE Id=@id";
            cmd.Parameters.AddWithValue("@n", customer.Name);
            cmd.Parameters.AddWithValue("@p", customer.Phone);
            cmd.Parameters.AddWithValue("@a", customer.Address);
            cmd.Parameters.AddWithValue("@id", customer.Id);
            cmd.ExecuteNonQuery();
        }
        // حذف عميل
        public static void DeleteCustomer(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Customers WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        // جلب جميع العملاء
        public static List<Models.Customer> GetAllCustomers()
        {
            var list = new List<Models.Customer>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Customers";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Models.Customer
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Phone = reader.GetString(2),
                    Address = reader.GetString(3)
                });
            }
            return list;
        }
        // إضافة مورد
        public static void AddSupplier(Models.Supplier supplier)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Suppliers (Name, Phone, Address) VALUES (@n, @p, @a)";
            cmd.Parameters.AddWithValue("@n", supplier.Name);
            cmd.Parameters.AddWithValue("@p", supplier.Phone);
            cmd.Parameters.AddWithValue("@a", supplier.Address);
            cmd.ExecuteNonQuery();
        }
        // تحديث مورد
        public static void UpdateSupplier(Models.Supplier supplier)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Suppliers SET Name=@n, Phone=@p, Address=@a WHERE Id=@id";
            cmd.Parameters.AddWithValue("@n", supplier.Name);
            cmd.Parameters.AddWithValue("@p", supplier.Phone);
            cmd.Parameters.AddWithValue("@a", supplier.Address);
            cmd.Parameters.AddWithValue("@id", supplier.Id);
            cmd.ExecuteNonQuery();
        }
        // حذف مورد
        public static void DeleteSupplier(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Suppliers WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        // جلب جميع الموردين
        public static List<Models.Supplier> GetAllSuppliers()
        {
            var list = new List<Models.Supplier>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Suppliers";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Models.Supplier
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Phone = reader.GetString(2),
                    Address = reader.GetString(3)
                });
            }
            return list;
        }
        // إضافة مستخدم
        public static void AddUser(Models.User user)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (Username, Password, Role) VALUES (@u, @p, @r)";
            cmd.Parameters.AddWithValue("@u", user.Username);
            cmd.Parameters.AddWithValue("@p", user.Password);
            cmd.Parameters.AddWithValue("@r", user.Role);
            cmd.ExecuteNonQuery();
        }
        // تحديث مستخدم
        public static void UpdateUser(Models.User user)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Users SET Username=@u, Role=@r WHERE Id=@id";
            cmd.Parameters.AddWithValue("@u", user.Username);
            cmd.Parameters.AddWithValue("@r", user.Role);
            cmd.Parameters.AddWithValue("@id", user.Id);
            cmd.ExecuteNonQuery();
        }
        // تغيير كلمة المرور
        public static void ChangePassword(int userId, string newPassword)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Users SET Password=@p WHERE Id=@id";
            cmd.Parameters.AddWithValue("@p", newPassword);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }
        // حذف مستخدم
        public static void DeleteUser(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Users WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        // جلب جميع المستخدمين
        public static List<Models.User> GetAllUsers()
        {
            var list = new List<Models.User>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Models.User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Role = reader.GetString(3)
                });
            }
            return list;
        }
        // إضافة فاتورة
        public static int AddInvoice(Models.Invoice invoice)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Invoices (Date, CustomerId, Total, Type) VALUES (@d, @c, @t, @type); SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@d", invoice.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@c", invoice.CustomerId.HasValue ? (object)invoice.CustomerId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@t", invoice.Total);
            cmd.Parameters.AddWithValue("@type", invoice.Type);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        // إضافة عنصر فاتورة
        public static void AddInvoiceItem(int invoiceId, Models.InvoiceItem item)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO InvoiceItems (InvoiceId, ItemId, Name, Quantity, Price) VALUES (@iid, @itid, @n, @q, @p)";
            cmd.Parameters.AddWithValue("@iid", invoiceId);
            cmd.Parameters.AddWithValue("@itid", item.ItemId);
            cmd.Parameters.AddWithValue("@n", item.Name);
            cmd.Parameters.AddWithValue("@q", item.Quantity);
            cmd.Parameters.AddWithValue("@p", item.Price);
            cmd.ExecuteNonQuery();
        }
        // تحديث كمية صنف (تقليل عند البيع، زيادة عند الشراء)
        public static void UpdateItemQuantity(int itemId, int quantityChange)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Items SET Quantity = Quantity + @q WHERE Id = @id";
            cmd.Parameters.AddWithValue("@q", quantityChange);
            cmd.Parameters.AddWithValue("@id", itemId);
            cmd.ExecuteNonQuery();
        }
        // جلب جميع الفواتير
        public static List<Models.Invoice> GetAllInvoices()
        {
            var list = new List<Models.Invoice>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Invoices ORDER BY Date DESC";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Models.Invoice
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.Parse(reader.GetString(1)),
                    CustomerId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                    Total = reader.GetDecimal(3),
                    Type = reader.GetString(4)
                });
            }
            return list;
        }
        // جلب عناصر فاتورة معينة
        public static List<Models.InvoiceItem> GetInvoiceItems(int invoiceId)
        {
            var list = new List<Models.InvoiceItem>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM InvoiceItems WHERE InvoiceId = @id";
            cmd.Parameters.AddWithValue("@id", invoiceId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Models.InvoiceItem
                {
                    ItemId = reader.GetInt32(2),
                    Name = reader.GetString(3),
                    Quantity = reader.GetInt32(4),
                    Price = reader.GetDecimal(5)
                });
            }
            return list;
        }
    }
}