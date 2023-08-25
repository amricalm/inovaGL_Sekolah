using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace EDUSIS.KeuanganPembayaran
{
    public class AdnPembayaranDtlPeriodeDao
    {
        private const short JUMLAH_KOLOM = 2;
        private const string NAMA_TABEL = "ku_pembayaran_dtl_periode";
        
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

        public AdnPembayaranDtlPeriodeDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnPembayaranDtlPeriodeDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnPembayaranDtlPeriodeDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnPembayaranDtlPeriode o)
        {
            short idx = 0;

            fld[idx] = "periode"; nilai[idx] = o.Periode.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_dtl"; nilai[idx] = o.KdDtl.ToString(); tipe[idx] = "n"; idx++;
        }

        public void Simpan(AdnPembayaranDtlPeriode o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai,tipe);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();               
        }

        public List<AdnPembayaranDtlPeriode> Get(int KdDtl)
        {
            List<AdnPembayaranDtlPeriode> lst = new List<AdnPembayaranDtlPeriode>();
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where kd_dtl = " + KdDtl ;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPembayaranDtlPeriode o = new AdnPembayaranDtlPeriode();
                    o.KdDtl  = KdDtl;
                    o.Periode = AdnFungsi.CStr(rdr["periode"]);
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            return lst;
        }
        
    }


    public class AdnLoketDtlPeriodeDao
    {
        private const short JUMLAH_KOLOM = 7;
        private const string NAMA_TABEL = "tbtr_loket_sppdetail_bulan";

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

        public AdnLoketDtlPeriodeDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnLoketDtlPeriodeDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnLoketDtlPeriodeDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnLoketDtlPeriode o)
        {
            short idx = 0;

            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nis"; nilai[idx] = o.Nis.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "no_bayar"; nilai[idx] = o.NoBayar.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kd_biaya"; nilai[idx] = o.KdBiaya.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "bulan"; nilai[idx] = o.Bulan.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kd_dtl"; nilai[idx] = o.KdDtl.ToString(); tipe[idx] = "n"; idx++;
        }

        public void Simpan(AdnLoketDtlPeriode o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public List<AdnLoketDtlPeriode> Get(string ThAjar, string KdSekolah, string Nis, Int64 NoBayar, string KdBiaya)
        {
            List<AdnLoketDtlPeriode> lst = new List<AdnLoketDtlPeriode>();
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where th_ajar  = '" + ThAjar + "'"
            + " and kd_sekolah = '" + KdSekolah + "'"
            + " and nis = '" + Nis + "'"
            + " and no_bayar = " + NoBayar
            + " and kd_biaya = '" + KdBiaya + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnLoketDtlPeriode o = new AdnLoketDtlPeriode();
                    o.ThAjar = ThAjar;
                    o.KdSekolah = KdSekolah;
                    o.Nis = Nis;
                    o.NoBayar = NoBayar;
                    o.KdBiaya = KdBiaya;
                    o.Bulan = AdnFungsi.CInt(rdr["bulan"], true);
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            return lst;
        }

    }

}
