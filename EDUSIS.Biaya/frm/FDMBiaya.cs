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

namespace EDUSIS.Biaya
{
    [AdnScObjectAtr("Form: Daftar Biaya", "Biaya")]
    public partial class FDMBiaya : Form
    {
        private SqlConnection cnn;
        private string AppName;
        private AdnScPengguna Pengguna;
        BindingSource bs = new BindingSource();
        public FDMBiaya(SqlConnection cnn, string AppName, AdnScPengguna Pengguna)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.Pengguna = Pengguna;

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

            bs.DataSource = new AdnBiayaDao(this.cnn).GetAll();
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
            FMBiaya ofm = new FMBiaya(this.cnn, this.AppName,this.Pengguna, AdnModeEdit.BARU, "", this);
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

            FMBiaya ofm = new FMBiaya(this.cnn, this.AppName,this.Pengguna, AdnModeEdit.BACA, AdnFungsi.CStr(dgv.CurrentRow.Cells["KdBiaya"]), this);
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
