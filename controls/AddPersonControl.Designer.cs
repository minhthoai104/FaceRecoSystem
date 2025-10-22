using System;
using System.Drawing;
using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    partial class AddPersonControl
    {
        // Dán đoạn code này vào AddPersonControl.Designer.cs

        private System.ComponentModel.IContainer components = null;

        // Vùng thông tin cá nhân
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.ComboBox cbGender;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;

        // Vùng dữ liệu khuôn mặt
        private System.Windows.Forms.GroupBox gbFaceData;
        private System.Windows.Forms.Button btnStartCapture;
        private System.Windows.Forms.FlowLayoutPanel pnlThumbnails;
        private System.Windows.Forms.PictureBox picThumbnailFront;
        private System.Windows.Forms.Label lblThumbFront;
        private System.Windows.Forms.PictureBox picThumbnailLeft;
        private System.Windows.Forms.Label lblThumbLeft;
        private System.Windows.Forms.PictureBox picThumbnailRight;
        private System.Windows.Forms.Label lblThumbRight;
        private System.Windows.Forms.Label lblTitle;

        // Nút hành động chính
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDeleteImages;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        // Thay thế toàn bộ hàm InitializeComponent trong file AddPersonControl.Designer.cs

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.cbGender = new System.Windows.Forms.ComboBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.lblAge = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.gbFaceData = new System.Windows.Forms.GroupBox();
            this.pnlThumbnails = new System.Windows.Forms.FlowLayoutPanel();
            this.cardFront = new System.Windows.Forms.FlowLayoutPanel();
            this.picThumbnailFront = new System.Windows.Forms.PictureBox();
            this.lblThumbFront = new System.Windows.Forms.Label();
            this.cardLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.picThumbnailLeft = new System.Windows.Forms.PictureBox();
            this.lblThumbLeft = new System.Windows.Forms.Label();
            this.cardRight = new System.Windows.Forms.FlowLayoutPanel();
            this.picThumbnailRight = new System.Windows.Forms.PictureBox();
            this.lblThumbRight = new System.Windows.Forms.Label();
            this.pnlCaptureButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStartCapture = new System.Windows.Forms.Button();
            this.btnDeleteImages = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this.gbInfo.SuspendLayout();
            this.gbFaceData.SuspendLayout();
            this.pnlThumbnails.SuspendLayout();
            this.cardFront.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picThumbnailFront)).BeginInit();
            this.cardLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picThumbnailLeft)).BeginInit();
            this.cardRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picThumbnailRight)).BeginInit();
            this.pnlCaptureButtons.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.lblTitle.Location = new System.Drawing.Point(50, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(800, 46);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Đăng Ký Nhân Viên Mới";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.txtAddress);
            this.gbInfo.Controls.Add(this.lblAddress);
            this.gbInfo.Controls.Add(this.cbGender);
            this.gbInfo.Controls.Add(this.lblGender);
            this.gbInfo.Controls.Add(this.txtAge);
            this.gbInfo.Controls.Add(this.lblAge);
            this.gbInfo.Controls.Add(this.txtName);
            this.gbInfo.Controls.Add(this.lblName);
            this.gbInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gbInfo.Location = new System.Drawing.Point(50, 66);
            this.gbInfo.Margin = new System.Windows.Forms.Padding(0, 10, 0, 20);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Padding = new System.Windows.Forms.Padding(30, 10, 30, 10);
            this.gbInfo.Size = new System.Drawing.Size(800, 250);
            this.gbInfo.TabIndex = 2;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "1. Thông Tin Cá Nhân";
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtAddress.Location = new System.Drawing.Point(150, 145);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(580, 80);
            this.txtAddress.TabIndex = 0;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblAddress.Location = new System.Drawing.Point(40, 150);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(74, 25);
            this.lblAddress.TabIndex = 1;
            this.lblAddress.Text = "Địa chỉ:";
            // 
            // cbGender
            // 
            this.cbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGender.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cbGender.Items.AddRange(new object[] {
            "Nam",
            "Nữ",
            "Khác"});
            this.cbGender.Location = new System.Drawing.Point(510, 95);
            this.cbGender.Name = "cbGender";
            this.cbGender.Size = new System.Drawing.Size(220, 33);
            this.cbGender.TabIndex = 2;
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblGender.Location = new System.Drawing.Point(396, 100);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(88, 25);
            this.lblGender.TabIndex = 3;
            this.lblGender.Text = "Giới tính:";
            // 
            // txtAge
            // 
            this.txtAge.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtAge.Location = new System.Drawing.Point(150, 95);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(220, 32);
            this.txtAge.TabIndex = 4;
            // 
            // lblAge
            // 
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblAge.Location = new System.Drawing.Point(40, 100);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(53, 25);
            this.lblAge.TabIndex = 5;
            this.lblAge.Text = "Tuổi:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtName.Location = new System.Drawing.Point(150, 45);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(580, 32);
            this.txtName.TabIndex = 6;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblName.Location = new System.Drawing.Point(40, 50);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(98, 25);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "Họ và Tên:";
            // 
            // gbFaceData
            // 
            this.gbFaceData.Controls.Add(this.pnlThumbnails);
            this.gbFaceData.Controls.Add(this.pnlCaptureButtons);
            this.gbFaceData.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFaceData.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gbFaceData.Location = new System.Drawing.Point(50, 316);
            this.gbFaceData.Name = "gbFaceData";
            this.gbFaceData.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.gbFaceData.Size = new System.Drawing.Size(800, 406);
            this.gbFaceData.TabIndex = 1;
            this.gbFaceData.TabStop = false;
            this.gbFaceData.Text = "2. Dữ Liệu Khuôn Mặt";
            // 
            // pnlThumbnails
            // 
            this.pnlThumbnails.Controls.Add(this.cardFront);
            this.pnlThumbnails.Controls.Add(this.cardLeft);
            this.pnlThumbnails.Controls.Add(this.cardRight);
            this.pnlThumbnails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThumbnails.Location = new System.Drawing.Point(30, 93);
            this.pnlThumbnails.Name = "pnlThumbnails";
            this.pnlThumbnails.Padding = new System.Windows.Forms.Padding(30, 15, 0, 0);
            this.pnlThumbnails.Size = new System.Drawing.Size(740, 293);
            this.pnlThumbnails.TabIndex = 0;
            // 
            // cardFront
            // 
            this.cardFront.Controls.Add(this.picThumbnailFront);
            this.cardFront.Controls.Add(this.lblThumbFront);
            this.cardFront.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.cardFront.Location = new System.Drawing.Point(40, 25);
            this.cardFront.Margin = new System.Windows.Forms.Padding(10);
            this.cardFront.Name = "cardFront";
            this.cardFront.Size = new System.Drawing.Size(220, 250);
            this.cardFront.TabIndex = 0;
            // 
            // picThumbnailFront
            // 
            this.picThumbnailFront.BackColor = System.Drawing.Color.Gainsboro;
            this.picThumbnailFront.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picThumbnailFront.Location = new System.Drawing.Point(3, 3);
            this.picThumbnailFront.Name = "picThumbnailFront";
            this.picThumbnailFront.Size = new System.Drawing.Size(210, 200);
            this.picThumbnailFront.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picThumbnailFront.TabIndex = 0;
            this.picThumbnailFront.TabStop = false;
            // 
            // lblThumbFront
            // 
            this.lblThumbFront.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblThumbFront.Location = new System.Drawing.Point(3, 206);
            this.lblThumbFront.Name = "lblThumbFront";
            this.lblThumbFront.Size = new System.Drawing.Size(210, 40);
            this.lblThumbFront.TabIndex = 1;
            this.lblThumbFront.Text = "Chính diện";
            this.lblThumbFront.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardLeft
            // 
            this.cardLeft.Controls.Add(this.picThumbnailLeft);
            this.cardLeft.Controls.Add(this.lblThumbLeft);
            this.cardLeft.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.cardLeft.Location = new System.Drawing.Point(280, 25);
            this.cardLeft.Margin = new System.Windows.Forms.Padding(10);
            this.cardLeft.Name = "cardLeft";
            this.cardLeft.Size = new System.Drawing.Size(220, 250);
            this.cardLeft.TabIndex = 1;
            // 
            // picThumbnailLeft
            // 
            this.picThumbnailLeft.BackColor = System.Drawing.Color.Gainsboro;
            this.picThumbnailLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picThumbnailLeft.Location = new System.Drawing.Point(3, 3);
            this.picThumbnailLeft.Name = "picThumbnailLeft";
            this.picThumbnailLeft.Size = new System.Drawing.Size(210, 200);
            this.picThumbnailLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picThumbnailLeft.TabIndex = 0;
            this.picThumbnailLeft.TabStop = false;
            // 
            // lblThumbLeft
            // 
            this.lblThumbLeft.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblThumbLeft.Location = new System.Drawing.Point(3, 206);
            this.lblThumbLeft.Name = "lblThumbLeft";
            this.lblThumbLeft.Size = new System.Drawing.Size(210, 40);
            this.lblThumbLeft.TabIndex = 1;
            this.lblThumbLeft.Text = "Góc trái";
            this.lblThumbLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardRight
            // 
            this.cardRight.Controls.Add(this.picThumbnailRight);
            this.cardRight.Controls.Add(this.lblThumbRight);
            this.cardRight.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.cardRight.Location = new System.Drawing.Point(40, 295);
            this.cardRight.Margin = new System.Windows.Forms.Padding(10);
            this.cardRight.Name = "cardRight";
            this.cardRight.Size = new System.Drawing.Size(220, 250);
            this.cardRight.TabIndex = 2;
            // 
            // picThumbnailRight
            // 
            this.picThumbnailRight.BackColor = System.Drawing.Color.Gainsboro;
            this.picThumbnailRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picThumbnailRight.Location = new System.Drawing.Point(3, 3);
            this.picThumbnailRight.Name = "picThumbnailRight";
            this.picThumbnailRight.Size = new System.Drawing.Size(210, 200);
            this.picThumbnailRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picThumbnailRight.TabIndex = 0;
            this.picThumbnailRight.TabStop = false;
            // 
            // lblThumbRight
            // 
            this.lblThumbRight.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblThumbRight.Location = new System.Drawing.Point(3, 206);
            this.lblThumbRight.Name = "lblThumbRight";
            this.lblThumbRight.Size = new System.Drawing.Size(210, 40);
            this.lblThumbRight.TabIndex = 1;
            this.lblThumbRight.Text = "Góc phải";
            this.lblThumbRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCaptureButtons
            // 
            this.pnlCaptureButtons.Controls.Add(this.btnStartCapture);
            this.pnlCaptureButtons.Controls.Add(this.btnDeleteImages);
            this.pnlCaptureButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCaptureButtons.Location = new System.Drawing.Point(30, 47);
            this.pnlCaptureButtons.Name = "pnlCaptureButtons";
            this.pnlCaptureButtons.Size = new System.Drawing.Size(740, 46);
            this.pnlCaptureButtons.TabIndex = 1;
            // 
            // btnStartCapture
            // 
            this.btnStartCapture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnStartCapture.FlatAppearance.BorderSize = 0;
            this.btnStartCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartCapture.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.btnStartCapture.ForeColor = System.Drawing.Color.White;
            this.btnStartCapture.Location = new System.Drawing.Point(0, 0);
            this.btnStartCapture.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnStartCapture.Name = "btnStartCapture";
            this.btnStartCapture.Size = new System.Drawing.Size(250, 44);
            this.btnStartCapture.TabIndex = 0;
            this.btnStartCapture.Text = "📸 Bắt Đầu Chụp Ảnh";
            this.btnStartCapture.UseVisualStyleBackColor = false;
            // 
            // btnDeleteImages
            // 
            this.btnDeleteImages.BackColor = System.Drawing.Color.Crimson;
            this.btnDeleteImages.FlatAppearance.BorderSize = 0;
            this.btnDeleteImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteImages.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.btnDeleteImages.ForeColor = System.Drawing.Color.White;
            this.btnDeleteImages.Location = new System.Drawing.Point(263, 3);
            this.btnDeleteImages.Name = "btnDeleteImages";
            this.btnDeleteImages.Size = new System.Drawing.Size(180, 41);
            this.btnDeleteImages.TabIndex = 1;
            this.btnDeleteImages.Text = "❌ Xóa Ảnh";
            this.btnDeleteImages.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(427, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(220, 55);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Lưu Thông Tin";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.btnCancel.Location = new System.Drawing.Point(660, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(140, 55);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnCancel);
            this.pnlActions.Controls.Add(this.btnSave);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlActions.Location = new System.Drawing.Point(50, 760);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.pnlActions.Size = new System.Drawing.Size(800, 70);
            this.pnlActions.TabIndex = 0;
            // 
            // AddPersonControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.gbFaceData);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.lblTitle);
            this.Name = "AddPersonControl";
            this.Padding = new System.Windows.Forms.Padding(50, 20, 50, 20);
            this.Size = new System.Drawing.Size(900, 850);
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.gbFaceData.ResumeLayout(false);
            this.pnlThumbnails.ResumeLayout(false);
            this.cardFront.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picThumbnailFront)).EndInit();
            this.cardLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picThumbnailLeft)).EndInit();
            this.cardRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picThumbnailRight)).EndInit();
            this.pnlCaptureButtons.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private FlowLayoutPanel cardFront;
        private FlowLayoutPanel cardLeft;
        private FlowLayoutPanel cardRight;
        private FlowLayoutPanel pnlCaptureButtons;
        private FlowLayoutPanel pnlActions;
    }
}
