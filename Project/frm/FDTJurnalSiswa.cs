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
    [AdnScObjectAtr("Form: Daftar Jurnal Siswa", "Jurnal Siswa")]
    public partial class FDTJurnalSiswa : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();

        public FDTJurnalSiswa(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            new AdnSysJenisJurnalDao(this.cnn).SetComboJurnalSiswa(comboBoxJenisJurnal,false);
            comboBoxJenisJurnal.SelectedIndex = -1;

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


            string JenisJurnal = (comboBoxJenisJurnal.SelectedIndex < 0) ? "" : comboBoxJenisJurnal.SelectedValue.ToString();
            bs.DataSource = new AdnJurnalDao(this.cnn).Get(dateTimePickerDr.Value,dateTimePickerSd.Value,JenisJurnal);
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
            FTJurnalSiswa ofm = new FTJurnalSiswa(this.cnn,this.AppName, AdnModeEdit.BARU, "", this,AppVar.KdSekolah,AppVar.ThAjar);
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

            FTJurnalSiswa ofm = new FTJurnalSiswa(this.cnn,this.AppName,  AdnModeEdit.BACA, AdnFungsi.CStr(dgv.CurrentRow.Cells["NoBukti"]),this, AppVar.KdSekolah,AppVar.ThAjar);
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
