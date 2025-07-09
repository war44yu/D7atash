using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Models;
using ClothingStoreManager.Helpers;

namespace ClothingStoreManager.Forms
{
    public class ChangePasswordForm : Form
    {
        private User user;
        private TextBox txtOldPassword, txtNewPassword, txtConfirmPassword;
        private Button btnSave;
        public ChangePasswordForm(User user)
        {
            this.user = user;
            this.Text = "تغيير كلمة المرور";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Size = new Size(400, 300);
            this.Font = new Font("Cairo", 12);
            InitializeComponents();
        }
        private void InitializeComponents()
        {
            txtOldPassword = new TextBox() { Width = 250, PasswordChar = '●' };
            txtNewPassword = new TextBox() { Width = 250, PasswordChar = '●' };
            txtConfirmPassword = new TextBox() { Width = 250, PasswordChar = '●' };
            btnSave = new Button() { Text = "حفظ", Width = 120 };
            btnSave.Click += (s, e) =>
            {
                if (txtOldPassword.Text != user.Password)
                {
                    MessageBox.Show("كلمة المرور القديمة غير صحيحة!", "خطأ");
                    return;
                }
                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("كلمة المرور الجديدة وتأكيدها غير متطابقتين!", "خطأ");
                    return;
                }
                DatabaseHelper.ChangePassword(user.Id, txtNewPassword.Text);
                MessageBox.Show("تم تغيير كلمة المرور بنجاح!", "نجح");
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            TableLayoutPanel table = new TableLayoutPanel() { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 2, AutoSize = true };
            table.Controls.Add(new Label() { Text = "كلمة المرور القديمة:" }, 0, 0); table.Controls.Add(txtOldPassword, 1, 0);
            table.Controls.Add(new Label() { Text = "كلمة المرور الجديدة:" }, 0, 1); table.Controls.Add(txtNewPassword, 1, 1);
            table.Controls.Add(new Label() { Text = "تأكيد كلمة المرور:" }, 0, 2); table.Controls.Add(txtConfirmPassword, 1, 2);
            FlowLayoutPanel panel = new FlowLayoutPanel() { Dock = DockStyle.Bottom, Height = 60, FlowDirection = FlowDirection.RightToLeft };
            panel.Controls.Add(btnSave);
            this.Controls.Add(table);
            this.Controls.Add(panel);
        }
    }
}