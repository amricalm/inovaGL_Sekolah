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
    [AdnScObjectAtr("Laporan: Bukti Kas Masuk", "Laporan")]
    public partial class FDlgLapBKM : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        public FDlgLapBKM(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,string Kd)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;

            new AdnKasMasukDao(this.cnn).SetCombo(comboBoxNoBKM);
            if (Kd != "")
            {
                comboBoxNoBKM.SelectedValue = Kd;
            }
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
            string NoBKM = "";

            NoBKM = Kd;

            if (comboBoxNoBKM.SelectedIndex > -1)
            {
                NoBKM = comboBoxNoBKM.SelectedValue.ToString();
            }

            if (NoBKM!="")
            {   
                DataTable lst = new AdnKasMasukDao(this.cnn).GetLapBKM(NoBKM);

                ReportDataSource rds = new ReportDataSource("rptKasMasuk", lst);
                List<ReportParameter> rpm = new List<ReportParameter>();
                rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));

                this.namaRPT = "BuktiKasMasuk";
                this.rds = rds;
                this.rpm = rpm;
                this.Text = "Receipt";

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
}
