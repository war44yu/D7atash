using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Helpers;
using ClothingStoreManager.Models;
using System.Collections.Generic;
using System.Linq;
using System;

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
            this.Load += ReportsForm_Load;
        }

        private void GenerateReport()
        {
            if (cmbReportType.SelectedIndex < 0) return;
            
            string reportType = cmbReportType.SelectedItem.ToString();
            switch (reportType)
            {
                case "مبيعات يومية":
                    GenerateDailySalesReport();
                    break;
                case "مبيعات شهرية":
                    GenerateMonthlySalesReport();
                    break;
                case "مخزون":
                    GenerateStockReport();
                    break;
                case "أرباح وخسائر":
                    GenerateProfitLossReport();
                    break;
            }
        }
        
        private void GenerateDailySalesReport()
        {
            var invoices = DatabaseHelper.GetAllInvoices()
                .Where(i => i.Type == "بيع" && i.Date.Date == DateTime.Today)
                .Select(i => new { 
                    i.Id, 
                    التاريخ = i.Date.ToString("HH:mm"), 
                    المبلغ = i.Total.ToString("0.00") 
                }).ToList();
            dgvReport.DataSource = invoices;
        }
        
        private void GenerateMonthlySalesReport()
        {
            var invoices = DatabaseHelper.GetAllInvoices()
                .Where(i => i.Type == "بيع" && i.Date.Month == DateTime.Now.Month && i.Date.Year == DateTime.Now.Year)
                .GroupBy(i => i.Date.Date)
                .Select(g => new { 
                    التاريخ = g.Key.ToString("yyyy-MM-dd"), 
                    عدد_الفواتير = g.Count(), 
                    إجمالي_المبيعات = g.Sum(i => i.Total).ToString("0.00") 
                }).ToList();
            dgvReport.DataSource = invoices;
        }
        
        private void GenerateStockReport()
        {
            var items = DatabaseHelper.GetAllItems()
                .Select(i => new { 
                    i.Name, 
                    النوع = i.Type, 
                    الكمية = i.Quantity, 
                    سعر_الشراء = i.PurchasePrice.ToString("0.00"), 
                    سعر_البيع = i.SalePrice.ToString("0.00") 
                }).ToList();
            dgvReport.DataSource = items;
        }
        
        private void GenerateProfitLossReport()
        {
            var invoices = DatabaseHelper.GetAllInvoices().Where(i => i.Type == "بيع");
            decimal totalSales = invoices.Sum(i => i.Total);
            decimal totalProfit = 0;
            
            foreach (var invoice in invoices)
            {
                var invoiceItems = DatabaseHelper.GetInvoiceItems(invoice.Id);
                foreach (var item in invoiceItems)
                {
                    var originalItem = DatabaseHelper.GetAllItems().FirstOrDefault(i => i.Id == item.ItemId);
                    if (originalItem != null)
                        totalProfit += (item.Price - originalItem.PurchasePrice) * item.Quantity;
                }
            }
            
            var report = new[] { 
                new { البند = "إجمالي المبيعات", المبلغ = totalSales.ToString("0.00") },
                new { البند = "إجمالي الأرباح", المبلغ = totalProfit.ToString("0.00") }
            };
            dgvReport.DataSource = report;
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            btnShowReport.Click += (s, e) => GenerateReport();
            btnExportPdf.Click += (s, e) => MessageBox.Show("تصدير PDF قيد التطوير", "تنبيه");
            btnExportExcel.Click += (s, e) => MessageBox.Show("تصدير Excel قيد التطوير", "تنبيه");
        }
    }
}