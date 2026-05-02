CREATE DATABASE QLSVNhom;
GO
USE QLSVNhom;
GO

-- 1. XÓA BẢNG THEO THỨ TỰ 
IF OBJECT_ID('BANGDIEM', 'U') IS NOT NULL DROP TABLE BANGDIEM;
IF OBJECT_ID('SINHVIEN', 'U') IS NOT NULL DROP TABLE SINHVIEN;
IF OBJECT_ID('LOP', 'U') IS NOT NULL DROP TABLE LOP;
IF OBJECT_ID('HOCPHAN', 'U') IS NOT NULL DROP TABLE HOCPHAN;
IF OBJECT_ID('NHANVIEN', 'U') IS NOT NULL DROP TABLE NHANVIEN;
GO

-- 2. TẠO CÁC BẢNG


CREATE TABLE SINHVIEN (
    MASV NVARCHAR(20) PRIMARY KEY,
    HOTEN NVARCHAR(100) NOT NULL,
    NGAYSINH DATETIME,
    DIACHI NVARCHAR(200),
    MALOP NVARCHAR(20),
    TENDN NVARCHAR(100) NOT NULL UNIQUE,
    MATKHAU VARBINARY(MAX) NOT NULL
);


CREATE TABLE NHANVIEN (
    MANV VARCHAR(20) PRIMARY KEY,
    HOTEN NVARCHAR(100) NOT NULL,
    EMAIL VARCHAR(20),
    LUONG VARBINARY(MAX), 
    TENDN NVARCHAR(100) NOT NULL UNIQUE,
    MATKHAU VARBINARY(MAX) NOT NULL,
    PUBKEY VARCHAR(20)
);

CREATE TABLE LOP (
    MALOP VARCHAR(20) PRIMARY KEY,
    TENLOP NVARCHAR(100) NOT NULL,
    MANV VARCHAR(20)
);

CREATE TABLE HOCPHAN (
    MAHP VARCHAR(20) PRIMARY KEY,
    TENHP NVARCHAR(100) NOT NULL,
    SOTC INT
);

CREATE TABLE BANGDIEM (
    MASV VARCHAR(20),
    MAHP VARCHAR(20),
    DIEMTHI VARBINARY(MAX),
    PRIMARY KEY (MASV, MAHP)
);
GO


-- i.
CREATE OR ALTER PROCEDURE SP_INS_PUBLIC_NHANVIEN
    @MANV VARCHAR(20),
    @HOTEN NVARCHAR(100),
    @EMAIL VARCHAR(20),
    @LUONGCB INT, -- Giả sử lương cơ bản truyền vào là số nguyên
    @TENDN NVARCHAR(100),
    @MK VARCHAR(MAX) -- Mật khẩu chưa mã hóa truyền vào
AS
BEGIN
    SET NOCOUNT ON;
    -- Tên khóa trùng với Mã Nhân Viên (@MANV)
    -- Sử dụng thuật toán RSA_512 và bảo vệ bằng mật khẩu (@MK)
    DECLARE @SqlCreateKey NVARCHAR(MAX);
    SET @SqlCreateKey = 'CREATE ASYMMETRIC KEY ' + QUOTENAME(@MANV) + 
                        ' WITH ALGORITHM = RSA_512 ENCRYPTION BY PASSWORD = ''' + @MK + '''';
    
    BEGIN TRY
        EXEC sp_executesql @SqlCreateKey;
        -- Chuyển LUONGCB về kiểu chuỗi/binary để mã hóa
        DECLARE @LUONG_ENCRYPTED VARBINARY(MAX);
        SET @LUONG_ENCRYPTED = ENCRYPTBYASYMKEY(ASYMKEY_ID(@MANV), CAST(@LUONGCB AS NVARCHAR(50)));

        DECLARE @MK_HASHED VARBINARY(MAX);
        SET @MK_HASHED = HASHBYTES('SHA1', @MK);

        -- Chèn dữ liệu vào bảng NHANVIEN
        INSERT INTO NHANVIEN (MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY)
        VALUES (
            @MANV, 
            @HOTEN, 
            @EMAIL, 
            @LUONG_ENCRYPTED, 
            @TENDN, 
            @MK_HASHED, 
            @MANV
        );

        PRINT 'Thêm nhân viên và tạo khóa thành công.';
    END TRY
    BEGIN CATCH
        PRINT 'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

---ii)
CREATE OR ALTER PROCEDURE SP_SEL_PUBLIC_NHANVIEN
    @TENDN NVARCHAR(100),
    @MK NVARCHAR(MAX) -- Mật khẩu dùng để giải mã khóa bí mật
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        MANV, 
        HOTEN, 
        EMAIL,
        CAST(DECRYPTBYASYMKEY(ASYMKEY_ID(PUBKEY), LUONG, @MK) AS NVARCHAR(50)) AS LUONGCB
    FROM NHANVIEN
    WHERE TENDN = @TENDN;
END;
GO

CREATE OR ALTER PROCEDURE SP_SEL_LOP
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MALOP, TENLOP, MANV
    FROM LOP
    ORDER BY MALOP;
END;
GO

CREATE OR ALTER PROCEDURE SP_SEL_SINHVIEN_BY_LOP
    @MALOP NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN
    FROM SINHVIEN
    WHERE MALOP = @MALOP
    ORDER BY MASV;
END;
GO

CREATE OR ALTER PROCEDURE SP_SEL_BANGDIEM
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MASV, MAHP, DIEMTHI
    FROM BANGDIEM
    ORDER BY MASV, MAHP;
END;
GO

ALTER PROCEDURE SP_INS_PUBLIC_NHANVIEN
    @MANV VARCHAR(20),
    @HOTEN NVARCHAR(100),
    @EMAIL VARCHAR(20),
    @LUONGCB INT,
    @TENDN NVARCHAR(100),
    @MK NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @SqlCreateKey NVARCHAR(MAX);
    DECLARE @KeyName NVARCHAR(100) = QUOTENAME(@MANV);

    -- BƯỚC 1: Xóa khóa cũ nếu tồn tại
    IF EXISTS (SELECT * FROM sys.asymmetric_keys WHERE name = @MANV)
    BEGIN
        DECLARE @SqlDropKey NVARCHAR(MAX) = 'DROP ASYMMETRIC KEY ' + @KeyName;
        EXEC sp_executesql @SqlDropKey;
    END

    -- BƯỚC 2: Tạo khóa RSA_512
    -- Sử dụng QUOTENAME(@MK, '''') để bảo vệ chuỗi mật khẩu an toàn
    SET @SqlCreateKey = 'CREATE ASYMMETRIC KEY ' + @KeyName + 
                        ' WITH ALGORITHM = RSA_2048' + 
                        ' ENCRYPTION BY PASSWORD = ' + QUOTENAME(@MK, '''');

    BEGIN TRY
        EXEC sp_executesql @SqlCreateKey;

        -- BƯỚC 3: Mã hóa lương
        -- Ép kiểu về NVARCHAR(50) để khi giải mã dùng CAST ngược lại dễ dàng
        DECLARE @LUONG_ENCRYPTED VARBINARY(MAX);
        SET @LUONG_ENCRYPTED = ENCRYPTBYASYMKEY(ASYMKEY_ID(@MANV), CAST(@LUONGCB AS NVARCHAR(50)));

        -- BƯỚC 4: Băm mật khẩu bằng SHA1
        -- Ép về VARCHAR để đồng bộ băm chuỗi ASCII thông thường
        DECLARE @MK_HASHED VARBINARY(MAX);
        SET @MK_HASHED = HASHBYTES('SHA1', CAST(@MK AS VARCHAR(MAX)));

        -- BƯỚC 5: Cập nhật hoặc Thêm mới nhân viên
        IF EXISTS (SELECT 1 FROM NHANVIEN WHERE MANV = @MANV)
            DELETE FROM NHANVIEN WHERE MANV = @MANV;

        INSERT INTO NHANVIEN (MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY)
        VALUES (@MANV, @HOTEN, @EMAIL, @LUONG_ENCRYPTED, @TENDN, @MK_HASHED, @MANV);

        PRINT N'✅ Thành công: Đã tạo khóa và thêm nhân viên ' + @MANV;
    END TRY
    BEGIN CATCH
        DECLARE @Err NVARCHAR(MAX) = ERROR_MESSAGE();
        PRINT N'❌ Lỗi thực thi: ' + @Err;
    END CATCH
END;
GO

EXEC SP_INS_PUBLIC_NHANVIEN 
    'NV03', 
    N'Trà Nguyễn Quang Thắng', 
    'thang@hcmus.edu.vn', 
    5000000, 
    'thang', 
    '123';
GO

CREATE OR ALTER PROCEDURE SP_SEL_LOP_BY_MANV
    @MANV VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MALOP, TENLOP, MANV
    FROM LOP
    WHERE MANV = @MANV
    ORDER BY MALOP;
END;
GO

CREATE OR ALTER PROCEDURE SP_INS_LOP
    @MALOP VARCHAR(20),
    @TENLOP NVARCHAR(100),
    @MANV VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM LOP WHERE MALOP = @MALOP)
    BEGIN
        RAISERROR (N'Mã lớp đã tồn tại.', 16, 1);
        RETURN;
    END

    INSERT INTO LOP (MALOP, TENLOP, MANV)
    VALUES (@MALOP, @TENLOP, @MANV);
END;
GO

CREATE OR ALTER PROCEDURE SP_UPD_LOP
    @MALOP VARCHAR(20),
    @TENLOP NVARCHAR(100),
    @MANV VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM LOP WHERE MALOP = @MALOP AND MANV = @MANV)
    BEGIN
        RAISERROR (N'Không có quyền cập nhật lớp này.', 16, 1);
        RETURN;
    END

    UPDATE LOP
    SET TENLOP = @TENLOP
    WHERE MALOP = @MALOP AND MANV = @MANV;
END;
GO

CREATE OR ALTER PROCEDURE SP_DEL_LOP
    @MALOP VARCHAR(20),
    @MANV VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM LOP WHERE MALOP = @MALOP AND MANV = @MANV)
    BEGIN
        RAISERROR (N'Không có quyền xóa lớp này.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM SINHVIEN WHERE MALOP = @MALOP)
    BEGIN
        RAISERROR (N'Không thể xóa lớp đã có sinh viên.', 16, 1);
        RETURN;
    END

    DELETE FROM LOP
    WHERE MALOP = @MALOP AND MANV = @MANV;
END;
GO

CREATE OR ALTER PROCEDURE SP_SEL_SINHVIEN_BY_LOP_NV
    @MALOP NVARCHAR(20),
    @MANV VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM LOP WHERE MALOP = @MALOP AND MANV = @MANV)
    BEGIN
        RAISERROR (N'Không có quyền xem lớp này.', 16, 1);
        RETURN;
    END

    SELECT MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN
    FROM SINHVIEN
    WHERE MALOP = @MALOP
    ORDER BY MASV;
END;
GO

CREATE OR ALTER PROCEDURE SP_UPD_SINHVIEN
    @MASV NVARCHAR(20),
    @HOTEN NVARCHAR(100),
    @NGAYSINH DATETIME = NULL,
    @DIACHI NVARCHAR(200) = NULL,
    @MALOP NVARCHAR(20),
    @MANV VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM SINHVIEN sv
        INNER JOIN LOP l ON sv.MALOP = l.MALOP
        WHERE sv.MASV = @MASV AND l.MANV = @MANV
    )
    BEGIN
        RAISERROR (N'Không có quyền cập nhật sinh viên này.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM LOP WHERE MALOP = @MALOP AND MANV = @MANV)
    BEGIN
        RAISERROR (N'Lớp đích không thuộc quyền quản lý.', 16, 1);
        RETURN;
    END

    UPDATE SINHVIEN
    SET HOTEN = @HOTEN,
        NGAYSINH = @NGAYSINH,
        DIACHI = @DIACHI,
        MALOP = @MALOP
    WHERE MASV = @MASV;
END;
GO

CREATE OR ALTER PROCEDURE SP_SEL_BANGDIEM_BY_MASV
    @MASV VARCHAR(20),
    @MANV VARCHAR(20),
    @MK NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM SINHVIEN sv
        INNER JOIN LOP l ON sv.MALOP = l.MALOP
        WHERE sv.MASV = @MASV AND l.MANV = @MANV
    )
    BEGIN
        RAISERROR (N'Không có quyền xem bảng điểm của sinh viên này.', 16, 1);
        RETURN;
    END

    SELECT bd.MASV,
           bd.MAHP,
           CAST(DECRYPTBYASYMKEY(ASYMKEY_ID(@MANV), bd.DIEMTHI, @MK) AS NVARCHAR(20)) AS DIEMTHI
    FROM BANGDIEM bd
    WHERE bd.MASV = @MASV
    ORDER BY bd.MAHP;
END;
GO

CREATE OR ALTER PROCEDURE SP_UPSERT_BANGDIEM_ENC
    @MASV VARCHAR(20),
    @MAHP VARCHAR(20),
    @DIEMTHI DECIMAL(5, 2),
    @MANV VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM SINHVIEN sv
        INNER JOIN LOP l ON sv.MALOP = l.MALOP
        WHERE sv.MASV = @MASV AND l.MANV = @MANV
    )
    BEGIN
        RAISERROR (N'Không có quyền nhập điểm cho sinh viên này.', 16, 1);
        RETURN;
    END

    DECLARE @DIEM_ENC VARBINARY(MAX);
    SET @DIEM_ENC = ENCRYPTBYASYMKEY(ASYMKEY_ID(@MANV), CAST(@DIEMTHI AS NVARCHAR(20)));

    IF EXISTS (SELECT 1 FROM BANGDIEM WHERE MASV = @MASV AND MAHP = @MAHP)
    BEGIN
        UPDATE BANGDIEM
        SET DIEMTHI = @DIEM_ENC
        WHERE MASV = @MASV AND MAHP = @MAHP;
    END
    ELSE
    BEGIN
        INSERT INTO BANGDIEM (MASV, MAHP, DIEMTHI)
        VALUES (@MASV, @MAHP, @DIEM_ENC);
    END
END;
GO

--EXEC SP_SEL_PUBLIC_NHANVIEN 'vuong', '123';
--GO

-- 2. Thêm 20 Lớp học
DECLARE @i INT = 1;
WHILE @i <= 20
BEGIN
    DECLARE @maLop VARCHAR(20) = 'L' + RIGHT('0' + CAST(@i AS VARCHAR), 2);
    DECLARE @tenLop NVARCHAR(100) = N'Lớp Công nghệ thông tin ' + CAST(@i AS NVARCHAR);
    
    INSERT INTO LOP (MALOP, TENLOP, MANV) 
    VALUES (@maLop, @tenLop, 'NV01');
    
    SET @i = @i + 1;
END;
GO

-- 3. Thêm 15 Sinh viên cho mỗi lớp (Tổng 300 SV)
DECLARE @l INT = 1; -- Biến chạy cho Lớp
WHILE @l <= 20
BEGIN
    DECLARE @maLopSV VARCHAR(20) = 'L' + RIGHT('0' + CAST(@l AS VARCHAR), 2);
    DECLARE @s INT = 1; -- Biến chạy cho Sinh viên trong mỗi lớp
    
    WHILE @s <= 15
    BEGIN
        -- Tạo mã SV duy nhất: ví dụ SV0115 (Lớp 01, SV 15)
        DECLARE @maSV NVARCHAR(20) = 'SV' + RIGHT('0' + CAST(@l AS VARCHAR), 2) + RIGHT('0' + CAST(@s AS VARCHAR), 2);
        DECLARE @tenSV NVARCHAR(100) = N'Sinh viên ' + CAST(@maSV AS NVARCHAR);
        DECLARE @tenDN NVARCHAR(100) = 'user' + @maSV;

        INSERT INTO SINHVIEN (MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN, MATKHAU)
        VALUES (
            @maSV, 
            @tenSV, 
            '2005-01-01', 
            N'TP. Hồ Chí Minh', 
            @maLopSV, 
            @tenDN, 
            HASHBYTES('SHA1', 'password123') -- Mật khẩu mặc định băm SHA1
        );
        
        SET @s = @s + 1;
    END;
    
    SET @l = @l + 1;
END;
GO

-- 4. Thêm một vài Học phần mẫu
INSERT INTO HOCPHAN (MAHP, TENHP, SOTC) VALUES ('HP01', N'Cơ sở dữ liệu', 4);
INSERT INTO HOCPHAN (MAHP, TENHP, SOTC) VALUES ('HP02', N'An toàn thông tin', 3);
GO

--

CREATE OR ALTER PROCEDURE SP_INS_SINHVIEN
    @MASV NVARCHAR(20),
    @HOTEN NVARCHAR(100),
    @NGAYSINH DATETIME,
    @DIACHI NVARCHAR(200),
    @MALOP NVARCHAR(20),
    @TENDN NVARCHAR(100),
    @MATKHAU VARCHAR(MAX) -- Mật khẩu thô từ ứng dụng
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Kiểm tra trùng mã sinh viên hoặc tên đăng nhập
    IF EXISTS (SELECT 1 FROM SINHVIEN WHERE MASV = @MASV OR TENDN = @TENDN)
    BEGIN
        RAISERROR (N'Mã sinh viên hoặc Tên đăng nhập đã tồn tại.', 16, 1);
        RETURN;
    END

    INSERT INTO SINHVIEN (MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN, MATKHAU)
    VALUES (
        @MASV, 
        @HOTEN, 
        @NGAYSINH, 
        @DIACHI, 
        @MALOP, 
        @TENDN, 
        HASHBYTES('SHA1', CAST(@MATKHAU AS VARCHAR(MAX))) -- Băm SHA1
    );
    
    PRINT N'✅ Thêm sinh viên thành công.';
END;
GO

CREATE OR ALTER PROCEDURE SP_DEL_SINHVIEN
    @MASV NVARCHAR(20),
    @MANV VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra quyền quản lý của nhân viên đối với sinh viên này
    IF NOT EXISTS (
        SELECT 1 
        FROM SINHVIEN sv
        JOIN LOP l ON sv.MALOP = l.MALOP
        WHERE sv.MASV = @MASV AND l.MANV = @MANV
    )
    BEGIN
        RAISERROR (N'Không có quyền xóa sinh viên này (Không thuộc lớp quản lý).', 16, 1);
        RETURN;
    END

    -- Xóa bảng điểm trước để tránh lỗi Foreign Key
    DELETE FROM BANGDIEM WHERE MASV = @MASV;

    -- Xóa sinh viên
    DELETE FROM SINHVIEN WHERE MASV = @MASV;

    PRINT N'✅ Đã xóa sinh viên và dữ liệu điểm liên quan.';
END;
GO

CREATE OR ALTER PROCEDURE SP_SEL_SINHVIEN_BY_MASV
    @MASV    NVARCHAR(20),
    @MANV    VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
 
    SELECT sv.MASV, sv.HOTEN, sv.NGAYSINH, sv.DIACHI, sv.MALOP, sv.TENDN
    FROM   SINHVIEN sv
    INNER JOIN LOP l ON sv.MALOP = l.MALOP
    WHERE  l.MANV   = @MANV
      AND  sv.MASV  LIKE '%' + @MASV + '%'
    ORDER BY sv.MASV;
END;
GO
