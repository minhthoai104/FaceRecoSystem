using System.Drawing;
using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    partial class UpdatePersonControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblAge = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.cbGender = new System.Windows.Forms.ComboBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.picAngle1 = new System.Windows.Forms.PictureBox();
            this.picAngle2 = new System.Windows.Forms.PictureBox();
            this.picAngle3 = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.gbFaceData = new System.Windows.Forms.GroupBox();
            this.pnlThumbnails = new System.Windows.Forms.FlowLayoutPanel();
            this.cardFront = new System.Windows.Forms.FlowLayoutPanel();
            this.cardLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.cardRight = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlDataActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDeleteImages = new System.Windows.Forms.Button();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle3)).BeginInit();
            this.gbInfo.SuspendLayout();
            this.gbFaceData.SuspendLayout();
            this.pnlThumbnails.SuspendLayout();
            this.cardFront.SuspendLayout();
            this.cardLeft.SuspendLayout();
            this.cardRight.SuspendLayout();
            this.pnlDataActions.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
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
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtName.Location = new System.Drawing.Point(150, 45);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(580, 32);
            this.txtName.TabIndex = 6;
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
            // txtAge
            // 
            this.txtAge.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtAge.Location = new System.Drawing.Point(150, 95);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(220, 32);
            this.txtAge.TabIndex = 4;
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
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtAddress.Location = new System.Drawing.Point(150, 145);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(580, 80);
            this.txtAddress.TabIndex = 0;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(487, 18);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(180, 50);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "💾 Cập Nhật";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.Color.ForestGreen;
            this.btnCapture.FlatAppearance.BorderSize = 0;
            this.btnCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapture.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.btnCapture.ForeColor = System.Drawing.Color.White;
            this.btnCapture.Location = new System.Drawing.Point(0, 0);
            this.btnCapture.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(200, 44);
            this.btnCapture.TabIndex = 10;
            this.btnCapture.Text = "📸 Chụp Ảnh";
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // picAngle1
            // 
            this.picAngle1.BackColor = System.Drawing.Color.Gainsboro;
            this.picAngle1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAngle1.Location = new System.Drawing.Point(3, 3);
            this.picAngle1.Name = "picAngle1";
            this.picAngle1.Size = new System.Drawing.Size(220, 200);
            this.picAngle1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAngle1.TabIndex = 11;
            this.picAngle1.TabStop = false;
            // 
            // picAngle2
            // 
            this.picAngle2.BackColor = System.Drawing.Color.Gainsboro;
            this.picAngle2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAngle2.Location = new System.Drawing.Point(3, 3);
            this.picAngle2.Name = "picAngle2";
            this.picAngle2.Size = new System.Drawing.Size(220, 200);
            this.picAngle2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAngle2.TabIndex = 12;
            this.picAngle2.TabStop = false;
            // 
            // picAngle3
            // 
            this.picAngle3.BackColor = System.Drawing.Color.Gainsboro;
            this.picAngle3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAngle3.Location = new System.Drawing.Point(3, 3);
            this.picAngle3.Name = "picAngle3";
            this.picAngle3.Size = new System.Drawing.Size(220, 200);
            this.picAngle3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAngle3.TabIndex = 13;
            this.picAngle3.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(30, 103);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(740, 23);
            this.lblStatus.TabIndex = 17;
            this.lblStatus.Text = " ";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(50, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(800, 61);
            this.lblTitle.TabIndex = 18;
            this.lblTitle.Text = "Cập Nhật Thông Tin Nhân Viên";
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
            this.gbInfo.Location = new System.Drawing.Point(50, 81);
            this.gbInfo.Margin = new System.Windows.Forms.Padding(0, 10, 0, 20);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Padding = new System.Windows.Forms.Padding(30, 10, 30, 10);
            this.gbInfo.Size = new System.Drawing.Size(800, 250);
            this.gbInfo.TabIndex = 19;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "1. Thông Tin Cá Nhân";
            // 
            // gbFaceData
            // 
            this.gbFaceData.Controls.Add(this.pnlThumbnails);
            this.gbFaceData.Controls.Add(this.lblStatus);
            this.gbFaceData.Controls.Add(this.pnlDataActions);
            this.gbFaceData.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFaceData.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gbFaceData.Location = new System.Drawing.Point(50, 331);
            this.gbFaceData.Name = "gbFaceData";
            this.gbFaceData.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.gbFaceData.Size = new System.Drawing.Size(800, 424);
            this.gbFaceData.TabIndex = 20;
            this.gbFaceData.TabStop = false;
            this.gbFaceData.Text = "2. Dữ Liệu Khuôn Mặt";
            // 
            // pnlThumbnails
            // 
            this.pnlThumbnails.Controls.Add(this.cardFront);
            this.pnlThumbnails.Controls.Add(this.cardLeft);
            this.pnlThumbnails.Controls.Add(this.cardRight);
            this.pnlThumbnails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlThumbnails.Location = new System.Drawing.Point(30, 137);
            this.pnlThumbnails.Name = "pnlThumbnails";
            this.pnlThumbnails.Size = new System.Drawing.Size(740, 267);
            this.pnlThumbnails.TabIndex = 18;
            this.pnlThumbnails.WrapContents = false;
            // 
            // cardFront
            // 
            this.cardFront.Controls.Add(this.picAngle1);
            this.cardFront.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.cardFront.Location = new System.Drawing.Point(20, 3);
            this.cardFront.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.cardFront.Name = "cardFront";
            this.cardFront.Size = new System.Drawing.Size(226, 252);
            this.cardFront.TabIndex = 0;
            // 
            // cardLeft
            // 
            this.cardLeft.Controls.Add(this.picAngle2);
            this.cardLeft.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.cardLeft.Location = new System.Drawing.Point(266, 3);
            this.cardLeft.Margin = new System.Windows.Forms.Padding(0, 3, 20, 3);
            this.cardLeft.Name = "cardLeft";
            this.cardLeft.Size = new System.Drawing.Size(226, 252);
            this.cardLeft.TabIndex = 1;
            // 
            // cardRight
            // 
            this.cardRight.Controls.Add(this.picAngle3);
            this.cardRight.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.cardRight.Location = new System.Drawing.Point(512, 3);
            this.cardRight.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.cardRight.Name = "cardRight";
            this.cardRight.Size = new System.Drawing.Size(226, 252);
            this.cardRight.TabIndex = 2;
            // 
            // pnlDataActions
            // 
            this.pnlDataActions.Controls.Add(this.btnCapture);
            this.pnlDataActions.Controls.Add(this.btnDeleteImages);
            this.pnlDataActions.Location = new System.Drawing.Point(30, 47);
            this.pnlDataActions.Name = "pnlDataActions";
            this.pnlDataActions.Size = new System.Drawing.Size(740, 43);
            this.pnlDataActions.TabIndex = 19;
            // 
            // btnDeleteImages
            // 
            this.btnDeleteImages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnDeleteImages.FlatAppearance.BorderSize = 0;
            this.btnDeleteImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteImages.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.btnDeleteImages.ForeColor = System.Drawing.Color.White;
            this.btnDeleteImages.Location = new System.Drawing.Point(220, 0);
            this.btnDeleteImages.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDeleteImages.Name = "btnDeleteImages";
            this.btnDeleteImages.Size = new System.Drawing.Size(200, 44);
            this.btnDeleteImages.TabIndex = 11;
            this.btnDeleteImages.Text = "❌ Xóa Ảnh";
            this.btnDeleteImages.UseVisualStyleBackColor = false;
            this.btnDeleteImages.Click += new System.EventHandler(this.btnDeleteImages_Click);
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnClose);
            this.pnlActions.Controls.Add(this.btnUpdate);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlActions.Location = new System.Drawing.Point(50, 760);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.pnlActions.Size = new System.Drawing.Size(800, 70);
            this.pnlActions.TabIndex = 21;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.btnClose.Location = new System.Drawing.Point(680, 15);
            this.btnClose.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 50);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // UpdatePersonControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.gbFaceData);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.Name = "UpdatePersonControl";
            this.Padding = new System.Windows.Forms.Padding(50, 20, 50, 20);
            this.Size = new System.Drawing.Size(900, 850);
            ((System.ComponentModel.ISupportInitialize)(this.picAngle1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle3)).EndInit();
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.gbFaceData.ResumeLayout(false);
            this.pnlThumbnails.ResumeLayout(false);
            this.cardFront.ResumeLayout(false);
            this.cardLeft.ResumeLayout(false);
            this.cardRight.ResumeLayout(false);
            this.pnlDataActions.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.ComboBox cbGender;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.PictureBox picAngle1;
        private System.Windows.Forms.PictureBox picAngle2;
        private System.Windows.Forms.PictureBox picAngle3;
        // Xóa 3 dòng khai báo nút xóa cũ
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.GroupBox gbFaceData;
        private System.Windows.Forms.FlowLayoutPanel pnlThumbnails;
        private System.Windows.Forms.FlowLayoutPanel cardFront;
        private System.Windows.Forms.FlowLayoutPanel cardLeft;
        private System.Windows.Forms.FlowLayoutPanel cardRight;
        private System.Windows.Forms.FlowLayoutPanel pnlDataActions;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private System.Windows.Forms.Button btnClose;
        // Thêm khai báo nút xóa mới
        private System.Windows.Forms.Button btnDeleteImages;
    }
}