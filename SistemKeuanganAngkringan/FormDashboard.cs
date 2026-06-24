using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormDashboard : Form
    {
        public FormDashboard()
        {
            InitializeComponent();
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            // Set DateTimePicker ke hari ini
            dtpTanggal.Value = DateTime.Now;

            // Load data awal
            LoadChartData();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadChartData();
        }

        private void LoadChartData()
        {
            try
            {
                DateTime tanggal = dtpTanggal.Value;
                DataTable dt = new DataTable();
                string mode = "";

                // ===== TENTUKAN MODE OTOMATIS DARI TANGGAL =====
                // Cek apakah user pilih 1 hari, 1 minggu, 1 bulan, atau 1 tahun
                DateTime today = DateTime.Now;

                // Cek apakah pilih 1 hari (format: dd MMMM yyyy)
                if (tanggal.Day == dtpTanggal.Value.Day &&
                    tanggal.Month == dtpTanggal.Value.Month &&
                    tanggal.Year == dtpTanggal.Value.Year)
                {
                    // Cek apakah ini pilihan hari (bukan bulan/tahun)
                    // Karena DateTimePicker selalu punya hari, kita cek apakah user pilih 1 hari
                    // Logika: jika user pilih 1 hari, maka tampilkan harian
                    dt = GetDataPemasukanPerHari(tanggal);
                    mode = "Harian";
                    lblPeriode.Text = $"Periode: {tanggal:dd MMMM yyyy} (Harian)";
                }
                else
                {
                    // Cek apakah pilih 1 minggu (7 hari)
                    // Kita cek dengan membandingkan minggu
                    int weekNumber = GetWeekNumber(tanggal);
                    int currentWeek = GetWeekNumber(DateTime.Now);
                    if (weekNumber == currentWeek && tanggal.Year == DateTime.Now.Year)
                    {
                        dt = GetDataPemasukanPerMinggu(tanggal);
                        mode = "Mingguan";
                        lblPeriode.Text = $"Periode: Minggu ke-{weekNumber} {tanggal.Year} (Mingguan)";
                    }
                    // Cek apakah pilih 1 bulan
                    else if (tanggal.Day == 1 && tanggal.Day == DateTime.DaysInMonth(tanggal.Year, tanggal.Month))
                    {
                        // User pilih 1 bulan penuh
                        dt = GetDataPemasukanPerBulan(tanggal.Year, tanggal.Month);
                        mode = "Bulanan";
                        lblPeriode.Text = $"Periode: {tanggal:MMMM yyyy} (Bulanan)";
                    }
                    // Cek apakah pilih 1 tahun (1 Januari)
                    else if (tanggal.Day == 1 && tanggal.Month == 1)
                    {
                        dt = GetDataPemasukanPerTahun(tanggal.Year);
                        mode = "Tahunan";
                        lblPeriode.Text = $"Periode: {tanggal:yyyy} (Tahunan)";
                    }
                    else
                    {
                        // Default: coba sebagai harian
                        dt = GetDataPemasukanPerHari(tanggal);
                        mode = "Harian";
                        lblPeriode.Text = $"Periode: {tanggal:dd MMMM yyyy} (Harian)";
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data transaksi untuk periode ini!",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chartPemasukan.Series["SeriesPemasukan"].Points.Clear();
                    lblTotalPemasukan.Text = "Rp 0";
                    lblJumlahTransaksi.Text = "0 Transaksi";
                    return;
                }

                // Hitung total
                int totalPemasukan = 0;
                int totalTransaksi = 0;
                foreach (DataRow row in dt.Rows)
                {
                    totalPemasukan += Convert.ToInt32(row["TotalPemasukan"]);
                    totalTransaksi += Convert.ToInt32(row["JumlahTransaksi"]);
                }

                lblTotalPemasukan.Text = $"Rp {totalPemasukan:N0}";
                lblJumlahTransaksi.Text = $"{totalTransaksi} Transaksi";

                // Tampilkan di Chart
                chartPemasukan.Series["SeriesPemasukan"].Points.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    string label = row["Label"].ToString();
                    int total = Convert.ToInt32(row["TotalPemasukan"]);
                    chartPemasukan.Series["SeriesPemasukan"].Points.AddXY(label, total);
                }

                // Set label sumbu X sesuai mode
                switch (mode)
                {
                    case "Harian":
                        chartPemasukan.ChartAreas[0].AxisX.Title = "Jam";
                        break;
                    case "Mingguan":
                        chartPemasukan.ChartAreas[0].AxisX.Title = "Hari";
                        break;
                    case "Bulanan":
                        chartPemasukan.ChartAreas[0].AxisX.Title = "Tanggal";
                        break;
                    case "Tahunan":
                        chartPemasukan.ChartAreas[0].AxisX.Title = "Bulan";
                        break;
                }

                chartPemasukan.ChartAreas[0].AxisY.Title = "Pemasukan (Rp)";
                chartPemasukan.Series["SeriesPemasukan"].IsValueShownAsLabel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper: Get week number
        private int GetWeekNumber(DateTime date)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            return ci.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        // ==================== METHOD AMBIL DATA ====================

        private DataTable GetDataPemasukanPerHari(DateTime tanggal)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPemasukanPerHari", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tanggal", tanggal.Date);
                    DBHelper.OpenConnection(conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    DBHelper.CloseConnection(conn);
                }
            }
            return dt;
        }

        private DataTable GetDataPemasukanPerMinggu(DateTime tanggal)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPemasukanPerMinggu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    DateTime start = tanggal.AddDays(-(int)tanggal.DayOfWeek + 1);
                    DateTime end = start.AddDays(6);
                    cmd.Parameters.AddWithValue("@StartDate", start);
                    cmd.Parameters.AddWithValue("@EndDate", end);
                    DBHelper.OpenConnection(conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    DBHelper.CloseConnection(conn);
                }
            }
            return dt;
        }

        private DataTable GetDataPemasukanPerBulan(int tahun, int bulan)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPemasukanPerBulan", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tahun", tahun);
                    cmd.Parameters.AddWithValue("@Bulan", bulan);
                    DBHelper.OpenConnection(conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    DBHelper.CloseConnection(conn);
                }
            }
            return dt;
        }

        private DataTable GetDataPemasukanPerTahun(int tahun)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPemasukanPerTahun", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tahun", tahun);
                    DBHelper.OpenConnection(conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    DBHelper.CloseConnection(conn);
                }
            }
            return dt;
        }
    }
}