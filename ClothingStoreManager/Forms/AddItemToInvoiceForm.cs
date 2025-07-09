using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace ClothingStoreManager.Forms
{
    public class AddItemToInvoiceForm : Form
    {
        public InvoiceItem InvoiceItem { get; private set; }
        private ComboBox cmbItems;
        private TextBox txtQuantity, txtPrice;
        private Button btnAdd;
        private List<Item> items;
        
        public AddItemToInvoiceForm(List<Item> items)
        {
            this.items = items;
            this.Text = "إضافة صنف للفاتورة";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Size = new Size(400, 300);
            this.Font = new Font("Cairo", 12);
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            cmbItems = new ComboBox() { Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var item in items.Where(i => i.Quantity > 0))
                cmbItems.Items.Add($"{item.Name} - {item.Type} - الكمية: {item.Quantity}");
            
            txtQuantity = new TextBox() { Width = 250, Text = "1" };
            txtPrice = new TextBox() { Width = 250 };
            
            cmbItems.SelectedIndexChanged += (s, e) =>
            {
                if (cmbItems.SelectedIndex >= 0)
                {
                    var selectedItem = items.Where(i => i.Quantity > 0).ToList()[cmbItems.SelectedIndex];
                    txtPrice.Text = selectedItem.SalePrice.ToString();
                }
            };
            
            btnAdd = new Button() { Text = "إضافة", Width = 120 };
            btnAdd.Click += (s, e) =>
            {
                if (cmbItems.SelectedIndex < 0)
                {
                    MessageBox.Show("يجب اختيار صنف!", "تنبيه");
                    return;
                }
                
                var selectedItem = items.Where(i => i.Quantity > 0).ToList()[cmbItems.SelectedIndex];
                int quantity = 0;
                decimal price = 0;
                
                if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
                {
                    MessageBox.Show("يجب إدخال كمية صحيحة!", "تنبيه");
                    return;
                }
                
                if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
                {
                    MessageBox.Show("يجب إدخال سعر صحيح!", "تنبيه");
                    return;
                }
                
                if (quantity > selectedItem.Quantity)
                {
                    MessageBox.Show($"الكمية المطلوبة أكبر من المتوفر ({selectedItem.Quantity})!", "تنبيه");
                    return;
                }
                
                InvoiceItem = new InvoiceItem()
                {
                    ItemId = selectedItem.Id,
                    Name = selectedItem.Name,
                    Quantity = quantity,
                    Price = price
                };
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            
            TableLayoutPanel table = new TableLayoutPanel() { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 2, AutoSize = true };
            table.Controls.Add(new Label() { Text = "الصنف:" }, 0, 0); table.Controls.Add(cmbItems, 1, 0);
            table.Controls.Add(new Label() { Text = "الكمية:" }, 0, 1); table.Controls.Add(txtQuantity, 1, 1);
            table.Controls.Add(new Label() { Text = "السعر:" }, 0, 2); table.Controls.Add(txtPrice, 1, 2);
            
            FlowLayoutPanel panel = new FlowLayoutPanel() { Dock = DockStyle.Bottom, Height = 60, FlowDirection = FlowDirection.RightToLeft };
            panel.Controls.Add(btnAdd);
            
            this.Controls.Add(table);
            this.Controls.Add(panel);
        }
    }
}