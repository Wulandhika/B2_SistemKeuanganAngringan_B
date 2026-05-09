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

        private void btnTransaksi_Click(object sender, EventArgs e)
        {
            FormTransaksi form = new FormTransaksi();
            form.ShowDialog();
        }

        private void btnRiwayat_Click(object sender, EventArgs e)
        {
            FormRiwayat form = new FormRiwayat();
            form.ShowDialog();
        }

        private void btnPemasukan_Click(object sender, EventArgs e)
        {
            FormPemasukan form = new FormPemasukan();
            form.ShowDialog();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FormMenu form = new FormMenu();
            form.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Yakin ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                FormLogin formLogin = new FormLogin();
                formLogin.Show();
                this.Close();
            }
        }
    }
}

// FORM UTAMA
// Menampilkan nama admin yang login
// Menyediakan tombol untuk membuka form transaksi, riwayat, pemasukan, dan menu
// Tombol logout untuk kembali ke form login