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
    public partial class ReorderProductsForm : Form
    {
        private string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";
        private DataTable productTable; // Add this at the top of the class
        private List<string> allProductNames = new List<string>();



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
            comboBoxSearchProducts.DropDownStyle = ComboBoxStyle.DropDown;
            comboBoxSearchProducts.TextChanged += ComboBoxSearchProducts_TextChanged;
        }

        private void ComboBoxSearchProducts_TextChanged(object sender, EventArgs e)
        {
            string typedText = comboBoxSearchProducts.Text;

            if (string.IsNullOrWhiteSpace(typedText))
            {
                dataGridViewReorderProducts.DataSource = productTable;
                UpdateSearchComboBoxItems(allProductNames);
                return;
            }

            var filteredRows = productTable.AsEnumerable()
                .Where(r => r.Field<string>("Name").IndexOf(typedText, StringComparison.OrdinalIgnoreCase) >= 0);

            if (filteredRows.Any())
            {
                dataGridViewReorderProducts.DataSource = filteredRows.CopyToDataTable();

                // Update dropdown items too
                var filteredNames = filteredRows
                    .Select(r => r.Field<string>("Name"))
                    .Distinct()
                    .ToList();

                UpdateSearchComboBoxItems(filteredNames);
            }
            else
            {
                dataGridViewReorderProducts.DataSource = productTable.Clone();
                comboBoxSearchProducts.DroppedDown = false;
            }
        }


        private void StyleReorderProductsGrid()
        {
            // Basic UI tweaks
            dataGridViewReorderProducts.EnableHeadersVisualStyles = false;
            dataGridViewReorderProducts.BorderStyle = BorderStyle.None;
            dataGridViewReorderProducts.RowHeadersVisible = false;

            // Header style
            dataGridViewReorderProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dataGridViewReorderProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);
            dataGridViewReorderProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dataGridViewReorderProducts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewReorderProducts.ColumnHeadersHeight = 35;

            // Cell style
            dataGridViewReorderProducts.DefaultCellStyle.BackColor = Color.White;
            dataGridViewReorderProducts.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewReorderProducts.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            dataGridViewReorderProducts.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewReorderProducts.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);

            // Alternating rows
            dataGridViewReorderProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255);

            // Grid & borders
            dataGridViewReorderProducts.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridViewReorderProducts.GridColor = Color.Gainsboro;

            // Columns
            dataGridViewReorderProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewReorderProducts.AutoResizeColumns();
            dataGridViewReorderProducts.ScrollBars = ScrollBars.Both;

            // Hover effect
            dataGridViewReorderProducts.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    dataGridViewReorderProducts.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(235, 240, 255);
                }
            };

            dataGridViewReorderProducts.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    Color altBack = e.RowIndex % 2 == 0
                        ? Color.White
                        : Color.FromArgb(245, 248, 255);
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

            // Add extra columns
            dt.Columns.Add("QuantityToReorder", typeof(int));
            dt.Columns.Add("Select", typeof(bool));

            foreach (DataRow row in dt.Rows)
            {
                int currentQty = Convert.ToInt32(row["Quantity"]);
                int safetyStock = Convert.ToInt32(row["SafetyStockLevel"]);
                row["QuantityToReorder"] = Math.Max(0, safetyStock - currentQty);
                row["Select"] = false;
            }

            if (filterCritical)
            {
                dt = dt.AsEnumerable()
                    .Where(r => Convert.ToInt32(r["Quantity"]) < Convert.ToInt32(r["SafetyStockLevel"]) / 3)
                    .CopyToDataTable();
            }

            dataGridViewReorderProducts.DataSource = dt;
            productTable = dt;

            // Initialize ComboBox search list
            allProductNames = dt.AsEnumerable()
                .Select(r => r.Field<string>("Name"))
                .Distinct()
                .ToList();

            UpdateSearchComboBoxItems(allProductNames);
        }

        private void UpdateSearchComboBoxItems(List<string> items)
        {
            string currentText = comboBoxSearchProducts.Text;

            comboBoxSearchProducts.TextChanged -= ComboBoxSearchProducts_TextChanged;
            comboBoxSearchProducts.Items.Clear();
            comboBoxSearchProducts.Items.AddRange(items.ToArray());
            comboBoxSearchProducts.Text = currentText;
            comboBoxSearchProducts.SelectionStart = currentText.Length;
            comboBoxSearchProducts.SelectionLength = 0;
            Cursor.Current = Cursors.Default;
            comboBoxSearchProducts.TextChanged += ComboBoxSearchProducts_TextChanged;
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

                // Quantity below 1/3 of safety stock
                if (quantity < criticalThreshold)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                // Quantity below reorder point but not critical
                else if (quantity < reorderPoint)
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                // log or debug
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


                OrderLowStockForm orderForm = new OrderLowStockForm(selectedRows);
                orderForm.Show();

                this.Hide();
            }
        }

        private void SelectAllProductsInGrid(bool selectAll = true)
        {
            if (productTable == null) return;

            // Detach to avoid UI lag
            dataGridViewReorderProducts.DataSource = null;

            foreach (DataRow row in productTable.Rows)
            {
                row["Select"] = selectAll;
            }

            // Rebind and reinitialize grid
            InitializeReorderGrid();
            dataGridViewReorderProducts.DataSource = productTable;

            // Optional: preserve styling
            StyleReorderProductsGrid();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private bool allSelected = false;
        private bool safetySelected = false;
        private bool reorderSelected = false;



        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            allSelected = !allSelected;

            SelectAllProductsInGrid(allSelected);

            buttonSelectAll.Text = allSelected ? "Deselect All" : "Select All";
            ToggleButtonColor(buttonSelectAll, allSelected);
        }

        // 🔶 Custom hover behavior for orange mode
        private void OrangeHover(object sender, EventArgs e)
        {
            if (sender is Button btn)
                btn.BackColor = Color.DarkOrange;
        }


        private void SelectBelowSafetyStock()
        {
            if (productTable == null) return;

            dataGridViewReorderProducts.DataSource = null;

            foreach (DataRow row in productTable.Rows)
            {
                int quantity = Convert.ToInt32(row["Quantity"]);
                int safetyStock = Convert.ToInt32(row["SafetyStockLevel"]);
                row["Select"] = quantity < safetyStock;
            }

            InitializeReorderGrid();
            dataGridViewReorderProducts.DataSource = productTable;
            StyleReorderProductsGrid();
        }

        private void SelectBelowReorderPoint()
        {
            if (productTable == null) return;

            dataGridViewReorderProducts.DataSource = null;

            foreach (DataRow row in productTable.Rows)
            {
                int quantity = Convert.ToInt32(row["Quantity"]);
                int reorderPoint = Convert.ToInt32(row["ReorderPoint"]);
                row["Select"] = quantity < reorderPoint;
            }

            InitializeReorderGrid();
            dataGridViewReorderProducts.DataSource = productTable;
            StyleReorderProductsGrid();
        }
        
        

        private void ToggleSelectBelowSafetyStock(Button triggerButton)
        {
            safetySelected = !safetySelected;

            if (productTable == null) return;

            dataGridViewReorderProducts.DataSource = null;

            foreach (DataRow row in productTable.Rows)
            {
                int quantity = Convert.ToInt32(row["Quantity"]);
                int safetyStock = Convert.ToInt32(row["SafetyStockLevel"]);
                row["Select"] = safetySelected && quantity < safetyStock;
            }

            InitializeReorderGrid();
            dataGridViewReorderProducts.DataSource = productTable;
            StyleReorderProductsGrid();

            triggerButton.Text = safetySelected ? "Deselect Safety Stock" : "Select Below Safety Stock";

            // 🔶 Toggle color + hover
            if (safetySelected)
            {
                triggerButton.BackColor = Color.Orange;
                triggerButton.MouseEnter += OrangeHover;
                triggerButton.MouseLeave += OrangeLeave;
            }
            else
            {
                triggerButton.MouseEnter -= OrangeHover;
                triggerButton.MouseLeave -= OrangeLeave;
                triggerButton.BackColor = Color.FromArgb(0, 160, 180);
            }
        }


        private void ToggleSelectBelowReorderPoint(Button triggerButton)
        {
            reorderSelected = !reorderSelected;

            if (productTable == null) return;

            dataGridViewReorderProducts.DataSource = null;

            foreach (DataRow row in productTable.Rows)
            {
                int quantity = Convert.ToInt32(row["Quantity"]);
                int reorderPoint = Convert.ToInt32(row["ReorderPoint"]);
                row["Select"] = reorderSelected && quantity < reorderPoint;
            }

            InitializeReorderGrid();
            dataGridViewReorderProducts.DataSource = productTable;
            StyleReorderProductsGrid();

            triggerButton.Text = reorderSelected ? "Deselect Reorder Point" : "Select Below Reorder Point";

            // 🔶 Toggle color + hover
            if (reorderSelected)
            {
                triggerButton.BackColor = Color.Orange;
                triggerButton.MouseEnter += OrangeHover;
                triggerButton.MouseLeave += OrangeLeave;
            }
            else
            {
                triggerButton.MouseEnter -= OrangeHover;
                triggerButton.MouseLeave -= OrangeLeave;
                triggerButton.BackColor = Color.FromArgb(0, 160, 180);
            }
        }




        private void OrangeLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
                btn.BackColor = Color.Orange;
        }

        private void buttonSelectBelowSafety_Click(object sender, EventArgs e)
        {
            safetySelected = !safetySelected;
            ToggleSelectWithCondition((Button)sender, safetySelected, row =>
            {
                int quantity = Convert.ToInt32(row["Quantity"]);
                int safetyStock = Convert.ToInt32(row["SafetyStockLevel"]);
                return quantity < safetyStock;
            }, "Select Below Safety Stock", "Deselect Safety Stock");

        }

        private void buttonSelectBelowReorder_Click(object sender, EventArgs e)
        {
            reorderSelected = !reorderSelected;
            ToggleSelectWithCondition((Button)sender, reorderSelected, row =>
            {
                int quantity = Convert.ToInt32(row["Quantity"]);
                int reorderPoint = Convert.ToInt32(row["ReorderPoint"]);
                return quantity < reorderPoint;
            }, "Select Below Reorder Point", "Deselect Reorder Point");

        }
        private void ToggleSelectWithCondition(Button triggerButton, bool isSelected, Func<DataRow, bool> condition, string labelSelect, string labelDeselect)
        {
            if (productTable == null) return;

            dataGridViewReorderProducts.DataSource = null;

            foreach (DataRow row in productTable.Rows)
            {
                row["Select"] = isSelected && condition(row);
            }

            InitializeReorderGrid();
            dataGridViewReorderProducts.DataSource = productTable;
            StyleReorderProductsGrid();

            triggerButton.Text = isSelected ? labelDeselect : labelSelect;
            ToggleButtonColor(triggerButton, isSelected);
        }
        private void ToggleButtonColor(Button btn, bool active)
        {
            if (active)
            {
                btn.BackColor = Color.Orange;
                btn.MouseEnter += OrangeHover;
                btn.MouseLeave += OrangeLeave;
            }
            else
            {
                btn.MouseEnter -= OrangeHover;
                btn.MouseLeave -= OrangeLeave;
                btn.BackColor = Color.FromArgb(0, 160, 180);
            }
        }
    }
}
