using System.Windows.Forms;
using System.Drawing;

namespace ClothingStoreManager.Forms
{
    public class SalesForm : Form
    {
        private DataGridView dgvInvoiceItems;
        private Button btnAddItem, btnRemoveItem, btnPrint, btnOpenDrawer, btnSaveInvoice;
        private ComboBox cmbCustomers;
        private Label lblTotal, lblTitle;

        public SalesForm()
        {
            this.Text = "المبيعات وإصدار الفواتير";
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
                Text = "إصدار فاتورة بيع",
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Cairo", 16, FontStyle.Bold)
            };

            dgvInvoiceItems = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAddItem = new Button() { Text = "إضافة صنف", Width = 120 };
            btnRemoveItem = new Button() { Text = "حذف صنف", Width = 100 };
            btnPrint = new Button() { Text = "طباعة الفاتورة", Width = 120 };
            btnOpenDrawer = new Button() { Text = "فتح الدرج", Width = 100 };
            btnSaveInvoice = new Button() { Text = "حفظ الفاتورة", Width = 120 };
            cmbCustomers = new ComboBox() { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            lblTotal = new Label() { Text = "الإجمالي: 0.00 جنيه", AutoSize = true, Font = new Font("Cairo", 14, FontStyle.Bold) };

            FlowLayoutPanel panel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                Height = 60,
                FlowDirection = FlowDirection.RightToLeft
            };
            panel.Controls.AddRange(new Control[] { btnAddItem, btnRemoveItem, btnSaveInvoice, btnPrint, btnOpenDrawer, cmbCustomers, lblTotal });

            this.Controls.Add(dgvInvoiceItems);
            this.Controls.Add(panel);
            this.Controls.Add(lblTitle);
        }
    }
}