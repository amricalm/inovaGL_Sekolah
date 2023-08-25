using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Andhana;

namespace EDUSIS.KeuanganPembayaran
{
    [AdnScObjectAtr("Form: Loket Pembayaran", "Pembayaran")]
    public partial class FTPembayaran : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        BindingSource bsTagihan = new BindingSource();
        private AdnScPengguna Pengguna;
        private string KdSekolah = "";
        private string ThAjar = "";
        private int ModeEdit;

        private string ReportPath;
        private string ReportExt;
        private string Organisasi;

        private bool UserProses = false;


        public FTPembayaran(SqlConnection cnn, string AppName, AdnScPengguna Pengguna, string KdSekolah,string ThAjar, string ReportPath, string ReportExt, string Organisasi)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.Pengguna=Pengguna;
            this.KdSekolah = KdSekolah.ToString().Trim();
            this.ThAjar = ThAjar.ToString().Trim();

            this.ReportPath = ReportPath;
            this.ReportExt = ReportExt;
            this.Organisasi = Organisasi;

            dateTimePickerTgl.Value = DateTime.Now;
            dateTimePickerTgl.Focus();

            new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
            comboBoxSekolah.SelectedValue = this.KdSekolah;

            new EDUSIS.Biaya.AdnBiayaDao(this.cnn).SetComboDgv(KdBiaya, true);
            new inovaGL.Data.AdnAkunDao(this.cnn).SetComboKas(comboBoxKas);

            //this.FillDataGridView();
        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();
            dgv.AutoGenerateColumns = false;

            string Kelas = "";
            string Nis ="";
            textBoxNmSiswa.Text = "";
            if (comboBoxKelas.SelectedIndex > -1)
            {
                Kelas = comboBoxKelas.SelectedValue.ToString().Trim();
                if (comboBoxNmSiswa.Items.Count>0)
                {
                    Nis = comboBoxNmSiswa.SelectedValue.ToString().Trim();
                    textBoxNmSiswa.Text = comboBoxNmSiswa.Text;
                }
            }

            //bs.DataSource = null;
            //int KdSiswa = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).GetKdSiswa(Nis,this.KdSekolah);
            //if (KdSiswa != 0)
            //{
            //    bs.DataSource = new AdnPembayaranDao(this.cnn).Get(KdSiswa, this.ThAjar, dateTimePickerTgl.Value);
            //}
            //dgv.DataSource = bs;

            this.HitungTotal();


            if (dgv.RowCount != 0)
            {
                toolStripButtonSimpan.Enabled = true;
            }

            this.UseWaitCursor = false;
        }

        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }
                
        public void Reload()
        {
            this.FillDataGridView();
        }

        private void toolStripButtonSimpan_Click(object sender, EventArgs e)
        {
            //this.Simpan();
            FTBayar ofm = new FTBayar(this.cnn, this.AppName, this.Pengguna, AdnFungsi.CDec(textBoxTotal),this);
            ofm.ShowDialog();
        }

        public void Simpan()
        {
            dgv.EndEdit();
            if (this.IsValid())
            {
                int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(this.KdSekolah).Tingkat;

                string Nis =  comboBoxNmSiswa.SelectedValue.ToString().Trim();
                int KdSiswa = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).GetKdSiswa(Nis, this.KdSekolah);
                
                //--- Header Pembayaran
                AdnPembayaran o = new AdnPembayaran();
                switch (this.ModeEdit)
                {
                    case AdnModeEdit.UBAH:
                        o.KdKwitansi = textBoxKwitansi.Text;
                        break;
                    default:
                        o.KdKwitansi = "";
                        break;

                }
                o.KdSiswa =KdSiswa;
                o.KdSekolah = this.KdSekolah;
                o.ThAjar = this.ThAjar;
                o.Tgl = dateTimePickerTgl.Value;
                o.Total = AdnFungsi.CDec(textBoxTotal);
                o.DfItem = new List<AdnPembayaranDtl>();
                //--- END --- Header Pembayaran
                //--- Header Jurnal
                inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                Jurnal.KdJurnal = o.KdKwitansi;
                Jurnal.Deskripsi = "Pembayaran Biaya Sekolah";
                Jurnal.Tgl = dateTimePickerTgl.Value;
                Jurnal.StatusPosting = false;
                Jurnal.Sumber = this.Name;
                Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();

                //--- END --- Header Jurnal
                int NoUrutDebet = 0;
                int NoUrutKredit = dgv.Rows.Count;
                decimal Total = 0;

                inovaGL.Data.AdnJurnalDtl JurnalDtl = null;
                foreach (DataGridViewRow baris in dgv.Rows)
                {
                    decimal Jmh = AdnFungsi.CDec(baris.Cells["Jmh"]);
                    if (Jmh > 0)
                    {
                        string StrPeriode = AdnFungsi.CStr(baris.Cells["ItemBulan"]);

                        // --- Rincian Biaya Pembayaran
                        AdnPembayaranDtl dtl = new AdnPembayaranDtl();
                        dtl.KdKwitansi = o.KdKwitansi;
                        dtl.KdDtl = 0;
                        dtl.KdBiaya = AdnFungsi.CStr(baris.Cells["KdBiaya"]);
                        dtl.Jmh = Jmh;
                        dtl.ItemBulan = StrPeriode;
                        dtl.Keterangan = "";

                        Total = Total + dtl.Jmh;
                        dtl.DfPeriode = new List<AdnPembayaranDtlPeriode>();

                        if (StrPeriode.Trim() != "")
                        {
                            string[] ArrPeriode = StrPeriode.Split(',');
                            foreach (string item in ArrPeriode)
                            {
                                // Periode/Bulan Tagihan
                                AdnPembayaranDtlPeriode oPeriode = new AdnPembayaranDtlPeriode();
                                oPeriode.Periode = item.ToString().Trim();

                                dtl.DfPeriode.Add(oPeriode);
                                // --- END --- Periode/Bulan Tagihan
                            }
                        }
                        o.DfItem.Add(dtl);
                        // --- END --- Rincian Biaya Tagihan

                        //--- Detail Jurnal Kredit
                        JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                        JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                        JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPiutang"]);
                        JurnalDtl.Debet = 0;
                        JurnalDtl.Kredit = dtl.Jmh;
                        JurnalDtl.KdDept = KdSekolah;
                        JurnalDtl.Memo = "Pembayaran a/n" + textBoxNmSiswa.Text.ToString(); ;
                        JurnalDtl.NoUrut = NoUrutKredit;

                        NoUrutKredit++;
                        //--- END --- Detail Jurnal Kredit

                        Jurnal.ItemDf.Add(JurnalDtl);

                    }
                }
                o.Total = Total;


                //--- Detail Jurnal Debet
                JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                JurnalDtl.KdAkun = comboBoxKas.SelectedValue.ToString();
                JurnalDtl.Debet = Total;
                JurnalDtl.Kredit = 0;
                JurnalDtl.KdDept = KdSekolah;
                JurnalDtl.Memo = "Pembayaran a/n" + textBoxNmSiswa.Text.ToString(); 
                JurnalDtl.NoUrut = NoUrutDebet;

                //--- END --- Detail Jurnal Debet

                Jurnal.ItemDf.Add(JurnalDtl);


                // Simpan Data
                SqlTransaction Trans=null;
                bool TransaksiBerhasil = false;
                try
                {
                    Trans = this.cnn.BeginTransaction();
                    AdnPembayaranDao dao = new AdnPembayaranDao(this.cnn, this.Pengguna, Trans);
                    inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
                    switch(this.ModeEdit)
                    {
                        case AdnModeEdit.BARU:
                            dao.Simpan(o);

                            Jurnal.KdJurnal = o.KdKwitansi;
                            daoJurnal.Simpan(Jurnal);
                            break;
                        case AdnModeEdit.UBAH:
                            o.KdKwitansi = textBoxKwitansi.Text;
                            Jurnal.KdJurnal = o.KdKwitansi;

                            dao.Update(o);
                            daoJurnal.Update(Jurnal);
                            break;
                        default:
                            //???
                            break;
                    }
                    Trans.Commit();
                    MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TransaksiBerhasil = true;
                    
                    // --- END --- Simpan Data
                    comboBoxNmSiswa.SelectedIndex = -1;
                }
                catch (Exception exp)
                {
                    Trans.Rollback();
                    AdnFungsi.LogErr(exp.Message);
                }

                if (checkBoxCetakKwitansi.Checked && TransaksiBerhasil)
                {
                    this.CetakKwitansi(o.KdKwitansi);
                }
            }
        }

        private bool IsValid()
        {
            string sPesan = "";

            if (dateTimePickerTgl.Text.Replace("/", "").ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Tanggal";
            }

            if (textBoxNmSiswa.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + " Nama Siswa";
            }

            if (comboBoxKas.SelectedIndex<0)
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + " Perkiraan/Rekening Kas";
            }
            
            if (sPesan != "")
            {
                sPesan = sPesan + " Harus Diisi.\n";
            }

            if (AdnFungsi.CDec(textBoxTotal) == 0)
            {
                sPesan = sPesan + "Transaksi = 0 (Nol).";
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
                Total = Total + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["Jmh"]);
            }
            textBoxTotal.Text = Total.ToString("N0");
        }

        private void comboBoxKelas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxKelas.SelectedIndex > -1)
            {
                new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).SetCombo(comboBoxNmSiswa, this.KdSekolah, this.ThAjar, comboBoxKelas.SelectedValue.ToString());
            }
            else
            {
                comboBoxNmSiswa.DataSource = null;

                this.DokumenBaru();
            }
        }

        private void comboBoxSekolah_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.KdSekolah = comboBoxSekolah.SelectedValue.ToString();
            new EDUSIS.Kelas.AdnKelasDao(this.cnn).SetCombo(comboBoxKelas, KdSekolah);
            comboBoxKelas.SelectedIndex = -1;

        }

        private void comboBoxNmSiswa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DokumenBaru();
            if (comboBoxNmSiswa.SelectedIndex > -1)
            {
                EDUSIS.Siswa.AdnSiswa swa = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(comboBoxNmSiswa.SelectedValue.ToString(), this.KdSekolah);
                if (swa != null)
                {
                    textBoxNis.Text = swa.NIS;
                    textBoxNmSiswa.Text = comboBoxNmSiswa.Text.ToString();
                    textBoxOrangTua.Text = swa.AyahNama;

                    textBoxTingkat.Text = new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).Get(this.KdSekolah, this.ThAjar, swa.NIS).oKelas.Tingkat.ToString();

                    dgvRiwayat.AutoGenerateColumns = false;
                    bs.DataSource = new AdnPembayaranDao(this.cnn).GetRingkasan(swa.KdSiswa);
                    dgvRiwayat.DataSource = bs;
                    DataGridViewColumn kolom = dgvRiwayat.Columns["KdKwitansi"];
                    dgvRiwayat.Sort(kolom,ListSortDirection.Descending);
                    
                }
            }
        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.HitungTotal();
        }

        private void dgv_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgv.Columns[e.ColumnIndex].Name == "KdBiaya" && AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["KdBiaya"]) != "")
            //{
            //    string KdBiaya = dgv.Rows[e.RowIndex].Cells["KdBiaya"].Value.ToString();
            //    dgv.Rows[e.RowIndex].ErrorText = "";
            //    dgv.Columns["Jmh"].ReadOnly = true;

            //    EDUSIS.Biaya.AdnBiayaSekolah obs = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).Get(this.KdSekolah, this.ThAjar,KdBiaya,textBoxTingkat.Text);
            //    if (obs != null)
            //    {
            //        dgv.Rows[e.RowIndex].Cells["Jmh"].Value = obs.Jmh;
            //        dgv.Rows[e.RowIndex].Cells["KdAkunPiutang"].Value = obs.oBiaya.KdAkunPiutang;
                    
            //        this.HitungTotal();

            //        if ((obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN
            //            || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP
            //            || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN_CICIL
            //            || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP_CICIL) )
            //        {
            //            EDUSIS.KeuanganPembayaran.FPilihPeriode ofm = new EDUSIS.KeuanganPembayaran.FPilihPeriode(this.cnn, this.AppName, this.Pengguna);
            //            ofm.ChildFormUpdate += new EDUSIS.KeuanganPembayaran.FPilihPeriode.ChildFormUpdateHandler(SetPilihPeriode);
            //            ofm.Owner = this;
            //            ofm.ShowDialog();

            //            if (obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN_CICIL
            //                || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP_CICIL)
            //            {
            //                dgv.Columns["Jmh"].ReadOnly = false;
            //            }

            //            if (AdnFungsi.CStr(dgv.CurrentRow.Cells["ItemBulan"]) == "")
            //            {
            //                MessageBox.Show("Periode/Bulan Belum Dipilih!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }
            //        }
            //    }
            //}

            //if (dgv.Rows[e.RowIndex].Cells["kd_program"].Value != null && dgv.Rows[e.RowIndex].Cells["qty"].Value != null)
            //{

            //    decimal dana = Convert.ToDecimal(dgv.Rows[e.RowIndex].Cells["dana"].Value);

            //    int? qty = AdnFungsi.CInt(dgv.Rows[e.RowIndex].Cells["qty"].Value.ToString());
            //    decimal jmh = Convert.ToDecimal(qty * dana);
            //    dgv.Rows[e.RowIndex].Cells["jmh"].Value = jmh;
                
            //}
        }

        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgv.Columns[e.ColumnIndex].Name == "KdBiaya")
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgv.Rows[e.RowIndex].ErrorText =
                        "Biaya Harus Diisi";
                    //e.Cancel = true;
                    dgv.Rows[e.RowIndex].Cells["KdBiaya"].Value = "";

                }
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "Jmh")
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgv.Rows[e.RowIndex].Cells["Jmh"].Value = 0;
                }
            }
        }

        private void SetPilihPeriode(object sender, EDUSIS.KeuanganPembayaran.ChildEventArgs e)
        {
            dgv.CurrentRow.Cells["ItemBulan"].Value = e.Periode;
            dgv.CurrentRow.Cells["Jmh"].Value = AdnFungsi.CDec(dgv.CurrentRow.Cells["Jmh"])*e.JmhPeriode;
        }

        private void DokumenBaru()
        {
            this.UserProses = false;
            textBoxTingkat.Text = "";
            textBoxNis.Text = "";
            textBoxNmSiswa.Text = "";
            textBoxOrangTua.Text = "";
            textBoxTotal.Text = "0";

            labelKwitansi.Visible = false;
            textBoxKwitansi.Visible = false;
            textBoxKwitansi.Text = "";

            this.ModeEdit = AdnModeEdit.BARU;

            dgv.Rows.Clear();
            bs.DataSource = null;
            dgvRiwayat.DataSource = bs;

        }

        private void toolStripButtonBatal_Click(object sender, EventArgs e)
        {
            comboBoxNmSiswa.SelectedIndex = -1;
        }

        private void buttonEditRiwayat_Click(object sender, EventArgs e)
        {
            buttonEditRiwayat.Enabled = false;
            dgv.Rows.Clear();
            textBoxTotal.Text = "0";

            textBoxKwitansi.Visible = true; labelKwitansi.Visible = true;
            textBoxKwitansi.Text = AdnFungsi.CStr(dgvRiwayat.CurrentRow.Cells["KdKwitansi"]);
            AdnPembayaran o = new AdnPembayaranDao(this.cnn).Get(textBoxKwitansi.Text);

            if (o != null)
            {
                this.UserProses = true;
                buttonHapusRiwayat.Enabled = true;
                this.ModeEdit = AdnModeEdit.UBAH;

                int i = 0;
                foreach (AdnPembayaranDtl item in o.DfItem)
                {
                    dgv.Rows.Add();
                    dgv.Rows[i].Cells["KdBiaya"].Value = item.KdBiaya;
                    dgv.Rows[i].Cells["KdAkunPiutang"].Value = new EDUSIS.Biaya.AdnBiayaDao(this.cnn, this.Pengguna).Get(item.KdBiaya).KdAkunPiutang.ToString();
                    dgv.Rows[i].Cells["ItemBulan"].Value = item.ItemBulan;
                    dgv.Rows[i].Cells["Jmh"].Value = item.Jmh;

                    i++;
                }
                this.HitungTotal();
                this.UserProses = false;
            }
            
            
        }

        private void CetakKwitansi(string KdKwitansi)
        {
            FDlgKwitansi ofm = new FDlgKwitansi(this.cnn, this.Pengguna, this.ReportPath, this.ReportExt, this.Organisasi, KdKwitansi);
            ofm.ShowDialog();
        }

        private void dgvRiwayat_SelectionChanged(object sender, EventArgs e)
        {
            buttonEditRiwayat.Enabled = false;
            buttonHapusRiwayat.Enabled = false;
            if (dgvRiwayat.Rows.Count > 0)
            {
                if (dgvRiwayat.Rows.IndexOf(dgvRiwayat.CurrentRow) == 0)
                {
                    //Validasi Yang Bisa Diedit
                    //Harus Tahun Ajar Aktif
                    //Belum Diposting
                    if (AdnFungsi.CStr(dgvRiwayat.CurrentRow.Cells["HdrThAjar"]) == this.ThAjar)
                    {
                        buttonEditRiwayat.Enabled = true;
                    }
                }
                bsTagihan.DataSource = new AdnPembayaranDao(this.cnn, this.Pengguna).GetLengkap(AdnFungsi.CStr(dgvRiwayat.CurrentRow.Cells["KdKwitansi"]));
                dgvRincian.AutoGenerateColumns = false;
                dgvRincian.DataSource = bsTagihan;
            }
            else
            {
                bsTagihan.DataSource = null;
                dgvRincian.DataSource = bsTagihan;
            }
        }

        private void buttonHapusRiwayat_Click(object sender, EventArgs e)
        {
            if (dgvRiwayat.Rows.Count > 0)
            {
                string Kd = AdnFungsi.CStr(dgvRiwayat.CurrentRow.Cells["KdKwitansi"]);
                if (MessageBox.Show("Hapus Kwitansi = " + Kd + " ?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    SqlTransaction Trans = null;
                    Trans = this.cnn.BeginTransaction();
                    try
                    {
                        new AdnPembayaranDao(this.cnn, this.Pengguna,Trans).Hapus(Kd);
                        new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna,Trans).Hapus(Kd);
                        Trans.Commit();
                        MessageBox.Show("Data Berhasil Dihapus!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        int pilih = comboBoxNmSiswa.SelectedIndex;
                        comboBoxNmSiswa.SelectedIndex = -1;
                        comboBoxNmSiswa.SelectedIndex = pilih;
                    }
                    catch(Exception exp)
                    {
                        Trans.Rollback();
                        AdnFungsi.LogErr(exp.Message);
                    }
                }
            }
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Columns[e.ColumnIndex].Name == "KdBiaya")
            {
                string KdBiaya = AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["KdBiaya"]);
                if (KdBiaya != "")
                {
                    
                    EDUSIS.Biaya.AdnBiayaSekolah obs = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).Get(this.KdSekolah, this.ThAjar, KdBiaya, textBoxTingkat.Text);
                    if (obs != null)
                    {
                        dgv.Rows[e.RowIndex].Cells["Jmh"].Value = obs.Jmh;
                        dgv.Rows[e.RowIndex].Cells["KdAkunPiutang"].Value = obs.oBiaya.KdAkunPiutang;

                        this.HitungTotal();

                        if ((obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN
                            || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP
                            || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN_CICIL
                            || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP_CICIL 
                            ) && !this.UserProses )
                        {
                            dgv.Columns["Jmh"].ReadOnly = true;
                            EDUSIS.KeuanganPembayaran.FPilihPeriode ofm = new EDUSIS.KeuanganPembayaran.FPilihPeriode(this.cnn, this.AppName, this.Pengguna);
                            ofm.ChildFormUpdate += new EDUSIS.KeuanganPembayaran.FPilihPeriode.ChildFormUpdateHandler(SetPilihPeriode);
                            ofm.Owner = this;
                            ofm.ShowDialog();

                            if (obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN_CICIL
                                || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP_CICIL)
                            {
                                dgv.Columns["Jmh"].ReadOnly = false;
                            }

                            if (AdnFungsi.CStr(dgv.CurrentRow.Cells["ItemBulan"]) == "")
                            {
                                MessageBox.Show("Periode/Bulan Belum Dipilih!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dgv.CancelEdit();
                            }
                        }
                        else
                        {
                            dgv.CurrentRow.Cells["ItemBulan"].Value = "";
                            dgv.Columns["Jmh"].ReadOnly = false;
                        }
                    }
                }//end if (KdBiaya != "")
            }//end if
        }

        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dgv.EditingControl.KeyPress += new KeyPressEventHandler(EditingControl_KeyPress);
        }

        void EditingControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (dgv.Columns[dgv.CurrentCell.ColumnIndex].Name)
            {
                case "Jmh":
                    e.Handled = AdnFungsi.CekAngka(e.KeyChar);
                    break;
            }
        }

        private void dgv_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            this.HitungTotal();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

    }
}
