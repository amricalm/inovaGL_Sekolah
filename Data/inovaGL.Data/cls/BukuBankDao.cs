using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace inovaGL
{
    public class AdnBukuBankDao
    {
        private const short JUMLAH_KOLOM = 7;
        private const string NAMA_TABEL = "ac_tbuku_bank";
        
        private string pkey = "kd";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnBukuBankDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnBukuBankDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnBukuBank o)
        {
            short idx = 0;

            fld[idx] = "kd"; nilai[idx] = o.Kd.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl"; nilai[idx] = o.Tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "deskripsi"; nilai[idx] = o.Deskripsi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "debet"; nilai[idx] = o.Debet.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kredit"; nilai[idx] = o.Kredit.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "no_kwitansi_donasi"; nilai[idx] = o.NoKwitansiDonasi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_kas"; nilai[idx] = o.KdKas.ToString(); tipe[idx] = "s"; idx++;

        }

        public void Simpan(AdnBukuBank o)
        {

            //-------------- Menentukan Kode 

            sql =
            "SELECT isnull(max(cast(kd as int)),0) as kd  "
            + " FROM " + NAMA_TABEL;

            cmd.CommandText = sql;
            long iMax = Convert.ToInt64(cmd.ExecuteScalar()) + 1;
            string kode = iMax.ToString().PadLeft(10,'0');
            //=============== END kode ===

            o.Kd = kode;
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
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
        public int Update(AdnBukuBank o)
        {
            int hasil=0;

            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.Kd + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                hasil=cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return hasil;
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

        public AdnBukuBank Get(string kd)
        {
            AdnBukuBank o = null;
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
                    o = new AdnBukuBank();
                    o.Kd = kd;
                    o.KdKas = AdnFungsi.CStr(rdr["kd_kas"]);
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.KdKas = AdnFungsi.CStr(rdr["kd_kas"]);
                    o.Debet = AdnFungsi.CDec(rdr["debet"]);
                    o.Kredit = AdnFungsi.CDec(rdr["kredit"]);
                    o.NoKwitansiDonasi = AdnFungsi.CStr(rdr["no_kwitansi_donasi"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnBukuBank> GetAll()
        {
            List<AdnBukuBank> lst = new List<AdnBukuBank>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnBukuBank o = new AdnBukuBank();
                    o.Kd = AdnFungsi.CStr(rdr["kd"]) ;
                    o.KdKas = AdnFungsi.CStr(rdr["kd_kas"]);
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.KdKas = AdnFungsi.CStr(rdr["kd_kas"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.Debet = AdnFungsi.CDec(rdr["debet"]);
                    o.Kredit = AdnFungsi.CDec(rdr["kredit"]);
                    o.NoKwitansiDonasi = AdnFungsi.CStr(rdr["no_kwitansi_donasi"]);
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

        public DataTable GetByArgs(DateTime TglDr, DateTime TglSd, string KdKas, string Deskripsi)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("Kd", typeof(String));
            tbl.Columns.Add("KdKas", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("Debet", typeof(decimal));
            tbl.Columns.Add("Kredit", typeof(decimal));
            tbl.Columns.Add("NoKwitansiDonasi", typeof(String));

            string Where = "";
            string sql =
            " select kd, tgl, deskripsi,debet,kredit,no_kwitansi_donasi, kd_kas "
            + " from " + NAMA_TABEL + " hdr "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl < '" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";
            
            if (KdKas != "")
            {
                sql = sql + " AND  kd_kas = '" + KdKas.Trim() + "'";
            }

            if (Deskripsi.Trim() != "")
            {
                sql = sql + " AND  deskripsi LIKE '%" + Deskripsi.Trim() + "%'";
            }

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["kd"] =AdnFungsi.CStr(rdr["kd"]).Trim();
                baris["KdKas"] = KdKas.Trim();
                baris["tgl"] = Convert.ToDateTime(rdr["tgl"]);
                baris["deskripsi"] = Convert.ToString(rdr["deskripsi"]).Trim();
                baris["debet"] = AdnFungsi.CDec(rdr["debet"]);
                baris["kredit"] = AdnFungsi.CDec(rdr["kredit"]);
                baris["NoKwitansiDonasi"] = AdnFungsi.CStr(rdr["no_kwitansi_donasi"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl;
        }
    }
}
