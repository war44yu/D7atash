using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Models;

namespace ClothingStoreManager.Forms
{
    public class UserEditForm : Form
    {
        public User User { get; private set; }
        private TextBox txtUsername, txtPassword;
        private ComboBox cmbRole;
        private Button btnSave;
        private bool isEdit;
        public UserEditForm(User? user = null)
        {
            isEdit = user != null;
            this.Text = user == null ? "إضافة مستخدم" : "تعديل مستخدم";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Size = new Size(400, 300);
            this.Font = new Font("Cairo", 12);
            User = user ?? new User();
            InitializeComponents();
            if (user != null) LoadUser();
        }
        private void InitializeComponents()
        {
            txtUsername = new TextBox() { Width = 250, Text = User.Username };
            txtPassword = new TextBox() { Width = 250, Text = User.Password, PasswordChar = '●' };
            cmbRole = new ComboBox() { Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRole.Items.AddRange(new string[] { "مدير", "بائع" });
            cmbRole.Text = User.Role;
            btnSave = new Button() { Text = "حفظ", Width = 120 };
            btnSave.Click += (s, e) =>
            {
                User.Username = txtUsername.Text;
                if (!isEdit || !string.IsNullOrEmpty(txtPassword.Text))
                    User.Password = txtPassword.Text;
                User.Role = cmbRole.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            TableLayoutPanel table = new TableLayoutPanel() { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 2, AutoSize = true };
            table.Controls.Add(new Label() { Text = "اسم المستخدم:" }, 0, 0); table.Controls.Add(txtUsername, 1, 0);
            table.Controls.Add(new Label() { Text = isEdit ? "كلمة مرور جديدة:" : "كلمة المرور:" }, 0, 1); table.Controls.Add(txtPassword, 1, 1);
            table.Controls.Add(new Label() { Text = "الصلاحية:" }, 0, 2); table.Controls.Add(cmbRole, 1, 2);
            FlowLayoutPanel panel = new FlowLayoutPanel() { Dock = DockStyle.Bottom, Height = 60, FlowDirection = FlowDirection.RightToLeft };
            panel.Controls.Add(btnSave);
            this.Controls.Add(table);
            this.Controls.Add(panel);
        }
        private void LoadUser()
        {
            txtUsername.Text = User.Username;
            txtPassword.Text = "";
            cmbRole.Text = User.Role;
        }
    }
}