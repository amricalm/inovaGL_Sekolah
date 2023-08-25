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
    public partial class FMGudang2 : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs =new BindingSource();
        private string AppName;
        private FDMGudang fInduk;

        public FMGudang2(SqlConnection cnn,string AppName,short ModeEdit,string kd,object fInduk)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;
            this.fInduk = (FDMGudang)fInduk;
            
            if (this.ModeEdit == AdnModeEdit.BACA)
            {
                this.GetData(kd);
            }
            else
            {
                this.DokumenBaru();
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

            textBoxAlamat.Enabled = false;
            textBoxNamaGudang.Enabled = false;
            textBoxKota.Focus();
            
            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = true;

            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;

        }
        private void Simpan()
        {
            //if (IsValid())
            //{
            //    AdnGudang o = new AdnGudang();
            //    o.barcode = textBoxNamaGudang.Text.Trim();
            //    o.kd_Gudang = textBoxAlamat.Text;
            //    o.nm_Gudang= textBoxKota.Text.Trim();
            //    o.kd_satuan=  comboBoxSatuan.SelectedValue.ToString();
            //    o.kd_group = comboBoxGroup.SelectedValue.ToString();
            //    o.stock_min =AdnFungsi.CInt(textBoxPOS,true);
            //    o.harga_jual = AdnFungsi.CDec(textBoxPropinsi);
            //    o.kd_pemasok = comboBoxPemasok.SelectedValue.ToString();
                
            //    o.uid = "Adn";
            //    o.uid_edit = "Adn";

            //    AdnGudangDao dao = new AdnGudangDao(this.cnn);
            //    switch (this.ModeEdit)
            //    {
            //        case AdnModeEdit.BARU:
            //            dao.Simpan(o);
            //            break;

            //        case AdnModeEdit.UBAH:
            //            dao.Update(o);
            //            break;
            //    }

            //    panelHdr.Enabled = false;

            //    toolStripButtonTambah.Enabled = true;
            //    toolStripButtonEdit.Enabled = true;
            //    toolStripButtonHapus.Enabled = true;

            //    toolStripButtonBatal.Enabled = false;
            //    toolStripButtonSimpan.Enabled = false;
            //}
        }
        private void Hapus()
        {
            if (MessageBox.Show("Hapus Data, Kode = " + textBoxAlamat.Text.ToString() + " ?",this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                AdnGudangDao dao = new AdnGudangDao(this.cnn);
                dao.Hapus(textBoxAlamat.Text.ToString());
                this.Batal();
            }
        }
        private void DokumenBaru()
        {
            AdnFungsi.Bersih(this, true);
            this.ModeEdit = AdnModeEdit.BARU;
            panelHdr.Enabled = true;

            textBoxNamaGudang.Enabled = true;
            textBoxAlamat.Enabled = true;

            textBoxNamaGudang.Focus();
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
            //AdnGudang o = new AdnGudangDao(this.cnn).Get(Kd);

            //if (o != null)
            //{
            //    this.ModeEdit = AdnModeEdit.BACA;
            //    toolStripButtonTambah.Enabled = true;
            //    toolStripButtonEdit.Enabled = true;
            //    toolStripButtonBatal.Enabled = true;
            //    toolStripButtonHapus.Enabled = true;
            //    toolStripButtonSimpan.Enabled = false;

            //    textBoxNamaGudang.Enabled = false;
            //    textBoxAlamat.Enabled = false;

            //    textBoxKota.Focus();

            //    textBoxAlamat.Text = o.kd_Gudang;
            //    textBoxNamaGudang.Text = o.barcode;

            //    textBoxKota.Text = o.nm_Gudang;
            //    comboBoxSatuan.SelectedValue = o.kd_satuan;
            //    comboBoxGroup.SelectedValue = o.kd_group;
            //    textBoxPOS.Text = o.stock_min.ToString();
            //    textBoxPropinsi.Text = o.harga_jual.ToString("N0");
            //    comboBoxPemasok.SelectedValue = o.kd_pemasok;
            //}
        }
        private bool IsValid()
        {
            //string sPesan = "";

            //if (textBoxNamaGudang.Text.ToString().Trim() == "")
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Barcode";
            //}

            //if (textBoxAlamat.Text.ToString().Trim() == "")
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Kode Gudang";
            //}

            //if (textBoxKota.Text.ToString().Trim() == "")
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Nama Gudang";
            //}

            //if (comboBoxGroup.SelectedIndex == -1)
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Group Gudang";
            //}

            //if (comboBoxSatuan.SelectedIndex == -1)
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Satuan";
            //}

            //if (AdnFungsi.CDec(textBoxPropinsi) == 0)
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Harga Jual";
            //}

            //if (comboBoxPemasok.SelectedIndex == -1)
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Pemasok";
            //}

            //if (sPesan != "")
            //{
            //    sPesan = sPesan + " Harus Diisi.\n";
            //}

            //if (sPesan == "")
            //{
            //    return true;
            //}
            //else
            //{
            //    MessageBox.Show(sPesan, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return false;
            //}

            return true;
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
            //if (textBoxKd.Text.ToString().Trim() != "")
            //{
            //    this.GetData(textBoxKd.Text.ToString());
            //}
            if (textBoxAlamat.Text.ToString().Trim() == "")
            {
                textBoxAlamat.Text = textBoxNamaGudang.Text;
            }
        }
        private void FMGudang_KeyDown(object sender, KeyEventArgs e)
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

        private void textBoxBarcode_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxNamaGudang.Text.ToString().Trim() != "")
            {
                this.GetData(textBoxNamaGudang.Text.ToString());
            }
        }

        private void textBoxStockMinimal_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void textBoxHargaJual_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void textBoxHargaJual_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

      
    }
}
