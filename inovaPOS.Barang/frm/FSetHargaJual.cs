using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using inovaPOS;
using Andhana;

namespace inovaPOS
{
    [AdnScObjectAtr("Form: Pengaturan Harga Jual", "Barang")]
    public partial class FSetHargaJual : Andhana.AdnBaseForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private string AppName;
        BindingSource bs = new BindingSource();

        public FSetHargaJual(SqlConnection cnn,string AppName,short ModeEdit,string kd)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;

            dgv.AutoGenerateColumns = false;
            new AdnGroupBarangDao(this.cnn).SetCombo(comboBoxGroup);
            new inovaPOS.AdnPemasokDao(this.cnn).SetCombo(comboBoxPemasok);
            //new inovaPOS.AdnPemasokDao(this.cnn).set
            //if (this.ModeEdit == AdnModeEdit.BACA)
            //{
            //    this.GetData(kd);
            //}
            //else
            //{
                FillDataGridView("");
                //this.DokumenBaru();
            //}
        }
        private void FSetHargaJual_KeyDown(object sender, KeyEventArgs e)
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
                        if (this.IsValid())
                        {
                            this.Simpan();
                        }
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
                    }
                    break;

                case Keys.D:
                    if (Control.ModifierKeys == Keys.Control)
                    {

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
        private void FillDataGridView(string sFilter)
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            string sPemasok = "";
            string sGroup = "";
            if (comboBoxGroup.SelectedValue != null)
            {
                sGroup = comboBoxGroup.SelectedValue.ToString();
            }
            if (comboBoxPemasok.SelectedValue != null)
            {
                sPemasok = comboBoxPemasok.SelectedValue.ToString();
            }

            bs.DataSource = new AdnBarangDao(this.cnn).GetByArgs(sGroup, sPemasok);
            dgv.DataSource = bs;

            this.UseWaitCursor = false;
        }

        #region "Prosedur CRUD"
        private void Simpan()
        {
            dgv.EndEdit();
            if (this.IsValid())
            {
                AdnBarangDao itemDao = new AdnBarangDao(this.cnn);
                foreach (DataGridViewRow baris in dgv.Rows)
                {
                    if (!baris.IsNewRow)
                    {
                        decimal hargaJual = decimal.Parse((baris.Cells["harga_jual"].Value ?? 0).ToString());

                        AdnBarang item = new AdnBarang();
                        item.kd_barang = baris.Cells["kd_barang"].Value.ToString();
                        item.barcode = baris.Cells["barcode"].Value.ToString();
                        item.harga_jual= hargaJual;

                        //dummy
                        item.nm_barang = "";
                        item.kd_group = "";
                        item.kd_pemasok = "";
                        item.kd_satuan = "";
                        item.stock_min = 0;

                        itemDao.UpdateHarga(item);

                    }
                }

                //panelHdr.Enabled = false;
                panelDtl.Enabled = false;

                toolStripButtonTambah.Enabled = false;
                toolStripButtonEdit.Enabled = false;
                toolStripButtonHapus.Enabled = false;

                toolStripButtonBatal.Enabled = false;
                toolStripButtonSimpan.Enabled = false;

                MessageBox.Show("Proses Simpan Berhasil...", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }//end isValid
        }
        private void Edit()
        {
            this.ModeEdit = AdnModeEdit.UBAH;
            //panelHdr.Enabled = true;
            panelDtl.Enabled = true;

            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = true;

            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;
        }
        private void Batal()
        {
            this.DokumenBaru();

        }
        private void Hapus()
        {
            //if (MessageBox.Show("Hapus Data, Kode = " +  maskedTextBoxTgl.Text.ToString()+ " ?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //{
            //    AdnStockDao dao = new AdnStockDao(this.cnn);
            //    dao.Hapus(AdnFungsi.CDate(maskedTextBoxTgl));
            //    this.Batal();
            //}
        }
        private void Tambah()
        {
            this.DokumenBaru();
        }
        //END - Prosedur CRUD

        #endregion
        private void DokumenBaru()
        {
            try
            {
                AdnFungsi.Bersih(this);
                this.ModeEdit = AdnModeEdit.BARU;
                dgv.Rows.Clear();
                //textBoxTotal.Text = "0";

                //if (this.AdnRole.tambah)
                //{
                toolStripButtonSimpan.Enabled = true;
                //panelHdr.Enabled = true;
                panelDtl.Enabled = true;
                //}
                //else
                //{
                //    toolStripButtonSimpan.Enabled =false;
                //    panelHdr.Enabled = false;
                //}

                toolStripButtonTambah.Enabled = false;
                toolStripButtonEdit.Enabled = false;
                toolStripButtonHapus.Enabled = false;
                toolStripButtonBatal.Enabled = false;
            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            finally
            {
                dgv.Focus();
            }
        }
        private bool IsValid()
        {
            string sPesan = "";

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
        public void GetData(string kd)
        {
            AdnBarangDao dao = new AdnBarangDao(this.cnn);
            List<AdnBarang> lst = dao.GetAll();

            if (lst.Count>0)
            {
                this.ModeEdit = AdnModeEdit.BACA;
                dgv.Rows.Add(lst.Count);
                int iBaris = 0;
                foreach (AdnBarang item in lst)
                {
                    dgv.Rows[iBaris].Cells["kd_barang"].Value = item.kd_barang;
                    dgv.Rows[iBaris].Cells["barcode"].Value = item.barcode;
                    dgv.Rows[iBaris].Cells["nm_barang"].Value = item.nm_barang;
                    dgv.Rows[iBaris].Cells["nm_satuan"].Value = item.satuan.nm_satuan;
                    dgv.Rows[iBaris].Cells["hpp"].Value = item.hpp;
                    dgv.Rows[iBaris].Cells["harga_jual"].Value = item.harga_jual;
                     
                    iBaris++;
                }

                toolStripButtonTambah.Enabled = false;
                toolStripButtonEdit.Enabled = false;
                toolStripButtonBatal.Enabled = false;
                toolStripButtonHapus.Enabled = false;
                toolStripButtonSimpan.Enabled = true;

                dgv.Focus();

            }
        }

        #region "toolStripButton"
        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void toolStripButtonTambah_Click(object sender, EventArgs e)
        {
            this.Tambah();
        }
        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            this.Edit();
        }
        private void toolStripButtonBatal_Click(object sender, EventArgs e)
        {
            this.Batal();
        }
        private void toolStripButtonHapus_Click(object sender, EventArgs e)
        {
            this.Hapus();
        }
        private void toolStripButtonSimpan_Click(object sender, EventArgs e)
        {
            this.Simpan();
        }
        #endregion
        
        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                }
            }
        }
        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dgv.EditingControl.KeyPress+=new KeyPressEventHandler(EditingControl_KeyPress);
        }
        void EditingControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (dgv.Columns[dgv.CurrentCell.ColumnIndex].Name)
            {
                case "harga_jual": 
                    e.Handled = AdnFungsi.CekAngka(e.KeyChar);
                    break;
            }
        }

        private void cariButton_Click(object sender, EventArgs e)
        {
            this.FillDataGridView("");
        }


    }
}
