namespace program.View
{
    partial class frmSinhVien
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle rowStyle = new System.Windows.Forms.DataGridViewCellStyle();

            // ── Khởi tạo controls ─────────────────────────────────────────
            pnlTop = new System.Windows.Forms.Panel();
            lblLop = new System.Windows.Forms.Label();
            cmbLop = new System.Windows.Forms.ComboBox();
            btnLoad = new System.Windows.Forms.Button();

            pnlSearch = new System.Windows.Forms.Panel();
            lblSearch = new System.Windows.Forms.Label();
            txtSearch = new System.Windows.Forms.TextBox();
            btnSearch = new System.Windows.Forms.Button();

            pnlInput = new System.Windows.Forms.Panel();
            lblMaSV = new System.Windows.Forms.Label();
            txtMaSV = new System.Windows.Forms.TextBox();
            lblHoTen = new System.Windows.Forms.Label();
            txtHoTen = new System.Windows.Forms.TextBox();
            lblNgaySinh = new System.Windows.Forms.Label();
            dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            lblDiaChi = new System.Windows.Forms.Label();
            txtDiaChi = new System.Windows.Forms.TextBox();
            lblTenDN = new System.Windows.Forms.Label();
            txtTenDN = new System.Windows.Forms.TextBox();
            lblMatKhau = new System.Windows.Forms.Label();
            txtMatKhau = new System.Windows.Forms.TextBox();
            btnAdd = new System.Windows.Forms.Button();
            btnUpdate = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();

            dgvSinhVien = new System.Windows.Forms.DataGridView();

            pnlTop.SuspendLayout();
            pnlSearch.SuspendLayout();
            pnlInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSinhVien).BeginInit();
            SuspendLayout();

            // ── pnlTop: chọn lớp ─────────────────────────────────────────
            pnlTop.Controls.Add(lblLop);
            pnlTop.Controls.Add(cmbLop);
            pnlTop.Controls.Add(btnLoad);
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new System.Windows.Forms.Padding(8);
            pnlTop.Size = new System.Drawing.Size(1000, 52);
            pnlTop.TabIndex = 0;

            lblLop.AutoSize = true;
            lblLop.Location = new System.Drawing.Point(8, 16);
            lblLop.Name = "lblLop";
            lblLop.Text = "Lớp";

            cmbLop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbLop.Location = new System.Drawing.Point(80, 12);
            cmbLop.Name = "cmbLop";
            cmbLop.Size = new System.Drawing.Size(720, 27);
            cmbLop.TabIndex = 0;

            btnLoad.Location = new System.Drawing.Point(808, 8);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(80, 36);
            btnLoad.TabIndex = 1;
            btnLoad.Text = "Tải lớp";
            btnLoad.UseVisualStyleBackColor = true;

            // ── pnlSearch: tìm kiếm theo Mã SV ───────────────────────────
            pnlSearch.Controls.Add(lblSearch);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            pnlSearch.Size = new System.Drawing.Size(1000, 44);
            pnlSearch.TabIndex = 1;
            // Đường kẻ phân cách nhẹ
            pnlSearch.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

            lblSearch.AutoSize = true;
            lblSearch.Location = new System.Drawing.Point(8, 13);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Tìm mã SV";

            txtSearch.Location = new System.Drawing.Point(90, 9);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(300, 27);
            txtSearch.TabIndex = 2;
            txtSearch.PlaceholderText = "Nhập mã SV (Enter để tìm)...";

            btnSearch.Location = new System.Drawing.Point(396, 7);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(90, 30);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = true;

            // ── pnlInput: nhập / chỉnh sửa thông tin sinh viên ───────────
            pnlInput.Controls.Add(lblMaSV);
            pnlInput.Controls.Add(txtMaSV);
            pnlInput.Controls.Add(lblHoTen);
            pnlInput.Controls.Add(txtHoTen);
            pnlInput.Controls.Add(lblNgaySinh);
            pnlInput.Controls.Add(dtpNgaySinh);
            pnlInput.Controls.Add(lblDiaChi);
            pnlInput.Controls.Add(txtDiaChi);
            pnlInput.Controls.Add(lblTenDN);
            pnlInput.Controls.Add(txtTenDN);
            pnlInput.Controls.Add(lblMatKhau);
            pnlInput.Controls.Add(txtMatKhau);
            pnlInput.Controls.Add(btnAdd);
            pnlInput.Controls.Add(btnUpdate);
            pnlInput.Controls.Add(btnDelete);
            pnlInput.Controls.Add(btnClear);
            pnlInput.Dock = System.Windows.Forms.DockStyle.Top;
            pnlInput.Name = "pnlInput";
            pnlInput.Padding = new System.Windows.Forms.Padding(8);
            pnlInput.Size = new System.Drawing.Size(1000, 112);
            pnlInput.TabIndex = 2;

            // Layout constants
            int lbl1 = 8, fld1 = 88, fldW = 160;
            int lbl2 = 256, fld2 = 340, fldW2 = 160;
            int lbl3 = 508, fld3 = 600, fldW3 = 155;
            int btnX = 763, btnW = 82, btnH = 36, btnGap = 4;
            int row1 = 10, row2 = 56;

            // Row 1
            lblMaSV.AutoSize = true;
            lblMaSV.Location = new System.Drawing.Point(lbl1, row1 + 5);
            lblMaSV.Name = "lblMaSV";
            lblMaSV.Text = "Mã SV";

            txtMaSV.Location = new System.Drawing.Point(fld1, row1);
            txtMaSV.Name = "txtMaSV";
            txtMaSV.Size = new System.Drawing.Size(fldW, 27);
            txtMaSV.TabIndex = 4;

            lblHoTen.AutoSize = true;
            lblHoTen.Location = new System.Drawing.Point(lbl2, row1 + 5);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Text = "Họ tên";

            txtHoTen.Location = new System.Drawing.Point(fld2, row1);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new System.Drawing.Size(fldW2, 27);
            txtHoTen.TabIndex = 5;

            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Location = new System.Drawing.Point(lbl3, row1 + 5);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Text = "Ngày sinh";

            dtpNgaySinh.Location = new System.Drawing.Point(fld3, row1);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new System.Drawing.Size(fldW3, 27);
            dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpNgaySinh.TabIndex = 6;

            // Row 2
            lblDiaChi.AutoSize = true;
            lblDiaChi.Location = new System.Drawing.Point(lbl1, row2 + 5);
            lblDiaChi.Name = "lblDiaChi";
            lblDiaChi.Text = "Địa chỉ";

            txtDiaChi.Location = new System.Drawing.Point(fld1, row2);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new System.Drawing.Size(fldW, 27);
            txtDiaChi.TabIndex = 7;

            lblTenDN.AutoSize = true;
            lblTenDN.Location = new System.Drawing.Point(lbl2, row2 + 5);
            lblTenDN.Name = "lblTenDN";
            lblTenDN.Text = "Tên ĐN";

            txtTenDN.Location = new System.Drawing.Point(fld2, row2);
            txtTenDN.Name = "txtTenDN";
            txtTenDN.Size = new System.Drawing.Size(fldW2, 27);
            txtTenDN.TabIndex = 8;

            lblMatKhau.AutoSize = true;
            lblMatKhau.Location = new System.Drawing.Point(lbl3, row2 + 5);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Text = "Mật khẩu";

            txtMatKhau.Location = new System.Drawing.Point(fld3, row2);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new System.Drawing.Size(fldW3, 27);
            txtMatKhau.UseSystemPasswordChar = true;
            txtMatKhau.TabIndex = 9;

            // Buttons 2×2 (Thêm | Sửa / Xóa | Làm mới)
            btnAdd.Location = new System.Drawing.Point(btnX, row1);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(btnW, btnH);
            btnAdd.TabIndex = 10;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = true;

            btnUpdate.Location = new System.Drawing.Point(btnX + btnW + btnGap, row1);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(btnW, btnH);
            btnUpdate.TabIndex = 11;
            btnUpdate.Text = "Sửa";
            btnUpdate.UseVisualStyleBackColor = true;

            btnDelete.Location = new System.Drawing.Point(btnX, row2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(btnW, btnH);
            btnDelete.TabIndex = 12;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = true;

            btnClear.Location = new System.Drawing.Point(btnX + btnW + btnGap, row2);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(btnW, btnH);
            btnClear.TabIndex = 13;
            btnClear.Text = "Làm mới";
            btnClear.UseVisualStyleBackColor = true;

            // ── dgvSinhVien ───────────────────────────────────────────────
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            headerStyle.BackColor = System.Drawing.SystemColors.Control;
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            headerStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            headerStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            headerStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            headerStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvSinhVien.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvSinhVien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            rowStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            rowStyle.BackColor = System.Drawing.SystemColors.Window;
            rowStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            rowStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            rowStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            rowStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            rowStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvSinhVien.DefaultCellStyle = rowStyle;
            dgvSinhVien.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvSinhVien.Margin = new System.Windows.Forms.Padding(8);
            dgvSinhVien.Name = "dgvSinhVien";
            dgvSinhVien.RowHeadersWidth = 51;
            dgvSinhVien.RowTemplate.Height = 29;
            dgvSinhVien.TabIndex = 14;

            // ── frmSinhVien ───────────────────────────────────────────────
            // Controls phải Add theo thứ tự ngược (Fill sẽ đẩy xuống đúng):
            // pnlTop (Top) → pnlSearch (Top) → pnlInput (Top) → dgvSinhVien (Fill)
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1000, 680);
            Controls.Add(dgvSinhVien);   // Fill — thêm trước
            Controls.Add(pnlInput);      // Top
            Controls.Add(pnlSearch);     // Top
            Controls.Add(pnlTop);        // Top — thêm sau cùng → nằm trên cùng
            Font = new System.Drawing.Font("Segoe UI", 9F);
            Name = "frmSinhVien";
            Text = "Quản lý sinh viên";

            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            pnlInput.ResumeLayout(false);
            pnlInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSinhVien).EndInit();
            ResumeLayout(false);
        }

        #endregion

        // ── Field declarations ────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblLop;
        private System.Windows.Forms.ComboBox cmbLop;
        private System.Windows.Forms.Button btnLoad;

        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;

        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Label lblMaSV;
        private System.Windows.Forms.TextBox txtMaSV;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblNgaySinh;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblTenDN;
        private System.Windows.Forms.TextBox txtTenDN;
        private System.Windows.Forms.Label lblMatKhau;
        private System.Windows.Forms.TextBox txtMatKhau;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;

        private System.Windows.Forms.DataGridView dgvSinhVien;
    }
}