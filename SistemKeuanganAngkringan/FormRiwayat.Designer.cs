namespace SistemKeuanganAngkringan
{
    partial class FormRiwayat
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dtpTanggal = new System.Windows.Forms.DateTimePicker();
            this.btnCari = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvRiwayat = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTotalTransaksi = new System.Windows.Forms.Label();
            this.lblInfoDetail = new System.Windows.Forms.Label();
            this.btnImportExcel = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRiwayat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Pilih Tanggal:";
            // 
            // dtpTanggal
            // 
            this.dtpTanggal.Location = new System.Drawing.Point(95, 37);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(150, 20);
            this.dtpTanggal.TabIndex = 2;
            // 
            // btnCari
            // 
            this.btnCari.BackColor = System.Drawing.Color.LightBlue;
            this.btnCari.Location = new System.Drawing.Point(260, 35);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(75, 23);
            this.btnCari.TabIndex = 3;
            this.btnCari.Text = "Cari";
            this.btnCari.UseVisualStyleBackColor = false;
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Daftar Transaksi:";
            // 
            // dgvRiwayat
            // 
            this.dgvRiwayat.Location = new System.Drawing.Point(12, 90);
            this.dgvRiwayat.Name = "dgvRiwayat";
            this.dgvRiwayat.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRiwayat.Size = new System.Drawing.Size(650, 150);
            this.dgvRiwayat.TabIndex = 7;
            this.dgvRiwayat.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRiwayat_CellClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(12, 260);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Detail Transaksi:";
            // 
            // dgvDetail
            // 
            this.dgvDetail.Location = new System.Drawing.Point(12, 280);
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.Size = new System.Drawing.Size(650, 150);
            this.dgvDetail.TabIndex = 9;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.LightYellow;
            this.btnRefresh.Location = new System.Drawing.Point(345, 35);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblTotalTransaksi
            // 
            this.lblTotalTransaksi.AutoSize = true;
            this.lblTotalTransaksi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalTransaksi.Location = new System.Drawing.Point(440, 38);
            this.lblTotalTransaksi.Name = "lblTotalTransaksi";
            this.lblTotalTransaksi.Size = new System.Drawing.Size(121, 15);
            this.lblTotalTransaksi.TabIndex = 5;
            this.lblTotalTransaksi.Text = "Total Transaksi: 0";
            // 
            // lblInfoDetail
            // 
            this.lblInfoDetail.AutoSize = true;
            this.lblInfoDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.lblInfoDetail.ForeColor = System.Drawing.Color.Gray;
            this.lblInfoDetail.Location = new System.Drawing.Point(12, 245);
            this.lblInfoDetail.Name = "lblInfoDetail";
            this.lblInfoDetail.Size = new System.Drawing.Size(232, 13);
            this.lblInfoDetail.TabIndex = 10;
            this.lblInfoDetail.Text = "ℹ️ Klik salah satu transaksi untuk melihat detail.";
            // 
            // btnImportExcel
            // 
            this.btnImportExcel.BackColor = System.Drawing.Color.LightGreen;
            this.btnImportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnImportExcel.Location = new System.Drawing.Point(563, 35);
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.Size = new System.Drawing.Size(95, 23);
            this.btnImportExcel.TabIndex = 11;
            this.btnImportExcel.Text = "📥 Import Excel";
            this.btnImportExcel.UseVisualStyleBackColor = false;
            this.btnImportExcel.Click += new System.EventHandler(this.btnImportExcel_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.BackColor = System.Drawing.Color.LightBlue;
            this.btnExportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnExportExcel.Location = new System.Drawing.Point(440, 5);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(110, 23);
            this.btnExportExcel.TabIndex = 12;
            this.btnExportExcel.Text = "📊 Export Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // FormRiwayat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 450);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.btnImportExcel);
            this.Controls.Add(this.lblInfoDetail);
            this.Controls.Add(this.dgvDetail);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvRiwayat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTotalTransaksi);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnCari);
            this.Controls.Add(this.dtpTanggal);
            this.Controls.Add(this.label1);
            this.Name = "FormRiwayat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Riwayat Transaksi - Sistem Keuangan Angkringan";
            this.Load += new System.EventHandler(this.FormRiwayat_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRiwayat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.Button btnCari;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvRiwayat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvDetail;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblTotalTransaksi;
        private System.Windows.Forms.Label lblInfoDetail;
        private System.Windows.Forms.Button btnImportExcel;
        private System.Windows.Forms.Button btnExportExcel;
    }
}