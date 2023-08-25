using System;
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
    [AdnScObjectAtr("Laporan: Neraca Standar", "Laporan")]
    public partial class FDlgLapNeracaStandar : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;
        private DateTime PeriodeAwal;
        private string ThAjar;

        public FDlgLapNeracaStandar(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,DateTime PeriodeAwal,string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;
            this.PeriodeAwal = PeriodeAwal;
            this.ThAjar = ThAjar;
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
            DateTime TglDr = dateTimePickerTglDari.Value;

            DataTable lst = new AdnLapNeraca(this.cnn).CetakFormatStandar("NRC", PeriodeAwal, TglDr, this.ThAjar);

            ReportDataSource rds = new ReportDataSource("rptLabaRugi", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
            rpm.Add(new ReportParameter("Tgl", dateTimePickerTglDari.Value.ToString(), false));

            this.namaRPT = "Neraca";
            this.rds = rds;
            this.rpm = rpm;
            this.Text = "Neraca";

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
