namespace SokProodos
{
    partial class ReorderProductsForm
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
            this.buttonBack = new System.Windows.Forms.Button();
            this.dataGridViewReorderProducts = new System.Windows.Forms.DataGridView();
            this.checkBoxCriticalOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxBuiltInHouse = new System.Windows.Forms.CheckBox();
            this.checkBoxModifiedRecently = new System.Windows.Forms.CheckBox();
            this.buttonApplyFilters = new System.Windows.Forms.Button();
            this.buttonReorderSelected = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReorderProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(12, 533);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(86, 29);
            this.buttonBack.TabIndex = 0;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // dataGridViewReorderProducts
            // 
            this.dataGridViewReorderProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReorderProducts.Location = new System.Drawing.Point(333, 12);
            this.dataGridViewReorderProducts.Name = "dataGridViewReorderProducts";
            this.dataGridViewReorderProducts.Size = new System.Drawing.Size(703, 391);
            this.dataGridViewReorderProducts.TabIndex = 1;
            // 
            // checkBoxCriticalOnly
            // 
            this.checkBoxCriticalOnly.AutoSize = true;
            this.checkBoxCriticalOnly.Location = new System.Drawing.Point(931, 420);
            this.checkBoxCriticalOnly.Name = "checkBoxCriticalOnly";
            this.checkBoxCriticalOnly.Size = new System.Drawing.Size(81, 17);
            this.checkBoxCriticalOnly.TabIndex = 5;
            this.checkBoxCriticalOnly.Text = "Critical Only";
            this.checkBoxCriticalOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxBuiltInHouse
            // 
            this.checkBoxBuiltInHouse.AutoSize = true;
            this.checkBoxBuiltInHouse.Location = new System.Drawing.Point(931, 443);
            this.checkBoxBuiltInHouse.Name = "checkBoxBuiltInHouse";
            this.checkBoxBuiltInHouse.Size = new System.Drawing.Size(87, 17);
            this.checkBoxBuiltInHouse.TabIndex = 6;
            this.checkBoxBuiltInHouse.Text = "Built Inhouse";
            this.checkBoxBuiltInHouse.UseVisualStyleBackColor = true;
            // 
            // checkBoxModifiedRecently
            // 
            this.checkBoxModifiedRecently.AutoSize = true;
            this.checkBoxModifiedRecently.Location = new System.Drawing.Point(931, 466);
            this.checkBoxModifiedRecently.Name = "checkBoxModifiedRecently";
            this.checkBoxModifiedRecently.Size = new System.Drawing.Size(111, 17);
            this.checkBoxModifiedRecently.TabIndex = 7;
            this.checkBoxModifiedRecently.Text = "Modified Recently";
            this.checkBoxModifiedRecently.UseVisualStyleBackColor = true;
            // 
            // buttonApplyFilters
            // 
            this.buttonApplyFilters.Location = new System.Drawing.Point(931, 525);
            this.buttonApplyFilters.Name = "buttonApplyFilters";
            this.buttonApplyFilters.Size = new System.Drawing.Size(105, 36);
            this.buttonApplyFilters.TabIndex = 8;
            this.buttonApplyFilters.Text = "Apply Filters";
            this.buttonApplyFilters.UseVisualStyleBackColor = true;
            this.buttonApplyFilters.Click += new System.EventHandler(this.buttonApplyFilters_Click);
            // 
            // buttonReorderSelected
            // 
            this.buttonReorderSelected.Location = new System.Drawing.Point(771, 525);
            this.buttonReorderSelected.Name = "buttonReorderSelected";
            this.buttonReorderSelected.Size = new System.Drawing.Size(104, 35);
            this.buttonReorderSelected.TabIndex = 9;
            this.buttonReorderSelected.Text = "Reorder Selected";
            this.buttonReorderSelected.UseVisualStyleBackColor = true;
            this.buttonReorderSelected.Click += new System.EventHandler(this.buttonReorderSelected_Click);
            // 
            // ReorderProductsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 574);
            this.Controls.Add(this.buttonReorderSelected);
            this.Controls.Add(this.buttonApplyFilters);
            this.Controls.Add(this.checkBoxModifiedRecently);
            this.Controls.Add(this.checkBoxBuiltInHouse);
            this.Controls.Add(this.checkBoxCriticalOnly);
            this.Controls.Add(this.dataGridViewReorderProducts);
            this.Controls.Add(this.buttonBack);
            this.Name = "ReorderProductsForm";
            this.Text = "ReorderProductsForm";
            this.Load += new System.EventHandler(this.ReorderProductsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReorderProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.DataGridView dataGridViewReorderProducts;
        private System.Windows.Forms.CheckBox checkBoxCriticalOnly;
        private System.Windows.Forms.CheckBox checkBoxBuiltInHouse;
        private System.Windows.Forms.CheckBox checkBoxModifiedRecently;
        private System.Windows.Forms.Button buttonApplyFilters;
        private System.Windows.Forms.Button buttonReorderSelected;
    }
}