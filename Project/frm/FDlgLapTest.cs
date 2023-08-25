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

namespace inovaGL
{
    [AdnScObjectAtr("Laporan: Daftar Transaksi", "Laporan")]
    public partial class FDlgLapTest : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        public FDlgLapTest(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,string Kd)
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

            //DataTable lst = new AdnJurnalDao(this.cnn).GetLapJU(dateTimePickerDr.Value, dateTimePickerSd.Value, KdProject, KdProgram);

            //ReportDataSource rds = new ReportDataSource("Jurnal", lst);
            //List<ReportParameter> rpm = new List<ReportParameter>();
            //rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));

            //this.namaRPT = "Jurnal";
            //this.rds = rds;
            //this.rpm = rpm;
            //this.Text = "Jurnal";

            //this.rvw.LocalReport.ReportPath = this.ReportPath + "\\" + this.namaRPT + "." + this.ReportExt;
            //if (this.rpm != null && this.rpm.Count != 0)
            //{
            //    this.rvw.LocalReport.SetParameters(this.rpm);
            //}
            //this.rvw.LocalReport.DataSources.Clear();
            //this.rvw.LocalReport.DataSources.Add(this.rds);
            //this.rvw.RefreshReport();

        }
    }
}
