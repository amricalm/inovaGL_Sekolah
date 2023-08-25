using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Globalization;
using Andhana;

namespace EDUSIS.KeuanganTagihan
{
    public class AdnTagihanSiswaDtlDao
    {
        private const short JUMLAH_KOLOM = 5;
        private const string NAMA_TABEL = "ku_tagihan_siswa_dtl";
        
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

        public AdnTagihanSiswaDtlDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnTagihanSiswaDtlDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnTagihanSiswaDtlDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnTagihanSiswaDtl o)
        {
            short idx = 0;
            CultureInfo culture = new CultureInfo("en-US");

            fld[idx] = "kd_tagihan"; nilai[idx] = o.KdTagihan.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "kd_dtl"; nilai[idx] = o.KdDtl.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kd_biaya"; nilai[idx] = o.KdBiaya.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "jmh"; nilai[idx] = o.Jmh.ToString("G",culture); tipe[idx] = "n"; idx++;
            fld[idx] = "item_bulan"; nilai[idx] = o.ItemBulan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "ket"; nilai[idx] = o.Keterangan.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnTagihanSiswaDtl o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai,tipe,"",true);
            cmd.CommandText = sql;
            o.KdDtl = (int)cmd.ExecuteScalar();

            foreach (AdnTagihanSiswaDtlPeriode item in o.DfPeriode)
            {
                item.KdDtl = o.KdDtl;
                new AdnTagihanSiswaDtlPeriodeDao(this.cnn, this.pengguna, this.trn).Simpan(item);
            }
        }
        public void Update(AdnTagihanSiswaDtl o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdTagihan + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
        }
        public void Hapus(string kd)
        {

            sWhere = this.pkey + "='" + kd + "'";
            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
        }

        public List<AdnTagihanSiswaDtl> Get(string KdSaw)
        {
            List<AdnTagihanSiswaDtl> lst = new List<AdnTagihanSiswaDtl>();
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where kd_tagihan = '" + KdSaw + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnTagihanSiswaDtl o = new AdnTagihanSiswaDtl();
                    o.KdTagihan = KdSaw;
                    o.KdDtl = AdnFungsi.CInt(rdr["kd_dtl"],true);
                    o.KdBiaya = AdnFungsi.CStr(rdr["kd_biaya"]);
                    o.Jmh = AdnFungsi.CDec(rdr["jmh"]);
                    o.ItemBulan = AdnFungsi.CStr(rdr["item_bulan"]);
                    o.Keterangan = AdnFungsi.CStr(rdr["ket"]);
                    lst.Add(o);
                }
                rdr.Close();
                foreach (AdnTagihanSiswaDtl item in lst)
                {
                    item.DfPeriode = new AdnTagihanSiswaDtlPeriodeDao(this.cnn).Get(item.KdDtl);
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
