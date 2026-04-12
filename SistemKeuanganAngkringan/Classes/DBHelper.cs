using System.Data.SqlClient;

namespace SistemKeuanganAngkringan.Classes
{
    public static class DBHelper
    {
        // Ganti dengan nama server SQL kamu!
        // Cara cek nama server: Buka SSMS, lihat di form login
        private static string connectionString = @"Data Source=LAPTOP-07AAA94J\SQLEXPRESS;Initial Catalog=DBAngkringan;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static void OpenConnection(SqlConnection conn)
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
        }

        public static void CloseConnection(SqlConnection conn)
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }
}

// CLASS DBHelper ini berfungsi untuk mengelola koneksi ke database SQL Server.
