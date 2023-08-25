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

namespace inovaGL
{
    [AdnScObjectAtr("Form: Input Akun", "Akun")]
    public partial class FMAkun : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private string AppName;
        //private FDMPelanggan fInduk;

        public FMAkun(SqlConnection cnn, string AppName, short ModeEdit, string kd, object fInduk)
        {
            InitializeComponent();
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;
            this.cnn = cnn;
            //this.fInduk = (FDMPelanggan)fInduk;

            //new AdnSysAkunJenisDao(this.cnn).SetCombo(comboBoxKelompok);
            //if (comboBoxKelompok.SelectedIndex > -1)
            //{
            //    string KdKelompok = comboBoxKelompok.SelectedValue.ToString();
            //    new AdnSysAkunGolonganDao(this.cnn).SetComboByJenis(comboBoxSubKelompok, KdKelompok);
            //}
            new AdnSysAkunGolonganDao(this.cnn).SetCombo(comboBoxSubKelompok);

            if (this.ModeEdit == AdnModeEdit.BACA)
            {
                this.GetData(kd);
            }
            else
            {
                this.DokumenBaru();
            }

        }
        private void FMAkun_KeyDown(object sender, KeyEventArgs e)
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
                AdnAkun o = new AdnAkun();
                o.KdAkun = textBoxKd.Text.ToString().Trim();
                o.NmAkun = textBoxNm.Text.ToString().Trim();
                
                if (radioButtonTotal.Checked)
                {
                    o.Tipe = AdnVar.Klasifikasi.TOTAL;
                }
                else if (radioButtonDetail.Checked)
                {
                    o.Tipe = AdnVar.Klasifikasi.DETAIL;
                }

                o.KdInduk = textBoxAkunInduk.Text.ToString().Trim();
                o.Turunan = new AdnAkunDao(this.cnn).GetTurunan(o.KdInduk);

                if (o.KdInduk!="")
                {
                    o.Turunan = o.Turunan + 1;
                }

                o.KdGolongan = comboBoxSubKelompok.SelectedValue.ToString().Trim();

                if (radioButtonDebet.Checked)
                {
                    o.DK = AdnVar.SaldoNormal.DEBET;
                }
                else if (radioButtonKredit.Checked)
                {
                    o.DK = AdnVar.SaldoNormal.KREDIT;
                }

                if (checkBoxTampilLoket.Checked)
                {
                    o.TampilDiLoket = true;
                }
                else
                {
                    o.TampilDiLoket = false;
                }

                AdnAkunDao dao = new AdnAkunDao(this.cnn,AppVar.AppPengguna);
                switch (this.ModeEdit)
                {
                    case AdnModeEdit.BARU:
                        dao.Simpan(o);
                        break;

                    case AdnModeEdit.UBAH:
                        o.KdAkun = textBoxKd.Text;
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
            if (MessageBox.Show("Hapus Data, Akun = " + textBoxNm.Text.ToString() + " ?",this.AppName , MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                AdnAkunDao dao = new AdnAkunDao(this.cnn);
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
            AdnAkun o = new AdnAkunDao(this.cnn,AppVar.AppPengguna).Get(Kd);

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
                textBoxNm.Text = o.NmAkun;

                if (o.Tipe == AdnVar.Klasifikasi.TOTAL)
                {
                    radioButtonTotal.Checked = true;
                }
                else if (o.Tipe == AdnVar.Klasifikasi.DETAIL)
                {
                    radioButtonDetail.Checked = true;
                }
                textBoxAkunInduk.Text = o.KdInduk;
                if (textBoxAkunInduk.Text.Trim() != "")
                {
                    //textBoxAkunIndukNama.Text = new AdnAkunDao(this.cnn).Get(textBoxAkunInduk.Text).NmAkun.Trim();
                    AdnAkun AkunInduk = new AdnAkunDao(this.cnn).Get(textBoxAkunInduk.Text);
                    if (AkunInduk != null)
                    {
                        textBoxAkunIndukNama.Text = AkunInduk.NmAkun;
                    }
                    else
                    {
                        textBoxAkunIndukNama.Text = "";
                    }
                    
                }
                if (o.KdGolongan == null)
                {
                    comboBoxSubKelompok.SelectedIndex = -1;
                    comboBoxKelompok.SelectedIndex = -1;
                }
                else
                {
                    AdnSysAkunGolongan oGol =  new AdnSysAkunGolonganDao(this.cnn).Get(o.KdGolongan.ToString());
                    if (oGol != null)
                    {
                        comboBoxKelompok.SelectedValue = oGol.KdJenis.Trim();
                    }
                    comboBoxSubKelompok.SelectedValue = o.KdGolongan;
                }

                if (o.DK == AdnVar.SaldoNormal.DEBET)
                {
                    radioButtonDebet.Checked = true;
                }
                else if (o.DK == AdnVar.SaldoNormal.KREDIT)
                {
                    radioButtonKredit.Checked = true;
                }

                checkBoxTampilLoket.Checked = o.TampilDiLoket;

            }
        }
        private bool IsValid()
        {
            string sPesan = "";
            if (textBoxKd.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Kode Akun";
            }

            if (textBoxNm.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Nama Akun";
            }

            if (comboBoxSubKelompok.SelectedIndex ==-1)
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Golongan";
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

        private void FMAkun_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (this.fInduk != null)
            //{
            //    this.fInduk.Reload();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //new AdnAkunDao(this.cnn).ImportDbAccessMsGolongan();
        }

        private void textBoxAkunInduk_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    FDCariAkun ofm = new FDCariAkun(this.cnn, this.AppName, this,null);
                    ofm.ShowDialog();
                    break;
            }
        }
        public void SetDataAkunInduk(string KdAkunInduk, string NmAkunInduk)
        {
            textBoxAkunInduk.Text = KdAkunInduk.Trim();
            textBoxAkunIndukNama.Text = NmAkunInduk.Trim();
        }

        private void buttonLookup_Click(object sender, EventArgs e)
        {
            FDCariAkun ofm = new FDCariAkun(this.cnn, this.AppName, this,null);
            ofm.ChildFormUpdate += new FDCariAkun.ChildFormUpdateHandler(SetKdAkunInduk);
            ofm.Owner = this;
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
        private void SetKdAkunInduk(object sender, ChildEventArgs e)
        {
            textBoxAkunInduk.Text = e.KdAkun.ToString().Trim();
            textBoxAkunIndukNama.Text = e.NmAkun.ToString().Trim();
        }

    }
}
