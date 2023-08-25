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
    [AdnScObjectAtr("Laporan: Daftar Piutang Siswa", "Laporan")]
    public partial class FDlgLapPiutangSiswa : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        private string ThAjar;

        public FDlgLapPiutangSiswa(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,string Kd, string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;
            this.ThAjar = ThAjar;

            new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
            new EDUSIS.Kelas.AdnKelasDao(this.cnn).SetCombo(comboBoxKelas,"");

            comboBoxSekolah.SelectedIndex = -1;
            comboBoxKelas.SelectedIndex = -1;

            this.Tampil(Kd);
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
            string KdSekolah = "";
            if (comboBoxKelas.SelectedIndex > -1)
            {
                Kelas = comboBoxKelas.SelectedValue.ToString();
            }

            if (comboBoxSekolah.SelectedIndex > -1)
            {
                Sekolah = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(comboBoxSekolah.SelectedValue.ToString()).NmSekolah;
                KdSekolah = comboBoxSekolah.SelectedValue.ToString();
            }

            Hashtable JenisBiaya = new Hashtable();
            DataTable lst = new AdnJurnalDao(this.cnn).GetPiutangPerSiswa(dateTimePickerDr.Value, dateTimePickerSd.Value, Kelas, KdSekolah, ThAjar, AdnBiaya.JenisBiaya.SPP,AdnBiaya.JenisBiaya.PSB,AdnBiaya.JenisBiaya.KEGIATAN );

            ReportDataSource rds = new ReportDataSource("rptPiutang", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", Sekolah, false));
            rpm.Add(new ReportParameter("Tgl", dateTimePickerSd.Value.ToString(), false));
            rpm.Add(new ReportParameter("Kelas", Kelas, false));
            //rpm.Add(new ReportParameter("TglDr", dateTimePickerDr.Value.ToString(), false));
            //rpm.Add(new ReportParameter("TglSd", dateTimePickerSd.Value.ToString(), false));
            

            this.namaRPT = "PiutangPerSiswa";
            this.rds = rds;
            this.rpm = rpm;
            this.Text = "Piutang Per Siswa";

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
