using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormMenu : Form
    {
        private SqlConnection conn;
        private DataTable dtMenu;

        public FormMenu()
        {
            InitializeComponent();
            conn = DBHelper.GetConnection();
            dtMenu = new DataTable();
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMenu.MultiSelect = false;
            dgvMenu.ReadOnly = true;
            dgvMenu.AllowUserToAddRows = false;
            dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvMenu.DataError += (s, ev) => { };

            nudHarga.Minimum = 1000;
            nudHarga.Maximum = 10000000;
            nudHarga.Increment = 500;
            nudHarga.Value = 1000;
            nudHarga.ThousandsSeparator = true;

            if (bindingNavigator1 != null)
                bindingNavigator1.BindingSource = bindingSource;

            LoadData();
            BindControls();
            HitungTotalMenu();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT id_menu, nama_menu, harga FROM menu ORDER BY nama_menu";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

                dtMenu.Clear();
                adapter.Fill(dtMenu);

                bindingSource.DataSource = dtMenu;
                dgvMenu.DataSource = bindingSource;

                HitungTotalMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        private void HitungTotalMenu()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM menu", conn))
                {
                    DBHelper.OpenConnection(conn);
                    int total = Convert.ToInt32(cmd.ExecuteScalar());
                    DBHelper.CloseConnection(conn);
                    lblTotal.Text = $"Total Menu: {total}";
                }
            }
            catch (Exception)
            {
                lblTotal.Text = "Total Menu: -";
            }
        }

        private void BindControls()
        {
            txtNamaMenu.DataBindings.Clear();
            nudHarga.DataBindings.Clear();

            txtNamaMenu.DataBindings.Add("Text", bindingSource, "nama_menu");

            Binding hargaBinding = new Binding("Value", bindingSource, "harga", true, DataSourceUpdateMode.OnPropertyChanged);
            hargaBinding.Format += (s, ev) =>
            {
                if (ev.Value == DBNull.Value || ev.Value == null)
                {
                    ev.Value = 1000m;
                    return;
                }
                try
                {
                    decimal v = Convert.ToDecimal(ev.Value);
                    if (v < 1000) v = 1000;
                    if (v > 10000000) v = 10000000;
                    ev.Value = v;
                }
                catch
                {
                    ev.Value = 1000m;
                }
            };
            nudHarga.DataBindings.Add(hargaBinding);
        }

        // ==================== VALIDASI NAMA MENU (KETAT) ====================
        private bool IsNamaMenuValid(string nama)
        {
            // Cek null atau kosong
            if (string.IsNullOrWhiteSpace(nama))
            {
                MessageBox.Show("❌ Nama menu tidak boleh kosong!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamaMenu.Focus();
                return false;
            }

            // Trim dan cek lagi
            string trimmed = nama.Trim();
            if (trimmed.Length == 0)
            {
                MessageBox.Show("❌ Nama menu tidak boleh kosong!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamaMenu.Focus();
                return false;
            }

            // Cek minimal 2 karakter
            if (trimmed.Length < 2)
            {
                MessageBox.Show("❌ Nama menu minimal 2 karakter!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamaMenu.Focus();
                return false;
            }

            // Cek hanya angka
            if (Regex.IsMatch(trimmed, @"^\d+$"))
            {
                MessageBox.Show("❌ Nama menu tidak boleh hanya angka!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamaMenu.Focus();
                return false;
            }

            // Cek karakter aneh
            if (Regex.IsMatch(trimmed, @"[<>{};'""&]"))
            {
                MessageBox.Show("❌ Nama menu mengandung karakter tidak valid ( < > { } ; ' \" & )!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamaMenu.Focus();
                return false;
            }

            return true;
        }

        // ==================== VALIDASI HARGA ====================
        private bool IsHargaValid()
        {
            if (nudHarga.Value < 1000)
            {
                MessageBox.Show("❌ Harga tidak boleh 0!\n\nHarga minimal Rp 1.000",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudHarga.Focus();
                nudHarga.Value = 1000;
                return false;
            }
            return true;
        }

        // ==================== CEK DUPLIKAT ====================
        private bool IsNamaMenuExist(string namaMenu, int excludeId = 0)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM menu WHERE nama_menu = @nama AND id_menu != @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", namaMenu.Trim());
                    cmd.Parameters.AddWithValue("@id", excludeId);
                    DBHelper.OpenConnection(conn);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    DBHelper.CloseConnection(conn);
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // ==================== INSERT ====================
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                string namaMenu = txtNamaMenu.Text.Trim();

                // Validasi nama
                if (!IsNamaMenuValid(namaMenu)) return;

                // Cek duplikat
                if (IsNamaMenuExist(namaMenu, 0))
                {
                    MessageBox.Show($"❌ Nama menu '{namaMenu}' sudah terdaftar!\nGunakan nama lain.",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNamaMenu.Focus();
                    return;
                }

                // Validasi harga
                if (!IsHargaValid()) return;

                string query = "INSERT INTO menu (nama_menu, harga) VALUES (@nama, @harga)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", namaMenu);
                    cmd.Parameters.AddWithValue("@harga", nudHarga.Value);

                    DBHelper.OpenConnection(conn);
                    cmd.ExecuteNonQuery();
                    DBHelper.CloseConnection(conn);

                    MessageBox.Show($"✅ Menu '{namaMenu}' berhasil ditambahkan!", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();
                    txtNamaMenu.Text = "";
                    nudHarga.Value = 1000;
                    txtNamaMenu.Focus();
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                MessageBox.Show("❌ Nama menu sudah terdaftar! Gunakan nama lain.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show("❌ Data tidak valid! Periksa kembali input Anda.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== UPDATE ====================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingSource.Current == null)
                {
                    MessageBox.Show("⚠️ Pilih menu yang akan diupdate!", "Peringatan",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView currentRow = (DataRowView)bindingSource.Current;
                int idMenu = Convert.ToInt32(currentRow["id_menu"]);
                string namaLama = currentRow["nama_menu"].ToString();
                string namaBaru = txtNamaMenu.Text.Trim();

                // Validasi nama (WAJIB)
                if (!IsNamaMenuValid(namaBaru)) return;

                // Cek duplikat (kecuali dirinya sendiri)
                if (IsNamaMenuExist(namaBaru, idMenu))
                {
                    MessageBox.Show($"❌ Nama menu '{namaBaru}' sudah terdaftar!\nGunakan nama lain.",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNamaMenu.Focus();
                    return;
                }

                // Validasi harga
                if (!IsHargaValid()) return;

                DialogResult confirm = MessageBox.Show($"Update menu '{namaLama}' menjadi '{namaBaru}'?\n\nHarga: Rp {nudHarga.Value:N0}",
                    "Konfirmasi Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                string query = "UPDATE menu SET nama_menu=@nama, harga=@harga WHERE id_menu=@id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idMenu);
                    cmd.Parameters.AddWithValue("@nama", namaBaru);
                    cmd.Parameters.AddWithValue("@harga", nudHarga.Value);

                    DBHelper.OpenConnection(conn);
                    int result = cmd.ExecuteNonQuery();
                    DBHelper.CloseConnection(conn);

                    if (result > 0)
                    {
                        MessageBox.Show($"✅ Menu berhasil diupdate!", "Sukses",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        txtNamaMenu.Text = "";
                        nudHarga.Value = 1000;
                    }
                    else
                    {
                        MessageBox.Show("⚠️ Menu tidak ditemukan!", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show("❌ Data tidak valid! Periksa kembali input Anda.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== DELETE ====================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingSource.Current == null)
                {
                    MessageBox.Show("⚠️ Pilih menu yang akan dihapus!", "Peringatan",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView currentRow = (DataRowView)bindingSource.Current;
                int idMenu = Convert.ToInt32(currentRow["id_menu"]);
                string namaMenu = currentRow["nama_menu"].ToString();

                DialogResult confirm = MessageBox.Show($"Hapus menu '{namaMenu}'?\n\n⚠️ Tindakan ini tidak dapat dibatalkan!",
                    "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.No) return;

                string query = "DELETE FROM menu WHERE id_menu=@id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idMenu);

                    DBHelper.OpenConnection(conn);
                    int result = cmd.ExecuteNonQuery();
                    DBHelper.CloseConnection(conn);

                    if (result > 0)
                    {
                        MessageBox.Show($"✅ Menu '{namaMenu}' berhasil dihapus!", "Sukses",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        txtNamaMenu.Text = "";
                        nudHarga.Value = 1000;
                    }
                    else
                    {
                        MessageBox.Show("⚠️ Menu tidak ditemukan!", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show("❌ Menu tidak bisa dihapus karena sudah pernah dibeli!\n\nHapus transaksi terlebih dahulu.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== SEARCH ====================
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim();

                if (string.IsNullOrEmpty(keyword))
                {
                    LoadData();
                    BindControls();
                    return;
                }

                DataTable searchResult = new DataTable();
                string query = "SELECT id_menu, nama_menu, harga FROM menu WHERE nama_menu LIKE '%' + @keyword + '%' ORDER BY nama_menu";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", keyword);
                    DBHelper.OpenConnection(conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    searchResult.Load(reader);
                    reader.Close();
                    DBHelper.CloseConnection(conn);
                }

                bindingSource.DataSource = searchResult;
                dgvMenu.DataSource = bindingSource;
                BindControls();

                lblTotal.Text = $"Hasil Pencarian: {searchResult.Rows.Count} menu";

                if (searchResult.Rows.Count == 0)
                {
                    MessageBox.Show("🔍 Data tidak ditemukan!", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                LoadData();
                BindControls();
            }
        }

        // ==================== REFRESH ====================
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadData();
            BindControls();
            MessageBox.Show("🔄 Data berhasil direfresh!", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ==================== SQL INJECTION DEMO ====================
        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            try
            {
                string input = ShowInputDialog(
                    "Masukkan kode SQL Injection:\n\nContoh: ' OR 1=1 --\n\nPERINGATAN: Semua nama menu akan berubah menjadi 'HACKED'!",
                    "SQL Injection Demo");

                if (string.IsNullOrEmpty(input)) return;

                string query = "UPDATE menu SET nama_menu = 'HACKED' WHERE nama_menu = '" + input + "'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    DBHelper.OpenConnection(conn);
                    int result = cmd.ExecuteNonQuery();
                    DBHelper.CloseConnection(conn);

                    MessageBox.Show($"Query yang dijalankan:\n\n{query}\n\n" +
                                    $"Hasil: {result} baris terupdate!\n\n" +
                                    "SEMUA nama menu sekarang menjadi 'HACKED'!\n\n" +
                                    "Klik RESET DATA untuk mengembalikan data ke normal.",
                                    "SQL Injection BERHASIL!",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                LoadData();
                BindControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== RESET DATA ====================
        private void btnResetData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult confirm = MessageBox.Show("Reset semua data menu ke kondisi awal?\n\n" +
                    "Data transaksi yang terkait akan ikut dihapus.\n\n" +
                    "⚠️ Tindakan ini tidak dapat dibatalkan!",
                    "Konfirmasi Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.No) return;

                string resetQuery = @"
                    DELETE FROM detail_transaksi;
                    DELETE FROM transaksi;
                    DELETE FROM menu;
                    INSERT INTO menu (nama_menu, harga)
                    SELECT nama_menu, harga FROM menu_backup;";

                using (SqlCommand cmd = new SqlCommand(resetQuery, conn))
                {
                    DBHelper.OpenConnection(conn);
                    cmd.ExecuteNonQuery();
                    DBHelper.CloseConnection(conn);
                }

                LoadData();
                BindControls();

                MessageBox.Show("✅ Data berhasil direset ke kondisi awal!", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset gagal: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
                BindControls();
            }
        }

        private string ShowInputDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 550,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label textLabel = new Label()
            {
                Left = 20,
                Top = 20,
                Text = text,
                Width = 500,
                Height = 80,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 9F)
            };

            TextBox textBox = new TextBox()
            {
                Left = 20,
                Top = 110,
                Width = 400,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 10F)
            };

            Button confirmation = new Button()
            {
                Text = "OK",
                Left = 430,
                Width = 80,
                Top = 108,
                DialogResult = DialogResult.OK,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold)
            };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            prompt.ShowDialog();
            return textBox.Text;
        }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMenu.Rows[e.RowIndex].Cells["nama_menu"].Value != null)
            {
                string nama = dgvMenu.Rows[e.RowIndex].Cells["nama_menu"].Value.ToString();
                if (!string.IsNullOrWhiteSpace(nama))
                {
                    txtNamaMenu.Text = nama;
                }

                if (dgvMenu.Rows[e.RowIndex].Cells["harga"].Value != null &&
                    dgvMenu.Rows[e.RowIndex].Cells["harga"].Value != DBNull.Value)
                {
                    nudHarga.Value = Convert.ToInt32(dgvMenu.Rows[e.RowIndex].Cells["harga"].Value);
                }
                else
                {
                    nudHarga.Value = 1000;
                }
            }
        }
    }
}

// comit 1: Validasi nama menu lebih ketat, cek duplikat, dan demo SQL Injection