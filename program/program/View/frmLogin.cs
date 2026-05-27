using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
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

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using var reg = new frmRegister();
            var result = reg.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                MessageBox.Show("Đăng ký hoàn tất. Vui lòng đăng nhập bằng tài khoản mới.");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text?.Trim() ?? string.Empty;
            string password = txtPass.Text ?? string.Empty;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                byte[] hashedPass = SecurityHelper.HashSHA1(password);

                SqlParameter[] paras = {
                    new SqlParameter("@TENDN", username),
                    new SqlParameter("@MK", hashedPass)
                };

                DataTable? dt = SqlDbContext.ExecuteQuery("SP_SEL_PUBLIC_ENCRYPT_NHANVIEN", paras);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string manv = dt.Rows[0]["MANV"]?.ToString() ?? string.Empty;
                    string hoten = dt.Rows[0]["HOTEN"]?.ToString() ?? string.Empty;

                    string luongGiaiMa = "";

                    if (dt.Rows[0]["LUONG"] != DBNull.Value)
                    {
                        byte[] luongEncrypted = (byte[])dt.Rows[0]["LUONG"];

                        string keysFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Keys");
                        string privateKeyPath = Path.Combine(keysFolder, $"PrivateKey_{manv}.xml");

                        if (File.Exists(privateKeyPath))
                        {
                            // Đọc Private Key và giải mã
                            string privateKeyXml = File.ReadAllText(privateKeyPath);
                            luongGiaiMa = SecurityHelper.DecryptRSA(luongEncrypted, privateKeyXml) ?? "Không thể giải mã";
                        }
                        else
                        {
                            MessageBox.Show($"Cảnh báo: Không tìm thấy file Khóa bí mật (PrivateKey_{manv}.xml) trên máy này. Hệ thống không thể giải mã lương của bạn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            luongGiaiMa = "Không thể giải mã";
                        }
                    }

                    UserSession.MaNV = manv;
                    UserSession.TenNV = hoten;
                    UserSession.TenDN = username;
                    UserSession.Password = password;

                    MessageBox.Show($"Đăng nhập thành công!\nChào {hoten}.\nLương CB của bạn: {luongGiaiMa}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmMain fMain = new frmMain();
                    fMain.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}