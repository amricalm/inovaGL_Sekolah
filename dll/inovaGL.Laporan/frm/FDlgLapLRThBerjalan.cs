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
    [AdnScObjectAtr("Laporan: Laba-Rugi", "Laporan")]
    public partial class FDlgLapLRThBerjalan : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;
        private DateTime PeriodeMulai;

        public FDlgLapLRThBerjalan(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,DateTime PeriodeMulai)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;
            this.PeriodeMulai = PeriodeMulai;

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
            //string NoBKK = "";

            //NoBKK = Kd;
           
            DataTable lst = new inovaGL.Data.AdnLapLabaRugi(this.cnn).CetakFormatThBerjalan("LR",this.PeriodeMulai,dateTimePickerDr.Value, dateTimePickerSd.Value);
            //List<Neraca> lst = new List<Neraca>();
            //lst.Add(new Neraca("Baris 0"));
            ReportDataSource rds = new ReportDataSource("rptLabaRugi", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
            rpm.Add(new ReportParameter("TglDr", dateTimePickerDr.Value.ToString(), false));
            rpm.Add(new ReportParameter("TglSd", dateTimePickerSd.Value.ToString(), false));

            this.namaRPT = "LabaRugiThBerjalan";
            this.rds = rds;
            this.rpm = rpm;
            this.Text = "Laba Rugi [Tahun Berjalan]";

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
