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
            int extraDays = 7; // Default fallback

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

            // ➕ Βάλε το εδώ για να κληθεί ΜΕΤΑ την πλήρωση δεδομένων
            UpdateDueDateBasedOnShipMethod();
        }

        private void LoadSelectedProducts()
        {
            invoiceTable = new DataTable(); // <-- Make sure it's assigned globally
            invoiceTable.Columns.Add("ProductID", typeof(int));
            invoiceTable.Columns.Add("Name", typeof(string));
            invoiceTable.Columns.Add("Quantity", typeof(int));
            invoiceTable.Columns.Add("QuantityToReorder", typeof(int));
            invoiceTable.Columns.Add("UnitPrice", typeof(decimal));
            invoiceTable.Columns.Add("TotalPrice", typeof(decimal));

            decimal totalCost = 0;

            foreach (DataRow row in selectedProducts)
            {
                int productId = Convert.ToInt32(row["ProductID"]);
                string name = row["Name"].ToString();
                int quantity = Convert.ToInt32(row["Quantity"]);
                int quantityToReorder = Convert.ToInt32(row["QuantityToReorder"]);
                decimal unitPrice = 0;
                decimal totalPrice = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT ListPrice FROM Production.Product WHERE ProductID = @ProductID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        object result = cmd.ExecuteScalar();

                        if (result != DBNull.Value && Convert.ToDecimal(result) > 0)
                        {
                            unitPrice = Convert.ToDecimal(result);
                            totalPrice = unitPrice * quantityToReorder;
                            totalCost += totalPrice;
                        }
                    }
                }

                invoiceTable.Rows.Add(productId, name, quantity, quantityToReorder, unitPrice, totalPrice);
            }

            dataGridViewInvoiceItems.DataSource = invoiceTable;

            // Allow only QuantityToReorder to be editable
            foreach (DataGridViewColumn col in dataGridViewInvoiceItems.Columns)
                col.ReadOnly = true;

            dataGridViewInvoiceItems.Columns["QuantityToReorder"].ReadOnly = false;

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

            // General UI
            grid.EnableHeadersVisualStyles = false;
            grid.BorderStyle = BorderStyle.None;
            grid.RowHeadersVisible = false;

            // Header Style
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersHeight = 35;

            // Cell Style
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.Black;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);

            // Alternating Rows
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255);

            // Borders
            grid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            grid.GridColor = Color.Gainsboro;

            // Columns
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.AutoResizeColumns();
            grid.ScrollBars = ScrollBars.Both;

            // Hover Effect
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
            if (comboBoxVendors.SelectedValue == null ||
        comboBoxEmployees.SelectedValue == null ||
        comboBoxShipMethod.SelectedValue == null)
            {
                MessageBox.Show("Please select Vendor, Employee and Ship Method before confirming the order.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                int vendorId = Convert.ToInt32(comboBoxVendors.SelectedValue);
                int employeeId = Convert.ToInt32(comboBoxEmployees.SelectedValue);
                int shipMethodId = Convert.ToInt32(comboBoxShipMethod.SelectedValue);

                // Dates
                DateTime orderDate = DateTime.Today;
                DateTime.TryParse(textBoxOrderDate.Text, out orderDate);

                DateTime dueDate = orderDate.AddDays(7); // fallback
                DateTime.TryParse(textBoxDueDate.Text, out dueDate);

                // Subtotal
                decimal subtotal = 0;
                foreach (DataGridViewRow row in dataGridViewInvoiceItems.Rows)
                {
                    if (row.IsNewRow) continue;

                    string totalStr = row.Cells["TotalPrice"].Value.ToString().Replace("€", "").Replace("$", "").Trim();
                    if (decimal.TryParse(totalStr, out decimal val))
                        subtotal += val;
                }

                // Tax
                decimal taxAmount = 0;
                decimal.TryParse(textBoxTaxAmount.Text.Replace("€", "").Replace("$", "").Trim(), out taxAmount);

                // 1. Create PurchaseOrderHeader
                SqlCommand cmdHeader = new SqlCommand(@"
            INSERT INTO Purchasing.PurchaseOrderHeader 
            (RevisionNumber, Status, EmployeeID, VendorID, ShipMethodID, OrderDate, ShipDate, SubTotal, TaxAmt, Freight)
            OUTPUT INSERTED.PurchaseOrderID
            VALUES 
            (1, 2, @EmployeeID, @VendorID, @ShipMethodID, @OrderDate, @ShipDate, @SubTotal, @TaxAmt, 0)", conn);

                cmdHeader.Parameters.AddWithValue("@EmployeeID", employeeId);
                cmdHeader.Parameters.AddWithValue("@VendorID", vendorId);
                cmdHeader.Parameters.AddWithValue("@ShipMethodID", shipMethodId);
                cmdHeader.Parameters.AddWithValue("@OrderDate", orderDate);
                cmdHeader.Parameters.AddWithValue("@ShipDate", orderDate);
                cmdHeader.Parameters.AddWithValue("@SubTotal", subtotal);
                cmdHeader.Parameters.AddWithValue("@TaxAmt", taxAmount);

                int poId = (int)cmdHeader.ExecuteScalar();

                // 2. Insert into PurchaseOrderDetail
                foreach (DataGridViewRow row in dataGridViewInvoiceItems.Rows)
                {
                    if (row.IsNewRow) continue;

                    int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                    int qty = Convert.ToInt32(row.Cells["QuantityToReorder"].Value);
                    string unitStr = row.Cells["UnitPrice"].Value.ToString().Replace("€", "").Replace("$", "").Trim();
                    if (!decimal.TryParse(unitStr, out decimal unitPrice)) unitPrice = 0;

                    SqlCommand cmdDetail = new SqlCommand(@"
    INSERT INTO Purchasing.PurchaseOrderDetail 
    (PurchaseOrderID, DueDate, OrderQty, ProductID, UnitPrice, ReceivedQty, RejectedQty)
    VALUES 
    (@POID, @DueDate, @Qty, @ProductID, @UnitPrice, 0, 0)", conn);


                    cmdDetail.Parameters.AddWithValue("@POID", poId);
                    cmdDetail.Parameters.AddWithValue("@DueDate", dueDate);
                    cmdDetail.Parameters.AddWithValue("@Qty", qty);
                    cmdDetail.Parameters.AddWithValue("@ProductID", productId);
                    cmdDetail.Parameters.AddWithValue("@UnitPrice", unitPrice);


                    cmdDetail.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Purchase Order created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MainForm MainForm = new MainForm();
            MainForm.Show();


            this.Hide();
            

        }
    }
}
