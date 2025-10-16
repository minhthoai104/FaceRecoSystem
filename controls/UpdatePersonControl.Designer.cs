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
            this.btnLoadInfo = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.picAngle1 = new System.Windows.Forms.PictureBox();
            this.picAngle2 = new System.Windows.Forms.PictureBox();
            this.picAngle3 = new System.Windows.Forms.PictureBox();
            this.btnDeleteAngle1 = new System.Windows.Forms.Button();
            this.btnDeleteAngle2 = new System.Windows.Forms.Button();
            this.btnDeleteAngle3 = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle3)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(181, 53);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(96, 25);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Họ và tên:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(296, 50);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(360, 32);
            this.txtName.TabIndex = 1;
            // 
            // lblAge
            // 
            this.lblAge.AutoSize = true;
            this.lblAge.Location = new System.Drawing.Point(181, 107);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(53, 25);
            this.lblAge.TabIndex = 2;
            this.lblAge.Text = "Tuổi:";
            // 
            // txtAge
            // 
            this.txtAge.Location = new System.Drawing.Point(296, 104);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(122, 32);
            this.txtAge.TabIndex = 3;
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(451, 107);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(88, 25);
            this.lblGender.TabIndex = 4;
            this.lblGender.Text = "Giới tính:";
            // 
            // cbGender
            // 
            this.cbGender.Location = new System.Drawing.Point(546, 104);
            this.cbGender.Name = "cbGender";
            this.cbGender.Size = new System.Drawing.Size(110, 33);
            this.cbGender.TabIndex = 5;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(181, 160);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(74, 25);
            this.lblAddress.TabIndex = 6;
            this.lblAddress.Text = "Địa chỉ:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(296, 157);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(360, 70);
            this.txtAddress.TabIndex = 7;
            // 
            // btnLoadInfo
            // 
            this.btnLoadInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnLoadInfo.FlatAppearance.BorderSize = 0;
            this.btnLoadInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadInfo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLoadInfo.ForeColor = System.Drawing.Color.White;
            this.btnLoadInfo.Location = new System.Drawing.Point(202, 251);
            this.btnLoadInfo.Name = "btnLoadInfo";
            this.btnLoadInfo.Size = new System.Drawing.Size(167, 45);
            this.btnLoadInfo.TabIndex = 8;
            this.btnLoadInfo.Text = "📄 Tải thông tin";
            this.btnLoadInfo.UseVisualStyleBackColor = false;
            this.btnLoadInfo.Click += new System.EventHandler(this.btnLoadInfo_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(390, 251);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(140, 45);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "💾 Cập nhật";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnCapture.FlatAppearance.BorderSize = 0;
            this.btnCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapture.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCapture.ForeColor = System.Drawing.Color.White;
            this.btnCapture.Location = new System.Drawing.Point(546, 251);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(146, 45);
            this.btnCapture.TabIndex = 10;
            this.btnCapture.Text = "📸 Chụp ảnh";
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // picAngle1
            // 
            this.picAngle1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAngle1.Location = new System.Drawing.Point(50, 367);
            this.picAngle1.Name = "picAngle1";
            this.picAngle1.Size = new System.Drawing.Size(237, 252);
            this.picAngle1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAngle1.TabIndex = 11;
            this.picAngle1.TabStop = false;
            // 
            // picAngle2
            // 
            this.picAngle2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAngle2.Location = new System.Drawing.Point(324, 367);
            this.picAngle2.Name = "picAngle2";
            this.picAngle2.Size = new System.Drawing.Size(237, 252);
            this.picAngle2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAngle2.TabIndex = 12;
            this.picAngle2.TabStop = false;
            // 
            // picAngle3
            // 
            this.picAngle3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAngle3.Location = new System.Drawing.Point(594, 367);
            this.picAngle3.Name = "picAngle3";
            this.picAngle3.Size = new System.Drawing.Size(237, 252);
            this.picAngle3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAngle3.TabIndex = 13;
            this.picAngle3.TabStop = false;
            // 
            // btnDeleteAngle1
            // 
            this.btnDeleteAngle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnDeleteAngle1.FlatAppearance.BorderSize = 0;
            this.btnDeleteAngle1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAngle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteAngle1.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAngle1.Location = new System.Drawing.Point(78, 625);
            this.btnDeleteAngle1.Name = "btnDeleteAngle1";
            this.btnDeleteAngle1.Size = new System.Drawing.Size(160, 40);
            this.btnDeleteAngle1.TabIndex = 14;
            this.btnDeleteAngle1.Text = "Xóa chính diện";
            this.btnDeleteAngle1.UseVisualStyleBackColor = false;
            this.btnDeleteAngle1.Click += new System.EventHandler(this.btnDeleteAngle1_Click);
            // 
            // btnDeleteAngle2
            // 
            this.btnDeleteAngle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnDeleteAngle2.FlatAppearance.BorderSize = 0;
            this.btnDeleteAngle2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAngle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteAngle2.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAngle2.Location = new System.Drawing.Point(365, 625);
            this.btnDeleteAngle2.Name = "btnDeleteAngle2";
            this.btnDeleteAngle2.Size = new System.Drawing.Size(160, 40);
            this.btnDeleteAngle2.TabIndex = 15;
            this.btnDeleteAngle2.Text = "Xóa góc trái";
            this.btnDeleteAngle2.UseVisualStyleBackColor = false;
            this.btnDeleteAngle2.Click += new System.EventHandler(this.btnDeleteAngle2_Click);
            // 
            // btnDeleteAngle3
            // 
            this.btnDeleteAngle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnDeleteAngle3.FlatAppearance.BorderSize = 0;
            this.btnDeleteAngle3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAngle3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteAngle3.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAngle3.Location = new System.Drawing.Point(636, 625);
            this.btnDeleteAngle3.Name = "btnDeleteAngle3";
            this.btnDeleteAngle3.Size = new System.Drawing.Size(160, 40);
            this.btnDeleteAngle3.TabIndex = 16;
            this.btnDeleteAngle3.Text = "Xóa góc phải";
            this.btnDeleteAngle3.UseVisualStyleBackColor = false;
            this.btnDeleteAngle3.Click += new System.EventHandler(this.btnDeleteAngle3_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(50, 300);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(15, 23);
            this.lblStatus.TabIndex = 17;
            this.lblStatus.Text = " ";
            // 
            // UpdatePersonControl
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblAge);
            this.Controls.Add(this.txtAge);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.cbGender);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.btnLoadInfo);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.picAngle1);
            this.Controls.Add(this.picAngle2);
            this.Controls.Add(this.picAngle3);
            this.Controls.Add(this.btnDeleteAngle1);
            this.Controls.Add(this.btnDeleteAngle2);
            this.Controls.Add(this.btnDeleteAngle3);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.Name = "UpdatePersonControl";
            this.Size = new System.Drawing.Size(898, 720);
            ((System.ComponentModel.ISupportInitialize)(this.picAngle1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngle3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Button btnLoadInfo;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.PictureBox picAngle1;
        private System.Windows.Forms.PictureBox picAngle2;
        private System.Windows.Forms.PictureBox picAngle3;
        private System.Windows.Forms.Button btnDeleteAngle1;
        private System.Windows.Forms.Button btnDeleteAngle2;
        private System.Windows.Forms.Button btnDeleteAngle3;
        private System.Windows.Forms.Label lblStatus;
    }
}
