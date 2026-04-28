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
CREATE PROCEDURE SP_INS_PUBLIC_NHANVIEN
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
CREATE PROCEDURE SP_SEL_PUBLIC_NHANVIEN
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

    -- BƯỚC 1: Kiểm tra và xóa khóa cũ nếu đã tồn tại để tránh lỗi "Already exists"
    IF EXISTS (SELECT * FROM sys.asymmetric_keys WHERE name = @MANV)
    BEGIN
        DECLARE @SqlDropKey NVARCHAR(MAX) = 'DROP ASYMMETRIC KEY ' + @KeyName;
        EXEC sp_executesql @SqlDropKey;
    END

    -- BƯỚC 2: Tạo câu lệnh SQL động với khoảng trắng và nháy đơn an toàn
    -- QUOTENAME(@MK, '''') sẽ tự bao bọc mật khẩu trong dấu nháy đơn và xử lý ký tự đặc biệt
    SET @SqlCreateKey = 'CREATE ASYMMETRIC KEY ' + @KeyName + 
                        ' WITH ALGORITHM = RSA_2048' + 
                        ' ENCRYPTION BY PASSWORD = ' + QUOTENAME(@MK, '''');

    BEGIN TRY
        -- Thực thi tạo khóa RSA_512 [cite: 34, 43]
        EXEC sp_executesql @SqlCreateKey;

        -- BƯỚC 3: Mã hóa lương bằng khóa vừa tạo [cite: 34, 43]
        DECLARE @LUONG_ENCRYPTED VARBINARY(MAX);
        SET @LUONG_ENCRYPTED = ENCRYPTBYASYMKEY(ASYMKEY_ID(@MANV), CAST(@LUONGCB AS NVARCHAR(50)));

        -- BƯỚC 4: Băm mật khẩu bằng SHA1 [cite: 34, 41]
        DECLARE @MK_HASHED VARBINARY(MAX);
        SET @MK_HASHED = HASHBYTES('SHA1', @MK);

        -- BƯỚC 5: Dọn dẹp dữ liệu cũ trong bảng NHANVIEN (nếu có) để Insert không bị trùng PK
        IF EXISTS (SELECT 1 FROM NHANVIEN WHERE MANV = @MANV)
            DELETE FROM NHANVIEN WHERE MANV = @MANV;

        INSERT INTO NHANVIEN (MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY)
        VALUES (@MANV, @HOTEN, @EMAIL, @LUONG_ENCRYPTED, @TENDN, @MK_HASHED, @MANV);

        PRINT '✅ Thành công: Đã tạo khóa ' + @KeyName + ' và thêm nhân viên ' + @MANV;
    END TRY
    BEGIN CATCH
        -- In lỗi chi tiết để bạn biết chính xác tại sao "Lỗi thực thi"
        DECLARE @Err NVARCHAR(MAX) = ERROR_MESSAGE();
        PRINT '❌ Lỗi thực thi: ' + @Err;
    END CATCH
END;
GO

EXEC SP_INS_PUBLIC_NHANVIEN 
    'NV01', 
    N'Nguyễn Thành Vương', 
    'vuong@hcmus.edu.vn', 
    5000000, 
    'vuong', 
    '123';