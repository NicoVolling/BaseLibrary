using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseLibrary.GUI.Controls.BaseControls
{
    /// <inheritdoc/>
    /// <summary>
    /// Ein <see cref="Label"/> mit einem aabgerundeten farbigen Hintergrund.
    /// </summary>
    public class TagLabel : Label
    {
        public override void Refresh()
        {
            base.Refresh();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            float X = 0;
            float Y = 0;
            float Height = this.Height;
            float Width = this.Width;
            Graphics gfx = e.Graphics;
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            Color DrawBackColor = this.BackColor;

            GraphicsPath grPath1 = new GraphicsPath();

            grPath1.AddPie(X, Y, Height, Height, 90, 180);
            grPath1.AddPie(Width - Height, 0, Height, Height, 270, 180);
            grPath1.AddRectangle(new RectangleF(Height / 2, Y, Width - Height, Height));

            gfx.Clear(this.Parent.BackColor);
            gfx.FillPath(new SolidBrush(DrawBackColor), grPath1);
        }
    }
}