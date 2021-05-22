using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaseLibrary.GUI.Controls.BaseControls
{
    /// <summary>
    /// Eine <see cref="TextboxBL"/>, die als Eingabefeld für Farben verwendet werden kann.
    /// </summary>
    /// <inheritdoc/>
    public class TextboxColorBL : TextboxBL
    {
        private Color _Color;

        private Panel ColorPanel = new Panel()
        {
            Dock = DockStyle.Left,
            BackColor = Color.Black,
            Size = new Size(5, 0)
        };

        /// <summary>
        /// Konstruktor
        /// </summary>
        public TextboxColorBL() : base()
        {
            Textbox.KeyDown += Textbox_KeyDown;
            Textbox.TextChanged += Textbox_TextChanged;
            Textbox.Leave += Textbox_Leave;
            Textbox.AutoCompleteCustomSource.AddRange(Enum.GetNames(typeof(KnownColor)));
            Textbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            Textbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Controls.Add(this.ColorPanel);
            this.SelectedColor = Color.Black.ToKnownColor();
            this.ColorPanel.BringToFront();
        }

        /// <summary>
        /// Die Eingabe als Farbe.
        /// </summary>
        [Category("Eingabe"), ReadOnly(false), Browsable(true), Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Always)]
        public KnownColor SelectedColor
        {
            get => _Color1.ToKnownColor();
            set
            {
                this._Color1 = System.Drawing.Color.FromKnownColor(value);
                this.Text = value.ToString();
            }
        }

        [DefaultValue("Black"), ReadOnly(true)]
        public override string Text { get => base.Text; set => base.Text = value; }

        private Color _Color1
        {
            get => _Color;
            set
            {
                this._Color = value;
                this.ColorPanel.BackColor = value;
            }
        }

        protected override void RefreshTextboxPosition()
        {
            Textbox.Size = new Size(Textbox.Parent.Width - 10 - 5, Textbox.Height);
            Textbox.Location = new Point(10, (Textbox.Parent.Height - Textbox.Height) / 2);
        }

        private Color ColorByText(string Text)
        {
            Color c = Color.Empty;
            if (!string.IsNullOrWhiteSpace(Text))
            {
                if (System.Drawing.Color.FromName(Text) == System.Drawing.Color.Empty)
                {
                    string[] str = Text.Split(';');
                    if (str.Length == 3)
                    {
                        int R = 0;
                        if (!int.TryParse(str[0], out R))
                        {
                            return c;
                        }
                        int G = 0;
                        if (!int.TryParse(str[0], out G))
                        {
                            return c;
                        }
                        int B = 0;
                        if (!int.TryParse(str[0], out B))
                        {
                            return c;
                        }
                        c = Color.FromArgb(R, G, B);
                    }
                }
                else
                {
                    return Color.FromName(Text);
                }
            }
            return c;
        }

        private void Textbox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Color C = ColorByText(Text);
            if (C.ToArgb() == Color.Empty.ToArgb()) { this._Color = Color.Black; }
            else
            {
                this._Color = C;
            }
        }

        private void Textbox_Leave(object sender, EventArgs e)
        {
            Color C = ColorByText(Text);
            if (C.ToArgb() == Color.Empty.ToArgb()) { this.Text = "Black"; }
        }

        private void Textbox_TextChanged(object sender, EventArgs e)
        {
            Color C = ColorByText(Text);
            if (C.ToArgb() == Color.Empty.ToArgb()) { this._Color = Color.Black; }
            else
            {
                this._Color = C;
            }
        }
    }
}