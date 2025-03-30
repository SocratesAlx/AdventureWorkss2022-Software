namespace SokProodos
{
    partial class MainForm
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
            this.labelCurrentUser = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelDateTime = new System.Windows.Forms.Label();
            this.labelTotalOrders = new System.Windows.Forms.Label();
            this.buttonLogOut = new System.Windows.Forms.Button();
            this.labelAboutUs = new System.Windows.Forms.Label();
            this.labelTotalOrderProfit = new System.Windows.Forms.Label();
            this.labelReorderProducts = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCurrentUser
            // 
            this.labelCurrentUser.AutoSize = true;
            this.labelCurrentUser.BackColor = System.Drawing.Color.White;
            this.labelCurrentUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentUser.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelCurrentUser.Location = new System.Drawing.Point(207, 7);
            this.labelCurrentUser.Name = "labelCurrentUser";
            this.labelCurrentUser.Size = new System.Drawing.Size(51, 20);
            this.labelCurrentUser.TabIndex = 20;
            this.labelCurrentUser.Text = "label1";
            this.labelCurrentUser.Click += new System.EventHandler(this.labelCurrentUser_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(199, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Insert and Update";
            // 
            // labelDateTime
            // 
            this.labelDateTime.AutoSize = true;
            this.labelDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDateTime.Location = new System.Drawing.Point(208, 777);
            this.labelDateTime.Name = "labelDateTime";
            this.labelDateTime.Size = new System.Drawing.Size(44, 16);
            this.labelDateTime.TabIndex = 23;
            this.labelDateTime.Text = "label2";
            // 
            // labelTotalOrders
            // 
            this.labelTotalOrders.AutoSize = true;
            this.labelTotalOrders.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalOrders.Location = new System.Drawing.Point(207, 43);
            this.labelTotalOrders.Name = "labelTotalOrders";
            this.labelTotalOrders.Size = new System.Drawing.Size(51, 20);
            this.labelTotalOrders.TabIndex = 24;
            this.labelTotalOrders.Text = "label2";
            // 
            // buttonLogOut
            // 
            this.buttonLogOut.Location = new System.Drawing.Point(51, 777);
            this.buttonLogOut.Name = "buttonLogOut";
            this.buttonLogOut.Size = new System.Drawing.Size(98, 34);
            this.buttonLogOut.TabIndex = 25;
            this.buttonLogOut.Text = "Log Out";
            this.buttonLogOut.UseVisualStyleBackColor = true;
            this.buttonLogOut.Click += new System.EventHandler(this.buttonLogOut_Click);
            // 
            // labelAboutUs
            // 
            this.labelAboutUs.AutoSize = true;
            this.labelAboutUs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAboutUs.Location = new System.Drawing.Point(207, 757);
            this.labelAboutUs.Name = "labelAboutUs";
            this.labelAboutUs.Size = new System.Drawing.Size(51, 20);
            this.labelAboutUs.TabIndex = 26;
            this.labelAboutUs.Text = "label2";
            // 
            // labelTotalOrderProfit
            // 
            this.labelTotalOrderProfit.AutoSize = true;
            this.labelTotalOrderProfit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalOrderProfit.Location = new System.Drawing.Point(207, 80);
            this.labelTotalOrderProfit.Name = "labelTotalOrderProfit";
            this.labelTotalOrderProfit.Size = new System.Drawing.Size(51, 20);
            this.labelTotalOrderProfit.TabIndex = 27;
            this.labelTotalOrderProfit.Text = "label2";
            // 
            // labelReorderProducts
            // 
            this.labelReorderProducts.AutoSize = true;
            this.labelReorderProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReorderProducts.Location = new System.Drawing.Point(206, 111);
            this.labelReorderProducts.Name = "labelReorderProducts";
            this.labelReorderProducts.Size = new System.Drawing.Size(215, 20);
            this.labelReorderProducts.TabIndex = 28;
            this.labelReorderProducts.Text = "📦 Reorder Status: Loading...";
            this.labelReorderProducts.Click += new System.EventHandler(this.labelReorderProducts_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SokProodos.Properties.Resources.awc_logo;
            this.pictureBox1.Location = new System.Drawing.Point(469, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(516, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::SokProodos.Properties.Resources.pngwing_com__17_;
            this.pictureBox3.Location = new System.Drawing.Point(293, 688);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(1095, 235);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 22;
            this.pictureBox3.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1269, 823);
            this.Controls.Add(this.labelReorderProducts);
            this.Controls.Add(this.labelTotalOrderProfit);
            this.Controls.Add(this.labelAboutUs);
            this.Controls.Add(this.buttonLogOut);
            this.Controls.Add(this.labelTotalOrders);
            this.Controls.Add(this.labelDateTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelCurrentUser);
            this.Controls.Add(this.pictureBox3);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelCurrentUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label labelDateTime;
        private System.Windows.Forms.Label labelTotalOrders;
        private System.Windows.Forms.Button buttonLogOut;
        private System.Windows.Forms.Label labelAboutUs;
        private System.Windows.Forms.Label labelTotalOrderProfit;
        private System.Windows.Forms.Label labelReorderProducts;
    }
}