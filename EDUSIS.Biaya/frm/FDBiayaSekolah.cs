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
    [AdnScObjectAtr("Form: Daftar Biaya Per Sekolah", "Biaya")]
    public partial class FDBiayaSekolah : Form
    {
        private SqlConnection cnn;
        private string AppName;
        private AdnScPengguna Pengguna;
        BindingSource bs = new BindingSource();

        private string KdSekolah;
        private string ThAjar;

        public FDBiayaSekolah(SqlConnection cnn, string AppName, AdnScPengguna Pengguna, string KdSekolah, string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.Pengguna = Pengguna;
            this.KdSekolah = KdSekolah;
            this.ThAjar = ThAjar;

            new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
            comboBoxSekolah.SelectedIndex = -1;

            new EDUSIS.Shared.AdnThAjarDao(this.cnn).SetCombo(comboBoxThAjar);
            comboBoxThAjar.SelectedIndex = -1;

            if (comboBoxThAjar.Items.Count > 0)
            {
                comboBoxThAjar.SelectedValue = this.ThAjar;
            }

        }

        private void FillDataGridView(int Tingkat)
        {
            dgv.AutoGenerateColumns = false;
            this.UseWaitCursor = true;
            Application.DoEvents();

            //bs.DataSource = new AdnBiayaSekolahDao(this.cnn).GetDf(this.KdSekolah, this.ThAjar, Tingkat.ToString());
            //DataTable lst =  new AdnBiayaSekolahDao(this.cnn).GetDf(this.KdSekolah, this.ThAjar, Tingkat.ToString());
            List<AdnBiaya> lst = new AdnBiayaDao(this.cnn).GetAll();
            List<AdnBiayaSekolah> lstBs = new AdnBiayaSekolahDao(this.cnn).Get(this.KdSekolah, this.ThAjar, Tingkat.ToString());

            int i = 0;
            dgv.Rows.Clear();
            foreach(AdnBiaya item in lst)
            {
                dgv.Rows.Add();
                dgv.Rows[i].Cells[0].Value = item.KdBiaya.ToString();
                dgv.Rows[i].Cells[1].Value = item.NmBiaya.ToString();
                AdnBiayaSekolah bs = lstBs.Find(o => o.KdBiaya == item.KdBiaya.ToString());
                if (bs != null)
                {
                    dgv.Rows[i].Cells[2].Value = bs.Jmh.ToString("N0");
                }
                i++;
            }

            
            //AdnBiayaSekolah bs = lstBs.Find(item => item.KdBiaya == "xx");
            
            //foreach (AdnBiayaSekolah item in lstBs)
            //{
                
            //}
            //int iTkKelasMulai=0;int iTkKelas=0; int iJumlahTk=0;
            //switch(Tingkat)
            //{
            //    case 0:
            //        iTkKelasMulai = 1;
            //        iTkKelas = 4;
            //        iJumlahTk = 4;
            //        break;
            //    case 1:
            //        iTkKelasMulai = 1;
            //        iTkKelas = 6;
            //        iJumlahTk = 6;
            //        break;
            //    case 2:
            //        iTkKelasMulai = 7;
            //        iTkKelas = 9;
            //        iJumlahTk = 3;
            //        break;
            //    case 3:
            //        iTkKelasMulai = 10;
            //        iTkKelas = 12;
            //        iJumlahTk = 3;
            //        break;
            //}


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
        

        private void buttonTampil_Click(object sender, EventArgs e)
        {
            int jmhKolom = dgv.Columns.Count;
            for (int i=2; i<jmhKolom;i++)
            {
                dgv.Columns.Remove(dgv.Columns[2]);
            }
            if (comboBoxSekolah.SelectedIndex > -1 && comboBoxThAjar.SelectedIndex > -1)
            {
                string KdSekolah = comboBoxSekolah.SelectedValue.ToString();
                int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(KdSekolah).Tingkat;

                int iTk = 0;
                switch (Tingkat)
                {
                    case 0:
                        iTk = 2;
                        break;
                    case 1:
                        iTk = 6;
                        break;
                    case 2:
                        iTk = 3;
                        break;
                    case 3:
                        iTk = 3;
                        break;
                }
                for (int i = 1; i <= iTk; i++)
                {
                    DataGridViewTextBoxColumn cTk = new DataGridViewTextBoxColumn();
                    cTk.Name = "Tk" + i.ToString();
                    cTk.HeaderText = i.ToString();
                    cTk.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgv.Columns.Add(cTk);
                }
                this.FillDataGridView(Tingkat);
            }
        }

        private void comboBoxSekolah_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSekolah.SelectedIndex > -1)
            {
                this.KdSekolah = comboBoxSekolah.SelectedValue.ToString();
            }
            else
            {
                this.KdSekolah = "";
            }
        }

        private void comboBoxThAjar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxThAjar.SelectedIndex > -1)
            {
                this.ThAjar = comboBoxThAjar.SelectedValue.ToString();
            }
            else
            {
                this.ThAjar = "";
            }
        }

     
    }
}
