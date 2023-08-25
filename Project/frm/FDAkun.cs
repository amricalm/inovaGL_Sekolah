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
    [AdnScObjectAtr("Form: Daftar Akun", "Akun")]
    public partial class FDAkun : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        public FDAkun(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            //new AdnSysAkunJenisDao(this.cnn).SetCombo(comboBoxKelompok);
            //if (comboBoxKelompok.SelectedIndex > -1)
            //{
            //    string KdKelompok = comboBoxKelompok.SelectedValue.ToString();
            //    new AdnSysAkunGolonganDao(this.cnn).SetComboByJenis(comboBoxSubKelompok, KdKelompok);
            //}
            new AdnSysAkunGolonganDao(this.cnn).SetCombo(comboBoxSubKelompok,true);
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

            string KdGol = "";
            if (comboBoxSubKelompok.SelectedIndex > -1)
            {
                KdGol = comboBoxSubKelompok.SelectedValue.ToString().Trim();
            }

            bs.DataSource = new AdnAkunDao(this.cnn).GetDf(KdGol);
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
            FMAkun ofm = new FMAkun(this.cnn,this.AppName, AdnModeEdit.BARU, "", this);
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

            FMAkun ofm = new FMAkun(this.cnn,this.AppName,  AdnModeEdit.BACA, AdnFungsi.CStr(dgv.CurrentRow.Cells["KdAkun"]), this);
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

        private void toolStripButtonTree_Click(object sender, EventArgs e)
        {
            FMAkunTree ofm = new FMAkunTree(this.cnn, this.AppName, AdnModeEdit.BACA, "", null);
            ofm.ShowDialog();
        }

        private void comboBoxKelompok_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxKelompok.SelectedIndex > -1)
            {
                string KdKelompok = comboBoxKelompok.SelectedValue.ToString();
                new AdnSysAkunGolonganDao(this.cnn).SetComboByJenis(comboBoxSubKelompok, KdKelompok);
            }
            else
            {
                comboBoxSubKelompok.SelectedIndex = -1;
                new AdnSysAkunGolonganDao(this.cnn).SetComboByJenis(comboBoxSubKelompok, "");
            }
        }


     
    }
}
