
namespace BaseLibrary.GUI.Controls.BaseControls
{
    partial class LoadingScreenLoadingBar
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadingScreenBar1 = new LoadingScreenBarItem();
            this.SuspendLayout();
            // 
            // loadingScreenBar1
            // 
            this.loadingScreenBar1.Location = new System.Drawing.Point(2, 3);
            this.loadingScreenBar1.Name = "loadingScreenBar1";
            this.loadingScreenBar1.Size = new System.Drawing.Size(43, 20);
            this.loadingScreenBar1.TabIndex = 1;
            this.loadingScreenBar1.Text = "loadingScreenBar1";
            // 
            // LoadingScreenLoadingBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loadingScreenBar1);
            this.Name = "LoadingScreenLoadingBar";
            this.Size = new System.Drawing.Size(584, 26);
            this.ResumeLayout(false);

        }

        #endregion

        private LoadingScreenBarItem loadingScreenBar1;
    }
}
