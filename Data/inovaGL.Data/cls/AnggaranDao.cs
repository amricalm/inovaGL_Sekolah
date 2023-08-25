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
    public class AdnAnggaranDao
    {
        private const short JUMLAH_KOLOM = 5;
        private const string NAMA_TABEL = "ac_tanggaran";
        
        private string pkey = "kd_akun";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnAnggaranDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnAnggaranDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnAnggaran o)
        {
            short idx = 0;

            fld[idx] = "kd_akun"; nilai[idx] = o.KdAkun.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "bulan"; nilai[idx] = o.Bulan.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "nilai"; nilai[idx] = o.Nilai.ToString(); tipe[idx] = "n"; idx++;
        }

        public void Simpan(AdnAnggaran o)
        {
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
        public int Update(AdnAnggaran o)
        {
            int Hasil = 0;
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdAkun + "' AND th_ajar ='" + o.ThAjar.Trim() + "' AND kd_sekolah ='" + o.KdSekolah + "'   AND bulan =" + o.Bulan ;
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                Hasil=cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return Hasil;
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

        public List<AdnAnggaran> Get(string ThAjar)
        {
            List<AdnAnggaran> lst = new List<AdnAnggaran>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnAnggaran o = new AdnAnggaran();
                    o.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]) ;
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.Bulan = AdnFungsi.CInt(rdr["bulan"],true);
                    o.Nilai = AdnFungsi.CDec(rdr["nilai"]);
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

        public DataTable GetDf(string ThAjar, string KdSekolah)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Total", typeof(Decimal));
            tbl.Columns.Add("Jul", typeof(Decimal));
            tbl.Columns.Add("Agt", typeof(Decimal));
            tbl.Columns.Add("Sep", typeof(Decimal));
            tbl.Columns.Add("Okt", typeof(Decimal));
            tbl.Columns.Add("Nov", typeof(Decimal));
            tbl.Columns.Add("Des", typeof(Decimal));
            tbl.Columns.Add("Jan", typeof(Decimal));
            tbl.Columns.Add("Feb", typeof(Decimal));
            tbl.Columns.Add("Mar", typeof(Decimal));
            tbl.Columns.Add("Apr", typeof(Decimal));
            tbl.Columns.Add("Mei", typeof(Decimal));
            tbl.Columns.Add("Jun", typeof(Decimal));

            string sql =
            " select mak.kd_akun, nm_akun,"
            + " SUM(b1) b1,SUM(b2) b2,SUM(b3) b3,SUM(b4) b4,SUM(b5) b5,"
            + " SUM(b6) b6,SUM(b7) b7,SUM(b8) b8,SUM(b9) b9,SUM(b10) b10,SUM(b11) b11,SUM(b12) b12,"
            + " sum(isnull(b1,0)+isnull(b2,0)+isnull(b3,0)+isnull(b4,0)+isnull(b5,0)+isnull(b6,0)"
            + " +isnull(b7,0)+isnull(b8,0)+isnull(b9,0)+isnull(b10,0)+isnull(b11,0)+isnull(b12,0)) total"
            + " from ac_makun mak"
            + " left outer join"
            + " ("
                + " SELECT kd_akun,"
                + "  case bulan when 7 then nilai end as 'b7',"
                + "  case bulan when 8 then nilai end as 'b8',"
                + "  case bulan when 9 then nilai end as 'b9',"
                + "  case bulan when 10 then nilai end as 'b10',"
                + "  case bulan when 11 then nilai end as 'b11',"
                + "  case bulan when 12 then nilai end as 'b12',"
                + "  case bulan when 1 then nilai end as 'b1',"
                + "  case bulan when 2 then nilai end as 'b2',"
                + "  case bulan when 3 then nilai end as 'b3',"
                + "  case bulan when 4 then nilai end as 'b4',"
                + "  case bulan when 5 then nilai end as 'b5',"
                + "  case bulan when 6 then nilai end as 'b6'"
                + " FROM ac_tanggaran"
                + " WHERE th_ajar ='" + ThAjar.ToString().Trim() + "'"
                + " AND kd_sekolah = '" + KdSekolah.ToString().Trim() + "'"
            + "   ) agr"
            + " on mak.kd_akun = agr.kd_akun "
            + " inner join ac_sys_gol_akun sys "
            + "     on sys.kd_gol = mak.kd_gol "
            + " where (sys.tipe='P' OR sys.tipe='B')  AND mak.tipe ='DTL'"
            + " group by mak.kd_akun, nm_akun"
            + " order by mak.kd_akun";
            

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["total"] = AdnFungsi.CDec(rdr["total"]);
                baris["Jul"] = AdnFungsi.CDec(rdr["b7"]);
                baris["Agt"] = AdnFungsi.CDec(rdr["b8"]);
                baris["Sep"] = AdnFungsi.CDec(rdr["b9"]);
                baris["Okt"] = AdnFungsi.CDec(rdr["b10"]);
                baris["Nov"] = AdnFungsi.CDec(rdr["b11"]);
                baris["Des"] = AdnFungsi.CDec(rdr["b12"]);
                baris["Jan"] = AdnFungsi.CDec(rdr["b1"]);
                baris["Feb"] = AdnFungsi.CDec(rdr["b2"]);
                baris["Mar"] = AdnFungsi.CDec(rdr["b3"]);
                baris["Apr"] = AdnFungsi.CDec(rdr["b4"]);
                baris["Mei"] = AdnFungsi.CDec(rdr["b5"]);
                baris["Jun"] = AdnFungsi.CDec(rdr["b6"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl;
        }

    }
}
