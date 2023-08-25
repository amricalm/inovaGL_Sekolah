﻿using System;
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
    [AdnScObjectAtr("Form: Input Retur Pembelian", "Retur Pembelian")]
    public partial class FTReturBeli : Andhana.AdnBaseForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private string AppName;
        private FDTReturBeli fInduk;

        public FTReturBeli(SqlConnection cnn,string AppName,short ModeEdit,string kd, object fInduk)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;
            this.fInduk = (FDTReturBeli)fInduk;

            comboBoxPemasok.ValueMember = "kd_ps";
            comboBoxPemasok.DisplayMember = "nm_ps";
            comboBoxPemasok.DataSource = new AdnPemasokDao(this.cnn).GetAll();

            dgv.AutoGenerateColumns = false;
            FillDataGridView("");

            if (this.ModeEdit == AdnModeEdit.BACA)
            {
                this.GetData(kd);
            }
            else
            {
                this.DokumenBaru();
            }
        }
        private void FTReturBeli_KeyDown(object sender, KeyEventArgs e)
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
            this.UseWaitCursor = false;
        }

        #region "Prosedur CRUD"
        private void Simpan()
        {
            dgv.EndEdit();
            if (this.IsValid())
            {

                AdnTReturBeli o = new AdnTReturBeli();

                o.no_faktur = textBoxKd.Text;
                o.tgl = AdnFungsi.CDate(maskedTextBoxTgl);
                o.kd_ps = comboBoxPemasok.SelectedValue.ToString();
                o.ket = textBoxKet.Text.ToString();

                o.uid = "Adn";
                o.uid_edit = o.uid;

                AdnTReturBeliDao dao = new AdnTReturBeliDao(this.cnn);
                AdnTReturBeliDtlDao itemDao = new AdnTReturBeliDtlDao(this.cnn);

                switch (this.ModeEdit)
                {
                    case AdnModeEdit.BARU:
                        dao.Simpan(o);
                        break;

                    case AdnModeEdit.UBAH:
                        itemDao.Hapus(o.no_faktur);
                        dao.Update(o);
                        break;
                }


                foreach (DataGridViewRow baris in dgv.Rows)
                {
                    if (!baris.IsNewRow)
                    {
                        string barcode = (baris.Cells["barcode"].Value ?? "").ToString();
                        int qty = int.Parse((baris.Cells["qty"].Value ?? 0).ToString());

                        if (barcode == "" && qty == 0)
                        {
                            //abaikan...
                        }
                        else
                        {
                            AdnTReturBeliDtl item = new AdnTReturBeliDtl();
                            item.no_faktur = o.no_faktur;
                            item.kd_barang = baris.Cells["kd_barang"].Value.ToString();

                            item.kd_satuan = (baris.Cells["kd_satuan"].Value ?? "").ToString();
                            item.qty = qty;
                            itemDao.Simpan(item);
                        }
                    }
                }

                panelHdr.Enabled = false;
                panelDtl.Enabled = false;

                toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonHapus.Enabled = true;

                toolStripButtonBatal.Enabled = false;
                toolStripButtonSimpan.Enabled = false;
            }//end isValid
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
        private void Batal()
        {
            this.DokumenBaru();

        }
        private void Hapus()
        {
            if (MessageBox.Show("Hapus Data, Kode = " + textBoxKd.Text.ToString() + " ?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                AdnTReturBeliDao dao = new AdnTReturBeliDao(this.cnn);
                dao.Hapus(textBoxKd.Text.ToString());
                this.Batal();
            }
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
                textBoxTotal.Text = "0";

                //if (this.AdnRole.tambah)
                //{
                toolStripButtonSimpan.Enabled = true;
                panelHdr.Enabled = true;
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
                toolStripButtonBatal.Enabled = true;
            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            finally
            {
                textBoxKd.Enabled = true;
                textBoxKd.Focus();
            }
        }
        private bool IsValid()
        {
            string sPesan = "";

            if (textBoxKd.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "No. Retur";
            }

            if (maskedTextBoxTgl.Text.Replace("/", "").ToString().Trim() == "")
            {

                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Tanggal";
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

            if (AdnFungsi.CDec(textBoxTotal) == 0 || textBoxTotal.Text.ToString()=="")
            {
                sPesan = sPesan + "TOTAL Item Tidak Boleh 0 (nol).\n";
            }

            foreach (DataGridViewRow baris in dgv.Rows)
            {
                if (!baris.IsNewRow)
                {
                    //validasi
                    int qty = int.Parse((baris.Cells["qty"].Value ?? 0).ToString());
                    string barcode = (baris.Cells["barcode"].Value ?? "").ToString().Trim();

                    if (barcode == "" && qty == 0)
                    {
                        //abaikan...
                    }
                    else
                    {
                        if (barcode == "")
                        {
                            sPesan = sPesan + " Barang Harus Diisi.";
                            break;
                        }

                        if (qty == 0)
                        {
                            sPesan = sPesan + " Qty Tidak Boleh 0 (Nol).";
                            break;
                        }
                    }
                }
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
        private void HitungTotal()
        {
            decimal Total = 0;
            for (int iBaris = 0; iBaris <= dgv.Rows.Count - 1; iBaris++)
            {
                if (dgv.Rows[iBaris].Cells["qty"].Value != null)
                {
                    Total = Total + decimal.Parse(dgv.Rows[iBaris].Cells["qty"].Value.ToString());
                }
                else
                {
                    Total = Total + 0;
                }
            }
            textBoxTotal.Text = Total.ToString("N0");
        }
        public void GetData(string kd)
        {
            AdnTReturBeliDao dao = new AdnTReturBeliDao(this.cnn);
            AdnTReturBeli o = dao.Get(kd);

            if (o != null)
            {
                this.ModeEdit = AdnModeEdit.BACA;

                textBoxKd.Text = kd;
                maskedTextBoxTgl.Text = AdnFungsi.CDateToStr(o.tgl);
                comboBoxPemasok.SelectedValue = o.kd_ps;
                textBoxKet.Text = o.ket;
                
                int iBaris = 0;
                dgv.Rows.Add(o.item_df.Count);
                foreach (AdnTReturBeliDtl item in o.item_df)
                {
                    dgv.Rows[iBaris].Cells["kd_barang"].Value = item.kd_barang;
                    dgv.Rows[iBaris].Cells["barcode"].Value = item.barang.barcode;
                    dgv.Rows[iBaris].Cells["nm_barang"].Value = item.barang.nm_barang;
                    dgv.Rows[iBaris].Cells["kd_satuan"].Value = item.kd_satuan;
                    dgv.Rows[iBaris].Cells["qty"].Value = item.qty;

                    iBaris++;
                }
                this.HitungTotal();

                toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonBatal.Enabled = true;
                toolStripButtonHapus.Enabled = true;
                toolStripButtonSimpan.Enabled = false;

                textBoxKd.Enabled = false;
                textBoxKd.Focus();

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
        
        private void maskedTextBoxTgl_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = AdnFungsi.CekFormatTanggal(sender, this.AppName);
        }

        private void dgv_CellValueValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows[e.RowIndex].Cells["barcode"].Value!= null)
            {
                if (dgv.Columns[e.ColumnIndex].Name == "barcode" && dgv.Rows[e.RowIndex].Cells["barcode"].Value.ToString() != "")
                {

                }

                if (dgv.Rows[e.RowIndex].Cells["barcode"].Value != null && dgv.Rows[e.RowIndex].Cells["qty"].Value != null)
                {

                    int? qty = AdnFungsi.CInt(dgv.Rows[e.RowIndex].Cells["qty"].Value.ToString());
                    this.HitungTotal();

                    //switch (dgv.Columns[e.ColumnIndex].Name)
                    //{
                    //    case "qty":
                    //    case "dana":                      
                    //        break;
                    //}
                }
            }//if value !=null
        }
        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgv.Columns[e.ColumnIndex].Name == "barcode")
            {
                //if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                //{
                //    dgv.Rows[e.RowIndex].ErrorText =
                //        "Barcode Harus Diisi";
                //    //e.Cancel = true;
                //    dgv.Rows[e.RowIndex].Cells["barcode"].Value = "";

                //}
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    MessageBox.Show("Barcode Harus Diisi!",this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //e.Cancel = true;
                }
                else
                {
                    string barcode = e.FormattedValue.ToString();
                    AdnBarangDao dao = new AdnBarangDao(this.cnn);
                    AdnBarang o = dao.Get(barcode);
                    if (o != null)
                    {
                        dgv.Rows[e.RowIndex].Cells["kd_barang"].Value = o.kd_barang;
                        dgv.Rows[e.RowIndex].Cells["nm_barang"].Value = o.nm_barang;
                        dgv.Rows[e.RowIndex].Cells["kd_satuan"].Value = o.kd_satuan;
                    }
                    else
                    {
                        MessageBox.Show("Barcode Tidak Dikenal!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        e.Cancel = true;
                    }
                }
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "qty")
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgv.Rows[e.RowIndex].Cells["qty"].Value = 0;
                }
            }
        }
        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dgv.EditingControl.KeyPress+=new KeyPressEventHandler(EditingControl_KeyPress);
        }
        private void dgv_Enter(object sender, EventArgs e)
        {
            if (dgv.Rows.Count == 0)
            {
                dgv.Rows.Add();
            }
        }
        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.TambahBaris();
        }
        void EditingControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (dgv.Columns[dgv.CurrentCell.ColumnIndex].Name)
            {
                case "qty":
                    e.Handled = AdnFungsi.CekAngka(e.KeyChar);
                    break;
            }

            int iBaris = dgv.CurrentCell.RowIndex;
            if (iBaris == dgv.Rows.Count - 1)
            {
                dgv.Rows.Add();
            }

        }
        void EditingControl_KeyDown(object sender ,KeyEventArgs e)
        {
            switch (dgv.Columns[dgv.CurrentCell.ColumnIndex].Name)
            {
                case "barcode":
                    if (e.KeyCode == Keys.Enter)
                    {
                        string barcode = dgv.CurrentRow.Cells["barcode"].Value.ToString();
                        AdnBarangDao dao = new AdnBarangDao(this.cnn);
                        AdnBarang o = dao.Get(barcode);
                        if (o != null)
                        {
                            dgv.CurrentRow.Cells["kd_barang"].Value = o.kd_barang;
                            dgv.CurrentRow.Cells["nm_barang"].Value = o.nm_barang;
                            //dgv.Rows[e.RowIndex].Cells["kd_satuan"].Value = o.kd_satuan;
                        }
                        else
                        {
                            MessageBox.Show("Barcode Tidak Dikenal!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        //if (dgv.Rows[e.RowIndex].Cells["qty"].Value == null)
                        //{
                        //    dgv.Rows[e.RowIndex].Cells["qty"].Value = "1";
                        //}
                    }
                    break;
            }
        }

        private void TambahBaris()
        {
            int BarisAkhir = dgv.Rows.Count - 1;
            if (dgv.Rows[BarisAkhir].Cells["nm_barang"].Value != null
                && dgv.Rows[BarisAkhir].Cells["nm_barang"].Value.ToString() != ""
                && AdnFungsi.CInt(dgv.Rows[BarisAkhir].Cells["qty"].Value,true) > 0)
            {
                dgv.Rows.Add();
            }

            if (dgv.Rows[BarisAkhir].Cells["nm_barang"].Value != null)
            {
                if (dgv.Rows[BarisAkhir].Cells["nm_barang"].Value.ToString() == "")
                {
                    dgv.CurrentCell = dgv.CurrentRow.Cells[1];

                }
            }
            else
            {
                dgv.CurrentCell = dgv.CurrentRow.Cells[1];
            }

        }

        private void FTReturBeli_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.fInduk != null)
            {
                this.fInduk.Reload();
            }
        }

        

        
    }
}
