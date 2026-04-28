using program.Models;
using System;
using System.Windows.Forms;

namespace program.View
{
    public partial class frmMain : Form
    {
        private Form activeForm = null;
        public frmMain()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Sự kiện chạy khi Form vừa được mở lên
        private void frmMain_Load(object sender, EventArgs e)
        {
            lblStatus.Text = $"Nhân viên: {UserSession.TenNV} | Mã NV: {UserSession.MaNV} | Đang hoạt động";
        }

        /// <summary>
        /// Hàm mở một Form con vào Panel trung tâm (Tối ưu giao diện)
        /// </summary>
        private void OpenChildForm(Form childForm)
        {
            // Nếu có form đang mở thì đóng lại để giải phóng bộ nhớ
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None; // Xóa viền Form con
            childForm.Dock = DockStyle.Fill; // Lấp đầy Panel

            pnlContent.Controls.Add(childForm);
            pnlContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // --- XỬ LÝ SỰ KIỆN MENU ---

        private void mnuLopHoc_Click(object sender, EventArgs e)
        {
            // Mở form quản lý lớp học
            OpenChildForm(new frmLopHoc());
        }

        private void mnuSinhVien_Click(object sender, EventArgs e)
        {
            // Mở form quản lý sinh viên
            OpenChildForm(new frmSinhVien());
        }

        private void mnuNhapDiem_Click(object sender, EventArgs e)
        {
            // Mở form nhập điểm (tại đây có thể thêm mã hóa/giải mã theo yêu cầu)
            OpenChildForm(new frmNhapDiem());
        }

        private void mnuLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Xóa thông tin session (Tùy chọn)
                UserSession.MaNV = "";
                UserSession.Password = "";

                // Hiện lại màn hình đăng nhập
                frmLogin login = new frmLogin();
                login.Show();
                this.Hide(); // Ẩn form chính đi
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }
    }
}