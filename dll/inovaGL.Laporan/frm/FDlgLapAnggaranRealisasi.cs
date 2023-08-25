﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using Andhana;
using inovaGL.Data;

namespace inovaGL.Laporan
{
    [AdnScObjectAtr("Laporan: Realisasi Anggaran", "Laporan")]
    public partial class FDlgLapAnggaranRealisasi : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;
        private string ThAjar;
        private DateTime PeriodeMulai;

        public FDlgLapAnggaranRealisasi(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,string ThAjar, DateTime PeriodeMulai)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;
            this.ThAjar=ThAjar;
            this.PeriodeMulai = PeriodeMulai;

            new AdnThAjarDao(this.cnn).SetCombo(comboBoxThAjar);
            new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
            comboBoxThAjar.SelectedValue=ThAjar;
            this.Tampil();
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonTampil_Click(object sender, EventArgs e)
        {
            this.Tampil(); 
        }
        private void Tampil()
        {
            string KdSekolah = "";
            if (comboBoxSekolah.SelectedIndex > -1)
            {
                KdSekolah = comboBoxSekolah.SelectedValue.ToString();
            }


            DataTable lst = new inovaGL.Data.AdnLapLabaRugi(this.cnn).Anggaran("LR", comboBoxThAjar.SelectedValue.ToString(), KdSekolah);
            ReportDataSource rds = new ReportDataSource("rpt", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();

            rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
            rpm.Add(new ReportParameter("ThAjar", comboBoxThAjar.SelectedValue.ToString(), false));
            rpm.Add(new ReportParameter("Sekolah", new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(KdSekolah).NmSekolah, false));
            rpm.Add(new ReportParameter("Periode", "Per Tahun Ajaran", false));

            this.namaRPT = "AnggaranRealisasi2";
            this.rds = rds;
            this.rpm = rpm;
            this.Text = "Realisasi Anggaran";

            this.rvw.LocalReport.ReportPath = this.ReportPath + "\\" + this.namaRPT + "." + this.ReportExt;
            if (this.rpm != null && this.rpm.Count != 0)
            {
                this.rvw.LocalReport.SetParameters(this.rpm);
            }
            this.rvw.LocalReport.DataSources.Clear();
            this.rvw.LocalReport.DataSources.Add(this.rds);
            this.rvw.RefreshReport();
        }
    }
}
