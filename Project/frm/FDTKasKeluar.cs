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
    [AdnScObjectAtr("Form: Daftar Kas Keluar", "Kas Keluar")]
    public partial class FDTKasKeluar : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        private string ThAjar;

        public FDTKasKeluar(SqlConnection cnn, string AppName,string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ThAjar = ThAjar;

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

            string Filter = " tgl >= '" + AdnFungsi.SetSqlTglEN(dateTimePickerDr.Value) + "' AND tgl < '" + AdnFungsi.SetSqlTglEN(dateTimePickerSd.Value.AddDays(1)) + "'";

            bs.DataSource = new AdnKasKeluarDao(this.cnn).GetByArgs(Filter);
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
            FTKasKeluar ofm = new FTKasKeluar(this.cnn,this.AppName, AdnModeEdit.BARU, "", this,this.ThAjar);
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

            FTKasKeluar ofm = new FTKasKeluar(this.cnn,this.AppName,  AdnModeEdit.BACA, AdnFungsi.CStr(dgv.CurrentRow.Cells["KdKK"]), this,this.ThAjar);
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
