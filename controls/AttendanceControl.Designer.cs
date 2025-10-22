// >> DÁN TOÀN BỘ CODE NÀY VÀO FILE: AttendanceControl.Designer.cs

namespace FaceRecoSystem.controls
{
    partial class AttendanceControl
    {
        private System.ComponentModel.IContainer components = null;
        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCameraArea = new System.Windows.Forms.Panel();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.picCamera = new System.Windows.Forms.PictureBox();
            this.pnlInfoArea = new System.Windows.Forms.Panel();
            this.lsvRecentActivity = new System.Windows.Forms.ListView();
            this.lblRecentActivityTitle = new System.Windows.Forms.Label();
            this.btnRegister = new System.Windows.Forms.Button();
            this.pnlStatusResult = new System.Windows.Forms.Panel();
            this.lblStatusResult = new System.Windows.Forms.Label();
            this.infoTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.panelInfoText = new System.Windows.Forms.Panel();
            this.lblTimestamp = new System.Windows.Forms.Label();
            this.lblEmployeeID = new System.Windows.Forms.Label();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblInfoTitle = new System.Windows.Forms.Label();
            this.separatorLine = new System.Windows.Forms.Label();
            this.lblClockDate = new System.Windows.Forms.Label();
            this.lblClockTime = new System.Windows.Forms.Label();
            this.mainTableLayoutPanel.SuspendLayout();
            this.pnlCameraArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).BeginInit();
            this.pnlInfoArea.SuspendLayout();
            this.pnlStatusResult.SuspendLayout();
            this.infoTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.panelInfoText.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 2;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.mainTableLayoutPanel.Controls.Add(this.pnlCameraArea, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.pnlInfoArea, 1, 0);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 1;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(1280, 720);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // pnlCameraArea
            // 
            this.pnlCameraArea.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlCameraArea.Controls.Add(this.lblInstruction);
            this.pnlCameraArea.Controls.Add(this.picCamera);
            this.pnlCameraArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCameraArea.Location = new System.Drawing.Point(10, 10);
            this.pnlCameraArea.Margin = new System.Windows.Forms.Padding(10);
            this.pnlCameraArea.Name = "pnlCameraArea";
            this.pnlCameraArea.Size = new System.Drawing.Size(876, 700);
            this.pnlCameraArea.TabIndex = 0;
            // 
            // lblInstruction
            // 
            this.lblInstruction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblInstruction.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstruction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblInstruction.Location = new System.Drawing.Point(0, 640);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(876, 60);
            this.lblInstruction.TabIndex = 1;
            this.lblInstruction.Text = "Vui lòng nhìn thẳng vào camera";
            this.lblInstruction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picCamera
            // 
            this.picCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picCamera.BackColor = System.Drawing.Color.Gainsboro;
            this.picCamera.Location = new System.Drawing.Point(20, 20);
            this.picCamera.Margin = new System.Windows.Forms.Padding(20);
            this.picCamera.Name = "picCamera";
            this.picCamera.Size = new System.Drawing.Size(836, 600);
            this.picCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCamera.TabIndex = 0;
            this.picCamera.TabStop = false;
            // 
            // pnlInfoArea
            // 
            this.pnlInfoArea.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlInfoArea.Controls.Add(this.lsvRecentActivity);
            this.pnlInfoArea.Controls.Add(this.lblRecentActivityTitle);
            this.pnlInfoArea.Controls.Add(this.btnRegister);
            this.pnlInfoArea.Controls.Add(this.pnlStatusResult);
            this.pnlInfoArea.Controls.Add(this.infoTableLayoutPanel);
            this.pnlInfoArea.Controls.Add(this.lblInfoTitle);
            this.pnlInfoArea.Controls.Add(this.separatorLine);
            this.pnlInfoArea.Controls.Add(this.lblClockDate);
            this.pnlInfoArea.Controls.Add(this.lblClockTime);
            this.pnlInfoArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInfoArea.Location = new System.Drawing.Point(906, 10);
            this.pnlInfoArea.Margin = new System.Windows.Forms.Padding(10);
            this.pnlInfoArea.Name = "pnlInfoArea";
            this.pnlInfoArea.Size = new System.Drawing.Size(364, 700);
            this.pnlInfoArea.TabIndex = 1;
            // 
            // lsvRecentActivity
            // 
            this.lsvRecentActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvRecentActivity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvRecentActivity.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsvRecentActivity.HideSelection = false;
            this.lsvRecentActivity.Location = new System.Drawing.Point(20, 575);
            this.lsvRecentActivity.Name = "lsvRecentActivity";
            this.lsvRecentActivity.Size = new System.Drawing.Size(324, 112);
            this.lsvRecentActivity.TabIndex = 10;
            this.lsvRecentActivity.UseCompatibleStateImageBehavior = false;
            this.lsvRecentActivity.View = System.Windows.Forms.View.List;
            // 
            // lblRecentActivityTitle
            // 
            this.lblRecentActivityTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecentActivityTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecentActivityTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblRecentActivityTitle.Location = new System.Drawing.Point(20, 544);
            this.lblRecentActivityTitle.Name = "lblRecentActivityTitle";
            this.lblRecentActivityTitle.Size = new System.Drawing.Size(324, 28);
            this.lblRecentActivityTitle.TabIndex = 9;
            this.lblRecentActivityTitle.Text = "HOẠT ĐỘNG GẦN ĐÂY";
            this.lblRecentActivityTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRegister
            // 
            this.btnRegister.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(20, 485);
            this.btnRegister.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(324, 50);
            this.btnRegister.TabIndex = 11;
            this.btnRegister.Text = "➕ Đăng Ký";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // pnlStatusResult
            // 
            this.pnlStatusResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlStatusResult.BackColor = System.Drawing.Color.SeaGreen;
            this.pnlStatusResult.Controls.Add(this.lblStatusResult);
            this.pnlStatusResult.Location = new System.Drawing.Point(20, 420);
            this.pnlStatusResult.Name = "pnlStatusResult";
            this.pnlStatusResult.Size = new System.Drawing.Size(324, 56);
            this.pnlStatusResult.TabIndex = 8;
            this.pnlStatusResult.Visible = false;
            // 
            // lblStatusResult
            // 
            this.lblStatusResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusResult.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusResult.ForeColor = System.Drawing.Color.White;
            this.lblStatusResult.Location = new System.Drawing.Point(0, 0);
            this.lblStatusResult.Name = "lblStatusResult";
            this.lblStatusResult.Size = new System.Drawing.Size(324, 56);
            this.lblStatusResult.TabIndex = 1;
            this.lblStatusResult.Text = "Chấm công vào thành công";
            this.lblStatusResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoTableLayoutPanel
            // 
            this.infoTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoTableLayoutPanel.ColumnCount = 2;
            this.infoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.infoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.infoTableLayoutPanel.Controls.Add(this.picAvatar, 0, 0);
            this.infoTableLayoutPanel.Controls.Add(this.panelInfoText, 1, 0);
            this.infoTableLayoutPanel.Location = new System.Drawing.Point(20, 200);
            this.infoTableLayoutPanel.Name = "infoTableLayoutPanel";
            this.infoTableLayoutPanel.RowCount = 1;
            this.infoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.infoTableLayoutPanel.Size = new System.Drawing.Size(324, 150);
            this.infoTableLayoutPanel.TabIndex = 4;
            // 
            // picAvatar
            // 
            this.picAvatar.BackColor = System.Drawing.Color.Gainsboro;
            this.picAvatar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picAvatar.Location = new System.Drawing.Point(3, 3);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(144, 144);
            this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAvatar.TabIndex = 4;
            this.picAvatar.TabStop = false;
            // 
            // panelInfoText
            // 
            this.panelInfoText.Controls.Add(this.lblTimestamp);
            this.panelInfoText.Controls.Add(this.lblEmployeeID);
            this.panelInfoText.Controls.Add(this.lblFullName);
            this.panelInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfoText.Location = new System.Drawing.Point(153, 3);
            this.panelInfoText.Name = "panelInfoText";
            this.panelInfoText.Size = new System.Drawing.Size(168, 144);
            this.panelInfoText.TabIndex = 5;
            // 
            // lblTimestamp
            // 
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimestamp.Location = new System.Drawing.Point(3, 89);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new System.Drawing.Size(94, 25);
            this.lblTimestamp.TabIndex = 7;
            this.lblTimestamp.Text = "Thời gian: ";
            // 
            // lblEmployeeID
            // 
            this.lblEmployeeID.AutoSize = true;
            this.lblEmployeeID.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeID.Location = new System.Drawing.Point(3, 51);
            this.lblEmployeeID.Name = "lblEmployeeID";
            this.lblEmployeeID.Size = new System.Drawing.Size(75, 25);
            this.lblEmployeeID.TabIndex = 6;
            this.lblEmployeeID.Text = "Mã NV: ";
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullName.Location = new System.Drawing.Point(3, 13);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(105, 25);
            this.lblFullName.TabIndex = 5;
            this.lblFullName.Text = "Họ và tên: ";
            // 
            // lblInfoTitle
            // 
            this.lblInfoTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfoTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblInfoTitle.Location = new System.Drawing.Point(3, 150);
            this.lblInfoTitle.Name = "lblInfoTitle";
            this.lblInfoTitle.Size = new System.Drawing.Size(358, 28);
            this.lblInfoTitle.TabIndex = 3;
            this.lblInfoTitle.Text = "THÔNG TIN CHẤM CÔNG";
            this.lblInfoTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // separatorLine
            // 
            this.separatorLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.separatorLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorLine.Location = new System.Drawing.Point(20, 130);
            this.separatorLine.Name = "separatorLine";
            this.separatorLine.Size = new System.Drawing.Size(324, 2);
            this.separatorLine.TabIndex = 2;
            // 
            // lblClockDate
            // 
            this.lblClockDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClockDate.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClockDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblClockDate.Location = new System.Drawing.Point(3, 75);
            this.lblClockDate.Name = "lblClockDate";
            this.lblClockDate.Size = new System.Drawing.Size(358, 38);
            this.lblClockDate.TabIndex = 1;
            this.lblClockDate.Text = "Thứ Bảy, 18/10/2025";
            this.lblClockDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblClockTime
            // 
            this.lblClockTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClockTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClockTime.Location = new System.Drawing.Point(3, 10);
            this.lblClockTime.Name = "lblClockTime";
            this.lblClockTime.Size = new System.Drawing.Size(358, 65);
            this.lblClockTime.TabIndex = 0;
            this.lblClockTime.Text = "14:22:15";
            this.lblClockTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AttendanceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Name = "AttendanceControl";
            this.Size = new System.Drawing.Size(1280, 720);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.pnlCameraArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).EndInit();
            this.pnlInfoArea.ResumeLayout(false);
            this.pnlStatusResult.ResumeLayout(false);
            this.infoTableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.panelInfoText.ResumeLayout(false);
            this.panelInfoText.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Panel pnlCameraArea;
        private System.Windows.Forms.Panel pnlInfoArea;
        private System.Windows.Forms.Label lblInstruction;
        private System.Windows.Forms.PictureBox picCamera;
        private System.Windows.Forms.Label lblClockDate;
        private System.Windows.Forms.Label lblClockTime;
        private System.Windows.Forms.Label separatorLine;
        private System.Windows.Forms.Label lblInfoTitle;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Label lblTimestamp;
        private System.Windows.Forms.Label lblEmployeeID;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Panel pnlStatusResult;
        private System.Windows.Forms.Label lblStatusResult;
        private System.Windows.Forms.Label lblRecentActivityTitle;
        private System.Windows.Forms.ListView lsvRecentActivity;
        private System.Windows.Forms.TableLayoutPanel infoTableLayoutPanel;
        private System.Windows.Forms.Panel panelInfoText;
        private System.Windows.Forms.Button btnRegister;
    }
}