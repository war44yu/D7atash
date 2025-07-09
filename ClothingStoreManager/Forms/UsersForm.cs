using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Helpers;
using ClothingStoreManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace ClothingStoreManager.Forms
{
    public class UsersForm : Form
    {
        private DataGridView dgvUsers;
        private Button btnAdd, btnEdit, btnDelete, btnChangePassword;
        private Label lblTitle;
        private List<User> users = new List<User>();

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
            this.Load += UsersForm_Load;
        }

        private void LoadUsers()
        {
            users = DatabaseHelper.GetAllUsers();
            // إخفاء كلمة المرور في العرض
            var display = users.Select(u => new { u.Id, u.Username, Password = "****", u.Role }).ToList();
            dgvUsers.DataSource = display;
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            btnAdd.Click += (s, e) =>
            {
                var form = new UserEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    DatabaseHelper.AddUser(form.User);
                    LoadUsers();
                }
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgvUsers.CurrentRow != null)
                {
                    var userId = (int)dgvUsers.CurrentRow.Cells[0].Value;
                    var user = users.FirstOrDefault(u => u.Id == userId);
                    if (user != null)
                    {
                        var form = new UserEditForm(user);
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            DatabaseHelper.UpdateUser(form.User);
                            LoadUsers();
                        }
                    }
                }
            };
            btnDelete.Click += (s, e) =>
            {
                if (dgvUsers.CurrentRow != null)
                {
                    var userId = (int)dgvUsers.CurrentRow.Cells[0].Value;
                    var user = users.FirstOrDefault(u => u.Id == userId);
                    if (user != null && MessageBox.Show($"هل تريد حذف المستخدم {user.Username}?", "تأكيد", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DatabaseHelper.DeleteUser(user.Id);
                        LoadUsers();
                    }
                }
            };
            btnChangePassword.Click += (s, e) =>
            {
                if (dgvUsers.CurrentRow != null)
                {
                    var userId = (int)dgvUsers.CurrentRow.Cells[0].Value;
                    var user = users.FirstOrDefault(u => u.Id == userId);
                    if (user != null)
                    {
                        var form = new ChangePasswordForm(user);
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadUsers();
                        }
                    }
                }
            };
            LoadUsers();
        }
    }
}