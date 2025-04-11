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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBoxSearchProducts = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonSelectBelowSafety = new System.Windows.Forms.Button();
            this.buttonSelectBelowReorder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReorderProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.dataGridViewReorderProducts.Location = new System.Drawing.Point(325, 71);
            this.dataGridViewReorderProducts.Name = "dataGridViewReorderProducts";
            this.dataGridViewReorderProducts.Size = new System.Drawing.Size(703, 391);
            this.dataGridViewReorderProducts.TabIndex = 1;
            // 
            // checkBoxCriticalOnly
            // 
            this.checkBoxCriticalOnly.AutoSize = true;
            this.checkBoxCriticalOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCriticalOnly.Location = new System.Drawing.Point(10, 68);
            this.checkBoxCriticalOnly.Name = "checkBoxCriticalOnly";
            this.checkBoxCriticalOnly.Size = new System.Drawing.Size(110, 24);
            this.checkBoxCriticalOnly.TabIndex = 5;
            this.checkBoxCriticalOnly.Text = "Critical Only";
            this.checkBoxCriticalOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxBuiltInHouse
            // 
            this.checkBoxBuiltInHouse.AutoSize = true;
            this.checkBoxBuiltInHouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxBuiltInHouse.Location = new System.Drawing.Point(10, 98);
            this.checkBoxBuiltInHouse.Name = "checkBoxBuiltInHouse";
            this.checkBoxBuiltInHouse.Size = new System.Drawing.Size(121, 24);
            this.checkBoxBuiltInHouse.TabIndex = 6;
            this.checkBoxBuiltInHouse.Text = "Built Inhouse";
            this.checkBoxBuiltInHouse.UseVisualStyleBackColor = true;
            // 
            // checkBoxModifiedRecently
            // 
            this.checkBoxModifiedRecently.AutoSize = true;
            this.checkBoxModifiedRecently.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxModifiedRecently.Location = new System.Drawing.Point(10, 128);
            this.checkBoxModifiedRecently.Name = "checkBoxModifiedRecently";
            this.checkBoxModifiedRecently.Size = new System.Drawing.Size(154, 24);
            this.checkBoxModifiedRecently.TabIndex = 7;
            this.checkBoxModifiedRecently.Text = "Modified Recently";
            this.checkBoxModifiedRecently.UseVisualStyleBackColor = true;
            // 
            // buttonApplyFilters
            // 
            this.buttonApplyFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonApplyFilters.Location = new System.Drawing.Point(931, 529);
            this.buttonApplyFilters.Name = "buttonApplyFilters";
            this.buttonApplyFilters.Size = new System.Drawing.Size(105, 36);
            this.buttonApplyFilters.TabIndex = 8;
            this.buttonApplyFilters.Text = "Apply Filters";
            this.buttonApplyFilters.UseVisualStyleBackColor = true;
            this.buttonApplyFilters.Click += new System.EventHandler(this.buttonApplyFilters_Click);
            // 
            // buttonReorderSelected
            // 
            this.buttonReorderSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReorderSelected.Location = new System.Drawing.Point(619, 492);
            this.buttonReorderSelected.Name = "buttonReorderSelected";
            this.buttonReorderSelected.Size = new System.Drawing.Size(144, 37);
            this.buttonReorderSelected.TabIndex = 9;
            this.buttonReorderSelected.Text = "Reorder Selected";
            this.buttonReorderSelected.UseVisualStyleBackColor = true;
            this.buttonReorderSelected.Click += new System.EventHandler(this.buttonReorderSelected_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SokProodos.Properties.Resources.pngwing_com__18_;
            this.pictureBox1.Location = new System.Drawing.Point(-18, -11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1077, 594);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // comboBoxSearchProducts
            // 
            this.comboBoxSearchProducts.FormattingEnabled = true;
            this.comboBoxSearchProducts.Location = new System.Drawing.Point(140, 30);
            this.comboBoxSearchProducts.Name = "comboBoxSearchProducts";
            this.comboBoxSearchProducts.Size = new System.Drawing.Size(106, 21);
            this.comboBoxSearchProducts.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Search Product";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxCriticalOnly);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBoxBuiltInHouse);
            this.groupBox1.Controls.Add(this.comboBoxSearchProducts);
            this.groupBox1.Controls.Add(this.checkBoxModifiedRecently);
            this.groupBox1.Location = new System.Drawing.Point(33, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 185);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSelectAll.Location = new System.Drawing.Point(33, 327);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(131, 31);
            this.buttonSelectAll.TabIndex = 14;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // buttonSelectBelowSafety
            // 
            this.buttonSelectBelowSafety.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSelectBelowSafety.Location = new System.Drawing.Point(33, 365);
            this.buttonSelectBelowSafety.Name = "buttonSelectBelowSafety";
            this.buttonSelectBelowSafety.Size = new System.Drawing.Size(131, 31);
            this.buttonSelectBelowSafety.TabIndex = 15;
            this.buttonSelectBelowSafety.Text = "Select Below Safety";
            this.buttonSelectBelowSafety.UseVisualStyleBackColor = true;
            this.buttonSelectBelowSafety.Click += new System.EventHandler(this.buttonSelectBelowSafety_Click);
            // 
            // buttonSelectBelowReorder
            // 
            this.buttonSelectBelowReorder.Location = new System.Drawing.Point(33, 403);
            this.buttonSelectBelowReorder.Name = "buttonSelectBelowReorder";
            this.buttonSelectBelowReorder.Size = new System.Drawing.Size(131, 31);
            this.buttonSelectBelowReorder.TabIndex = 16;
            this.buttonSelectBelowReorder.Text = "Select Below Reorder";
            this.buttonSelectBelowReorder.UseVisualStyleBackColor = true;
            this.buttonSelectBelowReorder.Click += new System.EventHandler(this.buttonSelectBelowReorder_Click);
            // 
            // ReorderProductsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 574);
            this.Controls.Add(this.buttonSelectBelowReorder);
            this.Controls.Add(this.buttonSelectBelowSafety);
            this.Controls.Add(this.buttonSelectAll);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonReorderSelected);
            this.Controls.Add(this.buttonApplyFilters);
            this.Controls.Add(this.dataGridViewReorderProducts);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ReorderProductsForm";
            this.Text = "ReorderProductsForm";
            this.Load += new System.EventHandler(this.ReorderProductsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReorderProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.DataGridView dataGridViewReorderProducts;
        private System.Windows.Forms.CheckBox checkBoxCriticalOnly;
        private System.Windows.Forms.CheckBox checkBoxBuiltInHouse;
        private System.Windows.Forms.CheckBox checkBoxModifiedRecently;
        private System.Windows.Forms.Button buttonApplyFilters;
        private System.Windows.Forms.Button buttonReorderSelected;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox comboBoxSearchProducts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonSelectBelowSafety;
        private System.Windows.Forms.Button buttonSelectBelowReorder;
    }
}