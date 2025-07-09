using System.Data.SQLite;
using System.Data;
using System.IO;

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
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}