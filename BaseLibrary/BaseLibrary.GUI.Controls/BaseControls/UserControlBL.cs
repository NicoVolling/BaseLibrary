using System.Windows.Forms;

namespace BaseLibrary.GUI.Controls.BaseControls
{
    /// <inheritdoc/>
    public partial class UserControlBL : UserControl
    {
        public UserControlBL()
        {
            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "UserControlBL";
            this.ResumeLayout(false);
        }
    }
}