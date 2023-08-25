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
    public class AdnJurnalDao
    {
        private const short JUMLAH_KOLOM = 4;
        private const string NAMA_TABEL = "ac_tjurnal";
        
        private string pkey = "kd_jurnal";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlTransaction trn;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnJurnalDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnJurnalDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnJurnalDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
            this.trn = trn;
        }
        private void SetFldNilai(AdnJurnal o)
        {
            short idx = 0;

            fld[idx] = "kd_jurnal"; nilai[idx] = o.KdJurnal.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl"; nilai[idx] = o.Tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "deskripsi"; nilai[idx] = o.Deskripsi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "st_posting"; nilai[idx] = o.StatusPosting.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnJurnal o)
        {
            o.KdJurnal = this.GetKode(o.Tgl);

            ////-------------- Menentukan Kode 
            //string periode = o.Tgl.Year.ToString().Substring(2) + o.Tgl.Month.ToString("00");

            //string kode = "";
            //sql =
            //"SELECT isnull(max(right(kd_jurnal,4)),0) as kd  "
            //+ " FROM " + NAMA_TABEL
            //+ " where left(kd_jurnal,6) ='JU" + periode.Trim() + "'";

            //cmd.CommandText = sql;
            //int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            //kode ="JU" +periode + iMax.ToString("0000");

            //o.KdJurnal = kode;
            ////=============== END kode ---

            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                foreach (AdnJurnalDtl item in o.ItemDf)
                {
                    item.KdJurnal = o.KdJurnal;
                    new AdnJurnalDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
                }

            }
            catch(DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
        }
        public void Update(AdnJurnal o)
        {
            string KdOriginal = o.KdJurnal.ToString();

            if (o.KdJurnal.ToString().Substring(2, 4) != o.Tgl.ToString("yyMM"))
            {
                o.KdJurnal = this.GetKode(o.Tgl);
            }
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + KdOriginal + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();


                new AdnJurnalDtlDao(this.cnn, this.pengguna, this.trn).Hapus(o.KdJurnal);

                foreach (AdnJurnalDtl item in o.ItemDf)
                {
                    item.KdJurnal = o.KdJurnal;
                    new AdnJurnalDtlDao(this.cnn, this.pengguna, this.trn).Simpan(item);
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

        public AdnJurnal Get(string kd)
        {
            AdnJurnal o = null;
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
                    o = new AdnJurnal();
                    o.KdJurnal = kd;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.StatusPosting = AdnFungsi.CBool(rdr["st_posting"],true);
                }
                rdr.Close();
                o.ItemDf = new AdnJurnalDtlDao(this.cnn,this.pengguna,this.trn).Get(kd);
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnJurnal> GetAll()
        {
            List<AdnJurnal> lst = new List<AdnJurnal>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnJurnal o = new AdnJurnal();
                    o.KdJurnal = AdnFungsi.CStr(rdr["kd_jurnal"]) ;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.StatusPosting = AdnFungsi.CBool(rdr["st_posting"],true);
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

        public DataTable GetLapJU(DateTime TglDr, DateTime TglSd, string KdProject)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoBukti", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdProject", typeof(String));
            tbl.Columns.Add("NmProject", typeof(String));
            tbl.Columns.Add("Akun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));

            string sql =
            " select tgl,  deskripsi, hdr.kd_jurnal, dtl.kd_project, "
            + "     dtl.kd_akun, nm_akun, debet, kredit, nm_project "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " inner join ac_makun mak "
            + "     on mak.kd_akun = dtl.kd_akun "
            + " left outer join mproject prj "
            + "     on dtl.kd_project = prj.kd_project "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            if (KdProject.ToString().Trim()!="")
            {
                sql = sql + " and dtl.kd_project = '" + KdProject.ToString().Trim() + "'";
            }


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoBukti"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                baris["KdProject"] = AdnFungsi.CStr(rdr["kd_project"]);
                baris["NmProject"] = AdnFungsi.CStr(rdr["nm_project"]);

                decimal debet = AdnFungsi.CDec(rdr["debet"]);
                if (debet > 0)
                {
                    baris["Akun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                }
                else
                {
                    baris["Akun"] = "".PadLeft(5) + AdnFungsi.CStr(rdr["nm_akun"]);
                }
                baris["Debet"] = debet;
                baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

        private string GetKode(DateTime Tgl)
        {
            string periode = Tgl.Year.ToString().Substring(2) + Tgl.Month.ToString("00");
            string kode = "";

            sql =
            "SELECT isnull(max(right(kd_jurnal,4)),0) as kd  "
            + " FROM " + NAMA_TABEL
            + " where left(kd_jurnal,6) ='JU" + periode.Trim() + "'";

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            kode = "JU" + periode + iMax.ToString("0000");

            return kode;
        }

        public DataTable GetLapBukuBesar(DateTime TglDr, DateTime TglSd, string KdProject)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoBukti", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdProject", typeof(String));
            tbl.Columns.Add("NmProject", typeof(String));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("NmDept", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));

            string sql =
            " select tgl,  deskripsi, dtl.memo, hdr.kd_jurnal, dtl.kd_project,kd_dept, nm_dept, "
            + "     dtl.kd_akun, nm_akun, debet, kredit, nm_project "
            + " from ac_tjurnal hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " inner join ac_makun mak "
            + "     on mak.kd_akun = dtl.kd_akun "
            + " left outer join mproject prj "
            + "     on dtl.kd_project = prj.kd_project "
            + " left outer join mdept dpt "
            + "     on dpt.kd_dept = dtl.kd_dept "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            if (KdProject.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_project = '" + KdProject.ToString().Trim() + "'";
            }


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoBukti"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["KdProject"] = AdnFungsi.CStr(rdr["kd_project"]);
                baris["NmProject"] = AdnFungsi.CStr(rdr["nm_project"]);

                if (AdnFungsi.CStr(rdr["memo"]) != "")
                {
                    baris["memo"] = AdnFungsi.CStr(rdr["memo"]);
                }
                else
                {
                    baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                }

                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                decimal debet = AdnFungsi.CDec(rdr["debet"]);
                //if (debet > 0)
                //{
                //    baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                //}
                //else
                //{
                //    baris["Akun"] = "".PadLeft(5) + AdnFungsi.CStr(rdr["nm_akun"]);
                //}
                baris["Debet"] = debet;
                baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

    }
}
