using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Helpers;
using ClothingStoreManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace ClothingStoreManager.Forms
{
    public class SuppliersForm : Form
    {
        private DataGridView dgvSuppliers;
        private Button btnAdd, btnEdit, btnDelete, btnSearch;
        private TextBox txtSearch;
        private Label lblTitle;
        private List<Supplier> suppliers = new List<Supplier>();

        public SuppliersForm()
        {
            this.Text = "إدارة الموردين";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Size = new Size(900, 600);
            this.Font = new Font("Cairo", 12);
            InitializeComponents();
            this.Load += SuppliersForm_Load;
        }

        private void InitializeComponents()
        {
            lblTitle = new Label()
            {
                Text = "قائمة الموردين",
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Cairo", 16, FontStyle.Bold)
            };

            dgvSuppliers = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAdd = new Button() { Text = "إضافة مورد", Width = 120 };
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

            this.Controls.Add(dgvSuppliers);
            this.Controls.Add(panel);
            this.Controls.Add(lblTitle);
        }

        private void LoadSuppliers(string? search = null)
        {
            suppliers = DatabaseHelper.GetAllSuppliers();
            if (!string.IsNullOrWhiteSpace(search))
                dgvSuppliers.DataSource = suppliers.Where(s => s.Name.Contains(search) || s.Phone.Contains(search)).ToList();
            else
                dgvSuppliers.DataSource = suppliers;
        }

        private void SuppliersForm_Load(object sender, EventArgs e)
        {
            btnAdd.Click += (s, e) =>
            {
                var form = new SupplierEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    DatabaseHelper.AddSupplier(form.Supplier);
                    LoadSuppliers();
                }
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgvSuppliers.CurrentRow != null)
                {
                    var supplier = (Supplier)dgvSuppliers.CurrentRow.DataBoundItem;
                    var form = new SupplierEditForm(supplier);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        DatabaseHelper.UpdateSupplier(form.Supplier);
                        LoadSuppliers();
                    }
                }
            };
            btnDelete.Click += (s, e) =>
            {
                if (dgvSuppliers.CurrentRow != null)
                {
                    var supplier = (Supplier)dgvSuppliers.CurrentRow.DataBoundItem;
                    if (MessageBox.Show($"هل تريد حذف المورد {supplier.Name}?", "تأكيد", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DatabaseHelper.DeleteSupplier(supplier.Id);
                        LoadSuppliers();
                    }
                }
            };
            btnSearch.Click += (s, e) => LoadSuppliers(txtSearch.Text);
            LoadSuppliers();
        }
    }
}