namespace SistemKeuanganAngkringan
{
    partial class FormImportExcel
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
            this.btnPilihFile = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnBatal = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPilihFile
            // 
            this.btnPilihFile.BackColor = System.Drawing.Color.LightBlue;
            this.btnPilihFile.Location = new System.Drawing.Point(12, 12);
            this.btnPilihFile.Name = "btnPilihFile";
            this.btnPilihFile.Size = new System.Drawing.Size(120, 30);
            this.btnPilihFile.TabIndex = 0;
            this.btnPilihFile.Text = "Pilih File Excel";
            this.btnPilihFile.UseVisualStyleBackColor = false;
            this.btnPilihFile.Click += new System.EventHandler(this.btnPilihFile_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(140, 18);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(450, 20);
            this.txtFilePath.TabIndex = 1;
            // 
            // dgvPreview
            // 
            this.dgvPreview.Location = new System.Drawing.Point(12, 50);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.Size = new System.Drawing.Size(650, 250);
            this.dgvPreview.TabIndex = 2;
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.LightGreen;
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(12, 320);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(150, 35);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import ke Database";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnBatal
            // 
            this.btnBatal.BackColor = System.Drawing.Color.LightCoral;
            this.btnBatal.Location = new System.Drawing.Point(180, 320);
            this.btnBatal.Name = "btnBatal";
            this.btnBatal.Size = new System.Drawing.Size(100, 35);
            this.btnBatal.TabIndex = 4;
            this.btnBatal.Text = "Batal";
            this.btnBatal.UseVisualStyleBackColor = false;
            this.btnBatal.Click += new System.EventHandler(this.btnBatal_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(300, 330);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(75, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Status: Siap";
            // 
            // FormImportExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 370);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnBatal);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.dgvPreview);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnPilihFile);
            this.Name = "FormImportExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Data Excel";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnPilihFile;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.DataGridView dgvPreview;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnBatal;
        private System.Windows.Forms.Label lblStatus;
    }
}