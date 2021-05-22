using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseLibrary.GUI.Controls.BaseControls
{
    public class StatusLed : Control
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            float Height = this.Height;
            float Width = this.Width;
            Graphics gfx = e.Graphics;
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            Color DrawForeColor = this.ForeColor;

            GraphicsPath grPath1 = new GraphicsPath();
            GraphicsPath grPath2 = new GraphicsPath();

            grPath1.AddPie(0, 0, Height, Height, 0, 360);
            grPath2.AddPie(1, 1, Height - 2, Height - 2, 0, 360);

            gfx.Clear(this.Parent.BackColor);
            gfx.FillPath(new SolidBrush(Color.Black), grPath1);
            gfx.FillPath(new SolidBrush(DrawForeColor), grPath2);
        }
    }
}