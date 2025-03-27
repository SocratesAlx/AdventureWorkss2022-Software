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
    public partial class SellersForm: Form
    {
        public SellersForm()
        {
            InitializeComponent();
            LoadSalesTerritories();
            this.StartPosition = FormStartPosition.CenterScreen;
            UIStyler.StyleButtonsInForm(this);
            GroupBoxStyler.StyleGroupBoxesInForm(this);
        }


        private void Button_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(114, 137, 218); 
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(88, 101, 242); 
        }

        private void LoadSalesTerritories()
        {
            comboBoxTerritoryID.Items.Clear();
            comboBoxTerritory.Items.Clear();

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT TerritoryID, Name FROM Sales.SalesTerritory";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int territoryID = reader.GetInt32(0);
                            string territoryName = reader.GetString(1);

                            
                            KeyValuePair<int, string> territoryEntry = new KeyValuePair<int, string>(territoryID, territoryName);

                            comboBoxTerritoryID.Items.Add(territoryEntry);
                            comboBoxTerritory.Items.Add(territoryEntry);
                        }
                    }

                    comboBoxTerritoryID.DisplayMember = "Key"; 
                    comboBoxTerritoryID.ValueMember = "Key";

                    comboBoxTerritory.DisplayMember = "Value"; 
                    comboBoxTerritory.ValueMember = "Key";

                    comboBoxTerritoryID.SelectedIndex = -1;
                    comboBoxTerritory.SelectedIndex = -1;

                    
                    comboBoxTerritory.SelectedIndexChanged += comboBoxTerritory_SelectedIndexChanged;
                    comboBoxTerritoryID.SelectedIndexChanged += comboBoxTerritoryID_SelectedIndexChanged;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading sales territories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // ton exei piei
        private void InsertSeller(string sellerName, decimal salesQuota, decimal bonus, decimal commissionPct, decimal salesYTD, decimal salesLastYear, int territoryId)
        {
            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    string businessEntityQuery = "INSERT INTO Person.BusinessEntity (rowguid, ModifiedDate) VALUES (NEWID(), GETDATE()); SELECT SCOPE_IDENTITY();";
                    int businessEntityId;
                    using (SqlCommand businessCmd = new SqlCommand(businessEntityQuery, connection))
                    {
                        businessEntityId = Convert.ToInt32(businessCmd.ExecuteScalar());
                    }

                    
                    string personQuery = @"
            INSERT INTO Person.Person (BusinessEntityID, PersonType, FirstName, LastName, ModifiedDate)
            VALUES (@BusinessEntityID, 'EM', @FirstName, @LastName, GETDATE());";

                    using (SqlCommand personCmd = new SqlCommand(personQuery, connection))
                    {
                        personCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        personCmd.Parameters.AddWithValue("@FirstName", sellerName.Split(' ')[0]);
                        personCmd.Parameters.AddWithValue("@LastName", sellerName.Contains(" ") ? sellerName.Split(' ')[1] : "Unknown");
                        personCmd.ExecuteNonQuery();
                    }

                    
                    string uniqueNationalID = new Random().Next(100000000, 999999999).ToString();

                    string employeeQuery = @"
INSERT INTO HumanResources.Employee (BusinessEntityID, NationalIDNumber, LoginID, JobTitle, BirthDate, MaritalStatus, Gender, HireDate)
VALUES (@BusinessEntityID, @NationalIDNumber, 'adventure-works\seller' + CAST(@BusinessEntityID AS NVARCHAR), 'Sales Representative', '1990-01-01', 'S', 'M', GETDATE());";

                    using (SqlCommand employeeCmd = new SqlCommand(employeeQuery, connection))
                    {
                        employeeCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        employeeCmd.Parameters.AddWithValue("@NationalIDNumber", uniqueNationalID); 
                        employeeCmd.ExecuteNonQuery();
                    }


                    
                    string sellerQuery = @"
            INSERT INTO Sales.SalesPerson (BusinessEntityID, SalesQuota, Bonus, CommissionPct, SalesYTD, SalesLastYear, TerritoryID, rowguid, ModifiedDate)
            VALUES (@BusinessEntityID, @SalesQuota, @Bonus, @CommissionPct, @SalesYTD, @SalesLastYear, @TerritoryID, NEWID(), GETDATE());";

                    using (SqlCommand sellerCmd = new SqlCommand(sellerQuery, connection))
                    {
                        sellerCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        sellerCmd.Parameters.AddWithValue("@SalesQuota", salesQuota);
                        sellerCmd.Parameters.AddWithValue("@Bonus", bonus);
                        sellerCmd.Parameters.AddWithValue("@CommissionPct", commissionPct);
                        sellerCmd.Parameters.AddWithValue("@SalesYTD", salesYTD);
                        sellerCmd.Parameters.AddWithValue("@SalesLastYear", salesLastYear);
                        sellerCmd.Parameters.AddWithValue("@TerritoryID", territoryId > 0 ? (object)territoryId : DBNull.Value);
                        sellerCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Seller added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    
                    textBoxSellerName.Clear();
                    textBoxSalesQuota.Clear();
                    textBoxBonus.Clear();
                    textBoxCommissionPct.Clear();
                    textBoxSalesYTD.Clear();
                    textBoxSalesLastYear.Clear();
                    comboBoxTerritory.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void buttonSaveSeller_Click(object sender, EventArgs e)
        {
            string sellerName = textBoxSellerName.Text.Trim();
            decimal salesQuota, bonus, commissionPct, salesYTD, salesLastYear;

            if (string.IsNullOrWhiteSpace(sellerName))
            {
                MessageBox.Show("Seller Name is required.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(textBoxSalesQuota.Text, out salesQuota) || salesQuota < 0 ||
                !decimal.TryParse(textBoxBonus.Text, out bonus) || bonus < 0 ||
                !decimal.TryParse(textBoxCommissionPct.Text, out commissionPct) || commissionPct < 0 || commissionPct > 1 ||
                !decimal.TryParse(textBoxSalesYTD.Text, out salesYTD) || salesYTD < 0 ||
                !decimal.TryParse(textBoxSalesLastYear.Text, out salesLastYear) || salesLastYear < 0)
            {
                MessageBox.Show("Please enter valid numeric values for financial fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            int territoryId = comboBoxTerritoryID.SelectedItem != null
                ? ((KeyValuePair<int, string>)comboBoxTerritoryID.SelectedItem).Key
                : 0;

            if (territoryId <= 0)
            {
                MessageBox.Show("Please select a valid Sales Territory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string insertQuery = @"
                INSERT INTO Sales.SalesPerson (BusinessEntityID, SalesQuota, Bonus, CommissionPct, SalesYTD, SalesLastYear, TerritoryID, rowguid, ModifiedDate)
                VALUES (@SellerID, @SalesQuota, @Bonus, @CommissionPct, @SalesYTD, @SalesLastYear, @TerritoryID, NEWID(), GETDATE());";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@SellerID", textBoxSellerID.Text); // Ensure this is populated correctly
                        cmd.Parameters.AddWithValue("@SalesQuota", salesQuota);
                        cmd.Parameters.AddWithValue("@Bonus", bonus);
                        cmd.Parameters.AddWithValue("@CommissionPct", commissionPct);
                        cmd.Parameters.AddWithValue("@SalesYTD", salesYTD);
                        cmd.Parameters.AddWithValue("@SalesLastYear", salesLastYear);
                        cmd.Parameters.AddWithValue("@TerritoryID", territoryId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Seller added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset Fields
                    textBoxSellerID.Clear();
                    textBoxSellerName.Clear();
                    textBoxSalesQuota.Clear();
                    textBoxBonus.Clear();
                    textBoxCommissionPct.Clear();
                    textBoxSalesYTD.Clear();
                    textBoxSalesLastYear.Clear();
                    comboBoxTerritoryID.SelectedIndex = -1;
                    comboBoxTerritory.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding seller: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            MainForm MainForm = new MainForm();
            MainForm.Show();


            this.Hide();
        }

        private void comboBoxTerritoryID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTerritoryID.SelectedItem != null)
            {
                int selectedTerritoryID = ((KeyValuePair<int, string>)comboBoxTerritoryID.SelectedItem).Key;

                
                comboBoxTerritory.SelectedItem = comboBoxTerritory.Items.Cast<KeyValuePair<int, string>>()
                    .FirstOrDefault(kvp => kvp.Key == selectedTerritoryID);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxSellerID.Text.Trim(), out int sellerID))
            {
                MessageBox.Show("Please enter a valid Seller ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal salesQuota, bonus, commissionPct, salesYTD, salesLastYear;

            if (!decimal.TryParse(textBoxSalesQuota.Text, out salesQuota) || salesQuota < 0 ||
                !decimal.TryParse(textBoxBonus.Text, out bonus) || bonus < 0 ||
                !decimal.TryParse(textBoxCommissionPct.Text, out commissionPct) || commissionPct < 0 || commissionPct > 1 ||
                !decimal.TryParse(textBoxSalesYTD.Text, out salesYTD) || salesYTD < 0 ||
                !decimal.TryParse(textBoxSalesLastYear.Text, out salesLastYear) || salesLastYear < 0)
            {
                MessageBox.Show("Please enter valid numeric values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int territoryId = comboBoxTerritoryID.SelectedItem != null
                ? ((KeyValuePair<int, string>)comboBoxTerritoryID.SelectedItem).Key
                : 0;

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string updateQuery = @"
                UPDATE Sales.SalesPerson
                SET SalesQuota = @SalesQuota, Bonus = @Bonus, CommissionPct = @CommissionPct,
                    SalesYTD = @SalesYTD, SalesLastYear = @SalesLastYear, TerritoryID = @TerritoryID, ModifiedDate = GETDATE()
                WHERE BusinessEntityID = @SellerID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@SellerID", sellerID);
                        cmd.Parameters.AddWithValue("@SalesQuota", salesQuota);
                        cmd.Parameters.AddWithValue("@Bonus", bonus);
                        cmd.Parameters.AddWithValue("@CommissionPct", commissionPct);
                        cmd.Parameters.AddWithValue("@SalesYTD", salesYTD);
                        cmd.Parameters.AddWithValue("@SalesLastYear", salesLastYear);
                        cmd.Parameters.AddWithValue("@TerritoryID", territoryId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Seller updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating seller: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void buttonFill_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxSellerID.Text.Trim(), out int sellerID))
            {
                MessageBox.Show("Please enter a valid Seller ID to search.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    sp.BusinessEntityID,
                    p.FirstName + ' ' + p.LastName AS SellerName,
                    sp.SalesQuota,
                    sp.Bonus,
                    sp.CommissionPct,
                    sp.SalesYTD,
                    sp.SalesLastYear,
                    sp.TerritoryID
                FROM Sales.SalesPerson sp
                JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
                WHERE sp.BusinessEntityID = @SellerID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SellerID", sellerID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBoxSellerName.Text = reader["SellerName"].ToString();
                                textBoxSalesQuota.Text = reader["SalesQuota"].ToString();
                                textBoxBonus.Text = reader["Bonus"].ToString();
                                textBoxCommissionPct.Text = reader["CommissionPct"].ToString();
                                textBoxSalesYTD.Text = reader["SalesYTD"].ToString();
                                textBoxSalesLastYear.Text = reader["SalesLastYear"].ToString();

                                
                                if (reader["TerritoryID"] != DBNull.Value)
                                {
                                    int territoryID = Convert.ToInt32(reader["TerritoryID"]);

                                    
                                    comboBoxTerritoryID.SelectedItem = comboBoxTerritoryID.Items.Cast<KeyValuePair<int, string>>()
                                        .FirstOrDefault(kvp => kvp.Key == territoryID);

                                    
                                    comboBoxTerritory.SelectedItem = comboBoxTerritory.Items.Cast<KeyValuePair<int, string>>()
                                        .FirstOrDefault(kvp => kvp.Key == territoryID);
                                }
                                else
                                {
                                    comboBoxTerritoryID.SelectedIndex = -1;
                                    comboBoxTerritory.SelectedIndex = -1;
                                }

                                MessageBox.Show("Seller data loaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Seller not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving seller data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void textBoxSellerID_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTerritory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTerritory.SelectedItem != null)
            {
                int selectedTerritoryID = ((KeyValuePair<int, string>)comboBoxTerritory.SelectedItem).Key;

                
                comboBoxTerritoryID.SelectedItem = comboBoxTerritoryID.Items.Cast<KeyValuePair<int, string>>()
                    .FirstOrDefault(kvp => kvp.Key == selectedTerritoryID);
            }
        }
    }
}
