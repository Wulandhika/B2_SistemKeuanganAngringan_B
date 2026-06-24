# Sistem Keuangan Angkringan

Aplikasi desktop berbasis Windows Forms untuk mencatat transaksi penjualan angkringan, mengelola menu, melihat riwayat transaksi, dan total pemasukan harian.

---

## Teknologi yang Digunakan

| Teknologi | Keterangan |
|-----------|------------|
| Bahasa Pemrograman | C# Windows Forms |
| Database | Microsoft SQL Server |
| Koneksi Database | ADO.NET (SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter) |
| Arsitektur | Connected & Disconnected |

---

## Fitur Aplikasi

| No | Fitur | Keterangan |
|----|-------|-------------|
| 1 | Login Admin | Autentikasi pengguna (username: admin, password: admin123) |
| 2 | Pencatatan Transaksi | Mencatat penjualan dengan keranjang belanja |
| 3 | Riwayat Transaksi | Lihat transaksi berdasarkan tanggal |
| 4 | Total Pemasukan Harian | Lihat jumlah transaksi dan total pendapatan |
| 5 | Kelola Menu | CRUD menu menggunakan Stored Procedure |
| 6 | SQL Injection Demo | Simulasi celah keamanan SQL Injection |

---

## Skenario SQL Injection

### Lokasi
Pada **FormMenu.cs** (Kelola Menu) → Tombol **"Test Injection"**

### Langkah-langkah Demo:

1. **Login** sebagai Admin
   - Username: `admin`
   - Password: `admin123`

2. Klik menu **"Kelola Menu"**

3. Klik tombol **"Test Injection"**

4. Pada kotak dialog yang muncul, masukkan kode berikut:



# Panduan Instalasi Aplikasi Sistem Keuangan Angkringan

## Persyaratan Sistem
- Windows 7/8/10/11
- .NET Framework 4.7.2
- SQL Server 2012 atau lebih baru

## Langkah Instalasi

### 1. Instal Database
1. Buka SQL Server Management Studio (SSMS)
2. Jalankan script SQL secara berurutan dari folder `Database/`:
   - 01_Create_Database.sql
   - 02_Views_And_Backup.sql
   - 03_Stored_Procedures.sql
   - 04_Stored_Procedures_Transaksi.sql
   - 05_Get_Log_Harga.sql
   - 06_Stored_Procedures_Report.sql

### 2. Instal Aplikasi
1. Jalankan file `ReyWuSETUP.exe`
2. Ikuti wizard instalasi
3. Aplikasi akan terinstall di `C:\Program Files\Angkringan ReyWu\`

### 3. Login
- Username: `admin`
- Password: `admin123`



# Panduan Penggunaan Aplikasi Sistem Keuangan Angkringan

## Login
1. Jalankan aplikasi dari shortcut desktop
2. Masukkan username: `admin`
3. Masukkan password: `admin123`
4. Klik Login

## Menu Utama
| Menu | Fungsi |
|------|--------|
| Pencatatan Transaksi | Input transaksi baru |
| Riwayat Transaksi | Lihat dan export transaksi |
| Total Pemasukan Harian | Lihat pemasukan per hari |
| Kelola Menu | Tambah, ubah, hapus menu |
| Dashboard | Grafik pemasukan |
| Laporan Transaksi | Cetak laporan |
| Import Excel | Import transaksi dari Excel |
| Logout | Keluar aplikasi |

## Fitur Export Excel
1. Buka Riwayat Transaksi
2. Pilih tanggal
3. Klik Export Excel
4. Pilih lokasi simpan

## Fitur Import Excel
1. Buka Riwayat Transaksi
2. Klik Import Excel
3. Pilih file Excel (.xlsx)
4. Klik Import ke Database



