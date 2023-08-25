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
using EDUSIS.KeuanganTagihan;

namespace inovaGL
{
    [AdnScObjectAtr("Form: Proses Bulanan", "Proses")]
    public partial class FProsesBulanan : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        BindingSource bsSiswa = new BindingSource();
        private AdnScPengguna Pengguna;
        private string KdSekolah = "";
        private string ThAjar = "";
       
        public FProsesBulanan(SqlConnection cnn, string AppName, AdnScPengguna Pengguna, string KdSekolah,string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.Pengguna = Pengguna;
            this.KdSekolah = KdSekolah.ToString().Trim();
            this.ThAjar = ThAjar.ToString().Trim();
            dateTimePickerTgl.Focus();
            dateTimePickerTgl.Value = DateTime.Now ;

            AdnFungsi.SetComboBulan(comboBoxBulan, false);
            numericUpDownTahun.Value = DateTime.Now.Year;
            comboBoxBulan.SelectedValue = DateTime.Now.Month;

            dgvSekolah.AutoGenerateColumns = false;
            BindingSource bs = new BindingSource();
            bs.DataSource = new EDUSIS.Shared.AdnSekolahDao(this.cnn).GetAll();
            dgvSekolah.DataSource = bs;

            //this.FillDataGridView("");
        }

        private void FillDataGridView(string KdTagihan)
        {
            this.UseWaitCursor = true;
            Application.DoEvents();
            //dgv.AutoGenerateColumns = false;
    
            //bs.DataSource = null;
            //bs.DataSource = new AdnTagihanSiswaDao(this.cnn).GetRincian(KdTagihan);
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
            //if (this.IsValid())
            //{
            //    int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(this.KdSekolah).Tingkat;

            //    int KdSiswa = AdnFungsi.CInt(dgvSiswa.Rows[dgvSiswa.CurrentCell.RowIndex].Cells["KdSiswa"], true);
            //    string Prefix = new AdnTagihanSiswaDao(this.cnn).GetPrefixTagihan(Tingkat);

            //    //--- Header Tagihan
            //    AdnTagihanSiswa o = new AdnTagihanSiswa();
            //    o.KdSiswa = KdSiswa;
            //    o.KdSekolah = this.KdSekolah;
            //    o.ThAjar = this.ThAjar;
            //    o.TglTerbit = dateTimePickerTgl.Value;
            //    o.Periode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            //    o.KdTagihan = new AdnTagihanSiswaDao(this.cnn).GetKode(Tingkat, o.Periode);
            //    o.Keterangan = "";
            //    o.Total = 0;// AdnFungsi.CDec(textBoxTotal);
            //    o.DfItem = new List<AdnTagihanSiswaDtl>();
            //    //--- END --- Header Tagihan

            //    //--- Header Jurnal
            //    inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
            //    Jurnal.KdJurnal = "";
            //    Jurnal.Deskripsi = "";
            //    Jurnal.Tgl = dateTimePickerTgl.Value;
            //    Jurnal.StatusPosting = false;
            //    Jurnal.Sumber = this.Name;
            //    Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();

            //    //--- END --- Header Jurnal
            //    int NoUrutDebet = 0;
            //    int NoUrutKredit = dgv.Rows.Count;
            //    decimal Total = 0;

            //    foreach (DataGridViewRow baris in dgv.Rows)
            //    {
            //        // --- Rincian Biaya Tagihan
            //        AdnTagihanSiswaDtl dtl = new AdnTagihanSiswaDtl();
            //        dtl.KdTagihan = o.KdTagihan;
            //        dtl.KdBiaya = AdnFungsi.CStr(baris.Cells["KdBiaya"]);
            //        dtl.Jmh =AdnFungsi.CDec(baris.Cells["Jmh"]);
            //        dtl.ItemBulan = "";
            //        dtl.Keterangan = AdnFungsi.CStr(baris.Cells["Keterangan"]);
                    
            //        Total = Total + dtl.Jmh;

            //        dtl.DfPeriode = new List<AdnTagihanSiswaDtlPeriode>();

            //        string StrPeriode = AdnFungsi.CStr(baris.Cells["ItemBulan"]);
            //        string[] ArrPeriode = StrPeriode.Split(',');
            //        foreach(string item in ArrPeriode)
            //        {
            //            // Periode/Bulan Tagihan
            //            AdnTagihanSiswaDtlPeriode oPeriode = new AdnTagihanSiswaDtlPeriode();
            //            oPeriode.Periode = item;
                        
            //            dtl.DfPeriode.Add(oPeriode);
            //            // --- END --- Periode/Bulan Tagihan
            //        }
            //        o.DfItem.Add(dtl);
            //        // --- END --- Rincian Biaya Tagihan

            //        //--- Detail Jurnal Debet
            //        inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
            //        JurnalDtl.KdJurnal = Jurnal.KdJurnal;
            //        JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPiutang"]);
            //        JurnalDtl.Debet = dtl.Jmh;
            //        JurnalDtl.Kredit = 0;
            //        JurnalDtl.KdDept = KdSekolah;
            //        JurnalDtl.Memo = "Tagihan Periode" + StrPeriode;
            //        JurnalDtl.NoUrut = NoUrutDebet;

            //        NoUrutDebet++;
            //        //--- END --- Detail Jurnal Debet

            //        Jurnal.ItemDf.Add(JurnalDtl);

            //        //--- Detail Jurnal Kredit
            //        JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
            //        JurnalDtl.KdJurnal = Jurnal.KdJurnal;
            //        JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPendapatan"]);
            //        JurnalDtl.Debet = 0;
            //        JurnalDtl.Kredit = dtl.Jmh;
            //        JurnalDtl.KdDept = KdSekolah;
            //        JurnalDtl.Memo = "Tagihan Periode" + StrPeriode;
            //        JurnalDtl.NoUrut = NoUrutKredit;

            //        NoUrutKredit++;
            //        //--- END --- Detail Jurnal Kredit

            //        Jurnal.ItemDf.Add(JurnalDtl);

            //    }
            //    o.Total = Total;
            //    // Simpan Data
            //    SqlTransaction Trans = null;
            //    try
            //    {
            //        Trans = this.cnn.BeginTransaction();

            //        AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
            //        inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
            //        dao.Simpan(o);

            //        Jurnal.KdJurnal = o.KdTagihan;
            //        daoJurnal.Simpan(Jurnal);

            //        Trans.Commit();
            //        dgvSiswa.CurrentRow.Cells["KdTagihan"].Value = o.KdTagihan;
            //        dgvSiswa.CurrentRow.Cells["Total"].Value = Total;
            //        textBoxKdTagihan.Text = o.KdTagihan;
            //        MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    catch (Exception exp)
            //    {
            //        AdnFungsi.LogErr(exp.Message);
            //    }

            //    // --- END --- Simpan Data

            //}
        }

        private bool IsValid()
        {
            string sPesan = "";

            if (dateTimePickerTgl.Text.Replace("/", "").ToString().Trim() == "")
            {

                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Tanggal";
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
            //decimal Total = 0;
            //for (int iBaris = 0; iBaris <= dgv.Rows.Count - 1; iBaris++)
            //{
            //    Total = Total + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["Jmh"]);
            //}
            //textBoxTotal.Text = Total.ToString("N0");
        }

        private void SetDefaultPeriode(string Periode)
        {
            //for (int iBaris = 0; iBaris <= dgv.Rows.Count - 1; iBaris++)
            //{
            //    if (AdnFungsi.CStr(dgv.Rows[iBaris].Cells["ItemBulan"]) == "")
            //    {
            //        dgv.Rows[iBaris].Cells["ItemBulan"].Value = Periode;
            //    }
            //}
        }

        private void buttonProses_Click(object sender, EventArgs e)
        {
            //this.Simpan();
            string KdSekolah = AdnFungsi.CStr(dgvSekolah.CurrentRow.Cells["KolomKdSekolah"]);
            string Periode =numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');

            foreach (DataGridViewRow row in dgvKelas.Rows)
            {
                if (AdnFungsi.CBool(row.Cells["PilihanKelas"],true))
                {
                    string Kelas = AdnFungsi.CStr(row.Cells["Kelas"]);
                    string Tingkat = new EDUSIS.Kelas.AdnKelasDao(this.cnn).Get(Kelas).Tingkat;

                    List<int> lstKdSiswa = new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).GetKdSiswa(KdSekolah, this.ThAjar, Kelas);
                    List<EDUSIS.Biaya.AdnBiayaSekolah> LstBiayaSekolah = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).Get(KdSekolah, this.ThAjar, Tingkat);

                    if (MembuatTagihan(KdSekolah, this.ThAjar, Periode, dateTimePickerTgl.Value, lstKdSiswa, LstBiayaSekolah))
                    {
                        MessageBox.Show("Semua Tagihan Siswa Berhasil Disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        row.Cells["Status"].Value = "SUKSES";
                    }
                    else
                    {
                        row.Cells["Status"].Value = "GAGAL";
                    }
                }
            }
        }


        private void buttonProsesBLdanTHN_Click(object sender, EventArgs e)
        {
            //dgv.EndEdit();
            //if (this.IsValid())
            //{
            //    int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(this.KdSekolah).Tingkat;

            //    int KdSiswa = AdnFungsi.CInt(dgvSiswa.Rows[dgvSiswa.CurrentCell.RowIndex].Cells["KdSiswa"], true);
            //    string Prefix = new AdnTagihanSiswaDao(this.cnn).GetPrefixTagihan(Tingkat);

            //    //--- Header Tagihan
            //    AdnTagihanSiswa o = new AdnTagihanSiswa();
            //    o.KdSiswa = KdSiswa;
            //    o.KdSekolah = this.KdSekolah;
            //    o.ThAjar = this.ThAjar;
            //    o.TglTerbit = dateTimePickerTgl.Value;
            //    o.Periode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            //    o.KdTagihan = new AdnTagihanSiswaDao(this.cnn).GetKode(Tingkat, o.Periode);
            //    o.Keterangan = "";
            //    o.Total = 0;
            //    o.DfItem = new List<AdnTagihanSiswaDtl>();
            //    //--- END --- Header Tagihan

            //    //--- Header Jurnal
            //    inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
            //    Jurnal.KdJurnal = "";
            //    Jurnal.Deskripsi = "";
            //    Jurnal.Tgl = dateTimePickerTgl.Value;
            //    Jurnal.StatusPosting = false;
            //    Jurnal.Sumber = this.Name;
            //    Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();

            //    //--- END --- Header Jurnal
            //    int NoUrutDebet = 0;
            //    int NoUrutKredit = dgv.Rows.Count;
            //    decimal Total = 0;

            //    foreach (DataGridViewRow baris in dgv.Rows)
            //    {
            //        if (AdnFungsi.CStr(baris.Cells["KdJenis"]) == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN
            //            || AdnFungsi.CStr(baris.Cells["KdJenis"]) == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN_CICIL
            //            || AdnFungsi.CStr(baris.Cells["KdJenis"]) == EDUSIS.Shared.EdusisVar.JenisBiaya.TAHUNAN
            //            || AdnFungsi.CStr(baris.Cells["KdJenis"]) == EDUSIS.Shared.EdusisVar.JenisBiaya.TAHUNAN_CICIL
            //            )
            //        {



            //            // --- Rincian Biaya Tagihan
            //            AdnTagihanSiswaDtl dtl = new AdnTagihanSiswaDtl();
            //            dtl.KdTagihan = o.KdTagihan;
            //            dtl.KdBiaya = AdnFungsi.CStr(baris.Cells["KdBiaya"]);
            //            dtl.Jmh = AdnFungsi.CDec(baris.Cells["Jmh"]);
            //            dtl.ItemBulan = "";
            //            dtl.Keterangan = AdnFungsi.CStr(baris.Cells["Keterangan"]);

            //            Total = Total + dtl.Jmh;

            //            dtl.DfPeriode = new List<AdnTagihanSiswaDtlPeriode>();

            //            string StrPeriode = AdnFungsi.CStr(baris.Cells["ItemBulan"]);
            //            string[] ArrPeriode = StrPeriode.Split(',');
            //            foreach (string item in ArrPeriode)
            //            {
            //                // Periode/Bulan Tagihan
            //                AdnTagihanSiswaDtlPeriode oPeriode = new AdnTagihanSiswaDtlPeriode();
            //                oPeriode.Periode = item;

            //                dtl.DfPeriode.Add(oPeriode);
            //                // --- END --- Periode/Bulan Tagihan
            //            }
            //            o.DfItem.Add(dtl);
            //            // --- END --- Rincian Biaya Tagihan

            //            //--- Detail Jurnal Debet
            //            inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
            //            JurnalDtl.KdJurnal = Jurnal.KdJurnal;
            //            JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPiutang"]);
            //            JurnalDtl.Debet = dtl.Jmh;
            //            JurnalDtl.Kredit = 0;
            //            JurnalDtl.KdDept = KdSekolah;
            //            JurnalDtl.Memo = "Tagihan Periode" + StrPeriode;
            //            JurnalDtl.NoUrut = NoUrutDebet;

            //            NoUrutDebet++;
            //            //--- END --- Detail Jurnal Debet

            //            Jurnal.ItemDf.Add(JurnalDtl);

            //            //--- Detail Jurnal Kredit
            //            JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
            //            JurnalDtl.KdJurnal = Jurnal.KdJurnal;

            //            if (AdnFungsi.CStr(baris.Cells["KdJenis"]) == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN
            //            || AdnFungsi.CStr(baris.Cells["KdJenis"]) == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN_CICIL
            //            )
            //            {
            //                JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPendapatan"]);
            //            }
            //            else
            //            {
            //                JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunKewajiban"]);
            //            }
            //            JurnalDtl.Debet = 0;
            //            JurnalDtl.Kredit = dtl.Jmh;
            //            JurnalDtl.KdDept = KdSekolah;
            //            JurnalDtl.Memo = "Tagihan Periode" + StrPeriode;
            //            JurnalDtl.NoUrut = NoUrutKredit;

            //            NoUrutKredit++;
            //            //--- END --- Detail Jurnal Kredit

            //            Jurnal.ItemDf.Add(JurnalDtl);

            //        }
            //        o.Total = Total;
            //        // Simpan Data
            //        SqlTransaction Trans = null;
            //        try
            //        {
            //            Trans = this.cnn.BeginTransaction();

            //            AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
            //            inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
            //            dao.Simpan(o);

            //            Jurnal.KdJurnal = o.KdTagihan;
            //            daoJurnal.Simpan(Jurnal);

            //            Trans.Commit();
            //            dgvSiswa.CurrentRow.Cells["KdTagihan"].Value = o.KdTagihan;
            //            dgvSiswa.CurrentRow.Cells["Total"].Value = Total;
            //            textBoxKdTagihan.Text = o.KdTagihan;
            //            MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //        catch (Exception exp)
            //        {
            //            AdnFungsi.LogErr(exp.Message);
            //        }

            //        // --- END --- Simpan Data
            //    }
            //}
        }

        private void buttonProsesPendapatan_Click(object sender, EventArgs e)
        {
            //dgv.EndEdit();
            //if (this.IsValid())
            //{
            //    int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(this.KdSekolah).Tingkat;

            //    int KdSiswa = AdnFungsi.CInt(dgvSiswa.Rows[dgvSiswa.CurrentCell.RowIndex].Cells["KdSiswa"], true);
            //    string Prefix = new AdnTagihanSiswaDao(this.cnn).GetPrefixTagihan(Tingkat);

            //    //--- Header Tagihan
            //    AdnTagihanSiswa o = new AdnTagihanSiswa();
            //    o.KdSiswa = KdSiswa;
            //    o.KdSekolah = this.KdSekolah;
            //    o.ThAjar = this.ThAjar;
            //    o.TglTerbit = dateTimePickerTgl.Value;
            //    o.Periode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            //    o.KdTagihan = new AdnTagihanSiswaDao(this.cnn).GetKode(Tingkat, o.Periode);
            //    o.Keterangan = "";
            //    o.Total = 0;
            //    o.DfItem = new List<AdnTagihanSiswaDtl>();
            //    //--- END --- Header Tagihan

            //    //--- Header Jurnal
            //    inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
            //    Jurnal.KdJurnal = "";
            //    Jurnal.Deskripsi = "";
            //    Jurnal.Tgl = dateTimePickerTgl.Value;
            //    Jurnal.StatusPosting = false;
            //    Jurnal.Sumber = this.Name;
            //    Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();

            //    //--- END --- Header Jurnal
            //    int NoUrutDebet = 0;
            //    int NoUrutKredit = dgv.Rows.Count;
            //    decimal Total = 0;

            //    foreach (DataGridViewRow baris in dgv.Rows)
            //    {
            //        if (AdnFungsi.CStr(baris.Cells["KdJenis"]) == EDUSIS.Shared.EdusisVar.JenisBiaya.PENGEMBANGAN
            //            || AdnFungsi.CStr(baris.Cells["KdJenis"]) == EDUSIS.Shared.EdusisVar.JenisBiaya.PENGEMBANGAN_CICIL
            //            || AdnFungsi.CStr(baris.Cells["KdJenis"]) == EDUSIS.Shared.EdusisVar.JenisBiaya.TAHUNAN
            //            || AdnFungsi.CStr(baris.Cells["KdJenis"]) == EDUSIS.Shared.EdusisVar.JenisBiaya.TAHUNAN_CICIL
            //            )
            //        {
            //            // --- Rincian Biaya Tagihan
            //            AdnTagihanSiswaDtl dtl = new AdnTagihanSiswaDtl();
            //            dtl.KdTagihan = o.KdTagihan;
            //            dtl.KdBiaya = AdnFungsi.CStr(baris.Cells["KdBiaya"]);
            //            dtl.Jmh = AdnFungsi.CDec(baris.Cells["Jmh"]);
            //            dtl.ItemBulan = "";
            //            dtl.Keterangan = AdnFungsi.CStr(baris.Cells["Keterangan"]);

            //            Total = Total + dtl.Jmh;

            //            dtl.DfPeriode = new List<AdnTagihanSiswaDtlPeriode>();

            //            string StrPeriode = AdnFungsi.CStr(baris.Cells["ItemBulan"]);
            //            string[] ArrPeriode = StrPeriode.Split(',');
            //            foreach (string item in ArrPeriode)
            //            {
            //                // Periode/Bulan Tagihan
            //                AdnTagihanSiswaDtlPeriode oPeriode = new AdnTagihanSiswaDtlPeriode();
            //                oPeriode.Periode = item;

            //                dtl.DfPeriode.Add(oPeriode);
            //                // --- END --- Periode/Bulan Tagihan
            //            }
            //            o.DfItem.Add(dtl);
            //            // --- END --- Rincian Biaya Tagihan

            //            //--- Detail Jurnal Debet
            //            inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
            //            JurnalDtl.KdJurnal = Jurnal.KdJurnal;
            //            JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunKewajiban"]);
            //            JurnalDtl.Debet = dtl.Jmh;
            //            JurnalDtl.Kredit = 0;
            //            JurnalDtl.KdDept = KdSekolah;
            //            JurnalDtl.Memo = "Pengakuan Pendapatan" + StrPeriode;
            //            JurnalDtl.NoUrut = NoUrutDebet;

            //            NoUrutDebet++;
            //            //--- END --- Detail Jurnal Debet

            //            Jurnal.ItemDf.Add(JurnalDtl);

            //            //--- Detail Jurnal Kredit
            //            JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
            //            JurnalDtl.KdJurnal = Jurnal.KdJurnal;
            //            JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPendapatan"]);
            //            JurnalDtl.Debet = 0;
            //            JurnalDtl.Kredit = dtl.Jmh;
            //            JurnalDtl.KdDept = KdSekolah;
            //            JurnalDtl.Memo = "Pengakuan Pendapatan" + StrPeriode;
            //            JurnalDtl.NoUrut = NoUrutKredit;

            //            NoUrutKredit++;
            //            //--- END --- Detail Jurnal Kredit

            //            Jurnal.ItemDf.Add(JurnalDtl);

            //        }
            //        o.Total = Total;
            //        // Simpan Data
            //        SqlTransaction Trans = null;
            //        try
            //        {
            //            Trans = this.cnn.BeginTransaction();

            //            AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
            //            inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
            //            dao.Simpan(o);

            //            Jurnal.KdJurnal = o.KdTagihan;
            //            daoJurnal.Simpan(Jurnal);

            //            Trans.Commit();
            //            dgvSiswa.CurrentRow.Cells["KdTagihan"].Value = o.KdTagihan;
            //            dgvSiswa.CurrentRow.Cells["Total"].Value = Total;
            //            textBoxKdTagihan.Text = o.KdTagihan;
            //            MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //        catch (Exception exp)
            //        {
            //            AdnFungsi.LogErr(exp.Message);
            //        }

            //        // --- END --- Simpan Data
            //    }
            //}
        }

        private void JurnalDanaBulanan(string aKdSekolah, string aKelas, DateTime aTglTerbit, string aPeriode)
        {
            int Bulan = int.Parse(aPeriode.Substring(4,2));
            DataTable lstDeposit = new inovaGL.Data.AdnLoketDao(this.cnn).GetDepositBiayaSPP(aKdSekolah, this.ThAjar, Bulan);

            int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(aKdSekolah).Tingkat;
            string Prefix = new AdnTagihanSiswaDao(this.cnn).GetPrefixTagihan(Tingkat);
            List<int> lstKdSiswa = new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).GetKdSiswa(aKdSekolah, this.ThAjar, aKelas);

            string TkKelas = new EDUSIS.Kelas.AdnKelasDao(this.cnn).Get(aKelas).Tingkat;
            List<EDUSIS.Biaya.AdnBiayaSekolah> lstBiaya = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetByJenis(aKdSekolah, this.ThAjar, TkKelas,EDUSIS.Shared.EdusisVar.JenisBiaya.SPP);

            foreach (int itemSiswa in lstKdSiswa)
            {
                int KdSiswa = itemSiswa;
                //--- Header Tagihan
                AdnTagihanSiswa o = new AdnTagihanSiswa();
                o.KdSiswa = KdSiswa;
                o.KdSekolah = aKdSekolah;
                o.ThAjar = this.ThAjar;
                o.TglTerbit = aTglTerbit;
                o.Periode = aPeriode;
                o.KdTagihan = new AdnTagihanSiswaDao(this.cnn).GetKode(Tingkat, o.Periode);
                o.Keterangan = "Jurnal Dana Bulanan " + aPeriode; 
                o.Total = 0;
                //o.Total = AdnFungsi.CDec(textBoxTotal);
                o.DfItem = new List<AdnTagihanSiswaDtl>();
                //--- END --- Header Tagihan

                //--- Header Jurnal
                inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                Jurnal.KdJurnal = o.KdTagihan;
                Jurnal.Deskripsi = o.Keterangan;
                Jurnal.Tgl = dateTimePickerTgl.Value;
                Jurnal.StatusPosting = false;
                Jurnal.Sumber = this.Name;
                Jurnal.JenisJurnal = inovaGL.Data.AdnJurnalVar.JenisJurnal.JBL_PIU_PDP;
                Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();
                Jurnal.KdSiswa = o.KdSiswa;
                Jurnal.KdSekolah = o.KdSekolah;
                Jurnal.Nis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(o.KdSiswa).NIS;
                Jurnal.ThAjar = o.ThAjar;
                Jurnal.ThAjarTagihan = o.ThAjar;

                //--- END --- Header Jurnal
                int NoUrutDebet = 0;
                int NoUrutKredit = 100;
                decimal Total = 0;

                foreach (EDUSIS.Biaya.AdnBiayaSekolah item in lstBiaya)
                {
                    // --- Rincian Biaya Tagihan
                    AdnTagihanSiswaDtl dtl = new AdnTagihanSiswaDtl();
                    dtl.KdTagihan = o.KdTagihan;
                    dtl.KdBiaya = item.KdBiaya;
                    dtl.Jmh = item.Jmh-new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetPotongan(aKdSekolah, this.ThAjar, item.KdBiaya, Jurnal.Nis);
                    dtl.ItemBulan = AppFungsi.KonversiAngkaToNamaBulan(AdnFungsi.CInt(aPeriode.Substring(4, 2), true));
                    dtl.Keterangan = "Tagihan Periode " + aPeriode; 

                    Total = Total + dtl.Jmh;

                    dtl.DfPeriode = new List<AdnTagihanSiswaDtlPeriode>();

                    string StrPeriode = aPeriode;
                    string[] ArrPeriode = StrPeriode.Split(',');
                    foreach (string itemPeriode in ArrPeriode)
                    {
                        // Periode/Bulan Tagihan
                        AdnTagihanSiswaDtlPeriode oPeriode = new AdnTagihanSiswaDtlPeriode();
                        oPeriode.Periode = itemPeriode;
                        dtl.DfPeriode.Add(oPeriode);
                        // --- END --- Periode/Bulan Tagihan
                    }
                    o.DfItem.Add(dtl);
                    // --- END --- Rincian Biaya Tagihan

                    //--- Detail Jurnal Debet
                    inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;

                    //Periksa Deposit Pembayaran SPP
                    lstDeposit.DefaultView.RowFilter = "kd_siswa =" + o.KdSiswa ;
                    DataTable dtOutput = lstDeposit.DefaultView.ToTable();
                    if (dtOutput.Rows.Count > 0)
                    {
                        JurnalDtl.KdAkun = item.oBiaya.KdAkunDeposit;
                    }
                    else
                    {
                        JurnalDtl.KdAkun = item.oBiaya.KdAkunPiutang;
                    }
                    
                    
                    JurnalDtl.Debet = dtl.Jmh;
                    JurnalDtl.Kredit = 0;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = "Tagihan Periode " + StrPeriode;
                    JurnalDtl.NoUrut = NoUrutDebet;

                    NoUrutDebet++;
                    //--- END --- Detail Jurnal Debet

                    Jurnal.ItemDf.Add(JurnalDtl);

                    //--- Detail Jurnal Kredit
                    JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = item.oBiaya.KdAkunPendapatan;
                    JurnalDtl.Debet = 0;
                    JurnalDtl.Kredit = dtl.Jmh;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = "Tagihan Periode " + StrPeriode;
                    JurnalDtl.NoUrut = NoUrutKredit;

                    NoUrutKredit++;
                    //--- END --- Detail Jurnal Kredit

                    Jurnal.ItemDf.Add(JurnalDtl);

                }
                o.Total = Total;
                // Simpan Data
                SqlTransaction Trans = null;
                try
                {
                    Trans = this.cnn.BeginTransaction();

                    AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
                    inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
                    dao.Simpan(o);

                    Jurnal.KdJurnal = o.KdTagihan;
                    daoJurnal.Simpan(Jurnal);

                    Trans.Commit();
                    //MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exp)
                {
                    AdnFungsi.LogErr(exp.Message);
                }

                // --- END --- Simpan Data

            }

        }

        private void buttonJurnalBulanan_Click(object sender, EventArgs e)
        {
            //this.JurnalDanaBulanan();
        }

        private void buttonJurnalTahunan_Click(object sender, EventArgs e)
        {
            this.JurnalDanaTahunan();
        }

        private void JurnalDanaTahunan()
        {

            //int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(this.KdSekolah).Tingkat;
            //string Prefix = new AdnTagihanSiswaDao(this.cnn).GetPrefixTagihan(Tingkat);


            //string Kelas = AdnFungsi.CStr(dgvKelas.CurrentRow.Cells["Kelas"]);
            //string Sekolah = AdnFungsi.CStr(dgvSekolah.CurrentRow.Cells["KolomKdSekolah"]);
            //List<int> lstKdSiswa = new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).GetKdSiswa(Sekolah, this.ThAjar, Kelas);

            //foreach (int itemSiswa in lstKdSiswa)
            //{
            //    int KdSiswa = itemSiswa;

            //    //--- Header Tagihan
            //    AdnTagihanSiswa o = new AdnTagihanSiswa();
            //    o.KdSiswa = KdSiswa;
            //    o.KdSekolah = this.KdSekolah;
            //    o.ThAjar = this.ThAjar;
            //    o.TglTerbit = dateTimePickerTgl.Value;
            //    o.Periode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            //    o.KdTagihan = new AdnTagihanSiswaDao(this.cnn).GetKode(Tingkat, o.Periode);
            //    o.Keterangan = "";
            //    //o.Total = AdnFungsi.CDec(textBoxTotal);
            //    o.DfItem = new List<AdnTagihanSiswaDtl>();
            //    //--- END --- Header Tagihan

            //    //--- Header Jurnal
            //    inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
            //    Jurnal.KdJurnal = "";
            //    Jurnal.Deskripsi = "";
            //    Jurnal.Tgl = dateTimePickerTgl.Value;
            //    Jurnal.StatusPosting = false;
            //    Jurnal.Sumber = this.Name;
            //    Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();

            //    //--- END --- Header Jurnal
            //    int NoUrutDebet = 0;
            //    int NoUrutKredit = dgv.Rows.Count;
            //    decimal Total = 0;

            //    foreach (DataGridViewRow baris in dgv.Rows)
            //    {
            //        // --- Rincian Biaya Tagihan
            //        AdnTagihanSiswaDtl dtl = new AdnTagihanSiswaDtl();
            //        dtl.KdTagihan = o.KdTagihan;
            //        dtl.KdBiaya = AdnFungsi.CStr(baris.Cells["KdBiaya"]);
            //        dtl.Jmh = AdnFungsi.CDec(baris.Cells["Jmh"]);
            //        dtl.ItemBulan = "";
            //        dtl.Keterangan = AdnFungsi.CStr(baris.Cells["Keterangan"]);

            //        Total = Total + dtl.Jmh;
 
            //        string StrPeriode = AdnFungsi.CStr(baris.Cells["ItemBulan"]);
            //        o.DfItem.Add(dtl);
            //        // --- END --- Rincian Biaya Tagihan

            //        //--- Detail Jurnal Debet
            //        inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
            //        JurnalDtl.KdJurnal = Jurnal.KdJurnal;
            //        JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPiutang"]);
            //        JurnalDtl.Debet = dtl.Jmh;
            //        JurnalDtl.Kredit = 0;
            //        JurnalDtl.KdDept = KdSekolah;
            //        JurnalDtl.Memo = "Tagihan Periode" + StrPeriode;
            //        JurnalDtl.NoUrut = NoUrutDebet;

            //        NoUrutDebet++;
            //        //--- END --- Detail Jurnal Debet

            //        Jurnal.ItemDf.Add(JurnalDtl);

            //        //--- Detail Jurnal Kredit
            //        JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
            //        JurnalDtl.KdJurnal = Jurnal.KdJurnal;
            //        JurnalDtl.KdAkun = AdnFungsi.CStr(baris.Cells["KdAkunPendapatan"]);
            //        JurnalDtl.Debet = 0;
            //        JurnalDtl.Kredit = dtl.Jmh;
            //        JurnalDtl.KdDept = KdSekolah;
            //        JurnalDtl.Memo = "Tagihan Periode" + StrPeriode;
            //        JurnalDtl.NoUrut = NoUrutKredit;

            //        NoUrutKredit++;
            //        //--- END --- Detail Jurnal Kredit

            //        Jurnal.ItemDf.Add(JurnalDtl);

            //    }
            //    o.Total = Total;
            //    // Simpan Data
            //    SqlTransaction Trans = null;
            //    try
            //    {
            //        Trans = this.cnn.BeginTransaction();

            //        AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
            //        inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
            //        dao.Simpan(o);

            //        Jurnal.KdJurnal = o.KdTagihan;
            //        daoJurnal.Simpan(Jurnal);

            //        Trans.Commit();
            //        dgvSiswa.CurrentRow.Cells["KdTagihan"].Value = o.KdTagihan;
            //        dgvSiswa.CurrentRow.Cells["Total"].Value = Total;
            //        textBoxKdTagihan.Text = o.KdTagihan;
            //        MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    catch (Exception exp)
            //    {
            //        AdnFungsi.LogErr(exp.Message);
            //    }

            //    // --- END --- Simpan Data

            //}

        }

        private bool  MembuatTagihan(string KdSekolah, string ThAjar, string Periode,DateTime TglTerbit, List<int> LstKdSiswa, List<EDUSIS.Biaya.AdnBiayaSekolah> LstBiayaSekolah)
        {
            bool Sukses = false;

            int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(KdSekolah).Tingkat;

            foreach (int itemSiswa in LstKdSiswa)
            {
                decimal Total = 0 ;
                int KdSiswa = itemSiswa;

                //--- Header Tagihan
                AdnTagihanSiswa o = new AdnTagihanSiswa();
                o.KdSiswa = KdSiswa;
                o.KdSekolah = KdSekolah;
                o.ThAjar = ThAjar;
                o.TglTerbit = TglTerbit;
                o.Periode = Periode;
                o.KdTagihan = new AdnTagihanSiswaDao(this.cnn).GetKode(Tingkat, o.Periode);
                o.Keterangan = "Tagihan Periode " + o.Periode ;
                o.Total = 0 ;
                o.DfItem = new List<AdnTagihanSiswaDtl>();
                //--- END --- Header Tagihan

                //--- Header Jurnal
                inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                Jurnal.KdJurnal = "";
                Jurnal.Deskripsi = "";
                Jurnal.Tgl = o.TglTerbit;
                Jurnal.StatusPosting = false;
                Jurnal.Sumber = this.Name;
                Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();

                //--- END --- Header Jurnal
                int NoUrutDebet = 0;
                int NoUrutKredit = LstBiayaSekolah.Count;

                foreach(EDUSIS.Biaya.AdnBiayaSekolah itemBS in LstBiayaSekolah)
                {
                    
                    // --- Rincian Biaya Tagihan
                    // --- Jurnal Biaya Bulanan OR
                    // --- Jurnal Biaya Tahunan OR (Jmh/12)
                    // --- Jurnal Biaya Pengembangan (Jmh/(12*5)


                    if (itemBS.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.BULANAN
                        || itemBS.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.SPP
                        )
                    {
                        AdnTagihanSiswaDtl dtl = new AdnTagihanSiswaDtl();
                        dtl.KdTagihan = o.KdTagihan;
                        dtl.KdBiaya = itemBS.KdBiaya;
                        dtl.Jmh = itemBS.Jmh;
                        dtl.ItemBulan = Periode;
                        dtl.Keterangan = "";

                        Total = Total + dtl.Jmh;

                        dtl.DfPeriode = new List<AdnTagihanSiswaDtlPeriode>();

                        string StrPeriode = Periode;
                        string[] ArrPeriode = StrPeriode.Split(',');
                        foreach (string item in ArrPeriode)
                        {
                            // Periode/Bulan Tagihan
                            AdnTagihanSiswaDtlPeriode oPeriode = new AdnTagihanSiswaDtlPeriode();
                            oPeriode.Periode = item;

                            dtl.DfPeriode.Add(oPeriode);
                            // --- END --- Periode/Bulan Tagihan
                        }
                        o.DfItem.Add(dtl);
                        // --- END --- Rincian Biaya Tagihan

                        //--- Detail Jurnal Debet
                        inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                        JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                        JurnalDtl.KdAkun = itemBS.oBiaya.KdAkunPiutang;
                        JurnalDtl.Debet = dtl.Jmh;
                        JurnalDtl.Kredit = 0;
                        JurnalDtl.KdDept = KdSekolah;
                        JurnalDtl.Memo = "Tagihan Periode" + StrPeriode;
                        JurnalDtl.NoUrut = NoUrutDebet;

                        NoUrutDebet++;
                        //--- END --- Detail Jurnal Debet

                        Jurnal.ItemDf.Add(JurnalDtl);

                        //--- Detail Jurnal Kredit
                        JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                        JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                        JurnalDtl.KdAkun = itemBS.oBiaya.KdAkunPendapatan;
                        JurnalDtl.Debet = 0;
                        JurnalDtl.Kredit = dtl.Jmh;
                        JurnalDtl.KdDept = KdSekolah;
                        JurnalDtl.Memo = "Tagihan Periode" + StrPeriode;
                        JurnalDtl.NoUrut = NoUrutKredit;

                        NoUrutKredit++;
                        //--- END --- Detail Jurnal Kredit

                        Jurnal.ItemDf.Add(JurnalDtl);
                    }
                    else if (itemBS.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.TAHUNAN
                        || itemBS.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.TAHUNAN_CICIL
                        || itemBS.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.KEGIATAN)
                    {
                        //Jurnal Tahunan 
                        //--- Detail Jurnal Debet
                        inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                        JurnalDtl.KdJurnal = "TH" + Jurnal.KdJurnal;
                        JurnalDtl.KdAkun = itemBS.oBiaya.KdAkunPiutang;
                        JurnalDtl.Debet = itemBS.Jmh/12;
                        JurnalDtl.Kredit = 0;
                        JurnalDtl.KdDept = KdSekolah;
                        JurnalDtl.Memo = "Jurnal Dana Tahunan " + Periode;
                        JurnalDtl.NoUrut = NoUrutDebet;

                        NoUrutDebet++;
                        //--- END --- Detail Jurnal Debet

                        Jurnal.ItemDf.Add(JurnalDtl);

                        //--- Detail Jurnal Kredit
                        JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                        JurnalDtl.KdJurnal = "TH" + Jurnal.KdJurnal;
                        JurnalDtl.KdAkun = itemBS.oBiaya.KdAkunKewajiban;
                        JurnalDtl.Debet = 0;
                        JurnalDtl.Kredit = itemBS.Jmh/12;
                        JurnalDtl.KdDept = KdSekolah;
                        JurnalDtl.Memo = "Jurnal Dana Tahunan " + Periode;
                        JurnalDtl.NoUrut = NoUrutKredit;

                        NoUrutKredit++;
                        //--- END --- Detail Jurnal Kredit

                        Jurnal.ItemDf.Add(JurnalDtl);
                    }
                    else if (itemBS.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.PSB
                        || itemBS.oBiaya.KdJenis == EDUSIS.Shared.EdusisVar.JenisBiaya.PSB_CICIL)
                    {
                        //Jurnal Tahunan 
                        //--- Detail Jurnal Debet
                        inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                        JurnalDtl.KdJurnal = "PG" + Jurnal.KdJurnal;
                        JurnalDtl.KdAkun = itemBS.oBiaya.KdAkunPiutang;
                        if (Tingkat == 1)
                        {
                            JurnalDtl.Debet = itemBS.Jmh / 60;
                        }
                        else if(Tingkat==0)
                        {
                            JurnalDtl.Debet = itemBS.Jmh / 12;
                        }
                        JurnalDtl.Kredit = 0;
                        JurnalDtl.KdDept = KdSekolah;
                        JurnalDtl.Memo = "Jurnal Dana Pengembangan " + Periode;
                        JurnalDtl.NoUrut = NoUrutDebet;

                        NoUrutDebet++;
                        //--- END --- Detail Jurnal Debet

                        Jurnal.ItemDf.Add(JurnalDtl);

                        //--- Detail Jurnal Kredit
                        JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                        JurnalDtl.KdJurnal = "PG" + Jurnal.KdJurnal;
                        JurnalDtl.KdAkun = itemBS.oBiaya.KdAkunKewajiban;
                        JurnalDtl.Debet = 0;
                        if (Tingkat == 1)
                        {
                            JurnalDtl.Kredit = itemBS.Jmh / 60;
                        }
                        else if (Tingkat == 0)
                        {
                            JurnalDtl.Kredit = itemBS.Jmh / 12;
                        }
                        JurnalDtl.KdDept = KdSekolah;
                        JurnalDtl.Memo = "Jurnal Dana Pengembangan " + Periode;
                        JurnalDtl.NoUrut = NoUrutKredit;

                        NoUrutKredit++;
                        //--- END --- Detail Jurnal Kredit

                        Jurnal.ItemDf.Add(JurnalDtl);
                    }
                }
                o.Total = Total;

                // ----- Simpan Data
                SqlTransaction Trans = null;
                try
                {
                    Trans = this.cnn.BeginTransaction();

                    AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
                    inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
                    dao.Simpan(o);

                    Jurnal.KdJurnal = o.KdTagihan;
                    daoJurnal.Simpan(Jurnal);

                    Trans.Commit();
                    Sukses = true;
                }
                catch (Exception exp)
                {
                    AdnFungsi.LogErr("Terjadi Kesalahan Pada Kode: " + KdSiswa +"; " + exp.Message);
                    break;
                }

                // --- END --- Simpan Data
            }
            return Sukses;
        }

        private void dgvKelas_SelectionChanged(object sender, EventArgs e)
        {
            //bsSiswa.DataSource = null;
            //dgvSiswa.DataSource = bsSiswa;
            //if (dgvKelas.Rows.Count>0 && comboBoxSekolah.SelectedIndex>-1)
            //{
            //    dgvSiswa.AutoGenerateColumns = false;

            //    string Kelas = AdnFungsi.CStr(dgvKelas.CurrentRow.Cells["Kelas"]);
            //    string Sekolah = comboBoxSekolah.SelectedValue.ToString();
            //    bsSiswa.DataSource = new AdnTagihanSiswaDao(this.cnn).GetRingkasan(Kelas, this.ThAjar, Sekolah, (int)numericUpDownTahun.Value, (int)comboBoxBulan.SelectedValue);
            //    dgvSiswa.DataSource = bsSiswa;
            //}
        }

        private void dgvSekolah_SelectionChanged(object sender, EventArgs e)
        {

            BindingSource bsKelas = new BindingSource();
            bsKelas.DataSource = null;
            dgvKelas.DataSource = bsKelas;
            if (dgvSekolah.Rows.Count > 0)
            {
                dgvKelas.AutoGenerateColumns = false;
                string Sekolah = AdnFungsi.CStr(dgvSekolah.CurrentRow.Cells["KolomKdSekolah"]);
                bsKelas.DataSource = new EDUSIS.Kelas.AdnKelasDao(this.cnn).GetBySekolah(Sekolah);
                dgvKelas.DataSource = bsKelas;
            }

        }

        private void buttonProsesJurnalDanaBulanan_Click(object sender, EventArgs e)
        {
            string Periode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            DateTime TglTerbit = dateTimePickerTgl.Value;

            string KdSekolah = AdnFungsi.CStr(dgvSekolah.CurrentRow.Cells["KolomKdSekolah"]);

            foreach (DataGridViewRow row in dgvKelas.Rows)
            {
                if (AdnFungsi.CBool(row.Cells["PilihanKelas"], true))
                {
                    string Kelas = AdnFungsi.CStr(row.Cells["Kelas"]);
                    this.JurnalDanaBulanan(KdSekolah, Kelas, TglTerbit, Periode);
                    MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private void buttonProsesJurnalDanaTahunan_Click(object sender, EventArgs e)
        {
            string Periode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            DateTime TglTerbit = dateTimePickerTgl.Value;

            string KdSekolah = AdnFungsi.CStr(dgvSekolah.CurrentRow.Cells["KolomKdSekolah"]);

            foreach (DataGridViewRow row in dgvKelas.Rows)
            {
                if (AdnFungsi.CBool(row.Cells["PilihanKelas"], true))
                {
                    string Kelas = AdnFungsi.CStr(row.Cells["Kelas"]);
                    this.JurnalDanaTahunan2(KdSekolah, Kelas, TglTerbit, Periode);
                    MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private void JurnalDanaTahunan2(string aKdSekolah, string aKelas, DateTime aTglTerbit, string aPeriode)
        {

            int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(aKdSekolah).Tingkat;
            string Prefix = new AdnTagihanSiswaDao(this.cnn).GetPrefixTagihan(Tingkat);
            List<int> lstKdSiswa = new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).GetKdSiswa(aKdSekolah, this.ThAjar, aKelas);

            string TkKelas = new EDUSIS.Kelas.AdnKelasDao(this.cnn).Get(aKelas).Tingkat;
            List<EDUSIS.Biaya.AdnBiayaSekolah> lstBiaya = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetByJenis(aKdSekolah, this.ThAjar, TkKelas, EDUSIS.Shared.EdusisVar.JenisBiaya.KEGIATAN);

            foreach (int itemSiswa in lstKdSiswa)
            {
                int KdSiswa = itemSiswa;

                //--- Header Jurnal
                inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                Jurnal.KdJurnal = "";
                Jurnal.Deskripsi = "Jurnal Dana Tahunan Periode " + aPeriode;
                Jurnal.Tgl = aTglTerbit;
                Jurnal.StatusPosting = false;
                Jurnal.Sumber = this.Name;
                Jurnal.JenisJurnal = inovaGL.Data.AdnJurnalVar.JenisJurnal.JDT_KEW_PDP;
                Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();
                Jurnal.KdSiswa =KdSiswa;
                Jurnal.KdSekolah = aKdSekolah;
                Jurnal.Nis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(KdSiswa).NIS;
                Jurnal.ThAjar = this.ThAjar;
                Jurnal.ThAjarTagihan = this.ThAjar;

                //--- END --- Header Jurnal
                int NoUrutDebet = 0;
                int NoUrutKredit = 100;
                //decimal Total = 0;

                foreach (EDUSIS.Biaya.AdnBiayaSekolah item in lstBiaya)
                {
                    
                    //--- Detail Jurnal Debet
                    inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = item.oBiaya.KdAkunKewajiban;
                    decimal JmhBiaya = item.Jmh - -new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetPotongan(aKdSekolah, this.ThAjar, item.KdBiaya, Jurnal.Nis);
                    JurnalDtl.Debet = JmhBiaya/12;
                    JurnalDtl.Kredit = 0;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = Jurnal.Deskripsi;
                    JurnalDtl.NoUrut = NoUrutDebet;

                    NoUrutDebet++;
                    //--- END --- Detail Jurnal Debet

                    Jurnal.ItemDf.Add(JurnalDtl);

                    //--- Detail Jurnal Kredit
                    JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = item.oBiaya.KdAkunPendapatan;
                    JurnalDtl.Debet = 0;
                    JurnalDtl.Kredit = JmhBiaya / 12;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = Jurnal.Deskripsi;
                    JurnalDtl.NoUrut = NoUrutKredit;

                    NoUrutKredit++;
                    //--- END --- Detail Jurnal Kredit

                    Jurnal.ItemDf.Add(JurnalDtl);

                }
                // Simpan Data
                SqlTransaction Trans = null;
                try
                {
                    Trans = this.cnn.BeginTransaction();

                    //AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
                    inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
                    //dao.Simpan(o);

                    //Jurnal.KdJurnal = o.KdTagihan;
                    daoJurnal.Simpan(Jurnal);

                    Trans.Commit();
                    //MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exp)
                {
                    AdnFungsi.LogErr(exp.Message);
                }

                // --- END --- Simpan Data

            }

        }

        private void buttonProsesJurnalDanaPengembangan_Click(object sender, EventArgs e)
        {
            string Periode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            DateTime TglTerbit = dateTimePickerTgl.Value;

            string KdSekolah = AdnFungsi.CStr(dgvSekolah.CurrentRow.Cells["KolomKdSekolah"]);

            foreach (DataGridViewRow row in dgvKelas.Rows)
            {
                if (AdnFungsi.CBool(row.Cells["PilihanKelas"], true))
                {
                    string Kelas = AdnFungsi.CStr(row.Cells["Kelas"]);
                    this.JurnalDanaPengembangan2(KdSekolah, Kelas, TglTerbit, Periode);
                    MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private void JurnalDanaPengembangan2(string aKdSekolah, string aKelas, DateTime aTglTerbit, string aPeriode)
        {

            int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(aKdSekolah).Tingkat;
            int MasaBelajar = 1;
            switch (Tingkat)
            {
                case 0:
                    MasaBelajar = 12;
                    break;
                case 1:
                    MasaBelajar = 72;
                    break;
                case 2:
                    MasaBelajar = 36;
                    break;
                case 3:
                    MasaBelajar = 36;
                    break;
            }

            string Prefix = new AdnTagihanSiswaDao(this.cnn).GetPrefixTagihan(Tingkat);
            List<int> lstKdSiswa = new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).GetKdSiswa(aKdSekolah, this.ThAjar, aKelas);

            string TkKelas = new EDUSIS.Kelas.AdnKelasDao(this.cnn).Get(aKelas).Tingkat;
            List<EDUSIS.Biaya.AdnBiayaSekolah> lstBiaya = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetByJenis(aKdSekolah, this.ThAjar, TkKelas, EDUSIS.Shared.EdusisVar.JenisBiaya.PSB);

            foreach (int itemSiswa in lstKdSiswa)
            {
                int KdSiswa = itemSiswa;

                //--- Header Jurnal
                inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                Jurnal.KdJurnal = "";
                Jurnal.Deskripsi = "Jurnal Dana Pengembangan Periode " + aPeriode;
                Jurnal.Tgl = aTglTerbit;
                Jurnal.StatusPosting = false;
                Jurnal.Sumber = this.Name;
                Jurnal.JenisJurnal = inovaGL.Data.AdnJurnalVar.JenisJurnal.JDP_KEW_PDP;
                Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();
                Jurnal.KdSiswa = KdSiswa;
                Jurnal.KdSekolah = aKdSekolah;
                Jurnal.Nis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(KdSiswa).NIS;
                Jurnal.ThAjar = this.ThAjar;
                Jurnal.ThAjarTagihan = this.ThAjar;

                //--- END --- Header Jurnal
                int NoUrutDebet = 0;
                int NoUrutKredit = 100;
                //decimal Total = 0;

                foreach (EDUSIS.Biaya.AdnBiayaSekolah item in lstBiaya)
                {

                    //--- Detail Jurnal Debet
                    inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = item.oBiaya.KdAkunKewajiban;
                    decimal JmhBiaya = item.Jmh - -new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetPotongan(aKdSekolah, this.ThAjar, item.KdBiaya, Jurnal.Nis);
                    JurnalDtl.Debet = JmhBiaya / MasaBelajar;
                    JurnalDtl.Kredit = 0;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = Jurnal.Deskripsi;
                    JurnalDtl.NoUrut = NoUrutDebet;

                    NoUrutDebet++;
                    //--- END --- Detail Jurnal Debet

                    Jurnal.ItemDf.Add(JurnalDtl);

                    //--- Detail Jurnal Kredit
                    JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = item.oBiaya.KdAkunPendapatan;
                    JurnalDtl.Debet = 0;
                    JurnalDtl.Kredit = JmhBiaya / MasaBelajar;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = Jurnal.Deskripsi;
                    JurnalDtl.NoUrut = NoUrutKredit;

                    NoUrutKredit++;
                    //--- END --- Detail Jurnal Kredit

                    Jurnal.ItemDf.Add(JurnalDtl);

                }
                // Simpan Data
                SqlTransaction Trans = null;
                try
                {
                    Trans = this.cnn.BeginTransaction();

                    //AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
                    inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
                    //dao.Simpan(o);

                    //Jurnal.KdJurnal = o.KdTagihan;
                    daoJurnal.Simpan(Jurnal);

                    Trans.Commit();
                    //MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exp)
                {
                    AdnFungsi.LogErr(exp.Message);
                }

                // --- END --- Simpan Data

            }

        }

        private void buttonProsesAwalTahunAjarJurnalDanaPengembangan_Click(object sender, EventArgs e)
        {
            string Periode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            DateTime TglTerbit = dateTimePickerTgl.Value;

            string KdSekolah = AdnFungsi.CStr(dgvSekolah.CurrentRow.Cells["KolomKdSekolah"]);

            foreach (DataGridViewRow row in dgvKelas.Rows)
            {
                if (AdnFungsi.CBool(row.Cells["PilihanKelas"], true))
                {
                    string Kelas = AdnFungsi.CStr(row.Cells["Kelas"]);
                    this.JurnalDanaPengembangan_DiAwalThAjaran(KdSekolah, Kelas, TglTerbit, Periode);
                    MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private void buttonProsesAwalTahunAjarJurnalDanaTahunan_Click(object sender, EventArgs e)
        {
            string Periode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            DateTime TglTerbit = dateTimePickerTgl.Value;

            string KdSekolah = AdnFungsi.CStr(dgvSekolah.CurrentRow.Cells["KolomKdSekolah"]);

            foreach (DataGridViewRow row in dgvKelas.Rows)
            {
                if (AdnFungsi.CBool(row.Cells["PilihanKelas"], true))
                {
                    string Kelas = AdnFungsi.CStr(row.Cells["Kelas"]);
                    this.JurnalDanaTahunan_DiAwalThAjaran(KdSekolah, Kelas, TglTerbit, Periode);
                    MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private void JurnalDanaPengembangan_DiAwalThAjaran(string aKdSekolah, string aKelas, DateTime aTglTerbit, string aPeriode)
        {

            int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(aKdSekolah).Tingkat;
            string Prefix = new AdnTagihanSiswaDao(this.cnn).GetPrefixTagihan(Tingkat);
            List<int> lstKdSiswa = new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).GetKdSiswa(aKdSekolah, this.ThAjar, aKelas);

            string TingkatKelas = new EDUSIS.Kelas.AdnKelasDao(this.cnn).Get(aKelas).Tingkat;
            List<EDUSIS.Biaya.AdnBiayaSekolah> lstBiaya = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetByJenis(aKdSekolah, this.ThAjar, TingkatKelas, EDUSIS.Shared.EdusisVar.JenisBiaya.PSB);

            foreach (int itemSiswa in lstKdSiswa)
            {
                int KdSiswa = itemSiswa;

                //--- Header Jurnal
                inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                Jurnal.KdJurnal = "";
                Jurnal.Deskripsi = "Jurnal Dana Pengembangan [Awal Tahun] Periode " + aPeriode;
                Jurnal.Tgl = aTglTerbit;
                Jurnal.StatusPosting = false;
                Jurnal.Sumber = this.Name;
                Jurnal.JenisJurnal = inovaGL.Data.AdnJurnalVar.JenisJurnal.JDP_PIU_KEW;
                Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();
                Jurnal.KdSiswa = KdSiswa;
                Jurnal.KdSekolah = aKdSekolah;
                Jurnal.Nis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(KdSiswa).NIS;
                Jurnal.ThAjar = this.ThAjar;
                Jurnal.ThAjarTagihan = this.ThAjar;

                //--- END --- Header Jurnal
                int NoUrutDebet = 0;
                int NoUrutKredit = 100;
                //decimal Total = 0;

                foreach (EDUSIS.Biaya.AdnBiayaSekolah item in lstBiaya)
                {

                    //--- Detail Jurnal Debet
                    inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = item.oBiaya.KdAkunPiutang;
                    JurnalDtl.Debet = item.Jmh;
                    JurnalDtl.Kredit = 0;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = Jurnal.Deskripsi;
                    JurnalDtl.NoUrut = NoUrutDebet;

                    NoUrutDebet++;
                    //--- END --- Detail Jurnal Debet

                    Jurnal.ItemDf.Add(JurnalDtl);

                    //--- Detail Jurnal Kredit
                    JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = item.oBiaya.KdAkunKewajiban;
                    JurnalDtl.Debet = 0;
                    JurnalDtl.Kredit = item.Jmh;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = Jurnal.Deskripsi;
                    JurnalDtl.NoUrut = NoUrutKredit;

                    NoUrutKredit++;
                    //--- END --- Detail Jurnal Kredit

                    Jurnal.ItemDf.Add(JurnalDtl);

                }
                // Simpan Data
                SqlTransaction Trans = null;
                try
                {
                    Trans = this.cnn.BeginTransaction();

                    //AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
                    inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
                    //dao.Simpan(o);

                    //Jurnal.KdJurnal = o.KdTagihan;
                    daoJurnal.Simpan(Jurnal);

                    Trans.Commit();
                    //MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exp)
                {
                    AdnFungsi.LogErr(exp.Message);
                }

                // --- END --- Simpan Data

            }

        }

        private void JurnalDanaTahunan_DiAwalThAjaran(string aKdSekolah, string aKelas, DateTime aTglTerbit, string aPeriode)
        {

            int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(aKdSekolah).Tingkat;
            string Prefix = new AdnTagihanSiswaDao(this.cnn).GetPrefixTagihan(Tingkat);
            List<int> lstKdSiswa = new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).GetKdSiswa(aKdSekolah, this.ThAjar, aKelas);

            string TingkatKelas = new EDUSIS.Kelas.AdnKelasDao(this.cnn).Get(aKelas).Tingkat;

            List<EDUSIS.Biaya.AdnBiayaSekolah> lstBiaya = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetByJenis(aKdSekolah, this.ThAjar, TingkatKelas, EDUSIS.Shared.EdusisVar.JenisBiaya.KEGIATAN);

            foreach (int itemSiswa in lstKdSiswa)
            {
                int KdSiswa = itemSiswa;

                //--- Header Jurnal
                inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                Jurnal.KdJurnal = "";
                Jurnal.Deskripsi = "Jurnal Dana Tahunan [Awal Tahun] Periode " + aPeriode;
                Jurnal.Tgl = aTglTerbit;
                Jurnal.StatusPosting = false;
                Jurnal.Sumber = this.Name;
                Jurnal.JenisJurnal = inovaGL.Data.AdnJurnalVar.JenisJurnal.JDT_PIU_KEW;
                Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();
                Jurnal.KdSiswa = KdSiswa;
                Jurnal.KdSekolah = aKdSekolah;
                Jurnal.Nis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(KdSiswa).NIS;
                Jurnal.ThAjar = this.ThAjar;
                Jurnal.ThAjarTagihan = this.ThAjar;

                //--- END --- Header Jurnal
                int NoUrutDebet = 0;
                int NoUrutKredit = 100;
                //decimal Total = 0;

                foreach (EDUSIS.Biaya.AdnBiayaSekolah item in lstBiaya)
                {

                    //--- Detail Jurnal Debet
                    inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = item.oBiaya.KdAkunPiutang;
                    JurnalDtl.Debet = item.Jmh;
                    JurnalDtl.Kredit = 0;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = Jurnal.Deskripsi;
                    JurnalDtl.NoUrut = NoUrutDebet;

                    NoUrutDebet++;
                    //--- END --- Detail Jurnal Debet

                    Jurnal.ItemDf.Add(JurnalDtl);

                    //--- Detail Jurnal Kredit
                    JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = item.oBiaya.KdAkunKewajiban;
                    JurnalDtl.Debet = 0;
                    JurnalDtl.Kredit = item.Jmh ;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = Jurnal.Deskripsi;
                    JurnalDtl.NoUrut = NoUrutKredit;

                    NoUrutKredit++;
                    //--- END --- Detail Jurnal Kredit

                    Jurnal.ItemDf.Add(JurnalDtl);

                }
                // Simpan Data
                SqlTransaction Trans = null;
                try
                {
                    Trans = this.cnn.BeginTransaction();

                    //AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
                    inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
                    //dao.Simpan(o);

                    //Jurnal.KdJurnal = o.KdTagihan;
                    daoJurnal.Simpan(Jurnal);

                    Trans.Commit();
                    //MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exp)
                {
                    AdnFungsi.LogErr(exp.Message);
                }

                // --- END --- Simpan Data

            }

        }

        private void buttonPenyesuaian_Click(object sender, EventArgs e)
        {
            string Periode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            DateTime TglTerbit = dateTimePickerTgl.Value;

            string KdSekolah = AdnFungsi.CStr(dgvSekolah.CurrentRow.Cells["KolomKdSekolah"]);

            foreach (DataGridViewRow row in dgvKelas.Rows)
            {
                if (AdnFungsi.CBool(row.Cells["PilihanKelas"], true))
                {
                    string Kelas = AdnFungsi.CStr(row.Cells["Kelas"]);
                    this.JurnalPenyesuaian_DiAwalThAjaran(KdSekolah, Kelas, TglTerbit, Periode);
                    MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void JurnalPenyesuaian_DiAwalThAjaran(string aKdSekolah, string aKelas, DateTime aTglTerbit, string aPeriode)
        {

            int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(aKdSekolah).Tingkat;
            string Prefix = new AdnTagihanSiswaDao(this.cnn).GetPrefixTagihan(Tingkat);
            List<int> lstKdSiswa = new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).GetKdSiswa(aKdSekolah, this.ThAjar, aKelas);

            string TingkatKelas = new EDUSIS.Kelas.AdnKelasDao(this.cnn).Get(aKelas).Tingkat;
            
            //Pilih Transaksi Pembayaran pada masa PSB
            //List<EDUSIS.Biaya.AdnBiayaSekolah> lstBiaya = new EDUSIS.Biaya.AdnBiayaSekolahDao(this.cnn).GetByJenis(aKdSekolah, this.ThAjar, TingkatKelas, EDUSIS.Shared.EdusisVar.JenisBiaya.PSB);

            foreach (int itemSiswa in lstKdSiswa)
            {
                int KdSiswa = itemSiswa;

                //--- Header Jurnal
                inovaGL.Data.AdnJurnal Jurnal = new inovaGL.Data.AdnJurnal();
                Jurnal.KdJurnal = "";
                Jurnal.Deskripsi = "Jurnal Dana Pengembangan [Penyesuaian] " + aPeriode;
                Jurnal.Tgl = aTglTerbit;
                Jurnal.StatusPosting = false;
                Jurnal.Sumber = this.Name;
                Jurnal.JenisJurnal = inovaGL.Data.AdnJurnalVar.JenisJurnal.JDT_DEP_PIU;
                Jurnal.ItemDf = new List<inovaGL.Data.AdnJurnalDtl>();
                Jurnal.KdSiswa = KdSiswa;
                Jurnal.KdSekolah = aKdSekolah;
                Jurnal.Nis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(KdSiswa).NIS;
                Jurnal.ThAjar = this.ThAjar;
                Jurnal.ThAjarTagihan = this.ThAjar;

                //--- END --- Header Jurnal
                int NoUrutDebet = 0;
                int NoUrutKredit = 100;
                //decimal Total = 0;


                DataTable lstPembayaran = new inovaGL.Data.AdnLoketPSBDao(this.cnn).GetByNis(aKdSekolah, this.ThAjar, Jurnal.Nis);
                for (int i = 0; i < lstPembayaran.Rows.Count; i++)
                {

                    //--- Detail Jurnal Debet
                    inovaGL.Data.AdnJurnalDtl JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = AdnFungsi.CStr(lstPembayaran.Rows[i]["KdAkunDeposit"]);
                    JurnalDtl.Debet = AdnFungsi.CDec(lstPembayaran.Rows[i]["jmh"]);
                    JurnalDtl.Kredit = 0;
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = Jurnal.Deskripsi;
                    JurnalDtl.NoUrut = NoUrutDebet;

                    NoUrutDebet++;
                    //--- END --- Detail Jurnal Debet

                    Jurnal.ItemDf.Add(JurnalDtl);

                    //--- Detail Jurnal Kredit
                    JurnalDtl = new inovaGL.Data.AdnJurnalDtl();
                    JurnalDtl.KdJurnal = Jurnal.KdJurnal;
                    JurnalDtl.KdAkun = AdnFungsi.CStr(lstPembayaran.Rows[i]["KdAkunPiutang"]);
                    JurnalDtl.Debet = 0;
                    JurnalDtl.Kredit = AdnFungsi.CDec(lstPembayaran.Rows[i]["jmh"]);
                    JurnalDtl.KdDept = Jurnal.KdSekolah;
                    JurnalDtl.Memo = Jurnal.Deskripsi;
                    JurnalDtl.NoUrut = NoUrutKredit;

                    NoUrutKredit++;
                    //--- END --- Detail Jurnal Kredit

                    Jurnal.ItemDf.Add(JurnalDtl);

                }
                // Simpan Data
                SqlTransaction Trans = null;
                try
                {
                    Trans = this.cnn.BeginTransaction();

                    //AdnTagihanSiswaDao dao = new AdnTagihanSiswaDao(this.cnn, this.Pengguna, Trans);
                    inovaGL.Data.AdnJurnalDao daoJurnal = new inovaGL.Data.AdnJurnalDao(this.cnn, this.Pengguna, Trans);
                    //dao.Simpan(o);

                    //Jurnal.KdJurnal = o.KdTagihan;
                    daoJurnal.Simpan(Jurnal);

                    Trans.Commit();
                    //MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exp)
                {
                    AdnFungsi.LogErr(exp.Message);
                }

                // --- END --- Simpan Data

            }

        }

        private void comboBoxBulan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBulan.SelectedIndex > -1)
            {
                dateTimePickerTgl.Value = new DateTime((int)numericUpDownTahun.Value, comboBoxBulan.SelectedIndex + 1, 1);
            }
        }

        private void dateTimePickerTgl_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDownTahun_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxBulan.SelectedIndex > -1)
            {
                dateTimePickerTgl.Value = new DateTime((int)numericUpDownTahun.Value, comboBoxBulan.SelectedIndex + 1, 1);
            }
        }


    }
}
