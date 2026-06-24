using ExcelDataReader;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormImportExcel : Form
    {
        private DataTable dtExcel;

        public FormImportExcel()
        {
            InitializeComponent();
            dtExcel = new DataTable();
        }

        private void btnPilihFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                    openFileDialog.Title = "Pilih File Excel Transaksi";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        txtFilePath.Text = openFileDialog.FileName;
                        LoadExcelData(openFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExcelData(string filePath)
        {
            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader reader;

                    if (filePath.EndsWith(".xls"))
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    reader.Close();

                    DataTable dt = result.Tables[0];
                    dtExcel = dt;

                    // ===== VALIDASI KOLOM =====
                    // Cek apakah kolom yang dibutuhkan ada
                    bool hasTanggal = dt.Columns.Contains("Tanggal");
                    bool hasNamaMenu = dt.Columns.Contains("Nama Menu");
                    bool hasJumlah = dt.Columns.Contains("Jumlah");
                    bool hasHargaSatuan = dt.Columns.Contains("Harga Satuan");
                    bool hasSubtotal = dt.Columns.Contains("Subtotal");

                    if (!hasTanggal || !hasNamaMenu || !hasJumlah || !hasHargaSatuan || !hasSubtotal)
                    {
                        MessageBox.Show("❌ Format Excel salah!\n\n" +
                            "Harus memiliki kolom:\n" +
                            "1. Tanggal\n" +
                            "2. Nama Menu\n" +
                            "3. Jumlah\n" +
                            "4. Harga Satuan\n" +
                            "5. Subtotal\n\n" +
                            "Contoh:\n" +
                            "Tanggal | Nama Menu | Jumlah | Harga Satuan | Subtotal\n" +
                            "01/06/2026 | Nasi Kucing | 2 | 5000 | 10000",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnImport.Enabled = false;
                        return;
                    }

                    dgvPreview.DataSource = dtExcel;
                    btnImport.Enabled = true;
                    lblStatus.Text = $"📊 {dtExcel.Rows.Count} baris data transaksi ditemukan";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error membaca Excel: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnImport.Enabled = false;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtExcel.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk diimport!", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult confirm = MessageBox.Show($"Yakin ingin mengimport {dtExcel.Rows.Count} transaksi ke database?",
                    "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.No) return;

                int success = 0;
                int failed = 0;
                string errorMessage = "";

                using (SqlConnection conn = DBHelper.GetConnection())
                {
                    DBHelper.OpenConnection(conn);

                    foreach (DataRow row in dtExcel.Rows)
                    {
                        try
                        {
                            // ===== BACA DATA DARI EXCEL =====
                            string tanggalStr = row["Tanggal"]?.ToString()?.Trim();
                            string namaMenu = row["Nama Menu"]?.ToString()?.Trim();
                            string jumlahStr = row["Jumlah"]?.ToString()?.Trim();
                            string hargaStr = row["Harga Satuan"]?.ToString()?.Trim();
                            string subtotalStr = row["Subtotal"]?.ToString()?.Trim();

                            // Validasi tidak boleh kosong
                            if (string.IsNullOrEmpty(tanggalStr) ||
                                string.IsNullOrEmpty(namaMenu) ||
                                string.IsNullOrEmpty(jumlahStr) ||
                                string.IsNullOrEmpty(hargaStr) ||
                                string.IsNullOrEmpty(subtotalStr))
                            {
                                failed++;
                                continue;
                            }

                            // Parse tanggal (dd/MM/yyyy)
                            if (!DateTime.TryParseExact(tanggalStr, "dd/MM/yyyy",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None, out DateTime tanggal))
                            {
                                failed++;
                                continue;
                            }

                            // Parse jumlah
                            if (!int.TryParse(jumlahStr, out int jumlah) || jumlah <= 0)
                            {
                                failed++;
                                continue;
                            }

                            // Parse harga satuan
                            if (!int.TryParse(hargaStr, out int hargaSatuan) || hargaSatuan <= 0)
                            {
                                failed++;
                                continue;
                            }

                            // Parse subtotal
                            if (!int.TryParse(subtotalStr, out int subtotal) || subtotal <= 0)
                            {
                                failed++;
                                continue;
                            }

                            // ===== CEK APAKAH MENU ADA =====
                            int idMenu = GetIdMenuByName(conn, namaMenu);
                            if (idMenu == 0)
                            {
                                failed++;
                                continue;
                            }

                            // ===== CEK ATAU BUAT TRANSAKSI =====
                            int idTransaksi = GetOrCreateTransaksi(conn, tanggal);

                            // ===== CEK DUPLIKAT =====
                            if (IsDetailExist(conn, idTransaksi, idMenu))
                            {
                                failed++;
                                continue;
                            }

                            // ===== INSERT DETAIL =====
                            InsertDetailTransaksi(conn, idTransaksi, idMenu, jumlah, subtotal);

                            // ===== UPDATE TOTAL TRANSAKSI =====
                            UpdateTotalTransaksi(conn, idTransaksi);

                            success++;
                        }
                        catch (Exception ex)
                        {
                            failed++;
                            errorMessage += ex.Message + "\n";
                        }
                    }

                    DBHelper.CloseConnection(conn);
                }

                MessageBox.Show($"✅ Import Selesai!\n\n" +
                    $"Berhasil: {success} transaksi\n" +
                    $"Gagal: {failed} transaksi\n" +
                    (string.IsNullOrEmpty(errorMessage) ? "" : $"\nError: {errorMessage}"),
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dtExcel.Clear();
                dgvPreview.DataSource = null;
                txtFilePath.Text = "";
                btnImport.Enabled = false;
                lblStatus.Text = "Status: Siap";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== HELPER METHOD ====================

        private int GetIdMenuByName(SqlConnection conn, string namaMenu)
        {
            string query = "SELECT id_menu FROM menu WHERE LTRIM(RTRIM(nama_menu)) = @nama";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nama", namaMenu.Trim());
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private int GetOrCreateTransaksi(SqlConnection conn, DateTime tanggal)
        {
            // Cek apakah transaksi dengan tanggal ini sudah ada
            string cekQuery = "SELECT id_transaksi FROM transaksi WHERE tanggal = @tanggal AND id_admin = 1";
            using (SqlCommand cmd = new SqlCommand(cekQuery, conn))
            {
                cmd.Parameters.AddWithValue("@tanggal", tanggal.Date);
                object result = cmd.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt32(result);
            }

            // Buat transaksi baru
            string insertQuery = "INSERT INTO transaksi (tanggal, id_admin, total_harga) VALUES (@tanggal, 1, 0); SELECT SCOPE_IDENTITY();";
            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@tanggal", tanggal.Date);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private bool IsDetailExist(SqlConnection conn, int idTransaksi, int idMenu)
        {
            string query = "SELECT COUNT(*) FROM detail_transaksi WHERE id_transaksi = @idTransaksi AND id_menu = @idMenu";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idTransaksi", idTransaksi);
                cmd.Parameters.AddWithValue("@idMenu", idMenu);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void InsertDetailTransaksi(SqlConnection conn, int idTransaksi, int idMenu, int jumlah, int subtotal)
        {
            string query = "INSERT INTO detail_transaksi (id_transaksi, id_menu, jumlah, subtotal) VALUES (@idTransaksi, @idMenu, @jumlah, @subtotal)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idTransaksi", idTransaksi);
                cmd.Parameters.AddWithValue("@idMenu", idMenu);
                cmd.Parameters.AddWithValue("@jumlah", jumlah);
                cmd.Parameters.AddWithValue("@subtotal", subtotal);
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateTotalTransaksi(SqlConnection conn, int idTransaksi)
        {
            string query = "UPDATE transaksi SET total_harga = (SELECT SUM(subtotal) FROM detail_transaksi WHERE id_transaksi = @idTransaksi) WHERE id_transaksi = @idTransaksi";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idTransaksi", idTransaksi);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}