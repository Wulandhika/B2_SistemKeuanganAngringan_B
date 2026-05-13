using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormTransaksi : Form
    {
        private SqlConnection conn;
        private DataTable cartTable;
        private int total = 0;
        private DataTable dtMenu;
        private BindingSource bindingSourceMenu;

        public FormTransaksi()
        {
            InitializeComponent();
            conn = DBHelper.GetConnection();
            dtMenu = new DataTable();
            bindingSourceMenu = new BindingSource();
            cartTable = new DataTable();
            cartTable.Columns.Add("id_menu");
            cartTable.Columns.Add("nama_menu");
            cartTable.Columns.Add("jumlah");
            cartTable.Columns.Add("harga");
            cartTable.Columns.Add("subtotal");
        }

        private void FormTransaksi_Load(object sender, EventArgs e)
        {
            // Setting DataGridView Menu
            dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMenu.MultiSelect = false;
            dgvMenu.ReadOnly = true;
            dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Setting BindingNavigator untuk Menu
            bindingNavigatorMenu.BindingSource = bindingSourceMenu;

            // Setting NumericUpDown
            nudJumlah.Minimum = 1;
            nudJumlah.Maximum = 99;
            nudJumlah.Value = 1;

            LoadMenu();
            UpdateCartDisplay();
        }

        // ==================== LOAD MENU (VIEW) ====================
        private void LoadMenu()
        {
            try
            {
                string query = "SELECT NamaMenu, Harga FROM vwMenuPublic ORDER BY NamaMenu";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                dtMenu.Clear();
                da.Fill(dtMenu);
                bindingSourceMenu.DataSource = dtMenu;
                dgvMenu.DataSource = bindingSourceMenu;

                if (dgvMenu.Columns["NamaMenu"] != null)
                    dgvMenu.Columns["NamaMenu"].HeaderText = "Nama Menu";
                if (dgvMenu.Columns["Harga"] != null)
                    dgvMenu.Columns["Harga"].HeaderText = "Harga (Rp)";

                lblInfoMenu.Text = $"Total Menu: {dtMenu.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Load Menu: " + ex.Message);
            }
        }

        // ==================== GET ID MENU DARI NAMA ====================
        private int GetIdMenuByName(string namaMenu)
        {
            int id = 0;
            string query = "SELECT id_menu FROM menu WHERE nama_menu = @nama";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nama", namaMenu);
                DBHelper.OpenConnection(conn);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    id = Convert.ToInt32(result);
                DBHelper.CloseConnection(conn);
            }
            return id;
        }

        // ==================== TAMBAH KE KERANJANG ====================
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

            string nama_menu = dgvMenu.CurrentRow.Cells["NamaMenu"].Value.ToString();
            int harga = Convert.ToInt32(dgvMenu.CurrentRow.Cells["Harga"].Value);
            int jumlah = (int)nudJumlah.Value;
            int subtotal = harga * jumlah;

            int id_menu = GetIdMenuByName(nama_menu);

            if (id_menu == 0)
            {
                MessageBox.Show("Menu tidak ditemukan di database!");
                return;
            }

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

            UpdateCartDisplay();
            nudJumlah.Value = 1;
        }

        // ==================== UPDATE TAMPILAN KERANJANG ====================
        private void UpdateCartDisplay()
        {
            lbCart.Items.Clear();

            if (cartTable.Rows.Count == 0)
            {
                lblTotal.Text = "Total: Rp 0";
                lblInfoCart.Text = "Item di keranjang: 0";
                total = 0;
                return;
            }

            foreach (DataRow row in cartTable.Rows)
            {
                lbCart.Items.Add($"{row["nama_menu"]} x {row["jumlah"]} = Rp {Convert.ToInt32(row["subtotal"]):N0}");
            }

            total = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                total += Convert.ToInt32(row["subtotal"]);
            }

            lblTotal.Text = $"Total: Rp {total:N0}";
            lblInfoCart.Text = $"Item di keranjang: {cartTable.Rows.Count}";
        }

        // ==================== HAPUS ITEM DARI KERANJANG ====================
        private void btnHapusItem_Click(object sender, EventArgs e)
        {
            if (lbCart.SelectedIndex == -1)
            {
                MessageBox.Show("Pilih item yang akan dihapus dari keranjang!");
                return;
            }

            DialogResult confirm = MessageBox.Show("Hapus item ini dari keranjang?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;

            int selectedIndex = lbCart.SelectedIndex;
            cartTable.Rows.RemoveAt(selectedIndex);
            UpdateCartDisplay();
        }

        // ==================== REFRESH MENU ====================
        private void btnRefreshMenu_Click(object sender, EventArgs e)
        {
            LoadMenu();
        }

        // ==================== SIMPAN TRANSAKSI ====================
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada item dalam transaksi!");
                return;
            }

            DialogResult confirm = MessageBox.Show($"Simpan transaksi dengan total Rp {total:N0}?",
                "Konfirmasi Simpan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;

            try
            {
                DBHelper.OpenConnection(conn);
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    int id_transaksi = 0;
                    using (SqlCommand cmd = new SqlCommand("sp_InsertTransaksi", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Tanggal", DateTime.Now.Date);
                        cmd.Parameters.AddWithValue("@IdAdmin", FormLogin.IdAdmin);
                        cmd.Parameters.AddWithValue("@TotalHarga", total);

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            id_transaksi = Convert.ToInt32(result);
                    }

                    if (id_transaksi == 0)
                        throw new Exception("Gagal menyimpan transaksi");

                    using (SqlCommand cmdDetail = new SqlCommand("sp_InsertDetailTransaksi", conn, transaction))
                    {
                        cmdDetail.CommandType = CommandType.StoredProcedure;

                        foreach (DataRow row in cartTable.Rows)
                        {
                            cmdDetail.Parameters.Clear();
                            cmdDetail.Parameters.AddWithValue("@IdTransaksi", id_transaksi);
                            cmdDetail.Parameters.AddWithValue("@IdMenu", row["id_menu"]);
                            cmdDetail.Parameters.AddWithValue("@Jumlah", row["jumlah"]);
                            cmdDetail.Parameters.AddWithValue("@Subtotal", row["subtotal"]);
                            cmdDetail.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();

                    MessageBox.Show($"Transaksi berhasil disimpan!\nID Transaksi: {id_transaksi}\nTotal: Rp {total:N0}",
                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetTransaksi();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    DBHelper.CloseConnection(conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // ==================== RESET TRANSAKSI ====================
        private void ResetTransaksi()
        {
            cartTable.Clear();
            total = 0;
            UpdateCartDisplay();
        }

        // ==================== BATAL ====================
        private void btnBatal_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count > 0)
            {
                DialogResult confirm = MessageBox.Show("Yakin ingin membatalkan transaksi?",
                    "Konfirmasi Batal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    ResetTransaksi();
                }
            }
        }
    }
}