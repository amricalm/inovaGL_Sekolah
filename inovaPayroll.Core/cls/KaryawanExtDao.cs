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
    public class AdnKaryawanExtDao
    {
        private const short JUMLAH_KOLOM = 3;
        private const string NAMA_TABEL = "ms_karyawan_ext";

        private string pkey = "nip";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnKaryawanExtDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnKaryawanExtDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnKaryawanExt o)
        {
            short idx = 0;

            fld[idx] = "nip"; nilai[idx] =o.Nip.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_ext"; nilai[idx] = o.KdExt.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "flag"; nilai[idx] = o.Flag.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnKaryawanExt o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
        }

        public int Update(AdnKaryawanExt o)
        {
            int Hasil = 0;
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" +o.Nip  +"'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere);

            try
            {
                cmd.CommandText = sql;
                Hasil = cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
            return Hasil;
        }

        public AdnKaryawanExt Get(string NoKaryawan, string Flag)
        {
            AdnKaryawanExt o = null;
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where kd_ext = '" + NoKaryawan + "' AND flag ='" + Flag + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnKaryawanExt();
                    o.KdExt = NoKaryawan;
                    o.Nip = AdnFungsi.CStr(rdr["nip"]);
                    o.Flag = Flag;
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
            return o;
        }

        public DataTable GetKaryawanExt(string NoKaryawan, string Flag)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoKaryawan", typeof(String));
            tbl.Columns.Add("Nip", typeof(String));
            
            string sql =
            " select kd_ext, nip "
            + " from ms_karyawan_ext " 
            + " where kd_ext = '" + NoKaryawan + "' AND flag ='" + Flag + "'";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoKaryawan"] = AdnFungsi.CStr(rdr["kd_ext"]);
                baris["Nip"] = AdnFungsi.CInt(rdr["nip"],true);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; 
        }
        public DataTable GetKaryawanExt(string Flag)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoKaryawan", typeof(String));
            tbl.Columns.Add("Nip", typeof(String));
            tbl.Columns.Add("NmKaryawan", typeof(String));

            string sql =
            " select kd_ext, kar.nip , kar.nama_lengkap "
            + " from ms_karyawan kar "
            + " LEFT OUTER JOIN ms_karyawan_ext ext "
            + "     ON kar.nip = ext.nip "
            + "     AND ext.flag ='" + Flag + "'";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoKaryawan"] = AdnFungsi.CStr(rdr["kd_ext"]);
                baris["Nip"] = AdnFungsi.CStr(rdr["nip"]);
                baris["NmKaryawan"] = AdnFungsi.CStr(rdr["nama_lengkap"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
    }
}
