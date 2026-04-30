using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using program.DataAccess;
using program.Helpers;
using program.Models;

namespace program.View
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                SqlParameter[] paras = {
                    new SqlParameter("@TENDN", username),
                    new SqlParameter("@MK", password) // MK dùng để giải mã Asymmetric Key [cite: 45]
                };

                // Gọi Stored Procedure đã tạo trong SQL
                DataTable dt = SqlDbContext.ExecuteQuery("SP_SEL_PUBLIC_NHANVIEN", paras);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string luongGiaiMa = Convert.ToString(dt.Rows[0]["LUONGCB"]) ?? string.Empty;

                    if (!string.IsNullOrEmpty(luongGiaiMa))
                    {
                        UserSession.MaNV = Convert.ToString(dt.Rows[0]["MANV"]) ?? string.Empty;
                        UserSession.TenNV = Convert.ToString(dt.Rows[0]["HOTEN"]) ?? string.Empty;
                        UserSession.TenDN = username;
                        UserSession.Password = password;

                        MessageBox.Show($"Đăng nhập thành công! Chào {dt.Rows[0]["HOTEN"]}");

                        frmMain fMain = new frmMain();
                        fMain.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Sai mật khẩu (Không thể giải mã dữ liệu)!");
                    }
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập không tồn tại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }
    }
}