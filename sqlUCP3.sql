-- FILE 1: 01_Create_Database.sql
-- ============================================================
-- DATABASE: DBshop
-- SISTEM KEUANGAN ANGKringAN
-- ============================================================

USE master;
GO

-- Hapus database lama jika ada
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'DBshop')
BEGIN
    ALTER DATABASE DBshop SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DBshop;
END
GO

-- Buat database baru
CREATE DATABASE DBshop;
GO

USE DBshop;
GO

-- ============================================================
-- TABEL-TABEL
-- ============================================================

-- 1. ADMIN
CREATE TABLE admin (
    id_admin INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(10) NOT NULL UNIQUE,
    password VARCHAR(10) NOT NULL,
    nama_admin VARCHAR(30) NOT NULL
);
GO

-- 2. MENU
CREATE TABLE menu (
    id_menu INT IDENTITY(1,1) PRIMARY KEY,
    nama_menu VARCHAR(100) NOT NULL,
    harga INT NOT NULL,
    CONSTRAINT CK_menu_harga_min CHECK (harga >= 1000 AND harga <= 20000),
    CONSTRAINT CK_menu_nama_not_empty CHECK (LEN(LTRIM(RTRIM(nama_menu))) >= 3)
);
GO

-- 3. LOG HARGA MENU
CREATE TABLE log_harga_menu (
    id_log INT IDENTITY(1,1) PRIMARY KEY,
    id_menu INT NOT NULL,
    nama_menu VARCHAR(100) NOT NULL,
    harga_lama INT NOT NULL,
    harga_baru INT NOT NULL,
    admin_id INT NOT NULL,
    tanggal DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_log_menu FOREIGN KEY (id_menu) REFERENCES menu(id_menu) ON DELETE CASCADE,
    CONSTRAINT FK_log_admin FOREIGN KEY (admin_id) REFERENCES admin(id_admin)
);
GO

-- 4. TRANSAKSI
CREATE TABLE transaksi (
    id_transaksi INT IDENTITY(1,1) PRIMARY KEY,
    tanggal DATE NOT NULL DEFAULT GETDATE(),
    id_admin INT NOT NULL,
    total_harga INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_transaksi_admin FOREIGN KEY (id_admin) REFERENCES admin(id_admin)
);
GO

-- 5. DETAIL TRANSAKSI
CREATE TABLE detail_transaksi (
    id_detail INT IDENTITY(1,1) PRIMARY KEY,
    id_transaksi INT NOT NULL,
    id_menu INT NOT NULL,
    jumlah INT NOT NULL CHECK (jumlah > 0),
    subtotal INT NOT NULL,
    CONSTRAINT FK_detail_transaksi FOREIGN KEY (id_transaksi) REFERENCES transaksi(id_transaksi) ON DELETE CASCADE,
    CONSTRAINT FK_detail_menu FOREIGN KEY (id_menu) REFERENCES menu(id_menu)
);
GO

-- ============================================================
-- DATA SAMPLE
-- ============================================================

INSERT INTO admin (username, password, nama_admin) VALUES 
('admin', 'admin123', 'Pemilik Angkringan');
GO

INSERT INTO menu (nama_menu, harga) VALUES 
('Nasi Kucing', 5000),
('Nasi Langgi', 8000),
('Sate Usus', 3000),
('Sate Telur Puyuh', 4000),
('Teh Hangat', 2000),
('Teh Es', 4000),
('Kopi Hitam', 3000),
('Kopi Susu', 5000),
('Susu Jahe', 4000),
('Jahe Hangat', 3000),
('Indomie Goreng', 7000),
('Indomie Rebus', 7000),
('Pisang Goreng', 5000),
('Tahu Isi', 3000),
('Tempe Mendoan', 4000),
('Es Jeruk', 5000),
('Es Kelapa Muda', 8000),
('Wedang Uwuh', 6000);
GO

-- ============================================================
-- CEK HASIL
-- ============================================================
SELECT 'DATA ADMIN' as [Info]; SELECT * FROM admin;
SELECT 'DATA MENU' as [Info]; SELECT * FROM menu ORDER BY nama_menu;
SELECT 'TOTAL MENU' as [Info]; SELECT COUNT(*) as Total FROM menu;
GO


-- FILE 2: 02_Views_And_Backup.sql
USE DBshop;
GO

-- ============================================================
-- VIEW (Untuk pembatasan akses data)
-- ============================================================

-- View untuk menampilkan menu publik (tanpa id_menu)
CREATE VIEW vwMenuPublic AS
SELECT 
    nama_menu AS NamaMenu,
    harga AS Harga
FROM menu
WHERE LTRIM(RTRIM(nama_menu)) != '' AND harga BETWEEN 1000 AND 20000;
GO

-- View untuk menampilkan transaksi publik (tanpa id_admin)
CREATE VIEW vwTransaksiPublic AS
SELECT 
    id_transaksi,
    tanggal,
    total_harga
FROM transaksi;
GO

-- ============================================================
-- TABEL BACKUP (Untuk Reset Data)
-- ============================================================

SELECT * INTO menu_backup FROM menu;
SELECT * INTO transaksi_backup FROM transaksi;
SELECT * INTO detail_transaksi_backup FROM detail_transaksi;
GO

-- ============================================================
-- CEK VIEW & BACKUP
-- ============================================================
SELECT 'VIEW MENU' as [Info]; SELECT * FROM vwMenuPublic;
SELECT 'VIEW TRANSAKSI' as [Info]; SELECT * FROM vwTransaksiPublic;
SELECT 'BACKUP' as [Info]; 
SELECT COUNT(*) as menu_backup FROM menu_backup;
SELECT COUNT(*) as transaksi_backup FROM transaksi_backup;
GO

-- FILE 3: 03_Stored_Procedures.sql
USE DBshop;
GO

-- ============================================================
-- STORED PROCEDURE UNTUK MENU
-- ============================================================

-- 1. GET ALL MENU
CREATE PROCEDURE sp_GetAllMenu
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_menu, nama_menu, harga 
    FROM menu 
    WHERE LTRIM(RTRIM(nama_menu)) != '' AND harga BETWEEN 1000 AND 20000
    ORDER BY nama_menu;
END
GO

-- 2. COUNT MENU
CREATE PROCEDURE sp_CountMenu
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM menu WHERE LTRIM(RTRIM(nama_menu)) != '' AND harga BETWEEN 1000 AND 20000;
END
GO

-- 3. CHECK MENU EXISTS (dengan OUTPUT parameter)
CREATE PROCEDURE sp_CheckMenuExists
    @NamaMenu NVARCHAR(100),
    @ExcludeId INT = 0,
    @Exists INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @Exists = COUNT(*) 
    FROM menu 
    WHERE LTRIM(RTRIM(nama_menu)) = LTRIM(RTRIM(@NamaMenu)) 
    AND id_menu != @ExcludeId;
END
GO

-- 4. GET HARGA BY ID
CREATE PROCEDURE sp_GetHargaMenuById
    @IdMenu INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT harga FROM menu WHERE id_menu = @IdMenu;
END
GO

-- 5. INSERT MENU
CREATE PROCEDURE sp_InsertMenu
    @NamaMenu NVARCHAR(100),
    @Harga INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO menu (nama_menu, harga) VALUES (@NamaMenu, @Harga);
    SELECT SCOPE_IDENTITY() AS IdMenu;
END
GO

-- 6. UPDATE MENU WITH LOG
CREATE PROCEDURE sp_UpdateMenuWithLog
    @IdMenu INT,
    @NamaMenu NVARCHAR(100),
    @HargaBaru INT,
    @AdminId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @HargaLama INT;
    DECLARE @NamaLama VARCHAR(100);
    
    -- Ambil data lama
    SELECT @HargaLama = harga, @NamaLama = nama_menu FROM menu WHERE id_menu = @IdMenu;
    
    IF @HargaLama IS NULL
    BEGIN
        SELECT 0 AS Result;
        RETURN;
    END
    
    -- Update menu
    UPDATE menu SET nama_menu = @NamaMenu, harga = @HargaBaru WHERE id_menu = @IdMenu;
    
    -- Catat ke log (jika ada perubahan)
    IF @HargaLama != @HargaBaru OR @NamaLama != @NamaMenu
    BEGIN
        INSERT INTO log_harga_menu (id_menu, nama_menu, harga_lama, harga_baru, admin_id, tanggal)
        VALUES (@IdMenu, @NamaLama, @HargaLama, @HargaBaru, @AdminId, GETDATE());
    END
    
    SELECT 1 AS Result;
END
GO

-- 7. DELETE MENU
CREATE PROCEDURE sp_DeleteMenu
    @IdMenu INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM menu WHERE id_menu = @IdMenu;
END
GO

-- 8. SEARCH MENU
CREATE PROCEDURE sp_SearchMenu
    @Keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_menu, nama_menu, harga 
    FROM menu 
    WHERE nama_menu LIKE '%' + @Keyword + '%' 
    AND harga BETWEEN 1000 AND 20000
    ORDER BY nama_menu;
END
GO

-- 9. RESET MENU DATA
CREATE PROCEDURE sp_ResetMenuData
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM detail_transaksi;
    DELETE FROM transaksi;
    DELETE FROM menu;
    
    INSERT INTO menu (nama_menu, harga)
    SELECT nama_menu, 
        CASE WHEN harga < 1000 THEN 1000 
             WHEN harga > 20000 THEN 20000 
             ELSE harga END
    FROM menu_backup;
END
GO

-- 10. GET ID MENU BY NAME (untuk FormTransaksi)
CREATE PROCEDURE sp_GetIdMenuByName
    @NamaMenu NVARCHAR(100),
    @IdMenu INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @IdMenu = id_menu 
    FROM menu 
    WHERE LTRIM(RTRIM(nama_menu)) = LTRIM(RTRIM(@NamaMenu));
    
    IF @IdMenu IS NULL SET @IdMenu = 0;
END
GO

-- ============================================================
-- CEK STORED PROCEDURE
-- ============================================================
SELECT 'STORED PROCEDURES' as [Info];
SELECT name FROM sys.objects WHERE type = 'P' AND name LIKE 'sp_%' ORDER BY name;
GO

-- FILE 4: 04_Stored_Procedures_Transaksi.sql
USE DBshop;
GO

-- ============================================================
-- STORED PROCEDURE UNTUK TRANSAKSI
-- ============================================================

-- 1. INSERT TRANSAKSI
CREATE PROCEDURE sp_InsertTransaksi
    @Tanggal DATE,
    @IdAdmin INT,
    @TotalHarga INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO transaksi (tanggal, id_admin, total_harga) 
    VALUES (@Tanggal, @IdAdmin, @TotalHarga);
    SELECT SCOPE_IDENTITY() AS IdTransaksi;
END
GO

-- 2. INSERT DETAIL TRANSAKSI
CREATE PROCEDURE sp_InsertDetailTransaksi
    @IdTransaksi INT,
    @IdMenu INT,
    @Jumlah INT,
    @Subtotal INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO detail_transaksi (id_transaksi, id_menu, jumlah, subtotal) 
    VALUES (@IdTransaksi, @IdMenu, @Jumlah, @Subtotal);
END
GO

-- 3. GET TRANSAKSI BY DATE (untuk FormRiwayat)
CREATE PROCEDURE sp_GetTransaksiByDate
    @Tanggal DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        t.id_transaksi,
        t.tanggal,
        a.nama_admin,
        t.total_harga
    FROM transaksi t
    INNER JOIN admin a ON t.id_admin = a.id_admin
    WHERE t.tanggal = @Tanggal
    ORDER BY t.id_transaksi DESC;
END
GO

-- 4. GET DETAIL TRANSAKSI (untuk FormRiwayat)
CREATE PROCEDURE sp_GetDetailTransaksi
    @IdTransaksi INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        m.nama_menu,
        d.jumlah,
        d.subtotal
    FROM detail_transaksi d
    INNER JOIN menu m ON d.id_menu = m.id_menu
    WHERE d.id_transaksi = @IdTransaksi;
END
GO

-- 5. GET PEMASUKAN BY DATE (untuk FormPemasukan)
CREATE PROCEDURE sp_GetPemasukanByDate
    @Tanggal DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        COUNT(*) AS JumlahTransaksi,
        ISNULL(SUM(total_harga), 0) AS TotalPemasukan
    FROM transaksi
    WHERE tanggal = @Tanggal;
END
GO

-- 6. GET TRANSAKSI BY DATE FOR PEMASUKAN
CREATE PROCEDURE sp_GetTransaksiByDateForPemasukan
    @Tanggal DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        t.id_transaksi,
        t.tanggal,
        a.nama_admin,
        t.total_harga
    FROM transaksi t
    INNER JOIN admin a ON t.id_admin = a.id_admin
    WHERE t.tanggal = @Tanggal
    ORDER BY t.id_transaksi DESC;
END
GO

-- ============================================================
-- CEK STORED PROCEDURE TRANSAKSI
-- ============================================================
SELECT 'STORED PROCEDURES TRANSAKSI' as [Info];
SELECT name FROM sys.objects WHERE type = 'P' AND name LIKE 'sp_%Transaksi%' ORDER BY name;
GO

-- FILE 5: 05_Get_Log_Harga.sql
USE DBshop;
GO

-- ============================================================
-- GET LOG HARGA MENU
-- ============================================================
CREATE PROCEDURE sp_GetLogHargaMenu
    @IdMenu INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        id_log,
        nama_menu,
        harga_lama,
        harga_baru,
        tanggal_perubahan,
        FORMAT(tanggal_perubahan, 'dd MMM yyyy HH:mm') as tanggal_format
    FROM log_harga_menu
    WHERE id_menu = @IdMenu
    ORDER BY tanggal_perubahan DESC;
END
GO

SELECT 'sp_GetLogHargaMenu' as [Stored Procedure];
GO

-- FIX LENGKAP (Jalankan Semua)
USE DBshop;
GO

-- ============================================
-- CEK STRUKTUR TABEL log_harga_menu
-- ============================================
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'log_harga_menu')
BEGIN
    -- Cek apakah kolom 'tanggal' ada
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('log_harga_menu') AND name = 'tanggal')
    BEGIN
        -- Tambahkan kolom 'tanggal' jika belum ada
        ALTER TABLE log_harga_menu ADD tanggal DATETIME DEFAULT GETDATE();
        PRINT '✅ Kolom tanggal berhasil ditambahkan!';
    END
    ELSE
    BEGIN
        PRINT '✅ Kolom tanggal sudah ada!';
    END
END
ELSE
BEGIN
    -- Buat tabel log_harga_menu dari awal
    CREATE TABLE log_harga_menu (
        id_log INT IDENTITY(1,1) PRIMARY KEY,
        id_menu INT NOT NULL,
        nama_menu VARCHAR(100) NOT NULL,
        harga_lama INT NOT NULL,
        harga_baru INT NOT NULL,
        admin_id INT NOT NULL,
        tanggal DATETIME DEFAULT GETDATE()
    );
    PRINT '✅ Tabel log_harga_menu berhasil dibuat!';
END
GO

-- ============================================
-- HAPUS SP LAMA
-- ============================================
DROP PROCEDURE IF EXISTS sp_GetLogHargaMenu;
GO

-- ============================================
-- BUAT SP_GetLogHargaMenu
-- ============================================
CREATE PROCEDURE sp_GetLogHargaMenu
    @IdMenu INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        id_log,
        nama_menu,
        harga_lama,
        harga_baru,
        tanggal AS tanggal_perubahan,
        FORMAT(tanggal, 'dd MMM yyyy HH:mm') AS tanggal_format
    FROM log_harga_menu
    WHERE id_menu = @IdMenu
    ORDER BY tanggal DESC;
END
GO

-- ============================================
-- CEK SP SUDAH TERBUAT
-- ============================================
SELECT 'sp_GetLogHargaMenu' as [Stored Procedure];
SELECT name FROM sys.objects WHERE type = 'P' AND name = 'sp_GetLogHargaMenu';
GO




-- TANGGAL 22 06 2026
USE DBshop;
GO

-- ============================================================
-- STORED PROCEDURE UNTUK REPORT TRANSAKSI
-- ============================================================

CREATE PROCEDURE sp_GetTransaksiReport
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        t.id_transaksi AS IdTransaksi,
        t.tanggal AS Tanggal,
        a.nama_admin AS NamaAdmin,
        t.total_harga AS TotalHarga,
        STUFF((
            SELECT ', ' + m.nama_menu + ' x' + CAST(d.jumlah AS VARCHAR) + ' = Rp' + CAST(d.subtotal AS VARCHAR)
            FROM detail_transaksi d
            INNER JOIN menu m ON d.id_menu = m.id_menu
            WHERE d.id_transaksi = t.id_transaksi
            FOR XML PATH('')
        ), 1, 2, '') AS DetailMenu
    FROM transaksi t
    INNER JOIN admin a ON t.id_admin = a.id_admin
    WHERE t.tanggal BETWEEN @StartDate AND @EndDate
    ORDER BY t.tanggal DESC, t.id_transaksi DESC;
END
GO

-- ============================================================
-- STORED PROCEDURE UNTUK REPORT PEMASUKAN
-- ============================================================

CREATE PROCEDURE sp_GetPemasukanReport
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        t.tanggal AS Tanggal,
        COUNT(*) AS JumlahTransaksi,
        SUM(t.total_harga) AS TotalPemasukan
    FROM transaksi t
    WHERE t.tanggal BETWEEN @StartDate AND @EndDate
    GROUP BY t.tanggal
    ORDER BY t.tanggal DESC;
END
GO

-- ============================================================
-- CEK SP SUDAH TERBUAT
-- ============================================================

SELECT 'STORED PROCEDURES REPORT' as [Info];
SELECT name FROM sys.objects WHERE type = 'P' AND name LIKE 'sp_%Report%' ORDER BY name;
GO


USE DBshop;
GO
SELECT name FROM sys.objects WHERE type = 'P' AND name LIKE 'sp_%Report%';
GO





USE DBshop;
GO

CREATE PROCEDURE sp_GetPemasukanPerBulan
    @Tahun INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        MONTH(tanggal) AS Bulan,
        DATENAME(MONTH, tanggal) AS NamaBulan,
        COUNT(*) AS JumlahTransaksi,
        SUM(total_harga) AS TotalPemasukan
    FROM transaksi
    WHERE YEAR(tanggal) = @Tahun
    GROUP BY MONTH(tanggal), DATENAME(MONTH, tanggal)
    ORDER BY MONTH(tanggal);
END
GO

-- CEK SP
SELECT name FROM sys.objects WHERE type = 'P' AND name = 'sp_GetPemasukanPerBulan';
GO



USE DBshop;
GO

-- HAPUS SP YANG LAMA DULU
DROP PROCEDURE IF EXISTS sp_GetPemasukanPerHari;
GO

-- BUAT ULANG SP PER HARI
CREATE PROCEDURE sp_GetPemasukanPerHari
    @Tanggal DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        DATEPART(HOUR, tanggal) AS Jam,
        CAST(DATEPART(HOUR, tanggal) AS VARCHAR) + ':00' AS Label,
        COUNT(*) AS JumlahTransaksi,
        SUM(total_harga) AS TotalPemasukan
    FROM transaksi
    WHERE CAST(tanggal AS DATE) = @Tanggal
    GROUP BY DATEPART(HOUR, tanggal)
    ORDER BY Jam;
END
GO

-- CEK SP SUDAH BENAR
SELECT name FROM sys.objects 
WHERE type = 'P' 
AND name IN ('sp_GetPemasukanPerHari', 'sp_GetPemasukanPerMinggu', 'sp_GetPemasukanPerBulan', 'sp_GetPemasukanPerTahun')
ORDER BY name;
GO




USE DBshop;
GO

-- HAPUS SP LAMA
DROP PROCEDURE IF EXISTS sp_GetPemasukanPerHari;
GO

-- BUAT SP HARIAN YANG BARU (tampilkan per menu terlaris)
CREATE PROCEDURE sp_GetPemasukanPerHari
    @Tanggal DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 10
        m.nama_menu AS Label,
        SUM(d.jumlah) AS JumlahTransaksi,
        SUM(d.subtotal) AS TotalPemasukan
    FROM transaksi t
    INNER JOIN detail_transaksi d ON t.id_transaksi = d.id_transaksi
    INNER JOIN menu m ON d.id_menu = m.id_menu
    WHERE CAST(t.tanggal AS DATE) = @Tanggal
    GROUP BY m.nama_menu
    ORDER BY TotalPemasukan DESC;
END
GO

-- CEK SP
SELECT name FROM sys.objects WHERE type = 'P' AND name = 'sp_GetPemasukanPerHari';
GO




USE DBshop;
GO

-- HAPUS SP LAMA
DROP PROCEDURE IF EXISTS sp_GetTransaksiReport;
GO

-- BUAT ULANG SP DENGAN NAMA FIELD YANG SAMA PERSIS DENGAN CLASS
CREATE PROCEDURE sp_GetTransaksiReport
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        t.id_transaksi AS IdTransaksi,
        t.tanggal AS Tanggal,
        a.nama_admin AS NamaAdmin,
        t.total_harga AS TotalHarga,
        STUFF((
            SELECT ', ' + m.nama_menu + ' x' + CAST(d.jumlah AS VARCHAR) + ' = Rp' + CAST(d.subtotal AS VARCHAR)
            FROM detail_transaksi d
            INNER JOIN menu m ON d.id_menu = m.id_menu
            WHERE d.id_transaksi = t.id_transaksi
            FOR XML PATH('')
        ), 1, 2, '') AS DetailMenu
    FROM transaksi t
    INNER JOIN admin a ON t.id_admin = a.id_admin
    WHERE t.tanggal BETWEEN @StartDate AND @EndDate
    ORDER BY t.tanggal DESC, t.id_transaksi DESC;
END
GO

-- CEK
SELECT name FROM sys.objects WHERE type = 'P' AND name = 'sp_GetTransaksiReport';
GO



-- TRIGGER
USE DBshop;
GO

-- ============================================================
-- 1. TABEL LogError (untuk menyimpan error aplikasi)
-- ============================================================
CREATE TABLE LogError (
    id_log INT IDENTITY(1,1) PRIMARY KEY,
    waktu DATETIME DEFAULT GETDATE(),
    pesan_error VARCHAR(MAX)
);
GO

-- ============================================================
-- 2. TABEL LogAktivitas (untuk mencatat aktivitas user)
-- ============================================================
CREATE TABLE LogAktivitas (
    id_log INT IDENTITY(1,1) PRIMARY KEY,
    aktivitas VARCHAR(200),
    waktu DATETIME DEFAULT GETDATE()
);
GO

-- ============================================================
-- 3. TABEL LogKeamanan (untuk monitoring keamanan)
-- ============================================================
CREATE TABLE LogKeamanan (
    id_log INT IDENTITY(1,1) PRIMARY KEY,
    aktivitas VARCHAR(200),
    jumlah_data INT,
    waktu DATETIME DEFAULT GETDATE()
);
GO

-- ============================================================
-- 4. TRIGGER INSERT untuk menu
-- ============================================================
CREATE TRIGGER trg_InsertMenu
ON menu
AFTER INSERT
AS
BEGIN
    INSERT INTO LogAktivitas (aktivitas, waktu)
    VALUES ('Tambah data menu', GETDATE());
END
GO

-- ============================================================
-- 5. TRIGGER DELETE untuk menu
-- ============================================================
CREATE TRIGGER trg_DeleteMenu
ON menu
AFTER DELETE
AS
BEGIN
    INSERT INTO LogAktivitas (aktivitas, waktu)
    VALUES ('Hapus data menu', GETDATE());
END
GO

-- ============================================================
-- 6. TRIGGER UPDATE (monitoring update massal)
-- ============================================================
CREATE TRIGGER trg_PreventMassUpdate
ON menu
AFTER UPDATE
AS
BEGIN
    DECLARE @jumlah INT;
    SELECT @jumlah = COUNT(*) FROM inserted;

    -- Jika update lebih dari 3 data (karena menu angkringan tidak banyak)
    IF @jumlah > 3
    BEGIN
        -- Simpan log keamanan
        INSERT INTO LogKeamanan (aktivitas, jumlah_data, waktu)
        VALUES ('WARNING: Update massal terdeteksi', @jumlah, GETDATE());

        -- Batalkan transaksi
        ROLLBACK TRANSACTION;

        -- Tampilkan pesan error
        RAISERROR('Update dibatalkan! Terlalu banyak data diubah.', 16, 1);
    END
END
GO

-- ============================================================
-- CEK SEMUA TABEL & TRIGGER
-- ============================================================
SELECT 'TABEL' as [Info];
SELECT name FROM sys.tables 
WHERE name IN ('LogError', 'LogAktivitas', 'LogKeamanan')
ORDER BY name;

SELECT 'TRIGGER' as [Info];
SELECT name FROM sys.triggers 
WHERE name IN ('trg_InsertMenu', 'trg_DeleteMenu', 'trg_PreventMassUpdate')
ORDER BY name;
GO



SELECT * FROM LogError ORDER BY id_log DESC;
SELECT * FROM LogAktivitas ORDER BY id_log DESC;
SELECT * FROM LogKeamanan ORDER BY id_log DESC;


-- CEK TABEL LOG (HARUS SUDAH ADA DARI MODUL 11)
USE DBshop;
GO

-- CEK TABEL LogError
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'LogError')
    PRINT '✅ Tabel LogError sudah ada'
ELSE
    PRINT '❌ Tabel LogError belum ada, jalankan script modul 11 dulu!'

-- CEK TABEL LogAktivitas
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'LogAktivitas')
    PRINT '✅ Tabel LogAktivitas sudah ada'
ELSE
    PRINT '❌ Tabel LogAktivitas belum ada, jalankan script modul 11 dulu!'

-- CEK TABEL LogKeamanan
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'LogKeamanan')
    PRINT '✅ Tabel LogKeamanan sudah ada'
ELSE
    PRINT '❌ Tabel LogKeamanan belum ada, jalankan script modul 11 dulu!'
GO



SELECT * FROM LogError ORDER BY id_log DESC;
SELECT * FROM LogAktivitas ORDER BY id_log DESC;



-- TEST INJECTION
USE DBshop;
GO

-- Nonaktifkan trigger
DISABLE TRIGGER trg_PreventMassUpdate ON menu;
GO

-- CEK trigger sudah nonaktif
SELECT name, is_disabled FROM sys.triggers WHERE name = 'trg_PreventMassUpdate';
GO

-- LANJUT INJECTION DI MENU


USE DBshop;
GO

-- Aktifkan trigger
ENABLE TRIGGER trg_PreventMassUpdate ON menu;
GO

-- CEK trigger sudah aktif
SELECT name, is_disabled FROM sys.triggers WHERE name = 'trg_PreventMassUpdate';
GO


-- CEK STATUS TRIGGER (Opsional)
SELECT name, is_disabled 
FROM sys.triggers 
WHERE name = 'trg_PreventMassUpdate';
GO