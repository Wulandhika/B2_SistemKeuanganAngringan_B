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
