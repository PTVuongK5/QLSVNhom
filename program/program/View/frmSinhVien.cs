using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using program.DataAccess;
using program.Models;

namespace program.View
{
    public partial class frmSinhVien : Form
    {
        // Flag để tắt SelectionChanged khi đang gán DataSource mới
        private bool _isBinding = false;

        public frmSinhVien()
        {
            InitializeComponent();
            Load += frmSinhVien_Load;
            btnLoad.Click += btnLoad_Click;
            btnSearch.Click += btnSearch_Click;
            txtSearch.KeyDown += txtSearch_KeyDown;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            dgvSinhVien.SelectionChanged += dgvSinhVien_SelectionChanged;
        }

        // ── LOAD ──────────────────────────────────────────────────────────────

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            dtpNgaySinh.Value = DateTime.Today;
            SetButtonStates(rowSelected: false);
            LoadLopForUser();
            AutoSelectFirstClass();
        }

        private void LoadLopForUser()
        {
            try
            {
                var dt = SqlDbContext.ExecuteQuery("SP_SEL_LOP_BY_MANV", new[]
                {
                    new SqlParameter("@MANV", UserSession.MaNV)
                });

                cmbLop.DisplayMember = "TENLOP";
                cmbLop.ValueMember = "MALOP";
                cmbLop.DataSource = dt;
                cmbLop.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách lớp: " + ex.Message);
            }
        }

        private void AutoSelectFirstClass()
        {
            if (cmbLop.Items.Count > 0)
            {
                cmbLop.SelectedIndex = 0;
                LoadStudentsByClass(cmbLop.SelectedValue?.ToString() ?? string.Empty);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (cmbLop.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn lớp.");
                return;
            }
            txtSearch.Clear();
            LoadStudentsByClass(cmbLop.SelectedValue?.ToString() ?? string.Empty);
        }

        public void LoadStudentsByClass(string maLop)
        {
            try
            {
                var dt = SqlDbContext.ExecuteQuery("SP_SEL_SINHVIEN_BY_LOP_NV", new[]
                {
                    new SqlParameter("@MALOP", maLop),
                    new SqlParameter("@MANV",  UserSession.MaNV)
                });

                BindGrid(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách sinh viên: " + ex.Message);
            }
        }

        // ── TÌM KIẾM THEO MÃ SINH VIÊN ───────────────────────────────────────

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                if (cmbLop.SelectedItem != null)
                    LoadStudentsByClass(cmbLop.SelectedValue?.ToString() ?? string.Empty);
                return;
            }

            try
            {
                var dt = SqlDbContext.ExecuteQuery("SP_SEL_SINHVIEN_BY_MASV", new[]
                {
                    new SqlParameter("@MASV", keyword),
                    new SqlParameter("@MANV", UserSession.MaNV)
                });

                BindGrid(dt);

                if (dt == null || dt.Rows.Count == 0)
                    MessageBox.Show("Không tìm thấy sinh viên với mã: " + keyword);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        // ── BIND GRID ─────────────────────────────────────────────────────────

        private void BindGrid(DataTable dt)
        {
            // Bật flag để ngăn SelectionChanged chạy trong lúc gán DataSource
            _isBinding = true;
            try
            {
                dgvSinhVien.AutoGenerateColumns = true;
                dgvSinhVien.DataSource = dt;
                dgvSinhVien.ReadOnly = true;
                dgvSinhVien.AllowUserToAddRows = false;
                dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvSinhVien.MultiSelect = false;
            }
            finally
            {
                // Luôn tắt flag dù có lỗi hay không
                _isBinding = false;
            }

            // Sau khi bind xong: nếu grid tự chọn dòng (thường xảy ra khi chỉ có 1 dòng)
            // thì điền thông tin luôn, ngược lại clear form
            if (dgvSinhVien.CurrentRow != null && !dgvSinhVien.CurrentRow.IsNewRow)
                PopulateFormFromRow(dgvSinhVien.CurrentRow);
            else
            {
                ClearInputFields();
                SetButtonStates(rowSelected: false);
            }
        }

        // ── CHỌN DÒNG → ĐIỀN VÀO FORM ────────────────────────────────────────

        private void dgvSinhVien_SelectionChanged(object sender, EventArgs e)
        {
            // Bỏ qua nếu đang trong quá trình bind dữ liệu
            if (_isBinding) return;

            if (dgvSinhVien.CurrentRow == null || dgvSinhVien.CurrentRow.IsNewRow)
            {
                ClearInputFields();
                SetButtonStates(rowSelected: false);
                return;
            }

            PopulateFormFromRow(dgvSinhVien.CurrentRow);
        }

        /// <summary>
        /// Điền thông tin sinh viên từ một dòng grid vào các ô nhập liệu.
        /// Dùng chung cho SelectionChanged và sau khi BindGrid (trường hợp 1 dòng).
        /// </summary>
        private void PopulateFormFromRow(DataGridViewRow row)
        {
            txtMaSV.Text = row.Cells["MASV"]?.Value?.ToString() ?? string.Empty;
            txtHoTen.Text = row.Cells["HOTEN"]?.Value?.ToString() ?? string.Empty;
            txtDiaChi.Text = row.Cells["DIACHI"]?.Value?.ToString() ?? string.Empty;
            txtTenDN.Text = row.Cells["TENDN"]?.Value?.ToString() ?? string.Empty;

            // Không hiển thị lại mật khẩu đã băm — để trống
            txtMatKhau.Text = string.Empty;

            if (row.Cells["NGAYSINH"]?.Value != null
                && DateTime.TryParse(row.Cells["NGAYSINH"].Value.ToString(), out var date))
                dtpNgaySinh.Value = date;
            else
                dtpNgaySinh.Value = DateTime.Today;

            // Khóa trường không cho sửa
            txtMaSV.ReadOnly = true;
            txtTenDN.ReadOnly = true;

            // Đồng bộ combo lớp với lớp của sinh viên được chọn
            string maLopSv = row.Cells["MALOP"]?.Value?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(maLopSv))
                cmbLop.SelectedValue = maLopSv;

            SetButtonStates(rowSelected: true);
        }

        // ── THÊM SINH VIÊN ────────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string maSv = txtMaSV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string tenDN = txtTenDN.Text.Trim();
            string matKhau = txtMatKhau.Text;
            string diaChi = txtDiaChi.Text.Trim();

            if (cmbLop.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn lớp trước.");
                return;
            }

            if (string.IsNullOrWhiteSpace(maSv) || string.IsNullOrWhiteSpace(hoTen)
                || string.IsNullOrWhiteSpace(tenDN) || string.IsNullOrWhiteSpace(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ: Mã SV, Họ tên, Tên đăng nhập, Mật khẩu.");
                return;
            }

            string maLop = cmbLop.SelectedValue?.ToString() ?? string.Empty;

            try
            {
                var paras = new[]
                {
                    new SqlParameter("@MASV",     maSv),
                    new SqlParameter("@HOTEN",    hoTen),
                    new SqlParameter("@NGAYSINH", (object)dtpNgaySinh.Value),
                    new SqlParameter("@DIACHI",   string.IsNullOrWhiteSpace(diaChi) ? (object)DBNull.Value : diaChi),
                    new SqlParameter("@MALOP",    maLop),
                    new SqlParameter("@TENDN",    tenDN),
                    new SqlParameter("@MATKHAU",  matKhau)
                };

                SqlDbContext.ExecuteNonQueryStoredProcedure("SP_INS_SINHVIEN", paras);
                LoadStudentsByClass(maLop);
                MessageBox.Show("Đã thêm sinh viên thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm sinh viên: " + ex.Message);
            }
        }

        // ── CẬP NHẬT SINH VIÊN ───────────────────────────────────────────────

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string maSv = txtMaSV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            if (string.IsNullOrWhiteSpace(maSv) || string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show("Mã SV và Họ tên không được để trống.");
                return;
            }

            if (cmbLop.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn lớp.");
                return;
            }

            string maLop = cmbLop.SelectedValue?.ToString() ?? string.Empty;

            try
            {
                var paras = new[]
                {
                    new SqlParameter("@MASV",     maSv),
                    new SqlParameter("@HOTEN",    hoTen),
                    new SqlParameter("@NGAYSINH", (object)dtpNgaySinh.Value),
                    new SqlParameter("@DIACHI",   string.IsNullOrWhiteSpace(diaChi) ? (object)DBNull.Value : diaChi),
                    new SqlParameter("@MALOP",    maLop),
                    new SqlParameter("@MANV",     UserSession.MaNV)
                };

                SqlDbContext.ExecuteNonQueryStoredProcedure("SP_UPD_SINHVIEN", paras);
                LoadStudentsByClass(maLop);
                MessageBox.Show("Đã cập nhật sinh viên.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể cập nhật sinh viên: " + ex.Message);
            }
        }

        // ── XÓA SINH VIÊN ────────────────────────────────────────────────────

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string maSv = txtMaSV.Text.Trim();

            if (string.IsNullOrWhiteSpace(maSv))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa.");
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn xóa sinh viên {maSv}?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string maLop = cmbLop.SelectedValue?.ToString() ?? string.Empty;

            try
            {
                var paras = new[]
                {
                    new SqlParameter("@MASV", maSv),
                    new SqlParameter("@MANV", UserSession.MaNV)
                };

                SqlDbContext.ExecuteNonQueryStoredProcedure("SP_DEL_SINHVIEN", paras);
                LoadStudentsByClass(maLop);
                MessageBox.Show("Đã xóa sinh viên.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa sinh viên: " + ex.Message);
            }
        }

        // ── LÀM MỚI ──────────────────────────────────────────────────────────

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            dgvSinhVien.ClearSelection();
            ClearInputFields();
            SetButtonStates(rowSelected: false);
        }

        // ── HELPERS ───────────────────────────────────────────────────────────

        private void ClearInputFields()
        {
            txtMaSV.ReadOnly = false;
            txtTenDN.ReadOnly = false;
            txtMaSV.Clear();
            txtHoTen.Clear();
            txtTenDN.Clear();
            txtMatKhau.Clear();
            txtDiaChi.Clear();
            dtpNgaySinh.Value = DateTime.Today;
        }

        private void SetButtonStates(bool rowSelected)
        {
            btnAdd.Enabled = !rowSelected;
            btnUpdate.Enabled = rowSelected;
            btnDelete.Enabled = rowSelected;
        }
    }
}