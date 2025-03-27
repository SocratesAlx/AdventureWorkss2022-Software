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
    public partial class SupplierInfo: Form
    {
        private string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";
        public SupplierInfo()
        {
            InitializeComponent();
            LoadSuppliers();
            this.StartPosition = FormStartPosition.CenterScreen;
            UIStyler.StyleButtonsInForm(this);
            GroupBoxStyler.StyleGroupBoxesInForm(this);

            comboBoxSuppliers.SelectedIndexChanged += comboBoxSuppliers_SelectedIndexChanged;
        }

        

        private void LoadSuppliers()
        {
            comboBoxSuppliers.Items.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT BusinessEntityID, Name 
                FROM Purchasing.Vendor 
                ORDER BY Name ASC;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxSuppliers.Items.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0), reader.GetString(1)));
                        }
                    }

                    comboBoxSuppliers.DisplayMember = "Value";
                    comboBoxSuppliers.ValueMember = "Key";
                    comboBoxSuppliers.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxSuppliers.AutoCompleteSource = AutoCompleteSource.ListItems;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading suppliers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadSupplierInfo(int supplierId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    v.BusinessEntityID,
                    v.Name AS SupplierName,
                    v.AccountNumber,
                    v.CreditRating,
                    CASE WHEN v.PreferredVendorStatus = 1 THEN 'Yes' ELSE 'No' END AS PreferredVendor,
                    CASE WHEN v.ActiveFlag = 1 THEN 'Active' ELSE 'Inactive' END AS Status,
                    ISNULL(v.PurchasingWebServiceURL, 'N/A') AS Website,
                    v.ModifiedDate
                FROM Purchasing.Vendor v
                WHERE v.BusinessEntityID = @SupplierID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SupplierID", supplierId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Assign data to DataGridView
                            dataGridViewSupplierInfo.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading supplier info: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void comboBoxSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSuppliers.SelectedItem != null)
            {
                int supplierId = ((KeyValuePair<int, string>)comboBoxSuppliers.SelectedItem).Key;

                
                MessageBox.Show($"Selected Supplier ID: {supplierId}", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadSupplierInfo(supplierId);
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
