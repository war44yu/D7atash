using System.Windows.Forms;
using System.Drawing;

namespace ClothingStoreManager.Forms
{
    public class CustomersForm : Form
    {
        private DataGridView dgvCustomers;
        private Button btnAdd, btnEdit, btnDelete, btnSearch;
        private TextBox txtSearch;
        private Label lblTitle;

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
    }
}