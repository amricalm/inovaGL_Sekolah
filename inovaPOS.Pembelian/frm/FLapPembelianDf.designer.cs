namespace inovaPOS
{
    partial class FLapPembelianDf
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.rvw = new Microsoft.Reporting.WinForms.ReportViewer();
            this.groupBoxHdr = new System.Windows.Forms.GroupBox();
            this.dateTimePickerDr = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonTampil = new System.Windows.Forms.Button();
            this.dateTimePickerSd = new System.Windows.Forms.DateTimePicker();
            this.groupBoxHdr.SuspendLayout();
            this.SuspendLayout();
            // 
            // rvw
            // 
            this.rvw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource2.Value = null;
            this.rvw.LocalReport.DataSources.Add(reportDataSource2);
            this.rvw.LocalReport.ReportEmbeddedResource = "MgmDonasi.Report1.rdlc";
            this.rvw.Location = new System.Drawing.Point(2, 61);
            this.rvw.Name = "rvw";
            this.rvw.Size = new System.Drawing.Size(878, 420);
            this.rvw.TabIndex = 0;
            // 
            // groupBoxHdr
            // 
            this.groupBoxHdr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxHdr.Controls.Add(this.dateTimePickerSd);
            this.groupBoxHdr.Controls.Add(this.dateTimePickerDr);
            this.groupBoxHdr.Controls.Add(this.label8);
            this.groupBoxHdr.Controls.Add(this.label5);
            this.groupBoxHdr.Controls.Add(this.buttonTampil);
            this.groupBoxHdr.Location = new System.Drawing.Point(2, 2);
            this.groupBoxHdr.Name = "groupBoxHdr";
            this.groupBoxHdr.Size = new System.Drawing.Size(878, 58);
            this.groupBoxHdr.TabIndex = 2;
            this.groupBoxHdr.TabStop = false;
            // 
            // dateTimePickerDr
            // 
            this.dateTimePickerDr.Location = new System.Drawing.Point(126, 21);
            this.dateTimePickerDr.Name = "dateTimePickerDr";
            this.dateTimePickerDr.Size = new System.Drawing.Size(123, 20);
            this.dateTimePickerDr.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(255, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "s/d";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Periode";
            // 
            // buttonTampil
            // 
            this.buttonTampil.Location = new System.Drawing.Point(416, 19);
            this.buttonTampil.Name = "buttonTampil";
            this.buttonTampil.Size = new System.Drawing.Size(54, 21);
            this.buttonTampil.TabIndex = 10;
            this.buttonTampil.Text = "Tampil";
            this.buttonTampil.UseVisualStyleBackColor = true;
            this.buttonTampil.Click += new System.EventHandler(this.buttonTampil_Click);
            // 
            // dateTimePickerSd
            // 
            this.dateTimePickerSd.Location = new System.Drawing.Point(287, 20);
            this.dateTimePickerSd.Name = "dateTimePickerSd";
            this.dateTimePickerSd.Size = new System.Drawing.Size(123, 20);
            this.dateTimePickerSd.TabIndex = 34;
            // 
            // FLapPembelianDf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 483);
            this.Controls.Add(this.groupBoxHdr);
            this.Controls.Add(this.rvw);
            this.Name = "FLapPembelianDf";
            this.Text = "FRpt";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBoxHdr.ResumeLayout(false);
            this.groupBoxHdr.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rvw;
        private System.Windows.Forms.GroupBox groupBoxHdr;
        private System.Windows.Forms.DateTimePicker dateTimePickerDr;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonTampil;
        private System.Windows.Forms.DateTimePicker dateTimePickerSd;

    }
}