using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Andhana;

namespace inovaPOS
{
    [AdnScObjectAtr("Form: Daftar Barang", "Barang")]
    public partial class FDCariBarang2 : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();

        private FTBeli fInduk = null;
        //private FTMutasiMasuk fIndukMM = null;
        //private FTMutasiKeluar fIndukMK = null;

        public FDCariBarang2(SqlConnection cnn, string AppName, object fInduk, object fIndukMM, object fIndukMK)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            if (fInduk != null)
            {
                this.fInduk = (FTBeli)fInduk;
            }
            //if (fIndukMM != null)
            //{
            //    //this.fIndukMM = (FTMutasiMasuk)fIndukMM;
            //}
            //if (fIndukMK != null)
            //{
            //    //this.fIndukMK = (FTMutasiKeluar)fIndukMK;
            //}

            comboBoxPemasok.ValueMember = "kd_ps";
            comboBoxPemasok.DisplayMember = "nm_ps";
            comboBoxPemasok.DataSource = new AdnPemasokDao(this.cnn).GetAll();

            comboBoxPemasok.SelectedIndex = -1;
        }
        private void FDCariBarang_Load(object sender, EventArgs e)
        {
            dgv.AutoGenerateColumns = false;
            this.FillDataGridView();
        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            string sPemasok = "";
            string KdBarang = "";
            string NmBarang = "";

            if (KdBarang != "")
            {
                KdBarang = textBoxKdBarang.Text.ToString().Trim();
            }

            if (NmBarang != "")
            {
                NmBarang = textBoxNmBarang.Text.ToString().Trim();
            }

            if (comboBoxPemasok.SelectedValue != null)
            {
                sPemasok = comboBoxPemasok.SelectedValue.ToString();
            }

            bs.DataSource = new AdnBarangDao(this.cnn).GetByArgs(sPemasok,KdBarang,NmBarang);
            dgv.DataSource = bs;

            if (dgv.RowCount == 0)
            {
                toolStripButtonPilih.Enabled = false;
            }
            else
            {
                toolStripButtonPilih.Enabled = true;
            }
            this.UseWaitCursor = false;
        }

        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void toolStripButtonTambah_Click(object sender, EventArgs e)
        {
            FMBarang ofm = new FMBarang(this.cnn,this.AppName, AdnModeEdit.BARU, "", this);
            ofm.ShowDialog();
        }
        private void toolStripButtonPilih_Click(object sender, EventArgs e)
        {
            this.Pilih();
        }

        private void Pilih()
        {
            panelHdr.Enabled = true;

            if (this.fInduk != null)
            {
                this.fInduk.SetKdBarang(AdnFungsi.CStr(dgv.CurrentRow.Cells["barcode"]));
            }
            //if (this.fIndukMM != null)
            //{
            //    this.fIndukMM.SetKdBarang(AdnFungsi.CStr(dgv.CurrentRow.Cells["barcode"]));
            //}
            //if (this.fIndukMK != null)
            //{
            //    this.fIndukMK.SetKdBarang(AdnFungsi.CStr(dgv.CurrentRow.Cells["barcode"]));
            //}                
            this.Close();
        }
        
        public void Reload()
        {
            this.FillDataGridView();
        }

        private void cariButton_Click(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }

        private void textBoxNmBarang_TextChanged(object sender, EventArgs e)
        {
            FilterBarang();

        }

        private void FilterBarang()
        {
            string sKd = textBoxKdBarang.Text.ToString().Trim();
            string sNmBarang = textBoxNmBarang.Text.ToString().Trim();
            string sPemasok = "";

            if (comboBoxPemasok.SelectedIndex > -1)
            {
                sPemasok = comboBoxPemasok.SelectedValue.ToString();
            }

            bs.Filter = "kd_barang LIKE '" + sKd + "*' AND nm_barang LIKE '" + sNmBarang + "*' AND kd_ps LIKE '" + sPemasok + "*'";
        }

        private void textBoxKdBarang_TextChanged(object sender, EventArgs e)
        {
            this.FilterBarang();
        }

        private void comboBoxPemasok_TextChanged(object sender, EventArgs e)
        {
            this.FilterBarang();
        }


    }
}
