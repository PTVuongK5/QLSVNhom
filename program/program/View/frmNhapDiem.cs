using Microsoft.Data.SqlClient;
using program.DataAccess;
using program.Helpers;
using program.Models;
using System;
using System.Data;
using System.Windows.Forms;

namespace program.View
{
    public partial class frmNhapDiem : Form
    {
        public frmNhapDiem()
        {
            InitializeComponent();
            btnSave.Click += btnSave_Click;
            btnLoad.Click += btnLoad_Click;
            dgvBangDiem.CellClick += dgvBangDiem_CellClick;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string maSv = txtMaSV.Text.Trim();
                if (string.IsNullOrWhiteSpace(maSv))
                {
                    MessageBox.Show("Vui lòng nhập mã sinh viên để tải bảng điểm.");
                    return;
                }

                LoadBangDiemByStudent(maSv);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải bảng điểm: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string maSv = txtMaSV.Text.Trim();
            string maHp = txtMaHP.Text.Trim();
            string diemText = txtDiem.Text.Trim();

            if (string.IsNullOrWhiteSpace(maSv) || string.IsNullOrWhiteSpace(maHp))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên và mã học phần.");
                return;
            }

            if (!decimal.TryParse(diemText, out var diemThi))
            {
                MessageBox.Show("Điểm không hợp lệ.");
                return;
            }

            if (diemThi < 0 || diemThi > 10)
            {
                MessageBox.Show("Điểm phải nằm trong khoảng 0 đến 10.");
                return;
            }

            try
            {
                string pubKeyXml = "";
                var dtNV = SqlDbContext.ExecuteQuery("SP_SEL_PUBKEY_BY_MANV", new[] { new SqlParameter("@MANV", UserSession.MaNV) });

                if (dtNV != null && dtNV.Rows.Count > 0 && dtNV.Rows[0]["PUBKEY"] != DBNull.Value)
                    pubKeyXml = dtNV.Rows[0]["PUBKEY"].ToString();

                if (string.IsNullOrEmpty(pubKeyXml))
                {
                    MessageBox.Show("Không tìm thấy Public Key để mã hóa!");
                    return;
                }

                byte[] encryptedDiem = SecurityHelper.EncryptRSAWithKey(diemThi.ToString(), pubKeyXml);

                var paras = new[]
                {
                    new SqlParameter("@MASV", maSv),
                    new SqlParameter("@MAHP", maHp),
                    new SqlParameter("@DIEM_ENC", encryptedDiem),
                    new SqlParameter("@MANV", UserSession.MaNV)
                };

                SqlDbContext.ExecuteNonQueryStoredProcedure("SP_UPSERT_BANGDIEM_ENC", paras);
                LoadBangDiemByStudent(maSv);
                MessageBox.Show("Đã lưu điểm.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lưu điểm: " + ex.Message);
            }
        }

        private void LoadBangDiemByStudent(string maSv)
        {
            var dt = SqlDbContext.ExecuteQuery("SP_SEL_BANGDIEM_BY_MASV", new[]
            {
                new SqlParameter("@MASV", maSv),
                new SqlParameter("@MANV", UserSession.MaNV),
                new SqlParameter("@MK", UserSession.Password)
            });
            if (dt != null && dt.Rows.Count > 0)
            {
                string keysFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Keys");
                string privKeyPath = Path.Combine(keysFolder, $"PrivateKey_{UserSession.MaNV}.enc");
                string privateKeyXml = "";

                if (File.Exists(privKeyPath))
                {
                    try
                    {
                        byte[] encryptedPrivKey = File.ReadAllBytes(privKeyPath);
                        privateKeyXml = SecurityHelper.DecryptAES(encryptedPrivKey, UserSession.Password);
                    }
                    catch { }
                }

                dt.Columns.Add("DIEM_TMP", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    if (row["DIEMTHI"] != DBNull.Value && !string.IsNullOrEmpty(privateKeyXml))
                    {
                        try
                        {
                            byte[] encDiem = (byte[])row["DIEMTHI"];
                            row["DIEM_TMP"] = SecurityHelper.DecryptRSA(encDiem, privateKeyXml);
                        }
                        catch
                        {
                            row["DIEM_TMP"] = "Lỗi giải mã";
                        }
                    }
                }

                dt.Columns.Remove("DIEMTHI");
                dt.Columns["DIEM_TMP"].ColumnName = "DIEMTHI";

            }

            dgvBangDiem.AutoGenerateColumns = true;
            dgvBangDiem.DataSource = dt;
            dgvBangDiem.AllowUserToAddRows = false;
            dgvBangDiem.ReadOnly = true;
            dgvBangDiem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBangDiem.MultiSelect = false;
        }

        private void dgvBangDiem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvBangDiem.Rows[e.RowIndex];
            if (row.IsNewRow) return;

            txtMaSV.Text = row.Cells["MASV"]?.Value?.ToString() ?? string.Empty;
            txtMaHP.Text = row.Cells["MAHP"]?.Value?.ToString() ?? string.Empty;
            txtDiem.Text = row.Cells["DIEMTHI"]?.Value?.ToString() ?? string.Empty;
        }
    }
}
