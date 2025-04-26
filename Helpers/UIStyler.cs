using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SokProodos
{
    public static class UIStyler
    {
        public static void StyleButtonsInForm(Form form)
        {
            StyleButtonsRecursive(form);
        }

        private static void StyleButtonsRecursive(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = Color.FromArgb(0, 160, 180);
                    btn.ForeColor = Color.White;
                    btn.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.Padding = new Padding(0);
                    btn.Cursor = Cursors.Hand;

                    // Hover effect
                    btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 140, 160);
                    btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(0, 160, 180);

                    // Rounded corners
                    btn.Paint += (s, e) =>
                    {
                        GraphicsPath path = GraphicsExtensions.CreateRoundedRect(btn.ClientRectangle, 8);
                        btn.Region = new Region(path);
                    };
                }

                // Recursively style buttons inside panels, groupboxes, etc.
                if (ctrl.HasChildren)
                    StyleButtonsRecursive(ctrl);
            }
        }
    }
}
