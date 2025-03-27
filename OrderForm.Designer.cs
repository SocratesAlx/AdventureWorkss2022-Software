namespace SokProodos
{
    partial class OrderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderForm));
            this.comboBoxCustomer = new System.Windows.Forms.ComboBox();
            this.labelCustomer = new System.Windows.Forms.Label();
            this.labelBillingAddress = new System.Windows.Forms.Label();
            this.textBoxBillingAddress = new System.Windows.Forms.TextBox();
            this.labelOrderDate = new System.Windows.Forms.Label();
            this.textBoxOrderDate = new System.Windows.Forms.TextBox();
            this.labelDueDate = new System.Windows.Forms.Label();
            this.textBoxDueDate = new System.Windows.Forms.TextBox();
            this.labelSeller = new System.Windows.Forms.Label();
            this.comboBoxSeller = new System.Windows.Forms.ComboBox();
            this.labelShipMethod = new System.Windows.Forms.Label();
            this.comboBoxShipMethod = new System.Windows.Forms.ComboBox();
            this.labelProduct = new System.Windows.Forms.Label();
            this.comboBoxProduct = new System.Windows.Forms.ComboBox();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.textBoxQuantity = new System.Windows.Forms.TextBox();
            this.dataGridViewInvoiceItems = new System.Windows.Forms.DataGridView();
            this.labelSpecialOffer = new System.Windows.Forms.Label();
            this.comboBoxSpecialOffer = new System.Windows.Forms.ComboBox();
            this.labelTotalPrice = new System.Windows.Forms.Label();
            this.textBoxTotalPrice = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.buttonFindBillingAddress = new System.Windows.Forms.Button();
            this.buttonCompleteOrder = new System.Windows.Forms.Button();
            this.buttonAddToOrder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInvoiceItems)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxCustomer
            // 
            this.comboBoxCustomer.FormattingEnabled = true;
            this.comboBoxCustomer.Location = new System.Drawing.Point(126, 19);
            this.comboBoxCustomer.Name = "comboBoxCustomer";
            this.comboBoxCustomer.Size = new System.Drawing.Size(121, 23);
            this.comboBoxCustomer.TabIndex = 0;
            // 
            // labelCustomer
            // 
            this.labelCustomer.AutoSize = true;
            this.labelCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomer.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelCustomer.Location = new System.Drawing.Point(14, 22);
            this.labelCustomer.Name = "labelCustomer";
            this.labelCustomer.Size = new System.Drawing.Size(78, 20);
            this.labelCustomer.TabIndex = 1;
            this.labelCustomer.Text = "Customer";
            // 
            // labelBillingAddress
            // 
            this.labelBillingAddress.AutoSize = true;
            this.labelBillingAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBillingAddress.Location = new System.Drawing.Point(14, 59);
            this.labelBillingAddress.Name = "labelBillingAddress";
            this.labelBillingAddress.Size = new System.Drawing.Size(113, 20);
            this.labelBillingAddress.TabIndex = 2;
            this.labelBillingAddress.Text = "Billing Address";
            // 
            // textBoxBillingAddress
            // 
            this.textBoxBillingAddress.Location = new System.Drawing.Point(126, 58);
            this.textBoxBillingAddress.Name = "textBoxBillingAddress";
            this.textBoxBillingAddress.Size = new System.Drawing.Size(121, 21);
            this.textBoxBillingAddress.TabIndex = 3;
            // 
            // labelOrderDate
            // 
            this.labelOrderDate.AutoSize = true;
            this.labelOrderDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOrderDate.Location = new System.Drawing.Point(14, 96);
            this.labelOrderDate.Name = "labelOrderDate";
            this.labelOrderDate.Size = new System.Drawing.Size(88, 20);
            this.labelOrderDate.TabIndex = 4;
            this.labelOrderDate.Text = "Order Date";
            // 
            // textBoxOrderDate
            // 
            this.textBoxOrderDate.Location = new System.Drawing.Point(126, 96);
            this.textBoxOrderDate.Name = "textBoxOrderDate";
            this.textBoxOrderDate.Size = new System.Drawing.Size(121, 21);
            this.textBoxOrderDate.TabIndex = 5;
            // 
            // labelDueDate
            // 
            this.labelDueDate.AutoSize = true;
            this.labelDueDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDueDate.Location = new System.Drawing.Point(14, 133);
            this.labelDueDate.Name = "labelDueDate";
            this.labelDueDate.Size = new System.Drawing.Size(78, 20);
            this.labelDueDate.TabIndex = 6;
            this.labelDueDate.Text = "Due Date";
            // 
            // textBoxDueDate
            // 
            this.textBoxDueDate.Location = new System.Drawing.Point(126, 133);
            this.textBoxDueDate.Name = "textBoxDueDate";
            this.textBoxDueDate.Size = new System.Drawing.Size(121, 21);
            this.textBoxDueDate.TabIndex = 7;
            // 
            // labelSeller
            // 
            this.labelSeller.AutoSize = true;
            this.labelSeller.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSeller.Location = new System.Drawing.Point(14, 168);
            this.labelSeller.Name = "labelSeller";
            this.labelSeller.Size = new System.Drawing.Size(49, 20);
            this.labelSeller.TabIndex = 8;
            this.labelSeller.Text = "Seller";
            // 
            // comboBoxSeller
            // 
            this.comboBoxSeller.FormattingEnabled = true;
            this.comboBoxSeller.Location = new System.Drawing.Point(126, 165);
            this.comboBoxSeller.Name = "comboBoxSeller";
            this.comboBoxSeller.Size = new System.Drawing.Size(121, 23);
            this.comboBoxSeller.TabIndex = 9;
            // 
            // labelShipMethod
            // 
            this.labelShipMethod.AutoSize = true;
            this.labelShipMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipMethod.Location = new System.Drawing.Point(14, 203);
            this.labelShipMethod.Name = "labelShipMethod";
            this.labelShipMethod.Size = new System.Drawing.Size(99, 20);
            this.labelShipMethod.TabIndex = 10;
            this.labelShipMethod.Text = "Ship Method";
            // 
            // comboBoxShipMethod
            // 
            this.comboBoxShipMethod.FormattingEnabled = true;
            this.comboBoxShipMethod.Location = new System.Drawing.Point(126, 200);
            this.comboBoxShipMethod.Name = "comboBoxShipMethod";
            this.comboBoxShipMethod.Size = new System.Drawing.Size(121, 23);
            this.comboBoxShipMethod.TabIndex = 11;
            this.comboBoxShipMethod.SelectedIndexChanged += new System.EventHandler(this.comboBoxShipMethod_SelectedIndexChanged);
            // 
            // labelProduct
            // 
            this.labelProduct.AutoSize = true;
            this.labelProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProduct.Location = new System.Drawing.Point(14, 237);
            this.labelProduct.Name = "labelProduct";
            this.labelProduct.Size = new System.Drawing.Size(64, 20);
            this.labelProduct.TabIndex = 12;
            this.labelProduct.Text = "Product";
            // 
            // comboBoxProduct
            // 
            this.comboBoxProduct.FormattingEnabled = true;
            this.comboBoxProduct.Location = new System.Drawing.Point(126, 234);
            this.comboBoxProduct.Name = "comboBoxProduct";
            this.comboBoxProduct.Size = new System.Drawing.Size(121, 23);
            this.comboBoxProduct.TabIndex = 13;
            // 
            // labelQuantity
            // 
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuantity.Location = new System.Drawing.Point(14, 277);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(68, 20);
            this.labelQuantity.TabIndex = 14;
            this.labelQuantity.Text = "Quantity";
            // 
            // textBoxQuantity
            // 
            this.textBoxQuantity.Location = new System.Drawing.Point(126, 276);
            this.textBoxQuantity.Name = "textBoxQuantity";
            this.textBoxQuantity.Size = new System.Drawing.Size(121, 21);
            this.textBoxQuantity.TabIndex = 15;
            // 
            // dataGridViewInvoiceItems
            // 
            this.dataGridViewInvoiceItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInvoiceItems.Location = new System.Drawing.Point(314, 32);
            this.dataGridViewInvoiceItems.Name = "dataGridViewInvoiceItems";
            this.dataGridViewInvoiceItems.Size = new System.Drawing.Size(690, 275);
            this.dataGridViewInvoiceItems.TabIndex = 16;
            // 
            // labelSpecialOffer
            // 
            this.labelSpecialOffer.AutoSize = true;
            this.labelSpecialOffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSpecialOffer.Location = new System.Drawing.Point(14, 318);
            this.labelSpecialOffer.Name = "labelSpecialOffer";
            this.labelSpecialOffer.Size = new System.Drawing.Size(101, 20);
            this.labelSpecialOffer.TabIndex = 17;
            this.labelSpecialOffer.Text = "Special Offer";
            // 
            // comboBoxSpecialOffer
            // 
            this.comboBoxSpecialOffer.FormattingEnabled = true;
            this.comboBoxSpecialOffer.Location = new System.Drawing.Point(126, 315);
            this.comboBoxSpecialOffer.Name = "comboBoxSpecialOffer";
            this.comboBoxSpecialOffer.Size = new System.Drawing.Size(121, 23);
            this.comboBoxSpecialOffer.TabIndex = 18;
            // 
            // labelTotalPrice
            // 
            this.labelTotalPrice.AutoSize = true;
            this.labelTotalPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalPrice.Location = new System.Drawing.Point(14, 357);
            this.labelTotalPrice.Name = "labelTotalPrice";
            this.labelTotalPrice.Size = new System.Drawing.Size(83, 20);
            this.labelTotalPrice.TabIndex = 19;
            this.labelTotalPrice.Text = "Total Price";
            // 
            // textBoxTotalPrice
            // 
            this.textBoxTotalPrice.Location = new System.Drawing.Point(126, 357);
            this.textBoxTotalPrice.Name = "textBoxTotalPrice";
            this.textBoxTotalPrice.Size = new System.Drawing.Size(121, 21);
            this.textBoxTotalPrice.TabIndex = 20;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonCancel.Location = new System.Drawing.Point(12, 504);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(102, 48);
            this.buttonCancel.TabIndex = 23;
            this.buttonCancel.Text = "Back";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            this.printPreviewDialog1.Load += new System.EventHandler(this.printPreviewDialog1_Load);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.comboBoxCustomer);
            this.groupBox1.Controls.Add(this.labelCustomer);
            this.groupBox1.Controls.Add(this.labelBillingAddress);
            this.groupBox1.Controls.Add(this.textBoxBillingAddress);
            this.groupBox1.Controls.Add(this.labelOrderDate);
            this.groupBox1.Controls.Add(this.textBoxTotalPrice);
            this.groupBox1.Controls.Add(this.textBoxOrderDate);
            this.groupBox1.Controls.Add(this.labelTotalPrice);
            this.groupBox1.Controls.Add(this.labelDueDate);
            this.groupBox1.Controls.Add(this.comboBoxSpecialOffer);
            this.groupBox1.Controls.Add(this.textBoxDueDate);
            this.groupBox1.Controls.Add(this.labelSpecialOffer);
            this.groupBox1.Controls.Add(this.labelSeller);
            this.groupBox1.Controls.Add(this.comboBoxSeller);
            this.groupBox1.Controls.Add(this.textBoxQuantity);
            this.groupBox1.Controls.Add(this.labelShipMethod);
            this.groupBox1.Controls.Add(this.labelQuantity);
            this.groupBox1.Controls.Add(this.comboBoxShipMethod);
            this.groupBox1.Controls.Add(this.comboBoxProduct);
            this.groupBox1.Controls.Add(this.labelProduct);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 401);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Invoice Details";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SokProodos.Properties.Resources.pngwing_com__18_;
            this.pictureBox2.Location = new System.Drawing.Point(-54, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1090, 603);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 27;
            this.pictureBox2.TabStop = false;
            // 
            // buttonFindBillingAddress
            // 
            this.buttonFindBillingAddress.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonFindBillingAddress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFindBillingAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFindBillingAddress.Location = new System.Drawing.Point(12, 450);
            this.buttonFindBillingAddress.Name = "buttonFindBillingAddress";
            this.buttonFindBillingAddress.Size = new System.Drawing.Size(178, 40);
            this.buttonFindBillingAddress.TabIndex = 24;
            this.buttonFindBillingAddress.Text = "Find Billing Address";
            this.buttonFindBillingAddress.UseVisualStyleBackColor = false;
            this.buttonFindBillingAddress.Visible = false;
            this.buttonFindBillingAddress.Click += new System.EventHandler(this.buttonFindBillingAddress_Click);
            // 
            // buttonCompleteOrder
            // 
            this.buttonCompleteOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonCompleteOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCompleteOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCompleteOrder.Location = new System.Drawing.Point(835, 489);
            this.buttonCompleteOrder.Name = "buttonCompleteOrder";
            this.buttonCompleteOrder.Size = new System.Drawing.Size(169, 44);
            this.buttonCompleteOrder.TabIndex = 22;
            this.buttonCompleteOrder.Text = "Complete Order";
            this.buttonCompleteOrder.UseVisualStyleBackColor = false;
            this.buttonCompleteOrder.Visible = false;
            this.buttonCompleteOrder.Click += new System.EventHandler(this.buttonCompleteOrder_Click);
            // 
            // buttonAddToOrder
            // 
            this.buttonAddToOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonAddToOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddToOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddToOrder.Location = new System.Drawing.Point(835, 439);
            this.buttonAddToOrder.Name = "buttonAddToOrder";
            this.buttonAddToOrder.Size = new System.Drawing.Size(169, 44);
            this.buttonAddToOrder.TabIndex = 21;
            this.buttonAddToOrder.Text = "Add To Invoice";
            this.buttonAddToOrder.UseVisualStyleBackColor = false;
            this.buttonAddToOrder.Visible = false;
            this.buttonAddToOrder.Click += new System.EventHandler(this.buttonAddToOrder_Click);
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1016, 562);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonFindBillingAddress);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonCompleteOrder);
            this.Controls.Add(this.buttonAddToOrder);
            this.Controls.Add(this.dataGridViewInvoiceItems);
            this.Controls.Add(this.pictureBox2);
            this.Name = "OrderForm";
            this.Text = "OrderForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInvoiceItems)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxCustomer;
        private System.Windows.Forms.Label labelCustomer;
        private System.Windows.Forms.Label labelBillingAddress;
        private System.Windows.Forms.TextBox textBoxBillingAddress;
        private System.Windows.Forms.Label labelOrderDate;
        private System.Windows.Forms.TextBox textBoxOrderDate;
        private System.Windows.Forms.Label labelDueDate;
        private System.Windows.Forms.TextBox textBoxDueDate;
        private System.Windows.Forms.Label labelSeller;
        private System.Windows.Forms.ComboBox comboBoxSeller;
        private System.Windows.Forms.Label labelShipMethod;
        private System.Windows.Forms.ComboBox comboBoxShipMethod;
        private System.Windows.Forms.Label labelProduct;
        private System.Windows.Forms.ComboBox comboBoxProduct;
        private System.Windows.Forms.Label labelQuantity;
        private System.Windows.Forms.TextBox textBoxQuantity;
        private System.Windows.Forms.DataGridView dataGridViewInvoiceItems;
        private System.Windows.Forms.Label labelSpecialOffer;
        private System.Windows.Forms.ComboBox comboBoxSpecialOffer;
        private System.Windows.Forms.Label labelTotalPrice;
        private System.Windows.Forms.TextBox textBoxTotalPrice;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button buttonFindBillingAddress;
        private System.Windows.Forms.Button buttonCompleteOrder;
        private System.Windows.Forms.Button buttonAddToOrder;
    }
}