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