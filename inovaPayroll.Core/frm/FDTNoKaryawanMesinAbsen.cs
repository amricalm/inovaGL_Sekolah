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

namespace inovaPayroll.Core
{
    [AdnScObjectAtr("Form: Pemetaan No Karyawan Mesin Absen - N I P", "Import Transaksi Mesin Absen")]
    public partial class FDTNoKaryawanMesinAbsen : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        private string ThAjar;
        private string KdSekolah;
        public FDTNoKaryawanMesinAbsen(SqlConnection cnn, string AppName,string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ThAjar = ThAjar;

            //new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
            //comboBoxSekolah.SelectedValue = this.KdSekolah;

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

            //string Kelas = "";

            //if (comboBoxKelas.SelectedIndex > -1)
            //{
            //    Kelas = comboBoxKelas.SelectedValue.ToString();
            //}

            bs.DataSource = new AdnKaryawanExtDao(this.cnn).GetKaryawanExt(MVar.Ext.MESIN_ABSEN);
            dgv.DataSource = bs;

            if (dgv.RowCount == 0)
            {
                toolStripButtonPilih.Enabled = false;
            }
            else
            {
                //toolStripButtonPilih.Enabled = true;
            }
            this.UseWaitCursor = false;
        }

        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void toolStripButtonTambah_Click(object sender, EventArgs e)
        {
            //FTJurnalUmum ofm = new FTJurnalUmum(this.cnn,this.AppName, AdnModeEdit.BARU, "",this, this.ThAjar);
            //ofm.ShowDialog();
        }
        private void toolStripButtonPilih_Click(object sender, EventArgs e)
        {
            this.Pilih();
        }

        private void Pilih()
        {
            panelHdr.Enabled = true;
            //panelDtl.Enabled = true;

            //FTJurnalUmum ofm = new FTJurnalUmum(this.cnn,this.AppName,  AdnModeEdit.BACA, AdnFungsi.CStr(dgv.CurrentRow.Cells["KdJU"]), this,this.ThAjar);
            //ofm.ShowDialog();

        }
        
        public void Reload()
        {
            this.FillDataGridView();
        }

        private void cariButton_Click(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }

        private void comboBoxSekolah_SelectedIndexChanged(object sender, EventArgs e)
        {
        //    this.KdSekolah = comboBoxSekolah.SelectedValue.ToString();
        //    new EDUSIS.Kelas.AdnKelasDao(this.cnn).SetCombo(comboBoxKelas, this.KdSekolah);
        //    comboBoxKelas.SelectedIndex = -1;
        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            inovaPayroll.Core.AdnKaryawanExt o = new inovaPayroll.Core.AdnKaryawanExt();
            o.Nip = AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["Nip"]);
            o.KdExt = AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["NoKaryawan"]);
            o.Flag = MVar.Ext.MESIN_ABSEN;

            int hasil = new inovaPayroll.Core.AdnKaryawanExtDao(this.cnn).Update(o);
            if (hasil == 0)
            {
                new inovaPayroll.Core.AdnKaryawanExtDao(this.cnn).Simpan(o);
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

     
    }
}
