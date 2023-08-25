namespace inovaGL
{
    partial class FDlgLapPenerimaanPerPeriode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDlgLapPenerimaanPerPeriode));
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.adnNav = new System.Windows.Forms.BindingNavigator(this.components);
            this.btnTutup = new System.Windows.Forms.ToolStripButton();
            this.groupBoxHdr = new System.Windows.Forms.GroupBox();
            this.comboBoxDept = new System.Windows.Forms.ComboBox();
            this.labelNmAkun = new System.Windows.Forms.Label();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxKdAkun = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerTglSampai = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTglDari = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
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
            this.groupBoxHdr.Controls.Add(this.comboBoxDept);
            this.groupBoxHdr.Controls.Add(this.labelNmAkun);
            this.groupBoxHdr.Controls.Add(this.comboBoxProject);
            this.groupBoxHdr.Controls.Add(this.label5);
            this.groupBoxHdr.Controls.Add(this.label4);
            this.groupBoxHdr.Controls.Add(this.textBoxKdAkun);
            this.groupBoxHdr.Controls.Add(this.label3);
            this.groupBoxHdr.Controls.Add(this.dateTimePickerTglSampai);
            this.groupBoxHdr.Controls.Add(this.dateTimePickerTglDari);
            this.groupBoxHdr.Controls.Add(this.label2);
            this.groupBoxHdr.Controls.Add(this.label1);
            this.groupBoxHdr.Controls.Add(this.buttonTampil);
            this.groupBoxHdr.Location = new System.Drawing.Point(3, 26);
            this.groupBoxHdr.Name = "groupBoxHdr";
            this.groupBoxHdr.Size = new System.Drawing.Size(980, 124);
            this.groupBoxHdr.TabIndex = 15;
            this.groupBoxHdr.TabStop = false;
            // 
            // comboBoxDept
            // 
            this.comboBoxDept.FormattingEnabled = true;
            this.comboBoxDept.Location = new System.Drawing.Point(129, 94);
            this.comboBoxDept.Name = "comboBoxDept";
            this.comboBoxDept.Size = new System.Drawing.Size(295, 21);
            this.comboBoxDept.TabIndex = 52;
            this.comboBoxDept.Visible = false;
            // 
            // labelNmAkun
            // 
            this.labelNmAkun.AutoSize = true;
            this.labelNmAkun.Location = new System.Drawing.Point(267, 20);
            this.labelNmAkun.Name = "labelNmAkun";
            this.labelNmAkun.Size = new System.Drawing.Size(19, 13);
            this.labelNmAkun.TabIndex = 51;
            this.labelNmAkun.Text = "...";
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(129, 66);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(295, 21);
            this.comboBoxProject.TabIndex = 50;
            this.comboBoxProject.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 49;
            this.label5.Text = "Departemen";
            this.label5.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "Project";
            this.label4.Visible = false;
            // 
            // textBoxKdAkun
            // 
            this.textBoxKdAkun.Location = new System.Drawing.Point(129, 14);
            this.textBoxKdAkun.Name = "textBoxKdAkun";
            this.textBoxKdAkun.Size = new System.Drawing.Size(132, 20);
            this.textBoxKdAkun.TabIndex = 47;
            this.textBoxKdAkun.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxKdAkun_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "Akun Perkiraan";
            // 
            // dateTimePickerTglSampai
            // 
            this.dateTimePickerTglSampai.Location = new System.Drawing.Point(292, 41);
            this.dateTimePickerTglSampai.Name = "dateTimePickerTglSampai";
            this.dateTimePickerTglSampai.Size = new System.Drawing.Size(132, 20);
            this.dateTimePickerTglSampai.TabIndex = 45;
            // 
            // dateTimePickerTglDari
            // 
            this.dateTimePickerTglDari.Location = new System.Drawing.Point(129, 40);
            this.dateTimePickerTglDari.Name = "dateTimePickerTglDari";
            this.dateTimePickerTglDari.Size = new System.Drawing.Size(132, 20);
            this.dateTimePickerTglDari.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "s/d";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Periode";
            // 
            // buttonTampil
            // 
            this.buttonTampil.AutoSize = true;
            this.buttonTampil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonTampil.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonTampil.Image = ((System.Drawing.Image)(resources.GetObject("buttonTampil.Image")));
            this.buttonTampil.Location = new System.Drawing.Point(430, 24);
            this.buttonTampil.Name = "buttonTampil";
            this.buttonTampil.Size = new System.Drawing.Size(35, 37);
            this.buttonTampil.TabIndex = 38;
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
            this.rvw.Location = new System.Drawing.Point(4, 156);
            this.rvw.Name = "rvw";
            this.rvw.Size = new System.Drawing.Size(980, 505);
            this.rvw.TabIndex = 17;
            // 
            // FDlgLapPenerimaanPerPeriode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 662);
            this.Controls.Add(this.rvw);
            this.Controls.Add(this.groupBoxHdr);
            this.Controls.Add(this.adnNav);
            this.Name = "FDlgLapPenerimaanPerPeriode";
            this.Text = "Pengeluaran Per Sumber Dana";
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
        private System.Windows.Forms.Button buttonTampil;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerTglSampai;
        private System.Windows.Forms.DateTimePicker dateTimePickerTglDari;
        private System.Windows.Forms.TextBox textBoxKdAkun;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxDept;
        private System.Windows.Forms.Label labelNmAkun;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private Microsoft.Reporting.WinForms.ReportViewer rvw;
    }
}