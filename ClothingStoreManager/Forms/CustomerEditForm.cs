using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Models;

namespace ClothingStoreManager.Forms
{
    public class CustomerEditForm : Form
    {
        public Customer Customer { get; private set; }
        private TextBox txtName, txtPhone, txtAddress;
        private Button btnSave;
        public CustomerEditForm(Customer? customer = null)
        {
            this.Text = customer == null ? "إضافة عميل" : "تعديل عميل";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Size = new Size(400, 300);
            this.Font = new Font("Cairo", 12);
            Customer = customer ?? new Customer();
            InitializeComponents();
            if (customer != null) LoadCustomer();
        }
        private void InitializeComponents()
        {
            txtName = new TextBox() { Width = 250, Text = Customer.Name };
            txtPhone = new TextBox() { Width = 250, Text = Customer.Phone };
            txtAddress = new TextBox() { Width = 250, Text = Customer.Address };
            btnSave = new Button() { Text = "حفظ", Width = 120 };
            btnSave.Click += (s, e) =>
            {
                Customer.Name = txtName.Text;
                Customer.Phone = txtPhone.Text;
                Customer.Address = txtAddress.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            TableLayoutPanel table = new TableLayoutPanel() { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 2, AutoSize = true };
            table.Controls.Add(new Label() { Text = "الاسم:" }, 0, 0); table.Controls.Add(txtName, 1, 0);
            table.Controls.Add(new Label() { Text = "الهاتف:" }, 0, 1); table.Controls.Add(txtPhone, 1, 1);
            table.Controls.Add(new Label() { Text = "العنوان:" }, 0, 2); table.Controls.Add(txtAddress, 1, 2);
            FlowLayoutPanel panel = new FlowLayoutPanel() { Dock = DockStyle.Bottom, Height = 60, FlowDirection = FlowDirection.RightToLeft };
            panel.Controls.Add(btnSave);
            this.Controls.Add(table);
            this.Controls.Add(panel);
        }
        private void LoadCustomer()
        {
            txtName.Text = Customer.Name;
            txtPhone.Text = Customer.Phone;
            txtAddress.Text = Customer.Address;
        }
    }
}