using System.Windows.Forms;
using System.Drawing;

namespace ClothingStoreManager.Forms
{
    public class UsersForm : Form
    {
        private DataGridView dgvUsers;
        private Button btnAdd, btnEdit, btnDelete, btnChangePassword;
        private Label lblTitle;

        public UsersForm()
        {
            this.Text = "إدارة المستخدمين";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Size = new Size(700, 500);
            this.Font = new Font("Cairo", 12);
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            lblTitle = new Label()
            {
                Text = "قائمة المستخدمين",
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Cairo", 16, FontStyle.Bold)
            };

            dgvUsers = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAdd = new Button() { Text = "إضافة مستخدم", Width = 120 };
            btnEdit = new Button() { Text = "تعديل", Width = 100 };
            btnDelete = new Button() { Text = "حذف", Width = 100 };
            btnChangePassword = new Button() { Text = "تغيير كلمة المرور", Width = 150 };

            FlowLayoutPanel panel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                Height = 60,
                FlowDirection = FlowDirection.RightToLeft
            };
            panel.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnChangePassword });

            this.Controls.Add(dgvUsers);
            this.Controls.Add(panel);
            this.Controls.Add(lblTitle);
        }
    }
}