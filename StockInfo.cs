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
    public partial class StockInfo: Form
    {
        private string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

        public StockInfo()
        {
            InitializeComponent();
            LoadStockItems();
            this.StartPosition = FormStartPosition.CenterScreen;
            UIStyler.StyleButtonsInForm(this);
            GroupBoxStyler.StyleGroupBoxesInForm(this);


            comboBoxStock.SelectedIndexChanged += comboBoxStock_SelectedIndexChanged;
        }



        private void LoadStockItems()
        {
            comboBoxStock.Items.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                SELECT ProductID, Name 
                FROM Production.Product 
                ORDER BY Name ASC;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxStock.Items.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0), reader.GetString(1)));
                        }
                    }

                    comboBoxStock.DisplayMember = "Value";
                    comboBoxStock.ValueMember = "Key";
                    comboBoxStock.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxStock.AutoCompleteSource = AutoCompleteSource.ListItems;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading stock items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void LoadStockInfo(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
            SELECT 
                p.ProductID,
                p.Name AS ProductName,
                ISNULL(pi.Quantity, 0) AS StockQuantity
            FROM Production.Product p
            LEFT JOIN Production.ProductInventory pi ON p.ProductID = pi.ProductID
            WHERE p.ProductID = @ProductID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                dataGridViewStockInfo.DataSource = dt;
                            }
                            else
                            {
                                dataGridViewStockInfo.DataSource = null;
                                MessageBox.Show("No product data found.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading stock info: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void comboBoxStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxStock.SelectedItem != null)
            {
                int productId = ((KeyValuePair<int, string>)comboBoxStock.SelectedItem).Key;

                

                LoadStockInfo(productId);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }
    }
}
