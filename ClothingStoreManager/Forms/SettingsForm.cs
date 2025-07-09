using System.Windows.Forms;
using System.Drawing;

namespace ClothingStoreManager.Forms
{
    public class SettingsForm : Form
    {
        private Label lblStoreName, lblStoreAddress, lblStorePhone, lblFooter;
        private TextBox txtStoreName, txtStoreAddress, txtStorePhone, txtFooter;
        private Button btnSave, btnBackup, btnRestore, btnUsers;

        public SettingsForm()
        {
            this.Text = "الإعدادات";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Size = new Size(600, 400);
            this.Font = new Font("Cairo", 12);
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            lblStoreName = new Label() { Text = "اسم المحل:", Width = 120 };
            lblStoreAddress = new Label() { Text = "العنوان:", Width = 120 };
            lblStorePhone = new Label() { Text = "الهاتف:", Width = 120 };
            lblFooter = new Label() { Text = "عبارة أسفل الفاتورة:", Width = 150 };

            txtStoreName = new TextBox() { Width = 300 };
            txtStoreAddress = new TextBox() { Width = 300 };
            txtStorePhone = new TextBox() { Width = 300 };
            txtFooter = new TextBox() { Width = 300 };

            btnSave = new Button() { Text = "حفظ الإعدادات", Width = 140 };
            btnBackup = new Button() { Text = "نسخ احتياطي", Width = 120 };
            btnRestore = new Button() { Text = "استعادة نسخة", Width = 120 };
            btnUsers = new Button() { Text = "إدارة المستخدمين", Width = 140 };

            TableLayoutPanel table = new TableLayoutPanel()
            {
                Dock = DockStyle.Top,
                RowCount = 5,
                ColumnCount = 2,
                Height = 200,
                AutoSize = true
            };
            table.Controls.Add(lblStoreName, 0, 0);
            table.Controls.Add(txtStoreName, 1, 0);
            table.Controls.Add(lblStoreAddress, 0, 1);
            table.Controls.Add(txtStoreAddress, 1, 1);
            table.Controls.Add(lblStorePhone, 0, 2);
            table.Controls.Add(txtStorePhone, 1, 2);
            table.Controls.Add(lblFooter, 0, 3);
            table.Controls.Add(txtFooter, 1, 3);

            FlowLayoutPanel panel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                FlowDirection = FlowDirection.RightToLeft
            };
            panel.Controls.AddRange(new Control[] { btnSave, btnBackup, btnRestore, btnUsers });

            this.Controls.Add(table);
            this.Controls.Add(panel);
        }
    }
}