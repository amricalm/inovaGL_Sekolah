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
    [AdnScObjectAtr("Form: Input Jurnal Umum", "Jurnal Umum")]
    public partial class FTJurnalUmum : Andhana.AdnBaseForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private string AppName;
        private FDTJurnalUmum fInduk;

        private int LastBaris = 0;
        private BindingSource bs=new BindingSource();
        public string KdJurnal = "";

        public FTJurnalUmum(SqlConnection cnn,string AppName,short ModeEdit,string kd, object fInduk,string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;
            this.fInduk = (FDTJurnalUmum)fInduk;

            new AdnThAjarDao(this.cnn).SetCombo(comboBoxThAjar);
            comboBoxThAjar.SelectedValue = ThAjar;
            
            AdnFungsi.SetComboBulan(comboBoxBulan, false);
            comboBoxBulan.SelectedIndex = DateTime.Now.Month - 1;
            updTahun.Value = DateTime.Now.Year;

            new AdnSysJenisJurnalDao(this.cnn).SetComboJurnalUmum(comboBoxJenisJurnal,false);
            comboBoxJenisJurnal.SelectedIndex = -1;

            bs.DataSource = new AdnAkunDao(this.cnn).GetDf(AdnVar.TipeAkun.DTL,false);
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

            ////--- DEPT ---
            DataGridViewComboBoxColumn cboDept = new DataGridViewComboBoxColumn();
            new AdnDeptDao(this.cnn).SetComboDgv(cboDept, true);

            dgv.Columns.Remove("kd_dept");
            dgv.Columns.Insert(3, cboDept);
            cboDept.HeaderText = "Dept";
            cboDept.Name = "kd_dept";
            cboDept.Width = 140;

            //=== END DEPT ===

            ////--- PROJECT ---
            //DataGridViewComboBoxColumn cboProject = new DataGridViewComboBoxColumn();

            //new AdnAkunDao(this.cnn).SetComboAkunPendapatanDgv(cboProject);

            //dgv.Columns.Remove("kd_project");
            //dgv.Columns.Insert(5, cboProject);
            //cboProject.HeaderText = "Sumber Dana";
            //cboProject.Name = "kd_project";
            //cboProject.Width = 110;

            //=== END PROJECT ===
            this.UseWaitCursor = false;
        }

        #region "Prosedur CRUD"
        private void Simpan()
        {
            dgv.EndEdit();
            if (this.IsValid())
            {

                AdnJurnalUmum o = new AdnJurnalUmum();

                o.KdJU = textBoxKd.Text;
                o.Tgl= AdnFungsi.CDate(maskedTextBoxTgl);
                o.Deskripsi = textBoxKet.Text.ToString().Trim();
                o.KdJurnal = o.KdJU;
                o.ThAjar = comboBoxThAjar.SelectedValue.ToString();
                o.ItemDf = new List<AdnJurnalUmumDtl>();

                AdnJurnal jurnal = new AdnJurnal();
                jurnal.KdJurnal = o.KdJurnal;
                jurnal.Tgl = o.Tgl;
                jurnal.Deskripsi = o.Deskripsi;
                jurnal.ThAjar = o.ThAjar;
                jurnal.ThAjarTagihan = jurnal.ThAjar;

                if (comboBoxJenisJurnal.SelectedIndex > -1)
                {
                    o.JenisJurnal = comboBoxJenisJurnal.SelectedValue.ToString();
                    jurnal.JenisJurnal = o.JenisJurnal;
                    
                }
                if (comboBoxAset.SelectedIndex > -1)
                {
                    o.Tag = comboBoxAset.SelectedValue.ToString();
                    jurnal.Tag = o.Tag;
                }

                if (o.JenisJurnal == AdnJurnalVar.JenisJurnal.JGAJ || o.JenisJurnal == AdnJurnalVar.JenisJurnal.JSUSUT)
                {
                    o.Periode = updTahun.Value + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
                    jurnal.Periode = o.Periode;
                }

                jurnal.ItemDf = new List<AdnJurnalDtl>();

                int cacah = 0;
                foreach (DataGridViewRow baris in dgv.Rows)
                {
                    if (!baris.IsNewRow)
                    {
                        string KdAkun = (baris.Cells["KdAkun"].Value ?? "").ToString();
                        decimal Debet = AdnFungsi.CDec(baris.Cells["debet"]);
                        decimal Kredit = AdnFungsi.CDec(baris.Cells["kredit"]);

                        if (KdAkun != "" && Debet - Kredit != 0)
                        {
                            AdnJurnalUmumDtl item = new AdnJurnalUmumDtl();
                            item.KdJU = o.KdJU;
                            item.KdAkun = KdAkun;
                            item.NoUrut = cacah; cacah++;
                            item.KdProject = "";// AdnFungsi.CStr(baris.Cells["kd_project"]);
                            item.KdDept = AdnFungsi.CStr(baris.Cells["kd_dept"]);
                            item.Memo = AdnFungsi.CStr(baris.Cells["memo"]);

                            item.Debet = Debet;
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
                    AdnJurnalUmumDao dao = new AdnJurnalUmumDao(this.cnn, AppVar.AppPengguna, Trans);
                    AdnJurnalDao jurnalDao = new AdnJurnalDao(this.cnn, AppVar.AppPengguna, Trans);

                    switch (this.ModeEdit)
                    {
                        case AdnModeEdit.BARU:
                            dao.Simpan(o);
                            jurnal.KdJurnal = o.KdJU;
                            jurnalDao.Simpan(jurnal);
                            textBoxKd.Text = o.KdJU;
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

                AdnJurnalUmumDao dao = new AdnJurnalUmumDao(this.cnn,AppVar.AppPengguna,Trans);
                AdnJurnalDao JurnalDao = new AdnJurnalDao(this.cnn, AppVar.AppPengguna, Trans);
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

                if (AppVar.BJUNoBuktiAuto == 1)
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
                textBoxTotalDebet.Text = "0";

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
                if (AppVar.BJUNoBuktiAuto == 0)
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

            if (comboBoxJenisJurnal.SelectedIndex > -1)
            {
                if (comboBoxJenisJurnal.SelectedValue.ToString() == AdnJurnalVar.JenisJurnal.JSUSUT)
                {
                    if (comboBoxAset.SelectedIndex == -1)
                    {
                        if (sPesan != "") { sPesan = sPesan + ", "; }
                        sPesan = sPesan + "Aset";
                    }

                    if (comboBoxBulan.SelectedIndex == -1)
                    {
                        if (sPesan != "") { sPesan = sPesan + ", "; }
                        sPesan = sPesan + "Bulan";
                    }
                }

                if (comboBoxJenisJurnal.SelectedValue.ToString() == AdnJurnalVar.JenisJurnal.JGAJ)
                {
                    if (comboBoxBulan.SelectedIndex == -1)
                    {
                        if (sPesan != "") { sPesan = sPesan + ", "; }
                        sPesan = sPesan + "Bulan";
                    }
                }

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

            if (AdnFungsi.CDec(textBoxTotalDebet) + AdnFungsi.CDec(textBoxTotalKredit) == 0)
            {
                sPesan = sPesan + "TOTAL Transaksi Tidak Boleh 0 (nol).\n";
            }

            if (AdnFungsi.CDec(textBoxTotalDebet) - AdnFungsi.CDec(textBoxTotalKredit) != 0)
            {
                sPesan = sPesan + "Transaksi Harus Seimbang.\n";
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
            decimal TotalDebet = 0;
            decimal TotalKredit = 0;
            for (int iBaris = 0; iBaris <= dgv.Rows.Count - 1; iBaris++)
            {
                TotalDebet = TotalDebet + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["debet"]);
                TotalKredit = TotalKredit + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["kredit"]);
            }
            textBoxTotalKredit.Text = TotalKredit.ToString("N0");
            textBoxTotalDebet.Text = TotalDebet.ToString("N0");
        }
        public void GetData(string kd)
        {
            AdnJurnalUmumDao dao = new AdnJurnalUmumDao(this.cnn);
            AdnJurnalUmum o = dao.Get(kd);

            if (o != null)
            {
                this.ModeEdit = AdnModeEdit.BACA;
                this.KdJurnal = o.KdJurnal;

                textBoxKd.Text = kd;
                maskedTextBoxTgl.Text = AdnFungsi.CDateToStr(o.Tgl);
                textBoxKet.Text = o.Deskripsi.Trim();

                comboBoxJenisJurnal.SelectedIndex = -1;
                comboBoxAset.SelectedIndex = -1;
                comboBoxThAjar.SelectedValue = o.ThAjar;
                
                if (o.JenisJurnal != "")
                {
                    comboBoxJenisJurnal.SelectedValue = o.JenisJurnal;
                    if (o.JenisJurnal == AdnJurnalVar.JenisJurnal.JSUSUT)
                    {
                        comboBoxAset.Visible = true;
                        comboBoxAset.SelectedValue = o.Tag;

                        lblPeriode.Visible = true;
                        comboBoxBulan.Visible = true;
                        updTahun.Visible = true;

                        comboBoxBulan.SelectedIndex = AdnFungsi.CInt(o.Periode.ToString().Substring(4, 2), true) - 1;
                        updTahun.Value = AdnFungsi.CInt(o.Periode.ToString().Substring(0, 4), true);
                    }

                    if (o.JenisJurnal == AdnJurnalVar.JenisJurnal.JGAJ)
                    {
                        lblPeriode.Visible = true;
                        comboBoxBulan.Visible = true;
                        updTahun.Visible = true;

                        comboBoxBulan.SelectedIndex = AdnFungsi.CInt(o.Periode.ToString().Substring(4, 2),true) -1;
                        updTahun.Value = AdnFungsi.CInt(o.Periode.ToString().Substring(0, 4), true);
                    }
                }

                int iBaris = 0;
                dgv.Rows.Add(o.ItemDf.Count);
                foreach (AdnJurnalUmumDtl item in o.ItemDf)
                {
                    //if (item.Debet > 0 && o.ItemDf.IndexOf(item) < o.ItemDf.Count-1)
                    //{
                        dgv.Rows[iBaris].Cells["KdAkun"].Value = item.KdAkun;
                        dgv.Rows[iBaris].Cells["NmAkun"].Value ="";
                        if (item.Akun!=null)
                        {
                            dgv.Rows[iBaris].Cells["NmAkun"].Value = item.Akun.NmAkun;
                        }
                        
                        dgv.Rows[iBaris].Cells["debet"].Value = item.Debet;
                        dgv.Rows[iBaris].Cells["kredit"].Value = item.Kredit;
                        //dgv.Rows[iBaris].Cells["kd_project"].Value = AdnFungsi.CStr(item.KdProject);
                        dgv.Rows[iBaris].Cells["kd_dept"].Value = AdnFungsi.CStr(item.KdDept);
                        dgv.Rows[iBaris].Cells["memo"].Value = item.Memo;

                        iBaris++;
                    //}
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
                    if (Kredit > 0)
                    {
                        dgv.Rows[e.RowIndex].Cells["debet"].Value = 0;
                    }
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
                && (AdnFungsi.CDec(dgv.Rows[BarisAkhir].Cells["debet"]) - AdnFungsi.CDec(dgv.Rows[BarisAkhir].Cells["kredit"]) != 0))
                
            {
                dgv.Rows.Add();
                this.HitungTotal();
            }
        }

        private void FTJurnalUmum_FormClosing(object sender, FormClosingEventArgs e)
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
            FDlgLapBJU ofm = new FDlgLapBJU(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, textBoxKd.Text.ToString().Trim());
            ofm.ShowDialog();
        }

        private void textBoxKet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
            }
        }

        private void dgv_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.HitungTotal();
        }


        private void textBoxKd_Validating(object sender, CancelEventArgs e)
        {
            string Kd = ((TextBox)sender).Text.ToString().Trim();
            if (Kd != "")
            {
                this.GetData(Kd);
            }
        }

        private void comboBoxJenisJurnal_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTag.Visible = false;
            comboBoxAset.Visible = false;

            lblPeriode.Visible = false;
            comboBoxBulan.Visible = false;
            updTahun.Visible = false;

            if (comboBoxJenisJurnal.SelectedIndex > -1)
            {
                string JnJurnal = comboBoxJenisJurnal.SelectedValue.ToString();
                if (JnJurnal == AdnJurnalVar.JenisJurnal.JSUSUT)
                {
                    lblTag.Visible = true;
                    comboBoxAset.Visible = true;

                    lblPeriode.Visible = true;
                    comboBoxBulan.Visible = true;
                    updTahun.Visible = true;

                    new AdnAsetDao(this.cnn).SetCombo(comboBoxAset);
                    comboBoxAset.SelectedIndex = -1;
                }
                if (JnJurnal == AdnJurnalVar.JenisJurnal.JGAJ)
                {
                    lblPeriode.Visible = true;
                    comboBoxBulan.Visible = true;
                    updTahun.Visible = true;
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }




   }
}
