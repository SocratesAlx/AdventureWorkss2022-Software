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
    public partial class ReorderProductsForm: Form
    {
        private string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";
        public ReorderProductsForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            UIStyler.StyleButtonsInForm(this);
            GroupBoxStyler.StyleGroupBoxesInForm(this);
            dataGridViewReorderProducts.CellFormatting += dataGridViewReorderProducts_CellFormatting;
            InitializeReorderGrid();
            LoadReorderProducts();
            StyleReorderProductsGrid();
            checkBoxCriticalOnly.CheckedChanged += FiltersChanged;
            checkBoxBuiltInHouse.CheckedChanged += FiltersChanged;
            checkBoxModifiedRecently.CheckedChanged += FiltersChanged;

        }

        private void StyleReorderProductsGrid()
        {
            // Disable default header styles
            dataGridViewReorderProducts.EnableHeadersVisualStyles = false;

            // Header styling
            dataGridViewReorderProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridViewReorderProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridViewReorderProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // Cell styling
            dataGridViewReorderProducts.DefaultCellStyle.BackColor = Color.White;
            dataGridViewReorderProducts.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewReorderProducts.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            // Alternate row coloring
            dataGridViewReorderProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            // Border + grid style
            dataGridViewReorderProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewReorderProducts.GridColor = Color.FromArgb(200, 200, 200);

            // Column sizing
            dataGridViewReorderProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewReorderProducts.AutoResizeColumns();
            dataGridViewReorderProducts.ScrollBars = ScrollBars.Both;

            // Optional: hover effect
            dataGridViewReorderProducts.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    dataGridViewReorderProducts.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
                }
            };

            dataGridViewReorderProducts.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    Color altBack = e.RowIndex % 2 == 0
                        ? Color.White
                        : Color.FromArgb(245, 245, 245);
                    dataGridViewReorderProducts.Rows[e.RowIndex].DefaultCellStyle.BackColor = altBack;
                }
            };
        }

        private void LoadReorderProducts(bool filterCritical = false, bool filterMakeFlag = false, bool filterRecent = false)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                StringBuilder query = new StringBuilder(@"
            SELECT 
                p.ProductID,
                p.Name,
                pi.Quantity,
                p.ReorderPoint,
                p.SafetyStockLevel,
                p.MakeFlag,
                pi.ModifiedDate
            FROM 
                Production.Product p
            JOIN 
                Production.ProductInventory pi ON p.ProductID = pi.ProductID
            WHERE 
                pi.Quantity < p.ReorderPoint
        ");

                if (filterMakeFlag)
                    query.Append(" AND p.MakeFlag = 1");

                if (filterRecent)
                    query.Append(" AND pi.ModifiedDate >= DATEADD(DAY, -30, GETDATE())");

                using (SqlCommand cmd = new SqlCommand(query.ToString(), conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    conn.Open();
                    adapter.Fill(dt);
                }
            }

            // Add editable column
            dt.Columns.Add("QuantityToReorder", typeof(int));
            dt.Columns.Add("Select", typeof(bool));

            foreach (DataRow row in dt.Rows)
            {
                int currentQty = Convert.ToInt32(row["Quantity"]);
                int safetyStock = Convert.ToInt32(row["SafetyStockLevel"]);
                row["QuantityToReorder"] = Math.Max(0, safetyStock - currentQty);
                row["Select"] = false;
            }

            // Apply critical stock filter after loading (not in SQL)
            if (filterCritical)
            {
                dt = dt.AsEnumerable()
                    .Where(r => Convert.ToInt32(r["Quantity"]) < Convert.ToInt32(r["SafetyStockLevel"]) / 3)
                    .CopyToDataTable();
            }

            dataGridViewReorderProducts.DataSource = dt;
        }


        private void InitializeReorderGrid()
{
    dataGridViewReorderProducts.Columns.Clear();

    // Select Checkbox (for batch reorder)
    DataGridViewCheckBoxColumn colSelect = new DataGridViewCheckBoxColumn
    {
        Name = "Select",
        HeaderText = "",
        DataPropertyName = "Select",
        Width = 30
    };

    // ProductID (hidden)
    DataGridViewTextBoxColumn colProductID = new DataGridViewTextBoxColumn
    {
        Name = "ProductID",
        HeaderText = "Product ID",
        DataPropertyName = "ProductID",
        Visible = false
    };

    // Product Name (FILL)
    DataGridViewTextBoxColumn colProductName = new DataGridViewTextBoxColumn
    {
        Name = "ProductName",
        HeaderText = "Product Name",
        DataPropertyName = "Name",
        ReadOnly = true,
        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        MinimumWidth = 150
    };

    // Current Quantity
    DataGridViewTextBoxColumn colQty = new DataGridViewTextBoxColumn
    {
        Name = "Quantity",
        HeaderText = "Current Quantity",
        DataPropertyName = "Quantity",
        ReadOnly = true,
        AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
    };

    // Reorder Point
    DataGridViewTextBoxColumn colReorder = new DataGridViewTextBoxColumn
    {
        Name = "ReorderPoint",
        HeaderText = "Reorder Point",
        DataPropertyName = "ReorderPoint",
        ReadOnly = true,
        AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
    };

    // Safety Stock Level
    DataGridViewTextBoxColumn colSafety = new DataGridViewTextBoxColumn
    {
        Name = "SafetyStockLevel",
        HeaderText = "Safety Stock",
        DataPropertyName = "SafetyStockLevel",
        ReadOnly = true,
        AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
    };

    // MakeFlag (readonly)
    DataGridViewTextBoxColumn colMakeFlag = new DataGridViewTextBoxColumn
    {
        Name = "MakeFlag",
        HeaderText = "Built In-House",
        DataPropertyName = "MakeFlag",
        ReadOnly = true,
        AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
    };

    // Quantity To Reorder (editable)
    DataGridViewTextBoxColumn colQtyToReorder = new DataGridViewTextBoxColumn
    {
        Name = "QuantityToReorder",
        HeaderText = "Qty To Reorder",
        DataPropertyName = "QuantityToReorder",
        AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
    };

    // Add all to grid
    dataGridViewReorderProducts.Columns.AddRange(
        colSelect,
        colProductID,
        colProductName,
        colQty,
        colReorder,
        colSafety,
        colMakeFlag,
        colQtyToReorder
    );
}



        private void dataGridViewReorderProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewReorderProducts.Rows[e.RowIndex].IsNewRow)
                return;

            var row = dataGridViewReorderProducts.Rows[e.RowIndex];

            try
            {
                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                int reorderPoint = Convert.ToInt32(row.Cells["ReorderPoint"].Value);
                int safetyStock = Convert.ToInt32(row.Cells["SafetyStockLevel"].Value);

                int criticalThreshold = safetyStock / 3;

                // 🔴 CRITICAL LOW: Quantity below 1/3 of safety stock
                if (quantity < criticalThreshold)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                // 🟠 LOW STOCK: Quantity below reorder point but not critical
                else if (quantity < reorderPoint)
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                // Optional: log or debug
                Console.WriteLine($"Formatting error on row {e.RowIndex}: {ex.Message}");
            }
        }



        private void ReorderProductsForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            MainForm MainForm = new MainForm();
            MainForm.Show();

            this.Hide();
        }
        private void FiltersChanged(object sender, EventArgs e)
        {
            buttonApplyFilters_Click(sender, e); // Reuse logic
        }



        private void buttonApplyFilters_Click(object sender, EventArgs e)
        {
            bool criticalOnly = checkBoxCriticalOnly.Checked;
            bool builtIn = checkBoxBuiltInHouse.Checked;
            bool recent = checkBoxModifiedRecently.Checked;
            
            InitializeReorderGrid();
            LoadReorderProducts(criticalOnly, builtIn, recent);
        }

        private void buttonReorderSelected_Click(object sender, EventArgs e)
        {
            List<DataRow> selectedRows = new List<DataRow>();

            if (dataGridViewReorderProducts.DataSource is DataTable source)
            {
                foreach (DataGridViewRow row in dataGridViewReorderProducts.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Select"].Value) == true)
                    {
                        int rowIndex = row.Index;
                        selectedRows.Add(source.Rows[rowIndex]);
                    }
                }

                if (selectedRows.Count == 0)
                {
                    MessageBox.Show("Please select at least one product to reorder.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ✅ Create and show the new form
                OrderLowStockForm orderForm = new OrderLowStockForm(selectedRows);
                orderForm.Show();

                this.Hide(); // hide only after showing new form
            }
        }
    }
}
