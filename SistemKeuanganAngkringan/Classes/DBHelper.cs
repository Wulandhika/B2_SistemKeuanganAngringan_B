using System.Data;
using System.Data.SqlClient;

namespace SistemKeuanganAngkringan.Classes
{
    public static class DBHelper
    {
        // Koneksi ke database DBshop (username 10, password 10)
        private static string connectionString = @"Data Source=LAPTOP-07AAA94J\SQLEXPRESS;Initial Catalog=DBshop;Integrated Security=True";

