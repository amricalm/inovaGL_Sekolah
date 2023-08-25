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

namespace EDUSIS.KeuanganPembayaran
{
    [AdnScObjectAtr("Laporan: Kwitansi", "Pembayaran")]
    public partial class FDlgKwitansi : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private AdnScPengguna Pengguna;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        public FDlgKwitansi(SqlConnection cnn, AdnScPengguna Pengguna, string ReportPath, string ReportExt, string Organisasi,string Kd)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.Pengguna=Pengguna;
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

                DataTable lst = new AdnPembayaranDao(this.cnn,this.Pengguna).GetLengkap(Kd);

                ReportDataSource rds = new ReportDataSource("rptKwitansi", lst);
                List<ReportParameter> rpm = new List<ReportParameter>();
                rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
                rpm.Add(new ReportParameter("Tgl",lst.Rows[0]["Tgl"].ToString()));
                rpm.Add(new ReportParameter("NIS", lst.Rows[0]["Nis"].ToString()));
                rpm.Add(new ReportParameter("NmLengkap", lst.Rows[0]["NmLengkap"].ToString()));
                rpm.Add(new ReportParameter("Kelas", lst.Rows[0]["Kelas"].ToString()));
                rpm.Add(new ReportParameter("NoKwitansi",Kd.ToString()));
                rpm.Add(new ReportParameter("Terbilang", AdnFungsi.Terbilang((decimal)lst.Rows[0]["Total"])));
                rpm.Add(new ReportParameter("Kasir", AdnFungsi.CStr(this.Pengguna.nm_lengkap)));

                this.namaRPT = "Kwitansi";
                this.rds = rds;
                this.rpm = rpm;
                this.Text = "Kwitansi";

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
