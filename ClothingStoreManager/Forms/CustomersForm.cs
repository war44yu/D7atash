using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Helpers;
using ClothingStoreManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace ClothingStoreManager.Forms
{
    public class CustomersForm : Form
    {
        private DataGridView dgvCustomers;
        private Button btnAdd, btnEdit, btnDelete, btnSearch;
        private TextBox txtSearch;
        private Label lblTitle;
        private List<Customer> customers = new List<Customer>();

        public CustomersForm()
        {
            this.Text = "إدارة العملاء";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Size = new Size(900, 600);
            this.Font = new Font("Cairo", 12);
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            lblTitle = new Label()
            {
                Text = "قائمة العملاء",
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Cairo", 16, FontStyle.Bold)
            };

            dgvCustomers = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAdd = new Button() { Text = "إضافة عميل", Width = 120 };
            btnEdit = new Button() { Text = "تعديل", Width = 100 };
            btnDelete = new Button() { Text = "حذف", Width = 100 };
            btnSearch = new Button() { Text = "بحث", Width = 100 };
            txtSearch = new TextBox() { Width = 200 };

            FlowLayoutPanel panel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                Height = 60,
                FlowDirection = FlowDirection.RightToLeft
            };
            panel.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnSearch, txtSearch });

            this.Controls.Add(dgvCustomers);
            this.Controls.Add(panel);
            this.Controls.Add(lblTitle);
        }

        private void LoadCustomers(string? search = null)
        {
            customers = DatabaseHelper.GetAllCustomers();
            if (!string.IsNullOrWhiteSpace(search))
                dgvCustomers.DataSource = customers.Where(c => c.Name.Contains(search) || c.Phone.Contains(search)).ToList();
            else
                dgvCustomers.DataSource = customers;
        }

        private void CustomersForm_Load(object sender, EventArgs e)
        {
            btnAdd.Click += (s, e) =>
            {
                var form = new CustomerEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    DatabaseHelper.AddCustomer(form.Customer);
                    LoadCustomers();
                }
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgvCustomers.CurrentRow != null)
                {
                    var customer = (Customer)dgvCustomers.CurrentRow.DataBoundItem;
                    var form = new CustomerEditForm(customer);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        DatabaseHelper.UpdateCustomer(form.Customer);
                        LoadCustomers();
                    }
                }
            };
            btnDelete.Click += (s, e) =>
            {
                if (dgvCustomers.CurrentRow != null)
                {
                    var customer = (Customer)dgvCustomers.CurrentRow.DataBoundItem;
                    if (MessageBox.Show($"هل تريد حذف العميل {customer.Name}?", "تأكيد", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DatabaseHelper.DeleteCustomer(customer.Id);
                        LoadCustomers();
                    }
                }
            };
            btnSearch.Click += (s, e) => LoadCustomers(txtSearch.Text);
            LoadCustomers();
        }
    }
}