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
    [AdnScObjectAtr("Laporan: Bukti Jurnal Umum", "Laporan")]
    public partial class FDlgLapBJU : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        public FDlgLapBJU(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,string Kd)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;

            new AdnJurnalUmumDao(this.cnn).SetCombo(comboBoxNoBKK);
            if (Kd != "")
            {
                comboBoxNoBKK.SelectedValue = Kd;
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
            string NoBJU = "";

            NoBJU = Kd;

            if (comboBoxNoBKK.SelectedIndex > -1)
            {
                NoBJU = comboBoxNoBKK.SelectedValue.ToString();
            }

            if (NoBJU!="")
            {   
                DataTable lst = new AdnJurnalUmumDao(this.cnn).GetLapBJU(NoBJU);

                ReportDataSource rds = new ReportDataSource("rptKasKeluar", lst);
                List<ReportParameter> rpm = new List<ReportParameter>();
                rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));

                this.namaRPT = "BuktiJurnalUmum";
                this.rds = rds;
                this.rpm = rpm;
                this.Text = "Jurnal Umum";

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
