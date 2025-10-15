using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    partial class PersonListControl
    {
        private System.ComponentModel.IContainer components = null;
        private ListView listView1;
        private PictureBox picCheckIn;
        private PictureBox picCheckOut;
        private Label lblName;
        private Label lblAge;
        private Label lblGender;
        private Label lblAddress;
        private GroupBox groupInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.picCheckIn = new System.Windows.Forms.PictureBox();
            this.picCheckOut = new System.Windows.Forms.PictureBox();
            this.groupInfo = new System.Windows.Forms.GroupBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblAge = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckOut)).BeginInit();
            this.groupInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(20, 20);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(695, 477);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // picCheckIn
            // 
            this.picCheckIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCheckIn.Location = new System.Drawing.Point(771, 20);
            this.picCheckIn.Name = "picCheckIn";
            this.picCheckIn.Size = new System.Drawing.Size(200, 200);
            this.picCheckIn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCheckIn.TabIndex = 1;
            this.picCheckIn.TabStop = false;
            // 
            // picCheckOut
            // 
            this.picCheckOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCheckOut.Location = new System.Drawing.Point(991, 20);
            this.picCheckOut.Name = "picCheckOut";
            this.picCheckOut.Size = new System.Drawing.Size(200, 200);
            this.picCheckOut.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCheckOut.TabIndex = 2;
            this.picCheckOut.TabStop = false;
            // 
            // groupInfo
            // 
            this.groupInfo.Controls.Add(this.lblName);
            this.groupInfo.Controls.Add(this.lblAge);
            this.groupInfo.Controls.Add(this.lblGender);
            this.groupInfo.Controls.Add(this.lblAddress);
            this.groupInfo.Location = new System.Drawing.Point(771, 270);
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.Size = new System.Drawing.Size(420, 150);
            this.groupInfo.TabIndex = 3;
            this.groupInfo.TabStop = false;
            this.groupInfo.Text = "Person Info";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(20, 30);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(350, 20);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // lblAge
            // 
            this.lblAge.Location = new System.Drawing.Point(20, 60);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(350, 20);
            this.lblAge.TabIndex = 1;
            this.lblAge.Text = "Age:";
            // 
            // lblGender
            // 
            this.lblGender.Location = new System.Drawing.Point(20, 90);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(350, 20);
            this.lblGender.TabIndex = 2;
            this.lblGender.Text = "Gender:";
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(20, 120);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(350, 20);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Address:";
            // 
            // PersonListControl
            // 
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.picCheckIn);
            this.Controls.Add(this.picCheckOut);
            this.Controls.Add(this.groupInfo);
            this.Name = "PersonListControl";
            this.Size = new System.Drawing.Size(1234, 525);
            this.Load += new System.EventHandler(this.PersonListControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCheckIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckOut)).EndInit();
            this.groupInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
