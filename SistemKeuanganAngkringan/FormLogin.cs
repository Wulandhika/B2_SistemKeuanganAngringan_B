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

