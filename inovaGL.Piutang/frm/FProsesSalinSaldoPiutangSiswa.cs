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

namespace inovaGL.Piutang
{
    [AdnScObjectAtr("Form: Proses Salin Saldo Piutang Siswa", "Saldo Piutang")]
    public partial class FProsesSalinSaldoPiutangSiswa : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        BindingSource bsSiswa = new BindingSource();
        private AdnScPengguna Pengguna;
        private string KdSekolah = "";
        private string ThAjar = "";
        private List<EDUSIS.Biaya.AdnBiayaSekolah> LstBiayaSekolah;
        private int KdSiswa;
        private string KdAkunPenyeimbangSaw = "";
        public FProsesSalinSaldoPiutangSiswa(SqlConnection cnn, string AppName, AdnScPengguna Pengguna, string KdSekolah, string ThAjar, List<EDUSIS.Biaya.AdnBiayaSekolah> LstBiayaSekolah, int KdSiswa, string NmSiswa, string KdAkunPenyeimbangSaw, DateTime PeriodeMulai)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.Pengguna = Pengguna;
            this.KdSekolah = KdSekolah.ToString().Trim();
            this.ThAjar = ThAjar.ToString().Trim();
            this.LstBiayaSekolah = LstBiayaSekolah;
            this.KdSiswa = KdSiswa;
            this.KdAkunPenyeimbangSaw = KdAkunPenyeimbangSaw;

            dateTimePickerTgl.Value = PeriodeMulai;
            dateTimePickerTgl.Focus();

            textBoxSekolah.Text = (new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(KdSekolah) == null) ? "" : new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(KdSekolah).NmSekolah;
            textBoxNmSiswa.Text = NmSiswa.Trim();
            new EDUSIS.Kelas.AdnKelasDao(this.cnn).SetCombo(comboBoxKelas, KdSekolah);
            comboBoxKelas.SelectedIndex = -1;
        }

        private void FillDataGridView(string KdSaw)
        {
            this.UseWaitCursor = true;
            Application.DoEvents();
            //dgv.AutoGenerateColumns = false;

            //bs.DataSource = null;
            //bs.DataSource = new AdnSaldoAwalDao(this.cnn).GetRincian(KdSaw);
            //dgv.DataSource = bs;

            //this.HitungTotal();

            //if (dgv.RowCount != 0)
            //{
            //    toolStripButtonSimpan.Enabled = true;
            //}

            this.UseWaitCursor = false;
        }

        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Reload()
        {
            this.FillDataGridView("");
        }

        private void toolStripButtonSimpan_Click(object sender, EventArgs e)
        {
            this.Simpan();
        }

        private void Simpan()
        {
            //dgv.EndEdit();
            //bool HapusDataLama = false;
            //bool InputData = true;
            //if (this.IsValid())
            //{

            //    int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(this.KdSekolah).Tingkat;
            //    int KdSiswa = AdnFungsi.CInt(dgvSiswa.Rows[dgvSiswa.CurrentCell.RowIndex].Cells["KdSiswa"],true);
            //    if (new inovaGL.Piutang.AdnSaldoAwalDao(this.cnn).Get(KdSiswa, ThAjar))
            //    {
            //        if (MessageBox.Show("Saldo Awal untuk Tahun Ajar: " + this.ThAjar + " Sudah Ada!\nSaldo Awal Mau Diinput Ulang?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //        {
            //            //HapusDataLama = true;
            //        }
            //        else
            //        {
            //            InputData = false;
            //        }
            //    }//Cek IsAda SAW

            //    if (InputData)
            //    {
            //        //--- Header SAW
            //        AdnSaldoAwal o = new AdnSaldoAwal();
            //        o.KdSaldoAwal = AdnFungsi.CStr(dgvSiswa.Rows[dgvSiswa.CurrentCell.RowIndex].Cells["KdSaw"]); ;
            //        o.KdSiswa = KdSiswa;
            //        o.KdSekolah = this.KdSekolah;
            //        o.ThAjar = this.ThAjar;
            //        o.Tgl = dateTimePickerTgl.Value;
            //        o.Periode = "";
            //        o.Total = AdnFungsi.CDec(textBoxTotal);

            //        o.DfItem = new List<AdnSaldoAwalDtl>();
            //        //--- END --- Header SAW


            //        //--- Header Jurnal
            //        inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
            //        Jurnal.KdJurnal = "";
            //        Jurnal.Deskripsi = "Jurnal Piutang Siswa [Saldo Awal]";
            //        Jurnal.Tgl = dateTimePickerTgl.Value;
            //        Jurnal.StatusPosting = false;
            //        Jurnal.Sumber = this.Name;
            //        Jurnal.JenisJurnal = inovaGL.Data.AdnJurnalVar.JenisJurnal.JSAW_PIUTANG;
            //        Jurnal.ItemDf = new List<Data.AdnJurnalDtl>();

            //        Jurnal.KdSiswa = KdSiswa;
            //        Jurnal.KdSekolah = o.KdSekolah;
            //        Jurnal.Nis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(KdSiswa).NIS;
            //        Jurnal.ThAjar = o.ThAjar;
            //        Jurnal.ThAjarTagihan = o.ThAjar;

            //        //--- END --- Header Jurnal
            //        int NoUrutDebet = 0;
            //        int NoUrutKredit = dgv.Rows.Count;
            //        decimal Total = 0;
            //        foreach (DataGridViewRow baris in dgv.Rows)
            //        {
            //            if (AdnFungsi.CDec(baris.Cells["Jmh"]) > 0)
            //            {
            //                // --- Rincian Biaya SAW
            //                AdnSaldoAwalDtl dtl = new AdnSaldoAwalDtl();
            //                dtl.KdSaldoAwal = o.KdSaldoAwal;
            //                dtl.KdDtl = 0;
            //                dtl.KdBiaya = AdnFungsi.CStr(baris.Cells["KdBiaya"]);
            //                dtl.Jmh = AdnFungsi.CDec(baris.Cells["Jmh"]);
            //                dtl.ItemBulan = "";
            //                dtl.Keterangan = AdnFungsi.CStr(baris.Cells["Keterangan"]);

            //                dtl.DfPeriode = new List<AdnSaldoAwalDtlPeriode>();
            //                Total = Total + dtl.Jmh;

            //                string StrPeriode = AdnFungsi.CStr(baris.Cells["ItemBulan"]);
            //                if (StrPeriode.Trim() != "")
            //                {
            //                    string[] ArrPeriode = StrPeriode.Split(',');
            //                    foreach (string item in ArrPeriode)
            //                    {
            //                        // Periode/Bulan Tagihan
            //                        AdnSaldoAwalDtlPeriode oPeriode = new AdnSaldoAwalDtlPeriode();
            //                        oPeriode.Periode = item;

            //                        dtl.DfPeriode.Add(oPeriode);
            //                        // --- END --- Periode/Bulan Tagihan
            //                    }
            //                }
            //                o.DfItem.Add(dtl);
            //                // --- END --- Rincian Biaya SAW

            //                //--- Detail Jurnal Debet
            //                inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
            //                JurnalDtl.KdJurnal = Jurnal.KdJurnal;
            //                JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPiutang"]);
            //                JurnalDtl.Debet = dtl.Jmh;
            //                JurnalDtl.Kredit = 0;
            //                JurnalDtl.KdDept = KdSekolah;
            //                JurnalDtl.Memo = "Saldo Awal Piutang";
            //                JurnalDtl.NoUrut = NoUrutDebet;

            //                NoUrutDebet++;
            //                //--- END --- Detail Jurnal Debet

            //                Jurnal.ItemDf.Add(JurnalDtl);

            //                //--- Detail Jurnal Kredit
            //                JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
            //                JurnalDtl.KdJurnal = Jurnal.KdJurnal;
            //                JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPendapatan"]);
            //                JurnalDtl.Debet = 0;
            //                JurnalDtl.Kredit = dtl.Jmh;
            //                JurnalDtl.KdDept = KdSekolah;
            //                JurnalDtl.Memo = "Saldo Awal Piutang";
            //                JurnalDtl.NoUrut = NoUrutKredit;

            //                NoUrutKredit++;
            //                //--- END --- Detail Jurnal Kredit

            //                Jurnal.ItemDf.Add(JurnalDtl);
            //            }
            //        }
            //        o.Total = Total;
            //        // Simpan Data
            //        SqlTransaction Trans = null;
            //        try
            //        {

            //            Trans = this.cnn.BeginTransaction();

            //            //if (HapusDataLama)
            //            //{
            //            //    new AdnSaldoAwalDao(this.cnn, this.Pengguna, Trans).Hapus(KdSiswa, this.ThAjar, dateTimePickerTgl.Value);
            //            //}

            //            AdnSaldoAwalDao dao = new AdnSaldoAwalDao(this.cnn, this.Pengguna, Trans);
            //            inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);

            //            if (o.KdSaldoAwal != "") //??????
            //            {
            //                dao.Hapus(o.KdSaldoAwal);
            //                daoJurnal.Hapus(o.KdSaldoAwal);
            //            }
            //            dao.Simpan(o);
            //            Jurnal.KdJurnal = o.KdSaldoAwal;
            //            daoJurnal.Simpan(Jurnal);

            //            Trans.Commit();
            //            dgvSiswa.CurrentRow.Cells["KdSaw"].Value = o.KdSaldoAwal;
            //            dgvSiswa.CurrentRow.Cells["Total"].Value = Total;
            //            MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //        catch (Exception exp)
            //        {
            //            Trans.Rollback();
            //            AdnFungsi.LogErr(exp.Message);
            //        }
            //        // --- END --- Simpan Data
            //    }//END --- InputData


            //}//END IsValid
        }

        private bool IsValid()
        {
            string sPesan = "";

            if (dateTimePickerTgl.Text.Replace("/", "").ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Tanggal";
            }

            if (this.LstBiayaSekolah.Count == 0)
            {
                sPesan = sPesan + "Rincian Tunggakan";
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

        private void HitungTotal()
        {
            //decimal TotalDebet = 0;
            //for (int iBaris = 0; iBaris <= dgv.Rows.Count - 1; iBaris++)
            //{
            //    TotalDebet = TotalDebet + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["Jmh"]);
            //}
            //textBoxTotal.Text = TotalDebet.ToString("N0");
        }

        private void comboBoxKelas_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxKelas.Enabled = false;
            this.UseWaitCursor = true;
            bsSiswa.DataSource = null;
            dgvSiswa.DataSource = bsSiswa;
            if (comboBoxKelas.SelectedIndex > -1)
            {

                //Application.DoEvents();
                dgvSiswa.AutoGenerateColumns = false;

                string Kelas = comboBoxKelas.SelectedValue.ToString().Trim();
                bsSiswa.DataSource = new AdnSaldoAwalDao(this.cnn).GetRingkasan(Kelas, this.ThAjar, this.KdSekolah, dateTimePickerTgl.Value);
                dgvSiswa.DataSource = bsSiswa;

            }
            this.UseWaitCursor = false;
            comboBoxKelas.Enabled = true;
        }

        private void dgvSiswa_SelectionChanged(object sender, EventArgs e)
        {
            //bs.DataSource = null;
            //dgv.DataSource = bs;

            //textBoxNmSiswa.Text = "";
            //buttonTampil.Enabled = false;
            //if (dgvSiswa.SelectedRows.Count > 0)
            //{
            //    DataGridViewRow row = dgvSiswa.SelectedRows[0];
            //    textBoxNmSiswa.Text = row.Cells["NmLengkap"].Value.ToString();

            //    buttonTampil.Enabled = true;
            //}
        }

        private void dateTimePickerTgl_ValueChanged(object sender, EventArgs e)
        {
            bs.DataSource = null;
            //dgv.DataSource = bs;

            bsSiswa.DataSource = null;
            dgvSiswa.DataSource = bsSiswa;

        }

        private void buttonTampil_Click(object sender, EventArgs e)
        {
            dgvSiswa.Enabled = false;
            //int KdSiswa = 0;
            string KdSaw = "";
            if (dgvSiswa.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvSiswa.SelectedRows[0];
                KdSaw = AdnFungsi.CStr(row.Cells["KdSaw"]);
            }

            this.FillDataGridView(KdSaw);
            dgvSiswa.Enabled = true;

        }

        private bool SimpanJurnal()
        {
            bool Hasil = false;


            return Hasil;
        }

        private void buttonProses_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                bool TimpaDataSaldoTerakhir = false;
                bool Batal = false;

                foreach (DataGridViewRow Baris in dgvSiswa.Rows)
                {
                    if (AdnFungsi.CBool(Baris.Cells["PilihanKdSiswa"], true))
                    {
                        if (AdnFungsi.CDec(Baris.Cells["Total"]) > 0)
                        {
                            if (MessageBox.Show("Saldo Awal untuk Tahun Ajar: " + this.ThAjar + " Sudah Ada!\nSaldo Awal Mau Diinput Ulang?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                TimpaDataSaldoTerakhir = true;
                            }
                            else
                            {
                                Batal = true;
                            }
                            break;
                        }
                    }
                }



                int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(this.KdSekolah).Tingkat;
                
                //if (new inovaGL.Piutang.AdnSaldoAwalDao(this.cnn).Get(KdSiswa, ThAjar))
                //{
                //    if (MessageBox.Show("Saldo Awal untuk Tahun Ajar: " + this.ThAjar + " Sudah Ada!\nSaldo Awal Mau Diinput Ulang?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                //    {
                //        //HapusDataLama = true;
                //    }
                //    else
                //    {
                //        InputData = false;
                //    }
                //}//Cek IsAda SAW

                if (!Batal)
                {
                    int KdSiswa;
                    bool TerjadiKesalahan = false;
                    foreach (DataGridViewRow Baris in dgvSiswa.Rows)
                    {
                        if (AdnFungsi.CBool(Baris.Cells["PilihanKdSiswa"], true))
                        {
                            KdSiswa = AdnFungsi.CInt(Baris.Cells["KolomKdSiswa"], true);
                            //--- Header SAW
                            decimal Total = 0;
                            AdnSaldoAwal o = new AdnSaldoAwal();
                            o.KdSaldoAwal = AdnFungsi.CStr(Baris.Cells["KdSaw"]); // Jika Belum Anda--> isi = ""
                            o.KdSiswa = KdSiswa;
                            o.KdSekolah = this.KdSekolah;
                            o.ThAjar = this.ThAjar;
                            o.Tgl = dateTimePickerTgl.Value;
                            o.Periode = "";
                            o.Total = 0; // Di Akhir Rincian/Detail Biaya, di tentukan ulang.

                            o.DfItem = new List<AdnSaldoAwalDtl>();
                            //--- END --- Header SAW

                            //--- Header Jurnal
                            inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                            Jurnal.KdJurnal = "";
                            Jurnal.Deskripsi = "Jurnal Piutang Siswa [Saldo Awal Piutang]";
                            Jurnal.Tgl = dateTimePickerTgl.Value;
                            Jurnal.StatusPosting = false;
                            Jurnal.Sumber = this.Name;
                            Jurnal.JenisJurnal = inovaGL.Data.AdnJurnalVar.JenisJurnal.JSAW_PIUTANG;
                            Jurnal.ItemDf = new List<Data.AdnJurnalDtl>();

                            Jurnal.KdSiswa = o.KdSiswa;
                            Jurnal.KdSekolah = o.KdSekolah;
                            Jurnal.Nis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(KdSiswa).NIS;
                            Jurnal.ThAjar = o.ThAjar;
                            Jurnal.ThAjarTagihan = o.ThAjar;

                            //--- END --- Header Jurnal
                            int NoUrutDebet = 0;
                            int NoUrutKredit = LstBiayaSekolah.Count;
                            foreach (EDUSIS.Biaya.AdnBiayaSekolah Bs in LstBiayaSekolah)
                            {
                                if (Bs.Jmh > 0)
                                {
                                    // --- Rincian Biaya SAW
                                    AdnSaldoAwalDtl dtl = new AdnSaldoAwalDtl();
                                    dtl.KdSaldoAwal = o.KdSaldoAwal;
                                    dtl.KdDtl = 0;
                                    dtl.KdBiaya = Bs.KdBiaya;
                                    dtl.Jmh = Bs.Jmh;
                                    dtl.ItemBulan = "";
                                    dtl.Keterangan = "";

                                    dtl.DfPeriode = new List<AdnSaldoAwalDtlPeriode>();
                                    Total = Total + dtl.Jmh;

                                    //string StrPeriode = AdnFungsi.CStr(baris.Cells["ItemBulan"]);
                                    //if (StrPeriode.Trim() != "")
                                    //{
                                    //    string[] ArrPeriode = StrPeriode.Split(',');
                                    //    foreach (string item in ArrPeriode)
                                    //    {
                                    //        // Periode/Bulan Tagihan
                                    //        AdnSaldoAwalDtlPeriode oPeriode = new AdnSaldoAwalDtlPeriode();
                                    //        oPeriode.Periode = item;

                                    //        dtl.DfPeriode.Add(oPeriode);
                                    //        // --- END --- Periode/Bulan Tagihan
                                    //    }
                                    //}
                                    o.DfItem.Add(dtl);
                                    // --- END --- Rincian Biaya SAW

                                    //--- Detail Jurnal Debet
                                    inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                                    JurnalDtl.KdAkun = Bs.oBiaya.KdAkunPiutang;
                                    JurnalDtl.Debet = dtl.Jmh;
                                    JurnalDtl.Kredit = 0;
                                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                                    JurnalDtl.Memo = "Saldo Awal Piutang";
                                    JurnalDtl.NoUrut = NoUrutDebet;

                                    NoUrutDebet++;
                                    //--- END --- Detail Jurnal Debet

                                    Jurnal.ItemDf.Add(JurnalDtl);

                                    //--- Detail Jurnal Kredit
                                    JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                                    JurnalDtl.KdAkun = this.KdAkunPenyeimbangSaw;
                                    JurnalDtl.Debet = 0;
                                    JurnalDtl.Kredit = dtl.Jmh;
                                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                                    JurnalDtl.Memo = "Saldo Awal Piutang";
                                    JurnalDtl.NoUrut = NoUrutKredit;

                                    NoUrutKredit++;
                                    //--- END --- Detail Jurnal Kredit

                                    Jurnal.ItemDf.Add(JurnalDtl);
                                }
                            } // --- END --- foreach LstBiayaSekolah
                            o.Total = Total;
                            // Simpan Data
                            SqlTransaction Trans = null;
                            try
                            {

                                Trans = this.cnn.BeginTransaction();

                                AdnSaldoAwalDao dao = new AdnSaldoAwalDao(this.cnn, this.Pengguna, Trans);
                                inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);

                                if (o.KdSaldoAwal != "") 
                                {
                                    dao.Hapus(o.KdSaldoAwal);
                                    daoJurnal.Hapus(o.KdSaldoAwal);
                                }
                                dao.Simpan(o);
                                Jurnal.KdJurnal = o.KdSaldoAwal;
                                daoJurnal.Simpan(Jurnal);

                                Trans.Commit();
                                Baris.Cells["KdSaw"].Value = o.KdSaldoAwal;
                                Baris.Cells["Total"].Value = Total;
                            }
                            catch (Exception exp)
                            {
                                Trans.Rollback();
                                AdnFungsi.LogErr(exp.Message);
                                break;
                                TerjadiKesalahan = true;
                            }
                            
                        } // -- END if Siswa Dipilih
                    }// -- END - foreach

                    if (!TerjadiKesalahan)
                    {
                        MessageBox.Show("Proses Salin Saldo Piutang Berhasil!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }//END --- Batal
            }//End IsValid()
        }

        private void checkBoxSemuaSiswa_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox o = (CheckBox)sender;
            foreach (DataGridViewRow Baris in dgvSiswa.Rows)
            {
                Baris.Cells["PilihanKdSiswa"].Value = o.Checked;
            }
        }
    }
}
