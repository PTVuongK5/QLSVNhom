using System;
using System.Data;
using System.Windows.Forms;
using program.DataAccess;

namespace program.View
{
    public partial class frmLopHoc : Form
    {
        public frmLopHoc()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                // Gọi stored procedure trả về danh sách lớp (tên SP có thể thay đổi theo DB của bạn)
                DataTable dt = SqlDbContext.ExecuteQuery("SP_SEL_LOP");
                // Nếu bạn tạo Designer cho frmLopHoc, chắc chắn có control dgvLop
                // Nếu chưa có, bạn có thể tạo control hoặc dùng MessageBox để kiểm tra
                // dgvLop.DataSource = dt;
                MessageBox.Show($"Đã tải {dt.Rows.Count} lớp.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu lớp: " + ex.Message);
            }
        }
    }
}
