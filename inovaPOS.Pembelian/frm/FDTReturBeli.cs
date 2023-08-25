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
    [AdnScObjectAtr("Form: Daftar Retur Pembelian", "Retur Pembelian")]
    public partial class FDTReturBeli : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        public FDTReturBeli(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            comboBoxPemasok.ValueMember = "kd_ps";
            comboBoxPemasok.DisplayMember = "nm_ps";
            comboBoxPemasok.DataSource = new AdnPemasokDao(this.cnn).GetAll();

        }
        private void FDTReturBeli_Load(object sender, EventArgs e)
        {
            dgv.AutoGenerateColumns = false;
            this.FillDataGridView();
        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            string sPemasok = "";
            if (comboBoxPemasok.SelectedValue != null)
            {
                sPemasok = comboBoxPemasok.SelectedValue.ToString();
            }

            bs.DataSource = new AdnTReturBeliDao(this.cnn).GetByArgs(dateTimePickerDr.Value, dateTimePickerSd.Value,sPemasok);
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
            FTReturBeli ofm = new FTReturBeli(this.cnn,this.AppName, AdnModeEdit.BARU, "", this);
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

            FTReturBeli ofm = new FTReturBeli(this.cnn,this.AppName,  AdnModeEdit.BACA, dgv.CurrentRow.Cells["no_faktur"].Value.ToString(), this);
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
