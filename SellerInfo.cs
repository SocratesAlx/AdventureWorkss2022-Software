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
    public partial class SellerInfo: Form
    {
        private string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";
        public SellerInfo()
        {
            InitializeComponent();
            LoadSellers();
            this.StartPosition = FormStartPosition.CenterScreen;
            UIStyler.StyleButtonsInForm(this);
            comboBoxSellers.SelectedIndexChanged += new EventHandler(comboBoxSellers_SelectedIndexChanged);
            GroupBoxStyler.StyleGroupBoxesInForm(this);
        }

        

        private void LoadSellers()
        {
            comboBoxSellers.Items.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT sp.BusinessEntityID, p.FirstName + ' ' + p.LastName AS SellerName 
                        FROM Sales.SalesPerson sp
                        JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
                        ORDER BY SellerName ASC;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxSellers.Items.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0), reader.GetString(1)));
                        }
                    }

                    comboBoxSellers.DisplayMember = "Value";
                    comboBoxSellers.ValueMember = "Key";
                    comboBoxSellers.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxSellers.AutoCompleteSource = AutoCompleteSource.ListItems;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading sellers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadSellerInfo(int sellerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    sp.BusinessEntityID AS 'Seller ID',  
                    p.FirstName + ' ' + p.LastName AS 'Seller Name',
                    sp.SalesQuota AS 'Sales Quota',
                    sp.Bonus AS 'Bonus',
                    sp.CommissionPct AS 'Commission %',
                    sp.SalesYTD AS 'Sales YTD',
                    sp.SalesLastYear AS 'Sales Last Year',
                    st.Name AS 'Territory',
                    e.NationalIDNumber AS 'National ID',
                    e.LoginID AS 'Login ID',
                    e.JobTitle AS 'Job Title',
                    e.HireDate AS 'Hire Date'
                FROM Sales.SalesPerson sp
                JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
                LEFT JOIN Sales.SalesTerritory st ON sp.TerritoryID = st.TerritoryID
                LEFT JOIN HumanResources.Employee e ON sp.BusinessEntityID = e.BusinessEntityID
                WHERE sp.BusinessEntityID = @SellerID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SellerID", sellerId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            
                            dataGridViewSellerInfo.DataSource = dt;

                            
                            if (dataGridViewSellerInfo.Columns["Seller ID"] != null)
                            {
                                dataGridViewSellerInfo.Columns["Seller ID"].DisplayIndex = 0;  
                                dataGridViewSellerInfo.Columns["Seller ID"].Width = 80;       
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading seller info: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void comboBoxSellers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSellers.SelectedItem != null)
            {
                int sellerId = ((KeyValuePair<int, string>)comboBoxSellers.SelectedItem).Key;

                

                LoadSellerInfo(sellerId);
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }
    }
}
