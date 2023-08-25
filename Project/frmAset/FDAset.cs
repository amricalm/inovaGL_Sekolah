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
    [AdnScObjectAtr("Form: Daftar Aset", "Aset")]
    public partial class FDAset: Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();

        public FDAset(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            new AdnAsetKelompokDao(this.cnn).SetCombo(comboBoxKelompokAset);

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
            string KelompokAset ="";
            if(comboBoxKelompokAset.SelectedIndex>-1)
            {
                KelompokAset=comboBoxKelompokAset.SelectedValue.ToString().Trim();
            }

            bs.DataSource = new AdnAsetDao(this.cnn).GetByKelompok(KelompokAset);
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
            FMAset ofm = new FMAset(this.cnn,this.AppName, AdnModeEdit.BARU, "", this);
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

            FMAset ofm = new FMAset(this.cnn,this.AppName,  AdnModeEdit.BACA, AdnFungsi.CStr(dgv.CurrentRow.Cells["KdAset"]), this);
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
