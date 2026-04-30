using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using program.DataAccess;
using program.Models;

namespace program.View
{
    public partial class frmLopHoc : Form
    {
        public frmLopHoc()
        {
            InitializeComponent();
            Load += frmLopHoc_Load;
            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            dgvLop.CellClick += dgvLop_CellClick;
        }

        private void frmLopHoc_Load(object sender, EventArgs e)
        {
            LoadLop();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadLop();
        }

        private void LoadLop()
        {
            try
            {
                DataTable dt = string.IsNullOrWhiteSpace(UserSession.MaNV)
                    ? SqlDbContext.ExecuteQuery("SP_SEL_LOP")
                    : SqlDbContext.ExecuteQuery("SP_SEL_LOP_BY_MANV", new[]
                    {
                        new SqlParameter("@MANV", UserSession.MaNV)
                    });

                dgvLop.AutoGenerateColumns = true;
                dgvLop.DataSource = dt;
                dgvLop.ReadOnly = true;
                dgvLop.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvLop.MultiSelect = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu lớp: " + ex.Message);
            }
        }

        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            PopulateFormFromRow(dgvLop.Rows[e.RowIndex]);
        }

        private void PopulateFormFromRow(DataGridViewRow row)
        {
            if (row == null || row.IsNewRow) return;
            txtMaLop.Text = row.Cells["MALOP"]?.Value?.ToString() ?? string.Empty;
            txtTenLop.Text = row.Cells["TENLOP"]?.Value?.ToString() ?? string.Empty;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string maLop = txtMaLop.Text.Trim();
            string tenLop = txtTenLop.Text.Trim();

            if (string.IsNullOrWhiteSpace(maLop) || string.IsNullOrWhiteSpace(tenLop))
            {
                MessageBox.Show("Vui lòng nhập mã lớp và tên lớp.");
                return;
            }

            try
            {
                var paras = new[]
                {
                    new SqlParameter("@MALOP", maLop),
                    new SqlParameter("@TENLOP", tenLop),
                    new SqlParameter("@MANV", UserSession.MaNV)
                };

                SqlDbContext.ExecuteNonQueryStoredProcedure("SP_INS_LOP", paras);
                LoadLop();
                MessageBox.Show("Đã thêm lớp.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm lớp: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string maLop = txtMaLop.Text.Trim();
            string tenLop = txtTenLop.Text.Trim();

            if (string.IsNullOrWhiteSpace(maLop) || string.IsNullOrWhiteSpace(tenLop))
            {
                MessageBox.Show("Vui lòng nhập mã lớp và tên lớp.");
                return;
            }

            try
            {
                var paras = new[]
                {
                    new SqlParameter("@MALOP", maLop),
                    new SqlParameter("@TENLOP", tenLop),
                    new SqlParameter("@MANV", UserSession.MaNV)
                };

                SqlDbContext.ExecuteNonQueryStoredProcedure("SP_UPD_LOP", paras);
                LoadLop();
                MessageBox.Show("Đã cập nhật lớp.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể cập nhật lớp: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string maLop = txtMaLop.Text.Trim();
            if (string.IsNullOrWhiteSpace(maLop))
            {
                MessageBox.Show("Vui lòng chọn lớp cần xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa lớp này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var paras = new[]
                {
                    new SqlParameter("@MALOP", maLop),
                    new SqlParameter("@MANV", UserSession.MaNV)
                };

                SqlDbContext.ExecuteNonQueryStoredProcedure("SP_DEL_LOP", paras);
                LoadLop();
                txtMaLop.Clear();
                txtTenLop.Clear();
                MessageBox.Show("Đã xóa lớp.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa lớp: " + ex.Message);
            }
        }
    }
}
