using System.Windows.Forms;
using System.Drawing;

namespace ClothingStoreManager
{
    public class MainForm : Form
    {
        private Panel sidePanel;
        private Button btnDashboard, btnItems, btnStock, btnSales, btnCustomers, btnSuppliers, btnReports, btnSettings;
        private Label lblTitle;

        public MainForm()
        {
            this.Text = "برنامج إدارة محل الملابس";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.WindowState = FormWindowState.Maximized;
            this.Font = new Font("Cairo", 12);

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            sidePanel = new Panel()
            {
                Dock = DockStyle.Right,
                Width = 220,
                BackColor = Color.FromArgb(44, 62, 80)
            };

            btnDashboard = CreateMenuButton("الرئيسية");
            btnItems = CreateMenuButton("الأصناف");
            btnStock = CreateMenuButton("المخزون");
            btnSales = CreateMenuButton("المبيعات");
            btnCustomers = CreateMenuButton("العملاء");
            btnSuppliers = CreateMenuButton("الموردين");
            btnReports = CreateMenuButton("التقارير");
            btnSettings = CreateMenuButton("الإعدادات");

            btnDashboard.Click += (s, e) => MessageBox.Show("لوحة المعلومات قيد التطوير", "تنبيه");
            btnItems.Click += (s, e) => new Forms.ItemsForm().ShowDialog();
            btnStock.Click += (s, e) => MessageBox.Show("نافذة المخزون قيد التطوير", "تنبيه");
            btnSales.Click += (s, e) => new Forms.SalesForm().ShowDialog();
            btnCustomers.Click += (s, e) => new Forms.CustomersForm().ShowDialog();
            btnSuppliers.Click += (s, e) => MessageBox.Show("نافذة الموردين قيد التطوير", "تنبيه");
            btnReports.Click += (s, e) => new Forms.ReportsForm().ShowDialog();
            btnSettings.Click += (s, e) => new Forms.SettingsForm().ShowDialog();

            btnDashboard.Top = 40;
            btnItems.Top = btnDashboard.Bottom + 10;
            btnStock.Top = btnItems.Bottom + 10;
            btnSales.Top = btnStock.Bottom + 10;
            btnCustomers.Top = btnSales.Bottom + 10;
            btnSuppliers.Top = btnCustomers.Bottom + 10;
            btnReports.Top = btnSuppliers.Bottom + 10;
            btnSettings.Top = btnReports.Bottom + 10;

            sidePanel.Controls.AddRange(new Control[] {
                btnDashboard, btnItems, btnStock, btnSales, btnCustomers, btnSuppliers, btnReports, btnSettings
            });

            lblTitle = new Label()
            {
                Text = "مرحبا بك في برنامج إدارة محل الملابس",
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Cairo", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(sidePanel);
        }

        private Button CreateMenuButton(string text)
        {
            return new Button()
            {
                Text = text,
                Width = 200,
                Height = 45,
                Left = 10,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Cairo", 13, FontStyle.Bold)
            };
        }
    }
}