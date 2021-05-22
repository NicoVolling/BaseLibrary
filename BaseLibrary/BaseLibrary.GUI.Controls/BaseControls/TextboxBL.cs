using System;
using System.ComponentModel;
using System.Drawing;

namespace BaseLibrary.GUI.Controls.BaseControls
{
    /// <summary>
    /// Eine Erweiterung der <see cref="System.Windows.Forms.TextBox"/>
    /// </summary>
    /// <inheritdoc/>
    public partial class TextboxBL : UserControlBL
    {
        #region Design

        /// <summary>
        /// Die in diesem Steuerelement enthaltene <see cref="System.Windows.Forms.TextBox"/>
        /// </summary>
        public System.Windows.Forms.TextBox Textbox;

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel PanelTB;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.PanelTB = new System.Windows.Forms.Panel();
            this.Textbox = new System.Windows.Forms.TextBox();
            this.PanelTB.SuspendLayout();
            this.SuspendLayout();
            //
            // PanelTB
            //
            this.PanelTB.BackColor = System.Drawing.Color.White;
            this.PanelTB.Controls.Add(this.Textbox);
            this.PanelTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTB.Location = new System.Drawing.Point(1, 1);
            this.PanelTB.Name = "PanelTB";
            this.PanelTB.Size = new System.Drawing.Size(245, 22);
            this.PanelTB.TabIndex = 0;
            this.PanelTB.Click += PanelTB_Click;
            this.PanelTB.Cursor = System.Windows.Forms.Cursors.IBeam;
            //
            // Textbox
            //
            this.Textbox.BackColor = System.Drawing.Color.White;
            this.Textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Textbox.Location = new System.Drawing.Point(3, 3);
            this.Textbox.Name = "Textbox";
            this.Textbox.Size = new System.Drawing.Size(239, 16);
            this.Textbox.TabIndex = 0;
            this.Textbox.GotFocus += Textbox_GotFocus;
            this.Textbox.LostFocus += Textbox_LostFocus;
            //
            // TextboxBL
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            base.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.PanelTB);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CTextbox";
            this.Border = true;
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Size = new System.Drawing.Size(247, 24);
            this.PanelTB.ResumeLayout(false);
            this.PanelTB.PerformLayout();
            this.ResumeLayout(false);
            this.RefreshTextboxPosition();
        }

        private void PanelTB_Click(object sender, EventArgs e)
        {
            Textbox.Focus();
        }

        private void Textbox_GotFocus(object sender, EventArgs e)
        {
            this.BorderColor = !ReadOnly ? Color.Black : Color.DimGray;
        }

        private void Textbox_LostFocus(object sender, EventArgs e)
        {
            this.BorderColor = Color.DimGray;
        }

        #endregion Design

        /// <summary>
        /// Konstruktor
        /// </summary>
        public TextboxBL()
        {
            InitializeComponent();
        }

        public new Color BackColor { get => PanelTB.BackColor; set { PanelTB.BackColor = value; Textbox.BackColor = value; } }

        /// <summary>
        /// Gibt an, ob eine Umrandung gezeichnet werden soll.
        /// </summary>
        [Category("Darstellung"), ReadOnly(false), Browsable(true), Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Always)]
        public bool Border { get => base.Padding.All == 1; set { base.Padding = value ? new System.Windows.Forms.Padding(1) : new System.Windows.Forms.Padding(0); RefreshTextboxPosition(); this.Refresh(); } }

        [Browsable(false), ReadOnly(true), Bindable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new System.Windows.Forms.BorderStyle BorderStyle { get; protected set; }

        public override Color ForeColor { get => Textbox.ForeColor; set => Textbox.ForeColor = value; }

        [Browsable(false), ReadOnly(true), Bindable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new System.Windows.Forms.Padding Padding { get; protected set; }

        /// <summary>
        /// Gibt an ob die <see cref="Textbox"/> schreibgeschützt ist oder nicht.
        /// </summary>
        [Category("Text"), ReadOnly(false), Browsable(true), Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Always), Description("Gibt an ob das Steuerelement Schreibgeschützt ist.")]
        public bool ReadOnly { get => Textbox.ReadOnly; set => Textbox.ReadOnly = value; }

        /// <summary>
        /// Der anzuzeigende Text der <see cref="Textbox"/>
        /// </summary>
        [Category("Darstellung"), ReadOnly(false), Browsable(true), Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text { get => Textbox.Text; set => Textbox.Text = value; }

        /// <summary>
        /// Gibt die aktuelle Farbe der Umrandung an
        /// </summary>
        protected Color BorderColor { get => base.BackColor; set => base.BackColor = value; }

        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);
            RefreshTextboxPosition();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            RefreshTextboxPosition();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RefreshTextboxPosition();
        }

        /// <summary>
        /// Berechnet die Position und Größe der <see cref="Textbox"/> neu
        /// </summary>
        protected virtual void RefreshTextboxPosition()
        {
            Textbox.Size = new Size(Textbox.Parent.Width - 5 - 5, Textbox.Height);
            Textbox.Location = new Point(5, (Textbox.Parent.Height - Textbox.Height) / 2);
        }
    }
}