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
                WHERE FinishedGoodsFlag = 1
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
                    p.ProductNumber,
                    p.Color,
                    p.StandardCost,
                    p.ListPrice,
                    p.Size,
                    p.Weight,
                    p.SafetyStockLevel,
                    p.ReorderPoint,
                    p.DaysToManufacture,
                    ps.Name AS Subcategory,
                    pm.Name AS Model,
                    ISNULL(pi.Quantity, 0) AS StockQuantity,
                    l.Name AS Location,
                    pi.Shelf,
                    pi.Bin,
                    ISNULL(so.Description, 'No Discount') AS SpecialOffer
                FROM Production.Product p
                LEFT JOIN Production.ProductSubcategory ps ON p.ProductSubcategoryID = ps.ProductSubcategoryID
                LEFT JOIN Production.ProductModel pm ON p.ProductModelID = pm.ProductModelID
                LEFT JOIN Production.ProductInventory pi ON p.ProductID = pi.ProductID
                LEFT JOIN Production.Location l ON pi.LocationID = l.LocationID
                LEFT JOIN Sales.SpecialOfferProduct sop ON p.ProductID = sop.ProductID
                LEFT JOIN Sales.SpecialOffer so ON sop.SpecialOfferID = so.SpecialOfferID
                WHERE p.ProductID = @ProductID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            
                            dataGridViewStockInfo.DataSource = dt;
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
