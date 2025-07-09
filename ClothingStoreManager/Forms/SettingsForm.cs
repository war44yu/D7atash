using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Helpers;
using System.IO;
using System;

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
            this.Load += SettingsForm_Load;
        }

        private void LoadSettings()
        {
            txtStoreName.Text = InvoiceTemplate.StoreName;
            txtStoreAddress.Text = InvoiceTemplate.StoreAddress;
            txtStorePhone.Text = InvoiceTemplate.StorePhone;
            txtFooter.Text = InvoiceTemplate.Footer;
        }
        
        private void SaveSettings()
        {
            InvoiceTemplate.StoreName = txtStoreName.Text;
            InvoiceTemplate.StoreAddress = txtStoreAddress.Text;
            InvoiceTemplate.StorePhone = txtStorePhone.Text;
            InvoiceTemplate.Footer = txtFooter.Text;
            MessageBox.Show("تم حفظ الإعدادات بنجاح!", "نجح");
        }
        
        private void BackupDatabase()
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "قاعدة بيانات|*.db";
                saveDialog.FileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.db";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    File.Copy("clothing_store.db", saveDialog.FileName, true);
                    MessageBox.Show("تم إنشاء النسخة الاحتياطية بنجاح!", "نجح");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إنشاء النسخة الاحتياطية: {ex.Message}", "خطأ");
            }
        }
        
        private void RestoreDatabase()
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "قاعدة بيانات|*.db";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("سيتم استبدال قاعدة البيانات الحالية. هل أنت متأكد؟", "تأكيد", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Copy(openDialog.FileName, "clothing_store.db", true);
                        MessageBox.Show("تم استعادة قاعدة البيانات بنجاح! يرجى إعادة تشغيل البرنامج.", "نجح");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في استعادة قاعدة البيانات: {ex.Message}", "خطأ");
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
            btnSave.Click += (s, e) => SaveSettings();
            btnBackup.Click += (s, e) => BackupDatabase();
            btnRestore.Click += (s, e) => RestoreDatabase();
            btnUsers.Click += (s, e) => new UsersForm().ShowDialog();
        }
    }
}