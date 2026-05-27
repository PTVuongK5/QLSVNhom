using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; // Thêm thư viện này để xử lý File/Directory
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using program.DataAccess;
using program.Helpers;
using program.Models;

namespace program.View
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string manv = txtMaNV.Text.Trim();
            if (string.IsNullOrWhiteSpace(manv))
            {
                MessageBox.Show("Vui lòng nhập Mã NV.");
                return;
            }

            // Kiểm tra format cơ bản: chỉ cho phép chữ số và chữ hoa thường, không cho khoảng trắng
            if (manv.Contains(" ") || manv.Length > 50)
            {
                MessageBox.Show("Mã NV không hợp lệ.");
                return;
            }

            // Kiểm tra MANV đã tồn tại trong DB
            try
            {
                var dtCheck = SqlDbContext.ExecuteQuery("SP_SEL_NHANVIEN_BY_MANV", new[] { new SqlParameter("@MANV", manv) });
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    MessageBox.Show("Mã NV đã tồn tại, vui lòng chọn Mã khác.");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kiểm tra Mã NV: " + ex.Message);
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string username = txtUser.Text.Trim();
            string password = txtPass.Text;

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ họ tên, tài khoản và mật khẩu.");
                return;
            }

            try
            {
                // MATKHAU: hash bằng SHA1
                byte[] hashedPass = SecurityHelper.HashSHA1(password);

                // LUONG: mã hóa từ LUONGCB bằng RSA 2048
                string luongCbText = txtLuongCB?.Text?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(luongCbText))
                    luongCbText = "0";

                // Tạo cặp khóa RSA trên client, mã hóa LUONGCB bằng public key sinh ra
                var rsaResult = SecurityHelper.EncryptRSA(luongCbText);
                byte[] luongEncrypted = rsaResult.encryptedData;
                string pubKeyXml = rsaResult.publicKey;
                string privateKeyXml = rsaResult.privateKey; // Giữ lại private key để lưu ra file

                var paras = new[]
                {
                    new SqlParameter("@MANV", manv),
                    new SqlParameter("@HOTEN", fullName),
                    new SqlParameter("@EMAIL", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email),
                    new SqlParameter("@LUONG", luongEncrypted),
                    new SqlParameter("@TENDN", username),
                    new SqlParameter("@MK", hashedPass),
                    new SqlParameter("@PUB", string.IsNullOrWhiteSpace(pubKeyXml) ? (object)DBNull.Value : pubKeyXml)
                };

                // 1. Lưu thông tin (kèm Public Key) xuống CSDL
                SqlDbContext.ExecuteNonQueryStoredProcedure("SP_INS_PUBLIC_ENCRYPT_NHANVIEN", paras);

                string keysFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Keys");
                if (!Directory.Exists(keysFolder))
                {
                    Directory.CreateDirectory(keysFolder);
                }

                // Đặt tên file theo định dạng PrivateKey_MANV.xml
                string privateKeyPath = Path.Combine(keysFolder, $"PrivateKey_{manv}.enc");

                // 1. Dùng mật khẩu người dùng nhập để mã hóa chuỗi XML của Private Key
                byte[] encryptedPrivateKey = SecurityHelper.EncryptAES(privateKeyXml, password);

                // 2. Ghi mảng byte đã mã hóa ra file
                File.WriteAllBytes(privateKeyPath, encryptedPrivateKey);

                MessageBox.Show($"Tạo tài khoản thành công.\nKhóa bí mật đã được mã hóa an toàn và lưu tại:\n{privateKeyPath}\n\nVui lòng không xóa file này!",
                                "Thông báo hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tạo tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}