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
    [AdnScObjectAtr("Form: Daftar Pembelian", "Pembelian")]
    public partial class FLapPembelianDf : Form
    {
        private string namaRPT;
        private ReportDataSource rds;
        private List<ReportParameter> rpm;
        private SqlConnection cnn;
        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        public FLapPembelianDf(SqlConnection cnn,string ReportPath, string ReportExt,string Organisasi)
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
            //string sKriteria = "";
            //sKriteria = "  tgl between '" + AdnFungsi.SetSqlTglEN(dateTimePickerDr.Value) + "' AND '" + AdnFungsi.SetSqlTglEN(dateTimePickerSd.Value) + "'";

            //if (comboBoxKas.SelectedIndex != -1)
            //{
            //    if (sKriteria != "")
            //    {
            //        sKriteria = sKriteria + " AND ";
            //    }
            //    sKriteria = sKriteria + "kd_kas = '" + comboBoxKas.SelectedValue + "'";
            //}

            //if (comboBoxAgen.SelectedIndex != -1)
            //{
            //    if (sKriteria != "")
            //    {
            //        sKriteria = sKriteria + " AND ";
            //    }
            //    sKriteria = sKriteria + "kd_agen = " + ((AdnAgen)comboBoxAgen.SelectedItem).kd_agen;
            //}

            List<AdnBeli> lst = new AdnBeliDao(this.cnn).GetByPeriode(dateTimePickerDr.Value, dateTimePickerSd.Value);

            ReportDataSource rds = new ReportDataSource("Lap_tbeli", lst);
            List<ReportParameter> rpm = new List<ReportParameter>();
            rpm.Add(new ReportParameter("Organisasi", this.Organisasi, false));
            rpm.Add(new ReportParameter("TglDr", dateTimePickerDr.Text, false));
            rpm.Add(new ReportParameter("TglSd", dateTimePickerSd.Text, false));
            
            this.namaRPT = "PembelianDf";
            this.Text="Daftar Pembelian";
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
