namespace inovaGL.Piutang
{
    partial class FProsesSalinSaldoPiutangSiswa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FProsesSalinSaldoPiutangSiswa));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHdr = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonProses = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSekolah = new System.Windows.Forms.TextBox();
            this.textBoxNmSiswa = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePickerTgl = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvSiswa = new System.Windows.Forms.DataGridView();
            this.comboBoxKelas = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
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
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.PilihanKdSiswa = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.KolomKdSiswa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KdSaw = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NmLengkap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkBoxSemuaSiswa = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSiswa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adnNav1)).BeginInit();
            this.adnNav1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHdr
            // 
            this.panelHdr.Location = new System.Drawing.Point(0, 28);
            this.panelHdr.Name = "panelHdr";
            this.panelHdr.Size = new System.Drawing.Size(447, 575);
            this.panelHdr.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxSemuaSiswa);
            this.groupBox2.Controls.Add(this.buttonProses);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxSekolah);
            this.groupBox2.Controls.Add(this.textBoxNmSiswa);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.dateTimePickerTgl);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.dgvSiswa);
            this.groupBox2.Controls.Add(this.comboBoxKelas);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(3, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(441, 572);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // buttonProses
            // 
            this.buttonProses.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProses.Location = new System.Drawing.Point(126, 182);
            this.buttonProses.Name = "buttonProses";
            this.buttonProses.Size = new System.Drawing.Size(85, 28);
            this.buttonProses.TabIndex = 50;
            this.buttonProses.Text = "Proses";
            this.buttonProses.UseVisualStyleBackColor = true;
            this.buttonProses.Click += new System.EventHandler(this.buttonProses_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(36, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(248, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "Menjadi Saldo Piutang dari Siswa Berikut :";
            // 
            // textBoxSekolah
            // 
            this.textBoxSekolah.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSekolah.Location = new System.Drawing.Point(126, 130);
            this.textBoxSekolah.Name = "textBoxSekolah";
            this.textBoxSekolah.Size = new System.Drawing.Size(264, 20);
            this.textBoxSekolah.TabIndex = 49;
            // 
            // textBoxNmSiswa
            // 
            this.textBoxNmSiswa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxNmSiswa.Location = new System.Drawing.Point(126, 43);
            this.textBoxNmSiswa.Name = "textBoxNmSiswa";
            this.textBoxNmSiswa.Size = new System.Drawing.Size(264, 20);
            this.textBoxNmSiswa.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(36, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Nama Siswa :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(36, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "Sekolah :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(36, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(188, 13);
            this.label5.TabIndex = 47;
            this.label5.Text = "Salin Saldo Piutang dari Siswa :";
            // 
            // dateTimePickerTgl
            // 
            this.dateTimePickerTgl.Location = new System.Drawing.Point(126, 104);
            this.dateTimePickerTgl.Name = "dateTimePickerTgl";
            this.dateTimePickerTgl.Size = new System.Drawing.Size(147, 20);
            this.dateTimePickerTgl.TabIndex = 0;
            this.dateTimePickerTgl.Value = new System.DateTime(2012, 1, 25, 10, 32, 23, 0);
            this.dateTimePickerTgl.ValueChanged += new System.EventHandler(this.dateTimePickerTgl_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Per Tanggal";
            // 
            // dgvSiswa
            // 
            this.dgvSiswa.AllowUserToAddRows = false;
            this.dgvSiswa.ColumnHeadersHeight = 25;
            this.dgvSiswa.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PilihanKdSiswa,
            this.KolomKdSiswa,
            this.KdSaw,
            this.Nis,
            this.NmLengkap,
            this.Total});
            this.dgvSiswa.Location = new System.Drawing.Point(39, 249);
            this.dgvSiswa.MultiSelect = false;
            this.dgvSiswa.Name = "dgvSiswa";
            this.dgvSiswa.RowHeadersWidth = 25;
            this.dgvSiswa.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSiswa.Size = new System.Drawing.Size(355, 316);
            this.dgvSiswa.TabIndex = 34;
            this.dgvSiswa.SelectionChanged += new System.EventHandler(this.dgvSiswa_SelectionChanged);
            // 
            // comboBoxKelas
            // 
            this.comboBoxKelas.FormattingEnabled = true;
            this.comboBoxKelas.Location = new System.Drawing.Point(126, 155);
            this.comboBoxKelas.Name = "comboBoxKelas";
            this.comboBoxKelas.Size = new System.Drawing.Size(264, 21);
            this.comboBoxKelas.TabIndex = 2;
            this.comboBoxKelas.SelectedIndexChanged += new System.EventHandler(this.comboBoxKelas_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(36, 158);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "Kelas :";
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
            this.toolStripButtonSimpan,
            this.toolStripSeparator6});
            this.adnNav1.Location = new System.Drawing.Point(0, 0);
            this.adnNav1.MoveFirstItem = null;
            this.adnNav1.MoveLastItem = null;
            this.adnNav1.MoveNextItem = null;
            this.adnNav1.MovePreviousItem = null;
            this.adnNav1.Name = "adnNav1";
            this.adnNav1.PositionItem = null;
            this.adnNav1.Size = new System.Drawing.Size(447, 25);
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
            this.toolStripButtonTambah.Enabled = false;
            this.toolStripButtonTambah.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTambah.Image")));
            this.toolStripButtonTambah.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTambah.Name = "toolStripButtonTambah";
            this.toolStripButtonTambah.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTambah.Text = "Tambah";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonPilih
            // 
            this.toolStripButtonPilih.Enabled = false;
            this.toolStripButtonPilih.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonPilih.ForeColor = System.Drawing.Color.Blue;
            this.toolStripButtonPilih.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPilih.Image")));
            this.toolStripButtonPilih.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPilih.Name = "toolStripButtonPilih";
            this.toolStripButtonPilih.Size = new System.Drawing.Size(50, 22);
            this.toolStripButtonPilih.Text = "Pilih";
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
            this.toolStripButtonSimpan.Click += new System.EventHandler(this.toolStripButtonSimpan_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // PilihanKdSiswa
            // 
            this.PilihanKdSiswa.HeaderText = "Pilih";
            this.PilihanKdSiswa.Name = "PilihanKdSiswa";
            this.PilihanKdSiswa.Width = 30;
            // 
            // KolomKdSiswa
            // 
            this.KolomKdSiswa.DataPropertyName = "KdSiswa";
            this.KolomKdSiswa.HeaderText = "Kode";
            this.KolomKdSiswa.Name = "KolomKdSiswa";
            this.KolomKdSiswa.ReadOnly = true;
            this.KolomKdSiswa.Visible = false;
            this.KolomKdSiswa.Width = 80;
            // 
            // KdSaw
            // 
            this.KdSaw.DataPropertyName = "KdSaw";
            this.KdSaw.HeaderText = "KdSaw";
            this.KdSaw.Name = "KdSaw";
            this.KdSaw.Visible = false;
            // 
            // Nis
            // 
            this.Nis.DataPropertyName = "Nis";
            this.Nis.HeaderText = "Nis";
            this.Nis.Name = "Nis";
            this.Nis.Visible = false;
            // 
            // NmLengkap
            // 
            this.NmLengkap.DataPropertyName = "NmLengkap";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.NmLengkap.DefaultCellStyle = dataGridViewCellStyle1;
            this.NmLengkap.HeaderText = "Nama Siswa";
            this.NmLengkap.Name = "NmLengkap";
            this.NmLengkap.ReadOnly = true;
            this.NmLengkap.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NmLengkap.Width = 180;
            // 
            // Total
            // 
            this.Total.DataPropertyName = "Total";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = "N0";
            this.Total.DefaultCellStyle = dataGridViewCellStyle2;
            this.Total.HeaderText = "Total";
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            this.Total.Width = 80;
            // 
            // checkBoxSemuaSiswa
            // 
            this.checkBoxSemuaSiswa.AutoSize = true;
            this.checkBoxSemuaSiswa.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSemuaSiswa.Location = new System.Drawing.Point(73, 226);
            this.checkBoxSemuaSiswa.Name = "checkBoxSemuaSiswa";
            this.checkBoxSemuaSiswa.Size = new System.Drawing.Size(100, 17);
            this.checkBoxSemuaSiswa.TabIndex = 51;
            this.checkBoxSemuaSiswa.Text = "Semua Siswa";
            this.checkBoxSemuaSiswa.UseVisualStyleBackColor = true;
            this.checkBoxSemuaSiswa.CheckedChanged += new System.EventHandler(this.checkBoxSemuaSiswa_CheckedChanged);
            // 
            // FProsesSalinSaldoPiutangSiswa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 605);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panelHdr);
            this.Controls.Add(this.adnNav1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Name = "FProsesSalinSaldoPiutangSiswa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Salin Saldo Piutang Siswa";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSiswa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adnNav1)).EndInit();
            this.adnNav1.ResumeLayout(false);
            this.adnNav1.PerformLayout();
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
        private System.Windows.Forms.ToolStripButton toolStripButtonPilih;
        private System.Windows.Forms.ComboBox comboBoxKelas;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxNmSiswa;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvSiswa;
        private System.Windows.Forms.DateTimePicker dateTimePickerTgl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonProses;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSekolah;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PilihanKdSiswa;
        private System.Windows.Forms.DataGridViewTextBoxColumn KolomKdSiswa;
        private System.Windows.Forms.DataGridViewTextBoxColumn KdSaw;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nis;
        private System.Windows.Forms.DataGridViewTextBoxColumn NmLengkap;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total;
        private System.Windows.Forms.CheckBox checkBoxSemuaSiswa;
    }
}