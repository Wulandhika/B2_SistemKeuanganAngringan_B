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
            this.dgvTransaksi = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransaksi)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pilih Tanggal:";
            // 
            // dtpTanggal
            // 
            this.dtpTanggal.Location = new System.Drawing.Point(95, 17);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(150, 20);
            this.dtpTanggal.TabIndex = 1;
            // 
            // btnCari
            // 
            this.btnCari.Location = new System.Drawing.Point(260, 15);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(75, 23);
            this.btnCari.TabIndex = 2;
            this.btnCari.Text = "Cari";
            this.btnCari.UseVisualStyleBackColor = true;
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // lblJumlahTransaksi
            // 
            this.lblJumlahTransaksi.AutoSize = true;
            this.lblJumlahTransaksi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblJumlahTransaksi.Location = new System.Drawing.Point(12, 60);
            this.lblJumlahTransaksi.Name = "lblJumlahTransaksi";
            this.lblJumlahTransaksi.Size = new System.Drawing.Size(154, 17);
            this.lblJumlahTransaksi.TabIndex = 3;
            this.lblJumlahTransaksi.Text = "Jumlah Transaksi: 0";
            // 
            // lblTotalPemasukan
            // 
            this.lblTotalPemasukan.AutoSize = true;
            this.lblTotalPemasukan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalPemasukan.ForeColor = System.Drawing.Color.Green;
            this.lblTotalPemasukan.Location = new System.Drawing.Point(12, 90);
            this.lblTotalPemasukan.Name = "lblTotalPemasukan";
            this.lblTotalPemasukan.Size = new System.Drawing.Size(178, 20);
            this.lblTotalPemasukan.TabIndex = 4;
            this.lblTotalPemasukan.Text = "Total Pemasukan: Rp 0";
            // 
            // dgvTransaksi
            // 
            this.dgvTransaksi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransaksi.Location = new System.Drawing.Point(12, 130);
            this.dgvTransaksi.Name = "dgvTransaksi";
            this.dgvTransaksi.Size = new System.Drawing.Size(500, 250);
            this.dgvTransaksi.TabIndex = 5;
            // 
            // FormPemasukan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 400);
            this.Controls.Add(this.dgvTransaksi);
            this.Controls.Add(this.lblTotalPemasukan);
            this.Controls.Add(this.lblJumlahTransaksi);
            this.Controls.Add(this.btnCari);
            this.Controls.Add(this.dtpTanggal);
            this.Controls.Add(this.label1);
            this.Name = "FormPemasukan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Total Pemasukan Harian - Angkringan";
            this.Load += new System.EventHandler(this.FormPemasukan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransaksi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.Button btnCari;
        private System.Windows.Forms.Label lblJumlahTransaksi;
        private System.Windows.Forms.Label lblTotalPemasukan;
        private System.Windows.Forms.DataGridView dgvTransaksi;
    }
}