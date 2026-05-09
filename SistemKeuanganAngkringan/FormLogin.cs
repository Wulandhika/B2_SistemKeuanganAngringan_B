using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormLogin : Form
    {
        public static string NamaAdmin = "";
        public static int IdAdmin = 0;

        public FormLogin()
        {
            InitializeComponent();

            // Setting PasswordChar untuk menyembunyikan password
            if (txtPassword != null)
                txtPassword.PasswordChar = '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // ========== VALIDASI USERNAME ==========
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Username harus diisi!", "Validasi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                // Validasi panjang username maksimal 10 karakter
                if (txtUsername.Text.Length > 10)
                {
                    MessageBox.Show("Username maksimal 10 karakter!", "Validasi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                // ========== VALIDASI PASSWORD ==========
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Password harus diisi!", "Validasi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Validasi panjang password maksimal 10 karakter (karena di database VARCHAR(10))
                if (txtPassword.Text.Length > 10)
                {
                    MessageBox.Show("Password maksimal 10 karakter!", "Validasi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // ========== KONEKSI DATABASE ==========
                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                // Query SELECT (username dan password sesuai dengan DBshop)
                string query = "SELECT id_admin, nama_admin FROM admin WHERE username = @user AND password = @pass";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());

                // Jalankan query dan baca hasil
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Login berhasil
                    IdAdmin = Convert.ToInt32(reader["id_admin"]);
                    NamaAdmin = reader["nama_admin"].ToString();

                    MessageBox.Show($"Login Berhasil! Selamat datang, {NamaAdmin}", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Buka Form Utama
                    FormUtama formUtama = new FormUtama();
                    formUtama.Show();

                    // Sembunyikan Form Login
                    this.Hide();
                }
                else
                {
                    // Login gagal
                    MessageBox.Show("Username atau Password salah!", "Gagal",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Kosongkan password
                    txtPassword.Text = "";
                    txtPassword.Focus();
                }

