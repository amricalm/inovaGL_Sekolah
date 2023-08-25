using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Andhana;
using inovaGL.Data;
using inovaGL.Definisi;

namespace inovaGL
{
    [AdnScObjectAtr("Form: Input Aset", "Aset")]
    public partial class FMAsetKelompok : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private string AppName;

        public FMAsetKelompok(SqlConnection cnn, string AppName, short ModeEdit, string kd, object fInduk)
        {
            InitializeComponent();
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;
            this.cnn = cnn;

            new AdnAkunDao(this.cnn).SetComboByGolDetail(comboBoxAkumulasi,AdnVar.GolonganAKun.AKUMULASI_PENYUSUTAN);
            new AdnAkunDao(this.cnn).SetComboByGolDetail(comboBoxBebanPenyusutan, AdnVar.GolonganAKun.BEBAN);

            if (this.ModeEdit == AdnModeEdit.BACA)
            {
                this.GetData(kd);
            }
            else
            {
                this.DokumenBaru();
            }

        }
        private void FMAsetKelompok_KeyDown(object sender, KeyEventArgs e)
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
                AdnAsetKelompok o = new AdnAsetKelompok();
                o.KdKelompokAset = textBoxKd.Text.ToString().Trim();
                o.NmKelompokAset = textBoxNm.Text.ToString().Trim();
                o.Keterangan = textBoxKeterangan.Text.ToString().Trim();

                if (comboBoxAkumulasi.SelectedIndex > -1)
                {
                    o.CoaAkumulasiPenyusutan = comboBoxAkumulasi.SelectedValue.ToString().Trim();
                }
                if (comboBoxBebanPenyusutan.SelectedIndex > -1)
                {
                    o.CoaBebanPenyusutan = comboBoxBebanPenyusutan.SelectedValue.ToString().Trim();
                }
                AdnAsetKelompokDao dao = new AdnAsetKelompokDao(this.cnn,AppVar.AppPengguna);
                switch (this.ModeEdit)
                {
                    case AdnModeEdit.BARU:
                        dao.Simpan(o);
                        break;

                    case AdnModeEdit.UBAH:
                        o.KdKelompokAset = textBoxKd.Text;
                        dao.Update(o);
                        break;
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
            if (MessageBox.Show("Hapus Data, Kelompok Aset = " + textBoxNm.Text.ToString() + " ?",this.AppName , MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                AdnAsetKelompokDao dao = new AdnAsetKelompokDao(this.cnn);
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
            AdnAsetKelompok o = new AdnAsetKelompokDao(this.cnn,AppVar.AppPengguna).Get(Kd);

            if (o != null)
            {
                this.ModeEdit = AdnModeEdit.BACA;
                textBoxKd.Enabled = false;
                textBoxNm.Focus();

                toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonBatal.Enabled = true;
                toolStripButtonHapus.Enabled = true;
                toolStripButtonSimpan.Enabled = false;

                textBoxKd.Text = Kd;
                textBoxNm.Text = o.NmKelompokAset;
                textBoxKeterangan.Text = o.Keterangan;

                if (o.CoaAkumulasiPenyusutan == null)
                {
                    comboBoxAkumulasi.SelectedIndex = -1;
                }
                else
                {
                    comboBoxAkumulasi.SelectedValue = o.CoaAkumulasiPenyusutan.ToString().Trim();
                }

                if (o.CoaBebanPenyusutan == null)
                {
                    comboBoxBebanPenyusutan.SelectedIndex = -1;
                }
                else 
                {
                    comboBoxBebanPenyusutan.SelectedValue = o.CoaBebanPenyusutan.ToString().Trim();
                }
            }
        }
        private bool IsValid()
        {
            string sPesan = "";
            if (textBoxKd.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Kode";
            }

            if (textBoxNm.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Nama Kelompok";
            }

            //if (comboBoxAkumulasi.SelectedIndex ==-1)
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Akumulasi Penyusutan [Akun]";
            //}

            //if (comboBoxBebanPenyusutan.SelectedIndex == -1)
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Beban Penyusutan [Akun]";
            //}

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
        private void FMAsetKelompok_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (this.fInduk != null)
            //{
            //    this.fInduk.Reload();
            //}
        }

    }
}
