using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using program.DataAccess;
using program.Helpers;

namespace program.View
{
    public class frmLuongManager : Form
    {
        private Label lblManv;
        private TextBox txtManv;
        private Label lblLuong;
        private TextBox txtLuong;
        private Button btnSave;


        public frmLuongManager()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblManv = new Label();
            this.txtManv = new TextBox();
            this.lblLuong = new Label();
            this.txtLuong = new TextBox();
            this.btnSave = new Button();

            this.SuspendLayout();

            // lblManv
            this.lblManv.AutoSize = true;
            this.lblManv.Location = new System.Drawing.Point(12, 15);
            this.lblManv.Name = "lblManv";
            this.lblManv.Size = new System.Drawing.Size(56, 20);
            this.lblManv.Text = "Mã NV";

            // txtManv
            this.txtManv.Location = new System.Drawing.Point(120, 12);
            this.txtManv.Size = new System.Drawing.Size(260, 27);

            // lblLuong
            this.lblLuong.AutoSize = true;
            this.lblLuong.Location = new System.Drawing.Point(12, 60);
            this.lblLuong.Text = "Lương CB";

            // txtLuong
            this.txtLuong.Location = new System.Drawing.Point(120, 57);
            this.txtLuong.Size = new System.Drawing.Size(260, 27);

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(120, 100);
            this.btnSave.Size = new System.Drawing.Size(100, 34);
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += BtnSave_Click;

            // form
            this.ClientSize = new System.Drawing.Size(420, 160);
            this.Controls.Add(this.lblManv);
            this.Controls.Add(this.txtManv);
            this.Controls.Add(this.lblLuong);
            this.Controls.Add(this.txtLuong);
            this.Controls.Add(this.btnSave);
            this.Text = "Quản lý lương";
            this.StartPosition = FormStartPosition.CenterParent;

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string manv = txtManv.Text?.Trim() ?? string.Empty;
            string luongText = txtLuong.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(manv) || string.IsNullOrEmpty(luongText))
            {
                MessageBox.Show("Vui lòng nhập Mã NV và Lương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string pubKeyXml = "";
                var dt = SqlDbContext.ExecuteQuery("SP_SEL_PUBKEY_BY_MANV", new[] { new SqlParameter("@MANV", manv) });

                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["PUBKEY"] != DBNull.Value)
                {
                    pubKeyXml = dt.Rows[0]["PUBKEY"].ToString() ?? string.Empty;
                }

                if (string.IsNullOrEmpty(pubKeyXml))
                {
                    MessageBox.Show("Không tìm thấy Public Key cho nhân viên này. Hãy kiểm tra lại Mã NV!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] encLuong = SecurityHelper.EncryptRSAWithKey(luongText, pubKeyXml);

                var paras = new[]
                {
                    new SqlParameter("@MANV", manv),
                    new SqlParameter("@LUONG_ENC", encLuong)
                };

                SqlDbContext.ExecuteNonQueryStoredProcedure("SP_UPD_NHANVIEN_LUONG", paras);

                MessageBox.Show("Đã cập nhật lương thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtLuong.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể cập nhật lương: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}