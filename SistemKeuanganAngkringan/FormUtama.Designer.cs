namespace SistemKeuanganAngkringan
{
    partial class FormUtama
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
            this.lblAdmin = new System.Windows.Forms.Label();
            this.btnTransaksi = new System.Windows.Forms.Button();
            this.btnRiwayat = new System.Windows.Forms.Button();
            this.btnPemasukan = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnLaporanTransaksi = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAdmin
            // 
            this.lblAdmin.AutoSize = true;
            this.lblAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblAdmin.Location = new System.Drawing.Point(50, 20);
            this.lblAdmin.Name = "lblAdmin";
            this.lblAdmin.Size = new System.Drawing.Size(144, 20);
            this.lblAdmin.TabIndex = 0;
            this.lblAdmin.Text = "Selamat Datang,";
            // 
            // btnTransaksi
            // 
            this.btnTransaksi.Location = new System.Drawing.Point(50, 50);
            this.btnTransaksi.Name = "btnTransaksi";
            this.btnTransaksi.Size = new System.Drawing.Size(200, 27);
            this.btnTransaksi.TabIndex = 1;
            this.btnTransaksi.Text = "Pencatatan Transaksi";
            this.btnTransaksi.UseVisualStyleBackColor = true;
            this.btnTransaksi.Click += new System.EventHandler(this.btnTransaksi_Click);
            // 
            // btnRiwayat
            // 
            this.btnRiwayat.Location = new System.Drawing.Point(50, 78);
            this.btnRiwayat.Name = "btnRiwayat";
            this.btnRiwayat.Size = new System.Drawing.Size(200, 27);
            this.btnRiwayat.TabIndex = 2;
            this.btnRiwayat.Text = "Riwayat Transaksi";
            this.btnRiwayat.UseVisualStyleBackColor = true;
            this.btnRiwayat.Click += new System.EventHandler(this.btnRiwayat_Click);
            // 
            // btnPemasukan
            // 
            this.btnPemasukan.Location = new System.Drawing.Point(50, 107);
            this.btnPemasukan.Name = "btnPemasukan";
            this.btnPemasukan.Size = new System.Drawing.Size(200, 29);
            this.btnPemasukan.TabIndex = 3;
            this.btnPemasukan.Text = "Total Pemasukan Harian";
            this.btnPemasukan.UseVisualStyleBackColor = true;
            this.btnPemasukan.Click += new System.EventHandler(this.btnPemasukan_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnMenu.Location = new System.Drawing.Point(50, 136);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(200, 29);
            this.btnMenu.TabIndex = 4;
            this.btnMenu.Text = "Kelola Menu";
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.LightCoral;
            this.btnLogout.Location = new System.Drawing.Point(50, 230);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(200, 40);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnLaporanTransaksi
            // 
            this.btnLaporanTransaksi.Location = new System.Drawing.Point(50, 200);
            this.btnLaporanTransaksi.Name = "btnLaporanTransaksi";
            this.btnLaporanTransaksi.Size = new System.Drawing.Size(200, 27);
            this.btnLaporanTransaksi.TabIndex = 6;
            this.btnLaporanTransaksi.Text = "Laporan Transaksi";
            this.btnLaporanTransaksi.UseVisualStyleBackColor = true;
            this.btnLaporanTransaksi.Click += new System.EventHandler(this.btnLaporanTransaksi_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.Location = new System.Drawing.Point(54, 172);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(196, 23);
            this.btnDashboard.TabIndex = 7;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // FormUtama
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 300);
            this.Controls.Add(this.btnDashboard);
            this.Controls.Add(this.btnLaporanTransaksi);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.btnPemasukan);
            this.Controls.Add(this.btnRiwayat);
            this.Controls.Add(this.btnTransaksi);
            this.Controls.Add(this.lblAdmin);
            this.Name = "FormUtama";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu Utama - Sistem Keuangan Angkringan";
            this.Load += new System.EventHandler(this.FormUtama_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblAdmin;
        private System.Windows.Forms.Button btnTransaksi;
        private System.Windows.Forms.Button btnRiwayat;
        private System.Windows.Forms.Button btnPemasukan;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnLaporanTransaksi;
        private System.Windows.Forms.Button btnDashboard;
    }
}