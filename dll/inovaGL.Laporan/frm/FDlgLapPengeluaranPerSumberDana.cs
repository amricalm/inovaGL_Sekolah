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
    [AdnScObjectAtr("Laporan: Pengeluaran Per Sumber Dana", "Laporan")]
    public partial class FDlgLapPengeluaranPerSumberDana : Andhana.AdnBaseForm
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;
        private string AppName;
        private DateTime PeriodeMulai;

        public FDlgLapPengeluaranPerSumberDana(SqlConnection cnn, string ReportPath, string ReportExt, string Organisasi, string AppName,DateTime PeriodeMulai)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;
            this.AppName = AppName;
            this.PeriodeMulai = PeriodeMulai;

            dateTimePickerTglDari.Format = DateTimePickerFormat.Short;
            dateTimePickerTglSampai.Format = DateTimePickerFormat.Short;

            new AdnDeptDao(this.cnn).SetCombo(comboBoxDept);
            new AdnProjectDao(this.cnn).SetCombo(comboBoxProject);

            comboBoxDept.SelectedIndex = -1;
            comboBoxProject.SelectedIndex = -1;

            new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
            comboBoxSekolah.SelectedIndex = -1;

            //this.Tampil();
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonTampil_Click(object sender, EventArgs e)
        {
            this.Tampil();   
        }
        private void Tampil()
        {
            string KdAkun = textBoxKdAkun.Text.ToString();
            string Project = "";
            string Dept = "";
            string KdSekolah = "";

            if (comboBoxProject.Text.ToString() != "")
            {
                Project = comboBoxProject.SelectedValue.ToString();
            }
            if (comboBoxDept.Text.ToString() != "")
            {
                Dept = comboBoxDept.SelectedValue.ToString();
            }

            if (comboBoxSekolah.Text.ToString() != "")
            {
                KdSekolah = comboBoxSekolah.SelectedValue.ToString();
            }

            if (KdAkun == "")
            {
                MessageBox.Show("Isi Kode Akun terlebih dahulu!",this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                DataTable lst = new AdnJurnalDao(this.cnn).GetLapPengeluaranPerSumberDana(dateTimePickerTglDari.Value, dateTimePickerTglSampai.Value, Project, KdAkun,KdSekolah);

                ReportDataSource rds = new ReportDataSource("BukuBesar", lst);
                List<ReportParameter> rpm = new List<ReportParameter>();

                rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
                rpm.Add(new ReportParameter("tgl_dari", dateTimePickerTglDari.Value.ToString(), false));
                rpm.Add(new ReportParameter("tgl_sampai", dateTimePickerTglSampai.Value.ToString(), false));
                rpm.Add(new ReportParameter("kd_akun", KdAkun, false));
                rpm.Add(new ReportParameter("nm_akun", labelNmAkun.Text, false));
                rpm.Add(new ReportParameter("nm_project", comboBoxProject.Text.ToString(), false));
                rpm.Add(new ReportParameter("nm_dept", comboBoxDept.Text.ToString(), false));

                decimal Saldo = 0;
                AdnAkun o = new AdnAkunDao(this.cnn).GetPengeluaranPerSumberDana(KdAkun, dateTimePickerTglDari.Value, new DateTime(1900,1,1), this.PeriodeMulai);
                if (o != null)
                {
                    Saldo = o.Saldo;
                }

                rpm.Add(new ReportParameter("saldo_awal", Saldo.ToString(), false));

                this.namaRPT = "PengeluaranPerSumberDana";
                this.rds = rds;
                this.rpm = rpm;
                this.Text = "Pengeluaran Per Sumber Dana";

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

        private void textBoxKdAkun_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    FDCariAkun ofm = new FDCariAkun(this.cnn, this.AppName,null,this,"PDT");
                    ofm.ChildFormUpdate += new FDCariAkun.ChildFormUpdateHandler(SetKdAkun);
                    ofm.Owner = this;
                    ofm.ShowDialog();
                    break;
            }
        }
        private void SetKdAkun(object sender,ChildEventArgs e)
        {
            textBoxKdAkun.Text = e.KdAkun.ToString().Trim();
            labelNmAkun.Text = e.NmAkun.ToString().Trim();
        }
 
    }
}
