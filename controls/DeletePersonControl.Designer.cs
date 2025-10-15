namespace FaceRecoSystem.controls
{
    partial class DeletePersonControl
    {
        private System.ComponentModel.IContainer components = null;
        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.tabControlDelete = new System.Windows.Forms.TabControl();
            this.tabPageByName = new System.Windows.Forms.TabPage();
            this.btnDeleteByName = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblTitleByName = new System.Windows.Forms.Label();
            this.tabPageByFace = new System.Windows.Forms.TabPage();
            this.lblScanStatus = new System.Windows.Forms.Label();
            this.btnDeleteByFace = new System.Windows.Forms.Button();
            this.btnStartScan = new System.Windows.Forms.Button();
            this.picCamera = new System.Windows.Forms.PictureBox();
            this.lblTitleByFace = new System.Windows.Forms.Label();
            this.tabControlDelete.SuspendLayout();
            this.tabPageByName.SuspendLayout();
            this.tabPageByFace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlDelete
            // 
            this.tabControlDelete.Controls.Add(this.tabPageByName);
            this.tabControlDelete.Controls.Add(this.tabPageByFace);
            this.tabControlDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDelete.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControlDelete.Location = new System.Drawing.Point(0, 0);
            this.tabControlDelete.Name = "tabControlDelete";
            this.tabControlDelete.SelectedIndex = 0;
            this.tabControlDelete.Size = new System.Drawing.Size(600, 450);
            this.tabControlDelete.TabIndex = 0;
            // 
            // tabPageByName
            // 
            this.tabPageByName.BackColor = System.Drawing.Color.White;
            this.tabPageByName.Controls.Add(this.btnDeleteByName);
            this.tabPageByName.Controls.Add(this.txtName);
            this.tabPageByName.Controls.Add(this.lblName);
            this.tabPageByName.Controls.Add(this.lblTitleByName);
            this.tabPageByName.Location = new System.Drawing.Point(4, 32);
            this.tabPageByName.Name = "tabPageByName";
            this.tabPageByName.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByName.Size = new System.Drawing.Size(592, 414);
            this.tabPageByName.TabIndex = 0;
            this.tabPageByName.Text = "Xóa theo tên";
            // 
            // btnDeleteByName
            // 
            this.btnDeleteByName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDeleteByName.FlatAppearance.BorderSize = 0;
            this.btnDeleteByName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteByName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteByName.ForeColor = System.Drawing.Color.White;
            this.btnDeleteByName.Location = new System.Drawing.Point(224, 139);
            this.btnDeleteByName.Name = "btnDeleteByName";
            this.btnDeleteByName.Size = new System.Drawing.Size(120, 40);
            this.btnDeleteByName.TabIndex = 3;
            this.btnDeleteByName.Text = "Xóa";
            this.btnDeleteByName.UseVisualStyleBackColor = false;
            this.btnDeleteByName.Click += new System.EventHandler(this.BtnDeleteByName_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(224, 86);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(280, 30);
            this.txtName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.ForeColor = System.Drawing.Color.Black;
            this.lblName.Location = new System.Drawing.Point(64, 89);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(150, 23);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Nhập tên cần xóa:";
            // 
            // lblTitleByName
            // 
            this.lblTitleByName.AutoSize = true;
            this.lblTitleByName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitleByName.ForeColor = System.Drawing.Color.Black;
            this.lblTitleByName.Location = new System.Drawing.Point(102, 22);
            this.lblTitleByName.Name = "lblTitleByName";
            this.lblTitleByName.Size = new System.Drawing.Size(358, 32);
            this.lblTitleByName.TabIndex = 0;
            this.lblTitleByName.Text = "Xóa người trong cơ sở dữ liệu";
            // 
            // tabPageByFace
            // 
            this.tabPageByFace.BackColor = System.Drawing.Color.White;
            this.tabPageByFace.Controls.Add(this.lblScanStatus);
            this.tabPageByFace.Controls.Add(this.btnDeleteByFace);
            this.tabPageByFace.Controls.Add(this.btnStartScan);
            this.tabPageByFace.Controls.Add(this.picCamera);
            this.tabPageByFace.Controls.Add(this.lblTitleByFace);
            this.tabPageByFace.Location = new System.Drawing.Point(4, 32);
            this.tabPageByFace.Name = "tabPageByFace";
            this.tabPageByFace.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByFace.Size = new System.Drawing.Size(592, 414);
            this.tabPageByFace.TabIndex = 1;
            this.tabPageByFace.Text = "Xóa bằng nhận diện";
            // 
            // lblScanStatus
            // 
            this.lblScanStatus.AutoSize = true;
            this.lblScanStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Italic);
            this.lblScanStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblScanStatus.Location = new System.Drawing.Point(382, 370);
            this.lblScanStatus.Name = "lblScanStatus";
            this.lblScanStatus.Size = new System.Drawing.Size(191, 25);
            this.lblScanStatus.TabIndex = 4;
            this.lblScanStatus.Text = "Trạng thái: Chưa quét";
            // 
            // btnDeleteByFace
            // 
            this.btnDeleteByFace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDeleteByFace.Enabled = false;
            this.btnDeleteByFace.FlatAppearance.BorderSize = 0;
            this.btnDeleteByFace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteByFace.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteByFace.ForeColor = System.Drawing.Color.White;
            this.btnDeleteByFace.Location = new System.Drawing.Point(423, 113);
            this.btnDeleteByFace.Name = "btnDeleteByFace";
            this.btnDeleteByFace.Size = new System.Drawing.Size(150, 40);
            this.btnDeleteByFace.TabIndex = 3;
            this.btnDeleteByFace.Text = "Xóa người này";
            this.btnDeleteByFace.UseVisualStyleBackColor = false;
            this.btnDeleteByFace.Click += new System.EventHandler(this.BtnDeleteByFace_Click);
            // 
            // btnStartScan
            // 
            this.btnStartScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnStartScan.FlatAppearance.BorderSize = 0;
            this.btnStartScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartScan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStartScan.ForeColor = System.Drawing.Color.White;
            this.btnStartScan.Location = new System.Drawing.Point(423, 55);
            this.btnStartScan.Name = "btnStartScan";
            this.btnStartScan.Size = new System.Drawing.Size(150, 40);
            this.btnStartScan.TabIndex = 2;
            this.btnStartScan.Text = "Bắt đầu quét";
            this.btnStartScan.UseVisualStyleBackColor = false;
            this.btnStartScan.Click += new System.EventHandler(this.BtnStartScan_Click);
            // 
            // picCamera
            // 
            this.picCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCamera.Location = new System.Drawing.Point(26, 55);
            this.picCamera.Name = "picCamera";
            this.picCamera.Size = new System.Drawing.Size(361, 298);
            this.picCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCamera.TabIndex = 1;
            this.picCamera.TabStop = false;
            // 
            // lblTitleByFace
            // 
            this.lblTitleByFace.AutoSize = true;
            this.lblTitleByFace.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleByFace.ForeColor = System.Drawing.Color.Black;
            this.lblTitleByFace.Location = new System.Drawing.Point(70, 14);
            this.lblTitleByFace.Name = "lblTitleByFace";
            this.lblTitleByFace.Size = new System.Drawing.Size(435, 28);
            this.lblTitleByFace.TabIndex = 0;
            this.lblTitleByFace.Text = "Xóa bằng cách quét và nhận diện khuôn mặt";
            // 
            // DeletePersonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlDelete);
            this.Name = "DeletePersonControl";
            this.Size = new System.Drawing.Size(600, 450);
            this.tabControlDelete.ResumeLayout(false);
            this.tabPageByName.ResumeLayout(false);
            this.tabPageByName.PerformLayout();
            this.tabPageByFace.ResumeLayout(false);
            this.tabPageByFace.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlDelete;
        private System.Windows.Forms.TabPage tabPageByName;
        private System.Windows.Forms.TabPage tabPageByFace;
        private System.Windows.Forms.Label lblTitleByName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnDeleteByName;
        private System.Windows.Forms.Label lblTitleByFace;
        private System.Windows.Forms.PictureBox picCamera;
        private System.Windows.Forms.Button btnStartScan;
        private System.Windows.Forms.Button btnDeleteByFace;
        private System.Windows.Forms.Label lblScanStatus;
        // Các controls đã được đổi tên và thêm mới
        private System.Windows.Forms.Label lblTitle;

    }
}