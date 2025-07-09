using System.Collections.Generic;

namespace ClothingStoreManager.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<int> Purchases { get; set; } = new List<int>(); // قائمة أرقام الفواتير
    }
}