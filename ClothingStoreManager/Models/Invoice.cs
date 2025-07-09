using System;
using System.Collections.Generic;

namespace ClothingStoreManager.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int? CustomerId { get; set; } // يمكن أن تكون فاتورة بدون عميل
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public decimal Total { get; set; }
        public string Type { get; set; } = "بيع"; // بيع أو شراء
    }

    public class InvoiceItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;
    }
}