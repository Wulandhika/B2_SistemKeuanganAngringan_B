using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormPemasukan : Form
    {
        private SqlConnection conn;
        private DataTable dtPemasukan;

        public FormPemasukan()
        {
            InitializeComponent();
            conn = DBHelper.GetConnection();
            dtPemasukan = new DataTable();
        }

        private void FormPemasukan_Load(object sender, EventArgs e)
        {
            // Setting DataGridView
            dgvPemasukan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPemasukan.MultiSelect = false;
            dgvPemasukan.ReadOnly = true;
            dgvPemasukan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Setting DateTimePicker
            dtpTanggal.Value = DateTime.Now;

            // Load Data
            LoadPemasukan();
        }

        // ==================== LOAD PEMASUKAN ====================
        private void LoadPemasukan()
        {
            try
            {
                // ===== 1. Ambil data ringkasan =====
                using (SqlCommand cmd = new SqlCommand("sp_GetPemasukanByDate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tanggal", dtpTanggal.Value.Date);

                    DBHelper.OpenConnection(conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int jumlahTransaksi = Convert.ToInt32(reader["JumlahTransaksi"]);
                        int totalPemasukan = Convert.ToInt32(reader["TotalPemasukan"]);

                        lblJumlahTransaksi.Text = $"Jumlah Transaksi: {jumlahTransaksi}";
                        lblTotalPemasukan.Text = $"Total Pemasukan: Rp {totalPemasukan:N0}";
                    }
                    else
                    {
                        lblJumlahTransaksi.Text = "Jumlah Transaksi: 0";
                        lblTotalPemasukan.Text = "Total Pemasukan: Rp 0";
                    }
                    reader.Close();
                    DBHelper.CloseConnection(conn);
                }

                // ===== 2. Ambil data detail transaksi =====
                using (SqlCommand cmd = new SqlCommand("sp_GetTransaksiByDateForPemasukan", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tanggal", dtpTanggal.Value.Date);

                    DBHelper.OpenConnection(conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    dtPemasukan.Clear();
                    dtPemasukan.Load(reader);
                    reader.Close();
                    DBHelper.CloseConnection(conn);
                }

                // ✅ LANGSUNG ke DataGridView (tanpa BindingSource)
                dgvPemasukan.DataSource = dtPemasukan;

                // Atur header kolom
                if (dgvPemasukan.Columns["id_transaksi"] != null)
                    dgvPemasukan.Columns["id_transaksi"].HeaderText = "ID Transaksi";
                if (dgvPemasukan.Columns["tanggal"] != null)
                    dgvPemasukan.Columns["tanggal"].HeaderText = "Tanggal";
                if (dgvPemasukan.Columns["nama_admin"] != null)
                    dgvPemasukan.Columns["nama_admin"].HeaderText = "Admin";
                if (dgvPemasukan.Columns["total_harga"] != null)
                    dgvPemasukan.Columns["total_harga"].HeaderText = "Total Harga";

                lblInfo.Text = $"Menampilkan {dtPemasukan.Rows.Count} transaksi";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Load Pemasukan: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== TOMBOL CARI ====================
        private void btnCari_Click(object sender, EventArgs e)
        {
            LoadPemasukan();
        }

        // ==================== TOMBOL REFRESH ====================
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtpTanggal.Value = DateTime.Now;
            LoadPemasukan();
        }
    }
}