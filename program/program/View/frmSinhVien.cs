using System;
using System.Data;
using System.Windows.Forms;
using program.DataAccess;

namespace program.View
{
    public partial class frmSinhVien : Form
    {
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Lấy mã lớp từ combobox và tải danh sách
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
                var dt = SqlDbContext.ExecuteQuery("SP_SEL_SINHVIEN_BY_LOP", new[] { new Microsoft.Data.SqlClient.SqlParameter("@MALOP", maLop) });
                // dgvSinhVien.DataSource = dt;
                MessageBox.Show($"Đã tải {dt.Rows.Count} sinh viên cho lớp {maLop}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách sinh viên: " + ex.Message);
            }
        }
    }
}
