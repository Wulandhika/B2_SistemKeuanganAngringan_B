using System;
using System.Windows.Forms;

namespace SistemKeuanganAngkringan
{
    public partial class FormRekapTransaksi : Form
    {
        public FormRekapTransaksi()
        {
            InitializeComponent();
        }

        private void FormRekapTransaksi_Load(object sender, EventArgs e)
        {
            // Isi ComboBox periode
            cmbPeriode.Items.Clear();
            cmbPeriode.Items.Add("Hari Ini");
            cmbPeriode.Items.Add("Minggu Ini");
            cmbPeriode.Items.Add("Bulan Ini");
            cmbPeriode.Items.Add("Tahun Ini");
            cmbPeriode.Items.Add("Custom");
            cmbPeriode.SelectedIndex = 0; // Default: Hari Ini

            // Set DateTimePicker ke hari ini
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;

            // Sesuai default (Hari Ini) -> nonaktif
            dtpStart.Enabled = false;
            dtpEnd.Enabled = false;
        }

        // Event saat pilihan periode berubah
        private void cmbPeriode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbPeriode.SelectedItem.ToString();

            if (selected == "Custom")
            {
                // ✅ CUSTOM: tanggal AKTIF (bisa dipilih)
                dtpStart.Enabled = true;
                dtpEnd.Enabled = true;
            }
            else
            {
                // ❌ BUKAN CUSTOM: tanggal NONAKTIF (otomatis)
                dtpStart.Enabled = false;
                dtpEnd.Enabled = false;

                // Set tanggal otomatis sesuai pilihan
                DateTime start, end;
                GetDateRange(selected, out start, out end);
                dtpStart.Value = start;
                dtpEnd.Value = end;
            }
        }

        // Method untuk mendapatkan range tanggal
        private void GetDateRange(string periode, out DateTime start, out DateTime end)
        {
            DateTime today = DateTime.Today;

            switch (periode)
            {
                case "Hari Ini":
                    start = today;
                    end = today;
                    break;

                case "Minggu Ini":
                    int diff = (today.DayOfWeek == DayOfWeek.Sunday) ? 6 : (int)today.DayOfWeek - 1;
                    start = today.AddDays(-diff);
                    end = start.AddDays(6);
                    break;

                case "Bulan Ini":
                    start = new DateTime(today.Year, today.Month, 1);
                    end = new DateTime(today.Year, today.Month,
                        DateTime.DaysInMonth(today.Year, today.Month));
                    break;

                case "Tahun Ini":
                    start = new DateTime(today.Year, 1, 1);
                    end = new DateTime(today.Year, 12, 31);
                    break;

                default:
                    start = today;
                    end = today;
                    break;
            }
        }

        // Event tombol Tampilkan Report
        private void btnTampilkan_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = dtpStart.Value.Date;
                DateTime endDate = dtpEnd.Value.Date;

                if (startDate > endDate)
                {
                    MessageBox.Show("❌ Tanggal Mulai tidak boleh lebih besar dari Tanggal Selesai!\n\n" +
                        "Silahkan pilih tanggal yang benar.",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                FormCetakReport formReport = new FormCetakReport(startDate, endDate, "Transaksi");
                formReport.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event tombol Batal
        private void btnBatal_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}