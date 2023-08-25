namespace inovaPOS
{
    partial class FDMGudang
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDMGudang));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.adnNav1 = new Andhana.Control.AdnNav();
            this.toolStripButtonTutup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonTambah = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPilih = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonHapus = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonBatal = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSimpan = new System.Windows.Forms.ToolStripButton();
            this.panelHdr = new System.Windows.Forms.Panel();
            this.groupBoxTbl = new System.Windows.Forms.GroupBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.kd_gudang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nm_gudang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.adnNav1)).BeginInit();
            this.adnNav1.SuspendLayout();
            this.panelHdr.SuspendLayout();
            this.groupBoxTbl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // adnNav1
            // 
            this.adnNav1.AddNewItem = null;
            this.adnNav1.CountItem = null;
            this.adnNav1.DeleteItem = null;
            this.adnNav1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTutup,
            this.toolStripSeparator1,
            this.toolStripButtonTambah,
            this.toolStripSeparator2,
            this.toolStripButtonPilih,
            this.toolStripSeparator5,
            this.toolStripButtonHapus,
            this.toolStripSeparator3,
            this.toolStripButtonBatal,
            this.toolStripSeparator4,
            this.toolStripButtonSimpan});
            this.adnNav1.Location = new System.Drawing.Point(0, 0);
            this.adnNav1.MoveFirstItem = null;
            this.adnNav1.MoveLastItem = null;
            this.adnNav1.MoveNextItem = null;
            this.adnNav1.MovePreviousItem = null;
            this.adnNav1.Name = "adnNav1";
            this.adnNav1.PositionItem = null;
            this.adnNav1.Size = new System.Drawing.Size(802, 25);
            this.adnNav1.TabIndex = 4;
            this.adnNav1.Text = "adnNav1";
            // 
            // toolStripButtonTutup
            // 
            this.toolStripButtonTutup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonTutup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonTutup.ForeColor = System.Drawing.Color.DarkGreen;
            this.toolStripButtonTutup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTutup.Name = "toolStripButtonTutup";
            this.toolStripButtonTutup.Size = new System.Drawing.Size(44, 22);
            this.toolStripButtonTutup.Text = "Tutup";
            this.toolStripButtonTutup.ToolTipText = "Tutup Jendela";
            this.toolStripButtonTutup.Click += new System.EventHandler(this.toolStripButtonTutup_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonTambah
            // 
            this.toolStripButtonTambah.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTambah.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTambah.Image")));
            this.toolStripButtonTambah.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTambah.Name = "toolStripButtonTambah";
            this.toolStripButtonTambah.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTambah.Text = "Tambah";
            this.toolStripButtonTambah.Click += new System.EventHandler(this.toolStripButtonTambah_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonPilih
            // 
            this.toolStripButtonPilih.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonPilih.ForeColor = System.Drawing.Color.Blue;
            this.toolStripButtonPilih.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPilih.Image")));
            this.toolStripButtonPilih.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPilih.Name = "toolStripButtonPilih";
            this.toolStripButtonPilih.Size = new System.Drawing.Size(50, 22);
            this.toolStripButtonPilih.Text = "Pilih";
            this.toolStripButtonPilih.Click += new System.EventHandler(this.toolStripButtonPilih_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonHapus
            // 
            this.toolStripButtonHapus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHapus.Enabled = false;
            this.toolStripButtonHapus.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHapus.Image")));
            this.toolStripButtonHapus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHapus.Name = "toolStripButtonHapus";
            this.toolStripButtonHapus.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHapus.Text = "Hapus";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonBatal
            // 
            this.toolStripButtonBatal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonBatal.Enabled = false;
            this.toolStripButtonBatal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonBatal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.toolStripButtonBatal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBatal.Name = "toolStripButtonBatal";
            this.toolStripButtonBatal.Size = new System.Drawing.Size(40, 22);
            this.toolStripButtonBatal.Text = "Batal";
            this.toolStripButtonBatal.ToolTipText = "Batal";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSimpan
            // 
            this.toolStripButtonSimpan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSimpan.Enabled = false;
            this.toolStripButtonSimpan.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSimpan.Image")));
            this.toolStripButtonSimpan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSimpan.Name = "toolStripButtonSimpan";
            this.toolStripButtonSimpan.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSimpan.Text = "toolStripButtonSimpan";
            this.toolStripButtonSimpan.ToolTipText = "Simpan";
            // 
            // panelHdr
            // 
            this.panelHdr.Controls.Add(this.groupBoxTbl);
            this.panelHdr.Location = new System.Drawing.Point(0, 28);
            this.panelHdr.Name = "panelHdr";
            this.panelHdr.Size = new System.Drawing.Size(799, 517);
            this.panelHdr.TabIndex = 5;
            // 
            // groupBoxTbl
            // 
            this.groupBoxTbl.Controls.Add(this.dgv);
            this.groupBoxTbl.Location = new System.Drawing.Point(3, 0);
            this.groupBoxTbl.Name = "groupBoxTbl";
            this.groupBoxTbl.Size = new System.Drawing.Size(793, 517);
            this.groupBoxTbl.TabIndex = 1;
            this.groupBoxTbl.TabStop = false;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.ColumnHeadersHeight = 25;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.kd_gudang,
            this.nm_gudang});
            this.dgv.Location = new System.Drawing.Point(6, 11);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(781, 399);
            this.dgv.TabIndex = 1;
            // 
            // kd_gudang
            // 
            this.kd_gudang.DataPropertyName = "kd_gudang";
            this.kd_gudang.HeaderText = "Kode Barang";
            this.kd_gudang.Name = "kd_gudang";
            this.kd_gudang.ReadOnly = true;
            // 
            // nm_gudang
            // 
            this.nm_gudang.DataPropertyName = "nm_gudang";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.nm_gudang.DefaultCellStyle = dataGridViewCellStyle1;
            this.nm_gudang.HeaderText = "Nama Gudang";
            this.nm_gudang.Name = "nm_gudang";
            this.nm_gudang.ReadOnly = true;
            this.nm_gudang.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // FDMGudang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 550);
            this.Controls.Add(this.panelHdr);
            this.Controls.Add(this.adnNav1);
            this.Name = "FDMGudang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Daftar Gudang";
            this.Load += new System.EventHandler(this.FDMBarang_Load);
            ((System.ComponentModel.ISupportInitialize)(this.adnNav1)).EndInit();
            this.adnNav1.ResumeLayout(false);
            this.adnNav1.PerformLayout();
            this.panelHdr.ResumeLayout(false);
            this.groupBoxTbl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Andhana.Control.AdnNav adnNav1;
        private System.Windows.Forms.ToolStripButton toolStripButtonTutup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonTambah;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonHapus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonBatal;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonSimpan;
        private System.Windows.Forms.Panel panelHdr;
        private System.Windows.Forms.GroupBox groupBoxTbl;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.ToolStripButton toolStripButtonPilih;
        private System.Windows.Forms.DataGridViewTextBoxColumn kd_gudang;
        private System.Windows.Forms.DataGridViewTextBoxColumn nm_gudang;
    }
}