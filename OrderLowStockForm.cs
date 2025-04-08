using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SokProodos
{
    public partial class OrderLowStockForm: Form
    {
        private List<DataRow> selectedProducts;
        public OrderLowStockForm(List<DataRow> selectedProducts)
        {
            InitializeComponent();
            this.selectedProducts = selectedProducts;
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadSelectedProducts();
        }
        private void LoadSelectedProducts()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("QuantityToReorder", typeof(int));

            foreach (DataRow row in selectedProducts)
            {
                dt.Rows.Add(
                    row["ProductID"],
                    row["Name"],
                    row["Quantity"],
                    row["QuantityToReorder"]
                );
            }

            dataGridViewInvoiceItems.DataSource = dt;
        }


        

        private void button1_Click(object sender, EventArgs e)
        {
            ReorderProductsForm reorderForm = new ReorderProductsForm();
            reorderForm.Show();

            this.Hide();
        }
    }
}
