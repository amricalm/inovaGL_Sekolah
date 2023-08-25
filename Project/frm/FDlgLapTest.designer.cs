namespace inovaGL
{
    partial class FDlgLapTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDlgLapTest));
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.adnNav = new System.Windows.Forms.BindingNavigator(this.components);
            this.btnTutup = new System.Windows.Forms.ToolStripButton();
            this.rvw = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.adnNav)).BeginInit();
            this.adnNav.SuspendLayout();
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
            // rvw
            // 
            this.rvw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Value = null;
            this.rvw.LocalReport.DataSources.Add(reportDataSource1);
            this.rvw.LocalReport.ReportEmbeddedResource = "MgmDonasi.Report1.rdlc";
            this.rvw.Location = new System.Drawing.Point(3, 28);
            this.rvw.Name = "rvw";
            this.rvw.Size = new System.Drawing.Size(980, 633);
            this.rvw.TabIndex = 16;
            // 
            // FDlgLapTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 662);
            this.Controls.Add(this.rvw);
            this.Controls.Add(this.adnNav);
            this.Name = "FDlgLapTest";
            this.Text = "Daftar Transaksi";
            ((System.ComponentModel.ISupportInitialize)(this.adnNav)).EndInit();
            this.adnNav.ResumeLayout(false);
            this.adnNav.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator adnNav;
        private System.Windows.Forms.ToolStripButton btnTutup;
        private Microsoft.Reporting.WinForms.ReportViewer rvw;
    }
}