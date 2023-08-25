using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaGL
{
    public class AdnJurnalUmumDao
    {
        private const short JUMLAH_KOLOM = 4;
        private const string NAMA_TABEL = "ac_tju";
        
        private string pkey = "kd_tju";
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

        public AdnJurnalUmumDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnJurnalUmumDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnJurnalUmumDao(SqlConnection cnn, AdnScPengguna pengguna,SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
            this.trn = trn;
        }

        private void SetFldNilai(AdnJurnalUmum o)
        {
            short idx = 0;

            fld[idx] = "kd_tju"; nilai[idx] = o.KdJU.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl"; nilai[idx] = o.Tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "deskripsi"; nilai[idx] = o.Deskripsi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_jurnal"; nilai[idx] = o.KdJurnal.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnJurnalUmum o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                foreach (AdnJurnalUmumDtl item in o.ItemDf)
                {
                    new AdnJurnalUmumDtlDao(this.cnn, this.pengguna, this.trn).Simpan(item);
                }

            }
            catch(DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
        }
        public void Update(AdnJurnalUmum o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdJU + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                new AdnJurnalUmumDtlDao(this.cnn,this.pengguna,this.trn).Hapus(o.KdJU);

                foreach (AdnJurnalUmumDtl item in o.ItemDf)
                {
                    new AdnJurnalUmumDtlDao(this.cnn, this.pengguna, this.trn).Simpan(item);
                }

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

        public AdnJurnalUmum Get(string kd)
        {
            AdnJurnalUmum o = null;
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + kd + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnJurnalUmum();
                    o.KdJU = kd;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.KdJurnal = AdnFungsi.CStr(rdr["kd_jurnal"]);
                }
                rdr.Close();
                o.ItemDf = new AdnJurnalUmumDtlDao(this.cnn).Get(kd);
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnJurnalUmum> GetAll()
        {
            List<AdnJurnalUmum> lst = new List<AdnJurnalUmum>();
            sql =
            " select kd_tju, tgl,  deskripsi "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnJurnalUmum o = new AdnJurnalUmum();
                    o.KdJU = AdnFungsi.CStr(rdr["kd_tju"]) ;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.KdJurnal = AdnFungsi.CStr(rdr["kd_jurnal"]);
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch(DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            return lst; 
        }
        public DataTable GetByArgs(string sFilter)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdJU", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Kepada", typeof(String));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdJurnal", typeof(String));

            string sql =
            " select kd_tju, tgl, deskripsi, kd_jurnal "
            + " from " + NAMA_TABEL;

            if (sFilter != "")
            {
                sql = sql + " WHERE " + sFilter;
            }

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdJU"] = AdnFungsi.CStr(rdr["kd_tju"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                baris["KdJurnal"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        
    }
}
