﻿using System;
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
using System.Windows.Forms.DataVisualization.Charting;




namespace SokProodos
{
    public partial class MainForm : Form
    {

        private Chart chartOrders;
        private Chart chartTopProducts;
        private Chart chartSalesCategories;
        string connectionString = @"Server=SOCHAX\SQLEXPRESS;Database=AdventureWorks2022;Trusted_Connection=True;";
        public MainForm()
        {
            InitializeComponent();
            InitializeDashboard();
            LoadOrderChart(DateTime.Now.AddMonths(-6));
            LoadTopProductsChart(DateTime.Now.AddMonths(-6));
            LoadStockChart(DateTime.Now.AddMonths(-6));
            LoadCurrentUser();
            LoadDashboardInfo();
            CreateStyledSideMenu();
            CreateRightActionButtons();
            this.StartPosition = FormStartPosition.CenterScreen;
            UIStyler.StyleButtonsInForm(this);


            Label labelCurrentUser = new Label
            {
                Name = "labelCurrentUser",
                Text = "👤 Current User: Loading...",
                Font = new Font("Segoe UI", 9, FontStyle.Regular), 
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(labelCurrentUser);

            
            Label labelDateTime = new Label
            {
                Name = "labelDateTime",
                Text = "🕒 Date: ...",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(20, 50),
                AutoSize = true
            };
            this.Controls.Add(labelDateTime);
            
            dateTimeTimer = new Timer();
            dateTimeTimer.Interval = 1000; 
            dateTimeTimer.Tick += DateTimeTimer_Tick;
            dateTimeTimer.Start();


            
            Label labelTotalOrders = new Label
            {
                Name = "labelTotalOrders",
                Text = "✅ Total Orders: ...",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(20, 70),
                AutoSize = true
            };
            this.Controls.Add(labelTotalOrders);

            labelAboutUs.Text = "🌐 About Us";
            labelAboutUs.Font = new Font("Segoe UI", 9, FontStyle.Underline);
            labelAboutUs.ForeColor = Color.LightBlue;
            labelAboutUs.Cursor = Cursors.Hand;
            labelAboutUs.BackColor = Color.Transparent; 

            labelAboutUs.Click += (s, e) =>
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://www.adventure-works.com", 
                    UseShellExecute = true
                });
            };

            labelAboutUs.MouseEnter += (s, e) => labelAboutUs.ForeColor = Color.DeepSkyBlue;
            labelAboutUs.MouseLeave += (s, e) => labelAboutUs.ForeColor = Color.LightBlue;
            labelAboutUs.BringToFront();


        }
        private void LoadCurrentUser()
        {
            if (!string.IsNullOrEmpty(GlobalSession.LoggedInUser))
            {
                labelCurrentUser.Text = "👤 Current User: " + GlobalSession.LoggedInUser;
            }
            else
            {
                labelCurrentUser.Text = "👤 Current User: Not Found";
            }
        }

        private Timer dateTimeTimer;
        private void DateTimeTimer_Tick(object sender, EventArgs e)
        {
            Label labelDateTime = this.Controls.Find("labelDateTime", true).FirstOrDefault() as Label;
            if (labelDateTime != null)
            {
                labelDateTime.Text = "🕒 Date: " + DateTime.Now.ToString("dddd, dd MMM yyyy HH:mm:ss");
            }
        }


        private void LoadDashboardInfo()
        {
            
            Label labelDateTime = this.Controls.Find("labelDateTime", true).FirstOrDefault() as Label;
            if (labelDateTime != null)
            {
                labelDateTime.Text = "🕒 Date: " + DateTime.Now.ToString("dddd, dd MMM yyyy HH:mm");
            }

            // total orders me bash to ID, den ksexorizei sales orders kai buy orders
            Label labelTotalOrders = this.Controls.Find("labelTotalOrders", true).FirstOrDefault() as Label;
            if (labelTotalOrders != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT COUNT(SalesOrderID) FROM Sales.SalesOrderHeader";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            int totalOrders = (int)cmd.ExecuteScalar();
                            labelTotalOrders.Text = "✅ Total Orders: " + totalOrders.ToString("N0");
                        }
                    }
                }
                catch (Exception ex)
                {
                    labelTotalOrders.Text = "❌ Error loading orders";
                }
            }
        }

        private void CreateStyledSideMenu()
        {
            Panel sideMenu = new Panel
            {
                Name = "sideMenuPanel",
                Size = new Size(200, this.Height),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(0, 180, 200),
                AutoScroll = true
            };
            this.Controls.Add(sideMenu);

            Font sectionFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font buttonFont = new Font("Segoe UI", 9, FontStyle.Regular);
            Color buttonColor = Color.FromArgb(0, 160, 180);
            Color hoverColor = Color.FromArgb(0, 140, 160);

            int paddingY = 12;
            int currentY = 20;

            void AddSectionLabel(string title)
            {
                Label label = new Label
                {
                    Text = title,
                    Font = sectionFont,
                    ForeColor = Color.White,
                    Location = new Point(10, currentY),
                    AutoSize = true
                };
                sideMenu.Controls.Add(label);
                currentY += 30;
            }

            Button CreateButton(string text, EventHandler onClick)
            {
                Button btn = new Button
                {
                    Text = text,
                    Font = buttonFont,
                    Size = new Size(180, 36),
                    Location = new Point(10, currentY),
                    BackColor = buttonColor,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(8, 0, 0, 0),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;

                btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
                btn.MouseLeave += (s, e) => btn.BackColor = buttonColor;

                // Round corners properly after paint (when size is known)
                btn.Paint += (s, e) =>
                {
                    GraphicsPath path = GraphicsExtensions.CreateRoundedRect(btn.ClientRectangle, 8);
                    btn.Region = new Region(path);
                };

                btn.Click += onClick;

                currentY += btn.Height + paddingY;
                return btn;
            }

            // prwto section
            AddSectionLabel("Insert / Update");
            sideMenu.Controls.Add(CreateButton("👤 Insert Customer", button1_Click));
            sideMenu.Controls.Add(CreateButton("🏭 Insert Supplier", button2_Click));
            sideMenu.Controls.Add(CreateButton("📦 Insert Stock", button3_Click));
            sideMenu.Controls.Add(CreateButton("👔 Insert Seller", button4_Click));

            currentY += 15;

            // deutero section
            AddSectionLabel("Information");
            sideMenu.Controls.Add(CreateButton("ℹ️ Customer Info", button10_Click));
            sideMenu.Controls.Add(CreateButton("ℹ️ Supplier Info", button11_Click));
            sideMenu.Controls.Add(CreateButton("ℹ️ Stock Info", button12_Click));
            sideMenu.Controls.Add(CreateButton("ℹ️ Seller Info", button9_Click));
        }


        private void CreateRightActionButtons()
        {
            Font buttonFont = new Font("Segoe UI", 9, FontStyle.Regular);
            Color buttonColor = Color.FromArgb(0, 160, 255);
            Color hoverColor = Color.FromArgb(0, 140, 220);

            int startX = this.Width - 180;
            int startY = 30;
            int buttonWidth = 150;
            int buttonHeight = 36;
            int spacing = 12;

            void AddStyledButton(string text, EventHandler onClick)
            {
                Button btn = new Button
                {
                    Text = text,
                    Font = buttonFont,
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(startX, startY),
                    BackColor = buttonColor,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor = Cursors.Hand,
                    Padding = new Padding(8, 0, 0, 0)
                };

                btn.FlatAppearance.BorderSize = 0;
                btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
                btn.MouseLeave += (s, e) => btn.BackColor = buttonColor;

                // rounded gwnies
                GraphicsPath path = GraphicsExtensions.CreateRoundedRect(btn.ClientRectangle, 8);
                btn.Region = new Region(path);

                btn.Click += onClick;

                
                this.Controls.Add(btn);

                // bring to front giati to blockarei to chart gtxm
                btn.BringToFront();

                
                startY += buttonHeight + spacing;
            }

            
            AddStyledButton("Create Sale Order", button7_Click);
            AddStyledButton("Order History", button6_Click);
            AddStyledButton("Stock History", button8_Click);
        }



        private void InitializeDashboard()
        {
            int panelWidth = 960;
            int panelHeight = 600;

            Panel panelDashboard = new Panel
            {
                Name = "panelDashboard",
                Size = new Size(panelWidth, panelHeight),
                Location = new Point(260, 160), // moved panel more to the right
                BackColor = Color.Transparent
            };
            this.Controls.Add(panelDashboard);

            
            Label lblDashboard = new Label
            {
                Text = "📊 Business Dashboard",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                AutoSize = true
            };
            panelDashboard.Controls.Add(lblDashboard);

            
            ComboBox comboBoxFilter = new ComboBox
            {
                Name = "comboBoxFilter",
                Location = new Point(10, 50),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                Width = 200
            };

            comboBoxFilter.Items.AddRange(new string[]
            {
        "Last 10 Days",
        "Last 1 Month",
        "Last 3 Months",
        "Last 6 Months",
        "Last 12 Months",
        "All Time"
            });

            comboBoxFilter.SelectedIndex = 3;
            comboBoxFilter.SelectedIndexChanged += ComboBoxFilter_SelectedIndexChanged;
            panelDashboard.Controls.Add(comboBoxFilter);

            string timeRangeText = comboBoxFilter.SelectedItem?.ToString() ?? "Last 6 Months";

            // Orders Label
            Label lblOrders = new Label
            {
                Name = "lblOrders",
                Text = $"Orders - {timeRangeText}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(10, 80),
                AutoSize = true
            };
            panelDashboard.Controls.Add(lblOrders);

            // Orders 
            Chart chartOrders = CreateChart("chartOrders", "Orders", SeriesChartType.Column, Color.FromArgb(0, 122, 204));
            chartOrders.Location = new Point(10, 100);
            chartOrders.Size = new Size(450, 200);
            panelDashboard.Controls.Add(chartOrders);

            // Stock Label
            Label lblStock = new Label
            {
                Name = "lblStock",
                Text = $"Stock - {timeRangeText}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(480, 80), 
                AutoSize = true
            };
            panelDashboard.Controls.Add(lblStock);

            // Stock 
            Chart chartStock = CreateChart("chartStock", "Stock", SeriesChartType.Pie, Color.FromArgb(0, 122, 204));
            chartStock.Series["Stock"]["PieLabelStyle"] = "Disabled";
            chartStock.Legends[0].Enabled = true;
            chartStock.Legends[0].Docking = Docking.Bottom;
            chartStock.Location = new Point(480, 100); 
            chartStock.Size = new Size(450, 200);
            panelDashboard.Controls.Add(chartStock);

            // Top Products Label
            Label lblTopProducts = new Label
            {
                Name = "lblTopProducts",
                Text = $"Top Products - {timeRangeText}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(10, 300),
                AutoSize = true
            };
            panelDashboard.Controls.Add(lblTopProducts);

            // Top Products 
            Chart chartTopProducts = CreateChart("chartTopProducts", "TopProducts", SeriesChartType.Bar, Color.FromArgb(0, 122, 204));
            chartTopProducts.Location = new Point(10, 320);
            chartTopProducts.Size = new Size(930, 180);
            panelDashboard.Controls.Add(chartTopProducts);
        }



        private void ComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string selected = comboBox.SelectedItem.ToString();

            DateTime? fromDate = null;

            switch (selected)
            {
                case "Last 10 Days":
                    fromDate = DateTime.Now.AddDays(-10);
                    break;
                case "Last 1 Month":
                    fromDate = DateTime.Now.AddMonths(-1);
                    break;
                case "Last 3 Months":
                    fromDate = DateTime.Now.AddMonths(-3);
                    break;
                case "Last 6 Months":
                    fromDate = DateTime.Now.AddMonths(-6);
                    break;
                case "Last 12 Months":
                    fromDate = DateTime.Now.AddMonths(-12);
                    break;
                case "All Time":
                    fromDate = null;
                    break;
            }

            
            LoadOrderChart(fromDate);
            LoadTopProductsChart(fromDate);
            LoadStockChart(fromDate);

            
            Control panel = this.Controls.Find("panelDashboard", true).FirstOrDefault();
            if (panel != null)
            {
                var lblOrders = panel.Controls.Find("lblOrders", true).FirstOrDefault() as Label;
                var lblStock = panel.Controls.Find("lblStock", true).FirstOrDefault() as Label;
                var lblTopProducts = panel.Controls.Find("lblTopProducts", true).FirstOrDefault() as Label;

                if (lblOrders != null) lblOrders.Text = $"Orders - {selected}";
                if (lblStock != null) lblStock.Text = $"Stock - {selected}";
                if (lblTopProducts != null) lblTopProducts.Text = $"Top Products - {selected}";
            }
        }



        private void LoadTopProductsChart(DateTime? fromDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string whereClause = fromDate.HasValue ? "WHERE h.OrderDate >= @fromDate" : "";
                string query = $@"
            SELECT TOP 5 p.Name, SUM(d.OrderQty) AS TotalSold
            FROM Sales.SalesOrderDetail d
            JOIN Production.Product p ON d.ProductID = p.ProductID
            JOIN Sales.SalesOrderHeader h ON d.SalesOrderID = h.SalesOrderID
            {whereClause}
            GROUP BY p.Name
            ORDER BY TotalSold DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (fromDate.HasValue)
                        command.Parameters.AddWithValue("@fromDate", fromDate.Value);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Chart chart = (Chart)this.Controls.Find("chartTopProducts", true)[0];
                        chart.Series["TopProducts"].Points.Clear();

                        while (reader.Read())
                        {
                            string productName = reader.GetString(0);
                            int totalSold = reader.GetInt32(1);
                            chart.Series["TopProducts"].Points.AddXY(productName, totalSold);
                        }

                        chart.Series["TopProducts"]["PixelPointWidth"] = "10";
                        chart.Series["TopProducts"]["PointWidth"] = "0.4";
                        chart.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
                        chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 9, FontStyle.Bold);
                        chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 9, FontStyle.Bold);
                    }
                }
            }
        }


        private void LoadStockChart(DateTime? fromDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string whereClause = fromDate.HasValue ? "WHERE h.OrderDate >= @fromDate" : "";
                string query = $@"
            SELECT TOP 8 p.Name, SUM(d.OrderQty) AS SoldQty
            FROM Sales.SalesOrderDetail d
            JOIN Production.Product p ON d.ProductID = p.ProductID
            JOIN Sales.SalesOrderHeader h ON d.SalesOrderID = h.SalesOrderID
            {whereClause}
            GROUP BY p.Name
            ORDER BY SoldQty DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (fromDate.HasValue)
                        command.Parameters.AddWithValue("@fromDate", fromDate.Value);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Chart chart = (Chart)this.Controls.Find("chartStock", true)[0];
                        chart.Series["Stock"].Points.Clear();

                        while (reader.Read())
                        {
                            string productName = reader.GetString(0);
                            int stockCount = reader.GetInt32(1);
                            chart.Series["Stock"].Points.AddXY(productName, stockCount);
                        }
                    }
                }
            }
        }



        private Chart CreateChart(string name, string seriesName, SeriesChartType chartType, Color color)
        {
            Chart chart = new Chart
            {
                Name = name,
                Size = new Size(320, 200),
                BackColor = Color.White,
                ForeColor = Color.Black,
                TextAntiAliasingQuality = TextAntiAliasingQuality.High,
                Palette = ChartColorPalette.None,
                BorderlineDashStyle = ChartDashStyle.Solid,
                BorderlineColor = Color.LightGray,
                BorderlineWidth = 1
            };

            
            ChartArea area = new ChartArea("MainArea")
            {
                BackColor = Color.White
            };

            area.AxisX.LineColor = Color.Gray;
            area.AxisX.LabelStyle.ForeColor = Color.Black;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;

            area.AxisY.LineColor = Color.Gray;
            area.AxisY.LabelStyle.ForeColor = Color.Black;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.ChartAreas.Add(area);

            Series series = new Series(seriesName)
            {
                ChartType = chartType,
                Color = color,
                BorderWidth = 2
            };

            series.ToolTip = "#VALX: #VALY";

            if (chartType == SeriesChartType.Pie)
            {
                series["PieDrawingStyle"] = "SoftEdge";
                series.Color = color;
            }

            chart.Series.Add(series);

            Legend legend = new Legend("Legend")
            {
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Docking = Docking.Bottom
            };

            chart.Legends.Add(legend);

            chart.MouseMove += Chart_MouseMove;
            chart.MouseLeave += Chart_MouseLeave;
            chart.DoubleBuffered(true);

            // padding
            chart.Margin = new Padding(10);

            return chart;
        }



        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            Chart chart = sender as Chart;
            if (chart == null) return;

            HitTestResult hit = chart.HitTest(e.X, e.Y);
            if (hit.ChartElementType == ChartElementType.DataPoint)
            {
                DataPoint point = hit.Series.Points[hit.PointIndex];

                
                if (point.Tag == null)
                {
                    point.Tag = point.Color; 
                    point.Color = ControlPaint.Light(point.Color);
                    point.MarkerSize = 10;
                }
            }
        }


        private void Chart_MouseLeave(object sender, EventArgs e)
        {
            Chart chart = sender as Chart;
            if (chart == null) return;

            foreach (var series in chart.Series)
            {
                foreach (var point in series.Points)
                {
                    if (point.Tag != null && point.Tag is Color originalColor)
                    {
                        point.Color = originalColor;
                        point.Tag = null;
                    }
                    point.MarkerSize = 5;
                }
            }
        }




        private void LoadOrderChart(DateTime? fromDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string whereClause = fromDate.HasValue ? "WHERE OrderDate >= @fromDate" : "";
                string query = $@"
            SELECT FORMAT(OrderDate, 'yyyy-MM') AS Month, COUNT(SalesOrderID) AS TotalOrders
            FROM Sales.SalesOrderHeader
            {whereClause}
            GROUP BY FORMAT(OrderDate, 'yyyy-MM')
            ORDER BY Month ASC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (fromDate.HasValue)
                        command.Parameters.AddWithValue("@fromDate", fromDate.Value);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Chart chart = (Chart)this.Controls.Find("chartOrders", true)[0];
                        chart.Series["Orders"].Points.Clear();

                        while (reader.Read())
                        {
                            string month = reader.GetString(0);
                            int totalOrders = reader.GetInt32(1);
                            chart.Series["Orders"].Points.AddXY(month, totalOrders);
                        }
                    }
                }
            }
        }




        private void LoadStockChart()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT TOP 8 p.Name, SUM(i.Quantity) AS StockCount
                    FROM Production.Product p
                    JOIN Production.ProductInventory i ON p.ProductID = i.ProductID
                    WHERE p.FinishedGoodsFlag = 1
                    GROUP BY p.Name
                    ORDER BY StockCount DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Chart chart = (Chart)this.Controls.Find("chartStock", true)[0];
                    chart.Series["Stock"].Points.Clear();

                    while (reader.Read())
                    {
                        string productName = reader.GetString(0);
                        int stockCount = reader.GetInt32(1);
                        chart.Series["Stock"].Points.AddXY(productName, stockCount);
                    }
                }
            }
        }

        
        private void LoadTopProductsChart()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT TOP 5 p.Name, SUM(d.OrderQty) AS TotalSold
            FROM Sales.SalesOrderDetail d
            JOIN Production.Product p ON d.ProductID = p.ProductID
            GROUP BY p.Name
            ORDER BY TotalSold DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Chart chart = (Chart)this.Controls.Find("chartTopProducts", true)[0];
                    chart.Series["TopProducts"].Points.Clear();

                    while (reader.Read())
                    {
                        string productName = reader.GetString(0);
                        int totalSold = reader.GetInt32(1);
                        chart.Series["TopProducts"].Points.AddXY(productName, totalSold);
                    }

                    
                    chart.Series["TopProducts"]["PixelPointWidth"] = "10";  
                    chart.Series["TopProducts"]["PointWidth"] = "0.4";      

                    
                    chart.ChartAreas[0].AxisX.LabelStyle.Angle = 0;  
                    chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 9, FontStyle.Bold);  
                    chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 9, FontStyle.Bold);  
                }
            }
        }



        private void Button_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(114, 137, 218); 
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(88, 101, 242); 
        }


        private void button1_Click(object sender, EventArgs e)
        {
            CustomerForm CustomerForm = new CustomerForm();
            CustomerForm.Show();


            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SupplierForm SupplierForm = new SupplierForm();
            SupplierForm.Show();


            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StockForm StockForm = new StockForm();
            StockForm.Show();


            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SellersForm SellersForm = new SellersForm();
            SellersForm.Show();


            this.Hide();
        }

        

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 Form1 = new Form1();
            Form1.Show();


            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            OrderHistoryForm OrderHistoryForm = new OrderHistoryForm();
            OrderHistoryForm.Show();


            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OrderForm OrderForm = new OrderForm();
            OrderForm.Show();


            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            StockHistory StockHistory = new StockHistory();
            StockHistory.Show();


            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SellerInfo SellerInfo = new SellerInfo();
            SellerInfo.Show();


            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CustomerInfo CustomerInfo = new CustomerInfo();
            CustomerInfo.Show();


            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SupplierInfo SupplierInfo = new SupplierInfo();
            SupplierInfo.Show();


            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            StockInfo StockInfo = new StockInfo();
            StockInfo.Show();


            this.Hide();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void labelCurrentUser_Click(object sender, EventArgs e)
        {

        }

        private void buttonLogOut_Click(object sender, EventArgs e)
        {
            Form1 Form1 = new Form1();
            Form1.Show();

            this.Hide();
        }
    }
    public static class ChartExtensions
    {
        public static void DoubleBuffered(this Control control, bool enable)
        {
            var doubleBufferPropertyInfo = control.GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            doubleBufferPropertyInfo?.SetValue(control, enable, null);
        }
    }

}
