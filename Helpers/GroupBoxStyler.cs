using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SokProodos
{
    public static class GroupBoxStyler
    {
        public static void StyleGroupBoxesInForm(Form form)
        {
            foreach (Control control in form.Controls)
            {
                if (control is GroupBox groupBox)
                {
                    groupBox.Paint += (sender, e) => PaintRoundedGroupBox(sender as GroupBox, e);
                }
            }
        }

        private static void PaintRoundedGroupBox(GroupBox box, PaintEventArgs e)
        {
            if (box == null) return;

            e.Graphics.Clear(box.BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Parameters
            int radius = 12;
            int borderWidth = 2;
            Color borderColor = Color.FromArgb(88, 101, 242); // Soft blue
            Color backgroundColor = box.BackColor;
            Color textColor = box.ForeColor;

            SizeF stringSize = e.Graphics.MeasureString(box.Text, box.Font);
            Rectangle rect = new Rectangle(0, (int)(stringSize.Height / 2), box.Width - 1, box.Height - (int)(stringSize.Height / 2) - 1);

            using (GraphicsPath path = GetRoundedRectanglePath(rect, radius))
            using (Pen pen = new Pen(borderColor, borderWidth))
            {
                // Border
                e.Graphics.DrawPath(pen, path);

                // Text
                e.Graphics.FillRectangle(new SolidBrush(backgroundColor), new RectangleF(10, 0, stringSize.Width + 4, stringSize.Height));
                e.Graphics.DrawString(box.Text, box.Font, new SolidBrush(textColor), 10, 0);
            }
        }

        private static GraphicsPath GetRoundedRectanglePath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
