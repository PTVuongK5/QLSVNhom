using System;
using System.Data;
using System.Windows.Forms;
using program.DataAccess;

namespace program.View
{
    public partial class frmNhapDiem : Form
    {
        public frmNhapDiem()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                var dt = SqlDbContext.ExecuteQuery("SP_SEL_BANGDIEM");
                // dgvBangDiem.DataSource = dt;
                MessageBox.Show($"Đã tải {dt.Rows.Count} bản ghi bảng điểm.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải bảng điểm: " + ex.Message);
            }
        }
    }
}
