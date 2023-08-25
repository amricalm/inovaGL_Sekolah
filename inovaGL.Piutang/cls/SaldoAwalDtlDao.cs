using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace inovaGL.Piutang
{
    public class AdnSaldoAwalDtlDao
    {
        private const short JUMLAH_KOLOM = 5;
        private const string NAMA_TABEL = "ac_saldo_piutang_siswa_dtl";
        
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

        public AdnSaldoAwalDtlDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnSaldoAwalDtlDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnSaldoAwalDtlDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnSaldoAwalDtl o)
        {
            short idx = 0;

            fld[idx] = "kd_saw"; nilai[idx] = o.KdSaldoAwal.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "kd_dtl"; nilai[idx] = o.KdDtl.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kd_biaya"; nilai[idx] = o.KdBiaya.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "jmh"; nilai[idx] = o.Jmh.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "item_bulan"; nilai[idx] = o.ItemBulan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "ket"; nilai[idx] = o.Keterangan.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnSaldoAwalDtl o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe, "", true);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            foreach (AdnSaldoAwalDtlPeriode item in o.DfPeriode)
            {
                item.KdDtl = o.KdDtl;
                new AdnSaldoAwalDtlPeriodeDao(this.cnn, this.pengguna, this.trn).Simpan(item);
            }
        }
        public void Update(AdnSaldoAwalDtl o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdSaldoAwal + "'";
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

        public List<AdnSaldoAwalDtl> Get(string KdSaw)
        {
            List<AdnSaldoAwalDtl> lst = new List<AdnSaldoAwalDtl>();
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where kd_saw = '" + KdSaw + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnSaldoAwalDtl o = new AdnSaldoAwalDtl();
                    o.KdSaldoAwal = KdSaw;
                    o.KdDtl = AdnFungsi.CInt(rdr["kd_dtl"],true);
                    o.KdBiaya = AdnFungsi.CStr(rdr["kd_biaya"]);
                    o.Jmh = AdnFungsi.CDec(rdr["jmh"]);
                    o.ItemBulan = AdnFungsi.CStr(rdr["item_bulan"]);
                    o.Keterangan = AdnFungsi.CStr(rdr["ket"]);
                    lst.Add(o);
                }
                rdr.Close();
                foreach (AdnSaldoAwalDtl item in lst)
                {
                    item.DfPeriode = new AdnSaldoAwalDtlPeriodeDao(this.cnn).Get(item.KdDtl);
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
