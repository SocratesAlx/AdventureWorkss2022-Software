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

     
        private void InsertSeller(string sellerName, decimal salesQuota, decimal bonus, decimal commissionPct, decimal salesYTD, decimal salesLastYear, int territoryId)
        {
            string email = textBoxEmail.Text.Trim();
            string phone = textBoxPhoneNumber.Text.Trim();

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string businessEntityQuery = "INSERT INTO Person.BusinessEntity (rowguid, ModifiedDate) VALUES (NEWID(), GETDATE()); SELECT SCOPE_IDENTITY();";
                    int businessEntityId;
                    using (SqlCommand businessCmd = new SqlCommand(businessEntityQuery, connection, transaction))
                    {
                        businessEntityId = Convert.ToInt32(businessCmd.ExecuteScalar());
                    }

                    string personQuery = @"
                INSERT INTO Person.Person (BusinessEntityID, PersonType, FirstName, LastName, ModifiedDate)
                VALUES (@BusinessEntityID, 'EM', @FirstName, @LastName, GETDATE());";

                    using (SqlCommand personCmd = new SqlCommand(personQuery, connection, transaction))
                    {
                        personCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        personCmd.Parameters.AddWithValue("@FirstName", sellerName.Split(' ')[0]);
                        personCmd.Parameters.AddWithValue("@LastName", sellerName.Contains(" ") ? sellerName.Split(' ')[1] : "Unknown");
                        personCmd.ExecuteNonQuery();
                    }

                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        string emailQuery = @"
                    INSERT INTO Person.EmailAddress (BusinessEntityID, EmailAddress)
                    VALUES (@BusinessEntityID, @Email);";

                        using (SqlCommand emailCmd = new SqlCommand(emailQuery, connection, transaction))
                        {
                            emailCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            emailCmd.Parameters.AddWithValue("@Email", email);
                            emailCmd.ExecuteNonQuery();
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        string phoneQuery = @"
                    INSERT INTO Person.PersonPhone (BusinessEntityID, PhoneNumber, PhoneNumberTypeID)
                    VALUES (@BusinessEntityID, @PhoneNumber, 1);";

                        using (SqlCommand phoneCmd = new SqlCommand(phoneQuery, connection, transaction))
                        {
                            phoneCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            phoneCmd.Parameters.AddWithValue("@PhoneNumber", phone);
                            phoneCmd.ExecuteNonQuery();
                        }
                    }

                    string uniqueNationalID = new Random().Next(100000000, 999999999).ToString();

                    string employeeQuery = @"
                INSERT INTO HumanResources.Employee (BusinessEntityID, NationalIDNumber, LoginID, JobTitle, BirthDate, MaritalStatus, Gender, HireDate)
                VALUES (@BusinessEntityID, @NationalIDNumber, 'adventure-works\\seller' + CAST(@BusinessEntityID AS NVARCHAR), 'Sales Representative', '1990-01-01', 'S', 'M', GETDATE());";

                    using (SqlCommand employeeCmd = new SqlCommand(employeeQuery, connection, transaction))
                    {
                        employeeCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        employeeCmd.Parameters.AddWithValue("@NationalIDNumber", uniqueNationalID);
                        employeeCmd.ExecuteNonQuery();
                    }

                    string sellerQuery = @"
                INSERT INTO Sales.SalesPerson (BusinessEntityID, SalesQuota, Bonus, CommissionPct, SalesYTD, SalesLastYear, TerritoryID, rowguid, ModifiedDate)
                VALUES (@BusinessEntityID, @SalesQuota, @Bonus, @CommissionPct, @SalesYTD, @SalesLastYear, @TerritoryID, NEWID(), GETDATE());";

                    using (SqlCommand sellerCmd = new SqlCommand(sellerQuery, connection, transaction))
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

                    transaction.Commit();
                    MessageBox.Show("Seller added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBoxSellerName.Clear();
                    textBoxSalesQuota.Clear();
                    textBoxBonus.Clear();
                    textBoxCommissionPct.Clear();
                    textBoxSalesYTD.Clear();
                    textBoxSalesLastYear.Clear();
                    textBoxEmail.Clear();
                    textBoxPhoneNumber.Clear();
                    comboBoxTerritory.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
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

            // ✅ Call updated InsertSeller method (now handles email & phone internally)
            InsertSeller(sellerName, salesQuota, bonus, commissionPct, salesYTD, salesLastYear, territoryId);
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

            string email = textBoxEmail.Text.Trim();
            string phone = textBoxPhoneNumber.Text.Trim();

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string updateSalesQuery = @"
                UPDATE Sales.SalesPerson
                SET SalesQuota = @SalesQuota, Bonus = @Bonus, CommissionPct = @CommissionPct,
                    SalesYTD = @SalesYTD, SalesLastYear = @SalesLastYear, TerritoryID = @TerritoryID, ModifiedDate = GETDATE()
                WHERE BusinessEntityID = @SellerID";

                    using (SqlCommand cmd = new SqlCommand(updateSalesQuery, connection, transaction))
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

                    string emailQuery = @"
                IF EXISTS (SELECT 1 FROM Person.EmailAddress WHERE BusinessEntityID = @SellerID)
                    UPDATE Person.EmailAddress SET EmailAddress = @Email WHERE BusinessEntityID = @SellerID
                ELSE
                    INSERT INTO Person.EmailAddress (BusinessEntityID, EmailAddress) VALUES (@SellerID, @Email);";

                    using (SqlCommand cmd = new SqlCommand(emailQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@SellerID", sellerID);
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(email) ? DBNull.Value : (object)email);
                        cmd.ExecuteNonQuery();
                    }

                    string phoneQuery = @"
                IF EXISTS (SELECT 1 FROM Person.PersonPhone WHERE BusinessEntityID = @SellerID)
                    UPDATE Person.PersonPhone SET PhoneNumber = @Phone WHERE BusinessEntityID = @SellerID
                ELSE
                    INSERT INTO Person.PersonPhone (BusinessEntityID, PhoneNumber, PhoneNumberTypeID)
                    VALUES (@SellerID, @Phone, 1);";

                    using (SqlCommand cmd = new SqlCommand(phoneQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@SellerID", sellerID);
                        cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(phone) ? DBNull.Value : (object)phone);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Seller updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
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
                    sp.TerritoryID,
                    ea.EmailAddress,
                    ph.PhoneNumber
                FROM Sales.SalesPerson sp
                JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
                LEFT JOIN Person.EmailAddress ea ON p.BusinessEntityID = ea.BusinessEntityID
                LEFT JOIN Person.PersonPhone ph ON p.BusinessEntityID = ph.BusinessEntityID
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
                                textBoxEmail.Text = reader["EmailAddress"] != DBNull.Value ? reader["EmailAddress"].ToString() : "";
                                textBoxPhoneNumber.Text = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : "";

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
