using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SistemKeuanganAngkringan.Classes;

namespace SistemKeuanganAngkringan
{
    public partial class FormPemasukan : Form
    {
        public FormPemasukan()
        {
            InitializeComponent();
        }

        private void FormPemasukan_Load(object sender, EventArgs e)
        {
            dtpTanggal.Value = DateTime.Now;
            LoadPemasukan();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            LoadPemasukan();
        }

