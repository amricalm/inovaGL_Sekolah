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

namespace inovaPOS
{
    [AdnScObjectAtr("Form: Daftar Pemasok", "Pemasok")]
    public partial class FLapPemasokDf : Form
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        public FLapPemasokDf(SqlConnection cnn,string ReportPath, string ReportExt,string Organisasi)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;
            this.Tampil();
        }

        private void buttonTampil_Click(object sender, EventArgs e)
        {
            this.Tampil();   
        }

        private void Tampil()
        {

            List<AdnPemasok> lst = new AdnPemasokDao(this.cnn).GetAll();

            ReportDataSource rds = new ReportDataSource("Laporan_donasi", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
            
            this.namaRPT = "PemasokDf";
            this.Text="Daftar Pemasok";
            this.rds = rds;
            this.rpm = rpm;

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
