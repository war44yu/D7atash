using System;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace ClothingStoreManager
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // تعيين الثقافة العربية ودعم RTL
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-EG");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-EG");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Helpers.DatabaseHelper.InitializeDatabase();
            using (var login = new Forms.LoginForm())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm(login.UserRole));
                }
            }
        }
    }
}