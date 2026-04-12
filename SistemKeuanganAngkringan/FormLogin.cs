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
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsername.Text == "")
                {
                    MessageBox.Show("Username harus diisi!", "Peringatan");
                    txtUsername.Focus();
                    return;
                }

                if (txtPassword.Text == "")
                {
                    MessageBox.Show("Password harus diisi!", "Peringatan");
                    txtPassword.Focus();
                    return;
                }

                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = "SELECT id_admin, nama_admin FROM admin WHERE username=@user AND password=@pass";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IdAdmin = Convert.ToInt32(reader["id_admin"]);
                    NamaAdmin = reader["nama_admin"].ToString();

                    MessageBox.Show("Login Berhasil! Selamat datang, " + NamaAdmin, "Sukses");

                    FormUtama formUtama = new FormUtama();
                    formUtama.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username atau Password salah!", "Gagal");
                }

                reader.Close();
                DBHelper.CloseConnection(conn);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}