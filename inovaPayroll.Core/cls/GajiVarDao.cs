using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaPayroll.Core
{
    public class AdnGajiVarDao
    {
        private const short JUMLAH_KOLOM = 4;  
        private const string NAMA_TABEL = "gaji_var";
        
        private string pkey = "kd_var";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;
        private SqlTransaction trn;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnGajiVarDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnGajiVarDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }

        public AdnGajiVarDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
            this.trn = trn;

        }
        private void SetFldNilai(AdnGajiVar o)
        {
            short idx = 0;

            fld[idx] = "kd_var"; nilai[idx] = o.KdJenis.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "periode"; nilai[idx] = o.Periode.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nip"; nilai[idx] = o.Nip.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "jmh"; nilai[idx] = o.Jmh.ToString(); tipe[idx] = "n"; idx++;

            
        }

        public void Simpan(AdnGajiVar o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe);

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            
        }



        public AdnGajiVar Get(string kd)
        {
            AdnGajiVar o = null;
            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + "='" + kd.ToString().Trim() +"'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnGajiVar();
                    o.KdJenis= AdnFungsi.CStr(rdr["kd_var"]);
                    o.Periode = AdnFungsi.CStr(rdr["periode"]);
                    o.Nip = AdnFungsi.CStr(rdr["nip"]);
                    o.Jmh = AdnFungsi.CInt(rdr["jmh"],true);

                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return o;
        }
        public List<AdnGajiVar> GetAll()
        {
            List<AdnGajiVar> lst = new List<AdnGajiVar>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnGajiVar o = new AdnGajiVar();
                    o.KdJenis = AdnFungsi.CStr(rdr["kd_var"]);
                    o.Periode = AdnFungsi.CStr(rdr["periode"]);
                    o.Nip = AdnFungsi.CStr(rdr["nip"]);
                    o.Jmh = AdnFungsi.CInt(rdr["jmh"], true);
  
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return lst;
        }
        

    }
}
