-- ============================================================
-- DATABASE: DBshop
-- SISTEM KEUANGAN ANGKringAN
-- ============================================================

-- Hapus database lama jika ada
USE master;
GO

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
-- TABEL 1: ADMIN
-- ============================================================
CREATE TABLE admin (
    id_admin INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(10) NOT NULL UNIQUE,   -- 10 huruf (simpel)
    password VARCHAR(10) NOT NULL,          -- 10 huruf (mudah diingat)
    nama_admin VARCHAR(30) NOT NULL
);
GO

-- ============================================================
-- TABEL 2: MENU
-- ============================================================
CREATE TABLE menu (
    id_menu INT IDENTITY(1,1) PRIMARY KEY,
    nama_menu VARCHAR(100) NOT NULL,
    harga INT NOT NULL CHECK (harga >= 1000)  -- minimal Rp 1.000
);
GO

-- ============================================================
-- TABEL 3: TRANSAKSI
-- ============================================================
CREATE TABLE transaksi (
    id_transaksi INT IDENTITY(1,1) PRIMARY KEY,
    tanggal DATE NOT NULL DEFAULT GETDATE(),
    id_admin INT NOT NULL,
    total_harga INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_transaksi_admin FOREIGN KEY (id_admin) REFERENCES admin(id_admin)
);
GO

-- ============================================================
-- TABEL 4: DETAIL TRANSAKSI
-- ============================================================
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

-- Insert Admin (username 10 huruf, password 10 huruf)
INSERT INTO admin (username, password, nama_admin) VALUES 
('admin', 'admin123', 'Pemilik Angkringan');
GO

-- Insert Menu (harga ribuan, nama produk wajar)
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
SELECT '=== DATA ADMIN ===' as Keterangan;
SELECT id_admin, username, password, nama_admin FROM admin;
GO

SELECT '=== DATA MENU ===' as Keterangan;
SELECT id_menu, nama_menu, harga FROM menu ORDER BY id_menu;
GO

SELECT '=== TOTAL MENU ===' as Keterangan;
SELECT COUNT(*) as jumlah_menu FROM menu;
GO


USE DBshop;
GO

-- ============================================================
-- LANGKAH 1: MEMBUAT VIEW (Pembatas Akses Data)
-- ============================================================
-- View untuk menampilkan data menu (tanpa id_menu)
CREATE VIEW vwMenuPublic AS
SELECT 
    nama_menu AS NamaMenu,
    harga AS Harga
FROM menu;
GO

-- View untuk menampilkan data transaksi (tanpa id_admin)
CREATE VIEW vwTransaksiPublic AS
SELECT 
    id_transaksi,
    tanggal,
    total_harga
FROM transaksi;
GO

-- ============================================================
-- LANGKAH 2: MEMBUAT TABEL BACKUP (Untuk Reset Data)
-- ============================================================

-- Backup tabel menu
SELECT * INTO menu_backup FROM menu;
GO

-- Backup tabel transaksi
SELECT * INTO transaksi_backup FROM transaksi;
GO

-- Backup tabel detail_transaksi
SELECT * INTO detail_transaksi_backup FROM detail_transaksi;
GO

-- ============================================================
-- CEK HASIL VIEW
-- ============================================================
SELECT '=== VIEW MENU ===' as Keterangan;
SELECT * FROM vwMenuPublic;
GO

SELECT '=== VIEW TRANSAKSI ===' as Keterangan;
SELECT * FROM vwTransaksiPublic;
GO

SELECT '=== TABEL BACKUP ===' as Keterangan;
SELECT COUNT(*) as backup_menu FROM menu_backup;
SELECT COUNT(*) as backup_transaksi FROM transaksi_backup;
GO

USE DBshop;
GO

-- Hapus VIEW jika sudah ada
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vwMenuPublic')
    DROP VIEW vwMenuPublic;
GO

-- Buat VIEW ulang
CREATE VIEW vwMenuPublic AS
SELECT 
    nama_menu AS NamaMenu,
    harga AS Harga
FROM menu;
GO

-- Cek apakah VIEW berhasil
SELECT * FROM vwMenuPublic;
GO


USE DBshop;
GO

-- Lihat data yang bermasalah
SELECT * FROM detail_transaksi;
SELECT * FROM transaksi;
SELECT * FROM menu;
GO

-- Hapus semua data terkait
DELETE FROM detail_transaksi;
DELETE FROM transaksi;
DELETE FROM menu;
GO

-- Reset dari backup
INSERT INTO menu (nama_menu, harga)
SELECT nama_menu, harga FROM menu_backup;
GO

-- Cek hasil
SELECT * FROM menu;
GO

USE DBshop;
GO

-- Cek apakah CHECK CONSTRAINT masih ada
SELECT name, definition 
FROM sys.check_constraints 
WHERE parent_object_id = OBJECT_ID('menu');
GO

-- Jika CHECK CONSTRAINT masih ada, hapus dulu
DECLARE @constraintName NVARCHAR(200)
SELECT @constraintName = name FROM sys.check_constraints 
WHERE parent_object_id = OBJECT_ID('menu') 
AND definition LIKE '%harga%'
IF @constraintName IS NOT NULL
    EXEC('ALTER TABLE menu DROP CONSTRAINT ' + @constraintName)
GO

-- Cek data menu sebelum injection
SELECT 'SEBELUM INJECTION' as Keterangan;
SELECT id_menu, nama_menu, harga FROM menu;
GO

-- Demo SQL Injection (ubah semua nama menu jadi HACKED)
DECLARE @input NVARCHAR(100) = ' '' OR 1=1 -- '
DECLARE @query NVARCHAR(500)

SET @query = 'UPDATE menu SET nama_menu = ''HACKED'' WHERE nama_menu = ''' + @input + ''''
PRINT @query
EXEC sp_executesql @query
GO

-- Cek hasil setelah injection
SELECT 'SETELAH INJECTION (SEMUA JADI HACKED)' as Keterangan;
SELECT id_menu, nama_menu, harga FROM menu;
GO

-- Reset data dari backup
DELETE FROM menu;
INSERT INTO menu (nama_menu, harga)
SELECT nama_menu, harga FROM menu_backup;
GO

-- Cek setelah reset
SELECT 'SETELAH RESET (KEMBALI NORMAL)' as Keterangan;
SELECT id_menu, nama_menu, harga FROM menu;
GO


-- storage prosedur

USE DBshop;
GO

-- ============================================================
-- 1. STORED PROCEDURE SELECT (Semua Data Menu)
-- ============================================================
CREATE PROCEDURE sp_GetAllMenu
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_menu, nama_menu, harga FROM menu ORDER BY nama_menu;
END
GO

-- ============================================================
-- 2. STORED PROCEDURE SELECT BY ID (Parameter Input)
-- ============================================================
CREATE PROCEDURE sp_GetMenuById
    @IdMenu INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_menu, nama_menu, harga FROM menu WHERE id_menu = @IdMenu;
END
GO

-- ============================================================
-- 3. STORED PROCEDURE INSERT
-- ============================================================
CREATE PROCEDURE sp_InsertMenu
    @NamaMenu VARCHAR(100),
    @Harga INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO menu (nama_menu, harga) VALUES (@NamaMenu, @Harga);
    SELECT SCOPE_IDENTITY() AS IdMenu;
END
GO

-- ============================================================
-- 4. STORED PROCEDURE UPDATE
-- ============================================================
CREATE PROCEDURE sp_UpdateMenu
    @IdMenu INT,
    @NamaMenu VARCHAR(100),
    @Harga INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE menu SET nama_menu = @NamaMenu, harga = @Harga WHERE id_menu = @IdMenu;
END
GO

-- ============================================================
-- 5. STORED PROCEDURE DELETE
-- ============================================================
CREATE PROCEDURE sp_DeleteMenu
    @IdMenu INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM menu WHERE id_menu = @IdMenu;
END
GO

-- ============================================================
-- 6. STORED PROCEDURE COUNT (OUTPUT PARAMETER)
-- ============================================================
CREATE PROCEDURE sp_CountMenu
    @Total INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @Total = COUNT(*) FROM menu;
END
GO

-- ============================================================
-- 7. STORED PROCEDURE SEARCH (Pencarian)
-- ============================================================
CREATE PROCEDURE sp_SearchMenu
    @Keyword VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_menu, nama_menu, harga 
    FROM menu 
    WHERE nama_menu LIKE '%' + @Keyword + '%'
    ORDER BY nama_menu;
END
GO

-- ============================================================
-- CEK STORED PROCEDURE
-- ============================================================
SELECT '=== STORED PROCEDURE YANG TERSEDIA ===' as Info;
SELECT name, type_desc 
FROM sys.objects 
WHERE type = 'P' 
AND name LIKE 'sp_%'
ORDER BY name;
GO


USE DBshop;
GO

-- Cek data yang NULL
SELECT * FROM detail_transaksi WHERE id_menu IS NULL OR subtotal IS NULL;
SELECT * FROM transaksi WHERE id_admin IS NULL;
SELECT * FROM menu WHERE nama_menu IS NULL OR harga IS NULL;

-- Update data yang NULL
UPDATE menu SET harga = 1000 WHERE harga IS NULL;
UPDATE menu SET nama_menu = 'Menu Tidak Dikenal' WHERE nama_menu IS NULL;
UPDATE detail_transaksi SET subtotal = 0 WHERE subtotal IS NULL;
UPDATE detail_transaksi SET id_menu = 1 WHERE id_menu IS NULL;
GO


USE DBshop;
GO

-- ========== LANGKAH 1: HAPUS SEMUA DATA YG BERELASI ==========
-- Hapus detail transaksi terlebih dahulu
DELETE FROM detail_transaksi;
GO

-- Hapus transaksi
DELETE FROM transaksi;
GO

-- Hapus semua menu
DELETE FROM menu;
GO

-- ========== LANGKAH 2: RESET DARI BACKUP ==========
-- Insert ulang dari backup
INSERT INTO menu (nama_menu, harga)
SELECT nama_menu, harga FROM menu_backup;
GO

-- ========== LANGKAH 3: CEK HASIL ==========
SELECT '=== DATA MENU SETELAH RESET ===' as Info;
SELECT id_menu, nama_menu, harga FROM menu;
GO

SELECT '=== TOTAL MENU ===' as Info;
SELECT COUNT(*) as total FROM menu;
GO


USE DBshop;
GO

-- Lihat data duplikat
SELECT nama_menu, COUNT(*) as jumlah 
FROM menu 
GROUP BY nama_menu 
HAVING COUNT(*) > 1;
GO

-- Hapus semua data menu (reset total)
DELETE FROM detail_transaksi;
DELETE FROM transaksi;
DELETE FROM menu;
GO

-- Reset dari backup (tanpa duplikat)
INSERT INTO menu (nama_menu, harga)
SELECT DISTINCT nama_menu, 
    CASE WHEN harga < 1000 THEN 1000 ELSE harga END
FROM menu_backup
WHERE nama_menu IS NOT NULL AND nama_menu != '';
GO

-- Cek lagi
SELECT id_menu, nama_menu, harga FROM menu ORDER BY nama_menu;
GO







-- UCP2
USE DBshop;
GO

-- ============================================================
-- STORED PROCEDURE UNTUK FORM TRANSAKSI
-- ============================================================

-- SP untuk INSERT transaksi utama
CREATE OR ALTER PROCEDURE sp_InsertTransaksi
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

-- SP untuk INSERT detail transaksi
CREATE OR ALTER PROCEDURE sp_InsertDetailTransaksi
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

-- ============================================================
-- STORED PROCEDURE UNTUK FORM RIWAYAT
-- ============================================================

-- SP untuk mendapatkan transaksi berdasarkan tanggal
CREATE OR ALTER PROCEDURE sp_GetTransaksiByDate
    @Tanggal DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.id_transaksi, t.tanggal, a.nama_admin, t.total_harga 
    FROM transaksi t
    JOIN admin a ON t.id_admin = a.id_admin
    WHERE t.tanggal = @Tanggal
    ORDER BY t.id_transaksi DESC;
END
GO

-- SP untuk mendapatkan detail transaksi
CREATE OR ALTER PROCEDURE sp_GetDetailTransaksi
    @IdTransaksi INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.nama_menu, d.jumlah, d.subtotal 
    FROM detail_transaksi d
    JOIN menu m ON d.id_menu = m.id_menu
    WHERE d.id_transaksi = @IdTransaksi;
END
GO

-- ============================================================
-- STORED PROCEDURE UNTUK FORM PEMASUKAN
-- ============================================================

-- SP untuk mendapatkan total pemasukan berdasarkan tanggal
CREATE OR ALTER PROCEDURE sp_GetPemasukanByDate
    @Tanggal DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        COUNT(*) as JumlahTransaksi,
        ISNULL(SUM(total_harga), 0) as TotalPemasukan
    FROM transaksi 
    WHERE tanggal = @Tanggal;
END
GO

-- ============================================================
-- STORED PROCEDURE UNTUK FORM LOGIN (Opsional tapi baik)
-- ============================================================

-- SP untuk login admin
CREATE OR ALTER PROCEDURE sp_LoginAdmin
    @Username VARCHAR(10),
    @Password VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_admin, nama_admin FROM admin 
    WHERE username = @Username AND password = @Password;
END
GO

-- ============================================================
-- CEK SEMUA STORED PROCEDURE
-- ============================================================
SELECT '=== SEMUA STORED PROCEDURE ===' as Info;
SELECT name, type_desc 
FROM sys.objects 
WHERE type = 'P' 
AND name LIKE 'sp_%'
ORDER BY name;
GO

-- ============================================================
-- CEK SEMUA VIEW
-- ============================================================
SELECT '=== SEMUA VIEW ===' as Info;
SELECT name, type_desc 
FROM sys.objects 
WHERE type = 'V' 
AND name LIKE 'vw%'
ORDER BY name;
GO




USE DBshop;
GO

-- ============================================================
-- SP UNTUK FORM PEMASUKAN (Detail Transaksi per Tanggal)
-- ============================================================
CREATE OR ALTER PROCEDURE sp_GetTransaksiByDateForPemasukan
    @Tanggal DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.id_transaksi, t.tanggal, a.nama_admin, t.total_harga 
    FROM transaksi t
    JOIN admin a ON t.id_admin = a.id_admin
    WHERE t.tanggal = @Tanggal
    ORDER BY t.id_transaksi DESC;
END
GO

-- ============================================================
-- CEK SEMUA SP
-- ============================================================
SELECT '=== SEMUA STORED PROCEDURE ===' as Info;
SELECT name FROM sys.objects WHERE type = 'P' AND name LIKE 'sp_%' ORDER BY name;
GO




USE DBshop;
GO

ALTER PROCEDURE sp_GetAllMenu
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_menu, nama_menu, harga FROM menu ORDER BY nama_menu;
END
GO


USE DBshop;
GO

-- Hapus SP yang lama
DROP PROCEDURE IF EXISTS sp_GetAllMenu;
GO

-- Buat SP baru dengan kolom yang benar
CREATE PROCEDURE sp_GetAllMenu
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_menu, nama_menu, harga FROM menu ORDER BY nama_menu;
END
GO

-- Test SP
EXEC sp_GetAllMenu;
GO





USE DBshop;
GO

-- Hapus data yang bermasalah (nama_menu kosong atau harga 0)
DELETE FROM menu WHERE nama_menu IS NULL OR nama_menu = '' OR harga <= 0;
GO

-- Update harga yang NULL menjadi 1000
UPDATE menu SET harga = 1000 WHERE harga IS NULL OR harga = 0;
GO

-- Update nama_menu yang kosong
UPDATE menu SET nama_menu = 'Menu Tidak Dikenal' WHERE nama_menu IS NULL OR nama_menu = '';
GO

-- Cek hasil
SELECT id_menu, nama_menu, harga FROM menu ORDER BY id_menu;
GO




USE DBshop;
GO

-- Hapus data yang nama_menu kosong
DELETE FROM menu WHERE nama_menu IS NULL OR nama_menu = '' OR LTRIM(RTRIM(nama_menu)) = '';
GO

-- Update harga yang 0 menjadi 1000
UPDATE menu SET harga = 1000 WHERE harga <= 0 OR harga IS NULL;
GO

-- Cek hasil
SELECT id_menu, nama_menu, harga FROM menu ORDER BY nama_menu;
GO



USE DBshop;
GO

-- ========== HAPUS DATA KOSONG ==========
-- Hapus data yang nama_menu kosong atau hanya spasi
DELETE FROM menu WHERE nama_menu IS NULL OR LTRIM(RTRIM(nama_menu)) = '';
GO

-- Update harga yang 0 atau NULL menjadi 1000
UPDATE menu SET harga = 1000 WHERE harga IS NULL OR harga <= 0;
GO

-- ========== TAMBAHKAN CONSTRAINT UNTUK MENCEGAH DATA KOSONG ==========
-- Hapus constraint lama jika ada
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_menu_nama_menu_not_empty')
    ALTER TABLE menu DROP CONSTRAINT CK_menu_nama_menu_not_empty;
GO

-- Tambah constraint agar nama_menu tidak boleh kosong
ALTER TABLE menu ADD CONSTRAINT CK_menu_nama_menu_not_empty CHECK (LTRIM(RTRIM(nama_menu)) != '');
GO

-- Tambah constraint agar harga minimal 1000
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_menu_harga_min')
    ALTER TABLE menu DROP CONSTRAINT CK_menu_harga_min;
GO

ALTER TABLE menu ADD CONSTRAINT CK_menu_harga_min CHECK (harga >= 1000);
GO

-- ========== CEK HASIL ==========
SELECT '=== DATA MENU SETELAH BERSIH ===' as Keterangan;
SELECT id_menu, nama_menu, harga FROM menu ORDER BY nama_menu;
GO

SELECT '=== TOTAL MENU ===' as Keterangan;
SELECT COUNT(*) as Total FROM menu;
GO


USE DBshop;
GO

-- ========== HAPUS DATA KOSONG YANG SUDAH ADA ==========
DELETE FROM menu WHERE nama_menu IS NULL OR LTRIM(RTRIM(nama_menu)) = '';
GO

-- ========== HAPUS CONSTRAINT LAMA JIKA ADA ==========
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_menu_nama_menu_not_empty')
    ALTER TABLE menu DROP CONSTRAINT CK_menu_nama_menu_not_empty;
GO

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_menu_harga_min')
    ALTER TABLE menu DROP CONSTRAINT CK_menu_harga_min;
GO

-- ========== TAMBAH CONSTRAINT BARU ==========
-- Nama menu tidak boleh kosong dan minimal 2 karakter
ALTER TABLE menu ADD CONSTRAINT CK_menu_nama_menu_not_empty CHECK (LEN(LTRIM(RTRIM(nama_menu))) >= 2);
GO

-- Harga minimal 1000
ALTER TABLE menu ADD CONSTRAINT CK_menu_harga_min CHECK (harga >= 1000);
GO

-- ========== CEK HASIL ==========
SELECT '=== DATA MENU SETELAH BERSIH ===' as Keterangan;
SELECT id_menu, nama_menu, harga FROM menu ORDER BY nama_menu;
GO