using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseLibrary.GUI.Forms
{
    public partial class BaseFrame : Form
    {
        public Hashtable Parameters = new Hashtable();
        private DialogResult _DialogResult = DialogResult.None;

        public BaseFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Der <see cref="Delegate"/>, der das <see cref="DialogResultChanged"/> aufruft.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="DialogResult"></param>
        public delegate void DialogResultChangedEventHanlder(BaseFrame sender, DialogResult DialogResult);

        /// <summary>
        /// Wird ausgelöst, wenn sich das <see cref="DialogResult"/> ändert.
        /// </summary>
        public event DialogResultChangedEventHanlder DialogResultChanged;

        /// <summary>
        /// Das Ergebnis des Dialogs.
        /// </summary>
        public new DialogResult DialogResult
        {
            get
            {
                return _DialogResult;
            }
            set
            {
                _DialogResult = value;
                if (_DialogResult != DialogResult.None)
                {
                    OnDialogResultChanged(value);
                    this.Close();
                }
            }
        }

        protected BaseFrame BlockingFrame { get; private set; }

        /// <summary>
        /// Blockiert das aktuelle Objekt.
        /// </summary>
        /// <param name="ChildFrame"></param>
        public void BlockWindow(BaseFrame ChildFrame)
        {
            BlockingFrame = ChildFrame;
            this.Enabled = false;
            ChildFrame.FormClosed += ChildFrame_FormClosed;
        }

        /// <summary>
        /// Blockiert das angegebene Objekt und zeigt das aktuelle.
        /// </summary>
        /// <param name="ParentFrame"></param>
        public void ShowBlock(BaseFrame ParentFrame)
        {
            ParentFrame.BlockWindow(this);
            this.Show();
        }

        /// <summary>
        /// Hebt den Block auf.
        /// </summary>
        public void UnblockWindow()
        {
            this.Enabled = true;
        }

        /// <summary>
        /// Aktiviert bzw. deaktiviert die angegebenen Objekte
        /// </summary>
        /// <param name="Enable"></param>
        /// <param name="Controls"></param>
        protected void EnabledControls(bool Enable, params Control[] Controls)
        {
            foreach (Control C in Controls)
            {
                if (C != null)
                {
                    C.Enabled = Enable;
                }
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (BlockingFrame != null && this.Enabled == false)
            {
                BlockingFrame.Activate();
            }
        }

        protected void OnDialogResultChanged(DialogResult DialogResult)
        {
            DialogResultChangedEventHanlder handler = DialogResultChanged;
            handler?.Invoke(this, this.DialogResult);
        }

        /// <summary>
        /// Sorgt dafür, dass das angegebene Objekt nur einmal geöffnet wird und das aktuelle Objekt blockiert.
        /// </summary>
        /// <param name="BaseFrame"></param>
        protected void OpenBlockFormOnce(ref BaseFrame BaseFrame)
        {
            BaseFrame cf = Application.OpenForms[BaseFrame.Name] as BaseFrame;
            if (cf == null)
            {
                if (BaseFrame.IsDisposed)
                {
                    Hashtable oldParams = BaseFrame.Parameters;
                    BaseFrame = Activator.CreateInstance(BaseFrame.GetType()) as BaseFrame;
                    BaseFrame.Parameters = oldParams;
                }
                BaseFrame.Show();
            }
            else
            {
                BaseFrame.BringToFront();
            }
        }

        /// <summary>
        /// Sorgt dafür, dass das angegebene Objekt nur einmal geöffnet wird.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Form"></param>
        protected void OpenFormOnce<T>(ref T Form) where T : BaseFrame, new()
        {
            Form cf = Application.OpenForms[Form.Name];
            if (cf == null)
            {
                if (Form.IsDisposed)
                {
                    Hashtable oldParams = Form.Parameters;
                    Form = new T();
                    Form.Parameters = oldParams;
                }
                Form.Show();
            }
            else
            {
                Form.BringToFront();
            }
        }

        private void ChildFrame_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnblockWindow();
        }
    }
}