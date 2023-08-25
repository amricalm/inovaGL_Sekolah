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

namespace inovaGL
{
    [AdnScObjectAtr("Laporan: Laba-Rugi", "Laporan")]
    public partial class FDlgLapLR : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        public FDlgLapLR(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,string Kd)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;

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
            //string NoBKK = "";

            //NoBKK = Kd;

            //DataTable lst = new AdnKasKeluarDao(this.cnn).GetLapBKK(NoBKK);

            List<Neraca> lst = new List<Neraca>();
            lst.Add(new Neraca("Baris 0"));
            ReportDataSource rds = new ReportDataSource("rpt_Neraca", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));

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
