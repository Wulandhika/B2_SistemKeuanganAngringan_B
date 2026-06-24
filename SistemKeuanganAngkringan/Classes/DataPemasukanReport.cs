using System;

namespace SistemKeuanganAngkringan.Classes
{
    public class DataPemasukanReport
    {
        // Property untuk menyimpan data pemasukan per hari
        public DateTime Tanggal { get; set; }
        public int JumlahTransaksi { get; set; }
        public int TotalPemasukan { get; set; }
    }
}