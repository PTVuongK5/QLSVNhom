namespace program.View
{
    partial class frmNhapDiem
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
            this.lblMaSV = new System.Windows.Forms.Label();
            this.txtMaSV = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.dgvBangDiem = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBangDiem)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMaSV
            // 
            this.lblMaSV.AutoSize = true;
            this.lblMaSV.Location = new System.Drawing.Point(16, 16);
            this.lblMaSV.Name = "lblMaSV";
            this.lblMaSV.Size = new System.Drawing.Size(61, 20);
            this.lblMaSV.TabIndex = 0;
            this.lblMaSV.Text = "Mã SV";
            // 
            // txtMaSV
            // 
            this.txtMaSV.Location = new System.Drawing.Point(84, 12);
            this.txtMaSV.Name = "txtMaSV";
            this.txtMaSV.Size = new System.Drawing.Size(200, 27);
            this.txtMaSV.TabIndex = 1;
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
            // dgvBangDiem
            // 
            this.dgvBangDiem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBangDiem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBangDiem.Location = new System.Drawing.Point(16, 50);
            this.dgvBangDiem.Name = "dgvBangDiem";
            this.dgvBangDiem.RowHeadersWidth = 51;
            this.dgvBangDiem.RowTemplate.Height = 29;
            this.dgvBangDiem.Size = new System.Drawing.Size(768, 380);
            this.dgvBangDiem.TabIndex = 3;
            // 
            // frmNhapDiem
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvBangDiem);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtMaSV);
            this.Controls.Add(this.lblMaSV);
            this.Name = "frmNhapDiem";
            this.Text = "Nhập điểm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBangDiem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label lblMaSV;
        private System.Windows.Forms.TextBox txtMaSV;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridView dgvBangDiem;
    }
}
