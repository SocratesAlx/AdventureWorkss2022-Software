namespace SokProodos
{
    partial class OrderLowStockForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridViewInvoiceItems = new System.Windows.Forms.DataGridView();
            this.buttonConfirmOrder = new System.Windows.Forms.Button();
            this.labelTotalCost = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBoxVendors = new System.Windows.Forms.ComboBox();
            this.comboBoxShipMethod = new System.Windows.Forms.ComboBox();
            this.comboBoxEmployees = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxOrderDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxDueDate = new System.Windows.Forms.TextBox();
            this.textBoxTaxAmount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInvoiceItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(12, 569);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "Back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridViewInvoiceItems
            // 
            this.dataGridViewInvoiceItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInvoiceItems.Location = new System.Drawing.Point(377, 36);
            this.dataGridViewInvoiceItems.Name = "dataGridViewInvoiceItems";
            this.dataGridViewInvoiceItems.Size = new System.Drawing.Size(726, 370);
            this.dataGridViewInvoiceItems.TabIndex = 1;
            // 
            // buttonConfirmOrder
            // 
            this.buttonConfirmOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConfirmOrder.Location = new System.Drawing.Point(972, 569);
            this.buttonConfirmOrder.Name = "buttonConfirmOrder";
            this.buttonConfirmOrder.Size = new System.Drawing.Size(131, 35);
            this.buttonConfirmOrder.TabIndex = 2;
            this.buttonConfirmOrder.Text = "Confirm Order";
            this.buttonConfirmOrder.UseVisualStyleBackColor = true;
            this.buttonConfirmOrder.Click += new System.EventHandler(this.buttonConfirmOrder_Click);
            // 
            // labelTotalCost
            // 
            this.labelTotalCost.AutoSize = true;
            this.labelTotalCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalCost.Location = new System.Drawing.Point(879, 429);
            this.labelTotalCost.Name = "labelTotalCost";
            this.labelTotalCost.Size = new System.Drawing.Size(70, 25);
            this.labelTotalCost.TabIndex = 3;
            this.labelTotalCost.Text = "label1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SokProodos.Properties.Resources.pngwing_com__18_;
            this.pictureBox1.Location = new System.Drawing.Point(-43, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1161, 625);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // comboBoxVendors
            // 
            this.comboBoxVendors.FormattingEnabled = true;
            this.comboBoxVendors.Location = new System.Drawing.Point(197, 80);
            this.comboBoxVendors.Name = "comboBoxVendors";
            this.comboBoxVendors.Size = new System.Drawing.Size(121, 21);
            this.comboBoxVendors.TabIndex = 5;
            // 
            // comboBoxShipMethod
            // 
            this.comboBoxShipMethod.FormattingEnabled = true;
            this.comboBoxShipMethod.Location = new System.Drawing.Point(197, 122);
            this.comboBoxShipMethod.Name = "comboBoxShipMethod";
            this.comboBoxShipMethod.Size = new System.Drawing.Size(121, 21);
            this.comboBoxShipMethod.TabIndex = 6;
            // 
            // comboBoxEmployees
            // 
            this.comboBoxEmployees.FormattingEnabled = true;
            this.comboBoxEmployees.Location = new System.Drawing.Point(197, 160);
            this.comboBoxEmployees.Name = "comboBoxEmployees";
            this.comboBoxEmployees.Size = new System.Drawing.Size(121, 21);
            this.comboBoxEmployees.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Choose Vendor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Choose Ship Method";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(33, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Employee";
            // 
            // textBoxOrderDate
            // 
            this.textBoxOrderDate.Location = new System.Drawing.Point(197, 198);
            this.textBoxOrderDate.Name = "textBoxOrderDate";
            this.textBoxOrderDate.Size = new System.Drawing.Size(121, 20);
            this.textBoxOrderDate.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(33, 198);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Order Date";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(33, 235);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Due Date";
            // 
            // textBoxDueDate
            // 
            this.textBoxDueDate.Location = new System.Drawing.Point(197, 234);
            this.textBoxDueDate.Name = "textBoxDueDate";
            this.textBoxDueDate.Size = new System.Drawing.Size(121, 20);
            this.textBoxDueDate.TabIndex = 14;
            // 
            // textBoxTaxAmount
            // 
            this.textBoxTaxAmount.Location = new System.Drawing.Point(197, 276);
            this.textBoxTaxAmount.Name = "textBoxTaxAmount";
            this.textBoxTaxAmount.Size = new System.Drawing.Size(121, 20);
            this.textBoxTaxAmount.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(33, 276);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "Tax Amount";
            // 
            // OrderLowStockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 615);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxTaxAmount);
            this.Controls.Add(this.textBoxDueDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxOrderDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxEmployees);
            this.Controls.Add(this.comboBoxShipMethod);
            this.Controls.Add(this.comboBoxVendors);
            this.Controls.Add(this.labelTotalCost);
            this.Controls.Add(this.buttonConfirmOrder);
            this.Controls.Add(this.dataGridViewInvoiceItems);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "OrderLowStockForm";
            this.Text = "OrderLowStockForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInvoiceItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridViewInvoiceItems;
        private System.Windows.Forms.Button buttonConfirmOrder;
        private System.Windows.Forms.Label labelTotalCost;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox comboBoxVendors;
        private System.Windows.Forms.ComboBox comboBoxShipMethod;
        private System.Windows.Forms.ComboBox comboBoxEmployees;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxOrderDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxDueDate;
        private System.Windows.Forms.TextBox textBoxTaxAmount;
        private System.Windows.Forms.Label label6;
    }
}