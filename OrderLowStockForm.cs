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
    public partial class OrderLowStockForm: Form
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
        }
        private void LoadSelectedProducts()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("QuantityToReorder", typeof(int));
            dt.Columns.Add("UnitPrice", typeof(string)); // Use string to allow "N/A"
            dt.Columns.Add("TotalPrice", typeof(string)); // Same for total

            decimal totalCost = 0;

            foreach (DataRow row in selectedProducts)
            {
                int productId = Convert.ToInt32(row["ProductID"]);
                string name = row["Name"].ToString();
                int quantity = Convert.ToInt32(row["Quantity"]);
                int quantityToReorder = Convert.ToInt32(row["QuantityToReorder"]);
                decimal unitPrice = 0;
                decimal totalPrice = 0;

                // Get UnitPrice from DB
                using (SqlConnection conn = new SqlConnection(@"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;"))
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

                            dt.Rows.Add(productId, name, quantity, quantityToReorder,
                                        unitPrice.ToString("C2"), totalPrice.ToString("C2"));
                        }
                        else
                        {
                            dt.Rows.Add(productId, name, quantity, quantityToReorder,
                                        "N/A", "N/A");
                        }
                    }
                }
            }

            dataGridViewInvoiceItems.DataSource = dt;

            // Allow QuantityToReorder to be editable
            foreach (DataGridViewColumn col in dataGridViewInvoiceItems.Columns)
            {
                col.ReadOnly = true;
            }

            if (dataGridViewInvoiceItems.Columns.Contains("QuantityToReorder"))
            {
                dataGridViewInvoiceItems.Columns["QuantityToReorder"].ReadOnly = false;
            }

            // Update total cost label
            labelTotalCost.Text = $"Total Cost: {totalCost.ToString("C2")}";
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
            decimal total = invoiceTable.AsEnumerable().Sum(r => r.Field<decimal>("TotalPrice"));
            labelTotalCost.Text = $"Total Cost: €{total:F2}";
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Create SalesOrderHeader
                SqlCommand cmdHeader = new SqlCommand(@"
                    INSERT INTO Sales.SalesOrderHeader (OrderDate, DueDate, ShipDate, Status, OnlineOrderFlag, TotalDue)
                    OUTPUT INSERTED.SalesOrderID
                    VALUES (GETDATE(), GETDATE(), GETDATE(), 1, 1, 0)", conn);

                int orderId = (int)cmdHeader.ExecuteScalar();

                // Create SalesOrderDetail lines
                foreach (DataRow row in invoiceTable.Rows)
                {
                    SqlCommand cmdDetail = new SqlCommand(@"
                        INSERT INTO Sales.SalesOrderDetail (SalesOrderID, ProductID, OrderQty, UnitPrice)
                        VALUES (@OrderID, @ProductID, @OrderQty, @UnitPrice)", conn);

                    cmdDetail.Parameters.AddWithValue("@OrderID", orderId);
                    cmdDetail.Parameters.AddWithValue("@ProductID", row["ProductID"]);
                    cmdDetail.Parameters.AddWithValue("@OrderQty", row["QuantityToReorder"]);
                    cmdDetail.Parameters.AddWithValue("@UnitPrice", row["UnitPrice"]);
                    cmdDetail.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Order placed successfully!", "Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
    
}
