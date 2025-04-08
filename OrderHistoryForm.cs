using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SokProodos
{
    public partial class OrderHistoryForm: Form
    {
        private string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";
        public OrderHistoryForm()
        {
            InitializeComponent();
            LoadFilters();
            LoadOrderHistory();
            ApplyModernDataGridStyle();
            this.StartPosition = FormStartPosition.CenterScreen;
            UIStyler.StyleButtonsInForm(this);



            comboBoxYear.SelectedIndexChanged += new EventHandler(FilterOrders);
            comboBoxMonth.SelectedIndexChanged += new EventHandler(FilterOrders);
            comboBoxCustomer.SelectedIndexChanged += new EventHandler(FilterOrders);
            comboBoxProduct.SelectedIndexChanged += new EventHandler(FilterOrders);
            GroupBoxStyler.StyleGroupBoxesInForm(this);

        }

        private void ApplyModernDataGridStyle()
        {
            dataGridViewOrderHistory.AllowUserToAddRows = false;  // Disables adding new rows
            dataGridViewOrderHistory.BorderStyle = BorderStyle.None;  // Remove default borders
            dataGridViewOrderHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;  // Set horizontal borders between rows

            // Row styling
            dataGridViewOrderHistory.RowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);  // Light background for rows
            dataGridViewOrderHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(233, 236, 239);  // Lighter background for alternating rows
            dataGridViewOrderHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(88, 101, 242);  // Selection color
            dataGridViewOrderHistory.DefaultCellStyle.SelectionForeColor = Color.White;  // White text on selection

            // Header styling
            dataGridViewOrderHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204);  // Correct header background color
            dataGridViewOrderHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;  // Header text color
            dataGridViewOrderHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);  // Header font
            dataGridViewOrderHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;  // Auto size headers

            // Enabling horizontal scrolling
            dataGridViewOrderHistory.ScrollBars = ScrollBars.Both;  // Enable both vertical and horizontal scroll bars

            // Column style settings
            foreach (DataGridViewColumn column in dataGridViewOrderHistory.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;  // Left-align cell content
            }

            // Resize columns automatically to fit content
            dataGridViewOrderHistory.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }



        private void LoadFilters()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Year Filter
                    string yearQuery = "SELECT DISTINCT YEAR(OrderDate) AS OrderYear FROM Sales.SalesOrderHeader ORDER BY OrderYear DESC;";
                    using (SqlCommand command = new SqlCommand(yearQuery, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        comboBoxYear.Items.Add("All");
                        while (reader.Read()) comboBoxYear.Items.Add(reader.GetInt32(0).ToString());
                    }

                    // Month Filter
                    comboBoxMonth.Items.Add("All");
                    string[] months = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                    for (int i = 0; i < months.Length - 1; i++)
                    {
                        comboBoxMonth.Items.Add(months[i]);
                    }

                    // Customer Filter
                    string customerQuery = @"
                SELECT DISTINCT c.CustomerID, ISNULL(p.FirstName + ' ' + p.LastName, s.Name) AS CustomerName
                FROM Sales.Customer c
                LEFT JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
                LEFT JOIN Sales.Store s ON c.StoreID = s.BusinessEntityID
                ORDER BY CustomerName ASC;";

                    using (SqlCommand command = new SqlCommand(customerQuery, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        comboBoxCustomer.Items.Add("All");
                        while (reader.Read())
                        {
                            comboBoxCustomer.Items.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                        }
                    }

                    // Product Filter
                    string productQuery = "SELECT ProductID, Name FROM Production.Product ORDER BY Name ASC";
                    using (SqlCommand command = new SqlCommand(productQuery, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        comboBoxProduct.Items.Add("All");
                        while (reader.Read())
                        {
                            comboBoxProduct.Items.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                        }
                    }

                    // Display settings
                    comboBoxCustomer.DisplayMember = "Value";
                    comboBoxCustomer.ValueMember = "Key";

                    comboBoxProduct.DisplayMember = "Value";
                    comboBoxProduct.ValueMember = "Key";

                    comboBoxYear.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxYear.AutoCompleteSource = AutoCompleteSource.ListItems;

                    comboBoxMonth.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxMonth.AutoCompleteSource = AutoCompleteSource.ListItems;

                    comboBoxCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;

                    comboBoxProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxProduct.AutoCompleteSource = AutoCompleteSource.ListItems;

                    // Default values
                    comboBoxYear.SelectedIndex = 0;
                    comboBoxMonth.SelectedIndex = 0;
                    comboBoxCustomer.SelectedIndex = 0;
                    comboBoxProduct.SelectedIndex = 0;

                    // Attach event
                    comboBoxProduct.SelectedIndexChanged += new EventHandler(FilterOrders);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading filters: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void Button_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(114, 137, 218); // Lighter blue on hover
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(88, 101, 242); // Normal state
        }

        private void LoadOrderHistory()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                SELECT 
                    soh.SalesOrderID AS 'Order ID',
                    soh.OrderDate AS 'Order Date',
                    soh.DueDate AS 'Due Date',
                    c.CustomerID AS 'Customer ID',
                    ISNULL(p.FirstName + ' ' + p.LastName, s.Name) AS 'Customer Name',
                    sp.BusinessEntityID AS 'Seller ID',
                    per.FirstName + ' ' + per.LastName AS 'Seller Name',
                    soh.TotalDue AS 'Total Amount',
                    sm.Name AS 'Shipping Method',
                    soh.BillToAddressID AS 'Billing Address ID',
                    a.AddressLine1 + ', ' + a.City AS 'Billing Address',
                    sod.SpecialOfferID AS 'Special Offer ID',
                    so.Description AS 'Special Offer',
                    sod.OrderQty AS 'Order Quantity',
                    sod.UnitPrice AS 'Unit Price',
                    (sod.UnitPrice * sod.OrderQty) AS 'Total Price',
                    sod.ProductID AS 'Product ID',
                    pr.Name AS 'Product Name',
                    CASE 
                        WHEN oa.Approved = 1 THEN 'Approved'
                        WHEN oa.Approved = 0 THEN 'Rejected'
                        ELSE 'Pending'
                    END AS 'Approval Status'
                FROM Sales.SalesOrderHeader soh
                JOIN Sales.Customer c ON soh.CustomerID = c.CustomerID
                LEFT JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
                LEFT JOIN Sales.Store s ON c.StoreID = s.BusinessEntityID
                LEFT JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
                LEFT JOIN Person.Person per ON sp.BusinessEntityID = per.BusinessEntityID
                LEFT JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
                LEFT JOIN Sales.SpecialOffer so ON sod.SpecialOfferID = so.SpecialOfferID
                LEFT JOIN Purchasing.ShipMethod sm ON soh.ShipMethodID = sm.ShipMethodID
                LEFT JOIN Person.Address a ON soh.BillToAddressID = a.AddressID
                LEFT JOIN Production.Product pr ON sod.ProductID = pr.ProductID
                LEFT JOIN OrderApprovals oa ON soh.SalesOrderID = oa.SalesOrderID
                ORDER BY soh.OrderDate DESC;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridViewOrderHistory.DataSource = dataTable;
                        ApplyApprovalStatusColors(); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading order history: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void FilterOrders(object sender, EventArgs e)
        {
            string yearFilter = comboBoxYear.SelectedItem.ToString();
            string monthFilter = comboBoxMonth.SelectedItem.ToString();
            var customerFilter = comboBoxCustomer.SelectedItem;
            var productFilter = comboBoxProduct.SelectedItem;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                SELECT 
                    soh.SalesOrderID AS 'Order ID',
                    soh.OrderDate AS 'Order Date',
                    soh.DueDate AS 'Due Date',
                    c.CustomerID AS 'Customer ID',
                    ISNULL(p.FirstName + ' ' + p.LastName, s.Name) AS 'Customer Name',
                    sp.BusinessEntityID AS 'Seller ID',
                    per.FirstName + ' ' + per.LastName AS 'Seller Name',
                    soh.TotalDue AS 'Total Amount',
                    sm.Name AS 'Shipping Method',
                    soh.BillToAddressID AS 'Billing Address ID',
                    a.AddressLine1 + ', ' + a.City AS 'Billing Address',
                    sod.SpecialOfferID AS 'Special Offer ID',
                    so.Description AS 'Special Offer',
                    sod.OrderQty AS 'Order Quantity',
                    sod.UnitPrice AS 'Unit Price',
                    (sod.UnitPrice * sod.OrderQty) AS 'Total Price',
                    sod.ProductID AS 'Product ID',
                    pr.Name AS 'Product Name',
                    oa.Approved AS 'Approval Status'
                FROM Sales.SalesOrderHeader soh
                JOIN Sales.Customer c ON soh.CustomerID = c.CustomerID
                LEFT JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
                LEFT JOIN Sales.Store s ON c.StoreID = s.BusinessEntityID
                LEFT JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
                LEFT JOIN Person.Person per ON sp.BusinessEntityID = per.BusinessEntityID
                LEFT JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
                LEFT JOIN Sales.SpecialOffer so ON sod.SpecialOfferID = so.SpecialOfferID
                LEFT JOIN Purchasing.ShipMethod sm ON soh.ShipMethodID = sm.ShipMethodID
                LEFT JOIN Person.Address a ON soh.BillToAddressID = a.AddressID
                LEFT JOIN Production.Product pr ON sod.ProductID = pr.ProductID
                LEFT JOIN OrderApprovals oa ON soh.SalesOrderID = oa.SalesOrderID
                WHERE 1=1";

                    List<SqlParameter> parameters = new List<SqlParameter>();

                    if (yearFilter != "All")
                    {
                        query += " AND YEAR(soh.OrderDate) = @Year";
                        parameters.Add(new SqlParameter("@Year", int.Parse(yearFilter)));
                    }

                    if (monthFilter != "All")
                    {
                        int monthValue = DateTime.ParseExact(monthFilter, "MMMM", null).Month;
                        query += " AND MONTH(soh.OrderDate) = @Month";
                        parameters.Add(new SqlParameter("@Month", monthValue));
                    }

                    if (customerFilter != null && customerFilter.ToString() != "All")
                    {
                        int customerId = ((KeyValuePair<int, string>)customerFilter).Key;
                        query += " AND c.CustomerID = @CustomerID";
                        parameters.Add(new SqlParameter("@CustomerID", customerId));
                    }

                    if (productFilter != null && productFilter.ToString() != "All")
                    {
                        int productId = ((KeyValuePair<int, string>)productFilter).Key;
                        query += " AND sod.ProductID = @ProductID";
                        parameters.Add(new SqlParameter("@ProductID", productId));
                    }

                    query += " ORDER BY soh.OrderDate DESC;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters.ToArray());

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridViewOrderHistory.DataSource = dt;
                            ApplyApprovalStatusColors(); // ➕ προσθήκη εδώ
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error filtering orders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void ApplyApprovalStatusColors()
        {
            foreach (DataGridViewRow row in dataGridViewOrderHistory.Rows)
            {
                if (row.Cells["Approval Status"].Value != null)
                {
                    string status = row.Cells["Approval Status"].Value.ToString().Trim().ToLower();

                    switch (status)
                    {
                        case "approved":
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                            break;
                        case "rejected":
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                            break;
                        case "pending":
                        default:
                            row.DefaultCellStyle.BackColor = Color.LightGray;
                            break;
                    }
                }
            }
        }




        private void buttonClose_Click_Click(object sender, EventArgs e)
        {
            MainForm MainForm = new MainForm();
            MainForm.Show();


            this.Hide();
        }

        private void dataGridViewOrderHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
