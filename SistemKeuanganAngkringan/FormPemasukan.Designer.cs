namespace SistemKeuanganAngkringan
{
    partial class FormPemasukan
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
            this.lblJumlahTransaksi = new System.Windows.Forms.Label();
            this.lblTotalPemasukan = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.dgvPemasukan = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPemasukan)).BeginInit();
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
            // lblJumlahTransaksi
            // 
            this.lblJumlahTransaksi.AutoSize = true;
            this.lblJumlahTransaksi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblJumlahTransaksi.Location = new System.Drawing.Point(12, 70);
            this.lblJumlahTransaksi.Name = "lblJumlahTransaksi";
            this.lblJumlahTransaksi.Size = new System.Drawing.Size(154, 17);
            this.lblJumlahTransaksi.TabIndex = 5;
            this.lblJumlahTransaksi.Text = "Jumlah Transaksi: 0";
            // 
            // lblTotalPemasukan
            // 
            this.lblTotalPemasukan.AutoSize = true;
            this.lblTotalPemasukan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalPemasukan.ForeColor = System.Drawing.Color.Green;
            this.lblTotalPemasukan.Location = new System.Drawing.Point(12, 95);
            this.lblTotalPemasukan.Name = "lblTotalPemasukan";
            this.lblTotalPemasukan.Size = new System.Drawing.Size(195, 20);
            this.lblTotalPemasukan.TabIndex = 6;
            this.lblTotalPemasukan.Text = "Total Pemasukan: Rp 0";
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
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.lblInfo.Location = new System.Drawing.Point(12, 120);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(0, 13);
            this.lblInfo.TabIndex = 7;
            // 
            // dgvPemasukan
            // 
            this.dgvPemasukan.Location = new System.Drawing.Point(12, 140);
            this.dgvPemasukan.Name = "dgvPemasukan";
            this.dgvPemasukan.Size = new System.Drawing.Size(650, 200);
            this.dgvPemasukan.TabIndex = 8;
            // 
            // FormPemasukan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 360);
            this.Controls.Add(this.dgvPemasukan);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblTotalPemasukan);
            this.Controls.Add(this.lblJumlahTransaksi);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnCari);
            this.Controls.Add(this.dtpTanggal);
            this.Controls.Add(this.label1);
            this.Name = "FormPemasukan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Total Pemasukan Harian - Sistem Keuangan Angkringan";
            this.Load += new System.EventHandler(this.FormPemasukan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPemasukan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.Button btnCari;
        private System.Windows.Forms.Label lblJumlahTransaksi;
        private System.Windows.Forms.Label lblTotalPemasukan;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.DataGridView dgvPemasukan;
    }
}