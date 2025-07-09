using System.Windows.Forms;
using System.Drawing;

namespace ClothingStoreManager.Forms
{
    public class ReportsForm : Form
    {
        private ComboBox cmbReportType;
        private Button btnExportPdf, btnExportExcel, btnShowReport;
        private DataGridView dgvReport;
        private Label lblTitle;

        public ReportsForm()
        {
            this.Text = "التقارير";
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
                Text = "التقارير",
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Cairo", 16, FontStyle.Bold)
            };

            cmbReportType = new ComboBox()
            {
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbReportType.Items.AddRange(new string[] { "مبيعات يومية", "مبيعات شهرية", "مخزون", "أرباح وخسائر" });

            btnShowReport = new Button() { Text = "عرض التقرير", Width = 120 };
            btnExportPdf = new Button() { Text = "تصدير PDF", Width = 100 };
            btnExportExcel = new Button() { Text = "تصدير Excel", Width = 100 };

            FlowLayoutPanel panel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                Height = 60,
                FlowDirection = FlowDirection.RightToLeft
            };
            panel.Controls.AddRange(new Control[] { btnShowReport, btnExportPdf, btnExportExcel, cmbReportType });

            dgvReport = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dgvReport);
            this.Controls.Add(panel);
            this.Controls.Add(lblTitle);
        }
    }
}