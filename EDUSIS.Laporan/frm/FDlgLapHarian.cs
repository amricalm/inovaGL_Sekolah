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

namespace EDUSIS.KeuanganLaporan
{
    [AdnScObjectAtr("Laporan: Penerimaan Harian", "Laporan")]
    public partial class FDlgLapHarian : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;
        private AdnScPengguna Pengguna;
        private string AppName;
        private string KdSekolah;

        public FDlgLapHarian(SqlConnection cnn,string AppName, AdnScPengguna Pengguna, string KdSekolah, string ReportPath, string ReportExt, string Organisasi,string Kd)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;
            this.AppName = AppName;
            this.Pengguna = Pengguna;
            this.KdSekolah = KdSekolah;

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

            DataTable lst = new EDUSIS.KeuanganPembayaran.AdnPembayaranDao(this.cnn).LapHarian(dateTimePicker.Value,this.KdSekolah,Pengguna.nm_lengkap);
            List<EDUSIS.Biaya.AdnBiaya> lstBiaya = new EDUSIS.Biaya.AdnBiayaDao(this.cnn, this.Pengguna).GetAll();

            int MaxKolom =60;
            string[] kolom = new string[MaxKolom+1];
            int i = 1;
            foreach (EDUSIS.Biaya.AdnBiaya item in lstBiaya)
            {
                kolom[i] =   item.NmBiaya.Trim();
                i++;
            }

            for (int iCacah = i; iCacah <= MaxKolom ; iCacah++)
            {
                kolom[iCacah] = "-";
            }

            ReportDataSource rds = new ReportDataSource("rptHarian", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
            rpm.Add(new ReportParameter("Tgl", dateTimePicker.Value.ToString("d")));
            rpm.Add(new ReportParameter("Kasir",AdnFungsi.CStr(this.Pengguna.nm_login)));
            rpm.Add(new ReportParameter("C1", kolom[1]));
            rpm.Add(new ReportParameter("C2", kolom[2]));
            rpm.Add(new ReportParameter("C3", kolom[3]));
            rpm.Add(new ReportParameter("C4", kolom[4]));
            rpm.Add(new ReportParameter("C5", kolom[5]));
            rpm.Add(new ReportParameter("C6", kolom[6]));
            rpm.Add(new ReportParameter("C7", kolom[7]));
            rpm.Add(new ReportParameter("C8", kolom[8]));
            rpm.Add(new ReportParameter("C9", kolom[9]));
            rpm.Add(new ReportParameter("C10", kolom[10]));

            this.namaRPT = "LapHarian";
            this.rds = rds;
            this.rpm = rpm;
            this.Text = "Penerimaan Keuangan Harian";

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
