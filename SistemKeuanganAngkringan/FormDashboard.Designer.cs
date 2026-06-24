namespace SistemKeuanganAngkringan
{
    partial class FormDashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.lblTotalPemasukan = new System.Windows.Forms.Label();
            this.lblJumlahTransaksi = new System.Windows.Forms.Label();
            this.lblPeriode = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.chartPemasukan = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dtpTanggal = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartPemasukan)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTotalPemasukan
            // 
            this.lblTotalPemasukan.AutoSize = true;
            this.lblTotalPemasukan.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalPemasukan.ForeColor = System.Drawing.Color.Green;
            this.lblTotalPemasukan.Location = new System.Drawing.Point(30, 20);
            this.lblTotalPemasukan.Name = "lblTotalPemasukan";
            this.lblTotalPemasukan.Size = new System.Drawing.Size(178, 29);
            this.lblTotalPemasukan.TabIndex = 0;
            this.lblTotalPemasukan.Text = "Rp 0";
            // 
            // lblJumlahTransaksi
            // 
            this.lblJumlahTransaksi.AutoSize = true;
            this.lblJumlahTransaksi.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblJumlahTransaksi.Location = new System.Drawing.Point(30, 55);
            this.lblJumlahTransaksi.Name = "lblJumlahTransaksi";
            this.lblJumlahTransaksi.Size = new System.Drawing.Size(152, 24);
            this.lblJumlahTransaksi.TabIndex = 1;
            this.lblJumlahTransaksi.Text = "0 Transaksi";
            // 
            // lblPeriode
            // 
            this.lblPeriode.AutoSize = true;
            this.lblPeriode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblPeriode.Location = new System.Drawing.Point(30, 100);
            this.lblPeriode.Name = "lblPeriode";
            this.lblPeriode.Size = new System.Drawing.Size(145, 17);
            this.lblPeriode.TabIndex = 2;
            this.lblPeriode.Text = "Periode: -";
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.LightBlue;
            this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnLoad.Location = new System.Drawing.Point(350, 130);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(120, 30);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Muat Data";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // chartPemasukan
            // 
            chartArea1.AxisX.Title = "Periode";
            chartArea1.AxisY.Title = "Pemasukan (Rp)";
            chartArea1.Name = "ChartArea1";
            this.chartPemasukan.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartPemasukan.Legends.Add(legend1);
            this.chartPemasukan.Location = new System.Drawing.Point(30, 180);
            this.chartPemasukan.Name = "chartPemasukan";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            series1.IsValueShownAsLabel = true;
            series1.Legend = "Legend1";
            series1.Name = "SeriesPemasukan";
            this.chartPemasukan.Series.Add(series1);
            this.chartPemasukan.Size = new System.Drawing.Size(700, 350);
            this.chartPemasukan.TabIndex = 5;
            this.chartPemasukan.Text = "chartPemasukan";
            // 
            // dtpTanggal
            // 
            this.dtpTanggal.CustomFormat = "dd MMMM yyyy";
            this.dtpTanggal.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTanggal.Location = new System.Drawing.Point(130, 133);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(180, 20);
            this.dtpTanggal.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Pilih Tanggal : ";
            // 
            // FormDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 550);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpTanggal);
            this.Controls.Add(this.chartPemasukan);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.lblPeriode);
            this.Controls.Add(this.lblJumlahTransaksi);
            this.Controls.Add(this.lblTotalPemasukan);
            this.Name = "FormDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard Pemasukan";
            this.Load += new System.EventHandler(this.FormDashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartPemasukan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTotalPemasukan;
        private System.Windows.Forms.Label lblJumlahTransaksi;
        private System.Windows.Forms.Label lblPeriode;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPemasukan;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.Label label1;
    }
}