using System;
using System.Windows.Forms;

namespace SistemKeuanganAngkringan
{
    public partial class FormUtama : Form
    {
        public FormUtama()
        {
            InitializeComponent();
        }

        private void FormUtama_Load(object sender, EventArgs e)
        {
            lblAdmin.Text = "Selamat Datang, " + FormLogin.NamaAdmin;
        }

        // Tombol Pencatatan Transaksi
        private void btnTransaksi_Click(object sender, EventArgs e)
        {
            FormTransaksi form = new FormTransaksi();
            form.ShowDialog();
        }

        // Tombol Riwayat Transaksi
        private void btnRiwayat_Click(object sender, EventArgs e)
        {
            FormRiwayat form = new FormRiwayat();
            form.ShowDialog();
        }

        // Tombol Total Pemasukan Harian
        private void btnPemasukan_Click(object sender, EventArgs e)
        {
            FormPemasukan form = new FormPemasukan();
            form.ShowDialog();
        }

        // Tombol Kelola Menu (CRUD dengan Stored Procedure)
        private void btnMenu_Click(object sender, EventArgs e)
        {
            FormMenu form = new FormMenu();
            form.ShowDialog();
        }

        // Tombol Logout
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                FormLogin formLogin = new FormLogin();
                formLogin.Show();
                this.Close();
            }
        }
    }
}
// Catatan: Pastikan untuk menambahkan referensi ke FormTransaksi, FormRiwayat, FormPemasukan, dan FormMenu sesuai dengan struktur proyek Anda.