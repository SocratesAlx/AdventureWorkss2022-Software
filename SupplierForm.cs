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
            ((Button)sender).BackColor = Color.FromArgb(114, 137, 218); // Lighter blue on hover
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(88, 101, 242); // Normal state
        }
        private void buttonSaveSupplier_Click(object sender, EventArgs e)
        {
            string supplierName = textBoxSupplierName.Text.Trim();
            string website = textBoxWebsite.Text.Trim();
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

            
            InsertSupplier(supplierName, creditRating, preferred, active, website);
        }

        private void InsertSupplier(string supplierName, int creditRating, bool preferred, bool active, string website)
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

                    
                    string accountNumber = "VN" + new Random().Next(100000, 999999); 

                    
                    string vendorQuery = @"
                INSERT INTO Purchasing.Vendor (BusinessEntityID, AccountNumber, Name, CreditRating, PreferredVendorStatus, ActiveFlag, PurchasingWebServiceURL, ModifiedDate)
                VALUES (@BusinessEntityID, @AccountNumber, @Name, @CreditRating, @Preferred, @Active, @Website, GETDATE());";

                    using (SqlCommand vendorCmd = new SqlCommand(vendorQuery, connection))
                    {
                        vendorCmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        vendorCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        vendorCmd.Parameters.AddWithValue("@Name", supplierName);
                        vendorCmd.Parameters.AddWithValue("@CreditRating", creditRating);
                        vendorCmd.Parameters.AddWithValue("@Preferred", preferred ? 1 : 0);
                        vendorCmd.Parameters.AddWithValue("@Active", active ? 1 : 0);
                        vendorCmd.Parameters.AddWithValue("@Website", string.IsNullOrWhiteSpace(website) ? (object)DBNull.Value : website);
                        vendorCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Supplier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    
                    textBoxSupplierName.Clear();
                    comboBoxCreditRating.SelectedIndex = -1;
                    checkBoxPreferred.Checked = false;
                    checkBoxActive.Checked = false;
                    textBoxWebsite.Clear();
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

                    // Step 1: Find Supplier ID
                    string findSupplierQuery = @"
                SELECT v.BusinessEntityID 
                FROM Purchasing.Vendor v
                WHERE v.Name = @SupplierName";

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

                    // Step 2: Update Supplier Information
                    string updateSupplierQuery = @"
                UPDATE Purchasing.Vendor
                SET CreditRating = @CreditRating, 
                    PreferredVendorStatus = @Preferred, 
                    ActiveFlag = @Active, 
                    PurchasingWebServiceURL = @Website,
                    ModifiedDate = GETDATE()
                WHERE BusinessEntityID = @BusinessEntityID";

                    using (SqlCommand cmd = new SqlCommand(updateSupplierQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BusinessEntityID", businessEntityId);
                        cmd.Parameters.AddWithValue("@CreditRating", creditRating);
                        cmd.Parameters.AddWithValue("@Preferred", preferred ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Active", active ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Website", string.IsNullOrWhiteSpace(website) ? (object)DBNull.Value : website);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Supplier details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating supplier details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string query = @"
                SELECT 
                    v.BusinessEntityID, 
                    v.CreditRating, 
                    v.PreferredVendorStatus, 
                    v.ActiveFlag, 
                    v.PurchasingWebServiceURL 
                FROM Purchasing.Vendor v
                WHERE v.Name = @SupplierName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SupplierName", supplierName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                comboBoxCreditRating.SelectedItem = Convert.ToInt32(reader["CreditRating"]);
                                checkBoxPreferred.Checked = Convert.ToBoolean(reader["PreferredVendorStatus"]);
                                checkBoxActive.Checked = Convert.ToBoolean(reader["ActiveFlag"]);
                                textBoxWebsite.Text = reader["PurchasingWebServiceURL"] != DBNull.Value ? reader["PurchasingWebServiceURL"].ToString() : "";

                                MessageBox.Show("Supplier data loaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Supplier not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving supplier data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
