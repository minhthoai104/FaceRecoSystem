using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    partial class AttendanceList
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckOut)).BeginInit();
            this.groupInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(20, 14);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(712, 652);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // picCheckIn
            // 
            this.picCheckIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCheckIn.Location = new System.Drawing.Point(738, 14);
            this.picCheckIn.Name = "picCheckIn";
            this.picCheckIn.Size = new System.Drawing.Size(230, 187);
            this.picCheckIn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCheckIn.TabIndex = 1;
            this.picCheckIn.TabStop = false;
            // 
            // picCheckOut
            // 
            this.picCheckOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCheckOut.Location = new System.Drawing.Point(738, 237);
            this.picCheckOut.Name = "picCheckOut";
            this.picCheckOut.Size = new System.Drawing.Size(230, 187);
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
            this.groupInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupInfo.Location = new System.Drawing.Point(738, 450);
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.Size = new System.Drawing.Size(261, 150);
            this.groupInfo.TabIndex = 3;
            this.groupInfo.TabStop = false;
            this.groupInfo.Text = "Thông tin nhân viên";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(20, 30);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(350, 20);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Họ và tên:";
            // 
            // lblAge
            // 
            this.lblAge.Location = new System.Drawing.Point(20, 60);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(350, 20);
            this.lblAge.TabIndex = 1;
            this.lblAge.Text = "Tuổi:";
            // 
            // lblGender
            // 
            this.lblGender.Location = new System.Drawing.Point(20, 90);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(350, 20);
            this.lblGender.TabIndex = 2;
            this.lblGender.Text = "Giới tính:";
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(20, 120);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(350, 20);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Địa chỉ:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(813, 617);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 35);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(809, 202);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Check In";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(809, 427);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Check Out";
            // 
            // AttendanceList
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.picCheckOut);
            this.Controls.Add(this.picCheckIn);
            this.Controls.Add(this.groupInfo);
            this.Name = "AttendanceList";
            this.Size = new System.Drawing.Size(1014, 675);
            this.Load += new System.EventHandler(this.AttendanceList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCheckIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckOut)).EndInit();
            this.groupInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Button btnRefresh;
        private Label label1;
        private Label label2;
    }
}
