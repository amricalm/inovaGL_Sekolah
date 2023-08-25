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
    [AdnScObjectAtr("Laporan: Daftar Utang Per Siswa", "Laporan")]
    public partial class FDlgLapUtangSiswaPerKelas : Andhana.AdnBaseForm
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

        public FDlgLapUtangSiswaPerKelas(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi, DateTime PeriodeMulai, string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;
            this.ThAjar = ThAjar;
            this.PeriodeMulai = PeriodeMulai;

            new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
            new EDUSIS.Kelas.AdnKelasDao(this.cnn).SetCombo(comboBoxKelas, "");

            comboBoxSekolah.SelectedIndex = -1;
            comboBoxKelas.SelectedIndex = -1;

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

            if (comboBoxKelas.SelectedIndex > -1)
            {
                Kelas = comboBoxKelas.SelectedValue.ToString();
            }

            if (comboBoxSekolah.SelectedIndex > -1)
            {
                Sekolah = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(comboBoxSekolah.SelectedValue.ToString()).NmSekolah;
                KdSekolah = comboBoxSekolah.SelectedValue.ToString();
            }

            List<AdnAkun> lstAkunPiutang = new List<AdnAkun>();
            lstAkunPiutang = new AdnAkunDao(this.cnn).GetUtangBiaya();

            DataTable lst = new AdnJurnalDao(this.cnn).GetUtangPerSiswaPerAkunTabular(this.PeriodeMulai, dateTimePickerDr.Value, KdSekolah, ThAjar,Kelas);

            ReportDataSource rds = new ReportDataSource("rpt", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", Sekolah, false));
            rpm.Add(new ReportParameter("Tgl", dateTimePickerDr.Value.ToString(), false));
            int i = 1;
            foreach (AdnAkun item in lstAkunPiutang)
            {
                rpm.Add(new ReportParameter("D" + i, item.KdAkun, false));
                i++;
            }
            rpm.Add(new ReportParameter("Sekolah", Sekolah, false));
            rpm.Add(new ReportParameter("Kelas", Kelas, false));

            this.namaRPT = "UtangPerAkunPerSiswa";
            this.rds = rds;
            this.rpm = rpm;
            this.Text = "Utang/Kewajiban Per Siswa";

            this.rvw.LocalReport.ReportPath = this.ReportPath + "\\" + this.namaRPT + "." + this.ReportExt;
            if (this.rpm != null && this.rpm.Count != 0)
            {
                this.rvw.LocalReport.SetParameters(this.rpm);
            }
            this.rvw.LocalReport.DataSources.Clear();
            this.rvw.LocalReport.DataSources.Add(this.rds);
            this.rvw.RefreshReport();

        }

        private void comboBoxSekolah_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSekolah.SelectedIndex > -1)
            {
                new EDUSIS.Kelas.AdnKelasDao(this.cnn).SetCombo(comboBoxKelas, comboBoxSekolah.SelectedValue.ToString());
            }
        }

    }
}
