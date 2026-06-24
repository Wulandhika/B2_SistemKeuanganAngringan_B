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
        private int currentPosition = 0;
        private int totalRecords = 0;

        public FormMenu()
        {
            InitializeComponent();
            conn = DBHelper.GetConnection();
            dtMenu = new DataTable();
        }

        // ==================== LOGGING ERROR ====================
        private void SimpanLog(string pesan)
        {
            try
            {
                using (SqlConnection logConn = DBHelper.GetConnection())
                {
                    string query = @"INSERT INTO LogError (waktu, pesan_error) VALUES (GETDATE(), @pesan)";
                    using (SqlCommand cmd = new SqlCommand(query, logConn))
                    {
                        cmd.Parameters.AddWithValue("@pesan", pesan);
                        DBHelper.OpenConnection(logConn);
                        cmd.ExecuteNonQuery();
                        DBHelper.CloseConnection(logConn);
                    }
                }
            }
            catch
            {
                // Abaikan error logging
            }
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMenu.MultiSelect = false;
            dgvMenu.ReadOnly = true;
            dgvMenu.AllowUserToAddRows = false;
            dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMenu.DataError += (s, ev) => { };

            bindingNavigator1.BindingSource = null;

            bindingNavigatorMoveFirstItem.Click += (s, ev) => NavigateFirst();
            bindingNavigatorMovePreviousItem.Click += (s, ev) => NavigatePrevious();
            bindingNavigatorMoveNextItem.Click += (s, ev) => NavigateNext();
            bindingNavigatorMoveLastItem.Click += (s, ev) => NavigateLast();
            bindingNavigatorAddNewItem.Click += (s, ev) => ClearInput();
            bindingNavigatorDeleteItem.Click += (s, ev) => btnDelete_Click(s, ev);

            nudHarga.Minimum = 0;
            nudHarga.Maximum = 999999999;
            nudHarga.Increment = 500;
            nudHarga.ThousandsSeparator = true;
            nudHarga.Value = 1000;

            nudHarga.Validating += NudHarga_Validating;
            nudHarga.ValueChanged += NudHarga_ValueChanged;
            nudHarga.KeyDown += NudHarga_KeyDown;

            LoadData();
            UpdateNavigatorUI();
            HitungTotalMenu();
        }

        private void NavigateFirst()
        {
            if (dtMenu.Rows.Count > 0)
            {
                currentPosition = 0;
                DisplayCurrentRow();
                UpdateNavigatorUI();
            }
        }

        private void NavigatePrevious()
        {
            if (currentPosition > 0 && dtMenu.Rows.Count > 0)
            {
                currentPosition--;
                DisplayCurrentRow();
                UpdateNavigatorUI();
            }
        }

        private void NavigateNext()
        {
            if (currentPosition < dtMenu.Rows.Count - 1)
            {
                currentPosition++;
                DisplayCurrentRow();
                UpdateNavigatorUI();
            }
        }

        private void NavigateLast()
        {
            if (dtMenu.Rows.Count > 0)
            {
                currentPosition = dtMenu.Rows.Count - 1;
                DisplayCurrentRow();
                UpdateNavigatorUI();
            }
        }

        private void ClearInput()
        {
            txtNamaMenu.Text = "";
            nudHarga.Value = 1000;
            txtNamaMenu.Focus();
        }

        private void DisplayCurrentRow()
        {
            if (dtMenu.Rows.Count > 0 && currentPosition >= 0 && currentPosition < dtMenu.Rows.Count)
            {
                DataRow row = dtMenu.Rows[currentPosition];
                txtNamaMenu.Text = row["nama_menu"].ToString();
                int harga = Convert.ToInt32(row["harga"]);
                if (harga < 0) harga = 0;
                nudHarga.Value = harga;

                dgvMenu.ClearSelection();
                if (currentPosition < dgvMenu.Rows.Count)
                {
                    dgvMenu.Rows[currentPosition].Selected = true;
                    dgvMenu.FirstDisplayedScrollingRowIndex = currentPosition;
                }
            }
        }

        private void UpdateNavigatorUI()
        {
            bindingNavigatorPositionItem.Text = (currentPosition + 1).ToString();
            bindingNavigatorCountItem.Text = $"of {totalRecords}";

            bindingNavigatorMoveFirstItem.Enabled = currentPosition > 0;
            bindingNavigatorMovePreviousItem.Enabled = currentPosition > 0;
            bindingNavigatorMoveNextItem.Enabled = currentPosition < totalRecords - 1;
            bindingNavigatorMoveLastItem.Enabled = currentPosition < totalRecords - 1;
        }

        private void NudHarga_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                MessageBox.Show("❌ Harga tidak boleh negatif!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                nudHarga.Value = 1000;
                nudHarga.Refresh();
                nudHarga.Update();
            }
        }

        private void NudHarga_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;

            if (nud.Value < 0)
            {
                MessageBox.Show($"❌ Harga tidak boleh negatif!\n\nHarga yang Anda masukkan (Rp {nud.Value:N0}) tidak valid.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nud.Value = 1000;
                nud.BackColor = System.Drawing.Color.White;
                nud.ForeColor = System.Drawing.Color.Black;
                toolTip1.SetToolTip(nud, "");
                nud.Refresh();
                nud.Update();
                return;
            }

            if (nud.Value == 0)
            {
                MessageBox.Show($"❌ Harga Rp 0 tidak diperbolehkan!\n\nHarga minimal untuk menu adalah Rp 1.000.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nud.Value = 1000;
                nud.BackColor = System.Drawing.Color.White;
                nud.ForeColor = System.Drawing.Color.Black;
                toolTip1.SetToolTip(nud, "");
                nud.Refresh();
                nud.Update();
                return;
            }

            if (nud.Value > 0 && nud.Value < 1000)
            {
                MessageBox.Show($"❌ Harga minimal Rp 1.000!\n\nHarga yang Anda masukkan (Rp {nud.Value:N0}) terlalu rendah.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nud.Value = 1000;
                nud.BackColor = System.Drawing.Color.White;
                nud.ForeColor = System.Drawing.Color.Black;
                toolTip1.SetToolTip(nud, "");
                nud.Refresh();
                nud.Update();
                return;
            }

            if (nud.Value > 20000)
            {
                MessageBox.Show($"❌ Harga maksimal Rp 20.000!\n\nHarga yang Anda masukkan (Rp {nud.Value:N0}) terlalu tinggi.\nMenu angkringan maksimal Rp 20.000.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nud.Value = 20000;
                nud.BackColor = System.Drawing.Color.White;
                nud.ForeColor = System.Drawing.Color.Black;
                toolTip1.SetToolTip(nud, "");
                nud.Refresh();
                nud.Update();
                return;
            }

            nud.BackColor = System.Drawing.Color.White;
            nud.ForeColor = System.Drawing.Color.Black;
            toolTip1.SetToolTip(nud, "");
            nud.Refresh();
            nud.Update();
        }

        private void NudHarga_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;

            if (nud.Value < 0)
            {
                nud.BackColor = System.Drawing.Color.Red;
                nud.ForeColor = System.Drawing.Color.White;
                toolTip1.SetToolTip(nud, "🚫 Harga tidak boleh negatif!");
                return;
            }

            if (nud.Value == 0)
            {
                nud.BackColor = System.Drawing.Color.LightPink;
                nud.ForeColor = System.Drawing.Color.Red;
                toolTip1.SetToolTip(nud, "⚠️ Harga minimal Rp 1.000");
                return;
            }

            if (nud.Value > 0 && nud.Value < 1000)
            {
                nud.BackColor = System.Drawing.Color.LightPink;
                nud.ForeColor = System.Drawing.Color.Red;
                toolTip1.SetToolTip(nud, "⚠️ Harga minimal Rp 1.000");
                return;
            }

            if (nud.Value > 20000)
            {
                nud.BackColor = System.Drawing.Color.LightPink;
                nud.ForeColor = System.Drawing.Color.Red;
                toolTip1.SetToolTip(nud, "⚠️ Harga maksimal Rp 20.000");
                return;
            }

            nud.BackColor = System.Drawing.Color.White;
            nud.ForeColor = System.Drawing.Color.Black;
            toolTip1.SetToolTip(nud, "");
        }

        private void LoadData()
        {
            try
            {
                dtMenu.Clear();

                using (SqlCommand cmd = new SqlCommand("sp_GetAllMenu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    DBHelper.OpenConnection(conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtMenu.Load(reader);
                    }
                    DBHelper.CloseConnection(conn);
                }

                dgvMenu.DataSource = dtMenu;

                totalRecords = dtMenu.Rows.Count;
                if (totalRecords > 0)
                {
                    currentPosition = 0;
                    DisplayCurrentRow();
                }
                else
                {
                    txtNamaMenu.Text = "";
                    nudHarga.Value = 1000;
                }

                UpdateNavigatorUI();
                HitungTotalMenu();
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Gagal load data: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HitungTotalMenu()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_CountMenu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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

        private bool IsNamaMenuValid(string nama)
        {
            if (string.IsNullOrWhiteSpace(nama))
            {
                MessageBox.Show("❌ Nama menu tidak boleh kosong!\n\nSilahkan isi nama menu terlebih dahulu.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamaMenu.Focus();
                txtNamaMenu.BackColor = System.Drawing.Color.LightPink;
                return false;
            }

            string trimmed = nama.Trim();

            if (trimmed.Length < 3)
            {
                MessageBox.Show($"❌ Nama menu terlalu pendek!\n\n'{trimmed}' hanya {trimmed.Length} karakter.\nMinimal 3 karakter.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamaMenu.Focus();
                txtNamaMenu.SelectAll();
                txtNamaMenu.BackColor = System.Drawing.Color.LightPink;
                return false;
            }

            if (trimmed.Length > 50)
            {
                MessageBox.Show($"❌ Nama menu terlalu panjang!\n\n'{trimmed}' memiliki {trimmed.Length} karakter.\nMaksimal 50 karakter.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamaMenu.Focus();
                txtNamaMenu.SelectAll();
                txtNamaMenu.BackColor = System.Drawing.Color.LightPink;
                return false;
            }

            if (Regex.IsMatch(trimmed, @"^\d+$"))
            {
                MessageBox.Show("❌ Nama menu tidak boleh hanya angka!\n\nContoh: 'Indomie', 'Es Teh', 'Nasi Goreng'.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamaMenu.Focus();
                txtNamaMenu.SelectAll();
                txtNamaMenu.BackColor = System.Drawing.Color.LightPink;
                return false;
            }

            if (Regex.IsMatch(trimmed, @"[<>{};'""&|\\/]"))
            {
                MessageBox.Show("❌ Nama menu mengandung karakter tidak valid!\n\nKarakter yang tidak diperbolehkan: < > { } ; ' \" & | \\ /",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamaMenu.Focus();
                txtNamaMenu.SelectAll();
                txtNamaMenu.BackColor = System.Drawing.Color.LightPink;
                return false;
            }

            txtNamaMenu.BackColor = System.Drawing.Color.White;
            return true;
        }

        private bool IsHargaValid()
        {
            if (nudHarga.Value < 0)
            {
                MessageBox.Show($"❌ Harga tidak boleh negatif!\n\nHarga yang Anda masukkan (Rp {nudHarga.Value:N0}) tidak valid.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nudHarga.Focus();
                nudHarga.BackColor = System.Drawing.Color.Red;
                nudHarga.ForeColor = System.Drawing.Color.White;
                nudHarga.Value = 1000;
                nudHarga.Refresh();
                nudHarga.Update();
                return false;
            }

            if (nudHarga.Value == 0)
            {
                MessageBox.Show($"❌ Harga Rp 0 tidak diperbolehkan!\n\nHarga minimal untuk menu adalah Rp 1.000.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudHarga.Focus();
                nudHarga.BackColor = System.Drawing.Color.LightPink;
                nudHarga.ForeColor = System.Drawing.Color.Red;
                nudHarga.Value = 1000;
                nudHarga.Refresh();
                nudHarga.Update();
                return false;
            }

            if (nudHarga.Value < 1000)
            {
                MessageBox.Show($"❌ Harga minimal Rp 1.000!\n\nHarga yang Anda masukkan (Rp {nudHarga.Value:N0}) terlalu rendah.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudHarga.Focus();
                nudHarga.BackColor = System.Drawing.Color.LightPink;
                nudHarga.ForeColor = System.Drawing.Color.Red;
                nudHarga.Value = 1000;
                nudHarga.Refresh();
                nudHarga.Update();
                return false;
            }

            if (nudHarga.Value > 20000)
            {
                MessageBox.Show($"❌ Harga maksimal Rp 20.000!\n\nHarga yang Anda masukkan (Rp {nudHarga.Value:N0}) terlalu tinggi.\nMenu angkringan maksimal Rp 20.000.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudHarga.Focus();
                nudHarga.BackColor = System.Drawing.Color.LightPink;
                nudHarga.ForeColor = System.Drawing.Color.Red;
                nudHarga.Value = 20000;
                nudHarga.Refresh();
                nudHarga.Update();
                return false;
            }

            nudHarga.BackColor = System.Drawing.Color.White;
            nudHarga.ForeColor = System.Drawing.Color.Black;
            nudHarga.Refresh();
            nudHarga.Update();
            return true;
        }

        private bool IsNamaMenuExist(string namaMenu, int excludeId = 0)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_CheckMenuExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NamaMenu", namaMenu.Trim());
                    cmd.Parameters.AddWithValue("@ExcludeId", excludeId);

                    SqlParameter outputParam = new SqlParameter("@Exists", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParam);

                    DBHelper.OpenConnection(conn);
                    cmd.ExecuteNonQuery();
                    DBHelper.CloseConnection(conn);

                    return Convert.ToInt32(outputParam.Value) > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private int GetHargaMenuById(int idMenu)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetHargaMenuById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMenu", idMenu);

                    DBHelper.OpenConnection(conn);
                    object result = cmd.ExecuteScalar();
                    DBHelper.CloseConnection(conn);

                    if (result == null || result == DBNull.Value)
                        return 0;

                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return 0;
            }
        }

        // ==================== INSERT (DENGAN TRANSACTION) ====================
        private void btnInsert_Click(object sender, EventArgs e)
        {
            SqlTransaction trans = null;
            try
            {
                string namaMenu = txtNamaMenu.Text.Trim();

                txtNamaMenu.BackColor = System.Drawing.Color.White;
                nudHarga.BackColor = System.Drawing.Color.White;
                nudHarga.ForeColor = System.Drawing.Color.Black;

                if (!IsNamaMenuValid(namaMenu)) return;
                if (!IsHargaValid()) return;

                if (IsNamaMenuExist(namaMenu, 0))
                {
                    MessageBox.Show($"❌ Nama menu '{namaMenu}' sudah terdaftar!\n\nGunakan nama lain yang unik.",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNamaMenu.Focus();
                    txtNamaMenu.SelectAll();
                    txtNamaMenu.BackColor = System.Drawing.Color.LightPink;
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    $"📝 Tambahkan menu baru?\n\n" +
                    $"┌─────────────────────────┐\n" +
                    $"│ Nama  : {namaMenu}\n" +
                    $"│ Harga : Rp {nudHarga.Value:N0}\n" +
                    $"└─────────────────────────┘\n\n" +
                    $"Yakin ingin menambahkan?",
                    "Konfirmasi Tambah",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (confirm == DialogResult.No) return;

                DBHelper.OpenConnection(conn);
                trans = conn.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand("sp_InsertMenu", conn, trans))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NamaMenu", namaMenu);
                    cmd.Parameters.AddWithValue("@Harga", nudHarga.Value);
                    cmd.ExecuteNonQuery();

                    using (SqlCommand cmdLog = new SqlCommand(
                        @"INSERT INTO LogAktivitas (aktivitas, waktu) VALUES (@aktivitas, GETDATE())",
                        conn, trans))
                    {
                        cmdLog.Parameters.AddWithValue("@aktivitas", "INSERT MENU : " + namaMenu);
                        cmdLog.ExecuteNonQuery();
                    }

                    trans.Commit();

                    MessageBox.Show($"✅ Menu '{namaMenu}' berhasil ditambahkan!\n\nHarga: Rp {nudHarga.Value:N0}",
                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();
                    txtNamaMenu.Text = "";
                    nudHarga.Value = 1000;
                    nudHarga.Refresh();
                    nudHarga.Update();
                    txtNamaMenu.BackColor = System.Drawing.Color.White;
                    nudHarga.BackColor = System.Drawing.Color.White;
                    nudHarga.ForeColor = System.Drawing.Color.Black;
                    txtNamaMenu.Focus();
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                if (trans != null) trans.Rollback();
                SimpanLog("ROLLBACK INSERT : " + ex.Message);
                MessageBox.Show("❌ Nama menu sudah terdaftar!\n\nGunakan nama lain yang unik.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                if (trans != null) trans.Rollback();
                SimpanLog("ROLLBACK INSERT : " + ex.Message);
                MessageBox.Show("Error Database: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (trans != null) trans.Rollback();
                SimpanLog("ROLLBACK INSERT : " + ex.Message);
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (trans != null) trans.Dispose();
                DBHelper.CloseConnection(conn);
            }
        }

        // ==================== UPDATE (DENGAN TRANSACTION) ====================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlTransaction trans = null;
            try
            {
                if (dtMenu.Rows.Count == 0)
                {
                    MessageBox.Show("⚠️ Tidak ada data untuk diupdate!\n\nTambahkan data terlebih dahulu.",
                        "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idMenu = Convert.ToInt32(dtMenu.Rows[currentPosition]["id_menu"]);
                string namaLama = dtMenu.Rows[currentPosition]["nama_menu"]?.ToString() ?? "";
                string namaBaru = txtNamaMenu.Text.Trim();

                txtNamaMenu.BackColor = System.Drawing.Color.White;
                nudHarga.BackColor = System.Drawing.Color.White;
                nudHarga.ForeColor = System.Drawing.Color.Black;

                if (!IsNamaMenuValid(namaBaru)) return;
                if (!IsHargaValid()) return;

                if (IsNamaMenuExist(namaBaru, idMenu))
                {
                    MessageBox.Show($"❌ Nama menu '{namaBaru}' sudah terdaftar!\n\nGunakan nama lain yang unik.",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNamaMenu.Focus();
                    txtNamaMenu.SelectAll();
                    txtNamaMenu.BackColor = System.Drawing.Color.LightPink;
                    return;
                }

                int hargaLama = GetHargaMenuById(idMenu);
                if (hargaLama == 0)
                {
                    MessageBox.Show("❌ Data menu tidak ditemukan di database!\n\nSilahkan refresh data.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                    return;
                }

                int hargaBaru = (int)nudHarga.Value;

                DialogResult confirm = MessageBox.Show(
                    $"✏️ Update menu?\n\n" +
                    $"┌─────────────────────────────────────────┐\n" +
                    $"│ Nama : {namaLama} → {namaBaru}\n" +
                    $"│ Harga: Rp {hargaLama:N0} → Rp {hargaBaru:N0}\n" +
                    $"└─────────────────────────────────────────┘\n\n" +
                    $"Yakin ingin mengupdate?",
                    "Konfirmasi Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (confirm == DialogResult.No) return;

                DBHelper.OpenConnection(conn);
                trans = conn.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand("sp_UpdateMenuWithLog", conn, trans))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMenu", idMenu);
                    cmd.Parameters.AddWithValue("@NamaMenu", namaBaru);
                    cmd.Parameters.AddWithValue("@HargaBaru", hargaBaru);
                    cmd.Parameters.AddWithValue("@AdminId", FormLogin.IdAdmin);

                    object resultObj = cmd.ExecuteScalar();
                    int result = resultObj != null ? Convert.ToInt32(resultObj) : 0;

                    if (result > 0)
                    {
                        trans.Commit();
                        MessageBox.Show($"✅ Menu berhasil diupdate!\n\nNama: {namaBaru}\nHarga: Rp {hargaBaru:N0}",
                            "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        txtNamaMenu.Text = "";
                        nudHarga.Value = 1000;
                        nudHarga.Refresh();
                        nudHarga.Update();
                        txtNamaMenu.BackColor = System.Drawing.Color.White;
                        nudHarga.BackColor = System.Drawing.Color.White;
                        nudHarga.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        if (trans != null) trans.Rollback();
                        MessageBox.Show("⚠️ Menu tidak ditemukan!\n\nSilahkan refresh data.",
                            "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
            }
            catch (SqlException ex)
            {
                if (trans != null) trans.Rollback();
                SimpanLog("ROLLBACK UPDATE : " + ex.Message);
                MessageBox.Show("Error Database: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (trans != null) trans.Rollback();
                SimpanLog("ROLLBACK UPDATE : " + ex.Message);
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (trans != null) trans.Dispose();
                DBHelper.CloseConnection(conn);
            }
        }

        // ==================== DELETE (DENGAN TRANSACTION) ====================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlTransaction trans = null;
            try
            {
                if (dtMenu.Rows.Count == 0)
                {
                    MessageBox.Show("⚠️ Tidak ada data untuk dihapus!", "Peringatan",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idMenu = Convert.ToInt32(dtMenu.Rows[currentPosition]["id_menu"]);
                string namaMenu = dtMenu.Rows[currentPosition]["nama_menu"].ToString();

                DialogResult confirm = MessageBox.Show(
                    $"🗑️ Hapus menu?\n\n" +
                    $"┌─────────────────────────┐\n" +
                    $"│ Nama  : {namaMenu}\n" +
                    $"└─────────────────────────┘\n\n" +
                    $"⚠️ Tindakan ini tidak dapat dibatalkan!\n\n" +
                    $"Yakin ingin menghapus?",
                    "Konfirmasi Hapus",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (confirm == DialogResult.No) return;

                DBHelper.OpenConnection(conn);
                trans = conn.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand("sp_DeleteMenu", conn, trans))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMenu", idMenu);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        using (SqlCommand cmdLog = new SqlCommand(
                            @"INSERT INTO LogAktivitas (aktivitas, waktu) VALUES (@aktivitas, GETDATE())",
                            conn, trans))
                        {
                            cmdLog.Parameters.AddWithValue("@aktivitas", "DELETE MENU : " + namaMenu);
                            cmdLog.ExecuteNonQuery();
                        }

                        trans.Commit();

                        MessageBox.Show($"✅ Menu '{namaMenu}' berhasil dihapus!",
                            "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        txtNamaMenu.Text = "";
                        nudHarga.Value = 1000;
                        nudHarga.Refresh();
                        nudHarga.Update();
                        txtNamaMenu.BackColor = System.Drawing.Color.White;
                        nudHarga.BackColor = System.Drawing.Color.White;
                        nudHarga.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        if (trans != null) trans.Rollback();
                        MessageBox.Show("⚠️ Menu tidak ditemukan!", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                if (trans != null) trans.Rollback();
                SimpanLog("ROLLBACK DELETE : " + ex.Message);
                MessageBox.Show("❌ Menu tidak bisa dihapus!\n\nMenu ini sudah pernah dibeli/dipesan.\nHapus transaksi terlebih dahulu.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                if (trans != null) trans.Rollback();
                SimpanLog("ROLLBACK DELETE : " + ex.Message);
                MessageBox.Show("Error Database: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (trans != null) trans.Rollback();
                SimpanLog("ROLLBACK DELETE : " + ex.Message);
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (trans != null) trans.Dispose();
                DBHelper.CloseConnection(conn);
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
                    return;
                }

                DataTable searchResult = new DataTable();

                using (SqlCommand cmd = new SqlCommand("sp_SearchMenu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    DBHelper.OpenConnection(conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        searchResult.Load(reader);
                    }
                    DBHelper.CloseConnection(conn);
                }

                dtMenu = searchResult;
                dgvMenu.DataSource = dtMenu;

                totalRecords = dtMenu.Rows.Count;
                if (totalRecords > 0)
                {
                    currentPosition = 0;
                    DisplayCurrentRow();
                }
                else
                {
                    txtNamaMenu.Text = "";
                    nudHarga.Value = 1000;
                }

                UpdateNavigatorUI();
                lblTotal.Text = $"Hasil Pencarian: {totalRecords} menu";

                if (totalRecords == 0)
                {
                    MessageBox.Show($"🔍 Data tidak ditemukan!\n\nKeyword: '{keyword}'",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Error: " + ex.Message);
                LoadData();
            }
        }

        // ==================== REFRESH ====================
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadData();
            txtNamaMenu.Text = "";
            nudHarga.Value = 1000;
            nudHarga.Refresh();
            nudHarga.Update();
            txtNamaMenu.BackColor = System.Drawing.Color.White;
            nudHarga.BackColor = System.Drawing.Color.White;
            nudHarga.ForeColor = System.Drawing.Color.Black;
            toolTip1.SetToolTip(nudHarga, "");
            MessageBox.Show("🔄 Data berhasil direfresh!", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ==================== TEST INJECTION (REALISTIS) ====================
        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            try
            {
                // ===== FORM "CARI MENU" (KELIHATAN NORMAL) =====
                Form prompt = new Form()
                {
                    Width = 350,
                    Height = 120,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = "Cari Menu",
                    StartPosition = FormStartPosition.CenterScreen,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    BackColor = System.Drawing.Color.White
                };

                Label textLabel = new Label()
                {
                    Left = 20,
                    Top = 20,
                    Text = "Masukkan nama menu yang dicari:",
                    Width = 250,
                    Height = 25,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 9F)
                };

                TextBox textBox = new TextBox()
                {
                    Left = 20,
                    Top = 48,
                    Width = 200,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 10F)
                };

                Button btnCari = new Button()
                {
                    Text = "Cari",
                    Left = 230,
                    Width = 80,
                    Top = 46,
                    DialogResult = DialogResult.OK,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold),
                    BackColor = System.Drawing.Color.LightBlue
                };

                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(btnCari);
                prompt.AcceptButton = btnCari;

                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    string input = textBox.Text;
                    if (string.IsNullOrEmpty(input))
                    {
                        MessageBox.Show("Masukkan kata kunci pencarian.", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // ===== QUERY RENTAN (TANPA PARAMETER) =====
                    string query = "UPDATE menu SET nama_menu = 'HACKED' WHERE nama_menu = '" + input + "'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        DBHelper.OpenConnection(conn);
                        int result = cmd.ExecuteNonQuery();
                        DBHelper.CloseConnection(conn);

                        if (result > 0)
                        {
                            SimpanLog("SQL INJECTION SUCCESS : " + input);
                            MessageBox.Show(
                                $"🔓 {result} data berhasil diubah!\n\n" +
                                "Ini adalah demo SQL Injection.\n" +
                                "Query yang dijalankan:\n" +
                                $"UPDATE menu SET nama_menu = 'HACKED' WHERE nama_menu = '{input}'",
                                "SQL Injection Demo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Tidak ada data yang ditemukan.", "Info",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SimpanLog("ERROR INJECTION : " + ex.Message);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== RESET DATA ====================
        private void btnResetData_Click(object sender, EventArgs e)
        {
            SqlTransaction trans = null;
            try
            {
                DialogResult confirm = MessageBox.Show(
                    "🔄 Reset semua data menu ke kondisi awal?\n\n" +
                    "Data yang akan direset:\n" +
                    "• Semua menu akan dikembalikan ke data awal\n" +
                    "• Data transaksi akan ikut dihapus\n\n" +
                    "⚠️ Tindakan ini tidak dapat dibatalkan!",
                    "Konfirmasi Reset",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (confirm == DialogResult.No) return;

                DBHelper.OpenConnection(conn);
                trans = conn.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand("sp_ResetMenuData", conn, trans))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    using (SqlCommand cmdLog = new SqlCommand(
                        @"INSERT INTO LogAktivitas (aktivitas, waktu) VALUES (@aktivitas, GETDATE())",
                        conn, trans))
                    {
                        cmdLog.Parameters.AddWithValue("@aktivitas", "RESET DATA MENU");
                        cmdLog.ExecuteNonQuery();
                    }

                    trans.Commit();
                }

                LoadData();

                MessageBox.Show("✅ Data berhasil direset ke kondisi awal!", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (trans != null) trans.Rollback();
                SimpanLog("ROLLBACK RESET : " + ex.Message);
                MessageBox.Show("Reset gagal: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
            }
            finally
            {
                if (trans != null) trans.Dispose();
                DBHelper.CloseConnection(conn);
            }
        }

        // ==================== RIWAYAT HARGA ====================
        private void btnLogHarga_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtMenu.Rows.Count == 0)
                {
                    MessageBox.Show("📋 Pilih menu terlebih dahulu!\n\nSilahkan klik salah satu baris di tabel.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int idMenu = Convert.ToInt32(dtMenu.Rows[currentPosition]["id_menu"]);
                string namaMenu = dtMenu.Rows[currentPosition]["nama_menu"].ToString();

                DataTable logTable = new DataTable();

                using (SqlCommand cmd = new SqlCommand("sp_GetLogHargaMenu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMenu", idMenu);

                    DBHelper.OpenConnection(conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        logTable.Load(reader);
                    }
                    DBHelper.CloseConnection(conn);
                }

                if (logTable.Rows.Count == 0)
                {
                    MessageBox.Show($"📊 Belum ada perubahan harga untuk '{namaMenu}'.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string logMessage = $"📊 RIWAYAT PERUBAHAN HARGA\n";
                logMessage += $"═══════════════════════════════════\n";
                logMessage += $"Menu: {namaMenu}\n";
                logMessage += $"═══════════════════════════════════\n\n";

                foreach (DataRow row in logTable.Rows)
                {
                    string tanggal = row["tanggal_format"].ToString();
                    int hargaLama = Convert.ToInt32(row["harga_lama"]);
                    int hargaBaru = Convert.ToInt32(row["harga_baru"]);
                    string status = hargaLama < hargaBaru ? "▲ Naik" : "▼ Turun";

                    logMessage += $"📅 {tanggal}\n";
                    logMessage += $"   {status}  Rp {hargaLama:N0} → Rp {hargaBaru:N0}\n\n";
                }

                MessageBox.Show(logMessage, $"Riwayat Harga - {namaMenu}",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // ==================== CLICK DATAGRIDVIEW ====================
        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dtMenu.Rows.Count)
            {
                currentPosition = e.RowIndex;
                DisplayCurrentRow();
                UpdateNavigatorUI();
            }
        }
    }
}