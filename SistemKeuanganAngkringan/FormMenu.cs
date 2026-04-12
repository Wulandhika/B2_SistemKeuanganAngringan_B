using System;
using System.Data;
using System.Data.SqlClient;
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
            nudHarga.Value = 0;
            txtNamaMenu.Focus();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNamaMenu.Text == "")
                {
                    MessageBox.Show("Nama menu harus diisi!");
                    return;
                }
                if (nudHarga.Value <= 0)
                {
                    MessageBox.Show("Harga harus lebih dari 0!");
                    return;
                }

                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = "INSERT INTO menu (nama_menu, harga) VALUES (@nama, @harga)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nama", txtNamaMenu.Text);
                cmd.Parameters.AddWithValue("@harga", nudHarga.Value);

                cmd.ExecuteNonQuery();
                DBHelper.CloseConnection(conn);

                MessageBox.Show("Menu berhasil ditambahkan!");
                LoadMenu();
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

                int id_menu = Convert.ToInt32(dgvMenu.CurrentRow.Cells["id_menu"].Value);

                if (txtNamaMenu.Text == "")
                {
                    MessageBox.Show("Nama menu harus diisi!");
                    return;
                }
                if (nudHarga.Value <= 0)
                {
                    MessageBox.Show("Harga harus lebih dari 0!");
                    return;
                }

                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = "UPDATE menu SET nama_menu=@nama, harga=@harga WHERE id_menu=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id_menu);
                cmd.Parameters.AddWithValue("@nama", txtNamaMenu.Text);
                cmd.Parameters.AddWithValue("@harga", nudHarga.Value);

                cmd.ExecuteNonQuery();
                DBHelper.CloseConnection(conn);

                MessageBox.Show("Menu berhasil diupdate!");
                LoadMenu();
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

                DialogResult confirm = MessageBox.Show($"Hapus menu '{nama_menu}'?", "Konfirmasi", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.No) return;

                SqlConnection conn = DBHelper.GetConnection();
                DBHelper.OpenConnection(conn);

                string query = "DELETE FROM menu WHERE id_menu=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id_menu);

                cmd.ExecuteNonQuery();
                DBHelper.CloseConnection(conn);

                MessageBox.Show("Menu berhasil dihapus!");
                LoadMenu();
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

// FORM MENU