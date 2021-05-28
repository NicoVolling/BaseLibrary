using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseLibrary.GUI.Controls.BaseControls
{
    /// <summary>
    /// Ein <see cref="Panel"/>, das ein Bild anzeigt, an das der Nutzer heranzoomen kann.
    /// </summary>
    public partial class ZoomPanel : Control
    {
        /// <summary>
        /// Der <see cref="Point"/>, der den Offset des Bildes zur oberen linken Bildschirmecke angibt.
        /// </summary>
        public Point Offset = new Point(0, 0);

        private Image _Image;

        private float _MaxZoomFactor = 1.0f;

        private float _MinZoomFactor = 1.0f;

        private float _ZoomFactor = 1.0f;

        private bool IsMouseDown = false;

        private Point OldMousePosition = new Point(0, 0);

        /// <summary>
        /// Konstruktor
        /// </summary>
        public ZoomPanel()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        /// <summary>
        /// Das Event, das ausgelöst wird, wenn sich der Zoom des Bildes ändert.
        /// </summary>
        public event EventHandler ZoomChanged;

        [Category("Content")]
        public Image Image
        {
            get => _Image; set
            {
                _Image = value;
                if (value != null)
                {
                    ZoomFactor = (float)Width / (float)value.Width;
                }
                this.Refresh();
            }
        }

        /// <summary>
        /// Der Faktor, um den das Bild maximal gezoomt wird.
        /// </summary>
        [Category("Content")]
        public float MaxZoomFactor
        {
            get => _MaxZoomFactor;
            set
            {
                _MaxZoomFactor = value;
                if (MinZoomFactor > MaxZoomFactor)
                {
                    MinZoomFactor = MaxZoomFactor;
                }
                if (ZoomFactor > MaxZoomFactor)
                {
                    ZoomFactor = MaxZoomFactor;
                }
            }
        }

        /// <summary>
        /// Der Faktor, um den das Bild minimal gezoomt wird.
        /// </summary>
        [Category("Content")]
        public float MinZoomFactor
        {
            get => _MinZoomFactor;
            set
            {
                _MinZoomFactor = value;
                if (MaxZoomFactor < MinZoomFactor)
                {
                    MaxZoomFactor = MinZoomFactor;
                }
                if (ZoomFactor < MinZoomFactor)
                {
                    ZoomFactor = MinZoomFactor;
                }
            }
        }

        /// <summary>
        /// Die aktuelle sichbare Größe des Bildes.
        /// </summary>
        [Category("Content")]
        public SizeF SizeF { get { if (Image == null) { return new SizeF(0, 0); } return new SizeF(Image.Width * ZoomFactor, Image.Height * ZoomFactor); } }

        /// <summary>
        /// Der aktuelle Faktor, um den das Bild gezoomt ist.
        /// </summary>
        [Category("Content")]
        public float ZoomFactor { get => _ZoomFactor; set { _ZoomFactor = value; this.Refresh(); OnZoomChanged(); } }

        public override void Refresh()
        {
            GC.Collect();
            base.Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Cursor = Cursors.SizeAll;
            OldMousePosition = e.Location;
            IsMouseDown = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsMouseDown)
            {
                int x = e.Location.X - OldMousePosition.X;
                int y = e.Location.Y - OldMousePosition.Y;
                Offset = new Point(Offset.X + x, Offset.Y + y);
                OldMousePosition = e.Location;
                this.Refresh();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            IsMouseDown = false;
            this.Cursor = Cursors.Default;
            OldMousePosition = new Point(0, 0);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta < 0)
            {
                float newZoom = ZoomFactor * (1 - 0.08f);
                if (newZoom < MinZoomFactor || newZoom > MaxZoomFactor)
                {
                    newZoom = ZoomFactor;
                }
                ZoomFactor = newZoom;
            }
            else
            {
                float newZoom = ZoomFactor * (1 + 0.08f);
                if (newZoom < MinZoomFactor || newZoom > MaxZoomFactor)
                {
                    newZoom = ZoomFactor;
                }
                ZoomFactor = newZoom;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            Graphics gfx = pevent.Graphics;
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            gfx.Clear(this.BackColor);
            if (Image == null) { return; }
            gfx.TranslateTransform((-Image.Width * ZoomFactor / 2) + Width / 2, (-Image.Height * ZoomFactor / 2) + Height / 2);
            gfx.DrawImage(Image, Offset.X * ZoomFactor, Offset.Y * ZoomFactor, Image.Width * ZoomFactor, Image.Height * ZoomFactor);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Refresh();
        }

        protected void OnZoomChanged()
        {
            EventHandler handler = ZoomChanged;
            handler?.Invoke(this, new EventArgs());
        }
    }
}