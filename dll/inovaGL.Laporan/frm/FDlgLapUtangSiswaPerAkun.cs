using System;
using System.Collections;
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
using EDUSIS.Biaya;

namespace inovaGL.Laporan
{
    [AdnScObjectAtr("Laporan: Daftar Utang Siswa", "Laporan")]
    public partial class FDlgLapUtangSiswaPerAkun : Andhana.AdnBaseForm
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

        public FDlgLapUtangSiswaPerAkun(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi, DateTime PeriodeMulai, string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;
            this.ThAjar = ThAjar;
            this.PeriodeMulai = PeriodeMulai;

            new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
            comboBoxSekolah.SelectedIndex = -1;

            this.Tampil("");
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonTampil_Click(object sender, EventArgs e)
        {
            this.Tampil("");   
        }
        private void Tampil(string Kd)
        {

            string Kelas = "";
            string Sekolah = "";
            string KdSekolah = "11";

            if (comboBoxSekolah.SelectedIndex > -1)
            {
                Sekolah = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(comboBoxSekolah.SelectedValue.ToString()).NmSekolah;
                KdSekolah = comboBoxSekolah.SelectedValue.ToString();
            }

            List<AdnAkun> lstAkunPiutang = new List<AdnAkun>();
            lstAkunPiutang = new AdnAkunDao(this.cnn).GetUtangBiaya();

            DataTable lst = new AdnJurnalDao(this.cnn).GetUtangPerAkunTabular(this.PeriodeMulai, dateTimePickerDr.Value,KdSekolah,ThAjar);

            ReportDataSource rds = new ReportDataSource("rpt", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", Sekolah, false));
            rpm.Add(new ReportParameter("Tgl", dateTimePickerDr.Value.ToString(), false));
            //rpm.Add(new ReportParameter("Kelas", Kelas, false));
            //rpm.Add(new ReportParameter("TglDr", dateTimePickerDr.Value.ToString(), false));
            //rpm.Add(new ReportParameter("TglSd", dateTimePickerSd.Value.ToString(), false));
            int i = 1;
            foreach (AdnAkun item in lstAkunPiutang)
            {
                rpm.Add(new ReportParameter("D"+i, item.KdAkun, false));
                i++;
            }
            this.namaRPT = "UtangPerAkun";
            this.rds = rds;
            this.rpm = rpm;
            this.Text = "Utang Per Kelas";

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
