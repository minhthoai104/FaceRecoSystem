namespace FaceRecoSystem.controls
{
    partial class UpdatePersonControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelInfo = new System.Windows.Forms.Panel();
            this.rbCaptureYes = new System.Windows.Forms.RadioButton();
            this.lblName = new System.Windows.Forms.Label();
            this.rbCaptureNo = new System.Windows.Forms.RadioButton();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblAge = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.cbGender = new System.Windows.Forms.ComboBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.btnLoadInfo = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.panelFace = new System.Windows.Forms.Panel();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.picAngle1 = new System.Windows.Forms.PictureBox();
            this.picAngle2 = new System.Windows.Forms.PictureBox();
            this.picAngle3 = new System.Windows.Forms.PictureBox();
            this.btnDeleteAngle1 = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnDeleteAngle2 = new System.Windows.Forms.Button();
            this.btnDeleteAngle3 = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelInfo.SuspendLayout();
            this.panelFace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle3)).BeginInit();
            this.SuspendLayout();
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.White;
            this.panelInfo.Controls.Add(this.rbCaptureYes);
            this.panelInfo.Controls.Add(this.lblName);
            this.panelInfo.Controls.Add(this.rbCaptureNo);
            this.panelInfo.Controls.Add(this.txtName);
            this.panelInfo.Controls.Add(this.lblAge);
            this.panelInfo.Controls.Add(this.txtAge);
            this.panelInfo.Controls.Add(this.lblGender);
            this.panelInfo.Controls.Add(this.cbGender);
            this.panelInfo.Controls.Add(this.lblAddress);
            this.panelInfo.Controls.Add(this.txtAddress);
            this.panelInfo.Controls.Add(this.btnLoadInfo);
            this.panelInfo.Controls.Add(this.btnUpdate);
            this.panelInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelInfo.Location = new System.Drawing.Point(20, 20);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(517, 692);
            this.panelInfo.TabIndex = 0;
            // 
            // rbCaptureYes
            // 
            this.rbCaptureYes.AutoSize = true;
            this.rbCaptureYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCaptureYes.Location = new System.Drawing.Point(90, 344);
            this.rbCaptureYes.Name = "rbCaptureYes";
            this.rbCaptureYes.Size = new System.Drawing.Size(155, 24);
            this.rbCaptureYes.TabIndex = 3;
            this.rbCaptureYes.Text = "📷 Dùng Camera";
            this.rbCaptureYes.CheckedChanged += new System.EventHandler(this.rbCaptureYes_CheckedChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(51, 42);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(86, 20);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Họ và tên:";
            // 
            // rbCaptureNo
            // 
            this.rbCaptureNo.AutoSize = true;
            this.rbCaptureNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCaptureNo.Location = new System.Drawing.Point(270, 344);
            this.rbCaptureNo.Name = "rbCaptureNo";
            this.rbCaptureNo.Size = new System.Drawing.Size(139, 24);
            this.rbCaptureNo.TabIndex = 4;
            this.rbCaptureNo.Text = "🖼️ Không dùng";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(160, 42);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(303, 27);
            this.txtName.TabIndex = 1;
            // 
            // lblAge
            // 
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAge.Location = new System.Drawing.Point(51, 87);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(46, 20);
            this.lblAge.TabIndex = 2;
            this.lblAge.Text = "Tuổi:";
            // 
            // txtAge
            // 
            this.txtAge.Location = new System.Drawing.Point(160, 84);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(120, 27);
            this.txtAge.TabIndex = 3;
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.Location = new System.Drawing.Point(51, 132);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(76, 20);
            this.lblGender.TabIndex = 4;
            this.lblGender.Text = "Giới tính:";
            // 
            // cbGender
            // 
            this.cbGender.Items.AddRange(new object[] {
            "Nam",
            "Nữ",
            "Khác"});
            this.cbGender.Location = new System.Drawing.Point(160, 129);
            this.cbGender.Name = "cbGender";
            this.cbGender.Size = new System.Drawing.Size(120, 28);
            this.cbGender.TabIndex = 5;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.Location = new System.Drawing.Point(51, 177);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(66, 20);
            this.lblAddress.TabIndex = 6;
            this.lblAddress.Text = "Địa chỉ:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(160, 174);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(303, 70);
            this.txtAddress.TabIndex = 7;
            // 
            // btnLoadInfo
            // 
            this.btnLoadInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadInfo.Location = new System.Drawing.Point(82, 277);
            this.btnLoadInfo.Name = "btnLoadInfo";
            this.btnLoadInfo.Size = new System.Drawing.Size(151, 40);
            this.btnLoadInfo.TabIndex = 8;
            this.btnLoadInfo.Text = "🔄 Tải thông tin";
            this.btnLoadInfo.Click += new System.EventHandler(this.btnLoadInfo_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(252, 277);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(145, 40);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "💾 Cập nhật";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // panelFace
            // 
            this.panelFace.BackColor = System.Drawing.Color.White;
            this.panelFace.Controls.Add(this.picPreview);
            this.panelFace.Controls.Add(this.picAngle1);
            this.panelFace.Controls.Add(this.picAngle2);
            this.panelFace.Controls.Add(this.picAngle3);
            this.panelFace.Controls.Add(this.btnDeleteAngle1);
            this.panelFace.Controls.Add(this.btnCapture);
            this.panelFace.Controls.Add(this.btnDeleteAngle2);
            this.panelFace.Controls.Add(this.btnDeleteAngle3);
            this.panelFace.Controls.Add(this.lblStatus);
            this.panelFace.Location = new System.Drawing.Point(556, 20);
            this.panelFace.Name = "panelFace";
            this.panelFace.Size = new System.Drawing.Size(628, 692);
            this.panelFace.TabIndex = 1;
            // 
            // picPreview
            // 
            this.picPreview.BackColor = System.Drawing.Color.White;
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPreview.Location = new System.Drawing.Point(17, 21);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(588, 405);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 5;
            this.picPreview.TabStop = false;
            // 
            // picAngle1
            // 
            this.picAngle1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAngle1.Location = new System.Drawing.Point(17, 504);
            this.picAngle1.Name = "picAngle1";
            this.picAngle1.Size = new System.Drawing.Size(178, 125);
            this.picAngle1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAngle1.TabIndex = 0;
            this.picAngle1.TabStop = false;
            // 
            // picAngle2
            // 
            this.picAngle2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAngle2.Location = new System.Drawing.Point(220, 504);
            this.picAngle2.Name = "picAngle2";
            this.picAngle2.Size = new System.Drawing.Size(178, 125);
            this.picAngle2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAngle2.TabIndex = 1;
            this.picAngle2.TabStop = false;
            // 
            // picAngle3
            // 
            this.picAngle3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAngle3.Location = new System.Drawing.Point(427, 504);
            this.picAngle3.Name = "picAngle3";
            this.picAngle3.Size = new System.Drawing.Size(178, 125);
            this.picAngle3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAngle3.TabIndex = 2;
            this.picAngle3.TabStop = false;
            // 
            // btnDeleteAngle1
            // 
            this.btnDeleteAngle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAngle1.Location = new System.Drawing.Point(17, 634);
            this.btnDeleteAngle1.Name = "btnDeleteAngle1";
            this.btnDeleteAngle1.Size = new System.Drawing.Size(178, 35);
            this.btnDeleteAngle1.TabIndex = 7;
            this.btnDeleteAngle1.Text = "Xóa chính diện";
            this.btnDeleteAngle1.Click += new System.EventHandler(this.btnDeleteAngle1_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCapture.Location = new System.Drawing.Point(17, 442);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(200, 45);
            this.btnCapture.TabIndex = 6;
            this.btnCapture.Text = "📸 Chụp ảnh";
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnDeleteAngle2
            // 
            this.btnDeleteAngle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAngle2.Location = new System.Drawing.Point(220, 634);
            this.btnDeleteAngle2.Name = "btnDeleteAngle2";
            this.btnDeleteAngle2.Size = new System.Drawing.Size(178, 35);
            this.btnDeleteAngle2.TabIndex = 8;
            this.btnDeleteAngle2.Text = "Xóa góc trái";
            this.btnDeleteAngle2.Click += new System.EventHandler(this.btnDeleteAngle2_Click);
            // 
            // btnDeleteAngle3
            // 
            this.btnDeleteAngle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAngle3.Location = new System.Drawing.Point(427, 634);
            this.btnDeleteAngle3.Name = "btnDeleteAngle3";
            this.btnDeleteAngle3.Size = new System.Drawing.Size(178, 35);
            this.btnDeleteAngle3.TabIndex = 9;
            this.btnDeleteAngle3.Text = "Xóa góc phải";
            this.btnDeleteAngle3.Click += new System.EventHandler(this.btnDeleteAngle3_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(220, 449);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(385, 30);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Chưa chụp ảnh";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpdatePersonControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panelFace);
            this.Name = "UpdatePersonControl";
            this.Size = new System.Drawing.Size(1221, 727);
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelFace.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panelFace;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.ComboBox cbGender;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Button btnLoadInfo;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.RadioButton rbCaptureYes;
        private System.Windows.Forms.RadioButton rbCaptureNo;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.PictureBox picAngle1;
        private System.Windows.Forms.PictureBox picAngle2;
        private System.Windows.Forms.PictureBox picAngle3;
        private System.Windows.Forms.Button btnDeleteAngle1;
        private System.Windows.Forms.Button btnDeleteAngle2;
        private System.Windows.Forms.Button btnDeleteAngle3;
    }
}

