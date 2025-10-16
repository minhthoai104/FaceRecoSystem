using System.Windows.Forms;
using System.Drawing;

namespace FaceRecoSystem
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelMenu;
        private Panel panelMain;
        private Button btnAttendance;
        private Button btnAddPerson;
        private Button btnUpdatePerson;
        private Button btnDeletePerson;
        private Button btnViewAttendanceList;
        private Button btnViewEmpList;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnDeletePerson = new System.Windows.Forms.Button();
            this.btnUpdatePerson = new System.Windows.Forms.Button();
            this.btnAddPerson = new System.Windows.Forms.Button();
            this.btnViewAttendanceList = new System.Windows.Forms.Button();
            this.btnAttendance = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnViewEmpList = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(114)))), ((int)(((byte)(175)))));
            this.panelMenu.Controls.Add(this.btnViewEmpList);
            this.panelMenu.Controls.Add(this.btnDeletePerson);
            this.panelMenu.Controls.Add(this.btnUpdatePerson);
            this.panelMenu.Controls.Add(this.btnAddPerson);
            this.panelMenu.Controls.Add(this.btnViewAttendanceList);
            this.panelMenu.Controls.Add(this.btnAttendance);
            this.panelMenu.Controls.Add(this.lblTitle);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(224, 700);
            this.panelMenu.TabIndex = 0;
            // 
            // button1
            // 

            // 
            // btnDeletePerson
            // 
            this.btnDeletePerson.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDeletePerson.FlatAppearance.BorderSize = 0;
            this.btnDeletePerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeletePerson.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeletePerson.ForeColor = System.Drawing.Color.White;
            this.btnDeletePerson.Location = new System.Drawing.Point(0, 300);
            this.btnDeletePerson.Name = "btnDeletePerson";
            this.btnDeletePerson.Size = new System.Drawing.Size(224, 60);
            this.btnDeletePerson.TabIndex = 2;
            this.btnDeletePerson.Text = "🗑 Xóa nhân viên";
            // 
            // btnUpdatePerson
            // 
            this.btnUpdatePerson.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUpdatePerson.FlatAppearance.BorderSize = 0;
            this.btnUpdatePerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdatePerson.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdatePerson.ForeColor = System.Drawing.Color.White;
            this.btnUpdatePerson.Location = new System.Drawing.Point(0, 240);
            this.btnUpdatePerson.Name = "btnUpdatePerson";
            this.btnUpdatePerson.Size = new System.Drawing.Size(224, 60);
            this.btnUpdatePerson.TabIndex = 3;
            this.btnUpdatePerson.Text = "🛠 Cập nhật";
            // 
            // btnAddPerson
            // 
            this.btnAddPerson.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddPerson.FlatAppearance.BorderSize = 0;
            this.btnAddPerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPerson.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddPerson.ForeColor = System.Drawing.Color.White;
            this.btnAddPerson.Location = new System.Drawing.Point(0, 180);
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(224, 60);
            this.btnAddPerson.TabIndex = 4;
            this.btnAddPerson.Text = "➕ Thêm nhân viên";
            // 
            // btnViewAttendanceList
            // 
            this.btnViewAttendanceList.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnViewAttendanceList.FlatAppearance.BorderSize = 0;
            this.btnViewAttendanceList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewAttendanceList.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewAttendanceList.ForeColor = System.Drawing.Color.White;
            this.btnViewAttendanceList.Location = new System.Drawing.Point(0, 120);
            this.btnViewAttendanceList.Name = "btnViewAttendanceList";
            this.btnViewAttendanceList.Size = new System.Drawing.Size(224, 60);
            this.btnViewAttendanceList.TabIndex = 0;
            this.btnViewAttendanceList.Text = "📋 Danh sách chấm công";
            // 
            // btnAttendance
            // 
            this.btnAttendance.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAttendance.FlatAppearance.BorderSize = 0;
            this.btnAttendance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAttendance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAttendance.ForeColor = System.Drawing.Color.White;
            this.btnAttendance.Location = new System.Drawing.Point(0, 60);
            this.btnAttendance.Name = "btnAttendance";
            this.btnAttendance.Size = new System.Drawing.Size(224, 60);
            this.btnAttendance.TabIndex = 1;
            this.btnAttendance.Text = "📸 Chấm công";
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(224, 60);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "HỆ THỐNG NHẬN DIỆN";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnViewEmpList
            // 
            this.btnViewEmpList.Location = new System.Drawing.Point(0, 0);
            this.btnViewEmpList.TabIndex = 0;
            this.btnViewEmpList.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnViewEmpList.FlatAppearance.BorderSize = 0;
            this.btnViewEmpList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewEmpList.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewEmpList.ForeColor = System.Drawing.Color.White;
            this.btnViewEmpList.Name = "btnViewEmpList";
            this.btnViewEmpList.Size = new System.Drawing.Size(224, 60);
            //this.btnViewEmpList.TabIndex = 6;
            this.btnViewEmpList.Text = "📋 Danh sách nhân viên";
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(224, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(976, 700);
            this.panelMain.TabIndex = 1;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelMenu);
            this.MinimumSize = new System.Drawing.Size(1024, 600);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống nhận diện khuôn mặt";
            this.Load += new System.EventHandler(this.FaceReco_Load);
            this.panelMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}