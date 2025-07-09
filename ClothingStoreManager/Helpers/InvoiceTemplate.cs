using System.Text;
using ClothingStoreManager.Models;
using System.Collections.Generic;

namespace ClothingStoreManager.Helpers
{
    public static class InvoiceTemplate
    {
        public static string StoreName = "محل الملابس الأنيق";
        public static string StoreAddress = "العنوان: شارع التسوق الرئيسي";
        public static string StorePhone = "هاتف: 01000000000";
        public static string Footer = "شكراً لتسوقكم معنا!";

        public static string GenerateInvoiceText(Invoice invoice, Customer? customer, List<InvoiceItem> items)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{StoreName}");
            sb.AppendLine($"{StoreAddress}");
            sb.AppendLine($"{StorePhone}");
            sb.AppendLine($"------------------------------");
            sb.AppendLine($"فاتورة رقم: {invoice.Id}");
            sb.AppendLine($"التاريخ: {invoice.Date:yyyy/MM/dd HH:mm}");
            if (customer != null)
                sb.AppendLine($"العميل: {customer.Name}");
            sb.AppendLine($"------------------------------");
            sb.AppendLine($"الصنف        الكمية   السعر   الإجمالي");
            foreach (var item in items)
            {
                sb.AppendLine($"{item.Name,-10} {item.Quantity,3} {item.Price,7:0.00} {item.Total,8:0.00}");
            }
            sb.AppendLine($"------------------------------");
            sb.AppendLine($"الإجمالي: {invoice.Total:0.00} جنيه");
            sb.AppendLine($"------------------------------");
            sb.AppendLine(Footer);
            return sb.ToString();
        }
    }
}