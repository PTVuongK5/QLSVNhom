namespace program.View
{
    partial class frmLopHoc
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
            this.lblMaLop = new System.Windows.Forms.Label();
            this.lblTenLop = new System.Windows.Forms.Label();
            this.txtMaLop = new System.Windows.Forms.TextBox();
            this.txtTenLop = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dgvLop = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLop)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMaLop
            // 
            this.lblMaLop.AutoSize = true;
            this.lblMaLop.Location = new System.Drawing.Point(16, 16);
            this.lblMaLop.Name = "lblMaLop";
            this.lblMaLop.Size = new System.Drawing.Size(58, 20);
            this.lblMaLop.TabIndex = 0;
            this.lblMaLop.Text = "Mã lớp";
            // 
            // lblTenLop
            // 
            this.lblTenLop.AutoSize = true;
            this.lblTenLop.Location = new System.Drawing.Point(16, 52);
            this.lblTenLop.Name = "lblTenLop";
            this.lblTenLop.Size = new System.Drawing.Size(61, 20);
            this.lblTenLop.TabIndex = 1;
            this.lblTenLop.Text = "Tên lớp";
            // 
            // txtMaLop
            // 
            this.txtMaLop.Location = new System.Drawing.Point(92, 13);
            this.txtMaLop.Name = "txtMaLop";
            this.txtMaLop.Size = new System.Drawing.Size(200, 27);
            this.txtMaLop.TabIndex = 2;
            // 
            // txtTenLop
            // 
            this.txtTenLop.Location = new System.Drawing.Point(92, 49);
            this.txtTenLop.Name = "txtTenLop";
            this.txtTenLop.Size = new System.Drawing.Size(300, 27);
            this.txtTenLop.TabIndex = 3;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(410, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(95, 30);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Tải";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(510, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(95, 30);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(610, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(95, 30);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(710, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(95, 30);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // dgvLop
            // 
            this.dgvLop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLop.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLop.Location = new System.Drawing.Point(16, 90);
            this.dgvLop.Name = "dgvLop";
            this.dgvLop.RowHeadersWidth = 51;
            this.dgvLop.RowTemplate.Height = 29;
            this.dgvLop.Size = new System.Drawing.Size(789, 340);
            this.dgvLop.TabIndex = 8;
            // 
            // frmLopHoc
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 450);
            this.Controls.Add(this.dgvLop);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtTenLop);
            this.Controls.Add(this.txtMaLop);
            this.Controls.Add(this.lblTenLop);
            this.Controls.Add(this.lblMaLop);
            this.Name = "frmLopHoc";
            this.Text = "Quản lý lớp học";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label lblMaLop;
        private System.Windows.Forms.Label lblTenLop;
        private System.Windows.Forms.TextBox txtMaLop;
        private System.Windows.Forms.TextBox txtTenLop;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dgvLop;
    }
}
