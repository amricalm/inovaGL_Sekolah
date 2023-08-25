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
    public class AdnKasKeluarDao
    {
        private const short JUMLAH_KOLOM = 6;
        private const string NAMA_TABEL = "ac_tkk";
        
        private string pkey = "kd_tkk";
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

        public AdnKasKeluarDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnKasKeluarDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnKasKeluarDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }

        private void SetFldNilai(AdnKasKeluar o)
        {
            short idx = 0;

            fld[idx] = "kd_tkk"; nilai[idx] = o.KdKK.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl"; nilai[idx] = o.Tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "kpd"; nilai[idx] = o.Kepada.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "deskripsi"; nilai[idx] = o.Deskripsi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_jurnal"; nilai[idx] = o.KdJurnal.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnKasKeluar o)
        {
            if (o.KdKK.Trim() == "")
            {
                o.KdKK = this.GetNoBukti(o.Tgl, o.ThAjar);
                o.KdJurnal = o.KdKK;
            }

            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                foreach (AdnKasKeluarDtl item in o.ItemDf)
                {
                    item.KdKK = o.KdKK;
                    new AdnKasKeluarDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
                }
            }
            catch(DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
        }
        public void Update(AdnKasKeluar o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdKK + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();


                new AdnKasKeluarDtlDao(this.cnn,this.pengguna,this.trn).Hapus(o.KdKK);

                foreach (AdnKasKeluarDtl item in o.ItemDf)
                {
                    new AdnKasKeluarDtlDao(this.cnn, this.pengguna, this.trn).Simpan(item);
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

        public AdnKasKeluar Get(string kd)
        {
            AdnKasKeluar o = null;
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
                    o = new AdnKasKeluar();
                    o.KdKK = kd;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Kepada = AdnFungsi.CStr(rdr["kpd"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.KdJurnal = AdnFungsi.CStr(rdr["kd_jurnal"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                }
                rdr.Close();
                if (o != null)
                {
                    o.ItemDf = new AdnKasKeluarDtlDao(this.cnn).Get(kd);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString().Trim());
            }
            return o;
        }
        public List<AdnKasKeluar> GetAll()
        {
            List<AdnKasKeluar> lst = new List<AdnKasKeluar>();
            sql =
            " select kd_tkk, tgl, kepada, deskripsi, kd_jurnal "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnKasKeluar o = new AdnKasKeluar();
                    o.KdKK = AdnFungsi.CStr(rdr["kd_tkk"]) ;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Kepada = AdnFungsi.CStr(rdr["kepada"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.KdJurnal = AdnFungsi.CStr(rdr["kd_jurnal"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
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

            tbl.Columns.Add("KdKK", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Kepada", typeof(String));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdJurnal", typeof(String));
            tbl.Columns.Add("Jmh", typeof(Decimal));

            string sql =
            " select ac_tkk.kd_tkk, tgl, kpd, deskripsi, kd_jurnal, jmh "
            + " from " + NAMA_TABEL
            + " inner join "
            + " ( "
            + "     select kd_tkk, sum(kredit) jmh "
            + "     from ac_tkk_dtl "
            + "     where kredit>0 "
            + "     group by kd_tkk "
            + " ) dtl "
            + " on ac_tkk.kd_tkk = dtl.kd_tkk ";

            if (sFilter != "")
            {
                sql = sql + " WHERE " + sFilter;
            }

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdKK"] = AdnFungsi.CStr(rdr["kd_tkk"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Kepada"] = AdnFungsi.CStr(rdr["kpd"]);
                baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                baris["KdJurnal"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["Jmh"] = AdnFungsi.CDec(rdr["jmh"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetLapBKK(string NoBKK)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdKK", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Kepada", typeof(String));
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

            tbl.Columns.Add("SumberDana", typeof(String));

            string sql =
            " select hdr.kd_tkk, tgl, kpd, deskripsi, kd_jurnal, "
            + "     dtl.kd_akun, mak.nm_akun, dtl.kd_project, debet, kredit "
            + "     ,dtl.kd_dept, dtl.memo, nm_dept "
            + "     , sdn.nm_akun as sumber_dana "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tkk_dtl dtl "
            + "     on hdr.kd_tkk = dtl.kd_tkk "
            + " inner join ac_makun mak "
            + "     on dtl.kd_akun = mak.kd_akun "
            + " left outer join ac_mdept "
            + "     on dtl.kd_dept = ac_mdept.kd_dept"
            + " left outer join ac_makun sdn "
            + "     on dtl.sumber_dana = sdn.kd_akun "
            + " where hdr.kd_tkk = '" + NoBKK.ToString().Trim() + "'"
            + " order by dtl.no_urut ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdKK"] = AdnFungsi.CStr(rdr["kd_tkk"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Kepada"] = AdnFungsi.CStr(rdr["kpd"]);
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
                baris["SumberDana"] = AdnFungsi.CStr(rdr["sumber_dana"]);


                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_tkk";
            string KolomDisplay = "deskripsi";

            string Value = "KdKK";
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

        public string GetNoBukti(DateTime Tgl)
        {
            string periode = Tgl.Year.ToString().Substring(2) + Tgl.Month.ToString("00");
            string kode = "";

            sql =
            "SELECT isnull(max(right(kd_tkk,3)),0) as kd  "
            + " FROM " + NAMA_TABEL
            + " where left(kd_tkk,7) ='BKK" + periode.Trim() + "'";

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            kode = "BKK" + periode + iMax.ToString("000");

            return kode;
        }
        public string GetNoBukti(DateTime Tgl, string ThAjar)
        {
            string periode = ThAjar.Substring(2, 2).ToString() + ThAjar.Substring(7, 2); //Tgl.Year.ToString().Substring(2) + Tgl.Month.ToString("00");
            string kode = "";

            sql =
            "SELECT isnull(max(right(kd_tkk,5)),0) as kd  "
            + " FROM " + NAMA_TABEL
            + " where left(kd_tkk,7) ='BKK" + periode.Trim() + "'";

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            kode = "BKK" + periode + iMax.ToString("00000");

            return kode;
        }
    }
}
