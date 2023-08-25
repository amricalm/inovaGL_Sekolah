using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace EDUSIS.KeuanganTagihan
{
    public class AdnTagihanSiswaDtlPeriodeDao
    {
        private const short JUMLAH_KOLOM = 2;
        private const string NAMA_TABEL = "ku_tagihan_siswa_dtl_periode";
        
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

        public AdnTagihanSiswaDtlPeriodeDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnTagihanSiswaDtlPeriodeDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnTagihanSiswaDtlPeriodeDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnTagihanSiswaDtlPeriode o)
        {
            short idx = 0;

            fld[idx] = "periode"; nilai[idx] = o.Periode.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_dtl"; nilai[idx] = o.KdDtl.ToString(); tipe[idx] = "n"; idx++;
        }

        public void Simpan(AdnTagihanSiswaDtlPeriode o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai,tipe);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();               
            }
            catch(DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
        }
        public void Update(AdnTagihanSiswaDtlPeriode o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdDtl + "'";
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
        public void Hapus(int KdDtl)
        {

            sWhere = this.pkey + "=" + KdDtl ;
            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
        }

        public List<AdnTagihanSiswaDtlPeriode> Get(int KdDtl)
        {
            List<AdnTagihanSiswaDtlPeriode> lst = new List<AdnTagihanSiswaDtlPeriode>();
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
                    AdnTagihanSiswaDtlPeriode o = new AdnTagihanSiswaDtlPeriode();
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
}
