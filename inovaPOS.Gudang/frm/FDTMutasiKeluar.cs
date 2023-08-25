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
    [AdnScObjectAtr("Form: Daftar Mutasi Keluar Gudang", "Mutasi Keluar")]
    public partial class FDTMutasiKeluar : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        public FDTMutasiKeluar(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            comboBoxPemasok.ValueMember = "kd_gudang";
            comboBoxPemasok.DisplayMember = "nm_gudang";
            comboBoxPemasok.DataSource = new AdnGudangDao(this.cnn).GetAll();

        }
        private void FDTMutasiKeluar_Load(object sender, EventArgs e)
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

            bs.DataSource = new AdnMutasiKeluarDao(this.cnn).GetByArgs(dateTimePickerDr.Value, dateTimePickerSd.Value,sPemasok);
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
            FTMutasiKeluar ofm = new FTMutasiKeluar(this.cnn,this.AppName, AdnModeEdit.BARU, "", this);
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

            FTMutasiKeluar ofm = new FTMutasiKeluar(this.cnn,this.AppName,  AdnModeEdit.BACA, dgv.CurrentRow.Cells["no_faktur"].Value.ToString(), this);
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
