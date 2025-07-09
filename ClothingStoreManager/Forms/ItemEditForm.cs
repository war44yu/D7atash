using System.Windows.Forms;
using System.Drawing;
using ClothingStoreManager.Models;

namespace ClothingStoreManager.Forms
{
    public class ItemEditForm : Form
    {
        public Item Item { get; private set; }
        private TextBox txtName, txtType, txtSize, txtColor, txtBrand, txtQuantity, txtPurchase, txtSale;
        private PictureBox picImage;
        private Button btnBrowse, btnSave;
        private OpenFileDialog openFileDialog;
        public ItemEditForm(Item? item = null)
        {
            this.Text = item == null ? "إضافة صنف" : "تعديل صنف";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Size = new Size(500, 600);
            this.Font = new Font("Cairo", 12);
            Item = item ?? new Item();
            InitializeComponents();
            if (item != null) LoadItem();
        }
        private void InitializeComponents()
        {
            txtName = new TextBox() { Width = 300, Text = Item.Name };
            txtType = new TextBox() { Width = 300, Text = Item.Type };
            txtSize = new TextBox() { Width = 300, Text = Item.Size };
            txtColor = new TextBox() { Width = 300, Text = Item.Color };
            txtBrand = new TextBox() { Width = 300, Text = Item.Brand };
            txtQuantity = new TextBox() { Width = 300, Text = Item.Quantity.ToString() };
            txtPurchase = new TextBox() { Width = 300, Text = Item.PurchasePrice.ToString() };
            txtSale = new TextBox() { Width = 300, Text = Item.SalePrice.ToString() };
            picImage = new PictureBox() { Width = 120, Height = 120, BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            if (!string.IsNullOrEmpty(Item.ImagePath) && System.IO.File.Exists(Item.ImagePath))
                picImage.Image = Image.FromFile(Item.ImagePath);
            btnBrowse = new Button() { Text = "اختيار صورة", Width = 120 };
            btnBrowse.Click += (s, e) =>
            {
                openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "صور|*.jpg;*.png;*.jpeg";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    picImage.Image = Image.FromFile(openFileDialog.FileName);
                    Item.ImagePath = openFileDialog.FileName;
                }
            };
            btnSave = new Button() { Text = "حفظ", Width = 120 };
            btnSave.Click += (s, e) =>
            {
                Item.Name = txtName.Text;
                Item.Type = txtType.Text;
                Item.Size = txtSize.Text;
                Item.Color = txtColor.Text;
                Item.Brand = txtBrand.Text;
                int.TryParse(txtQuantity.Text, out int q); Item.Quantity = q;
                decimal.TryParse(txtPurchase.Text, out decimal pp); Item.PurchasePrice = pp;
                decimal.TryParse(txtSale.Text, out decimal sp); Item.SalePrice = sp;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            TableLayoutPanel table = new TableLayoutPanel() { Dock = DockStyle.Fill, RowCount = 9, ColumnCount = 2, AutoSize = true };
            table.Controls.Add(new Label() { Text = "الاسم:" }, 0, 0); table.Controls.Add(txtName, 1, 0);
            table.Controls.Add(new Label() { Text = "النوع:" }, 0, 1); table.Controls.Add(txtType, 1, 1);
            table.Controls.Add(new Label() { Text = "المقاس:" }, 0, 2); table.Controls.Add(txtSize, 1, 2);
            table.Controls.Add(new Label() { Text = "اللون:" }, 0, 3); table.Controls.Add(txtColor, 1, 3);
            table.Controls.Add(new Label() { Text = "الماركة:" }, 0, 4); table.Controls.Add(txtBrand, 1, 4);
            table.Controls.Add(new Label() { Text = "الكمية:" }, 0, 5); table.Controls.Add(txtQuantity, 1, 5);
            table.Controls.Add(new Label() { Text = "سعر الشراء:" }, 0, 6); table.Controls.Add(txtPurchase, 1, 6);
            table.Controls.Add(new Label() { Text = "سعر البيع:" }, 0, 7); table.Controls.Add(txtSale, 1, 7);
            table.Controls.Add(picImage, 0, 8); table.Controls.Add(btnBrowse, 1, 8);
            FlowLayoutPanel panel = new FlowLayoutPanel() { Dock = DockStyle.Bottom, Height = 60, FlowDirection = FlowDirection.RightToLeft };
            panel.Controls.Add(btnSave);
            this.Controls.Add(table);
            this.Controls.Add(panel);
        }
        private void LoadItem()
        {
            txtName.Text = Item.Name;
            txtType.Text = Item.Type;
            txtSize.Text = Item.Size;
            txtColor.Text = Item.Color;
            txtBrand.Text = Item.Brand;
            txtQuantity.Text = Item.Quantity.ToString();
            txtPurchase.Text = Item.PurchasePrice.ToString();
            txtSale.Text = Item.SalePrice.ToString();
            if (!string.IsNullOrEmpty(Item.ImagePath) && System.IO.File.Exists(Item.ImagePath))
                picImage.Image = Image.FromFile(Item.ImagePath);
        }
    }
}