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
    [AdnScObjectAtr("Laporan: Saldo Pendapatan", "Laporan")]
    public partial class FDlgLapSaldoPendapatan : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;
        private DateTime PeriodeMulai;
        private string ThAjar;
        private string KdAkunUmSiswa;

        public FDlgLapSaldoPendapatan(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,string Kd,DateTime PeriodeMulai, string ThAjar, string KdAkunUmSiswa)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;
            this.PeriodeMulai = PeriodeMulai;
            this.ThAjar = ThAjar;
            this.KdAkunUmSiswa = KdAkunUmSiswa;

            new inovaGL.Data.AdnThAjarDao(this.cnn).SetCombo(comboBoxThAjar);
            comboBoxThAjar.SelectedValue = this.ThAjar;
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
            string sThAjar = "";
            if (comboBoxThAjar.SelectedValue != null)
            {
                sThAjar = comboBoxThAjar.SelectedValue.ToString();
            }
    

            DataTable lst = new inovaGL.Data.AdnJurnalDao(this.cnn).GetLapSaldoPendapatan(dateTimePickerDr.Value, dateTimePickerSd.Value,PeriodeMulai,sThAjar);
            DataTable lstUM = new inovaGL.Data.AdnJurnalDao(this.cnn).GetLapSaldoPendapatanUMSaw(dateTimePickerDr.Value, dateTimePickerSd.Value, PeriodeMulai, sThAjar, this.KdAkunUmSiswa);

            decimal nSaw=0, nPenerimaan =0, nPengeluaran =0,nLainLain=0;
            if (lstUM.Rows.Count>0)
            {
                nSaw = AdnFungsi.CDec(lstUM.Rows[0]["SaldoAwal"]);
                nPenerimaan = AdnFungsi.CDec(lstUM.Rows[0]["Penerimaan"]);
                nPengeluaran = AdnFungsi.CDec(lstUM.Rows[0]["Pengeluaran"]);
                nLainLain = AdnFungsi.CDec(lstUM.Rows[0]["LainLain"]);
            }


            ReportDataSource rds = new ReportDataSource("rptSaldoPendapatan", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
            rpm.Add(new ReportParameter("TglDr", dateTimePickerDr.Value.ToString(), false));
            rpm.Add(new ReportParameter("TglSd", dateTimePickerSd.Value.ToString(), false));
            rpm.Add(new ReportParameter("UMSaw",nSaw.ToString(), false));
            rpm.Add(new ReportParameter("UMPenerimaan", nPenerimaan.ToString(), false));
            rpm.Add(new ReportParameter("UMPengeluaran", nPengeluaran.ToString(), false));
            rpm.Add(new ReportParameter("UMLainLain", nLainLain.ToString(), false));

            this.namaRPT = "SaldoPendapatan";
            this.rds = rds;
            this.rpm = rpm;
            this.Text = "Saldo Pendapatan";

            this.rvw.LocalReport.ReportPath = this.ReportPath + "\\" + this.namaRPT + "." + this.ReportExt;
            if (this.rpm != null && this.rpm.Count != 0)
            {
                this.rvw.LocalReport.SetParameters(this.rpm);
            }
            this.rvw.LocalReport.DataSources.Clear();
            this.rvw.LocalReport.DataSources.Add(this.rds);
            this.rvw.RefreshReport();
        }

        private void FDlgLapSaldoPendapatan_Load(object sender, EventArgs e)
        {

        }
    }
}
