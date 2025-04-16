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
    public partial class StockHistory: Form


    {

        private string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";
        public StockHistory()
        {

            
            InitializeComponent();
            LoadStockHistory();
            LoadProductNames();
            this.StartPosition = FormStartPosition.CenterScreen;
            UIStyler.StyleButtonsInForm(this);
            //InitializeDataGridView();
            StyleAllButtonsLikeMainForm();
            GroupBoxStyler.StyleGroupBoxesInForm(this);
        }

        private void StyleAllButtonsLikeMainForm()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = Color.FromArgb(0, 160, 180);
                    btn.ForeColor = Color.White;
                    btn.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.Padding = new Padding(0);
                    btn.Cursor = Cursors.Hand;

                    // Hover
                    btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 140, 160);
                    btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(0, 160, 180);

                    // Rounded corners
                    btn.Paint += (s, e) =>
                    {
                        GraphicsPath path = GraphicsExtensions.CreateRoundedRect(btn.ClientRectangle, 8);
                        btn.Region = new Region(path);
                    };
                }
            }
        }



        private void Button_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(114, 137, 218); 
        }

        private void LoadProductNames()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ProductID, Name FROM Production.Product ORDER BY Name";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        comboBoxProductName.Items.Clear();
                        while (reader.Read())
                        {
                            int productId = reader.GetInt32(0);
                            string productName = reader.GetString(1);
                            comboBoxProductName.Items.Add(new KeyValuePair<int, string>(productId, productName));
                        }
                    }

                    comboBoxProductName.DisplayMember = "Value";
                    comboBoxProductName.ValueMember = "Key";
                    comboBoxProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxProductName.AutoCompleteSource = AutoCompleteSource.ListItems;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product names: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void LoadStockHistory(string productNameFilter = "", int productIdFilter = 0)
        {
            if (dataGridViewStockHistory == null)
                return;

            try
            {
                dataGridViewStockHistory.DataSource = null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
            SELECT 
                p.ProductID, 
                p.Name AS ProductName, 
                ISNULL(pi.Quantity, 0) AS StockQuantity,
                pi.ModifiedDate
            FROM Production.Product p
            LEFT JOIN Production.ProductInventory pi ON p.ProductID = pi.ProductID
            WHERE 1 = 1";

                    if (!string.IsNullOrWhiteSpace(productNameFilter))
                        query += " AND p.Name LIKE @ProductName";

                    if (productIdFilter > 0)
                        query += " AND p.ProductID = @ProductID";

                    query += " ORDER BY pi.ModifiedDate DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrWhiteSpace(productNameFilter))
                            command.Parameters.AddWithValue("@ProductName", "%" + productNameFilter + "%");

                        if (productIdFilter > 0)
                            command.Parameters.AddWithValue("@ProductID", productIdFilter);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridViewStockHistory.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stock history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void Button_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(88, 101, 242);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MainForm MainForm = new MainForm();
            MainForm.Show();


            this.Hide();
        }

        private void buttonReresh_Click(object sender, EventArgs e)
        {
            LoadStockHistory();
        }

        private void comboBoxProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProductName.SelectedItem != null)
            {
                var selectedProduct = (KeyValuePair<int, string>)comboBoxProductName.SelectedItem;
                textBoxProductID.Text = selectedProduct.Key.ToString();
                LoadStockHistory(selectedProduct.Value, selectedProduct.Key);
            }
        }

        private void textBoxProductID_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxProductID.Text.Trim(), out int productId))
            {
                var matched = comboBoxProductName.Items
                    .OfType<KeyValuePair<int, string>>()
                    .FirstOrDefault(kvp => kvp.Key == productId);

                if (!matched.Equals(default(KeyValuePair<int, string>)))
                    comboBoxProductName.SelectedItem = matched;

                LoadStockHistory("", productId);
            }
        }
    }
}
