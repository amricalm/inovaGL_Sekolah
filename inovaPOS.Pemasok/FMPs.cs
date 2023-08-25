using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Andhana;

namespace inovaPOS
{
    [AdnScObjectAtr("Form: Input Perusahaan", "Perusahaan")]
    public partial class FMPs : Andhana.AdnEntriForm
    {
        //private AdnScGroupRole AdnRole;
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs = new BindingSource();
        private string AppName;

        public FMPs(SqlConnection cnn, string Args)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.FillDataGridView("");
            this.AppName = Args;
            //this.AdnRole = new AdnScGroupRoleDao(this.cnn).GetByKd(AppVar.AppPengguna.kd_group,this.Name);
            this.DokumenBaru();
        }

        private void FillDataGridView(string sFilter)
        {
            dgv.AutoGenerateColumns = false;
            this.UseWaitCursor = true;

            this.bs.DataSource= new AdnContactPersonDao(this.cnn).GetByPS(textBoxKd.Text.ToString());
            dgv.DataSource =bs;
            this.UseWaitCursor = false;
        }

        private void Tambah()
        {
            this.DokumenBaru();
        }
        private void Edit()
        {
            this.ModeEdit = AdnModeEdit.UBAH;

            panelHdr.Enabled = true;
            panelDtl.Enabled = true;

            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = true;

            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;
        }
        private void Simpan()
        {
            AdnPerusahaan o = new AdnPerusahaan();
            o.nm_ps = textBoxNm.Text.Trim();
            o.alamat = textBoxAlamat.Text.Trim();
            o.kota = textBoxKota.Text.Trim();
            o.pos = textBoxPos.Text.Trim();
            o.propinsi = textBoxPropinsi.Text.Trim();
            o.telp = textBoxTelp.Text.Trim();
            o.fax = textBoxFax.Text.Trim();
            o.email = textBoxEmail.Text.Trim();
            o.web = textBoxWeb.Text;


            AdnPerusahaanDao dao = new AdnPerusahaanDao(this.cnn);
            switch (this.ModeEdit)
            {
                case AdnModeEdit.BARU:
                    string kd = dao.Simpan(o);
                    textBoxKd.Text = kd.ToString();
                    o.kd_ps = kd;
                    break;

                case AdnModeEdit.UBAH:
                    o.kd_ps = textBoxKd.Text.ToString().Trim();
                    dao.Update(o);
                    break;
            }

            panelHdr.Enabled = false;
            panelDtl.Enabled = true;

            toolStripButtonTambah.Enabled = true;
            toolStripButtonEdit.Enabled = true;
            toolStripButtonHapus.Enabled = true;

            toolStripButtonBatal.Enabled = false;
            toolStripButtonSimpan.Enabled = false;
        }
        private void Hapus()
        {
            if (MessageBox.Show("Hapus Data, Kode = " + textBoxKd.Text.ToString() + " ?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                AdnPerusahaanDao dao = new AdnPerusahaanDao(this.cnn);
                dao.Hapus(textBoxKd.Text);
                this.Batal();
            }
        }
        private void DokumenBaru()
        {
            AdnFungsi.Bersih(this);
            this.ModeEdit = AdnModeEdit.BARU;
            textBoxNm.Focus();

            //if (this.AdnRole.tambah)
            //{
                toolStripButtonSimpan.Enabled = true;
                panelHdr.Enabled = true;
            //}
            //else
            //{
            //    toolStripButtonSimpan.Enabled =false;
            //    panelHdr.Enabled = false;
            //}
            
            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = false;
            toolStripButtonBatal.Enabled = true;

        }
        private void Batal()
        {
            this.DokumenBaru();
        }
        public void GetData(string Kd)
        {
            AdnPerusahaanDao dao = new AdnPerusahaanDao(this.cnn);
            AdnPerusahaan o = dao.Get(Kd);

            if (o != null)
            {
                this.ModeEdit = AdnModeEdit.BACA;
                panelDtl.Enabled = false;
                toolStripButtonTambah.Enabled = true;
                //if (this.AdnRole.edit)
                //{
                    toolStripButtonEdit.Enabled = true;
                //}
                //else
                //{
                //    toolStripButtonEdit.Enabled = false;
                //}

                //if (this.AdnRole.hapus)
                //{
                    toolStripButtonHapus.Enabled = true;
                //}
                //else
                //{
                //    toolStripButtonHapus.Enabled = false;
                //}
                toolStripButtonBatal.Enabled = true;
                toolStripButtonSimpan.Enabled = false;

                textBoxKd.Text = Kd.Trim();
                textBoxNm.Text = o.nm_ps.Trim();
                textBoxAlamat.Text = o.alamat.Trim();
                textBoxKota.Text = o.kota.Trim();
                textBoxPos.Text = o.pos.Trim();
                textBoxPropinsi.Text = o.propinsi.Trim();
                textBoxTelp.Text = o.telp.Trim();
                textBoxFax.Text = o.fax.Trim();
                textBoxEmail.Text = o.email.Trim();
                textBoxWeb.Text = o.web;

                this.bs.DataSource = new AdnContactPersonDao(this.cnn).GetByPS(textBoxKd.Text.ToString());
            }
        }

        private void buttonTblTambah_Click(object sender, EventArgs e)
        {
            //FMCp ofm = new FMCp(this);
            //ofm.ShowDialog();
        }
        private void buttonTblHapus_Click(object sender, EventArgs e)
        {
            //AdnContactPersonDao dao = new AdnContactPersonDao(this.cnn);
            //dao.Hapus(int.Parse(dgv.CurrentRow.Cells["kd"].Value.ToString()));
            //bs.RemoveCurrent();
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
        private void toolStripButtonDaftar_Click(object sender, EventArgs e)
        {
            //FDMPs ofm = new FDMPs(this);
            //ofm.ShowDialog();
        }

        private void textBoxKd_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxKd.Text.ToString().Trim() != "")
            {
                this.GetData(textBoxKd.Text.ToString());
            }
        }
        private void FMPs_KeyDown(object sender, KeyEventArgs e)
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

        public void TambahDetail(object obj)
        {
            //this.bs.Add((AdnContactPerson)obj);
        }

        public void RefreshDetail(object obj)
        {
            //int idx = this.bs.Position;
            //this.bs.RemoveCurrent();
            //this.bs.Insert(idx,(AdnContactPerson)obj);
            ////this.bs.ResetCurrentItem();
        }

        public void HapusDetail()
        {
            //this.bs.RemoveCurrent();
        }

        public string GetKdPs()
        {
            //return textBoxKd.Text.ToString().Trim();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if(dgv.CurrentRow!=null)
            //{
            //FMCp ofm = new FMCp(this);
            //ofm.GetData(int.Parse(dgv.CurrentRow.Cells["kd"].Value.ToString()));
            //ofm.ShowDialog();
            //}
        }

        private void textBoxPos_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }
        
    }
}
