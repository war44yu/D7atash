using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Helpers;
using System.Data.SQLite;

namespace ClothingStoreManager.Forms
{
    public class LoginForm : Form
    {
        private Label lblUser, lblPass, lblTitle;
        private TextBox txtUser, txtPass;
        private Button btnLogin;
        public string? LoggedInUser { get; private set; }
        public string? UserRole { get; private set; }

        public LoginForm()
        {
            this.Text = "تسجيل الدخول";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Size = new Size(400, 300);
            this.Font = new Font("Cairo", 12);
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            lblTitle = new Label() { Text = "تسجيل الدخول", Dock = DockStyle.Top, Height = 50, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Cairo", 16, FontStyle.Bold) };
            lblUser = new Label() { Text = "اسم المستخدم:", Top = 70, Left = 40, Width = 120 };
            lblPass = new Label() { Text = "كلمة المرور:", Top = 120, Left = 40, Width = 120 };
            txtUser = new TextBox() { Top = 70, Left = 170, Width = 160 };
            txtPass = new TextBox() { Top = 120, Left = 170, Width = 160, PasswordChar = '●' };
            btnLogin = new Button() { Text = "دخول", Top = 180, Left = 170, Width = 120 };
            btnLogin.Click += BtnLogin_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUser);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtUser);
            this.Controls.Add(txtPass);
            this.Controls.Add(btnLogin);
        }

        private void BtnLogin_Click(object? sender, System.EventArgs e)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SQLiteCommand("SELECT Username, Role FROM Users WHERE Username=@u AND Password=@p", conn);
            cmd.Parameters.AddWithValue("@u", txtUser.Text.Trim());
            cmd.Parameters.AddWithValue("@p", txtPass.Text.Trim());
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                LoggedInUser = reader.GetString(0);
                UserRole = reader.GetString(1);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}