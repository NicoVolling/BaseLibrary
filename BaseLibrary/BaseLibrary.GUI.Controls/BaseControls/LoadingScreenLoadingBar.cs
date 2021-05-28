using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BaseLibrary.GUI.Controls.BaseControls
{
    /// <summary>
    /// Eine Ladeleiste
    /// </summary>
    public partial class LoadingScreenLoadingBar : BaseLibrary.GUI.Controls.BaseControls.UserControlBL
    {
        private int Factor = 1;

        private Timer t;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public LoadingScreenLoadingBar()
        {
            InitializeComponent();
            t = new Timer();
            t.Interval = 20;
            t.Tick += T_Tick;
        }

        /// <summary>
        /// Die alternativfarbe.
        /// </summary>
        [Category("Farben")]
        public Color SecondColor { get; set; } = Color.FromArgb(255, 128, 128);

        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);
            OnSizeChanged(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            t.Start();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (t != null) { t.Stop(); }
            loadingScreenBar1.Size = new Size(loadingScreenBar1.Width, this.Height - 6);
            loadingScreenBar1.Location = new Point(loadingScreenBar1.Location.X, 3);
            if (t != null) { t.Start(); }
        }

        private void T_Tick(object sender, EventArgs e)
        {
            loadingScreenBar1.Location = new Point(loadingScreenBar1.Location.X + 6 * Factor, loadingScreenBar1.Location.Y);
            if (loadingScreenBar1.Location.X + loadingScreenBar1.Width + 8 > this.Width)
            {
                Factor = -1;
                if (loadingScreenBar1.ForeColor == SecondColor) { loadingScreenBar1.ForeColor = this.ForeColor; } else { loadingScreenBar1.ForeColor = SecondColor; }
            }
            else
            if (loadingScreenBar1.Location.X - 8 < 0)
            {
                Factor = 1;
                if (loadingScreenBar1.ForeColor == SecondColor) { loadingScreenBar1.ForeColor = this.ForeColor; } else { loadingScreenBar1.ForeColor = SecondColor; }
            }
        }
    }
}