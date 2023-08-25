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
    public partial class FTPembayaranVac : Form
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
        private long KdSiswa;
        private decimal JmhTransfer;


        public FTPembayaranVac(SqlConnection cnn, string AppName, AdnScPengguna Pengguna, string KdSekolah,string ThAjar, string ReportPath, string ReportExt, string Organisasi,long KdSiswa, decimal JmhTransfer)
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

            this.KdSiswa = KdSiswa;
            this.JmhTransfer = JmhTransfer;

            dateTimePickerTgl.Value = DateTime.Now;
            dateTimePickerTgl.Focus();

            new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
            comboBoxSekolah.SelectedValue = this.KdSekolah;

            //new EDUSIS.Biaya.AdnBiayaDao(this.cnn).SetComboDgv(KdBiaya, true);
            new inovaGL.Data.AdnAkunDao(this.cnn).SetComboKasDetail(comboBoxKas);

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
            if (AdnFungsi.CDec(textBoxJmhTransfer) == AdnFungsi.CDec(textBoxTotal))
            {
                this.Simpan();
                this.DokumenBaru();
            }
            else
            {
                MessageBox.Show("Jumlah Transfer dan Total Transaksi Tidak Sama!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //FTBayar ofm = new FTBayar(this.cnn, this.AppName, this.Pengguna, AdnFungsi.CDec(textBoxTotal),this);
            //ofm.ShowDialog();
        }

        public void Simpan()
        {
            dgv.EndEdit();
            if (this.IsValid())
            {
                int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(this.KdSekolah).Tingkat;

                string Nis = textBoxNis.Text;//comboBoxNmSiswa.SelectedValue.ToString().Trim();
                int KdSiswa = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).GetKdSiswa(Nis, this.KdSekolah);
                
                //--- Header Pembayaran
                AdnLoket o = new AdnLoket();
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
                o.Nis = Nis;
                o.KdSekolah = this.KdSekolah;
                o.ThAjar = this.ThAjar;
                o.NoBayar = 0;
                o.Tgl = dateTimePickerTgl.Value;
                o.Total = AdnFungsi.CDec(textBoxTotal);

                o.KasPerkiraan = comboBoxKas.SelectedValue.ToString();
                o.Flag = "INTERN";
                o.ThAjarTagihan = this.ThAjar;

                o.DfItem = new List<AdnLoketDtl>();
                //--- END --- Header Pembayaran
                //--- Header Jurnal
                //inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                //Jurnal.KdJurnal = o.KdKwitansi;
                //Jurnal.Deskripsi = "Pembayaran Biaya Sekolah";
                //Jurnal.Tgl = dateTimePickerTgl.Value;
                //Jurnal.StatusPosting = false;
                //Jurnal.Sumber = this.Name;
                //Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();

                //--- END --- Header Jurnal
                //int NoUrutDebet = 0;
                //int NoUrutKredit = dgv.Rows.Count;
                decimal Total = 0;

                //inovaGL.Data.AdnJurnalDtl JurnalDtl = null;
                foreach (DataGridViewRow baris in dgv.Rows)
                {
                    decimal Jmh = AdnFungsi.CDec(baris.Cells["Jmh"]);
                    if (Jmh > 0)
                    {
                        string StrPeriode = AdnFungsi.CStr(baris.Cells["ItemBulanAngka"]);

                        // --- Rincian Biaya Pembayaran
                        AdnLoketDtl dtl = new AdnLoketDtl();
                        dtl.ThAjar = o.ThAjar;
                        dtl.KdSekolah = o.KdSekolah;
                        dtl.Nis = o.Nis;
                        dtl.NoBayar = o.NoBayar;
                        dtl.KdBiaya = AdnFungsi.CStr(baris.Cells["KdBiaya"]);

                        dtl.Tgl = o.Tgl;

                        //dtl.KdDtl = 0;
                        dtl.JmhSatuan = AdnFungsi.CDec(baris.Cells["Rupiah"]);
                        dtl.Jmh = AdnFungsi.CDec(baris.Cells["Rupiah"]) * AdnFungsi.CInt(baris.Cells["Qty"], true); 
                        dtl.Diskon = AdnFungsi.CDec(baris.Cells["Diskon"]);
                        dtl.ItemBulan = AdnFungsi.CStr(baris.Cells["ItemBulan"]);
                        dtl.JmhBulan = AdnFungsi.CInt(baris.Cells["JmhBulan"], true);
                        dtl.Potongan = AdnFungsi.CDec(baris.Cells["Potongan"]);

                        dtl.Qty = AdnFungsi.CInt(baris.Cells["Qty"],true);
                        dtl.Ket = AdnFungsi.CStr(baris.Cells["Ket"]); 
                        dtl.KetKdBiaya = AdnFungsi.CStr(baris.Cells["KetKdBiaya"]); 

                        Total = Total + dtl.Jmh;
                        dtl.DfPeriode = new List<AdnLoketDtlPeriode>();

                        if (StrPeriode.Trim() != "")
                        {
                            string[] ArrPeriode = StrPeriode.Split(',');
                            foreach (string item in ArrPeriode)
                            {
                                // Periode/Bulan Tagihan
                                AdnLoketDtlPeriode oPeriode = new AdnLoketDtlPeriode();
                                oPeriode.ThAjar = dtl.ThAjar;
                                oPeriode.KdSekolah = dtl.KdSekolah;
                                oPeriode.Nis = dtl.Nis;
                                oPeriode.NoBayar = dtl.NoBayar;
                                oPeriode.KdBiaya = dtl.KdBiaya;
                                oPeriode.Bulan = AdnFungsi.CInt(item,true);
                                //oPeriode.Periode = item.ToString().Trim();

                                dtl.DfPeriode.Add(oPeriode);
                                // --- END --- Periode/Bulan Tagihan
                            }
                        }
                        o.DfItem.Add(dtl);
                        // --- END --- Rincian Biaya Tagihan

                        //--- Detail Jurnal Kredit
                        //JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                        //JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                        //JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPiutang"]);
                        //JurnalDtl.Debet = 0;
                        //JurnalDtl.Kredit = dtl.Jmh;
                        //JurnalDtl.KdDept = KdSekolah;
                        //JurnalDtl.Memo = "Pembayaran a/n" + textBoxNmSiswa.Text.ToString(); ;
                        //JurnalDtl.NoUrut = NoUrutKredit;

                        //NoUrutKredit++;
                        //--- END --- Detail Jurnal Kredit

                        //Jurnal.ItemDf.Add(JurnalDtl);

                    }
                }
                o.Total = Total;


                //--- Detail Jurnal Debet
                //JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                //JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                //JurnalDtl.KdAkun = comboBoxKas.SelectedValue.ToString();
                //JurnalDtl.Debet = Total;
                //JurnalDtl.Kredit = 0;
                //JurnalDtl.KdDept = KdSekolah;
                //JurnalDtl.Memo = "Pembayaran a/n" + textBoxNmSiswa.Text.ToString(); 
                //JurnalDtl.NoUrut = NoUrutDebet;

                //--- END --- Detail Jurnal Debet

                //Jurnal.ItemDf.Add(JurnalDtl);


                // Simpan Data
                SqlTransaction Trans=null;
                bool TransaksiBerhasil = false;
                try
                {
                    Trans = this.cnn.BeginTransaction();
                    AdnLoketDao dao = new AdnLoketDao(this.cnn, this.Pengguna, Trans);
                    //inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
                    switch(this.ModeEdit)
                    {
                        case AdnModeEdit.BARU:
                            dao.Simpan(o);

                            //Jurnal.KdJurnal = o.KdKwitansi;
                            //daoJurnal.Simpan(Jurnal);
                            break;
                        case AdnModeEdit.UBAH:
                            o.KdKwitansi = textBoxKwitansi.Text;
                            //Jurnal.KdJurnal = o.KdKwitansi;

                            //dao.Update(o);
                            //daoJurnal.Update(Jurnal);
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

                    //dgvRiwayat.AutoGenerateColumns = false;
                    //bs.DataSource = new AdnPembayaranDao(this.cnn).GetRingkasan(swa.KdSiswa);
                    //dgvRiwayat.DataSource = bs;
                    //DataGridViewColumn kolom = dgvRiwayat.Columns["KdKwitansi"];
                    //dgvRiwayat.Sort(kolom,ListSortDirection.Descending);
                    
                }
            }
        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.HitungTotal();
        }

        private void dgv_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Columns[e.ColumnIndex].Name == "KdBiaya" && AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["KdBiaya"]) != "")
            {
                //string KdBiaya = dgv.Rows[e.RowIndex].Cells["KdBiaya"].Value.ToString();
                //dgv.Rows[e.RowIndex].ErrorText = "";
                //dgv.Columns["Jmh"].ReadOnly = true;

                //EDUSIS.Biaya.AdnBiayaSekolah obs = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).Get(this.KdSekolah, this.ThAjar, KdBiaya, textBoxTingkat.Text);
                //if (obs != null)
                //{
                    //dgv.Rows[e.RowIndex].Cells["Jmh"].Value = obs.Jmh;
                    //dgv.Rows[e.RowIndex].Cells["KdAkunPiutang"].Value = obs.oBiaya.KdAkunPiutang;

                    //this.HitungTotal();

                    //if ((obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN
                    //    || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP
                    //    || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN_CICIL
                    //    || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP_CICIL))
                    //{
                    //    EDUSIS.KeuanganPembayaran.FPilihPeriode ofm = new EDUSIS.KeuanganPembayaran.FPilihPeriode(this.cnn, this.AppName, this.Pengguna);
                    //    ofm.ChildFormUpdate += new EDUSIS.KeuanganPembayaran.FPilihPeriode.ChildFormUpdateHandler(SetPilihPeriode);
                    //    ofm.Owner = this;
                    //    ofm.ShowDialog();

                    //    if (obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN_CICIL
                    //        || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP_CICIL)
                    //    {
                    //        dgv.Columns["Jmh"].ReadOnly = false;
                    //    }

                    //    if (AdnFungsi.CStr(dgv.CurrentRow.Cells["ItemBulan"]) == "")
                    //    {
                    //        MessageBox.Show("Periode/Bulan Belum Dipilih!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    }
                    //}
                //}
            }

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

            comboBoxItem.DataSource = null;
            comboBoxOpsiItem.DataSource = null;
            textBoxBulan.Text = "";
            textBoxKet.Text = "";
            textBoxQty.Text = "";
            textBoxDiskon.Text = "";
            textBoxJmh.Text = "";
            textBoxPotongan.Text = "0";
            textBoxBulanAngka.Text = "";
            textBoxJmhBulan.Text = "";
            textBoxJmhTransfer.Text = "0";

            this.ModeEdit = AdnModeEdit.BARU;

            dgv.Rows.Clear();
            //bs.DataSource = null;
            //dgvRiwayat.DataSource = bs;

        }

        private void toolStripButtonBatal_Click(object sender, EventArgs e)
        {
            comboBoxNmSiswa.SelectedIndex = -1;
        }


        private void CetakKwitansi(string KdKwitansi)
        {
            FDlgKwitansi ofm = new FDlgKwitansi(this.cnn, this.Pengguna, this.ReportPath, this.ReportExt, this.Organisasi, KdKwitansi);
            ofm.ShowDialog();
        }

        private void dgvRiwayat_SelectionChanged(object sender, EventArgs e)
        {
            //if (dgvRiwayat.Rows.Count > 0)
            //{
            //    if (dgvRiwayat.Rows.IndexOf(dgvRiwayat.CurrentRow) == 0)
            //    {
            //        //Validasi Yang Bisa Diedit
            //        //Harus Tahun Ajar Aktif
            //        //Belum Diposting
            //        if (AdnFungsi.CStr(dgvRiwayat.CurrentRow.Cells["HdrThAjar"]) == this.ThAjar)
            //        {

            //        }
            //    }
            //    bsTagihan.DataSource = new AdnPembayaranDao(this.cnn, this.Pengguna).GetLengkap(AdnFungsi.CStr(dgvRiwayat.CurrentRow.Cells["KdKwitansi"]));
            //}
            //else
            //{
            //    bsTagihan.DataSource = null;
            //}
        }

        private void buttonHapusRiwayat_Click(object sender, EventArgs e)
        {
            //if (dgvRiwayat.Rows.Count > 0)
            //{
            //    string Kd = AdnFungsi.CStr(dgvRiwayat.CurrentRow.Cells["KdKwitansi"]);
            //    if (MessageBox.Show("Hapus Kwitansi = " + Kd + " ?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //    {
            //        SqlTransaction Trans = null;
            //        Trans = this.cnn.BeginTransaction();
            //        try
            //        {
            //            new AdnPembayaranDao(this.cnn, this.Pengguna,Trans).Hapus(Kd);
            //            new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna,Trans).Hapus(Kd);
            //            Trans.Commit();
            //            MessageBox.Show("Data Berhasil Dihapus!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            int pilih = comboBoxNmSiswa.SelectedIndex;
            //            comboBoxNmSiswa.SelectedIndex = -1;
            //            comboBoxNmSiswa.SelectedIndex = pilih;
            //        }
            //        catch(Exception exp)
            //        {
            //            Trans.Rollback();
            //            AdnFungsi.LogErr(exp.Message);
            //        }
            //    }
            //}
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Columns[e.ColumnIndex].Name == "KdBiaya")
            {
                string KdBiaya = AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["KdBiaya"]);
                if (KdBiaya != "")
                {
                    
                    //EDUSIS.Biaya.AdnBiayaSekolah obs = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).Get(this.KdSekolah, this.ThAjar, KdBiaya, textBoxTingkat.Text);
                    //if (obs != null)
                    //{
                    //    dgv.Rows[e.RowIndex].Cells["Jmh"].Value = obs.Jmh;
                    //    //dgv.Rows[e.RowIndex].Cells["KdAkunPiutang"].Value = obs.oBiaya.KdAkunPiutang;

                    //    this.HitungTotal();

                    //    if ((obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN
                    //        || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP
                    //        || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN_CICIL
                    //        || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP_CICIL 
                    //        ) && !this.UserProses )
                    //    {
                    //        dgv.Columns["Jmh"].ReadOnly = true;
                    //        EDUSIS.KeuanganPembayaran.FPilihPeriode ofm = new EDUSIS.KeuanganPembayaran.FPilihPeriode(this.cnn, this.AppName, this.Pengguna);
                    //        ofm.ChildFormUpdate += new EDUSIS.KeuanganPembayaran.FPilihPeriode.ChildFormUpdateHandler(SetPilihPeriode);
                    //        ofm.Owner = this;
                    //        ofm.ShowDialog();

                    //        if (obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN_CICIL
                    //            || obs.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP_CICIL)
                    //        {
                    //            dgv.Columns["Jmh"].ReadOnly = false;
                    //        }

                    //        if (AdnFungsi.CStr(dgv.CurrentRow.Cells["ItemBulan"]) == "")
                    //        {
                    //            MessageBox.Show("Periode/Bulan Belum Dipilih!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //            dgv.CancelEdit();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        dgv.CurrentRow.Cells["ItemBulan"].Value = "";
                    //        dgv.Columns["Jmh"].ReadOnly = false;
                    //    }
                    //}
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

        private void SetSiswa(long kd)
        {
            EDUSIS.Siswa.AdnSiswa swa = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(kd);
            if (swa != null)
            {
                textBoxNis.Text = swa.NIS;
                textBoxNmSiswa.Text =swa.NmLengkap.ToString();
                textBoxOrangTua.Text = swa.AyahNama;

                EDUSIS.KelasSiswa.AdnKelasSiswa ks = new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).Get(swa.KdSekolah, this.ThAjar, swa.NIS);

                if (ks == null)
                {
                    MessageBox.Show("Siswa Belum Memiliki Kelas!", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    string Tk = ks.oKelas.Tingkat.ToString();
                    dgvRiwayat.AutoGenerateColumns = false;
                    dgvRiwayat.DataSource = new inovaGL.Data.AdnLoketDao(this.cnn).GetRingkasan(kd, this.ThAjar, swa.KdSekolah, Tk);

                    textBoxTingkat.Text = Tk;
                    this.KdSekolah = swa.KdSekolah;
                    //dgvRiwayat.Rows.Clear();
                    //DataTable lst = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetDfUtama(swa.KdSekolah, this.ThAjar, Tk);
                    //for (int i = 0; i < lst.Rows.Count; i++)
                    //{
                    //    dgvRiwayat.Rows.Add();
                    //    dgvRiwayat.Rows[i].Cells["RiwayatKdBiaya"].Value = AdnFungsi.CStr(lst.Rows[i]["KdBiaya"]);
                    //    dgvRiwayat.Rows[i].Cells["NmBiaya"].Value = AdnFungsi.CStr(lst.Rows[i]["NmBiaya"]);
                        //row.Cells["RiwayatKdBiaya"].Value = AdnFungsi.CStr(lst.Rows[i]["KdBiaya"]);
                        //row.Cells["NmBiaya"].Value = AdnFungsi.CStr(lst.Rows[i]["NmBiaya"]);
                        //dgvRiwayat.Rows.Add(row);
                    //}
                }
                //bs.DataSource = new AdnPembayaranDao(this.cnn).GetRingkasan(swa.KdSiswa);
                //dgvRiwayat.DataSource = bs;
                //DataGridViewColumn kolom = dgvRiwayat.Columns["KdKwitansi"];
                //dgvRiwayat.Sort(kolom, ListSortDirection.Descending);

            }
        }

        private void FTPembayaranVac_Load(object sender, EventArgs e)
        {
            this.SetSiswa(this.KdSiswa);
            comboBoxItem.DisplayMember = "NmBiaya";
            comboBoxItem.ValueMember = "KdBiaya";
            comboBoxItem.DataSource = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetDf(this.KdSekolah, this.ThAjar, this.textBoxTingkat.Text);
            textBoxJmhTransfer.Text = this.JmhTransfer.ToString("N0");
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBoxTingkat_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBoxHdr_Enter(object sender, EventArgs e)
        {

        }

        private void buttonTambah_Click(object sender, EventArgs e)
        {
            dgv.Rows.Add();
            dgv.Rows[dgv.Rows.Count - 1].Cells["KdBiaya"].Value = comboBoxItem.SelectedValue.ToString();
            dgv.Rows[dgv.Rows.Count - 1].Cells["Ket"].Value = textBoxKet.Text;

            if (comboBoxOpsiItem.SelectedValue != null)
            {
                dgv.Rows[dgv.Rows.Count - 1].Cells["KetKdBiaya"].Value = comboBoxOpsiItem.SelectedValue.ToString();
            }
            else
            {
                dgv.Rows[dgv.Rows.Count - 1].Cells["KetKdBiaya"].Value = "";
            }
            string sItem = comboBoxItem.Text;
            if (comboBoxOpsiItem.Text!="") sItem=sItem + " > " + comboBoxOpsiItem.Text;
            if (textBoxBulan.Text != "") sItem = sItem + " > " + textBoxBulan.Text;

            dgv.Rows[dgv.Rows.Count - 1].Cells["Item"].Value = sItem;
            dgv.Rows[dgv.Rows.Count - 1].Cells["ItemBulan"].Value = textBoxBulan.Text; dgv.Rows[dgv.Rows.Count - 1].Cells["ItemBulanAngka"].Value = textBoxBulanAngka.Text;
            dgv.Rows[dgv.Rows.Count - 1].Cells["JmhBulan"].Value = textBoxJmhBulan.Text;
            dgv.Rows[dgv.Rows.Count - 1].Cells["Qty"].Value = textBoxQty.Text.ToString();
            dgv.Rows[dgv.Rows.Count - 1].Cells["Rupiah"].Value = textBoxRupiah.Text.ToString();
            dgv.Rows[dgv.Rows.Count - 1].Cells["Diskon"].Value = textBoxDiskon.Text.ToString();
            dgv.Rows[dgv.Rows.Count - 1].Cells["Jmh"].Value = AdnFungsi.CDec(textBoxQty) * AdnFungsi.CDec(textBoxRupiah) - AdnFungsi.CDec(textBoxDiskon);
            dgv.Rows[dgv.Rows.Count - 1].Cells["Potongan"].Value = textBoxPotongan.Text.ToString();

            this.HitungTotal();

            comboBoxOpsiItem.SelectedIndex = -1;
            textBoxBulan.Text = ""; textBoxBulanAngka.Text = ""; textBoxJmhBulan.Text = "";
            textBoxKet.Text = "";
            textBoxQty.Text = "";
            textBoxRupiah.Text = ""; textBoxPotongan.Text = "0";
            textBoxDiskon.Text = "";
            textBoxJmh.Text = "";
            //dgv.Rows.Add(baris);
        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string KdBiaya = "";
            if (((ComboBox)sender).SelectedValue != null)
            {
                KdBiaya = ((ComboBox)sender).SelectedValue.ToString();
            }

            EDUSIS.Biaya.AdnBiayaSekolah oBs = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).Get(this.KdSekolah, this.ThAjar,KdBiaya, textBoxTingkat.Text);

            
            decimal Potongan = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetPotongan(this.KdSekolah, this.ThAjar,KdBiaya,textBoxNis.Text);
            if (oBs != null)
            {
                textBoxRupiah.Text = (oBs.Jmh - Potongan).ToString("N0");
            }
            
            textBoxPotongan.Text = Potongan.ToString("N0");
            comboBoxOpsiItem.DataSource = null;
            comboBoxOpsiItem.Enabled = true;
            textBoxJmhBulan.Text = "0";

            if (oBs != null)
            {
                switch (oBs.oBiaya.KdJenis)
                {
                    case EDUSIS.Shared.EdusisVar.JenisBiaya.CATERING:
                        new EDUSIS.Shared.AdnCateringDao(this.cnn).SetCombo(comboBoxOpsiItem);
                        break;

                    case EDUSIS.Shared.EdusisVar.JenisBiaya.ESKUL:
                        new EDUSIS.Shared.AdnEskulDao(this.cnn).SetCombo(comboBoxOpsiItem);
                        break;

                    case EDUSIS.Shared.EdusisVar.JenisBiaya.SANGGAR:
                        new EDUSIS.Shared.AdnSanggarDao(this.cnn).SetCombo(comboBoxOpsiItem);
                        break;

                    default:
                        comboBoxOpsiItem.Enabled = false;
                        break;
                }
            
            
                switch (oBs.oBiaya.KdJenis)
                {
                    case EDUSIS.Shared.EdusisVar.JenisBiaya.CATERING:
                    case EDUSIS.Shared.EdusisVar.JenisBiaya.ESKUL:
                    case EDUSIS.Shared.EdusisVar.JenisBiaya.SANGGAR:
                    case EDUSIS.Shared.EdusisVar.JenisBiaya.SPP:
                    case EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN:
                        EDUSIS.KeuanganPembayaran.FPilihPeriode ofm = new EDUSIS.KeuanganPembayaran.FPilihPeriode(this.cnn, this.AppName, this.Pengguna);
                        ofm.ChildFormUpdate += new EDUSIS.KeuanganPembayaran.FPilihPeriode.ChildFormUpdateHandler(SetPeriode);
                        ofm.Owner = this;
                        ofm.ShowDialog();
                        break;
                    default:
                        break;
                }
            }

        }

        private void SetPeriode(object sender, ChildEventArgs e)
        {
            textBoxBulan.Text = "";
            if (e.Periode.Trim() != "")
            {
                string[] arrBulan = e.Periode.Split();
                string sBulan = "";
                string sBulanAngka = "";
                for (int i = 0; i < arrBulan.Length; i++)
                {
                    if (sBulan != "")
                    {
                        sBulan = sBulan + ", ";
                    }

                    if (sBulanAngka != "")
                    {
                        sBulanAngka = sBulanAngka + ", ";
                    }

                    sBulan = sBulan + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[int.Parse(arrBulan[i].Substring(5, 2)) - 1];
                    sBulanAngka = sBulanAngka + int.Parse(arrBulan[i].Substring(5, 2)).ToString();
                }
                textBoxBulan.Text = sBulan;
                textBoxBulanAngka.Text = sBulanAngka;
                textBoxJmhBulan.Text = arrBulan.Length.ToString();
            }
            textBoxQty.Text = e.JmhPeriode.ToString();
            textBoxJmh.Text = (AdnFungsi.CDec(textBoxQty) * AdnFungsi.CDec(textBoxRupiah)).ToString("N0");
        }

        private void textBoxQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void textBoxRupiah_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void textBoxDiskon_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void textBoxQty_TextChanged(object sender, EventArgs e)
        {
            this.HitungJumlah();
        }

        private void HitungJumlah()
        {
            decimal Jmh = AdnFungsi.CDec(textBoxQty) * AdnFungsi.CDec(textBoxRupiah) - AdnFungsi.CDec(textBoxDiskon);
            textBoxJmh.Text = Jmh.ToString("N0");
        }

        private void textBoxRupiah_TextChanged(object sender, EventArgs e)
        {
            this.HitungJumlah();
        }

        private void textBoxDiskon_TextChanged(object sender, EventArgs e)
        {
            this.HitungJumlah();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void buttonHapus_Click(object sender, EventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                dgv.Rows.RemoveAt(dgv.CurrentRow.Index);
                this.HitungTotal();
            }
        }

    }
}
