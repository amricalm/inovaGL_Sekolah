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
using inovaGL.Laporan;

namespace inovaGL
{
    [AdnScObjectAtr("Form: Input Kas Masuk", "Kas Masuk")]
    public partial class FTKasMasuk : Andhana.AdnBaseForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private string AppName;
        private FDTKasMasuk fInduk;

        private int LastBaris = 0;
        private BindingSource bs=new BindingSource();
        private string KdJurnal="";
        private string ThAjar;
        private int WM_KEYDOWN = 0x100;

        public FTKasMasuk(SqlConnection cnn,string AppName,short ModeEdit,string kd, object fInduk, string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;
            this.fInduk = (FDTKasMasuk)fInduk;
            this.ThAjar = ThAjar;

            new AdnThAjarDao(this.cnn).SetCombo(comboBoxThAjar);
            bs.DataSource = new AdnAkunDao(this.cnn).GetDf(AdnVar.Klasifikasi.DETAIL,false);
            dgvAkun.DataSource = bs;

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
        private void FTReceipt_KeyDown(object sender, KeyEventArgs e)
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

            new AdnAkunDao(this.cnn).SetComboKasDetail(comboBoxKas);

            ////--- DEPT ---
            DataGridViewComboBoxColumn cboDept = new DataGridViewComboBoxColumn();
            new AdnDeptDao(this.cnn).SetComboDgv(cboDept, true);

            dgv.Columns.Remove("kd_dept");
            dgv.Columns.Insert(3, cboDept);
            cboDept.HeaderText = "Dept";
            cboDept.Name = "kd_dept";
            cboDept.Width = 150;

            //=== END DEPT ===

            this.UseWaitCursor = false;
        }

        #region "Prosedur CRUD"
        private void Simpan()
        {
            dgv.EndEdit();
            if (this.IsValid())
            {
                AdnKasMasuk o = new AdnKasMasuk();

                o.KdKM = textBoxKd.Text;
                o.Tgl= AdnFungsi.CDate(maskedTextBoxTgl);
                o.Dari = textBoxDr.Text.ToString().Trim();
                o.Deskripsi = textBoxKet.Text.ToString().Trim();
                o.ThAjar = comboBoxThAjar.SelectedValue.ToString();
                o.KdJurnal = o.KdKM;

                o.ItemDf = new List<AdnKasMasukDtl>();

                AdnJurnal jurnal = new AdnJurnal();
                jurnal.KdJurnal = o.KdJurnal;
                jurnal.Tgl = o.Tgl;
                jurnal.ThAjar = o.ThAjar;
                jurnal.ThAjarTagihan = jurnal.ThAjar;
                jurnal.Deskripsi = o.Deskripsi;
                jurnal.ItemDf = new List<AdnJurnalDtl>();


                int cacah = 0;
                //Baris Debet
                AdnKasMasukDtl itemDebet = new AdnKasMasukDtl();
                itemDebet.KdKM = o.KdJurnal;
                itemDebet.KdAkun = comboBoxKas.SelectedValue.ToString();
                itemDebet.NoUrut = cacah; cacah++;
                itemDebet.KdProject = "";
                itemDebet.KdDept="";
                itemDebet.Memo = "";

                itemDebet.Debet = AdnFungsi.CDec(textBoxTotalKredit);
                itemDebet.Kredit = 0;

                o.ItemDf.Add(itemDebet);

                AdnJurnalDtl jurnalItemDebet = new AdnJurnalDtl();

                jurnalItemDebet.KdJurnal = o.KdJurnal;

                jurnalItemDebet.KdAkun = itemDebet.KdAkun;
                jurnalItemDebet.KdProject = itemDebet.KdProject;
                jurnalItemDebet.KdDept = itemDebet.KdDept;
                jurnalItemDebet.Memo = "";

                jurnalItemDebet.NoUrut = itemDebet.NoUrut;
                jurnalItemDebet.Debet = itemDebet.Debet;
                jurnalItemDebet.Kredit = itemDebet.Kredit;

                jurnal.ItemDf.Add(jurnalItemDebet);

                foreach (DataGridViewRow baris in dgv.Rows)
                {
                    if (!baris.IsNewRow)
                    {
                        string KdAkun = (baris.Cells["KdAkun"].Value ?? "").ToString();
                        //decimal Debet = 0;// AdnFungsi.CDec(baris.Cells["debet"]);
                        decimal Kredit = AdnFungsi.CDec(baris.Cells["kredit"]);

                        if (KdAkun != "" && Kredit != 0)
                        {

                            AdnKasMasukDtl item = new AdnKasMasukDtl();
                            item.KdKM = o.KdKM;
                            item.KdAkun = KdAkun;
                            item.NoUrut = cacah; cacah++;
                            item.KdProject = "";
                            item.KdDept= AdnFungsi.CStr(baris.Cells["kd_dept"]);
                            item.Memo = AdnFungsi.CStr(baris.Cells["memo"]);

                            item.Debet = 0;
                            item.Kredit = Kredit;

                            o.ItemDf.Add(item);
                            //itemDao.Simpan(item);

                            AdnJurnalDtl jurnalItem = new AdnJurnalDtl();

                            jurnalItem.KdJurnal = o.KdJurnal;

                            jurnalItem.KdAkun = item.KdAkun;
                            jurnalItem.KdProject = item.KdProject;
                            jurnalItem.KdDept = item.KdDept;
                            jurnalItem.Memo = item.Memo;

                            jurnalItem.NoUrut = item.NoUrut;
                            jurnalItem.Debet = item.Debet;
                            jurnalItem.Kredit = item.Kredit;

                            jurnal.ItemDf.Add(jurnalItem);

                        }
                    }
                }

                SqlTransaction Trans = null;
                try
                {
                    Trans = this.cnn.BeginTransaction();

                    AdnKasMasukDao dao = new AdnKasMasukDao(this.cnn, AppVar.AppPengguna, Trans);
                    AdnJurnalDao jurnalDao = new AdnJurnalDao(this.cnn, AppVar.AppPengguna, Trans);

                    switch (this.ModeEdit)
                    {
                        case AdnModeEdit.BARU:
                            dao.Simpan(o);
                            jurnal.KdJurnal = o.KdKM;
                            jurnalDao.Simpan(jurnal);
                            textBoxKd.Text = o.KdKM;
                            break;

                        case AdnModeEdit.UBAH:
                            jurnal.KdJurnal = this.KdJurnal;
                            jurnalDao.Update(jurnal);
                            o.KdJurnal = jurnal.KdJurnal;
                            dao.Update(o);
                            break;
                    }

                    Trans.Commit();
                    this.KdJurnal = jurnal.KdJurnal;

                    panelHdr.Enabled = false;
                    panelDtl.Enabled = false;

                    toolStripButtonTambah.Enabled = true;
                    toolStripButtonEdit.Enabled = true;
                    toolStripButtonHapus.Enabled = true;

                    toolStripButtonBatal.Enabled = false;
                    toolStripButtonSimpan.Enabled = false;
                }
                catch (SqlException ex)
                {
                    Trans.Rollback();
                    AdnFungsi.LogErr(ex.Message.ToString());
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    AdnFungsi.LogErr(ex.Message.ToString());
                }
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
                SqlTransaction Trans = this.cnn.BeginTransaction();

                AdnKasMasukDao dao = new AdnKasMasukDao(this.cnn, AppVar.AppPengguna, Trans);
                AdnJurnalDao JurnalDao= new AdnJurnalDao(this.cnn, AppVar.AppPengguna, Trans);
                JurnalDao.Hapus(textBoxKd.Text.ToString());
                dao.Hapus(textBoxKd.Text.ToString());

                Trans.Commit();

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
                AdnFungsi.Bersih(this,true);
                this.ModeEdit = AdnModeEdit.BARU;
                this.KdJurnal = "";//digunakan saat Edit Data

                if (AppVar.BKMNoBuktiAuto == 1)
                {
                    textBoxKd.Text = "";//no bukti ditentukan saat proses penyimpanan;
                    textBoxKd.Enabled = false;
                }
                else
                {
                    textBoxKd.Enabled = true;
                }

                dgv.Rows.Clear();
                textBoxTotalKredit.Text = "0";

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
                textBoxKd.Focus();
            }
        }
        private bool IsValid()
        {
            string sPesan = "";

            if (textBoxKd.Text.ToString().Trim() == "")
            {
                if (AppVar.BKMNoBuktiAuto == 0)
                {
                    if (sPesan != "") { sPesan = sPesan + ", "; }
                    sPesan = sPesan + "No. Bukti";
                }
            }

            if (maskedTextBoxTgl.Text.Replace("/", "").ToString().Trim() == "")
            {

                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Tanggal";
            }

            if (comboBoxKas.SelectedIndex<0)
            {

                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Perkiraan Kas";
            }

            if (comboBoxThAjar.SelectedIndex < 0)
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Tahun Ajar";
            }

            if (sPesan != "")
            {
                sPesan = sPesan + " Harus Diisi.\n";
            }

            if (AdnFungsi.CDec(textBoxTotalKredit) == 0)
            {
                sPesan = sPesan + "TOTAL Transaksi Tidak Boleh 0 (nol).\n";
            }


            //foreach (DataGridViewRow baris in dgv.Rows)
            //{
            //    if (!baris.IsNewRow)
            //    {
            //        //validasi
            //        decimal Debet = AdnFungsi.CDec(baris.Cells["Debet"]);
            //        decimal Kredit = AdnFungsi.CDec(baris.Cells["Kredit"]);
            //        string KdAkun = AdnFungsi.CStr(baris.Cells["KdAkun"]);

            //        if (KdAkun == "")
            //        {
            //            //abaikan...
            //        }
            //        else
            //        {
            //            if ((Debet-Kredit) == 0)
            //            {
            //                //abaikan...
            //            }
            //        }
            //    }
            //}

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
            //decimal TotalDebet = 0;
            decimal TotalKredit = 0;
            for (int iBaris = 0; iBaris <= dgv.Rows.Count - 1; iBaris++)
            {
                //TotalDebet = TotalDebet + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["debet"]);
                TotalKredit = TotalKredit + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["kredit"]);
            }
            textBoxTotalKredit.Text = TotalKredit.ToString("N0");
            //textBoxTotalDebet.Text = TotalDebet.ToString("N0");
        }
        public void GetData(string kd)
        {
            AdnKasMasukDao dao = new AdnKasMasukDao(this.cnn);
            AdnKasMasuk o = dao.Get(kd);
            

            if (o != null)
            {
                this.ModeEdit = AdnModeEdit.BACA;
                this.KdJurnal = o.KdJurnal;

                textBoxKd.Text = kd;
                maskedTextBoxTgl.Text = AdnFungsi.CDateToStr(o.Tgl);
                textBoxDr.Text = o.Dari.Trim();
                textBoxKet.Text = o.Deskripsi.Trim();
                comboBoxKas.SelectedValue = o.ItemDf[0].KdAkun.Trim();
                comboBoxThAjar.SelectedValue = o.ThAjar;

                int iBaris = 0;
                dgv.Rows.Add(o.ItemDf.Count-1);
                foreach (AdnKasMasukDtl item in o.ItemDf)
                {
                    if (o.ItemDf.IndexOf(item) > 0)
                    {
                        dgv.Rows[iBaris].Cells["KdAkun"].Value = item.KdAkun;
                        dgv.Rows[iBaris].Cells["NmAkun"].Value = item.Akun.NmAkun;
                        //dgv.Rows[iBaris].Cells["debet"].Value = item.Debet;
                        dgv.Rows[iBaris].Cells["kredit"].Value = item.Kredit;
                        dgv.Rows[iBaris].Cells["kd_dept"].Value = item.KdDept;
                        dgv.Rows[iBaris].Cells["memo"].Value = item.Memo;
                        iBaris++;
                    }
                    
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

            switch (dgv.Columns[e.ColumnIndex].Name)
            {

                case "debet":
                    decimal Debet = AdnFungsi.CDec(dgv.Rows[e.RowIndex].Cells["debet"]);
                    dgv.Rows[e.RowIndex].Cells["debet"].Value = Debet;

                    if (Debet > 0)
                    {
                        dgv.Rows[e.RowIndex].Cells["kredit"].Value = 0;
                    }
                    break;


                case "kredit":
                    decimal Kredit = AdnFungsi.CDec(dgv.Rows[e.RowIndex].Cells["kredit"]);
                    dgv.Rows[e.RowIndex].Cells["kredit"].Value = Kredit;
                    //if (Kredit > 0)
                    //{
                    //    dgv.Rows[e.RowIndex].Cells["debet"].Value = 0;
                    //}
                    break;

            }
            this.HitungTotal();
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
                case "KdAkun":

                    this.LastBaris = dgv.CurrentCell.RowIndex;

                    panelAkun.Visible = true;
                    Point Posisi = dgv.Location;
                    Posisi.Y= Posisi.Y+ (dgv.CurrentCellAddress.Y+1)*dgv.CurrentCell.Size.Height+3;
                    Posisi.X = Posisi.X + dgv.RowHeadersWidth;
                    panelAkun.Location = Posisi;

                    string Isi = ((DataGridViewTextBoxEditingControl)sender).Text.ToString().Trim() +((char)e.KeyChar).ToString();
                    dgv.CancelEdit();
                    textBoxKdAkun.Focus();
                    textBoxKdAkun.Text = Isi;
                    textBoxKdAkun.SelectionStart =Isi.Length;

                    break;
                  
                case "debet": case "kredit": 
                    e.Handled = AdnFungsi.CekAngka(e.KeyChar);
                    break;

                    
            }

        }
       
        private void TambahBaris()
        {
            int BarisAkhir = dgv.Rows.Count - 1;
            if (AdnFungsi.CStr(dgv.Rows[BarisAkhir].Cells["NmAkun"]) != ""
                && (AdnFungsi.CDec(dgv.Rows[BarisAkhir].Cells["kredit"])!=0))
                
            {
                dgv.Rows.Add();
                this.HitungTotal();
            }
        }

        private void FTKasMasuk_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (this.fInduk != null)
            //{
            //    this.fInduk.Reload();
            //}
        }

        private void textBoxKdAkun_TextChanged(object sender, EventArgs e)
        {
            string sKd = textBoxKdAkun.Text.ToString().Trim();
            if (panelAkun.Visible)
            {
                bs.Filter = "KdAkun LIKE '" + sKd + "*'";
            }
        }

        private void textBoxKdAkun_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (textBoxKdAkun.Text.ToString().Trim() == "")
                    {
                        MessageBox.Show("Kode Akun Harus Diisi!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dgv.Rows[this.LastBaris].Cells[0].Value = "";
                        dgv.Rows[this.LastBaris].Cells[1].Value = "";
                    }
                    else
                    {
                        if (dgvAkun.Rows.Count > 0)
                        {
                            dgv.Rows[this.LastBaris].Cells[0].Value = dgvAkun.SelectedRows[0].Cells[0].Value.ToString().Trim();
                            dgv.Rows[this.LastBaris].Cells[1].Value = dgvAkun.SelectedRows[0].Cells[1].Value.ToString().Trim();
                            textBoxKdAkun.Text = "";
                            dgv.CurrentCell = this.dgv[2, this.LastBaris];
                        }
                        else
                        {
                            dgv.Rows[this.LastBaris].Cells[0].Value = "";
                            dgv.Rows[this.LastBaris].Cells[1].Value = "";
                            textBoxKdAkun.Text = "";

                            MessageBox.Show("Kode Akun Tidak Benar!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            dgv.CurrentCell = this.dgv[0, this.LastBaris];
                        }
                    }

                    dgv.Focus();
                    panelAkun.Visible = false;

                    break;
                case Keys.Down:
                    if (dgvAkun.Rows.Count > 0)
                    {
                        dgvAkun.Focus();
                        dgvAkun.CurrentCell = dgvAkun[0, 0];
                    }
                    break;
            }

        }

        private void dgvAkun_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    dgv.Focus();
                    panelAkun.Visible = false;
                    if (dgvAkun.Rows.Count > 0)
                    {
                        dgv.Rows[this.LastBaris].Cells[0].Value = dgvAkun.SelectedRows[0].Cells[0].Value.ToString().Trim();
                        dgv.Rows[this.LastBaris].Cells[1].Value = dgvAkun.SelectedRows[0].Cells[1].Value.ToString().Trim();
                        textBoxKdAkun.Text = "";
                        dgv.CurrentCell = this.dgv[2, this.LastBaris];
                    }
                    break;
            }
        }

        private void textBoxKdAkun_Leave(object sender, EventArgs e)
        {
            if (!dgvAkun.Focused)
            {
                panelAkun.Visible = false;
                textBoxKdAkun.Text="";
            }
        }

        private void dgvAkun_Leave(object sender, EventArgs e)
        {
            if (!textBoxKdAkun.Focused)
            {
                panelAkun.Visible = false;
                textBoxKdAkun.Text = "";
            }
        }

        private void dgvAkun_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAkun.Rows.Count > 0)
            {
                dgv.Rows[this.LastBaris].Cells[0].Value = dgvAkun.SelectedRows[0].Cells[0].Value.ToString().Trim();
                dgv.Rows[this.LastBaris].Cells[1].Value = dgvAkun.SelectedRows[0].Cells[1].Value.ToString().Trim();
                textBoxKdAkun.Text = "";
                dgv.CurrentCell = this.dgv[2, this.LastBaris];
            }
            else
            {
                dgv.Rows[this.LastBaris].Cells[0].Value = "";
                dgv.Rows[this.LastBaris].Cells[1].Value = "";
                textBoxKdAkun.Text = "";

                MessageBox.Show("Kode Akun Tidak Benar!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                dgv.CurrentCell = this.dgv[0, this.LastBaris];
            }
            dgv.Focus();
            panelAkun.Visible = false;
        }

        private void toolStripButtonCetak_Click(object sender, EventArgs e)
        {
            FDlgLapBKM ofm = new FDlgLapBKM(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, textBoxKd.Text.ToString().Trim());
            ofm.ShowDialog();
        }

        private void textBoxKd_Validating(object sender, CancelEventArgs e)
        {
            string Kd = ((TextBox)sender).Text.ToString().Trim();
            if (Kd != "")
            {
                this.GetData(Kd);
            }
        }

        // Detect F1 through F9 during preprocessing and modify F3.
        public override bool PreProcessMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                Keys keyCode = (Keys)m.WParam & Keys.KeyCode;

                // Detect F1 through F9.
                switch (keyCode)
                {
                    case Keys.F1:
                    case Keys.F2:
                    case Keys.F3:
                    case Keys.F4:
                    case Keys.F5:
                    case Keys.F6:
                    case Keys.F7:
                    case Keys.F8:
                    case Keys.F9:

                        MessageBox.Show("Control.PreProcessMessage: '" +
                          keyCode.ToString() + "' pressed.");

                        // Replace F3 with F1, so that ProcessKeyMessage will  
                        // receive F1 instead of F3.
                        if (keyCode == Keys.F3)
                        {
                            m.WParam = (IntPtr)Keys.F1;
                            MessageBox.Show("Control.PreProcessMessage: '" +
                                keyCode.ToString() + "' replaced by F1.");
                        }
                        break;
                }
            }

            // Send all other messages to the base method.
            return base.PreProcessMessage(ref m);
        }

        private void dgv_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.HitungTotal();
        }

    }
}
