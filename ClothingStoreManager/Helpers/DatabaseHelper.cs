using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Collections.Generic; // Added for List
using ClothingStoreManager.Models; // Added for Models

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
    }
}