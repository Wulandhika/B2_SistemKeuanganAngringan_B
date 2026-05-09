using System.Data;
using System.Data.SqlClient;

namespace SistemKeuanganAngkringan.Classes
{
    public static class DBHelper
    {
        // Koneksi ke database DBshop (username 10, password 10)
        private static string connectionString = @"Data Source=LAPTOP-07AAA94J\SQLEXPRESS;Initial Catalog=DBshop;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static void OpenConnection(SqlConnection conn)
        {
            if (conn == null)
                return;

            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }

        public static void CloseConnection(SqlConnection conn)
        {
            if (conn == null)
                return;

            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        // Method untuk cek status koneksi
        public static bool IsConnectionOpen(SqlConnection conn)
        {
            if (conn == null)
                return false;
            return conn.State == ConnectionState.Open;
        }

        // Method untuk cek status koneksi (string)
        public static string GetConnectionStatus(SqlConnection conn)
        {
            if (conn == null)
                return "Tidak Ada Koneksi";

            switch (conn.State)
            {
                case ConnectionState.Closed:
                    return "Tertutup";
                case ConnectionState.Open:
                    return "Terbuka";
                case ConnectionState.Connecting:
                    return "Sedang Menghubungkan";
                case ConnectionState.Executing:
                    return "Sedang Menjalankan Query";
                case ConnectionState.Fetching:
                    return "Sedang Mengambil Data";
                case ConnectionState.Broken:
                    return "Koneksi Rusak";
                default:
                    return "Tidak Diketahui";
            }
        }
    }
}