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
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
            LoadStates();
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

        private void LoadStates()
        {
            comboBoxState.Items.Clear(); 

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT StateProvinceID, Name FROM Person.StateProvince";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int stateId = reader.GetInt32(0);
                        string stateName = reader.GetString(1);
                        comboBoxState.Items.Add(new KeyValuePair<int, string>(stateId, stateName));
                    }
                }
            }

            comboBoxState.DisplayMember = "Value";
            comboBoxState.ValueMember = "Key";
            comboBoxState.SelectedIndex = -1; 
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string firstName = textBoxFirstName.Text.Trim();
            string middleName = textBoxMiddleName.Text.Trim(); // νέο πεδίο
            string lastName = textBoxLastName.Text.Trim();
            string billingAddress = textBoxBillingAddress.Text.Trim();
            string city = textBoxCity.Text.Trim();
            string postalCode = textBoxPostalCode.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string phone = textBoxPhone.Text.Trim();

            if (comboBoxState.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid state!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int stateProvinceId = ((KeyValuePair<int, string>)comboBoxState.SelectedItem).Key;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(billingAddress))
            {
                MessageBox.Show("Please fill in all required fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    string businessEntityInsertQuery = "INSERT INTO Person.BusinessEntity DEFAULT VALUES; SELECT SCOPE_IDENTITY();";
                    int businessEntityId;
                    using (SqlCommand cmd = new SqlCommand(businessEntityInsertQuery, connection, transaction))
                    {
                        businessEntityId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    string personInsertQuery = @"
                INSERT INTO Person.Person (BusinessEntityID, FirstName, MiddleName, LastName, PersonType)
                VALUES (@BusinessEntityID, @FirstName, @MiddleName, @LastName, 'IN')";

                    using (SqlCommand cmd = new SqlCommand(personInsertQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@MiddleName", string.IsNullOrWhiteSpace(middleName) ? DBNull.Value : (object)middleName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.ExecuteNonQuery();
                    }

                    string addressInsertQuery = @"
                INSERT INTO Person.Address (AddressLine1, City, StateProvinceID, PostalCode)
                OUTPUT INSERTED.AddressID
                VALUES (@AddressLine1, @City, @StateProvinceID, @PostalCode)";

                    int addressId;
                    using (SqlCommand cmd = new SqlCommand(addressInsertQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@AddressLine1", billingAddress);
                        cmd.Parameters.AddWithValue("@City", city);
                        cmd.Parameters.AddWithValue("@StateProvinceID", stateProvinceId);
                        cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                        addressId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    string businessEntityAddressInsertQuery = @"
                INSERT INTO Person.BusinessEntityAddress (BusinessEntityID, AddressID, AddressTypeID)
                VALUES (@BusinessEntityID, @AddressID, 1)"; // 1 = Billing

                    using (SqlCommand cmd = new SqlCommand(businessEntityAddressInsertQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@AddressID", addressId);
                        cmd.ExecuteNonQuery();
                    }

                    string customerInsertQuery = @"
                INSERT INTO Sales.Customer (PersonID)
                VALUES (@PersonID)";

                    using (SqlCommand cmd = new SqlCommand(customerInsertQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@PersonID", businessEntityId);
                        cmd.ExecuteNonQuery();
                    }

                    
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        string emailQuery = @"
                    INSERT INTO Person.EmailAddress (BusinessEntityID, EmailAddress)
                    VALUES (@BusinessEntityID, @Email);";

                        using (SqlCommand cmd = new SqlCommand(emailQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    
                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        string phoneQuery = @"
                    INSERT INTO Person.PersonPhone (BusinessEntityID, PhoneNumber, PhoneNumberTypeID)
                    VALUES (@BusinessEntityID, @PhoneNumber, 1);"; // 1 = Cell Phone

                        using (SqlCommand cmd = new SqlCommand(phoneQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            cmd.Parameters.AddWithValue("@PhoneNumber", phone);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Customer and Billing Address added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void ResetForm()
        {
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            textBoxMiddleName.Clear();
            textBoxBillingAddress.Clear();
            textBoxCity.Clear();
            textBoxPostalCode.Clear();
            comboBoxState.SelectedIndex = -1; 
            textBoxFirstName.Focus(); 
        }


        private void InsertCustomer(string firstName, string lastName, string email, string phoneNumber)
        {
            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // insert se business entity id sto person.person
                    string businessEntityQuery = "INSERT INTO Person.BusinessEntity (rowguid, ModifiedDate) VALUES (NEWID(), GETDATE()); SELECT SCOPE_IDENTITY();";

                    int businessEntityId;
                    using (SqlCommand businessCmd = new SqlCommand(businessEntityQuery, connection))
                    {
                        businessEntityId = Convert.ToInt32(businessCmd.ExecuteScalar());
                    }

                    
                    string personQuery = @"
                INSERT INTO Person.Person (BusinessEntityID, FirstName, LastName, PersonType, ModifiedDate)
                VALUES (@BusinessEntityID, @FirstName, @LastName, 'IN', GETDATE());";

                    using (SqlCommand personCmd = new SqlCommand(personQuery, connection))
                    {
                        personCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        personCmd.Parameters.AddWithValue("@FirstName", firstName);
                        personCmd.Parameters.AddWithValue("@LastName", lastName);
                        personCmd.ExecuteNonQuery();
                    }

                    
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        string emailQuery = @"
                    INSERT INTO Person.EmailAddress (BusinessEntityID, EmailAddress)
                    VALUES (@BusinessEntityID, @Email);";

                        using (SqlCommand emailCmd = new SqlCommand(emailQuery, connection))
                        {
                            emailCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            emailCmd.Parameters.AddWithValue("@Email", email);
                            emailCmd.ExecuteNonQuery();
                        }
                    }

                    
                    if (!string.IsNullOrWhiteSpace(phoneNumber))
                    {
                        string phoneQuery = @"
                    INSERT INTO Person.PersonPhone (BusinessEntityID, PhoneNumber, PhoneNumberTypeID)
                    VALUES (@BusinessEntityID, @PhoneNumber, 1);"; // 1 = Cell Phone

                        using (SqlCommand phoneCmd = new SqlCommand(phoneQuery, connection))
                        {
                            phoneCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            phoneCmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            phoneCmd.ExecuteNonQuery();
                        }
                    }

                    // insert xwris account number gt einai auto incremented
                    string customerQuery = @"
                INSERT INTO Sales.Customer (PersonID, StoreID, TerritoryID, ModifiedDate)
                VALUES (@BusinessEntityID, NULL, NULL, GETDATE());";

                    using (SqlCommand customerCmd = new SqlCommand(customerQuery, connection))
                    {
                        customerCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        customerCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    
                    textBoxFirstName.Clear();
                    textBoxLastName.Clear();
                    textBoxEmail.Clear();
                    textBoxPhone.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            MainForm MainForm = new MainForm();
            MainForm.Show();


            this.Hide();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string firstName = textBoxFirstName.Text.Trim();
            string lastName = textBoxLastName.Text.Trim();
            string billingAddress = textBoxBillingAddress.Text.Trim();
            string city = textBoxCity.Text.Trim();
            string postalCode = textBoxPostalCode.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string phone = textBoxPhone.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Please enter First Name and Last Name to search for the customer.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxState.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid state!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int stateProvinceId = ((KeyValuePair<int, string>)comboBoxState.SelectedItem).Key;

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    // Step 1: Find Customer ID
                    string findCustomerQuery = @"
                SELECT c.CustomerID, p.BusinessEntityID, a.AddressID
                FROM Sales.Customer c
                JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
                LEFT JOIN Person.BusinessEntityAddress bea ON c.PersonID = bea.BusinessEntityID
                LEFT JOIN Person.Address a ON bea.AddressID = a.AddressID
                WHERE p.FirstName = @FirstName AND p.LastName = @LastName";

                    int customerId = 0, businessEntityId = 0, addressId = 0;
                    using (SqlCommand cmd = new SqlCommand(findCustomerQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customerId = Convert.ToInt32(reader["CustomerID"]);
                                businessEntityId = Convert.ToInt32(reader["BusinessEntityID"]);
                                addressId = reader["AddressID"] != DBNull.Value ? Convert.ToInt32(reader["AddressID"]) : 0;
                            }
                            else
                            {
                                MessageBox.Show("Customer not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    
                    if (addressId > 0)
                    {
                        string updateAddressQuery = @"
                    UPDATE Person.Address
                    SET AddressLine1 = @Address, City = @City, StateProvinceID = @StateProvinceID, PostalCode = @PostalCode
                    WHERE AddressID = @AddressID";

                        using (SqlCommand cmd = new SqlCommand(updateAddressQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Address", billingAddress);
                            cmd.Parameters.AddWithValue("@City", city);
                            cmd.Parameters.AddWithValue("@StateProvinceID", stateProvinceId);
                            cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                            cmd.Parameters.AddWithValue("@AddressID", addressId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    
                    string updateCustomerQuery = @"
                UPDATE Person.Person
                SET FirstName = @FirstName, LastName = @LastName
                WHERE BusinessEntityID = @BusinessEntityID";

                    using (SqlCommand cmd = new SqlCommand(updateCustomerQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.ExecuteNonQuery();
                    }

                    
                    string emailQuery = @"
                IF EXISTS (SELECT 1 FROM Person.EmailAddress WHERE BusinessEntityID = @BusinessEntityID)
                    UPDATE Person.EmailAddress 
                    SET EmailAddress = @Email 
                    WHERE BusinessEntityID = @BusinessEntityID;
                ELSE
                    INSERT INTO Person.EmailAddress (BusinessEntityID, EmailAddress) 
                    VALUES (@BusinessEntityID, @Email);";

                    using (SqlCommand cmd = new SqlCommand(emailQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(email) ? DBNull.Value : (object)email);
                        cmd.ExecuteNonQuery();
                    }

                    
                    string phoneQuery = @"
                IF EXISTS (SELECT 1 FROM Person.PersonPhone WHERE BusinessEntityID = @BusinessEntityID)
                    UPDATE Person.PersonPhone 
                    SET PhoneNumber = @PhoneNumber 
                    WHERE BusinessEntityID = @BusinessEntityID;
                ELSE
                    INSERT INTO Person.PersonPhone (BusinessEntityID, PhoneNumber, PhoneNumberTypeID) 
                    VALUES (@BusinessEntityID, @PhoneNumber, 1);";  // 1 = Cell Phone

                    using (SqlCommand cmd = new SqlCommand(phoneQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@PhoneNumber", string.IsNullOrWhiteSpace(phone) ? DBNull.Value : (object)phone);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Customer details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating customer details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void buttonFill_Click(object sender, EventArgs e)
        {
            string firstName = textBoxFirstName.Text.Trim();
            string lastName = textBoxLastName.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Please enter First Name and Last Name to search.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    c.CustomerID, 
                    p.FirstName, 
                    p.LastName, 
                    a.AddressLine1, 
                    a.City, 
                    a.PostalCode, 
                    sp.StateProvinceID,
                    e.EmailAddress, 
                    ph.PhoneNumber 
                FROM Sales.Customer c
                JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
                LEFT JOIN Person.BusinessEntityAddress bea ON c.PersonID = bea.BusinessEntityID
                LEFT JOIN Person.Address a ON bea.AddressID = a.AddressID
                LEFT JOIN Person.StateProvince sp ON a.StateProvinceID = sp.StateProvinceID
                LEFT JOIN Person.EmailAddress e ON p.BusinessEntityID = e.BusinessEntityID
                LEFT JOIN Person.PersonPhone ph ON p.BusinessEntityID = ph.BusinessEntityID
                WHERE p.FirstName = @FirstName AND p.LastName = @LastName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBoxBillingAddress.Text = reader["AddressLine1"].ToString();
                                textBoxCity.Text = reader["City"].ToString();
                                textBoxPostalCode.Text = reader["PostalCode"].ToString();
                                textBoxEmail.Text = reader["EmailAddress"] != DBNull.Value ? reader["EmailAddress"].ToString() : "";
                                textBoxPhone.Text = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : "";

                                int stateId = reader["StateProvinceID"] != DBNull.Value ? Convert.ToInt32(reader["StateProvinceID"]) : -1;
                                if (stateId > 0)
                                {
                                    comboBoxState.SelectedIndex = comboBoxState.FindStringExact(comboBoxState.Items.Cast<KeyValuePair<int, string>>()
                                        .FirstOrDefault(kvp => kvp.Key == stateId).Value);
                                }

                                MessageBox.Show("Customer data loaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Customer not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving customer data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}


