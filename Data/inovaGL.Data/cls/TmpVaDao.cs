using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaGL.Data
{
    public class AdnTmpVaDao
    {
        private const short JUMLAH_KOLOM = 1;
        private const string NAMA_TABEL = "ac_tmp_va";
        private string pkey = "kd";

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

        public AdnTmpVaDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnTmpVaDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnTmpVaDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
            this.trn = trn;

        }

        private void SetFldNilai(AdnTmpVa o)
        {
            short idx = 0;
            //fld[idx] = "kd"; nilai[idx] = o.Kd.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "nm_file"; nilai[idx] = o.NmFile.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnTmpVa o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login,true);

            cmd.CommandText = sql;
            o.Kd = (Int32)cmd.ExecuteScalar(); 

            foreach (AdnTmpVaDtl item in o.ItemDf)
            {
                item.Kd = o.Kd;
                new AdnTmpVaDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
            }
        }
        public void Update(AdnTmpVa o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "=" + o.Kd;
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            new AdnTmpVaDtlDao(this.cnn,this.pengguna,this.trn).Hapus(o.Kd);

            foreach (AdnTmpVaDtl item in o.ItemDf)
            {
                new AdnTmpVaDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
            }
        }
        public void Hapus(long kd)
        {
            sWhere = this.pkey + "=" + kd;
            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            cmd.ExecuteNonQuery();
        }
        public AdnTmpVa Get(long kd)
        {
            AdnTmpVa o = null;
            string sql =
            " select kd,nm_file"
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " =" + kd;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnTmpVa();
                    o.Kd = kd;
                    o.NmFile = AdnFungsi.CStr(rdr["nm_file"]);
                    o.ItemDf = new List<AdnTmpVaDtl>();
                }
                rdr.Close();
                if (o != null)
                {
                    o.ItemDf = new AdnTmpVaDtlDao(this.cnn).Get(kd);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
            return o;
        }
        public List<AdnTmpVa> GetAll()
        {
            List<AdnTmpVa> lst = new List<AdnTmpVa>();
            string sql =
            " select kd, nm_file "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnTmpVa o = new AdnTmpVa();
                    o.Kd = Convert.ToInt64(rdr["kd"]);
                    o.NmFile = AdnFungsi.CStr(rdr["nm_file"]);
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
        public DataTable GetAll2()
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdPos", typeof(String));
            tbl.Columns.Add("NmPos", typeof(String));

            string sql =
            " select kd, nm_file "
            + " from " + NAMA_TABEL + "";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["Kd"] = Convert.ToInt64(rdr["kd"]);
                baris["NmFile"] = AdnFungsi.CStr(rdr["nm_file"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl;
        }
        
        
    }
}
