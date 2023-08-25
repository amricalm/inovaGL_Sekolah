namespace inovaGL.Laporan
{
    partial class FDlgLapAnggaranRealisasiBulanBerjalan
    {
        /// </summary>
        private System.ComponentModel.IContainer components = null;
       /// <summary>
        /// Required designer variable.
 
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDlgLapAnggaranRealisasiBulanBerjalan));
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.adnNav = new System.Windows.Forms.BindingNavigator(this.components);
            this.btnTutup = new System.Windows.Forms.ToolStripButton();
            this.groupBoxHdr = new System.Windows.Forms.GroupBox();
            this.comboBoxThAjar = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownTahun = new System.Windows.Forms.NumericUpDown();
            this.comboBoxSekolah = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxBulan = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxSaldoNol = new System.Windows.Forms.CheckBox();
            this.buttonTampil = new System.Windows.Forms.Button();
            this.rvw = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.adnNav)).BeginInit();
            this.adnNav.SuspendLayout();
            this.groupBoxHdr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTahun)).BeginInit();
            this.SuspendLayout();
            // 
            // adnNav
            // 
            this.adnNav.AddNewItem = null;
            this.adnNav.CountItem = null;
            this.adnNav.DeleteItem = null;
            this.adnNav.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTutup});
            this.adnNav.Location = new System.Drawing.Point(0, 0);
            this.adnNav.MoveFirstItem = null;
            this.adnNav.MoveLastItem = null;
            this.adnNav.MoveNextItem = null;
            this.adnNav.MovePreviousItem = null;
            this.adnNav.Name = "adnNav";
            this.adnNav.PositionItem = null;
            this.adnNav.Size = new System.Drawing.Size(984, 25);
            this.adnNav.TabIndex = 14;
            this.adnNav.Text = "bindingNavigator1";
            // 
            // btnTutup
            // 
            this.btnTutup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTutup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTutup.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnTutup.Image = ((System.Drawing.Image)(resources.GetObject("btnTutup.Image")));
            this.btnTutup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTutup.Name = "btnTutup";
            this.btnTutup.Size = new System.Drawing.Size(44, 22);
            this.btnTutup.Text = "Tutup";
            this.btnTutup.Click += new System.EventHandler(this.btnTutup_Click);
            // 
            // groupBoxHdr
            // 
            this.groupBoxHdr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxHdr.Controls.Add(this.comboBoxThAjar);
            this.groupBoxHdr.Controls.Add(this.label1);
            this.groupBoxHdr.Controls.Add(this.numericUpDownTahun);
            this.groupBoxHdr.Controls.Add(this.comboBoxSekolah);
            this.groupBoxHdr.Controls.Add(this.label3);
            this.groupBoxHdr.Controls.Add(this.comboBoxBulan);
            this.groupBoxHdr.Controls.Add(this.label8);
            this.groupBoxHdr.Controls.Add(this.checkBoxSaldoNol);
            this.groupBoxHdr.Controls.Add(this.buttonTampil);
            this.groupBoxHdr.Location = new System.Drawing.Point(3, 26);
            this.groupBoxHdr.Name = "groupBoxHdr";
            this.groupBoxHdr.Size = new System.Drawing.Size(980, 124);
            this.groupBoxHdr.TabIndex = 0;
            this.groupBoxHdr.TabStop = false;
            // 
            // comboBoxThAjar
            // 
            this.comboBoxThAjar.FormattingEnabled = true;
            this.comboBoxThAjar.Location = new System.Drawing.Point(114, 53);
            this.comboBoxThAjar.Name = "comboBoxThAjar";
            this.comboBoxThAjar.Size = new System.Drawing.Size(227, 21);
            this.comboBoxThAjar.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(25, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 59;
            this.label1.Text = "Tahun Ajar";
            // 
            // numericUpDownTahun
            // 
            this.numericUpDownTahun.Location = new System.Drawing.Point(114, 81);
            this.numericUpDownTahun.Maximum = new decimal(new int[] {
            2030,
            0,
            0,
            0});
            this.numericUpDownTahun.Minimum = new decimal(new int[] {
            2010,
            0,
            0,
            0});
            this.numericUpDownTahun.Name = "numericUpDownTahun";
            this.numericUpDownTahun.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownTahun.TabIndex = 2;
            this.numericUpDownTahun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownTahun.Value = new decimal(new int[] {
            2010,
            0,
            0,
            0});
            // 
            // comboBoxSekolah
            // 
            this.comboBoxSekolah.FormattingEnabled = true;
            this.comboBoxSekolah.Location = new System.Drawing.Point(114, 26);
            this.comboBoxSekolah.Name = "comboBoxSekolah";
            this.comboBoxSekolah.Size = new System.Drawing.Size(227, 21);
            this.comboBoxSekolah.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(25, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 56;
            this.label3.Text = "Sekolah :";
            // 
            // comboBoxBulan
            // 
            this.comboBoxBulan.FormattingEnabled = true;
            this.comboBoxBulan.Location = new System.Drawing.Point(176, 80);
            this.comboBoxBulan.Name = "comboBoxBulan";
            this.comboBoxBulan.Size = new System.Drawing.Size(165, 21);
            this.comboBoxBulan.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 83);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 52;
            this.label8.Text = "Periode";
            // 
            // checkBoxSaldoNol
            // 
            this.checkBoxSaldoNol.AutoSize = true;
            this.checkBoxSaldoNol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSaldoNol.Location = new System.Drawing.Point(493, 81);
            this.checkBoxSaldoNol.Name = "checkBoxSaldoNol";
            this.checkBoxSaldoNol.Size = new System.Drawing.Size(140, 17);
            this.checkBoxSaldoNol.TabIndex = 50;
            this.checkBoxSaldoNol.Text = "Termasuk Saldo Nol";
            this.checkBoxSaldoNol.UseVisualStyleBackColor = true;
            this.checkBoxSaldoNol.Visible = false;
            // 
            // buttonTampil
            // 
            this.buttonTampil.AutoSize = true;
            this.buttonTampil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonTampil.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonTampil.Image = ((System.Drawing.Image)(resources.GetObject("buttonTampil.Image")));
            this.buttonTampil.Location = new System.Drawing.Point(347, 26);
            this.buttonTampil.Name = "buttonTampil";
            this.buttonTampil.Size = new System.Drawing.Size(35, 37);
            this.buttonTampil.TabIndex = 4;
            this.buttonTampil.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonTampil.UseVisualStyleBackColor = true;
            this.buttonTampil.Click += new System.EventHandler(this.buttonTampil_Click);
            // 
            // rvw
            // 
            this.rvw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Value = null;
            this.rvw.LocalReport.DataSources.Add(reportDataSource1);
            this.rvw.LocalReport.ReportEmbeddedResource = "MgmDonasi.Report1.rdlc";
            this.rvw.Location = new System.Drawing.Point(3, 156);
            this.rvw.Name = "rvw";
            this.rvw.Size = new System.Drawing.Size(980, 505);
            this.rvw.TabIndex = 16;
            // 
            // FDlgLapAnggaranRealisasiBulanBerjalan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 662);
            this.Controls.Add(this.rvw);
            this.Controls.Add(this.groupBoxHdr);
            this.Controls.Add(this.adnNav);
            this.Name = "FDlgLapAnggaranRealisasiBulanBerjalan";
            this.Text = "Realisasi Anggaran Per Bulan";
            ((System.ComponentModel.ISupportInitialize)(this.adnNav)).EndInit();
            this.adnNav.ResumeLayout(false);
            this.adnNav.PerformLayout();
            this.groupBoxHdr.ResumeLayout(false);
            this.groupBoxHdr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTahun)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator adnNav;
        private System.Windows.Forms.ToolStripButton btnTutup;
        private System.Windows.Forms.GroupBox groupBoxHdr;
        private Microsoft.Reporting.WinForms.ReportViewer rvw;
        private System.Windows.Forms.Button buttonTampil;
        private System.Windows.Forms.CheckBox checkBoxSaldoNol;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxBulan;
        private System.Windows.Forms.ComboBox comboBoxSekolah;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownTahun;
        private System.Windows.Forms.ComboBox comboBoxThAjar;
        private System.Windows.Forms.Label label1;
    }
}