using System;

namespace program.Models
{
    public static class UserSession
    {
        public static string MaNV { get; set; } = string.Empty;
        public static string TenNV { get; set; } = string.Empty;
        public static string TenDN { get; set; } = string.Empty;
        public static string Password { get; set; } = string.Empty;
    }

    public class NhanVien
    {
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public byte[] Luong { get; set; }
        public string TenDN { get; set; }
        public byte[] MatKhau { get; set; }
        public string PubKey { get; set; }
    }

    public class SinhVien
    {
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string MaLop { get; set; }
        public string TenDN { get; set; }
        public byte[] MatKhau { get; set; }
    }
    public class Lop
    {
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public string MaNV { get; set; }
    }

    public class HocPhan
    {
        public string MaHP { get; set; }
        public string TenHP { get; set; }
        public int SoTC { get; set; }
    }

    public class BangDiem
    {
        public string MaSV { get; set; }
        public string MaHP { get; set; }
        public byte[] DiemThi { get; set; }
    }
}