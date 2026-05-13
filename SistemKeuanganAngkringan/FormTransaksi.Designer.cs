namespace SistemKeuanganAngkringan
{
    partial class FormTransaksi
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransaksi));
            this.dgvMenu = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.nudJumlah = new System.Windows.Forms.NumericUpDown();
            this.btnTambah = new System.Windows.Forms.Button();
            this.lbCart = new System.Windows.Forms.ListBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnSimpan = new System.Windows.Forms.Button();
            this.btnBatal = new System.Windows.Forms.Button();
            this.btnHapusItem = new System.Windows.Forms.Button();
            this.btnRefreshMenu = new System.Windows.Forms.Button();
            this.lblInfoMenu = new System.Windows.Forms.Label();
            this.lblInfoCart = new System.Windows.Forms.Label();
            this.bindingNavigatorMenu = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudJumlah)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorMenu)).BeginInit();
            this.bindingNavigatorMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMenu
            // 
            this.dgvMenu.Location = new System.Drawing.Point(12, 60);
            this.dgvMenu.Name = "dgvMenu";
            this.dgvMenu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMenu.Size = new System.Drawing.Size(380, 200);
            this.dgvMenu.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(410, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Jumlah :";
            // 
            // nudJumlah
            // 
            this.nudJumlah.Location = new System.Drawing.Point(462, 63);
            this.nudJumlah.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudJumlah.Name = "nudJumlah";
            this.nudJumlah.Size = new System.Drawing.Size(60, 20);
            this.nudJumlah.TabIndex = 3;
            this.nudJumlah.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnTambah
            // 
            this.btnTambah.BackColor = System.Drawing.Color.LightBlue;
            this.btnTambah.Location = new System.Drawing.Point(410, 95);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(130, 30);
            this.btnTambah.TabIndex = 4;
            this.btnTambah.Text = "Tambah ke Keranjang";
            this.btnTambah.UseVisualStyleBackColor = false;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // lbCart
            // 
            this.lbCart.FormattingEnabled = true;
            this.lbCart.Location = new System.Drawing.Point(12, 270);
            this.lbCart.Name = "lbCart";
            this.lbCart.Size = new System.Drawing.Size(380, 160);
            this.lbCart.TabIndex = 7;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.Green;
            this.lblTotal.Location = new System.Drawing.Point(398, 318);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(200, 35);
            this.lblTotal.TabIndex = 10;
            this.lblTotal.Text = "Total: Rp 0";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSimpan
            // 
            this.btnSimpan.BackColor = System.Drawing.Color.LightGreen;
            this.btnSimpan.Location = new System.Drawing.Point(410, 370);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(130, 40);
            this.btnSimpan.TabIndex = 11;
            this.btnSimpan.Text = "Simpan Transaksi";
            this.btnSimpan.UseVisualStyleBackColor = false;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // btnBatal
            // 
            this.btnBatal.BackColor = System.Drawing.Color.LightCoral;
            this.btnBatal.Location = new System.Drawing.Point(410, 420);
            this.btnBatal.Name = "btnBatal";
            this.btnBatal.Size = new System.Drawing.Size(130, 40);
            this.btnBatal.TabIndex = 12;
            this.btnBatal.Text = "Batal";
            this.btnBatal.UseVisualStyleBackColor = false;
            this.btnBatal.Click += new System.EventHandler(this.btnBatal_Click);
            // 
            // btnHapusItem
            // 
            this.btnHapusItem.BackColor = System.Drawing.Color.LightCoral;
            this.btnHapusItem.Location = new System.Drawing.Point(410, 270);
            this.btnHapusItem.Name = "btnHapusItem";
            this.btnHapusItem.Size = new System.Drawing.Size(130, 30);
            this.btnHapusItem.TabIndex = 9;
            this.btnHapusItem.Text = "Hapus Item";
            this.btnHapusItem.UseVisualStyleBackColor = false;
            this.btnHapusItem.Click += new System.EventHandler(this.btnHapusItem_Click);
            // 
            // btnRefreshMenu
            // 
            this.btnRefreshMenu.BackColor = System.Drawing.Color.LightYellow;
            this.btnRefreshMenu.Location = new System.Drawing.Point(410, 135);
            this.btnRefreshMenu.Name = "btnRefreshMenu";
            this.btnRefreshMenu.Size = new System.Drawing.Size(130, 25);
            this.btnRefreshMenu.TabIndex = 5;
            this.btnRefreshMenu.Text = "Refresh Menu";
            this.btnRefreshMenu.UseVisualStyleBackColor = false;
            this.btnRefreshMenu.Click += new System.EventHandler(this.btnRefreshMenu_Click);
            // 
            // lblInfoMenu
            // 
            this.lblInfoMenu.AutoSize = true;
            this.lblInfoMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.lblInfoMenu.Location = new System.Drawing.Point(12, 45);
            this.lblInfoMenu.Name = "lblInfoMenu";
            this.lblInfoMenu.Size = new System.Drawing.Size(73, 13);
            this.lblInfoMenu.TabIndex = 6;
            this.lblInfoMenu.Text = "Total Menu: 0";
            // 
            // lblInfoCart
            // 
            this.lblInfoCart.AutoSize = true;
            this.lblInfoCart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.lblInfoCart.Location = new System.Drawing.Point(12, 255);
            this.lblInfoCart.Name = "lblInfoCart";
            this.lblInfoCart.Size = new System.Drawing.Size(100, 13);
            this.lblInfoCart.TabIndex = 8;
            this.lblInfoCart.Text = "Item di keranjang: 0";
            // 
            // bindingNavigatorMenu
            // 
            this.bindingNavigatorMenu.AddNewItem = this.bindingNavigatorAddNewItem;
            this.bindingNavigatorMenu.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigatorMenu.DeleteItem = this.bindingNavigatorDeleteItem;
            this.bindingNavigatorMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.bindingNavigatorMenu.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigatorMenu.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigatorMenu.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigatorMenu.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigatorMenu.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigatorMenu.Name = "bindingNavigatorMenu";
            this.bindingNavigatorMenu.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigatorMenu.Size = new System.Drawing.Size(684, 25);
            this.bindingNavigatorMenu.TabIndex = 13;
            this.bindingNavigatorMenu.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // FormTransaksi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 480);
            this.Controls.Add(this.bindingNavigatorMenu);
            this.Controls.Add(this.btnBatal);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnHapusItem);
            this.Controls.Add(this.lblInfoCart);
            this.Controls.Add(this.lbCart);
            this.Controls.Add(this.lblInfoMenu);
            this.Controls.Add(this.btnRefreshMenu);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.nudJumlah);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvMenu);
            this.Name = "FormTransaksi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pencatatan Transaksi - Sistem Keuangan Angkringan";
            this.Load += new System.EventHandler(this.FormTransaksi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudJumlah)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorMenu)).EndInit();
            this.bindingNavigatorMenu.ResumeLayout(false);
            this.bindingNavigatorMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.DataGridView dgvMenu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudJumlah;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.ListBox lbCart;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnSimpan;
        private System.Windows.Forms.Button btnBatal;
        private System.Windows.Forms.Button btnHapusItem;
        private System.Windows.Forms.Button btnRefreshMenu;
        private System.Windows.Forms.Label lblInfoMenu;
        private System.Windows.Forms.Label lblInfoCart;
        private System.Windows.Forms.BindingNavigator bindingNavigatorMenu;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
    }
}