using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormRiwayat : Form
    {
        public FormRiwayat()
        {
            InitializeComponent();
        }

        private void FormRiwayat_Load(object sender, EventArgs e)
        {
            dtpTanggal.Value = DateTime.Now;
            LoadRiwayat();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            LoadRiwayat();
        }

        private void LoadRiwayat()
        {
            try
            {
                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = @"SELECT t.id_transaksi, t.tanggal, a.nama_admin, t.total_harga 
                                FROM transaksi t
                                JOIN admin a ON t.id_admin = a.id_admin
                                WHERE t.tanggal = @tanggal
                                ORDER BY t.id_transaksi DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tanggal", dtpTanggal.Value.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvRiwayat.DataSource = dt;

                DBHelper.CloseConnection(conn);
                dgvRiwayat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvRiwayat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id_transaksi = Convert.ToInt32(dgvRiwayat.Rows[e.RowIndex].Cells["id_transaksi"].Value);
                LoadDetailTransaksi(id_transaksi);
            }
        }

        private void LoadDetailTransaksi(int id_transaksi)
        {
            try
            {
                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = @"SELECT m.nama_menu, d.jumlah, d.subtotal 
                                FROM detail_transaksi d
                                JOIN menu m ON d.id_menu = m.id_menu
                                WHERE d.id_transaksi = @id_transaksi";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_transaksi", id_transaksi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvDetail.DataSource = dt;

                DBHelper.CloseConnection(conn);
                dgvDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

// FORM RIWAYAT: Menampilkan riwayat transaksi berdasarkan tanggal yang dipilih. Ketika pengguna mengklik sebuah transaksi, detail dari transaksi tersebut akan ditampilkan di DataGridView lain.
// FORM RIWAYAT CS