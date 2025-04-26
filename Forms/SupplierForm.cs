using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;

namespace SokProodos
{
    public partial class SupplierForm: Form
    {
        public SupplierForm()
        {
            InitializeComponent();

            comboBoxCreditRating.Items.Add(1);
            comboBoxCreditRating.Items.Add(2);
            comboBoxCreditRating.Items.Add(3);
            comboBoxCreditRating.Items.Add(4);
            comboBoxCreditRating.Items.Add(5);

            this.StartPosition = FormStartPosition.CenterScreen;
            UIStyler.StyleButtonsInForm(this);
            comboBoxCreditRating.SelectedIndex = 0;
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
        private void buttonSaveSupplier_Click(object sender, EventArgs e)
        {
            string supplierName = textBoxSupplierName.Text.Trim();
            string website = textBoxWebsite.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string phone = textBoxPhone.Text.Trim();
            string address = textBoxAddress.Text.Trim();
            bool preferred = checkBoxPreferred.Checked;
            bool active = checkBoxActive.Checked;

            if (comboBoxCreditRating.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid Credit Rating (1-5).", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int creditRating;
            if (!int.TryParse(comboBoxCreditRating.SelectedItem.ToString(), out creditRating) || creditRating < 1 || creditRating > 5)
            {
                MessageBox.Show("Credit Rating must be between 1 and 5.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    // 1. Insert into BusinessEntity
                    string businessEntityQuery = "INSERT INTO Person.BusinessEntity (rowguid, ModifiedDate) VALUES (NEWID(), GETDATE()); SELECT SCOPE_IDENTITY();";
                    int businessEntityId;
                    using (SqlCommand cmd = new SqlCommand(businessEntityQuery, connection, transaction))
                    {
                        businessEntityId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // 2. Insert into Person.Person (needed for EmailAddress and PersonPhone)
                    string personQuery = @"
                INSERT INTO Person.Person (BusinessEntityID, PersonType, FirstName, LastName, ModifiedDate)
                VALUES (@BusinessEntityID, 'SC', @FirstName, @LastName, GETDATE());";
                    using (SqlCommand cmd = new SqlCommand(personQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@FirstName", supplierName.Split(' ')[0]);
                        cmd.Parameters.AddWithValue("@LastName", supplierName.Contains(" ") ? supplierName.Split(' ')[1] : "Unknown");
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Insert into Vendor
                    string accountNumber = "VN" + new Random().Next(100000, 999999);
                    string vendorQuery = @"
                INSERT INTO Purchasing.Vendor (BusinessEntityID, AccountNumber, Name, CreditRating, PreferredVendorStatus, ActiveFlag, PurchasingWebServiceURL, ModifiedDate)
                VALUES (@BusinessEntityID, @AccountNumber, @Name, @CreditRating, @Preferred, @Active, @Website, GETDATE());";
                    using (SqlCommand cmd = new SqlCommand(vendorQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        cmd.Parameters.AddWithValue("@Name", supplierName);
                        cmd.Parameters.AddWithValue("@CreditRating", creditRating);
                        cmd.Parameters.AddWithValue("@Preferred", preferred ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Active", active ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Website", string.IsNullOrWhiteSpace(website) ? DBNull.Value : (object)website);
                        cmd.ExecuteNonQuery();
                    }

                    // 4. Insert Email if available
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        string emailQuery = "INSERT INTO Person.EmailAddress (BusinessEntityID, EmailAddress) VALUES (@BusinessEntityID, @Email);";
                        using (SqlCommand cmd = new SqlCommand(emailQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 5. Insert Phone if available
                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        string phoneQuery = "INSERT INTO Person.PersonPhone (BusinessEntityID, PhoneNumber, PhoneNumberTypeID) VALUES (@BusinessEntityID, @Phone, 1);";
                        using (SqlCommand cmd = new SqlCommand(phoneQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            cmd.Parameters.AddWithValue("@Phone", phone);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 6. Insert Address if available
                    if (!string.IsNullOrWhiteSpace(address))
                    {
                        string insertAddressQuery = @"
                    INSERT INTO Person.Address (AddressLine1, City, StateProvinceID, PostalCode, rowguid, ModifiedDate)
                    VALUES (@AddressLine1, 'UnknownCity', 1, '00000', NEWID(), GETDATE());
                    SELECT SCOPE_IDENTITY();";
                        int addressId;
                        using (SqlCommand cmd = new SqlCommand(insertAddressQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@AddressLine1", address);
                            addressId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string linkQuery = @"
                    INSERT INTO Person.BusinessEntityAddress (BusinessEntityID, AddressID, AddressTypeID, rowguid, ModifiedDate)
                    VALUES (@BusinessEntityID, @AddressID, 1, NEWID(), GETDATE());";
                        using (SqlCommand cmd = new SqlCommand(linkQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            cmd.Parameters.AddWithValue("@AddressID", addressId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Supplier saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear fields
                    textBoxSupplierName.Clear();
                    textBoxWebsite.Clear();
                    textBoxEmail.Clear();
                    textBoxPhone.Clear();
                    textBoxAddress.Clear();
                    checkBoxPreferred.Checked = false;
                    checkBoxActive.Checked = false;
                    comboBoxCreditRating.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void InsertSupplier(string supplierName, int creditRating, bool preferred, bool active, string website)
        {
            string email = textBoxEmail.Text.Trim();
            string phone = textBoxPhone.Text.Trim();
            string address = textBoxAddress.Text.Trim();

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    // 1. Insert into BusinessEntity
                    string businessEntityQuery = "INSERT INTO Person.BusinessEntity (rowguid, ModifiedDate) VALUES (NEWID(), GETDATE()); SELECT SCOPE_IDENTITY();";
                    int businessEntityId;
                    using (SqlCommand cmd = new SqlCommand(businessEntityQuery, connection, transaction))
                    {
                        businessEntityId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // 2. Insert into Vendor
                    string accountNumber = "VN" + new Random().Next(100000, 999999);
                    string vendorQuery = @"
                INSERT INTO Purchasing.Vendor (BusinessEntityID, AccountNumber, Name, CreditRating, PreferredVendorStatus, ActiveFlag, PurchasingWebServiceURL, ModifiedDate)
                VALUES (@BusinessEntityID, @AccountNumber, @Name, @CreditRating, @Preferred, @Active, @Website, GETDATE());";

                    using (SqlCommand cmd = new SqlCommand(vendorQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        cmd.Parameters.AddWithValue("@Name", supplierName);
                        cmd.Parameters.AddWithValue("@CreditRating", creditRating);
                        cmd.Parameters.AddWithValue("@Preferred", preferred ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Active", active ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Website", string.IsNullOrWhiteSpace(website) ? (object)DBNull.Value : website);
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Optional: Insert Address
                    if (!string.IsNullOrWhiteSpace(address))
                    {
                        string addressQuery = @"
                    INSERT INTO Person.Address (AddressLine1, City, StateProvinceID, PostalCode)
                    OUTPUT INSERTED.AddressID
                    VALUES (@Address, 'UnknownCity', 1, '00000');";

                        int addressId;
                        using (SqlCommand cmd = new SqlCommand(addressQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Address", address);
                            addressId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string linkQuery = @"
                    INSERT INTO Person.BusinessEntityAddress (BusinessEntityID, AddressID, AddressTypeID)
                    VALUES (@BusinessEntityID, @AddressID, 1);";

                        using (SqlCommand cmd = new SqlCommand(linkQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            cmd.Parameters.AddWithValue("@AddressID", addressId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 4. Optional: Insert Email
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

                    // 5. Optional: Insert Phone
                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        string phoneQuery = @"
                    INSERT INTO Person.PersonPhone (BusinessEntityID, PhoneNumber, PhoneNumberTypeID)
                    VALUES (@BusinessEntityID, @Phone, 1);";

                        using (SqlCommand cmd = new SqlCommand(phoneQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            cmd.Parameters.AddWithValue("@Phone", phone);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Supplier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset Fields
                    textBoxSupplierName.Clear();
                    comboBoxCreditRating.SelectedIndex = -1;
                    checkBoxPreferred.Checked = false;
                    checkBoxActive.Checked = false;
                    textBoxWebsite.Clear();
                    textBoxPhone.Clear();
                    textBoxEmail.Clear();
                    textBoxAddress.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MainForm MainForm = new MainForm();
            MainForm.Show();


            this.Hide();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string supplierName = textBoxSupplierName.Text.Trim();
            string website = textBoxWebsite.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string phone = textBoxPhone.Text.Trim();
            string addressLine = textBoxAddress.Text.Trim();
            bool preferred = checkBoxPreferred.Checked;
            bool active = checkBoxActive.Checked;

            if (string.IsNullOrEmpty(supplierName))
            {
                MessageBox.Show("Please enter the Supplier Name to update the details.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxCreditRating.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid Credit Rating (1-5).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int creditRating = (int)comboBoxCreditRating.SelectedItem;
            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    // Step 1: Find BusinessEntityID
                    string findSupplierQuery = "SELECT BusinessEntityID FROM Purchasing.Vendor WHERE Name = @SupplierName";
                    int businessEntityId = 0;

                    using (SqlCommand cmd = new SqlCommand(findSupplierQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@SupplierName", supplierName);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            businessEntityId = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("Supplier not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Step 2: Update Vendor Info
                    string updateVendorQuery = @"
                UPDATE Purchasing.Vendor
                SET CreditRating = @CreditRating,
                    PreferredVendorStatus = @Preferred,
                    ActiveFlag = @Active,
                    PurchasingWebServiceURL = @Website,
                    ModifiedDate = GETDATE()
                WHERE BusinessEntityID = @BusinessEntityID";

                    using (SqlCommand cmd = new SqlCommand(updateVendorQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@CreditRating", creditRating);
                        cmd.Parameters.AddWithValue("@Preferred", preferred ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Active", active ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Website", string.IsNullOrWhiteSpace(website) ? DBNull.Value : (object)website);
                        cmd.ExecuteNonQuery();
                    }

                    // Step 3: Update or Insert Email
                    string emailQuery = @"
                IF EXISTS (SELECT 1 FROM Person.EmailAddress WHERE BusinessEntityID = @BusinessEntityID)
                    UPDATE Person.EmailAddress SET EmailAddress = @Email WHERE BusinessEntityID = @BusinessEntityID
                ELSE
                    INSERT INTO Person.EmailAddress (BusinessEntityID, EmailAddress) VALUES (@BusinessEntityID, @Email)";

                    using (SqlCommand cmd = new SqlCommand(emailQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(email) ? DBNull.Value : (object)email);
                        cmd.ExecuteNonQuery();
                    }

                    // Step 4: Update or Insert Phone
                    string phoneQuery = @"
                IF EXISTS (SELECT 1 FROM Person.PersonPhone WHERE BusinessEntityID = @BusinessEntityID)
                    UPDATE Person.PersonPhone SET PhoneNumber = @Phone WHERE BusinessEntityID = @BusinessEntityID
                ELSE
                    INSERT INTO Person.PersonPhone (BusinessEntityID, PhoneNumber, PhoneNumberTypeID) VALUES (@BusinessEntityID, @Phone, 1)";

                    using (SqlCommand cmd = new SqlCommand(phoneQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(phone) ? DBNull.Value : (object)phone);
                        cmd.ExecuteNonQuery();
                    }

                    // Step 5: Update or Insert Address
                    string findAddressIdQuery = @"
                SELECT AddressID FROM Person.BusinessEntityAddress WHERE BusinessEntityID = @BusinessEntityID";

                    object addressIdObj;
                    using (SqlCommand cmd = new SqlCommand(findAddressIdQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        addressIdObj = cmd.ExecuteScalar();
                    }

                    if (addressIdObj != null)
                    {
                        int addressId = Convert.ToInt32(addressIdObj);
                        string updateAddressQuery = @"
                    UPDATE Person.Address
                    SET AddressLine1 = @Address, ModifiedDate = GETDATE()
                    WHERE AddressID = @AddressID";

                        using (SqlCommand cmd = new SqlCommand(updateAddressQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@AddressID", addressId);
                            cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(addressLine) ? DBNull.Value : (object)addressLine);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertAddressQuery = @"
                    INSERT INTO Person.Address (AddressLine1, City, StateProvinceID, PostalCode, rowguid, ModifiedDate)
                    VALUES (@AddressLine1, 'UnknownCity', 1, '00000', NEWID(), GETDATE());
                    SELECT SCOPE_IDENTITY();";

                        int newAddressId;
                        using (SqlCommand cmd = new SqlCommand(insertAddressQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@AddressLine1", string.IsNullOrWhiteSpace(addressLine) ? DBNull.Value : (object)addressLine);
                            newAddressId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string linkQuery = @"
                    INSERT INTO Person.BusinessEntityAddress (BusinessEntityID, AddressID, AddressTypeID, rowguid, ModifiedDate)
                    VALUES (@BusinessEntityID, @AddressID, 1, NEWID(), GETDATE())";

                        using (SqlCommand cmd = new SqlCommand(linkQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                            cmd.Parameters.AddWithValue("@AddressID", newAddressId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void buttonFill_Click(object sender, EventArgs e)
        {
            string supplierName = textBoxSupplierName.Text.Trim();

            if (string.IsNullOrEmpty(supplierName))
            {
                MessageBox.Show("Please enter the Supplier Name to search.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Step 1: Get Vendor & BusinessEntityID
                    string vendorQuery = @"
                SELECT v.BusinessEntityID, v.CreditRating, v.PreferredVendorStatus, v.ActiveFlag, v.PurchasingWebServiceURL
                FROM Purchasing.Vendor v
                WHERE v.Name = @SupplierName";

                    int businessEntityId = 0;

                    using (SqlCommand cmd = new SqlCommand(vendorQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@SupplierName", supplierName);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                businessEntityId = Convert.ToInt32(reader["BusinessEntityID"]);
                                comboBoxCreditRating.SelectedItem = Convert.ToInt32(reader["CreditRating"]);
                                checkBoxPreferred.Checked = Convert.ToBoolean(reader["PreferredVendorStatus"]);
                                checkBoxActive.Checked = Convert.ToBoolean(reader["ActiveFlag"]);
                                textBoxWebsite.Text = reader["PurchasingWebServiceURL"] != DBNull.Value ? reader["PurchasingWebServiceURL"].ToString() : "";
                            }
                            else
                            {
                                MessageBox.Show("Supplier not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    // Step 2: Load Email
                    string emailQuery = "SELECT TOP 1 EmailAddress FROM Person.EmailAddress WHERE BusinessEntityID = @BusinessEntityID";
                    using (SqlCommand cmd = new SqlCommand(emailQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        object result = cmd.ExecuteScalar();
                        textBoxEmail.Text = result != null ? result.ToString() : "";
                    }

                    // Step 3: Load Phone
                    string phoneQuery = "SELECT TOP 1 PhoneNumber FROM Person.PersonPhone WHERE BusinessEntityID = @BusinessEntityID";
                    using (SqlCommand cmd = new SqlCommand(phoneQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        object result = cmd.ExecuteScalar();
                        textBoxPhone.Text = result != null ? result.ToString() : "";
                    }

                    // Step 4: Load Address
                    string addressQuery = @"
                SELECT TOP 1 a.AddressLine1 
                FROM Person.Address a
                JOIN Person.BusinessEntityAddress bea ON a.AddressID = bea.AddressID
                WHERE bea.BusinessEntityID = @BusinessEntityID";

                    using (SqlCommand cmd = new SqlCommand(addressQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        object result = cmd.ExecuteScalar();
                        textBoxAddress.Text = result != null ? result.ToString() : "";
                    }

                    MessageBox.Show("Supplier data loaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving supplier data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
