using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Andhana;

namespace inovaPOS
{
    [AdnScObjectAtr("Form: Input Gudang", "Gudang")]
    public partial class FMGudang : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs =new BindingSource();
        private string AppName;

        public FMGudang(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.DokumenBaru();
            this.FillDataGridView("");
        }
        private void FillDataGridView(string sFilter)
        {
            this.UseWaitCursor = true;
            Application.DoEvents();
            dgv.AutoGenerateColumns = false;
            bs.DataSource = new AdnGudangDao(this.cnn).GetAll();
            dgv.Columns["kd"].HeaderText = "Kode";
            dgv.Columns["nm"].HeaderText = "Nama Gudang";
            dgv.Columns["kd"].DataPropertyName = "kd_gudang";
            dgv.Columns["nm"].DataPropertyName = "nm_gudang";
            dgv.DataSource = bs;

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
            panelTbl.Enabled = true;

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
            if (IsValid())
            {
                AdnGudang o = new AdnGudang();
                o.nm_gudang = textBoxNm.Text.Trim();

                o.uid = "Adn";
                o.uid_edit = "Adn";

                AdnGudangDao dao = new AdnGudangDao(this.cnn);
                switch (this.ModeEdit)
                {
                    case AdnModeEdit.BARU:
                        dao.Simpan(o);
                        textBoxKd.Text = o.kd_gudang;
                        bs.Add(o);
                        bs.Position = dgv.Rows.Count - 1;
                        break;

                    case AdnModeEdit.UBAH:
                        o.kd_gudang = textBoxKd.Text.ToString();
                        List<AdnGudang> lst = this.bs.DataSource as List<AdnGudang>;
                        lst[dgv.CurrentRow.Index] = o;
                        this.bs.ResetItem(dgv.CurrentRow.Index);
                        dao.Update(o);
                        break;
                }

                panelHdr.Enabled = false;
                panelTbl.Enabled = true;

                toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonHapus.Enabled = true;

                toolStripButtonBatal.Enabled = false;
                toolStripButtonSimpan.Enabled = false;
            }
        }
        private void Hapus()
        {
            if (MessageBox.Show("Hapus Data, Kode = " + textBoxKd.Text.ToString() + " ?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                AdnGudangDao dao = new AdnGudangDao(this.cnn);
                dao.Hapus(textBoxKd.Text.ToString());
                bs.RemoveCurrent();
                this.Batal();
            }
        }
        private void DokumenBaru()
        {
            AdnFungsi.Bersih(this, true);
            this.ModeEdit = AdnModeEdit.BARU;
            panelHdr.Enabled = true;
            textBoxKd.Enabled = false;
            textBoxNm.Focus();
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
        private void GetData(string Kd)
        {
            AdnGudang o = new AdnGudangDao(this.cnn).Get(Kd);

            if (o != null)
            {
                this.ModeEdit = AdnModeEdit.BACA;
                toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonBatal.Enabled = true;
                toolStripButtonHapus.Enabled = true;
                toolStripButtonSimpan.Enabled = false;
                
                textBoxKd.Text = o.kd_gudang.ToString();
                textBoxNm.Text = o.nm_gudang;
            }
        }
        private bool IsValid()
        {
            string sPesan = "";

            if (textBoxNm.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Nama Gudang";
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

        private void FMSatuan_KeyDown(object sender, KeyEventArgs e)
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
                        e.Handled =true;
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
        private void dgv_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null)
            {
                this.GetData(dgv.CurrentRow.Cells["kd"].Value.ToString());
            }
        }
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.CurrentRow != null)
            {
                this.GetData(dgv.CurrentRow.Cells["kd"].Value.ToString());
            }
        }
      
    }
}
