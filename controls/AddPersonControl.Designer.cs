using System.Drawing;
using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    partial class AddPersonControl
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnSave;
        private PictureBox picFront;
        private PictureBox picLeft;
        private PictureBox picRight;
        private Label lblFront;
        private Label lblLeft;
        private Label lblRight;
        private Button btnDeleteFront;
        private Button btnDeleteLeft;
        private Button btnDeleteRight;
        private Label lblTitle;
        private TextBox txtName;
        private TextBox txtAge;
        private ComboBox cbGender;
        private TextBox txtAddress;
        private Button btnStartCapture;
        private Label lblName;
        private Label lblAge;
        private Label lblGender;
        private Label lblAddress;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.SuspendLayout();
            this.Name = "AddPersonControl";
            this.Size = new Size(1000, 600);
            this.ResumeLayout(false);
        }
    }
}
