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
        public frmSinhVien()
        {
            InitializeComponent();
            Load += frmSinhVien_Load;
            dgvSinhVien.CellEndEdit += dgvSinhVien_CellEndEdit;
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            LoadLopForUser();
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

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (cmbLop.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn lớp.");
                return;
            }

            string maLop = cmbLop.SelectedValue?.ToString() ?? cmbLop.SelectedItem.ToString();
            LoadStudentsByClass(maLop);
        }

        public void LoadStudentsByClass(string maLop)
        {
            try
            {
                var dt = SqlDbContext.ExecuteQuery("SP_SEL_SINHVIEN_BY_LOP_NV", new[]
                {
                    new SqlParameter("@MALOP", maLop),
                    new SqlParameter("@MANV", UserSession.MaNV)
                });

                dgvSinhVien.AutoGenerateColumns = true;
                dgvSinhVien.DataSource = dt;
                dgvSinhVien.AllowUserToAddRows = false;
                dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvSinhVien.MultiSelect = false;
                ConfigureStudentGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách sinh viên: " + ex.Message);
            }
        }

        private void ConfigureStudentGrid()
        {
            foreach (DataGridViewColumn col in dgvSinhVien.Columns)
            {
                if (string.Equals(col.Name, "MASV", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(col.Name, "MALOP", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(col.Name, "TENDN", StringComparison.OrdinalIgnoreCase))
                {
                    col.ReadOnly = true;
                }
            }
        }

        private void dgvSinhVien_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvSinhVien.Rows[e.RowIndex];
            if (row.IsNewRow) return;

            string maSv = row.Cells["MASV"]?.Value?.ToString();
            string maLop = row.Cells["MALOP"]?.Value?.ToString();
            string hoTen = row.Cells["HOTEN"]?.Value?.ToString();
            string diaChi = row.Cells["DIACHI"]?.Value?.ToString();

            if (string.IsNullOrWhiteSpace(maSv) || string.IsNullOrWhiteSpace(maLop))
            {
                return;
            }

            DateTime? ngaySinh = null;
            if (row.Cells["NGAYSINH"]?.Value != null && DateTime.TryParse(row.Cells["NGAYSINH"].Value.ToString(), out var parsedDate))
            {
                ngaySinh = parsedDate;
            }

            try
            {
                var paras = new[]
                {
                    new SqlParameter("@MASV", maSv),
                    new SqlParameter("@HOTEN", (object)hoTen ?? DBNull.Value),
                    new SqlParameter("@NGAYSINH", (object)ngaySinh ?? DBNull.Value),
                    new SqlParameter("@DIACHI", (object)diaChi ?? DBNull.Value),
                    new SqlParameter("@MALOP", maLop),
                    new SqlParameter("@MANV", UserSession.MaNV)
                };

                SqlDbContext.ExecuteNonQueryStoredProcedure("SP_UPD_SINHVIEN", paras);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể cập nhật sinh viên: " + ex.Message);
                LoadStudentsByClass(maLop);
            }
        }
    }
}
