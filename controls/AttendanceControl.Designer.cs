namespace FaceRecoSystem.controls
{
    partial class AttendanceControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox picCamera;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelContainer;

        private void InitializeComponent()
        {
            this.panelContainer = new System.Windows.Forms.Panel();
            this.picCamera = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).BeginInit();
            this.SuspendLayout();
            // 
            // panelContainer
            // 
            this.panelContainer.BackColor = System.Drawing.Color.Black;
            this.panelContainer.Controls.Add(this.picCamera);
            this.panelContainer.Controls.Add(this.lblStatus);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Padding = new System.Windows.Forms.Padding(5);
            this.panelContainer.Size = new System.Drawing.Size(1000, 700);
            this.panelContainer.TabIndex = 0;
            // 
            // picCamera
            // 
            this.picCamera.BackColor = System.Drawing.Color.Black;
            this.picCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCamera.Location = new System.Drawing.Point(5, 5);
            this.picCamera.Name = "picCamera";
            this.picCamera.Size = new System.Drawing.Size(990, 650);
            this.picCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCamera.TabIndex = 0;
            this.picCamera.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(5, 655);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(990, 40);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Đang khởi tạo camera...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AttendanceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContainer);
            this.Name = "AttendanceControl";
            this.Size = new System.Drawing.Size(1000, 700);
            this.Load += new System.EventHandler(this.AttendanceControl_Load);
            this.panelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
