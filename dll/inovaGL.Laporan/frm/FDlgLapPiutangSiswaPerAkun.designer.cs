﻿namespace inovaGL.Laporan
{
    partial class FDlgLapPiutangSiswaPerAkun
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDlgLapPiutangSiswaPerAkun));
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.adnNav = new System.Windows.Forms.BindingNavigator(this.components);
            this.btnTutup = new System.Windows.Forms.ToolStripButton();
            this.groupBoxHdr = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxSekolah = new System.Windows.Forms.ComboBox();
            this.dateTimePickerDr = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonTampil = new System.Windows.Forms.Button();
            this.rvw = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.adnNav)).BeginInit();
            this.adnNav.SuspendLayout();
            this.groupBoxHdr.SuspendLayout();
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
            this.groupBoxHdr.Controls.Add(this.label3);
            this.groupBoxHdr.Controls.Add(this.comboBoxSekolah);
            this.groupBoxHdr.Controls.Add(this.dateTimePickerDr);
            this.groupBoxHdr.Controls.Add(this.label1);
            this.groupBoxHdr.Controls.Add(this.buttonTampil);
            this.groupBoxHdr.Location = new System.Drawing.Point(3, 26);
            this.groupBoxHdr.Name = "groupBoxHdr";
            this.groupBoxHdr.Size = new System.Drawing.Size(980, 124);
            this.groupBoxHdr.TabIndex = 15;
            this.groupBoxHdr.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "Sekolah";
            this.label3.Visible = false;
            // 
            // comboBoxSekolah
            // 
            this.comboBoxSekolah.FormattingEnabled = true;
            this.comboBoxSekolah.Location = new System.Drawing.Point(131, 54);
            this.comboBoxSekolah.Name = "comboBoxSekolah";
            this.comboBoxSekolah.Size = new System.Drawing.Size(295, 21);
            this.comboBoxSekolah.TabIndex = 3;
            this.comboBoxSekolah.Visible = false;
            // 
            // dateTimePickerDr
            // 
            this.dateTimePickerDr.Location = new System.Drawing.Point(131, 28);
            this.dateTimePickerDr.Name = "dateTimePickerDr";
            this.dateTimePickerDr.Size = new System.Drawing.Size(132, 20);
            this.dateTimePickerDr.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Per Tanggal";
            // 
            // buttonTampil
            // 
            this.buttonTampil.AutoSize = true;
            this.buttonTampil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonTampil.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonTampil.Image = ((System.Drawing.Image)(resources.GetObject("buttonTampil.Image")));
            this.buttonTampil.Location = new System.Drawing.Point(441, 29);
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
            // FDlgLapPiutangSiswaPerAkun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 662);
            this.Controls.Add(this.rvw);
            this.Controls.Add(this.groupBoxHdr);
            this.Controls.Add(this.adnNav);
            this.Name = "FDlgLapPiutangSiswaPerAkun";
            this.Text = "Daftar Transaksi";
            ((System.ComponentModel.ISupportInitialize)(this.adnNav)).EndInit();
            this.adnNav.ResumeLayout(false);
            this.adnNav.PerformLayout();
            this.groupBoxHdr.ResumeLayout(false);
            this.groupBoxHdr.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator adnNav;
        private System.Windows.Forms.ToolStripButton btnTutup;
        private System.Windows.Forms.GroupBox groupBoxHdr;
        private Microsoft.Reporting.WinForms.ReportViewer rvw;
        private System.Windows.Forms.Button buttonTampil;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerDr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxSekolah;
    }
}