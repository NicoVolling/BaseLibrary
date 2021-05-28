using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseLibrary.GUI.Controls.BaseControls
{
    public partial class LoadingScreenBarItem : Control
    {
        public LoadingScreenBarItem()
        {
            InitializeComponent();
        }

        public override Color BackColor
        {
            get => base.BackColor; set
            {
                base.BackColor = value;
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            float Height = this.Height;
            float Width = this.Width;
            Graphics gfx = e.Graphics;
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            Color DrawBackColor = this.ForeColor;

            GraphicsPath grPath1 = new GraphicsPath();

            grPath1.AddPie(0, 0, Height, Height, 90, 180);
            grPath1.AddPie(Width - Height, 0, Height, Height, 270, 180);
            grPath1.AddRectangle(new RectangleF(Height / 2, 0, Width - Height, Height));

            gfx.Clear(this.Parent.BackColor);
            gfx.FillPath(new SolidBrush(DrawBackColor), grPath1);
        }
    }
}