using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SokProodos
{
    public class StyledButton : Button
    {
        private Color baseColor = Color.FromArgb(0, 160, 180);
        private Color hoverColor = Color.FromArgb(0, 140, 160);

        public StyledButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = baseColor;
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Padding = new Padding(8, 0, 0, 0);
            this.TextAlign = ContentAlignment.MiddleLeft;
            this.Cursor = Cursors.Hand;

            this.MouseEnter += (s, e) => this.BackColor = hoverColor;
            this.MouseLeave += (s, e) => this.BackColor = baseColor;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            GraphicsPath path = GraphicsExtensions.CreateRoundedRect(this.ClientRectangle, 8);
            this.Region = new Region(path);
        }
    }
}
