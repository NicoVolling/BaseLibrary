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
    /// Eine Titelleiste
    /// </summary>
    /// <inheritdoc/>
    public partial class TitleBar : BaseLibrary.GUI.Controls.BaseControls.UserControlBL
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public TitleBar()
        {
            InitializeComponent();
            this.TabStop = false;
        }

        public override Font Font { get => label1.Font; set => label1.Font = value; }

        /// <summary>
        /// Das anzuzeigende Bild.
        /// </summary>
        [Category("Darstellung")]
        public Image Image { get => pictureBox1.Image; set => pictureBox1.Image = value; }

        /// <summary>
        /// Sichtbarkeit des anzuzeigenden Bildes.
        /// </summary>
        [Category("Darstellung")]
        public bool ImageVisible { get => pictureBox1.Visible; set => pictureBox1.Visible = value; }

        /// <summary>
        /// Der anzuzeigende Titel.
        /// </summary>
        [Category("Darstellung")]
        public string Title { get => label1.Text; set => label1.Text = value; }

        private void panel1_DockChanged(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(pictureBox1.Height, pictureBox1.Height);
        }

        private void TitleBar_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(pictureBox1.Height, pictureBox1.Height);
        }
    }
}