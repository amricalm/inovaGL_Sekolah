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
    public class AdnAnggaranDao
    {
        private const short JUMLAH_KOLOM = 4;
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
            sWhere = this.pkey + "='" + o.KdAkun + "' AND th_ajar ='" + o.ThAjar.Trim() + "' AND bulan =" + o.Bulan ;
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

        public DataTable GetDf(string ThAjar)
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
            " select kd_akun, nm_akun "
            + " from ac_makun "
            + " where kd_gol ='B' AND tipe ='DTL'";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["Total"] = 0;
                baris["Jul"] = 0;
                baris["Agt"] = 0;
                baris["Sep"] = 0;
                baris["Okt"] = 0;
                baris["Nov"] = 0;
                baris["Des"] = 0;
                baris["Jan"] = 0;
                baris["Feb"] = 0;
                baris["Mar"] = 0;
                baris["Apr"] = 0;
                baris["Mei"] = 0;
                baris["Jun"] = 0;
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl;
        }

    }
}
