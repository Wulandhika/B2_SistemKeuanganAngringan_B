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
            this.btnTransaksi.Location = new System.Drawing.Point(50, 60);
            this.btnTransaksi.Name = "btnTransaksi";
            this.btnTransaksi.Size = new System.Drawing.Size(200, 40);
            this.btnTransaksi.TabIndex = 1;
            this.btnTransaksi.Text = "Pencatatan Transaksi";
            this.btnTransaksi.UseVisualStyleBackColor = true;
            this.btnTransaksi.Click += new System.EventHandler(this.btnTransaksi_Click);
            // 
            // btnRiwayat
            // 
            this.btnRiwayat.Location = new System.Drawing.Point(50, 110);
            this.btnRiwayat.Name = "btnRiwayat";
            this.btnRiwayat.Size = new System.Drawing.Size(200, 40);
            this.btnRiwayat.TabIndex = 2;
            this.btnRiwayat.Text = "Riwayat Transaksi";
            this.btnRiwayat.UseVisualStyleBackColor = true;
            this.btnRiwayat.Click += new System.EventHandler(this.btnRiwayat_Click);
            // 
            // btnPemasukan
            // 
            this.btnPemasukan.Location = new System.Drawing.Point(50, 160);
            this.btnPemasukan.Name = "btnPemasukan";
            this.btnPemasukan.Size = new System.Drawing.Size(200, 40);
            this.btnPemasukan.TabIndex = 3;
            this.btnPemasukan.Text = "Total Pemasukan Harian";
            this.btnPemasukan.UseVisualStyleBackColor = true;
            this.btnPemasukan.Click += new System.EventHandler(this.btnPemasukan_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnMenu.Location = new System.Drawing.Point(50, 210);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(200, 40);
            this.btnMenu.TabIndex = 4;
            this.btnMenu.Text = "Kelola Menu";
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.LightCoral;
            this.btnLogout.Location = new System.Drawing.Point(50, 270);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(200, 40);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // FormUtama
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 350);
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
    }
}