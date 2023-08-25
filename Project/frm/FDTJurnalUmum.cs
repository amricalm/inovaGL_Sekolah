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
using inovaGL.Data;

namespace inovaGL
{
    [AdnScObjectAtr("Form: Daftar Jurnal Umum", "Jurnal Umum")]
    public partial class FDTJurnalUmum : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        private string ThAjar;

        public FDTJurnalUmum(SqlConnection cnn, string AppName,string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ThAjar = ThAjar;

            new inovaGL.Data.AdnSysJenisJurnalDao(this.cnn).SetComboJurnalUmum(comboBoxJenisJurnal,true);

        }
        private void FDTReceipt_Load(object sender, EventArgs e)
        {
            dgv.AutoGenerateColumns = false;
            this.FillDataGridView();
        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            //string Filter = " tgl >= '" + AdnFungsi.SetSqlTglEN(dateTimePickerDr.Value) + "' AND tgl < '" + AdnFungsi.SetSqlTglEN(dateTimePickerSd.Value.AddDays(1)) + "'";
            string JenisJurnal = "";

            if (comboBoxJenisJurnal.SelectedIndex > -1)
            {
                JenisJurnal = comboBoxJenisJurnal.SelectedValue.ToString();
            }

            bs.DataSource = new AdnJurnalUmumDao(this.cnn).GetDf(dateTimePickerDr.Value,dateTimePickerSd.Value,JenisJurnal);
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
            FTJurnalUmum ofm = new FTJurnalUmum(this.cnn,this.AppName, AdnModeEdit.BARU, "",this, this.ThAjar);
            ofm.ShowDialog();
        }
        private void toolStripButtonPilih_Click(object sender, EventArgs e)
        {
            this.Pilih();
        }

        private void Pilih()
        {
            panelHdr.Enabled = true;
            //panelDtl.Enabled = true;

            FTJurnalUmum ofm = new FTJurnalUmum(this.cnn,this.AppName,  AdnModeEdit.BACA, AdnFungsi.CStr(dgv.CurrentRow.Cells["KdJU"]), this,this.ThAjar);
            ofm.ShowDialog();

        }
        
        public void Reload()
        {
            this.FillDataGridView();
        }

        private void cariButton_Click(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }

     
    }
}
