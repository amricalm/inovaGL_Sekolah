using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using Andhana;

namespace inovaPOS
{
    [AdnScObjectAtr("Form: Input Contact Person", "Pemasok")]
    public partial class FMCp : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private FMPemasok fInduk;
        private string AppName;

        public FMCp(object fInduk, SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.DokumenBaru();
            this.fInduk = (FMPemasok)fInduk;
        }

        private void Simpan()
        {
            AdnContactPerson o = new AdnContactPerson();
            o.nm_lengkap = textBoxNm.Text.Trim();
            o.jabatan = textBoxJabatan.Text.Trim();
            o.telp = textBoxTelp.Text.Trim();
            o.hp = textBoxHP.Text.Trim();
            o.email = textBoxEmail.Text.Trim();
            o.ket = textBoxKet.Text.Trim();
            o.kd_ps = fInduk.GetKdPs();

            AdnContactPersonDao dao = new AdnContactPersonDao(this.cnn);
            switch (this.ModeEdit)
            {
                case AdnModeEdit.BARU:
                    dao.Simpan(o);
                    textBoxKd.Text = o.kd_cp.ToString();
                    this.fInduk.TambahDetail(o);
                    break;

                case AdnModeEdit.UBAH:
                    o.kd_cp = int.Parse(textBoxKd.Text);
                    dao.Update(o);
                    this.fInduk.RefreshDetail(o);
                    break;
            }
            this.Batal();
        }
        private void DokumenBaru()
        {
            this.ModeEdit = AdnModeEdit.BARU;
            toolStripButtonTambah.Enabled = false;
            toolStripButtonHapus.Enabled = false;
            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;
        }
        private void Batal()
        {
            AdnFungsi.Bersih(this);
            textBoxKd.Focus();
            this.DokumenBaru();
        }
        private void Hapus()
        {
            if (MessageBox.Show("Hapus Data, Kode = " + textBoxKd.Text.ToString() + " ?", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                AdnContactPersonDao dao = new AdnContactPersonDao(this.cnn);
                dao.Hapus(int.Parse(textBoxKd.Text));
                this.Batal();
                this.fInduk.HapusDetail();
            }
        }
        public void GetData(int Kd)
        {
            AdnContactPersonDao dao = new AdnContactPersonDao(this.cnn);
            AdnContactPerson o = dao.Get(Kd);

            if (o != null)
            {
                this.ModeEdit = AdnModeEdit.UBAH;
                toolStripButtonTambah.Enabled = false;
                toolStripButtonBatal.Enabled = true;
                toolStripButtonHapus.Enabled = true;
                toolStripButtonSimpan.Enabled = true;

                textBoxKd.Text = Kd.ToString().Trim();
                textBoxNm.Text = o.nm_lengkap.Trim();
                textBoxJabatan.Text = o.jabatan.Trim();
                textBoxTelp.Text = o.telp.Trim();
                textBoxHP.Text = o.hp.Trim();
                textBoxEmail.Text = o.email.Trim();
                textBoxKet.Text = o.ket.Trim();

            }
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
            if (textBoxKd.Text.ToString().Trim() != "")
            {
                this.GetData(int.Parse(textBoxKd.Text.ToString()));
            }
        }
        private void FMCp_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F12:
                    this.Simpan();
                    break;

                case Keys.Escape:
                    this.Close();
                    break;
            }
        }
    }
}
