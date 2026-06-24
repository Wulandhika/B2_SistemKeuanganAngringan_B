using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;
using Excel = Microsoft.Office.Interop.Excel;

namespace SistemKeuanganAngkringan
{
    public partial class FormRiwayat : Form
    {
        private SqlConnection conn;
        private DataTable dtRiwayat;
        private DataTable dtDetail;

        public FormRiwayat()
        {
            InitializeComponent();
            conn = DBHelper.GetConnection();
            dtRiwayat = new DataTable();
            dtDetail = new DataTable();
        }

        private void FormRiwayat_Load(object sender, EventArgs e)
        {
            dgvRiwayat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRiwayat.MultiSelect = false;
            dgvRiwayat.ReadOnly = true;
            dgvRiwayat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDetail.ReadOnly = true;
            dgvDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dtpTanggal.Value = DateTime.Now;

            lblInfoDetail.Text = "ℹ️ Klik salah satu transaksi untuk melihat detail.";

            LoadRiwayat();
        }

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

                dgvRiwayat.DataSource = dtRiwayat;

                if (dgvRiwayat.Columns["id_transaksi"] != null)
                    dgvRiwayat.Columns["id_transaksi"].HeaderText = "ID Transaksi";
                if (dgvRiwayat.Columns["tanggal"] != null)
                    dgvRiwayat.Columns["tanggal"].HeaderText = "Tanggal";
                if (dgvRiwayat.Columns["nama_admin"] != null)
                    dgvRiwayat.Columns["nama_admin"].HeaderText = "Admin";
                if (dgvRiwayat.Columns["total_harga"] != null)
                    dgvRiwayat.Columns["total_harga"].HeaderText = "Total Harga";

                dtDetail.Clear();
                dgvDetail.DataSource = dtDetail;

                lblTotalTransaksi.Text = $"Total Transaksi: {dtRiwayat.Rows.Count}";
                lblInfoDetail.Text = "ℹ️ Klik salah satu transaksi untuk melihat detail.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Load Riwayat: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

                dgvDetail.DataSource = dtDetail;

                if (dgvDetail.Columns["nama_menu"] != null)
                    dgvDetail.Columns["nama_menu"].HeaderText = "Nama Menu";
                if (dgvDetail.Columns["jumlah"] != null)
                    dgvDetail.Columns["jumlah"].HeaderText = "Jumlah";
                if (dgvDetail.Columns["subtotal"] != null)
                    dgvDetail.Columns["subtotal"].HeaderText = "Subtotal";

                if (dtDetail.Rows.Count > 0)
                {
                    decimal total = 0;
                    foreach (DataRow row in dtDetail.Rows)
                    {
                        total += Convert.ToInt32(row["subtotal"]);
                    }
                    lblInfoDetail.Text = $"ℹ️ Harga yang tercantum adalah harga SAAT TRANSAKSI (Total: Rp {total:N0}), bukan harga menu terkini.";
                }
                else
                {
                    lblInfoDetail.Text = "ℹ️ Tidak ada detail transaksi.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Load Detail: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // ==================== TOMBOL IMPORT EXCEL ====================
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            FormImportExcel form = new FormImportExcel();
            form.ShowDialog();
            LoadRiwayat();
        }

        // ==================== EXPORT KE EXCEL ====================
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRiwayat.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk diexport!", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Export Data ke Excel";
                saveFileDialog.FileName = $"Riwayat_Transaksi_{DateTime.Now:ddMMyyyy}";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Buat object Excel
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook workbook = excelApp.Workbooks.Add();
                    Excel.Worksheet worksheet = workbook.ActiveSheet;

                    // ===== HEADER =====
                    worksheet.Cells[1, 1] = "ID Transaksi";
                    worksheet.Cells[1, 2] = "Tanggal";
                    worksheet.Cells[1, 3] = "Admin";
                    worksheet.Cells[1, 4] = "Total Harga";

                    // Header Bold
                    for (int i = 1; i <= 4; i++)
                    {
                        worksheet.Cells[1, i].Font.Bold = true;
                    }

                    // ===== DATA =====
                    for (int i = 0; i < dgvRiwayat.Rows.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1] = dgvRiwayat.Rows[i].Cells["id_transaksi"].Value?.ToString() ?? "";
                        worksheet.Cells[i + 2, 2] = dgvRiwayat.Rows[i].Cells["tanggal"].Value?.ToString() ?? "";
                        worksheet.Cells[i + 2, 3] = dgvRiwayat.Rows[i].Cells["nama_admin"].Value?.ToString() ?? "";
                        worksheet.Cells[i + 2, 4] = dgvRiwayat.Rows[i].Cells["total_harga"].Value?.ToString() ?? "";
                    }

                    // ===== TOTAL DI BAWAH =====
                    int lastRow = dgvRiwayat.Rows.Count + 2;
                    worksheet.Cells[lastRow, 1] = "TOTAL TRANSAKSI:";
                    worksheet.Cells[lastRow, 1].Font.Bold = true;
                    worksheet.Cells[lastRow, 4] = dtRiwayat.Rows.Count.ToString();
                    worksheet.Cells[lastRow, 4].Font.Bold = true;

                    // ===== AUTO FIT =====
                    worksheet.Columns.AutoFit();

                    // ===== SAVE =====
                    workbook.SaveAs(saveFileDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();

                    MessageBox.Show($"✅ Data berhasil diexport ke Excel!\n\n{saveFileDialog.FileName}",
                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Export Excel: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== KLIK DATAGRIDVIEW ====================
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