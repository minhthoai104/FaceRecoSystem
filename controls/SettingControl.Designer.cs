namespace FaceRecoSystem.controls
{
    partial class SettingControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numResetDelay = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.numCheckinCooldown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numCheckoutDelay = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numHoldTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numResetDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCheckinCooldown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCheckoutDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHoldTime)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.numResetDelay);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.numCheckinCooldown);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.numCheckoutDelay);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numHoldTime);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(34, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(907, 380);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cấu hình";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DimGray;
            this.label11.Location = new System.Drawing.Point(30, 325);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(525, 23);
            this.label11.TabIndex = 13;
            this.label11.Text = "* Thời gian tự động ẩn bảng thông tin chấm công sau khi hoàn tất.";
            // 
            // numResetDelay
            // 
            this.numResetDelay.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.numResetDelay.Location = new System.Drawing.Point(34, 288);
            this.numResetDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numResetDelay.Name = "numResetDelay";
            this.numResetDelay.Size = new System.Drawing.Size(120, 34);
            this.numResetDelay.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(29, 257);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(261, 28);
            this.label9.TabIndex = 11;
            this.label9.Text = "Thời gian tự động làm mới:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(160, 290);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 28);
            this.label8.TabIndex = 10;
            this.label8.Text = "giây";
            // 
            // numCheckinCooldown
            // 
            this.numCheckinCooldown.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.numCheckinCooldown.Location = new System.Drawing.Point(475, 171);
            this.numCheckinCooldown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numCheckinCooldown.Name = "numCheckinCooldown";
            this.numCheckinCooldown.Size = new System.Drawing.Size(120, 34);
            this.numCheckinCooldown.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(601, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 28);
            this.label5.TabIndex = 8;
            this.label5.Text = "phút";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(470, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(249, 28);
            this.label6.TabIndex = 7;
            this.label6.Text = "Thời gian chờ check-in lại:";
            // 
            // numCheckoutDelay
            // 
            this.numCheckoutDelay.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.numCheckoutDelay.Location = new System.Drawing.Point(34, 171);
            this.numCheckoutDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numCheckoutDelay.Name = "numCheckoutDelay";
            this.numCheckoutDelay.Size = new System.Drawing.Size(120, 34);
            this.numCheckoutDelay.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(160, 173);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 28);
            this.label7.TabIndex = 5;
            this.label7.Text = "phút";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(29, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(374, 28);
            this.label4.TabIndex = 4;
            this.label4.Text = "Thời gian tối thiểu giữa In/Out (ca làm):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(30, 208);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(813, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "*Ngăn một người check-in xong rồi check-out ngay lập tức. Giá trị này nên lớn hơn" +
    " thời gian chờ check-in lại.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(160, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 28);
            this.label2.TabIndex = 2;
            this.label2.Text = "giây";
            // 
            // numHoldTime
            // 
            this.numHoldTime.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numHoldTime.Location = new System.Drawing.Point(34, 69);
            this.numHoldTime.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numHoldTime.Name = "numHoldTime";
            this.numHoldTime.Size = new System.Drawing.Size(120, 34);
            this.numHoldTime.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thời gian giữ yên để chấm công:";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(114)))), ((int)(((byte)(175)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(792, 507);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 45);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.BackColor = System.Drawing.Color.Gainsboro;
            this.btnDefault.FlatAppearance.BorderSize = 0;
            this.btnDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefault.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnDefault.ForeColor = System.Drawing.Color.Black;
            this.btnDefault.Location = new System.Drawing.Point(636, 507);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(140, 45);
            this.btnDefault.TabIndex = 2;
            this.btnDefault.Text = "Mặc định";
            this.btnDefault.UseVisualStyleBackColor = false;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(114)))), ((int)(((byte)(175)))));
            this.label10.Location = new System.Drawing.Point(359, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(280, 38);
            this.label10.TabIndex = 3;
            this.label10.Text = "CÀI ĐẶT HỆ THỐNG";
            // 
            // SettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingControl";
            this.Size = new System.Drawing.Size(976, 700);
            this.Load += new System.EventHandler(this.SettingControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numResetDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCheckinCooldown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCheckoutDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHoldTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numHoldTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numCheckinCooldown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numCheckoutDelay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numResetDelay;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
    }
}