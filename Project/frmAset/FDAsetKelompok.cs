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
    [AdnScObjectAtr("Form: Daftar Kelompok Aset", "Aset")]
    public partial class FDAsetKelompok: Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();

        public FDAsetKelompok(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

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

            bs.DataSource = new AdnAsetKelompokDao(this.cnn).GetAll();
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
            FMAsetKelompok ofm = new FMAsetKelompok(this.cnn,this.AppName, AdnModeEdit.BARU, "", this);
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

            FMAsetKelompok ofm = new FMAsetKelompok(this.cnn,this.AppName,  AdnModeEdit.BACA, AdnFungsi.CStr(dgv.CurrentRow.Cells["KdKelompokAset"]), this);
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
