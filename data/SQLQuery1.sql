
IF DB_ID('QLSVNhom') IS NOT NULL
BEGIN
    ALTER DATABASE QLSVNhom SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLSVNhom;
END
GO

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


-- i. Stored dùng để thêm mới dữ liệu (Insert) vào table NHANVIEN.
CREATE OR ALTER PROCEDURE SP_INS_PUBLIC_NHANVIEN
    @MANV VARCHAR(20),
    @HOTEN NVARCHAR(100),
    @EMAIL VARCHAR(20),
    @LUONGCB INT, 
    @TENDN NVARCHAR(100),
    @MK VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @SqlCreateKey NVARCHAR(MAX);
    -- vì RSA_512 không còn hỗ trợ tại các phiên bản hiện tại nên nhóm em sử dụng RSA_2048
    SET @SqlCreateKey = 'CREATE ASYMMETRIC KEY ' + QUOTENAME(@MANV) + 
                        ' WITH ALGORITHM = RSA_2048 ENCRYPTION BY PASSWORD = ''' + @MK + '''';
    
    BEGIN TRY
        EXEC sp_executesql @SqlCreateKey;
        DECLARE @LUONG_ENCRYPTED VARBINARY(MAX);
        SET @LUONG_ENCRYPTED = ENCRYPTBYASYMKEY(ASYMKEY_ID(@MANV), CAST(@LUONGCB AS NVARCHAR(50)));

        DECLARE @MK_HASHED VARBINARY(MAX);
        SET @MK_HASHED = HASHBYTES('SHA1', @MK);

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

-- ii.  Stored dùng để truy vấn dữ liệu nhân viên (NHANVIEN) 
CREATE OR ALTER PROCEDURE SP_SEL_PUBLIC_NHANVIEN
    @TENDN NVARCHAR(100),
    @MK NVARCHAR(MAX) 
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

----------------------
--LOP
----------------------
--Lấy toàn bộ danh sách các lớp học
CREATE OR ALTER PROCEDURE SP_SEL_LOP
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MALOP, TENLOP, MANV
    FROM LOP
    ORDER BY MALOP;
END;
GO
--Lấy danh sách các lớp do một nhân viên cụ thể quản lý.
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

--Thêm mới một lớp học.
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

--Cập nhật thông tin lớp học (chỉ nhân viên quản lý lớp đó mới có quyền).
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

--Xóa lớp học (kiểm tra quyền quản lý và điều kiện lớp không có sinh viên).
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
-----------------------
--SINHVIEN
-----------------------
--Lấy danh sách sinh viên theo mã lớp.
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

--Lấy danh sách sinh viên theo lớp nhưng có kiểm tra quyền quản lý của nhân viên.
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

--Cập nhật thông tin sinh viên (có kiểm tra quyền của nhân viên quản lý lớp).
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

--Thêm mới một sinh viên với mật khẩu được băm SHA1.
CREATE OR ALTER PROCEDURE SP_INS_SINHVIEN
    @MASV NVARCHAR(20),
    @HOTEN NVARCHAR(100),
    @NGAYSINH DATETIME,
    @DIACHI NVARCHAR(200),
    @MALOP NVARCHAR(20),
    @TENDN NVARCHAR(100),
    @MATKHAU VARCHAR(MAX) 
AS
BEGIN
    SET NOCOUNT ON;
    
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

--Xóa sinh viên 
CREATE OR ALTER PROCEDURE SP_DEL_SINHVIEN
    @MASV NVARCHAR(20),
    @MANV VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

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
    DELETE FROM BANGDIEM WHERE MASV = @MASV;
    DELETE FROM SINHVIEN WHERE MASV = @MASV;
    PRINT N'✅ Đã xóa sinh viên và dữ liệu điểm liên quan.';
END;
GO
----------------------
--BANGDIEM
----------------------
--Lấy danh sách tất cả các bản ghi trong bảng điểm.
CREATE OR ALTER PROCEDURE SP_SEL_BANGDIEM
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MASV, MAHP, DIEMTHI
    FROM BANGDIEM
    ORDER BY MASV, MAHP;
END;
GO

--Giải mã và hiển thị điểm thi của một sinh viên cụ thể (yêu cầu mật khẩu để truy cập Private Key)
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

--Thêm mới hoặc cập nhật điểm thi đã được mã hóa bằng Public Key (RSA_2048) của nhân viên
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

--------------------
--TEST
--------------------

-- 1. Thêm Nhân viên
EXEC SP_INS_PUBLIC_NHANVIEN 
    'NV03', 
    N'Trà Nguyễn Quang Thắng', 
    'thang@hcmus.edu.vn', 
    5000000, 
    'thang', 
    '123';
GO

-- 2. Thêm 20 Lớp học
DECLARE @i INT = 1;
WHILE @i <= 20
BEGIN
    DECLARE @maLop VARCHAR(20) = 'L' + RIGHT('0' + CAST(@i AS VARCHAR), 2);
    DECLARE @tenLop NVARCHAR(100) = N'Lớp Công nghệ thông tin ' + CAST(@i AS NVARCHAR);
    
    INSERT INTO LOP (MALOP, TENLOP, MANV) 
    VALUES (@maLop, @tenLop, 'NV04');
    
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




EXEC SP_INS_PUBLIC_NHANVIEN 
    'NV04', 
    N'Phạm Thanh Vương', 
    'vuong@hcmus.edu.vn', 
    5000000, 
    'vuong', 
    '123';
GO