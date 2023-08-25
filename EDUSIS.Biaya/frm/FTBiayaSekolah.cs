using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Andhana;

namespace EDUSIS.Biaya
{
    [AdnScObjectAtr("Form: Input Biaya Sekolah", "Biaya")]
    public partial class FTBiayaSekolah : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private string AppName;
        private AdnScPengguna Pengguna;
        //private FDMPelanggan fInduk;

        public FTBiayaSekolah(SqlConnection cnn, string AppName, AdnScPengguna Pengguna, short ModeEdit, string kd, object fInduk)
        {
            InitializeComponent();
            this.AppName = AppName;
            this.Pengguna = Pengguna;
            this.ModeEdit = ModeEdit;
            this.cnn = cnn;

            if (this.ModeEdit == AdnModeEdit.BACA)
            {
                this.GetData(kd);
            }
            else
            {
                this.DokumenBaru();
            }

        }
        private void FTBiayaSekolah_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (MessageBox.Show("Yakin Jendela Ini Akan Ditutup?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    break;

                case Keys.F12:
                    if (toolStripButtonSimpan.Enabled)
                    {
                        this.Simpan();
                    }
                    break;

                case Keys.F11:
                    if (toolStripButtonTambah.Enabled)
                    {
                        this.Tambah();
                    }
                    break;

                case Keys.F10:
                    if (toolStripButtonEdit.Enabled)
                    {
                        this.Edit();
                        e.Handled = true;
                    }
                    break;

                case Keys.D:
                    if (Control.ModifierKeys == Keys.Control && toolStripButtonHapus.Enabled)
                    {
                        this.Hapus();
                    }
                    break;

                case Keys.N:
                    if (Control.ModifierKeys == Keys.Control && toolStripButtonBatal.Enabled)
                    {
                        this.Batal();
                    }
                    break;
            }
        }

        private void Tambah()
        {
            this.DokumenBaru();
        }
        private void Edit()
        {
            this.ModeEdit = AdnModeEdit.UBAH;

            panelHdr.Enabled = true;
            textBoxKd.Enabled = false;
            textBoxNm.Focus();

            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = true;

            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;
        }
        private void Simpan()
        {
            if (this.IsValid())
            {
                List<EDUSIS.Shared.AdnSekolah> lst = new EDUSIS.Shared.AdnSekolahDao(this.cnn).GetAll();
                foreach(EDUSIS.Shared.AdnSekolah item in lst)
                {
                    AdnBiayaSekolah o = new AdnBiayaSekolah();
                    o.KdBiaya = textBoxKd.Text.ToString().Trim();
                    o.KdSekolah = item.KdSekolah;
                    o.Tingkat =
                    switch (item.Tingkat)
                    {
                        case 0:
                            o.Jmh = AdnFungsi.CDec(textBoxJmhTK);
                            break;
                        case 1:
                            o.Jmh = AdnFungsi.CDec(textBoxJmhSD);
                            break;
                        case 2:
                            o.Jmh = AdnFungsi.CDec(textBoxJmhSMP);
                            break;

                        case 3:
                            //o.Jmh = AdnFungsi.CDec(textBoxJmhTK);
                            break;
                    }
                
                    AdnBiayaDao dao = new AdnBiayaDao(this.cnn,this.Pengguna);
                    switch (this.ModeEdit)
                    {
                        case AdnModeEdit.BARU:
                            dao.Simpan(o);
                            break;

                        case AdnModeEdit.UBAH:
                            o.KdBiaya = textBoxKd.Text;
                            dao.Update(o);
                            break;
                    }
                }
                panelHdr.Enabled = false;

                toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonHapus.Enabled = true;

                toolStripButtonBatal.Enabled = false;
                toolStripButtonSimpan.Enabled = false;
            }
        }
        private void Hapus()
        {
            if (MessageBox.Show("Hapus Data, Biaya = " + textBoxNm.Text.ToString() + " ?",this.AppName , MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                AdnBiayaDao dao = new AdnBiayaDao(this.cnn);
                dao.Hapus(textBoxKd.Text);
                this.Batal();
            }
        }
        private void DokumenBaru()
        {
            AdnFungsi.Bersih(this);
            this.ModeEdit = AdnModeEdit.BARU;
            panelHdr.Enabled = true;
            textBoxKd.Enabled = true;
            textBoxKd.Focus(); ;
            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = false;
            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;
        }
        private void Batal()
        {
            this.DokumenBaru();
        }
        public void GetData(string Kd)
        {
            AdnBiaya o = new AdnBiayaDao(this.cnn,this.Pengguna).Get(Kd);

            if (o != null)
            {
                this.ModeEdit = AdnModeEdit.BACA;
                textBoxKd.Focus();

                toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonBatal.Enabled = true;
                toolStripButtonHapus.Enabled = true;
                toolStripButtonSimpan.Enabled = false;

                textBoxKd.Text = Kd;
                textBoxNm.Text = o.NmBiaya;
                comboBoxJenis.SelectedValue = o.KdJenis;
                
                checkBoxGabungan.Checked = (o.Gabungan)?true:false;
                checkBoxPSB.Checked=(o.LaporanPSB)?true:false;
                checkBoxRutin.Checked=(o.LaporanRutin)?true:false;
                checkBoxTdkDijurnal.Checked = (o.TidakDijurnal) ? true : false;

                textBoxKeterangan.Text = o.Keterangan;
                comboBoxAkunPiutang.SelectedValue = o.KdAkunPiutang;
                comboBoxAkunKewajiban.SelectedValue = o.KdAkunKewajiban;
                comboBoxAkunPendapatan.SelectedValue = o.KdAkunPendapatan;
            }
        }
        private bool IsValid()
        {
            string sPesan = "";
            if (textBoxKd.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Kode Biaya";
            }

            if (textBoxNm.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Nama Biaya";
            }


            if (sPesan != "")
            {
                sPesan = sPesan + " Harus Diisi.\n";
            }

            if (sPesan == "")
            {
                return true;
            }
            else
            {
                MessageBox.Show(sPesan, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


        }

        private void toolStripButtonTambah_Click(object sender, EventArgs e)
        {
            this.Tambah();
        }
        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            this.Edit();
        }
        private void toolStripButtonSimpan_Click(object sender, EventArgs e)
        {
            this.Simpan();
        }
        private void toolStripButtonBatal_Click(object sender, EventArgs e)
        {
            this.Batal();
        }
        private void toolStripButtonHapus_Click(object sender, EventArgs e)
        {
            this.Hapus();
        }
        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }
   
        private void textBoxKd_Validating(object sender, CancelEventArgs e)
        {
            GetData(textBoxKd.Text.ToString().Trim());
        }
        private void textBoxPos_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void FTBiayaSekolah_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (this.fInduk != null)
            //{
            //    this.fInduk.Reload();
            //}
        }




    }
}
