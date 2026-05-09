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
        public FormMenu()
        {
            InitializeComponent();
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            LoadMenu();

            // ========== SETTING NUMERIC UPDOWN ==========
            nudHarga.Minimum = 1000;        // Minimal 1000 (tidak bisa 0 atau 1)
            nudHarga.Maximum = 10000000;    // Maksimal 10 juta
            nudHarga.Increment = 500;       // Naik/turun 500
            nudHarga.Value = 1000;          // Nilai default 1000
        }

        private void LoadMenu()
        {
            try
            {
                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = "SELECT id_menu, nama_menu, harga FROM menu ORDER BY id_menu";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvMenu.DataSource = dt;

                DBHelper.CloseConnection(conn);
                dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtNamaMenu.Text = "";
            nudHarga.Value = 1000;  // Default 1000
            txtNamaMenu.Focus();
        }

        // Validasi Nama Menu
        private bool IsNamaMenuValid(string nama)
        {
            if (string.IsNullOrWhiteSpace(nama))
            {
                MessageBox.Show("Nama menu harus diisi!", "Validasi");
                return false;
            }

            if (Regex.IsMatch(nama, @"^\d+$"))
            {
                MessageBox.Show("Nama menu tidak boleh hanya angka! Contoh: Nasi Kucing, Es Teh", "Validasi");
                return false;
            }

            if (nama.Length < 2)
            {
                MessageBox.Show("Nama menu minimal 2 karakter!", "Validasi");
                return false;
            }

            return true;
        }

        // Validasi Harga
        private bool IsHargaValid(int harga)
        {
            if (harga < 1000)
            {
                MessageBox.Show("Harga minimal Rp 1.000! (tidak boleh 0, 1, 2, 3, dst)", "Validasi");
                return false;
            }
            return true;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsNamaMenuValid(txtNamaMenu.Text)) return;
                if (!IsHargaValid((int)nudHarga.Value)) return;

                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = "INSERT INTO menu (nama_menu, harga) VALUES (@nama, @harga)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nama", txtNamaMenu.Text.Trim());
                cmd.Parameters.AddWithValue("@harga", (int)nudHarga.Value);

                int result = cmd.ExecuteNonQuery();
                DBHelper.CloseConnection(conn);

                if (result > 0)
                {
                    MessageBox.Show("Menu berhasil ditambahkan!", "Sukses");
                    LoadMenu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMenu.CurrentRow == null)
                {
                    MessageBox.Show("Pilih menu yang akan diupdate!");
                    return;
                }

                if (!IsNamaMenuValid(txtNamaMenu.Text)) return;
                if (!IsHargaValid((int)nudHarga.Value)) return;

                int id_menu = Convert.ToInt32(dgvMenu.CurrentRow.Cells["id_menu"].Value);

                DialogResult confirm = MessageBox.Show("Yakin update menu ini?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = "UPDATE menu SET nama_menu=@nama, harga=@harga WHERE id_menu=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id_menu);
                cmd.Parameters.AddWithValue("@nama", txtNamaMenu.Text.Trim());
                cmd.Parameters.AddWithValue("@harga", (int)nudHarga.Value);

                int result = cmd.ExecuteNonQuery();
                DBHelper.CloseConnection(conn);

                if (result > 0)
                {
                    MessageBox.Show("Menu berhasil diupdate!", "Sukses");
                    LoadMenu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMenu.CurrentRow == null)
                {
                    MessageBox.Show("Pilih menu yang akan dihapus!");
                    return;
                }

                int id_menu = Convert.ToInt32(dgvMenu.CurrentRow.Cells["id_menu"].Value);
                string nama_menu = dgvMenu.CurrentRow.Cells["nama_menu"].Value.ToString();

                DialogResult confirm = MessageBox.Show($"Hapus menu '{nama_menu}'?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = "DELETE FROM menu WHERE id_menu=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id_menu);

                int result = cmd.ExecuteNonQuery();
                DBHelper.CloseConnection(conn);

                if (result > 0)
                {
                    MessageBox.Show("Menu berhasil dihapus!", "Sukses");
                    LoadMenu();
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show("Menu tidak bisa dihapus karena sudah pernah dibeli!", "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNamaMenu.Text = dgvMenu.Rows[e.RowIndex].Cells["nama_menu"].Value.ToString();
                nudHarga.Value = Convert.ToInt32(dgvMenu.Rows[e.RowIndex].Cells["harga"].Value);
            }
        }
    }
}

//CEK MENU