using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Globalization;
using Andhana;

namespace EDUSIS.KeuanganPembayaran
{
    public class AdnPembayaranDtlDao
    {
        private const short JUMLAH_KOLOM = 6;
        private const string NAMA_TABEL = "ku_pembayaran_dtl";
        
        private string pkey = "kd_dtl";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private SqlTransaction trn;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnPembayaranDtlDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnPembayaranDtlDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnPembayaranDtlDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnPembayaranDtl o)
        {
            short idx = 0;
            CultureInfo culture = new CultureInfo("en-US");

            fld[idx] = "kd_kwitansi"; nilai[idx] = o.KdKwitansi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_tagihan"; nilai[idx] = o.KdTagihan.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "kd_dtl"; nilai[idx] = o.KdDtl.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kd_biaya"; nilai[idx] = o.KdBiaya.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "jmh"; nilai[idx] = o.Jmh.ToString("G",culture); tipe[idx] = "n"; idx++;
            fld[idx] = "item_bulan"; nilai[idx] = o.ItemBulan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "ket"; nilai[idx] = o.Keterangan.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnPembayaranDtl o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai,tipe,"",true);
            cmd.CommandText = sql;
            o.KdDtl = (int)cmd.ExecuteScalar();

            foreach (AdnPembayaranDtlPeriode item in o.DfPeriode)
            {
                item.KdDtl = o.KdDtl;
                new AdnPembayaranDtlPeriodeDao(this.cnn, this.pengguna, this.trn).Simpan(item);
            }
        }
        public void Hapus(string KdKwitansi)
        {

            sWhere = "kd_kwitansi ='" + KdKwitansi + "'";
            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public List<AdnPembayaranDtl> Get(string KdKwitansi)
        {
            List<AdnPembayaranDtl> lst = new List<AdnPembayaranDtl>();
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where kd_kwitansi = '" + KdKwitansi + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPembayaranDtl o = new AdnPembayaranDtl();
                    o.KdKwitansi = KdKwitansi;
                    o.KdTagihan = AdnFungsi.CStr(rdr["kd_tagihan"]);
                    o.KdDtl = AdnFungsi.CInt(rdr["kd_dtl"],true);
                    o.KdBiaya = AdnFungsi.CStr(rdr["kd_biaya"]);
                    o.Jmh = AdnFungsi.CDec(rdr["jmh"]);
                    o.ItemBulan = AdnFungsi.CStr(rdr["item_bulan"]);
                    o.Keterangan = AdnFungsi.CStr(rdr["ket"]);
                    lst.Add(o);
                }
                rdr.Close();
                foreach (AdnPembayaranDtl item in lst)
                {
                    item.DfPeriode = new AdnPembayaranDtlPeriodeDao(this.cnn).Get(item.KdDtl);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            return lst;
        }
        
    }

    public class AdnLoketDtlDao
    {
        private const short JUMLAH_KOLOM = 16;
        private const string NAMA_TABEL = "tbtr_loket_detail";

        private string pkey = "";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private SqlTransaction trn;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnLoketDtlDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnLoketDtlDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnLoketDtlDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnLoketDtl o)
        {
            short idx = 0;
            CultureInfo culture = new CultureInfo("en-US");

            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nis"; nilai[idx] = o.Nis.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "no_bayar"; nilai[idx] = o.NoBayar.ToString(); tipe[idx] = "n"; idx++;

            fld[idx] = "kd_biaya"; nilai[idx] = o.KdBiaya.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tanggal"; nilai[idx] = o.Tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "tag"; nilai[idx] = o.Tag.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "rupiah"; nilai[idx] = o.Jmh.ToString("G", culture); tipe[idx] = "n"; idx++;
            fld[idx] = "diskon"; nilai[idx] = o.Diskon.ToString("G", culture); tipe[idx] = "n"; idx++;
            fld[idx] = "item_bulan"; nilai[idx] = o.ItemBulan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "jmh_bulan"; nilai[idx] = o.JmhBulan.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "potongan"; nilai[idx] = o.Potongan.ToString("G", culture); tipe[idx] = "n"; idx++;

            fld[idx] = "ket"; nilai[idx] = o.Ket.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "ket_kd_biaya"; nilai[idx] = o.KetKdBiaya.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "qty"; nilai[idx] = o.Qty.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "rupiah_satuan"; nilai[idx] = o.JmhSatuan.ToString(); tipe[idx] = "n"; idx++;

        }

        public void Simpan(AdnLoketDtl o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe, "", true);
            sql = sql + ";SELECT SCOPE_IDENTITY();";
            cmd.CommandText = sql;
            o.KdDtl = (int)cmd.ExecuteScalar();

            foreach (AdnLoketDtlPeriode item in o.DfPeriode)
            {
                item.NoBayar = o.NoBayar;
                item.KdDtl = o.KdDtl;
                new AdnLoketDtlPeriodeDao(this.cnn, this.pengguna, this.trn).Simpan(item);
            }
        }
        public void Hapus(string ThAjar, string KdSekolah, string Nis, int NoBayar)
        {

            sWhere = "th_ajar  = '" + ThAjar + "'"
            + " and kd_sekolah = '" + KdSekolah + "'"
            + " and nis = '" + Nis + "'"
            + " and no_bayar = " + NoBayar;

            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public List<AdnLoketDtl> Get(string ThAjar, string KdSekolah, string Nis, Int64 NoBayar)
        {
            List<AdnLoketDtl> lst = new List<AdnLoketDtl>();
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where th_ajar  = '" + ThAjar + "'"
            + " and kd_sekolah = '" + KdSekolah + "'"
            + " and nis = '" + Nis + "'"
            + " and no_bayar = " + NoBayar;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnLoketDtl o = new AdnLoketDtl();
                    o.ThAjar = ThAjar;
                    o.KdSekolah = KdSekolah;
                    o.Nis = Nis;
                    o.NoBayar = NoBayar;
                    o.KdBiaya = AdnFungsi.CStr(rdr["kd_biaya"]);
                    o.Tgl = AdnFungsi.CDate(rdr["tanggal"]);
                    o.Tag = AdnFungsi.CInt(rdr["tag"]);

                    o.Jmh = AdnFungsi.CDec(rdr["jmh"]);
                    o.Diskon = AdnFungsi.CDec(rdr["diskon"]);
                    o.ItemBulan = AdnFungsi.CStr(rdr["item_bulan"]);
                    o.JmhBulan = AdnFungsi.CInt(rdr["jmh_bulan"],true);
                    o.Potongan = AdnFungsi.CDec(rdr["potongan"]);
                    lst.Add(o);
                }
                rdr.Close();
                foreach (AdnLoketDtl item in lst)
                {
                    item.DfPeriode = new AdnLoketDtlPeriodeDao(this.cnn).Get(item.ThAjar, item.KdSekolah, item.Nis, item.NoBayar, item.KdBiaya);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            return lst;
        }

    }
}
