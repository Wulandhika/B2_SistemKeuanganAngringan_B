using System;

namespace SistemKeuanganAngkringan.Classes
{
    public class DataTransaksiReport
    {
        public int IdTransaksi { get; set; }
        public DateTime Tanggal { get; set; }
        public string NamaAdmin { get; set; }
        public int TotalHarga { get; set; }
        public string DetailMenu { get; set; }
    }
}