using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormCetakReport : Form
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private string _jenisReport;

        public FormCetakReport(DateTime startDate, DateTime endDate, string jenisReport)
        {
            InitializeComponent();
            _startDate = startDate;
            _endDate = endDate;
            _jenisReport = jenisReport;
        }

        private void FormCetakReport_Load(object sender, EventArgs e)
        {
            try
            {
                if (_jenisReport == "Transaksi")
                {
                    LoadReportTransaksi();
                }
                else if (_jenisReport == "Pemasukan")
                {
                    LoadReportPemasukan();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReportTransaksi()
        {
            try
            {
                DataTable dt = GetDataTransaksiReport(_startDate, _endDate);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data transaksi untuk periode ini!",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ReportDocument report = new ReportDocument();
                string reportPath = Path.Combine(Application.StartupPath, "Reports", "ReportTransaksi.rpt");

                if (!File.Exists(reportPath))
                {
                    MessageBox.Show($"File report tidak ditemukan!\n\nPath: {reportPath}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                report.Load(reportPath);
                report.SetDataSource(dt);

                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();

                this.Text = $"Laporan Transaksi - {dt.Rows.Count} data";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Load Report:\n\n{ex.Message}\n\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReportPemasukan()
        {
            try
            {
                DataTable dt = GetDataPemasukanReport(_startDate, _endDate);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data pemasukan untuk periode ini!",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ReportDocument report = new ReportDocument();
                string reportPath = Path.Combine(Application.StartupPath, "Reports", "ReportPemasukan.rpt");

                if (!File.Exists(reportPath))
                {
                    MessageBox.Show($"File report tidak ditemukan!\n\nPath: {reportPath}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                report.Load(reportPath);
                report.SetDataSource(dt);

                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();

                this.Text = $"Laporan Pemasukan - {dt.Rows.Count} data";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Load Report Pemasukan:\n\n{ex.Message}\n\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== AMBIL DATA ====================

        private DataTable GetDataTransaksiReport(DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetTransaksiReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

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

        private DataTable GetDataPemasukanReport(DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPemasukanReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

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