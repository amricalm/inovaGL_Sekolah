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
    [AdnScObjectAtr("Form: Input Barang", "Barang")]
    public partial class FMBarang : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs =new BindingSource();
        private string AppName;
        private FDMBarang fInduk;

        public FMBarang(SqlConnection cnn,string AppName,short ModeEdit,string kd,object fInduk)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;
            this.fInduk = (FDMBarang)fInduk;

            //--- ComboBox Group
            comboBoxGroup.DisplayMember = "nm_group";
            comboBoxGroup.ValueMember = "kd_group";
            comboBoxGroup.DataSource = new AdnGroupBarangDao(this.cnn).GetAll();
            //--- END ---------------

            //--- ComboBox Satuan
            comboBoxSatuan.DisplayMember = "nm_satuan";
            comboBoxSatuan.ValueMember = "kd_satuan";
            comboBoxSatuan.DataSource = new AdnSatuanDao(this.cnn).GetAll();
            //--- END ---------------

            //--- ComboBox Pemasok
            comboBoxPemasok.DisplayMember = "nm_ps";
            comboBoxPemasok.ValueMember = "kd_ps";
            comboBoxPemasok.DataSource = new AdnPemasokDao(this.cnn).GetAll();
            //--- END ---------------

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

            textBoxKd.Enabled = false;
            textBoxBarcode.Enabled = false;
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
                AdnBarang o = new AdnBarang();
                o.barcode = textBoxBarcode.Text.Trim();
                o.kd_barang = textBoxKd.Text;
                o.nm_barang= textBoxNm.Text.Trim();
                o.kd_satuan=  comboBoxSatuan.SelectedValue.ToString();
                o.kd_group = comboBoxGroup.SelectedValue.ToString();
                o.stock_min =AdnFungsi.CInt(textBoxStockMinimal,true);
                o.harga_jual = AdnFungsi.CDec(textBoxHargaJual);
                o.kd_pemasok = comboBoxPemasok.SelectedValue.ToString();
                
                o.uid = "Adn";
                o.uid_edit = "Adn";

                AdnBarangDao dao = new AdnBarangDao(this.cnn);
                switch (this.ModeEdit)
                {
                    case AdnModeEdit.BARU:
                        dao.Simpan(o);
                        break;

                    case AdnModeEdit.UBAH:
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
            if (MessageBox.Show("Hapus Data, Kode = " + textBoxKd.Text.ToString() + " ?",this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                AdnBarangDao dao = new AdnBarangDao(this.cnn);
                dao.Hapus(textBoxKd.Text.ToString());
                this.Batal();
            }
        }
        private void DokumenBaru()
        {
            AdnFungsi.Bersih(this, true);
            this.ModeEdit = AdnModeEdit.BARU;
            panelHdr.Enabled = true;

            textBoxBarcode.Enabled = true;
            textBoxKd.Enabled = true;

            textBoxBarcode.Focus();
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
            AdnBarang o = new AdnBarangDao(this.cnn).Get(Kd);

            if (o != null)
            {
                this.ModeEdit = AdnModeEdit.BACA;
                toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonBatal.Enabled = true;
                toolStripButtonHapus.Enabled = true;
                toolStripButtonSimpan.Enabled = false;

                textBoxBarcode.Enabled = false;
                textBoxKd.Enabled = false;

                textBoxNm.Focus();

                textBoxKd.Text = o.kd_barang;
                textBoxBarcode.Text = o.barcode;

                textBoxNm.Text = o.nm_barang;
                comboBoxSatuan.SelectedValue = o.kd_satuan;
                comboBoxGroup.SelectedValue = o.kd_group;
                textBoxStockMinimal.Text = o.stock_min.ToString();
                textBoxHargaJual.Text = o.harga_jual.ToString("N0");
                comboBoxPemasok.SelectedValue = o.kd_pemasok;
            }
        }
        private bool IsValid()
        {
            string sPesan = "";

            if (textBoxBarcode.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Barcode";
            }

            if (textBoxKd.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Kode Barang";
            }

            if (textBoxNm.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Nama Barang";
            }

            if (comboBoxGroup.SelectedIndex == -1)
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Group Barang";
            }

            if (comboBoxSatuan.SelectedIndex == -1)
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Satuan";
            }

            if (AdnFungsi.CDec(textBoxHargaJual) == 0)
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Harga";
            }

            if (comboBoxPemasok.SelectedIndex == -1)
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Pemasok";
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
            //if (textBoxKd.Text.ToString().Trim() != "")
            //{
            //    this.GetData(textBoxKd.Text.ToString());
            //}
            if (textBoxKd.Text.ToString().Trim() == "")
            {
                textBoxKd.Text = textBoxBarcode.Text;
            }
        }
        private void FMBarang_KeyDown(object sender, KeyEventArgs e)
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
            if (textBoxBarcode.Text.ToString().Trim() != "")
            {
                this.GetData(textBoxBarcode.Text.ToString());
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

      
    }
}
