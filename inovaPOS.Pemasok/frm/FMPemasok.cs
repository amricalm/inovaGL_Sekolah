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
    [AdnScObjectAtr("Form: Input Pemasok", "Pemasok")]
    public partial class FMPemasok : Andhana.AdnEntriForm
    {
        private AdnScGroupRole AdnRole;
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs = new BindingSource();
        private string AppName;

        public FMPemasok(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            this.FillDataGridView("");
            //this.AdnRole = new AdnScGroupRoleDao(this.cnn).GetByKd(AppVar.AppPengguna.kd_group,this.Name);
            this.DokumenBaru();
        }

        private void FillDataGridView(string sFilter)
        {
            dgv.AutoGenerateColumns = false;
            this.UseWaitCursor = true;
            Application.DoEvents();
            this.bs.DataSource= new AdnContactPersonDao(this.cnn).GetByPS(textBoxKd.Text.ToString());
            dgv.DataSource =bs;

            if (dgv.RowCount == 0)
            {
                //toolStripButtonPilih.Enabled = false;
            }
            else
            {
                //toolStripButtonPilih.Enabled = true;
            }
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
            AdnPemasok o = new AdnPemasok();
            o.nm_ps = textBoxNm.Text.Trim();
            o.alamat = textBoxAlamat.Text.Trim();
            o.kota = textBoxKota.Text.Trim();
            o.pos = textBoxPos.Text.Trim();
            o.propinsi = textBoxPropinsi.Text.Trim();
            o.telp = textBoxTelp.Text.Trim();
            o.fax = textBoxFax.Text.Trim();
            o.email = textBoxEmail.Text.Trim();
            o.web = textBoxWeb.Text;

            o.uid = "adn";
            o.uid_edit = "adn";

            AdnPemasokDao dao = new AdnPemasokDao(this.cnn);
            switch (this.ModeEdit)
            {
                case AdnModeEdit.BARU:
                    dao.Simpan(o);
                    textBoxKd.Text = o.kd_ps;
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
                AdnPemasokDao dao = new AdnPemasokDao(this.cnn);
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
            AdnPemasokDao dao = new AdnPemasokDao(this.cnn);
            AdnPemasok o = dao.Get(Kd);

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
                dgv.DataSource = bs;
            }
        }

        private void buttonTblTambah_Click(object sender, EventArgs e)
        {
            FMCp ofm = new FMCp(this,this.cnn,this.AppName);
            ofm.ShowDialog();
        }
        private void buttonTblHapus_Click(object sender, EventArgs e)
        {
            AdnContactPersonDao dao = new AdnContactPersonDao(this.cnn);
            dao.Hapus(int.Parse(dgv.CurrentRow.Cells["kd"].Value.ToString()));
            bs.RemoveCurrent();
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
            FDMPemasok ofm = new FDMPemasok(this,this.cnn,this.AppName);
            ofm.ShowDialog();
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

        public void TambahDetail(object obj)
        {
            this.bs.Add((AdnContactPerson)obj);
        }

        public void RefreshDetail(object obj)
        {
            int idx = this.bs.Position;
            this.bs.RemoveCurrent();
            this.bs.Insert(idx,(AdnContactPerson)obj);
            //this.bs.ResetCurrentItem();
        }

        public void HapusDetail()
        {
            this.bs.RemoveCurrent();
        }

        public string GetKdPs()
        {
            return textBoxKd.Text.ToString().Trim();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgv.CurrentRow!=null)
            {
            FMCp ofm = new FMCp(this,this.cnn,this.AppName);
            ofm.GetData(int.Parse(dgv.CurrentRow.Cells["kd"].Value.ToString()));
            ofm.ShowDialog();
            }
        }

        private void textBoxPos_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void textBoxAset_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void textBoxOmset_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void textBoxJmhKaryawan_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void textBoxModal_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void FMPs_Load(object sender, EventArgs e)
        {

        }

        
        

        
    }
}
