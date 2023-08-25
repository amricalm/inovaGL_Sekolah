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
    [AdnScObjectAtr("Laporan: Daftar Transaksi", "Laporan")]
    public partial class FDlgLapTr : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        public FDlgLapTr(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi,string Kd)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;

            new AdnDeptDao(this.cnn).SetCombo(comboBoxDept);
            new AdnProjectDao(this.cnn).SetCombo(comboBoxProject);

            comboBoxDept.SelectedIndex = -1;
            comboBoxProject.SelectedIndex = -1;

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

            string KdProject = "";
            string KdProgram = "";
            if (comboBoxProject.SelectedIndex > -1)
            {
                KdProject = comboBoxProject.SelectedValue.ToString();
            }

            if (comboBoxDept.SelectedIndex > -1)
            {
                KdProgram = comboBoxDept.SelectedValue.ToString();
            }

            DataTable lst = new AdnJurnalDao(this.cnn).GetLapJU(dateTimePickerDr.Value, dateTimePickerSd.Value, KdProject, KdProgram);

            ReportDataSource rds = new ReportDataSource("Jurnal", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
            rpm.Add(new ReportParameter("TglDr", dateTimePickerDr.Value.ToString(), false));
            rpm.Add(new ReportParameter("TglSd", dateTimePickerSd.Value.ToString(), false));

            this.namaRPT = "Jurnal";
            this.rds = rds;
            this.rpm = rpm;
            this.Text = "Jurnal";

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
