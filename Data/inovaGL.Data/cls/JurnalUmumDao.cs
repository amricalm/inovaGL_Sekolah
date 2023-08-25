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
    public class AdnJurnalUmumDao
    {
        private const short JUMLAH_KOLOM = 9;
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

            fld[idx] = "tag"; nilai[idx] = o.Tag.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "periode"; nilai[idx] = o.Periode.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "sumber"; nilai[idx] = o.Sumber.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "jn_jurnal"; nilai[idx] = o.JenisJurnal.ToString(); tipe[idx] = "s"; idx++;

            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnJurnalUmum o)
        {
            if (o.KdJU.Trim() == "")
            {
                o.KdJU = this.GetNoBukti(o.Tgl, o.ThAjar);
                o.KdJurnal = o.KdJU;
            }

            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            //try
            //{
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                foreach (AdnJurnalUmumDtl item in o.ItemDf)
                {
                    item.KdJU = o.KdJU;
                    new AdnJurnalUmumDtlDao(this.cnn, this.pengguna, this.trn).Simpan(item);
                }

            //}
            //catch(DbException exp)
            //{
            //    throw new Exception(exp.Message.ToString());
            //}
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

                    o.Tag = AdnFungsi.CStr(rdr["tag"]);
                    o.Periode= AdnFungsi.CStr(rdr["periode"]);
                    o.JenisJurnal = AdnFungsi.CStr(rdr["jn_jurnal"]);
                    o.Sumber = AdnFungsi.CStr(rdr["sumber"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                }
                rdr.Close();
                if(o!=null)
                {
                    o.ItemDf = new AdnJurnalUmumDtlDao(this.cnn).Get(kd);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString().Trim());
            }
            return o;
        }
        public List<AdnJurnalUmum> GetAll()
        {
            List<AdnJurnalUmum> lst = new List<AdnJurnalUmum>();
            sql =
            " select kd_tju, tgl,  deskripsi, tag,sumber,jn_jurnal,periode "
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

                    o.Tag = AdnFungsi.CStr(rdr["tag"]);
                    o.Periode = AdnFungsi.CStr(rdr["periode"]);
                    o.JenisJurnal = AdnFungsi.CStr(rdr["jn_jurnal"]);
                    o.Sumber = AdnFungsi.CStr(rdr["sumber"]);

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
        public DataTable GetDf(DateTime TglDr, DateTime TglSd, string JenisJurnal)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdJU", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Kepada", typeof(String));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdJurnal", typeof(String));
            tbl.Columns.Add("Jmh", typeof(Decimal));

            tbl.Columns.Add("Tag", typeof(String));
            tbl.Columns.Add("Sumber", typeof(String));
            tbl.Columns.Add("Periode", typeof(String));
            tbl.Columns.Add("JenisJurnal", typeof(String));

            string sql =
            " select ac_tju.kd_tju, tgl, deskripsi, kd_jurnal, tag, sumber, periode, jn_jurnal,jmh "
            + " from " + NAMA_TABEL

            + " inner join "
            + " ( "
            + "     select kd_tju, sum(debet) jmh "
            + "     from ac_tju_dtl "
            + "     where debet>0 "
            + "     group by kd_tju "
            + " ) dtl "
            + " on ac_tju.kd_tju = dtl.kd_tju ";

            sql = sql + " WHERE tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND tgl < '" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            if (JenisJurnal.ToString().Trim() != "")
            {
                sql+=" AND jn_jurnal = '" + JenisJurnal.ToString().Trim() + "'";
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
                baris["Jmh"] = AdnFungsi.CDec(rdr["jmh"]);

                baris["Tag"] = AdnFungsi.CStr(rdr["tag"]);
                baris["Periode"] = AdnFungsi.CStr(rdr["periode"]);
                baris["JenisJurnal"] = AdnFungsi.CStr(rdr["jn_jurnal"]);
                baris["Sumber"] = AdnFungsi.CStr(rdr["sumber"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

        public string GetNoBukti(DateTime Tgl)
        {
            string periode = Tgl.Year.ToString().Substring(2) + Tgl.Month.ToString("00");
            string kode = "";

            sql =
            "SELECT isnull(max(right(kd_tju,3)),0) as kd  "
            + " FROM " + NAMA_TABEL
            + " where left(kd_tju,7) ='BJU" + periode.Trim() + "'";

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            kode = "BJU" + periode + iMax.ToString("000");

            return kode;
        }
        public string GetNoBukti(DateTime Tgl, string ThAjar)
        {
            string periode = ThAjar.Substring(2, 2).ToString() + ThAjar.Substring(7, 2); //Tgl.Year.ToString().Substring(2) + Tgl.Month.ToString("00");
            string kode = "";

            sql =
            "SELECT isnull(max(right(kd_tju,5)),0) as kd  "
            + " FROM " + NAMA_TABEL
            + " where left(kd_tju,7) ='BJU" + periode.Trim() + "'";

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            kode = "BJU" + periode + iMax.ToString("00000");

            return kode;
        }
        public DataTable GetLapBJU(string NoBJU)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdJU", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdJurnal", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));

            tbl.Columns.Add("KdProject", typeof(String));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("Memo", typeof(String));
            tbl.Columns.Add("NmDept", typeof(String));

            string sql =
            " select hdr.kd_tju, tgl,  deskripsi, kd_jurnal, "
            + "     dtl.kd_akun, nm_akun, dtl.kd_project, debet, kredit "
            + "     ,dtl.kd_dept, dtl.memo, nm_dept "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tju_dtl dtl "
            + "     on hdr.kd_tju = dtl.kd_tju "
            + " inner join ac_makun mak "
            + "     on dtl.kd_akun = mak.kd_akun "
            + " left outer join ac_mdept "
            + "     on dtl.kd_dept = ac_mdept.kd_dept"
            + " where hdr.kd_tju = '" + NoBJU.ToString().Trim() + "'"
            + " order by dtl.no_urut ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdJU"] = AdnFungsi.CStr(rdr["kd_tju"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                baris["KdJurnal"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);
                baris["KdProject"] = AdnFungsi.CStr(rdr["kd_project"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["Memo"] = AdnFungsi.CStr(rdr["memo"]);
                baris["NmDept"] = AdnFungsi.CStr(rdr["nm_dept"]);


                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_tju";
            string KolomDisplay = "deskripsi";

            string Value = "KdJU";
            string Display = "Deskripsi";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " ORDER BY " + KolomValue;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
    }
}
