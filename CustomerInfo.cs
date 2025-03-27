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
    public partial class CustomerInfo: Form
    {
        private string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

        public CustomerInfo()
        {
            InitializeComponent();
            LoadCustomers();
            GroupBoxStyler.StyleGroupBoxesInForm(this);


            comboBoxCustomers.SelectedIndexChanged += new EventHandler(comboBoxCustomers_SelectedIndexChanged);
        }


        private void LoadCustomers()
        {
            comboBoxCustomers.Items.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    c.CustomerID, 
                    ISNULL(
                        p.FirstName + ' ' + 
                        ISNULL(p.MiddleName + ' ', '') + 
                        p.LastName, 
                        s.Name
                    ) AS CustomerName
                FROM Sales.Customer c
                LEFT JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
                LEFT JOIN Sales.Store s ON c.StoreID = s.BusinessEntityID
                ORDER BY CustomerName ASC;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxCustomers.Items.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0), reader.GetString(1)));
                        }
                    }

                    comboBoxCustomers.DisplayMember = "Value";
                    comboBoxCustomers.ValueMember = "Key";
                    comboBoxCustomers.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxCustomers.AutoCompleteSource = AutoCompleteSource.ListItems;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void LoadCustomerInfo(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    c.CustomerID,
                    ISNULL(
                        p.FirstName + ' ' + 
                        ISNULL(p.MiddleName + ' ', '') + 
                        p.LastName, 
                        s.Name
                    ) AS CustomerName,
                    COUNT(soh.SalesOrderID) AS TotalOrders,
                    SUM(soh.TotalDue) AS TotalSpent,
                    ba.AddressLine1 AS BillingAddress,
                    sa.AddressLine1 AS ShippingAddress,
                    ba.City AS BillingCity,
                    sa.City AS ShippingCity,
                    ba.PostalCode AS BillingPostalCode,
                    sa.PostalCode AS ShippingPostalCode,
                    sp.Name AS State,
                    ea.EmailAddress,
                    ph.PhoneNumber
                FROM Sales.Customer c
                LEFT JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
                LEFT JOIN Sales.Store s ON c.StoreID = s.BusinessEntityID
                LEFT JOIN Sales.SalesOrderHeader soh ON c.CustomerID = soh.CustomerID
                LEFT JOIN Person.BusinessEntityAddress bea ON c.PersonID = bea.BusinessEntityID
                LEFT JOIN Person.Address ba ON bea.AddressID = ba.AddressID
                LEFT JOIN Person.BusinessEntityAddress sea ON c.PersonID = sea.BusinessEntityID
                LEFT JOIN Person.Address sa ON sea.AddressID = sa.AddressID
                LEFT JOIN Person.StateProvince sp ON ba.StateProvinceID = sp.StateProvinceID
                LEFT JOIN Person.EmailAddress ea ON c.PersonID = ea.BusinessEntityID
                LEFT JOIN Person.PersonPhone ph ON c.PersonID = ph.BusinessEntityID
                WHERE c.CustomerID = @CustomerID
                GROUP BY 
                    c.CustomerID, 
                    p.FirstName, p.MiddleName, p.LastName, 
                    s.Name, 
                    ba.AddressLine1, sa.AddressLine1, 
                    ba.City, sa.City,
                    ba.PostalCode, sa.PostalCode, 
                    sp.Name, 
                    ea.EmailAddress, ph.PhoneNumber;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", customerId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dataGridViewCustomerInfo.DataSource = dt;

                            
                            if (dataGridViewCustomerInfo.Columns.Contains("CustomerName"))
                            {
                                dataGridViewCustomerInfo.Columns["CustomerName"].HeaderText = "Full Name";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customer info: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void comboBoxCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomers.SelectedItem != null)
            {
                int customerId = ((KeyValuePair<int, string>)comboBoxCustomers.SelectedItem).Key;

                
              

                LoadCustomerInfo(customerId);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
