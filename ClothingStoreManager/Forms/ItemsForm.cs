using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Helpers;
using ClothingStoreManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace ClothingStoreManager.Forms
{
    public class ItemsForm : Form
    {
        private DataGridView dgvItems;
        private Button btnAdd, btnEdit, btnDelete, btnSearch;
        private TextBox txtSearch;
        private Label lblTitle;
        private List<Item> items = new List<Item>();

        public ItemsForm()
        {
            this.Text = "إدارة الأصناف";
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
                Text = "قائمة الأصناف",
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Cairo", 16, FontStyle.Bold)
            };

            dgvItems = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAdd = new Button() { Text = "إضافة صنف", Width = 120 };
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

            this.Controls.Add(dgvItems);
            this.Controls.Add(panel);
            this.Controls.Add(lblTitle);
        }

        private void LoadItems(string? search = null)
        {
            items = DatabaseHelper.GetAllItems();
            if (!string.IsNullOrWhiteSpace(search))
                dgvItems.DataSource = items.Where(i => i.Name.Contains(search) || i.Type.Contains(search) || i.Brand.Contains(search)).ToList();
            else
                dgvItems.DataSource = items;
        }

        private void ItemsForm_Load(object sender, EventArgs e)
        {
            btnAdd.Click += (s, e) =>
            {
                var form = new ItemEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    DatabaseHelper.AddItem(form.Item);
                    LoadItems();
                }
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgvItems.CurrentRow != null)
                {
                    var item = (Item)dgvItems.CurrentRow.DataBoundItem;
                    var form = new ItemEditForm(item);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        DatabaseHelper.UpdateItem(form.Item);
                        LoadItems();
                    }
                }
            };
            btnDelete.Click += (s, e) =>
            {
                if (dgvItems.CurrentRow != null)
                {
                    var item = (Item)dgvItems.CurrentRow.DataBoundItem;
                    if (MessageBox.Show($"هل تريد حذف الصنف {item.Name}?", "تأكيد", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DatabaseHelper.DeleteItem(item.Id);
                        LoadItems();
                    }
                }
            };
            btnSearch.Click += (s, e) => LoadItems(txtSearch.Text);
            LoadItems();
        }
    }
}