using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SokProodos
{
    public static class GraphicsExtensions
    {
        public static GraphicsPath CreateRoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            
            path.AddArc(arc, 180, 90);

            
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}

