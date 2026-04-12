using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormPemasukan : Form
    {
        public FormPemasukan()
        {
            InitializeComponent();
        }

        private void FormPemasukan_Load(object sender, EventArgs e)
        {
            dtpTanggal.Value = DateTime.Now;
            LoadPemasukan();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            LoadPemasukan();
        }

        private void LoadPemasukan()
        {
            try
            {
                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string queryTotal = @"SELECT 
                                        COUNT(*) as jumlah_transaksi, 
                                        ISNULL(SUM(total_harga),0) as total_pemasukan 
                                     FROM transaksi 
                                     WHERE tanggal = @tanggal";

                SqlCommand cmdTotal = new SqlCommand(queryTotal, conn);
                cmdTotal.Parameters.AddWithValue("@tanggal", dtpTanggal.Value.Date);

                SqlDataReader reader = cmdTotal.ExecuteReader();
                if (reader.Read())
                {
                    int jumlahTransaksi = Convert.ToInt32(reader["jumlah_transaksi"]);
                    int totalPemasukan = Convert.ToInt32(reader["total_pemasukan"]);

                    lblJumlahTransaksi.Text = $"Jumlah Transaksi: {jumlahTransaksi}";
                    lblTotalPemasukan.Text = $"Total Pemasukan: Rp {totalPemasukan:N0}";
                }
                reader.Close();

                string queryTransaksi = @"SELECT 
                                            t.id_transaksi, 
                                            a.nama_admin, 
                                            t.total_harga 
                                         FROM transaksi t
                                         JOIN admin a ON t.id_admin = a.id_admin
                                         WHERE t.tanggal = @tanggal
                                         ORDER BY t.id_transaksi DESC";

                SqlCommand cmdTransaksi = new SqlCommand(queryTransaksi, conn);
                cmdTransaksi.Parameters.AddWithValue("@tanggal", dtpTanggal.Value.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmdTransaksi);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvTransaksi.DataSource = dt;

                DBHelper.CloseConnection(conn);
                dgvTransaksi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

// FORM PEMASUKAN