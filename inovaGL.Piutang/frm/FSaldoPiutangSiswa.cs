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
    [AdnScObjectAtr("Form: Daftar Saldo Awal Piutang Siswa", "Saldo Awal")]
    public partial class FTSaldoPiutangSiswa : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        BindingSource bsSiswa = new BindingSource();
        private AdnScPengguna Pengguna;
        private string KdSekolah = "";
        private string ThAjar = "";
        private string KdAkunPenyeimbangSaw = "";
        private DateTime PeriodeMulai;

        public FTSaldoPiutangSiswa(SqlConnection cnn, string AppName, AdnScPengguna Pengguna, string KdSekolah,string ThAjar, string KdAkunPenyeimbangSaw, DateTime PeriodeMulai)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.Pengguna=Pengguna;
            this.KdSekolah = KdSekolah.ToString().Trim();
            this.ThAjar = ThAjar.ToString().Trim();
            this.KdAkunPenyeimbangSaw = KdAkunPenyeimbangSaw;

            this.PeriodeMulai = PeriodeMulai;
            dateTimePickerTgl.Value = PeriodeMulai;
            dateTimePickerTgl.Focus();

            new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
            comboBoxSekolah.SelectedIndex = -1;

            if(!this.KdSekolah.Equals(""))
            {
                comboBoxSekolah.SelectedValue=KdSekolah;
            }

            new EDUSIS.Kelas.AdnKelasDao(this.cnn).SetCombo(comboBoxKelas, KdSekolah);
            comboBoxKelas.SelectedIndex = -1;

        }

        private void FillDataGridView(string KdSaw)
        {
            this.UseWaitCursor = true;
            Application.DoEvents();
            dgv.AutoGenerateColumns = false;
    
            bs.DataSource = null;
            bs.DataSource = new AdnSaldoAwalDao(this.cnn).GetRincian(KdSaw);
            dgv.DataSource = bs;

            this.HitungTotal();

            if (dgv.RowCount != 0)
            {
                toolStripButtonSimpan.Enabled = true;
            }

            this.UseWaitCursor = false;
        }
        private void FillDataGridViewBiayaSekolah(string KdSekolah,string Kelas,  int KdSiswa, string Nis)
        {
            this.UseWaitCursor = true;
            Application.DoEvents();
            dgv.AutoGenerateColumns = false;

            bs.DataSource = null;
            string Tingkat = new EDUSIS.Kelas.AdnKelasDao(this.cnn).Get(Kelas).Tingkat;
            bs.DataSource = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetRincian(KdSekolah, this.ThAjar, Tingkat,Nis);
            dgv.DataSource = bs;

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
            this.FillDataGridView("");
        }

        private void toolStripButtonSimpan_Click(object sender, EventArgs e)
        {
            this.Simpan();
        }

        private void Simpan()
        {
            dgv.EndEdit();
            //bool HapusDataLama = false;
            bool InputData = true;
            if (this.IsValid())
            {
                
                int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(this.KdSekolah).Tingkat;
                int KdSiswa = AdnFungsi.CInt(dgvSiswa.Rows[dgvSiswa.CurrentCell.RowIndex].Cells["KdSiswa"],true);
                if (new inovaGL.Piutang.AdnSaldoAwalDao(this.cnn).Get(KdSiswa, ThAjar))
                {
                    if (MessageBox.Show("Saldo Awal untuk Tahun Ajar: " + this.ThAjar + " Sudah Ada!\nSaldo Awal Mau Diinput Ulang?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        //HapusDataLama = true;
                    }
                    else
                    {
                        InputData = false;
                    }
                }//Cek IsAda SAW

                if (InputData)
                {
                    //--- Header SAW
                    AdnSaldoAwal o = new AdnSaldoAwal();
                    o.KdSaldoAwal = AdnFungsi.CStr(dgvSiswa.Rows[dgvSiswa.CurrentCell.RowIndex].Cells["KdSaw"]); ;
                    o.KdSiswa = KdSiswa;
                    o.KdSekolah = this.KdSekolah;
                    o.ThAjar = this.ThAjar;
                    o.Tgl = dateTimePickerTgl.Value;
                    o.Periode = "";
                    o.Total = AdnFungsi.CDec(textBoxTotal);

                    o.DfItem = new List<AdnSaldoAwalDtl>();
                    //--- END --- Header SAW


                    //--- Header Jurnal
                    inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                    Jurnal.KdJurnal = "";
                    Jurnal.Deskripsi = "Jurnal Piutang Siswa [Saldo Awal]";
                    Jurnal.Tgl = dateTimePickerTgl.Value;
                    Jurnal.StatusPosting = false;
                    Jurnal.Sumber = this.Name;
                    Jurnal.JenisJurnal = inovaGL.Data.AdnJurnalVar.JenisJurnal.JSAW_PIUTANG;
                    Jurnal.ItemDf = new List<Data.AdnJurnalDtl>();

                    Jurnal.KdSiswa = KdSiswa;
                    Jurnal.KdSekolah = o.KdSekolah;
                    Jurnal.Nis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(KdSiswa).NIS;
                    Jurnal.ThAjar = o.ThAjar;
                    Jurnal.ThAjarTagihan = o.ThAjar;

                    //--- END --- Header Jurnal
                    int NoUrutDebet = 0;
                    int NoUrutKredit = dgv.Rows.Count;
                    decimal Total = 0;
                    foreach (DataGridViewRow baris in dgv.Rows)
                    {
                        if (AdnFungsi.CDec(baris.Cells["Jmh"]) > 0)
                        {
                            // --- Rincian Biaya SAW
                            AdnSaldoAwalDtl dtl = new AdnSaldoAwalDtl();
                            dtl.KdSaldoAwal = o.KdSaldoAwal;
                            dtl.KdDtl = 0;
                            dtl.KdBiaya = AdnFungsi.CStr(baris.Cells["KdBiaya"]);
                            dtl.Jmh = AdnFungsi.CDec(baris.Cells["Jmh"]);
                            dtl.ItemBulan = "";
                            dtl.Keterangan = AdnFungsi.CStr(baris.Cells["Keterangan"]);

                            dtl.DfPeriode = new List<AdnSaldoAwalDtlPeriode>();
                            Total = Total + dtl.Jmh;

                            string StrPeriode = AdnFungsi.CStr(baris.Cells["ItemBulan"]);
                            if (StrPeriode.Trim() != "")
                            {
                                string[] ArrPeriode = StrPeriode.Split(',');
                                foreach (string item in ArrPeriode)
                                {
                                    // Periode/Bulan Tagihan
                                    AdnSaldoAwalDtlPeriode oPeriode = new AdnSaldoAwalDtlPeriode();
                                    oPeriode.Periode = item;

                                    dtl.DfPeriode.Add(oPeriode);
                                    // --- END --- Periode/Bulan Tagihan
                                }
                            }
                            o.DfItem.Add(dtl);
                            // --- END --- Rincian Biaya SAW

                            //--- Detail Jurnal Debet
                            inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                            JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                            JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPiutang"]);
                            JurnalDtl.Debet = dtl.Jmh;
                            JurnalDtl.Kredit = 0;
                            JurnalDtl.KdDept = KdSekolah;
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
                            JurnalDtl.KdDept = KdSekolah;
                            JurnalDtl.Memo = "Saldo Awal Piutang";
                            JurnalDtl.NoUrut = NoUrutKredit;

                            NoUrutKredit++;
                            //--- END --- Detail Jurnal Kredit

                            Jurnal.ItemDf.Add(JurnalDtl);
                        }
                    }
                    o.Total = Total;
                    // Simpan Data
                    SqlTransaction Trans = null;
                    try
                    {

                        Trans = this.cnn.BeginTransaction();

                        //if (HapusDataLama)
                        //{
                        //    new AdnSaldoAwalDao(this.cnn, this.Pengguna, Trans).Hapus(KdSiswa, this.ThAjar, dateTimePickerTgl.Value);
                        //}

                        AdnSaldoAwalDao dao = new AdnSaldoAwalDao(this.cnn, this.Pengguna, Trans);
                        inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);

                        if (o.KdSaldoAwal != "") //??????
                        {
                            dao.Hapus(o.KdSaldoAwal);
                            daoJurnal.Hapus(o.KdSaldoAwal);
                        }
                        dao.Simpan(o);
                        Jurnal.KdJurnal = o.KdSaldoAwal;
                        daoJurnal.Simpan(Jurnal);

                        Trans.Commit();
                        dgvSiswa.CurrentRow.Cells["KdSaw"].Value = o.KdSaldoAwal;
                        dgvSiswa.CurrentRow.Cells["Total"].Value = Total;
                        MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exp)
                    {
                        Trans.Rollback();
                        AdnFungsi.LogErr(exp.Message);
                    }
                    // --- END --- Simpan Data
                }//END --- InputData


            }//END IsValid
        }

        private bool IsValid()
        {
            string sPesan = "";

            if (dateTimePickerTgl.Text.Replace("/", "").ToString().Trim() == "")
            {

                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Tanggal";
            }

            if (dgv.Rows.Count== 0)
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
            decimal TotalDebet = 0;
            for (int iBaris = 0; iBaris <= dgv.Rows.Count - 1; iBaris++)
            {
                TotalDebet = TotalDebet + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["Jmh"]);
            }
            textBoxTotal.Text = TotalDebet.ToString("N0");
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
            bs.DataSource = null;
            dgv.DataSource = bs;

            textBoxNmSiswa.Text = "";
            buttonTampil.Enabled = false;
            buttonSalin.Enabled = false;
            buttonBiaya.Enabled = false;
            if (dgvSiswa.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvSiswa.SelectedRows[0];
                textBoxNmSiswa.Text = row.Cells["NmLengkap"].Value.ToString();

                buttonTampil.Enabled = true;
                buttonBiaya.Enabled = true;
            }
        }

        private void dateTimePickerTgl_ValueChanged(object sender, EventArgs e)
        {
            bs.DataSource = null;
            dgv.DataSource = bs;

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
            buttonSalin.Enabled = true;

        }

        private bool SimpanJurnal()
        {
            bool Hasil = false;


            return Hasil;
        }

        private void comboBoxSekolah_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSekolah.SelectedIndex > -1)
            {
                this.KdSekolah = comboBoxSekolah.SelectedValue.ToString();
                new EDUSIS.Kelas.AdnKelasDao(this.cnn).SetCombo(comboBoxKelas, this.KdSekolah);
                comboBoxKelas.SelectedIndex = -1;
            }
            else
            {

            }
        }

        private void buttonSalin_Click(object sender, EventArgs e)
        {
            if (textBoxNmSiswa.Text.Trim() == "")
            {
                MessageBox.Show("Siswa belum dipilih!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                //if(AdnFungsi.CDec(dgvSiswa.CurrentRow.Cells["Total"])==0)
                //{
                //    MessageBox.Show("Jumlah Piutang = 0 (Nol)!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
                //else
                //{
                    if (comboBoxSekolah.SelectedIndex > -1)
                    {
                        string KdSekolah = comboBoxSekolah.SelectedValue.ToString();
                        List<EDUSIS.Biaya.AdnBiayaSekolah> lst = new List<EDUSIS.Biaya.AdnBiayaSekolah>();

                        foreach (DataGridViewRow Baris in dgv.Rows)
                        {
                            EDUSIS.Biaya.AdnBiayaSekolah bs = new EDUSIS.Biaya.AdnBiayaSekolah();
                            bs.oBiaya = new EDUSIS.Biaya.AdnBiaya();

                            bs.KdBiaya = AdnFungsi.CStr(Baris.Cells["KdBiaya"]);
                            bs.KdSekolah = KdSekolah;
                            bs.ThAjar = this.ThAjar;
                            bs.Jmh = AdnFungsi.CDec(Baris.Cells["Jmh"]);
                            bs.oBiaya.KdAkunPiutang = AdnFungsi.CStr(Baris.Cells["KdAkunPiutang"]);
                            bs.oBiaya.KdAkunPendapatan = AdnFungsi.CStr(Baris.Cells["KdAkunPendapatan"]);
                            lst.Add(bs);
                        }

                        int KdSiswa = AdnFungsi.CInt(dgvSiswa.CurrentRow.Cells["KdSiswa"], true);
                        FProsesSalinSaldoPiutangSiswa ofm = new FProsesSalinSaldoPiutangSiswa(this.cnn, this.AppName, this.Pengguna, KdSekolah, this.ThAjar, lst, KdSiswa, textBoxNmSiswa.Text,this.KdAkunPenyeimbangSaw, this.PeriodeMulai);
                        ofm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Pilih Salah Satu Sekolah!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                //}
            }
        }

        private void FTSaldoPiutangSiswa_Load(object sender, EventArgs e)
        {
            if (this.KdAkunPenyeimbangSaw.Trim() == "")
            {
                MessageBox.Show("Akun Penyeimbang Saldo Awal Belum Ditentukan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                groupBoxHdr.Enabled = false;
                groupBoxDtl.Enabled = false;
            }
        }

        private void buttonBiaya_Click(object sender, EventArgs e)
        {
            dgvSiswa.Enabled = false;
            string KdSekolah = "";
            string Kelas="";
            int KdSiswa = 0;
            string Nis = "";

            if (comboBoxSekolah.SelectedIndex > -1)
            {
                KdSekolah = comboBoxSekolah.SelectedValue.ToString();
            }

            if (comboBoxKelas.SelectedIndex > -1)
            {
                Kelas = comboBoxKelas.SelectedValue.ToString();
            }

            if (dgvSiswa.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvSiswa.SelectedRows[0];
                KdSiswa = AdnFungsi.CInt(row.Cells["KdSiswa"],true);
                Nis = AdnFungsi.CStr(row.Cells["Nis"]);
            }

            this.FillDataGridViewBiayaSekolah(KdSekolah,Kelas,KdSiswa,Nis);
            dgvSiswa.Enabled = true;
            buttonSalin.Enabled = true;
        }
    }
}
