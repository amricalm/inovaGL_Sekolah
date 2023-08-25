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
    [AdnScObjectAtr("Laporan: Laba-Rugi Per Dept [Standar]", "Laporan")]
    public partial class FDlgLapLRbyDeptStandar : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        public FDlgLapLRbyDeptStandar(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,string Kd)
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

            DataTable lst = new inovaGL.Data.AdnLapLabaRugi(this.cnn).CetakByDeptFormatStandar("LR",dateTimePickerDr.Value, dateTimePickerSd.Value);
            //List<Neraca> lst = new List<Neraca>();
            //lst.Add(new Neraca("Baris 0"));
            ReportDataSource rds = new ReportDataSource("rpt", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
            rpm.Add(new ReportParameter("TglDr", dateTimePickerDr.Value.ToString(), false));
            rpm.Add(new ReportParameter("TglSd", dateTimePickerSd.Value.ToString(), false));


            switch (inovaGL.Data.AdnLaporanModel.EdusisModel)
            {
                case  AdnVar.EdusisModel.AlHamid:
                    rpm.Add(new ReportParameter("D1", "TK/RA", false));
                    rpm.Add(new ReportParameter("D2", "SD/MI", false));
                    rpm.Add(new ReportParameter("D3", "SMP/MTs", false));
                    rpm.Add(new ReportParameter("D4", "SMA/MA", false));
                    rpm.Add(new ReportParameter("D5", "YAYASAN", false));
                    rpm.Add(new ReportParameter("D6", "PESANTREN", false));

                    this.namaRPT = "LabaRugiDept6Kolom";

                    break;

                default:
                    rpm.Add(new ReportParameter("D1", "TK", false));
                    rpm.Add(new ReportParameter("D2", "SD", false));
                    rpm.Add(new ReportParameter("D3", "SMP", false));
                    rpm.Add(new ReportParameter("D4", "YAYASAN", false));

                    this.namaRPT = "LabaRugiDept";

                    break;

            }
            

            
            this.rds = rds;
            this.rpm = rpm;
            this.Text = "Laba Rugi Per Dept";

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
