namespace inovaPOS
{
    partial class FMGudang
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMGudang));
            this.adnNavMenu = new Andhana.Control.AdnNav();
            this.toolStripButtonTutup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonTambah = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonHapus = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonBatal = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSimpan = new System.Windows.Forms.ToolStripButton();
            this.panelHdr = new System.Windows.Forms.Panel();
            this.textBoxNm = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxKd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelTbl = new System.Windows.Forms.Panel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.kd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.adnNavMenu)).BeginInit();
            this.adnNavMenu.SuspendLayout();
            this.panelHdr.SuspendLayout();
            this.panelTbl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // adnNavMenu
            // 
            this.adnNavMenu.AddNewItem = null;
            this.adnNavMenu.CountItem = null;
            this.adnNavMenu.DeleteItem = null;
            this.adnNavMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTutup,
            this.toolStripSeparator1,
            this.toolStripButtonTambah,
            this.toolStripSeparator5,
            this.toolStripButtonEdit,
            this.toolStripSeparator2,
            this.toolStripButtonHapus,
            this.toolStripSeparator3,
            this.toolStripButtonBatal,
            this.toolStripSeparator4,
            this.toolStripButtonSimpan});
            this.adnNavMenu.Location = new System.Drawing.Point(0, 0);
            this.adnNavMenu.MoveFirstItem = null;
            this.adnNavMenu.MoveLastItem = null;
            this.adnNavMenu.MoveNextItem = null;
            this.adnNavMenu.MovePreviousItem = null;
            this.adnNavMenu.Name = "adnNavMenu";
            this.adnNavMenu.PositionItem = null;
            this.adnNavMenu.Size = new System.Drawing.Size(621, 25);
            this.adnNavMenu.TabIndex = 1;
            this.adnNavMenu.Text = "adnNav1";
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
            this.toolStripButtonTambah.Enabled = false;
            this.toolStripButtonTambah.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTambah.Image")));
            this.toolStripButtonTambah.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTambah.Name = "toolStripButtonTambah";
            this.toolStripButtonTambah.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTambah.Text = "Tambah";
            this.toolStripButtonTambah.Click += new System.EventHandler(this.toolStripButtonTambah_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonEdit
            // 
            this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEdit.Enabled = false;
            this.toolStripButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEdit.Image")));
            this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdit.Name = "toolStripButtonEdit";
            this.toolStripButtonEdit.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEdit.Text = "Edit";
            this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonHapus
            // 
            this.toolStripButtonHapus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHapus.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHapus.Image")));
            this.toolStripButtonHapus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHapus.Name = "toolStripButtonHapus";
            this.toolStripButtonHapus.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHapus.Text = "Hapus";
            this.toolStripButtonHapus.Click += new System.EventHandler(this.toolStripButtonHapus_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonBatal
            // 
            this.toolStripButtonBatal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonBatal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonBatal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.toolStripButtonBatal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBatal.Name = "toolStripButtonBatal";
            this.toolStripButtonBatal.Size = new System.Drawing.Size(40, 22);
            this.toolStripButtonBatal.Text = "Batal";
            this.toolStripButtonBatal.ToolTipText = "Batal";
            this.toolStripButtonBatal.Click += new System.EventHandler(this.toolStripButtonBatal_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSimpan
            // 
            this.toolStripButtonSimpan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSimpan.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSimpan.Image")));
            this.toolStripButtonSimpan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSimpan.Name = "toolStripButtonSimpan";
            this.toolStripButtonSimpan.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSimpan.Text = "toolStripButtonSimpan";
            this.toolStripButtonSimpan.ToolTipText = "Simpan";
            this.toolStripButtonSimpan.Click += new System.EventHandler(this.toolStripButtonSimpan_Click);
            // 
            // panelHdr
            // 
            this.panelHdr.Controls.Add(this.textBoxNm);
            this.panelHdr.Controls.Add(this.label2);
            this.panelHdr.Controls.Add(this.textBoxKd);
            this.panelHdr.Controls.Add(this.label1);
            this.panelHdr.Location = new System.Drawing.Point(0, 28);
            this.panelHdr.Name = "panelHdr";
            this.panelHdr.Size = new System.Drawing.Size(620, 112);
            this.panelHdr.TabIndex = 2;
            // 
            // textBoxNm
            // 
            this.textBoxNm.Location = new System.Drawing.Point(160, 61);
            this.textBoxNm.MaxLength = 20;
            this.textBoxNm.Name = "textBoxNm";
            this.textBoxNm.Size = new System.Drawing.Size(249, 20);
            this.textBoxNm.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nama Gudang";
            // 
            // textBoxKd
            // 
            this.textBoxKd.Enabled = false;
            this.textBoxKd.Location = new System.Drawing.Point(160, 35);
            this.textBoxKd.MaxLength = 3;
            this.textBoxKd.Name = "textBoxKd";
            this.textBoxKd.Size = new System.Drawing.Size(71, 20);
            this.textBoxKd.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kode";
            // 
            // panelTbl
            // 
            this.panelTbl.Controls.Add(this.dgv);
            this.panelTbl.Location = new System.Drawing.Point(1, 146);
            this.panelTbl.Name = "panelTbl";
            this.panelTbl.Size = new System.Drawing.Size(620, 205);
            this.panelTbl.TabIndex = 3;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeight = 25;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.kd,
            this.nm});
            this.dgv.Location = new System.Drawing.Point(55, 3);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(509, 197);
            this.dgv.TabIndex = 5;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
            this.dgv.CurrentCellChanged += new System.EventHandler(this.dgv_CurrentCellChanged);
            // 
            // kd
            // 
            this.kd.HeaderText = "Kode";
            this.kd.Name = "kd";
            this.kd.ReadOnly = true;
            // 
            // nm
            // 
            this.nm.HeaderText = "Nama";
            this.nm.Name = "nm";
            this.nm.ReadOnly = true;
            this.nm.Width = 350;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Black;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 373);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(621, 22);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(457, 17);
            this.toolStripStatusLabel1.Text = "ESC: Tutup  |  F10: Edit  | F11: Tambah  | F12: Simpan  |  CTL+D: Hapus  |  CTL+N" +
                ": Batal";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FMGudang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(621, 395);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelTbl);
            this.Controls.Add(this.panelHdr);
            this.Controls.Add(this.adnNavMenu);
            this.KeyPreview = true;
            this.Name = "FMGudang";
            this.Text = "Gudang";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FMSatuan_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.adnNavMenu)).EndInit();
            this.adnNavMenu.ResumeLayout(false);
            this.adnNavMenu.PerformLayout();
            this.panelHdr.ResumeLayout(false);
            this.panelHdr.PerformLayout();
            this.panelTbl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Andhana.Control.AdnNav adnNavMenu;
        private System.Windows.Forms.ToolStripButton toolStripButtonTutup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonTambah;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonHapus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonBatal;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonSimpan;
        private System.Windows.Forms.Panel panelHdr;
        private System.Windows.Forms.TextBox textBoxNm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxKd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
        private System.Windows.Forms.Panel panelTbl;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn kd;
        private System.Windows.Forms.DataGridViewTextBoxColumn nm;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}
