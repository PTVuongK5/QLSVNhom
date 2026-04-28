namespace program.View
{
    partial class frmMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuLopHoc = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSinhVien = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNhapDiem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLopHoc,
            this.mnuSinhVien,
            this.mnuNhapDiem,
            this.mnuLogout,
            this.mnuExit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(900, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuLopHoc
            // 
            this.mnuLopHoc.Name = "mnuLopHoc";
            this.mnuLopHoc.Size = new System.Drawing.Size(79, 24);
            this.mnuLopHoc.Text = "Lớp học";
            this.mnuLopHoc.Click += new System.EventHandler(this.mnuLopHoc_Click);
            // 
            // mnuSinhVien
            // 
            this.mnuSinhVien.Name = "mnuSinhVien";
            this.mnuSinhVien.Size = new System.Drawing.Size(89, 24);
            this.mnuSinhVien.Text = "Sinh viên";
            this.mnuSinhVien.Click += new System.EventHandler(this.mnuSinhVien_Click);
            // 
            // mnuNhapDiem
            // 
            this.mnuNhapDiem.Name = "mnuNhapDiem";
            this.mnuNhapDiem.Size = new System.Drawing.Size(90, 24);
            this.mnuNhapDiem.Text = "Nhập điểm";
            this.mnuNhapDiem.Click += new System.EventHandler(this.mnuNhapDiem_Click);
            // 
            // mnuLogout
            // 
            this.mnuLogout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuLogout.Name = "mnuLogout";
            this.mnuLogout.Size = new System.Drawing.Size(74, 24);
            this.mnuLogout.Text = "Đăng xuất";
            this.mnuLogout.Click += new System.EventHandler(this.mnuLogout_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(62, 24);
            this.mnuExit.Text = "Thoát";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 28);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(900, 420);
            this.pnlContent.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 448);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(900, 24);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Nhân viên: - | Mã NV: - | Trạng thái";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 472);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuLopHoc;
        private System.Windows.Forms.ToolStripMenuItem mnuSinhVien;
        private System.Windows.Forms.ToolStripMenuItem mnuNhapDiem;
        private System.Windows.Forms.ToolStripMenuItem mnuLogout;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblStatus;
    }
}