namespace EDUSIS.Biaya
{
    partial class FDMBiaya
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDMBiaya));
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
            this.groupBoxHdr = new System.Windows.Forms.GroupBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.KdBiaya = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NmBiaya = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KdJenis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KdAkunPiutang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KdAkunPendapatan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KdAkunKewajiban = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KdAkunDeposit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.adnNav1)).BeginInit();
            this.adnNav1.SuspendLayout();
            this.panelHdr.SuspendLayout();
            this.groupBoxHdr.SuspendLayout();
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
            this.adnNav1.Size = new System.Drawing.Size(834, 25);
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
            this.panelHdr.Controls.Add(this.groupBoxHdr);
            this.panelHdr.Location = new System.Drawing.Point(0, 28);
            this.panelHdr.Name = "panelHdr";
            this.panelHdr.Size = new System.Drawing.Size(834, 517);
            this.panelHdr.TabIndex = 5;
            // 
            // groupBoxHdr
            // 
            this.groupBoxHdr.Controls.Add(this.dgv);
            this.groupBoxHdr.Location = new System.Drawing.Point(3, 0);
            this.groupBoxHdr.Name = "groupBoxHdr";
            this.groupBoxHdr.Size = new System.Drawing.Size(828, 517);
            this.groupBoxHdr.TabIndex = 1;
            this.groupBoxHdr.TabStop = false;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.ColumnHeadersHeight = 25;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KdBiaya,
            this.NmBiaya,
            this.KdJenis,
            this.KdAkunPiutang,
            this.KdAkunPendapatan,
            this.KdAkunKewajiban,
            this.KdAkunDeposit});
            this.dgv.Location = new System.Drawing.Point(6, 11);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(816, 499);
            this.dgv.TabIndex = 1;
            // 
            // KdBiaya
            // 
            this.KdBiaya.DataPropertyName = "KdBiaya";
            this.KdBiaya.HeaderText = "Kode";
            this.KdBiaya.Name = "KdBiaya";
            this.KdBiaya.ReadOnly = true;
            this.KdBiaya.Width = 70;
            // 
            // NmBiaya
            // 
            this.NmBiaya.DataPropertyName = "NmBiaya";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.NmBiaya.DefaultCellStyle = dataGridViewCellStyle1;
            this.NmBiaya.HeaderText = "Nama Biaya";
            this.NmBiaya.Name = "NmBiaya";
            this.NmBiaya.ReadOnly = true;
            this.NmBiaya.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NmBiaya.Width = 140;
            // 
            // KdJenis
            // 
            this.KdJenis.DataPropertyName = "KdJenis";
            this.KdJenis.HeaderText = "Jenis";
            this.KdJenis.Name = "KdJenis";
            this.KdJenis.ReadOnly = true;
            // 
            // KdAkunPiutang
            // 
            this.KdAkunPiutang.DataPropertyName = "KdAkunPiutang";
            this.KdAkunPiutang.HeaderText = "Akun Piutang";
            this.KdAkunPiutang.Name = "KdAkunPiutang";
            this.KdAkunPiutang.ReadOnly = true;
            this.KdAkunPiutang.Width = 120;
            // 
            // KdAkunPendapatan
            // 
            this.KdAkunPendapatan.DataPropertyName = "KdAkunPendapatan";
            this.KdAkunPendapatan.HeaderText = "Akun Pendapatan";
            this.KdAkunPendapatan.Name = "KdAkunPendapatan";
            this.KdAkunPendapatan.Width = 120;
            // 
            // KdAkunKewajiban
            // 
            this.KdAkunKewajiban.DataPropertyName = "KdAkunKewajiban";
            this.KdAkunKewajiban.HeaderText = "Akun Kewajiban";
            this.KdAkunKewajiban.Name = "KdAkunKewajiban";
            this.KdAkunKewajiban.Width = 120;
            // 
            // KdAkunDeposit
            // 
            this.KdAkunDeposit.DataPropertyName = "KdAkunDeposit";
            this.KdAkunDeposit.HeaderText = "Akun Deposit";
            this.KdAkunDeposit.Name = "KdAkunDeposit";
            this.KdAkunDeposit.ReadOnly = true;
            // 
            // FDMBiaya
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 550);
            this.Controls.Add(this.panelHdr);
            this.Controls.Add(this.adnNav1);
            this.Name = "FDMBiaya";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Daftar Biaya";
            this.Load += new System.EventHandler(this.FDTReceipt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.adnNav1)).EndInit();
            this.adnNav1.ResumeLayout(false);
            this.adnNav1.PerformLayout();
            this.panelHdr.ResumeLayout(false);
            this.groupBoxHdr.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBoxHdr;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.ToolStripButton toolStripButtonPilih;
        private System.Windows.Forms.DataGridViewTextBoxColumn KdBiaya;
        private System.Windows.Forms.DataGridViewTextBoxColumn NmBiaya;
        private System.Windows.Forms.DataGridViewTextBoxColumn KdJenis;
        private System.Windows.Forms.DataGridViewTextBoxColumn KdAkunPiutang;
        private System.Windows.Forms.DataGridViewTextBoxColumn KdAkunPendapatan;
        private System.Windows.Forms.DataGridViewTextBoxColumn KdAkunKewajiban;
        private System.Windows.Forms.DataGridViewTextBoxColumn KdAkunDeposit;
    }
}