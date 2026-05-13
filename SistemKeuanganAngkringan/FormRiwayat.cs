using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormRiwayat : Form
    {
        private SqlConnection conn;
        private DataTable dtRiwayat;
        private DataTable dtDetail;
        private BindingSource bindingSourceRiwayat;
        private BindingSource bindingSourceDetail;

        public FormRiwayat()
        {
            InitializeComponent();
            conn = DBHelper.GetConnection();
            dtRiwayat = new DataTable();
            dtDetail = new DataTable();
            bindingSourceRiwayat = new BindingSource();
            bindingSourceDetail = new BindingSource();
        }

        private void FormRiwayat_Load(object sender, EventArgs e)
        {
            // Setting DataGridView Riwayat
            dgvRiwayat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRiwayat.MultiSelect = false;
            dgvRiwayat.ReadOnly = true;
            dgvRiwayat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Setting DataGridView Detail
            dgvDetail.ReadOnly = true;
            dgvDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Setting BindingNavigator
            bindingNavigatorRiwayat.BindingSource = bindingSourceRiwayat;

            // Setting DateTimePicker
            dtpTanggal.Value = DateTime.Now;

            // Load Data
            LoadRiwayat();
        }

        // ==================== LOAD RIWAYAT (STORED PROCEDURE) ====================
        private void LoadRiwayat()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetTransaksiByDate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tanggal", dtpTanggal.Value.Date);

                    DBHelper.OpenConnection(conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    dtRiwayat.Clear();
                    dtRiwayat.Load(reader);
                    reader.Close();
                    DBHelper.CloseConnection(conn);
                }

                bindingSourceRiwayat.DataSource = dtRiwayat;
                dgvRiwayat.DataSource = bindingSourceRiwayat;

                // Atur header kolom
                if (dgvRiwayat.Columns["id_transaksi"] != null)
                    dgvRiwayat.Columns["id_transaksi"].HeaderText = "ID Transaksi";
                if (dgvRiwayat.Columns["tanggal"] != null)
                    dgvRiwayat.Columns["tanggal"].HeaderText = "Tanggal";
                if (dgvRiwayat.Columns["nama_admin"] != null)
                    dgvRiwayat.Columns["nama_admin"].HeaderText = "Admin";
                if (dgvRiwayat.Columns["total_harga"] != null)
                    dgvRiwayat.Columns["total_harga"].HeaderText = "Total Harga";

                // Kosongkan detail
                dtDetail.Clear();
                bindingSourceDetail.DataSource = dtDetail;
                dgvDetail.DataSource = bindingSourceDetail;

                // Update label total
                lblTotalTransaksi.Text = $"Total Transaksi: {dtRiwayat.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Load Riwayat: " + ex.Message);
            }
        }

        // ==================== LOAD DETAIL TRANSAKSI (STORED PROCEDURE) ====================
        private void LoadDetailTransaksi(int id_transaksi)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDetailTransaksi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdTransaksi", id_transaksi);

                    DBHelper.OpenConnection(conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    dtDetail.Clear();
                    dtDetail.Load(reader);
                    reader.Close();
                    DBHelper.CloseConnection(conn);
                }

                bindingSourceDetail.DataSource = dtDetail;
                dgvDetail.DataSource = bindingSourceDetail;

                // Atur header kolom detail
                if (dgvDetail.Columns["nama_menu"] != null)
                    dgvDetail.Columns["nama_menu"].HeaderText = "Nama Menu";
                if (dgvDetail.Columns["jumlah"] != null)
                    dgvDetail.Columns["jumlah"].HeaderText = "Jumlah";
                if (dgvDetail.Columns["subtotal"] != null)
                    dgvDetail.Columns["subtotal"].HeaderText = "Subtotal";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Load Detail: " + ex.Message);
            }
        }

        // ==================== TOMBOL CARI ====================
        private void btnCari_Click(object sender, EventArgs e)
        {
            LoadRiwayat();
        }

        // ==================== TOMBOL REFRESH ====================
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtpTanggal.Value = DateTime.Now;
            LoadRiwayat();
        }

        // ==================== KLIK DATAGRIDVIEW RIWAYAT ====================
        private void dgvRiwayat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRiwayat.Rows[e.RowIndex].Cells["id_transaksi"].Value != DBNull.Value)
            {
                int id_transaksi = Convert.ToInt32(dgvRiwayat.Rows[e.RowIndex].Cells["id_transaksi"].Value);
                LoadDetailTransaksi(id_transaksi);
            }
        }
    }
}