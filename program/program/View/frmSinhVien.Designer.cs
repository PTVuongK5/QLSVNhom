namespace program.View
{
    partial class frmSinhVien
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMaLop = new System.Windows.Forms.Label();
            this.cmbLop = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.dgvSinhVien = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSinhVien)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMaLop
            // 
            this.lblMaLop.AutoSize = true;
            this.lblMaLop.Location = new System.Drawing.Point(16, 16);
            this.lblMaLop.Name = "lblMaLop";
            this.lblMaLop.Size = new System.Drawing.Size(58, 20);
            this.lblMaLop.TabIndex = 0;
            this.lblMaLop.Text = "Lớp";
            // 
            // cmbLop
            // 
            this.cmbLop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLop.Location = new System.Drawing.Point(80, 12);
            this.cmbLop.Name = "cmbLop";
            this.cmbLop.Size = new System.Drawing.Size(200, 28);
            this.cmbLop.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(300, 10);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(95, 30);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Tải";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dgvSinhVien
            // 
            this.dgvSinhVien.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSinhVien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSinhVien.Location = new System.Drawing.Point(16, 50);
            this.dgvSinhVien.Name = "dgvSinhVien";
            this.dgvSinhVien.RowHeadersWidth = 51;
            this.dgvSinhVien.RowTemplate.Height = 29;
            this.dgvSinhVien.Size = new System.Drawing.Size(768, 380);
            this.dgvSinhVien.TabIndex = 3;
            // 
            // frmSinhVien
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvSinhVien);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.cmbLop);
            this.Controls.Add(this.lblMaLop);
            this.Name = "frmSinhVien";
            this.Text = "Quản lý sinh viên";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSinhVien)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label lblMaLop;
        private System.Windows.Forms.ComboBox cmbLop;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridView dgvSinhVien;
    }
}
