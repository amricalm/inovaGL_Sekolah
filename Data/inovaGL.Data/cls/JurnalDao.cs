using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaGL.Data
{
    public class AdnJurnalDao
    {
        private const short JUMLAH_KOLOM = 14;
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
            fld[idx] = "sumber"; nilai[idx] = o.Sumber.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "st_posting"; nilai[idx] = o.StatusPosting.ToString(); tipe[idx] = "b"; idx++;
            fld[idx] = "jn_jurnal"; nilai[idx] = o.JenisJurnal.ToString(); tipe[idx] = "s"; idx++;

            fld[idx] = "kd_siswa"; nilai[idx] = o.KdSiswa.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "nis"; nilai[idx] = o.Nis.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "th_ajar_tagihan"; nilai[idx] = o.ThAjarTagihan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "no_kwitansi"; nilai[idx] = o.NoKwitansi.ToString(); tipe[idx] = "s"; idx++;

            fld[idx] = "tag"; nilai[idx] = o.Tag.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "periode"; nilai[idx] = o.Periode.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnJurnal o)
        {
            if (o.KdJurnal.Trim() == "")
            {
                o.KdJurnal = this.GetKode(o.Tgl);
            }

            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            foreach (AdnJurnalDtl item in o.ItemDf)
            {
                item.KdJurnal = o.KdJurnal;
                new AdnJurnalDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
            }

        }
        public void Update(AdnJurnal o)
        {
            //string KdOriginal = o.KdJurnal.ToString();

            //if (o.KdJurnal.ToString().Substring(2, 4) != o.Tgl.ToString("yyMM"))
            //{
            //    o.KdJurnal = this.GetKode(o.Tgl);
            //}
            this.SetFldNilai(o);
            //sWhere = this.pkey + "='" + KdOriginal + "'";
            sWhere = this.pkey + "='" + o.KdJurnal.ToString().Trim() + "'";
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
                    o.StatusPosting = AdnFungsi.CBool(rdr["st_posting"], true);

                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.ThAjarTagihan = AdnFungsi.CStr(rdr["th_ajar_tagihan"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"], true);
                    o.Nis = AdnFungsi.CStr(rdr["nis"]);

                    o.NoKwitansi = AdnFungsi.CStr(rdr["no_kwitansi"]);
                    o.Sumber = AdnFungsi.CStr(rdr["sumber"]);
                    o.JenisJurnal = AdnFungsi.CStr(rdr["jn_jurnal"]);

                    o.Tag = AdnFungsi.CStr(rdr["tag"]);
                    o.Periode = AdnFungsi.CStr(rdr["periode"]);
                }
                rdr.Close();
                if (o != null)
                {
                    o.ItemDf = new AdnJurnalDtlDao(this.cnn, this.pengguna, this.trn).Get(kd);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            return o;
        }
        public DataTable Get(DateTime TglDr, DateTime TglSd)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoBukti", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("NmDept", typeof(String));
            tbl.Columns.Add("KdProject", typeof(String));
            tbl.Columns.Add("NmProject", typeof(String));
            tbl.Columns.Add("Akun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Memo", typeof(String));

            string sql =
            " select tgl,  deskripsi, hdr.kd_jurnal "
            //+ "     , dtl.kd_dept,dtl.memo,dtl.kd_akun, nm_akun, debet, kredit, nm_dept,nm_project,dtl.kd_project "
            + " from " + NAMA_TABEL + " hdr"
            //+ " inner join ac_tjurnal_dtl dtl "
            //+ "     on hdr.kd_jurnal = dtl.kd_jurnal "
            //+ " inner join ac_makun mak "
            //+ "     on mak.kd_akun = dtl.kd_akun "
            //+ " left outer join ac_mdept dep "
            //+ "     on dtl.kd_dept = dep.kd_dept "
            //+ " left outer join ac_mproject prj "
            //+ "     on dtl.kd_project = prj.kd_project "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            //if (KdDept.ToString().Trim() != "")
            //{
            //    sql = sql + " and dtl.kd_dept = '" + KdDept.ToString().Trim() + "'";
            //}

            //if (KdProject.ToString().Trim() != "")
            //{
            //    sql = sql + " and dtl.kd_project = '" + KdProject.ToString().Trim() + "'";
            //}

            sql += " ORDER BY tgl, hdr.kd_jurnal ";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoBukti"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                //baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                //baris["NmDept"] = AdnFungsi.CStr(rdr["nm_dept"]);
                //baris["KdProject"] = AdnFungsi.CStr(rdr["kd_project"]);
                //baris["NmProject"] = AdnFungsi.CStr(rdr["nm_project"]);
                //baris["Memo"] = AdnFungsi.CStr(rdr["Memo"]);

                //decimal debet = AdnFungsi.CDec(rdr["debet"]);
                //if (debet > 0)
                //{
                //    baris["Akun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                //}
                //else
                //{
                //    baris["Akun"] = "".PadLeft(5) + AdnFungsi.CStr(rdr["nm_akun"]);
                //}
                //baris["Debet"] = debet;
                //baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable Get(DateTime TglDr, DateTime TglSd, string JenisJurnal)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoBukti", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("NmDept", typeof(String));
            tbl.Columns.Add("KdProject", typeof(String));
            tbl.Columns.Add("NmProject", typeof(String));
            tbl.Columns.Add("Akun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Memo", typeof(String));
            tbl.Columns.Add("Total", typeof(Decimal));

            string sql =
            " select tgl, deskripsi, hdr.kd_jurnal, sum(debet) total "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            + "     and hdr.kd_siswa>0 ";

            if (JenisJurnal.ToString().Trim() != "")
            {
                sql = sql + " and hdr.jn_jurnal = '" + JenisJurnal.ToString().Trim() + "'";
            }

            sql += " GROUP  BY tgl, deskripsi, hdr.kd_jurnal ";
            sql += " ORDER BY tgl, hdr.kd_jurnal ";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoBukti"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                baris["Total"] = AdnFungsi.CDec(rdr["total"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
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

        public DataTable GetLapJU(DateTime TglDr, DateTime TglSd,string KdProject, string KdDept)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoBukti", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("NmDept", typeof(String));
            tbl.Columns.Add("KdProject", typeof(String));
            tbl.Columns.Add("NmProject", typeof(String));
            tbl.Columns.Add("Akun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Memo", typeof(String));

            string sql =
            " select tgl,  deskripsi, hdr.kd_jurnal, dtl.kd_dept,dtl.memo, "
            + "     dtl.kd_akun, nm_akun, debet, kredit, nm_dept,nm_project,dtl.kd_project "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " inner join ac_makun mak "
            + "     on mak.kd_akun = dtl.kd_akun "
            + " left outer join ac_mdept dep "
            + "     on dtl.kd_dept = dep.kd_dept "
            + " left outer join ac_mproject prj "
            + "     on dtl.kd_project = prj.kd_project "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            if (KdDept.ToString().Trim()!="")
            {
                sql = sql + " and dtl.kd_dept = '" + KdDept.ToString().Trim() + "'";
            }

            if (KdProject.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_project = '" + KdProject.ToString().Trim() + "'";
            }

            sql += " ORDER BY tgl, hdr.kd_jurnal, dtl.no_urut ";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoBukti"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["NmDept"] = AdnFungsi.CStr(rdr["nm_dept"]);
                baris["KdProject"] = AdnFungsi.CStr(rdr["kd_project"]);
                baris["NmProject"] = AdnFungsi.CStr(rdr["nm_project"]);
                baris["Memo"] = AdnFungsi.CStr(rdr["Memo"]);

                decimal debet = AdnFungsi.CDec(rdr["debet"]);
                if (debet > 0)
                {
                    baris["Akun"] = AdnFungsi.CStr(rdr["kd_akun"]) + "  " +  AdnFungsi.CStr(rdr["nm_akun"]);
                }
                else
                {
                    baris["Akun"] = "".PadLeft(5) + AdnFungsi.CStr(rdr["kd_akun"]) + "  " + AdnFungsi.CStr(rdr["nm_akun"]);
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
        public DataTable GetLapBukuBesar(DateTime TglDr, DateTime TglSd, string KdProject, string KdAkun, string KdDept)
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
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Memo", typeof(String));

            string sql =
            " select tgl,  deskripsi, dtl.memo, hdr.kd_jurnal, dtl.kd_project,dtl.kd_dept, nm_dept, "
            + "     dtl.kd_akun, nm_akun, debet, kredit, nm_project, mak.dk "
            + " from ac_tjurnal hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " inner join ac_makun mak "
            + "     on mak.kd_akun = dtl.kd_akun "
            + " left outer join ac_mproject prj "
            + "     on dtl.kd_project = prj.kd_project "
            + " left outer join ac_mdept dpt "
            + "     on dpt.kd_dept = dtl.kd_dept "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            + "     AND dtl.kd_akun = '" + KdAkun.ToString().Trim() + "'";

            if (KdProject.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_project = '" + KdProject.ToString().Trim() + "'";
            }

            if (KdDept.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_dept = '" + KdDept.ToString().Trim() + "'";
            }

            sql = sql + " ORDER BY tgl ";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoBukti"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["KdProject"] = AdnFungsi.CStr(rdr["kd_project"]);
                baris["NmProject"] = AdnFungsi.CStr(rdr["nm_project"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["NmDept"] = AdnFungsi.CStr(rdr["nm_dept"]);

                if (AdnFungsi.CStr(rdr["memo"]) != "")
                {
                    baris["Memo"] = AdnFungsi.CStr(rdr["memo"]);
                }
                else
                {
                    baris["Memo"] = AdnFungsi.CStr(rdr["deskripsi"]);
                }

                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["DK"] = AdnFungsi.CStr(rdr["dk"]);
                baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                tbl.Rows.Add(baris);
            }

            rdr.Close();
            return tbl; ;
        }
        public DataTable GetLapPengeluaranPerSumberDana(DateTime TglDr, DateTime TglSd, string KdProject, string KdAkun, string KdDept)
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
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Memo", typeof(String));

            string sql =
            " select tgl,  deskripsi, dtl.memo, hdr.kd_jurnal, dtl.kd_project,dtl.kd_dept, nm_dept, "
            + "     dtl.kd_akun, nm_akun, debet, kredit, nm_project, mak.dk "
            + " from ac_tjurnal hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " inner join ac_makun mak "
            + "     on mak.kd_akun = dtl.kd_akun "
            + " left outer join ac_mproject prj "
            + "     on dtl.kd_project = prj.kd_project "
            + " left outer join ac_mdept dpt "
            + "     on dpt.kd_dept = dtl.kd_dept "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            + "     AND dtl.sumber_dana = '" + KdAkun.ToString().Trim() + "'";

            if (KdProject.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_project = '" + KdProject.ToString().Trim() + "'";
            }

            if (KdDept.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_dept = '" + KdDept.ToString().Trim() + "'";
            }

            sql = sql + " ORDER BY tgl ";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoBukti"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["KdProject"] = AdnFungsi.CStr(rdr["kd_project"]);
                baris["NmProject"] = AdnFungsi.CStr(rdr["nm_project"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["NmDept"] = AdnFungsi.CStr(rdr["nm_dept"]);
                baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                if (AdnFungsi.CStr(rdr["memo"]) != "")
                {
                    baris["Memo"] = AdnFungsi.CStr(rdr["memo"]);
                }
                else
                {
                    baris["Memo"] = AdnFungsi.CStr(rdr["deskripsi"]);
                }

                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["DK"] = AdnFungsi.CStr(rdr["dk"]);
                baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                tbl.Rows.Add(baris);
            }

            rdr.Close();
            return tbl; ;
        }
        
        public DataTable GetLapJurnal2(DateTime TglDr, DateTime TglSd, string KdProject, string KdDept)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoBukti", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdProject", typeof(String));
            tbl.Columns.Add("NmProject", typeof(String));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("NmDept", typeof(String));
            tbl.Columns.Add("Memo", typeof(String));
            tbl.Columns.Add("Akun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));

            string sql =

            " select tgl,  deskripsi, kd_jurnal, kd_project, "
            + "     kd_akun, nm_akun, sum(debet) debet, sum(kredit) kredit, nm_project, kd_dept, nm_dept, memo, no_urut "
            + " FROM "
                + " ( "
                + " select tgl,  deskripsi, hdr.kd_tkm as kd_jurnal, dtl.kd_project, "
                + "     dtl.kd_akun, nm_akun, debet, kredit, nm_project, dtl.kd_dept, nm_dept, dtl.memo, dtl.no_urut "
                + " from ac_tkm hdr"
                + " inner join ac_tkm_dtl dtl "
                + "     on hdr.kd_tkm = dtl.kd_tkm "
                + " inner join ac_makun mak "
                + "     on mak.kd_akun = dtl.kd_akun "
                + " left outer join ac_mproject prj "
                + "     on dtl.kd_project = prj.kd_project "
                + " left outer join ac_mdept prg "
                + "     on dtl.kd_dept  = prg.kd_dept "
                + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            if (KdProject.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_project = '" + KdProject.ToString().Trim() + "'";
            }

            if (KdDept.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_dept = '" + KdDept.ToString().Trim() + "'";
            }


            sql +=
            " UNION "
            + " select tgl,  deskripsi, hdr.kd_tkk as kd_jurnal, dtl.kd_project, "
            + "     dtl.kd_akun, nm_akun, debet, kredit, nm_project, dtl.kd_dept, nm_dept, dtl.memo, dtl.no_urut "
            + " from ac_tkk hdr"
            + " inner join ac_tkk_dtl dtl "
            + "     on hdr.kd_tkk = dtl.kd_tkk "
            + " inner join ac_makun mak "
            + "     on mak.kd_akun = dtl.kd_akun "
            + " left outer join ac_mproject prj "
            + "     on dtl.kd_project = prj.kd_project "
            + " left outer join ac_mdept prg "
            + "     on dtl.kd_dept  = prg.kd_dept "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            if (KdProject.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_project = '" + KdProject.ToString().Trim() + "'";
            }

            if (KdDept.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_dept = '" + KdDept.ToString().Trim() + "'";
            }

            sql +=
            " UNION "
            + " select tgl,  deskripsi, hdr.kd_tju as kd_jurnal, dtl.kd_project, "
            + "     dtl.kd_akun, nm_akun, debet, kredit, nm_project, dtl.kd_dept, nm_dept, dtl.memo, dtl.no_urut "
            + " from ac_tju hdr"
            + " inner join ac_tju_dtl dtl "
            + "     on hdr.kd_tju = dtl.kd_tju "
            + " inner join ac_makun mak "
            + "     on mak.kd_akun = dtl.kd_akun "
            + " left outer join ac_mproject prj "
            + "     on dtl.kd_project = prj.kd_project "
            + " left outer join ac_mdept prg "
            + "     on dtl.kd_dept  = prg.kd_dept "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            if (KdProject.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_project = '" + KdProject.ToString().Trim() + "'";
            }

            if (KdDept.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_dept = '" + KdDept.ToString().Trim() + "'";
            }

            sql +=
            " ) TR "
            + " GROUP BY tgl,  deskripsi, kd_jurnal, kd_project, "
            + "     kd_akun, nm_akun,  nm_project, kd_dept, nm_dept, memo, no_urut "
            + " ORDER BY tgl , kd_jurnal, no_urut ";

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
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["NmDept"] = AdnFungsi.CStr(rdr["nm_dept"]);
                baris["Memo"] = AdnFungsi.CStr(rdr["Memo"]);

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

        public DataTable GetPiutangPerSiswa(DateTime TglDr, DateTime TglSd, string Kelas, string KdSekolah, string ThAjar,
            string JenisSPP, string JenisDPG, string JenisDTH)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("Nis", typeof(String));
            tbl.Columns.Add("KdSiswa", typeof(String));
            tbl.Columns.Add("NmSiswa", typeof(String));
            tbl.Columns.Add("DanaPengembangan", typeof(Decimal));
            tbl.Columns.Add("DanaTahunan", typeof(Decimal));
            tbl.Columns.Add("DanaSPP", typeof(Decimal));
            tbl.Columns.Add("Jumlah", typeof(Decimal));

            string sql =
            " select sis.nis,  sis.kd_siswa, sis.nama_lengkap as nm_siswa, "
            + "     (isnull(tspp.debet,0) - isnull(tspp.kredit,0)) spp, (isnull(tdpg.debet,0) - isnull(tdpg.kredit,0)) as dpg, (isnull(tdth.debet,0) - isnull(tdth.kredit,0)) as dth "
            + " from ms_siswa sis "
            + " inner join kelas_siswa ks "
            + "     on sis.nis = ks.nis "
            + "     and sis.kd_sekolah = ks.kd_sekolah "
            + "     and ks.th_ajar = '" + ThAjar + "'"
            + " left outer join "
            + " ( "

            + " select hdr.kd_siswa, sum(dtl.debet) debet , sum(dtl.kredit) kredit "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " inner join ac_makun mak "
            + "     on dtl.kd_akun = mak.kd_akun "
            + " inner join ms_biaya bya "
            + "     on mak.kd_akun = bya.kd_akun_piutang "
            + "     and bya.kd_jenis ='" + JenisSPP + "'"
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            + "     and hdr.kd_sekolah = '" + KdSekolah + "'"
            + "     and hdr.th_ajar_tagihan = '" + ThAjar + "'"
            + " group by hdr.kd_siswa "
            + " ) tspp "
            + "     on sis.kd_siswa  = tspp.kd_siswa "

            + " left outer join "
            + " ( "

            + " select hdr.kd_siswa, sum(dtl.debet) debet , sum(dtl.kredit) kredit "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " inner join ac_makun mak "
            + "     on dtl.kd_akun = mak.kd_akun "
            + " inner join ms_biaya bya "
            + "     on mak.kd_akun = bya.kd_akun_piutang "
            + "     and bya.kd_jenis ='" + JenisDTH + "'"
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            + "     and hdr.kd_sekolah = '" + KdSekolah + "'"
            + "     and hdr.th_ajar_tagihan = '" + ThAjar + "'"
            + " group by hdr.kd_siswa "
            + " ) tdth "
            + "     on sis.kd_siswa  = tdth.kd_siswa "

            + " left outer join "
            + " ( "

            + " select hdr.kd_siswa, sum(dtl.debet) debet , sum(dtl.kredit) kredit "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " inner join ac_makun mak "
            + "     on dtl.kd_akun = mak.kd_akun "
            + " inner join ms_biaya bya "
            + "     on mak.kd_akun = bya.kd_akun_piutang "
            + "     and bya.kd_jenis ='" + JenisDPG + "'"
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            + "     and hdr.kd_sekolah = '" + KdSekolah + "'"
            + "     and hdr.th_ajar_tagihan = '" + ThAjar + "'"
            + " group by hdr.kd_siswa "
            + " ) tdpg "
            + "     on sis.kd_siswa  = tdpg.kd_siswa "


            + " where ks.kelas = '" + Kelas + "'"
            + "     and ks.kd_sekolah = '" + KdSekolah + "'";

            sql += " ORDER BY sis.nama_lengkap ";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["Nis"] = AdnFungsi.CStr(rdr["nis"]);
                baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"],true);
                baris["NmSiswa"] = AdnFungsi.CStr(rdr["nm_siswa"]);
                baris["DanaPengembangan"] = AdnFungsi.CDec(rdr["dpg"]);
                baris["DanaTahunan"] = AdnFungsi.CDec(rdr["dth"]);
                baris["DanaSPP"] = AdnFungsi.CDec(rdr["spp"]);
                baris["Jumlah"] = AdnFungsi.CDec(rdr["dpg"]) + AdnFungsi.CDec(rdr["dth"]) + AdnFungsi.CDec(rdr["spp"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

        public DataTable GetPiutangPerSiswaTabular(DateTime TglDr, DateTime TglSd, string Kelas, string KdSekolah, string ThAjar,
            string JenisSPP, string JenisDPG, string JenisDTH)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("Nis", typeof(String));
            tbl.Columns.Add("KdSiswa", typeof(String));
            tbl.Columns.Add("NmSiswa", typeof(String));
            tbl.Columns.Add("DanaPengembangan1", typeof(Decimal));
            tbl.Columns.Add("DanaTahunan1", typeof(Decimal));
            tbl.Columns.Add("DanaSPP1", typeof(Decimal));

            tbl.Columns.Add("DanaPengembangan2", typeof(Decimal));
            tbl.Columns.Add("DanaTahunan2", typeof(Decimal));
            tbl.Columns.Add("DanaSPP2", typeof(Decimal));

            tbl.Columns.Add("DanaPengembangan3", typeof(Decimal));
            tbl.Columns.Add("DanaTahunan3", typeof(Decimal));
            tbl.Columns.Add("DanaSPP3", typeof(Decimal));

            tbl.Columns.Add("DanaPengembangan4", typeof(Decimal));
            tbl.Columns.Add("DanaTahunan4", typeof(Decimal));
            tbl.Columns.Add("DanaSPP4", typeof(Decimal));

            tbl.Columns.Add("DanaPengembangan5", typeof(Decimal));
            tbl.Columns.Add("DanaTahunan5", typeof(Decimal));
            tbl.Columns.Add("DanaSPP5", typeof(Decimal));

            tbl.Columns.Add("DanaPengembangan6", typeof(Decimal));
            tbl.Columns.Add("DanaTahunan6", typeof(Decimal));
            tbl.Columns.Add("DanaSPP6", typeof(Decimal));

            tbl.Columns.Add("Jumlah", typeof(Decimal));

            string sql =
            " select sis.nis,  sis.kd_siswa, sis.nama_lengkap as nm_siswa, msk.tingkat, "
            + "     (isnull(tspp.debet,0) - isnull(tspp.kredit,0)) spp, (isnull(tdpg.debet,0) - isnull(tdpg.kredit,0)) as dpg, (isnull(tdth.debet,0) - isnull(tdth.kredit,0)) as dth "
            + " from ms_siswa sis "
            + " inner join kelas_siswa ks "
            + "     on sis.nis = ks.nis "
            + "     and sis.kd_sekolah = ks.kd_sekolah "
            + "     and ks.th_ajar = '" + ThAjar + "'"
            + " inner join ms_kelas msk "
            + "     on msk.kelas = ks.kelas "
            + "     and msk.kd_sekolah = ks.kd_sekolah "
            + " left outer join "
            + " ( "

            + " select mak.kd_akun, sum(dtl.debet) debet , sum(dtl.kredit) kredit "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " inner join ac_makun mak "
            + "     on dtl.kd_akun = mak.kd_akun "
            + " inner join ms_biaya bya "
            + "     on mak.kd_akun = bya.kd_akun_piutang "
            //+ "     and bya.kd_jenis ='" + JenisSPP + "'"
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            + "     and hdr.kd_sekolah = '" + KdSekolah + "'"
            + "     and hdr.th_ajar_tagihan = '" + ThAjar + "'"
            + " group by mak.kd_akun "
            + " ) tspp "
            + "     on sis.kd_siswa  = tspp.kd_siswa "

            //+ " left outer join "
            //+ " ( "

            //+ " select mak.kd_akun, sum(dtl.debet) debet , sum(dtl.kredit) kredit "
            //+ " from " + NAMA_TABEL + " hdr"
            //+ " inner join ac_tjurnal_dtl dtl "
            //+ "     on hdr.kd_jurnal = dtl.kd_jurnal "
            //+ " inner join ac_makun mak "
            //+ "     on dtl.kd_akun = mak.kd_akun "
            //+ " inner join ms_biaya bya "
            //+ "     on mak.kd_akun = bya.kd_akun_piutang "
            //+ "     and bya.kd_jenis ='" + JenisDTH + "'"
            //+ " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            //+ "     and hdr.kd_sekolah = '" + KdSekolah + "'"
            //+ "     and hdr.th_ajar_tagihan = '" + ThAjar + "'"
            //+ " group by mak.kd_akun "
            //+ " ) tdth "
            //+ "     on sis.kd_siswa  = tdth.kd_siswa "

            //+ " left outer join "
            //+ " ( "

            //+ " select mak.kd_akun, sum(dtl.debet) debet , sum(dtl.kredit) kredit "
            //+ " from " + NAMA_TABEL + " hdr"
            //+ " inner join ac_tjurnal_dtl dtl "
            //+ "     on hdr.kd_jurnal = dtl.kd_jurnal "
            //+ " inner join ac_makun mak "
            //+ "     on dtl.kd_akun = mak.kd_akun "
            //+ " inner join ms_biaya bya "
            //+ "     on mak.kd_akun = bya.kd_akun_piutang "
            //+ "     and bya.kd_jenis ='" + JenisDPG + "'"
            //+ " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            //+ "     and hdr.kd_sekolah = '" + KdSekolah + "'"
            //+ "     and hdr.th_ajar_tagihan = '" + ThAjar + "'"
            //+ " group by mak.kd_akun "
            //+ " ) tdpg "
            //+ "     on sis.kd_siswa  = tdpg.kd_siswa "


            + " where ks.kelas = '" + Kelas + "'"
            + "     and ks.kd_sekolah = '" + KdSekolah + "'";

            sql += " ORDER BY sis.nama_lengkap ";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["Nis"] = AdnFungsi.CStr(rdr["nis"]);
                baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"], true);
                baris["NmSiswa"] = AdnFungsi.CStr(rdr["nm_siswa"]);

                switch (AdnFungsi.CStr(rdr["tingkat"]))
                {
                    case "1":
                        baris["DanaPengembangan1"] = AdnFungsi.CDec(rdr["dpg"]);
                        baris["DanaTahunan1"] = AdnFungsi.CDec(rdr["dth"]);
                        baris["DanaSPP1"] = AdnFungsi.CDec(rdr["spp"]);
                        break;

                    case "2":

                        break;

                    case "3":

                        break;

                    case "4":

                        break;

                    case "5":

                        break;

                    case "6":
                        break;

                    default:
                        baris["DanaPengembangan"] = AdnFungsi.CDec(rdr["dpg"]);
                        baris["DanaTahunan"] = AdnFungsi.CDec(rdr["dth"]);
                        baris["DanaSPP"] = AdnFungsi.CDec(rdr["spp"]);
                        baris["Jumlah"] = AdnFungsi.CDec(rdr["dpg"]) + AdnFungsi.CDec(rdr["dth"]) + AdnFungsi.CDec(rdr["spp"]);

                        break;

                }

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

        public DataTable GetPiutangPerKelasTabular(DateTime TglDr, DateTime TglSd, string KdSekolah, string ThAjar)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));

            //Cetak Kolom Nilai Per Kelas
            string sKolomTk = ""; string sSumKolomTk = "";
            string sql =
                "select distinct tingkat "
                + " from ms_kelas "
                + " where kd_sekolah = '" + KdSekolah.ToString().Trim() + "'"
                + " order by tingkat";

            this.cmd.CommandText = sql;
            SqlDataReader rdr = this.cmd.ExecuteReader();

            ArrayList prg = new ArrayList();
            int BarisTk = 1;
            while (rdr.Read())
            {
                object[] fld = new object[rdr.FieldCount];
                rdr.GetValues(fld);
                prg.Add(fld);

                if (sKolomTk != "")
                {
                    sKolomTk = sKolomTk + ",";
                    sSumKolomTk = sSumKolomTk + ",";
                }

                sKolomTk = sKolomTk + "'debet" + BarisTk.ToString() + "' = case when msk.tingkat = '" + fld[0].ToString().Trim() + "' then (dtl.debet) end, 'kredit" + BarisTk.ToString() + "' = case when msk.tingkat = '" + fld[0].ToString().Trim() + "' then (dtl.kredit) end";
                sSumKolomTk = sSumKolomTk + "isnull(sum(debet" + BarisTk.ToString() + "),0)debet" + BarisTk.ToString() + ", isnull(sum(kredit" + BarisTk.ToString() + "),0)kredit" + BarisTk.ToString();

                //Tambah Kolom
                tbl.Columns.Add("d" + BarisTk.ToString(), typeof(Decimal));

                BarisTk++;
            }
            rdr.Close();

            //== END  Cetak Kolom Nilai Per Kelas


            sql = " select mak.kd_akun, mak.nm_akun, mak.dk ";

            if (sSumKolomTk != "")
            {
                sql = sql + "," + sSumKolomTk;
            }

            sql = sql
                  + " from ac_makun mak "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT mak.kd_akun, mak.nm_akun ";

            if (sKolomTk != "")
            {
                sql = sql + "," + sKolomTk;
            }

            sql = sql
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "

                  + "       inner join ms_biaya bya " 
                  + "           on mak.kd_akun = bya.kd_akun_piutang"
                  + "       inner join ms_siswa sis"
                  + "           on sis.kd_siswa = hdr.kd_siswa"
                  + "       inner join kelas_siswa ks"
                  + "           on sis.nis = ks.nis"
                  + "           and sis.kd_sekolah = ks.kd_sekolah" 
                  + "           and ks.th_ajar = '" + ThAjar +  "'"
                  + "       inner join ms_kelas msk "
                  + "           on ks.kelas = msk.kelas"
                  + "       and ks.kd_sekolah = msk.kd_sekolah"


                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                
                  + " ) tr "
                  + "       on mak.kd_akun = tr.kd_akun "

                  + " inner join ms_biaya bya "
                  + "       on mak.kd_akun = bya.kd_akun_piutang"

                  + " GROUP BY mak.kd_akun, mak.nm_akun, mak.dk "
                  + " order by mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_akun"]);

                    for (int i = 1; i < BarisTk; i++)
                    {
                        if (AdnFungsi.CStr(rdr["dk"]).Trim().ToUpper() == AdnVar.SaldoNormal.DEBET)
                        {
                            baris["d" + i.ToString()] = Convert.ToDecimal(rdr["debet" + i.ToString()]) - Convert.ToDecimal(rdr["kredit" + i.ToString()]);
                        }
                        else
                        {
                            baris["d" + i.ToString()] = Convert.ToDecimal(rdr["kredit" + i.ToString()]) - Convert.ToDecimal(rdr["debet" + i.ToString()]);
                        }
                    }

                    tbl.Rows.Add(baris);
                }
                rdr.Close();
            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            //string sql =
            //" select sis.nis,  sis.kd_siswa, sis.nama_lengkap as nm_siswa, msk.tingkat, "
            //+ "     (isnull(tspp.debet,0) - isnull(tspp.kredit,0)) spp, (isnull(tdpg.debet,0) - isnull(tdpg.kredit,0)) as dpg, (isnull(tdth.debet,0) - isnull(tdth.kredit,0)) as dth "
            //+ " from ms_siswa sis "
            //+ " inner join kelas_siswa ks "
            //+ "     on sis.nis = ks.nis "
            //+ "     and sis.kd_sekolah = ks.kd_sekolah "
            //+ "     and ks.th_ajar = '" + ThAjar + "'"
            //+ " inner join ms_kelas msk "
            //+ "     on msk.kelas = ks.kelas "
            //+ "     and msk.kd_sekolah = ks.kd_sekolah "
            //+ " left outer join "
            //+ " ( "

            //+ " select mak.kd_akun, sum(dtl.debet) debet , sum(dtl.kredit) kredit "
            //+ " from " + NAMA_TABEL + " hdr"
            //+ " inner join ac_tjurnal_dtl dtl "
            //+ "     on hdr.kd_jurnal = dtl.kd_jurnal "
            //+ " inner join ac_makun mak "
            //+ "     on dtl.kd_akun = mak.kd_akun "
            //+ " inner join ms_biaya bya "
            //+ "     on mak.kd_akun = bya.kd_akun_piutang "
            //    //+ "     and bya.kd_jenis ='" + JenisSPP + "'"
            //+ " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            //+ "     and hdr.kd_sekolah = '" + KdSekolah + "'"
            //+ "     and hdr.th_ajar_tagihan = '" + ThAjar + "'"
            //+ " group by mak.kd_akun "
            //+ " ) tspp "
            //+ "     on sis.kd_siswa  = tspp.kd_siswa ";

            //+ " left outer join "
                //+ " ( "

            //+ " select mak.kd_akun, sum(dtl.debet) debet , sum(dtl.kredit) kredit "
                //+ " from " + NAMA_TABEL + " hdr"
                //+ " inner join ac_tjurnal_dtl dtl "
                //+ "     on hdr.kd_jurnal = dtl.kd_jurnal "
                //+ " inner join ac_makun mak "
                //+ "     on dtl.kd_akun = mak.kd_akun "
                //+ " inner join ms_biaya bya "
                //+ "     on mak.kd_akun = bya.kd_akun_piutang "
                //+ "     and bya.kd_jenis ='" + JenisDTH + "'"
                //+ " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                //+ "     and hdr.kd_sekolah = '" + KdSekolah + "'"
                //+ "     and hdr.th_ajar_tagihan = '" + ThAjar + "'"
                //+ " group by mak.kd_akun "
                //+ " ) tdth "
                //+ "     on sis.kd_siswa  = tdth.kd_siswa "

            //+ " left outer join "
                //+ " ( "

            //+ " select mak.kd_akun, sum(dtl.debet) debet , sum(dtl.kredit) kredit "
                //+ " from " + NAMA_TABEL + " hdr"
                //+ " inner join ac_tjurnal_dtl dtl "
                //+ "     on hdr.kd_jurnal = dtl.kd_jurnal "
                //+ " inner join ac_makun mak "
                //+ "     on dtl.kd_akun = mak.kd_akun "
                //+ " inner join ms_biaya bya "
                //+ "     on mak.kd_akun = bya.kd_akun_piutang "
                //+ "     and bya.kd_jenis ='" + JenisDPG + "'"
                //+ " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                //+ "     and hdr.kd_sekolah = '" + KdSekolah + "'"
                //+ "     and hdr.th_ajar_tagihan = '" + ThAjar + "'"
                //+ " group by mak.kd_akun "
                //+ " ) tdpg "
                //+ "     on sis.kd_siswa  = tdpg.kd_siswa "


            //+ " where ks.kelas = '" + Kelas + "'"
            //+ "     and ks.kd_sekolah = '" + KdSekolah + "'";

            //sql += " ORDER BY sis.nama_lengkap ";


            //SqlCommand cmd = new SqlCommand(sql, this.cnn);
            //SqlDataReader rdr = cmd.ExecuteReader();

            //while (rdr.Read())
            //{
            //    DataRow baris = tbl.NewRow();
            //    baris["Nis"] = AdnFungsi.CStr(rdr["nis"]);
            //    baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"], true);
            //    baris["NmSiswa"] = AdnFungsi.CStr(rdr["nm_siswa"]);

            //    switch (AdnFungsi.CStr(rdr["tingkat"]))
            //    {
            //        case "1":
            //            baris["DanaPengembangan1"] = AdnFungsi.CDec(rdr["dpg"]);
            //            baris["DanaTahunan1"] = AdnFungsi.CDec(rdr["dth"]);
            //            baris["DanaSPP1"] = AdnFungsi.CDec(rdr["spp"]);
            //            break;

            //        case "2":

            //            break;

            //        case "3":

            //            break;

            //        case "4":

            //            break;

            //        case "5":

            //            break;

            //        case "6":
            //            break;

            //        default:
            //            baris["DanaPengembangan"] = AdnFungsi.CDec(rdr["dpg"]);
            //            baris["DanaTahunan"] = AdnFungsi.CDec(rdr["dth"]);
            //            baris["DanaSPP"] = AdnFungsi.CDec(rdr["spp"]);
            //            baris["Jumlah"] = AdnFungsi.CDec(rdr["dpg"]) + AdnFungsi.CDec(rdr["dth"]) + AdnFungsi.CDec(rdr["spp"]);

            //            break;

            //    }

            //    tbl.Rows.Add(baris);
            //}
            return tbl; 
        }
        public DataTable GetPiutangPerAkunTabular(DateTime TglDr, DateTime TglSd, string KdSekolah, string ThAjar)
        {
            DataTable tbl = new DataTable("AppTabel");
            tbl.Columns.Add("KdSekolah",typeof(String));
            tbl.Columns.Add("Kelas", typeof(String));

            //Cetak Kolom Nilai Per Akun Tabulan
            string sKolomAkun = ""; string sSumKolomAkun = "";
            string sql =
                "select distinct mak.kd_akun "
                + " from ac_makun mak "
                + " inner join ms_biaya bya "
                + "     on mak.kd_akun = bya.kd_akun_piutang"
                + " order by kd_akun";

            this.cmd.CommandText = sql;
            SqlDataReader rdr = this.cmd.ExecuteReader();

            ArrayList prg = new ArrayList();
            int BarisAkun = 1;
            while (rdr.Read())
            {
                object[] fld = new object[rdr.FieldCount];
                rdr.GetValues(fld);
                prg.Add(fld);

                if (sKolomAkun != "")
                {
                    sKolomAkun = sKolomAkun + ",";
                    sSumKolomAkun = sSumKolomAkun + ",";
                }

                sKolomAkun = sKolomAkun + "'debet" + BarisAkun.ToString() + "' = case when dtl.kd_akun = '" + fld[0].ToString().Trim() + "' then (dtl.debet) end, 'kredit" + BarisAkun.ToString() + "' = case when  dtl.kd_akun = '" + fld[0].ToString().Trim() + "' then (dtl.kredit) end";
                sSumKolomAkun = sSumKolomAkun + "isnull(sum(debet" + BarisAkun.ToString() + "),0)debet" + BarisAkun.ToString() + ", isnull(sum(kredit" + BarisAkun.ToString() + "),0)kredit" + BarisAkun.ToString();

                //Tambah Kolom
                tbl.Columns.Add("d" + BarisAkun.ToString(), typeof(Decimal));

                BarisAkun++;
            }
            rdr.Close();

            //== END  Cetak Kolom Nilai Per Kelas


            sql = " select msk.kd_sekolah, msk.kelas ";

            if (sSumKolomAkun != "")
            {
                sql = sql + "," + sSumKolomAkun;
            }

            sql = sql
                  + " from ms_kelas msk "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT ks.kd_sekolah, ks.kelas ";

            if (sKolomAkun != "")
            {
                sql = sql + "," + sKolomAkun;
            }

            sql = sql
                  + "       FROM ms_kelas msk "
                  + "       inner join kelas_siswa ks "
                  + "           on ks.kelas = msk.kelas"
                  + "           and ks.kd_sekolah = msk.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                   + "      inner join ms_siswa sis"
                  + "           on sis.nis = ks.nis"
                  + "           and sis.kd_sekolah = ks.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           on sis.kd_siswa = hdr.kd_siswa"
                  + "       INNER JOIN ac_tjurnal_dtl dtl "                 
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       inner join ac_makun mak "
                  + "           on mak.kd_akun = dtl.kd_akun"
                  + "       inner join ms_biaya bya "
                  + "           on mak.kd_akun = bya.kd_akun_piutang"
                 
                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "       and hdr.th_ajar = '" + ThAjar + "'"
                  + " ) tr "
                  + "       on msk.kelas = tr.kelas "
                  + "       and msk.kd_sekolah = tr.kd_sekolah "

                  + " GROUP BY msk.kd_sekolah, msk.kelas "
                  + " order by msk.kelas ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdSekolah"] = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    baris["Kelas"] = AdnFungsi.CStr(rdr["kelas"]);

                    for (int i = 1; i < BarisAkun; i++)
                    {
                        baris["d" + i.ToString()] = Convert.ToDecimal(rdr["debet" + i.ToString()]) - Convert.ToDecimal(rdr["kredit" + i.ToString()]);
                    }

                    tbl.Rows.Add(baris);
                }
                rdr.Close();
            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;
        }
        public DataTable GetPiutangPerSiswaPerAkunTabular(DateTime TglDr, DateTime TglSd, string KdSekolah, string ThAjar, string Kelas)
        {
            DataTable tbl = new DataTable("AppTabel");
            tbl.Columns.Add("KdSekolah", typeof(String));
            tbl.Columns.Add("Kelas", typeof(String));
            tbl.Columns.Add("Nis", typeof(String));
            tbl.Columns.Add("NmSiswa", typeof(String));

            //Cetak Kolom Nilai Per Akun Tabulan
            string sKolomAkun = ""; string sSumKolomAkun = "";
            string sql =
                "select distinct mak.kd_akun "
                + " from ac_makun mak "
                + " inner join ms_biaya bya "
                + "     on mak.kd_akun = bya.kd_akun_piutang"
                + " order by kd_akun";

            this.cmd.CommandText = sql;
            SqlDataReader rdr = this.cmd.ExecuteReader();

            ArrayList prg = new ArrayList();
            int BarisAkun = 1;
            while (rdr.Read())
            {
                object[] fld = new object[rdr.FieldCount];
                rdr.GetValues(fld);
                prg.Add(fld);

                if (sKolomAkun != "")
                {
                    sKolomAkun = sKolomAkun + ",";
                    sSumKolomAkun = sSumKolomAkun + ",";
                }

                sKolomAkun = sKolomAkun + "'debet" + BarisAkun.ToString() + "' = case when dtl.kd_akun = '" + fld[0].ToString().Trim() + "' then (dtl.debet) end, 'kredit" + BarisAkun.ToString() + "' = case when  dtl.kd_akun = '" + fld[0].ToString().Trim() + "' then (dtl.kredit) end";
                sSumKolomAkun = sSumKolomAkun + "isnull(sum(debet" + BarisAkun.ToString() + "),0)debet" + BarisAkun.ToString() + ", isnull(sum(kredit" + BarisAkun.ToString() + "),0)kredit" + BarisAkun.ToString();

                //Tambah Kolom
                tbl.Columns.Add("d" + BarisAkun.ToString(), typeof(Decimal));

                BarisAkun++;
            }
            rdr.Close();

            //== END  Cetak Kolom Nilai Per Kelas


            sql = " select msk.kd_sekolah, msk.kelas, sis.nis, sis.nama_lengkap ";

            if (sSumKolomAkun != "")
            {
                sql = sql + "," + sSumKolomAkun;
            }

            sql = sql
                  + " from ms_kelas msk "
                  + "      inner join kelas_siswa ks "
                  + "           on ks.kelas = msk.kelas"
                  + "           and ks.kd_sekolah = msk.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                  + "           and ks.kd_sekolah = '" + KdSekolah + "'"
                  + "            and ks.kelas = '" + Kelas + "'"
                  + "      inner join ms_siswa sis"
                  + "           on sis.nis = ks.nis"
                  + "           and sis.kd_sekolah = ks.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                  + "           and ks.kd_sekolah = '" + KdSekolah + "'"
                  + "            and ks.kelas = '" + Kelas + "'"

                  + " left outer join  "
                  + " ( "
                  + "       SELECT ks.kd_sekolah, ks.kelas, sis.nis ";

            if (sKolomAkun != "")
            {
                sql = sql + "," + sKolomAkun;
            }

            sql = sql
                  + "       FROM ms_kelas msk "
                  + "       inner join kelas_siswa ks "
                  + "           on ks.kelas = msk.kelas"
                  + "           and ks.kd_sekolah = msk.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                  + "           and ks.kd_sekolah = '" + KdSekolah + "'"
                  + "            and ks.kelas = '" + Kelas + "'"
                  + "      inner join ms_siswa sis"
                  + "           on sis.nis = ks.nis"
                  + "           and sis.kd_sekolah = ks.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                  + "           and ks.kd_sekolah = '" + KdSekolah + "'"
                  + "            and ks.kelas = '" + Kelas + "'"
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           on sis.kd_siswa = hdr.kd_siswa"
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       inner join ac_makun mak "
                  + "           on mak.kd_akun = dtl.kd_akun"
                  + "       inner join ms_biaya bya "
                  + "           on mak.kd_akun = bya.kd_akun_piutang"

                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "       and hdr.th_ajar = '" + ThAjar + "'"
                  + " ) tr "
                  + "       on msk.kelas = tr.kelas "
                  + "       and msk.kd_sekolah = tr.kd_sekolah "
                  + "       and sis.nis = tr.nis "

                  + " GROUP BY msk.kd_sekolah, msk.kelas, sis.nis, sis.nama_lengkap "
                  + " order by msk.kd_sekolah, msk.kelas, sis.nama_lengkap ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdSekolah"] = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    baris["Kelas"] = AdnFungsi.CStr(rdr["kelas"]);
                    baris["Nis"] = AdnFungsi.CStr(rdr["nis"]);
                    baris["NmSiswa"] = AdnFungsi.CStr(rdr["nama_lengkap"]);

                    for (int i = 1; i < BarisAkun; i++)
                    {
                        baris["d" + i.ToString()] = Convert.ToDecimal(rdr["debet" + i.ToString()]) - Convert.ToDecimal(rdr["kredit" + i.ToString()]);
                    }

                    tbl.Rows.Add(baris);
                }
                rdr.Close();
            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;
        }
        
        public DataTable GetUtangPerAkunTabular(DateTime TglDr, DateTime TglSd, string KdSekolah, string ThAjar)
        {
            DataTable tbl = new DataTable("AppTabel");
            tbl.Columns.Add("KdSekolah", typeof(String));
            tbl.Columns.Add("Kelas", typeof(String));

            //Cetak Kolom Nilai Per Akun Tabular
            string sKolomAkun = ""; string sSumKolomAkun = "";
            string sql =
                "select distinct mak.kd_akun "
                + " from ac_makun mak "
                + " inner join ms_biaya bya "
                + "     on mak.kd_akun = bya.kd_akun_kewajiban"
                + " order by kd_akun";

            this.cmd.CommandText = sql;
            SqlDataReader rdr = this.cmd.ExecuteReader();

            ArrayList prg = new ArrayList();
            int BarisAkun = 1;
            while (rdr.Read())
            {
                object[] fld = new object[rdr.FieldCount];
                rdr.GetValues(fld);
                prg.Add(fld);

                if (sKolomAkun != "")
                {
                    sKolomAkun = sKolomAkun + ",";
                    sSumKolomAkun = sSumKolomAkun + ",";
                }

                sKolomAkun = sKolomAkun + "'debet" + BarisAkun.ToString() + "' = case when dtl.kd_akun = '" + fld[0].ToString().Trim() + "' then (dtl.debet) end, 'kredit" + BarisAkun.ToString() + "' = case when  dtl.kd_akun = '" + fld[0].ToString().Trim() + "' then (dtl.kredit) end";
                sSumKolomAkun = sSumKolomAkun + "isnull(sum(debet" + BarisAkun.ToString() + "),0)debet" + BarisAkun.ToString() + ", isnull(sum(kredit" + BarisAkun.ToString() + "),0)kredit" + BarisAkun.ToString();

                //Tambah Kolom
                tbl.Columns.Add("d" + BarisAkun.ToString(), typeof(Decimal));

                BarisAkun++;
            }
            rdr.Close();

            //== END  Cetak Kolom Nilai Per Kelas


            sql = " select msk.kd_sekolah, msk.kelas ";

            if (sSumKolomAkun != "")
            {
                sql = sql + "," + sSumKolomAkun;
            }

            sql = sql
                  + " from ms_kelas msk "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT ks.kd_sekolah, ks.kelas ";

            if (sKolomAkun != "")
            {
                sql = sql + "," + sKolomAkun;
            }

            sql = sql
                  + "       FROM ms_kelas msk "
                  + "       inner join kelas_siswa ks "
                  + "           on ks.kelas = msk.kelas"
                  + "           and ks.kd_sekolah = msk.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                   + "      inner join ms_siswa sis"
                  + "           on sis.nis = ks.nis"
                  + "           and sis.kd_sekolah = ks.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           on sis.kd_siswa = hdr.kd_siswa"
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       inner join ac_makun mak "
                  + "           on mak.kd_akun = dtl.kd_akun"
                  + "       inner join ms_biaya bya "
                  + "           on mak.kd_akun = bya.kd_akun_kewajiban"

                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "       and hdr.th_ajar = '" + ThAjar + "'"
                  + " ) tr "
                  + "       on msk.kelas = tr.kelas "
                  + "       and msk.kd_sekolah = tr.kd_sekolah "

                  + " GROUP BY msk.kd_sekolah, msk.kelas "
                  + " order by msk.kelas ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdSekolah"] = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    baris["Kelas"] = AdnFungsi.CStr(rdr["kelas"]);

                    for (int i = 1; i < BarisAkun; i++)
                    {
                        baris["d" + i.ToString()] =Convert.ToDecimal(rdr["kredit" + i.ToString()]) - Convert.ToDecimal(rdr["debet" + i.ToString()]) ;
                    }

                    tbl.Rows.Add(baris);
                }
                rdr.Close();
            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;
        }
        public DataTable GetUtangPerSiswaPerAkunTabular(DateTime TglDr, DateTime TglSd, string KdSekolah, string ThAjar, string Kelas)
        {
            DataTable tbl = new DataTable("AppTabel");
            tbl.Columns.Add("KdSekolah", typeof(String));
            tbl.Columns.Add("Kelas", typeof(String));
            tbl.Columns.Add("Nis", typeof(String));
            tbl.Columns.Add("NmSiswa", typeof(String));

            //Cetak Kolom Nilai Per Akun Tabular
            string sKolomAkun = ""; string sSumKolomAkun = "";
            string sql =
                "select distinct mak.kd_akun "
                + " from ac_makun mak "
                + " inner join ms_biaya bya "
                + "     on mak.kd_akun = bya.kd_akun_kewajiban"
                + " order by kd_akun";

            this.cmd.CommandText = sql;
            SqlDataReader rdr = this.cmd.ExecuteReader();

            ArrayList prg = new ArrayList();
            int BarisAkun = 1;
            while (rdr.Read())
            {
                object[] fld = new object[rdr.FieldCount];
                rdr.GetValues(fld);
                prg.Add(fld);

                if (sKolomAkun != "")
                {
                    sKolomAkun = sKolomAkun + ",";
                    sSumKolomAkun = sSumKolomAkun + ",";
                }

                sKolomAkun = sKolomAkun + "'debet" + BarisAkun.ToString() + "' = case when dtl.kd_akun = '" + fld[0].ToString().Trim() + "' then (dtl.debet) end, 'kredit" + BarisAkun.ToString() + "' = case when  dtl.kd_akun = '" + fld[0].ToString().Trim() + "' then (dtl.kredit) end";
                sSumKolomAkun = sSumKolomAkun + "isnull(sum(debet" + BarisAkun.ToString() + "),0)debet" + BarisAkun.ToString() + ", isnull(sum(kredit" + BarisAkun.ToString() + "),0)kredit" + BarisAkun.ToString();

                //Tambah Kolom
                tbl.Columns.Add("d" + BarisAkun.ToString(), typeof(Decimal));

                BarisAkun++;
            }
            rdr.Close();

            //== END  Cetak Kolom Nilai Per Kelas


            sql = " select msk.kd_sekolah, msk.kelas , sis.nis, sis.nama_lengkap ";

            if (sSumKolomAkun != "")
            {
                sql = sql + "," + sSumKolomAkun;
            }

            sql = sql
                  + " from ms_kelas msk "
                  + "      inner join kelas_siswa ks "
                  + "           on ks.kelas = msk.kelas"
                  + "           and ks.kd_sekolah = msk.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                  + "           and ks.kd_sekolah = '" + KdSekolah + "'"
                  + "            and ks.kelas = '" + Kelas + "'"
                  + "      inner join ms_siswa sis"
                  + "           on sis.nis = ks.nis"
                  + "           and sis.kd_sekolah = ks.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                  + "           and ks.kd_sekolah = '" + KdSekolah + "'"
                  + "            and ks.kelas = '" + Kelas + "'"

                  + " left outer join  "
                  + " ( "
                  + "       SELECT ks.kd_sekolah, ks.kelas, sis.nis ";

            if (sKolomAkun != "")
            {
                sql = sql + "," + sKolomAkun;
            }

            sql = sql
                  + "       FROM ms_kelas msk "
                  + "       inner join kelas_siswa ks "
                  + "           on ks.kelas = msk.kelas"
                  + "           and ks.kd_sekolah = msk.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                  + "           and ks.kd_sekolah = '" + KdSekolah + "'"
                  + "            and ks.kelas = '" + Kelas + "'"
                   + "      inner join ms_siswa sis"
                  + "           on sis.nis = ks.nis"
                  + "           and sis.kd_sekolah = ks.kd_sekolah"
                  + "           and ks.th_ajar = '" + ThAjar + "'"
                  + "           and ks.kd_sekolah = '" + KdSekolah + "'"
                  + "            and ks.kelas = '" + Kelas + "'"
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           on sis.kd_siswa = hdr.kd_siswa"
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       inner join ac_makun mak "
                  + "           on mak.kd_akun = dtl.kd_akun"
                  + "       inner join ms_biaya bya "
                  + "           on mak.kd_akun = bya.kd_akun_kewajiban"

                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "       and hdr.th_ajar = '" + ThAjar + "'"
                  + " ) tr "
                  + "       on msk.kelas = tr.kelas "
                  + "       and msk.kd_sekolah = tr.kd_sekolah "
                  + "       and sis.nis = tr.nis "

                  + " GROUP BY msk.kd_sekolah, msk.kelas, sis.nis, sis.nama_lengkap "
                  + " order by msk.kd_sekolah, msk.kelas, sis.nama_lengkap ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdSekolah"] = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    baris["Kelas"] = AdnFungsi.CStr(rdr["kelas"]);
                    baris["Nis"] = AdnFungsi.CStr(rdr["nis"]);
                    baris["NmSiswa"] = AdnFungsi.CStr(rdr["nama_lengkap"]);

                    for (int i = 1; i < BarisAkun; i++)
                    {
                        baris["d" + i.ToString()] = Convert.ToDecimal(rdr["kredit" + i.ToString()]) - Convert.ToDecimal(rdr["debet" + i.ToString()]);
                    }

                    tbl.Rows.Add(baris);
                }
                rdr.Close();
            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;
        }

        public DataTable GetLapSaldoPendapatan(DateTime TglDr, DateTime TglSd, DateTime PeriodeMulai, string ThAjar)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("KdSekolah", typeof(String));
            tbl.Columns.Add("NmSekolah", typeof(String));
            tbl.Columns.Add("SaldoAwal", typeof(Decimal));
            tbl.Columns.Add("Penerimaan", typeof(Decimal));
            tbl.Columns.Add("Pengeluaran", typeof(Decimal));
            tbl.Columns.Add("LainLain", typeof(Decimal));
            tbl.Columns.Add("Saldo", typeof(Decimal));

            // Saldo Awal ------------------------------------------------------------------------------
            string sql =
            " select mak.kd_akun, mak.nm_akun, mss.kd_sekolah,mss.nama_sekolah,"
            + " (isnull(tra.penerimaan,0) + isnull(trc.penerimaan,0)) penerimaan, isnull(trb.pengeluaran,0)pengeluaran"
            + " from ac_makun mak"
            + " cross join ms_sekolah mss"

            //Penerimaan Loket
            + " left outer join"
            + " ("
            + " select dtl.kd_akun, dtl.kd_dept, sum(kredit-debet) penerimaan"
            + " from ac_tjurnal hdr"
            + " inner join ac_tjurnal_dtl dtl"
            + "     on hdr.kd_jurnal=dtl.kd_jurnal"
            + " inner join ac_makun mak"
            + "     on dtl.kd_akun = mak.kd_akun"
            + " where mak.kd_gol = 'PDT'"
            + "     and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglDr) + "'"
            + "     and (hdr.sumber = 'LOKET' OR hdr.sumber = 'LOKET PSB')";

            if (ThAjar != "")
            {
                sql += " AND hdr.th_ajar_tagihan = '" + ThAjar + "'";
            }

            sql = sql + " group by dtl.kd_akun, dtl.kd_dept"
            + " ) tra"
            + " on mak.kd_akun = tra.kd_akun"
            + " and mss.kd_sekolah = tra.kd_dept"

            //Pengeluaran
            + " left outer join"
            + " ("
            + " select sumber_dana,dtl.kd_dept, sum(dtl.debet) pengeluaran"
            + " from ac_tjurnal hdr"
            + " inner join ac_tjurnal_dtl dtl"
            + "     on hdr.kd_jurnal = dtl.kd_jurnal"
            + " where sumber_dana is not null and LEN(ltrim(rtrim(sumber_dana)))>0"
            + " and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglDr) + "'";

            if (ThAjar != "")
            {
                sql += " AND hdr.th_ajar = '" + ThAjar + "'";
            }

            sql = sql + " group by sumber_dana,dtl.kd_dept"
            + " ) trb"
            + " on mak.kd_akun = trb.sumber_dana"
            + " and mss.kd_sekolah = trb.kd_dept"

            //Penerimaan Non Loket
            + " left outer join"
            + " ("
            + " select dtl.kd_akun, dtl.kd_dept, sum(kredit-debet) penerimaan"
            + " from ac_tjurnal hdr"
            + " inner join ac_tjurnal_dtl dtl"
            + " on hdr.kd_jurnal=dtl.kd_jurnal"
            + " inner join ac_makun mak"
            + " on dtl.kd_akun = mak.kd_akun"
            + " where mak.kd_gol = 'PDT'"
            + "     and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglDr) + "'"
            + "     and (not (hdr.sumber = 'LOKET' OR hdr.sumber = 'LOKET PSB') OR hdr.sumber is null)";

            if (ThAjar != "")
            {
                sql += " AND hdr.th_ajar = '" + ThAjar + "'";
            }

            sql = sql + " group by dtl.kd_akun, dtl.kd_dept"
            + " ) trc"
            + " on mak.kd_akun = trc.kd_akun"
            + " and mss.kd_sekolah = trc.kd_dept"


            + " where mak.kd_gol = 'PDT'"
            + " and mak.tipe ='DTL'"
            + " order by mak.nm_akun, mss.kd_sekolah ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["KdSekolah"] = AdnFungsi.CStr(rdr["kd_sekolah"]);
                baris["NmSekolah"] = AdnFungsi.CStr(rdr["nama_sekolah"]);
                baris["SaldoAwal"] = AdnFungsi.CDec(rdr["Penerimaan"]) - AdnFungsi.CDec(rdr["Pengeluaran"]);
                baris["Penerimaan"] = 0;
                baris["Pengeluaran"] = 0;
                baris["LainLain"] = 0;
                baris["Saldo"] = 0;

                tbl.Rows.Add(baris);
            }
            rdr.Close();

            // END Saldo Awal------------------------------------------------------------------------------
            if (tbl.Rows.Count > 0)
            {
                // Mutasi------------------------------------------------------------------------------
                sql =
                " select mak.kd_akun, mak.nm_akun, mss.kd_sekolah,mss.nama_sekolah,"
                + " isnull(tra.penerimaan,0) penerimaan, isnull(trb.pengeluaran,0)pengeluaran, isnull(trc.lain,0) lain"
                + " from ac_makun mak"
                + " cross join ms_sekolah mss"

                + " left outer join"
                + " ("
                + " select dtl.kd_akun, dtl.kd_dept, sum(kredit) penerimaan"
                + " from ac_tjurnal hdr"
                + " inner join ac_tjurnal_dtl dtl"
                + " on hdr.kd_jurnal=dtl.kd_jurnal"
                + " inner join ac_makun mak"
                + " on dtl.kd_akun = mak.kd_akun"
                + " where mak.kd_gol = 'PDT'"
                + "     and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                + "     and (hdr.sumber = 'LOKET' OR hdr.sumber = 'LOKET PSB')";

                if (ThAjar != "")
                {
                    sql += " AND hdr.th_ajar_tagihan = '" + ThAjar + "'";
                }

                sql = sql + " group by dtl.kd_akun, dtl.kd_dept"
                + " ) tra"
                + " on mak.kd_akun = tra.kd_akun"
                + " and mss.kd_sekolah = tra.kd_dept"
                + " left outer join"
                + " ("
                + " select sumber_dana,dtl.kd_dept, sum(dtl.debet) pengeluaran"
                + " from ac_tjurnal hdr"
                + " inner join ac_tjurnal_dtl dtl"
                + " on hdr.kd_jurnal = dtl.kd_jurnal"
                + " where sumber_dana is not null and LEN(ltrim(rtrim(sumber_dana)))>0"
                + " and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

                if (ThAjar != "")
                {
                    sql += " AND hdr.th_ajar = '" + ThAjar + "'";
                }


                sql = sql + " group by sumber_dana,dtl.kd_dept"
                + " ) trb"
                + " on mak.kd_akun = trb.sumber_dana"
                + " and mss.kd_sekolah = trb.kd_dept"

                + " left outer join"
                + " ("
                + " select dtl.kd_akun, dtl.kd_dept, sum(kredit) lain"
                + " from ac_tjurnal hdr"
                + " inner join ac_tjurnal_dtl dtl"
                + " on hdr.kd_jurnal=dtl.kd_jurnal"
                + " inner join ac_makun mak"
                + " on dtl.kd_akun = mak.kd_akun"
                + " where mak.kd_gol = 'PDT'";

                if (TglDr.Date == PeriodeMulai)
                {
                    sql += " and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr.AddDays(1)) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";
                }
                else
                {
                    sql+=" and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";
                }

                if (ThAjar != "")
                {
                    sql += " AND hdr.th_ajar = '" + ThAjar + "'";
                }


                sql += " and (not (hdr.sumber = 'LOKET' OR hdr.sumber = 'LOKET PSB') OR hdr.sumber is null)"
                + " group by dtl.kd_akun, dtl.kd_dept"
                + " ) trc"
                + " on mak.kd_akun = trc.kd_akun"
                + " and mss.kd_sekolah = trc.kd_dept"


                + " where mak.kd_gol = 'PDT'"
                + " and mak.tipe ='DTL'"
                + " order by mak.nm_akun, mss.kd_sekolah ";

                cmd = new SqlCommand(sql, this.cnn);
                rdr = cmd.ExecuteReader();
                int idx = 0;
                while (rdr.Read())
                {
                    tbl.Rows[idx]["Penerimaan"] = AdnFungsi.CDec(rdr["penerimaan"]);
                    tbl.Rows[idx]["Pengeluaran"] = AdnFungsi.CDec(rdr["pengeluaran"]);
                    tbl.Rows[idx]["LainLain"] = AdnFungsi.CDec(rdr["lain"]);
                    tbl.Rows[idx]["Saldo"] = AdnFungsi.CDec(tbl.Rows[idx]["SaldoAwal"]) + AdnFungsi.CDec(rdr["penerimaan"]) - AdnFungsi.CDec(rdr["pengeluaran"]) + AdnFungsi.CDec(rdr["lain"]);
                    idx++;
                }
                rdr.Close();
            }
            // END Mutasi------------------------------------------------------------------------------


            return tbl; ;
        }
        public DataTable GetLapSaldoPendapatanUMSaw(DateTime TglDr, DateTime TglSd, DateTime PeriodeMulai, string ThAjar,string KdAkunUMSiswa)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("KdSekolah", typeof(String));
            tbl.Columns.Add("NmSekolah", typeof(String));
            tbl.Columns.Add("SaldoAwal", typeof(Decimal));
            tbl.Columns.Add("Penerimaan", typeof(Decimal));
            tbl.Columns.Add("Pengeluaran", typeof(Decimal));
            tbl.Columns.Add("LainLain", typeof(Decimal));
            tbl.Columns.Add("Saldo", typeof(Decimal));

            // Saldo Awal ------------------------------------------------------------------------------
            string sql =
            " select mak.kd_akun, mak.nm_akun,"
            + " isnull(tra.penerimaan,0) penerimaan " //, isnull(trb.pengeluaran,0)pengeluaran"
            + " from ac_makun mak"

            + " left outer join"
            + " ("
            + " select dtl.kd_akun, sum(kredit-debet) penerimaan"
            + " from ac_tjurnal hdr"
            + " inner join ac_tjurnal_dtl dtl"
            + " on hdr.kd_jurnal=dtl.kd_jurnal"
            + " inner join ac_makun mak"
            + " on dtl.kd_akun = mak.kd_akun"
            + " where mak.kd_akun = '" + KdAkunUMSiswa + "'"
            + " and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglDr.AddDays(1)) + "'";

            if (ThAjar != "")
            {
                sql += " AND hdr.th_ajar_tagihan = '" + ThAjar + "'";
            }

            sql = sql + " group by dtl.kd_akun"
            + " ) tra"
            + " on mak.kd_akun = tra.kd_akun"
            
            //+ " left outer join"
            //+ " ("
            //+ " select sumber_dana,dtl.kd_dept, sum(dtl.debet) pengeluaran"
            //+ " from ac_tjurnal hdr"
            //+ " inner join ac_tjurnal_dtl dtl"
            //+ " on hdr.kd_jurnal = dtl.kd_jurnal"
            //+ " where sumber_dana is not null and LEN(ltrim(rtrim(sumber_dana)))>0"
            //+ " where mak.kd_akun = '" + KdAkunUMSiswa + "'"
            //+ " and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglDr.AddDays(1)) + "'";

            //if (ThAjar != "")
            //{
            //    sql += " AND hdr.th_ajar = '" + ThAjar + "'";
            //}


            //sql = sql + " group by sumber_dana,dtl.kd_dept"
            //+ " ) trb"
            //+ " on mak.kd_akun = trb.sumber_dana"
            //+ " and mss.kd_sekolah = trb.kd_dept"

            + " where mak.kd_akun = '" + KdAkunUMSiswa + "'"
            + " order by mak.nm_akun ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["KdSekolah"] = "";
                baris["NmSekolah"] = "";
                baris["SaldoAwal"] = AdnFungsi.CDec(rdr["Penerimaan"]);// -AdnFungsi.CDec(rdr["Pengeluaran"]);
                baris["Penerimaan"] = 0;
                baris["Pengeluaran"] = 0;
                baris["LainLain"] = 0;
                baris["Saldo"] = 0;

                tbl.Rows.Add(baris);
            }
            rdr.Close();

            // END Saldo Awal------------------------------------------------------------------------------
            if (tbl.Rows.Count > 0)
            {
                // Mutasi------------------------------------------------------------------------------
                sql =
                " select mak.kd_akun, mak.nm_akun, "
                + " isnull(tra.penerimaan,0) penerimaan, isnull(trb.pengeluaran,0)pengeluaran, 0 as lain"
                + " from ac_makun mak"

                + " left outer join"
                + " ("
                + " select dtl.kd_akun, sum(kredit) penerimaan"
                + " from ac_tjurnal hdr"
                + " inner join ac_tjurnal_dtl dtl"
                + " on hdr.kd_jurnal=dtl.kd_jurnal"
                + " inner join ac_makun mak"
                + " on dtl.kd_akun = mak.kd_akun"
                + " where mak.kd_akun = '" + KdAkunUMSiswa + "'"
                + "     and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";
                //+ "     and (hdr.sumber = 'LOKET' OR hdr.sumber = 'LOKET PSB')";

                if (ThAjar != "")
                {
                    sql += " AND hdr.th_ajar_tagihan = '" + ThAjar + "'";
                }

                sql = sql + " group by dtl.kd_akun "
                + " ) tra"
                + " on mak.kd_akun = tra.kd_akun"
 
                + " left outer join"
                + " ("
                + " select dtl.kd_akun, sum(dtl.debet) pengeluaran"
                + " from ac_tjurnal hdr"
                + " inner join ac_tjurnal_dtl dtl"
                + "     on hdr.kd_jurnal = dtl.kd_jurnal"
                + " where dtl.kd_akun = '" + KdAkunUMSiswa + "'"
                + " and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

                if (ThAjar != "")
                {
                    sql += " AND hdr.th_ajar_tagihan = '" + ThAjar + "'";
                }

                sql = sql + " group by dtl.kd_akun "
                + " ) trb"
                + " on mak.kd_akun = trb.kd_akun"


                //+ " left outer join"
                //+ " ("
                //+ " select dtl.kd_akun, dtl.kd_dept, sum(kredit) lain"
                //+ " from ac_tjurnal hdr"
                //+ " inner join ac_tjurnal_dtl dtl"
                //+ " on hdr.kd_jurnal=dtl.kd_jurnal"
                //+ " inner join ac_makun mak"
                //+ " on dtl.kd_akun = mak.kd_akun"
                //+ " where mak.kd_gol = 'PDT'";

                //if (TglDr.Date == PeriodeMulai)
                //{
                //    sql += " and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr.AddDays(1)) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";
                //}
                //else
                //{
                //    sql += " and hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' and hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";
                //}

                //if (ThAjar != "")
                //{
                //    sql += " AND hdr.th_ajar = '" + ThAjar + "'";
                //}


                //sql += " and not (hdr.sumber = 'LOKET' OR hdr.sumber = 'LOKET PSB')"
                //+ " group by dtl.kd_akun, dtl.kd_dept"
                //+ " ) trc"
                //+ " on mak.kd_akun = trc.kd_akun"
                //+ " and mss.kd_sekolah = trc.kd_dept"


                + " where mak.kd_akun = '" + KdAkunUMSiswa + "'"
                + " order by mak.nm_akun ";

                cmd = new SqlCommand(sql, this.cnn);
                rdr = cmd.ExecuteReader();
                int idx = 0;
                while (rdr.Read())
                {
                    tbl.Rows[idx]["Penerimaan"] = AdnFungsi.CDec(rdr["penerimaan"]);
                    tbl.Rows[idx]["Pengeluaran"] = AdnFungsi.CDec(rdr["pengeluaran"]);
                    tbl.Rows[idx]["LainLain"] = AdnFungsi.CDec(rdr["lain"]);
                    tbl.Rows[idx]["Saldo"] = AdnFungsi.CDec(tbl.Rows[idx]["SaldoAwal"]) + AdnFungsi.CDec(rdr["penerimaan"]) - AdnFungsi.CDec(rdr["pengeluaran"]) + AdnFungsi.CDec(rdr["lain"]);
                    idx++;
                }
                rdr.Close();
            }
            // END Mutasi------------------------------------------------------------------------------


            return tbl; ;
        }
        public DataTable GetLapPenerimaanPerPeriode(DateTime TglDr, DateTime TglSd, string KdProject, string KdAkun, string KdDept)
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
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Memo", typeof(String));

            string sql =
            " select tgl,  deskripsi, dtl.memo, hdr.kd_jurnal, dtl.kd_project,dtl.kd_dept, nm_dept, "
            + "     dtl.kd_akun, nm_akun, debet, kredit, nm_project, mak.dk "
            + " from ac_tjurnal hdr"
            + " inner join ac_tjurnal_dtl dtl "
            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
            + " inner join ac_makun mak "
            + "     on mak.kd_akun = dtl.kd_akun "
            + " left outer join ac_mproject prj "
            + "     on dtl.kd_project = prj.kd_project "
            + " left outer join ac_mdept dpt "
            + "     on dpt.kd_dept = dtl.kd_dept "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
            + "     AND dtl.sumber_dana = '" + KdAkun.ToString().Trim() + "'";

            if (KdProject.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_project = '" + KdProject.ToString().Trim() + "'";
            }

            if (KdDept.ToString().Trim() != "")
            {
                sql = sql + " and dtl.kd_dept = '" + KdDept.ToString().Trim() + "'";
            }

            sql = sql + " ORDER BY tgl ";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoBukti"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["KdProject"] = AdnFungsi.CStr(rdr["kd_project"]);
                baris["NmProject"] = AdnFungsi.CStr(rdr["nm_project"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["NmDept"] = AdnFungsi.CStr(rdr["nm_dept"]);

                if (AdnFungsi.CStr(rdr["memo"]) != "")
                {
                    baris["Memo"] = AdnFungsi.CStr(rdr["memo"]);
                }
                else
                {
                    baris["Memo"] = AdnFungsi.CStr(rdr["deskripsi"]);
                }

                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["DK"] = AdnFungsi.CStr(rdr["dk"]);
                baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                tbl.Rows.Add(baris);
            }

            rdr.Close();
            return tbl; ;
        }

        public string BatchJurnalGaji(DateTime Tgl, string Periode, string Kas, string KdAkunGaji)
        {
            string Pesan = "";

            DataTable tbl = new DataTable("AppTabel");
            DataRow row;
            tbl.Columns.Add("Nis", typeof(string));
            tbl.Columns.Add("ThAjar", typeof(string));
            tbl.Columns.Add("KdSekolah", typeof(string));
            tbl.Columns.Add("NoKwitansi", typeof(string));
            tbl.Columns.Add("KdBiaya", typeof(string));
            tbl.Columns.Add("Ket", typeof(string));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Jmh", typeof(Decimal));
            tbl.Columns.Add("Status", typeof(string));

            //-------------- Menentukan No Bukti
            string KdJurnal = "";
            string sPrefix = "GAJ" + Tgl.ToString("yy") + Tgl.ToString("MM");
            string sql =
            "SELECT isnull(max(right(rtrim(kd_jurnal),4)),0) as kd  "
            + " FROM ac_tjurnal "
            + " WHERE left(kd_jurnal, 7)= '" + sPrefix + "'";

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            KdJurnal = sPrefix + iMax.ToString().PadLeft(4, '0');
            //=============== END 

            sql =
            " SELECT dep.kd_dept, sum(jmh) jmh "
            + " FROM gaji_karyawan gaj "
            + " INNER JOIN ms_karyawan kar "
            + "     ON gaj.nip = kar.nip "
            + " INNER JOIN ac_mdept dep "
            + "     ON dep.kd_dept = kar.unit_kerja "
            + " WHERE periode = '" + Periode + "'"
            + "     AND gaj.posting = 0 "
            + " GROUP BY dep.kd_dept ";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            List<AdnJurnal> lstJurnal = new List<AdnJurnal>();

            int cacah = 0;
            string LastKdDept = "";

            AdnJurnal o = null;

            List<AdnJurnalDtl> lstItem = null;
            //ArrayList ListKM = new ArrayList();
            while (rdr.Read())
            {
                if (LastKdDept != AdnFungsi.CStr(rdr["kd_dept"]))
                {
                    if (LastKdDept != "")//Bukan Pertama Kali
                    {
                        o.ItemDf = lstItem;
                        lstJurnal.Add(o);

                        iMax++;
                        KdJurnal = sPrefix + iMax.ToString().PadLeft(3, '0');
                    }

                    LastKdDept = AdnFungsi.CStr(rdr["kd_dept"]);

                    o = new AdnJurnal();
                    lstItem = new List<AdnJurnalDtl>();

                    o.KdJurnal = KdJurnal;
                    o.Tgl = Tgl;
                    o.Sumber = "GAJI";
                    o.Deskripsi = " Pembayaran Gaji, #" + LastKdDept + "#, Periode #" + Periode + "#";
                    o.JenisJurnal = AdnJurnalVar.JenisJurnal.JGAJ;

                    o.ThAjar = "";
                    o.ThAjarTagihan = "";
                    o.KdSiswa = 0;
                    o.Nis = "";
                    o.KdSekolah = LastKdDept;
                    o.NoKwitansi = "";


                    //Debet
                    AdnJurnalDtl itemDebet = new AdnJurnalDtl();

                    itemDebet.KdJurnal = o.KdJurnal;
                    itemDebet.KdAkun = KdAkunGaji;
                    itemDebet.NoUrut = cacah; cacah++;
                    itemDebet.Memo = " Pembayaran Gaji, #" + LastKdDept + "#, Periode #" + Periode + "#";
                    itemDebet.Debet = AdnFungsi.CDec(rdr["jmh"]);
                    itemDebet.Kredit = 0;

                    lstItem.Add(itemDebet);

                }

                AdnJurnalDtl itemKredit = new AdnJurnalDtl();

                itemKredit.KdJurnal = KdJurnal;
                itemKredit.KdAkun = Kas;
                itemKredit.NoUrut = cacah; cacah++;
                itemKredit.Debet = 0;
                itemKredit.Kredit = AdnFungsi.CDec(rdr["jmh"]);
                itemKredit.KdDept = "";
                itemKredit.Memo = " Pembayaran Gaji, #" + LastKdDept + "#, Periode #" + Periode + "#";

                lstItem.Add(itemKredit);
            }

            if (o != null)
            {
                o.ItemDf = lstItem;
                lstJurnal.Add(o);
            }

            rdr.Close();
            AdnJurnalDao dao = new AdnJurnalDao(this.cnn, this.pengguna, this.trn);
            foreach (AdnJurnal item in lstJurnal)
            {
                dao.Simpan(item);
                sql = "UPDATE gaji_karyawan  "
                + " SET posting = 1"
                + " WHERE periode  ='" + Periode + "'";

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                Pesan = "Berhasil";
            }

            return Pesan;

        }

        
    }
}
