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
            this.dataGridViewInvoiceItems.Location = new System.Drawing.Point(201, 55);
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
            this.labelTotalCost.Location = new System.Drawing.Point(450, 442);
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
            // OrderLowStockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 615);
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
    }
}