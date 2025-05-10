using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SokProodos
{
    public partial class OrderLowStockForm : Form
    {
        private List<DataRow> selectedProducts;
        private DataTable invoiceTable;
        private string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";
        public OrderLowStockForm(List<DataRow> selectedProducts)
        {
            InitializeComponent();
            comboBoxVendors.Visible = false;
            label1.Visible = false; 
            this.selectedProducts = selectedProducts;
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadSelectedProducts();
            UIStyler.StyleButtonsInForm(this);
            GroupBoxStyler.StyleGroupBoxesInForm(this);
            StyleInvoiceGrid();
            this.Load += OrderLowStockForm_Load;
            SetOrderDate();
            UpdateDueDateBasedOnShipMethod();
            this.comboBoxShipMethod.SelectedIndexChanged += new System.EventHandler(this.comboBoxShipMethod_SelectedIndexChanged);
            this.textBoxTaxAmount.TextChanged += new System.EventHandler(this.textBoxTaxAmount_TextChanged);
            dataGridViewInvoiceItems.CellValueChanged += dataGridViewInvoiceItems_CellValueChanged;
        }
        private void OrderLowStockForm_Load(object sender, EventArgs e)
        {
            LoadVendors();
            LoadEmployees();
            LoadShipMethods();
        }

        private void SetOrderDate()
        {
            textBoxOrderDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }
        private void UpdateDueDateBasedOnShipMethod()
        {
            int extraDays = 7; 

            if (comboBoxShipMethod.SelectedItem is DataRowView drv)
            {
                string method = drv["Name"].ToString().ToLower();

                if (method.Contains("express"))
                    extraDays = 3;
                else if (method.Contains("standard"))
                    extraDays = 10;
                else if (method.Contains("overnight"))
                    extraDays = 1;
            }

            DateTime orderDate;
            if (!DateTime.TryParse(textBoxOrderDate.Text, out orderDate))
            {
                orderDate = DateTime.Today;
            }

            textBoxDueDate.Text = orderDate.AddDays(extraDays).ToString("yyyy-MM-dd");
        }

        private void comboBoxShipMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDueDateBasedOnShipMethod();
        }

        private void LoadVendors()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT BusinessEntityID, Name FROM Purchasing.Vendor", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboBoxVendors.DataSource = dt;
                comboBoxVendors.DisplayMember = "Name";
                comboBoxVendors.ValueMember = "BusinessEntityID";
            }
        }

        private void LoadEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT BusinessEntityID, JobTitle FROM HumanResources.Employee", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboBoxEmployees.DataSource = dt;
                comboBoxEmployees.DisplayMember = "JobTitle";
                comboBoxEmployees.ValueMember = "BusinessEntityID";
            }
        }

        private void LoadShipMethods()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT ShipMethodID, Name FROM Purchasing.ShipMethod", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboBoxShipMethod.DataSource = dt;
                comboBoxShipMethod.DisplayMember = "Name";
                comboBoxShipMethod.ValueMember = "ShipMethodID";
            }

            
            UpdateDueDateBasedOnShipMethod();
        }

        private void LoadSelectedProducts()
        {
            invoiceTable = new DataTable();
            invoiceTable.Columns.Add("ProductID", typeof(int));
            invoiceTable.Columns.Add("Name", typeof(string));
            invoiceTable.Columns.Add("VendorID", typeof(int)); // ✅ New column
            invoiceTable.Columns.Add("VendorName", typeof(string));
            invoiceTable.Columns.Add("Quantity", typeof(int));
            invoiceTable.Columns.Add("QuantityToReorder", typeof(int));
            invoiceTable.Columns.Add("UnitPrice", typeof(decimal));
            invoiceTable.Columns.Add("TotalPrice", typeof(decimal));

            decimal totalCost = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataRow row in selectedProducts)
                {
                    int productId = Convert.ToInt32(row["ProductID"]);
                    string name = row["Name"].ToString();
                    int quantity = Convert.ToInt32(row["Quantity"]);
                    int quantityToReorder = Convert.ToInt32(row["QuantityToReorder"]);
                    decimal unitPrice = 0;
                    string vendorName = "Unknown Vendor";
                    int vendorId = -1;

                    // Get unit price
                    using (SqlCommand cmdPrice = new SqlCommand("SELECT ListPrice FROM Production.Product WHERE ProductID = @ProductID", conn))
                    {
                        cmdPrice.Parameters.AddWithValue("@ProductID", productId);
                        object priceResult = cmdPrice.ExecuteScalar();
                        if (priceResult != DBNull.Value)
                        {
                            unitPrice = Convert.ToDecimal(priceResult);
                        }
                    }

                    // Get vendor info
                    using (SqlCommand cmdVendor = new SqlCommand(@"
                SELECT TOP 1 v.BusinessEntityID, v.Name
                FROM Purchasing.ProductVendor pv
                INNER JOIN Purchasing.Vendor v ON pv.BusinessEntityID = v.BusinessEntityID
                WHERE pv.ProductID = @ProductID
                ORDER BY pv.StandardPrice ASC", conn))
                    {
                        cmdVendor.Parameters.AddWithValue("@ProductID", productId);

                        using (SqlDataReader reader = cmdVendor.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                vendorId = Convert.ToInt32(reader["BusinessEntityID"]);
                                vendorName = reader["Name"].ToString();
                            }
                        }
                    }

                    decimal totalPrice = unitPrice * quantityToReorder;
                    totalCost += totalPrice;

                    invoiceTable.Rows.Add(productId, name, vendorId, vendorName, quantity, quantityToReorder, unitPrice, totalPrice);
                }
            }

            dataGridViewInvoiceItems.DataSource = invoiceTable;

            // Set column visibility and readonly status
            foreach (DataGridViewColumn col in dataGridViewInvoiceItems.Columns)
                col.ReadOnly = true;

            dataGridViewInvoiceItems.Columns["QuantityToReorder"].ReadOnly = false;

            // Optional: Hide VendorID column from UI
            if (dataGridViewInvoiceItems.Columns.Contains("VendorID"))
                dataGridViewInvoiceItems.Columns["VendorID"].Visible = false;

            UpdateTotalCostLabel();
        }





        private void dataGridViewInvoiceItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridViewInvoiceItems.Columns[e.ColumnIndex].Name == "QuantityToReorder")
            {
                var row = invoiceTable.Rows[e.RowIndex];

                int qty = Convert.ToInt32(row["QuantityToReorder"]);
                decimal unitPrice = Convert.ToDecimal(row["UnitPrice"]);
                row["TotalPrice"] = qty * unitPrice;

                UpdateTotalCostLabel();
            }
        }


        private void UpdateTotalCostLabel()
        {
            decimal subtotal = invoiceTable.AsEnumerable()
                .Sum(r => r.Field<decimal>("TotalPrice"));

            decimal tax = 0;
            if (!string.IsNullOrWhiteSpace(textBoxTaxAmount.Text))
                decimal.TryParse(textBoxTaxAmount.Text.Replace("€", "").Replace("$", "").Trim(), out tax);

            decimal finalTotal = subtotal + tax;
            labelTotalCost.Text = $"Total Cost: {finalTotal.ToString("C2")}";
        }


        private void textBoxTaxAmount_TextChanged(object sender, EventArgs e)
        {
            UpdateTotalCostLabel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReorderProductsForm reorderForm = new ReorderProductsForm();
            reorderForm.Show();

            this.Hide();
        }

        private void StyleInvoiceGrid()
        {
            var grid = dataGridViewInvoiceItems;

            
            grid.EnableHeadersVisualStyles = false;
            grid.BorderStyle = BorderStyle.None;
            grid.RowHeadersVisible = false;

            
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersHeight = 35;

            
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.Black;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);

            
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255);

            
            grid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            grid.GridColor = Color.Gainsboro;

            
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.AutoResizeColumns();
            grid.ScrollBars = ScrollBars.Both;

            
            grid.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(235, 240, 255);
            };

            grid.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    Color altBack = e.RowIndex % 2 == 0
                        ? Color.White
                        : Color.FromArgb(245, 248, 255);
                    grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = altBack;
                }
            };
        }


        private void buttonConfirmOrder_Click(object sender, EventArgs e)
        {
            if (comboBoxEmployees.SelectedValue == null || comboBoxShipMethod.SelectedValue == null)
            {
                MessageBox.Show("Please select Employee and Ship Method before confirming the order.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                int employeeId = Convert.ToInt32(comboBoxEmployees.SelectedValue);
                int shipMethodId = Convert.ToInt32(comboBoxShipMethod.SelectedValue);
                DateTime orderDate = DateTime.Today;
                DateTime.TryParse(textBoxOrderDate.Text, out orderDate);
                DateTime dueDate = orderDate.AddDays(7);
                DateTime.TryParse(textBoxDueDate.Text, out dueDate);
                decimal.TryParse(textBoxTaxAmount.Text.Replace("€", "").Replace("$", "").Trim(), out decimal taxAmount);

                var groupedByVendor = invoiceTable.AsEnumerable()
                    .GroupBy(r => r.Field<int>("VendorID"));

                foreach (var vendorGroup in groupedByVendor)
                {
                    int vendorId = vendorGroup.Key;
                    decimal subtotal = vendorGroup.Sum(r => r.Field<decimal>("TotalPrice"));

                    SqlCommand cmdHeader = new SqlCommand(@"
                INSERT INTO Purchasing.PurchaseOrderHeader 
                (RevisionNumber, Status, EmployeeID, VendorID, ShipMethodID, OrderDate, ShipDate, SubTotal, TaxAmt, Freight)
                OUTPUT INSERTED.PurchaseOrderID
                VALUES (1, 1, @EmployeeID, @VendorID, @ShipMethodID, @OrderDate, @ShipDate, @SubTotal, @TaxAmt, 0)", conn);

                    cmdHeader.Parameters.AddWithValue("@EmployeeID", employeeId);
                    cmdHeader.Parameters.AddWithValue("@VendorID", vendorId);
                    cmdHeader.Parameters.AddWithValue("@ShipMethodID", shipMethodId);
                    cmdHeader.Parameters.AddWithValue("@OrderDate", orderDate);
                    cmdHeader.Parameters.AddWithValue("@ShipDate", orderDate);
                    cmdHeader.Parameters.AddWithValue("@SubTotal", subtotal);
                    cmdHeader.Parameters.AddWithValue("@TaxAmt", taxAmount);

                    int poId = (int)cmdHeader.ExecuteScalar();

                    foreach (var row in vendorGroup)
                    {
                        int productId = row.Field<int>("ProductID");
                        int qty = row.Field<int>("QuantityToReorder");
                        decimal unitPrice = row.Field<decimal>("UnitPrice");

                        if (qty <= 0) continue;

                        SqlCommand cmdDetail = new SqlCommand(@"
                    INSERT INTO Purchasing.PurchaseOrderDetail 
                    (PurchaseOrderID, DueDate, OrderQty, ProductID, UnitPrice, ReceivedQty, RejectedQty)
                    VALUES (@POID, @DueDate, @Qty, @ProductID, @UnitPrice, 0, 0)", conn);

                        cmdDetail.Parameters.AddWithValue("@POID", poId);
                        cmdDetail.Parameters.AddWithValue("@DueDate", dueDate);
                        cmdDetail.Parameters.AddWithValue("@Qty", qty);
                        cmdDetail.Parameters.AddWithValue("@ProductID", productId);
                        cmdDetail.Parameters.AddWithValue("@UnitPrice", unitPrice);
                        cmdDetail.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Purchase Orders created per supplier successfully and awaiting approval!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MainForm MainForm = new MainForm();
            MainForm.Show();
            this.Hide();
        }



        }
}
