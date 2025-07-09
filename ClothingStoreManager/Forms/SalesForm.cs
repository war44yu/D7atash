using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Helpers;
using ClothingStoreManager.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ClothingStoreManager.Forms
{
    public class SalesForm : Form
    {
        private DataGridView dgvInvoiceItems;
        private Button btnAddItem, btnRemoveItem, btnPrint, btnOpenDrawer, btnSaveInvoice;
        private ComboBox cmbCustomers;
        private Label lblTotal, lblTitle;
        private List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
        private List<Customer> customers = new List<Customer>();
        private List<Item> items = new List<Item>();
        private decimal total = 0;

        public SalesForm()
        {
            this.Text = "المبيعات وإصدار الفواتير";
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
                Text = "إصدار فاتورة بيع",
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Cairo", 16, FontStyle.Bold)
            };

            dgvInvoiceItems = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAddItem = new Button() { Text = "إضافة صنف", Width = 120 };
            btnRemoveItem = new Button() { Text = "حذف صنف", Width = 100 };
            btnPrint = new Button() { Text = "طباعة الفاتورة", Width = 120 };
            btnOpenDrawer = new Button() { Text = "فتح الدرج", Width = 100 };
            btnSaveInvoice = new Button() { Text = "حفظ الفاتورة", Width = 120 };
            cmbCustomers = new ComboBox() { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            lblTotal = new Label() { Text = "الإجمالي: 0.00 جنيه", AutoSize = true, Font = new Font("Cairo", 14, FontStyle.Bold) };

            FlowLayoutPanel panel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                Height = 60,
                FlowDirection = FlowDirection.RightToLeft
            };
            panel.Controls.AddRange(new Control[] { btnAddItem, btnRemoveItem, btnSaveInvoice, btnPrint, btnOpenDrawer, cmbCustomers, lblTotal });

            this.Controls.Add(dgvInvoiceItems);
            this.Controls.Add(panel);
            this.Controls.Add(lblTitle);
            this.Load += SalesForm_Load;
        }

        private void LoadData()
        {
            customers = DatabaseHelper.GetAllCustomers();
            items = DatabaseHelper.GetAllItems();
            cmbCustomers.Items.Clear();
            cmbCustomers.Items.Add("عميل عادي");
            foreach (var customer in customers)
                cmbCustomers.Items.Add($"{customer.Name} - {customer.Phone}");
            cmbCustomers.SelectedIndex = 0;
        }

        private void UpdateTotal()
        {
            total = invoiceItems.Sum(i => i.Total);
            lblTotal.Text = $"الإجمالي: {total:0.00} جنيه";
        }

        private void RefreshInvoiceGrid()
        {
            dgvInvoiceItems.DataSource = null;
            dgvInvoiceItems.DataSource = invoiceItems.ToList();
            UpdateTotal();
        }

        private void SalesForm_Load(object sender, EventArgs e)
        {
            LoadData();
            RefreshInvoiceGrid();
            
            btnAddItem.Click += (s, e) =>
            {
                var form = new AddItemToInvoiceForm(items);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    invoiceItems.Add(form.InvoiceItem);
                    RefreshInvoiceGrid();
                }
            };
            
            btnRemoveItem.Click += (s, e) =>
            {
                if (dgvInvoiceItems.CurrentRow != null)
                {
                    var item = (InvoiceItem)dgvInvoiceItems.CurrentRow.DataBoundItem;
                    invoiceItems.Remove(item);
                    RefreshInvoiceGrid();
                }
            };
            
            btnSaveInvoice.Click += (s, e) =>
            {
                if (invoiceItems.Count == 0)
                {
                    MessageBox.Show("يجب إضافة أصناف للفاتورة!", "تنبيه");
                    return;
                }
                var invoice = new Invoice()
                {
                    Date = DateTime.Now,
                    CustomerId = cmbCustomers.SelectedIndex > 0 ? customers[cmbCustomers.SelectedIndex - 1].Id : null,
                    Total = total,
                    Type = "بيع"
                };
                int invoiceId = DatabaseHelper.AddInvoice(invoice);
                foreach (var item in invoiceItems)
                {
                    DatabaseHelper.AddInvoiceItem(invoiceId, item);
                    DatabaseHelper.UpdateItemQuantity(item.ItemId, -item.Quantity);
                }
                MessageBox.Show($"تم حفظ الفاتورة رقم {invoiceId} بنجاح!", "نجح");
                invoiceItems.Clear();
                RefreshInvoiceGrid();
            };
            
            btnPrint.Click += (s, e) =>
            {
                if (invoiceItems.Count > 0)
                {
                    var invoice = new Invoice() { Id = 0, Date = DateTime.Now, Total = total };
                    var customer = cmbCustomers.SelectedIndex > 0 ? customers[cmbCustomers.SelectedIndex - 1] : null;
                    string invoiceText = InvoiceTemplate.GenerateInvoiceText(invoice, customer, invoiceItems);
                    PrinterHelper.PrintText(invoiceText);
                }
            };
            
            btnOpenDrawer.Click += (s, e) =>
            {
                PrinterHelper.OpenCashDrawer();
                MessageBox.Show("تم فتح درج الكاشير!", "تم");
            };
        }
    }
}