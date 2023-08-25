using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace inovaGL.Data
{
    public class AdnPosDtlDao
    {
        private const short JUMLAH_KOLOM = 2;
        private const string NAMA_TABEL = "ac_mpos_dtl";
        
        private string pkey = "kd_pos";
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

        public AdnPosDtlDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnPosDtlDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnPosDtlDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnPosDtl o)
        {
            short idx = 0;

            fld[idx] = "kd_pos"; nilai[idx] = o.KdPos.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_akun"; nilai[idx] = o.KdAkun.ToString(); tipe[idx] = "s"; idx++;
       }

        public void Simpan(AdnPosDtl o)
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
        public void Update(AdnPosDtl o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdPos + "'";
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

        public List<AdnPosDtl> Get(string kd)
        {
            List<AdnPosDtl> lst = new List<AdnPosDtl>();
            string sql =
            " select kd_pos,kd_akun "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + kd + "'"
            + " order by kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPosDtl o = new AdnPosDtl();
                    o.KdPos = kd;
                    o.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]);
                    lst.Add(o);
                }
                rdr.Close();
                foreach (AdnPosDtl item in lst)
                {
                    item.Akun = new AdnAkunDao(this.cnn, this.pengguna, this.trn).Get(item.KdAkun);
                }
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return lst;
        }
        
    }
}
