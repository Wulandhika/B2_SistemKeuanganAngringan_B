using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormTransaksi : Form
    {
        DataTable cartTable;
        int total = 0;

        public FormTransaksi()
        {
            InitializeComponent();
            cartTable = new DataTable();
            cartTable.Columns.Add("id_menu");
            cartTable.Columns.Add("nama_menu");
            cartTable.Columns.Add("jumlah");
            cartTable.Columns.Add("harga");
            cartTable.Columns.Add("subtotal");
        }

        private void FormTransaksi_Load(object sender, EventArgs e)
        {
            LoadMenu();
        }

        private void LoadMenu()
        {
            try
            {
                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = "SELECT id_menu, nama_menu, harga FROM menu ORDER BY nama_menu";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvMenu.DataSource = dt;

                DBHelper.CloseConnection(conn);
                dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow == null)
            {
                MessageBox.Show("Pilih menu terlebih dahulu!");
                return;
            }

            if (nudJumlah.Value <= 0)
            {
                MessageBox.Show("Jumlah harus lebih dari 0!");
                return;
            }

            int id_menu = Convert.ToInt32(dgvMenu.CurrentRow.Cells["id_menu"].Value);
            string nama_menu = dgvMenu.CurrentRow.Cells["nama_menu"].Value.ToString();
            int harga = Convert.ToInt32(dgvMenu.CurrentRow.Cells["harga"].Value);
            int jumlah = (int)nudJumlah.Value;
            int subtotal = harga * jumlah;

            bool found = false;
            foreach (DataRow row in cartTable.Rows)
            {
                if (Convert.ToInt32(row["id_menu"]) == id_menu)
                {
                    row["jumlah"] = Convert.ToInt32(row["jumlah"]) + jumlah;
                    row["subtotal"] = Convert.ToInt32(row["subtotal"]) + subtotal;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                cartTable.Rows.Add(id_menu, nama_menu, jumlah, harga, subtotal);
            }

            total += subtotal;
            UpdateCartDisplay();
            nudJumlah.Value = 1;
        }

        private void UpdateCartDisplay()
        {
            lbCart.Items.Clear();
            foreach (DataRow row in cartTable.Rows)
            {
                lbCart.Items.Add($"{row["nama_menu"]} x {row["jumlah"]} = Rp {Convert.ToInt32(row["subtotal"]):N0}");
            }
            lblTotal.Text = $"Total: Rp {total:N0}";
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada item dalam transaksi!");
                return;
            }

            try
            {
                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string queryTransaksi = "INSERT INTO transaksi (tanggal, id_admin, total_harga) VALUES (@tanggal, @id_admin, @total); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdTransaksi = new SqlCommand(queryTransaksi, conn, transaction);
                    cmdTransaksi.Parameters.AddWithValue("@tanggal", DateTime.Now.Date);
                    cmdTransaksi.Parameters.AddWithValue("@id_admin", FormLogin.IdAdmin);
                    cmdTransaksi.Parameters.AddWithValue("@total", total);

                    int id_transaksi = Convert.ToInt32(cmdTransaksi.ExecuteScalar());

                    string queryDetail = "INSERT INTO detail_transaksi (id_transaksi, id_menu, jumlah, subtotal) VALUES (@id_transaksi, @id_menu, @jumlah, @subtotal)";

                    foreach (DataRow row in cartTable.Rows)
                    {
                        SqlCommand cmdDetail = new SqlCommand(queryDetail, conn, transaction);
                        cmdDetail.Parameters.AddWithValue("@id_transaksi", id_transaksi);
                        cmdDetail.Parameters.AddWithValue("@id_menu", row["id_menu"]);
                        cmdDetail.Parameters.AddWithValue("@jumlah", row["jumlah"]);
                        cmdDetail.Parameters.AddWithValue("@subtotal", row["subtotal"]);
                        cmdDetail.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    MessageBox.Show($"Transaksi berhasil disimpan!\nTotal: Rp {total:N0}");

                    cartTable.Clear();
                    total = 0;
                    UpdateCartDisplay();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }

                DBHelper.CloseConnection(conn);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            cartTable.Clear();
            total = 0;
            UpdateCartDisplay();
        }
    }
}
// FORM TRANSAKSI: Form untuk melakukan transaksi penjualan. Pengguna dapat memilih menu, menentukan jumlah, dan menyimpan transaksi ke database. Transaksi disimpan dalam tabel 'transaksi' dan detailnya disimpan dalam tabel 'detail_transaksi'.