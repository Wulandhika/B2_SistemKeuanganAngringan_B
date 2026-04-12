-- CREATE DATABASE
CREATE DATABASE DBAngkringan;
GO

USE DBAngkringan;
GO

-- ==================== TABEL 1: ADMIN ====================
CREATE TABLE admin (
    id_admin INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(100) NOT NULL,
    nama_admin VARCHAR(100) NOT NULL
);
GO

-- ==================== TABEL 2: MENU ====================
CREATE TABLE menu (
    id_menu INT IDENTITY(1,1) PRIMARY KEY,
    nama_menu VARCHAR(100) NOT NULL,
    harga INT NOT NULL CHECK (harga > 0)
);
GO

-- ==================== TABEL 3: TRANSAKSI ====================
CREATE TABLE transaksi (
    id_transaksi INT IDENTITY(1,1) PRIMARY KEY,
    tanggal DATE NOT NULL DEFAULT GETDATE(),
    id_admin INT NOT NULL,
    total_harga INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_transaksi_admin FOREIGN KEY (id_admin) REFERENCES admin(id_admin)
);
GO

-- ==================== TABEL 4: DETAIL TRANSAKSI ====================
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

-- ==================== DATA SAMPLE ====================
-- Insert Admin
INSERT INTO admin (username, password, nama_admin) VALUES 
('admin', 'admin123', 'Pemilik Angkringan'),
('kasir1', 'kasir123', 'Kasir Utama');

-- Insert Menu Makanan & Minuman
INSERT INTO menu (nama_menu, harga) VALUES 
('Nasi Kucing', 5000),
('Sate Usus', 3000),
('Teh Hangat', 2000),
('Kopi Hitam', 3000),
('Susu Jahe', 4000),
('Indomie Goreng', 7000),
('Indomie Rebus', 7000),
('Pisang Goreng', 5000),
('Tahu Isi', 3000),
('Es Teh', 4000);

-- Insert Sample Transaksi
INSERT INTO transaksi (tanggal, id_admin, total_harga) VALUES 
('2026-04-12', 1, 25000),
('2026-04-12', 1, 15000);

-- Insert Sample Detail Transaksi
INSERT INTO detail_transaksi (id_transaksi, id_menu, jumlah, subtotal) VALUES 
(1, 1, 2, 10000),
(1, 2, 3, 9000),
(1, 3, 3, 6000),
(2, 4, 2, 6000),
(2, 6, 1, 7000),
(2, 5, 1, 4000);

-- Cek data
SELECT * FROM admin;
SELECT * FROM menu;
SELECT * FROM transaksi;
SELECT * FROM detail_transaksi;
GO

-- ==================== VIEW TOTAL PEMASUKAN HARIAN ====================
CREATE VIEW vw_pemasukan_harian AS
SELECT 
    tanggal,
    COUNT(*) as jumlah_transaksi,
    SUM(total_harga) as total_pemasukan
FROM transaksi
GROUP BY tanggal;
GO

-- ==================== STORED PROCEDURE ====================
-- SP untuk mencatat transaksi lengkap
CREATE PROCEDURE sp_insert_transaksi
    @tanggal DATE,
    @id_admin INT,
    @items NVARCHAR(MAX) -- format: id_menu:jumlah,id_menu:jumlah
AS
BEGIN
    DECLARE @id_transaksi INT;
    DECLARE @total_harga INT = 0;
    
    -- Insert ke transaksi dulu (total_harga 0 dulu)
    INSERT INTO transaksi (tanggal, id_admin, total_harga) 
    VALUES (@tanggal, @id_admin, 0);
    
    SET @id_transaksi = SCOPE_IDENTITY();
    
    -- Insert detail transaksi
    -- (Sederhananya, lebih mudah via aplikasi)
    
    SELECT @id_transaksi as id_transaksi;
END;
GO


USE DBAngkringan;
GO

-- Insert transaksi sample
INSERT INTO transaksi (tanggal, id_admin, total_harga) VALUES 
('2026-04-12', 1, 25000),
('2026-04-12', 1, 15000),
('2026-04-11', 1, 30000);

-- Insert detail transaksi
INSERT INTO detail_transaksi (id_transaksi, id_menu, jumlah, subtotal) VALUES 
(1, 1, 2, 10000),
(1, 2, 3, 9000),
(1, 3, 2, 6000),
(2, 4, 2, 6000),
(2, 6, 1, 7000),
(2, 5, 1, 4000),
(3, 1, 3, 15000),
(3, 4, 3, 9000),
(3, 8, 2, 10000);

-- Cek data
SELECT * FROM transaksi;
SELECT * FROM detail_transaksi;
GO